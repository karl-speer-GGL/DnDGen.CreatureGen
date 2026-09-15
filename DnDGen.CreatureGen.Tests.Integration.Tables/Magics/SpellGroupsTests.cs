using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Helpers;
using DnDGen.Infrastructure.Models;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Magics
{
    [TestFixture]
    public class SpellGroupsTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.SpellGroups;

        [Test]
        public void SpellGroupsHaveAllNames()
        {
            var casters = new[]
            {
                SpellConstants.Casters.Bard,
                SpellConstants.Casters.Cleric,
                SpellConstants.Casters.Druid,
                SpellConstants.Casters.Sorcerer,
            };

            var domains = new[]
            {
                SpellConstants.Domains.Air,
                SpellConstants.Domains.Animal,
                SpellConstants.Domains.Chaos,
                SpellConstants.Domains.Destruction,
                SpellConstants.Domains.Earth,
                SpellConstants.Domains.Enchantment,
                SpellConstants.Domains.Evil,
                SpellConstants.Domains.Fire,
                SpellConstants.Domains.Good,
                SpellConstants.Domains.Healing,
                SpellConstants.Domains.Illusion,
                SpellConstants.Domains.Knowledge,
                SpellConstants.Domains.Law,
                SpellConstants.Domains.Luck,
                SpellConstants.Domains.Plant,
                SpellConstants.Domains.Protection,
                SpellConstants.Domains.Sun,
                SpellConstants.Domains.Trickery,
                SpellConstants.Domains.War,
                SpellConstants.Domains.Water,
            };

            var creatureProhibited = CreatureConstants.GetAll()
                .Select(c => $"{c}:Prohibited");

            var alignmentProhibited = new[]
            {
                $"{AlignmentConstants.Chaotic}:Prohibited",
                $"{AlignmentConstants.Neutral}:Prohibited",
                $"{AlignmentConstants.Evil}:Prohibited",
                $"{AlignmentConstants.Lawful}:Prohibited",
                $"{AlignmentConstants.Good}:Prohibited",
            };

            var casterProhibited = casters
                .Select(c => $"{c}:Prohibited");

            var domainProhibited = domains
                .Select(d => $"{d}:Prohibited");

            var names = casters
                .Union(domains)
                .Union(creatureProhibited)
                .Union(alignmentProhibited)
                .Union(casterProhibited)
                .Union(domainProhibited);

            AssertCollectionNames(names);
        }

        [TestCase(SpellConstants.Casters.Bard)]
        [TestCase(SpellConstants.Casters.Cleric)]
        [TestCase(SpellConstants.Casters.Druid)]
        [TestCase(SpellConstants.Casters.Sorcerer)]
        [TestCase(SpellConstants.Domains.Air)]
        [TestCase(SpellConstants.Domains.Animal)]
        [TestCase(SpellConstants.Domains.Chaos)]
        [TestCase(SpellConstants.Domains.Destruction)]
        [TestCase(SpellConstants.Domains.Earth)]
        [TestCase(SpellConstants.Domains.Enchantment)]
        [TestCase(SpellConstants.Domains.Evil)]
        [TestCase(SpellConstants.Domains.Fire)]
        [TestCase(SpellConstants.Domains.Good)]
        [TestCase(SpellConstants.Domains.Healing)]
        [TestCase(SpellConstants.Domains.Illusion)]
        [TestCase(SpellConstants.Domains.Knowledge)]
        [TestCase(SpellConstants.Domains.Law)]
        [TestCase(SpellConstants.Domains.Luck)]
        [TestCase(SpellConstants.Domains.Plant)]
        [TestCase(SpellConstants.Domains.Protection)]
        [TestCase(SpellConstants.Domains.Sun)]
        [TestCase(SpellConstants.Domains.Trickery)]
        [TestCase(SpellConstants.Domains.War)]
        [TestCase(SpellConstants.Domains.Water)]
        public void SpellGroupMatchesSpellLevels(string group)
        {
            var spellLevels = collectionMapper.Map(Config.Name, TableNameConstants.TypeAndAmount.SpellLevels);
            Assert.That(spellLevels, Contains.Key(group));

            var spells = spellLevels[group]
                .Select(DataHelper.Parse<TypeAndAmountDataSelection>)
                .Select(s => s.Type)
                .ToArray();

            AssertCollection(group, spells);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureWithoutCasterHasNoProhibitedSpells(string creature)
        {
            var casters = collectionMapper.Map(Config.Name, TableNameConstants.TypeAndAmount.Casters);
            Assert.That(casters, Contains.Key(creature));

            if (!casters[creature].Any())
            {
                AssertCollection($"{creature}:Prohibited");
            }
        }

        [TestCase(CreatureConstants.Androsphinx)]
        [TestCase(CreatureConstants.Angel_Planetar)]
        [TestCase(CreatureConstants.Angel_Solar)]
        [TestCase(CreatureConstants.Aranea,
            SpellConstants.BurningHands,
            SpellConstants.ContinualFlame,
            SpellConstants.DelayedBlastFireball,
            SpellConstants.Fireball,
            SpellConstants.FireSeeds,
            SpellConstants.FireShield,
            SpellConstants.FireStorm,
            SpellConstants.FireTrap,
            SpellConstants.FlameArrow,
            SpellConstants.FlameBlade,
            SpellConstants.FlameStrike,
            SpellConstants.FlamingSphere,
            SpellConstants.MeteorSwarm,
            SpellConstants.Pyrotechnics,
            SpellConstants.ScorchingRay,
            SpellConstants.WallOfFire)]
        [TestCase(CreatureConstants.Couatl)]
        [TestCase(CreatureConstants.Dragon_Black_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Black_Adult)]
        [TestCase(CreatureConstants.Dragon_Black_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Black_Old)]
        [TestCase(CreatureConstants.Dragon_Black_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Black_Ancient)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Blue_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Blue_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Blue_Adult)]
        [TestCase(CreatureConstants.Dragon_Blue_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Blue_Old)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Blue_Ancient)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Green_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Green_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Green_Adult)]
        [TestCase(CreatureConstants.Dragon_Green_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Green_Old)]
        [TestCase(CreatureConstants.Dragon_Green_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Green_Ancient)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Red_Young)]
        [TestCase(CreatureConstants.Dragon_Red_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Red_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Red_Adult)]
        [TestCase(CreatureConstants.Dragon_Red_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Red_Old)]
        [TestCase(CreatureConstants.Dragon_Red_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Red_Ancient)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_White_Adult)]
        [TestCase(CreatureConstants.Dragon_White_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_White_Old)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld)]
        [TestCase(CreatureConstants.Dragon_White_Ancient)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Brass_Young)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Brass_Old)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Bronze_Young)]
        [TestCase(CreatureConstants.Dragon_Bronze_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Bronze_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Bronze_Adult)]
        [TestCase(CreatureConstants.Dragon_Bronze_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Bronze_Old)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Bronze_Ancient)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Copper_Young)]
        [TestCase(CreatureConstants.Dragon_Copper_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Copper_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Copper_Adult)]
        [TestCase(CreatureConstants.Dragon_Copper_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Copper_Old)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Copper_Ancient)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Gold_Young)]
        [TestCase(CreatureConstants.Dragon_Gold_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Gold_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Gold_Adult)]
        [TestCase(CreatureConstants.Dragon_Gold_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Gold_Old)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Gold_Ancient)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Silver_Young)]
        [TestCase(CreatureConstants.Dragon_Silver_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Silver_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Silver_Adult)]
        [TestCase(CreatureConstants.Dragon_Silver_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Silver_Old)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Silver_Ancient)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm)]
        [TestCase(CreatureConstants.Drider)]
        [TestCase(CreatureConstants.FormianQueen)]
        [TestCase(CreatureConstants.Ghaele)]
        [TestCase(CreatureConstants.Lammasu)]
        [TestCase(CreatureConstants.Lillend)]
        [TestCase(CreatureConstants.Naga_Dark)]
        [TestCase(CreatureConstants.Naga_Guardian)]
        [TestCase(CreatureConstants.Naga_Spirit)]
        [TestCase(CreatureConstants.Naga_Water,
            SpellConstants.BurningHands,
            SpellConstants.ContinualFlame,
            SpellConstants.DelayedBlastFireball,
            SpellConstants.Fireball,
            SpellConstants.FireSeeds,
            SpellConstants.FireShield,
            SpellConstants.FireStorm,
            SpellConstants.FireTrap,
            SpellConstants.FlameArrow,
            SpellConstants.FlameBlade,
            SpellConstants.FlameStrike,
            SpellConstants.FlamingSphere,
            SpellConstants.MeteorSwarm,
            SpellConstants.Pyrotechnics,
            SpellConstants.ScorchingRay,
            SpellConstants.WallOfFire)]
        [TestCase(CreatureConstants.Nymph)]
        [TestCase(CreatureConstants.Rakshasa)]
        [TestCase(CreatureConstants.TrumpetArchon)]
        [TestCase(SpellConstants.Casters.Bard)]
        [TestCase(SpellConstants.Casters.Cleric)]
        [TestCase(SpellConstants.Casters.Druid)]
        [TestCase(SpellConstants.Casters.Sorcerer)]
        [TestCase(SpellConstants.Domains.Air)]
        [TestCase(SpellConstants.Domains.Animal)]
        [TestCase(SpellConstants.Domains.Chaos)]
        [TestCase(SpellConstants.Domains.Destruction)]
        [TestCase(SpellConstants.Domains.Earth)]
        [TestCase(SpellConstants.Domains.Enchantment)]
        [TestCase(SpellConstants.Domains.Evil, Ignore = "Duplicates the alignment check")]
        [TestCase(SpellConstants.Domains.Fire)]
        [TestCase(SpellConstants.Domains.Good, Ignore = "Duplicates the alignment check")]
        [TestCase(SpellConstants.Domains.Healing)]
        [TestCase(SpellConstants.Domains.Illusion)]
        [TestCase(SpellConstants.Domains.Knowledge)]
        [TestCase(SpellConstants.Domains.Law)]
        [TestCase(SpellConstants.Domains.Luck)]
        [TestCase(SpellConstants.Domains.Plant)]
        [TestCase(SpellConstants.Domains.Protection)]
        [TestCase(SpellConstants.Domains.Sun)]
        [TestCase(SpellConstants.Domains.Trickery)]
        [TestCase(SpellConstants.Domains.War)]
        [TestCase(SpellConstants.Domains.Water)]
        [TestCase(AlignmentConstants.Chaotic,
            SpellConstants.ProtectionFromChaos,
            SpellConstants.MagicCircleAgainstChaos,
            SpellConstants.OrdersWrath,
            SpellConstants.DispelChaos,
            SpellConstants.ShieldOfLaw)]
        [TestCase(AlignmentConstants.Lawful,
            SpellConstants.ProtectionFromLaw,
            SpellConstants.MagicCircleAgainstLaw,
            SpellConstants.ChaosHammer,
            SpellConstants.DispelLaw,
            SpellConstants.WordOfChaos,
            SpellConstants.CloakOfChaos)]
        [TestCase(AlignmentConstants.Neutral)]
        [TestCase(AlignmentConstants.Good,
            SpellConstants.ProtectionFromGood,
            SpellConstants.Desecrate,
            SpellConstants.MagicCircleAgainstGood,
            SpellConstants.UnholyBlight,
            SpellConstants.DispelGood,
            SpellConstants.Blasphemy,
            SpellConstants.UnholyAura)]
        [TestCase(AlignmentConstants.Evil,
            SpellConstants.ProtectionFromEvil,
            SpellConstants.Consecrate,
            SpellConstants.MagicCircleAgainstEvil,
            SpellConstants.DispelEvil,
            SpellConstants.HolySmite,
            SpellConstants.HolyWord,
            SpellConstants.HolySword,
            SpellConstants.HolyAura)]
        public void ProhibitedSpells(string name, params string[] spells)
        {
            AssertCollection($"{name}:Prohibited", spells);
        }
    }
}
