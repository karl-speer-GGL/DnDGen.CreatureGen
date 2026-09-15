using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.Appearances
{
    [TestFixture]
    internal class SkinAppearancesTests : AppearancesTests
    {
        protected override string TableName =>
            TableNameConstants.Collection.Appearances(TableNameConstants.Collection.AppearanceCategories.Skin);

        [Test]
        public void SkinAppearancesNames()
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
        public void CreatureSkinAppearances(string creature)
        {
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, creature);
            var creatureKeys = GetCollectionCreatureKeys();
            var keys = genders
                .Select(g => creature + g)
                .Concat([creature])
                .Intersect(creatureKeys);

            foreach (var key in keys)
            {
                AssertCreatureAppearance(TableNameConstants.Collection.AppearanceCategories.Skin, key);
            }
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void TemplateSkinAppearances(string template)
        {
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, CreatureConstants.Human);
            var creatureKeys = GetCollectionCreatureKeys();
            var keys = genders
                .Select(g => template + g)
                .Concat([template])
                .Intersect(creatureKeys);

            Assert.That(creatureKeys, Contains.Item(template));
            foreach (var key in keys)
            {
                AssertCreatureAppearance(TableNameConstants.Collection.AppearanceCategories.Skin, key);
            }
        }

        [Test]
        [Ignore("Don't run this unless you need to bulk update the appearances file")]
        public void DEBUG_WriteXml()
        {
            WriteXml(TableNameConstants.Collection.AppearanceCategories.Skin);
        }

        [Test]
        public void NoAppearancesIncludeTODO()
        {
            AssertNoAppearancesIncludeTODO(TableNameConstants.Collection.AppearanceCategories.Skin);
        }

        [Test]
        public void BUG_AlbinoHalfElfIsVeryRare()
        {
            Assert.That(creatureAppearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                Is.All.Not.Contain("albino"));
            Assert.That(creatureAppearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                Is.All.Not.Contain("albino"));
            Assert.That(creatureAppearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                Is.All.Not.Contain("albino"));
            Assert.That(creatureAppearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                Has.Some.Contain("albino"));
        }
    }
}
