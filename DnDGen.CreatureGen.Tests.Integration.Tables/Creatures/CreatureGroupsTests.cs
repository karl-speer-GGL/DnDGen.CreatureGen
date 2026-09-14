using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Abilities;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Templates;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Selectors.Collections;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures
{
    [TestFixture]
    public class CreatureGroupsTests : CollectionTests
    {
        protected override string tableName => TableNameConstants.Collection.CreatureGroups;

        private ICreaturePrototypeFactory prototypeFactory;
        private ICollectionTypeAndAmountSelector typeAndAmountSelector;

        [SetUp]
        public void Setup()
        {
            prototypeFactory = GetNewInstanceOf<ICreaturePrototypeFactory>();
            typeAndAmountSelector = GetNewInstanceOf<ICollectionTypeAndAmountSelector>();
        }

        [Test]
        public void CreatureGroupNames()
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

        [Test]
        public void CreatureGroup_All()
        {
            var allCreatures = CreatureConstants.GetAll();
            AssertDistinctCollection(GroupConstants.All, [.. allCreatures]);
        }

        [Test]
        public void CreatureGroup_Characters()
        {
            var allCharacters = CreatureConstants.GetAllCharacters();
            AssertDistinctCollection(GroupConstants.Characters, [.. allCharacters]);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_Template(string template)
        {
            var allCreatures = CreatureConstants.GetAll();
            AssertTemplateGroup(template, allCreatures, false);
        }

        private void AssertTemplateGroup(string template, IEnumerable<string> source, bool asCharacter)
        {
            //INFO: We explicitly do not want to use the creatureVerifier or its cached lookups, as this test is what populates those caches
            var sourcePrototypes = GetPrototypes(source, asCharacter);
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);
            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p))
                .Select(p => p.Name);

            AssertDistinctCollection(template + asCharacter.ToString(), [.. templateCreatures]);
        }

        private CreaturePrototype[] GetPrototypes(IEnumerable<string> source, bool asCharacter)
        {
            var randomizer = new AbilityRandomizer(AbilityConstants.RandomizerRolls.BestOfFour);
            var prototypes = prototypeFactory.Build(source, asCharacter, randomizer).ToArray();
            return prototypes;
        }

        private IEnumerable<CreaturePrototype> GetAllPrototypes()
        {
            var allCreatures = CreatureConstants.GetAll();
            var prototypes = GetPrototypes(allCreatures, false);

            var allCharacters = CreatureConstants.GetAllCharacters();
            var characters = GetPrototypes(allCharacters, true);

            return prototypes.Concat(characters);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_TemplateAsCharacter(string template)
        {
            //INFO: We can do this as a shortcut, because if asCharacter = true, then our set of base creatures is only characters.
            //Setting asCharacter = true and generating a creature that can't be a character produces a compatibility error.
            var allCharacters = CreatureConstants.GetAllCharacters();
            AssertTemplateGroup(template, allCharacters, true);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_Template_HasNonEmptyVariation(string template)
        {
            Assert.That(table, Contains.Key(template + bool.TrueString)
                .And.ContainKey(template + bool.FalseString));

            var allVariations = table[template + bool.FalseString]
                .Union(table[template + bool.TrueString]);
            Assert.That(allVariations, Is.Not.Empty);
        }

        //INFO: Can't construct this programmatically, because to know the minimum abilities requires getting all template aplicators, which requires non-static construction
        //-10 is lowest adjustment
        //Valid ability randomizer must have minimum roll of 1
        //Ghost has Minimum Charisma 6, so biggest required adjustment is 6 - 1 = 5
        //Half-Celestial and Half-Fiend have Minimum Intelligence 4, so biggest required adjustment is 4 - 1 = 3
        [TestCase(AbilityConstants.Charisma, -10)]
        [TestCase(AbilityConstants.Charisma, -9)]
        [TestCase(AbilityConstants.Charisma, -8)]
        [TestCase(AbilityConstants.Charisma, -7)]
        [TestCase(AbilityConstants.Charisma, -6)]
        [TestCase(AbilityConstants.Charisma, -5)]
        [TestCase(AbilityConstants.Charisma, -4)]
        [TestCase(AbilityConstants.Charisma, -3)]
        [TestCase(AbilityConstants.Charisma, -2)]
        [TestCase(AbilityConstants.Charisma, -1)]
        [TestCase(AbilityConstants.Charisma, 0)]
        [TestCase(AbilityConstants.Charisma, 1)]
        [TestCase(AbilityConstants.Charisma, 2)]
        [TestCase(AbilityConstants.Charisma, 3)]
        [TestCase(AbilityConstants.Charisma, 4)]
        [TestCase(AbilityConstants.Charisma, 5)]
        [TestCase(AbilityConstants.Intelligence, -10)]
        [TestCase(AbilityConstants.Intelligence, -9)]
        [TestCase(AbilityConstants.Intelligence, -8)]
        [TestCase(AbilityConstants.Intelligence, -7)]
        [TestCase(AbilityConstants.Intelligence, -6)]
        [TestCase(AbilityConstants.Intelligence, -5)]
        [TestCase(AbilityConstants.Intelligence, -4)]
        [TestCase(AbilityConstants.Intelligence, -3)]
        [TestCase(AbilityConstants.Intelligence, -2)]
        [TestCase(AbilityConstants.Intelligence, -1)]
        [TestCase(AbilityConstants.Intelligence, 0)]
        [TestCase(AbilityConstants.Intelligence, 1)]
        [TestCase(AbilityConstants.Intelligence, 2)]
        [TestCase(AbilityConstants.Intelligence, 3)]
        public void CreatureGroup_MinimumAbilityAdjustment(string ability, int adjustment)
        {
            var allCreatures = CreatureConstants.GetAll();
            var abilityAdjustments = typeAndAmountSelector.SelectAllFrom(Config.Name, TableNameConstants.TypeAndAmount.AbilityAdjustments)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(d => d.Type, d => d.Amount));
            var abilityCreatures = allCreatures
                .Where(c => abilityAdjustments[c].ContainsKey(ability) && abilityAdjustments[c][ability] >= adjustment);

            var groupName = ability + adjustment;
            AssertDistinctCollection(groupName, [.. abilityCreatures]);
        }

        private static IEnumerable TemplatesWithTypeFilter
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();
                var types = CreatureConstants.Types.GetAll();
                var subtypes = CreatureConstants.Types.Subtypes.GetAll();

                foreach (var template in templates)
                {
                    foreach (var type in types)
                        yield return new TestCaseData(template, type);

                    foreach (var subtype in subtypes)
                        yield return new TestCaseData(template, subtype);
                }
            }
        }

        [TestCaseSource(nameof(TemplatesWithTypeFilter))]
        public void CreatureGroup_Template_ResultsInType(string template, string type)
        {
            var sourcePrototypes = GetAllPrototypes();
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { Types = [type] }))
                .Select(p => p.Name)
                .Distinct();

            var groupName = template + type;
            AssertDistinctCollection(groupName, [.. templateCreatures]);
        }

        private static string[] Alignments =>
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

        private static IEnumerable<string> ChallengeRatings => ChallengeRatingConstants.GetOrdered()
                    .Union(Enumerable.Range(1, 30).Select(cr => cr.ToString()));

        private static IEnumerable TemplatesWithAlignmentFilter
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();

                foreach (var template in templates)
                    foreach (var alignment in Alignments)
                        yield return new TestCaseData(template, alignment);
            }
        }

        [TestCaseSource(nameof(TemplatesWithAlignmentFilter))]
        public void CreatureGroup_Template_ResultsInAlignment(string template, string alignment)
        {
            var sourcePrototypes = GetAllPrototypes();
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { Alignments = [alignment] }))
                .Select(p => p.Name)
                .Distinct();

            var groupName = template + alignment;
            AssertDistinctCollection(groupName, [.. templateCreatures]);
        }

        private static IEnumerable TemplatesWithChallengeRatingFilter
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();

                foreach (var template in templates)
                    foreach (var cr in ChallengeRatings)
                        yield return new TestCaseData(template, cr);
            }
        }

        [TestCaseSource(nameof(TemplatesWithChallengeRatingFilter))]
        public void CreatureGroup_Template_ResultsInChallengeRating(string template, string cr)
        {
            var allCreatures = CreatureConstants.GetAll();
            var sourcePrototypes = GetPrototypes(allCreatures, false);
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { ChallengeRatings = [cr] }))
                .Select(p => p.Name);

            var groupName = template + bool.FalseString + cr;
            AssertDistinctCollection(groupName, [.. templateCreatures]);
        }

        [TestCaseSource(nameof(TemplatesWithChallengeRatingFilter))]
        public void CreatureGroup_TemplateAsCharacter_ResultsInChallengeRating(string template, string cr)
        {
            var allCharacters = CreatureConstants.GetAllCharacters();
            var sourcePrototypes = GetPrototypes(allCharacters, true);
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { ChallengeRatings = [cr] }))
                .Select(p => p.Name);

            var groupName = template + bool.TrueString + cr;
            AssertDistinctCollection(groupName, [.. templateCreatures]);
        }
    }
}
