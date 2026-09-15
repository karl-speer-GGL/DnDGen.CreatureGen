using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.Appearances
{
    [TestFixture]
    internal class EyesAppearancesTests : AppearancesTests
    {
        protected override string TableName =>
            TableNameConstants.Collection.Appearances(TableNameConstants.Collection.AppearanceCategories.Eyes);

        [Test]
        public void EyesAppearancesNames()
        {
            var creatureKeys = GetCollectionCreatureKeys();
            var names = new List<string>();

            foreach (var creatureKey in creatureKeys)
            {
                names.Add(creatureKey + Rarity.Common.ToString());
                names.Add(creatureKey + Rarity.Uncommon.ToString());
                names.Add(creatureKey + Rarity.Rare.ToString());
                names.Add(creatureKey + Rarity.VeryRare.ToString());
            }

            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureEyesAppearances(string creature)
        {
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, creature);
            var creatureKeys = GetCollectionCreatureKeys();
            var keys = genders
                .Select(g => creature + g)
                .Concat([creature])
                .Intersect(creatureKeys);

            foreach (var key in keys)
            {
                AssertCreatureAppearance(TableNameConstants.Collection.AppearanceCategories.Eyes, key);
            }
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void TemplateEyesAppearances(string template)
        {
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, CreatureConstants.Human);
            var templateKeys = GetCollectionCreatureKeys();
            var keys = genders
                .Select(g => template + g)
                .Concat([template])
                .Intersect(templateKeys);

            Assert.That(templateKeys, Contains.Item(template));
            foreach (var key in keys)
            {
                AssertCreatureAppearance(TableNameConstants.Collection.AppearanceCategories.Eyes, key);
            }
        }

        [Test]
        [Ignore("Don't run this unless you need to bulk update the appearances file")]
        public void DEBUG_WriteXml()
        {
            WriteXml(TableNameConstants.Collection.AppearanceCategories.Eyes);
        }

        [Test]
        public void NoAppearancesIncludeTODO()
        {
            AssertNoAppearancesIncludeTODO(TableNameConstants.Collection.AppearanceCategories.Eyes);
        }
    }
}
