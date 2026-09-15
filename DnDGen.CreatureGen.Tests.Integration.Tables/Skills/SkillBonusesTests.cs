using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Skills;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Skills
{
    [TestFixture]
    public class SkillBonusesTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.SkillBonuses;

        private Dictionary<string, List<string>> skillBonusesData;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            skillBonusesData = SkillBonusesTestData.GetSkillBonusesData();
        }

        [Test]
        public void SkillBonusesNames()
        {
            var creatures = CreatureConstants.GetAll();
            var types = CreatureConstants.Types.GetAll();
            var subtypes = CreatureConstants.Types.Subtypes.GetAll();

            var foci = collectionMapper.Map(Config.Name, TableNameConstants.Collection.FeatFoci);
            var skills = foci[GroupConstants.Skills];

            var nonFociSkills = new[]
            {
                SkillConstants.Craft,
                SkillConstants.Knowledge,
                SkillConstants.Perform,
                SkillConstants.Profession,
            };

            var names = creatures.Union(types).Union(subtypes).Union(skills).Union(nonFociSkills);
            Assert.That(skillBonusesData.Keys, Is.EquivalentTo(names));
            Assert.That(SkillBonusesTestData.SkillSynergyNames, Is.EquivalentTo(skills.Union(nonFociSkills)));
            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Types))]
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Subtypes))]
        public void SkillBonuses(string source)
        {
            Assert.That(skillBonusesData, Contains.Key(source));

            AssertCollection(source, [.. skillBonusesData[source]]);
        }

        [TestCaseSource(typeof(SkillBonusesTestData), nameof(SkillBonusesTestData.SkillSynergies))]
        public void SkillSynergies(string source)
        {
            Assert.That(skillBonusesData, Contains.Key(source));

            AssertCollection(source, [.. skillBonusesData[source]]);
        }
    }
}
