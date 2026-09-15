using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.Tables.Helpers;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures
{
    [TestFixture]
    public class CreatureDataTests : CollectionTests
    {
        private Dictionary<string, string> creatureData;
        private SpaceReachHelper spaceReachHelper;
        private Dictionary<string, BaseAttackQuality> baseAttackQualities;
        private Dictionary<string, string[]> creatureTypes;
        private Dictionary<string, double> creatureHitDiceQuantities;
        private Dictionary<string, int> hitDies;

        protected override string TableName => TableNameConstants.Collection.CreatureData;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            spaceReachHelper = GetNewInstanceOf<SpaceReachHelper>();
            creatureTypes = GetCreatureTypes();
            creatureHitDiceQuantities = GetCreatureHitDiceQuantities();

            baseAttackQualities = new Dictionary<string, BaseAttackQuality>
            {
                [CreatureConstants.Types.Aberration] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Animal] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Construct] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Elemental] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Giant] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Humanoid] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Ooze] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Plant] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Vermin] = BaseAttackQuality.Average,
                [CreatureConstants.Types.Fey] = BaseAttackQuality.Poor,
                [CreatureConstants.Types.Undead] = BaseAttackQuality.Poor,
                [CreatureConstants.Types.Dragon] = BaseAttackQuality.Good,
                [CreatureConstants.Types.MagicalBeast] = BaseAttackQuality.Good,
                [CreatureConstants.Types.MonstrousHumanoid] = BaseAttackQuality.Good,
                [CreatureConstants.Types.Outsider] = BaseAttackQuality.Good,
            };

            hitDies = new Dictionary<string, int>
            {
                [CreatureConstants.Types.Aberration] = 8,
                [CreatureConstants.Types.Animal] = 8,
                [CreatureConstants.Types.Construct] = 10,
                [CreatureConstants.Types.Dragon] = 12,
                [CreatureConstants.Types.Elemental] = 8,
                [CreatureConstants.Types.Fey] = 6,
                [CreatureConstants.Types.Giant] = 8,
                [CreatureConstants.Types.Humanoid] = 8,
                [CreatureConstants.Types.MagicalBeast] = 10,
                [CreatureConstants.Types.MonstrousHumanoid] = 8,
                [CreatureConstants.Types.Ooze] = 10,
                [CreatureConstants.Types.Outsider] = 8,
                [CreatureConstants.Types.Plant] = 8,
                [CreatureConstants.Types.Undead] = 12,
                [CreatureConstants.Types.Vermin] = 8,
            };

            creatureData = GetCreatureTestData();
        }

        private Dictionary<string, string[]> GetCreatureTypes()
        {
            var types = new Dictionary<string, string[]>
            {
                [CreatureConstants.Aasimar] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Native],
                [CreatureConstants.Aboleth] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Achaierai] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Allip] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Incorporeal],
                [CreatureConstants.Androsphinx] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.AnimatedObject_Colossal] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Colossal_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Gargantuan_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Huge_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Long] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Large_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_MultipleLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Medium_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_MultipleLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Small_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_Flexible] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_MultipleLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_Sheetlike] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_TwoLegs] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.AnimatedObject_Tiny_Wooden] = [CreatureConstants.Types.Construct],
                [CreatureConstants.Ankheg] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Annis] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Ant_Giant_Queen] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Ant_Giant_Soldier] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Ant_Giant_Worker] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Ape] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Ape_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Aranea] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Shapechanger],
                [CreatureConstants.HoundArchon] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Archon,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.LanternArchon] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Archon,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.TrumpetArchon] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Archon,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Arrowhawk_Adult] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Arrowhawk_Elder] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Arrowhawk_Juvenile] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Angel_AstralDeva] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Angel,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good],
                [CreatureConstants.Angel_Planetar] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Angel,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good],
                [CreatureConstants.Angel_Solar] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Angel,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good],
                [CreatureConstants.AssassinVine] = [CreatureConstants.Types.Plant],
                [CreatureConstants.Athach] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Avoral] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Good,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Azer] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Babau] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Baboon] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Badger] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Badger_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Balor] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.BarbedDevil_Hamatula] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Barghest] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Shapechanger],
                [CreatureConstants.Barghest_Greater] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Shapechanger],
                [CreatureConstants.Basilisk] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Basilisk_Greater] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Bat] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Bat_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Bat_Swarm] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Swarm],
                [CreatureConstants.Bear_Black] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Bear_Brown] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Bear_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Bear_Polar] = [CreatureConstants.Types.Animal],
                [CreatureConstants.BeardedDevil_Barbazu] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Bebilith] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Bee_Giant] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Behir] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Beholder] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Beholder_Gauth] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Belker] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Bison] = [CreatureConstants.Types.Animal],
                [CreatureConstants.BlackPudding] = [CreatureConstants.Types.Ooze],
                [CreatureConstants.BlackPudding_Elder] = [CreatureConstants.Types.Ooze],
                [CreatureConstants.BlinkDog] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Boar] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Boar_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Bodak] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.BombardierBeetle_Giant] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.BoneDevil_Osyluth] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Bralani] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Good,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Bugbear] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Goblinoid],
                [CreatureConstants.Bulette] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Camel_Bactrian] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Camel_Dromedary] = [CreatureConstants.Types.Animal],
                [CreatureConstants.CarrionCrawler] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Cat] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Centaur] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Centipede_Monstrous_Colossal] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Monstrous_Gargantuan] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Monstrous_Huge] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Monstrous_Large] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Monstrous_Medium] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Monstrous_Small] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Monstrous_Tiny] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Centipede_Swarm] = [CreatureConstants.Types.Vermin,
                    CreatureConstants.Types.Subtypes.Swarm],
                [CreatureConstants.ChainDevil_Kyton] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.ChaosBeast] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Cheetah] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Chimera_Black] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Chimera_Blue] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Chimera_Green] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Chimera_Red] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Chimera_White] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Choker] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Chuul] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Cloaker] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Cockatrice] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Couatl] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Native],
                [CreatureConstants.Criosphinx] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Cryohydra_10Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_11Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_12Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_5Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_6Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_7Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_8Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Cryohydra_9Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Crocodile] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Crocodile_Giant] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Darkmantle] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Deinonychus] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Delver] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Derro] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Derro_Sane] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Destrachan] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Devourer] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Digester] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.DisplacerBeast] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.DisplacerBeast_PackLord] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Djinni] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Djinni_Noble] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Dog] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Dog_Riding] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Donkey] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Doppelganger] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Shapechanger],
                [CreatureConstants.Dragon_Black_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Black_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Blue_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Blue_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Green_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Green_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Dragon_Red_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Red_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_White_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_White_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Brass_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Brass_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Bronze_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Bronze_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Water],
                [CreatureConstants.Dragon_Copper_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Copper_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Dragon_Gold_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Gold_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Dragon_Silver_Adult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_Ancient] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_GreatWyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_Juvenile] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_MatureAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_Old] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_VeryOld] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_VeryYoung] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_Wyrm] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_Wyrmling] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_Young] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Dragon_Silver_YoungAdult] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.DragonTurtle] = [CreatureConstants.Types.Dragon,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Dragonne] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Dretch] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Drider] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Dryad] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Dwarf_Duergar] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Dwarf],
                [CreatureConstants.Dwarf_Deep] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Dwarf],
                [CreatureConstants.Dwarf_Hill] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Dwarf],
                [CreatureConstants.Dwarf_Mountain] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Dwarf],
                [CreatureConstants.Eagle] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Eagle_Giant] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Efreeti] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elasmosaurus] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Elemental_Air_Elder] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Air_Greater] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Air_Huge] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Air_Large] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Air_Medium] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Air_Small] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Earth_Elder] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Earth_Greater] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Earth_Huge] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Earth_Large] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Earth_Medium] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Earth_Small] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Fire_Elder] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Fire_Greater] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Fire_Huge] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Fire_Large] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Fire_Medium] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Fire_Small] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Water_Elder] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Water_Greater] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Water_Huge] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Water_Large] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Water_Medium] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elemental_Water_Small] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Elephant] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Elf_Aquatic] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Elf_Drow] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf],
                [CreatureConstants.Elf_Gray] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf],
                [CreatureConstants.Elf_Half] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf,
                    CreatureConstants.Types.Subtypes.Human],
                [CreatureConstants.Elf_High] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf],
                [CreatureConstants.Elf_Wild] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf],
                [CreatureConstants.Elf_Wood] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Elf],
                [CreatureConstants.Erinyes] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.EtherealFilcher] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.EtherealMarauder] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Ettercap] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Ettin] = [CreatureConstants.Types.Giant],
                [CreatureConstants.FireBeetle_Giant] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.FormianMyrmarch] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.FormianQueen] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.FormianTaskmaster] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.FormianWarrior] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.FormianWorker] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Lawful,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.FrostWorm] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Gargoyle] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Gargoyle_Kapoacinth] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.GelatinousCube] = [CreatureConstants.Types.Ooze],
                [CreatureConstants.Ghaele] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good],
                [CreatureConstants.Ghoul] = [CreatureConstants.Types.Undead],
                [CreatureConstants.Ghoul_Ghast] = [CreatureConstants.Types.Undead],
                [CreatureConstants.Ghoul_Lacedon] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Giant_Cloud] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.Giant_Fire] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Giant_Frost] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Giant_Hill] = [CreatureConstants.Types.Giant],
                [CreatureConstants.Giant_Stone] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Giant_Stone_Elder] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Earth],
                [CreatureConstants.Giant_Storm] = [CreatureConstants.Types.Giant],
                [CreatureConstants.GibberingMouther] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Girallon] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Githyanki] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Githzerai] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Glabrezu] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Gnoll] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Gnoll],
                [CreatureConstants.Gnome_Forest] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Gnome],
                [CreatureConstants.Gnome_Rock] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Gnome],
                [CreatureConstants.Gnome_Svirfneblin] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Gnome],
                [CreatureConstants.Goblin] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Goblinoid],
                [CreatureConstants.Golem_Clay] = [CreatureConstants.Types.Construct],
                [CreatureConstants.Golem_Flesh] = [CreatureConstants.Types.Construct],
                [CreatureConstants.Golem_Iron] = [CreatureConstants.Types.Construct],
                [CreatureConstants.Golem_Stone] = [CreatureConstants.Types.Construct],
                [CreatureConstants.Golem_Stone_Greater] = [CreatureConstants.Types.Construct],
                [CreatureConstants.Gorgon] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.GrayOoze] = [CreatureConstants.Types.Ooze],
                [CreatureConstants.GrayRender] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.GreenHag] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Grick] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Griffon] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Grig] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Grig_WithFiddle] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Nixie] = [CreatureConstants.Types.Fey,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Pixie] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Pixie_WithIrresistibleDance] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Grimlock] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Gynosphinx] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Halfling_Deep] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Halfling],
                [CreatureConstants.Halfling_Lightfoot] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Halfling],
                [CreatureConstants.Halfling_Tallfellow] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Halfling],
                [CreatureConstants.Harpy] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Hawk] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Hellcat_Bezekira] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.HellHound] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.HellHound_NessianWarhound] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Hellwasp_Swarm] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Swarm],
                [CreatureConstants.Hezrou] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Hieracosphinx] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hippogriff] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hobgoblin] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Goblinoid],
                [CreatureConstants.Homunculus] = [CreatureConstants.Types.Construct],
                [CreatureConstants.HornedDevil_Cornugon] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Horse_Heavy] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Horse_Heavy_War] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Horse_Light] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Horse_Light_War] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Howler] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Human] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Human],
                [CreatureConstants.Hydra_10Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_11Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_12Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_5Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_6Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_7Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_8Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hydra_9Heads] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Hyena] = [CreatureConstants.Types.Animal],
                [CreatureConstants.IceDevil_Gelugon] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Imp] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.InvisibleStalker] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Janni] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Native],
                [CreatureConstants.Kobold] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Reptilian],
                [CreatureConstants.Kolyarut] = [CreatureConstants.Types.Construct,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Marut] = [CreatureConstants.Types.Construct,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Zelekhut] = [CreatureConstants.Types.Construct,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Kraken] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Krenshar] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.KuoToa] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Lamia] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Lammasu] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Lemure] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Leonal] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good],
                [CreatureConstants.Leopard] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Lillend] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Good],
                [CreatureConstants.Lion] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Lion_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Lizard] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Lizard_Monitor] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Lizardfolk] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Reptilian],
                [CreatureConstants.Locathah] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Locust_Swarm] = [CreatureConstants.Types.Vermin,
                    CreatureConstants.Types.Subtypes.Swarm],
                [CreatureConstants.Magmin] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.MantaRay] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Manticore] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Marilith] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Medusa] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Megaraptor] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Mephit_Air] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Dust] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Earth] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Fire] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Ice] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Air,
                    CreatureConstants.Types.Subtypes.Cold,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Magma] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Ooze] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Salt] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Steam] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Fire,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Mephit_Water] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Merfolk] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Mimic] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Shapechanger],
                [CreatureConstants.MindFlayer] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Minotaur] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.Mohrg] = [CreatureConstants.Types.Undead],
                [CreatureConstants.Monkey] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Mule] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Mummy] = [CreatureConstants.Types.Undead],
                [CreatureConstants.Naga_Dark] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Naga_Guardian] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Naga_Spirit] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Naga_Water] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Nalfeshnee] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.NightHag] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Nightmare] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Nightmare_Cauchemar] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Nightcrawler] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Nightwalker] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Nightwing] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Nymph] = [CreatureConstants.Types.Fey],
                [CreatureConstants.OchreJelly] = [CreatureConstants.Types.Ooze],
                [CreatureConstants.Octopus] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Octopus_Giant] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Ogre] = [CreatureConstants.Types.Giant],
                [CreatureConstants.Ogre_Merrow] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.OgreMage] = [CreatureConstants.Types.Giant],
                [CreatureConstants.Orc] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Orc],
                [CreatureConstants.Orc_Half] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Human,
                    CreatureConstants.Types.Subtypes.Orc],
                [CreatureConstants.Otyugh] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Owl] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Owl_Giant] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Owlbear] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Pegasus] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.PhantomFungus] = [CreatureConstants.Types.Plant],
                [CreatureConstants.PhaseSpider] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Phasm] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Shapechanger],
                [CreatureConstants.PitFiend] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Lawful],
                [CreatureConstants.Pony] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Pony_War] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Porpoise] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.PrayingMantis_Giant] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Pseudodragon] = [CreatureConstants.Types.Dragon],
                [CreatureConstants.PurpleWorm] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Pyrohydra_10Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_11Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_12Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_5Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_6Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_7Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_8Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Pyrohydra_9Heads] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Quasit] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Rakshasa] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Native],
                [CreatureConstants.Rast] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Rat] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Rat_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Rat_Swarm] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Swarm],
                [CreatureConstants.Raven] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Ravid] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.RazorBoar] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Remorhaz] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Retriever] = [CreatureConstants.Types.Construct,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Rhinoceras] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Roc] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Roper] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.RustMonster] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Sahuagin] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Sahuagin_Malenti] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Sahuagin_Mutant] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Salamander_Average] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Salamander_Flamebrother] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Salamander_Noble] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Satyr] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Satyr_WithPipes] = [CreatureConstants.Types.Fey],
                [CreatureConstants.Scorpion_Monstrous_Colossal] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpion_Monstrous_Gargantuan] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpion_Monstrous_Huge] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpion_Monstrous_Large] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpion_Monstrous_Medium] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpion_Monstrous_Small] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpion_Monstrous_Tiny] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Scorpionfolk] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.SeaCat] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.SeaHag] = [CreatureConstants.Types.MonstrousHumanoid,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Shadow] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Incorporeal],
                [CreatureConstants.Shadow_Greater] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Incorporeal],
                [CreatureConstants.ShadowMastiff] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.ShamblingMound] = [CreatureConstants.Types.Plant],
                [CreatureConstants.Shark_Dire] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Shark_Huge] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Shark_Large] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Shark_Medium] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.ShieldGuardian] = [CreatureConstants.Types.Construct],
                [CreatureConstants.ShockerLizard] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Shrieker] = [CreatureConstants.Types.Plant],
                [CreatureConstants.VioletFungus] = [CreatureConstants.Types.Plant],
                [CreatureConstants.Skum] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Slaad_Blue] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Slaad_Death] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Slaad_Gray] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Slaad_Green] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Slaad_Red] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Snake_Constrictor] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Snake_Constrictor_Giant] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Snake_Viper_Huge] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Snake_Viper_Large] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Snake_Viper_Medium] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Snake_Viper_Small] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Snake_Viper_Tiny] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Spectre] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Incorporeal],
                [CreatureConstants.Spider_Monstrous_Hunter_Colossal] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_Hunter_Gargantuan] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_Hunter_Huge] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_Hunter_Large] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_Hunter_Medium] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_Hunter_Small] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_Hunter_Tiny] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Colossal] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Huge] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Large] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Medium] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Small] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Monstrous_WebSpinner_Tiny] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Spider_Swarm] = [CreatureConstants.Types.Vermin,
                    CreatureConstants.Types.Subtypes.Swarm],
                [CreatureConstants.SpiderEater] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Squid] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Squid_Giant] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.StagBeetle_Giant] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Stirge] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Succubus] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Tarrasque] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Tendriculos] = [CreatureConstants.Types.Plant],
                [CreatureConstants.Thoqqua] = [CreatureConstants.Types.Elemental,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Fire],
                [CreatureConstants.Tiefling] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Native],
                [CreatureConstants.Tiger] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Tiger_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Titan] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Chaotic,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Toad] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Tojanida_Adult] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Tojanida_Elder] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Tojanida_Juvenile] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Treant] = [CreatureConstants.Types.Plant],
                [CreatureConstants.Triceratops] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Triton] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Water,
                    CreatureConstants.Types.Subtypes.Native],
                [CreatureConstants.Troglodyte] = [CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.Subtypes.Reptilian],
                [CreatureConstants.Troll] = [CreatureConstants.Types.Giant],
                [CreatureConstants.Troll_Scrag] = [CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Tyrannosaurus] = [CreatureConstants.Types.Animal],
                [CreatureConstants.UmberHulk] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.UmberHulk_TrulyHorrid] = [CreatureConstants.Types.Aberration],
                [CreatureConstants.Unicorn] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.VampireSpawn] = [CreatureConstants.Types.Undead],
                [CreatureConstants.Vargouille] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Vrock] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar,
                    CreatureConstants.Types.Subtypes.Chaotic],
                [CreatureConstants.Wasp_Giant] = [CreatureConstants.Types.Vermin],
                [CreatureConstants.Weasel] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Weasel_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Whale_Baleen] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Whale_Cachalot] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Whale_Orca] = [CreatureConstants.Types.Animal,
                    CreatureConstants.Types.Subtypes.Aquatic],
                [CreatureConstants.Wight] = [CreatureConstants.Types.Undead],
                [CreatureConstants.WillOWisp] = [CreatureConstants.Types.Aberration,
                    CreatureConstants.Types.Subtypes.Air],
                [CreatureConstants.WinterWolf] = [CreatureConstants.Types.MagicalBeast,
                    CreatureConstants.Types.Subtypes.Cold],
                [CreatureConstants.Wolf] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Wolf_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Wolverine] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Wolverine_Dire] = [CreatureConstants.Types.Animal],
                [CreatureConstants.Worg] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.Wraith] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Incorporeal],
                [CreatureConstants.Wraith_Dread] = [CreatureConstants.Types.Undead,
                    CreatureConstants.Types.Subtypes.Incorporeal],
                [CreatureConstants.Wyvern] = [CreatureConstants.Types.Dragon],
                [CreatureConstants.Xill] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Xorn_Average] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Xorn_Elder] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Xorn_Minor] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Earth,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.YethHound] = [CreatureConstants.Types.Outsider,
                    CreatureConstants.Types.Subtypes.Evil,
                    CreatureConstants.Types.Subtypes.Extraplanar],
                [CreatureConstants.Yrthak] = [CreatureConstants.Types.MagicalBeast],
                [CreatureConstants.YuanTi_Abomination] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.YuanTi_Halfblood_SnakeArms] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.YuanTi_Halfblood_SnakeHead] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.YuanTi_Halfblood_SnakeTail] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs] = [CreatureConstants.Types.MonstrousHumanoid],
                [CreatureConstants.YuanTi_Pureblood] = [CreatureConstants.Types.MonstrousHumanoid],
            };

            return types;
        }

        private Dictionary<string, double> GetCreatureHitDiceQuantities()
        {
            var quantities = new Dictionary<string, double>
            {
                [CreatureConstants.Aasimar] = 1,
                [CreatureConstants.Aboleth] = 8,
                [CreatureConstants.Achaierai] = 6,
                [CreatureConstants.Allip] = 4,
                [CreatureConstants.Angel_AstralDeva] = 12,
                [CreatureConstants.Angel_Planetar] = 14,
                [CreatureConstants.Angel_Solar] = 22,
                [CreatureConstants.Ankheg] = 3,
                [CreatureConstants.AnimatedObject_Colossal] = 32,
                [CreatureConstants.AnimatedObject_Colossal_Flexible] = 32,
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long] = 32,
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden] = 32,
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall] = 32,
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden] = 32,
                [CreatureConstants.AnimatedObject_Colossal_Sheetlike] = 32,
                [CreatureConstants.AnimatedObject_Colossal_TwoLegs] = 32,
                [CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden] = 32,
                [CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden] = 32,
                [CreatureConstants.AnimatedObject_Colossal_Wooden] = 32,
                [CreatureConstants.AnimatedObject_Gargantuan] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_Flexible] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_Sheetlike] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_TwoLegs] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden] = 16,
                [CreatureConstants.AnimatedObject_Gargantuan_Wooden] = 16,
                [CreatureConstants.AnimatedObject_Huge] = 8,
                [CreatureConstants.AnimatedObject_Huge_Flexible] = 8,
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long] = 8,
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden] = 8,
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall] = 8,
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden] = 8,
                [CreatureConstants.AnimatedObject_Huge_Sheetlike] = 8,
                [CreatureConstants.AnimatedObject_Huge_TwoLegs] = 8,
                [CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden] = 8,
                [CreatureConstants.AnimatedObject_Huge_Wheels_Wooden] = 8,
                [CreatureConstants.AnimatedObject_Huge_Wooden] = 8,
                [CreatureConstants.AnimatedObject_Large] = 4,
                [CreatureConstants.AnimatedObject_Large_Flexible] = 4,
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Long] = 4,
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden] = 4,
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall] = 4,
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden] = 4,
                [CreatureConstants.AnimatedObject_Large_Sheetlike] = 4,
                [CreatureConstants.AnimatedObject_Large_TwoLegs] = 4,
                [CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden] = 4,
                [CreatureConstants.AnimatedObject_Large_Wheels_Wooden] = 4,
                [CreatureConstants.AnimatedObject_Large_Wooden] = 4,
                [CreatureConstants.AnimatedObject_Medium] = 2,
                [CreatureConstants.AnimatedObject_Medium_Flexible] = 2,
                [CreatureConstants.AnimatedObject_Medium_MultipleLegs] = 2,
                [CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden] = 2,
                [CreatureConstants.AnimatedObject_Medium_Sheetlike] = 2,
                [CreatureConstants.AnimatedObject_Medium_TwoLegs] = 2,
                [CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden] = 2,
                [CreatureConstants.AnimatedObject_Medium_Wheels_Wooden] = 2,
                [CreatureConstants.AnimatedObject_Medium_Wooden] = 2,
                [CreatureConstants.AnimatedObject_Small] = 1,
                [CreatureConstants.AnimatedObject_Small_Flexible] = 1,
                [CreatureConstants.AnimatedObject_Small_MultipleLegs] = 1,
                [CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden] = 1,
                [CreatureConstants.AnimatedObject_Small_Sheetlike] = 1,
                [CreatureConstants.AnimatedObject_Small_TwoLegs] = 1,
                [CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden] = 1,
                [CreatureConstants.AnimatedObject_Small_Wheels_Wooden] = 1,
                [CreatureConstants.AnimatedObject_Small_Wooden] = 1,
                [CreatureConstants.AnimatedObject_Tiny] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_Flexible] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_MultipleLegs] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_Sheetlike] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_TwoLegs] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden] = 0.5,
                [CreatureConstants.AnimatedObject_Tiny_Wooden] = 0.5,
                [CreatureConstants.Annis] = 7,
                [CreatureConstants.Ape] = 4,
                [CreatureConstants.Ape_Dire] = 5,
                [CreatureConstants.Arrowhawk_Adult] = 7,
                [CreatureConstants.Arrowhawk_Elder] = 15,
                [CreatureConstants.Arrowhawk_Juvenile] = 3,
                [CreatureConstants.Androsphinx] = 12,
                [CreatureConstants.Ant_Giant_Queen] = 4,
                [CreatureConstants.Ant_Giant_Soldier] = 2,
                [CreatureConstants.Ant_Giant_Worker] = 2,
                [CreatureConstants.Aranea] = 3,
                [CreatureConstants.AssassinVine] = 4,
                [CreatureConstants.Athach] = 14,
                [CreatureConstants.Avoral] = 7,
                [CreatureConstants.Azer] = 2,
                [CreatureConstants.Babau] = 7,
                [CreatureConstants.Baboon] = 1,
                [CreatureConstants.Badger] = 1,
                [CreatureConstants.Badger_Dire] = 3,
                [CreatureConstants.Balor] = 20,
                [CreatureConstants.BarbedDevil_Hamatula] = 12,
                [CreatureConstants.Barghest] = 6,
                [CreatureConstants.Barghest_Greater] = 9,
                [CreatureConstants.Basilisk] = 6,
                [CreatureConstants.Basilisk_Greater] = 18,
                [CreatureConstants.Bat] = .25,
                [CreatureConstants.Bat_Dire] = 4,
                [CreatureConstants.Bat_Swarm] = 3,
                [CreatureConstants.Bear_Black] = 3,
                [CreatureConstants.Bear_Brown] = 6,
                [CreatureConstants.Bear_Dire] = 12,
                [CreatureConstants.Bear_Polar] = 8,
                [CreatureConstants.BeardedDevil_Barbazu] = 6,
                [CreatureConstants.Bebilith] = 12,
                [CreatureConstants.Bee_Giant] = 3,
                [CreatureConstants.Behir] = 9,
                [CreatureConstants.Beholder] = 11,
                [CreatureConstants.Beholder_Gauth] = 6,
                [CreatureConstants.Belker] = 7,
                [CreatureConstants.Bison] = 5,
                [CreatureConstants.BlackPudding] = 10,
                [CreatureConstants.BlackPudding_Elder] = 20,
                [CreatureConstants.BlinkDog] = 4,
                [CreatureConstants.Boar] = 3,
                [CreatureConstants.Boar_Dire] = 7,
                [CreatureConstants.Bodak] = 9,
                [CreatureConstants.BombardierBeetle_Giant] = 2,
                [CreatureConstants.BoneDevil_Osyluth] = 10,
                [CreatureConstants.Bralani] = 6,
                [CreatureConstants.Bugbear] = 3,
                [CreatureConstants.Bulette] = 9,
                [CreatureConstants.Camel_Bactrian] = 3,
                [CreatureConstants.Camel_Dromedary] = 3,
                [CreatureConstants.CarrionCrawler] = 3,
                [CreatureConstants.Cat] = .5,
                [CreatureConstants.Centaur] = 4,
                [CreatureConstants.Centipede_Monstrous_Colossal] = 24,
                [CreatureConstants.Centipede_Monstrous_Gargantuan] = 12,
                [CreatureConstants.Centipede_Monstrous_Huge] = 6,
                [CreatureConstants.Centipede_Monstrous_Large] = 3,
                [CreatureConstants.Centipede_Monstrous_Medium] = 1,
                [CreatureConstants.Centipede_Monstrous_Small] = .5,
                [CreatureConstants.Centipede_Monstrous_Tiny] = .25,
                [CreatureConstants.Centipede_Swarm] = 9,
                [CreatureConstants.ChainDevil_Kyton] = 8,
                [CreatureConstants.ChaosBeast] = 8,
                [CreatureConstants.Cheetah] = 3,
                [CreatureConstants.Chimera_Black] = 9,
                [CreatureConstants.Chimera_Blue] = 9,
                [CreatureConstants.Chimera_Green] = 9,
                [CreatureConstants.Chimera_Red] = 9,
                [CreatureConstants.Chimera_White] = 9,
                [CreatureConstants.Choker] = 3,
                [CreatureConstants.Chuul] = 11,
                [CreatureConstants.Cloaker] = 6,
                [CreatureConstants.Cockatrice] = 5,
                [CreatureConstants.Couatl] = 9,
                [CreatureConstants.Criosphinx] = 10,
                [CreatureConstants.Crocodile] = 3,
                [CreatureConstants.Crocodile_Giant] = 7,
                [CreatureConstants.Cryohydra_10Heads] = 10,
                [CreatureConstants.Cryohydra_11Heads] = 11,
                [CreatureConstants.Cryohydra_12Heads] = 12,
                [CreatureConstants.Cryohydra_5Heads] = 5,
                [CreatureConstants.Cryohydra_6Heads] = 6,
                [CreatureConstants.Cryohydra_7Heads] = 7,
                [CreatureConstants.Cryohydra_8Heads] = 8,
                [CreatureConstants.Cryohydra_9Heads] = 9,
                [CreatureConstants.Darkmantle] = 1,
                [CreatureConstants.Deinonychus] = 4,
                [CreatureConstants.Delver] = 15,
                [CreatureConstants.Derro] = 3,
                [CreatureConstants.Derro_Sane] = 3,
                [CreatureConstants.Destrachan] = 8,
                [CreatureConstants.Devourer] = 12,
                [CreatureConstants.Digester] = 8,
                [CreatureConstants.DisplacerBeast] = 6,
                [CreatureConstants.DisplacerBeast_PackLord] = 18,
                [CreatureConstants.Djinni] = 7,
                [CreatureConstants.Djinni_Noble] = 10,
                [CreatureConstants.Dog] = 1,
                [CreatureConstants.Dog_Riding] = 2,
                [CreatureConstants.Donkey] = 2,
                [CreatureConstants.Doppelganger] = 4,
                [CreatureConstants.Dragon_Black_Wyrmling] = 4,
                [CreatureConstants.Dragon_Black_VeryYoung] = 7,
                [CreatureConstants.Dragon_Black_Young] = 10,
                [CreatureConstants.Dragon_Black_Juvenile] = 13,
                [CreatureConstants.Dragon_Black_YoungAdult] = 16,
                [CreatureConstants.Dragon_Black_Adult] = 19,
                [CreatureConstants.Dragon_Black_MatureAdult] = 22,
                [CreatureConstants.Dragon_Black_Old] = 25,
                [CreatureConstants.Dragon_Black_VeryOld] = 28,
                [CreatureConstants.Dragon_Black_Ancient] = 31,
                [CreatureConstants.Dragon_Black_Wyrm] = 34,
                [CreatureConstants.Dragon_Black_GreatWyrm] = 37,
                [CreatureConstants.Dragon_Blue_Wyrmling] = 6,
                [CreatureConstants.Dragon_Blue_VeryYoung] = 9,
                [CreatureConstants.Dragon_Blue_Young] = 12,
                [CreatureConstants.Dragon_Blue_Juvenile] = 15,
                [CreatureConstants.Dragon_Blue_YoungAdult] = 18,
                [CreatureConstants.Dragon_Blue_Adult] = 21,
                [CreatureConstants.Dragon_Blue_MatureAdult] = 24,
                [CreatureConstants.Dragon_Blue_Old] = 27,
                [CreatureConstants.Dragon_Blue_VeryOld] = 30,
                [CreatureConstants.Dragon_Blue_Ancient] = 33,
                [CreatureConstants.Dragon_Blue_Wyrm] = 36,
                [CreatureConstants.Dragon_Blue_GreatWyrm] = 39,
                [CreatureConstants.Dragon_Green_Wyrmling] = 5,
                [CreatureConstants.Dragon_Green_VeryYoung] = 8,
                [CreatureConstants.Dragon_Green_Young] = 11,
                [CreatureConstants.Dragon_Green_Juvenile] = 14,
                [CreatureConstants.Dragon_Green_YoungAdult] = 17,
                [CreatureConstants.Dragon_Green_Adult] = 20,
                [CreatureConstants.Dragon_Green_MatureAdult] = 23,
                [CreatureConstants.Dragon_Green_Old] = 26,
                [CreatureConstants.Dragon_Green_VeryOld] = 29,
                [CreatureConstants.Dragon_Green_Ancient] = 32,
                [CreatureConstants.Dragon_Green_Wyrm] = 35,
                [CreatureConstants.Dragon_Green_GreatWyrm] = 38,
                [CreatureConstants.Dragon_Red_Wyrmling] = 7,
                [CreatureConstants.Dragon_Red_VeryYoung] = 10,
                [CreatureConstants.Dragon_Red_Young] = 13,
                [CreatureConstants.Dragon_Red_Juvenile] = 16,
                [CreatureConstants.Dragon_Red_YoungAdult] = 19,
                [CreatureConstants.Dragon_Red_Adult] = 22,
                [CreatureConstants.Dragon_Red_MatureAdult] = 25,
                [CreatureConstants.Dragon_Red_Old] = 28,
                [CreatureConstants.Dragon_Red_VeryOld] = 31,
                [CreatureConstants.Dragon_Red_Ancient] = 34,
                [CreatureConstants.Dragon_Red_Wyrm] = 37,
                [CreatureConstants.Dragon_Red_GreatWyrm] = 40,
                [CreatureConstants.Dragon_White_Wyrmling] = 3,
                [CreatureConstants.Dragon_White_VeryYoung] = 6,
                [CreatureConstants.Dragon_White_Young] = 9,
                [CreatureConstants.Dragon_White_Juvenile] = 12,
                [CreatureConstants.Dragon_White_YoungAdult] = 15,
                [CreatureConstants.Dragon_White_Adult] = 18,
                [CreatureConstants.Dragon_White_MatureAdult] = 21,
                [CreatureConstants.Dragon_White_Old] = 24,
                [CreatureConstants.Dragon_White_VeryOld] = 27,
                [CreatureConstants.Dragon_White_Ancient] = 30,
                [CreatureConstants.Dragon_White_Wyrm] = 33,
                [CreatureConstants.Dragon_White_GreatWyrm] = 36,
                [CreatureConstants.Dragon_Brass_Wyrmling] = 4,
                [CreatureConstants.Dragon_Brass_VeryYoung] = 7,
                [CreatureConstants.Dragon_Brass_Young] = 10,
                [CreatureConstants.Dragon_Brass_Juvenile] = 13,
                [CreatureConstants.Dragon_Brass_YoungAdult] = 16,
                [CreatureConstants.Dragon_Brass_Adult] = 19,
                [CreatureConstants.Dragon_Brass_MatureAdult] = 22,
                [CreatureConstants.Dragon_Brass_Old] = 25,
                [CreatureConstants.Dragon_Brass_VeryOld] = 28,
                [CreatureConstants.Dragon_Brass_Ancient] = 31,
                [CreatureConstants.Dragon_Brass_Wyrm] = 34,
                [CreatureConstants.Dragon_Brass_GreatWyrm] = 37,
                [CreatureConstants.Dragon_Bronze_Wyrmling] = 6,
                [CreatureConstants.Dragon_Bronze_VeryYoung] = 9,
                [CreatureConstants.Dragon_Bronze_Young] = 12,
                [CreatureConstants.Dragon_Bronze_Juvenile] = 15,
                [CreatureConstants.Dragon_Bronze_YoungAdult] = 18,
                [CreatureConstants.Dragon_Bronze_Adult] = 21,
                [CreatureConstants.Dragon_Bronze_MatureAdult] = 24,
                [CreatureConstants.Dragon_Bronze_Old] = 27,
                [CreatureConstants.Dragon_Bronze_VeryOld] = 30,
                [CreatureConstants.Dragon_Bronze_Ancient] = 33,
                [CreatureConstants.Dragon_Bronze_Wyrm] = 36,
                [CreatureConstants.Dragon_Bronze_GreatWyrm] = 39,
                [CreatureConstants.Dragon_Copper_Wyrmling] = 5,
                [CreatureConstants.Dragon_Copper_VeryYoung] = 8,
                [CreatureConstants.Dragon_Copper_Young] = 11,
                [CreatureConstants.Dragon_Copper_Juvenile] = 14,
                [CreatureConstants.Dragon_Copper_YoungAdult] = 17,
                [CreatureConstants.Dragon_Copper_Adult] = 20,
                [CreatureConstants.Dragon_Copper_MatureAdult] = 23,
                [CreatureConstants.Dragon_Copper_Old] = 26,
                [CreatureConstants.Dragon_Copper_VeryOld] = 29,
                [CreatureConstants.Dragon_Copper_Ancient] = 32,
                [CreatureConstants.Dragon_Copper_Wyrm] = 35,
                [CreatureConstants.Dragon_Copper_GreatWyrm] = 38,
                [CreatureConstants.Dragon_Gold_Wyrmling] = 8,
                [CreatureConstants.Dragon_Gold_VeryYoung] = 11,
                [CreatureConstants.Dragon_Gold_Young] = 14,
                [CreatureConstants.Dragon_Gold_Juvenile] = 17,
                [CreatureConstants.Dragon_Gold_YoungAdult] = 20,
                [CreatureConstants.Dragon_Gold_Adult] = 23,
                [CreatureConstants.Dragon_Gold_MatureAdult] = 26,
                [CreatureConstants.Dragon_Gold_Old] = 29,
                [CreatureConstants.Dragon_Gold_VeryOld] = 32,
                [CreatureConstants.Dragon_Gold_Ancient] = 35,
                [CreatureConstants.Dragon_Gold_Wyrm] = 38,
                [CreatureConstants.Dragon_Gold_GreatWyrm] = 41,
                [CreatureConstants.Dragon_Silver_Wyrmling] = 7,
                [CreatureConstants.Dragon_Silver_VeryYoung] = 10,
                [CreatureConstants.Dragon_Silver_Young] = 13,
                [CreatureConstants.Dragon_Silver_Juvenile] = 16,
                [CreatureConstants.Dragon_Silver_YoungAdult] = 19,
                [CreatureConstants.Dragon_Silver_Adult] = 22,
                [CreatureConstants.Dragon_Silver_MatureAdult] = 25,
                [CreatureConstants.Dragon_Silver_Old] = 28,
                [CreatureConstants.Dragon_Silver_VeryOld] = 31,
                [CreatureConstants.Dragon_Silver_Ancient] = 34,
                [CreatureConstants.Dragon_Silver_Wyrm] = 37,
                [CreatureConstants.Dragon_Silver_GreatWyrm] = 40,
                [CreatureConstants.DragonTurtle] = 12,
                [CreatureConstants.Dragonne] = 9,
                [CreatureConstants.Dretch] = 2,
                [CreatureConstants.Drider] = 6,
                [CreatureConstants.Dryad] = 4,
                [CreatureConstants.Dwarf_Deep] = 1,
                [CreatureConstants.Dwarf_Duergar] = 1,
                [CreatureConstants.Dwarf_Hill] = 1,
                [CreatureConstants.Dwarf_Mountain] = 1,
                [CreatureConstants.Eagle] = 1,
                [CreatureConstants.Eagle_Giant] = 4,
                [CreatureConstants.Efreeti] = 10,
                [CreatureConstants.Elasmosaurus] = 10,
                [CreatureConstants.Elemental_Air_Elder] = 24,
                [CreatureConstants.Elemental_Air_Greater] = 21,
                [CreatureConstants.Elemental_Air_Huge] = 16,
                [CreatureConstants.Elemental_Air_Large] = 8,
                [CreatureConstants.Elemental_Air_Medium] = 4,
                [CreatureConstants.Elemental_Air_Small] = 2,
                [CreatureConstants.Elemental_Earth_Elder] = 24,
                [CreatureConstants.Elemental_Earth_Greater] = 21,
                [CreatureConstants.Elemental_Earth_Huge] = 16,
                [CreatureConstants.Elemental_Earth_Large] = 8,
                [CreatureConstants.Elemental_Earth_Medium] = 4,
                [CreatureConstants.Elemental_Earth_Small] = 2,
                [CreatureConstants.Elemental_Fire_Elder] = 24,
                [CreatureConstants.Elemental_Fire_Greater] = 21,
                [CreatureConstants.Elemental_Fire_Huge] = 16,
                [CreatureConstants.Elemental_Fire_Large] = 8,
                [CreatureConstants.Elemental_Fire_Medium] = 4,
                [CreatureConstants.Elemental_Fire_Small] = 2,
                [CreatureConstants.Elemental_Water_Elder] = 24,
                [CreatureConstants.Elemental_Water_Greater] = 21,
                [CreatureConstants.Elemental_Water_Huge] = 16,
                [CreatureConstants.Elemental_Water_Large] = 8,
                [CreatureConstants.Elemental_Water_Medium] = 4,
                [CreatureConstants.Elemental_Water_Small] = 2,
                [CreatureConstants.Elephant] = 11,
                [CreatureConstants.Elf_Aquatic] = 1,
                [CreatureConstants.Elf_Drow] = 1,
                [CreatureConstants.Elf_Gray] = 1,
                [CreatureConstants.Elf_Half] = 1,
                [CreatureConstants.Elf_High] = 1,
                [CreatureConstants.Elf_Wild] = 1,
                [CreatureConstants.Elf_Wood] = 1,
                [CreatureConstants.Erinyes] = 9,
                [CreatureConstants.EtherealFilcher] = 5,
                [CreatureConstants.EtherealMarauder] = 2,
                [CreatureConstants.Ettercap] = 5,
                [CreatureConstants.Ettin] = 10,
                [CreatureConstants.FireBeetle_Giant] = 1,
                [CreatureConstants.FormianMyrmarch] = 12,
                [CreatureConstants.FormianQueen] = 20,
                [CreatureConstants.FormianTaskmaster] = 6,
                [CreatureConstants.FormianWarrior] = 4,
                [CreatureConstants.FormianWorker] = 1,
                [CreatureConstants.FrostWorm] = 14,
                [CreatureConstants.Gargoyle] = 4,
                [CreatureConstants.Gargoyle_Kapoacinth] = 4,
                [CreatureConstants.GelatinousCube] = 4,
                [CreatureConstants.Ghaele] = 10,
                [CreatureConstants.Ghoul] = 2,
                [CreatureConstants.Ghoul_Ghast] = 4,
                [CreatureConstants.Ghoul_Lacedon] = 2,
                [CreatureConstants.Giant_Cloud] = 17,
                [CreatureConstants.Giant_Fire] = 15,
                [CreatureConstants.Giant_Frost] = 14,
                [CreatureConstants.Giant_Hill] = 12,
                [CreatureConstants.Giant_Stone] = 14,
                [CreatureConstants.Giant_Stone_Elder] = 14,
                [CreatureConstants.Giant_Storm] = 19,
                [CreatureConstants.GibberingMouther] = 4,
                [CreatureConstants.Girallon] = 7,
                [CreatureConstants.Githyanki] = 1,
                [CreatureConstants.Githzerai] = 1,
                [CreatureConstants.Glabrezu] = 12,
                [CreatureConstants.Gnoll] = 2,
                [CreatureConstants.Gnome_Forest] = 1,
                [CreatureConstants.Gnome_Rock] = 1,
                [CreatureConstants.Gnome_Svirfneblin] = 1,
                [CreatureConstants.Goblin] = 1,
                [CreatureConstants.Golem_Clay] = 11,
                [CreatureConstants.Golem_Flesh] = 9,
                [CreatureConstants.Golem_Iron] = 18,
                [CreatureConstants.Golem_Stone] = 14,
                [CreatureConstants.Golem_Stone_Greater] = 42,
                [CreatureConstants.Gorgon] = 8,
                [CreatureConstants.GrayOoze] = 3,
                [CreatureConstants.GrayRender] = 10,
                [CreatureConstants.GreenHag] = 9,
                [CreatureConstants.Grick] = 2,
                [CreatureConstants.Griffon] = 7,
                [CreatureConstants.Grig] = .5,
                [CreatureConstants.Grig_WithFiddle] = .5,
                [CreatureConstants.Grimlock] = 2,
                [CreatureConstants.Gynosphinx] = 8,
                [CreatureConstants.Halfling_Deep] = 1,
                [CreatureConstants.Halfling_Lightfoot] = 1,
                [CreatureConstants.Halfling_Tallfellow] = 1,
                [CreatureConstants.Harpy] = 7,
                [CreatureConstants.Hawk] = 1,
                [CreatureConstants.Hellcat_Bezekira] = 8,
                [CreatureConstants.HellHound] = 4,
                [CreatureConstants.HellHound_NessianWarhound] = 12,
                [CreatureConstants.Hellwasp_Swarm] = 12,
                [CreatureConstants.Hezrou] = 10,
                [CreatureConstants.Hieracosphinx] = 9,
                [CreatureConstants.Hippogriff] = 3,
                [CreatureConstants.Hobgoblin] = 1,
                [CreatureConstants.Homunculus] = 2,
                [CreatureConstants.HornedDevil_Cornugon] = 15,
                [CreatureConstants.Horse_Heavy] = 3,
                [CreatureConstants.Horse_Heavy_War] = 4,
                [CreatureConstants.Horse_Light] = 3,
                [CreatureConstants.Horse_Light_War] = 3,
                [CreatureConstants.HoundArchon] = 6,
                [CreatureConstants.Howler] = 6,
                [CreatureConstants.Human] = 1,
                [CreatureConstants.Hydra_10Heads] = 10,
                [CreatureConstants.Hydra_11Heads] = 11,
                [CreatureConstants.Hydra_12Heads] = 12,
                [CreatureConstants.Hydra_5Heads] = 5,
                [CreatureConstants.Hydra_6Heads] = 6,
                [CreatureConstants.Hydra_7Heads] = 7,
                [CreatureConstants.Hydra_8Heads] = 8,
                [CreatureConstants.Hydra_9Heads] = 9,
                [CreatureConstants.Hyena] = 2,
                [CreatureConstants.IceDevil_Gelugon] = 14,
                [CreatureConstants.Imp] = 3,
                [CreatureConstants.InvisibleStalker] = 8,
                [CreatureConstants.Janni] = 6,
                [CreatureConstants.Kobold] = 1,
                [CreatureConstants.Kolyarut] = 13,
                [CreatureConstants.Kraken] = 20,
                [CreatureConstants.Krenshar] = 2,
                [CreatureConstants.KuoToa] = 2,
                [CreatureConstants.Lamia] = 9,
                [CreatureConstants.Lammasu] = 7,
                [CreatureConstants.LanternArchon] = 1,
                [CreatureConstants.Lemure] = 2,
                [CreatureConstants.Leonal] = 12,
                [CreatureConstants.Leopard] = 3,
                [CreatureConstants.Lillend] = 7,
                [CreatureConstants.Lion] = 5,
                [CreatureConstants.Lion_Dire] = 8,
                [CreatureConstants.Lizard] = .5,
                [CreatureConstants.Lizard_Monitor] = 3,
                [CreatureConstants.Lizardfolk] = 2,
                [CreatureConstants.Locathah] = 2,
                [CreatureConstants.Locust_Swarm] = 6,
                [CreatureConstants.Magmin] = 2,
                [CreatureConstants.MantaRay] = 4,
                [CreatureConstants.Manticore] = 6,
                [CreatureConstants.Marilith] = 16,
                [CreatureConstants.Marut] = 15,
                [CreatureConstants.Medusa] = 6,
                [CreatureConstants.Megaraptor] = 8,
                [CreatureConstants.Mephit_Air] = 3,
                [CreatureConstants.Mephit_Dust] = 3,
                [CreatureConstants.Mephit_Earth] = 3,
                [CreatureConstants.Mephit_Fire] = 3,
                [CreatureConstants.Mephit_Ice] = 3,
                [CreatureConstants.Mephit_Magma] = 3,
                [CreatureConstants.Mephit_Ooze] = 3,
                [CreatureConstants.Mephit_Salt] = 3,
                [CreatureConstants.Mephit_Steam] = 3,
                [CreatureConstants.Mephit_Water] = 3,
                [CreatureConstants.Merfolk] = 1,
                [CreatureConstants.Mimic] = 7,
                [CreatureConstants.MindFlayer] = 8,
                [CreatureConstants.Minotaur] = 6,
                [CreatureConstants.Mohrg] = 14,
                [CreatureConstants.Monkey] = 1,
                [CreatureConstants.Mule] = 3,
                [CreatureConstants.Mummy] = 8,
                [CreatureConstants.Naga_Dark] = 9,
                [CreatureConstants.Naga_Guardian] = 11,
                [CreatureConstants.Naga_Spirit] = 9,
                [CreatureConstants.Naga_Water] = 7,
                [CreatureConstants.Nalfeshnee] = 14,
                [CreatureConstants.NightHag] = 8,
                [CreatureConstants.Nightcrawler] = 25,
                [CreatureConstants.Nightmare] = 6,
                [CreatureConstants.Nightmare_Cauchemar] = 15,
                [CreatureConstants.Nightwalker] = 21,
                [CreatureConstants.Nightwing] = 17,
                [CreatureConstants.Nixie] = 1,
                [CreatureConstants.Nymph] = 6,
                [CreatureConstants.OchreJelly] = 6,
                [CreatureConstants.Octopus] = 2,
                [CreatureConstants.Octopus_Giant] = 8,
                [CreatureConstants.Ogre] = 4,
                [CreatureConstants.Ogre_Merrow] = 4,
                [CreatureConstants.OgreMage] = 5,
                [CreatureConstants.Orc] = 1,
                [CreatureConstants.Orc_Half] = 1,
                [CreatureConstants.Otyugh] = 6,
                [CreatureConstants.Owl] = 1,
                [CreatureConstants.Owl_Giant] = 4,
                [CreatureConstants.Owlbear] = 5,
                [CreatureConstants.Pegasus] = 4,
                [CreatureConstants.PhantomFungus] = 2,
                [CreatureConstants.PhaseSpider] = 5,
                [CreatureConstants.Phasm] = 15,
                [CreatureConstants.PitFiend] = 18,
                [CreatureConstants.Pixie] = 1,
                [CreatureConstants.Pixie_WithIrresistibleDance] = 1,
                [CreatureConstants.Pony] = 2,
                [CreatureConstants.Pony_War] = 2,
                [CreatureConstants.Porpoise] = 2,
                [CreatureConstants.PrayingMantis_Giant] = 4,
                [CreatureConstants.Pseudodragon] = 2,
                [CreatureConstants.PurpleWorm] = 16,
                [CreatureConstants.Pyrohydra_10Heads] = 10,
                [CreatureConstants.Pyrohydra_11Heads] = 11,
                [CreatureConstants.Pyrohydra_12Heads] = 12,
                [CreatureConstants.Pyrohydra_5Heads] = 5,
                [CreatureConstants.Pyrohydra_6Heads] = 6,
                [CreatureConstants.Pyrohydra_7Heads] = 7,
                [CreatureConstants.Pyrohydra_8Heads] = 8,
                [CreatureConstants.Pyrohydra_9Heads] = 9,
                [CreatureConstants.Quasit] = 3,
                [CreatureConstants.Rakshasa] = 7,
                [CreatureConstants.Rast] = 4,
                [CreatureConstants.Rat] = .25,
                [CreatureConstants.Rat_Dire] = 1,
                [CreatureConstants.Rat_Swarm] = 4,
                [CreatureConstants.Raven] = .25,
                [CreatureConstants.Ravid] = 3,
                [CreatureConstants.RazorBoar] = 15,
                [CreatureConstants.Remorhaz] = 7,
                [CreatureConstants.Retriever] = 10,
                [CreatureConstants.Rhinoceras] = 8,
                [CreatureConstants.Roc] = 18,
                [CreatureConstants.Roper] = 10,
                [CreatureConstants.RustMonster] = 5,
                [CreatureConstants.Sahuagin] = 2,
                [CreatureConstants.Sahuagin_Malenti] = 2,
                [CreatureConstants.Sahuagin_Mutant] = 2,
                [CreatureConstants.Salamander_Average] = 9,
                [CreatureConstants.Salamander_Flamebrother] = 4,
                [CreatureConstants.Salamander_Noble] = 15,
                [CreatureConstants.Satyr] = 5,
                [CreatureConstants.Satyr_WithPipes] = 5,
                [CreatureConstants.Scorpion_Monstrous_Colossal] = 40,
                [CreatureConstants.Scorpion_Monstrous_Gargantuan] = 20,
                [CreatureConstants.Scorpion_Monstrous_Huge] = 10,
                [CreatureConstants.Scorpion_Monstrous_Large] = 5,
                [CreatureConstants.Scorpion_Monstrous_Medium] = 2,
                [CreatureConstants.Scorpion_Monstrous_Small] = 1,
                [CreatureConstants.Scorpion_Monstrous_Tiny] = .5,
                [CreatureConstants.Scorpionfolk] = 12,
                [CreatureConstants.SeaCat] = 6,
                [CreatureConstants.SeaHag] = 3,
                [CreatureConstants.Shadow] = 3,
                [CreatureConstants.Shadow_Greater] = 9,
                [CreatureConstants.ShadowMastiff] = 4,
                [CreatureConstants.ShamblingMound] = 8,
                [CreatureConstants.Shark_Dire] = 18,
                [CreatureConstants.Shark_Huge] = 10,
                [CreatureConstants.Shark_Large] = 7,
                [CreatureConstants.Shark_Medium] = 3,
                [CreatureConstants.ShieldGuardian] = 15,
                [CreatureConstants.ShockerLizard] = 2,
                [CreatureConstants.Shrieker] = 2,
                [CreatureConstants.Skum] = 2,
                [CreatureConstants.Slaad_Blue] = 8,
                [CreatureConstants.Slaad_Death] = 15,
                [CreatureConstants.Slaad_Gray] = 10,
                [CreatureConstants.Slaad_Green] = 9,
                [CreatureConstants.Slaad_Red] = 7,
                [CreatureConstants.Snake_Constrictor] = 3,
                [CreatureConstants.Snake_Constrictor_Giant] = 11,
                [CreatureConstants.Snake_Viper_Huge] = 6,
                [CreatureConstants.Snake_Viper_Large] = 3,
                [CreatureConstants.Snake_Viper_Medium] = 2,
                [CreatureConstants.Snake_Viper_Small] = 1,
                [CreatureConstants.Snake_Viper_Tiny] = .25,
                [CreatureConstants.Spectre] = 7,
                [CreatureConstants.Spider_Monstrous_Hunter_Colossal] = 32,
                [CreatureConstants.Spider_Monstrous_Hunter_Gargantuan] = 16,
                [CreatureConstants.Spider_Monstrous_Hunter_Huge] = 8,
                [CreatureConstants.Spider_Monstrous_Hunter_Large] = 4,
                [CreatureConstants.Spider_Monstrous_Hunter_Medium] = 2,
                [CreatureConstants.Spider_Monstrous_Hunter_Small] = 1,
                [CreatureConstants.Spider_Monstrous_Hunter_Tiny] = .5,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Colossal] = 32,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan] = 16,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Huge] = 8,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Large] = 4,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Medium] = 2,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Small] = 1,
                [CreatureConstants.Spider_Monstrous_WebSpinner_Tiny] = .5,
                [CreatureConstants.Spider_Swarm] = 2,
                [CreatureConstants.SpiderEater] = 4,
                [CreatureConstants.Squid] = 3,
                [CreatureConstants.Squid_Giant] = 12,
                [CreatureConstants.StagBeetle_Giant] = 7,
                [CreatureConstants.Stirge] = 1,
                [CreatureConstants.Succubus] = 6,
                [CreatureConstants.Tarrasque] = 48,
                [CreatureConstants.Tendriculos] = 9,
                [CreatureConstants.Thoqqua] = 3,
                [CreatureConstants.Tiefling] = 1,
                [CreatureConstants.Tiger] = 6,
                [CreatureConstants.Tiger_Dire] = 16,
                [CreatureConstants.Titan] = 20,
                [CreatureConstants.Toad] = .25,
                [CreatureConstants.Tojanida_Adult] = 7,
                [CreatureConstants.Tojanida_Elder] = 15,
                [CreatureConstants.Tojanida_Juvenile] = 3,
                [CreatureConstants.Treant] = 7,
                [CreatureConstants.Triceratops] = 16,
                [CreatureConstants.Triton] = 3,
                [CreatureConstants.Troglodyte] = 2,
                [CreatureConstants.Troll] = 6,
                [CreatureConstants.Troll_Scrag] = 6,
                [CreatureConstants.TrumpetArchon] = 12,
                [CreatureConstants.Tyrannosaurus] = 18,
                [CreatureConstants.UmberHulk] = 8,
                [CreatureConstants.UmberHulk_TrulyHorrid] = 20,
                [CreatureConstants.Unicorn] = 4,
                [CreatureConstants.VampireSpawn] = 4,
                [CreatureConstants.Vargouille] = 1,
                [CreatureConstants.VioletFungus] = 2,
                [CreatureConstants.Vrock] = 10,
                [CreatureConstants.Wasp_Giant] = 5,
                [CreatureConstants.Weasel] = .5,
                [CreatureConstants.Weasel_Dire] = 3,
                [CreatureConstants.Whale_Baleen] = 12,
                [CreatureConstants.Whale_Cachalot] = 12,
                [CreatureConstants.Whale_Orca] = 9,
                [CreatureConstants.Wight] = 4,
                [CreatureConstants.WillOWisp] = 9,
                [CreatureConstants.WinterWolf] = 6,
                [CreatureConstants.Wolf] = 2,
                [CreatureConstants.Wolf_Dire] = 6,
                [CreatureConstants.Wolverine] = 3,
                [CreatureConstants.Wolverine_Dire] = 5,
                [CreatureConstants.Worg] = 4,
                [CreatureConstants.Wraith] = 5,
                [CreatureConstants.Wraith_Dread] = 16,
                [CreatureConstants.Wyvern] = 7,
                [CreatureConstants.Xill] = 5,
                [CreatureConstants.Xorn_Average] = 7,
                [CreatureConstants.Xorn_Elder] = 15,
                [CreatureConstants.Xorn_Minor] = 3,
                [CreatureConstants.YethHound] = 3,
                [CreatureConstants.Yrthak] = 12,
                [CreatureConstants.YuanTi_Abomination] = 9,
                [CreatureConstants.YuanTi_Halfblood_SnakeArms] = 7,
                [CreatureConstants.YuanTi_Halfblood_SnakeHead] = 7,
                [CreatureConstants.YuanTi_Halfblood_SnakeTail] = 7,
                [CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs] = 7,
                [CreatureConstants.YuanTi_Pureblood] = 4,
                [CreatureConstants.Zelekhut] = 8,
            };

            return quantities;
        }

        [Test]
        public void CreatureDataNames()
        {
            var names = CreatureConstants.GetAll();
            AssertCollectionNames(names);
        }

        [Test]
        public void AllTypesHaveBaseAttackQuality()
        {
            var types = CreatureConstants.Types.GetAll();
            Assert.That(baseAttackQualities.Keys, Is.EquivalentTo(types));
        }

        [Test]
        public void AllCreaturesHaveTypes()
        {
            var creatures = CreatureConstants.GetAll();
            Assert.That(creatureTypes.Keys, Is.EquivalentTo(creatures));
            Assert.That(creatureTypes.Values, Is.All.Not.Empty);

            var types = CreatureConstants.Types.GetAll();
            Assert.That(creatureTypes.Values.Select(t => t.First()).Distinct(), Is.EquivalentTo(types));

            //INFO: Augmented only applies to Templates, so no creature will have that subtype
            var subtypes = CreatureConstants.Types.Subtypes.GetAll().Except([CreatureConstants.Types.Subtypes.Augmented]);
            Assert.That(creatureTypes.Values.SelectMany(t => t.Skip(1)).Distinct(), Is.EquivalentTo(subtypes));
        }

        [Test]
        public void AllCreaturesHaveHitDiceQuantities()
        {
            var creatures = CreatureConstants.GetAll();
            Assert.That(creatureHitDiceQuantities.Keys, Is.EquivalentTo(creatures));
            Assert.That(creatureHitDiceQuantities.Values, Is.All.Positive);
        }

        [Test]
        public void AllTypesHaveHitDie()
        {
            var types = CreatureConstants.Types.GetAll();
            Assert.That(hitDies.Keys, Is.EquivalentTo(types));

            var validDie = new[] { 2, 3, 4, 6, 8, 10, 12, 20, 100 };
            var dies = hitDies.Values.Distinct();
            Assert.That(dies, Is.SubsetOf(validDie));
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureData(string creature)
        {
            var selection = DataHelper.Parse<CreatureDataSelection>(creatureData[creature]);

            //Valid Challenge Ratings
            var challengeRatings = ChallengeRatingConstants.GetOrdered();

            var cr = selection.GetEffectiveChallengeRating(false);
            Assert.That(cr, Is.Not.Empty.And.Not.EqualTo(ChallengeRatingConstants.CR0), creature);
            Assert.That(new[] { cr }, Is.SubsetOf(challengeRatings), creature);

            //Valid Sizes
            var sizes = SizeConstants.GetOrdered();

            Assert.That(selection.Size, Is.Not.Empty, creature);
            Assert.That(sizes, Contains.Item(selection.Size), creature);

            //Valid Reach
            Assert.That(selection.Reach, Is.Not.Negative, creature);

            //Valid Space
            Assert.That(selection.Space, Is.Positive, creature);

            //Valid Level Adjustment
            var characters = CreatureConstants.GetAllCharacters();

            if (selection.LevelAdjustment != null)
            {
                Assert.That(selection.LevelAdjustment, Is.Not.Negative, creature);
                Assert.That(characters, Contains.Item(creature));
            }
            else
            {
                Assert.That(selection.LevelAdjustment, Is.Null, creature);
                Assert.That(characters, Does.Not.Contains(creature), creature);
            }

            //Valid Can Use Equipment
            Assert.That(selection.CanUseEquipment, Is.True.Or.False, creature);

            //Valid Caster Level
            Assert.That(selection.CasterLevel, Is.Not.Negative, creature);

            //Valid Natural Armor
            Assert.That(selection.NaturalArmor, Is.Not.Negative, creature);

            //Valid Number Of Hands
            Assert.That(selection.NumberOfHands, Is.Not.Negative, creature);

            AssertCollection(creature, creatureData[creature]);
        }

        private Dictionary<string, string> GetCreatureTestData()
        {
            var data = new Dictionary<string, string>
            {
                [CreatureConstants.Aasimar] = GetCreatureData(CreatureConstants.Aasimar, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 1, true, 1, 0, 2, true),
                [CreatureConstants.Aboleth] = GetCreatureData(CreatureConstants.Aboleth, SizeConstants.Huge, ChallengeRatingConstants.CR7, 0, false, 16, 7, 0, true),
                [CreatureConstants.Achaierai] = GetCreatureData(CreatureConstants.Achaierai, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 16, 10, 0, false),
                [CreatureConstants.Allip] = GetCreatureData(CreatureConstants.Allip, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 0, 0, false),
                [CreatureConstants.Androsphinx] = GetCreatureData(CreatureConstants.Androsphinx, SizeConstants.Large, ChallengeRatingConstants.CR9, 5, false, 6, 13, 0, true),
                [CreatureConstants.Angel_AstralDeva] =
                    GetCreatureData(CreatureConstants.Angel_AstralDeva, SizeConstants.Medium, ChallengeRatingConstants.CR14, 8, true, 12, 15, 2, false),
                [CreatureConstants.Angel_Planetar] =
                    GetCreatureData(CreatureConstants.Angel_Planetar, SizeConstants.Large, ChallengeRatingConstants.CR16, null, true, 17, 19, 2, false),
                [CreatureConstants.Angel_Solar] = GetCreatureData(CreatureConstants.Angel_Solar, SizeConstants.Large, ChallengeRatingConstants.CR23, null, true, 20, 21, 2, false),
                [CreatureConstants.AnimatedObject_Colossal] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_Flexible] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_Flexible, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden,
                        SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden,
                        SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_Sheetlike] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_Sheetlike, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_TwoLegs] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_TwoLegs, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Colossal_Wooden] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Colossal_Wooden, SizeConstants.Colossal, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_Flexible] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_Flexible, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall] =
                    GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_Sheetlike] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_Sheetlike, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_TwoLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Gargantuan_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Gargantuan_Wooden, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 8, 0, false),
                [CreatureConstants.AnimatedObject_Huge] = GetCreatureData(CreatureConstants.AnimatedObject_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_Flexible] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_Flexible, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_Sheetlike] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_Sheetlike, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_TwoLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_TwoLegs, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_Wheels_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_Wheels_Wooden, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Huge_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Huge_Wooden, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.AnimatedObject_Large] = GetCreatureData(CreatureConstants.AnimatedObject_Large, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_Flexible] = GetCreatureData(CreatureConstants.AnimatedObject_Large_Flexible, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Long] = GetCreatureData(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall] = GetCreatureData(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_Sheetlike] = GetCreatureData(CreatureConstants.AnimatedObject_Large_Sheetlike, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_TwoLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Large_TwoLegs, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_Wheels_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Large_Wheels_Wooden, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Large_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Large_Wooden, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, false),
                [CreatureConstants.AnimatedObject_Medium] = GetCreatureData(CreatureConstants.AnimatedObject_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_Flexible] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_Flexible, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_MultipleLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_MultipleLegs, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_Sheetlike] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_Sheetlike, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_TwoLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_TwoLegs, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_Wheels_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_Wheels_Wooden, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Medium_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Medium_Wooden, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, false),
                [CreatureConstants.AnimatedObject_Small] = GetCreatureData(CreatureConstants.AnimatedObject_Small, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_Flexible] = GetCreatureData(CreatureConstants.AnimatedObject_Small_Flexible, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_MultipleLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Small_MultipleLegs, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_Sheetlike] = GetCreatureData(CreatureConstants.AnimatedObject_Small_Sheetlike, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_TwoLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Small_TwoLegs, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_Wheels_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Small_Wheels_Wooden, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Small_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Small_Wooden, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.AnimatedObject_Tiny] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_Flexible] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_Flexible, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_MultipleLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_MultipleLegs, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_Sheetlike] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_Sheetlike, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_TwoLegs] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_TwoLegs, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.AnimatedObject_Tiny_Wooden] = GetCreatureData(CreatureConstants.AnimatedObject_Tiny_Wooden, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.Ankheg] = GetCreatureData(CreatureConstants.Ankheg, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 9, 0, true),
                [CreatureConstants.Annis] = GetCreatureData(CreatureConstants.Annis, SizeConstants.Large, ChallengeRatingConstants.CR6, 0, true, 8, 10, 2, true),
                [CreatureConstants.Ant_Giant_Queen] = GetCreatureData(CreatureConstants.Ant_Giant_Queen, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 9, 0, false),
                [CreatureConstants.Ant_Giant_Soldier] = GetCreatureData(CreatureConstants.Ant_Giant_Soldier, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 7, 0, false),
                [CreatureConstants.Ant_Giant_Worker] = GetCreatureData(CreatureConstants.Ant_Giant_Worker, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 7, 0, false),
                [CreatureConstants.Ape] = GetCreatureData(CreatureConstants.Ape, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 3, 0, true),
                [CreatureConstants.Ape_Dire] = GetCreatureData(CreatureConstants.Ape_Dire, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 4, 0, true),
                [CreatureConstants.Aranea] = GetCreatureData(CreatureConstants.Aranea, SizeConstants.Medium, ChallengeRatingConstants.CR4, 4, true, 3, 1, 0, true),
                [CreatureConstants.Arrowhawk_Adult] = GetCreatureData(CreatureConstants.Arrowhawk_Adult, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, false),
                [CreatureConstants.Arrowhawk_Elder] = GetCreatureData(CreatureConstants.Arrowhawk_Elder, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 8, 0, false),
                [CreatureConstants.Arrowhawk_Juvenile] = GetCreatureData(CreatureConstants.Arrowhawk_Juvenile, SizeConstants.Small, ChallengeRatingConstants.CR3, null, false, 0, 4, 0, false),
                [CreatureConstants.AssassinVine] = GetCreatureData(CreatureConstants.AssassinVine, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 4, 6, 0, false),
                [CreatureConstants.Athach] = GetCreatureData(CreatureConstants.Athach, SizeConstants.Huge, ChallengeRatingConstants.CR8, 5, true, 0, 8, 3, true),
                [CreatureConstants.Avoral] = GetCreatureData(CreatureConstants.Avoral, SizeConstants.Medium, ChallengeRatingConstants.CR9, null, true, 8, 8, 2, false),
                [CreatureConstants.Azer] = GetCreatureData(CreatureConstants.Azer, SizeConstants.Medium, ChallengeRatingConstants.CR2, 4, true, 0, 6, 2, false),
                [CreatureConstants.Babau] = GetCreatureData(CreatureConstants.Babau, SizeConstants.Medium, ChallengeRatingConstants.CR6, null, true, 7, 8, 0, false),
                [CreatureConstants.Baboon] = GetCreatureData(CreatureConstants.Baboon, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, null, false, 0, 1, 0, true),
                [CreatureConstants.Badger] = GetCreatureData(CreatureConstants.Badger, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, false, 0, 1, 0, true),
                [CreatureConstants.Badger_Dire] = GetCreatureData(CreatureConstants.Badger_Dire, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 3, 0, true),
                [CreatureConstants.Balor] = GetCreatureData(CreatureConstants.Balor, SizeConstants.Large, ChallengeRatingConstants.CR20, null, true, 20, 19, 2, false),
                [CreatureConstants.BarbedDevil_Hamatula] = GetCreatureData(CreatureConstants.BarbedDevil_Hamatula, SizeConstants.Medium, ChallengeRatingConstants.CR11, null, true, 12, 13, 2, false),
                [CreatureConstants.Barghest] = GetCreatureData(CreatureConstants.Barghest, SizeConstants.Medium, ChallengeRatingConstants.CR4, null, true, 0, 6, 2, false),
                [CreatureConstants.Barghest_Greater] = GetCreatureData(CreatureConstants.Barghest_Greater, SizeConstants.Large, ChallengeRatingConstants.CR5, null, true, 0, 9, 2, false),
                [CreatureConstants.Basilisk] = GetCreatureData(CreatureConstants.Basilisk, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 0, 7, 0, true),
                [CreatureConstants.Basilisk_Greater] = GetCreatureData(CreatureConstants.Basilisk_Greater, SizeConstants.Large, ChallengeRatingConstants.CR12, null, false, 0, 9, 0, true),
                [CreatureConstants.Bat] = GetCreatureData(CreatureConstants.Bat, SizeConstants.Diminutive, ChallengeRatingConstants.CR1_10th, null, false, 0, 0, 0, true),
                [CreatureConstants.Bat_Dire] = GetCreatureData(CreatureConstants.Bat_Dire, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 5, 0, true),
                [CreatureConstants.Bat_Swarm] = GetCreatureData(CreatureConstants.Bat_Swarm, SizeConstants.Diminutive, ChallengeRatingConstants.CR2, null, false, 0, 0, 0, true, space: 10),
                [CreatureConstants.Bear_Black] = GetCreatureData(CreatureConstants.Bear_Black, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 2, 0, true),
                [CreatureConstants.Bear_Brown] = GetCreatureData(CreatureConstants.Bear_Brown, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 5, 0, true),
                [CreatureConstants.Bear_Dire] = GetCreatureData(CreatureConstants.Bear_Dire, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 0, 7, 0, true),
                [CreatureConstants.Bear_Polar] = GetCreatureData(CreatureConstants.Bear_Polar, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 5, 0, true),
                [CreatureConstants.BeardedDevil_Barbazu] = GetCreatureData(CreatureConstants.BeardedDevil_Barbazu, SizeConstants.Medium, ChallengeRatingConstants.CR5, 6, true, 12, 7, 2, false),
                [CreatureConstants.Bebilith] = GetCreatureData(CreatureConstants.Bebilith, SizeConstants.Huge, ChallengeRatingConstants.CR10, null, false, 12, 13, 0, false),
                [CreatureConstants.Bee_Giant] = GetCreatureData(CreatureConstants.Bee_Giant, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.Behir] = GetCreatureData(CreatureConstants.Behir, SizeConstants.Huge, ChallengeRatingConstants.CR8, null, false, 0, 11, 0, true),
                [CreatureConstants.Beholder] = GetCreatureData(CreatureConstants.Beholder, SizeConstants.Large, ChallengeRatingConstants.CR13, null, false, 13, 15, 0, true),
                [CreatureConstants.Beholder_Gauth] = GetCreatureData(CreatureConstants.Beholder_Gauth, SizeConstants.Medium, ChallengeRatingConstants.CR6, null, false, 8, 7, 0, true),
                [CreatureConstants.Belker] = GetCreatureData(CreatureConstants.Belker, SizeConstants.Large, ChallengeRatingConstants.CR6, null, false, 0, 8, 0, false),
                [CreatureConstants.Bison] = GetCreatureData(CreatureConstants.Bison, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, true),
                [CreatureConstants.BlackPudding] = GetCreatureData(CreatureConstants.BlackPudding, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 0, 0, false),
                [CreatureConstants.BlackPudding_Elder] = GetCreatureData(CreatureConstants.BlackPudding_Elder, SizeConstants.Gargantuan, ChallengeRatingConstants.CR12, null, false, 0, 0, 0, false),
                [CreatureConstants.BlinkDog] = GetCreatureData(CreatureConstants.BlinkDog, SizeConstants.Medium, ChallengeRatingConstants.CR2, 2, false, 8, 3, 0, true),
                [CreatureConstants.Boar] = GetCreatureData(CreatureConstants.Boar, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 6, 0, true),
                [CreatureConstants.Boar_Dire] = GetCreatureData(CreatureConstants.Boar_Dire, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 6, 0, true),
                [CreatureConstants.Bodak] = GetCreatureData(CreatureConstants.Bodak, SizeConstants.Medium, ChallengeRatingConstants.CR8, null, false, 0, 8, 2, false),
                [CreatureConstants.BombardierBeetle_Giant] = GetCreatureData(CreatureConstants.BombardierBeetle_Giant, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 6, 0, false),
                [CreatureConstants.BoneDevil_Osyluth] = GetCreatureData(CreatureConstants.BoneDevil_Osyluth, SizeConstants.Large, ChallengeRatingConstants.CR9, null, true, 12, 11, 2, false),
                [CreatureConstants.Bralani] = GetCreatureData(CreatureConstants.Bralani, SizeConstants.Medium, ChallengeRatingConstants.CR6, 5, true, 6, 6, 2, false),
                [CreatureConstants.Bugbear] = GetCreatureData(CreatureConstants.Bugbear, SizeConstants.Medium, ChallengeRatingConstants.CR2, 1, true, 0, 3, 2, true),
                [CreatureConstants.Bulette] = GetCreatureData(CreatureConstants.Bulette, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 12, 0, true),
                [CreatureConstants.Camel_Bactrian] = GetCreatureData(CreatureConstants.Camel_Bactrian, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 1, 0, true),
                [CreatureConstants.Camel_Dromedary] = GetCreatureData(CreatureConstants.Camel_Dromedary, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 1, 0, true),
                [CreatureConstants.CarrionCrawler] = GetCreatureData(CreatureConstants.CarrionCrawler, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 6, 0, true),
                [CreatureConstants.Cat] = GetCreatureData(CreatureConstants.Cat, SizeConstants.Tiny, ChallengeRatingConstants.CR1_4th, null, false, 0, 0, 0, true),
                [CreatureConstants.Centaur] = GetCreatureData(CreatureConstants.Centaur, SizeConstants.Large, ChallengeRatingConstants.CR3, 2, true, 0, 3, 2, true),
                [CreatureConstants.Centipede_Monstrous_Colossal] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Colossal, SizeConstants.Colossal, ChallengeRatingConstants.CR9, null, false, 0, 16, 0, false),
                [CreatureConstants.Centipede_Monstrous_Gargantuan] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Gargantuan, SizeConstants.Gargantuan, ChallengeRatingConstants.CR6, null, false, 0, 10, 0, false),
                [CreatureConstants.Centipede_Monstrous_Huge] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR2, null, false, 0, 6, 0, false),
                [CreatureConstants.Centipede_Monstrous_Large] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Large, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, false),
                [CreatureConstants.Centipede_Monstrous_Medium] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, null, false, 0, 2, 0, false),
                [CreatureConstants.Centipede_Monstrous_Small] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Small, SizeConstants.Small, ChallengeRatingConstants.CR1_4th, null, false, 0, 1, 0, false),
                [CreatureConstants.Centipede_Monstrous_Tiny] = GetCreatureData(CreatureConstants.Centipede_Monstrous_Tiny, SizeConstants.Tiny, ChallengeRatingConstants.CR1_8th, null, false, 0, 0, 0, false),
                [CreatureConstants.Centipede_Swarm] = GetCreatureData(CreatureConstants.Centipede_Swarm, SizeConstants.Diminutive, ChallengeRatingConstants.CR4, null, false, 0, 0, 0, false, space: 10),
                [CreatureConstants.ChainDevil_Kyton] = GetCreatureData(CreatureConstants.ChainDevil_Kyton, SizeConstants.Medium, ChallengeRatingConstants.CR6, 6, true, 0, 8, 2, false),
                [CreatureConstants.ChaosBeast] = GetCreatureData(CreatureConstants.ChaosBeast, SizeConstants.Medium, ChallengeRatingConstants.CR7, null, false, 0, 5, 0, false),
                [CreatureConstants.Cheetah] = GetCreatureData(CreatureConstants.Cheetah, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 1, 0, true),
                [CreatureConstants.Chimera_Black] = GetCreatureData(CreatureConstants.Chimera_Black, SizeConstants.Large, ChallengeRatingConstants.CR7, 2, false, 0, 9, 0, true),
                [CreatureConstants.Chimera_Blue] = GetCreatureData(CreatureConstants.Chimera_Blue, SizeConstants.Large, ChallengeRatingConstants.CR7, 2, false, 0, 9, 0, true),
                [CreatureConstants.Chimera_Green] = GetCreatureData(CreatureConstants.Chimera_Green, SizeConstants.Large, ChallengeRatingConstants.CR7, 2, false, 0, 9, 0, true),
                [CreatureConstants.Chimera_Red] = GetCreatureData(CreatureConstants.Chimera_Red, SizeConstants.Large, ChallengeRatingConstants.CR7, 2, false, 0, 9, 0, true),
                [CreatureConstants.Chimera_White] = GetCreatureData(CreatureConstants.Chimera_White, SizeConstants.Large, ChallengeRatingConstants.CR7, 2, false, 0, 9, 0, true),
                [CreatureConstants.Choker] = GetCreatureData(CreatureConstants.Choker, SizeConstants.Small, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, true, reach: 10),
                [CreatureConstants.Chuul] = GetCreatureData(CreatureConstants.Chuul, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 0, 10, 0, true),
                [CreatureConstants.Cloaker] = GetCreatureData(CreatureConstants.Cloaker, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 7, 0, true),
                [CreatureConstants.Cockatrice] = GetCreatureData(CreatureConstants.Cockatrice, SizeConstants.Small, ChallengeRatingConstants.CR3, null, false, 0, 0, 0, true),
                [CreatureConstants.Couatl] = GetCreatureData(CreatureConstants.Couatl, SizeConstants.Large, ChallengeRatingConstants.CR10, 7, true, 9, 9, 0, true),
                [CreatureConstants.Criosphinx] = GetCreatureData(CreatureConstants.Criosphinx, SizeConstants.Large, ChallengeRatingConstants.CR7, 3, false, 0, 11, 0, true),
                [CreatureConstants.Crocodile] = GetCreatureData(CreatureConstants.Crocodile, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, true),
                [CreatureConstants.Crocodile_Giant] = GetCreatureData(CreatureConstants.Crocodile_Giant, SizeConstants.Huge, ChallengeRatingConstants.CR4, null, false, 0, 7, 0, true),
                [CreatureConstants.Cryohydra_5Heads] = GetCreatureData(CreatureConstants.Cryohydra_5Heads, SizeConstants.Huge, ChallengeRatingConstants.CR6, null, false, 0, 6, 0, true),
                [CreatureConstants.Cryohydra_6Heads] = GetCreatureData(CreatureConstants.Cryohydra_6Heads, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 7, 0, true),
                [CreatureConstants.Cryohydra_7Heads] = GetCreatureData(CreatureConstants.Cryohydra_7Heads, SizeConstants.Huge, ChallengeRatingConstants.CR8, null, false, 0, 8, 0, true),
                [CreatureConstants.Cryohydra_8Heads] = GetCreatureData(CreatureConstants.Cryohydra_8Heads, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 9, 0, true),
                [CreatureConstants.Cryohydra_9Heads] = GetCreatureData(CreatureConstants.Cryohydra_9Heads, SizeConstants.Huge, ChallengeRatingConstants.CR10, null, false, 0, 10, 0, true),
                [CreatureConstants.Cryohydra_10Heads] = GetCreatureData(CreatureConstants.Cryohydra_10Heads, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 11, 0, true),
                [CreatureConstants.Cryohydra_11Heads] = GetCreatureData(CreatureConstants.Cryohydra_11Heads, SizeConstants.Huge, ChallengeRatingConstants.CR12, null, false, 0, 12, 0, true),
                [CreatureConstants.Cryohydra_12Heads] = GetCreatureData(CreatureConstants.Cryohydra_12Heads, SizeConstants.Huge, ChallengeRatingConstants.CR13, null, false, 0, 13, 0, true),
                [CreatureConstants.Darkmantle] = GetCreatureData(CreatureConstants.Darkmantle, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 5, 6, 0, true),
                [CreatureConstants.Deinonychus] = GetCreatureData(CreatureConstants.Deinonychus, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, true),
                [CreatureConstants.Delver] = GetCreatureData(CreatureConstants.Delver, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 15, 15, 0, true),
                [CreatureConstants.Derro] = GetCreatureData(CreatureConstants.Derro, SizeConstants.Small, ChallengeRatingConstants.CR3, 0, true, 3, 2, 2, true),
                [CreatureConstants.Derro_Sane] = GetCreatureData(CreatureConstants.Derro_Sane, SizeConstants.Small, ChallengeRatingConstants.CR3, 2, true, 3, 2, 2, true),
                [CreatureConstants.Destrachan] = GetCreatureData(CreatureConstants.Destrachan, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 8, 0, true),
                [CreatureConstants.Devourer] = GetCreatureData(CreatureConstants.Devourer, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 18, 15, 2, false),
                [CreatureConstants.Digester] = GetCreatureData(CreatureConstants.Digester, SizeConstants.Medium, ChallengeRatingConstants.CR6, null, false, 0, 5, 0, true),
                [CreatureConstants.DisplacerBeast] = GetCreatureData(CreatureConstants.DisplacerBeast, SizeConstants.Large, ChallengeRatingConstants.CR4, 4, false, 0, 5, 0, true),
                [CreatureConstants.DisplacerBeast_PackLord] = GetCreatureData(CreatureConstants.DisplacerBeast_PackLord, SizeConstants.Huge, ChallengeRatingConstants.CR12, null, false, 0, 8, 0, true),
                [CreatureConstants.Djinni] = GetCreatureData(CreatureConstants.Djinni, SizeConstants.Large, ChallengeRatingConstants.CR5, 6, true, 20, 3, 2, false),
                [CreatureConstants.Djinni_Noble] = GetCreatureData(CreatureConstants.Djinni_Noble, SizeConstants.Large, ChallengeRatingConstants.CR8, null, true, 20, 3, 2, false),
                [CreatureConstants.Dog] = GetCreatureData(CreatureConstants.Dog, SizeConstants.Small, ChallengeRatingConstants.CR1_3rd, null, false, 0, 1, 0, true),
                [CreatureConstants.Dog_Riding] = GetCreatureData(CreatureConstants.Dog_Riding, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 4, 0, true),
                [CreatureConstants.Donkey] = GetCreatureData(CreatureConstants.Donkey, SizeConstants.Medium, ChallengeRatingConstants.CR1_6th, null, false, 0, 2, 0, true),
                [CreatureConstants.Doppelganger] = GetCreatureData(CreatureConstants.Doppelganger, SizeConstants.Medium, ChallengeRatingConstants.CR3, 4, true, 18, 4, 2, true),
                [CreatureConstants.Dragon_Black_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Black_Wyrmling, SizeConstants.Tiny, ChallengeRatingConstants.CR3, 3, false, 0, 3, 0, true),
                [CreatureConstants.Dragon_Black_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Black_VeryYoung, SizeConstants.Small, ChallengeRatingConstants.CR4, 3, false, 0, 6, 0, true),
                [CreatureConstants.Dragon_Black_Young] = GetCreatureData(CreatureConstants.Dragon_Black_Young, SizeConstants.Medium, ChallengeRatingConstants.CR5, 3, false, 0, 9, 0, true),
                [CreatureConstants.Dragon_Black_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Black_Juvenile, SizeConstants.Medium, ChallengeRatingConstants.CR7, 4, false, 0, 12, 0, true),
                [CreatureConstants.Dragon_Black_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Black_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR9, null, false, 1, 15, 0, true),
                [CreatureConstants.Dragon_Black_Adult] = GetCreatureData(CreatureConstants.Dragon_Black_Adult, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 3, 18, 0, true),
                [CreatureConstants.Dragon_Black_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Black_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR14, null, false, 5, 21, 0, true),
                [CreatureConstants.Dragon_Black_Old] = GetCreatureData(CreatureConstants.Dragon_Black_Old, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, false, 7, 24, 0, true),
                [CreatureConstants.Dragon_Black_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Black_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR18, null, false, 9, 27, 0, true),
                [CreatureConstants.Dragon_Black_Ancient] = GetCreatureData(CreatureConstants.Dragon_Black_Ancient, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, false, 11, 30, 0, true),
                [CreatureConstants.Dragon_Black_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Black_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR20, null, false, 13, 33, 0, true),
                [CreatureConstants.Dragon_Black_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Black_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR22, null, false, 15, 36, 0, true),
                [CreatureConstants.Dragon_Blue_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Blue_Wyrmling, SizeConstants.Small, ChallengeRatingConstants.CR3, 4, false, 0, 5, 0, true),
                [CreatureConstants.Dragon_Blue_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Blue_VeryYoung, SizeConstants.Medium, ChallengeRatingConstants.CR4, 4, false, 0, 8, 0, true),
                [CreatureConstants.Dragon_Blue_Young] = GetCreatureData(CreatureConstants.Dragon_Blue_Young, SizeConstants.Medium, ChallengeRatingConstants.CR6, 5, false, 0, 11, 0, true),
                [CreatureConstants.Dragon_Blue_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Blue_Juvenile, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 1, 14, 0, true),
                [CreatureConstants.Dragon_Blue_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Blue_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 3, 17, 0, true),
                [CreatureConstants.Dragon_Blue_Adult] = GetCreatureData(CreatureConstants.Dragon_Blue_Adult, SizeConstants.Huge, ChallengeRatingConstants.CR14, null, false, 5, 20, 0, true),
                [CreatureConstants.Dragon_Blue_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Blue_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, false, 7, 23, 0, true),
                [CreatureConstants.Dragon_Blue_Old] = GetCreatureData(CreatureConstants.Dragon_Blue_Old, SizeConstants.Huge, ChallengeRatingConstants.CR18, null, false, 9, 26, 0, true),
                [CreatureConstants.Dragon_Blue_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Blue_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, false, 11, 29, 0, true),
                [CreatureConstants.Dragon_Blue_Ancient] = GetCreatureData(CreatureConstants.Dragon_Blue_Ancient, SizeConstants.Gargantuan, ChallengeRatingConstants.CR21, null, false, 13, 32, 0, true),
                [CreatureConstants.Dragon_Blue_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Blue_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR23, null, false, 15, 35, 0, true),
                [CreatureConstants.Dragon_Blue_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Blue_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR25, null, false, 17, 38, 0, true),
                [CreatureConstants.Dragon_Brass_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Brass_Wyrmling, SizeConstants.Tiny, ChallengeRatingConstants.CR3, 2, false, 0, 3, 0, true),
                [CreatureConstants.Dragon_Brass_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Brass_VeryYoung, SizeConstants.Small, ChallengeRatingConstants.CR4, 3, false, 0, 6, 0, true),
                [CreatureConstants.Dragon_Brass_Young] = GetCreatureData(CreatureConstants.Dragon_Brass_Young, SizeConstants.Medium, ChallengeRatingConstants.CR6, 4, false, 1, 9, 0, true),
                [CreatureConstants.Dragon_Brass_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Brass_Juvenile, SizeConstants.Medium, ChallengeRatingConstants.CR8, 4, false, 3, 12, 0, true),
                [CreatureConstants.Dragon_Brass_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Brass_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR10, null, false, 5, 15, 0, true),
                [CreatureConstants.Dragon_Brass_Adult] = GetCreatureData(CreatureConstants.Dragon_Brass_Adult, SizeConstants.Large, ChallengeRatingConstants.CR12, null, false, 7, 18, 0, true),
                [CreatureConstants.Dragon_Brass_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Brass_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR15, null, false, 9, 21, 0, true),
                [CreatureConstants.Dragon_Brass_Old] = GetCreatureData(CreatureConstants.Dragon_Brass_Old, SizeConstants.Huge, ChallengeRatingConstants.CR17, null, false, 11, 24, 0, true),
                [CreatureConstants.Dragon_Brass_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Brass_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, false, 13, 27, 0, true),
                [CreatureConstants.Dragon_Brass_Ancient] = GetCreatureData(CreatureConstants.Dragon_Brass_Ancient, SizeConstants.Huge, ChallengeRatingConstants.CR20, null, false, 15, 30, 0, true),
                [CreatureConstants.Dragon_Brass_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Brass_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR21, null, false, 17, 33, 0, true),
                [CreatureConstants.Dragon_Brass_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Brass_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR23, null, false, 19, 36, 0, true),
                [CreatureConstants.Dragon_Bronze_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Bronze_Wyrmling, SizeConstants.Small, ChallengeRatingConstants.CR3, 4, false, 0, 5, 0, true),
                [CreatureConstants.Dragon_Bronze_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Bronze_VeryYoung, SizeConstants.Medium, ChallengeRatingConstants.CR5, 4, false, 0, 8, 0, true),
                [CreatureConstants.Dragon_Bronze_Young] = GetCreatureData(CreatureConstants.Dragon_Bronze_Young, SizeConstants.Medium, ChallengeRatingConstants.CR7, 6, true, 1, 11, 0, true),
                [CreatureConstants.Dragon_Bronze_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Bronze_Juvenile, SizeConstants.Large, ChallengeRatingConstants.CR9, null, true, 3, 14, 0, true),
                [CreatureConstants.Dragon_Bronze_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Bronze_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR12, null, true, 5, 17, 0, true),
                [CreatureConstants.Dragon_Bronze_Adult] = GetCreatureData(CreatureConstants.Dragon_Bronze_Adult, SizeConstants.Huge, ChallengeRatingConstants.CR15, null, true, 7, 20, 0, true),
                [CreatureConstants.Dragon_Bronze_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Bronze_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR17, null, true, 9, 23, 0, true),
                [CreatureConstants.Dragon_Bronze_Old] = GetCreatureData(CreatureConstants.Dragon_Bronze_Old, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, true, 11, 26, 0, true),
                [CreatureConstants.Dragon_Bronze_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Bronze_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR20, null, true, 13, 29, 0, true),
                [CreatureConstants.Dragon_Bronze_Ancient] = GetCreatureData(CreatureConstants.Dragon_Bronze_Ancient, SizeConstants.Gargantuan, ChallengeRatingConstants.CR22, null, true, 15, 32, 0, true),
                [CreatureConstants.Dragon_Bronze_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Bronze_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR23, null, true, 17, 35, 0, true),
                [CreatureConstants.Dragon_Bronze_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Bronze_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR25, null, true, 19, 38, 0, true),
                [CreatureConstants.Dragon_Copper_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Copper_Wyrmling, SizeConstants.Tiny, ChallengeRatingConstants.CR3, 2, false, 0, 4, 0, true),
                [CreatureConstants.Dragon_Copper_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Copper_VeryYoung, SizeConstants.Small, ChallengeRatingConstants.CR5, 3, false, 0, 7, 0, true),
                [CreatureConstants.Dragon_Copper_Young] = GetCreatureData(CreatureConstants.Dragon_Copper_Young, SizeConstants.Medium, ChallengeRatingConstants.CR7, 4, false, 1, 10, 0, true),
                [CreatureConstants.Dragon_Copper_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Copper_Juvenile, SizeConstants.Medium, ChallengeRatingConstants.CR9, 4, false, 3, 13, 0, true),
                [CreatureConstants.Dragon_Copper_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Copper_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 5, 16, 0, true),
                [CreatureConstants.Dragon_Copper_Adult] = GetCreatureData(CreatureConstants.Dragon_Copper_Adult, SizeConstants.Large, ChallengeRatingConstants.CR14, null, false, 7, 19, 0, true),
                [CreatureConstants.Dragon_Copper_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Copper_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, false, 9, 22, 0, true),
                [CreatureConstants.Dragon_Copper_Old] = GetCreatureData(CreatureConstants.Dragon_Copper_Old, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, false, 11, 25, 0, true),
                [CreatureConstants.Dragon_Copper_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Copper_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR20, null, false, 13, 28, 0, true),
                [CreatureConstants.Dragon_Copper_Ancient] = GetCreatureData(CreatureConstants.Dragon_Copper_Ancient, SizeConstants.Huge, ChallengeRatingConstants.CR22, null, false, 15, 31, 0, true),
                [CreatureConstants.Dragon_Copper_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Copper_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR23, null, false, 17, 34, 0, true),
                [CreatureConstants.Dragon_Copper_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Copper_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR25, null, false, 19, 37, 0, true),
                [CreatureConstants.Dragon_Gold_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Gold_Wyrmling, SizeConstants.Medium, ChallengeRatingConstants.CR5, 4, true, 0, 7, 0, true),
                [CreatureConstants.Dragon_Gold_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Gold_VeryYoung, SizeConstants.Large, ChallengeRatingConstants.CR7, 5, true, 0, 10, 0, true),
                [CreatureConstants.Dragon_Gold_Young] = GetCreatureData(CreatureConstants.Dragon_Gold_Young, SizeConstants.Large, ChallengeRatingConstants.CR9, 6, true, 1, 13, 0, true),
                [CreatureConstants.Dragon_Gold_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Gold_Juvenile, SizeConstants.Large, ChallengeRatingConstants.CR11, null, true, 3, 16, 0, true),
                [CreatureConstants.Dragon_Gold_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Gold_YoungAdult, SizeConstants.Huge, ChallengeRatingConstants.CR14, null, true, 5, 19, 0, true),
                [CreatureConstants.Dragon_Gold_Adult] = GetCreatureData(CreatureConstants.Dragon_Gold_Adult, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, true, 7, 22, 0, true),
                [CreatureConstants.Dragon_Gold_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Gold_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, true, 9, 25, 0, true),
                [CreatureConstants.Dragon_Gold_Old] = GetCreatureData(CreatureConstants.Dragon_Gold_Old, SizeConstants.Gargantuan, ChallengeRatingConstants.CR21, null, true, 11, 28, 0, true),
                [CreatureConstants.Dragon_Gold_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Gold_VeryOld, SizeConstants.Gargantuan, ChallengeRatingConstants.CR22, null, true, 13, 31, 0, true),
                [CreatureConstants.Dragon_Gold_Ancient] = GetCreatureData(CreatureConstants.Dragon_Gold_Ancient, SizeConstants.Gargantuan, ChallengeRatingConstants.CR24, null, true, 15, 34, 0, true),
                [CreatureConstants.Dragon_Gold_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Gold_Wyrm, SizeConstants.Colossal, ChallengeRatingConstants.CR25, null, true, 17, 37, 0, true),
                [CreatureConstants.Dragon_Gold_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Gold_GreatWyrm, SizeConstants.Colossal, ChallengeRatingConstants.CR27, null, true, 19, 40, 0, true),
                [CreatureConstants.Dragon_Green_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Green_Wyrmling, SizeConstants.Small, ChallengeRatingConstants.CR3, 5, false, 0, 4, 0, true),
                [CreatureConstants.Dragon_Green_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Green_VeryYoung, SizeConstants.Medium, ChallengeRatingConstants.CR4, 5, false, 0, 7, 0, true),
                [CreatureConstants.Dragon_Green_Young] = GetCreatureData(CreatureConstants.Dragon_Green_Young, SizeConstants.Medium, ChallengeRatingConstants.CR5, 5, false, 0, 10, 0, true),
                [CreatureConstants.Dragon_Green_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Green_Juvenile, SizeConstants.Large, ChallengeRatingConstants.CR8, 6, false, 1, 13, 0, true),
                [CreatureConstants.Dragon_Green_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Green_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 3, 16, 0, true),
                [CreatureConstants.Dragon_Green_Adult] = GetCreatureData(CreatureConstants.Dragon_Green_Adult, SizeConstants.Huge, ChallengeRatingConstants.CR13, null, false, 5, 19, 0, true),
                [CreatureConstants.Dragon_Green_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Green_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, false, 7, 22, 0, true),
                [CreatureConstants.Dragon_Green_Old] = GetCreatureData(CreatureConstants.Dragon_Green_Old, SizeConstants.Huge, ChallengeRatingConstants.CR18, null, false, 9, 25, 0, true),
                [CreatureConstants.Dragon_Green_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Green_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR19, null, false, 11, 28, 0, true),
                [CreatureConstants.Dragon_Green_Ancient] = GetCreatureData(CreatureConstants.Dragon_Green_Ancient, SizeConstants.Gargantuan, ChallengeRatingConstants.CR21, null, false, 13, 31, 0, true),
                [CreatureConstants.Dragon_Green_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Green_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR22, null, false, 15, 34, 0, true),
                [CreatureConstants.Dragon_Green_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Green_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR24, null, false, 17, 37, 0, true),
                [CreatureConstants.Dragon_Red_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Red_Wyrmling, SizeConstants.Medium, ChallengeRatingConstants.CR4, 4, false, 0, 6, 0, true),
                [CreatureConstants.Dragon_Red_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Red_VeryYoung, SizeConstants.Large, ChallengeRatingConstants.CR5, 5, false, 0, 9, 0, true),
                [CreatureConstants.Dragon_Red_Young] = GetCreatureData(CreatureConstants.Dragon_Red_Young, SizeConstants.Large, ChallengeRatingConstants.CR7, 6, false, 1, 12, 0, true),
                [CreatureConstants.Dragon_Red_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Red_Juvenile, SizeConstants.Large, ChallengeRatingConstants.CR10, null, false, 3, 15, 0, true),
                [CreatureConstants.Dragon_Red_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Red_YoungAdult, SizeConstants.Huge, ChallengeRatingConstants.CR13, null, false, 5, 18, 0, true),
                [CreatureConstants.Dragon_Red_Adult] = GetCreatureData(CreatureConstants.Dragon_Red_Adult, SizeConstants.Huge, ChallengeRatingConstants.CR15, null, false, 7, 21, 0, true),
                [CreatureConstants.Dragon_Red_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Red_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR18, null, false, 9, 24, 0, true),
                [CreatureConstants.Dragon_Red_Old] = GetCreatureData(CreatureConstants.Dragon_Red_Old, SizeConstants.Gargantuan, ChallengeRatingConstants.CR20, null, false, 11, 27, 0, true),
                [CreatureConstants.Dragon_Red_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Red_VeryOld, SizeConstants.Gargantuan, ChallengeRatingConstants.CR21, null, false, 13, 30, 0, true),
                [CreatureConstants.Dragon_Red_Ancient] = GetCreatureData(CreatureConstants.Dragon_Red_Ancient, SizeConstants.Gargantuan, ChallengeRatingConstants.CR23, null, false, 15, 33, 0, true),
                [CreatureConstants.Dragon_Red_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Red_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR24, null, false, 17, 36, 0, true),
                [CreatureConstants.Dragon_Red_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Red_GreatWyrm, SizeConstants.Colossal, ChallengeRatingConstants.CR26, null, false, 19, 39, 0, true),
                [CreatureConstants.Dragon_Silver_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_Silver_Wyrmling, SizeConstants.Small, ChallengeRatingConstants.CR4, 4, true, 0, 6, 0, true),
                [CreatureConstants.Dragon_Silver_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_Silver_VeryYoung, SizeConstants.Medium, ChallengeRatingConstants.CR5, 4, true, 0, 9, 0, true),
                [CreatureConstants.Dragon_Silver_Young] = GetCreatureData(CreatureConstants.Dragon_Silver_Young, SizeConstants.Medium, ChallengeRatingConstants.CR7, 5, true, 1, 12, 0, true),
                [CreatureConstants.Dragon_Silver_Juvenile] = GetCreatureData(CreatureConstants.Dragon_Silver_Juvenile, SizeConstants.Large, ChallengeRatingConstants.CR10, null, true, 3, 15, 0, true),
                [CreatureConstants.Dragon_Silver_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_Silver_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR13, null, true, 5, 18, 0, true),
                [CreatureConstants.Dragon_Silver_Adult] = GetCreatureData(CreatureConstants.Dragon_Silver_Adult, SizeConstants.Huge, ChallengeRatingConstants.CR15, null, true, 7, 21, 0, true),
                [CreatureConstants.Dragon_Silver_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_Silver_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR18, null, true, 9, 24, 0, true),
                [CreatureConstants.Dragon_Silver_Old] = GetCreatureData(CreatureConstants.Dragon_Silver_Old, SizeConstants.Huge, ChallengeRatingConstants.CR20, null, true, 11, 27, 0, true),
                [CreatureConstants.Dragon_Silver_VeryOld] = GetCreatureData(CreatureConstants.Dragon_Silver_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR21, null, true, 13, 30, 0, true),
                [CreatureConstants.Dragon_Silver_Ancient] = GetCreatureData(CreatureConstants.Dragon_Silver_Ancient, SizeConstants.Gargantuan, ChallengeRatingConstants.CR23, null, true, 15, 33, 0, true),
                [CreatureConstants.Dragon_Silver_Wyrm] = GetCreatureData(CreatureConstants.Dragon_Silver_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR24, null, true, 17, 36, 0, true),
                [CreatureConstants.Dragon_Silver_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_Silver_GreatWyrm, SizeConstants.Colossal, ChallengeRatingConstants.CR26, null, true, 19, 39, 0, true),
                [CreatureConstants.Dragon_White_Wyrmling] = GetCreatureData(CreatureConstants.Dragon_White_Wyrmling, SizeConstants.Tiny, ChallengeRatingConstants.CR2, 2, false, 0, 2, 0, true),
                [CreatureConstants.Dragon_White_VeryYoung] = GetCreatureData(CreatureConstants.Dragon_White_VeryYoung, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, false, 0, 5, 0, true),
                [CreatureConstants.Dragon_White_Young] = GetCreatureData(CreatureConstants.Dragon_White_Young, SizeConstants.Medium, ChallengeRatingConstants.CR4, 3, false, 0, 8, 0, true),
                [CreatureConstants.Dragon_White_Juvenile] = GetCreatureData(CreatureConstants.Dragon_White_Juvenile, SizeConstants.Medium, ChallengeRatingConstants.CR6, 5, false, 0, 11, 0, true),
                [CreatureConstants.Dragon_White_YoungAdult] = GetCreatureData(CreatureConstants.Dragon_White_YoungAdult, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 14, 0, true),
                [CreatureConstants.Dragon_White_Adult] = GetCreatureData(CreatureConstants.Dragon_White_Adult, SizeConstants.Large, ChallengeRatingConstants.CR10, null, false, 1, 17, 0, true),
                [CreatureConstants.Dragon_White_MatureAdult] = GetCreatureData(CreatureConstants.Dragon_White_MatureAdult, SizeConstants.Huge, ChallengeRatingConstants.CR12, null, false, 3, 20, 0, true),
                [CreatureConstants.Dragon_White_Old] = GetCreatureData(CreatureConstants.Dragon_White_Old, SizeConstants.Huge, ChallengeRatingConstants.CR15, null, false, 5, 23, 0, true),
                [CreatureConstants.Dragon_White_VeryOld] = GetCreatureData(CreatureConstants.Dragon_White_VeryOld, SizeConstants.Huge, ChallengeRatingConstants.CR17, null, false, 7, 26, 0, true),
                [CreatureConstants.Dragon_White_Ancient] = GetCreatureData(CreatureConstants.Dragon_White_Ancient, SizeConstants.Huge, ChallengeRatingConstants.CR18, null, false, 9, 29, 0, true),
                [CreatureConstants.Dragon_White_Wyrm] = GetCreatureData(CreatureConstants.Dragon_White_Wyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR19, null, false, 11, 32, 0, true),
                [CreatureConstants.Dragon_White_GreatWyrm] = GetCreatureData(CreatureConstants.Dragon_White_GreatWyrm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR21, null, false, 13, 35, 0, true),
                [CreatureConstants.DragonTurtle] = GetCreatureData(CreatureConstants.DragonTurtle, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 17, 0, true),
                [CreatureConstants.Dragonne] = GetCreatureData(CreatureConstants.Dragonne, SizeConstants.Large, ChallengeRatingConstants.CR7, 4, false, 0, 7, 0, true),
                [CreatureConstants.Dretch] = GetCreatureData(CreatureConstants.Dretch, SizeConstants.Small, ChallengeRatingConstants.CR2, 2, true, 2, 5, 2, false),
                [CreatureConstants.Drider] = GetCreatureData(CreatureConstants.Drider, SizeConstants.Large, ChallengeRatingConstants.CR7, 4, true, 6, 6, 2, true),
                [CreatureConstants.Dryad] = GetCreatureData(CreatureConstants.Dryad, SizeConstants.Medium, ChallengeRatingConstants.CR3, 0, true, 6, 3, 2, true),
                [CreatureConstants.Dwarf_Deep] = GetCreatureData(CreatureConstants.Dwarf_Deep, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Dwarf_Duergar] = GetCreatureData(CreatureConstants.Dwarf_Duergar, SizeConstants.Medium, ChallengeRatingConstants.CR1, 1, true, 0, 0, 2, true),
                [CreatureConstants.Dwarf_Hill] = GetCreatureData(CreatureConstants.Dwarf_Hill, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Dwarf_Mountain] = GetCreatureData(CreatureConstants.Dwarf_Mountain, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Eagle] = GetCreatureData(CreatureConstants.Eagle, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, false, 0, 1, 0, true),
                [CreatureConstants.Eagle_Giant] = GetCreatureData(CreatureConstants.Eagle_Giant, SizeConstants.Large, ChallengeRatingConstants.CR3, 2, false, 0, 3, 0, true),
                [CreatureConstants.Efreeti] = GetCreatureData(CreatureConstants.Efreeti, SizeConstants.Large, ChallengeRatingConstants.CR8, null, true, 12, 6, 2, false),
                [CreatureConstants.Elasmosaurus] = GetCreatureData(CreatureConstants.Elasmosaurus, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 3, 0, true),
                [CreatureConstants.Elemental_Air_Elder] = GetCreatureData(CreatureConstants.Elemental_Air_Elder, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 8, 0, false),
                [CreatureConstants.Elemental_Air_Greater] = GetCreatureData(CreatureConstants.Elemental_Air_Greater, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 8, 0, false),
                [CreatureConstants.Elemental_Air_Huge] = GetCreatureData(CreatureConstants.Elemental_Air_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 4, 0, false),
                [CreatureConstants.Elemental_Air_Large] = GetCreatureData(CreatureConstants.Elemental_Air_Large, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 4, 0, false),
                [CreatureConstants.Elemental_Air_Medium] = GetCreatureData(CreatureConstants.Elemental_Air_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 3, 0, false),
                [CreatureConstants.Elemental_Air_Small] = GetCreatureData(CreatureConstants.Elemental_Air_Small, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, false),
                [CreatureConstants.Elemental_Earth_Elder] = GetCreatureData(CreatureConstants.Elemental_Earth_Elder, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 15, 0, false),
                [CreatureConstants.Elemental_Earth_Greater] = GetCreatureData(CreatureConstants.Elemental_Earth_Greater, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 13, 0, false),
                [CreatureConstants.Elemental_Earth_Huge] = GetCreatureData(CreatureConstants.Elemental_Earth_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 11, 0, false),
                [CreatureConstants.Elemental_Earth_Large] = GetCreatureData(CreatureConstants.Elemental_Earth_Large, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 10, 0, false),
                [CreatureConstants.Elemental_Earth_Medium] = GetCreatureData(CreatureConstants.Elemental_Earth_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 9, 0, false),
                [CreatureConstants.Elemental_Earth_Small] = GetCreatureData(CreatureConstants.Elemental_Earth_Small, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 7, 0, false),
                [CreatureConstants.Elemental_Fire_Elder] = GetCreatureData(CreatureConstants.Elemental_Fire_Elder, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 8, 0, false),
                [CreatureConstants.Elemental_Fire_Greater] = GetCreatureData(CreatureConstants.Elemental_Fire_Greater, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 8, 0, false),
                [CreatureConstants.Elemental_Fire_Huge] = GetCreatureData(CreatureConstants.Elemental_Fire_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 4, 0, false),
                [CreatureConstants.Elemental_Fire_Large] = GetCreatureData(CreatureConstants.Elemental_Fire_Large, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 4, 0, false),
                [CreatureConstants.Elemental_Fire_Medium] = GetCreatureData(CreatureConstants.Elemental_Fire_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 3, 0, false),
                [CreatureConstants.Elemental_Fire_Small] = GetCreatureData(CreatureConstants.Elemental_Fire_Small, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, false),
                [CreatureConstants.Elemental_Water_Elder] = GetCreatureData(CreatureConstants.Elemental_Water_Elder, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 9, 0, false),
                [CreatureConstants.Elemental_Water_Greater] = GetCreatureData(CreatureConstants.Elemental_Water_Greater, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 9, 0, false),
                [CreatureConstants.Elemental_Water_Huge] = GetCreatureData(CreatureConstants.Elemental_Water_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 9, 0, false),
                [CreatureConstants.Elemental_Water_Large] = GetCreatureData(CreatureConstants.Elemental_Water_Large, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 9, 0, false),
                [CreatureConstants.Elemental_Water_Medium] = GetCreatureData(CreatureConstants.Elemental_Water_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 8, 0, false),
                [CreatureConstants.Elemental_Water_Small] = GetCreatureData(CreatureConstants.Elemental_Water_Small, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 6, 0, false),
                [CreatureConstants.Elephant] = GetCreatureData(CreatureConstants.Elephant, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 7, 0, true),
                [CreatureConstants.Elf_Aquatic] = GetCreatureData(CreatureConstants.Elf_Aquatic, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Elf_Drow] = GetCreatureData(CreatureConstants.Elf_Drow, SizeConstants.Medium, ChallengeRatingConstants.CR1, 2, true, 1, 0, 2, true),
                [CreatureConstants.Elf_Gray] = GetCreatureData(CreatureConstants.Elf_Gray, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Elf_Half] = GetCreatureData(CreatureConstants.Elf_Half, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Elf_High] = GetCreatureData(CreatureConstants.Elf_High, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Elf_Wild] = GetCreatureData(CreatureConstants.Elf_Wild, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Elf_Wood] = GetCreatureData(CreatureConstants.Elf_Wood, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Erinyes] = GetCreatureData(CreatureConstants.Erinyes, SizeConstants.Medium, ChallengeRatingConstants.CR8, 7, true, 12, 8, 2, false),
                [CreatureConstants.EtherealFilcher] = GetCreatureData(CreatureConstants.EtherealFilcher, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 5, 3, 4, true),
                [CreatureConstants.EtherealMarauder] = GetCreatureData(CreatureConstants.EtherealMarauder, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 15, 3, 0, true),
                [CreatureConstants.Ettercap] = GetCreatureData(CreatureConstants.Ettercap, SizeConstants.Medium, ChallengeRatingConstants.CR3, 4, false, 0, 1, 0, true),
                [CreatureConstants.Ettin] = GetCreatureData(CreatureConstants.Ettin, SizeConstants.Large, ChallengeRatingConstants.CR6, 5, true, 0, 7, 2, true),
                [CreatureConstants.FireBeetle_Giant] = GetCreatureData(CreatureConstants.FireBeetle_Giant, SizeConstants.Small, ChallengeRatingConstants.CR1_3rd, null, false, 0, 5, 0, false),
                [CreatureConstants.FormianMyrmarch] = GetCreatureData(CreatureConstants.FormianMyrmarch, SizeConstants.Large, ChallengeRatingConstants.CR10, null, true, 12, 15, 2, false),
                [CreatureConstants.FormianQueen] = GetCreatureData(CreatureConstants.FormianQueen, SizeConstants.Large, ChallengeRatingConstants.CR17, null, true, 17, 14, 2, false),
                [CreatureConstants.FormianTaskmaster] = GetCreatureData(CreatureConstants.FormianTaskmaster, SizeConstants.Medium, ChallengeRatingConstants.CR7, null, true, 10, 6, 2, false),
                [CreatureConstants.FormianWarrior] = GetCreatureData(CreatureConstants.FormianWarrior, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, true, 0, 5, 2, false),
                [CreatureConstants.FormianWorker] = GetCreatureData(CreatureConstants.FormianWorker, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, true, 0, 4, 2, false),
                [CreatureConstants.FrostWorm] = GetCreatureData(CreatureConstants.FrostWorm, SizeConstants.Huge, ChallengeRatingConstants.CR12, null, false, 0, 10, 0, false),
                [CreatureConstants.Gargoyle] = GetCreatureData(CreatureConstants.Gargoyle, SizeConstants.Medium, ChallengeRatingConstants.CR4, 5, true, 0, 4, 2, true),
                [CreatureConstants.Gargoyle_Kapoacinth] = GetCreatureData(CreatureConstants.Gargoyle_Kapoacinth, SizeConstants.Medium, ChallengeRatingConstants.CR4, 5, true, 0, 4, 2, true),
                [CreatureConstants.GelatinousCube] = GetCreatureData(CreatureConstants.GelatinousCube, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 0, 0, false),
                [CreatureConstants.Ghaele] = GetCreatureData(CreatureConstants.Ghaele, SizeConstants.Medium, ChallengeRatingConstants.CR13, null, true, 12, 14, 2, false),
                [CreatureConstants.Ghoul] = GetCreatureData(CreatureConstants.Ghoul, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 2, 2, false),
                [CreatureConstants.Ghoul_Ghast] = GetCreatureData(CreatureConstants.Ghoul_Ghast, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 4, 2, false),
                [CreatureConstants.Ghoul_Lacedon] = GetCreatureData(CreatureConstants.Ghoul_Lacedon, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 2, 2, false),
                [CreatureConstants.Giant_Cloud] = GetCreatureData(CreatureConstants.Giant_Cloud, SizeConstants.Huge, ChallengeRatingConstants.CR11, 0, true, 15, 12, 2, true),
                [CreatureConstants.Giant_Fire] = GetCreatureData(CreatureConstants.Giant_Fire, SizeConstants.Large, ChallengeRatingConstants.CR10, 4, true, 0, 8, 2, true),
                [CreatureConstants.Giant_Frost] = GetCreatureData(CreatureConstants.Giant_Frost, SizeConstants.Large, ChallengeRatingConstants.CR9, 4, true, 0, 9, 2, true),
                [CreatureConstants.Giant_Hill] = GetCreatureData(CreatureConstants.Giant_Hill, SizeConstants.Large, ChallengeRatingConstants.CR7, 4, true, 0, 9, 2, true),
                [CreatureConstants.Giant_Stone] = GetCreatureData(CreatureConstants.Giant_Stone, SizeConstants.Large, ChallengeRatingConstants.CR8, 4, true, 0, 11, 2, true),
                [CreatureConstants.Giant_Stone_Elder] = GetCreatureData(CreatureConstants.Giant_Stone_Elder, SizeConstants.Large, ChallengeRatingConstants.CR9, 6, true, 10, 11, 2, true),
                [CreatureConstants.Giant_Storm] = GetCreatureData(CreatureConstants.Giant_Storm, SizeConstants.Huge, ChallengeRatingConstants.CR13, 0, true, 20, 12, 2, true),
                [CreatureConstants.GibberingMouther] = GetCreatureData(CreatureConstants.GibberingMouther, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 0, 8, 0, false),
                [CreatureConstants.Girallon] = GetCreatureData(CreatureConstants.Girallon, SizeConstants.Large, ChallengeRatingConstants.CR6, null, false, 0, 4, 4, true),
                [CreatureConstants.Githyanki] = GetCreatureData(CreatureConstants.Githyanki, SizeConstants.Medium, ChallengeRatingConstants.CR1, 2, true, 1, 0, 2, true),
                [CreatureConstants.Githzerai] = GetCreatureData(CreatureConstants.Githzerai, SizeConstants.Medium, ChallengeRatingConstants.CR1, 2, true, 1, 0, 2, true),
                [CreatureConstants.Glabrezu] = GetCreatureData(CreatureConstants.Glabrezu, SizeConstants.Huge, ChallengeRatingConstants.CR13, null, true, 14, 19, 2, false),
                [CreatureConstants.Gnoll] = GetCreatureData(CreatureConstants.Gnoll, SizeConstants.Medium, ChallengeRatingConstants.CR1, 1, true, 0, 1, 2, true),
                [CreatureConstants.Gnome_Forest] = GetCreatureData(CreatureConstants.Gnome_Forest, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Gnome_Rock] = GetCreatureData(CreatureConstants.Gnome_Rock, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Gnome_Svirfneblin] = GetCreatureData(CreatureConstants.Gnome_Svirfneblin, SizeConstants.Small, ChallengeRatingConstants.CR1, 3, true, 1, 0, 2, true),
                [CreatureConstants.Goblin] = GetCreatureData(CreatureConstants.Goblin, SizeConstants.Small, ChallengeRatingConstants.CR1_3rd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Golem_Clay] = GetCreatureData(CreatureConstants.Golem_Clay, SizeConstants.Large, ChallengeRatingConstants.CR10, null, false, 0, 14, 2, false),
                [CreatureConstants.Golem_Flesh] = GetCreatureData(CreatureConstants.Golem_Flesh, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 0, 10, 2, false),
                [CreatureConstants.Golem_Iron] = GetCreatureData(CreatureConstants.Golem_Iron, SizeConstants.Large, ChallengeRatingConstants.CR13, null, false, 0, 22, 2, false),
                [CreatureConstants.Golem_Stone] = GetCreatureData(CreatureConstants.Golem_Stone, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 0, 18, 2, false),
                [CreatureConstants.Golem_Stone_Greater] = GetCreatureData(CreatureConstants.Golem_Stone_Greater, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, false, 0, 21, 2, false),
                [CreatureConstants.Gorgon] = GetCreatureData(CreatureConstants.Gorgon, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 11, 0, true),
                [CreatureConstants.GrayOoze] = GetCreatureData(CreatureConstants.GrayOoze, SizeConstants.Medium, ChallengeRatingConstants.CR4, null, false, 0, 0, 0, false),
                [CreatureConstants.GrayRender] = GetCreatureData(CreatureConstants.GrayRender, SizeConstants.Large, ChallengeRatingConstants.CR8, 5, false, 0, 10, 2, true),
                [CreatureConstants.GreenHag] = GetCreatureData(CreatureConstants.GreenHag, SizeConstants.Medium, ChallengeRatingConstants.CR5, 0, true, 9, 11, 2, true),
                [CreatureConstants.Grick] = GetCreatureData(CreatureConstants.Grick, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 4, 0, true),
                [CreatureConstants.Grig] = GetCreatureData(CreatureConstants.Grig, SizeConstants.Tiny, ChallengeRatingConstants.CR1, 3, true, 9, 2, 2, true),
                [CreatureConstants.Grig_WithFiddle] = GetCreatureData(CreatureConstants.Grig_WithFiddle, SizeConstants.Tiny, ChallengeRatingConstants.CR1, 3, true, 9, 2, 2, true),
                [CreatureConstants.Griffon] = GetCreatureData(CreatureConstants.Griffon, SizeConstants.Large, ChallengeRatingConstants.CR4, 3, false, 0, 6, 0, true),
                [CreatureConstants.Grimlock] = GetCreatureData(CreatureConstants.Grimlock, SizeConstants.Medium, ChallengeRatingConstants.CR1, 2, true, 0, 4, 2, true),
                [CreatureConstants.Gynosphinx] = GetCreatureData(CreatureConstants.Gynosphinx, SizeConstants.Large, ChallengeRatingConstants.CR8, 4, false, 14, 11, 0, true),
                [CreatureConstants.Halfling_Deep] = GetCreatureData(CreatureConstants.Halfling_Deep, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Halfling_Lightfoot] = GetCreatureData(CreatureConstants.Halfling_Lightfoot, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Halfling_Tallfellow] = GetCreatureData(CreatureConstants.Halfling_Tallfellow, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Harpy] = GetCreatureData(CreatureConstants.Harpy, SizeConstants.Medium, ChallengeRatingConstants.CR4, 3, true, 0, 1, 2, true),
                [CreatureConstants.Hawk] = GetCreatureData(CreatureConstants.Hawk, SizeConstants.Tiny, ChallengeRatingConstants.CR1_3rd, null, false, 0, 2, 0, true),
                [CreatureConstants.Hellcat_Bezekira] = GetCreatureData(CreatureConstants.Hellcat_Bezekira, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 0, 7, 0, false),
                [CreatureConstants.Hellwasp_Swarm] = GetCreatureData(CreatureConstants.Hellwasp_Swarm, SizeConstants.Diminutive, ChallengeRatingConstants.CR8, null, false, 0, 0, 0, false, space: 10),
                [CreatureConstants.HellHound] = GetCreatureData(CreatureConstants.HellHound, SizeConstants.Medium, ChallengeRatingConstants.CR3, 3, false, 0, 5, 0, false),
                [CreatureConstants.HellHound_NessianWarhound] = GetCreatureData(CreatureConstants.HellHound_NessianWarhound, SizeConstants.Large, ChallengeRatingConstants.CR9, 4, false, 0, 7, 0, false),
                [CreatureConstants.Hezrou] = GetCreatureData(CreatureConstants.Hezrou, SizeConstants.Large, ChallengeRatingConstants.CR11, 9, true, 13, 14, 2, false),
                [CreatureConstants.Hieracosphinx] = GetCreatureData(CreatureConstants.Hieracosphinx, SizeConstants.Large, ChallengeRatingConstants.CR5, 3, false, 0, 8, 0, true),
                [CreatureConstants.Hippogriff] = GetCreatureData(CreatureConstants.Hippogriff, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, true),
                [CreatureConstants.Hobgoblin] = GetCreatureData(CreatureConstants.Hobgoblin, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 1, true, 0, 0, 2, true),
                [CreatureConstants.Homunculus] = GetCreatureData(CreatureConstants.Homunculus, SizeConstants.Tiny, ChallengeRatingConstants.CR1, null, false, 0, 0, 0, false),
                [CreatureConstants.HornedDevil_Cornugon] = GetCreatureData(CreatureConstants.HornedDevil_Cornugon, SizeConstants.Large, ChallengeRatingConstants.CR16, null, true, 15, 19, 2, false),
                [CreatureConstants.Horse_Heavy] = GetCreatureData(CreatureConstants.Horse_Heavy, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, true),
                [CreatureConstants.Horse_Heavy_War] = GetCreatureData(CreatureConstants.Horse_Heavy_War, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, true),
                [CreatureConstants.Horse_Light] = GetCreatureData(CreatureConstants.Horse_Light, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, true),
                [CreatureConstants.Horse_Light_War] = GetCreatureData(CreatureConstants.Horse_Light_War, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 4, 0, true),
                [CreatureConstants.HoundArchon] = GetCreatureData(CreatureConstants.HoundArchon, SizeConstants.Medium, ChallengeRatingConstants.CR4, 5, true, 6, 0, 2, false),
                [CreatureConstants.Howler] = GetCreatureData(CreatureConstants.Howler, SizeConstants.Large, ChallengeRatingConstants.CR3, 3, false, 0, 5, 0, false),
                [CreatureConstants.Human] = GetCreatureData(CreatureConstants.Human, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Hydra_5Heads] = GetCreatureData(CreatureConstants.Hydra_5Heads, SizeConstants.Huge, ChallengeRatingConstants.CR4, null, false, 0, 6, 0, true),
                [CreatureConstants.Hydra_6Heads] = GetCreatureData(CreatureConstants.Hydra_6Heads, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 7, 0, true),
                [CreatureConstants.Hydra_7Heads] = GetCreatureData(CreatureConstants.Hydra_7Heads, SizeConstants.Huge, ChallengeRatingConstants.CR6, null, false, 0, 8, 0, true),
                [CreatureConstants.Hydra_8Heads] = GetCreatureData(CreatureConstants.Hydra_8Heads, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 9, 0, true),
                [CreatureConstants.Hydra_9Heads] = GetCreatureData(CreatureConstants.Hydra_9Heads, SizeConstants.Huge, ChallengeRatingConstants.CR8, null, false, 0, 10, 0, true),
                [CreatureConstants.Hydra_10Heads] = GetCreatureData(CreatureConstants.Hydra_10Heads, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 11, 0, true),
                [CreatureConstants.Hydra_11Heads] = GetCreatureData(CreatureConstants.Hydra_11Heads, SizeConstants.Huge, ChallengeRatingConstants.CR10, null, false, 0, 12, 0, true),
                [CreatureConstants.Hydra_12Heads] = GetCreatureData(CreatureConstants.Hydra_12Heads, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 13, 0, true),
                [CreatureConstants.Hyena] = GetCreatureData(CreatureConstants.Hyena, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, true),
                [CreatureConstants.IceDevil_Gelugon] = GetCreatureData(CreatureConstants.IceDevil_Gelugon, SizeConstants.Large, ChallengeRatingConstants.CR13, null, true, 13, 18, 2, false),
                [CreatureConstants.Imp] = GetCreatureData(CreatureConstants.Imp, SizeConstants.Tiny, ChallengeRatingConstants.CR2, null, false, 6, 5, 0, false),
                [CreatureConstants.InvisibleStalker] = GetCreatureData(CreatureConstants.InvisibleStalker, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 0, 4, 0, false),
                [CreatureConstants.Janni] = GetCreatureData(CreatureConstants.Janni, SizeConstants.Medium, ChallengeRatingConstants.CR4, 5, true, 12, 1, 2, true),
                [CreatureConstants.Kobold] = GetCreatureData(CreatureConstants.Kobold, SizeConstants.Small, ChallengeRatingConstants.CR1_4th, 0, true, 0, 1, 2, true),
                [CreatureConstants.Kolyarut] = GetCreatureData(CreatureConstants.Kolyarut, SizeConstants.Medium, ChallengeRatingConstants.CR12, null, true, 13, 10, 2, false),
                [CreatureConstants.Kraken] = GetCreatureData(CreatureConstants.Kraken, SizeConstants.Gargantuan, ChallengeRatingConstants.CR12, null, false, 9, 14, 0, false),
                [CreatureConstants.Krenshar] = GetCreatureData(CreatureConstants.Krenshar, SizeConstants.Medium, ChallengeRatingConstants.CR1, 2, false, 3, 3, 0, true),
                [CreatureConstants.KuoToa] = GetCreatureData(CreatureConstants.KuoToa, SizeConstants.Medium, ChallengeRatingConstants.CR2, 3, true, 0, 6, 2, true),
                [CreatureConstants.Lamia] = GetCreatureData(CreatureConstants.Lamia, SizeConstants.Large, ChallengeRatingConstants.CR6, 4, true, 9, 7, 2, true),
                [CreatureConstants.Lammasu] = GetCreatureData(CreatureConstants.Lammasu, SizeConstants.Large, ChallengeRatingConstants.CR8, 5, false, 7, 10, 0, true),
                [CreatureConstants.LanternArchon] = GetCreatureData(CreatureConstants.LanternArchon, SizeConstants.Small, ChallengeRatingConstants.CR2, null, false, 3, 4, 0, false),
                [CreatureConstants.Lemure] = GetCreatureData(CreatureConstants.Lemure, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 4, 0, false),
                [CreatureConstants.Leonal] = GetCreatureData(CreatureConstants.Leonal, SizeConstants.Medium, ChallengeRatingConstants.CR12, null, false, 10, 14, 2, false),
                [CreatureConstants.Leopard] = GetCreatureData(CreatureConstants.Leopard, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 1, 0, true),
                [CreatureConstants.Lillend] = GetCreatureData(CreatureConstants.Lillend, SizeConstants.Large, ChallengeRatingConstants.CR7, 6, true, 10, 5, 2, false),
                [CreatureConstants.Lion] = GetCreatureData(CreatureConstants.Lion, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 3, 0, true),
                [CreatureConstants.Lion_Dire] = GetCreatureData(CreatureConstants.Lion_Dire, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 4, 0, true),
                [CreatureConstants.Lizard] = GetCreatureData(CreatureConstants.Lizard, SizeConstants.Tiny, ChallengeRatingConstants.CR1_6th, null, false, 0, 0, 0, true),
                [CreatureConstants.Lizard_Monitor] = GetCreatureData(CreatureConstants.Lizard_Monitor, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 3, 0, true),
                [CreatureConstants.Lizardfolk] = GetCreatureData(CreatureConstants.Lizardfolk, SizeConstants.Medium, ChallengeRatingConstants.CR1, 1, true, 0, 5, 2, true),
                [CreatureConstants.Locathah] = GetCreatureData(CreatureConstants.Locathah, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 1, true, 0, 3, 2, true),
                [CreatureConstants.Locust_Swarm] = GetCreatureData(CreatureConstants.Locust_Swarm, SizeConstants.Diminutive, ChallengeRatingConstants.CR3, null, false, 0, 0, 0, false, space: 10),
                [CreatureConstants.Magmin] = GetCreatureData(CreatureConstants.Magmin, SizeConstants.Small, ChallengeRatingConstants.CR3, null, false, 0, 6, 0, false),
                [CreatureConstants.MantaRay] = GetCreatureData(CreatureConstants.MantaRay, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, true),
                [CreatureConstants.Manticore] = GetCreatureData(CreatureConstants.Manticore, SizeConstants.Large, ChallengeRatingConstants.CR5, 3, false, 0, 6, 0, true),
                [CreatureConstants.Marilith] = GetCreatureData(CreatureConstants.Marilith, SizeConstants.Large, ChallengeRatingConstants.CR17, null, true, 16, 16, 6, false),
                [CreatureConstants.Marut] = GetCreatureData(CreatureConstants.Marut, SizeConstants.Large, ChallengeRatingConstants.CR15, null, false, 14, 16, 2, false),
                [CreatureConstants.Medusa] = GetCreatureData(CreatureConstants.Medusa, SizeConstants.Medium, ChallengeRatingConstants.CR7, 0, true, 0, 3, 2, true),
                [CreatureConstants.Megaraptor] = GetCreatureData(CreatureConstants.Megaraptor, SizeConstants.Large, ChallengeRatingConstants.CR6, null, false, 0, 6, 0, true),
                [CreatureConstants.Mephit_Air] = GetCreatureData(CreatureConstants.Mephit_Air, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 3, 2, false),
                [CreatureConstants.Mephit_Dust] = GetCreatureData(CreatureConstants.Mephit_Dust, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 3, 2, false),
                [CreatureConstants.Mephit_Earth] = GetCreatureData(CreatureConstants.Mephit_Earth, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 6, 6, 2, false),
                [CreatureConstants.Mephit_Fire] = GetCreatureData(CreatureConstants.Mephit_Fire, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 4, 2, false),
                [CreatureConstants.Mephit_Ice] = GetCreatureData(CreatureConstants.Mephit_Ice, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 4, 2, false),
                [CreatureConstants.Mephit_Magma] = GetCreatureData(CreatureConstants.Mephit_Magma, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 6, 4, 2, false),
                [CreatureConstants.Mephit_Ooze] = GetCreatureData(CreatureConstants.Mephit_Ooze, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 5, 2, false),
                [CreatureConstants.Mephit_Salt] = GetCreatureData(CreatureConstants.Mephit_Salt, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 6, 2, false),
                [CreatureConstants.Mephit_Steam] = GetCreatureData(CreatureConstants.Mephit_Steam, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 4, 2, false),
                [CreatureConstants.Mephit_Water] = GetCreatureData(CreatureConstants.Mephit_Water, SizeConstants.Small, ChallengeRatingConstants.CR3, 3, true, 3, 5, 2, false),
                [CreatureConstants.Merfolk] = GetCreatureData(CreatureConstants.Merfolk, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 1, true, 0, 0, 2, true),
                [CreatureConstants.Mimic] = GetCreatureData(CreatureConstants.Mimic, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 5, 0, false),
                [CreatureConstants.MindFlayer] = GetCreatureData(CreatureConstants.MindFlayer, SizeConstants.Medium, ChallengeRatingConstants.CR8, 7, true, 8, 3, 2, true),
                [CreatureConstants.Minotaur] = GetCreatureData(CreatureConstants.Minotaur, SizeConstants.Large, ChallengeRatingConstants.CR4, 2, true, 0, 5, 2, true),
                [CreatureConstants.Mohrg] = GetCreatureData(CreatureConstants.Mohrg, SizeConstants.Medium, ChallengeRatingConstants.CR8, null, false, 0, 9, 0, false),
                [CreatureConstants.Monkey] = GetCreatureData(CreatureConstants.Monkey, SizeConstants.Tiny, ChallengeRatingConstants.CR1_6th, null, false, 0, 0, 0, true),
                [CreatureConstants.Mule] = GetCreatureData(CreatureConstants.Mule, SizeConstants.Large, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, true),
                [CreatureConstants.Mummy] = GetCreatureData(CreatureConstants.Mummy, SizeConstants.Medium, ChallengeRatingConstants.CR5, 0, true, 0, 10, 2, false),
                [CreatureConstants.Naga_Dark] = GetCreatureData(CreatureConstants.Naga_Dark, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 7, 3, 0, true),
                [CreatureConstants.Naga_Guardian] = GetCreatureData(CreatureConstants.Naga_Guardian, SizeConstants.Large, ChallengeRatingConstants.CR10, null, false, 9, 7, 0, true),
                [CreatureConstants.Naga_Spirit] = GetCreatureData(CreatureConstants.Naga_Spirit, SizeConstants.Large, ChallengeRatingConstants.CR9, null, false, 7, 6, 0, true),
                [CreatureConstants.Naga_Water] = GetCreatureData(CreatureConstants.Naga_Water, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 7, 5, 0, true),
                [CreatureConstants.Nalfeshnee] = GetCreatureData(CreatureConstants.Nalfeshnee, SizeConstants.Huge, ChallengeRatingConstants.CR14, null, true, 12, 18, 2, false),
                [CreatureConstants.NightHag] = GetCreatureData(CreatureConstants.NightHag, SizeConstants.Medium, ChallengeRatingConstants.CR9, null, true, 8, 11, 2, false),
                [CreatureConstants.Nightcrawler] = GetCreatureData(CreatureConstants.Nightcrawler, SizeConstants.Gargantuan, ChallengeRatingConstants.CR18, null, false, 25, 29, 0, false),
                [CreatureConstants.Nightmare] = GetCreatureData(CreatureConstants.Nightmare, SizeConstants.Large, ChallengeRatingConstants.CR5, 4, false, 20, 13, 0, false),
                [CreatureConstants.Nightmare_Cauchemar] = GetCreatureData(CreatureConstants.Nightmare_Cauchemar, SizeConstants.Huge, ChallengeRatingConstants.CR11, 4, false, 20, 16, 0, false),
                [CreatureConstants.Nightwalker] = GetCreatureData(CreatureConstants.Nightwalker, SizeConstants.Huge, ChallengeRatingConstants.CR16, null, false, 21, 22, 2, false),
                [CreatureConstants.Nightwing] = GetCreatureData(CreatureConstants.Nightwing, SizeConstants.Huge, ChallengeRatingConstants.CR14, null, false, 17, 18, 0, false),
                [CreatureConstants.Nixie] = GetCreatureData(CreatureConstants.Nixie, SizeConstants.Small, ChallengeRatingConstants.CR1, 3, true, 12, 0, 2, true),
                [CreatureConstants.Nymph] = GetCreatureData(CreatureConstants.Nymph, SizeConstants.Medium, ChallengeRatingConstants.CR7, 7, true, 7, 0, 2, true),
                [CreatureConstants.OchreJelly] = GetCreatureData(CreatureConstants.OchreJelly, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 0, 0, 0, false),
                [CreatureConstants.Octopus] = GetCreatureData(CreatureConstants.Octopus, SizeConstants.Small, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, false),
                [CreatureConstants.Octopus_Giant] = GetCreatureData(CreatureConstants.Octopus_Giant, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 7, 0, false),
                [CreatureConstants.Ogre] = GetCreatureData(CreatureConstants.Ogre, SizeConstants.Large, ChallengeRatingConstants.CR3, 2, true, 0, 5, 2, true),
                [CreatureConstants.Ogre_Merrow] = GetCreatureData(CreatureConstants.Ogre_Merrow, SizeConstants.Large, ChallengeRatingConstants.CR3, 2, true, 0, 5, 2, true),
                [CreatureConstants.OgreMage] = GetCreatureData(CreatureConstants.OgreMage, SizeConstants.Large, ChallengeRatingConstants.CR8, 7, true, 9, 5, 2, true),
                [CreatureConstants.Orc] = GetCreatureData(CreatureConstants.Orc, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Orc_Half] = GetCreatureData(CreatureConstants.Orc_Half, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 0, true, 0, 0, 2, true),
                [CreatureConstants.Otyugh] = GetCreatureData(CreatureConstants.Otyugh, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 8, 0, true),
                [CreatureConstants.Owl] = GetCreatureData(CreatureConstants.Owl, SizeConstants.Tiny, ChallengeRatingConstants.CR1_4th, null, false, 0, 2, 0, true),
                [CreatureConstants.Owl_Giant] = GetCreatureData(CreatureConstants.Owl_Giant, SizeConstants.Large, ChallengeRatingConstants.CR3, 2, false, 0, 3, 0, true),
                [CreatureConstants.Owlbear] = GetCreatureData(CreatureConstants.Owlbear, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 5, 0, true),
                [CreatureConstants.Pegasus] = GetCreatureData(CreatureConstants.Pegasus, SizeConstants.Large, ChallengeRatingConstants.CR3, 2, false, 5, 3, 0, true),
                [CreatureConstants.PhantomFungus] = GetCreatureData(CreatureConstants.PhantomFungus, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 12, 4, 0, false),
                [CreatureConstants.PhaseSpider] = GetCreatureData(CreatureConstants.PhaseSpider, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 15, 3, 0, false),
                [CreatureConstants.Phasm] = GetCreatureData(CreatureConstants.Phasm, SizeConstants.Medium, ChallengeRatingConstants.CR7, null, true, 0, 5, 0, false),
                [CreatureConstants.PitFiend] = GetCreatureData(CreatureConstants.PitFiend, SizeConstants.Large, ChallengeRatingConstants.CR20, null, false, 18, 23, 2, false),
                [CreatureConstants.Pixie] = GetCreatureData(CreatureConstants.Pixie, SizeConstants.Small, ChallengeRatingConstants.CR4, 4, true, 8, 1, 2, true),
                [CreatureConstants.Pixie_WithIrresistibleDance] = GetCreatureData(CreatureConstants.Pixie_WithIrresistibleDance, SizeConstants.Small, ChallengeRatingConstants.CR5, 6, true, 8, 1, 2, true),
                [CreatureConstants.Pony] = GetCreatureData(CreatureConstants.Pony, SizeConstants.Medium, ChallengeRatingConstants.CR1_4th, null, false, 0, 2, 0, true),
                [CreatureConstants.Pony_War] = GetCreatureData(CreatureConstants.Pony_War, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, null, false, 0, 2, 0, true),
                [CreatureConstants.Porpoise] = GetCreatureData(CreatureConstants.Porpoise, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, null, false, 0, 2, 0, true),
                [CreatureConstants.PrayingMantis_Giant] = GetCreatureData(CreatureConstants.PrayingMantis_Giant, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 6, 0, false),
                [CreatureConstants.Pseudodragon] = GetCreatureData(CreatureConstants.Pseudodragon, SizeConstants.Tiny, ChallengeRatingConstants.CR1, 3, false, 0, 4, 0, true),
                [CreatureConstants.PurpleWorm] = GetCreatureData(CreatureConstants.PurpleWorm, SizeConstants.Gargantuan, ChallengeRatingConstants.CR12, null, false, 0, 15, 0, false),
                [CreatureConstants.Pyrohydra_5Heads] = GetCreatureData(CreatureConstants.Pyrohydra_5Heads, SizeConstants.Huge, ChallengeRatingConstants.CR6, null, false, 0, 6, 0, true),
                [CreatureConstants.Pyrohydra_6Heads] = GetCreatureData(CreatureConstants.Pyrohydra_6Heads, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 7, 0, true),
                [CreatureConstants.Pyrohydra_7Heads] = GetCreatureData(CreatureConstants.Pyrohydra_7Heads, SizeConstants.Huge, ChallengeRatingConstants.CR8, null, false, 0, 8, 0, true),
                [CreatureConstants.Pyrohydra_8Heads] = GetCreatureData(CreatureConstants.Pyrohydra_8Heads, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 9, 0, true),
                [CreatureConstants.Pyrohydra_9Heads] = GetCreatureData(CreatureConstants.Pyrohydra_9Heads, SizeConstants.Huge, ChallengeRatingConstants.CR10, null, false, 0, 10, 0, true),
                [CreatureConstants.Pyrohydra_10Heads] = GetCreatureData(CreatureConstants.Pyrohydra_10Heads, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 11, 0, true),
                [CreatureConstants.Pyrohydra_11Heads] = GetCreatureData(CreatureConstants.Pyrohydra_11Heads, SizeConstants.Huge, ChallengeRatingConstants.CR12, null, false, 0, 12, 0, true),
                [CreatureConstants.Pyrohydra_12Heads] = GetCreatureData(CreatureConstants.Pyrohydra_12Heads, SizeConstants.Huge, ChallengeRatingConstants.CR13, null, false, 0, 13, 0, true),
                [CreatureConstants.Quasit] = GetCreatureData(CreatureConstants.Quasit, SizeConstants.Tiny, ChallengeRatingConstants.CR2, null, false, 6, 3, 2, false),
                [CreatureConstants.Rakshasa] = GetCreatureData(CreatureConstants.Rakshasa, SizeConstants.Medium, ChallengeRatingConstants.CR10, 7, true, 7, 9, 2, true),
                [CreatureConstants.Rast] = GetCreatureData(CreatureConstants.Rast, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 0, 4, 0, false),
                [CreatureConstants.Rat] = GetCreatureData(CreatureConstants.Rat, SizeConstants.Tiny, ChallengeRatingConstants.CR1_8th, null, false, 0, 0, 0, true),
                [CreatureConstants.Rat_Dire] = GetCreatureData(CreatureConstants.Rat_Dire, SizeConstants.Small, ChallengeRatingConstants.CR1_3rd, null, false, 0, 1, 0, true),
                [CreatureConstants.Rat_Swarm] = GetCreatureData(CreatureConstants.Rat_Swarm, SizeConstants.Tiny, ChallengeRatingConstants.CR2, null, false, 0, 0, 0, true, space: 10),
                [CreatureConstants.Raven] = GetCreatureData(CreatureConstants.Raven, SizeConstants.Tiny, ChallengeRatingConstants.CR1_6th, null, false, 0, 0, 0, true),
                [CreatureConstants.Ravid] = GetCreatureData(CreatureConstants.Ravid, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 20, 15, 1, false),
                [CreatureConstants.RazorBoar] = GetCreatureData(CreatureConstants.RazorBoar, SizeConstants.Large, ChallengeRatingConstants.CR10, null, false, 0, 17, 0, true),
                [CreatureConstants.Remorhaz] = GetCreatureData(CreatureConstants.Remorhaz, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 11, 0, false),
                [CreatureConstants.Retriever] = GetCreatureData(CreatureConstants.Retriever, SizeConstants.Huge, ChallengeRatingConstants.CR11, null, false, 0, 10, 0, false),
                [CreatureConstants.Rhinoceras] = GetCreatureData(CreatureConstants.Rhinoceras, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 7, 0, true),
                [CreatureConstants.Roc] = GetCreatureData(CreatureConstants.Roc, SizeConstants.Gargantuan, ChallengeRatingConstants.CR9, null, false, 0, 9, 0, true),
                [CreatureConstants.Roper] = GetCreatureData(CreatureConstants.Roper, SizeConstants.Large, ChallengeRatingConstants.CR12, null, false, 0, 14, 0, false),
                [CreatureConstants.RustMonster] = GetCreatureData(CreatureConstants.RustMonster, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 5, 0, true),
                [CreatureConstants.Sahuagin] = GetCreatureData(CreatureConstants.Sahuagin, SizeConstants.Medium, ChallengeRatingConstants.CR2, 2, true, 0, 5, 2, true),
                [CreatureConstants.Sahuagin_Mutant] = GetCreatureData(CreatureConstants.Sahuagin_Mutant, SizeConstants.Medium, ChallengeRatingConstants.CR2, 3, true, 0, 5, 4, true),
                [CreatureConstants.Sahuagin_Malenti] = GetCreatureData(CreatureConstants.Sahuagin_Malenti, SizeConstants.Medium, ChallengeRatingConstants.CR2, 2, true, 0, 5, 2, true),
                [CreatureConstants.Salamander_Average] = GetCreatureData(CreatureConstants.Salamander_Average, SizeConstants.Medium, ChallengeRatingConstants.CR6, 5, true, 0, 7, 2, false),
                [CreatureConstants.Salamander_Flamebrother] = GetCreatureData(CreatureConstants.Salamander_Flamebrother, SizeConstants.Small, ChallengeRatingConstants.CR3, 4, true, 0, 7, 2, false),
                [CreatureConstants.Salamander_Noble] = GetCreatureData(CreatureConstants.Salamander_Noble, SizeConstants.Large, ChallengeRatingConstants.CR10, null, true, 15, 8, 2, false),
                [CreatureConstants.Satyr] = GetCreatureData(CreatureConstants.Satyr, SizeConstants.Medium, ChallengeRatingConstants.CR2, 2, true, 0, 4, 2, true),
                [CreatureConstants.Satyr_WithPipes] = GetCreatureData(CreatureConstants.Satyr_WithPipes, SizeConstants.Medium, ChallengeRatingConstants.CR4, 2, true, 10, 4, 2, true),
                [CreatureConstants.Scorpion_Monstrous_Colossal] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Colossal, SizeConstants.Colossal, ChallengeRatingConstants.CR12, null, false, 0, 25, 0, false, space: 40, reach: 30),
                [CreatureConstants.Scorpion_Monstrous_Gargantuan] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Gargantuan, SizeConstants.Gargantuan, ChallengeRatingConstants.CR10, null, false, 0, 18, 0, false),
                [CreatureConstants.Scorpion_Monstrous_Huge] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR7, null, false, 0, 12, 0, false),
                [CreatureConstants.Scorpion_Monstrous_Large] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Large, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 7, 0, false),
                [CreatureConstants.Scorpion_Monstrous_Medium] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 4, 0, false),
                [CreatureConstants.Scorpion_Monstrous_Small] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Small, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, false, 0, 3, 0, false),
                [CreatureConstants.Scorpion_Monstrous_Tiny] = GetCreatureData(CreatureConstants.Scorpion_Monstrous_Tiny, SizeConstants.Tiny, ChallengeRatingConstants.CR1_4th, null, false, 0, 2, 0, false),
                [CreatureConstants.Scorpionfolk] = GetCreatureData(CreatureConstants.Scorpionfolk, SizeConstants.Large, ChallengeRatingConstants.CR7, 4, true, 10, 6, 2, true),
                [CreatureConstants.SeaCat] = GetCreatureData(CreatureConstants.SeaCat, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 8, 0, true),
                [CreatureConstants.SeaHag] = GetCreatureData(CreatureConstants.SeaHag, SizeConstants.Medium, ChallengeRatingConstants.CR4, 0, true, 0, 3, 2, true),
                [CreatureConstants.Shadow] = GetCreatureData(CreatureConstants.Shadow, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 0, 0, false),
                [CreatureConstants.Shadow_Greater] = GetCreatureData(CreatureConstants.Shadow_Greater, SizeConstants.Medium, ChallengeRatingConstants.CR8, null, false, 0, 0, 0, false),
                [CreatureConstants.ShadowMastiff] = GetCreatureData(CreatureConstants.ShadowMastiff, SizeConstants.Medium, ChallengeRatingConstants.CR5, 3, false, 0, 3, 0, false),
                [CreatureConstants.ShamblingMound] = GetCreatureData(CreatureConstants.ShamblingMound, SizeConstants.Large, ChallengeRatingConstants.CR6, 6, false, 0, 11, 0, false),
                [CreatureConstants.Shark_Dire] = GetCreatureData(CreatureConstants.Shark_Dire, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 7, 0, true),
                [CreatureConstants.Shark_Huge] = GetCreatureData(CreatureConstants.Shark_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR4, null, false, 0, 5, 0, true),
                [CreatureConstants.Shark_Large] = GetCreatureData(CreatureConstants.Shark_Large, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 4, 0, true),
                [CreatureConstants.Shark_Medium] = GetCreatureData(CreatureConstants.Shark_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, true),
                [CreatureConstants.ShieldGuardian] = GetCreatureData(CreatureConstants.ShieldGuardian, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 15, 2, false),
                [CreatureConstants.ShockerLizard] = GetCreatureData(CreatureConstants.ShockerLizard, SizeConstants.Small, ChallengeRatingConstants.CR2, null, false, 0, 3, 0, true),
                [CreatureConstants.Shrieker] = GetCreatureData(CreatureConstants.Shrieker, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, false, reach: 0),
                [CreatureConstants.Skum] = GetCreatureData(CreatureConstants.Skum, SizeConstants.Medium, ChallengeRatingConstants.CR2, 3, true, 0, 2, 2, true),
                [CreatureConstants.Slaad_Blue] = GetCreatureData(CreatureConstants.Slaad_Blue, SizeConstants.Large, ChallengeRatingConstants.CR8, 6, true, 8, 9, 2, false),
                [CreatureConstants.Slaad_Death] = GetCreatureData(CreatureConstants.Slaad_Death, SizeConstants.Medium, ChallengeRatingConstants.CR13, null, true, 15, 12, 2, false),
                [CreatureConstants.Slaad_Gray] = GetCreatureData(CreatureConstants.Slaad_Gray, SizeConstants.Medium, ChallengeRatingConstants.CR10, 6, true, 10, 11, 2, false),
                [CreatureConstants.Slaad_Green] = GetCreatureData(CreatureConstants.Slaad_Green, SizeConstants.Large, ChallengeRatingConstants.CR9, 7, true, 9, 13, 2, false),
                [CreatureConstants.Slaad_Red] = GetCreatureData(CreatureConstants.Slaad_Red, SizeConstants.Large, ChallengeRatingConstants.CR7, 6, true, 0, 8, 2, false),
                [CreatureConstants.Snake_Constrictor] = GetCreatureData(CreatureConstants.Snake_Constrictor, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 2, 0, true),
                [CreatureConstants.Snake_Constrictor_Giant] = GetCreatureData(CreatureConstants.Snake_Constrictor_Giant, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 4, 0, true),
                [CreatureConstants.Snake_Viper_Huge] = GetCreatureData(CreatureConstants.Snake_Viper_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR3, null, false, 0, 3, 0, true),
                [CreatureConstants.Snake_Viper_Large] = GetCreatureData(CreatureConstants.Snake_Viper_Large, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 3, 0, true),
                [CreatureConstants.Snake_Viper_Medium] = GetCreatureData(CreatureConstants.Snake_Viper_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, true),
                [CreatureConstants.Snake_Viper_Small] = GetCreatureData(CreatureConstants.Snake_Viper_Small, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, false, 0, 3, 0, true),
                [CreatureConstants.Snake_Viper_Tiny] = GetCreatureData(CreatureConstants.Snake_Viper_Tiny, SizeConstants.Tiny, ChallengeRatingConstants.CR1_3rd, null, false, 0, 2, 0, true),
                [CreatureConstants.Spectre] = GetCreatureData(CreatureConstants.Spectre, SizeConstants.Medium, ChallengeRatingConstants.CR7, null, false, 0, 0, 0, false),
                [CreatureConstants.Spider_Monstrous_Hunter_Colossal] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Colossal, SizeConstants.Colossal, ChallengeRatingConstants.CR11, null, false, 0, 18, 0, false, space: 40, reach: 30),
                [CreatureConstants.Spider_Monstrous_Hunter_Gargantuan] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Gargantuan, SizeConstants.Gargantuan, ChallengeRatingConstants.CR8, null, false, 0, 10, 0, false),
                [CreatureConstants.Spider_Monstrous_Hunter_Huge] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 5, 0, false),
                [CreatureConstants.Spider_Monstrous_Hunter_Large] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Large, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 2, 0, false),
                [CreatureConstants.Spider_Monstrous_Hunter_Medium] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 1, 0, false),
                [CreatureConstants.Spider_Monstrous_Hunter_Small] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Small, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.Spider_Monstrous_Hunter_Tiny] = GetCreatureData(CreatureConstants.Spider_Monstrous_Hunter_Tiny, SizeConstants.Tiny, ChallengeRatingConstants.CR1_4th, null, false, 0, 0, 0, false),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Colossal] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Colossal, SizeConstants.Colossal, ChallengeRatingConstants.CR11, null, false, 0, 18, 0, false, space: 40, reach: 30),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan, SizeConstants.Gargantuan, ChallengeRatingConstants.CR8, null, false, 0, 10, 0, false),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Huge] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Huge, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 5, 0, false),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Large] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Large, SizeConstants.Large, ChallengeRatingConstants.CR2, null, false, 0, 2, 0, false),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Medium] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Medium, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 1, 0, false),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Small] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Small, SizeConstants.Small, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.Spider_Monstrous_WebSpinner_Tiny] = GetCreatureData(CreatureConstants.Spider_Monstrous_WebSpinner_Tiny, SizeConstants.Tiny, ChallengeRatingConstants.CR1_4th, null, false, 0, 0, 0, false),
                [CreatureConstants.SpiderEater] = GetCreatureData(CreatureConstants.SpiderEater, SizeConstants.Large, ChallengeRatingConstants.CR5, null, false, 12, 4, 0, false),
                [CreatureConstants.Spider_Swarm] = GetCreatureData(CreatureConstants.Spider_Swarm, SizeConstants.Diminutive, ChallengeRatingConstants.CR1, null, false, 0, 0, 0, false, space: 10),
                [CreatureConstants.Squid] = GetCreatureData(CreatureConstants.Squid, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 3, 0, false),
                [CreatureConstants.Squid_Giant] = GetCreatureData(CreatureConstants.Squid_Giant, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 6, 0, false),
                [CreatureConstants.StagBeetle_Giant] = GetCreatureData(CreatureConstants.StagBeetle_Giant, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 10, 0, false),
                [CreatureConstants.Stirge] = GetCreatureData(CreatureConstants.Stirge, SizeConstants.Tiny, ChallengeRatingConstants.CR1_2nd, null, false, 0, 0, 0, false),
                [CreatureConstants.Succubus] = GetCreatureData(CreatureConstants.Succubus, SizeConstants.Medium, ChallengeRatingConstants.CR7, 6, true, 12, 9, 2, false),
                [CreatureConstants.Tarrasque] = GetCreatureData(CreatureConstants.Tarrasque, SizeConstants.Colossal, ChallengeRatingConstants.CR20, null, false, 0, 30, 2, true),
                [CreatureConstants.Tendriculos] = GetCreatureData(CreatureConstants.Tendriculos, SizeConstants.Huge, ChallengeRatingConstants.CR6, null, false, 0, 9, 0, false),
                [CreatureConstants.Thoqqua] = GetCreatureData(CreatureConstants.Thoqqua, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 7, 0, false),
                [CreatureConstants.Tiefling] = GetCreatureData(CreatureConstants.Tiefling, SizeConstants.Medium, ChallengeRatingConstants.CR1_2nd, 1, true, 1, 0, 2, true),
                [CreatureConstants.Tiger] = GetCreatureData(CreatureConstants.Tiger, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 3, 0, true),
                [CreatureConstants.Tiger_Dire] = GetCreatureData(CreatureConstants.Tiger_Dire, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 6, 0, true),
                [CreatureConstants.Titan] = GetCreatureData(CreatureConstants.Titan, SizeConstants.Huge, ChallengeRatingConstants.CR21, null, true, 20, 19, 2, false),
                [CreatureConstants.Toad] = GetCreatureData(CreatureConstants.Toad, SizeConstants.Diminutive, ChallengeRatingConstants.CR1_10th, null, false, 0, 0, 0, true),
                [CreatureConstants.Tojanida_Adult] = GetCreatureData(CreatureConstants.Tojanida_Adult, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 0, 12, 0, false),
                [CreatureConstants.Tojanida_Elder] = GetCreatureData(CreatureConstants.Tojanida_Elder, SizeConstants.Large, ChallengeRatingConstants.CR9, null, false, 0, 14, 0, false),
                [CreatureConstants.Tojanida_Juvenile] = GetCreatureData(CreatureConstants.Tojanida_Juvenile, SizeConstants.Small, ChallengeRatingConstants.CR3, null, false, 0, 10, 0, false),
                [CreatureConstants.Treant] = GetCreatureData(CreatureConstants.Treant, SizeConstants.Huge, ChallengeRatingConstants.CR8, 5, false, 12, 13, 2, false),
                [CreatureConstants.Triceratops] = GetCreatureData(CreatureConstants.Triceratops, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 11, 0, true),
                [CreatureConstants.Triton] = GetCreatureData(CreatureConstants.Triton, SizeConstants.Medium, ChallengeRatingConstants.CR2, 2, true, 7, 6, 2, true),
                [CreatureConstants.Troglodyte] = GetCreatureData(CreatureConstants.Troglodyte, SizeConstants.Medium, ChallengeRatingConstants.CR1, 2, true, 0, 6, 2, true),
                [CreatureConstants.Troll] = GetCreatureData(CreatureConstants.Troll, SizeConstants.Large, ChallengeRatingConstants.CR5, 5, true, 0, 5, 2, true),
                [CreatureConstants.Troll_Scrag] = GetCreatureData(CreatureConstants.Troll_Scrag, SizeConstants.Large, ChallengeRatingConstants.CR5, 5, true, 0, 5, 2, true),
                [CreatureConstants.TrumpetArchon] = GetCreatureData(CreatureConstants.TrumpetArchon, SizeConstants.Medium, ChallengeRatingConstants.CR14, 8, true, 12, 14, 2, false),
                [CreatureConstants.Tyrannosaurus] = GetCreatureData(CreatureConstants.Tyrannosaurus, SizeConstants.Huge, ChallengeRatingConstants.CR8, null, false, 0, 5, 0, true),
                [CreatureConstants.UmberHulk] = GetCreatureData(CreatureConstants.UmberHulk, SizeConstants.Large, ChallengeRatingConstants.CR7, null, false, 8, 8, 0, true),
                [CreatureConstants.UmberHulk_TrulyHorrid] = GetCreatureData(CreatureConstants.UmberHulk_TrulyHorrid, SizeConstants.Huge, ChallengeRatingConstants.CR14, null, false, 8, 14, 0, true),
                [CreatureConstants.Unicorn] = GetCreatureData(CreatureConstants.Unicorn, SizeConstants.Large, ChallengeRatingConstants.CR3, 4, false, 5, 6, 0, true),
                [CreatureConstants.VampireSpawn] = GetCreatureData(CreatureConstants.VampireSpawn, SizeConstants.Medium, ChallengeRatingConstants.CR4, null, false, 6, 3, 2, false),
                [CreatureConstants.Vargouille] = GetCreatureData(CreatureConstants.Vargouille, SizeConstants.Small, ChallengeRatingConstants.CR2, null, false, 0, 0, 0, false),
                [CreatureConstants.VioletFungus] = GetCreatureData(CreatureConstants.VioletFungus, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 4, 0, false, reach: 10),
                [CreatureConstants.Vrock] = GetCreatureData(CreatureConstants.Vrock, SizeConstants.Large, ChallengeRatingConstants.CR9, 8, true, 12, 11, 2, false),
                [CreatureConstants.Wasp_Giant] = GetCreatureData(CreatureConstants.Wasp_Giant, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 4, 0, false),
                [CreatureConstants.Weasel] = GetCreatureData(CreatureConstants.Weasel, SizeConstants.Tiny, ChallengeRatingConstants.CR1_4th, null, false, 0, 0, 0, true),
                [CreatureConstants.Weasel_Dire] = GetCreatureData(CreatureConstants.Weasel_Dire, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 2, 0, true),
                [CreatureConstants.Whale_Baleen] = GetCreatureData(CreatureConstants.Whale_Baleen, SizeConstants.Gargantuan, ChallengeRatingConstants.CR6, null, false, 0, 9, 0, true),
                [CreatureConstants.Whale_Cachalot] = GetCreatureData(CreatureConstants.Whale_Cachalot, SizeConstants.Gargantuan, ChallengeRatingConstants.CR7, null, false, 0, 9, 0, true),
                [CreatureConstants.Whale_Orca] = GetCreatureData(CreatureConstants.Whale_Orca, SizeConstants.Huge, ChallengeRatingConstants.CR5, null, false, 0, 6, 0, true),
                [CreatureConstants.Wight] = GetCreatureData(CreatureConstants.Wight, SizeConstants.Medium, ChallengeRatingConstants.CR3, null, false, 0, 4, 2, false),
                [CreatureConstants.WillOWisp] = GetCreatureData(CreatureConstants.WillOWisp, SizeConstants.Small, ChallengeRatingConstants.CR6, null, false, 0, 0, 0, false),
                [CreatureConstants.WinterWolf] = GetCreatureData(CreatureConstants.WinterWolf, SizeConstants.Large, ChallengeRatingConstants.CR5, 3, false, 0, 5, 0, true),
                [CreatureConstants.Wolf] = GetCreatureData(CreatureConstants.Wolf, SizeConstants.Medium, ChallengeRatingConstants.CR1, null, false, 0, 2, 0, true),
                [CreatureConstants.Wolf_Dire] = GetCreatureData(CreatureConstants.Wolf_Dire, SizeConstants.Large, ChallengeRatingConstants.CR3, null, false, 0, 3, 0, true),
                [CreatureConstants.Wolverine] = GetCreatureData(CreatureConstants.Wolverine, SizeConstants.Medium, ChallengeRatingConstants.CR2, null, false, 0, 2, 0, true),
                [CreatureConstants.Wolverine_Dire] = GetCreatureData(CreatureConstants.Wolverine_Dire, SizeConstants.Large, ChallengeRatingConstants.CR4, null, false, 0, 4, 0, true),
                [CreatureConstants.Worg] = GetCreatureData(CreatureConstants.Worg, SizeConstants.Medium, ChallengeRatingConstants.CR2, 1, false, 0, 2, 0, true),
                [CreatureConstants.Wraith] = GetCreatureData(CreatureConstants.Wraith, SizeConstants.Medium, ChallengeRatingConstants.CR5, null, false, 0, 0, 0, false),
                [CreatureConstants.Wraith_Dread] = GetCreatureData(CreatureConstants.Wraith_Dread, SizeConstants.Large, ChallengeRatingConstants.CR11, null, false, 0, 0, 0, false),
                [CreatureConstants.Wyvern] = GetCreatureData(CreatureConstants.Wyvern, SizeConstants.Large, ChallengeRatingConstants.CR6, null, false, 0, 8, 0, true),
                [CreatureConstants.Xill] = GetCreatureData(CreatureConstants.Xill, SizeConstants.Medium, ChallengeRatingConstants.CR6, 4, true, 0, 7, 4, false),
                [CreatureConstants.Xorn_Average] = GetCreatureData(CreatureConstants.Xorn_Average, SizeConstants.Medium, ChallengeRatingConstants.CR6, null, false, 0, 14, 0, false),
                [CreatureConstants.Xorn_Elder] = GetCreatureData(CreatureConstants.Xorn_Elder, SizeConstants.Large, ChallengeRatingConstants.CR8, null, false, 0, 16, 0, false),
                [CreatureConstants.Xorn_Minor] = GetCreatureData(CreatureConstants.Xorn_Minor, SizeConstants.Small, ChallengeRatingConstants.CR3, null, false, 0, 12, 0, false),
                [CreatureConstants.YethHound] = GetCreatureData(CreatureConstants.YethHound, SizeConstants.Medium, ChallengeRatingConstants.CR3, 3, false, 0, 8, 0, false),
                [CreatureConstants.Yrthak] = GetCreatureData(CreatureConstants.Yrthak, SizeConstants.Huge, ChallengeRatingConstants.CR9, null, false, 0, 8, 0, true),
                [CreatureConstants.YuanTi_Abomination] = GetCreatureData(CreatureConstants.YuanTi_Abomination, SizeConstants.Large, ChallengeRatingConstants.CR7, 7, true, 10, 10, 2, true),
                [CreatureConstants.YuanTi_Halfblood_SnakeHead] = GetCreatureData(CreatureConstants.YuanTi_Halfblood_SnakeHead, SizeConstants.Medium, ChallengeRatingConstants.CR5, 5, true, 4, 4, 2, true),
                [CreatureConstants.YuanTi_Halfblood_SnakeArms] = GetCreatureData(CreatureConstants.YuanTi_Halfblood_SnakeArms, SizeConstants.Medium, ChallengeRatingConstants.CR5, 5, true, 4, 4, 0, true),
                [CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs] = GetCreatureData(CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs, SizeConstants.Medium, ChallengeRatingConstants.CR5, 5, true, 4, 4, 2, true),
                [CreatureConstants.YuanTi_Halfblood_SnakeTail] = GetCreatureData(CreatureConstants.YuanTi_Halfblood_SnakeTail, SizeConstants.Medium, ChallengeRatingConstants.CR5, 5, true, 4, 4, 2, true),
                [CreatureConstants.YuanTi_Pureblood] = GetCreatureData(CreatureConstants.YuanTi_Pureblood, SizeConstants.Medium, ChallengeRatingConstants.CR3, 2, true, 4, 1, 2, true),
                [CreatureConstants.Zelekhut] = GetCreatureData(CreatureConstants.Zelekhut, SizeConstants.Large, ChallengeRatingConstants.CR9, 7, true, 8, 10, 2, false)
            };

            return data;
        }

        private string GetCreatureData(
            string creature,
            string size,
            string challengeRating,
            int? levelAdjustment,
            bool canUseEquipment,
            int casterLevel,
            int naturalArmor,
            int numberOfHands,
            bool hasSkeleton,
            double? space = null,
            double? reach = null)
        {
            var creatureType = creatureTypes[creature].First();
            var selection = new CreatureDataSelection
            {
                ChallengeRating = challengeRating,
                LevelAdjustment = levelAdjustment,
                Reach = reach ?? spaceReachHelper.GetDefaultReach(creature, size),
                Size = size,
                Space = space ?? spaceReachHelper.GetDefaultSpace(size),
                CanUseEquipment = canUseEquipment,
                NaturalArmor = naturalArmor,
                NumberOfHands = numberOfHands,
                CasterLevel = casterLevel,
                Types = creatureTypes[creature],
                BaseAttackQuality = baseAttackQualities[creatureType],
                HitDiceQuantity = creatureHitDiceQuantities[creature],
                HitDie = hitDies[creatureType],
                HasSkeleton = hasSkeleton,
            };
            var data = DataHelper.Parse(selection);

            return data;
        }

        [TestCase(CreatureConstants.Types.Animal, false)]
        [TestCase(CreatureConstants.Types.Fey, true)]
        [TestCase(CreatureConstants.Types.Giant, true)]
        [TestCase(CreatureConstants.Types.Humanoid, true)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid, true)]
        [TestCase(CreatureConstants.Types.Ooze, false)]
        [TestCase(CreatureConstants.Types.Vermin, false)]
        public void CreaturesOfTypeCanUseEquipment(string creatureType, bool useEquipment)
        {
            var creatures = creatureTypes
                .Where(kvp => kvp.Value.Contains(creatureType))
                .Select(kvp => kvp.Key);

            foreach (var creature in creatures)
            {
                Assert.That(creatureData, Contains.Key(creature));

                var data = DataHelper.Parse<CreatureDataSelection>(creatureData[creature]);
                Assert.That(data.CanUseEquipment, Is.EqualTo(useEquipment), $"TEST DATA: {creature}");
            }
        }

        [TestCase(CreatureConstants.Types.Construct)]
        [TestCase(CreatureConstants.Types.Elemental)]
        [TestCase(CreatureConstants.Types.Ooze)]
        [TestCase(CreatureConstants.Types.Plant)]
        [TestCase(CreatureConstants.Types.Subtypes.Incorporeal)]
        public void TypeDoesNotHaveSkeleton(string creatureType)
        {
            var creaturesOfType = creatureTypes
                .Where(kvp => kvp.Value.Contains(creatureType))
                .Select(kvp => kvp.Key);

            var bonelessCreatures = creatureData
                .Where(kvp => !DataHelper.Parse<CreatureDataSelection>(kvp.Value).HasSkeleton)
                .Select(kvp => kvp.Key);

            Assert.That(creaturesOfType, Is.SubsetOf(bonelessCreatures));
        }

        [Test]
        public void NonNativeOutsidersDoNotHaveSkeleton()
        {
            var creaturesOfType = creatureTypes
                .Where(kvp => kvp.Value.Contains(CreatureConstants.Types.Outsider) && !kvp.Value.Contains(CreatureConstants.Types.Subtypes.Native))
                .Select(kvp => kvp.Key);

            var bonelessCreatures = creatureData
                .Where(kvp => !DataHelper.Parse<CreatureDataSelection>(kvp.Value).HasSkeleton)
                .Select(kvp => kvp.Key);

            Assert.That(creaturesOfType, Is.SubsetOf(bonelessCreatures));
        }

        [TestCase(CreatureConstants.Types.Dragon)]
        [TestCase(CreatureConstants.Types.Fey)]
        [TestCase(CreatureConstants.Types.Giant)]
        [TestCase(CreatureConstants.Types.Humanoid)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid)]
        [TestCase(CreatureConstants.Types.Subtypes.Native)]
        public void TypeHasSkeleton(string creatureType)
        {
            var creaturesOfType = creatureTypes
                .Where(kvp => kvp.Value.Contains(creatureType))
                .Select(kvp => kvp.Key);

            var skeletalCreatures = creatureData
                .Where(kvp => DataHelper.Parse<CreatureDataSelection>(kvp.Value).HasSkeleton)
                .Select(kvp => kvp.Key);

            Assert.That(creaturesOfType, Is.SubsetOf(skeletalCreatures));
        }

        [TestCase(CreatureConstants.Aasimar, 5, 5)]
        [TestCase(CreatureConstants.Aboleth, 15, 10)]
        [TestCase(CreatureConstants.Achaierai, 10, 10)]
        [TestCase(CreatureConstants.Allip, 5, 5)]
        [TestCase(CreatureConstants.Androsphinx, 10, 5)]
        [TestCase(CreatureConstants.Angel_AstralDeva, 5, 5)]
        [TestCase(CreatureConstants.Angel_Planetar, 10, 10)]
        [TestCase(CreatureConstants.Angel_Solar, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal, 30, 30)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Flexible, 30, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long, 30, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden, 30, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall, 30, 30)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden, 30, 30)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Sheetlike, 30, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_TwoLegs, 30, 30)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden, 30, 30)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden, 30, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wooden, 30, 30)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan, 20, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Flexible, 20, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long, 20, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden, 20, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall, 20, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden, 20, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Sheetlike, 20, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs, 20, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden, 20, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden, 20, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wooden, 20, 20)]
        [TestCase(CreatureConstants.AnimatedObject_Huge, 15, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Flexible, 15, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long, 15, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden, 15, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall, 15, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden, 15, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Sheetlike, 15, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_TwoLegs, 15, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden, 15, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wheels_Wooden, 15, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wooden, 15, 15)]
        [TestCase(CreatureConstants.AnimatedObject_Large, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Flexible, 10, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long, 10, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden, 10, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Sheetlike, 10, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Large_TwoLegs, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wheels_Wooden, 10, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wooden, 10, 10)]
        [TestCase(CreatureConstants.AnimatedObject_Medium, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Flexible, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_MultipleLegs, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Sheetlike, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_TwoLegs, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wheels_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Flexible, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_MultipleLegs, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Sheetlike, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_TwoLegs, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wheels_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wooden, 5, 5)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Flexible, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_MultipleLegs, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Sheetlike, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_TwoLegs, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden, 2.5, 0)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wooden, 2.5, 0)]
        [TestCase(CreatureConstants.Ankheg, 10, 5)]
        [TestCase(CreatureConstants.Annis, 10, 10)]
        [TestCase(CreatureConstants.Ant_Giant_Queen, 10, 5)]
        [TestCase(CreatureConstants.Ant_Giant_Soldier, 5, 5)]
        [TestCase(CreatureConstants.Ant_Giant_Worker, 5, 5)]
        [TestCase(CreatureConstants.Ape, 10, 10)]
        [TestCase(CreatureConstants.Ape_Dire, 10, 10)]
        [TestCase(CreatureConstants.Aranea, 5, 5)]
        [TestCase(CreatureConstants.Arrowhawk_Adult, 5, 5)]
        [TestCase(CreatureConstants.Arrowhawk_Elder, 10, 5)]
        [TestCase(CreatureConstants.Arrowhawk_Juvenile, 5, 5)]
        [TestCase(CreatureConstants.AssassinVine, 10, 10)]
        [TestCase(CreatureConstants.Athach, 15, 15)]
        [TestCase(CreatureConstants.Avoral, 5, 5)]
        [TestCase(CreatureConstants.Azer, 5, 5)]
        [TestCase(CreatureConstants.Babau, 5, 5)]
        [TestCase(CreatureConstants.Baboon, 5, 5)]
        [TestCase(CreatureConstants.Badger, 5, 5)]
        [TestCase(CreatureConstants.Badger_Dire, 5, 5)]
        [TestCase(CreatureConstants.Balor, 10, 10)]
        [TestCase(CreatureConstants.BarbedDevil_Hamatula, 5, 5)]
        [TestCase(CreatureConstants.Barghest, 5, 5)]
        [TestCase(CreatureConstants.Barghest_Greater, 10, 5)]
        [TestCase(CreatureConstants.Basilisk, 5, 5)]
        [TestCase(CreatureConstants.Basilisk_Greater, 10, 5)]
        [TestCase(CreatureConstants.Bat, 1, 0)]
        [TestCase(CreatureConstants.Bat_Dire, 10, 5)]
        [TestCase(CreatureConstants.Bat_Swarm, 10, 0)]
        [TestCase(CreatureConstants.Bear_Black, 5, 5)]
        [TestCase(CreatureConstants.Bear_Brown, 10, 5)]
        [TestCase(CreatureConstants.Bear_Dire, 10, 5)]
        [TestCase(CreatureConstants.Bear_Polar, 10, 5)]
        [TestCase(CreatureConstants.BeardedDevil_Barbazu, 5, 5)]
        [TestCase(CreatureConstants.Bebilith, 15, 10)]
        [TestCase(CreatureConstants.Bee_Giant, 5, 5)]
        [TestCase(CreatureConstants.Behir, 15, 10)]
        [TestCase(CreatureConstants.Beholder, 10, 5)]
        [TestCase(CreatureConstants.Beholder_Gauth, 5, 5)]
        [TestCase(CreatureConstants.Belker, 10, 10)]
        [TestCase(CreatureConstants.Bison, 10, 5)]
        [TestCase(CreatureConstants.BlackPudding, 15, 10)]
        [TestCase(CreatureConstants.BlackPudding_Elder, 20, 20)]
        [TestCase(CreatureConstants.BlinkDog, 5, 5)]
        [TestCase(CreatureConstants.Boar, 5, 5)]
        [TestCase(CreatureConstants.Boar_Dire, 10, 5)]
        [TestCase(CreatureConstants.Bodak, 5, 5)]
        [TestCase(CreatureConstants.BombardierBeetle_Giant, 5, 5)]
        [TestCase(CreatureConstants.BoneDevil_Osyluth, 10, 10)]
        [TestCase(CreatureConstants.Bralani, 5, 5)]
        [TestCase(CreatureConstants.Bugbear, 5, 5)]
        [TestCase(CreatureConstants.Bulette, 15, 10)]
        [TestCase(CreatureConstants.Camel_Bactrian, 10, 5)]
        [TestCase(CreatureConstants.Camel_Dromedary, 10, 5)]
        [TestCase(CreatureConstants.CarrionCrawler, 10, 5)]
        [TestCase(CreatureConstants.Cat, 2.5, 0)]
        [TestCase(CreatureConstants.Centaur, 10, 5)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Colossal, 30, 20)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Gargantuan, 20, 15)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Huge, 15, 10)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Large, 10, 5)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Medium, 5, 5)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Small, 5, 5)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Tiny, 2.5, 0)]
        [TestCase(CreatureConstants.Centipede_Swarm, 10, 0)]
        [TestCase(CreatureConstants.ChainDevil_Kyton, 5, 5)]
        [TestCase(CreatureConstants.ChaosBeast, 5, 5)]
        [TestCase(CreatureConstants.Cheetah, 5, 5)]
        [TestCase(CreatureConstants.Chimera_Black, 10, 5)]
        [TestCase(CreatureConstants.Chimera_Blue, 10, 5)]
        [TestCase(CreatureConstants.Chimera_Green, 10, 5)]
        [TestCase(CreatureConstants.Chimera_Red, 10, 5)]
        [TestCase(CreatureConstants.Chimera_White, 10, 5)]
        [TestCase(CreatureConstants.Choker, 5, 10)]
        [TestCase(CreatureConstants.Chuul, 10, 5)]
        [TestCase(CreatureConstants.Cloaker, 10, 10)]
        [TestCase(CreatureConstants.Cockatrice, 5, 5)]
        [TestCase(CreatureConstants.Couatl, 10, 5)]
        [TestCase(CreatureConstants.Criosphinx, 10, 5)]
        [TestCase(CreatureConstants.Crocodile, 5, 5)]
        [TestCase(CreatureConstants.Crocodile_Giant, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_5Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_6Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_7Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_8Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_9Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_10Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_11Heads, 15, 10)]
        [TestCase(CreatureConstants.Cryohydra_12Heads, 15, 10)]
        [TestCase(CreatureConstants.Darkmantle, 5, 5)]
        [TestCase(CreatureConstants.Deinonychus, 5, 5)]
        [TestCase(CreatureConstants.Delver, 15, 10)]
        [TestCase(CreatureConstants.Derro, 5, 5)]
        [TestCase(CreatureConstants.Derro_Sane, 5, 5)]
        [TestCase(CreatureConstants.Destrachan, 10, 5)]
        [TestCase(CreatureConstants.Devourer, 10, 10)]
        [TestCase(CreatureConstants.Digester, 5, 5)]
        [TestCase(CreatureConstants.DisplacerBeast, 10, 5)]
        [TestCase(CreatureConstants.DisplacerBeast_PackLord, 15, 10)]
        [TestCase(CreatureConstants.Djinni, 10, 10)]
        [TestCase(CreatureConstants.Djinni_Noble, 10, 10)]
        [TestCase(CreatureConstants.Dog, 5, 5)]
        [TestCase(CreatureConstants.Dog_Riding, 5, 5)]
        [TestCase(CreatureConstants.Donkey, 5, 5)]
        [TestCase(CreatureConstants.Doppelganger, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrmling, 2.5, 0)]
        [TestCase(CreatureConstants.Dragon_Black_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Black_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Black_Juvenile, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Black_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Black_Adult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Black_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Black_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Black_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Black_Ancient, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrmling, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Blue_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Blue_Juvenile, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Blue_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Blue_Adult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Blue_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Blue_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Blue_Ancient, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrmling, 2.5, 0)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Brass_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Brass_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrmling, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Bronze_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Bronze_Juvenile, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Bronze_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Bronze_Adult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Bronze_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Bronze_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Bronze_Ancient, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrmling, 2.5, 0)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Copper_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Copper_Juvenile, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Copper_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Copper_Adult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Copper_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Copper_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Copper_Ancient, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrmling, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryYoung, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Gold_Young, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Gold_Juvenile, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Gold_YoungAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Gold_Adult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Gold_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Gold_Old, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryOld, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Gold_Ancient, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrm, 30, 20)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm, 30, 20)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrmling, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Green_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Green_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Green_Juvenile, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Green_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Green_Adult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Green_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Green_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Green_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Green_Ancient, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrmling, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Red_VeryYoung, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Red_Young, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Red_Juvenile, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Red_YoungAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Red_Adult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Red_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Red_Old, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Red_VeryOld, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Red_Ancient, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm, 30, 20)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrmling, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Silver_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_Silver_Juvenile, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Silver_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_Silver_Adult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Silver_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Silver_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_Silver_Ancient, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm, 30, 20)]
        [TestCase(CreatureConstants.Dragon_White_Wyrmling, 2.5, 0)]
        [TestCase(CreatureConstants.Dragon_White_VeryYoung, 5, 5)]
        [TestCase(CreatureConstants.Dragon_White_Young, 5, 5)]
        [TestCase(CreatureConstants.Dragon_White_Juvenile, 5, 5)]
        [TestCase(CreatureConstants.Dragon_White_YoungAdult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_White_Adult, 10, 5)]
        [TestCase(CreatureConstants.Dragon_White_MatureAdult, 15, 10)]
        [TestCase(CreatureConstants.Dragon_White_Old, 15, 10)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld, 15, 10)]
        [TestCase(CreatureConstants.Dragon_White_Ancient, 15, 10)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm, 20, 15)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm, 20, 15)]
        [TestCase(CreatureConstants.DragonTurtle, 15, 10)]
        [TestCase(CreatureConstants.Dragonne, 10, 5)]
        [TestCase(CreatureConstants.Dretch, 5, 5)]
        [TestCase(CreatureConstants.Drider, 10, 5)]
        [TestCase(CreatureConstants.Dryad, 5, 5)]
        [TestCase(CreatureConstants.Dwarf_Deep, 5, 5)]
        [TestCase(CreatureConstants.Dwarf_Duergar, 5, 5)]
        [TestCase(CreatureConstants.Dwarf_Hill, 5, 5)]
        [TestCase(CreatureConstants.Dwarf_Mountain, 5, 5)]
        [TestCase(CreatureConstants.Eagle, 5, 5)]
        [TestCase(CreatureConstants.Eagle_Giant, 10, 5)]
        [TestCase(CreatureConstants.Efreeti, 10, 10)]
        [TestCase(CreatureConstants.Elasmosaurus, 15, 10)]
        [TestCase(CreatureConstants.Elemental_Air_Elder, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Air_Greater, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Air_Huge, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Air_Large, 10, 10)]
        [TestCase(CreatureConstants.Elemental_Air_Medium, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Air_Small, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Earth_Elder, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Earth_Greater, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Earth_Huge, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Earth_Large, 10, 10)]
        [TestCase(CreatureConstants.Elemental_Earth_Medium, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Earth_Small, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Fire_Elder, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Fire_Greater, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Fire_Huge, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Fire_Large, 10, 10)]
        [TestCase(CreatureConstants.Elemental_Fire_Medium, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Fire_Small, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Water_Elder, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Water_Greater, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Water_Huge, 15, 15)]
        [TestCase(CreatureConstants.Elemental_Water_Large, 10, 10)]
        [TestCase(CreatureConstants.Elemental_Water_Medium, 5, 5)]
        [TestCase(CreatureConstants.Elemental_Water_Small, 5, 5)]
        [TestCase(CreatureConstants.Elephant, 15, 10)]
        [TestCase(CreatureConstants.Elf_Aquatic, 5, 5)]
        [TestCase(CreatureConstants.Elf_Drow, 5, 5)]
        [TestCase(CreatureConstants.Elf_Gray, 5, 5)]
        [TestCase(CreatureConstants.Elf_Half, 5, 5)]
        [TestCase(CreatureConstants.Elf_High, 5, 5)]
        [TestCase(CreatureConstants.Elf_Wild, 5, 5)]
        [TestCase(CreatureConstants.Elf_Wood, 5, 5)]
        [TestCase(CreatureConstants.Erinyes, 5, 5)]
        [TestCase(CreatureConstants.EtherealFilcher, 5, 5)]
        [TestCase(CreatureConstants.EtherealMarauder, 5, 5)]
        [TestCase(CreatureConstants.Ettercap, 5, 5)]
        [TestCase(CreatureConstants.Ettin, 10, 10)]
        [TestCase(CreatureConstants.FireBeetle_Giant, 5, 5)]
        [TestCase(CreatureConstants.FormianMyrmarch, 10, 5)]
        [TestCase(CreatureConstants.FormianQueen, 10, 5)]
        [TestCase(CreatureConstants.FormianTaskmaster, 5, 5)]
        [TestCase(CreatureConstants.FormianWarrior, 5, 5)]
        [TestCase(CreatureConstants.FormianWorker, 5, 5)]
        [TestCase(CreatureConstants.FrostWorm, 15, 10)]
        [TestCase(CreatureConstants.Gargoyle, 5, 5)]
        [TestCase(CreatureConstants.Gargoyle_Kapoacinth, 5, 5)]
        [TestCase(CreatureConstants.GelatinousCube, 10, 5)]
        [TestCase(CreatureConstants.Ghaele, 5, 5)]
        [TestCase(CreatureConstants.Ghoul, 5, 5)]
        [TestCase(CreatureConstants.Ghoul_Ghast, 5, 5)]
        [TestCase(CreatureConstants.Ghoul_Lacedon, 5, 5)]
        [TestCase(CreatureConstants.Giant_Cloud, 15, 15)]
        [TestCase(CreatureConstants.Giant_Fire, 10, 10)]
        [TestCase(CreatureConstants.Giant_Frost, 10, 10)]
        [TestCase(CreatureConstants.Giant_Hill, 10, 10)]
        [TestCase(CreatureConstants.Giant_Stone, 10, 10)]
        [TestCase(CreatureConstants.Giant_Stone_Elder, 10, 10)]
        [TestCase(CreatureConstants.Giant_Storm, 15, 15)]
        [TestCase(CreatureConstants.GibberingMouther, 5, 5)]
        [TestCase(CreatureConstants.Girallon, 10, 10)]
        [TestCase(CreatureConstants.Githyanki, 5, 5)]
        [TestCase(CreatureConstants.Githzerai, 5, 5)]
        [TestCase(CreatureConstants.Glabrezu, 15, 15)]
        [TestCase(CreatureConstants.Gnoll, 5, 5)]
        [TestCase(CreatureConstants.Gnome_Forest, 5, 5)]
        [TestCase(CreatureConstants.Gnome_Rock, 5, 5)]
        [TestCase(CreatureConstants.Gnome_Svirfneblin, 5, 5)]
        [TestCase(CreatureConstants.Goblin, 5, 5)]
        [TestCase(CreatureConstants.Golem_Clay, 10, 10)]
        [TestCase(CreatureConstants.Golem_Flesh, 10, 10)]
        [TestCase(CreatureConstants.Golem_Iron, 10, 10)]
        [TestCase(CreatureConstants.Golem_Stone, 10, 10)]
        [TestCase(CreatureConstants.Golem_Stone_Greater, 15, 15)]
        [TestCase(CreatureConstants.Gorgon, 10, 5)]
        [TestCase(CreatureConstants.GrayOoze, 5, 5)]
        [TestCase(CreatureConstants.GrayRender, 10, 10)]
        [TestCase(CreatureConstants.GreenHag, 5, 5)]
        [TestCase(CreatureConstants.Grick, 5, 5)]
        [TestCase(CreatureConstants.Grig, 2.5, 0)]
        [TestCase(CreatureConstants.Grig_WithFiddle, 2.5, 0)]
        [TestCase(CreatureConstants.Griffon, 10, 5)]
        [TestCase(CreatureConstants.Grimlock, 5, 5)]
        [TestCase(CreatureConstants.Gynosphinx, 10, 5)]
        [TestCase(CreatureConstants.Halfling_Deep, 5, 5)]
        [TestCase(CreatureConstants.Halfling_Lightfoot, 5, 5)]
        [TestCase(CreatureConstants.Halfling_Tallfellow, 5, 5)]
        [TestCase(CreatureConstants.Harpy, 5, 5)]
        [TestCase(CreatureConstants.Hawk, 2.5, 0)]
        [TestCase(CreatureConstants.Hellcat_Bezekira, 10, 5)]
        [TestCase(CreatureConstants.Hellwasp_Swarm, 10, 0)]
        [TestCase(CreatureConstants.HellHound, 5, 5)]
        [TestCase(CreatureConstants.HellHound_NessianWarhound, 10, 10)]
        [TestCase(CreatureConstants.Hezrou, 10, 10)]
        [TestCase(CreatureConstants.Hieracosphinx, 10, 5)]
        [TestCase(CreatureConstants.Hippogriff, 10, 5)]
        [TestCase(CreatureConstants.Hobgoblin, 5, 5)]
        [TestCase(CreatureConstants.Homunculus, 2.5, 0)]
        [TestCase(CreatureConstants.HornedDevil_Cornugon, 10, 10)]
        [TestCase(CreatureConstants.Horse_Heavy, 10, 5)]
        [TestCase(CreatureConstants.Horse_Heavy_War, 10, 5)]
        [TestCase(CreatureConstants.Horse_Light, 10, 5)]
        [TestCase(CreatureConstants.Horse_Light_War, 10, 5)]
        [TestCase(CreatureConstants.HoundArchon, 5, 5)]
        [TestCase(CreatureConstants.Howler, 10, 5)]
        [TestCase(CreatureConstants.Human, 5, 5)]
        [TestCase(CreatureConstants.Hydra_5Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_6Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_7Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_8Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_9Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_10Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_11Heads, 15, 10)]
        [TestCase(CreatureConstants.Hydra_12Heads, 15, 10)]
        [TestCase(CreatureConstants.Hyena, 5, 5)]
        [TestCase(CreatureConstants.IceDevil_Gelugon, 10, 10)]
        [TestCase(CreatureConstants.Imp, 2.5, 0)]
        [TestCase(CreatureConstants.InvisibleStalker, 10, 10)]
        [TestCase(CreatureConstants.Janni, 5, 5)]
        [TestCase(CreatureConstants.Kobold, 5, 5)]
        [TestCase(CreatureConstants.Kolyarut, 5, 5)]
        [TestCase(CreatureConstants.Kraken, 20, 15)]
        [TestCase(CreatureConstants.Krenshar, 5, 5)]
        [TestCase(CreatureConstants.KuoToa, 5, 5)]
        [TestCase(CreatureConstants.Lamia, 10, 5)]
        [TestCase(CreatureConstants.Lammasu, 10, 5)]
        [TestCase(CreatureConstants.LanternArchon, 5, 5)]
        [TestCase(CreatureConstants.Lemure, 5, 5)]
        [TestCase(CreatureConstants.Leonal, 5, 5)]
        [TestCase(CreatureConstants.Leopard, 5, 5)]
        [TestCase(CreatureConstants.Lillend, 10, 10)]
        [TestCase(CreatureConstants.Lion, 10, 5)]
        [TestCase(CreatureConstants.Lion_Dire, 10, 5)]
        [TestCase(CreatureConstants.Lizard, 2.5, 0)]
        [TestCase(CreatureConstants.Lizard_Monitor, 5, 5)]
        [TestCase(CreatureConstants.Lizardfolk, 5, 5)]
        [TestCase(CreatureConstants.Locathah, 5, 5)]
        [TestCase(CreatureConstants.Locust_Swarm, 10, 0)]
        [TestCase(CreatureConstants.Magmin, 5, 5)]
        [TestCase(CreatureConstants.MantaRay, 10, 5)]
        [TestCase(CreatureConstants.Manticore, 10, 5)]
        [TestCase(CreatureConstants.Marilith, 10, 10)]
        [TestCase(CreatureConstants.Marut, 10, 10)]
        [TestCase(CreatureConstants.Medusa, 5, 5)]
        [TestCase(CreatureConstants.Megaraptor, 10, 5)]
        [TestCase(CreatureConstants.Mephit_Air, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Dust, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Earth, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Fire, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Ice, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Magma, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Ooze, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Salt, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Steam, 5, 5)]
        [TestCase(CreatureConstants.Mephit_Water, 5, 5)]
        [TestCase(CreatureConstants.Merfolk, 5, 5)]
        [TestCase(CreatureConstants.Mimic, 10, 10)]
        [TestCase(CreatureConstants.MindFlayer, 5, 5)]
        [TestCase(CreatureConstants.Minotaur, 10, 10)]
        [TestCase(CreatureConstants.Mohrg, 5, 5)]
        [TestCase(CreatureConstants.Monkey, 2.5, 0)]
        [TestCase(CreatureConstants.Mule, 10, 5)]
        [TestCase(CreatureConstants.Mummy, 5, 5)]
        [TestCase(CreatureConstants.Naga_Dark, 10, 5)]
        [TestCase(CreatureConstants.Naga_Guardian, 10, 5)]
        [TestCase(CreatureConstants.Naga_Spirit, 10, 5)]
        [TestCase(CreatureConstants.Naga_Water, 10, 5)]
        [TestCase(CreatureConstants.Nalfeshnee, 15, 15)]
        [TestCase(CreatureConstants.NightHag, 5, 5)]
        [TestCase(CreatureConstants.Nightcrawler, 20, 15)]
        [TestCase(CreatureConstants.Nightmare, 10, 5)]
        [TestCase(CreatureConstants.Nightmare_Cauchemar, 15, 10)]
        [TestCase(CreatureConstants.Nightwalker, 15, 15)]
        [TestCase(CreatureConstants.Nightwing, 15, 10)]
        [TestCase(CreatureConstants.Nixie, 5, 5)]
        [TestCase(CreatureConstants.Nymph, 5, 5)]
        [TestCase(CreatureConstants.OchreJelly, 10, 5)]
        [TestCase(CreatureConstants.Octopus, 5, 5)]
        [TestCase(CreatureConstants.Octopus_Giant, 10, 10)]
        [TestCase(CreatureConstants.Ogre, 10, 10)]
        [TestCase(CreatureConstants.Ogre_Merrow, 10, 10)]
        [TestCase(CreatureConstants.OgreMage, 10, 10)]
        [TestCase(CreatureConstants.Orc, 5, 5)]
        [TestCase(CreatureConstants.Orc_Half, 5, 5)]
        [TestCase(CreatureConstants.Otyugh, 10, 10)]
        [TestCase(CreatureConstants.Owl, 2.5, 0)]
        [TestCase(CreatureConstants.Owl_Giant, 10, 5)]
        [TestCase(CreatureConstants.Owlbear, 10, 5)]
        [TestCase(CreatureConstants.Pegasus, 10, 5)]
        [TestCase(CreatureConstants.PhantomFungus, 5, 5)]
        [TestCase(CreatureConstants.PhaseSpider, 10, 5)]
        [TestCase(CreatureConstants.Phasm, 5, 5)]
        [TestCase(CreatureConstants.PitFiend, 10, 10)]
        [TestCase(CreatureConstants.Pixie, 5, 5)]
        [TestCase(CreatureConstants.Pixie_WithIrresistibleDance, 5, 5)]
        [TestCase(CreatureConstants.Pony, 5, 5)]
        [TestCase(CreatureConstants.Pony_War, 5, 5)]
        [TestCase(CreatureConstants.Porpoise, 5, 5)]
        [TestCase(CreatureConstants.PrayingMantis_Giant, 10, 5)]
        [TestCase(CreatureConstants.Pseudodragon, 2.5, 0)]
        [TestCase(CreatureConstants.PurpleWorm, 20, 15)]
        [TestCase(CreatureConstants.Pyrohydra_5Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_6Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_7Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_8Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_9Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_10Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_11Heads, 15, 10)]
        [TestCase(CreatureConstants.Pyrohydra_12Heads, 15, 10)]
        [TestCase(CreatureConstants.Quasit, 2.5, 0)]
        [TestCase(CreatureConstants.Rakshasa, 5, 5)]
        [TestCase(CreatureConstants.Rast, 5, 5)]
        [TestCase(CreatureConstants.Rat, 2.5, 0)]
        [TestCase(CreatureConstants.Rat_Dire, 5, 5)]
        [TestCase(CreatureConstants.Rat_Swarm, 10, 0)]
        [TestCase(CreatureConstants.Raven, 2.5, 0)]
        [TestCase(CreatureConstants.Ravid, 5, 5)]
        [TestCase(CreatureConstants.RazorBoar, 10, 5)]
        [TestCase(CreatureConstants.Remorhaz, 15, 10)]
        [TestCase(CreatureConstants.Retriever, 15, 10)]
        [TestCase(CreatureConstants.Rhinoceras, 10, 5)]
        [TestCase(CreatureConstants.Roc, 20, 15)]
        [TestCase(CreatureConstants.Roper, 10, 10)]
        [TestCase(CreatureConstants.RustMonster, 5, 5)]
        [TestCase(CreatureConstants.Sahuagin, 5, 5)]
        [TestCase(CreatureConstants.Sahuagin_Mutant, 5, 5)]
        [TestCase(CreatureConstants.Sahuagin_Malenti, 5, 5)]
        [TestCase(CreatureConstants.Salamander_Average, 5, 5)]
        [TestCase(CreatureConstants.Salamander_Flamebrother, 5, 5)]
        [TestCase(CreatureConstants.Salamander_Noble, 10, 10)]
        [TestCase(CreatureConstants.Satyr, 5, 5)]
        [TestCase(CreatureConstants.Satyr_WithPipes, 5, 5)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Colossal, 40, 30)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Gargantuan, 20, 15)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Huge, 15, 10)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Large, 10, 5)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Medium, 5, 5)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Small, 5, 5)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Tiny, 2.5, 0)]
        [TestCase(CreatureConstants.Scorpionfolk, 10, 5)]
        [TestCase(CreatureConstants.SeaCat, 10, 5)]
        [TestCase(CreatureConstants.SeaHag, 5, 5)]
        [TestCase(CreatureConstants.Shadow, 5, 5)]
        [TestCase(CreatureConstants.Shadow_Greater, 5, 5)]
        [TestCase(CreatureConstants.ShadowMastiff, 5, 5)]
        [TestCase(CreatureConstants.ShamblingMound, 10, 10)]
        [TestCase(CreatureConstants.Shark_Dire, 15, 10)]
        [TestCase(CreatureConstants.Shark_Huge, 15, 10)]
        [TestCase(CreatureConstants.Shark_Large, 10, 5)]
        [TestCase(CreatureConstants.Shark_Medium, 5, 5)]
        [TestCase(CreatureConstants.ShieldGuardian, 10, 10)]
        [TestCase(CreatureConstants.ShockerLizard, 5, 5)]
        [TestCase(CreatureConstants.Shrieker, 5, 0)]
        [TestCase(CreatureConstants.Skum, 5, 5)]
        [TestCase(CreatureConstants.Slaad_Blue, 10, 10)]
        [TestCase(CreatureConstants.Slaad_Death, 5, 5)]
        [TestCase(CreatureConstants.Slaad_Gray, 5, 5)]
        [TestCase(CreatureConstants.Slaad_Green, 10, 10)]
        [TestCase(CreatureConstants.Slaad_Red, 10, 10)]
        [TestCase(CreatureConstants.Snake_Constrictor, 5, 5)]
        [TestCase(CreatureConstants.Snake_Constrictor_Giant, 15, 10)]
        [TestCase(CreatureConstants.Snake_Viper_Huge, 15, 10)]
        [TestCase(CreatureConstants.Snake_Viper_Large, 10, 5)]
        [TestCase(CreatureConstants.Snake_Viper_Medium, 5, 5)]
        [TestCase(CreatureConstants.Snake_Viper_Small, 5, 5)]
        [TestCase(CreatureConstants.Snake_Viper_Tiny, 2.5, 0)]
        [TestCase(CreatureConstants.Spectre, 5, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Colossal, 40, 30)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Gargantuan, 20, 15)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Huge, 15, 10)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Large, 10, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Medium, 5, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Small, 5, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Tiny, 2.5, 0)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Colossal, 40, 30)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan, 20, 15)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Huge, 15, 10)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Large, 10, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Medium, 5, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Small, 5, 5)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Tiny, 2.5, 0)]
        [TestCase(CreatureConstants.SpiderEater, 10, 5)]
        [TestCase(CreatureConstants.Spider_Swarm, 10, 0)]
        [TestCase(CreatureConstants.Squid, 5, 5)]
        [TestCase(CreatureConstants.Squid_Giant, 15, 15)]
        [TestCase(CreatureConstants.StagBeetle_Giant, 10, 5)]
        [TestCase(CreatureConstants.Stirge, 2.5, 0)]
        [TestCase(CreatureConstants.Succubus, 5, 5)]
        [TestCase(CreatureConstants.Tarrasque, 30, 20)]
        [TestCase(CreatureConstants.Tendriculos, 15, 15)]
        [TestCase(CreatureConstants.Thoqqua, 5, 5)]
        [TestCase(CreatureConstants.Tiefling, 5, 5)]
        [TestCase(CreatureConstants.Tiger, 10, 5)]
        [TestCase(CreatureConstants.Tiger_Dire, 10, 5)]
        [TestCase(CreatureConstants.Titan, 15, 15)]
        [TestCase(CreatureConstants.Toad, 1, 0)]
        [TestCase(CreatureConstants.Tojanida_Adult, 5, 5)]
        [TestCase(CreatureConstants.Tojanida_Elder, 10, 5)]
        [TestCase(CreatureConstants.Tojanida_Juvenile, 5, 5)]
        [TestCase(CreatureConstants.Treant, 15, 15)]
        [TestCase(CreatureConstants.Triceratops, 15, 10)]
        [TestCase(CreatureConstants.Triton, 5, 5)]
        [TestCase(CreatureConstants.Troglodyte, 5, 5)]
        [TestCase(CreatureConstants.Troll, 10, 10)]
        [TestCase(CreatureConstants.Troll_Scrag, 10, 10)]
        [TestCase(CreatureConstants.TrumpetArchon, 5, 5)]
        [TestCase(CreatureConstants.Tyrannosaurus, 15, 10)]
        [TestCase(CreatureConstants.UmberHulk, 10, 10)]
        [TestCase(CreatureConstants.UmberHulk_TrulyHorrid, 15, 15)]
        [TestCase(CreatureConstants.Unicorn, 10, 5)]
        [TestCase(CreatureConstants.VampireSpawn, 5, 5)]
        [TestCase(CreatureConstants.Vargouille, 5, 5)]
        [TestCase(CreatureConstants.VioletFungus, 5, 10)]
        [TestCase(CreatureConstants.Vrock, 10, 10)]
        [TestCase(CreatureConstants.Wasp_Giant, 10, 5)]
        [TestCase(CreatureConstants.Weasel, 2.5, 0)]
        [TestCase(CreatureConstants.Weasel_Dire, 5, 5)]
        [TestCase(CreatureConstants.Whale_Baleen, 20, 15)]
        [TestCase(CreatureConstants.Whale_Cachalot, 20, 15)]
        [TestCase(CreatureConstants.Whale_Orca, 15, 10)]
        [TestCase(CreatureConstants.Wight, 5, 5)]
        [TestCase(CreatureConstants.WillOWisp, 5, 5)]
        [TestCase(CreatureConstants.WinterWolf, 10, 5)]
        [TestCase(CreatureConstants.Wolf, 5, 5)]
        [TestCase(CreatureConstants.Wolf_Dire, 10, 5)]
        [TestCase(CreatureConstants.Wolverine, 5, 5)]
        [TestCase(CreatureConstants.Wolverine_Dire, 10, 5)]
        [TestCase(CreatureConstants.Worg, 5, 5)]
        [TestCase(CreatureConstants.Wraith, 5, 5)]
        [TestCase(CreatureConstants.Wraith_Dread, 10, 10)]
        [TestCase(CreatureConstants.Wyvern, 10, 5)]
        [TestCase(CreatureConstants.Xill, 5, 5)]
        [TestCase(CreatureConstants.Xorn_Average, 5, 5)]
        [TestCase(CreatureConstants.Xorn_Elder, 10, 10)]
        [TestCase(CreatureConstants.Xorn_Minor, 5, 5)]
        [TestCase(CreatureConstants.YethHound, 5, 5)]
        [TestCase(CreatureConstants.Yrthak, 15, 10)]
        [TestCase(CreatureConstants.YuanTi_Abomination, 10, 10)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeHead, 5, 5)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeArms, 5, 5)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs, 5, 5)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTail, 5, 5)]
        [TestCase(CreatureConstants.YuanTi_Pureblood, 5, 5)]
        [TestCase(CreatureConstants.Zelekhut, 10, 10)]
        public void SpaceAndReachMatchOfficialCreatureData(string creature, double officialSpace, double officialReach)
        {
            var data = DataHelper.Parse<CreatureDataSelection>(creatureData[creature]);
            Assert.That(data.Space, Is.EqualTo(officialSpace));
            Assert.That(data.Reach, Is.EqualTo(officialReach));
        }
    }
}
