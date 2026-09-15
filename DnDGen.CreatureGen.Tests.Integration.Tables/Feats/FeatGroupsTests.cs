using DnDGen.CreatureGen.Defenses;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Feats
{
    [TestFixture]
    public class FeatGroupsTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.FeatGroups;

        [Test]
        public void FeatGroupsNames()
        {
            var names = new[]
            {
                GroupConstants.AttackBonus,
                GroupConstants.Initiative,
                GroupConstants.WeaponProficiency,
                GroupConstants.ArmorProficiency,
                SaveConstants.Fortitude,
                SaveConstants.Reflex,
                SaveConstants.Will,
            };

            AssertCollectionNames(names);
        }

        [TestCase(GroupConstants.AttackBonus,
            FeatConstants.SpecialQualities.AttackBonus)]
        [TestCase(GroupConstants.Initiative,
            FeatConstants.Initiative_Improved)]
        [TestCase(GroupConstants.WeaponProficiency,
            FeatConstants.WeaponProficiency_Exotic,
            FeatConstants.WeaponProficiency_Martial,
            FeatConstants.WeaponProficiency_Simple)]
        [TestCase(GroupConstants.ArmorProficiency,
            FeatConstants.ArmorProficiency_Heavy,
            FeatConstants.ArmorProficiency_Light,
            FeatConstants.ArmorProficiency_Medium,
            FeatConstants.ShieldProficiency,
            FeatConstants.ShieldProficiency_Tower)]
        [TestCase(SaveConstants.Fortitude,
            FeatConstants.GreatFortitude)]
        [TestCase(SaveConstants.Reflex,
            FeatConstants.LightningReflexes)]
        [TestCase(SaveConstants.Will,
            FeatConstants.IronWill)]
        public void FeatGroup(string name, params string[] collection)
        {
            AssertDistinctCollection(name, collection);
        }
    }
}