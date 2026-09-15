using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Abilities
{
    [TestFixture]
    public class AbilityGroupsTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.AbilityGroups;

        [Test]
        public void AbilityGroupsHasAllGroups()
        {
            var names = new[]
            {
                $"{SpellConstants.Casters.Bard}:Spellcaster",
                $"{SpellConstants.Casters.Cleric}:Spellcaster",
                $"{SpellConstants.Casters.Druid}:Spellcaster",
                $"{SpellConstants.Casters.Sorcerer}:Spellcaster",
            };

            AssertCollectionNames(names);
        }

        [TestCase(SpellConstants.Casters.Bard + ":Spellcaster", AbilityConstants.Charisma)]
        [TestCase(SpellConstants.Casters.Cleric + ":Spellcaster", AbilityConstants.Wisdom)]
        [TestCase(SpellConstants.Casters.Druid + ":Spellcaster", AbilityConstants.Wisdom)]
        [TestCase(SpellConstants.Casters.Sorcerer + ":Spellcaster", AbilityConstants.Charisma)]
        public void AbilityGroup(string name, params string[] abilities)
        {
            AssertCollection(name, abilities);
        }
    }
}
