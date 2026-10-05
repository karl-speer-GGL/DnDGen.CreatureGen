using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateChallengeRatingCreatureGroupsLimitsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureAllGroupNames() => AssertAllCreatureGroupNames();

        [TestCase(true)]
        [TestCase(false)]
        public void TemplateChallengeRatingCreatureGroupNames(bool asCharacter)
        {
            AssertSubsetCreatureGroupNames(TemplateChallengeRatingPairs.Select(p => p[0] + asCharacter + p[1]));
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_Template_OverMaxChallengeRatingIsEmpty(string template)
        {
            AssertNoCompatibleCreatures(template, false);
        }

        private void AssertNoCompatibleCreatures(string template, bool asCharacter)
        {
            var lastCr = ChallengeRatingConstants.GetOrdered()[^1];
            var overages = new List<string>
            {
                ChallengeRatingConstants.IncreaseChallengeRating(lastCr, 1),
                ChallengeRatingConstants.IncreaseChallengeRating(lastCr, 2)
            };
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);
            var allCreatures = CreatureConstants.GetAll();

            var sourcePrototypes = GetPrototypes(allCreatures, asCharacter);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { ChallengeRatings = overages }))
                .Select(p => p.Name);
            Assert.That(templateCreatures, Is.Empty);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_TemplateAsCharacter_OverMaxChallengeRatingIsEmpty(string template)
        {
            AssertNoCompatibleCreatures(template, true);
        }
    }
}
