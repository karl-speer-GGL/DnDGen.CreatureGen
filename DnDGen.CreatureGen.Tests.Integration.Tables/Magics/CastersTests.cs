using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Selectors.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Magics
{
    [TestFixture]
    public class CastersTests : TypesAndAmountsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.Casters;

        private ICollectionDataSelector<AttackDataSelection> attackDataSelector;

        [SetUp]
        public void Setup()
        {
            attackDataSelector = GetNewInstanceOf<ICollectionDataSelector<AttackDataSelection>>();
        }

        [Test]
        public void CastersNames()
        {
            var names = CreatureConstants.GetAll();
            AssertCollectionNames(names);
        }

        [TestCase(CreatureConstants.Androsphinx, SpellConstants.Casters.Cleric, 6)]
        [TestCase(CreatureConstants.Angel_Planetar, SpellConstants.Casters.Cleric, 17)]
        [TestCase(CreatureConstants.Angel_Solar, SpellConstants.Casters.Cleric, 20)]
        [TestCase(CreatureConstants.Aranea, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Couatl, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Black_YoungAdult, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Black_Adult, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Black_MatureAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Black_Old, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Black_VeryOld, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Black_Ancient, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrm, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Blue_Juvenile, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Blue_YoungAdult, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Blue_Adult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Blue_MatureAdult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Blue_Old, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryOld, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Blue_Ancient, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrm, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Green_Juvenile, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Green_YoungAdult, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Green_Adult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Green_MatureAdult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Green_Old, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Green_VeryOld, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Green_Ancient, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrm, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Red_Young, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Red_Juvenile, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Red_YoungAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Red_Adult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Red_MatureAdult, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Red_Old, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Red_VeryOld, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Red_Ancient, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm, SpellConstants.Casters.Sorcerer, 19)]
        [TestCase(CreatureConstants.Dragon_White_Adult, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_White_MatureAdult, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_White_Old, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_White_Ancient, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Brass_Young, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Brass_Old, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm, SpellConstants.Casters.Sorcerer, 19)]
        [TestCase(CreatureConstants.Dragon_Bronze_Young, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Bronze_Juvenile, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Bronze_YoungAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Bronze_Adult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Bronze_MatureAdult, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Bronze_Old, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryOld, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Bronze_Ancient, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm, SpellConstants.Casters.Sorcerer, 19)]
        [TestCase(CreatureConstants.Dragon_Copper_Young, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Copper_Juvenile, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Copper_YoungAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Copper_Adult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Copper_MatureAdult, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Copper_Old, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryOld, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Copper_Ancient, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm, SpellConstants.Casters.Sorcerer, 19)]
        [TestCase(CreatureConstants.Dragon_Gold_Young, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Gold_Juvenile, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Gold_YoungAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Gold_Adult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Gold_MatureAdult, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Gold_Old, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryOld, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Gold_Ancient, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm, SpellConstants.Casters.Sorcerer, 19)]
        [TestCase(CreatureConstants.Dragon_Silver_Young, SpellConstants.Casters.Sorcerer, 1)]
        [TestCase(CreatureConstants.Dragon_Silver_Juvenile, SpellConstants.Casters.Sorcerer, 3)]
        [TestCase(CreatureConstants.Dragon_Silver_YoungAdult, SpellConstants.Casters.Sorcerer, 5)]
        [TestCase(CreatureConstants.Dragon_Silver_Adult, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Dragon_Silver_MatureAdult, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Dragon_Silver_Old, SpellConstants.Casters.Sorcerer, 11)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryOld, SpellConstants.Casters.Sorcerer, 13)]
        [TestCase(CreatureConstants.Dragon_Silver_Ancient, SpellConstants.Casters.Sorcerer, 15)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrm, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm, SpellConstants.Casters.Sorcerer, 19)]
        [TestCase(CreatureConstants.Drider, SpellConstants.Casters.Sorcerer, 6)]
        [TestCase(CreatureConstants.FormianQueen, SpellConstants.Casters.Sorcerer, 17)]
        [TestCase(CreatureConstants.Ghaele, SpellConstants.Casters.Cleric, 14)]
        [TestCase(CreatureConstants.Lammasu, SpellConstants.Casters.Cleric, 7)]
        [TestCase(CreatureConstants.Lillend, SpellConstants.Casters.Bard, 6)]
        [TestCase(CreatureConstants.Naga_Dark, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Naga_Guardian, SpellConstants.Casters.Sorcerer, 9)]
        [TestCase(CreatureConstants.Naga_Spirit, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Naga_Water, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.Nymph, SpellConstants.Casters.Druid, 7)]
        [TestCase(CreatureConstants.Rakshasa, SpellConstants.Casters.Sorcerer, 7)]
        [TestCase(CreatureConstants.TrumpetArchon, SpellConstants.Casters.Cleric, 14)]
        public void CreatureCaster(string creature, string caster, int casterLevel)
        {
            var typeAndAmount = new Dictionary<string, int>
            {
                [caster] = casterLevel
            };

            AssertTypesAndAmounts(creature, typeAndAmount);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreaturesWithCasterHaveSpellAttack(string creature)
        {
            var attacks = attackDataSelector.SelectFrom(Config.Name, TableNameConstants.Collection.AttackData, creature);
            var hasCaster = table[creature].Any();

            Assert.That(attacks.Any(a => a.Name == "Spells"), Is.EqualTo(hasCaster));
        }
    }
}
