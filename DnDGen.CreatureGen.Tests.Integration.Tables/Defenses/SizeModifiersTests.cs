using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Combats
{
    [TestFixture]
    public class SizeModifiersTests : AdjustmentsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.SizeModifiers;

        [Test]
        public void SizeModifersNames()
        {
            var names = new[]
            {
                SizeConstants.Colossal,
                SizeConstants.Gargantuan,
                SizeConstants.Huge,
                SizeConstants.Large,
                SizeConstants.Medium,
                SizeConstants.Small,
                SizeConstants.Tiny,
                SizeConstants.Diminutive,
                SizeConstants.Fine,
            };

            AssertCollectionNames(names);
        }

        [TestCase(SizeConstants.Colossal, -8)]
        [TestCase(SizeConstants.Gargantuan, -4)]
        [TestCase(SizeConstants.Huge, -2)]
        [TestCase(SizeConstants.Large, -1)]
        [TestCase(SizeConstants.Medium, 0)]
        [TestCase(SizeConstants.Small, 1)]
        [TestCase(SizeConstants.Tiny, 2)]
        [TestCase(SizeConstants.Diminutive, 4)]
        [TestCase(SizeConstants.Fine, 8)]
        public void SizeModifier(string size, int modifier)
        {
            base.AssertAdjustment(size, modifier);
        }
    }
}
