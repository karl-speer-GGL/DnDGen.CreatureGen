using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Skills
{
    [TestFixture]
    public class SkillPointsTests : AdjustmentsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.SkillPoints;

        [Test]
        public void SkillPointsNames()
        {
            var types = CreatureConstants.Types.GetAll();
            AssertCollectionNames(types);
        }

        [TestCase(CreatureConstants.Types.Aberration, 2)]
        [TestCase(CreatureConstants.Types.Animal, 2)]
        [TestCase(CreatureConstants.Types.Construct, 2)]
        [TestCase(CreatureConstants.Types.Dragon, 6)]
        [TestCase(CreatureConstants.Types.Elemental, 2)]
        [TestCase(CreatureConstants.Types.Fey, 6)]
        [TestCase(CreatureConstants.Types.Giant, 2)]
        [TestCase(CreatureConstants.Types.Humanoid, 2)]
        [TestCase(CreatureConstants.Types.MagicalBeast, 2)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid, 2)]
        [TestCase(CreatureConstants.Types.Ooze, 2)]
        [TestCase(CreatureConstants.Types.Outsider, 8)]
        [TestCase(CreatureConstants.Types.Plant, 2)]
        [TestCase(CreatureConstants.Types.Undead, 4)]
        [TestCase(CreatureConstants.Types.Vermin, 2)]
        public void SkillPoints(string creatureType, int points)
        {
            base.AssertAdjustment(creatureType, points);
        }
    }
}