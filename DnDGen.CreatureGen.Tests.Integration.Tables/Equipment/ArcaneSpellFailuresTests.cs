using DnDGen.CreatureGen.Tables;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Equipment
{
    [TestFixture]
    public class ArcaneSpellFailuresTests : AdjustmentsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.ArcaneSpellFailures;

        [Test]
        public void ArcaneSpellFailuresContainsAllItemNames()
        {
            var armorNames = ArmorConstants.GetAllArmorsAndShields(true);
            AssertCollectionNames(armorNames);
        }

        [TestCase(ArmorConstants.AbsorbingShield, 15)]
        [TestCase(ArmorConstants.ArmorOfArrowAttraction, 35)]
        [TestCase(ArmorConstants.ArmorOfRage, 35)]
        [TestCase(ArmorConstants.BandedMail, 35)]
        [TestCase(ArmorConstants.BandedMailOfLuck, 35)]
        [TestCase(ArmorConstants.Breastplate, 25)]
        [TestCase(ArmorConstants.BreastplateOfCommand, 25)]
        [TestCase(ArmorConstants.Buckler, 5)]
        [TestCase(ArmorConstants.CastersShield, 5)]
        [TestCase(ArmorConstants.CelestialArmor, 15)]
        [TestCase(ArmorConstants.Chainmail, 30)]
        [TestCase(ArmorConstants.ChainShirt, 20)]
        [TestCase(ArmorConstants.DemonArmor, 35)]
        [TestCase(ArmorConstants.DwarvenPlate, 35)]
        [TestCase(ArmorConstants.ElvenChain, 20)]
        [TestCase(ArmorConstants.FullPlate, 35)]
        [TestCase(ArmorConstants.FullPlateOfSpeed, 35)]
        [TestCase(ArmorConstants.HalfPlate, 40)]
        [TestCase(ArmorConstants.HeavySteelShield, 15)]
        [TestCase(ArmorConstants.HeavyWoodenShield, 15)]
        [TestCase(ArmorConstants.HideArmor, 20)]
        [TestCase(ArmorConstants.LeatherArmor, 10)]
        [TestCase(ArmorConstants.LightSteelShield, 5)]
        [TestCase(ArmorConstants.LightWoodenShield, 5)]
        [TestCase(ArmorConstants.LionsShield, 15)]
        [TestCase(ArmorConstants.PaddedArmor, 5)]
        [TestCase(ArmorConstants.PlateArmorOfTheDeep, 35)]
        [TestCase(ArmorConstants.RhinoHide, 20)]
        [TestCase(ArmorConstants.ScaleMail, 25)]
        [TestCase(ArmorConstants.SpinedShield, 15)]
        [TestCase(ArmorConstants.SplintMail, 40)]
        [TestCase(ArmorConstants.StuddedLeatherArmor, 15)]
        [TestCase(ArmorConstants.TowerShield, 50)]
        [TestCase(ArmorConstants.WingedShield, 15)]
        public void ArcaneSpellFailureChance(string armorName, int failureChance)
        {
            AssertAdjustment(armorName, failureChance);
        }
    }
}
