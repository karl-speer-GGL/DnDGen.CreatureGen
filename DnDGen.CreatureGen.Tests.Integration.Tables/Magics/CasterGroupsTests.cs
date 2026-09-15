using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Magics
{
    [TestFixture]
    public class CasterGroupsTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.CasterGroups;

        [Test]
        public void CasterGroupsHasAllGroups()
        {
            var names = new[]
            {
                SpellConstants.Sources.Arcane,
                SpellConstants.Sources.Divine,
                GroupConstants.PreparesSpells,
            };

            AssertCollectionNames(names);
        }

        [TestCase(SpellConstants.Sources.Arcane,
            SpellConstants.Casters.Bard,
            SpellConstants.Casters.Sorcerer)]
        [TestCase(SpellConstants.Sources.Divine,
            SpellConstants.Casters.Cleric,
            SpellConstants.Casters.Druid)]
        [TestCase(GroupConstants.PreparesSpells,
            SpellConstants.Casters.Cleric,
            SpellConstants.Casters.Druid)]
        public void CasterGroup(string name, params string[] casters)
        {
            AssertCollection(name, casters);
        }
    }
}
