using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.Tables.Helpers;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Helpers;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures
{
    [TestFixture]
    public class AdvancementsTests : CollectionTests
    {
        private Dice dice;
        private Dictionary<string, string[]> advancements;
        private Dictionary<string, int> typeDivisors;
        private string[] sizes;
        private Dictionary<string, CreatureDataSelection> creatureData;
        private SpaceReachHelper spaceReachHelper;

        protected override string TableName => TableNameConstants.Collection.Advancements;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            sizes = SizeConstants.GetOrdered();
            typeDivisors = new()
            {
                [CreatureConstants.Types.Aberration] = 4,
                [CreatureConstants.Types.Animal] = 3,
                [CreatureConstants.Types.Construct] = 4,
                [CreatureConstants.Types.Dragon] = 2,
                [CreatureConstants.Types.Elemental] = 4,
                [CreatureConstants.Types.Fey] = 4,
                [CreatureConstants.Types.Giant] = 4,
                [CreatureConstants.Types.Humanoid] = 4,
                [CreatureConstants.Types.MagicalBeast] = 3,
                [CreatureConstants.Types.MonstrousHumanoid] = 3,
                [CreatureConstants.Types.Ooze] = 4,
                [CreatureConstants.Types.Outsider] = 2,
                [CreatureConstants.Types.Plant] = 4,
                [CreatureConstants.Types.Undead] = 4,
                [CreatureConstants.Types.Vermin] = 4,
            };
            var creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
            creatureData = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Single());

            spaceReachHelper = GetNewInstanceOf<SpaceReachHelper>();
            advancements = GetAdvancementsTestData();
        }

        [SetUp]
        public void Setup()
        {
            dice = GetNewInstanceOf<Dice>();
        }

        [Test]
        public void AdvancementsNames()
        {
            var creatures = CreatureConstants.GetAll();
            AssertCollectionNames(creatures);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void Advancements(string creature)
        {
            Assert.That(advancements, Contains.Key(creature));

            AssertHitDieOnlyIncreases(creature);
            AssertSizeOnlyIncreases(creature);
            AssertDragonSizesStayTheSame(creature);
            AssertCollection(creature, advancements[creature]);
        }

        private void AssertHitDieOnlyIncreases(string creature)
        {
            if (advancements[creature].Length == 0)
                return;

            var rolls = advancements[creature]
                .Select(DataHelper.Parse<AdvancementDataSelection>)
                .Select(s => s.AdditionalHitDiceRoll);

            foreach (var roll in rolls)
            {
                var minimum = dice.Roll(roll).AsPotentialMinimum();
                Assert.That(minimum, Is.Positive, roll);

                var maximum = dice.Roll(roll).AsPotentialMaximum();

                foreach (var otherRoll in rolls.Except([roll]))
                {
                    var otherMinimum = dice.Roll(otherRoll).AsPotentialMinimum();
                    var otherMaximum = dice.Roll(otherRoll).AsPotentialMaximum();

                    Assert.That(otherMaximum, Is.Not.EqualTo(minimum).And.Not.EqualTo(maximum), $"{roll} vs {otherRoll}");
                    Assert.That(otherMinimum, Is.Not.EqualTo(minimum).And.Not.EqualTo(maximum), $"{roll} vs {otherRoll}");

                    if (otherMinimum < maximum)
                        Assert.That(otherMaximum, Is.LessThan(minimum), $"{roll} vs {otherRoll}");
                    else if (otherMaximum > minimum)
                        Assert.That(otherMinimum, Is.GreaterThan(maximum), $"{roll} vs {otherRoll}");
                }
            }
        }

        private void AssertSizeOnlyIncreases(string creature)
        {
            if (advancements[creature].Length == 0)
                return;

            var sizes = advancements[creature]
                .Select(DataHelper.Parse<AdvancementDataSelection>)
                .Select(s => s.Size);
            Assert.That(sizes, Is.Unique);

            var orderedSizes = SizeConstants.GetOrdered();
            var originalSizeIndex = Array.IndexOf(orderedSizes, creatureData[creature].Size);

            foreach (var size in sizes)
            {
                var sizeIndex = Array.IndexOf(orderedSizes, size);
                Assert.That(sizeIndex, Is.AtLeast(originalSizeIndex), $"{size} >= {creatureData[creature].Size}");
            }

            foreach (var advancementData in advancements[creature])
            {
                var advancement = DataHelper.Parse<AdvancementDataSelection>(advancementData);
                var size = advancement.Size;
                var sizeIndex = Array.IndexOf(orderedSizes, size);

                var roll = advancement.AdditionalHitDiceRoll;
                var minimum = dice.Roll(roll).AsPotentialMinimum();

                foreach (var otherAdvancementData in advancements[creature].Except([advancementData]))
                {
                    var otherAdvancement = DataHelper.Parse<AdvancementDataSelection>(otherAdvancementData);
                    var otherSize = otherAdvancement.Size;
                    var otherSizeIndex = Array.IndexOf(orderedSizes, otherSize);

                    var otherRoll = otherAdvancement.AdditionalHitDiceRoll;
                    var otherMinimum = dice.Roll(otherRoll).AsPotentialMinimum();

                    if (minimum < otherMinimum)
                        Assert.That(sizeIndex, Is.LessThan(otherSizeIndex), $"{size} < {otherSize}");
                    else
                        Assert.That(sizeIndex, Is.GreaterThan(otherSizeIndex), $"{size} > {otherSize}");
                }
            }
        }

        private void AssertDragonSizesStayTheSame(string creature)
        {
            if (advancements[creature].Length == 0)
                return;

            if (!creature.Contains("Dragon,"))
                return;

            var advancedSizes = advancements[creature]
                .Select(DataHelper.Parse<AdvancementDataSelection>)
                .Select(s => s.Size);
            Assert.That(advancedSizes, Is.Unique.And.EqualTo(new[] { creatureData[creature].Size }));
        }

        private Dictionary<string, string[]> GetAdvancementsTestData()
        {
            var testCases = new Dictionary<string, string[]>();
            var creatures = CreatureConstants.GetAll();

            foreach (var creature in creatures)
            {
                testCases[creature] = [];
            }

            testCases[CreatureConstants.Aboleth] =
            [
                GetData(CreatureConstants.Aboleth, SizeConstants.Huge, 9, 16),
                GetData(CreatureConstants.Aboleth, SizeConstants.Gargantuan, 17, 24),
            ];
            testCases[CreatureConstants.Achaierai] =
            [
                GetData(CreatureConstants.Achaierai, SizeConstants.Large, 7, 12),
                GetData(CreatureConstants.Achaierai, SizeConstants.Huge, 13, 18),
            ];
            testCases[CreatureConstants.Allip] =
            [
                GetData(CreatureConstants.Allip, SizeConstants.Medium, 5, 12),
            ];
            testCases[CreatureConstants.Androsphinx] =
            [
                GetData(CreatureConstants.Androsphinx, SizeConstants.Large, 13, 18),
                GetData(CreatureConstants.Androsphinx, SizeConstants.Huge, 19, 36),
            ];
            testCases[CreatureConstants.Angel_AstralDeva] =
            [
                GetData(CreatureConstants.Angel_AstralDeva, SizeConstants.Medium, 13, 18),
                GetData(CreatureConstants.Angel_AstralDeva, SizeConstants.Large, 19, 36),
            ];
            testCases[CreatureConstants.Angel_Planetar] =
            [
                GetData(CreatureConstants.Angel_Planetar, SizeConstants.Large, 15, 21),
                GetData(CreatureConstants.Angel_Planetar, SizeConstants.Huge, 22, 42),
            ];
            testCases[CreatureConstants.Angel_Solar] =
            [
                GetData(CreatureConstants.Angel_Solar, SizeConstants.Large, 23, 33),
                GetData(CreatureConstants.Angel_Solar, SizeConstants.Huge, 34, 66),
            ];
            testCases[CreatureConstants.Ankheg] =
            [
                GetData(CreatureConstants.Ankheg, SizeConstants.Large, 4, 4),
                GetData(CreatureConstants.Ankheg, SizeConstants.Huge, 5, 9),
            ];
            testCases[CreatureConstants.Ant_Giant_Queen] =
            [
                GetData(CreatureConstants.Ant_Giant_Queen, SizeConstants.Large, 5, 6),
                GetData(CreatureConstants.Ant_Giant_Queen, SizeConstants.Huge, 7, 8),
            ];
            testCases[CreatureConstants.Ant_Giant_Soldier] =
            [
                GetData(CreatureConstants.Ant_Giant_Soldier, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.Ant_Giant_Soldier, SizeConstants.Large, 5, 6),
            ];
            testCases[CreatureConstants.Ant_Giant_Worker] =
            [
                GetData(CreatureConstants.Ant_Giant_Worker, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.Ant_Giant_Worker, SizeConstants.Large, 5, 6),
            ];
            testCases[CreatureConstants.Ape] =
            [
                GetData(CreatureConstants.Ape, SizeConstants.Large, 5, 8),
            ];
            testCases[CreatureConstants.Ape_Dire] =
            [
                GetData(CreatureConstants.Ape_Dire, SizeConstants.Large, 6, 15),
            ];
            testCases[CreatureConstants.Arrowhawk_Adult] =
            [
                GetData(CreatureConstants.Arrowhawk_Adult, SizeConstants.Medium, 8, 14),
            ];
            testCases[CreatureConstants.Arrowhawk_Elder] =
            [
                GetData(CreatureConstants.Arrowhawk_Elder, SizeConstants.Large, 16, 24),
            ];
            testCases[CreatureConstants.Arrowhawk_Juvenile] =
            [
                GetData(CreatureConstants.Arrowhawk_Juvenile, SizeConstants.Small, 4, 6),
            ];
            testCases[CreatureConstants.AssassinVine] =
            [
                GetData(CreatureConstants.AssassinVine, SizeConstants.Huge, 5, 16),
                GetData(CreatureConstants.AssassinVine, SizeConstants.Gargantuan,  17, 32),
                GetData(CreatureConstants.AssassinVine, SizeConstants.Colossal, 33, 100),
            ];
            testCases[CreatureConstants.Athach] =
            [
                GetData(CreatureConstants.Athach, SizeConstants.Huge, 15, 28),
            ];
            testCases[CreatureConstants.Avoral] =
            [
                GetData(CreatureConstants.Avoral, SizeConstants.Medium, 8, 14),
                GetData(CreatureConstants.Avoral, SizeConstants.Large, 15, 21),
            ];
            testCases[CreatureConstants.Babau] =
            [
                GetData(CreatureConstants.Babau, SizeConstants.Large, 8, 14),
                GetData(CreatureConstants.Babau, SizeConstants.Huge, 15, 21),
            ];
            testCases[CreatureConstants.Baboon] =
            [
                GetData(CreatureConstants.Baboon, SizeConstants.Medium, 2, 3),
            ];
            testCases[CreatureConstants.Badger] =
            [
                GetData(CreatureConstants.Badger, SizeConstants.Small, 2, 2),
            ];
            testCases[CreatureConstants.Badger_Dire] =
            [
                GetData(CreatureConstants.Badger_Dire, SizeConstants.Large, 4, 9),
            ];
            testCases[CreatureConstants.Balor] =
            [
                GetData(CreatureConstants.Balor, SizeConstants.Large, 21, 30),
                GetData(CreatureConstants.Balor, SizeConstants.Huge, 31, 60),
            ];
            testCases[CreatureConstants.BarbedDevil_Hamatula] =
            [
                GetData(CreatureConstants.BarbedDevil_Hamatula, SizeConstants.Medium, 13, 24),
                GetData(CreatureConstants.BarbedDevil_Hamatula, SizeConstants.Large, 25, 36),
            ];
            testCases[CreatureConstants.Barghest] =
            [
                GetData(CreatureConstants.Barghest, SizeConstants.Medium, 7, 8),
            ];
            testCases[CreatureConstants.Barghest_Greater] =
            [
                GetData(CreatureConstants.Barghest_Greater, SizeConstants.Large, 10, 18),
            ];
            testCases[CreatureConstants.Basilisk] =
            [
                GetData(CreatureConstants.Basilisk, SizeConstants.Medium, 7, 10),
                GetData(CreatureConstants.Basilisk, SizeConstants.Large, 11, 18),
            ];
            testCases[CreatureConstants.Bat_Dire] =
            [
                GetData(CreatureConstants.Bat_Dire, SizeConstants.Large, 5, 12),
            ];
            testCases[CreatureConstants.Bear_Black] =
            [
                GetData(CreatureConstants.Bear_Black, SizeConstants.Medium, 4, 5),
            ];
            testCases[CreatureConstants.Bear_Brown] =
            [
                GetData(CreatureConstants.Bear_Brown, SizeConstants.Large, 7, 10),
            ];
            testCases[CreatureConstants.Bear_Polar] =
            [
                GetData(CreatureConstants.Bear_Polar, SizeConstants.Large, 9, 12),
            ];
            testCases[CreatureConstants.Bear_Dire] =
            [
                GetData(CreatureConstants.Bear_Dire, SizeConstants.Large, 13, 16),
                GetData(CreatureConstants.Bear_Dire, SizeConstants.Huge, 17, 36),
            ];
            testCases[CreatureConstants.BeardedDevil_Barbazu] =
            [
                GetData(CreatureConstants.BeardedDevil_Barbazu, SizeConstants.Medium, 7, 9),
                GetData(CreatureConstants.BeardedDevil_Barbazu, SizeConstants.Large, 10, 18),
            ];
            testCases[CreatureConstants.Bebilith] =
            [
                GetData(CreatureConstants.Bebilith, SizeConstants.Huge, 13, 18),
                GetData(CreatureConstants.Bebilith, SizeConstants.Gargantuan, 19, 36),
            ];
            testCases[CreatureConstants.Bee_Giant] =
            [
                GetData(CreatureConstants.Bee_Giant, SizeConstants.Medium, 4, 6),
                GetData(CreatureConstants.Bee_Giant, SizeConstants.Large, 7, 9),
            ];
            testCases[CreatureConstants.Behir] =
            [
                GetData(CreatureConstants.Behir, SizeConstants.Huge, 10, 13),
                GetData(CreatureConstants.Behir, SizeConstants.Gargantuan, 14, 27),
            ];
            testCases[CreatureConstants.Beholder] =
            [
                GetData(CreatureConstants.Beholder, SizeConstants.Large, 12, 16),
                GetData(CreatureConstants.Beholder, SizeConstants.Huge, 17, 33),
            ];
            testCases[CreatureConstants.Beholder_Gauth] =
            [
                GetData(CreatureConstants.Beholder_Gauth, SizeConstants.Medium, 7, 12),
                GetData(CreatureConstants.Beholder_Gauth, SizeConstants.Large, 13, 18),
            ];
            testCases[CreatureConstants.Belker] =
            [
                GetData(CreatureConstants.Belker, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Belker, SizeConstants.Huge, 11, 21),
            ];
            testCases[CreatureConstants.Bison] =
            [
                GetData(CreatureConstants.Bison, SizeConstants.Large, 6, 7),
            ];
            testCases[CreatureConstants.BlackPudding] =
            [
                GetData(CreatureConstants.BlackPudding, SizeConstants.Huge, 11, 15),
            ];
            testCases[CreatureConstants.BlinkDog] =
            [
                GetData(CreatureConstants.BlinkDog, SizeConstants.Medium, 5, 7),
                GetData(CreatureConstants.BlinkDog, SizeConstants.Large, 8, 12),
            ];
            testCases[CreatureConstants.Boar] =
            [
                GetData(CreatureConstants.Boar, SizeConstants.Medium, 4, 5),
            ];
            testCases[CreatureConstants.Boar_Dire] =
            [
                GetData(CreatureConstants.Boar_Dire, SizeConstants.Large, 8, 16),
                GetData(CreatureConstants.Boar_Dire, SizeConstants.Huge, 17, 21),
            ];
            testCases[CreatureConstants.Bodak] =
            [
                GetData(CreatureConstants.Bodak, SizeConstants.Medium, 10, 13),
                GetData(CreatureConstants.Bodak, SizeConstants.Large, 14, 27),
            ];
            testCases[CreatureConstants.BombardierBeetle_Giant] =
            [
                GetData(CreatureConstants.BombardierBeetle_Giant, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.BombardierBeetle_Giant, SizeConstants.Large, 5, 6),
            ];
            testCases[CreatureConstants.BoneDevil_Osyluth] =
            [
                GetData(CreatureConstants.BoneDevil_Osyluth, SizeConstants.Large, 11, 20),
                GetData(CreatureConstants.BoneDevil_Osyluth, SizeConstants.Huge, 21, 30),
            ];
            testCases[CreatureConstants.Bralani] =
            [
                GetData(CreatureConstants.Bralani, SizeConstants.Medium, 7, 12),
                GetData(CreatureConstants.Bralani, SizeConstants.Large, 13, 18),
            ];
            testCases[CreatureConstants.Bulette] =
            [
                GetData(CreatureConstants.Bulette, SizeConstants.Huge, 10, 16),
                GetData(CreatureConstants.Bulette, SizeConstants.Gargantuan, 17, 27),
            ];
            testCases[CreatureConstants.CarrionCrawler] =
            [
                GetData(CreatureConstants.CarrionCrawler, SizeConstants.Large, 4, 6),
                GetData(CreatureConstants.CarrionCrawler, SizeConstants.Huge, 7, 9),
            ];
            testCases[CreatureConstants.Centipede_Monstrous_Colossal] =
            [
                GetData(CreatureConstants.Centipede_Monstrous_Colossal, SizeConstants.Colossal, 25, 48),
            ];
            testCases[CreatureConstants.Centipede_Monstrous_Gargantuan] =
            [
                GetData(CreatureConstants.Centipede_Monstrous_Gargantuan, SizeConstants.Gargantuan, 13, 23),
            ];
            testCases[CreatureConstants.Centipede_Monstrous_Huge] =
            [
                GetData(CreatureConstants.Centipede_Monstrous_Huge, SizeConstants.Huge, 7, 11),
            ];
            testCases[CreatureConstants.Centipede_Monstrous_Large] =
            [
                GetData(CreatureConstants.Centipede_Monstrous_Large, SizeConstants.Large, 4, 5),
            ];
            testCases[CreatureConstants.ChainDevil_Kyton] =
            [
                GetData(CreatureConstants.ChainDevil_Kyton, SizeConstants.Medium, 9, 16),
            ];
            testCases[CreatureConstants.ChaosBeast] =
            [
                GetData(CreatureConstants.ChaosBeast, SizeConstants.Medium, 9, 12),
                GetData(CreatureConstants.ChaosBeast, SizeConstants.Large, 13, 24),
            ];
            testCases[CreatureConstants.Cheetah] =
            [
                GetData(CreatureConstants.Cheetah, SizeConstants.Medium, 4, 5),
            ];
            testCases[CreatureConstants.Chimera_Black] =
            [
                GetData(CreatureConstants.Chimera_Black, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Chimera_Black, SizeConstants.Huge, 14, 27),
            ];
            testCases[CreatureConstants.Chimera_Blue] =
            [
                GetData(CreatureConstants.Chimera_Blue, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Chimera_Blue, SizeConstants.Huge, 14, 27),
            ];
            testCases[CreatureConstants.Chimera_Green] =
            [
                GetData(CreatureConstants.Chimera_Green, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Chimera_Green, SizeConstants.Huge, 14, 27),
            ];
            testCases[CreatureConstants.Chimera_Red] =
            [
                GetData(CreatureConstants.Chimera_Red, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Chimera_Red, SizeConstants.Huge, 14, 27),
            ];
            testCases[CreatureConstants.Chimera_White] =
            [
                GetData(CreatureConstants.Chimera_White, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Chimera_White, SizeConstants.Huge, 14, 27),
            ];
            testCases[CreatureConstants.Choker] =
            [
                GetData(CreatureConstants.Choker, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Choker, SizeConstants.Medium, 7, 12),
            ];
            testCases[CreatureConstants.Chuul] =
            [
                GetData(CreatureConstants.Chuul, SizeConstants.Large, 12, 16),
                GetData(CreatureConstants.Chuul, SizeConstants.Huge, 17, 33),
            ];
            testCases[CreatureConstants.Cloaker] =
            [
                GetData(CreatureConstants.Cloaker, SizeConstants.Large, 7, 9),
                GetData(CreatureConstants.Cloaker, SizeConstants.Huge, 10, 18),
            ];
            testCases[CreatureConstants.Cockatrice] =
            [
                GetData(CreatureConstants.Cockatrice, SizeConstants.Small, 6, 8),
                GetData(CreatureConstants.Cockatrice, SizeConstants.Medium, 9, 15),
            ];
            testCases[CreatureConstants.Couatl] =
            [
                GetData(CreatureConstants.Couatl, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Couatl, SizeConstants.Huge, 14, 27),
            ];
            testCases[CreatureConstants.Criosphinx] =
            [
                GetData(CreatureConstants.Criosphinx, SizeConstants.Large, 11, 15),
                GetData(CreatureConstants.Criosphinx, SizeConstants.Huge, 16, 30),
            ];
            testCases[CreatureConstants.Crocodile] =
            [
                GetData(CreatureConstants.Crocodile, SizeConstants.Medium, 4, 5),
            ];
            testCases[CreatureConstants.Crocodile_Giant] =
            [
                GetData(CreatureConstants.Crocodile_Giant, SizeConstants.Huge, 8, 14),
            ];
            testCases[CreatureConstants.Darkmantle] =
            [
                GetData(CreatureConstants.Darkmantle, SizeConstants.Small, 2, 3),
            ];
            testCases[CreatureConstants.Deinonychus] =
            [
                GetData(CreatureConstants.Deinonychus, SizeConstants.Medium, 5, 8),
            ];
            testCases[CreatureConstants.Delver] =
            [
                GetData(CreatureConstants.Delver, SizeConstants.Huge, 16, 30),
                GetData(CreatureConstants.Delver, SizeConstants.Gargantuan, 31, 45),
            ];
            testCases[CreatureConstants.Destrachan] =
            [
                GetData(CreatureConstants.Destrachan, SizeConstants.Large, 9, 16),
                GetData(CreatureConstants.Destrachan, SizeConstants.Huge, 17, 24),
            ];
            testCases[CreatureConstants.Devourer] =
            [
                GetData(CreatureConstants.Devourer, SizeConstants.Large, 13, 24),
                GetData(CreatureConstants.Devourer, SizeConstants.Huge, 25, 36),
            ];
            testCases[CreatureConstants.Digester] =
            [
                GetData(CreatureConstants.Digester, SizeConstants.Medium, 9, 12),
                GetData(CreatureConstants.Digester, SizeConstants.Large, 13, 24),
            ];
            testCases[CreatureConstants.DisplacerBeast] =
            [
                GetData(CreatureConstants.DisplacerBeast, SizeConstants.Large, 7, 9),
                GetData(CreatureConstants.DisplacerBeast, SizeConstants.Huge, 10, 18),
            ];
            testCases[CreatureConstants.Djinni] =
            [
                GetData(CreatureConstants.Djinni, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Djinni, SizeConstants.Huge, 11, 21),
            ];
            testCases[CreatureConstants.Djinni_Noble] =
            [
                GetData(CreatureConstants.Djinni_Noble, SizeConstants.Large, 11, 15),
                GetData(CreatureConstants.Djinni_Noble, SizeConstants.Huge, 16, 30),
            ];
            testCases[CreatureConstants.Dragon_Black_Wyrmling] = [GetData(CreatureConstants.Dragon_Black_Wyrmling, SizeConstants.Tiny, 5, 6)];
            testCases[CreatureConstants.Dragon_Black_VeryYoung] = [GetData(CreatureConstants.Dragon_Black_VeryYoung, SizeConstants.Small, 8, 9)];
            testCases[CreatureConstants.Dragon_Black_Young] = [GetData(CreatureConstants.Dragon_Black_Young, SizeConstants.Medium, 11, 12)];
            testCases[CreatureConstants.Dragon_Black_Juvenile] = [GetData(CreatureConstants.Dragon_Black_Juvenile, SizeConstants.Medium, 14, 15)];
            testCases[CreatureConstants.Dragon_Black_YoungAdult] = [GetData(CreatureConstants.Dragon_Black_YoungAdult, SizeConstants.Large, 17, 18)];
            testCases[CreatureConstants.Dragon_Black_Adult] = [GetData(CreatureConstants.Dragon_Black_Adult, SizeConstants.Large, 20, 21)];
            testCases[CreatureConstants.Dragon_Black_MatureAdult] = [GetData(CreatureConstants.Dragon_Black_MatureAdult, SizeConstants.Huge, 23, 24)];
            testCases[CreatureConstants.Dragon_Black_Old] = [GetData(CreatureConstants.Dragon_Black_Old, SizeConstants.Huge, 26, 27)];
            testCases[CreatureConstants.Dragon_Black_VeryOld] = [GetData(CreatureConstants.Dragon_Black_VeryOld, SizeConstants.Huge, 29, 30)];
            testCases[CreatureConstants.Dragon_Black_Ancient] = [GetData(CreatureConstants.Dragon_Black_Ancient, SizeConstants.Huge, 32, 33)];
            testCases[CreatureConstants.Dragon_Black_Wyrm] = [GetData(CreatureConstants.Dragon_Black_Wyrm, SizeConstants.Gargantuan, 35, 36)];
            testCases[CreatureConstants.Dragon_Black_GreatWyrm] = [GetData(CreatureConstants.Dragon_Black_GreatWyrm, SizeConstants.Gargantuan, 38, 100)];

            testCases[CreatureConstants.Dragon_Blue_Wyrmling] = [GetData(CreatureConstants.Dragon_Blue_Wyrmling, SizeConstants.Small, 7, 8)];
            testCases[CreatureConstants.Dragon_Blue_VeryYoung] = [GetData(CreatureConstants.Dragon_Blue_VeryYoung, SizeConstants.Medium, 10, 11)];
            testCases[CreatureConstants.Dragon_Blue_Young] = [GetData(CreatureConstants.Dragon_Blue_Young, SizeConstants.Medium, 13, 14)];
            testCases[CreatureConstants.Dragon_Blue_Juvenile] = [GetData(CreatureConstants.Dragon_Blue_Juvenile, SizeConstants.Large, 16, 17)];
            testCases[CreatureConstants.Dragon_Blue_YoungAdult] = [GetData(CreatureConstants.Dragon_Blue_YoungAdult, SizeConstants.Large, 19, 20)];
            testCases[CreatureConstants.Dragon_Blue_Adult] = [GetData(CreatureConstants.Dragon_Blue_Adult, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Dragon_Blue_MatureAdult] = [GetData(CreatureConstants.Dragon_Blue_MatureAdult, SizeConstants.Huge, 25, 26)];
            testCases[CreatureConstants.Dragon_Blue_Old] = [GetData(CreatureConstants.Dragon_Blue_Old, SizeConstants.Huge, 28, 29)];
            testCases[CreatureConstants.Dragon_Blue_VeryOld] = [GetData(CreatureConstants.Dragon_Blue_VeryOld, SizeConstants.Huge, 31, 32)];
            testCases[CreatureConstants.Dragon_Blue_Ancient] = [GetData(CreatureConstants.Dragon_Blue_Ancient, SizeConstants.Gargantuan, 34, 35)];
            testCases[CreatureConstants.Dragon_Blue_Wyrm] = [GetData(CreatureConstants.Dragon_Blue_Wyrm, SizeConstants.Gargantuan, 37, 38)];
            testCases[CreatureConstants.Dragon_Blue_GreatWyrm] = [GetData(CreatureConstants.Dragon_Blue_GreatWyrm, SizeConstants.Gargantuan, 40, 100)];

            testCases[CreatureConstants.Dragon_Green_Wyrmling] = [GetData(CreatureConstants.Dragon_Green_Wyrmling, SizeConstants.Small, 6, 7)];
            testCases[CreatureConstants.Dragon_Green_VeryYoung] = [GetData(CreatureConstants.Dragon_Green_VeryYoung, SizeConstants.Medium, 9, 10)];
            testCases[CreatureConstants.Dragon_Green_Young] = [GetData(CreatureConstants.Dragon_Green_Young, SizeConstants.Medium, 12, 13)];
            testCases[CreatureConstants.Dragon_Green_Juvenile] = [GetData(CreatureConstants.Dragon_Green_Juvenile, SizeConstants.Large, 15, 16)];
            testCases[CreatureConstants.Dragon_Green_YoungAdult] = [GetData(CreatureConstants.Dragon_Green_YoungAdult, SizeConstants.Large, 18, 19)];
            testCases[CreatureConstants.Dragon_Green_Adult] = [GetData(CreatureConstants.Dragon_Green_Adult, SizeConstants.Huge, 21, 22)];
            testCases[CreatureConstants.Dragon_Green_MatureAdult] = [GetData(CreatureConstants.Dragon_Green_MatureAdult, SizeConstants.Huge, 24, 25)];
            testCases[CreatureConstants.Dragon_Green_Old] = [GetData(CreatureConstants.Dragon_Green_Old, SizeConstants.Huge, 27, 28)];
            testCases[CreatureConstants.Dragon_Green_VeryOld] = [GetData(CreatureConstants.Dragon_Green_VeryOld, SizeConstants.Huge, 30, 31)];
            testCases[CreatureConstants.Dragon_Green_Ancient] = [GetData(CreatureConstants.Dragon_Green_Ancient, SizeConstants.Gargantuan, 33, 34)];
            testCases[CreatureConstants.Dragon_Green_Wyrm] = [GetData(CreatureConstants.Dragon_Green_Wyrm, SizeConstants.Gargantuan, 36, 37)];
            testCases[CreatureConstants.Dragon_Green_GreatWyrm] = [GetData(CreatureConstants.Dragon_Green_GreatWyrm, SizeConstants.Gargantuan, 39, 100)];

            testCases[CreatureConstants.Dragon_Red_Wyrmling] = [GetData(CreatureConstants.Dragon_Red_Wyrmling, SizeConstants.Medium, 8, 9)];
            testCases[CreatureConstants.Dragon_Red_VeryYoung] = [GetData(CreatureConstants.Dragon_Red_VeryYoung, SizeConstants.Large, 11, 12)];
            testCases[CreatureConstants.Dragon_Red_Young] = [GetData(CreatureConstants.Dragon_Red_Young, SizeConstants.Large, 14, 15)];
            testCases[CreatureConstants.Dragon_Red_Juvenile] = [GetData(CreatureConstants.Dragon_Red_Juvenile, SizeConstants.Large, 17, 18)];
            testCases[CreatureConstants.Dragon_Red_YoungAdult] = [GetData(CreatureConstants.Dragon_Red_YoungAdult, SizeConstants.Huge, 20, 21)];
            testCases[CreatureConstants.Dragon_Red_Adult] = [GetData(CreatureConstants.Dragon_Red_Adult, SizeConstants.Huge, 23, 24)];
            testCases[CreatureConstants.Dragon_Red_MatureAdult] = [GetData(CreatureConstants.Dragon_Red_MatureAdult, SizeConstants.Huge, 26, 27)];
            testCases[CreatureConstants.Dragon_Red_Old] = [GetData(CreatureConstants.Dragon_Red_Old, SizeConstants.Gargantuan, 29, 30)];
            testCases[CreatureConstants.Dragon_Red_VeryOld] = [GetData(CreatureConstants.Dragon_Red_VeryOld, SizeConstants.Gargantuan, 32, 33)];
            testCases[CreatureConstants.Dragon_Red_Ancient] = [GetData(CreatureConstants.Dragon_Red_Ancient, SizeConstants.Gargantuan, 35, 36)];
            testCases[CreatureConstants.Dragon_Red_Wyrm] = [GetData(CreatureConstants.Dragon_Red_Wyrm, SizeConstants.Gargantuan, 38, 39)];
            testCases[CreatureConstants.Dragon_Red_GreatWyrm] = [GetData(CreatureConstants.Dragon_Red_GreatWyrm, SizeConstants.Colossal, 41, 100)];

            testCases[CreatureConstants.Dragon_White_Wyrmling] = [GetData(CreatureConstants.Dragon_White_Wyrmling, SizeConstants.Tiny, 4, 5)];
            testCases[CreatureConstants.Dragon_White_VeryYoung] = [GetData(CreatureConstants.Dragon_White_VeryYoung, SizeConstants.Small, 7, 8)];
            testCases[CreatureConstants.Dragon_White_Young] = [GetData(CreatureConstants.Dragon_White_Young, SizeConstants.Medium, 10, 11)];
            testCases[CreatureConstants.Dragon_White_Juvenile] = [GetData(CreatureConstants.Dragon_White_Juvenile, SizeConstants.Medium, 13, 14)];
            testCases[CreatureConstants.Dragon_White_YoungAdult] = [GetData(CreatureConstants.Dragon_White_YoungAdult, SizeConstants.Large, 16, 17)];
            testCases[CreatureConstants.Dragon_White_Adult] = [GetData(CreatureConstants.Dragon_White_Adult, SizeConstants.Large, 19, 20)];
            testCases[CreatureConstants.Dragon_White_MatureAdult] = [GetData(CreatureConstants.Dragon_White_MatureAdult, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Dragon_White_Old] = [GetData(CreatureConstants.Dragon_White_Old, SizeConstants.Huge, 25, 26)];
            testCases[CreatureConstants.Dragon_White_VeryOld] = [GetData(CreatureConstants.Dragon_White_VeryOld, SizeConstants.Huge, 28, 29)];
            testCases[CreatureConstants.Dragon_White_Ancient] = [GetData(CreatureConstants.Dragon_White_Ancient, SizeConstants.Huge, 31, 32)];
            testCases[CreatureConstants.Dragon_White_Wyrm] = [GetData(CreatureConstants.Dragon_White_Wyrm, SizeConstants.Gargantuan, 34, 35)];
            testCases[CreatureConstants.Dragon_White_GreatWyrm] = [GetData(CreatureConstants.Dragon_White_GreatWyrm, SizeConstants.Gargantuan, 37, 100)];

            testCases[CreatureConstants.Dragon_Brass_Wyrmling] = [GetData(CreatureConstants.Dragon_Brass_Wyrmling, SizeConstants.Tiny, 5, 6)];
            testCases[CreatureConstants.Dragon_Brass_VeryYoung] = [GetData(CreatureConstants.Dragon_Brass_VeryYoung, SizeConstants.Small, 8, 9)];
            testCases[CreatureConstants.Dragon_Brass_Young] = [GetData(CreatureConstants.Dragon_Brass_Young, SizeConstants.Medium, 11, 12)];
            testCases[CreatureConstants.Dragon_Brass_Juvenile] = [GetData(CreatureConstants.Dragon_Brass_Juvenile, SizeConstants.Medium, 14, 15)];
            testCases[CreatureConstants.Dragon_Brass_YoungAdult] = [GetData(CreatureConstants.Dragon_Brass_YoungAdult, SizeConstants.Large, 17, 18)];
            testCases[CreatureConstants.Dragon_Brass_Adult] = [GetData(CreatureConstants.Dragon_Brass_Adult, SizeConstants.Large, 20, 21)];
            testCases[CreatureConstants.Dragon_Brass_MatureAdult] = [GetData(CreatureConstants.Dragon_Brass_MatureAdult, SizeConstants.Huge, 23, 24)];
            testCases[CreatureConstants.Dragon_Brass_Old] = [GetData(CreatureConstants.Dragon_Brass_Old, SizeConstants.Huge, 26, 27)];
            testCases[CreatureConstants.Dragon_Brass_VeryOld] = [GetData(CreatureConstants.Dragon_Brass_VeryOld, SizeConstants.Huge, 29, 30)];
            testCases[CreatureConstants.Dragon_Brass_Ancient] = [GetData(CreatureConstants.Dragon_Brass_Ancient, SizeConstants.Huge, 32, 33)];
            testCases[CreatureConstants.Dragon_Brass_Wyrm] = [GetData(CreatureConstants.Dragon_Brass_Wyrm, SizeConstants.Gargantuan, 35, 36)];
            testCases[CreatureConstants.Dragon_Brass_GreatWyrm] = [GetData(CreatureConstants.Dragon_Brass_GreatWyrm, SizeConstants.Gargantuan, 38, 100)];

            testCases[CreatureConstants.Dragon_Bronze_Wyrmling] = [GetData(CreatureConstants.Dragon_Bronze_Wyrmling, SizeConstants.Small, 7, 8)];
            testCases[CreatureConstants.Dragon_Bronze_VeryYoung] = [GetData(CreatureConstants.Dragon_Bronze_VeryYoung, SizeConstants.Medium, 10, 11)];
            testCases[CreatureConstants.Dragon_Bronze_Young] = [GetData(CreatureConstants.Dragon_Bronze_Young, SizeConstants.Medium, 13, 14)];
            testCases[CreatureConstants.Dragon_Bronze_Juvenile] = [GetData(CreatureConstants.Dragon_Bronze_Juvenile, SizeConstants.Large, 16, 17)];
            testCases[CreatureConstants.Dragon_Bronze_YoungAdult] = [GetData(CreatureConstants.Dragon_Bronze_YoungAdult, SizeConstants.Large, 19, 20)];
            testCases[CreatureConstants.Dragon_Bronze_Adult] = [GetData(CreatureConstants.Dragon_Bronze_Adult, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Dragon_Bronze_MatureAdult] = [GetData(CreatureConstants.Dragon_Bronze_MatureAdult, SizeConstants.Huge, 25, 26)];
            testCases[CreatureConstants.Dragon_Bronze_Old] = [GetData(CreatureConstants.Dragon_Bronze_Old, SizeConstants.Huge, 28, 29)];
            testCases[CreatureConstants.Dragon_Bronze_VeryOld] = [GetData(CreatureConstants.Dragon_Bronze_VeryOld, SizeConstants.Huge, 31, 32)];
            testCases[CreatureConstants.Dragon_Bronze_Ancient] = [GetData(CreatureConstants.Dragon_Bronze_Ancient, SizeConstants.Gargantuan, 34, 35)];
            testCases[CreatureConstants.Dragon_Bronze_Wyrm] = [GetData(CreatureConstants.Dragon_Bronze_Wyrm, SizeConstants.Gargantuan, 37, 38)];
            testCases[CreatureConstants.Dragon_Bronze_GreatWyrm] = [GetData(CreatureConstants.Dragon_Bronze_GreatWyrm, SizeConstants.Gargantuan, 40, 100)];

            testCases[CreatureConstants.Dragon_Copper_Wyrmling] = [GetData(CreatureConstants.Dragon_Copper_Wyrmling, SizeConstants.Tiny, 6, 7)];
            testCases[CreatureConstants.Dragon_Copper_VeryYoung] = [GetData(CreatureConstants.Dragon_Copper_VeryYoung, SizeConstants.Small, 9, 10)];
            testCases[CreatureConstants.Dragon_Copper_Young] = [GetData(CreatureConstants.Dragon_Copper_Young, SizeConstants.Medium, 12, 13)];
            testCases[CreatureConstants.Dragon_Copper_Juvenile] = [GetData(CreatureConstants.Dragon_Copper_Juvenile, SizeConstants.Medium, 15, 16)];
            testCases[CreatureConstants.Dragon_Copper_YoungAdult] = [GetData(CreatureConstants.Dragon_Copper_YoungAdult, SizeConstants.Large, 18, 19)];
            testCases[CreatureConstants.Dragon_Copper_Adult] = [GetData(CreatureConstants.Dragon_Copper_Adult, SizeConstants.Large, 21, 22)];
            testCases[CreatureConstants.Dragon_Copper_MatureAdult] = [GetData(CreatureConstants.Dragon_Copper_MatureAdult, SizeConstants.Huge, 24, 25)];
            testCases[CreatureConstants.Dragon_Copper_Old] = [GetData(CreatureConstants.Dragon_Copper_Old, SizeConstants.Huge, 27, 28)];
            testCases[CreatureConstants.Dragon_Copper_VeryOld] = [GetData(CreatureConstants.Dragon_Copper_VeryOld, SizeConstants.Huge, 30, 31)];
            testCases[CreatureConstants.Dragon_Copper_Ancient] = [GetData(CreatureConstants.Dragon_Copper_Ancient, SizeConstants.Huge, 33, 34)];
            testCases[CreatureConstants.Dragon_Copper_Wyrm] = [GetData(CreatureConstants.Dragon_Copper_Wyrm, SizeConstants.Gargantuan, 36, 37)];
            testCases[CreatureConstants.Dragon_Copper_GreatWyrm] = [GetData(CreatureConstants.Dragon_Copper_GreatWyrm, SizeConstants.Gargantuan, 39, 100)];

            testCases[CreatureConstants.Dragon_Gold_Wyrmling] = [GetData(CreatureConstants.Dragon_Gold_Wyrmling, SizeConstants.Medium, 9, 10)];
            testCases[CreatureConstants.Dragon_Gold_VeryYoung] = [GetData(CreatureConstants.Dragon_Gold_VeryYoung, SizeConstants.Large, 12, 13)];
            testCases[CreatureConstants.Dragon_Gold_Young] = [GetData(CreatureConstants.Dragon_Gold_Young, SizeConstants.Large, 15, 16)];
            testCases[CreatureConstants.Dragon_Gold_Juvenile] = [GetData(CreatureConstants.Dragon_Gold_Juvenile, SizeConstants.Large, 18, 19)];
            testCases[CreatureConstants.Dragon_Gold_YoungAdult] = [GetData(CreatureConstants.Dragon_Gold_YoungAdult, SizeConstants.Huge, 21, 22)];
            testCases[CreatureConstants.Dragon_Gold_Adult] = [GetData(CreatureConstants.Dragon_Gold_Adult, SizeConstants.Huge, 24, 25)];
            testCases[CreatureConstants.Dragon_Gold_MatureAdult] = [GetData(CreatureConstants.Dragon_Gold_MatureAdult, SizeConstants.Huge, 27, 28)];
            testCases[CreatureConstants.Dragon_Gold_Old] = [GetData(CreatureConstants.Dragon_Gold_Old, SizeConstants.Gargantuan, 30, 31)];
            testCases[CreatureConstants.Dragon_Gold_VeryOld] = [GetData(CreatureConstants.Dragon_Gold_VeryOld, SizeConstants.Gargantuan, 33, 34)];
            testCases[CreatureConstants.Dragon_Gold_Ancient] = [GetData(CreatureConstants.Dragon_Gold_Ancient, SizeConstants.Gargantuan, 36, 37)];
            testCases[CreatureConstants.Dragon_Gold_Wyrm] = [GetData(CreatureConstants.Dragon_Gold_Wyrm, SizeConstants.Colossal, 39, 40)];
            testCases[CreatureConstants.Dragon_Gold_GreatWyrm] = [GetData(CreatureConstants.Dragon_Gold_GreatWyrm, SizeConstants.Colossal, 42, 100)];

            testCases[CreatureConstants.Dragon_Silver_Wyrmling] = [GetData(CreatureConstants.Dragon_Silver_Wyrmling, SizeConstants.Small, 8, 9)];
            testCases[CreatureConstants.Dragon_Silver_VeryYoung] = [GetData(CreatureConstants.Dragon_Silver_VeryYoung, SizeConstants.Medium, 11, 12)];
            testCases[CreatureConstants.Dragon_Silver_Young] = [GetData(CreatureConstants.Dragon_Silver_Young, SizeConstants.Medium, 14, 15)];
            testCases[CreatureConstants.Dragon_Silver_Juvenile] = [GetData(CreatureConstants.Dragon_Silver_Juvenile, SizeConstants.Large, 17, 18)];
            testCases[CreatureConstants.Dragon_Silver_YoungAdult] = [GetData(CreatureConstants.Dragon_Silver_YoungAdult, SizeConstants.Large, 20, 21)];
            testCases[CreatureConstants.Dragon_Silver_Adult] = [GetData(CreatureConstants.Dragon_Silver_Adult, SizeConstants.Huge, 23, 24)];
            testCases[CreatureConstants.Dragon_Silver_MatureAdult] = [GetData(CreatureConstants.Dragon_Silver_MatureAdult, SizeConstants.Huge, 26, 27)];
            testCases[CreatureConstants.Dragon_Silver_Old] = [GetData(CreatureConstants.Dragon_Silver_Old, SizeConstants.Huge, 29, 30)];
            testCases[CreatureConstants.Dragon_Silver_VeryOld] = [GetData(CreatureConstants.Dragon_Silver_VeryOld, SizeConstants.Huge, 32, 33)];
            testCases[CreatureConstants.Dragon_Silver_Ancient] = [GetData(CreatureConstants.Dragon_Silver_Ancient, SizeConstants.Gargantuan, 35, 36)];
            testCases[CreatureConstants.Dragon_Silver_Wyrm] = [GetData(CreatureConstants.Dragon_Silver_Wyrm, SizeConstants.Gargantuan, 38, 39)];
            testCases[CreatureConstants.Dragon_Silver_GreatWyrm] = [GetData(CreatureConstants.Dragon_Silver_GreatWyrm, SizeConstants.Colossal, 41, 100)];

            testCases[CreatureConstants.DragonTurtle] =
            [
                GetData(CreatureConstants.DragonTurtle, SizeConstants.Huge, 13, 24),
                GetData(CreatureConstants.DragonTurtle, SizeConstants.Gargantuan, 25, 36),
            ];
            testCases[CreatureConstants.Dragonne] =
            [
                GetData(CreatureConstants.Dragonne, SizeConstants.Large, 10, 12),
                GetData(CreatureConstants.Dragonne, SizeConstants.Huge, 13, 27),
            ];
            testCases[CreatureConstants.Dretch] = [GetData(CreatureConstants.Dretch, SizeConstants.Small, 3, 6)];
            testCases[CreatureConstants.Eagle] = [GetData(CreatureConstants.Eagle, SizeConstants.Medium, 2, 3)];
            testCases[CreatureConstants.Eagle_Giant] =
            [
                GetData(CreatureConstants.Eagle_Giant, SizeConstants.Large, 5, 8),
                GetData(CreatureConstants.Eagle_Giant, SizeConstants.Huge, 9, 12),
            ];
            testCases[CreatureConstants.Efreeti] =
            [
                GetData(CreatureConstants.Efreeti, SizeConstants.Large, 11, 15),
                GetData(CreatureConstants.Efreeti, SizeConstants.Huge, 16, 30),
            ];
            testCases[CreatureConstants.Elasmosaurus] =
            [
                GetData(CreatureConstants.Elasmosaurus, SizeConstants.Huge, 11, 20),
                GetData(CreatureConstants.Elasmosaurus, SizeConstants.Gargantuan, 21, 30),
            ];
            testCases[CreatureConstants.Elemental_Air_Elder] = [GetData(CreatureConstants.Elemental_Air_Elder, SizeConstants.Huge, 25, 48)];
            testCases[CreatureConstants.Elemental_Air_Greater] = [GetData(CreatureConstants.Elemental_Air_Greater, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Elemental_Air_Huge] = [GetData(CreatureConstants.Elemental_Air_Huge, SizeConstants.Huge, 17, 20)];
            testCases[CreatureConstants.Elemental_Air_Large] = [GetData(CreatureConstants.Elemental_Air_Large, SizeConstants.Large, 9, 15)];
            testCases[CreatureConstants.Elemental_Air_Medium] = [GetData(CreatureConstants.Elemental_Air_Medium, SizeConstants.Medium, 5, 7)];
            testCases[CreatureConstants.Elemental_Air_Small] = [GetData(CreatureConstants.Elemental_Air_Small, SizeConstants.Small, 3, 3)];
            testCases[CreatureConstants.Elemental_Earth_Elder] = [GetData(CreatureConstants.Elemental_Earth_Elder, SizeConstants.Huge, 25, 48)];
            testCases[CreatureConstants.Elemental_Earth_Greater] = [GetData(CreatureConstants.Elemental_Earth_Greater, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Elemental_Earth_Huge] = [GetData(CreatureConstants.Elemental_Earth_Huge, SizeConstants.Huge, 17, 20)];
            testCases[CreatureConstants.Elemental_Earth_Large] = [GetData(CreatureConstants.Elemental_Earth_Large, SizeConstants.Large, 9, 15)];
            testCases[CreatureConstants.Elemental_Earth_Medium] = [GetData(CreatureConstants.Elemental_Earth_Medium, SizeConstants.Medium, 5, 7)];
            testCases[CreatureConstants.Elemental_Earth_Small] = [GetData(CreatureConstants.Elemental_Earth_Small, SizeConstants.Small, 3, 3)];
            testCases[CreatureConstants.Elemental_Fire_Elder] = [GetData(CreatureConstants.Elemental_Fire_Elder, SizeConstants.Huge, 25, 48)];
            testCases[CreatureConstants.Elemental_Fire_Greater] = [GetData(CreatureConstants.Elemental_Fire_Greater, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Elemental_Fire_Huge] = [GetData(CreatureConstants.Elemental_Fire_Huge, SizeConstants.Huge, 17, 20)];
            testCases[CreatureConstants.Elemental_Fire_Large] = [GetData(CreatureConstants.Elemental_Fire_Large, SizeConstants.Large, 9, 15)];
            testCases[CreatureConstants.Elemental_Fire_Medium] = [GetData(CreatureConstants.Elemental_Fire_Medium, SizeConstants.Medium, 5, 7)];
            testCases[CreatureConstants.Elemental_Fire_Small] = [GetData(CreatureConstants.Elemental_Fire_Small, SizeConstants.Small, 3, 3)];
            testCases[CreatureConstants.Elemental_Water_Elder] = [GetData(CreatureConstants.Elemental_Water_Elder, SizeConstants.Huge, 25, 48)];
            testCases[CreatureConstants.Elemental_Water_Greater] = [GetData(CreatureConstants.Elemental_Water_Greater, SizeConstants.Huge, 22, 23)];
            testCases[CreatureConstants.Elemental_Water_Huge] = [GetData(CreatureConstants.Elemental_Water_Huge, SizeConstants.Huge, 17, 20)];
            testCases[CreatureConstants.Elemental_Water_Large] = [GetData(CreatureConstants.Elemental_Water_Large, SizeConstants.Large, 9, 15)];
            testCases[CreatureConstants.Elemental_Water_Medium] = [GetData(CreatureConstants.Elemental_Water_Medium, SizeConstants.Medium, 5, 7)];
            testCases[CreatureConstants.Elemental_Water_Small] = [GetData(CreatureConstants.Elemental_Water_Small, SizeConstants.Small, 3, 3)];
            testCases[CreatureConstants.Elephant] = [GetData(CreatureConstants.Elephant, SizeConstants.Huge, 12, 22)];
            testCases[CreatureConstants.Erinyes] = [GetData(CreatureConstants.Erinyes, SizeConstants.Medium, 10, 18)];
            testCases[CreatureConstants.EtherealFilcher] =
            [
                GetData(CreatureConstants.EtherealFilcher, SizeConstants.Medium, 6, 7),
                GetData(CreatureConstants.EtherealFilcher, SizeConstants.Large, 8, 15),
            ];
            testCases[CreatureConstants.EtherealMarauder] =
            [
                GetData(CreatureConstants.EtherealMarauder, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.EtherealMarauder, SizeConstants.Large, 5, 6),
            ];
            testCases[CreatureConstants.Ettercap] =
            [
                GetData(CreatureConstants.Ettercap, SizeConstants.Medium, 6, 7),
                GetData(CreatureConstants.Ettercap, SizeConstants.Large, 8, 15),
            ];
            testCases[CreatureConstants.FireBeetle_Giant] = [GetData(CreatureConstants.FireBeetle_Giant, SizeConstants.Small, 2, 3)];
            testCases[CreatureConstants.FormianMyrmarch] =
            [
                GetData(CreatureConstants.FormianMyrmarch, SizeConstants.Large, 13, 18),
                GetData(CreatureConstants.FormianMyrmarch, SizeConstants.Huge, 19, 24),
            ];
            testCases[CreatureConstants.FormianQueen] =
            [
                GetData(CreatureConstants.FormianQueen, SizeConstants.Huge, 21, 30),
                GetData(CreatureConstants.FormianQueen, SizeConstants.Gargantuan, 31, 40),
            ];
            testCases[CreatureConstants.FormianTaskmaster] =
            [
                GetData(CreatureConstants.FormianTaskmaster, SizeConstants.Medium, 7, 9),
                GetData(CreatureConstants.FormianTaskmaster, SizeConstants.Large, 10, 12),
            ];
            testCases[CreatureConstants.FormianWarrior] =
            [
                GetData(CreatureConstants.FormianWarrior, SizeConstants.Medium, 5, 8),
                GetData(CreatureConstants.FormianWarrior, SizeConstants.Large, 9, 12),
            ];
            testCases[CreatureConstants.FormianWorker] = [GetData(CreatureConstants.FormianWorker, SizeConstants.Medium, 2, 3)];
            testCases[CreatureConstants.FrostWorm] =
            [
                GetData(CreatureConstants.FrostWorm, SizeConstants.Huge, 15, 21),
                GetData(CreatureConstants.FrostWorm, SizeConstants.Gargantuan, 22, 42),
            ];
            testCases[CreatureConstants.Gargoyle] =
            [
                GetData(CreatureConstants.Gargoyle, SizeConstants.Medium, 5, 6),
                GetData(CreatureConstants.Gargoyle, SizeConstants.Large, 7, 12),
            ];
            testCases[CreatureConstants.Gargoyle_Kapoacinth] =
            [
                GetData(CreatureConstants.Gargoyle_Kapoacinth, SizeConstants.Medium, 5, 6),
                GetData(CreatureConstants.Gargoyle_Kapoacinth, SizeConstants.Large, 7, 12),
            ];
            testCases[CreatureConstants.Gargoyle_Kapoacinth] =
            [
                GetData(CreatureConstants.Gargoyle_Kapoacinth, SizeConstants.Medium, 5, 6),
                GetData(CreatureConstants.Gargoyle_Kapoacinth, SizeConstants.Large, 7, 12),
            ];
            testCases[CreatureConstants.GelatinousCube] =
            [
                GetData(CreatureConstants.GelatinousCube, SizeConstants.Large, 5, 12),
                GetData(CreatureConstants.GelatinousCube, SizeConstants.Huge, 13, 24),
            ];
            testCases[CreatureConstants.Ghaele] =
            [
                GetData(CreatureConstants.Ghaele, SizeConstants.Medium, 11, 15),
                GetData(CreatureConstants.Ghaele, SizeConstants.Large, 16, 30),
            ];
            testCases[CreatureConstants.Ghoul] = [GetData(CreatureConstants.Ghoul, SizeConstants.Medium, 3, 3)];
            testCases[CreatureConstants.Ghoul_Ghast] = [GetData(CreatureConstants.Ghoul_Ghast, SizeConstants.Medium, 5, 8)];
            testCases[CreatureConstants.Ghoul_Lacedon] = [GetData(CreatureConstants.Ghoul_Lacedon, SizeConstants.Medium, 3, 3)];
            testCases[CreatureConstants.GibberingMouther] = [GetData(CreatureConstants.GibberingMouther, SizeConstants.Large, 5, 12)];
            testCases[CreatureConstants.Girallon] =
            [
                GetData(CreatureConstants.Girallon, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Girallon, SizeConstants.Huge, 11, 21),
            ];
            testCases[CreatureConstants.Glabrezu] =
            [
                GetData(CreatureConstants.Glabrezu, SizeConstants.Huge, 13, 18),
                GetData(CreatureConstants.Glabrezu, SizeConstants.Gargantuan, 19, 36),
            ];
            testCases[CreatureConstants.Golem_Clay] =
            [
                GetData(CreatureConstants.Golem_Clay, SizeConstants.Large, 12, 18),
                GetData(CreatureConstants.Golem_Clay, SizeConstants.Huge, 19, 33),
            ];
            testCases[CreatureConstants.Golem_Flesh] =
            [
                GetData(CreatureConstants.Golem_Flesh, SizeConstants.Large, 10, 18),
                GetData(CreatureConstants.Golem_Flesh, SizeConstants.Huge, 19, 27),
            ];
            testCases[CreatureConstants.Golem_Iron] =
            [
                GetData(CreatureConstants.Golem_Iron, SizeConstants.Large, 19, 24),
                GetData(CreatureConstants.Golem_Iron, SizeConstants.Huge, 25, 54),
            ];
            testCases[CreatureConstants.Golem_Stone] =
            [
                GetData(CreatureConstants.Golem_Stone, SizeConstants.Large, 15, 21),
                GetData(CreatureConstants.Golem_Stone, SizeConstants.Huge, 22, 42),
            ];
            testCases[CreatureConstants.Gorgon] =
            [
                GetData(CreatureConstants.Gorgon, SizeConstants.Large, 9, 15),
                GetData(CreatureConstants.Gorgon, SizeConstants.Huge, 16, 24),
            ];
            testCases[CreatureConstants.GrayOoze] =
            [
                GetData(CreatureConstants.GrayOoze, SizeConstants.Medium, 4, 6),
                GetData(CreatureConstants.GrayOoze, SizeConstants.Large, 7, 9),
            ];
            testCases[CreatureConstants.GrayRender] =
            [
                GetData(CreatureConstants.GrayRender, SizeConstants.Large, 11, 15),
                GetData(CreatureConstants.GrayRender, SizeConstants.Huge, 16, 30),
            ];
            testCases[CreatureConstants.Grick] =
            [
                GetData(CreatureConstants.Grick, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.Grick, SizeConstants.Large, 5, 6),
            ];
            testCases[CreatureConstants.Griffon] =
            [
                GetData(CreatureConstants.Griffon, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Griffon, SizeConstants.Huge, 11, 21),
            ];
            testCases[CreatureConstants.Grig] = [GetData(CreatureConstants.Grig, SizeConstants.Tiny, 1, 3)];
            testCases[CreatureConstants.Grig_WithFiddle] = [GetData(CreatureConstants.Grig_WithFiddle, SizeConstants.Tiny, 1, 3)];
            testCases[CreatureConstants.Gynosphinx] =
            [
                GetData(CreatureConstants.Gynosphinx, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.Gynosphinx, SizeConstants.Huge, 13, 24),
            ];

            testCases[CreatureConstants.HellHound] =
                [GetData(CreatureConstants.HellHound, SizeConstants.Medium, 5, 8),
                GetData(CreatureConstants.HellHound, SizeConstants.Large, 9, 12)];
            testCases[CreatureConstants.HellHound_NessianWarhound] =
                [GetData(CreatureConstants.HellHound_NessianWarhound, SizeConstants.Large, 13, 17),
                GetData(CreatureConstants.HellHound_NessianWarhound, SizeConstants.Huge, 18, 24)];
            testCases[CreatureConstants.Hellcat_Bezekira] =
                [GetData(CreatureConstants.Hellcat_Bezekira, SizeConstants.Large, 9, 10),
                GetData(CreatureConstants.Hellcat_Bezekira, SizeConstants.Huge, 11, 24)];
            testCases[CreatureConstants.Hezrou] =
                [GetData(CreatureConstants.Hezrou, SizeConstants.Large, 11, 15),
                GetData(CreatureConstants.Hezrou, SizeConstants.Huge, 16, 30)];
            testCases[CreatureConstants.Hieracosphinx] =
                [GetData(CreatureConstants.Hieracosphinx, SizeConstants.Large, 10, 14),
                GetData(CreatureConstants.Hieracosphinx, SizeConstants.Huge, 15, 27)];
            testCases[CreatureConstants.Hippogriff] =
                [GetData(CreatureConstants.Hippogriff, SizeConstants.Large, 4, 6),
                GetData(CreatureConstants.Hippogriff, SizeConstants.Huge, 7, 9)];
            testCases[CreatureConstants.Homunculus] = [GetData(CreatureConstants.Homunculus, SizeConstants.Tiny, 3, 6)];
            testCases[CreatureConstants.HornedDevil_Cornugon] =
                [GetData(CreatureConstants.HornedDevil_Cornugon, SizeConstants.Large, 16, 20),
                GetData(CreatureConstants.HornedDevil_Cornugon, SizeConstants.Huge, 21, 45)];
            testCases[CreatureConstants.HoundArchon] =
                [GetData(CreatureConstants.HoundArchon, SizeConstants.Medium, 7, 9),
                GetData(CreatureConstants.HoundArchon, SizeConstants.Large, 10, 18)];
            testCases[CreatureConstants.Howler] =
                [GetData(CreatureConstants.Howler, SizeConstants.Large, 7, 9),
                GetData(CreatureConstants.Howler, SizeConstants.Huge, 10, 18)];
            testCases[CreatureConstants.Hyena] =
                [GetData(CreatureConstants.Hyena, SizeConstants.Medium, 3, 3),
                GetData(CreatureConstants.Hyena, SizeConstants.Large, 4, 5)];
            testCases[CreatureConstants.IceDevil_Gelugon] =
                [GetData(CreatureConstants.IceDevil_Gelugon, SizeConstants.Large, 15, 28),
                GetData(CreatureConstants.IceDevil_Gelugon, SizeConstants.Huge, 29, 42)];
            testCases[CreatureConstants.Imp] = [GetData(CreatureConstants.Imp, SizeConstants.Tiny, 4, 6)];
            testCases[CreatureConstants.InvisibleStalker] =
                [GetData(CreatureConstants.InvisibleStalker, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.InvisibleStalker, SizeConstants.Huge, 13, 24)];
            testCases[CreatureConstants.Janni] =
                [GetData(CreatureConstants.Janni, SizeConstants.Medium, 7, 9),
                GetData(CreatureConstants.Janni, SizeConstants.Large, 10, 18)];
            testCases[CreatureConstants.Kolyarut] =
                [GetData(CreatureConstants.Kolyarut, SizeConstants.Medium, 14, 22),
                GetData(CreatureConstants.Kolyarut, SizeConstants.Large, 23, 39)];
            testCases[CreatureConstants.Kraken] =
                [GetData(CreatureConstants.Kraken, SizeConstants.Gargantuan, 21, 32),
                GetData(CreatureConstants.Kraken, SizeConstants.Colossal, 33, 60)];
            testCases[CreatureConstants.Krenshar] =
                [GetData(CreatureConstants.Krenshar, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.Krenshar, SizeConstants.Large, 5, 8)];
            testCases[CreatureConstants.Lamia] =
                [GetData(CreatureConstants.Lamia, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Lamia, SizeConstants.Huge, 14, 27)];
            testCases[CreatureConstants.Lammasu] =
                [GetData(CreatureConstants.Lammasu, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Lammasu, SizeConstants.Huge, 11, 21)];
            testCases[CreatureConstants.LanternArchon] = [GetData(CreatureConstants.LanternArchon, SizeConstants.Small, 2, 4)];
            testCases[CreatureConstants.Lemure] = [GetData(CreatureConstants.Lemure, SizeConstants.Medium, 3, 6)];
            testCases[CreatureConstants.Leonal] =
                [GetData(CreatureConstants.Leonal, SizeConstants.Medium, 13, 18),
                GetData(CreatureConstants.Leonal, SizeConstants.Large, 19, 36)];
            testCases[CreatureConstants.Leopard] = [GetData(CreatureConstants.Leopard, SizeConstants.Medium, 4, 5)];
            testCases[CreatureConstants.Lillend] =
                [GetData(CreatureConstants.Lillend, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Lillend, SizeConstants.Huge, 11, 21)];
            testCases[CreatureConstants.Lion] = [GetData(CreatureConstants.Lion, SizeConstants.Large, 6, 8)];
            testCases[CreatureConstants.Lion_Dire] =
                [GetData(CreatureConstants.Lion_Dire, SizeConstants.Large, 9, 16),
                GetData(CreatureConstants.Lion_Dire, SizeConstants.Huge, 17, 24)];
            testCases[CreatureConstants.Lizard_Monitor] = [GetData(CreatureConstants.Lizard_Monitor, SizeConstants.Medium, 4, 5)];
            testCases[CreatureConstants.Magmin] =
                [GetData(CreatureConstants.Magmin, SizeConstants.Small, 3, 4),
                GetData(CreatureConstants.Magmin, SizeConstants.Medium, 5, 6)];
            testCases[CreatureConstants.MantaRay] = [GetData(CreatureConstants.MantaRay, SizeConstants.Large, 5, 6)];
            testCases[CreatureConstants.Manticore] =
                [GetData(CreatureConstants.Manticore, SizeConstants.Large, 7, 16),
                GetData(CreatureConstants.Manticore, SizeConstants.Huge, 17, 18)];
            testCases[CreatureConstants.Marilith] =
                [GetData(CreatureConstants.Marilith, SizeConstants.Large, 17, 20),
                GetData(CreatureConstants.Marilith, SizeConstants.Huge, 21, 48)];
            testCases[CreatureConstants.Marut] =
                [GetData(CreatureConstants.Marut, SizeConstants.Large, 16, 28),
                GetData(CreatureConstants.Marut, SizeConstants.Huge, 29, 45)];
            testCases[CreatureConstants.Megaraptor] =
                [GetData(CreatureConstants.Megaraptor, SizeConstants.Large, 9, 16),
                GetData(CreatureConstants.Megaraptor, SizeConstants.Huge, 17, 24)];
            testCases[CreatureConstants.Mephit_Air] =
                [GetData(CreatureConstants.Mephit_Air, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Air, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Dust] =
                [GetData(CreatureConstants.Mephit_Dust, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Dust, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Earth] =
                [GetData(CreatureConstants.Mephit_Earth, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Earth, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Fire] =
                [GetData(CreatureConstants.Mephit_Fire, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Fire, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Ice] =
                [GetData(CreatureConstants.Mephit_Ice, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Ice, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Magma] =
                [GetData(CreatureConstants.Mephit_Magma, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Magma, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Ooze] =
                [GetData(CreatureConstants.Mephit_Ooze, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Ooze, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Salt] =
                [GetData(CreatureConstants.Mephit_Salt, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Salt, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Steam] =
                [GetData(CreatureConstants.Mephit_Steam, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Steam, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mephit_Water] =
                [GetData(CreatureConstants.Mephit_Water, SizeConstants.Small, 4, 6),
                GetData(CreatureConstants.Mephit_Water, SizeConstants.Medium, 7, 9)];
            testCases[CreatureConstants.Mimic] =
                [GetData(CreatureConstants.Mimic, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Mimic, SizeConstants.Huge, 11, 21)];
            testCases[CreatureConstants.Mohrg] =
                [GetData(CreatureConstants.Mohrg, SizeConstants.Medium, 15, 21),
                GetData(CreatureConstants.Mohrg, SizeConstants.Large, 22, 28)];
            testCases[CreatureConstants.Monkey] = [GetData(CreatureConstants.Monkey, SizeConstants.Small, 2, 3)];
            testCases[CreatureConstants.Mummy] =
                [GetData(CreatureConstants.Mummy, SizeConstants.Medium, 9, 16),
                GetData(CreatureConstants.Mummy, SizeConstants.Large, 17, 24)];
            testCases[CreatureConstants.Naga_Dark] =
                [GetData(CreatureConstants.Naga_Dark, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Naga_Dark, SizeConstants.Huge, 14, 27)];
            testCases[CreatureConstants.Naga_Guardian] =
                [GetData(CreatureConstants.Naga_Guardian, SizeConstants.Large, 12, 16),
                GetData(CreatureConstants.Naga_Guardian, SizeConstants.Huge, 17, 33)];
            testCases[CreatureConstants.Naga_Spirit] =
                [GetData(CreatureConstants.Naga_Spirit, SizeConstants.Large, 10, 13),
                GetData(CreatureConstants.Naga_Spirit, SizeConstants.Huge, 14, 27)];
            testCases[CreatureConstants.Naga_Water] =
                [GetData(CreatureConstants.Naga_Water, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Naga_Water, SizeConstants.Huge, 11, 21)];
            testCases[CreatureConstants.Nalfeshnee] =
                [GetData(CreatureConstants.Nalfeshnee, SizeConstants.Huge, 15, 20),
                GetData(CreatureConstants.Nalfeshnee, SizeConstants.Gargantuan, 21, 42)];
            testCases[CreatureConstants.NightHag] = [GetData(CreatureConstants.NightHag, SizeConstants.Medium, 9, 16)];
            testCases[CreatureConstants.Nightcrawler] = [GetData(CreatureConstants.Nightcrawler, SizeConstants.Colossal, 26, 50)];
            testCases[CreatureConstants.Nightmare] =
                [GetData(CreatureConstants.Nightmare, SizeConstants.Large, 7, 10),
                GetData(CreatureConstants.Nightmare, SizeConstants.Huge, 11, 18)];
            testCases[CreatureConstants.Nightwalker] =
                [GetData(CreatureConstants.Nightwalker, SizeConstants.Huge, 22, 31),
                GetData(CreatureConstants.Nightwalker, SizeConstants.Gargantuan, 32, 42)];
            testCases[CreatureConstants.Nightwing] =
                [GetData(CreatureConstants.Nightwing, SizeConstants.Huge, 18, 25),
                GetData(CreatureConstants.Nightwing, SizeConstants.Gargantuan, 26, 34)];
            testCases[CreatureConstants.Nixie] = [GetData(CreatureConstants.Nixie, SizeConstants.Small, 2, 3)];
            testCases[CreatureConstants.Nymph] = [GetData(CreatureConstants.Nymph, SizeConstants.Medium, 7, 12)];
            testCases[CreatureConstants.OchreJelly] =
                [GetData(CreatureConstants.OchreJelly, SizeConstants.Large, 7, 9),
                GetData(CreatureConstants.OchreJelly, SizeConstants.Huge, 10, 18)];
            testCases[CreatureConstants.Octopus] = [GetData(CreatureConstants.Octopus, SizeConstants.Medium, 3, 6)];
            testCases[CreatureConstants.Octopus_Giant] =
                [GetData(CreatureConstants.Octopus_Giant, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.Octopus_Giant, SizeConstants.Huge, 13, 24)];
            testCases[CreatureConstants.Otyugh] =
                [GetData(CreatureConstants.Otyugh, SizeConstants.Large, 7, 8),
                GetData(CreatureConstants.Otyugh, SizeConstants.Huge, 9, 18)];
            testCases[CreatureConstants.Owl] = [GetData(CreatureConstants.Owl, SizeConstants.Small, 2, 2)];
            testCases[CreatureConstants.Owl_Giant] =
                [GetData(CreatureConstants.Owl_Giant, SizeConstants.Large, 5, 8),
                GetData(CreatureConstants.Owl_Giant, SizeConstants.Huge, 9, 12)];
            testCases[CreatureConstants.Owlbear] =
                [GetData(CreatureConstants.Owlbear, SizeConstants.Large, 6, 8),
                GetData(CreatureConstants.Owlbear, SizeConstants.Huge, 9, 15)];
            testCases[CreatureConstants.Pegasus] = [GetData(CreatureConstants.Pegasus, SizeConstants.Large, 5, 8)];
            testCases[CreatureConstants.PhantomFungus] =
                [GetData(CreatureConstants.PhantomFungus, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.PhantomFungus, SizeConstants.Large, 5, 6)];
            testCases[CreatureConstants.PhaseSpider] =
                [GetData(CreatureConstants.PhaseSpider, SizeConstants.Large, 6, 8),
                GetData(CreatureConstants.PhaseSpider, SizeConstants.Huge, 9, 15)];
            testCases[CreatureConstants.Phasm] =
                [GetData(CreatureConstants.Phasm, SizeConstants.Huge, 16, 21),
                GetData(CreatureConstants.Phasm, SizeConstants.Gargantuan, 22, 45)];
            testCases[CreatureConstants.PitFiend] =
                [GetData(CreatureConstants.PitFiend, SizeConstants.Large, 19, 36),
                GetData(CreatureConstants.PitFiend, SizeConstants.Huge, 37, 54)];
            testCases[CreatureConstants.Pixie] = [GetData(CreatureConstants.Pixie, SizeConstants.Small, 2, 3)];
            testCases[CreatureConstants.Pixie_WithIrresistibleDance] = [GetData(CreatureConstants.Pixie_WithIrresistibleDance, SizeConstants.Small, 2, 3)];
            testCases[CreatureConstants.Porpoise] = [GetData(CreatureConstants.Porpoise, SizeConstants.Medium, 3, 4)];
            testCases[CreatureConstants.PrayingMantis_Giant] =
                [GetData(CreatureConstants.PrayingMantis_Giant, SizeConstants.Large, 5, 8),
                GetData(CreatureConstants.PrayingMantis_Giant, SizeConstants.Huge, 9, 12)];
            testCases[CreatureConstants.Pseudodragon] = [GetData(CreatureConstants.Pseudodragon, SizeConstants.Tiny, 3, 4)];
            testCases[CreatureConstants.PurpleWorm] =
                [GetData(CreatureConstants.PurpleWorm, SizeConstants.Gargantuan, 17, 32),
                GetData(CreatureConstants.PurpleWorm, SizeConstants.Colossal, 33, 48)];
            testCases[CreatureConstants.Quasit] = [GetData(CreatureConstants.Quasit, SizeConstants.Tiny, 4, 6)];
            testCases[CreatureConstants.Rast] =
                [GetData(CreatureConstants.Rast, SizeConstants.Medium, 5, 6),
                GetData(CreatureConstants.Rast, SizeConstants.Large, 7, 12)];
            testCases[CreatureConstants.Rat_Dire] =
                [GetData(CreatureConstants.Rat_Dire, SizeConstants.Small, 2, 3),
                GetData(CreatureConstants.Rat_Dire, SizeConstants.Medium, 4, 6)];
            testCases[CreatureConstants.Ravid] =
                [GetData(CreatureConstants.Ravid, SizeConstants.Medium, 4, 4),
                GetData(CreatureConstants.Ravid, SizeConstants.Large, 5, 9)];
            testCases[CreatureConstants.RazorBoar] =
                [GetData(CreatureConstants.RazorBoar, SizeConstants.Large, 16, 30),
                GetData(CreatureConstants.RazorBoar, SizeConstants.Huge, 31, 45)];
            testCases[CreatureConstants.Remorhaz] =
                [GetData(CreatureConstants.Remorhaz, SizeConstants.Huge, 8, 14),
                GetData(CreatureConstants.Remorhaz, SizeConstants.Gargantuan, 15, 21)];
            testCases[CreatureConstants.Retriever] =
                [GetData(CreatureConstants.Retriever, SizeConstants.Huge, 11, 15),
                GetData(CreatureConstants.Retriever, SizeConstants.Gargantuan, 16, 30)];
            testCases[CreatureConstants.Rhinoceras] =
                [GetData(CreatureConstants.Rhinoceras, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.Rhinoceras, SizeConstants.Huge, 13, 24)];
            testCases[CreatureConstants.Roc] =
                [GetData(CreatureConstants.Roc, SizeConstants.Gargantuan, 19, 32),
                GetData(CreatureConstants.Roc, SizeConstants.Colossal, 33, 54)];
            testCases[CreatureConstants.Roper] =
                [GetData(CreatureConstants.Roper, SizeConstants.Large, 11, 15),
                GetData(CreatureConstants.Roper, SizeConstants.Huge, 16, 30)];
            testCases[CreatureConstants.RustMonster] =
                [GetData(CreatureConstants.RustMonster, SizeConstants.Medium, 6, 8),
                GetData(CreatureConstants.RustMonster, SizeConstants.Large, 9, 15)];
            testCases[CreatureConstants.Sahuagin] =
                [GetData(CreatureConstants.Sahuagin, SizeConstants.Medium, 3, 5),
                GetData(CreatureConstants.Sahuagin, SizeConstants.Large, 6, 10)];
            testCases[CreatureConstants.Sahuagin_Malenti] =
                [GetData(CreatureConstants.Sahuagin_Malenti, SizeConstants.Medium, 3, 5),
                GetData(CreatureConstants.Sahuagin_Malenti, SizeConstants.Large, 6, 10)];
            testCases[CreatureConstants.Sahuagin_Mutant] =
                [GetData(CreatureConstants.Sahuagin_Mutant, SizeConstants.Medium, 3, 5),
                GetData(CreatureConstants.Sahuagin_Mutant, SizeConstants.Large, 6, 10)];
            testCases[CreatureConstants.Salamander_Average] = [GetData(CreatureConstants.Salamander_Average, SizeConstants.Medium, 10, 14)];
            testCases[CreatureConstants.Salamander_Flamebrother] = [GetData(CreatureConstants.Salamander_Flamebrother, SizeConstants.Small, 5, 6)];
            testCases[CreatureConstants.Salamander_Noble] =
                [GetData(CreatureConstants.Salamander_Noble, SizeConstants.Large, 16, 21),
                GetData(CreatureConstants.Salamander_Noble, SizeConstants.Huge, 22, 45)];
            testCases[CreatureConstants.Satyr] = [GetData(CreatureConstants.Satyr, SizeConstants.Medium, 6, 10)];
            testCases[CreatureConstants.Satyr_WithPipes] = [GetData(CreatureConstants.Satyr_WithPipes, SizeConstants.Medium, 6, 10)];
            testCases[CreatureConstants.Scorpion_Monstrous_Colossal] = [GetData(CreatureConstants.Scorpion_Monstrous_Colossal, SizeConstants.Colossal, 41, 60)];
            testCases[CreatureConstants.Scorpion_Monstrous_Gargantuan] = [GetData(CreatureConstants.Scorpion_Monstrous_Gargantuan, SizeConstants.Gargantuan, 21, 39)];
            testCases[CreatureConstants.Scorpion_Monstrous_Huge] = [GetData(CreatureConstants.Scorpion_Monstrous_Huge, SizeConstants.Huge, 11, 19)];
            testCases[CreatureConstants.Scorpion_Monstrous_Large] = [GetData(CreatureConstants.Scorpion_Monstrous_Large, SizeConstants.Large, 6, 9)];
            testCases[CreatureConstants.Scorpion_Monstrous_Medium] = [GetData(CreatureConstants.Scorpion_Monstrous_Medium, SizeConstants.Medium, 3, 4)];
            testCases[CreatureConstants.SeaCat] =
                [GetData(CreatureConstants.SeaCat, SizeConstants.Large, 7, 9),
                GetData(CreatureConstants.SeaCat, SizeConstants.Huge, 10, 18)];
            testCases[CreatureConstants.Shadow] = [GetData(CreatureConstants.Shadow, SizeConstants.Medium, 4, 9)];
            testCases[CreatureConstants.ShadowMastiff] =
                [GetData(CreatureConstants.ShadowMastiff, SizeConstants.Medium, 5, 6),
                GetData(CreatureConstants.ShadowMastiff, SizeConstants.Large, 7, 12)];
            testCases[CreatureConstants.ShamblingMound] =
                [GetData(CreatureConstants.ShamblingMound, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.ShamblingMound, SizeConstants.Huge, 13, 24)];
            testCases[CreatureConstants.Shark_Dire] =
                [GetData(CreatureConstants.Shark_Dire, SizeConstants.Huge, 19, 32),
                GetData(CreatureConstants.Shark_Dire, SizeConstants.Gargantuan, 33, 54)];
            testCases[CreatureConstants.Shark_Huge] = [GetData(CreatureConstants.Shark_Huge, SizeConstants.Huge, 11, 17)];
            testCases[CreatureConstants.Shark_Large] = [GetData(CreatureConstants.Shark_Large, SizeConstants.Large, 8, 9)];
            testCases[CreatureConstants.Shark_Medium] = [GetData(CreatureConstants.Shark_Medium, SizeConstants.Medium, 4, 6)];
            testCases[CreatureConstants.ShieldGuardian] =
                [GetData(CreatureConstants.ShieldGuardian, SizeConstants.Large, 16, 24),
                GetData(CreatureConstants.ShieldGuardian, SizeConstants.Huge, 25, 45)];
            testCases[CreatureConstants.ShockerLizard] =
                [GetData(CreatureConstants.ShockerLizard, SizeConstants.Small, 3, 4),
                GetData(CreatureConstants.ShockerLizard, SizeConstants.Medium, 5, 6)];
            testCases[CreatureConstants.Shrieker] = [GetData(CreatureConstants.Shrieker, SizeConstants.Medium, 3, 3)];
            testCases[CreatureConstants.Skum] =
                [GetData(CreatureConstants.Skum, SizeConstants.Medium, 3, 4),
                GetData(CreatureConstants.Skum, SizeConstants.Large, 5, 6)];
            testCases[CreatureConstants.Slaad_Blue] =
                [GetData(CreatureConstants.Slaad_Blue, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.Slaad_Blue, SizeConstants.Huge, 13, 24)];
            testCases[CreatureConstants.Slaad_Death] =
                [GetData(CreatureConstants.Slaad_Death, SizeConstants.Medium, 16, 22),
                GetData(CreatureConstants.Slaad_Death, SizeConstants.Large, 23, 45)];
            testCases[CreatureConstants.Slaad_Gray] = [GetData(CreatureConstants.Slaad_Gray, SizeConstants.Medium, 11, 15),
                GetData(CreatureConstants.Slaad_Gray, SizeConstants.Large, 16, 30)];
            testCases[CreatureConstants.Slaad_Green] = [GetData(CreatureConstants.Slaad_Green, SizeConstants.Large, 10, 15),
                GetData(CreatureConstants.Slaad_Green, SizeConstants.Huge, 16, 27)];
            testCases[CreatureConstants.Slaad_Red] = [GetData(CreatureConstants.Slaad_Red, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.Slaad_Red, SizeConstants.Huge, 11, 21)];
            testCases[CreatureConstants.Snake_Constrictor] = [GetData(CreatureConstants.Snake_Constrictor, SizeConstants.Medium, 4, 5),
                GetData(CreatureConstants.Snake_Constrictor, SizeConstants.Large, 6, 10)];
            testCases[CreatureConstants.Snake_Constrictor_Giant] = [GetData(CreatureConstants.Snake_Constrictor_Giant, SizeConstants.Huge, 12, 16),
                GetData(CreatureConstants.Snake_Constrictor_Giant, SizeConstants.Gargantuan, 17, 33)];
            testCases[CreatureConstants.Snake_Viper_Huge] = [GetData(CreatureConstants.Snake_Viper_Huge, SizeConstants.Huge, 7, 18)];
            testCases[CreatureConstants.Spectre] = [GetData(CreatureConstants.Spectre, SizeConstants.Medium, 8, 14)];
            testCases[CreatureConstants.Spider_Monstrous_Hunter_Colossal] = [GetData(CreatureConstants.Spider_Monstrous_Hunter_Colossal, SizeConstants.Colossal, 33, 60)];
            testCases[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan] =
                [GetData(CreatureConstants.Spider_Monstrous_Hunter_Gargantuan, SizeConstants.Gargantuan, 17, 31)];
            testCases[CreatureConstants.Spider_Monstrous_Hunter_Huge] = [GetData(CreatureConstants.Spider_Monstrous_Hunter_Huge, SizeConstants.Huge, 9, 15)];
            testCases[CreatureConstants.Spider_Monstrous_Hunter_Large] = [GetData(CreatureConstants.Spider_Monstrous_Hunter_Large, SizeConstants.Large, 5, 7)];
            testCases[CreatureConstants.Spider_Monstrous_Hunter_Medium] = [GetData(CreatureConstants.Spider_Monstrous_Hunter_Medium, SizeConstants.Medium, 3, 3)];
            testCases[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal] =
                [GetData(CreatureConstants.Spider_Monstrous_WebSpinner_Colossal, SizeConstants.Colossal, 33, 60)];
            testCases[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan] =
                [GetData(CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan, SizeConstants.Gargantuan, 17, 31)];
            testCases[CreatureConstants.Spider_Monstrous_WebSpinner_Huge] = [GetData(CreatureConstants.Spider_Monstrous_WebSpinner_Huge, SizeConstants.Huge, 9, 15)];
            testCases[CreatureConstants.Spider_Monstrous_WebSpinner_Large] = [GetData(CreatureConstants.Spider_Monstrous_WebSpinner_Large, SizeConstants.Large, 5, 7)];
            testCases[CreatureConstants.Spider_Monstrous_WebSpinner_Medium] = [GetData(CreatureConstants.Spider_Monstrous_WebSpinner_Medium, SizeConstants.Medium, 3, 3)];
            testCases[CreatureConstants.SpiderEater] = [GetData(CreatureConstants.SpiderEater, SizeConstants.Huge, 5, 12)];
            testCases[CreatureConstants.Squid] =
                [GetData(CreatureConstants.Squid, SizeConstants.Medium, 4, 6),
                GetData(CreatureConstants.Squid, SizeConstants.Large, 7, 11)];
            testCases[CreatureConstants.Squid_Giant] =
                [GetData(CreatureConstants.Squid_Giant, SizeConstants.Huge, 13, 18),
                GetData(CreatureConstants.Squid_Giant, SizeConstants.Gargantuan, 19, 36)];
            testCases[CreatureConstants.StagBeetle_Giant] =
                [GetData(CreatureConstants.StagBeetle_Giant, SizeConstants.Large, 8, 10),
                GetData(CreatureConstants.StagBeetle_Giant, SizeConstants.Huge, 11, 21)];
            testCases[CreatureConstants.Succubus] = [GetData(CreatureConstants.Succubus, SizeConstants.Medium, 7, 12)];
            testCases[CreatureConstants.Tarrasque] = [GetData(CreatureConstants.Tarrasque, SizeConstants.Colossal, 49, 100)];
            testCases[CreatureConstants.Tendriculos] =
                [GetData(CreatureConstants.Tendriculos, SizeConstants.Huge, 10, 16),
                GetData(CreatureConstants.Tendriculos, SizeConstants.Gargantuan, 17, 27)];
            testCases[CreatureConstants.Thoqqua] = [GetData(CreatureConstants.Thoqqua, SizeConstants.Large, 4, 9)];
            testCases[CreatureConstants.Tiger] =
                [GetData(CreatureConstants.Tiger, SizeConstants.Large, 7, 12),
                GetData(CreatureConstants.Tiger, SizeConstants.Huge, 13, 18)];
            testCases[CreatureConstants.Tiger_Dire] =
                [GetData(CreatureConstants.Tiger_Dire, SizeConstants.Large, 17, 32),
                GetData(CreatureConstants.Tiger_Dire, SizeConstants.Huge, 33, 48)];
            testCases[CreatureConstants.Titan] =
                [GetData(CreatureConstants.Titan, SizeConstants.Huge, 21, 30),
                GetData(CreatureConstants.Titan, SizeConstants.Gargantuan, 31, 60)];
            testCases[CreatureConstants.Tojanida_Adult] = [GetData(CreatureConstants.Tojanida_Adult, SizeConstants.Medium, 8, 14)];
            testCases[CreatureConstants.Tojanida_Elder] = [GetData(CreatureConstants.Tojanida_Elder, SizeConstants.Large, 16, 24)];
            testCases[CreatureConstants.Tojanida_Juvenile] = [GetData(CreatureConstants.Tojanida_Juvenile, SizeConstants.Small, 4, 6)];
            testCases[CreatureConstants.Treant] =
                [GetData(CreatureConstants.Treant, SizeConstants.Huge, 8, 16),
                GetData(CreatureConstants.Treant, SizeConstants.Gargantuan, 17, 21)];
            testCases[CreatureConstants.Triceratops] =
                [GetData(CreatureConstants.Triceratops, SizeConstants.Huge, 17, 32),
                GetData(CreatureConstants.Triceratops, SizeConstants.Gargantuan, 33, 48)];
            testCases[CreatureConstants.Triton] = [GetData(CreatureConstants.Triton, SizeConstants.Medium, 4, 9)];
            testCases[CreatureConstants.TrumpetArchon] =
                [GetData(CreatureConstants.TrumpetArchon, SizeConstants.Medium, 13, 18),
                GetData(CreatureConstants.TrumpetArchon, SizeConstants.Large, 19, 36)];
            testCases[CreatureConstants.Tyrannosaurus] =
                [GetData(CreatureConstants.Tyrannosaurus, SizeConstants.Huge, 19, 36),
                GetData(CreatureConstants.Tyrannosaurus, SizeConstants.Gargantuan, 37, 54)];
            testCases[CreatureConstants.Unicorn] = [GetData(CreatureConstants.Unicorn, SizeConstants.Large, 5, 8)];
            testCases[CreatureConstants.UmberHulk] =
                [GetData(CreatureConstants.UmberHulk, SizeConstants.Large, 9, 12),
                GetData(CreatureConstants.UmberHulk, SizeConstants.Huge, 13, 24)];
            testCases[CreatureConstants.Vargouille] = [GetData(CreatureConstants.Vargouille, SizeConstants.Small, 2, 3)];
            testCases[CreatureConstants.VioletFungus] = [GetData(CreatureConstants.VioletFungus, SizeConstants.Medium, 3, 6)];
            testCases[CreatureConstants.Vrock] =
                [GetData(CreatureConstants.Vrock, SizeConstants.Large, 11, 14),
                GetData(CreatureConstants.Vrock, SizeConstants.Huge, 15, 30)];
            testCases[CreatureConstants.Wasp_Giant] =
                [GetData(CreatureConstants.Wasp_Giant, SizeConstants.Large, 6, 8),
                GetData(CreatureConstants.Wasp_Giant, SizeConstants.Huge, 9, 15)];
            testCases[CreatureConstants.Weasel_Dire] =
                [GetData(CreatureConstants.Weasel_Dire, SizeConstants.Medium, 4, 6),
                GetData(CreatureConstants.Weasel_Dire, SizeConstants.Large, 7, 9)];
            testCases[CreatureConstants.Whale_Baleen] =
                [GetData(CreatureConstants.Whale_Baleen, SizeConstants.Gargantuan, 13, 18),
                GetData(CreatureConstants.Whale_Baleen, SizeConstants.Colossal, 19, 36)];
            testCases[CreatureConstants.Whale_Cachalot] =
                [GetData(CreatureConstants.Whale_Cachalot, SizeConstants.Gargantuan, 13, 18),
                GetData(CreatureConstants.Whale_Cachalot, SizeConstants.Colossal, 19, 36)];
            testCases[CreatureConstants.Whale_Orca] =
                [GetData(CreatureConstants.Whale_Orca, SizeConstants.Huge, 10, 13),
                GetData(CreatureConstants.Whale_Orca, SizeConstants.Gargantuan, 14, 27)];
            testCases[CreatureConstants.Wight] = [GetData(CreatureConstants.Wight, SizeConstants.Medium, 5, 8)];
            testCases[CreatureConstants.WillOWisp] = [GetData(CreatureConstants.WillOWisp, SizeConstants.Small, 10, 18)];
            testCases[CreatureConstants.WinterWolf] =
                [GetData(CreatureConstants.WinterWolf, SizeConstants.Large, 7, 9),
                GetData(CreatureConstants.WinterWolf, SizeConstants.Huge, 10, 18)];
            testCases[CreatureConstants.Wolf] =
                [GetData(CreatureConstants.Wolf, SizeConstants.Medium, 3, 3),
                GetData(CreatureConstants.Wolf, SizeConstants.Large, 4, 6)];
            testCases[CreatureConstants.Wolf_Dire] = [GetData(CreatureConstants.Wolf_Dire, SizeConstants.Large, 7, 18)];
            testCases[CreatureConstants.Wolverine] = [GetData(CreatureConstants.Wolverine, SizeConstants.Large, 4, 5)];
            testCases[CreatureConstants.Wolverine_Dire] = [GetData(CreatureConstants.Wolverine_Dire, SizeConstants.Large, 6, 15)];
            testCases[CreatureConstants.Worg] =
                [GetData(CreatureConstants.Worg, SizeConstants.Medium, 5, 6),
                GetData(CreatureConstants.Worg, SizeConstants.Large, 7, 12)];
            testCases[CreatureConstants.Wraith] = [GetData(CreatureConstants.Wraith, SizeConstants.Medium, 6, 10)];
            testCases[CreatureConstants.Wraith_Dread] = [GetData(CreatureConstants.Wraith_Dread, SizeConstants.Large, 17, 32)];
            testCases[CreatureConstants.Wyvern] =
                [GetData(CreatureConstants.Wyvern, SizeConstants.Huge, 8, 10),
                GetData(CreatureConstants.Wyvern, SizeConstants.Gargantuan, 11, 21)];
            testCases[CreatureConstants.Xill] =
                [GetData(CreatureConstants.Xill, SizeConstants.Medium, 6, 8),
                GetData(CreatureConstants.Xill, SizeConstants.Large, 9, 15)];
            testCases[CreatureConstants.Xorn_Average] = [GetData(CreatureConstants.Xorn_Average, SizeConstants.Medium, 8, 14)];
            testCases[CreatureConstants.Xorn_Elder] =
                [GetData(CreatureConstants.Xorn_Elder, SizeConstants.Large, 16, 21),
                GetData(CreatureConstants.Xorn_Elder, SizeConstants.Huge, 22, 45)];
            testCases[CreatureConstants.Xorn_Minor] = [GetData(CreatureConstants.Xorn_Minor, SizeConstants.Small, 4, 6)];
            testCases[CreatureConstants.YethHound] =
                [GetData(CreatureConstants.YethHound, SizeConstants.Medium, 4, 6),
                GetData(CreatureConstants.YethHound, SizeConstants.Large, 7, 9)];
            testCases[CreatureConstants.Yrthak] =
                [GetData(CreatureConstants.Yrthak, SizeConstants.Huge, 13, 16),
                GetData(CreatureConstants.Yrthak, SizeConstants.Gargantuan, 17, 36)];
            testCases[CreatureConstants.Zelekhut] =
                [GetData(CreatureConstants.Zelekhut, SizeConstants.Large, 9, 16),
                GetData(CreatureConstants.Zelekhut, SizeConstants.Huge, 17, 24)];

            return testCases;
        }

        private string GetData(string creature, string advancedSize, int lowerHitDice, int upperHitDice)
        {
            var rawQuantity = creatureData[creature].GetEffectiveHitDiceQuantity(false);
            var creatureHitDiceQuantity = rawQuantity < 1 ? 0 : (int)rawQuantity;

            var selection = new AdvancementDataSelection
            {
                Reach = spaceReachHelper.GetAdvancedReach(creature, creatureData[creature].Size, creatureData[creature].Reach, advancedSize),
                Space = spaceReachHelper.GetAdvancedSpace(creatureData[creature].Size, creatureData[creature].Space, advancedSize),
                Size = advancedSize,
                AdditionalHitDiceRoll = RollHelper.GetRollWithMostEvenDistribution(creatureHitDiceQuantity, lowerHitDice, upperHitDice, true),
                StrengthAdjustment = GetStrengthAdjustment(creatureData[creature].Size, advancedSize),
                ConstitutionAdjustment = GetConstitutionAdjustment(creatureData[creature].Size, advancedSize),
                DexterityAdjustment = GetDexterityAdjustment(creatureData[creature].Size, advancedSize),
                NaturalArmorAdjustment = GetNaturalArmorAdjustment(creatureData[creature].Size, advancedSize),
                ChallengeRatingDivisor = GetChallengeRatingDivisor(creature),
                AdjustedChallengeRating = GetAdjustedChallengeRating(creatureData[creature].GetEffectiveChallengeRating(false), creatureData[creature].Size, advancedSize),
            };

            return DataHelper.Parse(selection);
        }

        private string GetAdjustedChallengeRating(string cr, string originalSize, string advancedSize)
        {
            var originalSizeIndex = Array.IndexOf(sizes, originalSize);
            var advancedIndex = Array.IndexOf(sizes, advancedSize);
            var largeIndex = Array.IndexOf(sizes, SizeConstants.Large);

            if (advancedIndex < largeIndex || originalSize == advancedSize)
            {
                return cr;
            }

            var increase = advancedIndex - Math.Max(largeIndex - 1, originalSizeIndex);
            return ChallengeRatingConstants.IncreaseChallengeRating(cr, increase);
        }

        private int GetChallengeRatingDivisor(string creature)
        {
            var creatureType = creatureData[creature].Types.First();
            return typeDivisors[creatureType];
        }

        private static int GetConstitutionAdjustment(string originalSize, string advancedSize)
        {
            var constitutionAdjustment = 0;
            var currentSize = originalSize;

            while (currentSize != advancedSize)
            {
                switch (currentSize)
                {
                    case SizeConstants.Fine: currentSize = SizeConstants.Diminutive; break;
                    case SizeConstants.Diminutive: currentSize = SizeConstants.Tiny; break;
                    case SizeConstants.Tiny: currentSize = SizeConstants.Small; break;
                    case SizeConstants.Small: currentSize = SizeConstants.Medium; constitutionAdjustment += 2; break;
                    case SizeConstants.Medium: currentSize = SizeConstants.Large; constitutionAdjustment += 4; break;
                    case SizeConstants.Large: currentSize = SizeConstants.Huge; constitutionAdjustment += 4; break;
                    case SizeConstants.Huge: currentSize = SizeConstants.Gargantuan; constitutionAdjustment += 4; break;
                    case SizeConstants.Gargantuan: currentSize = SizeConstants.Colossal; constitutionAdjustment += 4; break;
                    case SizeConstants.Colossal:
                    default: throw new ArgumentException($"{currentSize} is not a valid size that can be advanced");
                }
            }

            return constitutionAdjustment;
        }

        private static int GetDexterityAdjustment(string originalSize, string advancedSize)
        {
            var dexterityAdjustment = 0;
            var currentSize = originalSize;

            while (currentSize != advancedSize)
            {
                switch (currentSize)
                {
                    case SizeConstants.Fine: currentSize = SizeConstants.Diminutive; dexterityAdjustment -= 2; break;
                    case SizeConstants.Diminutive: currentSize = SizeConstants.Tiny; dexterityAdjustment -= 2; break;
                    case SizeConstants.Tiny: currentSize = SizeConstants.Small; dexterityAdjustment -= 2; break;
                    case SizeConstants.Small: currentSize = SizeConstants.Medium; dexterityAdjustment -= 2; break;
                    case SizeConstants.Medium: currentSize = SizeConstants.Large; dexterityAdjustment -= 2; break;
                    case SizeConstants.Large: currentSize = SizeConstants.Huge; dexterityAdjustment -= 2; break;
                    case SizeConstants.Huge: currentSize = SizeConstants.Gargantuan; break;
                    case SizeConstants.Gargantuan: currentSize = SizeConstants.Colossal; break;
                    case SizeConstants.Colossal:
                    default: throw new ArgumentException($"{currentSize} is not a valid size that can be advanced");
                }
            }

            return dexterityAdjustment;
        }

        private static int GetStrengthAdjustment(string originalSize, string advancedSize)
        {
            var strengthAdjustment = 0;
            var currentSize = originalSize;

            while (currentSize != advancedSize)
            {
                switch (currentSize)
                {
                    case SizeConstants.Fine: currentSize = SizeConstants.Diminutive; break;
                    case SizeConstants.Diminutive: currentSize = SizeConstants.Tiny; strengthAdjustment += 2; break;
                    case SizeConstants.Tiny: currentSize = SizeConstants.Small; strengthAdjustment += 4; break;
                    case SizeConstants.Small: currentSize = SizeConstants.Medium; strengthAdjustment += 4; break;
                    case SizeConstants.Medium: currentSize = SizeConstants.Large; strengthAdjustment += 8; break;
                    case SizeConstants.Large: currentSize = SizeConstants.Huge; strengthAdjustment += 8; break;
                    case SizeConstants.Huge: currentSize = SizeConstants.Gargantuan; strengthAdjustment += 8; break;
                    case SizeConstants.Gargantuan: currentSize = SizeConstants.Colossal; strengthAdjustment += 8; break;
                    case SizeConstants.Colossal:
                    default: throw new ArgumentException($"{currentSize} is not a valid size that can be advanced");
                }
            }

            return strengthAdjustment;
        }

        private static int GetNaturalArmorAdjustment(string originalSize, string advancedSize)
        {
            var naturalArmorAdjustment = 0;
            var currentSize = originalSize;

            while (currentSize != advancedSize)
            {
                switch (currentSize)
                {
                    case SizeConstants.Fine: currentSize = SizeConstants.Diminutive; break;
                    case SizeConstants.Diminutive: currentSize = SizeConstants.Tiny; break;
                    case SizeConstants.Tiny: currentSize = SizeConstants.Small; break;
                    case SizeConstants.Small: currentSize = SizeConstants.Medium; break;
                    case SizeConstants.Medium: currentSize = SizeConstants.Large; naturalArmorAdjustment += 2; break;
                    case SizeConstants.Large: currentSize = SizeConstants.Huge; naturalArmorAdjustment += 3; break;
                    case SizeConstants.Huge: currentSize = SizeConstants.Gargantuan; naturalArmorAdjustment += 4; break;
                    case SizeConstants.Gargantuan: currentSize = SizeConstants.Colossal; naturalArmorAdjustment += 5; break;
                    case SizeConstants.Colossal:
                    default: throw new ArgumentException($"{currentSize} is not a valid size that can be advanced");
                }
            }

            return naturalArmorAdjustment;
        }
    }
}
