using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateChallengeRatingCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureGroupNames() => AssertCreatureGroupNames();

        [TestCaseSource(typeof(CreatureGroupsTestBase), nameof(TemplatesWithChallengeRatingFilter))]
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
    }
}
