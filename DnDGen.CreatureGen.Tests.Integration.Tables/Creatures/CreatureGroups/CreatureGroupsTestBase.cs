using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Abilities;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Templates;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class CreatureGroupsTestBase : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.CreatureGroups;

        private ICreaturePrototypeFactory prototypeFactory;

        [SetUp]
        public void CreatureGroupsSetup()
        {
            prototypeFactory = GetNewInstanceOf<ICreaturePrototypeFactory>();
        }

        protected void AssertCreatureGroupNames()
        {
            var templates = CreatureConstants.Templates.GetAll();
            var types = CreatureConstants.Types.GetAll();
            var subtypes = CreatureConstants.Types.Subtypes.GetAll();

            var entries = new[]
            {
                GroupConstants.All,
                GroupConstants.Characters,
            };

            var names = entries
                .Union(templates.Select(t => t + bool.FalseString))
                .Union(templates.Select(t => t + bool.TrueString))
                .Union(Alignments.SelectMany(a => templates.Select(t => t + a)))
                .Union(types.SelectMany(ty => templates.Select(t => t + ty)))
                .Union(subtypes.SelectMany(sty => templates.Select(t => t + sty)))
                .Union(ChallengeRatings.SelectMany(cr => templates.Select(t => t + bool.FalseString + cr)))
                .Union(ChallengeRatings.SelectMany(cr => templates.Select(t => t + bool.TrueString + cr)));

            const int LowestAdjustment = -9;
            const int MinimumAbilityScore = 1;
            foreach (var template in templates)
            {
                var applicator = GetNewInstanceOf<TemplateApplicator>(template);
                if (applicator.MinimumAbility == null)
                    continue;

                var adjustmentCount = (applicator.MinimumAbility.FullScore - MinimumAbilityScore) - LowestAdjustment + 1;
                var adjustments = Enumerable.Range(LowestAdjustment, adjustmentCount);
                names = names.Union(adjustments.Select(adj => applicator.MinimumAbility.Name + adj));
            }

            AssertCollectionNames(names);
        }

        protected static string[] Alignments =>
        [
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.TrueNeutral,
        ];

        protected static IEnumerable<string> ChallengeRatings => ChallengeRatingConstants.GetOrdered()
                    .Union(Enumerable.Range(1, 30).Select(cr => cr.ToString()));

        protected void AssertTemplateGroup(string template, IEnumerable<string> source, bool asCharacter)
        {
            //INFO: We explicitly do not want to use the creatureVerifier or its cached lookups, as this test is what populates those caches
            var sourcePrototypes = GetPrototypes(source, asCharacter);
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);
            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p))
                .Select(p => p.Name);

            AssertDistinctCollection(template + asCharacter.ToString(), [.. templateCreatures]);
        }

        internal CreaturePrototype[] GetPrototypes(IEnumerable<string> source, bool asCharacter)
        {
            var randomizer = new AbilityRandomizer(AbilityConstants.RandomizerRolls.BestOfFour);
            var prototypes = prototypeFactory.Build(source, asCharacter, randomizer).ToArray();
            return prototypes;
        }

        internal IEnumerable<CreaturePrototype> GetAllPrototypes()
        {
            var allCreatures = CreatureConstants.GetAll();
            var prototypes = GetPrototypes(allCreatures, false);

            var allCharacters = CreatureConstants.GetAllCharacters();
            var characters = GetPrototypes(allCharacters, true);

            return prototypes.Concat(characters);
        }

        protected static IEnumerable TemplatesWithChallengeRatingFilter
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();

                foreach (var template in templates)
                    foreach (var cr in ChallengeRatings)
                        yield return new TestCaseData(template, cr);
            }
        }
    }
}
