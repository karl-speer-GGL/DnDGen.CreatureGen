using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateChallengeRatingAsCharacterCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureAllGroupNames() => AssertAllCreatureGroupNames();

        [Test]
        public void TemplateChallengeRatingAsCharacterCreatureGroupNames()
        {
            AssertSubsetCreatureGroupNames(TemplateChallengeRatingPairs.Select(p => p[0] + bool.TrueString + p[1]));
        }

        [TestCaseSource(typeof(CreatureGroupsTestBase), nameof(TemplatesWithChallengeRatingFilter))]
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

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_TemplateAsCharacter_Final3ChallengeRatingGroupsAreEmpty(string template)
        {
            var last3 = ChallengeRatings.Reverse().Take(3);
            Assert.That(last3, Contains.Item(40.ToString()));
            
            foreach (var cr in last3)
            {
                var groupName = template + bool.TrueString + cr;
                AssertDistinctCollection(groupName, []);
            }
        }
    }
}
