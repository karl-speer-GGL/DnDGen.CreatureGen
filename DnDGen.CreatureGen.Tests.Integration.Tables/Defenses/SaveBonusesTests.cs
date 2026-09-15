using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Defenses
{
    [TestFixture]
    public class SaveBonusesTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.SaveBonuses;

        private Dictionary<string, List<string>> saveBonusesData;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            saveBonusesData = SaveBonusesTestData.GetSaveBonusesData();
        }

        [Test]
        public void SaveBonusesNames()
        {
            var creatures = CreatureConstants.GetAll();
            var types = CreatureConstants.Types.GetAll();
            var subtypes = CreatureConstants.Types.Subtypes.GetAll();
            var templates = CreatureConstants.Templates.GetAll();

            var names = creatures.Union(types).Union(subtypes).Union(templates);
            Assert.That(saveBonusesData.Keys, Is.EquivalentTo(names));
            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Types))]
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Subtypes))]
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void SaveBonuses(string source)
        {
            Assert.That(saveBonusesData, Contains.Key(source));
            //INFO: Doing this because of the Clear() call later - don't want to modify the original data collection
            var entries = new List<string>(saveBonusesData[source]);

            if (!entries.Any())
                Assert.Fail("Test case did not specify saves bonuses or NONE");

            if (entries[0] == SaveBonusesTestData.None)
                entries.Clear();

            AssertCollection(source, [.. entries]);
        }
    }
}
