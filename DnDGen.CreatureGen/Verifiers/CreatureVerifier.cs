using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Abilities;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Templates;
using DnDGen.Infrastructure.Factories;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Verifiers
{
    internal class CreatureVerifier(
        JustInTimeFactory factory,
        ICollectionSelector collectionSelector,
        Dice dice,
        ICreaturePrototypeFactory prototypeFactory) : ICreatureVerifier
    {
        public IEnumerable<CreaturePrototype> GetChainedTemplates(
            IEnumerable<string> sourceCreatures,
            string[] templates,
            bool asCharacter,
            AbilityRandomizer abilityRandomizer = null,
            Filters filters = null)
        {
            if (templates.Length >= 2)
            {
                var prototypes = prototypeFactory.Build(sourceCreatures, asCharacter, abilityRandomizer);
                return GetChainedTemplates(prototypes, templates, filters);
            }

            var firstTemplate = templates.FirstOrDefault();
            var compatibleCreatures = GetCompatibleCreaturesForTemplate(sourceCreatures, firstTemplate, asCharacter, abilityRandomizer, filters);
            var compatiblePrototypes = prototypeFactory.Build(compatibleCreatures, asCharacter, abilityRandomizer);

            if (templates.Length == 1)
                return GetCompatiblePrototypes(compatiblePrototypes, templates[0], filters);

            return compatiblePrototypes;
        }

        public IEnumerable<string> GetCompatibleCreaturesForTemplate(
            IEnumerable<string> sourceCreatures,
            string template,
            bool asCharacter,
            AbilityRandomizer abilityRandomizer = null,
            Filters filters = null)
        {
            template ??= CreatureConstants.Templates.None;

            var templateCreatures = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.CreatureGroups, template + asCharacter);
            var filteredBaseCreatures = sourceCreatures.Intersect(templateCreatures);

            filteredBaseCreatures = ApplyFilterTo(filteredBaseCreatures, filters?.Alignments, template);
            filteredBaseCreatures = ApplyFilterTo(filteredBaseCreatures, filters?.ChallengeRatings, template + asCharacter);
            filteredBaseCreatures = ApplyFilterTo(filteredBaseCreatures, filters?.Types, template);

            var applicator = factory.Build<TemplateApplicator>(template);
            if (applicator.MinimumAbility is not null)
            {
                abilityRandomizer ??= new();
                var lowestAdjustment = applicator.MinimumAbility.FullScore - abilityRandomizer.GetMax(dice, applicator.MinimumAbility.Name);

                //INFO: Lowest possible ability adjustment is -10
                //We still want to filter for this, as it will remove creatures that don't have the ability at all
                lowestAdjustment = Math.Max(-10, lowestAdjustment);
                filteredBaseCreatures = ApplyFilterTo(filteredBaseCreatures, [applicator.MinimumAbility.Name + lowestAdjustment], string.Empty);
            }

            return filteredBaseCreatures;
        }

        private IEnumerable<string> ApplyFilterTo(IEnumerable<string> source, List<string> filter, string prefix)
        {
            if (filter?.Count > 0 && filter.Any(g => !string.IsNullOrEmpty(g)))
            {
                var groupCreatures = GetUnifiedCreatureGroups(prefix, filter);
                source = source.Intersect(groupCreatures);
            }

            return source;
        }

        private IEnumerable<string> GetUnifiedCreatureGroups(string prefix, IEnumerable<string> groupNames)
        {
            var group = Enumerable.Empty<string>();

            foreach (var groupName in groupNames.Where(g => !string.IsNullOrEmpty(g)))
            {
                if (!collectionSelector.IsCollection(Config.Name, TableNameConstants.Collection.CreatureGroups, prefix + groupName))
                    continue;

                var creatures = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.CreatureGroups, prefix + groupName);
                group = group.Union(creatures);
            }

            return group;
        }

        private IEnumerable<CreaturePrototype> GetCompatiblePrototypes(IEnumerable<CreaturePrototype> sourceCreatures, string template, Filters filters = null)
        {
            var applicator = factory.Build<TemplateApplicator>(template);
            var compatiblePrototypes = sourceCreatures.Where(p => applicator.IsCompatible(p, filters));
            var updatedPrototypes = compatiblePrototypes.Select(p => applicator.ApplyTo(p, filters));

            //INFO: Trigger immediate execution, so it won't re-apply templates or re-compute validity.
            return [.. updatedPrototypes];
        }

        public bool VerifyCompatibility(bool asCharacter, string creature = null, AbilityRandomizer abilityRandomizer = null, Filters filters = null, params string[] templates)
        {
            var valid = abilityRandomizer?.Validate(dice) ?? true;
            if (!valid)
                return false;

            IEnumerable<string> baseCreatures = [creature];
            if (string.IsNullOrEmpty(creature))
            {
                baseCreatures = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.CreatureGroups, GroupConstants.All);
            }

            if (asCharacter)
            {
                var characters = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.CreatureGroups, GroupConstants.Characters);
                baseCreatures = baseCreatures.Intersect(characters);
            }

            if (!baseCreatures.Any())
                return false;

            var cleanTemplates = templates.Where(t => !string.IsNullOrEmpty(t)).ToArray();
            if (cleanTemplates.Length == 1)
            {
                var compatibleCreatures = GetCompatibleCreaturesForTemplate(baseCreatures, cleanTemplates[0], asCharacter, abilityRandomizer, filters);
                return compatibleCreatures.Any();
            }

            if (cleanTemplates.Length > 1)
            {
                var compatibleCreatures = GetChainedTemplates(baseCreatures, cleanTemplates, asCharacter, abilityRandomizer, filters);
                return compatibleCreatures.Any();
            }

            //INFO: We can use the None template to verify the filters
            //If there are any of the base creatures in this group, then the filters are valid
            var filteredCreatures = GetCompatibleCreaturesForTemplate(baseCreatures, null, asCharacter, abilityRandomizer, filters);
            if (filteredCreatures.Any())
                return true;

            //INFO: This means that the filters aren't valid for non-templated base creatures.
            //We need to check the templates to see if any of them are valid
            var allTemplates = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All);
            foreach (var template in allTemplates)
            {
                filteredCreatures = GetCompatibleCreaturesForTemplate(baseCreatures, template, asCharacter, abilityRandomizer, filters);
                if (filteredCreatures.Any())
                    return true;
            }

            return false;
        }

        public IEnumerable<CreaturePrototype> GetChainedTemplates(IEnumerable<CreaturePrototype> prototypes, string[] templates, Filters filters = null)
        {
            if (templates.Length == 0)
                return GetCompatiblePrototypes(prototypes, CreatureConstants.Templates.None, filters);

            //INFO: We only want to apply filters to the last creature in a series of chained templates
            for (var i = 0; i < templates.Length - 1; i++)
            {
                prototypes = GetCompatiblePrototypes(prototypes, templates[i]);
            }

            prototypes = GetCompatiblePrototypes(prototypes, templates[^1], filters);

            return prototypes;
        }
    }
}