using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.Tables.Helpers;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures
{
    [TestFixture]
    public class WeightsTests : TypesAndAmountsTests
    {
        private Dice dice;
        private ICollectionDataSelector<CreatureDataSelection> creatureDataSelector;
        private MeasurementHelper measurementHelper;
        private const int BASE_INDEX = 1;
        private const int MULTIPLIER_INDEX = 0;

        protected override string TableName => TableNameConstants.TypeAndAmount.Weights;

        private Dictionary<string, Dictionary<string, string>> heights;
        private Dictionary<string, Dictionary<string, string>> lengths;
        private Dictionary<string, Dictionary<string, (int Lower, int Upper)>> creatureWeightRanges;
        private Dictionary<string, Dictionary<string, string>> creatureWeightRolls;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var typeAndAmountSelector = GetNewInstanceOf<ICollectionTypeAndAmountSelector>();
            heights = typeAndAmountSelector.SelectAllFrom(Config.Name, TableNameConstants.TypeAndAmount.Heights)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(v => v.Type, v => v.Roll));
            lengths = typeAndAmountSelector.SelectAllFrom(Config.Name, TableNameConstants.TypeAndAmount.Lengths)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(v => v.Type, v => v.Roll));

            creatureWeightRanges = GetCreatureWeightRanges();
            creatureWeightRolls = GetCreatureWeightRolls();
        }

        [SetUp]
        public void Setup()
        {
            dice = GetNewInstanceOf<Dice>();
            creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
            measurementHelper = GetNewInstanceOf<MeasurementHelper>();
        }

        [Test]
        public void WeightsNames()
        {
            var creatures = CreatureConstants.GetAll();
            var templates = CreatureConstants.Templates.GetAll();
            var names = creatures.Union(templates);
            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureWeights(string name)
        {
            Assert.That(creatureWeightRolls, Contains.Key(name), $"TEST DATA: {name}");

            var rolls = creatureWeightRolls[name];
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, name);
            Assert.That(rolls.Keys, Is.EquivalentTo(genders.Union([name])), $"TEST DATA: {name}");

            foreach (var roll in rolls.Values)
            {
                Assert.That(roll, Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"));

                var isValid = dice.Roll(roll).IsValid();
                Assert.That(isValid, Is.True, roll, $"TEST DATA: {name}");
            }

            AssertIncorporealCreaturesAreWeightless(name);
            AssertTypesAndAmounts(name, rolls);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void TemplateWeights(string name)
        {
            Assert.That(creatureWeightRolls, Contains.Key(name), $"TEST DATA: {name}");

            var rolls = creatureWeightRolls[name];
            Assert.That(rolls.Keys, Is.EquivalentTo([name]), $"TEST DATA: {name}");

            foreach (var roll in rolls.Values)
            {
                Assert.That(roll, Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"));

                var isValid = dice.Roll(roll).IsValid();
                Assert.That(isValid, Is.True, roll, $"TEST DATA: {name}");
                Assert.That(roll, Is.AnyOf(["-1", "0", "1"]), $"TEST DATA: {name}");
            }

            AssertTypesAndAmounts(name, rolls);
        }

        [Test]
        public void CreatureWeightRollsAreValidForRanges()
        {
            foreach (var kvp in creatureWeightRanges)
            {
                var creature = kvp.Key;

                var ranges = kvp.Value.ToDictionary(g => g.Key, g => g.Value.Upper - g.Value.Lower);
                var formattedRanges = string.Join("; ", ranges.Select(r => $"{r.Key}: {r.Value}"));
                Assert.That(ranges.Values, Is.Not.Empty, $"{creature} has no genders");
                Assert.That(ranges.Values, Is.All.EqualTo(ranges.Values.First()), $"{creature} dimorphic ranges unequal. {formattedRanges}");

                var modifierRolls = kvp.Value.ToDictionary(g => g.Key, g => GetMultiplierFromRange(creature, g.Value.Lower, g.Value.Upper));
                var formattedModifierRolls = string.Join("; ", modifierRolls.Select(r => $"{r.Key}: {r.Value}"));
                Assert.That(modifierRolls.Values, Is.Not.Empty, $"{creature} has no genders");
                Assert.That(modifierRolls.Values, Is.All.EqualTo(modifierRolls.Values.First()), $"{creature} dimorphic modifier rolls unequal. {formattedModifierRolls}");

                foreach (var genderKvp in kvp.Value)
                {
                    var gender = genderKvp.Key;
                    var range = genderKvp.Value;

                    AssertRollRange(creature, gender, range.Lower, range.Upper, false);
                }
            }
        }

        private void AssertRollRange(string creature, string gender, int lower, int upper, bool exact)
        {
            var heightLength = GetMaxOfHeightLength(creature);
            Assert.That(heightLength, Is.Not.Empty.And.ContainKey(creature).And.ContainKey(gender));

            var multiplierMin = dice.Roll(heightLength[creature]).AsPotentialMinimum();
            var multiplierMax = dice.Roll(heightLength[creature]).AsPotentialMaximum();

            Assert.That(creatureWeightRolls, Contains.Key(creature));
            Assert.That(creatureWeightRolls[creature], Contains.Key(creature).And.ContainKey(gender));

            var baseRoll = creatureWeightRolls[creature][gender];
            var modifierRoll = creatureWeightRolls[creature][creature];
            Assert.That(baseRoll, Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"), $"Base; {gender} {creature}");
            Assert.That(modifierRoll, Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"), $"Modifier; {gender} {creature}");

            var weightBaseMin = dice.Roll(baseRoll).AsPotentialMinimum();
            var weightBaseMax = dice.Roll(baseRoll).AsPotentialMaximum();
            var modifierMin = dice.Roll(modifierRoll).AsPotentialMinimum();
            var modifierMax = dice.Roll(modifierRoll).AsPotentialMaximum();
            var minSigma = exact ? 0 : new[] { 1d, (upper + lower) * 0.025, multiplierMin / 2d }.Max();
            var maxSigma = exact ? 0 : new[] { 1d, (upper + lower) * 0.025, multiplierMax / 2d }.Max();

            if (lower == 0 && upper == 0)
            {
                Assert.That(weightBaseMin + multiplierMin * modifierMin, Is.Zero,
                    $"Min; {gender} {creature}; Base: {weightBaseMin}; Mult: {multiplierMin}; Wm: {modifierMin}");
                Assert.That(weightBaseMax + multiplierMax * modifierMax, Is.Zero,
                    $"Max; {gender} {creature}; Base: {weightBaseMax}; Mult: {multiplierMax}; Wm: {modifierMax}");
                return;
            }

            Assert.That(weightBaseMin + multiplierMin * modifierMin, Is.Positive.And.EqualTo(lower).Within(minSigma),
                $"Min; {gender} {creature}; Base: {weightBaseMin}; rollM: {heightLength[creature]}; rollW: {modifierRoll}");
            Assert.That(weightBaseMax + multiplierMax * modifierMax, Is.Positive.And.EqualTo(upper).Within(maxSigma),
                $"Max; {gender} {creature}; Base: {weightBaseMax}; rollM: {heightLength[creature]}; rollW: {modifierRoll}");
        }

        private Dictionary<string, Dictionary<string, (int Lower, int Upper)>> GetCreatureWeightRanges()
        {
            var creatures = CreatureConstants.GetAll();
            var weights = new Dictionary<string, Dictionary<string, (int Lower, int Upper)>>();

            foreach (var creature in creatures)
            {
                weights[creature] = [];
            }

            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Human][GenderConstants.Female] = (85 + 2 * 2, 85 + 2 * 10 * 2 * 4);
            weights[CreatureConstants.Human][GenderConstants.Male] = (120 + 2 * 2, 120 + 2 * 10 * 2 * 4);

            //Source: https://forgottenrealms.fandom.com/wiki/Aasimar
            weights[CreatureConstants.Aasimar][GenderConstants.Female] = (89, 245);
            weights[CreatureConstants.Aasimar][GenderConstants.Male] = (124, 280);
            //Source: https://forgottenrealms.fandom.com/wiki/Aboleth
            weights[CreatureConstants.Aboleth][GenderConstants.Hermaphrodite] = GetRangeFromUpTo(6500);
            //Source: https://www.d20srd.org/srd/monsters/achaierai.htm
            weights[CreatureConstants.Achaierai][GenderConstants.Female] = GetRangeFromAverage(750);
            weights[CreatureConstants.Achaierai][GenderConstants.Male] = GetRangeFromAverage(750);
            //Incorporeal, so weight is 0
            weights[CreatureConstants.Allip][GenderConstants.Female] = (0, 0);
            weights[CreatureConstants.Allip][GenderConstants.Male] = (0, 0);
            weights[CreatureConstants.Allip][CreatureConstants.Allip] = (0, 0);
            //Source: https://www.mojobob.com/roleplay/monstrousmanual/s/sphinx.html
            weights[CreatureConstants.Androsphinx][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://forgottenrealms.fandom.com/wiki/Astral_Deva
            weights[CreatureConstants.Angel_AstralDeva][GenderConstants.Female] = GetRangeFromAverage(250);
            weights[CreatureConstants.Angel_AstralDeva][GenderConstants.Male] = GetRangeFromAverage(250);
            //Source: https://forgottenrealms.fandom.com/wiki/Planetar
            weights[CreatureConstants.Angel_Planetar][GenderConstants.Female] = GetRangeFromAverage(500);
            weights[CreatureConstants.Angel_Planetar][GenderConstants.Male] = GetRangeFromAverage(500);
            //Source: https://forgottenrealms.fandom.com/wiki/Solar
            weights[CreatureConstants.Angel_Solar][GenderConstants.Female] = GetRangeFromAverage(500);
            weights[CreatureConstants.Angel_Solar][GenderConstants.Male] = GetRangeFromAverage(500);
            //Source: https://www.d20srd.org/srd/combat/movementPositionAndDistance.htm
            weights[CreatureConstants.AnimatedObject_Colossal][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_Flexible][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_Sheetlike][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_TwoLegs][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Colossal_Wooden][GenderConstants.Agender] = (125 * 2000, 1000 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_Flexible][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_Sheetlike][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Gargantuan_Wooden][GenderConstants.Agender] = (16 * 2000, 125 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_Flexible][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_Sheetlike][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_TwoLegs][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_Wheels_Wooden][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Huge_Wooden][GenderConstants.Agender] = (2 * 2000, 16 * 2000);
            weights[CreatureConstants.AnimatedObject_Large][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_Flexible][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_Sheetlike][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_TwoLegs][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_Wheels_Wooden][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Large_Wooden][GenderConstants.Agender] = (500, 2 * 2000);
            weights[CreatureConstants.AnimatedObject_Medium][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_Flexible][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_MultipleLegs][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_Sheetlike][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_TwoLegs][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_Wheels_Wooden][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Medium_Wooden][GenderConstants.Agender] = (60, 500);
            weights[CreatureConstants.AnimatedObject_Small][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_Flexible][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_MultipleLegs][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_Sheetlike][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_TwoLegs][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_Wheels_Wooden][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Small_Wooden][GenderConstants.Agender] = (8, 60);
            weights[CreatureConstants.AnimatedObject_Tiny][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_Flexible][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_MultipleLegs][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_Sheetlike][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_TwoLegs][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden][GenderConstants.Agender] = (1, 8);
            weights[CreatureConstants.AnimatedObject_Tiny_Wooden][GenderConstants.Agender] = (1, 8);
            //Source: https://www.d20srd.org/srd/monsters/ankheg.htm
            weights[CreatureConstants.Ankheg][GenderConstants.Female] = GetRangeFromAverage(800);
            weights[CreatureConstants.Ankheg][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://www.d20srd.org/srd/monsters/hag.htm#annis
            weights[CreatureConstants.Annis][GenderConstants.Female] = GetRangeFromAverage(325);
            //Source: https://www.d20srd.org/srd/monsters/giantAnt.htm
            //https://www.findingdulcinea.com/how-much-does-an-ant-weigh/
            //https://www.dimensions.com/element/black-garden-ant-lasius-niger - scale up
            weights[CreatureConstants.Ant_Giant_Worker][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetMilligramToPound(1), GetMilligramToPound(2), .14, .2, 6 * 12); //(206, 300);
            weights[CreatureConstants.Ant_Giant_Soldier][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetMilligramToPound(1), GetMilligramToPound(2), .14, .2, 6 * 12); //(206, 300);
            //https://www.dimensions.com/element/black-garden-ant-lasius-niger - scale up, up to 10mg
            //https://www.retirefearless.com/post/how-much-does-an-ant-weigh#spanblack-garden-antspan
            weights[CreatureConstants.Ant_Giant_Queen][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetMilligramToPound(10) * 9 / 11, GetMilligramToPound(10), .31, .35, 9 * 12); //(648, 932);
            //Source: https://www.dimensions.com/element/eastern-lowland-gorilla-gorilla-beringei-graueri (using for female with male range)
            //https://www.d20srd.org/srd/monsters/ape.htm (male)
            weights[CreatureConstants.Ape][GenderConstants.Female] = (220, 320);
            weights[CreatureConstants.Ape][GenderConstants.Male] = (300, 400);
            //Source: https://www.d20srd.org/srd/monsters/direApe.htm
            weights[CreatureConstants.Ape_Dire][GenderConstants.Female] = (800, 1200);
            weights[CreatureConstants.Ape_Dire][GenderConstants.Male] = (800, 1200);
            //Source: https://www.d20srd.org/srd/monsters/aranea.htm
            weights[CreatureConstants.Aranea][GenderConstants.Female] = GetRangeFromAverage(150);
            weights[CreatureConstants.Aranea][GenderConstants.Male] = GetRangeFromAverage(150);
            //Source: https://www.d20srd.org/srd/monsters/arrowhawk.htm
            weights[CreatureConstants.Arrowhawk_Juvenile][GenderConstants.Female] = GetRangeFromAverage(20);
            weights[CreatureConstants.Arrowhawk_Juvenile][GenderConstants.Male] = GetRangeFromAverage(20);
            weights[CreatureConstants.Arrowhawk_Adult][GenderConstants.Female] = GetRangeFromAverage(100);
            weights[CreatureConstants.Arrowhawk_Adult][GenderConstants.Male] = GetRangeFromAverage(100);
            weights[CreatureConstants.Arrowhawk_Elder][GenderConstants.Female] = GetRangeFromAverage(800);
            weights[CreatureConstants.Arrowhawk_Elder][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://www.epicpath.org/index.php/Assassin_Vine
            weights[CreatureConstants.AssassinVine][GenderConstants.Agender] = GetRangeFromAverage(50);
            //Source: https://www.d20srd.org/srd/monsters/athach.htm
            weights[CreatureConstants.Athach][GenderConstants.Female] = GetRangeFromAverage(4500);
            weights[CreatureConstants.Athach][GenderConstants.Male] = GetRangeFromAverage(4500);
            //Source: https://forgottenrealms.fandom.com/wiki/Avoral
            weights[CreatureConstants.Avoral][GenderConstants.Female] = GetRangeFromAverage(120);
            weights[CreatureConstants.Avoral][GenderConstants.Male] = GetRangeFromAverage(120);
            weights[CreatureConstants.Avoral][GenderConstants.Agender] = GetRangeFromAverage(120);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/azer-article
            weights[CreatureConstants.Azer][GenderConstants.Female] = (160, 220);
            weights[CreatureConstants.Azer][GenderConstants.Male] = (160, 220);
            weights[CreatureConstants.Azer][GenderConstants.Agender] = (160, 220);
            //Source: https://forgottenrealms.fandom.com/wiki/Babau
            weights[CreatureConstants.Babau][GenderConstants.Agender] = GetRangeFromAverage(140);
            //Source: https://www.dimensions.com/element/mandrill-mandrillus-sphinx
            //https://www.d20srd.org/srd/monsters/baboon.htm (male) (use range from female)
            weights[CreatureConstants.Baboon][GenderConstants.Female] = (26, 82);
            weights[CreatureConstants.Baboon][GenderConstants.Male] = (34, 90);
            //Source: https://www.d20srd.org/srd/monsters/badger.htm
            weights[CreatureConstants.Badger][GenderConstants.Female] = (25, 35);
            weights[CreatureConstants.Badger][GenderConstants.Male] = (25, 35);
            //Source: https://www.d20srd.org/srd/monsters/direBadger.htm
            weights[CreatureConstants.Badger_Dire][GenderConstants.Female] = GetRangeFromUpTo(500);
            weights[CreatureConstants.Badger_Dire][GenderConstants.Male] = GetRangeFromUpTo(500);
            //Source: https://www.d20srd.org/srd/monsters/demon.htm#balor
            weights[CreatureConstants.Balor][GenderConstants.Agender] = GetRangeFromAverage(4500);
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#barbedDevilHamatula
            weights[CreatureConstants.BarbedDevil_Hamatula][GenderConstants.Agender] = GetRangeFromAverage(300);
            //Source: https://www.d20srd.org/srd/monsters/barghest.htm
            //Average from 180 doesn't have nice rolls, so using alternate average
            weights[CreatureConstants.Barghest][GenderConstants.Female] = GetRangeFromAverage(180);
            //weights[CreatureConstants.Barghest][GenderConstants.Female] = GetRangeFromAverage(180, 200);
            weights[CreatureConstants.Barghest][GenderConstants.Male] = GetRangeFromAverage(180);
            //weights[CreatureConstants.Barghest][GenderConstants.Male] = GetRangeFromAverage(180, 200);
            weights[CreatureConstants.Barghest_Greater][GenderConstants.Female] = GetRangeFromAverage(400);
            weights[CreatureConstants.Barghest_Greater][GenderConstants.Male] = GetRangeFromAverage(400);
            //Source: https://forgottenrealms.fandom.com/wiki/Basilisk
            weights[CreatureConstants.Basilisk][GenderConstants.Female] = GetRangeFromAverage(300);
            weights[CreatureConstants.Basilisk][GenderConstants.Male] = GetRangeFromAverage(300);
            //Scaling up. Since 1 size category bigger, x8
            weights[CreatureConstants.Basilisk_Greater][GenderConstants.Female] = GetRangeFromAverage(300 * 8);
            weights[CreatureConstants.Basilisk_Greater][GenderConstants.Male] = GetRangeFromAverage(300 * 8);
            //Source: https://www.dimensions.com/element/little-brown-bat-myotis-lucifugus
            weights[CreatureConstants.Bat][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Bat][GenderConstants.Male] = GetRangeFromAverage(1);
            //Source: https://www.d20srd.org/srd/monsters/direBat.htm
            weights[CreatureConstants.Bat_Dire][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.Bat_Dire][GenderConstants.Male] = GetRangeFromAverage(200);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm Bats are Diminutive, so (.18oz-.46oz)x5000
            weights[CreatureConstants.Bat_Swarm][GenderConstants.Agender] = ((int)(GetOunceToPound(.18) * 5000), (int)(GetOunceToPound(.46) * 5000));
            //Source: https://www.dimensions.com/element/american-black-bear Using female range, adjusting male lower
            weights[CreatureConstants.Bear_Black][GenderConstants.Female] = (200, 450);
            weights[CreatureConstants.Bear_Black][GenderConstants.Male] = (350, 600);
            //Source: https://www.d20srd.org/srd/monsters/bearBrown.htm
            weights[CreatureConstants.Bear_Brown][GenderConstants.Female] = GetRangeFromAtLeast(1800);
            weights[CreatureConstants.Bear_Brown][GenderConstants.Male] = GetRangeFromAtLeast(1800);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_bear
            weights[CreatureConstants.Bear_Dire][GenderConstants.Female] = GetRangeFromAverage(8000);
            weights[CreatureConstants.Bear_Dire][GenderConstants.Male] = GetRangeFromAverage(8000);
            //Source: https://www.dimensions.com/element/polar-bears Using male range, adjusting female upper
            weights[CreatureConstants.Bear_Polar][GenderConstants.Female] = (330, 1055);
            weights[CreatureConstants.Bear_Polar][GenderConstants.Male] = (775, 1500);
            //Source: https://forgottenrealms.fandom.com/wiki/Barbazu
            weights[CreatureConstants.BeardedDevil_Barbazu][GenderConstants.Agender] = GetRangeFromAverage(225);
            //Source: https://forgottenrealms.fandom.com/wiki/Bebilith
            weights[CreatureConstants.Bebilith][GenderConstants.Agender] = GetRangeFromAtLeast(4000);
            //Source: https://www.d20srd.org/srd/monsters/giantBee.htm
            //https://www.dimensions.com/element/western-honey-bee-apis-mellifera scale up
            weights[CreatureConstants.Bee_Giant][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetMilligramToPound(115), GetMilligramToPound(128), .39, .59, 5 * 12); //(297, 923);
            //Source: https://forgottenrealms.fandom.com/wiki/Behir
            weights[CreatureConstants.Behir][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Behir][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: http://thecampaign20xx.blogspot.com/2015/07/dungeons-dragons-guide-to-beholder.html
            weights[CreatureConstants.Beholder][GenderConstants.Agender] = GetRangeFromAverage(4500);
            //Scaling down from Beholder. /2 length, so /8 weight
            weights[CreatureConstants.Beholder_Gauth][GenderConstants.Agender] = GetRangeFromAverage(4500 / 8);
            //Copying from Large air elemental
            weights[CreatureConstants.Belker][GenderConstants.Agender] = GetRangeFromAverage(4);
            //Source: https://www.d20srd.org/srd/monsters/bison.htm
            weights[CreatureConstants.Bison][GenderConstants.Female] = (1800, 2400);
            weights[CreatureConstants.Bison][GenderConstants.Male] = (1800, 2400);
            //Source: https://forgottenrealms.fandom.com/wiki/Black_pudding
            weights[CreatureConstants.BlackPudding][GenderConstants.Agender] = GetRangeFromAverage(18_000);
            //Elder is a size category up, so x8 weight
            weights[CreatureConstants.BlackPudding_Elder][GenderConstants.Agender] = GetRangeFromAverage(18_000 * 8);
            //Source: https://www.d20pfsrd.com/bestiary/monster-listings/magical-beasts/blink-dog
            weights[CreatureConstants.BlinkDog][GenderConstants.Female] = GetRangeFromAtLeast(180);
            weights[CreatureConstants.BlinkDog][GenderConstants.Male] = GetRangeFromAtLeast(180);
            //Source: https://www.dimensions.com/element/wild-boar
            weights[CreatureConstants.Boar][GenderConstants.Female] = (150, 220);
            weights[CreatureConstants.Boar][GenderConstants.Male] = (150, 220);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_boar
            weights[CreatureConstants.Boar_Dire][GenderConstants.Female] = GetRangeFromAverage(1200);
            weights[CreatureConstants.Boar_Dire][GenderConstants.Male] = GetRangeFromAverage(1200);
            //INFO: Basing off of humans
            weights[CreatureConstants.Bodak][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.Bodak][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://factanimal.com/bombardier-beetle/ up to 1g.
            //https://www.d20srd.org/srd/monsters/giantBombardierBeetle.htm scale up
            weights[CreatureConstants.BombardierBeetle_Giant][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(1) * 9 / 11, GetGramToPound(1), .9, 1.1, 6 * 12);
            weights[CreatureConstants.BombardierBeetle_Giant][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(1) * 9 / 11, GetGramToPound(1), .9, 1.1, 6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Osyluth
            weights[CreatureConstants.BoneDevil_Osyluth][GenderConstants.Agender] = GetRangeFromAverage(500);
            //Source: https://forgottenrealms.fandom.com/wiki/Bralani
            weights[CreatureConstants.Bralani][GenderConstants.Female] = (113, 140);
            weights[CreatureConstants.Bralani][GenderConstants.Male] = (128, 155);
            //Source: https://forgottenrealms.fandom.com/wiki/Bugbear
            weights[CreatureConstants.Bugbear][GenderConstants.Female] = (250, 350);
            weights[CreatureConstants.Bugbear][GenderConstants.Male] = (250, 350);
            //Source: http://gurpswiki.wikidot.com/m:bulette
            weights[CreatureConstants.Bulette][GenderConstants.Female] = GetRangeFromAverage(1200);
            weights[CreatureConstants.Bulette][GenderConstants.Male] = GetRangeFromAverage(1200);
            //Source: https://www.dimensions.com/element/bactrian-camel
            weights[CreatureConstants.Camel_Bactrian][GenderConstants.Female] = (990, 1100);
            weights[CreatureConstants.Camel_Bactrian][GenderConstants.Male] = (990, 1100);
            //Source: https://www.dimensions.com/element/dromedary-camel
            weights[CreatureConstants.Camel_Dromedary][GenderConstants.Female] = (880, 1320);
            weights[CreatureConstants.Camel_Dromedary][GenderConstants.Male] = (880, 1320);
            //Source: https://forgottenrealms.fandom.com/wiki/Carrion_crawler
            weights[CreatureConstants.CarrionCrawler][GenderConstants.Female] = GetRangeFromAverage(500);
            weights[CreatureConstants.CarrionCrawler][GenderConstants.Male] = GetRangeFromAverage(500);
            //Source: https://www.dimensions.com/element/american-shorthair-cat
            weights[CreatureConstants.Cat][GenderConstants.Female] = (10, 15);
            weights[CreatureConstants.Cat][GenderConstants.Male] = (10, 15);
            //Source: https://forgottenrealms.fandom.com/wiki/Centaur
            weights[CreatureConstants.Centaur][GenderConstants.Female] = GetRangeFromAverage(2100);
            weights[CreatureConstants.Centaur][GenderConstants.Male] = GetRangeFromAverage(2100);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Tiny
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Tiny][GenderConstants.Female] = GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 24);
            weights[CreatureConstants.Centipede_Monstrous_Tiny][GenderConstants.Male] = GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 24);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Small
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Small][GenderConstants.Female] = GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 48);
            weights[CreatureConstants.Centipede_Monstrous_Small][GenderConstants.Male] = GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 48);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Medium
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Medium][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 8 * 12);
            weights[CreatureConstants.Centipede_Monstrous_Medium][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 8 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Large
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Large][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 15 * 12);//(56, 200);
            weights[CreatureConstants.Centipede_Monstrous_Large][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 15 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Huge
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Huge][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 30 * 12);//(450, 1607);
            weights[CreatureConstants.Centipede_Monstrous_Huge][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 30 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Gargantuan
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Gargantuan][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 60 * 12);//(3599, 12_857);
            weights[CreatureConstants.Centipede_Monstrous_Gargantuan][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 60 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Colossal
            //https://www.dimensions.com/element/tiger-centipede-scolopendra-polymorpha
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut Scale up
            weights[CreatureConstants.Centipede_Monstrous_Colossal][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 120 * 12);//(28_788, 102_859);
            weights[CreatureConstants.Centipede_Monstrous_Colossal][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(1), GetGramToPound(1.5), 4, 7, 120 * 12);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm Centipedes are Diminutive, so x1500
            //https://alexaanswers.amazon.com/question/3zw2aivt2nsallUz6Ylyut [1-1.5 grams]x1500 = [3,5]
            weights[CreatureConstants.Centipede_Swarm][GenderConstants.Agender] = ((int)(GetGramToPound(1) * 1500), (int)(GetGramToPound(1.5) * 1500));
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#chainDevilKyton
            weights[CreatureConstants.ChainDevil_Kyton][GenderConstants.Agender] = GetRangeFromAverage(300);
            weights[CreatureConstants.ChainDevil_Kyton][GenderConstants.Female] = GetRangeFromAverage(300);
            weights[CreatureConstants.ChainDevil_Kyton][GenderConstants.Male] = GetRangeFromAverage(300);
            //Source: https://forgottenrealms.fandom.com/wiki/Chaos_beast
            weights[CreatureConstants.ChaosBeast][GenderConstants.Agender] = GetRangeFromAverage(200);
            //Source: https://www.d20srd.org/srd/monsters/cheetah.htm
            weights[CreatureConstants.Cheetah][GenderConstants.Female] = (110, 130);
            weights[CreatureConstants.Cheetah][GenderConstants.Male] = (110, 130);
            //Source: https://forgottenrealms.fandom.com/wiki/Chimera
            weights[CreatureConstants.Chimera_Black][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Black][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Blue][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Blue][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Green][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Green][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Red][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_Red][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_White][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Chimera_White][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://forgottenrealms.fandom.com/wiki/Choker
            weights[CreatureConstants.Choker][GenderConstants.Female] = GetRangeFromAverage(35);
            weights[CreatureConstants.Choker][GenderConstants.Male] = GetRangeFromAverage(35);
            //Source: https://forgottenrealms.fandom.com/wiki/Chuul
            weights[CreatureConstants.Chuul][GenderConstants.Female] = GetRangeFromAverage(650);
            weights[CreatureConstants.Chuul][GenderConstants.Male] = GetRangeFromAverage(650);
            //Source: https://forgottenrealms.fandom.com/wiki/Cloaker
            weights[CreatureConstants.Cloaker][GenderConstants.Agender] = GetRangeFromAverage(100);
            //Source: https://forgottenrealms.fandom.com/wiki/Cockatrice
            weights[CreatureConstants.Cockatrice][GenderConstants.Female] = GetRangeFromAverage(25);
            weights[CreatureConstants.Cockatrice][GenderConstants.Male] = GetRangeFromAverage(25);
            //Source: https://forgottenrealms.fandom.com/wiki/Couatl
            weights[CreatureConstants.Couatl][GenderConstants.Female] = GetRangeFromAverage(1800);
            weights[CreatureConstants.Couatl][GenderConstants.Male] = GetRangeFromAverage(1800);
            //Source: https://www.d20srd.org/srd/monsters/sphinx.htm
            weights[CreatureConstants.Criosphinx][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://www.dimensions.com/element/nile-crocodile-crocodylus-niloticus
            weights[CreatureConstants.Crocodile][GenderConstants.Female] = (496, 1102);
            weights[CreatureConstants.Crocodile][GenderConstants.Male] = (496, 1102);
            //Source: https://www.dimensions.com/element/saltwater-crocodile-crocodylus-porosus Using male range, adjust female upper
            weights[CreatureConstants.Crocodile_Giant][GenderConstants.Female] = (180, 1500);
            weights[CreatureConstants.Crocodile_Giant][GenderConstants.Male] = (880, 2200);
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra
            weights[CreatureConstants.Cryohydra_5Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_5Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_6Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_6Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_7Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_7Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_8Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_8Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_9Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_9Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_10Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_10Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_11Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_11Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_12Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Cryohydra_12Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://forgottenrealms.fandom.com/wiki/Darkmantle
            weights[CreatureConstants.Darkmantle][GenderConstants.Hermaphrodite] = GetRangeFromAverage(30);
            //Source: https://www.dimensions.com/element/deinonychus-deinonychus-antirrhopus
            weights[CreatureConstants.Deinonychus][GenderConstants.Female] = (160, 220);
            weights[CreatureConstants.Deinonychus][GenderConstants.Male] = (160, 220);
            //Source: https://dungeonsdragons.fandom.com/wiki/Delver
            weights[CreatureConstants.Delver][GenderConstants.Female] = GetRangeFromAverage(6000);
            weights[CreatureConstants.Delver][GenderConstants.Male] = GetRangeFromAverage(6000);
            //Source: https://monster.fandom.com/wiki/Derro
            weights[CreatureConstants.Derro][GenderConstants.Female] = GetRangeFromUpTo(40);
            weights[CreatureConstants.Derro][GenderConstants.Male] = GetRangeFromUpTo(40);
            weights[CreatureConstants.Derro_Sane][GenderConstants.Female] = GetRangeFromUpTo(40);
            weights[CreatureConstants.Derro_Sane][GenderConstants.Male] = GetRangeFromUpTo(40);
            //Source: https://forgottenrealms.fandom.com/wiki/Destrachan
            weights[CreatureConstants.Destrachan][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Destrachan][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://www.d20srd.org/srd/monsters/devourer.htm
            weights[CreatureConstants.Devourer][GenderConstants.Agender] = GetRangeFromAverage(500);
            //Source: https://forgottenrealms.fandom.com/wiki/Digester
            weights[CreatureConstants.Digester][GenderConstants.Female] = GetRangeFromAverage(350);
            weights[CreatureConstants.Digester][GenderConstants.Male] = GetRangeFromAverage(350);
            //Source: https://forgottenrealms.fandom.com/wiki/Displacer_beast
            weights[CreatureConstants.DisplacerBeast][GenderConstants.Female] = (450, 950);
            weights[CreatureConstants.DisplacerBeast][GenderConstants.Male] = (500, 1000);
            //Source: scale up from normal, x8
            weights[CreatureConstants.DisplacerBeast_PackLord][GenderConstants.Female] = (3600, 7600);
            weights[CreatureConstants.DisplacerBeast_PackLord][GenderConstants.Male] = (4000, 8000);
            //Source: https://forgottenrealms.fandom.com/wiki/Djinni
            weights[CreatureConstants.Djinni][GenderConstants.Agender] = GetRangeFromAverage(1000);
            weights[CreatureConstants.Djinni][GenderConstants.Female] = GetRangeFromAverage(1000);
            weights[CreatureConstants.Djinni][GenderConstants.Male] = GetRangeFromAverage(1000);
            //Source: https://forgottenrealms.fandom.com/wiki/Noble_djinni Scale up from normal djinn
            weights[CreatureConstants.Djinni_Noble][GenderConstants.Agender] = GetScaledUpRangeFromAverage(
                weights[CreatureConstants.Djinni][GenderConstants.Agender], 10 * 12 + 6, 12 * 12);
            weights[CreatureConstants.Djinni_Noble][GenderConstants.Female] = GetScaledUpRangeFromAverage(
                weights[CreatureConstants.Djinni][GenderConstants.Female], 10 * 12 + 6, 12 * 12);
            weights[CreatureConstants.Djinni_Noble][GenderConstants.Male] = GetScaledUpRangeFromAverage(
                weights[CreatureConstants.Djinni][GenderConstants.Male], 10 * 12 + 6, 12 * 12);
            //Source: https://www.d20srd.org/srd/monsters/dog.htm
            weights[CreatureConstants.Dog][GenderConstants.Female] = (20, 50);
            weights[CreatureConstants.Dog][GenderConstants.Male] = (20, 50);
            //Source: https://www.dimensions.com/element/saint-bernard-dog M:140-180,F:120-140
            //https://www.dimensions.com/element/siberian-husky 35-65
            //https://www.dimensions.com/element/dogs-collie M:45-65,F:40-55 Using female range, adjusting male lower
            weights[CreatureConstants.Dog_Riding][GenderConstants.Female] = (35, 140);
            weights[CreatureConstants.Dog_Riding][GenderConstants.Male] = (75, 180);
            //Source: https://www.dimensions.com/element/donkey-equus-africanus-asinus
            weights[CreatureConstants.Donkey][GenderConstants.Female] = (400, 500);
            weights[CreatureConstants.Donkey][GenderConstants.Male] = (400, 500);
            //Source: https://forgottenrealms.fandom.com/wiki/Doppelganger
            weights[CreatureConstants.Doppelganger][GenderConstants.Agender] = GetRangeFromAverage(150);
            //Source: Draconomicon
            weights[CreatureConstants.Dragon_Black_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_Black_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_Black_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Black_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Black_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Black_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Black_Juvenile][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Black_Juvenile][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Black_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Black_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Black_Adult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Black_Adult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Black_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_Ancient][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_Ancient][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Black_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Black_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Black_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Black_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Blue_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Blue_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Blue_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Blue_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Blue_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Blue_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Blue_Juvenile][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Blue_Juvenile][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Blue_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Blue_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Blue_Adult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_Adult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Blue_Ancient][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Blue_Ancient][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Blue_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Blue_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Blue_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Blue_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Brass_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_Brass_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_Brass_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Brass_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Brass_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Brass_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Brass_Juvenile][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Brass_Juvenile][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Brass_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Brass_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Brass_Adult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Brass_Adult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Brass_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_Ancient][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_Ancient][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Brass_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Brass_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Brass_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Brass_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Bronze_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Bronze_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Bronze_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Bronze_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Bronze_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Bronze_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Bronze_Juvenile][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Bronze_Juvenile][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Bronze_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Bronze_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Bronze_Adult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_Adult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Bronze_Ancient][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Bronze_Ancient][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Bronze_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Bronze_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Bronze_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Bronze_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Copper_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_Copper_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_Copper_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Copper_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Copper_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Copper_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Copper_Juvenile][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Copper_Juvenile][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Copper_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Copper_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Copper_Adult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Copper_Adult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Copper_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_Ancient][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_Ancient][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Copper_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Copper_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Copper_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Copper_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Gold_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Gold_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Gold_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Gold_Young][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Gold_Young][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Gold_Juvenile][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Gold_Juvenile][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Gold_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Gold_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Gold_Adult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Gold_Adult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Gold_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Gold_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Gold_Old][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_Old][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_VeryOld][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_VeryOld][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_Ancient][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_Ancient][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Gold_Wyrm][GenderConstants.Female] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Gold_Wyrm][GenderConstants.Male] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Gold_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Gold_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Green_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Green_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Green_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Green_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Green_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Green_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Green_Juvenile][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Green_Juvenile][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Green_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Green_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Green_Adult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_Adult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Green_Ancient][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Green_Ancient][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Green_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Green_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Green_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Green_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Red_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Red_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Red_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Red_Young][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Red_Young][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Red_Juvenile][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Red_Juvenile][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Red_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Red_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Red_Adult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Red_Adult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Red_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Red_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Red_Old][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_Old][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_VeryOld][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_VeryOld][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_Ancient][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_Ancient][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Red_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Red_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Silver_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Silver_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_Silver_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Silver_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Silver_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Silver_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_Silver_Juvenile][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Silver_Juvenile][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Silver_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Silver_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_Silver_Adult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_Adult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_Silver_Ancient][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Silver_Ancient][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Silver_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Silver_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_Silver_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_Silver_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(1_280_000);
            weights[CreatureConstants.Dragon_White_Wyrmling][GenderConstants.Female] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_White_Wyrmling][GenderConstants.Male] = GetRangeFromAverage(5);
            weights[CreatureConstants.Dragon_White_VeryYoung][GenderConstants.Female] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_White_VeryYoung][GenderConstants.Male] = GetRangeFromAverage(40);
            weights[CreatureConstants.Dragon_White_Young][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_White_Young][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_White_Juvenile][GenderConstants.Female] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_White_Juvenile][GenderConstants.Male] = GetRangeFromAverage(320);
            weights[CreatureConstants.Dragon_White_YoungAdult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_White_YoungAdult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_White_Adult][GenderConstants.Female] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_White_Adult][GenderConstants.Male] = GetRangeFromAverage(2500);
            weights[CreatureConstants.Dragon_White_MatureAdult][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_MatureAdult][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_Old][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_Old][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_VeryOld][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_VeryOld][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_Ancient][GenderConstants.Female] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_Ancient][GenderConstants.Male] = GetRangeFromAverage(20_000);
            weights[CreatureConstants.Dragon_White_Wyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_White_Wyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_White_GreatWyrm][GenderConstants.Female] = GetRangeFromAverage(160_000);
            weights[CreatureConstants.Dragon_White_GreatWyrm][GenderConstants.Male] = GetRangeFromAverage(160_000);
            //Source: https://forgottenrealms.fandom.com/wiki/Dragon_turtle
            weights[CreatureConstants.DragonTurtle][GenderConstants.Female] = (8000, 32_000);
            weights[CreatureConstants.DragonTurtle][GenderConstants.Male] = (8000, 32_000);
            //Source: https://forgottenrealms.fandom.com/wiki/Dragonne
            weights[CreatureConstants.Dragonne][GenderConstants.Female] = GetRangeFromAverage(700);
            weights[CreatureConstants.Dragonne][GenderConstants.Male] = GetRangeFromAverage(700);
            //Source: https://forgottenrealms.fandom.com/wiki/Dretch
            weights[CreatureConstants.Dretch][GenderConstants.Agender] = GetRangeFromAverage(60);
            weights[CreatureConstants.Dretch][GenderConstants.Female] = GetRangeFromAverage(60);
            weights[CreatureConstants.Dretch][GenderConstants.Male] = GetRangeFromAverage(60);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/drider-species
            weights[CreatureConstants.Drider][GenderConstants.Agender] = (230, 270);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/dryad-species
            weights[CreatureConstants.Dryad][GenderConstants.Female] = GetRangeFromAverage(150);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            //https://www.d20srd.org/srd/monsters/dwarf.htm
            //"Leaner" than Hill Dwarves, so 90%
            weights[CreatureConstants.Dwarf_Deep][GenderConstants.Female] = (90 + 2 * 2, 90 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Deep][GenderConstants.Male] = (130 - 13 + 2 * 2, 130 - 13 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Duergar][GenderConstants.Female] = (100 + 2 * 2, 100 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Duergar][GenderConstants.Male] = (130 + 2 * 2, 130 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Hill][GenderConstants.Female] = (100 + 2 * 2, 100 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Hill][GenderConstants.Male] = (130 + 2 * 2, 130 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Mountain][GenderConstants.Female] = (100 + 2 * 2, 100 + 2 * 4 * 2 * 6);
            weights[CreatureConstants.Dwarf_Mountain][GenderConstants.Male] = (130 + 2 * 2, 130 + 2 * 4 * 2 * 6);
            //Source: https://www.dimensions.com/element/bald-eagle-haliaeetus-leucocephalus
            weights[CreatureConstants.Eagle][GenderConstants.Female] = (6, 14);
            weights[CreatureConstants.Eagle][GenderConstants.Male] = (6, 14);
            //Source: https://www.d20srd.org/srd/monsters/eagleGiant.htm
            weights[CreatureConstants.Eagle_Giant][GenderConstants.Female] = GetRangeFromAverage(500);
            weights[CreatureConstants.Eagle_Giant][GenderConstants.Male] = GetRangeFromAverage(500);
            //Source: https://forgottenrealms.fandom.com/wiki/Efreeti
            weights[CreatureConstants.Efreeti][GenderConstants.Agender] = GetRangeFromAverage(2000);
            weights[CreatureConstants.Efreeti][GenderConstants.Female] = GetRangeFromAverage(2000);
            weights[CreatureConstants.Efreeti][GenderConstants.Male] = GetRangeFromAverage(2000);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Elasmosaurus
            weights[CreatureConstants.Elasmosaurus][GenderConstants.Female] = GetRangeFromAverage(9 * 2000);
            weights[CreatureConstants.Elasmosaurus][GenderConstants.Male] = GetRangeFromAverage(9 * 2000);
            //Source: https://www.d20srd.org/srd/monsters/elemental.htm
            weights[CreatureConstants.Elemental_Air_Small][GenderConstants.Agender] = GetRangeFromAverage(1);
            weights[CreatureConstants.Elemental_Air_Medium][GenderConstants.Agender] = GetRangeFromAverage(2);
            weights[CreatureConstants.Elemental_Air_Large][GenderConstants.Agender] = GetRangeFromAverage(4);
            weights[CreatureConstants.Elemental_Air_Huge][GenderConstants.Agender] = GetRangeFromAverage(8);
            weights[CreatureConstants.Elemental_Air_Greater][GenderConstants.Agender] = GetRangeFromAverage(10);
            weights[CreatureConstants.Elemental_Air_Elder][GenderConstants.Agender] = GetRangeFromAverage(12);
            weights[CreatureConstants.Elemental_Earth_Small][GenderConstants.Agender] = GetRangeFromAverage(80);
            weights[CreatureConstants.Elemental_Earth_Medium][GenderConstants.Agender] = GetRangeFromAverage(750);
            weights[CreatureConstants.Elemental_Earth_Large][GenderConstants.Agender] = GetRangeFromAverage(6000);
            weights[CreatureConstants.Elemental_Earth_Huge][GenderConstants.Agender] = GetRangeFromAverage(48_000);
            weights[CreatureConstants.Elemental_Earth_Greater][GenderConstants.Agender] = GetRangeFromAverage(54_000);
            weights[CreatureConstants.Elemental_Earth_Elder][GenderConstants.Agender] = GetRangeFromAverage(60_000);
            weights[CreatureConstants.Elemental_Fire_Small][GenderConstants.Agender] = GetRangeFromAverage(1);
            weights[CreatureConstants.Elemental_Fire_Medium][GenderConstants.Agender] = GetRangeFromAverage(2);
            weights[CreatureConstants.Elemental_Fire_Large][GenderConstants.Agender] = GetRangeFromAverage(4);
            weights[CreatureConstants.Elemental_Fire_Huge][GenderConstants.Agender] = GetRangeFromAverage(8);
            weights[CreatureConstants.Elemental_Fire_Greater][GenderConstants.Agender] = GetRangeFromAverage(10);
            weights[CreatureConstants.Elemental_Fire_Elder][GenderConstants.Agender] = GetRangeFromAverage(12);
            weights[CreatureConstants.Elemental_Water_Small][GenderConstants.Agender] = GetRangeFromAverage(34);
            weights[CreatureConstants.Elemental_Water_Medium][GenderConstants.Agender] = GetRangeFromAverage(280);
            weights[CreatureConstants.Elemental_Water_Large][GenderConstants.Agender] = GetRangeFromAverage(2250);
            weights[CreatureConstants.Elemental_Water_Huge][GenderConstants.Agender] = GetRangeFromAverage(18_000);
            weights[CreatureConstants.Elemental_Water_Greater][GenderConstants.Agender] = GetRangeFromAverage(21_000);
            weights[CreatureConstants.Elemental_Water_Elder][GenderConstants.Agender] = GetRangeFromAverage(24_000);
            //Source: https://www.dimensions.com/element/african-bush-elephant-loxodonta-africana
            weights[CreatureConstants.Elephant][GenderConstants.Female] = (5500, 15_400);
            weights[CreatureConstants.Elephant][GenderConstants.Male] = (5500, 15_400);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            //https://forgottenrealms.fandom.com/wiki/Aquatic_elf - larger and heavier, so 110%
            weights[CreatureConstants.Elf_Aquatic][GenderConstants.Female] = (80 + 2 * 1 + 8, 80 + 2 * 6 * 1 * 6 + 8);
            weights[CreatureConstants.Elf_Aquatic][GenderConstants.Male] = (85 + 2 * 1 + 8, 85 + 2 * 6 * 1 * 6 + 8);
            //Source: https://forgottenrealms.fandom.com/wiki/Drow
            weights[CreatureConstants.Elf_Drow][GenderConstants.Female] = (82, 152);
            weights[CreatureConstants.Elf_Drow][GenderConstants.Male] = (87, 157);
            weights[CreatureConstants.Elf_Gray][GenderConstants.Female] = (80 + 2 * 1, 80 + 2 * 6 * 1 * 6);
            weights[CreatureConstants.Elf_Gray][GenderConstants.Male] = (85 + 2 * 1, 85 + 2 * 6 * 1 * 6);
            weights[CreatureConstants.Elf_Half][GenderConstants.Female] = (80 + 2 * 2, 80 + 2 * 8 * 2 * 4);
            weights[CreatureConstants.Elf_Half][GenderConstants.Male] = (100 + 2 * 2, 100 + 2 * 8 * 2 * 4);
            weights[CreatureConstants.Elf_High][GenderConstants.Female] = (80 + 2 * 1, 80 + 2 * 6 * 1 * 6);
            weights[CreatureConstants.Elf_High][GenderConstants.Male] = (85 + 2 * 1, 85 + 2 * 6 * 1 * 6);
            //https://forgottenrealms.fandom.com/wiki/Wild_elf Males 20 pounds heavier than females
            weights[CreatureConstants.Elf_Wild][GenderConstants.Female] = (80 + 2 * 1, 80 + 2 * 6 * 1 * 6);
            weights[CreatureConstants.Elf_Wild][GenderConstants.Male] = (100 + 2 * 1, 100 + 2 * 6 * 1 * 6);
            weights[CreatureConstants.Elf_Wood][GenderConstants.Female] = (80 + 2 * 1, 80 + 2 * 6 * 1 * 6);
            weights[CreatureConstants.Elf_Wood][GenderConstants.Male] = (85 + 2 * 1, 85 + 2 * 6 * 1 * 6);
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#erinyes
            weights[CreatureConstants.Erinyes][GenderConstants.Female] = GetRangeFromAverage(150);
            weights[CreatureConstants.Erinyes][GenderConstants.Male] = GetRangeFromAverage(150);
            //Can't find any source on weight. So, using human
            weights[CreatureConstants.EtherealFilcher][GenderConstants.Agender] = weights[CreatureConstants.Human][GenderConstants.Female];
            //Source: https://www.d20srd.org/srd/monsters/etherealMarauder.htm
            weights[CreatureConstants.EtherealMarauder][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.EtherealMarauder][GenderConstants.Male] = GetRangeFromAverage(200);
            //Source: https://syrikdarkenedskies.obsidianportal.com/wikis/ettercap-race
            weights[CreatureConstants.Ettercap][GenderConstants.Female] = (160, 270);
            weights[CreatureConstants.Ettercap][GenderConstants.Male] = (140, 250);
            //Source: https://forgottenrealms.fandom.com/wiki/Ettin Males are heavier, so +10%
            weights[CreatureConstants.Ettin][GenderConstants.Female] = (930, 5200 - 90);
            weights[CreatureConstants.Ettin][GenderConstants.Male] = (930 + 90, 5200);
            //Source: https://www.d20srd.org/srd/monsters/giantFireBeetle.htm
            //https://www.guinnessworldrecords.com/world-records/most-bioluminescent-insect scale up
            weights[CreatureConstants.FireBeetle_Giant][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetGramToPound(.3) * .9, GetGramToPound(.3) * 1.1, .39, .55, 2 * 12); //(55, 154);
            weights[CreatureConstants.FireBeetle_Giant][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(.3) * .9, GetGramToPound(.3) * 1.1, .39, .55, 2 * 12);
            //Source: https://www.d20srd.org/srd/monsters/formian.htm
            weights[CreatureConstants.FormianWorker][GenderConstants.Male] = GetRangeFromAverage(60);
            weights[CreatureConstants.FormianWarrior][GenderConstants.Male] = GetRangeFromAverage(180);
            weights[CreatureConstants.FormianTaskmaster][GenderConstants.Male] = GetRangeFromAverage(180);
            weights[CreatureConstants.FormianMyrmarch][GenderConstants.Male] = GetRangeFromAverage(1500);
            weights[CreatureConstants.FormianQueen][GenderConstants.Female] = GetRangeFromAverage(3500);
            //Source: https://forgottenrealms.fandom.com/wiki/Frost_worm
            weights[CreatureConstants.FrostWorm][GenderConstants.Female] = GetRangeFromAverage(8000);
            weights[CreatureConstants.FrostWorm][GenderConstants.Male] = GetRangeFromAverage(8000);
            //Source: https://dnd-wiki.org/wiki/Gargoyle_(3.5e_Race) using 2d10 for heights
            weights[CreatureConstants.Gargoyle][GenderConstants.Agender] = (300 + 2 * 2, 300 + 2 * 10 * 2 * 6);
            weights[CreatureConstants.Gargoyle_Kapoacinth][GenderConstants.Agender] = (300 + 2 * 2, 300 + 2 * 10 * 2 * 6);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/gelatinous-cube-species
            weights[CreatureConstants.GelatinousCube][GenderConstants.Agender] = GetRangeFromAverage(15_000);
            //Source: https://forgottenrealms.fandom.com/wiki/Ghaele
            weights[CreatureConstants.Ghaele][GenderConstants.Female] = (131, 185);
            weights[CreatureConstants.Ghaele][GenderConstants.Male] = (146, 200);
            //Source: https://www.dandwiki.com/wiki/Ghoul_(5e_Race)
            weights[CreatureConstants.Ghoul][GenderConstants.Female] = (110 + 2 * 1, 110 + 2 * 12 * 1 * 4);
            weights[CreatureConstants.Ghoul][GenderConstants.Male] = (110 + 2 * 1, 110 + 2 * 12 * 1 * 4);
            weights[CreatureConstants.Ghoul_Ghast][GenderConstants.Female] = (110 + 2 * 1, 110 + 2 * 12 * 1 * 4);
            weights[CreatureConstants.Ghoul_Ghast][GenderConstants.Male] = (110 + 2 * 1, 110 + 2 * 12 * 1 * 4);
            weights[CreatureConstants.Ghoul_Lacedon][GenderConstants.Female] = (110 + 2 * 1, 110 + 2 * 12 * 1 * 4);
            weights[CreatureConstants.Ghoul_Lacedon][GenderConstants.Male] = (110 + 2 * 1, 110 + 2 * 12 * 1 * 4);
            //Source: https://forgottenrealms.fandom.com/wiki/Cloud_giant
            weights[CreatureConstants.Giant_Cloud][GenderConstants.Female] = GetRangeFromAverage(11_500);
            weights[CreatureConstants.Giant_Cloud][GenderConstants.Male] = GetRangeFromAverage(11_500);
            //Source: https://forgottenrealms.fandom.com/wiki/Fire_giant
            weights[CreatureConstants.Giant_Fire][GenderConstants.Female] = (7000, 8000);
            weights[CreatureConstants.Giant_Fire][GenderConstants.Male] = (7000, 8000);
            //Source: https://forgottenrealms.fandom.com/wiki/Frost_giant
            weights[CreatureConstants.Giant_Frost][GenderConstants.Female] = GetRangeFromAverage(8000);
            weights[CreatureConstants.Giant_Frost][GenderConstants.Male] = GetRangeFromAverage(8000);
            //Source: https://forgottenrealms.fandom.com/wiki/Hill_giant
            weights[CreatureConstants.Giant_Hill][GenderConstants.Female] = GetRangeFromAverage(4500);
            weights[CreatureConstants.Giant_Hill][GenderConstants.Male] = GetRangeFromAverage(4500);
            //Source: https://forgottenrealms.fandom.com/wiki/Stone_giant
            weights[CreatureConstants.Giant_Stone][GenderConstants.Female] = GetRangeFromAverage(9000);
            weights[CreatureConstants.Giant_Stone][GenderConstants.Male] = GetRangeFromAverage(9000);
            weights[CreatureConstants.Giant_Stone_Elder][GenderConstants.Female] = GetRangeFromAverage(9000);
            weights[CreatureConstants.Giant_Stone_Elder][GenderConstants.Male] = GetRangeFromAverage(9000);
            //Source: https://forgottenrealms.fandom.com/wiki/Storm_giant
            weights[CreatureConstants.Giant_Storm][GenderConstants.Female] = GetRangeFromAverage(15_000);
            weights[CreatureConstants.Giant_Storm][GenderConstants.Male] = GetRangeFromAverage(15_000);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/gibbering-mouther-species
            weights[CreatureConstants.GibberingMouther][GenderConstants.Agender] = GetRangeFromAverage(200);
            //Source: https://www.d20srd.org/srd/monsters/girallon.htm
            weights[CreatureConstants.Girallon][GenderConstants.Female] = GetRangeFromAverage(800);
            weights[CreatureConstants.Girallon][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://forgottenrealms.fandom.com/wiki/Githyanki
            weights[CreatureConstants.Githyanki][GenderConstants.Female] = (89, 245);
            weights[CreatureConstants.Githyanki][GenderConstants.Male] = (124, 280);
            //Source: https://forgottenrealms.fandom.com/wiki/Githzerai
            weights[CreatureConstants.Githzerai][GenderConstants.Female] = (92, 196);
            weights[CreatureConstants.Githzerai][GenderConstants.Male] = (92, 196);
            //Source: https://forgottenrealms.fandom.com/wiki/Glabrezu
            weights[CreatureConstants.Glabrezu][GenderConstants.Agender] = GetRangeFromAverage(5500);
            //Source: https://forgottenrealms.fandom.com/wiki/Gnoll
            weights[CreatureConstants.Gnoll][GenderConstants.Female] = (280, 320);
            weights[CreatureConstants.Gnoll][GenderConstants.Male] = (280, 320);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Gnome_Forest][GenderConstants.Female] = (35 + 2 * 1, 35 + 2 * 4 * 1);
            weights[CreatureConstants.Gnome_Forest][GenderConstants.Male] = (40 + 2 * 1, 40 + 2 * 4 * 1);
            weights[CreatureConstants.Gnome_Rock][GenderConstants.Female] = (35 + 2 * 1, 35 + 2 * 4 * 1);
            weights[CreatureConstants.Gnome_Rock][GenderConstants.Male] = (40 + 2 * 1, 40 + 2 * 4 * 1);
            weights[CreatureConstants.Gnome_Svirfneblin][GenderConstants.Female] = (35 + 2 * 1, 35 + 2 * 4 * 1);
            weights[CreatureConstants.Gnome_Svirfneblin][GenderConstants.Male] = (40 + 2 * 1, 40 + 2 * 4 * 1);
            //Source: https://forgottenrealms.fandom.com/wiki/Goblin
            weights[CreatureConstants.Goblin][GenderConstants.Female] = (40, 55);
            weights[CreatureConstants.Goblin][GenderConstants.Male] = (40, 55);
            //Source: https://pathfinderwiki.com/wiki/Clay_golem
            weights[CreatureConstants.Golem_Clay][GenderConstants.Agender] = GetRangeFromAverage(600);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            weights[CreatureConstants.Golem_Flesh][GenderConstants.Agender] = GetRangeFromUpTo(500);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            weights[CreatureConstants.Golem_Iron][GenderConstants.Agender] = GetRangeFromAverage(5000);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            weights[CreatureConstants.Golem_Stone][GenderConstants.Agender] = GetRangeFromAverage(2000);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            weights[CreatureConstants.Golem_Stone_Greater][GenderConstants.Agender] = GetRangeFromAverage(32_000);
            //Source: https://www.d20srd.org/srd/monsters/gorgon.htm
            weights[CreatureConstants.Gorgon][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Gorgon][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://www.d20srd.org/srd/monsters/ooze.htm#grayOoze
            weights[CreatureConstants.GrayOoze][GenderConstants.Agender] = GetRangeFromAverage(700);
            //Source: https://www.d20srd.org/srd/monsters/grayRender.htm
            weights[CreatureConstants.GrayRender][GenderConstants.Agender] = GetRangeFromAverage(4000);
            //Source: https://www.d20srd.org/srd/monsters/hag.htm#greenHag Female human
            weights[CreatureConstants.GreenHag][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            //Source: https://www.d20srd.org/srd/monsters/grick.htm
            weights[CreatureConstants.Grick][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.Grick][GenderConstants.Male] = GetRangeFromAverage(200);
            //Source: https://www.d20srd.org/srd/monsters/griffon.htm
            weights[CreatureConstants.Griffon][GenderConstants.Female] = GetRangeFromAverage(500);
            weights[CreatureConstants.Griffon][GenderConstants.Male] = GetRangeFromAverage(500);
            //Source: https://www.d20srd.org/srd/monsters/sprite.htm#grig
            weights[CreatureConstants.Grig][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Grig][GenderConstants.Male] = GetRangeFromAverage(1);
            weights[CreatureConstants.Grig_WithFiddle][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Grig_WithFiddle][GenderConstants.Male] = GetRangeFromAverage(1);
            //Source: https://forgottenrealms.fandom.com/wiki/Grimlock
            weights[CreatureConstants.Grimlock][GenderConstants.Female] = GetRangeFromAverage(180);
            weights[CreatureConstants.Grimlock][GenderConstants.Male] = GetRangeFromAverage(180);
            //Source: https://www.d20srd.org/srd/monsters/sphinx.htm
            weights[CreatureConstants.Gynosphinx][GenderConstants.Female] = GetRangeFromAverage(800);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Halfling_Deep][GenderConstants.Female] = (25 + 2 * 1, 25 + 2 * 4 * 1);
            weights[CreatureConstants.Halfling_Deep][GenderConstants.Male] = (30 + 2 * 1, 30 + 2 * 4 * 1);
            weights[CreatureConstants.Halfling_Lightfoot][GenderConstants.Female] = (25 + 2 * 1, 25 + 2 * 4 * 1);
            weights[CreatureConstants.Halfling_Lightfoot][GenderConstants.Male] = (30 + 2 * 1, 30 + 2 * 4 * 1);
            weights[CreatureConstants.Halfling_Tallfellow][GenderConstants.Female] = (25 + 2 * 1, 25 + 2 * 4 * 1);
            weights[CreatureConstants.Halfling_Tallfellow][GenderConstants.Male] = (30 + 2 * 1, 30 + 2 * 4 * 1);
            //Source: https://www.5esrd.com/database/race/harpy/
            weights[CreatureConstants.Harpy][GenderConstants.Female] = GetRangeFromUpTo(100);
            weights[CreatureConstants.Harpy][GenderConstants.Male] = GetRangeFromUpTo(100);
            //Source: https://www.dimensions.com/element/osprey-pandion-haliaetus
            weights[CreatureConstants.Hawk][GenderConstants.Female] = (2, 4);
            weights[CreatureConstants.Hawk][GenderConstants.Male] = (2, 4);
            //Source: https://forgottenrealms.fandom.com/wiki/Hell_hound
            weights[CreatureConstants.HellHound][GenderConstants.Female] = GetRangeFromAverage(120);
            weights[CreatureConstants.HellHound][GenderConstants.Male] = GetRangeFromAverage(120);
            //Scale up from hell hound
            weights[CreatureConstants.HellHound_NessianWarhound][GenderConstants.Female] = GetScaledUpRange(
                weights[CreatureConstants.HellHound][GenderConstants.Female], 24, 54, 64, 72);
            weights[CreatureConstants.HellHound_NessianWarhound][GenderConstants.Male] = GetScaledUpRange(
                weights[CreatureConstants.HellHound][GenderConstants.Male], 24, 54, 64, 72);
            //Source: https://forgottenrealms.fandom.com/wiki/Hellcat
            weights[CreatureConstants.Hellcat_Bezekira][GenderConstants.Female] = GetRangeFromAverage(900);
            weights[CreatureConstants.Hellcat_Bezekira][GenderConstants.Male] = GetRangeFromAverage(900);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm Diminutive flying = 5000 creatures, so 5000x(.5 oz)
            //https://www.dnr.sc.gov/magazine/articles/mayjune2015/FWW-PaperWasp.html
            weights[CreatureConstants.Hellwasp_Swarm][GenderConstants.Agender] = GetRangeFromUpTo((int)(GetOunceToPound(.5) * 5000));
            //Source: https://www.d20srd.org/srd/monsters/demon.htm#hezrou
            weights[CreatureConstants.Hezrou][GenderConstants.Agender] = GetRangeFromAverage(750);
            //Source: https://www.d20srd.org/srd/monsters/sphinx.htm
            weights[CreatureConstants.Hieracosphinx][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://www.d20srd.org/srd/monsters/hippogriff.htm
            weights[CreatureConstants.Hippogriff][GenderConstants.Female] = GetRangeFromAverage(1000);
            weights[CreatureConstants.Hippogriff][GenderConstants.Male] = GetRangeFromAverage(1000);
            //Source: https://forgottenrealms.fandom.com/wiki/Hobgoblin
            weights[CreatureConstants.Hobgoblin][GenderConstants.Female] = (150, 200);
            weights[CreatureConstants.Hobgoblin][GenderConstants.Male] = (150, 200);
            //Source: https://forgottenrealms.fandom.com/wiki/Homunculus
            //https://www.dimensions.com/element/eastern-gray-squirrel
            weights[CreatureConstants.Homunculus][GenderConstants.Agender] = GetRangeFromAverage(1);
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#hornedDevilCornugon
            weights[CreatureConstants.HornedDevil_Cornugon][GenderConstants.Agender] = GetRangeFromAverage(600);
            //Source: https://www.dimensions.com/element/clydesdale-horse
            weights[CreatureConstants.Horse_Heavy][GenderConstants.Female] = (1800, 2200);
            weights[CreatureConstants.Horse_Heavy][GenderConstants.Male] = (1800, 2200);
            //Source: https://www.dimensions.com/element/arabian-horse
            weights[CreatureConstants.Horse_Light][GenderConstants.Female] = (800, 1000);
            weights[CreatureConstants.Horse_Light][GenderConstants.Male] = (800, 1000);
            //Source: https://www.dimensions.com/element/clydesdale-horse
            weights[CreatureConstants.Horse_Heavy_War][GenderConstants.Female] = (1800, 2200);
            weights[CreatureConstants.Horse_Heavy_War][GenderConstants.Male] = (1800, 2200);
            //Source: https://www.dimensions.com/element/arabian-horse
            weights[CreatureConstants.Horse_Light_War][GenderConstants.Female] = (800, 1000);
            weights[CreatureConstants.Horse_Light_War][GenderConstants.Male] = (800, 1000);
            //Source: https://dungeons.fandom.com/wiki/Hound_Archon
            weights[CreatureConstants.HoundArchon][GenderConstants.Agender] = (180, 260);
            //Source: https://forgottenrealms.fandom.com/wiki/Howler
            weights[CreatureConstants.Howler][GenderConstants.Agender] = GetRangeFromAverage(2000);
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra
            weights[CreatureConstants.Hydra_5Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_5Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_6Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_6Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_7Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_7Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_8Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_8Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_9Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_9Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_10Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_10Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_11Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_11Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_12Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Hydra_12Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://www.d20srd.org/srd/monsters/hyena.htm
            weights[CreatureConstants.Hyena][GenderConstants.Female] = GetRangeFromAverage(120);
            weights[CreatureConstants.Hyena][GenderConstants.Male] = GetRangeFromAverage(120);
            //Source: https://forgottenrealms.fandom.com/wiki/Gelugon
            weights[CreatureConstants.IceDevil_Gelugon][GenderConstants.Agender] = GetRangeFromAverage(700);
            //Source: https://forgottenrealms.fandom.com/wiki/Imp
            weights[CreatureConstants.Imp][GenderConstants.Agender] = GetRangeFromAverage(8);
            weights[CreatureConstants.Imp][GenderConstants.Female] = GetRangeFromAverage(8);
            weights[CreatureConstants.Imp][GenderConstants.Male] = GetRangeFromAverage(8);
            //Source: https://www.d20srd.org/srd/monsters/invisibleStalker.htm
            //https://www.d20srd.org/srd/monsters/elemental.htm using Large air elemental
            weights[CreatureConstants.InvisibleStalker][GenderConstants.Agender] = weights[CreatureConstants.Elemental_Air_Large][GenderConstants.Agender];
            //Source: https://forgottenrealms.fandom.com/wiki/Janni
            //Resemble humans or half-elves, so scaling up from Human height
            weights[CreatureConstants.Janni][GenderConstants.Agender] = GetScaledUpRange(weights[CreatureConstants.Human][GenderConstants.Male], 60, 78, 6 * 12, 7 * 12);
            weights[CreatureConstants.Janni][GenderConstants.Female] = GetScaledUpRange(weights[CreatureConstants.Human][GenderConstants.Male], 60, 78, 6 * 12, 7 * 12);
            weights[CreatureConstants.Janni][GenderConstants.Male] = GetScaledUpRange(weights[CreatureConstants.Human][GenderConstants.Male], 60, 78, 6 * 12, 7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Kobold
            weights[CreatureConstants.Kobold][GenderConstants.Female] = (35, 45);
            weights[CreatureConstants.Kobold][GenderConstants.Male] = (35, 45);
            //Source: https://pathfinderwiki.com/wiki/Kolyarut
            //Can't find definitive weight, but "weighing far more than a human". Made of metal, so use iron
            weights[CreatureConstants.Kolyarut][GenderConstants.Agender] = GetScaledUpRange(5000, 12 * 12, 62, 84);
            //Source: https://forgottenrealms.fandom.com/wiki/Kraken
            weights[CreatureConstants.Kraken][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Kraken][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://www.d20srd.org/srd/monsters/krenshar.htm
            weights[CreatureConstants.Krenshar][GenderConstants.Female] = GetRangeFromAverage(175);
            weights[CreatureConstants.Krenshar][GenderConstants.Male] = GetRangeFromAverage(175);
            //Source: https://forgottenrealms.fandom.com/wiki/Kuo-toa
            weights[CreatureConstants.KuoToa][GenderConstants.Female] = GetRangeFromAverage(160);
            weights[CreatureConstants.KuoToa][GenderConstants.Male] = GetRangeFromAverage(160);
            //Source: https://forgottenrealms.fandom.com/wiki/Lamia
            weights[CreatureConstants.Lamia][GenderConstants.Female] = (650, 700);
            weights[CreatureConstants.Lamia][GenderConstants.Male] = (650, 700);
            //Source: https://www.d20srd.org/srd/monsters/lammasu.htm
            weights[CreatureConstants.Lammasu][GenderConstants.Female] = GetRangeFromAverage(500);
            weights[CreatureConstants.Lammasu][GenderConstants.Male] = GetRangeFromAverage(500);
            //Since they float and are just balls of light, say weight is 0
            weights[CreatureConstants.LanternArchon][GenderConstants.Agender] = (0, 0);
            //Source: https://forgottenrealms.fandom.com/wiki/Lemure
            weights[CreatureConstants.Lemure][GenderConstants.Agender] = GetRangeFromAverage(100);
            //Source: https://www.5esrd.com/database/creature/agathion-leonal-3pp/
            weights[CreatureConstants.Leonal][GenderConstants.Female] = GetRangeFromAverage(270);
            weights[CreatureConstants.Leonal][GenderConstants.Male] = GetRangeFromAverage(270);
            //Source: https://www.d20srd.org/srd/monsters/leopard.htm
            weights[CreatureConstants.Leopard][GenderConstants.Female] = GetRangeFromAverage(120);
            weights[CreatureConstants.Leopard][GenderConstants.Male] = GetRangeFromAverage(120);
            //Source: https://www.d20srd.org/srd/monsters/lillend.htm
            weights[CreatureConstants.Lillend][GenderConstants.Female] = GetRangeFromAverage(3800);
            weights[CreatureConstants.Lillend][GenderConstants.Male] = GetRangeFromAverage(3800);
            //Source: https://www.d20srd.org/srd/monsters/lion.htm
            weights[CreatureConstants.Lion][GenderConstants.Female] = (330, 550);
            weights[CreatureConstants.Lion][GenderConstants.Male] = (330, 550);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_lion
            weights[CreatureConstants.Lion_Dire][GenderConstants.Female] = GetRangeFromUpTo(3500);
            weights[CreatureConstants.Lion_Dire][GenderConstants.Male] = GetRangeFromUpTo(3500);
            //Source: https://www.dimensions.com/element/green-iguana-iguana-iguana
            weights[CreatureConstants.Lizard][GenderConstants.Female] = (2, 9);
            weights[CreatureConstants.Lizard][GenderConstants.Male] = (2, 9);
            //Source: https://www.dimensions.com/element/savannah-monitor-varanus-exanthematicus
            weights[CreatureConstants.Lizard_Monitor][GenderConstants.Female] = (11, 13);
            weights[CreatureConstants.Lizard_Monitor][GenderConstants.Male] = (11, 13);
            //Source: https://forgottenrealms.fandom.com/wiki/Lizardfolk
            weights[CreatureConstants.Lizardfolk][GenderConstants.Female] = (200, 250);
            weights[CreatureConstants.Lizardfolk][GenderConstants.Male] = (200, 250);
            //Source: https://forgottenrealms.fandom.com/wiki/Locathah
            weights[CreatureConstants.Locathah][GenderConstants.Female] = GetRangeFromAverage(175);
            weights[CreatureConstants.Locathah][GenderConstants.Male] = GetRangeFromAverage(175);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm Diminutive flying = 5000 creatures
            //https://www.dimensions.com/element/desert-locust-schistocerca-gregaria
            weights[CreatureConstants.Locust_Swarm][GenderConstants.Agender] = GetRangeFromAverage((int)(GetOunceToPound(.07) * 5000));
            //Source: https://forgottenrealms.fandom.com/wiki/Magmin
            weights[CreatureConstants.Magmin][GenderConstants.Agender] = GetRangeFromAverage(400);
            //Source: https://www.dimensions.com/element/reef-manta-ray-mobula-alfredi
            weights[CreatureConstants.MantaRay][GenderConstants.Female] = (1543, 3086);
            weights[CreatureConstants.MantaRay][GenderConstants.Male] = (1543, 3086);
            //Source: https://forgottenrealms.fandom.com/wiki/Manticore
            weights[CreatureConstants.Manticore][GenderConstants.Female] = GetRangeFromAverage(1000);
            weights[CreatureConstants.Manticore][GenderConstants.Male] = GetRangeFromAverage(1000);
            //Source: https://forgottenrealms.fandom.com/wiki/Marilith
            weights[CreatureConstants.Marilith][GenderConstants.Female] = GetRangeFromAverage(2 * 2000);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm scaling up from stone golem
            weights[CreatureConstants.Marut][GenderConstants.Agender] = GetScaledUpRangeFromAverage(
                weights[CreatureConstants.Golem_Stone][GenderConstants.Agender], 9 * 12, 12 * 12);
            //Source: https://www.d20srd.org/srd/monsters/medusa.htm Using Human
            weights[CreatureConstants.Medusa][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.Medusa][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Velociraptor
            weights[CreatureConstants.Megaraptor][GenderConstants.Female] = GetRangeFromAverage(498);
            weights[CreatureConstants.Megaraptor][GenderConstants.Male] = GetRangeFromAverage(498);
            //Source: https://www.d20srd.org/srd/monsters/mephit.htm
            weights[CreatureConstants.Mephit_Air][GenderConstants.Agender] = GetRangeFromAverage(1);
            weights[CreatureConstants.Mephit_Air][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Mephit_Air][GenderConstants.Male] = GetRangeFromAverage(1);
            weights[CreatureConstants.Mephit_Dust][GenderConstants.Agender] = GetRangeFromAverage(2);
            weights[CreatureConstants.Mephit_Dust][GenderConstants.Female] = GetRangeFromAverage(2);
            weights[CreatureConstants.Mephit_Dust][GenderConstants.Male] = GetRangeFromAverage(2);
            weights[CreatureConstants.Mephit_Earth][GenderConstants.Agender] = GetRangeFromAverage(80);
            weights[CreatureConstants.Mephit_Earth][GenderConstants.Female] = GetRangeFromAverage(80);
            weights[CreatureConstants.Mephit_Earth][GenderConstants.Male] = GetRangeFromAverage(80);
            weights[CreatureConstants.Mephit_Fire][GenderConstants.Agender] = GetRangeFromAverage(1);
            weights[CreatureConstants.Mephit_Fire][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Mephit_Fire][GenderConstants.Male] = GetRangeFromAverage(1);
            weights[CreatureConstants.Mephit_Ice][GenderConstants.Agender] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Ice][GenderConstants.Female] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Ice][GenderConstants.Male] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Magma][GenderConstants.Agender] = GetRangeFromAverage(60);
            weights[CreatureConstants.Mephit_Magma][GenderConstants.Female] = GetRangeFromAverage(60);
            weights[CreatureConstants.Mephit_Magma][GenderConstants.Male] = GetRangeFromAverage(60);
            weights[CreatureConstants.Mephit_Ooze][GenderConstants.Agender] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Ooze][GenderConstants.Female] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Ooze][GenderConstants.Male] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Salt][GenderConstants.Agender] = GetRangeFromAverage(80);
            weights[CreatureConstants.Mephit_Salt][GenderConstants.Female] = GetRangeFromAverage(80);
            weights[CreatureConstants.Mephit_Salt][GenderConstants.Male] = GetRangeFromAverage(80);
            weights[CreatureConstants.Mephit_Steam][GenderConstants.Agender] = GetRangeFromAverage(2);
            weights[CreatureConstants.Mephit_Steam][GenderConstants.Female] = GetRangeFromAverage(2);
            weights[CreatureConstants.Mephit_Steam][GenderConstants.Male] = GetRangeFromAverage(2);
            weights[CreatureConstants.Mephit_Water][GenderConstants.Agender] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Water][GenderConstants.Female] = GetRangeFromAverage(30);
            weights[CreatureConstants.Mephit_Water][GenderConstants.Male] = GetRangeFromAverage(30);
            //Source: https://forgottenrealms.fandom.com/wiki/Merfolk
            weights[CreatureConstants.Merfolk][GenderConstants.Female] = GetRangeFromAverage(400);
            weights[CreatureConstants.Merfolk][GenderConstants.Male] = GetRangeFromAverage(400);
            //Source: https://www.d20srd.org/srd/monsters/mimic.htm
            weights[CreatureConstants.Mimic][GenderConstants.Agender] = GetRangeFromAverage(4500);
            //Source: https://forgottenrealms.fandom.com/wiki/Mind_flayer Using Human
            weights[CreatureConstants.MindFlayer][GenderConstants.Agender] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://forgottenrealms.fandom.com/wiki/Minotaur
            weights[CreatureConstants.Minotaur][GenderConstants.Female] = GetRangeFromAverage(700);
            weights[CreatureConstants.Minotaur][GenderConstants.Male] = GetRangeFromAverage(700);
            //Source: https://www.d20srd.org/srd/monsters/mohrg.htm
            weights[CreatureConstants.Mohrg][GenderConstants.Agender] = GetRangeFromAverage(120);
            //Source: https://www.dimensions.com/element/tufted-capuchin-sapajus-apella
            weights[CreatureConstants.Monkey][GenderConstants.Female] = (4, 11);
            weights[CreatureConstants.Monkey][GenderConstants.Male] = (4, 11);
            //Source: https://www.dimensions.com/element/mule-equus-asinus-x-equus-caballus
            weights[CreatureConstants.Mule][GenderConstants.Female] = (820, 1000);
            weights[CreatureConstants.Mule][GenderConstants.Male] = (820, 1000);
            //Source: https://www.d20srd.org/srd/monsters/mummy.htm
            weights[CreatureConstants.Mummy][GenderConstants.Female] = GetRangeFromAverage(120);
            weights[CreatureConstants.Mummy][GenderConstants.Male] = GetRangeFromAverage(120);
            //Source: https://www.d20srd.org/srd/monsters/naga.htm
            weights[CreatureConstants.Naga_Dark][GenderConstants.Hermaphrodite] = (200, 500);
            //Source: https://forgottenrealms.fandom.com/wiki/Guardian_naga
            weights[CreatureConstants.Naga_Guardian][GenderConstants.Hermaphrodite] = (200, 500);
            //Source: https://forgottenrealms.fandom.com/wiki/Spirit_naga
            weights[CreatureConstants.Naga_Spirit][GenderConstants.Hermaphrodite] = GetRangeFromAverage(300);
            //Source: https://forgottenrealms.fandom.com/wiki/Water_naga
            weights[CreatureConstants.Naga_Water][GenderConstants.Hermaphrodite] = (200, 500);
            //Source: https://forgottenrealms.fandom.com/wiki/Nalfeshnee
            weights[CreatureConstants.Nalfeshnee][GenderConstants.Agender] = GetRangeFromAverage(8000);
            //Source: https://www.d20srd.org/srd/monsters/nightHag.htm
            weights[CreatureConstants.NightHag][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            //Source: https://www.d20srd.org/srd/monsters/nightshade.htm
            weights[CreatureConstants.Nightcrawler][GenderConstants.Agender] = GetRangeFromAverage(55_000);
            //Source: https://www.d20srd.org/srd/monsters/nightmare.htm
            weights[CreatureConstants.Nightmare][GenderConstants.Agender] = (800, 1000);
            //Scale up x8
            weights[CreatureConstants.Nightmare_Cauchemar][GenderConstants.Agender] = (800 * 8, 1000 * 8);
            //Source: https://www.d20srd.org/srd/monsters/nightshade.htm
            weights[CreatureConstants.Nightwalker][GenderConstants.Agender] = GetRangeFromAverage(12_000);
            //Source: https://www.d20srd.org/srd/monsters/nightshade.htm
            weights[CreatureConstants.Nightwing][GenderConstants.Agender] = GetRangeFromAverage(4000);
            //Source: https://www.d20srd.org/srd/monsters/sprite.htm#nixie
            weights[CreatureConstants.Nixie][GenderConstants.Female] = GetRangeFromAverage(45);
            weights[CreatureConstants.Nixie][GenderConstants.Male] = GetRangeFromAverage(45);
            //Source: https://www.d20srd.org/srd/monsters/nymph.htm
            weights[CreatureConstants.Nymph][GenderConstants.Female] = weights[CreatureConstants.Elf_High][GenderConstants.Female];
            //Source: https://www.d20srd.org/srd/monsters/ooze.htm#ochreJelly
            weights[CreatureConstants.OchreJelly][GenderConstants.Agender] = GetRangeFromAverage(5600);
            //Source: https://www.dimensions.com/element/common-octopus-octopus-vulgaris
            weights[CreatureConstants.Octopus][GenderConstants.Female] = (6, 22);
            weights[CreatureConstants.Octopus][GenderConstants.Male] = (6, 22);
            //Source: https://www.dimensions.com/element/giant-pacific-octopus-enteroctopus-dofleini
            weights[CreatureConstants.Octopus_Giant][GenderConstants.Female] = (22, 110);
            weights[CreatureConstants.Octopus_Giant][GenderConstants.Male] = (22, 110);
            //Source: https://forgottenrealms.fandom.com/wiki/Ogre
            weights[CreatureConstants.Ogre][GenderConstants.Female] = (600, 690);
            weights[CreatureConstants.Ogre][GenderConstants.Male] = (600, 690);
            weights[CreatureConstants.Ogre_Merrow][GenderConstants.Female] = (600, 690);
            weights[CreatureConstants.Ogre_Merrow][GenderConstants.Male] = (600, 690);
            //Source: https://forgottenrealms.fandom.com/wiki/Oni_mage
            weights[CreatureConstants.OgreMage][GenderConstants.Female] = GetRangeFromAverage(700);
            weights[CreatureConstants.OgreMage][GenderConstants.Male] = GetRangeFromAverage(700);
            //Source: https://forgottenrealms.fandom.com/wiki/Orc
            weights[CreatureConstants.Orc][GenderConstants.Female] = (230, 280);
            weights[CreatureConstants.Orc][GenderConstants.Male] = (230, 280);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Orc_Half][GenderConstants.Female] = (110 + 2 * 2, 110 + 2 * 12 * 2 * 6);
            weights[CreatureConstants.Orc_Half][GenderConstants.Male] = (150 + 2 * 2, 150 + 2 * 12 * 2 * 6);
            //Source: https://www.d20srd.org/srd/monsters/otyugh.htm
            weights[CreatureConstants.Otyugh][GenderConstants.Hermaphrodite] = GetRangeFromAverage(500);
            //Source: https://www.dimensions.com/element/great-horned-owl-bubo-virginianus
            weights[CreatureConstants.Owl][GenderConstants.Female] = (2, 6);
            weights[CreatureConstants.Owl][GenderConstants.Male] = (2, 6);
            //Source: https://www.d20srd.org/srd/monsters/owlGiant.htm
            //https://www.dimensions.com/element/great-horned-owl-bubo-virginianus scale up
            //weights[CreatureConstants.Owl_Giant][GenderConstants.Female] = (2754, 3456);
            weights[CreatureConstants.Owl_Giant][GenderConstants.Female] = GetScaledUpRangeFromAverage(2, 6, 9, 14, 9 * 12);
            weights[CreatureConstants.Owl_Giant][GenderConstants.Male] = GetScaledUpRangeFromAverage(2, 6, 9, 14, 9 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Owlbear Females are a little smaller, so 95%
            weights[CreatureConstants.Owlbear][GenderConstants.Female] = (1235, 1435);
            weights[CreatureConstants.Owlbear][GenderConstants.Male] = (1300, 1500);
            //Source: https://www.d20srd.org/srd/monsters/pegasus.htm
            weights[CreatureConstants.Pegasus][GenderConstants.Female] = GetRangeFromAverage(1500);
            weights[CreatureConstants.Pegasus][GenderConstants.Male] = GetRangeFromAverage(1500);
            //Source: https://www.d20pfsrd.com/bestiary/monster-listings/plants/fungus-phantom/
            weights[CreatureConstants.PhantomFungus][GenderConstants.Agender] = GetRangeFromAverage(200);
            //Source: https://www.d20srd.org/srd/monsters/phaseSpider.htm
            weights[CreatureConstants.PhaseSpider][GenderConstants.Female] = GetRangeFromAverage(700);
            weights[CreatureConstants.PhaseSpider][GenderConstants.Male] = GetRangeFromAverage(700);
            //Source: https://www.d20srd.org/srd/monsters/phasm.htm
            weights[CreatureConstants.Phasm][GenderConstants.Agender] = GetRangeFromAverage(400);
            //Source: https://forgottenrealms.fandom.com/wiki/Pit_fiend
            weights[CreatureConstants.PitFiend][GenderConstants.Agender] = GetRangeFromAverage(800);
            //Source: https://forgottenrealms.fandom.com/wiki/Pixie
            weights[CreatureConstants.Pixie][GenderConstants.Female] = GetRangeFromAverage(30);
            weights[CreatureConstants.Pixie][GenderConstants.Male] = GetRangeFromAverage(30);
            weights[CreatureConstants.Pixie_WithIrresistibleDance][GenderConstants.Female] = GetRangeFromAverage(30);
            weights[CreatureConstants.Pixie_WithIrresistibleDance][GenderConstants.Male] = GetRangeFromAverage(30);
            //Source: https://www.dimensions.com/element/shetland-pony
            weights[CreatureConstants.Pony][GenderConstants.Female] = (400, 450);
            weights[CreatureConstants.Pony][GenderConstants.Male] = (400, 450);
            weights[CreatureConstants.Pony_War][GenderConstants.Female] = (400, 450);
            weights[CreatureConstants.Pony_War][GenderConstants.Male] = (400, 450);
            //Source: https://www.d20srd.org/srd/monsters/porpoise.htm
            weights[CreatureConstants.Porpoise][GenderConstants.Female] = (110, 160);
            weights[CreatureConstants.Porpoise][GenderConstants.Male] = (110, 160);
            //Source: http://www.biokids.umich.edu/critters/Tenodera_aridifolia/
            //https://forgottenrealms.fandom.com/wiki/Giant_praying_mantis scale up
            weights[CreatureConstants.PrayingMantis_Giant][GenderConstants.Female] =
                GetScaledUpRange(GetGramToPound(3) * .9, GetGramToPound(3) * 1.1, 2, 5, 2 * 12, 5 * 12);
            weights[CreatureConstants.PrayingMantis_Giant][GenderConstants.Male] =
                GetScaledUpRange(GetGramToPound(3) * .9, GetGramToPound(3) * 1.1, 2, 5, 2 * 12, 5 * 12);
            //Source: https://www.d20srd.org/srd/monsters/pseudodragon.htm
            weights[CreatureConstants.Pseudodragon][GenderConstants.Female] = GetRangeFromAverage(7);
            weights[CreatureConstants.Pseudodragon][GenderConstants.Male] = GetRangeFromAverage(7);
            //Source: https://www.d20srd.org/srd/monsters/purpleWorm.htm
            weights[CreatureConstants.PurpleWorm][GenderConstants.Female] = GetRangeFromAverage(40_000);
            weights[CreatureConstants.PurpleWorm][GenderConstants.Male] = GetRangeFromAverage(40_000);
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra
            weights[CreatureConstants.Pyrohydra_5Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_5Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_6Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_6Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_7Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_7Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_8Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_8Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_9Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_9Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_10Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_10Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_11Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_11Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_12Heads][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.Pyrohydra_12Heads][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://forgottenrealms.fandom.com/wiki/Quasit
            weights[CreatureConstants.Quasit][GenderConstants.Agender] = GetRangeFromAverage(8);
            //Source: https://www.d20srd.org/srd/monsters/rakshasa.htm
            weights[CreatureConstants.Rakshasa][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.Rakshasa][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://www.d20srd.org/srd/monsters/rast.htm
            weights[CreatureConstants.Rast][GenderConstants.Agender] = GetRangeFromAverage(200);
            //Source: https://www.dimensions.com/element/common-rat
            weights[CreatureConstants.Rat][GenderConstants.Female] = (1, 2);
            weights[CreatureConstants.Rat][GenderConstants.Male] = (1, 2);
            //Source: https://www.d20srd.org/srd/monsters/direRat.htm
            weights[CreatureConstants.Rat_Dire][GenderConstants.Female] = GetRangeFromAtLeast(50);
            weights[CreatureConstants.Rat_Dire][GenderConstants.Male] = GetRangeFromAtLeast(50);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm Tiny non-flying, so 300x(.6-1.5)=[180,450]
            weights[CreatureConstants.Rat_Swarm][GenderConstants.Agender] = ((int)(.6 * 300), (int)(1.5 * 300));
            //Source: https://www.dimensions.com/element/common-raven-corvus-corax
            weights[CreatureConstants.Raven][GenderConstants.Female] = (1, 5);
            weights[CreatureConstants.Raven][GenderConstants.Male] = (1, 5);
            //Source: https://www.d20srd.org/srd/monsters/ravid.htm
            weights[CreatureConstants.Ravid][GenderConstants.Agender] = GetRangeFromAverage(75);
            //Source: https://www.d20srd.org/srd/monsters/razorBoar.htm - Copying from Dire Boar
            weights[CreatureConstants.RazorBoar][GenderConstants.Female] = weights[CreatureConstants.Boar_Dire][GenderConstants.Female];
            weights[CreatureConstants.RazorBoar][GenderConstants.Male] = weights[CreatureConstants.Boar_Dire][GenderConstants.Male];
            //Source: https://www.d20srd.org/srd/monsters/remorhaz.htm
            weights[CreatureConstants.Remorhaz][GenderConstants.Female] = GetRangeFromAverage(10_000);
            weights[CreatureConstants.Remorhaz][GenderConstants.Male] = GetRangeFromAverage(10_000);
            //Source: https://forgottenrealms.fandom.com/wiki/Retriever
            weights[CreatureConstants.Retriever][GenderConstants.Agender] = GetRangeFromAverage(6500);
            //Source: https://www.d20srd.org/srd/monsters/rhinoceros.htm
            weights[CreatureConstants.Rhinoceras][GenderConstants.Female] = GetRangeFromUpTo(6000);
            weights[CreatureConstants.Rhinoceras][GenderConstants.Male] = GetRangeFromUpTo(6000);
            //Source: https://forgottenrealms.fandom.com/wiki/Roc
            weights[CreatureConstants.Roc][GenderConstants.Female] = GetRangeFromAverage(8000);
            weights[CreatureConstants.Roc][GenderConstants.Male] = GetRangeFromAverage(8000);
            //Source: https://www.d20srd.org/srd/monsters/roper.htm
            weights[CreatureConstants.Roper][GenderConstants.Hermaphrodite] = GetRangeFromAverage(2200);
            //Source: https://www.d20srd.org/srd/monsters/rustMonster.htm
            weights[CreatureConstants.RustMonster][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.RustMonster][GenderConstants.Male] = GetRangeFromAverage(200);
            //Source: https://forgottenrealms.fandom.com/wiki/Sahuagin
            weights[CreatureConstants.Sahuagin][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.Sahuagin][GenderConstants.Male] = GetRangeFromAverage(200);
            weights[CreatureConstants.Sahuagin_Malenti][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.Sahuagin_Malenti][GenderConstants.Male] = GetRangeFromAverage(200);
            weights[CreatureConstants.Sahuagin_Mutant][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.Sahuagin_Mutant][GenderConstants.Male] = GetRangeFromAverage(200);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/salamander-article (average)
            //Scaling down (eighth) for flamebrother, Scaling up x8 for noble.
            weights[CreatureConstants.Salamander_Flamebrother][GenderConstants.Agender] = (120 / 8, 130 / 8);
            weights[CreatureConstants.Salamander_Average][GenderConstants.Agender] = (120, 130);
            weights[CreatureConstants.Salamander_Noble][GenderConstants.Agender] = (120 * 8, 130 * 8);
            //Source: https://www.d20srd.org/srd/monsters/satyr.htm - copy from Half-Elf
            weights[CreatureConstants.Satyr][GenderConstants.Male] = weights[CreatureConstants.Elf_Half][GenderConstants.Male];
            weights[CreatureConstants.Satyr_WithPipes][GenderConstants.Male] = weights[CreatureConstants.Elf_Half][GenderConstants.Male];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Tiny
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Tiny][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 24);
            weights[CreatureConstants.Scorpion_Monstrous_Tiny][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 24);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Small
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Small][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 48);
            weights[CreatureConstants.Scorpion_Monstrous_Small][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 48);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Medium
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Medium][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 6 * 12);
            weights[CreatureConstants.Scorpion_Monstrous_Medium][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 6 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Large
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Large][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 120);
            weights[CreatureConstants.Scorpion_Monstrous_Large][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 120);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Huge
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Huge][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 240);
            weights[CreatureConstants.Scorpion_Monstrous_Huge][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 240);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Gargantuan
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Gargantuan][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 480);
            weights[CreatureConstants.Scorpion_Monstrous_Gargantuan][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 480);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Colossal
            //https://www.dimensions.com/element/giant-hairy-scorpion-hadrurus-arizonensis Scale up
            weights[CreatureConstants.Scorpion_Monstrous_Colossal][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 80 * 12);
            weights[CreatureConstants.Scorpion_Monstrous_Colossal][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(.14), GetOunceToPound(.25), 4, 7, 80 * 12);
            //Source: https://www.dandwiki.com/wiki/Tlincalli_(5e_Race)
            weights[CreatureConstants.Scorpionfolk][GenderConstants.Female] = (450 + 2 * 1, 450 + 2 * 12 * 1 * 4);
            weights[CreatureConstants.Scorpionfolk][GenderConstants.Male] = (450 + 2 * 1, 450 + 2 * 12 * 1 * 4);
            //Source: https://forgottenrealms.fandom.com/wiki/Sea_cat
            weights[CreatureConstants.SeaCat][GenderConstants.Female] = GetRangeFromUpTo(800);
            weights[CreatureConstants.SeaCat][GenderConstants.Male] = GetRangeFromUpTo(800);
            //Source: https://www.d20srd.org/srd/monsters/hag.htm - copy from Human
            weights[CreatureConstants.SeaHag][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            //Source: https://www.d20srd.org/srd/monsters/shadow.htm
            weights[CreatureConstants.Shadow][GenderConstants.Agender] = (0, 0);
            weights[CreatureConstants.Shadow_Greater][GenderConstants.Agender] = (0, 0);
            //Source: https://www.d20srd.org/srd/monsters/shadowMastiff.htm
            weights[CreatureConstants.ShadowMastiff][GenderConstants.Female] = GetRangeFromAverage(200);
            weights[CreatureConstants.ShadowMastiff][GenderConstants.Male] = GetRangeFromAverage(200);
            //Source: https://www.d20srd.org/srd/monsters/shamblingMound.htm
            weights[CreatureConstants.ShamblingMound][GenderConstants.Agender] = GetRangeFromAverage(3800);
            //Source: https://www.dimensions.com/element/blacktip-shark-carcharhinus-limbatus
            weights[CreatureConstants.Shark_Medium][GenderConstants.Female] = (150, 270);
            weights[CreatureConstants.Shark_Medium][GenderConstants.Male] = (150, 270);
            //Source: https://www.dimensions.com/element/thresher-shark
            weights[CreatureConstants.Shark_Large][GenderConstants.Female] = (500, 775);
            weights[CreatureConstants.Shark_Large][GenderConstants.Male] = (500, 775);
            //Source: https://www.dimensions.com/element/great-white-shark
            weights[CreatureConstants.Shark_Huge][GenderConstants.Female] = (1500, 2400);
            weights[CreatureConstants.Shark_Huge][GenderConstants.Male] = (1500, 2400);
            //Source: https://www.d20srd.org/srd/monsters/direShark.htm
            weights[CreatureConstants.Shark_Dire][GenderConstants.Female] = GetRangeFromAtLeast(20_000);
            weights[CreatureConstants.Shark_Dire][GenderConstants.Male] = GetRangeFromAtLeast(20_000);
            //Source: https://www.d20srd.org/srd/monsters/shieldGuardian.htm
            weights[CreatureConstants.ShieldGuardian][GenderConstants.Agender] = GetRangeFromAtLeast(1200);
            //Source: https://www.d20srd.org/srd/monsters/shockerLizard.htm
            weights[CreatureConstants.ShockerLizard][GenderConstants.Female] = GetRangeFromAverage(25);
            weights[CreatureConstants.ShockerLizard][GenderConstants.Male] = GetRangeFromAverage(25);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/shrieker-species
            weights[CreatureConstants.Shrieker][GenderConstants.Agender] = GetRangeFromAverage(35);
            //Source: https://www.d20srd.org/srd/monsters/skum.htm Copying from Human
            weights[CreatureConstants.Skum][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.Skum][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://forgottenrealms.fandom.com/wiki/Blue_slaad
            weights[CreatureConstants.Slaad_Blue][GenderConstants.Agender] = GetRangeFromAverage(1000);
            //Source: https://forgottenrealms.fandom.com/wiki/Red_slaad
            weights[CreatureConstants.Slaad_Red][GenderConstants.Agender] = GetRangeFromAverage(650);
            //Source: https://forgottenrealms.fandom.com/wiki/Green_slaad
            weights[CreatureConstants.Slaad_Green][GenderConstants.Agender] = GetRangeFromAverage(1000);
            //Source: https://forgottenrealms.fandom.com/wiki/Gray_slaad Scaling down from green
            weights[CreatureConstants.Slaad_Gray][GenderConstants.Agender] = GetRangeFromAverage(1000 / 8);
            //Source: https://forgottenrealms.fandom.com/wiki/Gray_slaadsame as gray 
            weights[CreatureConstants.Slaad_Death][GenderConstants.Agender] = weights[CreatureConstants.Slaad_Gray][GenderConstants.Agender];
            //Source: https://www.dimensions.com/element/green-tree-python-morelia-viridis
            weights[CreatureConstants.Snake_Constrictor][GenderConstants.Female] = (2, 4);
            weights[CreatureConstants.Snake_Constrictor][GenderConstants.Male] = (2, 4);
            //Source: https://www.dimensions.com/element/burmese-python-python-bivittatus
            weights[CreatureConstants.Snake_Constrictor_Giant][GenderConstants.Female] = (15, 165);
            weights[CreatureConstants.Snake_Constrictor_Giant][GenderConstants.Male] = (15, 165);
            //Source: https://www.dimensions.com/element/ribbon-snake-thamnophis-saurita 
            weights[CreatureConstants.Snake_Viper_Tiny][GenderConstants.Female] = (2, 3);
            weights[CreatureConstants.Snake_Viper_Tiny][GenderConstants.Male] = (2, 3);
            //Source: https://www.dimensions.com/element/copperhead-agkistrodon-contortrix 
            weights[CreatureConstants.Snake_Viper_Small][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Snake_Viper_Small][GenderConstants.Male] = GetRangeFromAverage(1);
            //Source: https://www.dimensions.com/element/western-diamondback-rattlesnake-crotalus-atrox 
            weights[CreatureConstants.Snake_Viper_Medium][GenderConstants.Female] = (3, 15);
            weights[CreatureConstants.Snake_Viper_Medium][GenderConstants.Male] = (3, 15);
            //Source: https://www.dimensions.com/element/black-mamba-dendroaspis-polylepis 
            weights[CreatureConstants.Snake_Viper_Large][GenderConstants.Female] = (2, 4);
            weights[CreatureConstants.Snake_Viper_Large][GenderConstants.Male] = (2, 4);
            //Source: https://www.dimensions.com/element/king-cobra-ophiophagus-hannah 
            weights[CreatureConstants.Snake_Viper_Huge][GenderConstants.Female] = (11, 15);
            weights[CreatureConstants.Snake_Viper_Huge][GenderConstants.Male] = (11, 15);
            weights[CreatureConstants.Spectre][GenderConstants.Female] = (0, 0);
            weights[CreatureConstants.Spectre][GenderConstants.Male] = (0, 0);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Tiny
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Tiny][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 2 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Tiny][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 2 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Small
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Small][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 3 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Small][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 3 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Medium][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 5 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Medium][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 5 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Large][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 10 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Large][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 10 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Huge
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Huge][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 15 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Huge][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 15 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Gargantuan
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 20 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 20 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Colossal
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_Hunter_Colossal][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 40 * 12);
            weights[CreatureConstants.Spider_Monstrous_Hunter_Colossal][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 40 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Tiny
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Tiny][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 2 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Tiny][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 2 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Small
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Small][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 3 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Small][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 3 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Medium][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 5 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Medium][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 5 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Large][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 10 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Large][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 10 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Huge
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Huge][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 15 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Huge][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 15 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Gargantuan
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 20 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 20 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Colossal
            //https://www.dimensions.com/element/goliath-birdeater-theraphosa-blondi Scale up
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal][GenderConstants.Female] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 40 * 12);
            weights[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetOunceToPound(5), GetOunceToPound(6.2), 8, 11, 40 * 12);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm Spiders are Diminutive, so x5000
            //https://a-z-animals.com/animals/black-widow-spider/ [0.035 ounces]x5000 = 10.9 pounds
            weights[CreatureConstants.Spider_Swarm][GenderConstants.Agender] = GetRangeFromAverage((int)(GetOunceToPound(.035) * 5000));
            //Source: https://www.d20srd.org/srd/monsters/spiderEater.htm
            weights[CreatureConstants.SpiderEater][GenderConstants.Female] = GetRangeFromAverage(4000);
            weights[CreatureConstants.SpiderEater][GenderConstants.Male] = GetRangeFromAverage(4000);
            //Source: https://www.dimensions.com/element/humboldt-squid-dosidicus-gigas
            weights[CreatureConstants.Squid][GenderConstants.Female] = (99, 110);
            weights[CreatureConstants.Squid][GenderConstants.Male] = (99, 110);
            //Source: https://www.dimensions.com/element/giant-squid 
            weights[CreatureConstants.Squid_Giant][GenderConstants.Female] = (440, 2000);
            weights[CreatureConstants.Squid_Giant][GenderConstants.Male] = (440, 2000);
            //Source: https://www.d20srd.org/srd/monsters/giantStagBeetle.htm
            //https://www.theanimalfacts.com/insects-spiders/hercules-beetle/ scale up
            weights[CreatureConstants.StagBeetle_Giant][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(34) * .9, GetGramToPound(34) * 1.1, 2.36, 7.09, 10 * 12);

            var femaleStagBeetle = GetScaledUpRangeFromAverage(GetGramToPound(16.3) * .9, GetGramToPound(16.3) * 1.1, 2.36, 7.09, 10 * 12);
            var maleStagBeetle = weights[CreatureConstants.StagBeetle_Giant][GenderConstants.Male];
            var maleRange = maleStagBeetle.Upper - maleStagBeetle.Lower;
            weights[CreatureConstants.StagBeetle_Giant][GenderConstants.Female] = (femaleStagBeetle.Lower, femaleStagBeetle.Lower + maleRange);
            //Source: https://www.d20srd.org/srd/monsters/stirge.htm
            weights[CreatureConstants.Stirge][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Stirge][GenderConstants.Male] = GetRangeFromAverage(1);
            //Source: https://forgottenrealms.fandom.com/wiki/Succubus
            weights[CreatureConstants.Succubus][GenderConstants.Female] = GetRangeFromAverage(125);
            weights[CreatureConstants.Succubus][GenderConstants.Male] = GetRangeFromAverage(125);
            //Source: https://www.d20srd.org/srd/monsters/tarrasque.htm
            weights[CreatureConstants.Tarrasque][GenderConstants.Agender] = GetRangeFromAverage(130 * 2000);
            //Source: https://www.d20srd.org/srd/monsters/tendriculos.htm
            weights[CreatureConstants.Tendriculos][GenderConstants.Agender] = GetRangeFromAverage(3500);
            //Source: https://www.d20srd.org/srd/monsters/thoqqua.htm
            weights[CreatureConstants.Thoqqua][GenderConstants.Agender] = GetRangeFromAverage(200);
            //Source: https://forgottenrealms.fandom.com/wiki/Tiefling
            weights[CreatureConstants.Tiefling][GenderConstants.Female] = (114, 238);
            weights[CreatureConstants.Tiefling][GenderConstants.Male] = (114, 238);
            //Source: https://www.d20srd.org/srd/monsters/tiger.htm
            weights[CreatureConstants.Tiger][GenderConstants.Female] = (400, 600);
            weights[CreatureConstants.Tiger][GenderConstants.Male] = (400, 600);
            //Source: https://www.d20srd.org/srd/monsters/direTiger.htm
            weights[CreatureConstants.Tiger_Dire][GenderConstants.Female] = GetRangeFromUpTo(6000);
            weights[CreatureConstants.Tiger_Dire][GenderConstants.Male] = GetRangeFromUpTo(6000);
            //Source: https://www.d20srd.org/srd/monsters/titan.htm
            weights[CreatureConstants.Titan][GenderConstants.Female] = GetRangeFromAverage(14_000);
            weights[CreatureConstants.Titan][GenderConstants.Male] = GetRangeFromAverage(14_000);
            //Source: https://www.dimensions.com/element/common-toad-bufo-bufo
            weights[CreatureConstants.Toad][GenderConstants.Female] = GetRangeFromAverage(1);
            weights[CreatureConstants.Toad][GenderConstants.Male] = GetRangeFromAverage(1);
            //Source: https://forgottenrealms.fandom.com/wiki/Tojanida
            weights[CreatureConstants.Tojanida_Juvenile][GenderConstants.Agender] = GetRangeFromAverage(60);
            weights[CreatureConstants.Tojanida_Adult][GenderConstants.Agender] = GetRangeFromAverage(220);
            weights[CreatureConstants.Tojanida_Elder][GenderConstants.Agender] = GetRangeFromAverage(500);
            //Source: https://www.d20srd.org/srd/monsters/treant.htm
            weights[CreatureConstants.Treant][GenderConstants.Female] = GetRangeFromAverage(4500);
            weights[CreatureConstants.Treant][GenderConstants.Male] = GetRangeFromAverage(4500);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Triceratops
            weights[CreatureConstants.Triceratops][GenderConstants.Female] = GetRangeFromAverage(12 * 2000);
            weights[CreatureConstants.Triceratops][GenderConstants.Male] = GetRangeFromAverage(12 * 2000);
            //Source: https://www.d20srd.org/srd/monsters/triton.htm Copying from Human
            weights[CreatureConstants.Triton][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.Triton][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://forgottenrealms.fandom.com/wiki/Troglodyte
            weights[CreatureConstants.Troglodyte][GenderConstants.Female] = GetRangeFromAverage(150);
            weights[CreatureConstants.Troglodyte][GenderConstants.Male] = GetRangeFromAverage(150);
            //Source: https://www.d20srd.org/srd/monsters/troll.htm Female "slightly larger than males", so 110%
            weights[CreatureConstants.Troll][GenderConstants.Male] = GetRangeFromAverage(500);
            weights[CreatureConstants.Troll][GenderConstants.Female] = GetRangeFromAverage(550, 500);
            weights[CreatureConstants.Troll_Scrag][GenderConstants.Male] = GetRangeFromAverage(500);
            weights[CreatureConstants.Troll_Scrag][GenderConstants.Female] = GetRangeFromAverage(550, 500);
            //Source: https://forgottenrealms.fandom.com/wiki/Trumpet_archon
            weights[CreatureConstants.TrumpetArchon][GenderConstants.Female] = (165, 210);
            weights[CreatureConstants.TrumpetArchon][GenderConstants.Male] = (185, 230);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Tyrannosaurus
            weights[CreatureConstants.Tyrannosaurus][GenderConstants.Male] = GetRangeFromAverage(36_600);
            weights[CreatureConstants.Tyrannosaurus][GenderConstants.Female] = GetRangeFromAverage(36_600);
            //Source: https://forgottenrealms.fandom.com/wiki/Umber_hulk
            weights[CreatureConstants.UmberHulk][GenderConstants.Female] = (800, 1750);
            weights[CreatureConstants.UmberHulk][GenderConstants.Male] = (800, 1750);
            weights[CreatureConstants.UmberHulk_TrulyHorrid][GenderConstants.Female] = GetRangeFromAverage(8000);
            weights[CreatureConstants.UmberHulk_TrulyHorrid][GenderConstants.Male] = GetRangeFromAverage(8000);
            //Source: https://www.d20srd.org/srd/monsters/unicorn.htm Females "slightly smaller", so 90%
            weights[CreatureConstants.Unicorn][GenderConstants.Female] = GetRangeFromAverage(1080, 1200);
            weights[CreatureConstants.Unicorn][GenderConstants.Male] = GetRangeFromAverage(1200);
            //Source: https://www.d20srd.org/srd/monsters/vampire.htm#vampireSpawn Copying from Human
            weights[CreatureConstants.VampireSpawn][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.VampireSpawn][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://www.d20srd.org/srd/monsters/vargouille.htm
            weights[CreatureConstants.Vargouille][GenderConstants.Agender] = GetRangeFromAverage(10);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/violet-fungus-species
            weights[CreatureConstants.VioletFungus][GenderConstants.Agender] = GetRangeFromAverage(50);
            //Source: https://forgottenrealms.fandom.com/wiki/Vrock
            weights[CreatureConstants.Vrock][GenderConstants.Agender] = GetRangeFromAverage(500);
            //Source: https://en.wikipedia.org/wiki/Eastern_yellowjacket scale up
            weights[CreatureConstants.Wasp_Giant][GenderConstants.Male] =
                GetScaledUpRangeFromAverage(GetGramToPound(.04) * .9, GetGramToPound(.04) * 1.1, .94, 1.26, 5 * 12);
            //Source: https://www.dimensions.com/element/least-weasel-mustela-nivalis
            weights[CreatureConstants.Weasel][GenderConstants.Female] = (1, 3);
            weights[CreatureConstants.Weasel][GenderConstants.Male] = (1, 3);
            //Source: https://www.d20srd.org/srd/monsters/direWeasel.htm
            weights[CreatureConstants.Weasel_Dire][GenderConstants.Female] = GetRangeFromUpTo(700);
            weights[CreatureConstants.Weasel_Dire][GenderConstants.Male] = GetRangeFromUpTo(700);
            //Source: https://www.dimensions.com/element/humpback-whale-megaptera-novaeangliae
            weights[CreatureConstants.Whale_Baleen][GenderConstants.Female] = (27 * 2000 + 1000, 33 * 2000);
            weights[CreatureConstants.Whale_Baleen][GenderConstants.Male] = (27 * 2000 + 1000, 33 * 2000);
            //Source: https://www.dimensions.com/element/sperm-whale-physeter-macrocephalus
            weights[CreatureConstants.Whale_Cachalot][GenderConstants.Female] = (39 * 2000, 65 * 2000);
            weights[CreatureConstants.Whale_Cachalot][GenderConstants.Male] = (39 * 2000, 65 * 2000);
            //Source: https://www.dimensions.com/element/orca-killer-whale-orcinus-orca
            weights[CreatureConstants.Whale_Orca][GenderConstants.Female] = (3000, 6 * 2000);
            weights[CreatureConstants.Whale_Orca][GenderConstants.Male] = (3000, 6 * 2000);
            //Source: https://www.d20srd.org/srd/monsters/wight.htm Copy from Human
            weights[CreatureConstants.Wight][GenderConstants.Female] = weights[CreatureConstants.Human][GenderConstants.Female];
            weights[CreatureConstants.Wight][GenderConstants.Male] = weights[CreatureConstants.Human][GenderConstants.Male];
            //Source: https://www.d20srd.org/srd/monsters/willOWisp.htm
            weights[CreatureConstants.WillOWisp][GenderConstants.Agender] = GetRangeFromAverage(3);
            //Source: https://www.d20srd.org/srd/monsters/winterWolf.htm
            weights[CreatureConstants.WinterWolf][GenderConstants.Female] = GetRangeFromAverage(450);
            weights[CreatureConstants.WinterWolf][GenderConstants.Male] = GetRangeFromAverage(450);
            //Source: https://www.dimensions.com/element/gray-wolf
            weights[CreatureConstants.Wolf][GenderConstants.Female] = (50, 150);
            weights[CreatureConstants.Wolf][GenderConstants.Male] = (50, 150);
            //Source: https://www.d20srd.org/srd/monsters/direWolf.htm
            weights[CreatureConstants.Wolf_Dire][GenderConstants.Female] = GetRangeFromAverage(800);
            weights[CreatureConstants.Wolf_Dire][GenderConstants.Male] = GetRangeFromAverage(800);
            //Source: https://www.dimensions.com/element/wolverine-gulo-gulo
            weights[CreatureConstants.Wolverine][GenderConstants.Female] = (15, 62);
            weights[CreatureConstants.Wolverine][GenderConstants.Male] = (15, 62);
            //Source: https://www.d20srd.org/srd/monsters/direWolverine.htm
            weights[CreatureConstants.Wolverine_Dire][GenderConstants.Female] = GetRangeFromUpTo(2000);
            weights[CreatureConstants.Wolverine_Dire][GenderConstants.Male] = GetRangeFromUpTo(2000);
            //Source: https://www.d20srd.org/srd/monsters/worg.htm
            weights[CreatureConstants.Worg][GenderConstants.Female] = GetRangeFromAverage(300);
            weights[CreatureConstants.Worg][GenderConstants.Male] = GetRangeFromAverage(300);
            //Source: https://www.d20srd.org/srd/monsters/wraith.htm
            weights[CreatureConstants.Wraith][GenderConstants.Agender] = (0, 0);
            weights[CreatureConstants.Wraith_Dread][GenderConstants.Agender] = (0, 0);
            //Source: https://forgottenrealms.fandom.com/wiki/Wyvern
            weights[CreatureConstants.Wyvern][GenderConstants.Female] = GetRangeFromAverage(2000);
            weights[CreatureConstants.Wyvern][GenderConstants.Male] = GetRangeFromAverage(2000);
            //Source: https://www.d20srd.org/srd/monsters/xill.htm
            weights[CreatureConstants.Xill][GenderConstants.Agender] = GetRangeFromAverage(100);
            //Source: https://forgottenrealms.fandom.com/wiki/Xorn
            weights[CreatureConstants.Xorn_Minor][GenderConstants.Agender] = GetRangeFromAverage(120);
            weights[CreatureConstants.Xorn_Average][GenderConstants.Agender] = GetRangeFromAverage(600);
            weights[CreatureConstants.Xorn_Elder][GenderConstants.Agender] = GetRangeFromAverage(9000);
            //Source: https://forgottenrealms.fandom.com/wiki/Yeth_hound
            weights[CreatureConstants.YethHound][GenderConstants.Agender] = GetRangeFromAverage(400);
            //Source: https://www.d20srd.org/srd/monsters/yrthak.htm
            weights[CreatureConstants.Yrthak][GenderConstants.Agender] = GetRangeFromAverage(5000);
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_pureblood
            weights[CreatureConstants.YuanTi_Pureblood][GenderConstants.Female] = (90, 280);
            weights[CreatureConstants.YuanTi_Pureblood][GenderConstants.Male] = (90, 280);
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_malison
            weights[CreatureConstants.YuanTi_Halfblood_SnakeArms][GenderConstants.Female] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeArms][GenderConstants.Male] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeHead][GenderConstants.Female] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeHead][GenderConstants.Male] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeTail][GenderConstants.Female] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeTail][GenderConstants.Male] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs][GenderConstants.Female] = (90, 280);
            weights[CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs][GenderConstants.Male] = (90, 280);
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_abomination
            weights[CreatureConstants.YuanTi_Abomination][GenderConstants.Female] = (200, 300);
            weights[CreatureConstants.YuanTi_Abomination][GenderConstants.Male] = (200, 300);
            //Source: https://forgottenrealms.fandom.com/wiki/Zelekhut - using Centaur for target scale, but iron golem for source scale
            weights[CreatureConstants.Zelekhut][GenderConstants.Agender] =
                GetScaledUpRangeFromAverage(weights[CreatureConstants.Golem_Iron][GenderConstants.Agender], 12 * 12, 8 * 12);

            return weights;
        }

        public Dictionary<string, Dictionary<string, string>> GetCreatureWeightRolls()
        {
            var weights = new Dictionary<string, Dictionary<string, string>>();

            foreach (var kvp in creatureWeightRanges)
            {
                var creature = kvp.Key;
                weights[creature] = [];

                if (!kvp.Value.Values.Any())
                {
                    weights[creature][creature] = "NOT A VALID ROLL: NO GENDERS";
                    continue;
                }

                var genders = kvp.Value.Keys;
                var range = kvp.Value.Values.First();

                if (range.Upper < range.Lower)
                    throw new Exception($"{creature} has an invalid range of weights: [{range.Lower},{range.Upper}]");

                weights[creature][creature] = GetMultiplierFromRange(creature, range.Lower, range.Upper);

                foreach (var gender in genders)
                {
                    weights[creature][gender] = GetBaseFromRange(creature, kvp.Value[gender].Lower, kvp.Value[gender].Upper);
                }
            }

            //Incorporeal, so weight is 0
            weights[CreatureConstants.Allip][GenderConstants.Female] = "0";
            weights[CreatureConstants.Allip][GenderConstants.Male] = "0";
            weights[CreatureConstants.Allip][CreatureConstants.Allip] = "0";
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Dwarf_Duergar][GenderConstants.Female] = "100";
            weights[CreatureConstants.Dwarf_Duergar][GenderConstants.Male] = "130";
            weights[CreatureConstants.Dwarf_Duergar][CreatureConstants.Dwarf_Duergar] = "2d6";
            weights[CreatureConstants.Dwarf_Hill][GenderConstants.Female] = "100";
            weights[CreatureConstants.Dwarf_Hill][GenderConstants.Male] = "130";
            weights[CreatureConstants.Dwarf_Hill][CreatureConstants.Dwarf_Hill] = "2d6";
            weights[CreatureConstants.Dwarf_Mountain][GenderConstants.Female] = "100";
            weights[CreatureConstants.Dwarf_Mountain][GenderConstants.Male] = "130";
            weights[CreatureConstants.Dwarf_Mountain][CreatureConstants.Dwarf_Mountain] = "2d6";
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Elf_Drow][GenderConstants.Female] = "80";
            weights[CreatureConstants.Elf_Drow][GenderConstants.Male] = "85";
            weights[CreatureConstants.Elf_Drow][CreatureConstants.Elf_Drow] = "1d6";
            weights[CreatureConstants.Elf_Gray][GenderConstants.Female] = "80";
            weights[CreatureConstants.Elf_Gray][GenderConstants.Male] = "85";
            weights[CreatureConstants.Elf_Gray][CreatureConstants.Elf_Gray] = "1d6";
            weights[CreatureConstants.Elf_Half][GenderConstants.Female] = "80";
            weights[CreatureConstants.Elf_Half][GenderConstants.Male] = "100";
            weights[CreatureConstants.Elf_Half][CreatureConstants.Elf_Half] = "2d4";
            weights[CreatureConstants.Elf_High][GenderConstants.Female] = "80";
            weights[CreatureConstants.Elf_High][GenderConstants.Male] = "85";
            weights[CreatureConstants.Elf_High][CreatureConstants.Elf_High] = "1d6";
            weights[CreatureConstants.Elf_Wood][GenderConstants.Female] = "80";
            weights[CreatureConstants.Elf_Wood][GenderConstants.Male] = "85";
            weights[CreatureConstants.Elf_Wood][CreatureConstants.Elf_Wood] = "1d6";
            //Source: https://www.dandwiki.com/wiki/Ghoul_(5e_Race)
            weights[CreatureConstants.Ghoul][GenderConstants.Female] = "110";
            weights[CreatureConstants.Ghoul][GenderConstants.Male] = "110";
            weights[CreatureConstants.Ghoul][CreatureConstants.Ghoul] = "1d4";
            weights[CreatureConstants.Ghoul_Ghast][GenderConstants.Female] = "110";
            weights[CreatureConstants.Ghoul_Ghast][GenderConstants.Male] = "110";
            weights[CreatureConstants.Ghoul_Ghast][CreatureConstants.Ghoul_Ghast] = "1d4";
            weights[CreatureConstants.Ghoul_Lacedon][GenderConstants.Female] = "110";
            weights[CreatureConstants.Ghoul_Lacedon][GenderConstants.Male] = "110";
            weights[CreatureConstants.Ghoul_Lacedon][CreatureConstants.Ghoul_Lacedon] = "1d4";
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Gnome_Forest][GenderConstants.Female] = "35";
            weights[CreatureConstants.Gnome_Forest][GenderConstants.Male] = "40";
            weights[CreatureConstants.Gnome_Forest][CreatureConstants.Gnome_Forest] = "1";
            weights[CreatureConstants.Gnome_Rock][GenderConstants.Female] = "35";
            weights[CreatureConstants.Gnome_Rock][GenderConstants.Male] = "40";
            weights[CreatureConstants.Gnome_Rock][CreatureConstants.Gnome_Rock] = "1";
            weights[CreatureConstants.Gnome_Svirfneblin][GenderConstants.Female] = "35";
            weights[CreatureConstants.Gnome_Svirfneblin][GenderConstants.Male] = "40";
            weights[CreatureConstants.Gnome_Svirfneblin][CreatureConstants.Gnome_Svirfneblin] = "1";
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Halfling_Deep][GenderConstants.Female] = "25";
            weights[CreatureConstants.Halfling_Deep][GenderConstants.Male] = "30";
            weights[CreatureConstants.Halfling_Deep][CreatureConstants.Halfling_Deep] = "1";
            weights[CreatureConstants.Halfling_Lightfoot][GenderConstants.Female] = "25";
            weights[CreatureConstants.Halfling_Lightfoot][GenderConstants.Male] = "30";
            weights[CreatureConstants.Halfling_Lightfoot][CreatureConstants.Halfling_Lightfoot] = "1";
            weights[CreatureConstants.Halfling_Tallfellow][GenderConstants.Female] = "25";
            weights[CreatureConstants.Halfling_Tallfellow][GenderConstants.Male] = "30";
            weights[CreatureConstants.Halfling_Tallfellow][CreatureConstants.Halfling_Tallfellow] = "1";
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Human][GenderConstants.Female] = "85";
            weights[CreatureConstants.Human][GenderConstants.Male] = "120";
            weights[CreatureConstants.Human][CreatureConstants.Human] = "2d4";
            //Since they float and are just balls of light, say weight is 0
            weights[CreatureConstants.LanternArchon][GenderConstants.Agender] = "0";
            weights[CreatureConstants.LanternArchon][CreatureConstants.LanternArchon] = "0";
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            weights[CreatureConstants.Orc_Half][GenderConstants.Female] = "110";
            weights[CreatureConstants.Orc_Half][GenderConstants.Male] = "150";
            weights[CreatureConstants.Orc_Half][CreatureConstants.Orc_Half] = "2d6";
            //Source: https://www.d20srd.org/srd/monsters/shadow.htm
            weights[CreatureConstants.Shadow][GenderConstants.Agender] = "0";
            weights[CreatureConstants.Shadow][CreatureConstants.Shadow] = "0";
            weights[CreatureConstants.Shadow_Greater][GenderConstants.Agender] = "0";
            weights[CreatureConstants.Shadow_Greater][CreatureConstants.Shadow_Greater] = "0";
            weights[CreatureConstants.Spectre][GenderConstants.Female] = "0";
            weights[CreatureConstants.Spectre][GenderConstants.Male] = "0";
            weights[CreatureConstants.Spectre][CreatureConstants.Spectre] = "0";
            //Source: https://www.d20srd.org/srd/monsters/wraith.htm
            weights[CreatureConstants.Wraith][GenderConstants.Agender] = "0";
            weights[CreatureConstants.Wraith][CreatureConstants.Wraith] = "0";
            weights[CreatureConstants.Wraith_Dread][GenderConstants.Agender] = "0";
            weights[CreatureConstants.Wraith_Dread][CreatureConstants.Wraith_Dread] = "0";

            var templates = CreatureConstants.Templates.GetAll();
            const string BelowAverage = "-1";
            const string NoChange = "0";
            const string AboveAverage = "1";
            foreach (var template in templates)
            {
                weights[template] = [];
                weights[template][template] = NoChange;
            }

            weights[CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted][CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural][CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted][CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural][CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted][CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural][CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted][CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural][CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Boar_Afflicted][CreatureConstants.Templates.Lycanthrope_Boar_Afflicted] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Boar_Natural][CreatureConstants.Templates.Lycanthrope_Boar_Natural] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted][CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural][CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural] = AboveAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Rat_Afflicted][CreatureConstants.Templates.Lycanthrope_Rat_Afflicted] = BelowAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Rat_Natural][CreatureConstants.Templates.Lycanthrope_Rat_Natural] = BelowAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted][CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted] = BelowAverage;
            weights[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural][CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural] = BelowAverage;

            return weights;
        }

        private (int Lower, int Upper) GetRangeFromAverage(int average) => (Math.Max(1, average * 9 / 10), Math.Max(1, average * 11 / 10));
        private (int Lower, int Upper) GetRangeFromAverage(int average, int altAverage) => (average - altAverage / 10, average + altAverage / 10);
        private (int Lower, int Upper) GetRangeFromUpTo(int upTo) => (Math.Max(1, upTo * 9 / 11), Math.Max(1, upTo));
        private (int Lower, int Upper) GetRangeFromAtLeast(int atLeast) => (Math.Max(1, atLeast), Math.Max(1, atLeast * 11 / 9));

        private (int Lower, int Upper) GetScaledUpRange(int sourceWeightAverage, int sourceAverage, double targetLower, double targetUpper)
        {
            var sourceWeightRange = GetRangeFromAverage(sourceWeightAverage);
            var sourceRange = GetRangeFromAverage(sourceAverage);
            return GetScaledUpRange(sourceWeightRange, sourceRange.Lower, sourceRange.Upper, targetLower, targetUpper);
        }

        private (int Lower, int Upper) GetScaledUpRange((int Lower, int Upper) sourceWeightRange, double sourceLower, double sourceUpper, double targetLower, double targetUpper)
            => GetScaledUpRange(sourceWeightRange.Lower, sourceWeightRange.Upper, sourceLower, sourceUpper, targetLower, targetUpper);
        private (int Lower, int Upper) GetScaledUpRange(double sourceWeightLower, double sourceWeightUpper, double sourceLower, double sourceUpper, double targetLower, double targetUpper)
        {
            var target1 = sourceWeightLower * Math.Pow(targetLower / sourceLower, 3);
            var target2 = sourceWeightUpper * Math.Pow(targetUpper / sourceUpper, 3);

            var targetWeightLower = (int)Math.Min(target1, target2);
            var targetWeightUpper = (int)Math.Max(target1, target2);

            return (Math.Max(1, targetWeightLower), Math.Max(1, targetWeightUpper));
        }

        private double GetOunceToPound(double oz) => oz / 16;
        private double GetGramToPound(double grams) => grams / 453.6;
        private double GetMilligramToPound(double mg) => GetGramToPound(mg / 1000);

        private (int Lower, int Upper) GetScaledUpRangeFromAverage((int Lower, int Upper) sourceWeightRange, int sourceAverage, int average)
        {
            var sourceRange = GetRangeFromAverage(sourceAverage);
            return GetScaledUpRangeFromAverage(sourceWeightRange, sourceRange.Lower, sourceRange.Upper, average);
        }

        private (int Lower, int Upper) GetScaledUpRangeFromAverage((int Lower, int Upper) sourceWeightRange, double sourceLower, double sourceUpper, int average)
            => GetScaledUpRangeFromAverage(sourceWeightRange.Lower, sourceWeightRange.Upper, sourceLower, sourceUpper, average);
        private (int Lower, int Upper) GetScaledUpRangeFromAverage(double sourceWeightLower, double sourceWeightUpper, double sourceLower, double sourceUpper, int average)
        {
            var range = GetRangeFromAverage(average);
            return GetScaledUpRange(sourceWeightLower, sourceWeightUpper, sourceLower, sourceUpper, range.Lower, range.Upper);
        }

        private string GetBaseFromRange(string creature, int lower, int upper) => GetFromRange(creature, lower, upper, BASE_INDEX);
        private string GetMultiplierFromRange(string creature, int lower, int upper) => GetFromRange(creature, lower, upper, MULTIPLIER_INDEX);

        private string GetFromRange(string creature, int lower, int upper, int index)
        {
            var roll = GetTheoreticalWeightRoll(creature, lower, upper);
            if (string.IsNullOrEmpty(roll))
                return string.Empty;
            else if (roll.Contains("NO VALID WEIGHT ROLL"))
                return roll;

            return GetFromRoll(roll, index);
        }

        private string GetFromRoll(string roll, int index)
        {
            var plusIndex = roll.IndexOf('+');
            var minusIndex = roll.IndexOf('-');

            if (plusIndex == -1 && minusIndex == -1)
            {
                if (index == BASE_INDEX)
                    return "0";

                return roll;
            }

            var adjustmentIndex = new[] { plusIndex, minusIndex }.Where(i => i > -1).Min();

            if (index == MULTIPLIER_INDEX)
            {
                return roll[..adjustmentIndex];
            }

            if (plusIndex > -1)
            {
                return roll[(adjustmentIndex + 1)..];
            }

            return roll[adjustmentIndex..];
        }

        private string GetTheoreticalWeightRoll(string creature, int lower, int upper)
        {
            if (upper - lower == 0)
            {
                return $"0+{lower}";
            }

            lower = Math.Max(lower, 1);

            var heightLength = GetMaxOfHeightLength(creature);
            if (!heightLength.Any())
            {
                return string.Empty;
            }

            var multiplierRoll = ParseRoll(heightLength[creature]);
            var bestPrototype = WeightRollPrototype.GetBest(lower, upper, multiplierRoll.Quantity, multiplierRoll.Quantity * multiplierRoll.Die);

            if (!bestPrototype.IsValid)
            {
                return $"NO VALID WEIGHT ROLL FOR [{lower},{upper}]. MULTIPLIER ROLL: {heightLength[creature]}; BEST GUESS: {bestPrototype.Roll}; LOWER DIFF: {bestPrototype.LowerDiff}; UPPER DIFF: {bestPrototype.UpperDiff}";
            }

            return bestPrototype.Roll;
        }

        private Dictionary<string, string> GetMaxOfHeightLength(string creature)
        {
            if (!heights.ContainsKey(creature) || !heights[creature].ContainsKey(creature)
                || !lengths.ContainsKey(creature) || !lengths[creature].ContainsKey(creature))
            {
                return [];
            }

            var heightRoll = ParseRoll(heights[creature][creature]);
            var lengthRoll = ParseRoll(lengths[creature][creature]);

            if (lengthRoll.Quantity * lengthRoll.Die > heightRoll.Quantity * heightRoll.Die)
            {
                return lengths[creature];
            }

            return heights[creature];
        }

        private (int Quantity, int Die, int Adjustment) ParseRoll(string roll)
        {
            if (!roll.Contains('d'))
            {
                if (!roll.Contains('+'))
                    return (0, 0, Convert.ToInt32(roll));

                var subsections = roll.Split('+', '-');
                var subq = Convert.ToInt32(subsections[0]);
                var suba = 0;

                if (subsections.Length > 1)
                    suba = Convert.ToInt32(subsections[1]);

                if (roll.Contains('-'))
                    suba *= -1;

                return (subq, 1, suba);
            }

            var sections = roll.Split('d', '+', '-');
            var q = Convert.ToInt32(sections[0]);
            var d = 1;
            var a = 0;

            if (sections.Length > 1)
                d = Convert.ToInt32(sections[1]);

            if (sections.Length > 2)
                a = Convert.ToInt32(sections[2]);

            if (roll.Contains('-'))
                a *= -1;

            return (q, d, a);
        }

        [TestCase(CreatureConstants.Allip)]
        [TestCase(CreatureConstants.Dwarf_Duergar)]
        [TestCase(CreatureConstants.Dwarf_Hill)]
        [TestCase(CreatureConstants.Dwarf_Mountain)]
        [TestCase(CreatureConstants.Elf_Drow)]
        [TestCase(CreatureConstants.Elf_Gray)]
        [TestCase(CreatureConstants.Elf_Half)]
        [TestCase(CreatureConstants.Elf_High)]
        [TestCase(CreatureConstants.Elf_Wood)]
        [TestCase(CreatureConstants.Ghoul)]
        [TestCase(CreatureConstants.Ghoul_Ghast)]
        [TestCase(CreatureConstants.Ghoul_Lacedon)]
        [TestCase(CreatureConstants.Gnome_Forest)]
        [TestCase(CreatureConstants.Gnome_Rock)]
        [TestCase(CreatureConstants.Gnome_Svirfneblin)]
        [TestCase(CreatureConstants.Halfling_Deep)]
        [TestCase(CreatureConstants.Halfling_Lightfoot)]
        [TestCase(CreatureConstants.Halfling_Tallfellow)]
        [TestCase(CreatureConstants.Human)]
        [TestCase(CreatureConstants.LanternArchon)]
        [TestCase(CreatureConstants.Orc_Half)]
        [TestCase(CreatureConstants.Shadow)]
        [TestCase(CreatureConstants.Shadow_Greater)]
        [TestCase(CreatureConstants.Spectre)]
        [TestCase(CreatureConstants.Wraith)]
        [TestCase(CreatureConstants.Wraith_Dread)]
        public void HardCodedWeightRollMatchesRange(string creature)
        {
            foreach (var genderKvp in creatureWeightRanges[creature])
            {
                var gender = genderKvp.Key;
                var range = genderKvp.Value;

                AssertRollRange(creature, gender, range.Lower, range.Upper, true);

                if (range.Lower > 0)
                {
                    var roll = GetTheoreticalWeightRoll(creature, range.Lower, range.Upper);
                    var parsedRoll = ParseRoll(roll);

                    if (parsedRoll.Die > 1)
                        Assert.That($"{parsedRoll.Quantity}d{parsedRoll.Die}", Is.EqualTo(creatureWeightRolls[creature][creature]));
                    else
                        Assert.That(parsedRoll.Quantity.ToString(), Is.EqualTo(creatureWeightRolls[creature][creature]));

                    Assert.That(parsedRoll.Adjustment.ToString(), Is.EqualTo(creatureWeightRolls[creature][gender]));
                }
                else
                {
                    Assert.That(creatureWeightRolls[creature][creature], Is.EqualTo("0"));
                    Assert.That(creatureWeightRolls[creature][gender], Is.EqualTo("0"));
                }
            }
        }

        [TestCase(CreatureConstants.Aasimar)]
        [TestCase(CreatureConstants.Aboleth)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal)]
        [TestCase(CreatureConstants.AnimatedObject_Medium)]
        [TestCase(CreatureConstants.AnimatedObject_Small)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny)]
        [TestCase(CreatureConstants.Arrowhawk_Adult)]
        [TestCase(CreatureConstants.Arrowhawk_Elder)]
        [TestCase(CreatureConstants.Arrowhawk_Juvenile)]
        [TestCase(CreatureConstants.Barghest)]
        [TestCase(CreatureConstants.Beholder)]
        [TestCase(CreatureConstants.Cat)]
        [TestCase(CreatureConstants.ChainDevil_Kyton)]
        [TestCase(CreatureConstants.Crocodile_Giant)]
        [TestCase(CreatureConstants.Dragon_Silver_Young)]
        [TestCase(CreatureConstants.Gargoyle)]
        [TestCase(CreatureConstants.Raven)]
        [TestCase(CreatureConstants.Snake_Constrictor_Giant)]
        [TestCase(CreatureConstants.StagBeetle_Giant)]
        public void DEBUG_ValidateWeightRoll(string creature)
        {
            foreach (var genderKvp in creatureWeightRanges[creature])
            {
                var gender = genderKvp.Key;
                var range = genderKvp.Value;
                var roll = GetTheoreticalWeightRoll(creature, range.Lower, range.Upper);

                Assert.That(roll, Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"));
                Assert.That(dice.Roll(roll).IsValid(), Is.True);

                AssertRollRange(creature, gender, range.Lower, range.Upper, false);
            }
        }

        private void AssertIncorporealCreaturesAreWeightless(string creature)
        {
            var data = creatureDataSelector.SelectOneFrom(Config.Name, TableNameConstants.Collection.CreatureData, creature);
            if (data.Types.Contains(CreatureConstants.Types.Subtypes.Incorporeal) || creature == CreatureConstants.LanternArchon)
            {
                foreach (var genderKvp in creatureWeightRanges[creature])
                {
                    Assert.That(genderKvp.Value.Lower, Is.Zero, $"Lower; {genderKvp.Key} {creature}");
                    Assert.That(genderKvp.Value.Upper, Is.Zero, $"Upper; {genderKvp.Key} {creature}");
                }

                foreach (var genderKvp in creatureWeightRolls[creature])
                {
                    Assert.That(genderKvp.Value, Is.EqualTo("0"), $"Roll; {genderKvp.Key} {creature}");
                }
            }
            else
            {
                foreach (var genderKvp in creatureWeightRanges[creature])
                {
                    Assert.That(genderKvp.Value.Lower, Is.Positive, $"Lower; {genderKvp.Key} {creature}");
                    Assert.That(genderKvp.Value.Upper, Is.Positive.And.AtLeast(genderKvp.Value.Lower), $"Upper; {genderKvp.Key} {creature}");
                }

                var multiplier = GetMaxOfHeightLength(creature);

                foreach (var genderKvp in creatureWeightRolls[creature].Where(kvp => kvp.Key != creature))
                {
                    Assert.That(genderKvp.Value, Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"), $"Base; {genderKvp.Key} {creature}");
                    Assert.That(creatureWeightRolls[creature][creature], Is.Not.Empty.And.Not.Contain("NO VALID WEIGHT ROLL"), $"Base; {genderKvp.Key} {creature}");

                    var roll = dice.Roll($"{genderKvp.Value}+{multiplier[creature]}*{creatureWeightRolls[creature][creature]}").AsPotentialMinimum();
                    Assert.That(roll, Is.Positive);
                }
            }
        }
    }
}