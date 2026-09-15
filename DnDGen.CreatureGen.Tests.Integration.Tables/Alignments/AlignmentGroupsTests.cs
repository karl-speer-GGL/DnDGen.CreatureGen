using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.Infrastructure.Selectors.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Alignments
{
    [TestFixture]
    public class AlignmentGroupsTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.AlignmentGroups;

        private string[][] alignmentGrid;
        private string[] partialAlignmentsStarts;
        private string[] partialAlignmentsEnds;
        private string[] partialAlignments;
        private string[] creatures;
        private ICollectionDataSelector<CreatureDataSelection> creatureDataSelector;

        [SetUp]
        public void Setup()
        {
            alignmentGrid =
            [
                [AlignmentConstants.LawfulGood, AlignmentConstants.LawfulNeutral, AlignmentConstants.LawfulEvil],
                [AlignmentConstants.NeutralGood, AlignmentConstants.TrueNeutral, AlignmentConstants.NeutralEvil],
                [AlignmentConstants.ChaoticGood, AlignmentConstants.ChaoticNeutral, AlignmentConstants.ChaoticEvil],
            ];

            partialAlignmentsStarts =
            [
                AlignmentConstants.Chaotic,
                AlignmentConstants.Lawful,
            ];
            partialAlignmentsEnds =
            [
                AlignmentConstants.Good,
                AlignmentConstants.Evil,
                AlignmentConstants.Neutral,
            ];
            partialAlignments = [.. partialAlignmentsStarts, .. partialAlignmentsEnds, string.Empty];
            creatures = [.. CreatureConstants.GetAll()];

            creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
        }

        [Test]
        public void AlignmentGroupsNames()
        {
            var creatures = CreatureConstants.GetAll();
            var templates = CreatureConstants.Templates.GetAll();

            var names = creatures
                .Union(templates)
                .Union(templates.Select(c => c + GroupConstants.AllowedInput))
                .Union([GroupConstants.All]);

            AssertCollectionNames(names);
        }

        [Test]
        public void AllAlignmentGroup()
        {
            var allAlignments = new[]
            {
                AlignmentConstants.LawfulGood,
                AlignmentConstants.NeutralGood,
                AlignmentConstants.ChaoticGood,
                AlignmentConstants.LawfulNeutral,
                AlignmentConstants.TrueNeutral,
                AlignmentConstants.ChaoticNeutral,
                AlignmentConstants.LawfulEvil,
                AlignmentConstants.NeutralEvil,
                AlignmentConstants.ChaoticEvil,
            };

            Assert.That(allAlignments, Is.EquivalentTo(alignmentGrid.SelectMany(l => l)));
            AssertDistinctCollection(GroupConstants.All, allAlignments);
        }

        [TestCase(CreatureConstants.Aasimar, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.Good)]
        [TestCase(CreatureConstants.Aboleth, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Achaierai, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Allip, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Androsphinx, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_MultipleLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_MultipleLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Flexible, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_MultipleLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Sheetlike, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_TwoLegs, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wooden, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ankheg, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Annis, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Ant_Giant_Queen, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ant_Giant_Soldier, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ant_Giant_Worker, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ape, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ape_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Aranea, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Arrowhawk_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Arrowhawk_Elder, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Arrowhawk_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.AssassinVine, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Angel_AstralDeva, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Good)]
        [TestCase(CreatureConstants.Athach, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Avoral, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Azer, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.Babau, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Baboon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Badger, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Badger_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Balor, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.BarbedDevil_Hamatula, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Barghest, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Barghest_Greater, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Basilisk, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Basilisk_Greater, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bat, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bat_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bat_Swarm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bear_Black, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bear_Brown, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bear_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bear_Polar, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.BeardedDevil_Barbazu, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Bebilith, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Bee_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Behir, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Beholder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Beholder_Gauth, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Belker, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Bison, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.BlackPudding, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.BlackPudding_Elder, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.BlinkDog, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Boar, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Boar_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Bodak, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.BombardierBeetle_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.BoneDevil_Osyluth, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Bralani, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Bugbear, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Bulette, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Camel_Bactrian, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Camel_Dromedary, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.CarrionCrawler, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cat, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centaur, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Colossal, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Gargantuan, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Small, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Tiny, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Centipede_Swarm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.ChainDevil_Kyton, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.ChaosBeast, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Cheetah, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Chimera_Black, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Chimera_Blue, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Chimera_Green, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Chimera_Red, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Chimera_White, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Choker, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Chuul, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Cloaker, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Cockatrice, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Couatl, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Criosphinx, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_10Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_11Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_12Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_5Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_6Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_7Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_8Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Cryohydra_9Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Crocodile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Crocodile_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Darkmantle, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Deinonychus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Delver, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Derro, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Derro_Sane, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Destrachan, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Devourer, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Digester, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.DisplacerBeast, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.DisplacerBeast_PackLord, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Djinni, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Djinni_Noble, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dog, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Dog_Riding, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Donkey, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Doppelganger, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Dragon_Black_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Black_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Blue_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Bronze_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Copper_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Gold_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Green_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Green_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Red_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_Silver_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_Silver_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dragon_White_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_Ancient, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_MatureAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_Old, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_VeryYoung, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_Wyrmling, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_Young, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dragon_White_YoungAdult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.DragonTurtle, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Dragonne, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Dretch, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Drider, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Dryad, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Dwarf_Duergar, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Dwarf_Deep,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulNeutral,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Dwarf_Hill, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Dwarf_Mountain, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Eagle, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Eagle_Giant, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Efreeti, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Elasmosaurus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Air_Elder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Air_Greater, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Air_Huge, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Air_Large, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Air_Medium, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Air_Small, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Earth_Elder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Earth_Greater, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Earth_Huge, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Earth_Large, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Earth_Medium, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Earth_Small, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Fire_Elder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Fire_Greater, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Fire_Huge, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Fire_Large, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Fire_Medium, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Fire_Small, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Water_Elder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Water_Greater, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Water_Huge, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Water_Large, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Water_Medium, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elemental_Water_Small, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elephant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Elf_Aquatic, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Elf_Drow, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Elf_Gray, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Elf_Half,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Elf_High, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Elf_Wild, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Elf_Wood, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Erinyes, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.EtherealFilcher, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.EtherealMarauder, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ettercap, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Ettin, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.FireBeetle_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.FormianMyrmarch, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.FormianQueen, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.FormianTaskmaster, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.FormianWarrior, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.FormianWorker, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.FrostWorm, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Gargoyle, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Gargoyle_Kapoacinth, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.GelatinousCube, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ghaele, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Ghoul, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Ghoul_Ghast, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Ghoul_Lacedon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Giant_Cloud,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Giant_Fire, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Giant_Frost, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Giant_Hill, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Giant_Stone, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Giant_Stone_Elder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Giant_Storm, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.GibberingMouther, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Girallon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Githyanki, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Githzerai, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Neutral)]
        [TestCase(CreatureConstants.Glabrezu, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Gnoll, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Gnome_Forest, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Gnome_Rock, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Gnome_Svirfneblin, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Goblin, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Golem_Clay, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Golem_Flesh, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Golem_Iron, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Golem_Stone, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Golem_Stone_Greater, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Gorgon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.GrayRender, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.GreenHag, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Grick, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Griffon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Grig, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Grig_WithFiddle, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Grimlock, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Gynosphinx, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Halfling_Deep, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Halfling_Lightfoot, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Halfling_Tallfellow, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Harpy, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Hawk, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hellcat_Bezekira, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.HellHound, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.HellHound_NessianWarhound, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Hellwasp_Swarm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Hezrou, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Hieracosphinx, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Hippogriff, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hobgoblin, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Homunculus, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.HornedDevil_Cornugon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Horse_Heavy, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Horse_Heavy_War, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Horse_Light, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Horse_Light_War, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.HoundArchon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Howler, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Human, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Hydra_10Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_11Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_12Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_5Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_6Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_7Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_8Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hydra_9Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Hyena, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.IceDevil_Gelugon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Imp, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.InvisibleStalker, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Janni, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Kobold, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Kolyarut, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.Kraken, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Krenshar, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.KuoToa, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Lamia, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Lammasu, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Lemure, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.LanternArchon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Leonal, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Leopard, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Lillend, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Lion, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Lion_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Lizard, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Lizard_Monitor, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Lizardfolk, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Locathah, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Locust_Swarm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Magmin, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.MantaRay, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Manticore, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Marilith, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Marut, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(CreatureConstants.Medusa, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Megaraptor, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Air, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Dust, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Earth, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Fire, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Ice, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Magma, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Ooze, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Salt, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Steam, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mephit_Water, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Merfolk, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mimic, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.MindFlayer, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Minotaur, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Mohrg, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Monkey, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mule, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Mummy, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Naga_Dark, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Naga_Guardian, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Naga_Spirit, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Naga_Water, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Nalfeshnee, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.NightHag, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Nightcrawler, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Nightmare, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Nightmare_Cauchemar, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Nightwalker, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Nightwing, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Nixie, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Nymph, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Octopus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Octopus_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ogre, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Ogre_Merrow, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.OgreMage, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Orc, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Orc_Half,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.NeutralEvil,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Otyugh, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Owl, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Owl_Giant, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Owlbear, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.GrayOoze, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.OchreJelly, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pegasus, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.Angel_Planetar, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Good)]
        [TestCase(CreatureConstants.PhantomFungus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.PhaseSpider, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Phasm, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.PitFiend, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Pixie, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Pixie_WithIrresistibleDance, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Pony, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pony_War, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Porpoise, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.PrayingMantis_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pseudodragon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.PurpleWorm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_10Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_11Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_12Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_5Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_6Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_7Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_8Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Pyrohydra_9Heads, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Quasit, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Rakshasa, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Rast, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Rat, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Rat_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Rat_Swarm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Raven, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Ravid, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.RazorBoar, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Remorhaz, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Retriever, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Rhinoceras, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Roc, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Roper, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.RustMonster, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Sahuagin, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Sahuagin_Malenti, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Sahuagin_Mutant, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Salamander_Average, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Salamander_Flamebrother, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Salamander_Noble, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Satyr, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Satyr_WithPipes, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Colossal, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Gargantuan, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Small, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Tiny, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Scorpionfolk, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.SeaCat, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.SeaHag, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Shadow, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Shadow_Greater, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.ShadowMastiff, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.ShamblingMound, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Shark_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Shark_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Shark_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Shark_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.ShieldGuardian, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.ShockerLizard, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Shrieker, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Skum, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Slaad_Blue, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Slaad_Death, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Slaad_Gray, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Slaad_Green, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Slaad_Red, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Angel_Solar, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Good)]
        [TestCase(CreatureConstants.Snake_Constrictor, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Snake_Constrictor_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Snake_Viper_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Snake_Viper_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Snake_Viper_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Snake_Viper_Small, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Snake_Viper_Tiny, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spectre, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Colossal, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Gargantuan, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Small, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Tiny, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Colossal, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Huge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Large, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Medium, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Small, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Tiny, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Spider_Swarm, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.SpiderEater, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Squid, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Squid_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.StagBeetle_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Stirge, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Succubus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Tarrasque, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Tendriculos, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Thoqqua, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Tiefling, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Tiger, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Tiger_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Titan, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Chaotic)]
        [TestCase(CreatureConstants.Toad, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Tojanida_Adult, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Tojanida_Elder, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Tojanida_Juvenile, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Treant, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Triceratops, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Triton, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood)]
        [TestCase(CreatureConstants.Troglodyte, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Troll, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Troll_Scrag, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.TrumpetArchon, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood)]
        [TestCase(CreatureConstants.Tyrannosaurus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.UmberHulk, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.UmberHulk_TrulyHorrid, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Unicorn, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(CreatureConstants.VampireSpawn, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Vargouille, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.VioletFungus, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Vrock, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Wasp_Giant, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Weasel, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Weasel_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Whale_Baleen, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Whale_Cachalot, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Whale_Orca, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Wight, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.WillOWisp, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.WinterWolf, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Wolf, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Wolf_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Wolverine, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Wolverine_Dire, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Worg, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Wraith, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Wraith_Dread, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Wyvern, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Xill, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil)]
        [TestCase(CreatureConstants.Xorn_Average, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Xorn_Elder, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.Xorn_Minor, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.YethHound, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Yrthak, AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.TrueNeutral)]
        [TestCase(CreatureConstants.YuanTi_Abomination, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeArms, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeHead, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTail, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.YuanTi_Pureblood, AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Zelekhut, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        public void CreatureAlignmentGroup(string name, params string[] collection)
        {
            var alignments = new List<string>();
            foreach (var group in collection)
            {
                var (modifier, alignment) = ParseGroup(group);
                var alignmentGroup = ComputeAlignmentGroup(modifier, alignment);
                alignments.AddRange(alignmentGroup);
            }

            AssertCollection(name, [.. alignments]);
        }

        [TestCase(CreatureConstants.Templates.CelestialCreature, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Good)]
        [TestCase(CreatureConstants.Templates.FiendishCreature, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Templates.Ghost, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.HalfCelestial, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Good)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Black, CreatureConstants.Dragon_Black_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Blue, CreatureConstants.Dragon_Blue_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Brass, CreatureConstants.Dragon_Brass_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Bronze, CreatureConstants.Dragon_Bronze_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Copper, CreatureConstants.Dragon_Copper_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Gold, CreatureConstants.Dragon_Gold_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Green, CreatureConstants.Dragon_Green_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Red, CreatureConstants.Dragon_Red_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Silver, CreatureConstants.Dragon_Silver_Adult)]
        [TestCase(CreatureConstants.Templates.HalfDragon_White, CreatureConstants.Dragon_White_Adult)]
        [TestCase(CreatureConstants.Templates.HalfFiend, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Templates.Lich, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Afflicted, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Natural, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.None, AlignmentConstants.Modifiers.Any)]
        [TestCase(CreatureConstants.Templates.Skeleton, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        [TestCase(CreatureConstants.Templates.Vampire, AlignmentConstants.Modifiers.Any + "|" + AlignmentConstants.Evil)]
        [TestCase(CreatureConstants.Templates.Zombie, AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil)]
        public void TemplateAlignmentGroup(string name, params string[] collection)
        {
            var alignments = new List<string>();
            foreach (var group in collection)
            {
                var (modifier, alignment) = ParseGroup(group);
                var alignmentGroup = ComputeAlignmentGroup(modifier, alignment);
                alignments.AddRange(alignmentGroup);
            }

            AssertCollection(name, [.. alignments]);
        }

        private static (string modifier, string alignment) ParseGroup(string group)
        {
            var sections = group.Split('|');
            var modifier = sections[0];
            var alignment = sections.Length > 1 ? sections[1] : string.Empty;
            return (modifier, alignment);
        }

        [TestCase(AlignmentConstants.Modifiers.Any, "",
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Any, AlignmentConstants.Good,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Good)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.Good,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralGood,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.Good,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulGood,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralGood,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.Good,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulGood,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.NeutralGood,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.Modifiers.Any, AlignmentConstants.Neutral,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Neutral)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.Neutral,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.TrueNeutral,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.Neutral,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulNeutral,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.TrueNeutral,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.Neutral,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulNeutral,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.TrueNeutral,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Any, AlignmentConstants.Evil,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Evil)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.Evil,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.NeutralEvil,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.Evil,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.NeutralEvil,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.Evil,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulEvil,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.NeutralEvil,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Any, AlignmentConstants.Lawful,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Lawful)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.Lawful,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulEvil,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulGood,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.Lawful,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulEvil,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulGood,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.Lawful,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulEvil,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulGood,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Any, AlignmentConstants.Chaotic,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.Chaotic)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.Chaotic,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticGood,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.Modifiers.Always + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.Chaotic,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticGood,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.Modifiers.Usually + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.Chaotic,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticGood,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.Modifiers.Often + "|" + AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.LawfulGood, AlignmentConstants.LawfulGood)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.LawfulNeutral, AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.ChaoticEvil, AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.ChaoticGood, AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.ChaoticNeutral, AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.NeutralEvil, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.NeutralGood, AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.Modifiers.Always, AlignmentConstants.TrueNeutral, AlignmentConstants.TrueNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.TrueNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.Modifiers.Usually, AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.LawfulGood)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Modifiers.Often, AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.ChaoticEvil,
            AlignmentConstants.ChaoticEvil)]
        public void ComputedAlignmentGroupSpotCheck(string modifier, string alignment, params string[] collection)
        {
            var alignmentGroup = ComputeAlignmentGroup(modifier, alignment);

            var subgroups = collection.Where(c => c.Contains('|'));
            var expected = new List<string>(collection.Where(c => !c.Contains('|')));
            foreach (var subgroup in subgroups)
            {
                var (subModifier, subAlignment) = ParseGroup(subgroup);
                var subgroupCollection = ComputeAlignmentGroup(subModifier, subAlignment);
                expected.AddRange(subgroupCollection);
            }

            Assert.That(alignmentGroup, Is.EquivalentTo(expected));
        }

        private bool AlignmentMatchesPartial(string alignment, string partial)
        {
            return (partialAlignmentsStarts.Contains(partial) && alignment.StartsWith(partial))
                || (partialAlignmentsEnds.Contains(partial) && alignment.EndsWith(partial))
                || partial == string.Empty;
        }

        private List<string> ComputeAlignmentGroup(string modifier, string alignment)
        {
            if (creatures.Contains(modifier))
            {
                return [.. table[modifier]];
            }

            if (partialAlignments.Contains(alignment))
            {
                var matchedAlignments = alignmentGrid.SelectMany(a => a).Where(a => AlignmentMatchesPartial(a, alignment));
                var alignments = new List<string>();

                foreach (var matchedAlignment in matchedAlignments)
                {
                    var computedAlignments = ComputeAlignmentGroup(modifier, matchedAlignment);
                    alignments.AddRange(computedAlignments);
                }

                return alignments;
            }

            var (l, g) = GetAlignmentIndex(alignment);

            if (l < 0 || g < 0)
                throw new ArgumentException($"Alignment {alignment} is not supported");

            //INFO: Needed to make True Neutral the majority in the Usually group
            var trueNeutralPadding = alignment == AlignmentConstants.TrueNeutral ? new[] { alignment } : [];

            return modifier switch
            {
                AlignmentConstants.Modifiers.Often => TraverseAlignmentGrid(l, g, 2),
                AlignmentConstants.Modifiers.Usually => [.. TraverseAlignmentGrid(l, g, 1), .. trueNeutralPadding],
                AlignmentConstants.Modifiers.Any => TraverseAlignmentGrid(l, g, 0),
                AlignmentConstants.Modifiers.Always => TraverseAlignmentGrid(l, g, 0),
                _ => throw new ArgumentException($"Modifier {modifier} is not supported"),
            };
        }

        private (int l, int g) GetAlignmentIndex(string alignment)
        {
            for (var l = 0; l < alignmentGrid.Length; l++)
                for (var g = 0; g < alignmentGrid[l].Length; g++)
                    if (alignmentGrid[l][g] == alignment)
                        return (l, g);

            return (-1, -1);
        }

        private List<string> TraverseAlignmentGrid(int l, int g, int travel)
        {
            if (travel < 0 || l < 0 || g < 0 || l >= alignmentGrid.Length || g >= alignmentGrid[l].Length)
                return [];

            var alignments = new List<string>();
            while (alignments.Count < travel + 1)
                alignments.Add(alignmentGrid[l][g]);

            alignments.AddRange(TraverseAlignmentGrid(l, g, travel - 1));
            alignments.AddRange(TraverseAlignmentGrid(l - 1, g, travel - 1));
            alignments.AddRange(TraverseAlignmentGrid(l + 1, g, travel - 1));
            alignments.AddRange(TraverseAlignmentGrid(l, g - 1, travel - 1));
            alignments.AddRange(TraverseAlignmentGrid(l, g + 1, travel - 1));

            return alignments;
        }

        [TestCase(AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.LawfulGood)]
        [TestCase(AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.TrueNeutral)]
        public void AlwaysAlignmentIsOnlyAlignment(string alignment)
        {
            var alignmentGroup = ComputeAlignmentGroup(AlignmentConstants.Modifiers.Always, alignment);
            Assert.That(alignmentGroup, Is.EqualTo([alignment]));
        }

        [TestCase(AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.LawfulGood)]
        [TestCase(AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.TrueNeutral)]
        public void UsuallyAlignmentHasMajority(string alignment)
        {
            var alignmentGroup = ComputeAlignmentGroup(AlignmentConstants.Modifiers.Usually, alignment);

            var alignmentCount = alignmentGroup.Count(a => a == alignment);
            var halfCount = alignmentGroup.Count / 2d;
            Assert.That(alignmentCount, Is.AtLeast(halfCount));
        }

        [TestCase(AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.LawfulGood)]
        [TestCase(AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.TrueNeutral)]
        public void OftenAlignmentIsMode(string alignment)
        {
            var alignmentGroup = ComputeAlignmentGroup(AlignmentConstants.Modifiers.Often, alignment);

            var modeCount = alignmentGroup
                .GroupBy(a => a)
                .Max(g => g.Count());
            var mode = alignmentGroup
                .GroupBy(a => a)
                .Where(g => g.Count() == modeCount)
                .Select(g => g.Key)
                .Single();

            Assert.That(mode, Is.EqualTo(alignment));

            //INFO: Ideally, for often, while the alignment is the mode, it is not the outright majority
            var alignmentCount = alignmentGroup.Count(a => a == alignment);
            var halfCount = alignmentGroup.Count / 2d;
            Assert.That(alignmentCount, Is.AtMost(halfCount));
        }

        [TestCase(CreatureConstants.Types.Animal)]
        [TestCase(CreatureConstants.Types.Vermin)]
        public void AllCreaturesOfTypeAreTrueNeutral(string creatureType)
        {
            var creatures = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData)
                .Where(kvp => kvp.Value.Single().Types.Contains(creatureType))
                .Select(kvp => kvp.Key);

            foreach (var creature in creatures)
            {
                Assert.That(table, Contains.Key(creature), creature);
                Assert.That(table[creature], Has.Count.EqualTo(1).And.Contains(AlignmentConstants.TrueNeutral), creature);
            }
        }

        [TestCase(CreatureConstants.Types.Subtypes.Lawful, AlignmentConstants.Lawful)]
        [TestCase(CreatureConstants.Types.Subtypes.Chaotic, AlignmentConstants.Chaotic)]
        [TestCase(CreatureConstants.Types.Subtypes.Good, AlignmentConstants.Good)]
        [TestCase(CreatureConstants.Types.Subtypes.Evil, AlignmentConstants.Evil)]
        public void CreatureOfAlignmentTypeIsAtLeastUsuallyThatAlignment(string subtype, string partialAlignment)
        {
            var creatures = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData)
                .Where(kvp => kvp.Value.Single().Types.Contains(subtype))
                .Select(kvp => kvp.Key);

            foreach (var creature in creatures)
            {
                Assert.That(table, Contains.Key(creature), creature);

                var halfCount = table[creature].Count() / 2d;

                if (partialAlignmentsStarts.Contains(partialAlignment))
                    Assert.That(table[creature].Count(a => a.StartsWith(partialAlignment)), Is.AtLeast(halfCount), creature);
                else if (partialAlignmentsEnds.Contains(partialAlignment))
                    Assert.That(table[creature].Count(a => a.EndsWith(partialAlignment)), Is.AtLeast(halfCount), creature);
                else
                    Assert.Fail($"Partial alignment {partialAlignment} is not a supported partial alignment");
            }
        }

        [TestCase(CreatureConstants.Templates.CelestialCreature,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Templates.FiendishCreature,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Ghost,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfCelestial,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Black,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Blue,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Brass,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Bronze,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Copper,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Gold,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Green,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Red,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_Silver,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfDragon_White,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.HalfFiend,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lich,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Afflicted,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Natural,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.None,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Skeleton,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Vampire,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        [TestCase(CreatureConstants.Templates.Zombie,
            AlignmentConstants.LawfulGood,
            AlignmentConstants.NeutralGood,
            AlignmentConstants.ChaoticGood,
            AlignmentConstants.LawfulNeutral,
            AlignmentConstants.TrueNeutral,
            AlignmentConstants.ChaoticNeutral,
            AlignmentConstants.LawfulEvil,
            AlignmentConstants.NeutralEvil,
            AlignmentConstants.ChaoticEvil)]
        public void TemplateAllowedAlignmentInputs(string template, params string[] alignments)
        {
            Assert.That(table.Keys, Contains.Item(template + GroupConstants.AllowedInput));
            Assert.That(table[template + GroupConstants.AllowedInput], Is.EquivalentTo(alignments));
        }
    }
}
