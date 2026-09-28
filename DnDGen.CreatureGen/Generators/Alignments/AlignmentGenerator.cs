using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Verifiers;
using DnDGen.CreatureGen.Verifiers.Exceptions;
using DnDGen.Infrastructure.Selectors.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Generators.Alignments
{
    internal class AlignmentGenerator(ICollectionSelector collectionSelector, ICreatureVerifier creatureVerifier, ICreaturePrototypeFactory prototypeFactory) : IAlignmentGenerator
    {
        public Alignment Generate(string creatureName, string[] templates, Filters filters)
        {
            templates ??= [];
            var weightedAlignments = GetWeightedAlignments(creatureName, templates, filters);

            if (!weightedAlignments.Any())
                throw new InvalidCreatureException(
                    $"Creature {creatureName} has no valid alignments for templates [{string.Join(", ", templates)}]",
                    false,
                    creatureName,
                    filters);

            var randomAlignment = collectionSelector.SelectRandomFrom(weightedAlignments);
            return new Alignment(randomAlignment);
        }

        private IEnumerable<string> GetWeightedAlignments(string creatureName, string[] templates, Filters filters)
        {
            var weightedAlignments = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.AlignmentGroups, creatureName);
            var hasAlignmentFilter = filters?.Alignments?.Any() == true;

            if (templates.Length == 0 && hasAlignmentFilter)
                return weightedAlignments.Where(filters.Alignments.Contains);
            else if (templates.Length == 0)
                return weightedAlignments;

            if (templates.Length == 1 && !hasAlignmentFilter)
            {
                var templateAlignments = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.AlignmentGroups, templates[0] + GroupConstants.AllowedInput);

                //INFO: Doing this instead of intersect in order to preserve duplicates/weighting
                return weightedAlignments.Where(templateAlignments.Contains);
            }

            var creaturePrototype = prototypeFactory.Build(creatureName, false);

            //HACK: This is very inefficient, but:
            //1. This usecase will only occur when an alignment filter is set AND templates are specified, OR more than 1 template is specified
            //2. Only builds the prototype once and then re-clones base values (avoiding multiple "SelectAll" calls in the factory)
            //3. Only applies the templates to the single prototype
            //4. Only calls .Any() - so since we only have 1 prototype, will basically be 1 or 0
            //5. Even if all alignments allowed, that's N = 9, which for an edge case, feels manageable.
            //We can't pre-cache it because of the combinatorials of the template chaining
            bool BaseAlignmentMatchesTemplatesAndFilters(string alignment)
            {
                var prototype = prototypeFactory.Clone(creaturePrototype);
                prototype.Alignments = [new(alignment)];

                var prototypes = creatureVerifier.GetChainedTemplates([prototype], templates, filters: filters);
                return prototypes.Any();
            }

            weightedAlignments = weightedAlignments.Where(BaseAlignmentMatchesTemplatesAndFilters);

            return weightedAlignments;
        }
    }
}