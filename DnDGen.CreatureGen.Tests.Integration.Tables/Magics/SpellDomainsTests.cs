using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Magics
{
    [TestFixture]
    public class SpellDomainsTests : TypesAndAmountsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.SpellDomains;

        [Test]
        public void SpellDomainsContainsAllCreatures()
        {
            var names = CreatureConstants.GetAll();
            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreaturesWithoutCasterDoNotHaveDomains(string creature)
        {
            var casterTable = collectionMapper.Map(Config.Name, TableNameConstants.TypeAndAmount.Casters);

            if (!casterTable[creature].Any())
                Assert.That(table[creature], Is.Empty);
        }

        [TestCase(CreatureConstants.Androsphinx, 3,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Healing,
            SpellConstants.Domains.Protection)]
        [TestCase(CreatureConstants.Angel_Planetar, 2,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Destruction,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.War)]
        [TestCase(CreatureConstants.Angel_Solar, 2,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Destruction,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.War)]
        [TestCase(CreatureConstants.Aranea, 2,
            SpellConstants.Domains.Illusion,
            SpellConstants.Domains.Enchantment)]
        [TestCase(CreatureConstants.Couatl, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Black_YoungAdult, 0)]
        [TestCase(CreatureConstants.Dragon_Black_Adult, 0)]
        [TestCase(CreatureConstants.Dragon_Black_MatureAdult, 0)]
        [TestCase(CreatureConstants.Dragon_Black_Old, 0)]
        [TestCase(CreatureConstants.Dragon_Black_VeryOld, 0)]
        [TestCase(CreatureConstants.Dragon_Black_Ancient, 0)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrm, 0)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm, 0)]
        [TestCase(CreatureConstants.Dragon_Blue_Juvenile, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_YoungAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_Adult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_MatureAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_Old, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryOld, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_Ancient, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Dragon_Green_Juvenile, 0)]
        [TestCase(CreatureConstants.Dragon_Green_YoungAdult, 0)]
        [TestCase(CreatureConstants.Dragon_Green_Adult, 0)]
        [TestCase(CreatureConstants.Dragon_Green_MatureAdult, 0)]
        [TestCase(CreatureConstants.Dragon_Green_Old, 0)]
        [TestCase(CreatureConstants.Dragon_Green_VeryOld, 0)]
        [TestCase(CreatureConstants.Dragon_Green_Ancient, 0)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrm, 0)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm, 0)]
        [TestCase(CreatureConstants.Dragon_Red_Young, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_Juvenile, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_YoungAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_Adult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_MatureAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_Old, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_VeryOld, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_Ancient, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Fire)]
        [TestCase(CreatureConstants.Dragon_White_Adult, 0)]
        [TestCase(CreatureConstants.Dragon_White_MatureAdult, 0)]
        [TestCase(CreatureConstants.Dragon_White_Old, 0)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld, 0)]
        [TestCase(CreatureConstants.Dragon_White_Ancient, 0)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm, 0)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm, 0)]
        [TestCase(CreatureConstants.Dragon_Brass_Young, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_Old, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Knowledge)]
        [TestCase(CreatureConstants.Dragon_Bronze_Young, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_Juvenile, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_YoungAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_Adult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_MatureAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_Old, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryOld, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_Ancient, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Water)]
        [TestCase(CreatureConstants.Dragon_Copper_Young, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_Juvenile, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_YoungAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_Adult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_MatureAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_Old, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryOld, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_Ancient, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Earth,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.Dragon_Gold_Young, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_Juvenile, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_YoungAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_Adult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_MatureAdult, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_Old, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryOld, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_Ancient, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm, 4,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Luck,
            SpellConstants.Domains.Good)]
        [TestCase(CreatureConstants.Dragon_Silver_Young, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_Juvenile, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_YoungAdult, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_Adult, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_MatureAdult, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_Old, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryOld, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_Ancient, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrm, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm, 5,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.Sun)]
        [TestCase(CreatureConstants.Drider, 2,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Destruction,
            SpellConstants.Domains.Evil,
            SpellConstants.Domains.Trickery)]
        [TestCase(CreatureConstants.FormianQueen, 0)]
        [TestCase(CreatureConstants.Ghaele, 2,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Animal,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Plant)]
        [TestCase(CreatureConstants.Lammasu, 2,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Healing,
            SpellConstants.Domains.Knowledge,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Lillend, 0)]
        [TestCase(CreatureConstants.Naga_Dark, 0)]
        [TestCase(CreatureConstants.Naga_Guardian, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law)]
        [TestCase(CreatureConstants.Naga_Spirit, 3,
            SpellConstants.Casters.Cleric,
            SpellConstants.Domains.Chaos,
            SpellConstants.Domains.Evil)]
        [TestCase(CreatureConstants.Naga_Water, 0)]
        [TestCase(CreatureConstants.Nymph, 0)]
        [TestCase(CreatureConstants.Rakshasa, 0)]
        [TestCase(CreatureConstants.TrumpetArchon, 2,
            SpellConstants.Domains.Air,
            SpellConstants.Domains.Destruction,
            SpellConstants.Domains.Good,
            SpellConstants.Domains.Law,
            SpellConstants.Domains.War)]
        public void CreatureDomains(string creature, int domainCount, params string[] domains)
        {
            var typeAndAmount = new Dictionary<string, int>();

            foreach (var domain in domains)
            {
                typeAndAmount[domain] = domainCount;
            }

            AssertTypesAndAmounts(creature, typeAndAmount);
        }
    }
}
