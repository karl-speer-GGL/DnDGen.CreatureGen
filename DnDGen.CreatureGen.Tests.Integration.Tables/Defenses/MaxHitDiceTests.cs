using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Defenses
{
    [TestFixture]
    public class MaxHitDiceTests : AdjustmentsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.MaxHitDice;

        [Test]
        public void MaxHitDiceNames()
        {
            var templates = CreatureConstants.Templates.GetAll();
            AssertCollectionNames(templates);
        }

        [TestCase(CreatureConstants.Templates.CelestialCreature, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.FiendishCreature, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Ghost, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfCelestial, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Black, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Blue, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Brass, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Bronze, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Copper, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Gold, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Green, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Red, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Silver, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfDragon_White, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.HalfFiend, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lich, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.None, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Skeleton, 20)]
        [TestCase(CreatureConstants.Templates.Vampire, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Afflicted, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Natural, int.MaxValue)]
        [TestCase(CreatureConstants.Templates.Zombie, 10)]
        public void MaxHitDice(string template, double quantity)
        {
            Assert.That(quantity, Is.Positive);
            AssertAdjustment(template, quantity);
        }
    }
}
