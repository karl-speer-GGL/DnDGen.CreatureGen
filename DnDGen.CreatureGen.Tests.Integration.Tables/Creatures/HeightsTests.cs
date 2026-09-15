using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.Tables.Helpers;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.RollGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures
{
    [TestFixture]
    public class HeightsTests : TypesAndAmountsTests
    {
        private Dice dice;
        private MeasurementHelper measurementHelper;

        protected override string TableName => TableNameConstants.TypeAndAmount.Heights;
        private Dictionary<string, Dictionary<string, string>> creatureHeights;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            creatureHeights = GetCreatureHeights();
        }

        [SetUp]
        public void Setup()
        {
            dice = GetNewInstanceOf<Dice>();
            measurementHelper = GetNewInstanceOf<MeasurementHelper>();
        }

        [Test]
        public void HeightsNames()
        {
            var creatures = CreatureConstants.GetAll();
            var templates = CreatureConstants.Templates.GetAll();
            var names = creatures.Union(templates);
            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureHeights(string name)
        {
            Assert.That(creatureHeights, Contains.Key(name), $"TEST DATA: {name}");

            var typesAndRolls = creatureHeights[name];
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, name);
            Assert.That(typesAndRolls.Keys, Is.EquivalentTo(genders.Union([name])).And.Not.Empty, $"TEST DATA: {name}");

            foreach (var roll in typesAndRolls.Values)
            {
                var isValid = dice.Roll(roll).IsValid();
                Assert.That(isValid, Is.True, roll);
            }

            AssertTypesAndAmounts(name, typesAndRolls);
            AssertIfCreatureHasNoHeight_HasLength(name);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void TemplateHeights(string name)
        {
            Assert.That(creatureHeights, Contains.Key(name), $"TEST DATA: {name}");

            var typesAndRolls = creatureHeights[name];
            Assert.That(typesAndRolls.Keys, Is.EquivalentTo([name]), $"TEST DATA: {name}");

            foreach (var roll in typesAndRolls.Values)
            {
                var isValid = dice.Roll(roll).IsValid();
                Assert.That(isValid, Is.True, roll, $"TEST DATA: {name}");
                Assert.That(roll, Is.AnyOf(["-1", "0", "1"]), $"TEST DATA: {name}");
            }

            AssertTypesAndAmounts(name, typesAndRolls);
        }

        private Dictionary<string, Dictionary<string, string>> GetCreatureHeights()
        {
            var creatures = CreatureConstants.GetAll();
            var heights = new Dictionary<string, Dictionary<string, string>>();

            foreach (var creature in creatures)
            {
                heights[creature] = [];
            }

            //Source: https://forgottenrealms.fandom.com/wiki/Aasimar
            heights[CreatureConstants.Aasimar][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 1);
            heights[CreatureConstants.Aasimar][GenderConstants.Male] = GetBaseFromRange(5 * 12, 6 * 12 + 6);
            heights[CreatureConstants.Aasimar][CreatureConstants.Aasimar] = GetMultiplierFromRange(5 * 12, 6 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Aboleth
            heights[CreatureConstants.Aboleth][GenderConstants.Hermaphrodite] = "0";
            heights[CreatureConstants.Aboleth][CreatureConstants.Aboleth] = "0";
            //Source: https://www.d20srd.org/srd/monsters/achaierai.htm
            heights[CreatureConstants.Achaierai][GenderConstants.Female] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Achaierai][GenderConstants.Male] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Achaierai][CreatureConstants.Achaierai] = GetMultiplierFromAverage(15 * 12);
            //Basing off humans
            heights[CreatureConstants.Allip][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Allip][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Allip][CreatureConstants.Allip] = "2d10";
            //Source: https://www.mojobob.com/roleplay/monstrousmanual/s/sphinx.html
            heights[CreatureConstants.Androsphinx][GenderConstants.Male] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Androsphinx][CreatureConstants.Androsphinx] = GetMultiplierFromAverage(8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Astral_deva
            heights[CreatureConstants.Angel_AstralDeva][GenderConstants.Female] = GetBaseFromRange(7 * 12, 7 * 12 + 6);
            heights[CreatureConstants.Angel_AstralDeva][GenderConstants.Male] = GetBaseFromRange(7 * 12, 7 * 12 + 6);
            heights[CreatureConstants.Angel_AstralDeva][CreatureConstants.Angel_AstralDeva] = GetMultiplierFromRange(7 * 12, 7 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Planetar
            heights[CreatureConstants.Angel_Planetar][GenderConstants.Female] = GetBaseFromRange(8 * 12, 9 * 12);
            heights[CreatureConstants.Angel_Planetar][GenderConstants.Male] = GetBaseFromRange(8 * 12, 9 * 12);
            heights[CreatureConstants.Angel_Planetar][CreatureConstants.Angel_Planetar] = GetMultiplierFromRange(8 * 12, 9 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Solar
            heights[CreatureConstants.Angel_Solar][GenderConstants.Female] = GetBaseFromRange(9 * 12, 10 * 12);
            heights[CreatureConstants.Angel_Solar][GenderConstants.Male] = GetBaseFromRange(9 * 12, 10 * 12);
            heights[CreatureConstants.Angel_Solar][CreatureConstants.Angel_Solar] = GetMultiplierFromRange(9 * 12, 10 * 12);
            //Source: https://www.d20srd.org/srd/combat/movementPositionAndDistance.htm
            heights[CreatureConstants.AnimatedObject_Colossal][GenderConstants.Agender] = GetBaseFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal][CreatureConstants.AnimatedObject_Colossal] = GetMultiplierFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Colossal_Flexible][CreatureConstants.AnimatedObject_Colossal_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long][GenderConstants.Agender] = GetBaseFromRange(64 * 12 / 2, 128 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long][CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long] =
                GetMultiplierFromRange(64 * 12 / 2, 128 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden][GenderConstants.Agender] = GetBaseFromRange(64 * 12 / 2, 128 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden][CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden] =
                GetMultiplierFromRange(64 * 12 / 2, 128 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall][GenderConstants.Agender] = GetBaseFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall][CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall] =
                GetMultiplierFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = GetBaseFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden][CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden] =
                GetMultiplierFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Colossal_Sheetlike][CreatureConstants.AnimatedObject_Colossal_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Colossal_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_TwoLegs][CreatureConstants.AnimatedObject_Colossal_TwoLegs] = GetMultiplierFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden] =
                GetMultiplierFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(64 * 12 / 2, 128 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden][CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden] =
                GetMultiplierFromRange(64 * 12 / 2, 128 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Colossal_Wooden][GenderConstants.Agender] = GetBaseFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Colossal_Wooden][CreatureConstants.AnimatedObject_Colossal_Wooden] = GetMultiplierFromRange(64 * 12, 128 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan][GenderConstants.Agender] = GetBaseFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan][CreatureConstants.AnimatedObject_Gargantuan] = GetMultiplierFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Gargantuan_Flexible][CreatureConstants.AnimatedObject_Gargantuan_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long][GenderConstants.Agender] = GetBaseFromRange(32 * 12 / 2, 64 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long][CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long] =
                GetMultiplierFromRange(32 * 12 / 2, 64 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden][GenderConstants.Agender] = GetBaseFromRange(32 * 12 / 2, 64 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden][CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden] =
                GetMultiplierFromRange(32 * 12 / 2, 64 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall][GenderConstants.Agender] = GetBaseFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall][CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall] =
                GetMultiplierFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = GetBaseFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden][CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden] =
                GetMultiplierFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Gargantuan_Sheetlike][CreatureConstants.AnimatedObject_Gargantuan_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs][CreatureConstants.AnimatedObject_Gargantuan_TwoLegs] = GetMultiplierFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden] =
                GetMultiplierFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(32 * 12 / 2, 64 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden][CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden] =
                GetMultiplierFromRange(32 * 12 / 2, 64 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Gargantuan_Wooden][GenderConstants.Agender] = GetBaseFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Gargantuan_Wooden][CreatureConstants.AnimatedObject_Gargantuan_Wooden] = GetMultiplierFromRange(32 * 12, 64 * 12);
            heights[CreatureConstants.AnimatedObject_Huge][GenderConstants.Agender] = GetBaseFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge][CreatureConstants.AnimatedObject_Huge] = GetMultiplierFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Huge_Flexible][CreatureConstants.AnimatedObject_Huge_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long][GenderConstants.Agender] = GetBaseFromRange(16 * 12 / 2, 32 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long][CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long] =
                GetMultiplierFromRange(16 * 12 / 2, 32 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden][GenderConstants.Agender] = GetBaseFromRange(16 * 12 / 2, 32 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden][CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden] =
                GetMultiplierFromRange(16 * 12 / 2, 32 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall][GenderConstants.Agender] = GetBaseFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall][CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall] =
                GetMultiplierFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = GetBaseFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden][CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden] =
                GetMultiplierFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Huge_Sheetlike][CreatureConstants.AnimatedObject_Huge_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Huge_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_TwoLegs][CreatureConstants.AnimatedObject_Huge_TwoLegs] = GetMultiplierFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden] = GetMultiplierFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(16 * 12 / 2, 32 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Huge_Wheels_Wooden][CreatureConstants.AnimatedObject_Huge_Wheels_Wooden] =
                GetMultiplierFromRange(16 * 12 / 2, 32 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Huge_Wooden][GenderConstants.Agender] = GetBaseFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Huge_Wooden][CreatureConstants.AnimatedObject_Huge_Wooden] = GetMultiplierFromRange(16 * 12, 32 * 12);
            heights[CreatureConstants.AnimatedObject_Large][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large][CreatureConstants.AnimatedObject_Large] = GetMultiplierFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Large_Flexible][CreatureConstants.AnimatedObject_Large_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long][GenderConstants.Agender] = GetBaseFromRange(8 * 12 / 2, 16 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long][CreatureConstants.AnimatedObject_Large_MultipleLegs_Long] =
                GetMultiplierFromRange(8 * 12 / 2, 16 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden][GenderConstants.Agender] = GetBaseFromRange(8 * 12 / 2, 16 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden][CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden] =
                GetMultiplierFromRange(8 * 12 / 2, 16 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall][CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall] =
                GetMultiplierFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden][CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden] =
                GetMultiplierFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Large_Sheetlike][CreatureConstants.AnimatedObject_Large_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Large_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_TwoLegs][CreatureConstants.AnimatedObject_Large_TwoLegs] = GetMultiplierFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden] = GetMultiplierFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(8 * 12 / 2, 16 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Large_Wheels_Wooden][CreatureConstants.AnimatedObject_Large_Wheels_Wooden] =
                GetMultiplierFromRange(8 * 12 / 2, 16 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Large_Wooden][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Large_Wooden][CreatureConstants.AnimatedObject_Large_Wooden] = GetMultiplierFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.AnimatedObject_Medium][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium][CreatureConstants.AnimatedObject_Medium] = GetMultiplierFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Medium_Flexible][CreatureConstants.AnimatedObject_Medium_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Medium_MultipleLegs][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_MultipleLegs][CreatureConstants.AnimatedObject_Medium_MultipleLegs] = GetMultiplierFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden][CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden] = GetMultiplierFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Medium_Sheetlike][CreatureConstants.AnimatedObject_Medium_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Medium_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_TwoLegs][CreatureConstants.AnimatedObject_Medium_TwoLegs] = GetMultiplierFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden] = GetMultiplierFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(4 * 12 / 2, 8 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Medium_Wheels_Wooden][CreatureConstants.AnimatedObject_Medium_Wheels_Wooden] =
                GetMultiplierFromRange(4 * 12 / 2, 8 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Medium_Wooden][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Medium_Wooden][CreatureConstants.AnimatedObject_Medium_Wooden] = GetMultiplierFromRange(4 * 12, 8 * 12);
            heights[CreatureConstants.AnimatedObject_Small][GenderConstants.Agender] = GetBaseFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small][CreatureConstants.AnimatedObject_Small] = GetMultiplierFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Small_Flexible][CreatureConstants.AnimatedObject_Small_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Small_MultipleLegs][GenderConstants.Agender] = GetBaseFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_MultipleLegs][CreatureConstants.AnimatedObject_Small_MultipleLegs] = GetMultiplierFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden][CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden] = GetMultiplierFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Small_Sheetlike][CreatureConstants.AnimatedObject_Small_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Small_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_TwoLegs][CreatureConstants.AnimatedObject_Small_TwoLegs] = GetMultiplierFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden] = GetMultiplierFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(2 * 12 / 2, 4 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Small_Wheels_Wooden][CreatureConstants.AnimatedObject_Small_Wheels_Wooden] =
                GetMultiplierFromRange(2 * 12 / 2, 4 * 12 / 2);
            heights[CreatureConstants.AnimatedObject_Small_Wooden][GenderConstants.Agender] = GetBaseFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Small_Wooden][CreatureConstants.AnimatedObject_Small_Wooden] = GetMultiplierFromRange(2 * 12, 4 * 12);
            heights[CreatureConstants.AnimatedObject_Tiny][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny][CreatureConstants.AnimatedObject_Tiny] = GetMultiplierFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_Flexible][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Tiny_Flexible][CreatureConstants.AnimatedObject_Tiny_Flexible] = "0";
            heights[CreatureConstants.AnimatedObject_Tiny_MultipleLegs][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_MultipleLegs][CreatureConstants.AnimatedObject_Tiny_MultipleLegs] = GetMultiplierFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden][CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden] = GetMultiplierFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_Sheetlike][GenderConstants.Agender] = "0";
            heights[CreatureConstants.AnimatedObject_Tiny_Sheetlike][CreatureConstants.AnimatedObject_Tiny_Sheetlike] = "0";
            heights[CreatureConstants.AnimatedObject_Tiny_TwoLegs][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_TwoLegs][CreatureConstants.AnimatedObject_Tiny_TwoLegs] = GetMultiplierFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden][CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden] = GetMultiplierFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden][GenderConstants.Agender] = GetBaseFromRange(12 / 2, 24 / 2);
            heights[CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden][CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden] = GetMultiplierFromRange(12 / 2, 24 / 2);
            heights[CreatureConstants.AnimatedObject_Tiny_Wooden][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.AnimatedObject_Tiny_Wooden][CreatureConstants.AnimatedObject_Tiny_Wooden] = GetMultiplierFromRange(12, 24);
            //Source: https://www.d20srd.org/srd/monsters/ankheg.htm
            heights[CreatureConstants.Ankheg][GenderConstants.Female] = "0";
            heights[CreatureConstants.Ankheg][GenderConstants.Male] = "0";
            heights[CreatureConstants.Ankheg][CreatureConstants.Ankheg] = "0";
            //Source: https://www.d20srd.org/srd/monsters/hag.htm#annis
            heights[CreatureConstants.Annis][GenderConstants.Female] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Annis][CreatureConstants.Annis] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.d20srd.org/srd/monsters/giantAnt.htm
            //https://www.dimensions.com/element/black-garden-ant-lasius-niger - scale up, [.035,.05]*6*12/[.14,.2] = [18,18]
            heights[CreatureConstants.Ant_Giant_Worker][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Ant_Giant_Worker][CreatureConstants.Ant_Giant_Worker] = GetMultiplierFromAverage(18);
            heights[CreatureConstants.Ant_Giant_Soldier][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Ant_Giant_Soldier][CreatureConstants.Ant_Giant_Soldier] = GetMultiplierFromAverage(18);
            //https://www.dimensions.com/element/black-garden-ant-lasius-niger - scale up, [.035,.05]*9*12/[.31,.35] = [12,16]
            heights[CreatureConstants.Ant_Giant_Queen][GenderConstants.Female] = GetBaseFromRange(12, 16);
            heights[CreatureConstants.Ant_Giant_Queen][CreatureConstants.Ant_Giant_Queen] = GetMultiplierFromRange(12, 16);
            //Source: https://www.dimensions.com/element/eastern-lowland-gorilla-gorilla-beringei-graueri (using for female)
            //https://www.d20srd.org/srd/monsters/ape.htm (male)
            //Adjusting female max -3" to match range
            heights[CreatureConstants.Ape][GenderConstants.Female] = GetBaseFromRange(63, 72 - 3);
            heights[CreatureConstants.Ape][GenderConstants.Male] = GetBaseFromRange(5 * 12 + 6, 6 * 12);
            heights[CreatureConstants.Ape][CreatureConstants.Ape] = GetMultiplierFromRange(5 * 12 + 6, 6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/direApe.htm
            heights[CreatureConstants.Ape_Dire][GenderConstants.Female] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Ape_Dire][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Ape_Dire][CreatureConstants.Ape_Dire] = GetMultiplierFromAverage(9 * 12);
            //INFO: Based on Half-Elf, since could be Human, Half-Elf, or Drow
            heights[CreatureConstants.Aranea][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Aranea][GenderConstants.Male] = "4*12+7";
            heights[CreatureConstants.Aranea][CreatureConstants.Aranea] = "2d8";
            //Source: https://www.d20srd.org/srd/monsters/arrowhawk.htm
            heights[CreatureConstants.Arrowhawk_Juvenile][GenderConstants.Female] = "0";
            heights[CreatureConstants.Arrowhawk_Juvenile][GenderConstants.Male] = "0";
            heights[CreatureConstants.Arrowhawk_Juvenile][CreatureConstants.Arrowhawk_Juvenile] = "0";
            heights[CreatureConstants.Arrowhawk_Adult][GenderConstants.Female] = "0";
            heights[CreatureConstants.Arrowhawk_Adult][GenderConstants.Male] = "0";
            heights[CreatureConstants.Arrowhawk_Adult][CreatureConstants.Arrowhawk_Adult] = "0";
            heights[CreatureConstants.Arrowhawk_Elder][GenderConstants.Female] = "0";
            heights[CreatureConstants.Arrowhawk_Elder][GenderConstants.Male] = "0";
            heights[CreatureConstants.Arrowhawk_Elder][CreatureConstants.Arrowhawk_Elder] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Assassin_vine
            heights[CreatureConstants.AssassinVine][GenderConstants.Agender] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.AssassinVine][CreatureConstants.AssassinVine] = GetMultiplierFromAverage(20 * 12);
            //Source: https://www.d20srd.org/srd/monsters/athach.htm
            heights[CreatureConstants.Athach][GenderConstants.Female] = GetBaseFromAverage(18 * 12);
            heights[CreatureConstants.Athach][GenderConstants.Male] = GetBaseFromAverage(18 * 12);
            heights[CreatureConstants.Athach][CreatureConstants.Athach] = GetMultiplierFromAverage(18 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Avoral
            heights[CreatureConstants.Avoral][GenderConstants.Female] = GetBaseFromRange(6 * 12 + 6, 7 * 12);
            heights[CreatureConstants.Avoral][GenderConstants.Male] = GetBaseFromRange(6 * 12 + 6, 7 * 12);
            heights[CreatureConstants.Avoral][GenderConstants.Agender] = GetBaseFromRange(6 * 12 + 6, 7 * 12);
            heights[CreatureConstants.Avoral][CreatureConstants.Avoral] = GetMultiplierFromRange(6 * 12 + 6, 7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Azer
            heights[CreatureConstants.Azer][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Azer][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Azer][GenderConstants.Agender] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Azer][CreatureConstants.Azer] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Babau
            heights[CreatureConstants.Babau][GenderConstants.Agender] = GetBaseFromRange(6 * 12, 7 * 12);
            heights[CreatureConstants.Babau][CreatureConstants.Babau] = GetMultiplierFromRange(6 * 12, 7 * 12);
            //Source: https://www.dimensions.com/element/mandrill-mandrillus-sphinx
            heights[CreatureConstants.Baboon][GenderConstants.Female] = GetBaseFromRange(20, 36);
            heights[CreatureConstants.Baboon][GenderConstants.Male] = GetBaseFromRange(20, 36);
            heights[CreatureConstants.Baboon][CreatureConstants.Baboon] = GetMultiplierFromRange(20, 36);
            //Source: https://www.dimensions.com/element/honey-badger-mellivora-capensis
            heights[CreatureConstants.Badger][GenderConstants.Female] = GetBaseFromRange(11, 16);
            heights[CreatureConstants.Badger][GenderConstants.Male] = GetBaseFromRange(11, 16);
            heights[CreatureConstants.Badger][CreatureConstants.Badger] = GetMultiplierFromRange(11, 16);
            //Multiplying up from normal badger. Length is about x2 from normal low, 2.5x for high
            heights[CreatureConstants.Badger_Dire][GenderConstants.Female] = GetBaseFromRange(22, 40);
            heights[CreatureConstants.Badger_Dire][GenderConstants.Male] = GetBaseFromRange(22, 40);
            heights[CreatureConstants.Badger_Dire][CreatureConstants.Badger_Dire] = GetMultiplierFromRange(22, 40);
            //Source: https://forgottenrealms.fandom.com/wiki/Balor
            heights[CreatureConstants.Balor][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Balor][CreatureConstants.Balor] = GetMultiplierFromAverage(12 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Hamatula
            heights[CreatureConstants.BarbedDevil_Hamatula][GenderConstants.Agender] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.BarbedDevil_Hamatula][CreatureConstants.BarbedDevil_Hamatula] = GetMultiplierFromAverage(7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Barghest
            heights[CreatureConstants.Barghest][GenderConstants.Female] = GetBaseFromRange(5 * 12, 7 * 12);
            heights[CreatureConstants.Barghest][GenderConstants.Male] = GetBaseFromRange(5 * 12, 7 * 12);
            heights[CreatureConstants.Barghest][CreatureConstants.Barghest] = GetMultiplierFromRange(5 * 12, 7 * 12);
            heights[CreatureConstants.Barghest_Greater][GenderConstants.Female] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Barghest_Greater][GenderConstants.Male] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Barghest_Greater][CreatureConstants.Barghest_Greater] = GetMultiplierFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Basilisk][GenderConstants.Female] = "0";
            heights[CreatureConstants.Basilisk][GenderConstants.Male] = "0";
            heights[CreatureConstants.Basilisk][CreatureConstants.Basilisk] = "0";
            heights[CreatureConstants.Basilisk_Greater][GenderConstants.Female] = "0";
            heights[CreatureConstants.Basilisk_Greater][GenderConstants.Male] = "0";
            heights[CreatureConstants.Basilisk_Greater][CreatureConstants.Basilisk_Greater] = "0";
            //Source: https://www.dimensions.com/element/little-brown-bat-myotis-lucifugus
            heights[CreatureConstants.Bat][GenderConstants.Female] = GetBaseFromRange(3, 4);
            heights[CreatureConstants.Bat][GenderConstants.Male] = GetBaseFromRange(3, 4);
            heights[CreatureConstants.Bat][CreatureConstants.Bat] = GetMultiplierFromRange(3, 4);
            //Scaled up from bat, x15 based on wingspan
            heights[CreatureConstants.Bat_Dire][GenderConstants.Female] = GetBaseFromRange(45, 60);
            heights[CreatureConstants.Bat_Dire][GenderConstants.Male] = GetBaseFromRange(45, 60);
            heights[CreatureConstants.Bat_Dire][CreatureConstants.Bat_Dire] = GetMultiplierFromRange(45, 60);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            heights[CreatureConstants.Bat_Swarm][GenderConstants.Agender] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Bat_Swarm][CreatureConstants.Bat_Swarm] = GetMultiplierFromAverage(10 * 12);
            //Source: https://www.dimensions.com/element/american-black-bear shoulder height
            //Adjusting upper female -1" to match range
            heights[CreatureConstants.Bear_Black][GenderConstants.Female] = GetBaseFromRange(2 * 12 + 6, 3 * 12 + 1 - 1);
            heights[CreatureConstants.Bear_Black][GenderConstants.Male] = GetBaseFromRange(2 * 12 + 11, 3 * 12 + 5);
            heights[CreatureConstants.Bear_Black][CreatureConstants.Bear_Black] = GetMultiplierFromRange(2 * 12 + 11, 3 * 12 + 5);
            //Source: https://www.dimensions.com/element/grizzly-bear shoulder height
            //Adjusting upper female +4" to match range
            heights[CreatureConstants.Bear_Brown][GenderConstants.Female] = GetBaseFromRange(3 * 12, 3 * 12 + 8 + 4);
            heights[CreatureConstants.Bear_Brown][GenderConstants.Male] = GetBaseFromRange(3 * 12 + 6, 4 * 12 + 6);
            heights[CreatureConstants.Bear_Brown][CreatureConstants.Bear_Brown] = GetMultiplierFromRange(3 * 12 + 6, 4 * 12 + 6);
            //Source: Scaled up from grizzly, x1.5
            heights[CreatureConstants.Bear_Dire][GenderConstants.Female] = GetBaseFromRange(63, 81);
            heights[CreatureConstants.Bear_Dire][GenderConstants.Male] = GetBaseFromRange(63, 81);
            heights[CreatureConstants.Bear_Dire][CreatureConstants.Bear_Dire] = GetMultiplierFromRange(63, 81);
            //Source: https://www.dimensions.com/element/polar-bears shoulder height
            //Adjusting female max +5" to match range
            heights[CreatureConstants.Bear_Polar][GenderConstants.Female] = GetBaseFromRange(2 * 12 + 8, 3 * 12 + 11 + 5);
            heights[CreatureConstants.Bear_Polar][GenderConstants.Male] = GetBaseFromRange(3 * 12 + 7, 5 * 12 + 3);
            heights[CreatureConstants.Bear_Polar][CreatureConstants.Bear_Polar] = GetMultiplierFromRange(3 * 12 + 7, 5 * 12 + 3);
            //Source: https://forgottenrealms.fandom.com/wiki/Barbazu
            heights[CreatureConstants.BeardedDevil_Barbazu][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.BeardedDevil_Barbazu][CreatureConstants.BeardedDevil_Barbazu] = GetMultiplierFromAverage(6 * 12);
            heights[CreatureConstants.Bebilith][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Bebilith][CreatureConstants.Bebilith] = "0";
            //Source: https://www.d20srd.org/srd/monsters/giantBee.htm
            //https://www.dimensions.com/element/western-honey-bee-apis-mellifera scale up, [.12,.2]*5*12/[.39,.59] = [18,21]
            heights[CreatureConstants.Bee_Giant][GenderConstants.Male] = GetBaseFromRange(18, 21);
            heights[CreatureConstants.Bee_Giant][CreatureConstants.Bee_Giant] = GetMultiplierFromRange(18, 21);
            //Source: https://forgottenrealms.fandom.com/wiki/Behir
            heights[CreatureConstants.Behir][GenderConstants.Female] = "0";
            heights[CreatureConstants.Behir][GenderConstants.Male] = "0";
            heights[CreatureConstants.Behir][CreatureConstants.Behir] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Beholder
            heights[CreatureConstants.Beholder][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Beholder][CreatureConstants.Beholder] = GetMultiplierFromAverage(8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Gauth
            heights[CreatureConstants.Beholder_Gauth][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 6 * 12);
            heights[CreatureConstants.Beholder_Gauth][CreatureConstants.Beholder_Gauth] = GetMultiplierFromRange(4 * 12, 6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Belker
            heights[CreatureConstants.Belker][GenderConstants.Agender] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Belker][CreatureConstants.Belker] = GetMultiplierFromRange(7 * 12, 9 * 12);
            //Source: https://www.dimensions.com/element/american-bison-bison-bison, withers height
            heights[CreatureConstants.Bison][GenderConstants.Female] = GetBaseFromRange(60, 78);
            heights[CreatureConstants.Bison][GenderConstants.Male] = GetBaseFromRange(60, 78);
            heights[CreatureConstants.Bison][CreatureConstants.Bison] = GetMultiplierFromRange(60, 78);
            //Source: https://forgottenrealms.fandom.com/wiki/Black_pudding
            heights[CreatureConstants.BlackPudding][GenderConstants.Agender] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.BlackPudding][CreatureConstants.BlackPudding] = GetMultiplierFromAverage(2 * 12);
            //Elder is a size category up, so double dimensions
            heights[CreatureConstants.BlackPudding_Elder][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.BlackPudding_Elder][CreatureConstants.BlackPudding_Elder] = GetMultiplierFromAverage(4 * 12);
            //Source: https://www.d20pfsrd.com/bestiary/monster-listings/magical-beasts/blink-dog
            heights[CreatureConstants.BlinkDog][GenderConstants.Female] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.BlinkDog][GenderConstants.Male] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.BlinkDog][CreatureConstants.BlinkDog] = GetMultiplierFromAverage(3 * 12);
            //Source: https://www.dimensions.com/element/wild-boar
            heights[CreatureConstants.Boar][GenderConstants.Female] = GetBaseFromRange(2 * 12 + 6, 3 * 12);
            heights[CreatureConstants.Boar][GenderConstants.Male] = GetBaseFromRange(2 * 12 + 6, 3 * 12);
            heights[CreatureConstants.Boar][CreatureConstants.Boar] = GetMultiplierFromRange(2 * 12 + 6, 3 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_boar
            heights[CreatureConstants.Boar_Dire][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Boar_Dire][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Boar_Dire][CreatureConstants.Boar_Dire] = GetMultiplierFromAverage(6 * 12);
            //INFO: Basing off of humans
            heights[CreatureConstants.Bodak][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Bodak][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Bodak][CreatureConstants.Bodak] = "2d10";
            //Source: https://web.stanford.edu/~cbross/bombbeetle.html
            heights[CreatureConstants.BombardierBeetle_Giant][GenderConstants.Female] = "0";
            heights[CreatureConstants.BombardierBeetle_Giant][GenderConstants.Male] = "0";
            heights[CreatureConstants.BombardierBeetle_Giant][CreatureConstants.BombardierBeetle_Giant] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Osyluth
            heights[CreatureConstants.BoneDevil_Osyluth][GenderConstants.Agender] = GetBaseFromRange(9 * 12, 9 * 12 + 6);
            heights[CreatureConstants.BoneDevil_Osyluth][CreatureConstants.BoneDevil_Osyluth] = GetMultiplierFromRange(9 * 12, 9 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Bralani
            heights[CreatureConstants.Bralani][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Bralani][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Bralani][CreatureConstants.Bralani] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Bugbear
            heights[CreatureConstants.Bugbear][GenderConstants.Female] = GetBaseFromRange(6 * 12, 8 * 12);
            heights[CreatureConstants.Bugbear][GenderConstants.Male] = GetBaseFromRange(6 * 12, 8 * 12);
            heights[CreatureConstants.Bugbear][CreatureConstants.Bugbear] = GetMultiplierFromRange(6 * 12, 8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Bulette
            heights[CreatureConstants.Bulette][GenderConstants.Female] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Bulette][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Bulette][CreatureConstants.Bulette] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.dimensions.com/element/bactrian-camel
            heights[CreatureConstants.Camel_Bactrian][GenderConstants.Female] = GetBaseFromRange(62, 71);
            heights[CreatureConstants.Camel_Bactrian][GenderConstants.Male] = GetBaseFromRange(62, 71);
            heights[CreatureConstants.Camel_Bactrian][CreatureConstants.Camel_Bactrian] = GetMultiplierFromRange(62, 71);
            //Source: https://www.dimensions.com/element/dromedary-camel
            heights[CreatureConstants.Camel_Dromedary][GenderConstants.Female] = GetBaseFromRange(71, 78);
            heights[CreatureConstants.Camel_Dromedary][GenderConstants.Male] = GetBaseFromRange(71, 78);
            heights[CreatureConstants.Camel_Dromedary][CreatureConstants.Camel_Dromedary] = GetMultiplierFromRange(71, 78);
            //Source: https://forgottenrealms.fandom.com/wiki/Carrion_crawler
            heights[CreatureConstants.CarrionCrawler][GenderConstants.Female] = "0";
            heights[CreatureConstants.CarrionCrawler][GenderConstants.Male] = "0";
            heights[CreatureConstants.CarrionCrawler][CreatureConstants.CarrionCrawler] = "0";
            //Source: https://www.dimensions.com/element/american-shorthair-cat
            heights[CreatureConstants.Cat][GenderConstants.Female] = GetBaseFromRange(8, 10);
            heights[CreatureConstants.Cat][GenderConstants.Male] = GetBaseFromRange(8, 10);
            heights[CreatureConstants.Cat][CreatureConstants.Cat] = GetMultiplierFromRange(8, 10);
            //Source: https://forgottenrealms.fandom.com/wiki/Centaur
            heights[CreatureConstants.Centaur][GenderConstants.Female] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Centaur][GenderConstants.Male] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Centaur][CreatureConstants.Centaur] = GetMultiplierFromRange(7 * 12, 9 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Tiny
            heights[CreatureConstants.Centipede_Monstrous_Tiny][GenderConstants.Female] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Centipede_Monstrous_Tiny][GenderConstants.Male] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Centipede_Monstrous_Tiny][CreatureConstants.Centipede_Monstrous_Tiny] = GetMultiplierFromRange(1, 2);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Small
            heights[CreatureConstants.Centipede_Monstrous_Small][GenderConstants.Female] = GetBaseFromAverage(3);
            heights[CreatureConstants.Centipede_Monstrous_Small][GenderConstants.Male] = GetBaseFromAverage(3);
            heights[CreatureConstants.Centipede_Monstrous_Small][CreatureConstants.Centipede_Monstrous_Small] = GetMultiplierFromAverage(3);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Medium
            heights[CreatureConstants.Centipede_Monstrous_Medium][GenderConstants.Female] = GetBaseFromAverage(6);
            heights[CreatureConstants.Centipede_Monstrous_Medium][GenderConstants.Male] = GetBaseFromAverage(6);
            heights[CreatureConstants.Centipede_Monstrous_Medium][CreatureConstants.Centipede_Monstrous_Medium] = GetMultiplierFromAverage(6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Large
            heights[CreatureConstants.Centipede_Monstrous_Large][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.Centipede_Monstrous_Large][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.Centipede_Monstrous_Large][CreatureConstants.Centipede_Monstrous_Large] = GetMultiplierFromAverage(12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Huge
            heights[CreatureConstants.Centipede_Monstrous_Huge][GenderConstants.Female] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Centipede_Monstrous_Huge][GenderConstants.Male] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Centipede_Monstrous_Huge][CreatureConstants.Centipede_Monstrous_Huge] = GetMultiplierFromAverage(2 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Gargantuan
            heights[CreatureConstants.Centipede_Monstrous_Gargantuan][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Centipede_Monstrous_Gargantuan][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Centipede_Monstrous_Gargantuan][CreatureConstants.Centipede_Monstrous_Gargantuan] = GetMultiplierFromAverage(4 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Colossal
            heights[CreatureConstants.Centipede_Monstrous_Colossal][GenderConstants.Female] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Centipede_Monstrous_Colossal][GenderConstants.Male] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Centipede_Monstrous_Colossal][CreatureConstants.Centipede_Monstrous_Colossal] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            heights[CreatureConstants.Centipede_Swarm][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Centipede_Swarm][CreatureConstants.Centipede_Swarm] = "0";
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#chainDevilKyton
            heights[CreatureConstants.ChainDevil_Kyton][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.ChainDevil_Kyton][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.ChainDevil_Kyton][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.ChainDevil_Kyton][CreatureConstants.ChainDevil_Kyton] = GetMultiplierFromAverage(6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Chaos_beast
            heights[CreatureConstants.ChaosBeast][GenderConstants.Agender] = GetBaseFromRange(5 * 12, 7 * 12);
            heights[CreatureConstants.ChaosBeast][CreatureConstants.ChaosBeast] = GetMultiplierFromRange(5 * 12, 7 * 12);
            //Source: https://www.dimensions.com/element/cheetahs
            heights[CreatureConstants.Cheetah][GenderConstants.Female] = GetBaseFromRange(28, 35);
            heights[CreatureConstants.Cheetah][GenderConstants.Male] = GetBaseFromRange(28, 35);
            heights[CreatureConstants.Cheetah][CreatureConstants.Cheetah] = GetMultiplierFromRange(28, 35);
            //Source: https://forgottenrealms.fandom.com/wiki/Chimera
            heights[CreatureConstants.Chimera_Black][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Black][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Black][CreatureConstants.Chimera_Black] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Blue][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Blue][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Blue][CreatureConstants.Chimera_Blue] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Green][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Green][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Green][CreatureConstants.Chimera_Green] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Red][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Red][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_Red][CreatureConstants.Chimera_Red] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_White][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_White][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Chimera_White][CreatureConstants.Chimera_White] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Choker
            heights[CreatureConstants.Choker][GenderConstants.Female] = GetBaseFromAverage(3 * 12 + 6);
            heights[CreatureConstants.Choker][GenderConstants.Male] = GetBaseFromAverage(3 * 12 + 6);
            heights[CreatureConstants.Choker][CreatureConstants.Choker] = GetMultiplierFromAverage(3 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Chuul
            heights[CreatureConstants.Chuul][GenderConstants.Female] = "0";
            heights[CreatureConstants.Chuul][GenderConstants.Male] = "0";
            heights[CreatureConstants.Chuul][CreatureConstants.Chuul] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Cloaker and https://www.mojobob.com/roleplay/monstrousmanual/c/cloaker.html
            heights[CreatureConstants.Cloaker][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Cloaker][CreatureConstants.Cloaker] = GetMultiplierFromAverage(8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Cockatrice
            heights[CreatureConstants.Cockatrice][GenderConstants.Female] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.Cockatrice][GenderConstants.Male] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.Cockatrice][CreatureConstants.Cockatrice] = GetMultiplierFromAverage(3 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Couatl
            heights[CreatureConstants.Couatl][GenderConstants.Female] = "0";
            heights[CreatureConstants.Couatl][GenderConstants.Male] = "0";
            heights[CreatureConstants.Couatl][CreatureConstants.Couatl] = "0";
            //Source: https://www.mojobob.com/roleplay/monstrousmanual/s/sphinx.html
            heights[CreatureConstants.Criosphinx][GenderConstants.Male] = GetBaseFromAverage(7 * 12 + 6);
            heights[CreatureConstants.Criosphinx][CreatureConstants.Criosphinx] = GetMultiplierFromAverage(7 * 12 + 6);
            //Source: https://www.dimensions.com/element/nile-crocodile-crocodylus-niloticus
            heights[CreatureConstants.Crocodile][GenderConstants.Female] = GetBaseFromRange(12, 19);
            heights[CreatureConstants.Crocodile][GenderConstants.Male] = GetBaseFromRange(12, 19);
            heights[CreatureConstants.Crocodile][CreatureConstants.Crocodile] = GetMultiplierFromRange(12, 19);
            //Source: https://www.dimensions.com/element/saltwater-crocodile-crocodylus-porosus
            heights[CreatureConstants.Crocodile_Giant][GenderConstants.Female] = GetBaseFromRange(10, 30);
            heights[CreatureConstants.Crocodile_Giant][GenderConstants.Male] = GetBaseFromRange(10, 30);
            heights[CreatureConstants.Crocodile_Giant][CreatureConstants.Crocodile_Giant] = GetMultiplierFromRange(10, 30);
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra half of length for height
            heights[CreatureConstants.Cryohydra_5Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_5Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_5Heads][CreatureConstants.Cryohydra_5Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_6Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_6Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_6Heads][CreatureConstants.Cryohydra_6Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_7Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_7Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_7Heads][CreatureConstants.Cryohydra_7Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_8Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_8Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_8Heads][CreatureConstants.Cryohydra_8Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_9Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_9Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_9Heads][CreatureConstants.Cryohydra_9Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_10Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_10Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_10Heads][CreatureConstants.Cryohydra_10Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_11Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_11Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_11Heads][CreatureConstants.Cryohydra_11Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_12Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_12Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Cryohydra_12Heads][CreatureConstants.Cryohydra_12Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            //Source: https://forgottenrealms.fandom.com/wiki/Darkmantle
            heights[CreatureConstants.Darkmantle][GenderConstants.Hermaphrodite] = "0";
            heights[CreatureConstants.Darkmantle][CreatureConstants.Darkmantle] = "0";
            //Source: https://www.dimensions.com/element/deinonychus-deinonychus-antirrhopus
            heights[CreatureConstants.Deinonychus][GenderConstants.Female] = GetBaseFromRange(34, 57);
            heights[CreatureConstants.Deinonychus][GenderConstants.Male] = GetBaseFromRange(34, 57);
            heights[CreatureConstants.Deinonychus][CreatureConstants.Deinonychus] = GetMultiplierFromRange(34, 57);
            //Source: https://dungeonsdragons.fandom.com/wiki/Delver
            heights[CreatureConstants.Delver][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Delver][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Delver][CreatureConstants.Delver] = GetMultiplierFromAverage(12 * 12);
            //Source: https://monster.fandom.com/wiki/Derro
            heights[CreatureConstants.Derro][GenderConstants.Female] = GetBaseFromRange(3 * 12, 4 * 12);
            heights[CreatureConstants.Derro][GenderConstants.Male] = GetBaseFromRange(3 * 12, 4 * 12);
            heights[CreatureConstants.Derro][CreatureConstants.Derro] = GetMultiplierFromRange(3 * 12, 4 * 12);
            heights[CreatureConstants.Derro_Sane][GenderConstants.Female] = GetBaseFromRange(3 * 12, 4 * 12);
            heights[CreatureConstants.Derro_Sane][GenderConstants.Male] = GetBaseFromRange(3 * 12, 4 * 12);
            heights[CreatureConstants.Derro_Sane][CreatureConstants.Derro_Sane] = GetMultiplierFromRange(3 * 12, 4 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Destrachan
            heights[CreatureConstants.Destrachan][GenderConstants.Female] = "0";
            heights[CreatureConstants.Destrachan][GenderConstants.Male] = "0";
            heights[CreatureConstants.Destrachan][CreatureConstants.Destrachan] = "0";
            //Source: https://www.d20srd.org/srd/monsters/devourer.htm
            heights[CreatureConstants.Devourer][GenderConstants.Agender] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Devourer][CreatureConstants.Devourer] = GetMultiplierFromAverage(9 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Digester
            heights[CreatureConstants.Digester][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Digester][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Digester][CreatureConstants.Digester] = GetMultiplierFromAverage(5 * 12);
            //Scaled down from Pack Lord, x2 based on length
            heights[CreatureConstants.DisplacerBeast][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.DisplacerBeast][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.DisplacerBeast][CreatureConstants.DisplacerBeast] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Displacer_beast
            heights[CreatureConstants.DisplacerBeast_PackLord][GenderConstants.Female] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.DisplacerBeast_PackLord][GenderConstants.Male] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.DisplacerBeast_PackLord][CreatureConstants.DisplacerBeast_PackLord] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Djinni
            heights[CreatureConstants.Djinni][GenderConstants.Agender] = GetBaseFromAverage(10 * 12 + 6);
            heights[CreatureConstants.Djinni][GenderConstants.Female] = GetBaseFromAverage(10 * 12 + 6);
            heights[CreatureConstants.Djinni][GenderConstants.Male] = GetBaseFromAverage(10 * 12 + 6);
            heights[CreatureConstants.Djinni][CreatureConstants.Djinni] = GetMultiplierFromAverage(10 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Noble_djinni
            heights[CreatureConstants.Djinni_Noble][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Djinni_Noble][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Djinni_Noble][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Djinni_Noble][CreatureConstants.Djinni_Noble] = GetMultiplierFromAverage(12 * 12);
            //Source: https://www.dimensions.com/search?query=dog average of various dogs in the 20-50 pound weight range, including coyote
            heights[CreatureConstants.Dog][GenderConstants.Female] = GetBaseFromRange(16, 25);
            heights[CreatureConstants.Dog][GenderConstants.Male] = GetBaseFromRange(16, 25);
            heights[CreatureConstants.Dog][CreatureConstants.Dog] = GetMultiplierFromRange(16, 25);
            //Source: https://www.dimensions.com/element/saint-bernard-dog M:28-30,F:26-28
            //https://www.dimensions.com/element/siberian-husky 20-24
            //https://www.dimensions.com/element/dogs-collie M:22-24,F:20-22
            heights[CreatureConstants.Dog_Riding][GenderConstants.Female] = GetBaseFromRange(20, 28);
            heights[CreatureConstants.Dog_Riding][GenderConstants.Male] = GetBaseFromRange(22, 30);
            heights[CreatureConstants.Dog_Riding][CreatureConstants.Dog_Riding] = GetMultiplierFromRange(22, 30);
            //Source: https://www.dimensions.com/element/donkey-equus-africanus-asinus
            heights[CreatureConstants.Donkey][GenderConstants.Female] = GetBaseFromRange(36, 48);
            heights[CreatureConstants.Donkey][GenderConstants.Male] = GetBaseFromRange(36, 48);
            heights[CreatureConstants.Donkey][CreatureConstants.Donkey] = GetMultiplierFromRange(36, 48);
            //Source: https://forgottenrealms.fandom.com/wiki/Doppelganger
            heights[CreatureConstants.Doppelganger][GenderConstants.Agender] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Doppelganger][CreatureConstants.Doppelganger] = GetMultiplierFromAverage(5 * 12);
            //Source: Draconomicon
            heights[CreatureConstants.Dragon_Black_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_Black_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_Black_Wyrmling][CreatureConstants.Dragon_Black_Wyrmling] = GetMultiplierFromAverage(12);
            heights[CreatureConstants.Dragon_Black_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Black_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Black_VeryYoung][CreatureConstants.Dragon_Black_VeryYoung] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Dragon_Black_Young][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Black_Young][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Black_Young][CreatureConstants.Dragon_Black_Young] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Black_Juvenile][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Black_Juvenile][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Black_Juvenile][CreatureConstants.Dragon_Black_Juvenile] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Black_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Black_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Black_YoungAdult][CreatureConstants.Dragon_Black_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Black_Adult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Black_Adult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Black_Adult][CreatureConstants.Dragon_Black_Adult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Black_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_MatureAdult][CreatureConstants.Dragon_Black_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Old][CreatureConstants.Dragon_Black_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_VeryOld][CreatureConstants.Dragon_Black_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Ancient][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Ancient][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Ancient][CreatureConstants.Dragon_Black_Ancient] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Black_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Black_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Black_Wyrm][CreatureConstants.Dragon_Black_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Black_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Black_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Black_GreatWyrm][CreatureConstants.Dragon_Black_GreatWyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Blue_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Blue_Wyrmling][CreatureConstants.Dragon_Blue_Wyrmling] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Dragon_Blue_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Blue_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Blue_VeryYoung][CreatureConstants.Dragon_Blue_VeryYoung] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Blue_Young][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Blue_Young][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Blue_Young][CreatureConstants.Dragon_Blue_Young] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Blue_Juvenile][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Blue_Juvenile][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Blue_Juvenile][CreatureConstants.Dragon_Blue_Juvenile] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Blue_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Blue_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Blue_YoungAdult][CreatureConstants.Dragon_Blue_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Blue_Adult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_Adult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_Adult][CreatureConstants.Dragon_Blue_Adult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_MatureAdult][CreatureConstants.Dragon_Blue_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_Old][CreatureConstants.Dragon_Blue_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_VeryOld][CreatureConstants.Dragon_Blue_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Blue_Ancient][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_Ancient][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_Ancient][CreatureConstants.Dragon_Blue_Ancient] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_Wyrm][CreatureConstants.Dragon_Blue_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Blue_GreatWyrm][CreatureConstants.Dragon_Blue_GreatWyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Brass_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_Brass_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_Brass_Wyrmling][CreatureConstants.Dragon_Brass_Wyrmling] = GetMultiplierFromAverage(12);
            heights[CreatureConstants.Dragon_Brass_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Brass_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Brass_VeryYoung][CreatureConstants.Dragon_Brass_VeryYoung] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Dragon_Brass_Young][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Brass_Young][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Brass_Young][CreatureConstants.Dragon_Brass_Young] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Brass_Juvenile][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Brass_Juvenile][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Brass_Juvenile][CreatureConstants.Dragon_Brass_Juvenile] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Brass_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Brass_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Brass_YoungAdult][CreatureConstants.Dragon_Brass_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Brass_Adult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Brass_Adult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Brass_Adult][CreatureConstants.Dragon_Brass_Adult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Brass_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_MatureAdult][CreatureConstants.Dragon_Brass_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Old][CreatureConstants.Dragon_Brass_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_VeryOld][CreatureConstants.Dragon_Brass_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Ancient][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Ancient][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Ancient][CreatureConstants.Dragon_Brass_Ancient] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Brass_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Brass_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Brass_Wyrm][CreatureConstants.Dragon_Brass_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Brass_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Brass_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Brass_GreatWyrm][CreatureConstants.Dragon_Brass_GreatWyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Bronze_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Bronze_Wyrmling][CreatureConstants.Dragon_Bronze_Wyrmling] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Dragon_Bronze_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Bronze_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Bronze_VeryYoung][CreatureConstants.Dragon_Bronze_VeryYoung] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Bronze_Young][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Bronze_Young][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Bronze_Young][CreatureConstants.Dragon_Bronze_Young] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Bronze_Juvenile][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Bronze_Juvenile][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Bronze_Juvenile][CreatureConstants.Dragon_Bronze_Juvenile] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Bronze_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Bronze_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Bronze_YoungAdult][CreatureConstants.Dragon_Bronze_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Bronze_Adult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_Adult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_Adult][CreatureConstants.Dragon_Bronze_Adult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_MatureAdult][CreatureConstants.Dragon_Bronze_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_Old][CreatureConstants.Dragon_Bronze_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_VeryOld][CreatureConstants.Dragon_Bronze_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Bronze_Ancient][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_Ancient][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_Ancient][CreatureConstants.Dragon_Bronze_Ancient] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_Wyrm][CreatureConstants.Dragon_Bronze_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Bronze_GreatWyrm][CreatureConstants.Dragon_Bronze_GreatWyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Copper_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_Copper_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_Copper_Wyrmling][CreatureConstants.Dragon_Copper_Wyrmling] = GetMultiplierFromAverage(12);
            heights[CreatureConstants.Dragon_Copper_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Copper_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_Copper_VeryYoung][CreatureConstants.Dragon_Copper_VeryYoung] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Dragon_Copper_Young][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Copper_Young][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Copper_Young][CreatureConstants.Dragon_Copper_Young] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Copper_Juvenile][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Copper_Juvenile][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Copper_Juvenile][CreatureConstants.Dragon_Copper_Juvenile] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Copper_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Copper_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Copper_YoungAdult][CreatureConstants.Dragon_Copper_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Copper_Adult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Copper_Adult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Copper_Adult][CreatureConstants.Dragon_Copper_Adult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Copper_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_MatureAdult][CreatureConstants.Dragon_Copper_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Old][CreatureConstants.Dragon_Copper_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_VeryOld][CreatureConstants.Dragon_Copper_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Ancient][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Ancient][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Ancient][CreatureConstants.Dragon_Copper_Ancient] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Copper_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Copper_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Copper_Wyrm][CreatureConstants.Dragon_Copper_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Copper_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Copper_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Copper_GreatWyrm][CreatureConstants.Dragon_Copper_GreatWyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Gold_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Gold_Wyrmling][CreatureConstants.Dragon_Gold_Wyrmling] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Gold_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_VeryYoung][CreatureConstants.Dragon_Gold_VeryYoung] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_Young][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_Young][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_Young][CreatureConstants.Dragon_Gold_Young] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_Juvenile][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_Juvenile][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_Juvenile][CreatureConstants.Dragon_Gold_Juvenile] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Gold_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_YoungAdult][CreatureConstants.Dragon_Gold_YoungAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_Adult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_Adult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_Adult][CreatureConstants.Dragon_Gold_Adult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_MatureAdult][CreatureConstants.Dragon_Gold_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Gold_Old][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Old][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Old][CreatureConstants.Dragon_Gold_Old] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_VeryOld][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_VeryOld][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_VeryOld][CreatureConstants.Dragon_Gold_VeryOld] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Ancient][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Ancient][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Ancient][CreatureConstants.Dragon_Gold_Ancient] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Gold_Wyrm][GenderConstants.Female] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Gold_Wyrm][GenderConstants.Male] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Gold_Wyrm][CreatureConstants.Dragon_Gold_Wyrm] = GetMultiplierFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Gold_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Gold_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Gold_GreatWyrm][CreatureConstants.Dragon_Gold_GreatWyrm] = GetMultiplierFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Green_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(30);
            heights[CreatureConstants.Dragon_Green_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(30);
            heights[CreatureConstants.Dragon_Green_Wyrmling][CreatureConstants.Dragon_Green_Wyrmling] = GetMultiplierFromAverage(30);
            heights[CreatureConstants.Dragon_Green_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dragon_Green_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dragon_Green_VeryYoung][CreatureConstants.Dragon_Green_VeryYoung] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Dragon_Green_Young][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dragon_Green_Young][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dragon_Green_Young][CreatureConstants.Dragon_Green_Young] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Dragon_Green_Juvenile][GenderConstants.Female] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Dragon_Green_Juvenile][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Dragon_Green_Juvenile][CreatureConstants.Dragon_Green_Juvenile] = GetMultiplierFromAverage(9 * 12);
            heights[CreatureConstants.Dragon_Green_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Dragon_Green_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Dragon_Green_YoungAdult][CreatureConstants.Dragon_Green_YoungAdult] = GetMultiplierFromAverage(9 * 12);
            heights[CreatureConstants.Dragon_Green_Adult][GenderConstants.Female] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_Adult][GenderConstants.Male] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_Adult][CreatureConstants.Dragon_Green_Adult] = GetMultiplierFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_MatureAdult][CreatureConstants.Dragon_Green_MatureAdult] = GetMultiplierFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_Old][GenderConstants.Female] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_Old][GenderConstants.Male] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_Old][CreatureConstants.Dragon_Green_Old] = GetMultiplierFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_VeryOld][GenderConstants.Female] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_VeryOld][GenderConstants.Male] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_VeryOld][CreatureConstants.Dragon_Green_VeryOld] = GetMultiplierFromAverage(15 * 12);
            heights[CreatureConstants.Dragon_Green_Ancient][GenderConstants.Female] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_Ancient][GenderConstants.Male] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_Ancient][CreatureConstants.Dragon_Green_Ancient] = GetMultiplierFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_Wyrm][GenderConstants.Female] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_Wyrm][GenderConstants.Male] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_Wyrm][CreatureConstants.Dragon_Green_Wyrm] = GetMultiplierFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Green_GreatWyrm][CreatureConstants.Dragon_Green_GreatWyrm] = GetMultiplierFromAverage(20 * 12);
            heights[CreatureConstants.Dragon_Red_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Red_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Red_Wyrmling][CreatureConstants.Dragon_Red_Wyrmling] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Red_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_VeryYoung][CreatureConstants.Dragon_Red_VeryYoung] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_Young][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_Young][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_Young][CreatureConstants.Dragon_Red_Young] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_Juvenile][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_Juvenile][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_Juvenile][CreatureConstants.Dragon_Red_Juvenile] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Red_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_YoungAdult][CreatureConstants.Dragon_Red_YoungAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_Adult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_Adult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_Adult][CreatureConstants.Dragon_Red_Adult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_MatureAdult][CreatureConstants.Dragon_Red_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Red_Old][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Old][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Old][CreatureConstants.Dragon_Red_Old] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_VeryOld][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_VeryOld][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_VeryOld][CreatureConstants.Dragon_Red_VeryOld] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Ancient][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Ancient][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Ancient][CreatureConstants.Dragon_Red_Ancient] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_Wyrm][CreatureConstants.Dragon_Red_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Red_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Red_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Red_GreatWyrm][CreatureConstants.Dragon_Red_GreatWyrm] = GetMultiplierFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Silver_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Dragon_Silver_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Dragon_Silver_Wyrmling][CreatureConstants.Dragon_Silver_Wyrmling] = GetMultiplierFromAverage(2 * 12);
            heights[CreatureConstants.Dragon_Silver_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Silver_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Silver_VeryYoung][CreatureConstants.Dragon_Silver_VeryYoung] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_Silver_Young][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_Young][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_Young][CreatureConstants.Dragon_Silver_Young] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_Juvenile][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_Juvenile][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_Juvenile][CreatureConstants.Dragon_Silver_Juvenile] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_YoungAdult][CreatureConstants.Dragon_Silver_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_Silver_Adult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_Adult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_Adult][CreatureConstants.Dragon_Silver_Adult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_MatureAdult][CreatureConstants.Dragon_Silver_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_Old][CreatureConstants.Dragon_Silver_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_VeryOld][CreatureConstants.Dragon_Silver_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_Silver_Ancient][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Silver_Ancient][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Silver_Ancient][CreatureConstants.Dragon_Silver_Ancient] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Silver_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Silver_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Silver_Wyrm][CreatureConstants.Dragon_Silver_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_Silver_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Silver_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_Silver_GreatWyrm][CreatureConstants.Dragon_Silver_GreatWyrm] = GetMultiplierFromAverage(22 * 12);
            heights[CreatureConstants.Dragon_White_Wyrmling][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_White_Wyrmling][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.Dragon_White_Wyrmling][CreatureConstants.Dragon_White_Wyrmling] = GetMultiplierFromAverage(12);
            heights[CreatureConstants.Dragon_White_VeryYoung][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_White_VeryYoung][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.Dragon_White_VeryYoung][CreatureConstants.Dragon_White_VeryYoung] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Dragon_White_Young][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_White_Young][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_White_Young][CreatureConstants.Dragon_White_Young] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_White_Juvenile][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_White_Juvenile][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_White_Juvenile][CreatureConstants.Dragon_White_Juvenile] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Dragon_White_YoungAdult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_White_YoungAdult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_White_YoungAdult][CreatureConstants.Dragon_White_YoungAdult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_White_Adult][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_White_Adult][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_White_Adult][CreatureConstants.Dragon_White_Adult] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Dragon_White_MatureAdult][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_MatureAdult][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_MatureAdult][CreatureConstants.Dragon_White_MatureAdult] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Old][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Old][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Old][CreatureConstants.Dragon_White_Old] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_VeryOld][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_VeryOld][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_VeryOld][CreatureConstants.Dragon_White_VeryOld] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Ancient][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Ancient][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Ancient][CreatureConstants.Dragon_White_Ancient] = GetMultiplierFromAverage(12 * 12);
            heights[CreatureConstants.Dragon_White_Wyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_White_Wyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_White_Wyrm][CreatureConstants.Dragon_White_Wyrm] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_White_GreatWyrm][GenderConstants.Female] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_White_GreatWyrm][GenderConstants.Male] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Dragon_White_GreatWyrm][CreatureConstants.Dragon_White_GreatWyrm] = GetMultiplierFromAverage(16 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Dragon_turtle
            heights[CreatureConstants.DragonTurtle][GenderConstants.Female] = "0";
            heights[CreatureConstants.DragonTurtle][GenderConstants.Male] = "0";
            heights[CreatureConstants.DragonTurtle][CreatureConstants.DragonTurtle] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Dragonne
            heights[CreatureConstants.Dragonne][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dragonne][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dragonne][CreatureConstants.Dragonne] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Dretch
            heights[CreatureConstants.Dretch][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dretch][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dretch][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Dretch][CreatureConstants.Dretch] = GetMultiplierFromAverage(4 * 12);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/drider-species
            //https://www.5esrd.com/database/race/drider/ (for height)
            heights[CreatureConstants.Drider][GenderConstants.Agender] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Drider][CreatureConstants.Drider] = GetMultiplierFromAverage(7 * 12);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/dryad-species
            heights[CreatureConstants.Dryad][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Dryad][CreatureConstants.Dryad] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            heights[CreatureConstants.Dwarf_Deep][GenderConstants.Female] = "3*12+7";
            heights[CreatureConstants.Dwarf_Deep][GenderConstants.Male] = "3*12+9";
            heights[CreatureConstants.Dwarf_Deep][CreatureConstants.Dwarf_Deep] = "2d4";
            heights[CreatureConstants.Dwarf_Duergar][GenderConstants.Female] = "3*12+7";
            heights[CreatureConstants.Dwarf_Duergar][GenderConstants.Male] = "3*12+9";
            heights[CreatureConstants.Dwarf_Duergar][CreatureConstants.Dwarf_Duergar] = "2d4";
            heights[CreatureConstants.Dwarf_Hill][GenderConstants.Female] = "3*12+7";
            heights[CreatureConstants.Dwarf_Hill][GenderConstants.Male] = "3*12+9";
            heights[CreatureConstants.Dwarf_Hill][CreatureConstants.Dwarf_Hill] = "2d4";
            //https://www.d20srd.org/srd/monsters/dwarf.htm On average 6" taller than Hill dwarves
            heights[CreatureConstants.Dwarf_Mountain][GenderConstants.Female] = "3*12+7+6";
            heights[CreatureConstants.Dwarf_Mountain][GenderConstants.Male] = "3*12+9+6";
            heights[CreatureConstants.Dwarf_Mountain][CreatureConstants.Dwarf_Mountain] = "2d4";
            //Source: https://www.dimensions.com/element/bald-eagle-haliaeetus-leucocephalus
            heights[CreatureConstants.Eagle][GenderConstants.Female] = GetBaseFromRange(17, 24);
            heights[CreatureConstants.Eagle][GenderConstants.Male] = GetBaseFromRange(17, 24);
            heights[CreatureConstants.Eagle][CreatureConstants.Eagle] = GetMultiplierFromRange(17, 24);
            //Source: https://www.d20srd.org/srd/monsters/eagleGiant.htm
            heights[CreatureConstants.Eagle_Giant][GenderConstants.Female] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Eagle_Giant][GenderConstants.Male] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Eagle_Giant][CreatureConstants.Eagle_Giant] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Efreeti
            heights[CreatureConstants.Efreeti][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Efreeti][GenderConstants.Female] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Efreeti][GenderConstants.Male] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Efreeti][CreatureConstants.Efreeti] = GetMultiplierFromAverage(12 * 12);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Elasmosaurus
            heights[CreatureConstants.Elasmosaurus][GenderConstants.Female] = GetBaseFromAverage(40);
            heights[CreatureConstants.Elasmosaurus][GenderConstants.Male] = GetBaseFromAverage(40);
            heights[CreatureConstants.Elasmosaurus][CreatureConstants.Elasmosaurus] = GetMultiplierFromAverage(40);
            //Source: https://www.d20srd.org/srd/monsters/elemental.htm
            heights[CreatureConstants.Elemental_Air_Small][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Air_Small][CreatureConstants.Elemental_Air_Small] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Air_Medium][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Air_Medium][CreatureConstants.Elemental_Air_Medium] = GetMultiplierFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Air_Large][GenderConstants.Agender] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Air_Large][CreatureConstants.Elemental_Air_Large] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Air_Huge][GenderConstants.Agender] = GetBaseFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Air_Huge][CreatureConstants.Elemental_Air_Huge] = GetMultiplierFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Air_Greater][GenderConstants.Agender] = GetBaseFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Air_Greater][CreatureConstants.Elemental_Air_Greater] = GetMultiplierFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Air_Elder][GenderConstants.Agender] = GetBaseFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Air_Elder][CreatureConstants.Elemental_Air_Elder] = GetMultiplierFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Earth_Small][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Earth_Small][CreatureConstants.Elemental_Earth_Small] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Earth_Medium][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Earth_Medium][CreatureConstants.Elemental_Earth_Medium] = GetMultiplierFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Earth_Large][GenderConstants.Agender] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Earth_Large][CreatureConstants.Elemental_Earth_Large] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Earth_Huge][GenderConstants.Agender] = GetBaseFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Earth_Huge][CreatureConstants.Elemental_Earth_Huge] = GetMultiplierFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Earth_Greater][GenderConstants.Agender] = GetBaseFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Earth_Greater][CreatureConstants.Elemental_Earth_Greater] = GetMultiplierFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Earth_Elder][GenderConstants.Agender] = GetBaseFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Earth_Elder][CreatureConstants.Elemental_Earth_Elder] = GetMultiplierFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Fire_Small][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Fire_Small][CreatureConstants.Elemental_Fire_Small] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Fire_Medium][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Fire_Medium][CreatureConstants.Elemental_Fire_Medium] = GetMultiplierFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Fire_Large][GenderConstants.Agender] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Fire_Large][CreatureConstants.Elemental_Fire_Large] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Fire_Huge][GenderConstants.Agender] = GetBaseFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Fire_Huge][CreatureConstants.Elemental_Fire_Huge] = GetMultiplierFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Fire_Greater][GenderConstants.Agender] = GetBaseFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Fire_Greater][CreatureConstants.Elemental_Fire_Greater] = GetMultiplierFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Fire_Elder][GenderConstants.Agender] = GetBaseFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Fire_Elder][CreatureConstants.Elemental_Fire_Elder] = GetMultiplierFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Water_Small][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Water_Small][CreatureConstants.Elemental_Water_Small] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Elemental_Water_Medium][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Water_Medium][CreatureConstants.Elemental_Water_Medium] = GetMultiplierFromAverage(8 * 12);
            heights[CreatureConstants.Elemental_Water_Large][GenderConstants.Agender] = GetBaseFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Water_Large][CreatureConstants.Elemental_Water_Large] = GetMultiplierFromAverage(16 * 12);
            heights[CreatureConstants.Elemental_Water_Huge][GenderConstants.Agender] = GetBaseFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Water_Huge][CreatureConstants.Elemental_Water_Huge] = GetMultiplierFromAverage(32 * 12);
            heights[CreatureConstants.Elemental_Water_Greater][GenderConstants.Agender] = GetBaseFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Water_Greater][CreatureConstants.Elemental_Water_Greater] = GetMultiplierFromAverage(36 * 12);
            heights[CreatureConstants.Elemental_Water_Elder][GenderConstants.Agender] = GetBaseFromAverage(40 * 12);
            heights[CreatureConstants.Elemental_Water_Elder][CreatureConstants.Elemental_Water_Elder] = GetMultiplierFromAverage(40 * 12);
            //Source: https://www.dimensions.com/element/african-bush-elephant-loxodonta-africana
            heights[CreatureConstants.Elephant][GenderConstants.Female] = GetBaseFromRange(8 * 12 + 6, 13 * 12);
            heights[CreatureConstants.Elephant][GenderConstants.Male] = GetBaseFromRange(8 * 12 + 6, 13 * 12);
            heights[CreatureConstants.Elephant][CreatureConstants.Elephant] = GetMultiplierFromRange(8 * 12 + 6, 13 * 12);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            //https://forgottenrealms.fandom.com/wiki/Aquatic_elf - larger and heavier, so 110%
            heights[CreatureConstants.Elf_Aquatic][GenderConstants.Female] = "(4*12+5)*1.1";
            heights[CreatureConstants.Elf_Aquatic][GenderConstants.Male] = "(4*12+5)*1.1";
            heights[CreatureConstants.Elf_Aquatic][CreatureConstants.Elf_Aquatic] = "2d6";
            //Source: https://forgottenrealms.fandom.com/wiki/Drow
            heights[CreatureConstants.Elf_Drow][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 5 * 12 + 5);
            heights[CreatureConstants.Elf_Drow][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 5 * 12 + 5);
            heights[CreatureConstants.Elf_Drow][CreatureConstants.Elf_Drow] = GetMultiplierFromRange(4 * 12 + 7, 5 * 12 + 5);
            //Source: https://www.d20srd.org/srd/monsters/elf.htm taller, so 110%
            heights[CreatureConstants.Elf_Gray][GenderConstants.Female] = "(4*12+5)*1.1";
            heights[CreatureConstants.Elf_Gray][GenderConstants.Male] = "(4*12+5)*1.1";
            heights[CreatureConstants.Elf_Gray][CreatureConstants.Elf_Gray] = "2d6";
            heights[CreatureConstants.Elf_Half][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Elf_Half][GenderConstants.Male] = "4*12+7";
            heights[CreatureConstants.Elf_Half][CreatureConstants.Elf_Half] = "2d8";
            heights[CreatureConstants.Elf_High][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Elf_High][GenderConstants.Male] = "4*12+5";
            heights[CreatureConstants.Elf_High][CreatureConstants.Elf_High] = "2d6";
            //https://forgottenrealms.fandom.com/wiki/Wild_elf Males 5 inches taller
            heights[CreatureConstants.Elf_Wild][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Elf_Wild][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Elf_Wild][CreatureConstants.Elf_Wild] = "2d6";
            heights[CreatureConstants.Elf_Wood][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Elf_Wood][GenderConstants.Male] = "4*12+5";
            heights[CreatureConstants.Elf_Wood][CreatureConstants.Elf_Wood] = "2d6";
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#erinyes
            heights[CreatureConstants.Erinyes][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Erinyes][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Erinyes][CreatureConstants.Erinyes] = GetMultiplierFromAverage(6 * 12);
            //Source: https://aminoapps.com/c/officialdd/page/item/ethereal-filcher/5B63_a5vi5Ia7NM1djj0V1QYVoxpXweGW1z
            heights[CreatureConstants.EtherealFilcher][GenderConstants.Agender] = GetBaseFromAverage(4 * 12 + 6);
            heights[CreatureConstants.EtherealFilcher][CreatureConstants.EtherealFilcher] = GetMultiplierFromAverage(4 * 12 + 6);
            //Source: https://www.d20srd.org/srd/monsters/etherealMarauder.htm
            heights[CreatureConstants.EtherealMarauder][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.EtherealMarauder][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.EtherealMarauder][CreatureConstants.EtherealMarauder] = GetMultiplierFromAverage(4 * 12);
            //Source: https://syrikdarkenedskies.obsidianportal.com/wikis/ettercap-race
            heights[CreatureConstants.Ettercap][GenderConstants.Female] = "5*12+7";
            heights[CreatureConstants.Ettercap][GenderConstants.Male] = "5*12+2";
            heights[CreatureConstants.Ettercap][CreatureConstants.Ettercap] = "2d10";
            //Source: https://forgottenrealms.fandom.com/wiki/Ettin
            heights[CreatureConstants.Ettin][GenderConstants.Female] = GetBaseFromRange(12 * 12 + 4, 13 * 12 + 2);
            heights[CreatureConstants.Ettin][GenderConstants.Male] = GetBaseFromRange(13 * 12, 13 * 12 + 10);
            heights[CreatureConstants.Ettin][CreatureConstants.Ettin] = GetMultiplierFromRange(13 * 12, 13 * 12 + 10);
            heights[CreatureConstants.FireBeetle_Giant][GenderConstants.Female] = "0";
            heights[CreatureConstants.FireBeetle_Giant][GenderConstants.Male] = "0";
            heights[CreatureConstants.FireBeetle_Giant][CreatureConstants.FireBeetle_Giant] = "0";
            //Source: https://www.d20srd.org/srd/monsters/formian.htm
            heights[CreatureConstants.FormianWorker][GenderConstants.Male] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.FormianWorker][CreatureConstants.FormianWorker] = GetMultiplierFromAverage(2 * 12 + 6);
            heights[CreatureConstants.FormianWarrior][GenderConstants.Male] = GetBaseFromAverage(4 * 12 + 6);
            heights[CreatureConstants.FormianWarrior][CreatureConstants.FormianWarrior] = GetMultiplierFromAverage(4 * 12 + 6);
            heights[CreatureConstants.FormianTaskmaster][GenderConstants.Male] = GetBaseFromAverage(4 * 12 + 6);
            heights[CreatureConstants.FormianTaskmaster][CreatureConstants.FormianTaskmaster] = GetMultiplierFromAverage(4 * 12 + 6);
            heights[CreatureConstants.FormianMyrmarch][GenderConstants.Male] = GetBaseFromAverage(5 * 12 + 6);
            heights[CreatureConstants.FormianMyrmarch][CreatureConstants.FormianMyrmarch] = GetMultiplierFromAverage(5 * 12 + 6);
            heights[CreatureConstants.FormianQueen][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.FormianQueen][CreatureConstants.FormianQueen] = GetMultiplierFromAverage(4 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Frost_worm
            heights[CreatureConstants.FrostWorm][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.FrostWorm][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.FrostWorm][CreatureConstants.FrostWorm] = GetMultiplierFromAverage(5 * 12);
            //Source: https://dungeonsdragons.fandom.com/wiki/Gargoyle
            heights[CreatureConstants.Gargoyle][GenderConstants.Agender] = "5*12";
            heights[CreatureConstants.Gargoyle][CreatureConstants.Gargoyle] = "2d10";
            heights[CreatureConstants.Gargoyle_Kapoacinth][GenderConstants.Agender] = "5*12";
            heights[CreatureConstants.Gargoyle_Kapoacinth][CreatureConstants.Gargoyle_Kapoacinth] = "2d10";
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/gelatinous-cube-species
            heights[CreatureConstants.GelatinousCube][GenderConstants.Agender] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.GelatinousCube][CreatureConstants.GelatinousCube] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Ghaele
            //Adjusting female -2" to match range
            heights[CreatureConstants.Ghaele][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 7 - 2);
            heights[CreatureConstants.Ghaele][GenderConstants.Male] = GetBaseFromRange(5 * 12 + 2, 7 * 12);
            heights[CreatureConstants.Ghaele][CreatureConstants.Ghaele] = GetMultiplierFromRange(5 * 12 + 2, 7 * 12);
            //Source: https://www.dandwiki.com/wiki/Ghoul_(5e_Race)
            heights[CreatureConstants.Ghoul][GenderConstants.Female] = "4*12";
            heights[CreatureConstants.Ghoul][GenderConstants.Male] = "4*12";
            heights[CreatureConstants.Ghoul][CreatureConstants.Ghoul] = "2d12";
            heights[CreatureConstants.Ghoul_Ghast][GenderConstants.Female] = "4*12";
            heights[CreatureConstants.Ghoul_Ghast][GenderConstants.Male] = "4*12";
            heights[CreatureConstants.Ghoul_Ghast][CreatureConstants.Ghoul_Ghast] = "2d12";
            heights[CreatureConstants.Ghoul_Lacedon][GenderConstants.Female] = "4*12";
            heights[CreatureConstants.Ghoul_Lacedon][GenderConstants.Male] = "4*12";
            heights[CreatureConstants.Ghoul_Lacedon][CreatureConstants.Ghoul_Lacedon] = "2d12";
            //Source: https://forgottenrealms.fandom.com/wiki/Cloud_giant
            heights[CreatureConstants.Giant_Cloud][GenderConstants.Female] = GetBaseFromRange(22 * 12 + 8, 25 * 12);
            heights[CreatureConstants.Giant_Cloud][GenderConstants.Male] = GetBaseFromRange(24 * 12 + 4, 26 * 12 + 8);
            heights[CreatureConstants.Giant_Cloud][CreatureConstants.Giant_Cloud] = GetMultiplierFromRange(22 * 12 + 8, 25 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Fire_giant
            //Adjusting female max -1" to match range
            heights[CreatureConstants.Giant_Fire][GenderConstants.Female] = GetBaseFromRange(17 * 12 + 5, 19 * 12 - 1);
            heights[CreatureConstants.Giant_Fire][GenderConstants.Male] = GetBaseFromRange(18 * 12 + 2, 19 * 12 + 8);
            heights[CreatureConstants.Giant_Fire][CreatureConstants.Giant_Fire] = GetMultiplierFromRange(18 * 12 + 2, 19 * 12 + 8);
            //Source: https://forgottenrealms.fandom.com/wiki/Frost_giant
            heights[CreatureConstants.Giant_Frost][GenderConstants.Female] = GetBaseFromRange(20 * 12 + 1, 22 * 12 + 4);
            heights[CreatureConstants.Giant_Frost][GenderConstants.Male] = GetBaseFromRange(21 * 12 + 3, 23 * 12 + 6);
            heights[CreatureConstants.Giant_Frost][CreatureConstants.Giant_Frost] = GetMultiplierFromRange(21 * 12 + 3, 23 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Hill_giant
            heights[CreatureConstants.Giant_Hill][GenderConstants.Female] = GetBaseFromRange(15 * 12 + 5, 16 * 12 + 4);
            heights[CreatureConstants.Giant_Hill][GenderConstants.Male] = GetBaseFromRange(16 * 12 + 1, 17 * 12);
            heights[CreatureConstants.Giant_Hill][CreatureConstants.Giant_Hill] = GetMultiplierFromRange(15 * 12 + 5, 16 * 12 + 4);
            //Source: https://forgottenrealms.fandom.com/wiki/Stone_giant
            //Adjusting female max -1" to match range
            heights[CreatureConstants.Giant_Stone][GenderConstants.Female] = GetBaseFromRange(17 * 12 + 5, 19 * 12 - 1);
            heights[CreatureConstants.Giant_Stone][GenderConstants.Male] = GetBaseFromRange(18 * 12 + 2, 19 * 12 + 8);
            heights[CreatureConstants.Giant_Stone][CreatureConstants.Giant_Stone] = GetMultiplierFromRange(18 * 12 + 2, 19 * 12 + 8);
            heights[CreatureConstants.Giant_Stone_Elder][GenderConstants.Female] = GetBaseFromRange(17 * 12 + 5, 19 * 12 - 1);
            heights[CreatureConstants.Giant_Stone_Elder][GenderConstants.Male] = GetBaseFromRange(18 * 12 + 2, 19 * 12 + 8);
            heights[CreatureConstants.Giant_Stone_Elder][CreatureConstants.Giant_Stone_Elder] = GetMultiplierFromRange(18 * 12 + 2, 19 * 12 + 8);
            //Source: https://forgottenrealms.fandom.com/wiki/Storm_giant
            heights[CreatureConstants.Giant_Storm][GenderConstants.Female] = GetBaseFromRange(23 * 12 + 8, 26 * 12 + 8);
            heights[CreatureConstants.Giant_Storm][GenderConstants.Male] = GetBaseFromRange(26 * 12 + 4, 29 * 12 + 4);
            heights[CreatureConstants.Giant_Storm][CreatureConstants.Giant_Storm] = GetMultiplierFromRange(26 * 12 + 4, 29 * 12 + 4);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/gibbering-mouther-species
            heights[CreatureConstants.GibberingMouther][GenderConstants.Agender] = GetBaseFromRange(3 * 12, 7 * 12);
            heights[CreatureConstants.GibberingMouther][CreatureConstants.GibberingMouther] = GetMultiplierFromRange(3 * 12, 7 * 12);
            //Source: https://www.d20srd.org/srd/monsters/girallon.htm
            heights[CreatureConstants.Girallon][GenderConstants.Female] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Girallon][GenderConstants.Male] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Girallon][CreatureConstants.Girallon] = GetMultiplierFromAverage(8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Githyanki
            heights[CreatureConstants.Githyanki][GenderConstants.Female] = GetBaseFromRange(5 * 12 + 4, 6 * 12 + 10);
            heights[CreatureConstants.Githyanki][GenderConstants.Male] = GetBaseFromRange(5 * 12 + 5, 6 * 12 + 11);
            heights[CreatureConstants.Githyanki][CreatureConstants.Githyanki] = GetMultiplierFromRange(5 * 12 + 5, 6 * 12 + 11);
            //Source: https://forgottenrealms.fandom.com/wiki/Githzerai
            heights[CreatureConstants.Githzerai][GenderConstants.Female] = GetBaseFromRange(5 * 12 + 1, 7 * 12);
            heights[CreatureConstants.Githzerai][GenderConstants.Male] = GetBaseFromRange(5 * 12 + 1, 7 * 12);
            heights[CreatureConstants.Githzerai][CreatureConstants.Githzerai] = GetMultiplierFromRange(5 * 12 + 1, 7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Glabrezu
            heights[CreatureConstants.Glabrezu][GenderConstants.Agender] = GetBaseFromRange(9 * 12, 15 * 12);
            heights[CreatureConstants.Glabrezu][CreatureConstants.Glabrezu] = GetMultiplierFromRange(9 * 12, 15 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Gnoll
            heights[CreatureConstants.Gnoll][GenderConstants.Female] = GetBaseFromRange(7 * 12, 7 * 12 + 6);
            heights[CreatureConstants.Gnoll][GenderConstants.Male] = GetBaseFromRange(7 * 12, 7 * 12 + 6);
            heights[CreatureConstants.Gnoll][CreatureConstants.Gnoll] = GetMultiplierFromRange(7 * 12, 7 * 12 + 6);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            heights[CreatureConstants.Gnome_Forest][GenderConstants.Female] = "2*12+10";
            heights[CreatureConstants.Gnome_Forest][GenderConstants.Male] = "3*12+0";
            heights[CreatureConstants.Gnome_Forest][CreatureConstants.Gnome_Forest] = "2d4";
            heights[CreatureConstants.Gnome_Rock][GenderConstants.Female] = "2*12+10";
            heights[CreatureConstants.Gnome_Rock][GenderConstants.Male] = "3*12+0";
            heights[CreatureConstants.Gnome_Rock][CreatureConstants.Gnome_Rock] = "2d4";
            heights[CreatureConstants.Gnome_Svirfneblin][GenderConstants.Female] = "2*12+10";
            heights[CreatureConstants.Gnome_Svirfneblin][GenderConstants.Male] = "3*12+0";
            heights[CreatureConstants.Gnome_Svirfneblin][CreatureConstants.Gnome_Svirfneblin] = "2d4";
            //Source: https://forgottenrealms.fandom.com/wiki/Goblin
            heights[CreatureConstants.Goblin][GenderConstants.Female] = GetBaseFromRange(3 * 12 + 4, 3 * 12 + 8);
            heights[CreatureConstants.Goblin][GenderConstants.Male] = GetBaseFromRange(3 * 12 + 4, 3 * 12 + 8);
            heights[CreatureConstants.Goblin][CreatureConstants.Goblin] = GetMultiplierFromRange(3 * 12 + 4, 3 * 12 + 8);
            //Source: https://pathfinderwiki.com/wiki/Clay_golem
            heights[CreatureConstants.Golem_Clay][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Golem_Clay][CreatureConstants.Golem_Clay] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            heights[CreatureConstants.Golem_Flesh][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Golem_Flesh][CreatureConstants.Golem_Flesh] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            heights[CreatureConstants.Golem_Iron][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Golem_Iron][CreatureConstants.Golem_Iron] = GetMultiplierFromAverage(12 * 12);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            heights[CreatureConstants.Golem_Stone][GenderConstants.Agender] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Golem_Stone][CreatureConstants.Golem_Stone] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            heights[CreatureConstants.Golem_Stone_Greater][GenderConstants.Agender] = GetBaseFromAverage(18 * 12);
            heights[CreatureConstants.Golem_Stone_Greater][CreatureConstants.Golem_Stone_Greater] = GetMultiplierFromAverage(18 * 12);
            //Source: https://www.d20srd.org/srd/monsters/gorgon.htm
            heights[CreatureConstants.Gorgon][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Gorgon][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Gorgon][CreatureConstants.Gorgon] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/ooze.htm#grayOoze
            heights[CreatureConstants.GrayOoze][GenderConstants.Agender] = GetBaseFromAverage(6);
            heights[CreatureConstants.GrayOoze][CreatureConstants.GrayOoze] = GetMultiplierFromAverage(6);
            //Source: https://www.d20srd.org/srd/monsters/grayRender.htm
            heights[CreatureConstants.GrayRender][GenderConstants.Agender] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.GrayRender][CreatureConstants.GrayRender] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.d20srd.org/srd/monsters/hag.htm#greenHag Copy from Human
            heights[CreatureConstants.GreenHag][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.GreenHag][CreatureConstants.GreenHag] = "2d10";
            //Source: https://www.d20srd.org/srd/monsters/grick.htm
            heights[CreatureConstants.Grick][GenderConstants.Female] = "0";
            heights[CreatureConstants.Grick][GenderConstants.Male] = "0";
            heights[CreatureConstants.Grick][CreatureConstants.Grick] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Griffon
            heights[CreatureConstants.Griffon][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Griffon][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Griffon][CreatureConstants.Griffon] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.d20srd.org/srd/monsters/sprite.htm#grig
            heights[CreatureConstants.Grig][GenderConstants.Female] = GetBaseFromAverage(18);
            heights[CreatureConstants.Grig][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Grig][CreatureConstants.Grig] = GetMultiplierFromAverage(18);
            heights[CreatureConstants.Grig_WithFiddle][GenderConstants.Female] = GetBaseFromAverage(18);
            heights[CreatureConstants.Grig_WithFiddle][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Grig_WithFiddle][CreatureConstants.Grig_WithFiddle] = GetMultiplierFromAverage(18);
            //Source: https://forgottenrealms.fandom.com/wiki/Grimlock
            heights[CreatureConstants.Grimlock][GenderConstants.Female] = GetBaseFromRange(5 * 12, 5 * 12 + 6);
            heights[CreatureConstants.Grimlock][GenderConstants.Male] = GetBaseFromRange(5 * 12, 5 * 12 + 6);
            heights[CreatureConstants.Grimlock][CreatureConstants.Grimlock] = GetMultiplierFromRange(5 * 12, 5 * 12 + 6);
            //Source: https://www.mojobob.com/roleplay/monstrousmanual/s/sphinx.html
            heights[CreatureConstants.Gynosphinx][GenderConstants.Female] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Gynosphinx][CreatureConstants.Gynosphinx] = GetMultiplierFromAverage(7 * 12);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            heights[CreatureConstants.Halfling_Deep][GenderConstants.Female] = "2*12+6";
            heights[CreatureConstants.Halfling_Deep][GenderConstants.Male] = "2*12+8";
            heights[CreatureConstants.Halfling_Deep][CreatureConstants.Halfling_Deep] = "2d4";
            heights[CreatureConstants.Halfling_Lightfoot][GenderConstants.Female] = "2*12+6";
            heights[CreatureConstants.Halfling_Lightfoot][GenderConstants.Male] = "2*12+8";
            heights[CreatureConstants.Halfling_Lightfoot][CreatureConstants.Halfling_Lightfoot] = "2d4";
            heights[CreatureConstants.Halfling_Tallfellow][GenderConstants.Female] = "3*12+6";
            heights[CreatureConstants.Halfling_Tallfellow][GenderConstants.Male] = "3*12+8";
            heights[CreatureConstants.Halfling_Tallfellow][CreatureConstants.Halfling_Tallfellow] = "2d4";
            //Source: https://www.5esrd.com/database/race/harpy/
            heights[CreatureConstants.Harpy][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Harpy][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Harpy][CreatureConstants.Harpy] = "2d10";
            //Source: https://www.dimensions.com/element/osprey-pandion-haliaetus
            heights[CreatureConstants.Hawk][GenderConstants.Female] = GetBaseFromRange(11, 16);
            heights[CreatureConstants.Hawk][GenderConstants.Male] = GetBaseFromRange(11, 16);
            heights[CreatureConstants.Hawk][CreatureConstants.Hawk] = GetMultiplierFromRange(11, 16);
            heights[CreatureConstants.Hellcat_Bezekira][GenderConstants.Female] = "0";
            heights[CreatureConstants.Hellcat_Bezekira][GenderConstants.Male] = "0";
            heights[CreatureConstants.Hellcat_Bezekira][CreatureConstants.Hellcat_Bezekira] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Hell_hound
            heights[CreatureConstants.HellHound][GenderConstants.Female] = GetBaseFromRange(24, 48 + 6);
            heights[CreatureConstants.HellHound][GenderConstants.Male] = GetBaseFromRange(24, 48 + 6);
            heights[CreatureConstants.HellHound][CreatureConstants.HellHound] = GetMultiplierFromRange(24, 48 + 6);
            heights[CreatureConstants.HellHound_NessianWarhound][GenderConstants.Female] = GetBaseFromRange(64, 72);
            heights[CreatureConstants.HellHound_NessianWarhound][GenderConstants.Male] = GetBaseFromRange(64, 72);
            heights[CreatureConstants.HellHound_NessianWarhound][CreatureConstants.HellHound_NessianWarhound] = GetMultiplierFromRange(64, 72);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            heights[CreatureConstants.Hellwasp_Swarm][GenderConstants.Agender] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Hellwasp_Swarm][CreatureConstants.Hellwasp_Swarm] = GetMultiplierFromAverage(10 * 12);
            //Source: https://www.d20srd.org/srd/monsters/demon.htm#hezrou
            heights[CreatureConstants.Hezrou][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Hezrou][CreatureConstants.Hezrou] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.mojobob.com/roleplay/monstrousmanual/s/sphinx.html
            heights[CreatureConstants.Hieracosphinx][GenderConstants.Male] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Hieracosphinx][CreatureConstants.Hieracosphinx] = GetMultiplierFromAverage(7 * 12);
            heights[CreatureConstants.Hippogriff][GenderConstants.Female] = "0";
            heights[CreatureConstants.Hippogriff][GenderConstants.Male] = "0";
            heights[CreatureConstants.Hippogriff][CreatureConstants.Hippogriff] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Hobgoblin
            heights[CreatureConstants.Hobgoblin][GenderConstants.Female] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Hobgoblin][GenderConstants.Male] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Hobgoblin][CreatureConstants.Hobgoblin] = GetMultiplierFromRange(5 * 12, 6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Homunculus
            //https://www.dimensions.com/element/eastern-gray-squirrel
            heights[CreatureConstants.Homunculus][GenderConstants.Agender] = GetBaseFromRange(4, 6);
            heights[CreatureConstants.Homunculus][CreatureConstants.Homunculus] = GetMultiplierFromRange(4, 6);
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#hornedDevilCornugon
            heights[CreatureConstants.HornedDevil_Cornugon][GenderConstants.Agender] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.HornedDevil_Cornugon][CreatureConstants.HornedDevil_Cornugon] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.dimensions.com/element/clydesdale-horse
            heights[CreatureConstants.Horse_Heavy][GenderConstants.Female] = GetBaseFromRange(64, 72);
            heights[CreatureConstants.Horse_Heavy][GenderConstants.Male] = GetBaseFromRange(64, 72);
            heights[CreatureConstants.Horse_Heavy][CreatureConstants.Horse_Heavy] = GetMultiplierFromRange(64, 72);
            //Source: https://www.dimensions.com/element/arabian-horse
            heights[CreatureConstants.Horse_Light][GenderConstants.Female] = GetBaseFromRange(57, 61);
            heights[CreatureConstants.Horse_Light][GenderConstants.Male] = GetBaseFromRange(57, 61);
            heights[CreatureConstants.Horse_Light][CreatureConstants.Horse_Light] = GetMultiplierFromRange(57, 61);
            //Source: https://www.dimensions.com/element/clydesdale-horse
            heights[CreatureConstants.Horse_Heavy_War][GenderConstants.Female] = GetBaseFromRange(64, 72);
            heights[CreatureConstants.Horse_Heavy_War][GenderConstants.Male] = GetBaseFromRange(64, 72);
            heights[CreatureConstants.Horse_Heavy_War][CreatureConstants.Horse_Heavy_War] = GetMultiplierFromRange(64, 72);
            //Source: https://www.dimensions.com/element/arabian-horse
            heights[CreatureConstants.Horse_Light_War][GenderConstants.Female] = GetBaseFromRange(57, 61);
            heights[CreatureConstants.Horse_Light_War][GenderConstants.Male] = GetBaseFromRange(57, 61);
            heights[CreatureConstants.Horse_Light_War][CreatureConstants.Horse_Light_War] = GetMultiplierFromRange(57, 61);
            //Source: https://forgottenrealms.fandom.com/wiki/Hound_archon
            heights[CreatureConstants.HoundArchon][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.HoundArchon][CreatureConstants.HoundArchon] = GetMultiplierFromAverage(6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Howler
            heights[CreatureConstants.Howler][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Howler][CreatureConstants.Howler] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            heights[CreatureConstants.Human][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Human][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Human][CreatureConstants.Human] = "2d10";
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra half of length for height
            heights[CreatureConstants.Hydra_5Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_5Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_5Heads][CreatureConstants.Hydra_5Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_6Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_6Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_6Heads][CreatureConstants.Hydra_6Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_7Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_7Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_7Heads][CreatureConstants.Hydra_7Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_8Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_8Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_8Heads][CreatureConstants.Hydra_8Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_9Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_9Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_9Heads][CreatureConstants.Hydra_9Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_10Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_10Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_10Heads][CreatureConstants.Hydra_10Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_11Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_11Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_11Heads][CreatureConstants.Hydra_11Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_12Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_12Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Hydra_12Heads][CreatureConstants.Hydra_12Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            //Source: https://www.dimensions.com/element/striped-hyena-hyaena-hyaena
            heights[CreatureConstants.Hyena][GenderConstants.Female] = GetBaseFromRange(25, 30);
            heights[CreatureConstants.Hyena][GenderConstants.Male] = GetBaseFromRange(25, 30);
            heights[CreatureConstants.Hyena][CreatureConstants.Hyena] = GetMultiplierFromRange(25, 30);
            //Source: https://forgottenrealms.fandom.com/wiki/Gelugon
            heights[CreatureConstants.IceDevil_Gelugon][GenderConstants.Agender] = GetBaseFromRange(10 * 12 + 6, 12 * 12);
            heights[CreatureConstants.IceDevil_Gelugon][CreatureConstants.IceDevil_Gelugon] = GetMultiplierFromRange(10 * 12 + 6, 12 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Imp
            heights[CreatureConstants.Imp][GenderConstants.Agender] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Imp][GenderConstants.Female] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Imp][GenderConstants.Male] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Imp][CreatureConstants.Imp] = GetMultiplierFromAverage(2 * 12);
            //Source: https://www.d20srd.org/srd/monsters/invisibleStalker.htm
            //https://www.d20srd.org/srd/combat/movementPositionAndDistance.htm using generic Large, since actual form is unknown
            heights[CreatureConstants.InvisibleStalker][GenderConstants.Agender] = GetBaseFromRange(8 * 12, 16 * 12);
            heights[CreatureConstants.InvisibleStalker][CreatureConstants.InvisibleStalker] = GetMultiplierFromRange(8 * 12, 16 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Janni
            heights[CreatureConstants.Janni][GenderConstants.Agender] = GetBaseFromRange(6 * 12, 7 * 12);
            heights[CreatureConstants.Janni][GenderConstants.Female] = GetBaseFromRange(6 * 12, 7 * 12);
            heights[CreatureConstants.Janni][GenderConstants.Male] = GetBaseFromRange(6 * 12, 7 * 12);
            heights[CreatureConstants.Janni][CreatureConstants.Janni] = GetMultiplierFromRange(6 * 12, 7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Kobold
            heights[CreatureConstants.Kobold][GenderConstants.Female] = GetBaseFromRange(2 * 12, 2 * 12 + 6);
            heights[CreatureConstants.Kobold][GenderConstants.Male] = GetBaseFromRange(2 * 12, 2 * 12 + 6);
            heights[CreatureConstants.Kobold][CreatureConstants.Kobold] = GetMultiplierFromRange(2 * 12, 2 * 12 + 6);
            //Source: https://pathfinderwiki.com/wiki/Kolyarut
            //Can't find definitive height, but "size of a tall humanoid", so using human + some
            heights[CreatureConstants.Kolyarut][GenderConstants.Agender] = "5*12";
            heights[CreatureConstants.Kolyarut][CreatureConstants.Kolyarut] = "2d12";
            //Source: https://forgottenrealms.fandom.com/wiki/Kraken
            heights[CreatureConstants.Kraken][GenderConstants.Female] = GetBaseFromAverage(30 * 12);
            heights[CreatureConstants.Kraken][GenderConstants.Male] = GetBaseFromAverage(30 * 12);
            heights[CreatureConstants.Kraken][CreatureConstants.Kraken] = GetMultiplierFromAverage(30 * 12);
            //Source: https://www.d20srd.org/srd/monsters/krenshar.htm
            heights[CreatureConstants.Krenshar][GenderConstants.Female] = "0";
            heights[CreatureConstants.Krenshar][GenderConstants.Male] = "0";
            heights[CreatureConstants.Krenshar][CreatureConstants.Krenshar] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Kuo-toa
            heights[CreatureConstants.KuoToa][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.KuoToa][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.KuoToa][CreatureConstants.KuoToa] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Lamia
            heights[CreatureConstants.Lamia][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Lamia][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Lamia][CreatureConstants.Lamia] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/lammasu.htm
            //https://www.dimensions.com/element/african-lion scale up from lion: [44,50]*8*12/[54,78] = [78,62]
            heights[CreatureConstants.Lammasu][GenderConstants.Female] = GetBaseFromRange(62, 78);
            heights[CreatureConstants.Lammasu][GenderConstants.Male] = GetBaseFromRange(62, 78);
            heights[CreatureConstants.Lammasu][CreatureConstants.Lammasu] = GetMultiplierFromRange(62, 78);
            //Source: https://forgottenrealms.fandom.com/wiki/Lantern_archon
            heights[CreatureConstants.LanternArchon][GenderConstants.Agender] = GetBaseFromRange(12, 36);
            heights[CreatureConstants.LanternArchon][CreatureConstants.LanternArchon] = GetMultiplierFromRange(12, 36);
            //Source: https://forgottenrealms.fandom.com/wiki/Lemure
            heights[CreatureConstants.Lemure][GenderConstants.Agender] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Lemure][CreatureConstants.Lemure] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Leonal
            heights[CreatureConstants.Leonal][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Leonal][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Leonal][CreatureConstants.Leonal] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.dimensions.com/element/cougar
            heights[CreatureConstants.Leopard][GenderConstants.Female] = GetBaseFromRange(21, 28);
            heights[CreatureConstants.Leopard][GenderConstants.Male] = GetBaseFromRange(21, 28);
            heights[CreatureConstants.Leopard][CreatureConstants.Leopard] = GetMultiplierFromRange(21, 28);
            //Using Human
            heights[CreatureConstants.Lillend][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Lillend][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Lillend][CreatureConstants.Lillend] = "2d10";
            //Source: https://www.dimensions.com/element/african-lion
            heights[CreatureConstants.Lion][GenderConstants.Female] = GetBaseFromAverage(2 * 12 + 10, 3 * 12 + 4);
            heights[CreatureConstants.Lion][GenderConstants.Male] = GetBaseFromAverage(3 * 12 + 4);
            heights[CreatureConstants.Lion][CreatureConstants.Lion] = GetMultiplierFromAverage(3 * 12 + 4);
            //Scaling up from lion, x3 based on length. Since length doesn't differ for male/female, only use male
            heights[CreatureConstants.Lion_Dire][GenderConstants.Female] = GetBaseFromAverage((3 * 12 + 4) * 3);
            heights[CreatureConstants.Lion_Dire][GenderConstants.Male] = GetBaseFromAverage((3 * 12 + 4) * 3);
            heights[CreatureConstants.Lion_Dire][CreatureConstants.Lion_Dire] = GetMultiplierFromAverage((3 * 12 + 4) * 3);
            //Source: https://www.dimensions.com/element/green-iguana-iguana-iguana
            heights[CreatureConstants.Lizard][GenderConstants.Female] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Lizard][GenderConstants.Male] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Lizard][CreatureConstants.Lizard] = GetMultiplierFromRange(1, 2);
            //Source: https://www.dimensions.com/element/savannah-monitor-varanus-exanthematicus
            heights[CreatureConstants.Lizard_Monitor][GenderConstants.Female] = GetBaseFromRange(7, 8);
            heights[CreatureConstants.Lizard_Monitor][GenderConstants.Male] = GetBaseFromRange(7, 8);
            heights[CreatureConstants.Lizard_Monitor][CreatureConstants.Lizard_Monitor] = GetMultiplierFromRange(7, 8);
            //Source: https://forgottenrealms.fandom.com/wiki/Lizardfolk
            heights[CreatureConstants.Lizardfolk][GenderConstants.Female] = GetBaseFromRange(6 * 12, 7 * 12);
            heights[CreatureConstants.Lizardfolk][GenderConstants.Male] = GetBaseFromRange(6 * 12, 7 * 12);
            heights[CreatureConstants.Lizardfolk][CreatureConstants.Lizardfolk] = GetMultiplierFromRange(6 * 12, 7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Locathah
            heights[CreatureConstants.Locathah][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Locathah][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Locathah][CreatureConstants.Locathah] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            heights[CreatureConstants.Locust_Swarm][GenderConstants.Agender] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Locust_Swarm][CreatureConstants.Locust_Swarm] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Magmin
            heights[CreatureConstants.Magmin][GenderConstants.Agender] = GetBaseFromRange(3 * 12, 4 * 12);
            heights[CreatureConstants.Magmin][CreatureConstants.Magmin] = GetMultiplierFromRange(3 * 12, 4 * 12);
            //Source: https://www.dimensions.com/element/reef-manta-ray-mobula-alfredi
            heights[CreatureConstants.MantaRay][GenderConstants.Female] = "0";
            heights[CreatureConstants.MantaRay][GenderConstants.Male] = "0";
            heights[CreatureConstants.MantaRay][CreatureConstants.MantaRay] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Manticore Using Lion for height
            heights[CreatureConstants.Manticore][GenderConstants.Female] = GetBaseFromAverage(2 * 12 + 10, 3 * 12 + 4);
            heights[CreatureConstants.Manticore][GenderConstants.Male] = GetBaseFromAverage(3 * 12 + 4);
            heights[CreatureConstants.Manticore][CreatureConstants.Manticore] = GetMultiplierFromAverage(3 * 12 + 4);
            //Source: https://forgottenrealms.fandom.com/wiki/Marilith
            heights[CreatureConstants.Marilith][GenderConstants.Female] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Marilith][CreatureConstants.Marilith] = GetMultiplierFromRange(7 * 12, 9 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Marut
            heights[CreatureConstants.Marut][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Marut][CreatureConstants.Marut] = GetMultiplierFromAverage(12 * 12);
            //Source: https://www.d20srd.org/srd/monsters/medusa.htm
            heights[CreatureConstants.Medusa][GenderConstants.Female] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Medusa][GenderConstants.Male] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Medusa][CreatureConstants.Medusa] = GetMultiplierFromRange(5 * 12, 6 * 12);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Velociraptor
            heights[CreatureConstants.Megaraptor][GenderConstants.Female] = GetBaseFromAverage(67);
            heights[CreatureConstants.Megaraptor][GenderConstants.Male] = GetBaseFromAverage(67);
            heights[CreatureConstants.Megaraptor][CreatureConstants.Megaraptor] = GetMultiplierFromAverage(67);
            //Source: https://forgottenrealms.fandom.com/wiki/Mephit
            heights[CreatureConstants.Mephit_Air][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Air][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Air][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Air][CreatureConstants.Mephit_Air] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Dust][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Dust][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Dust][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Dust][CreatureConstants.Mephit_Dust] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Earth][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Earth][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Earth][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Earth][CreatureConstants.Mephit_Earth] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Fire][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Fire][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Fire][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Fire][CreatureConstants.Mephit_Fire] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ice][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ice][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ice][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ice][CreatureConstants.Mephit_Ice] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Magma][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Magma][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Magma][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Magma][CreatureConstants.Mephit_Magma] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ooze][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ooze][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ooze][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Ooze][CreatureConstants.Mephit_Ooze] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Salt][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Salt][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Salt][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Salt][CreatureConstants.Mephit_Salt] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Steam][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Steam][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Steam][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Steam][CreatureConstants.Mephit_Steam] = GetMultiplierFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Water][GenderConstants.Agender] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Water][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Water][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Mephit_Water][CreatureConstants.Mephit_Water] = GetMultiplierFromAverage(4 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Merfolk
            heights[CreatureConstants.Merfolk][GenderConstants.Female] = "0";
            heights[CreatureConstants.Merfolk][GenderConstants.Male] = "0";
            heights[CreatureConstants.Merfolk][CreatureConstants.Merfolk] = "0";
            //Source: https://www.d20srd.org/srd/monsters/mimic.htm
            heights[CreatureConstants.Mimic][GenderConstants.Agender] = GetBaseFromRange(3 * 12, 10 * 12);
            heights[CreatureConstants.Mimic][CreatureConstants.Mimic] = GetMultiplierFromRange(3 * 12, 10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Mind_flayer
            heights[CreatureConstants.MindFlayer][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.MindFlayer][CreatureConstants.MindFlayer] = GetMultiplierFromAverage(6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Minotaur
            heights[CreatureConstants.Minotaur][GenderConstants.Female] = GetBaseFromAverage(7 * 12, 9 * 12);
            heights[CreatureConstants.Minotaur][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Minotaur][CreatureConstants.Minotaur] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.d20srd.org/srd/monsters/mohrg.htm
            heights[CreatureConstants.Mohrg][GenderConstants.Agender] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Mohrg][CreatureConstants.Mohrg] = GetMultiplierFromRange(5 * 12, 6 * 12);
            //Source: https://www.dimensions.com/element/tufted-capuchin-sapajus-apella
            heights[CreatureConstants.Monkey][GenderConstants.Female] = GetBaseFromRange(10, 16);
            heights[CreatureConstants.Monkey][GenderConstants.Male] = GetBaseFromRange(10, 16);
            heights[CreatureConstants.Monkey][CreatureConstants.Monkey] = GetMultiplierFromRange(10, 16);
            //Source: https://www.dimensions.com/element/mule-equus-asinus-x-equus-caballus
            heights[CreatureConstants.Mule][GenderConstants.Female] = GetBaseFromRange(56, 68);
            heights[CreatureConstants.Mule][GenderConstants.Male] = GetBaseFromRange(56, 68);
            heights[CreatureConstants.Mule][CreatureConstants.Mule] = GetMultiplierFromRange(56, 68);
            //Source: https://www.d20srd.org/srd/monsters/mummy.htm
            heights[CreatureConstants.Mummy][GenderConstants.Female] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Mummy][GenderConstants.Male] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Mummy][CreatureConstants.Mummy] = GetMultiplierFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Naga_Dark][GenderConstants.Hermaphrodite] = "0";
            heights[CreatureConstants.Naga_Dark][CreatureConstants.Naga_Dark] = "0";
            heights[CreatureConstants.Naga_Guardian][GenderConstants.Hermaphrodite] = "0";
            heights[CreatureConstants.Naga_Guardian][CreatureConstants.Naga_Guardian] = "0";
            heights[CreatureConstants.Naga_Spirit][GenderConstants.Hermaphrodite] = "0";
            heights[CreatureConstants.Naga_Spirit][CreatureConstants.Naga_Spirit] = "0";
            heights[CreatureConstants.Naga_Water][GenderConstants.Hermaphrodite] = "0";
            heights[CreatureConstants.Naga_Water][CreatureConstants.Naga_Water] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Nalfeshnee
            heights[CreatureConstants.Nalfeshnee][GenderConstants.Agender] = GetBaseFromRange(10 * 12, 20 * 12);
            heights[CreatureConstants.Nalfeshnee][CreatureConstants.Nalfeshnee] = GetMultiplierFromRange(10 * 12, 20 * 12);
            //Source: https://www.d20srd.org/srd/monsters/nightHag.htm Copy from Human
            heights[CreatureConstants.NightHag][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.NightHag][CreatureConstants.NightHag] = "2d10";
            //Source: https://www.d20srd.org/srd/monsters/nightshade.htm
            heights[CreatureConstants.Nightcrawler][GenderConstants.Agender] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Nightcrawler][CreatureConstants.Nightcrawler] = GetMultiplierFromAverage(7 * 12);
            //Source: https://www.d20srd.org/srd/monsters/nightmare.htm
            heights[CreatureConstants.Nightmare][GenderConstants.Agender] = GetBaseFromRange(57, 61);
            heights[CreatureConstants.Nightmare][CreatureConstants.Nightmare] = GetMultiplierFromRange(57, 61);
            //Scale up x2
            heights[CreatureConstants.Nightmare_Cauchemar][GenderConstants.Agender] = GetBaseFromRange(57 * 2, 61 * 2);
            heights[CreatureConstants.Nightmare_Cauchemar][CreatureConstants.Nightmare_Cauchemar] = GetMultiplierFromRange(57 * 2, 61 * 2);
            //Source: https://www.d20srd.org/srd/monsters/nightshade.htm
            heights[CreatureConstants.Nightwalker][GenderConstants.Agender] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Nightwalker][CreatureConstants.Nightwalker] = GetMultiplierFromAverage(20 * 12);
            //Source: https://www.d20srd.org/srd/monsters/nightshade.htm
            //https://www.dimensions.com/element/giant-golden-crowned-flying-fox-acerodon-jubatus scaled up: [11,16]*40*12/[59,67] = [89,115]
            heights[CreatureConstants.Nightwing][GenderConstants.Agender] = GetBaseFromRange(89, 115);
            heights[CreatureConstants.Nightwing][CreatureConstants.Nightwing] = GetMultiplierFromRange(89, 115);
            //Source: https://www.d20srd.org/srd/monsters/sprite.htm#nixie
            heights[CreatureConstants.Nixie][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Nixie][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.Nixie][CreatureConstants.Nixie] = GetMultiplierFromAverage(4 * 12);
            //Source: https://www.d20srd.org/srd/monsters/nymph.htm Copy from High Elf
            heights[CreatureConstants.Nymph][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Nymph][CreatureConstants.Nymph] = "2d6";
            //Source: https://www.d20srd.org/srd/monsters/ooze.htm#ochreJelly
            heights[CreatureConstants.OchreJelly][GenderConstants.Agender] = GetBaseFromAverage(6);
            heights[CreatureConstants.OchreJelly][CreatureConstants.OchreJelly] = GetMultiplierFromAverage(6);
            //Source: https://www.dimensions.com/element/common-octopus-octopus-vulgaris
            heights[CreatureConstants.Octopus][GenderConstants.Female] = GetBaseFromRange(30, 39);
            heights[CreatureConstants.Octopus][GenderConstants.Male] = GetBaseFromRange(30, 39);
            heights[CreatureConstants.Octopus][CreatureConstants.Octopus] = GetMultiplierFromRange(30, 39);
            //Source: https://www.d20srd.org/srd/monsters/octopusGiant.htm
            heights[CreatureConstants.Octopus_Giant][GenderConstants.Female] = GetBaseFromAtLeast(10 * 12);
            heights[CreatureConstants.Octopus_Giant][GenderConstants.Male] = GetBaseFromAtLeast(10 * 12);
            heights[CreatureConstants.Octopus_Giant][CreatureConstants.Octopus_Giant] = GetMultiplierFromAtLeast(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Ogre
            heights[CreatureConstants.Ogre][GenderConstants.Female] = GetBaseFromRange(9 * 12 + 3, 10 * 12);
            heights[CreatureConstants.Ogre][GenderConstants.Male] = GetBaseFromRange(10 * 12 + 1, 10 * 12 + 10);
            heights[CreatureConstants.Ogre][CreatureConstants.Ogre] = GetMultiplierFromRange(10 * 12 + 1, 10 * 12 + 10);
            heights[CreatureConstants.Ogre_Merrow][GenderConstants.Female] = GetBaseFromRange(9 * 12 + 3, 10 * 12);
            heights[CreatureConstants.Ogre_Merrow][GenderConstants.Male] = GetBaseFromRange(10 * 12 + 1, 10 * 12 + 10);
            heights[CreatureConstants.Ogre_Merrow][CreatureConstants.Ogre_Merrow] = GetMultiplierFromRange(10 * 12 + 1, 10 * 12 + 10);
            //Source: https://forgottenrealms.fandom.com/wiki/Oni_mage
            heights[CreatureConstants.OgreMage][GenderConstants.Female] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.OgreMage][GenderConstants.Male] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.OgreMage][CreatureConstants.OgreMage] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Orc
            heights[CreatureConstants.Orc][GenderConstants.Female] = GetBaseFromAtLeast(6 * 12);
            heights[CreatureConstants.Orc][GenderConstants.Male] = GetBaseFromAtLeast(6 * 12);
            heights[CreatureConstants.Orc][CreatureConstants.Orc] = GetMultiplierFromAtLeast(6 * 12);
            //Source: https://www.d20srd.org/srd/description.htm#vitalStatistics
            heights[CreatureConstants.Orc_Half][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Orc_Half][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Orc_Half][CreatureConstants.Orc_Half] = "2d12";
            //Source: https://criticalrole.fandom.com/wiki/Otyugh
            heights[CreatureConstants.Otyugh][GenderConstants.Hermaphrodite] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Otyugh][CreatureConstants.Otyugh] = GetMultiplierFromAverage(15 * 12);
            //Source: https://www.dimensions.com/element/great-horned-owl-bubo-virginianus
            heights[CreatureConstants.Owl][GenderConstants.Female] = GetBaseFromRange(9, 14);
            heights[CreatureConstants.Owl][GenderConstants.Male] = GetBaseFromRange(9, 14);
            heights[CreatureConstants.Owl][CreatureConstants.Owl] = GetMultiplierFromRange(9, 14);
            //Source: https://www.d20srd.org/srd/monsters/owlGiant.htm 
            heights[CreatureConstants.Owl_Giant][GenderConstants.Female] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Owl_Giant][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Owl_Giant][CreatureConstants.Owl_Giant] = GetMultiplierFromAverage(9 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Owlbear
            //https://www.dimensions.com/element/polar-bears polar bears are similar length, so using for height
            //Adjusting female max +5" to match range
            heights[CreatureConstants.Owlbear][GenderConstants.Female] = GetBaseFromRange(2 * 12 + 8, 3 * 12 + 11 + 5);
            heights[CreatureConstants.Owlbear][GenderConstants.Male] = GetBaseFromRange(3 * 12 + 7, 5 * 12 + 3);
            heights[CreatureConstants.Owlbear][CreatureConstants.Owlbear] = GetMultiplierFromRange(3 * 12 + 7, 5 * 12 + 3);
            //Source: https://www.d20srd.org/srd/monsters/pegasus.htm
            heights[CreatureConstants.Pegasus][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Pegasus][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Pegasus][CreatureConstants.Pegasus] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.d20pfsrd.com/bestiary/monster-listings/plants/fungus-phantom/
            heights[CreatureConstants.PhantomFungus][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.PhantomFungus][CreatureConstants.PhantomFungus] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/phaseSpider.htm
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large - going between, so 12 inches
            heights[CreatureConstants.PhaseSpider][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.PhaseSpider][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.PhaseSpider][CreatureConstants.PhaseSpider] = GetMultiplierFromAverage(12);
            //Source: https://www.d20srd.org/srd/monsters/phasm.htm
            heights[CreatureConstants.Phasm][GenderConstants.Agender] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.Phasm][CreatureConstants.Phasm] = GetMultiplierFromAverage(2 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Pit_fiend
            heights[CreatureConstants.PitFiend][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.PitFiend][CreatureConstants.PitFiend] = GetMultiplierFromAverage(12 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Pixie
            heights[CreatureConstants.Pixie][GenderConstants.Female] = GetBaseFromRange(12, 30);
            heights[CreatureConstants.Pixie][GenderConstants.Male] = GetBaseFromRange(12, 30);
            heights[CreatureConstants.Pixie][CreatureConstants.Pixie] = GetMultiplierFromRange(12, 30);
            heights[CreatureConstants.Pixie_WithIrresistibleDance][GenderConstants.Female] = GetBaseFromRange(12, 30);
            heights[CreatureConstants.Pixie_WithIrresistibleDance][GenderConstants.Male] = GetBaseFromRange(12, 30);
            heights[CreatureConstants.Pixie_WithIrresistibleDance][CreatureConstants.Pixie_WithIrresistibleDance] = GetMultiplierFromRange(12, 30);
            //Source: https://www.dimensions.com/element/shetland-pony
            heights[CreatureConstants.Pony][GenderConstants.Female] = GetBaseFromRange(28, 44);
            heights[CreatureConstants.Pony][GenderConstants.Male] = GetBaseFromRange(28, 44);
            heights[CreatureConstants.Pony][CreatureConstants.Pony] = GetMultiplierFromRange(28, 44);
            heights[CreatureConstants.Pony_War][GenderConstants.Female] = GetBaseFromRange(28, 44);
            heights[CreatureConstants.Pony_War][GenderConstants.Male] = GetBaseFromRange(28, 44);
            heights[CreatureConstants.Pony_War][CreatureConstants.Pony_War] = GetMultiplierFromRange(28, 44);
            //Source: https://www.dimensions.com/element/harbour-porpoise-phocoena-phocoena
            heights[CreatureConstants.Porpoise][GenderConstants.Female] = GetBaseFromRange(14, 16);
            heights[CreatureConstants.Porpoise][GenderConstants.Male] = GetBaseFromRange(14, 16);
            heights[CreatureConstants.Porpoise][CreatureConstants.Porpoise] = GetMultiplierFromRange(14, 16);
            heights[CreatureConstants.PrayingMantis_Giant][GenderConstants.Female] = "0";
            heights[CreatureConstants.PrayingMantis_Giant][GenderConstants.Male] = "0";
            heights[CreatureConstants.PrayingMantis_Giant][CreatureConstants.PrayingMantis_Giant] = "0";
            //Source: https://www.d20srd.org/srd/monsters/pseudodragon.htm
            //Scale from Red Dragon Wyrmling: 48*3/16 = 9
            heights[CreatureConstants.Pseudodragon][GenderConstants.Female] = GetBaseFromAverage(9);
            heights[CreatureConstants.Pseudodragon][GenderConstants.Male] = GetBaseFromAverage(9);
            heights[CreatureConstants.Pseudodragon][CreatureConstants.Pseudodragon] = GetMultiplierFromAverage(9);
            //Source: https://www.d20srd.org/srd/monsters/purpleWorm.htm
            heights[CreatureConstants.PurpleWorm][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.PurpleWorm][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.PurpleWorm][CreatureConstants.PurpleWorm] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra half of length for height
            heights[CreatureConstants.Pyrohydra_5Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_5Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_5Heads][CreatureConstants.Pyrohydra_5Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_6Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_6Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_6Heads][CreatureConstants.Pyrohydra_6Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_7Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_7Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_7Heads][CreatureConstants.Pyrohydra_7Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_8Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_8Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_8Heads][CreatureConstants.Pyrohydra_8Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_9Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_9Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_9Heads][CreatureConstants.Pyrohydra_9Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_10Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_10Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_10Heads][CreatureConstants.Pyrohydra_10Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_11Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_11Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_11Heads][CreatureConstants.Pyrohydra_11Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_12Heads][GenderConstants.Female] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_12Heads][GenderConstants.Male] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Pyrohydra_12Heads][CreatureConstants.Pyrohydra_12Heads] = GetMultiplierFromAverage(20 * 12 / 2);
            //Source: https://forgottenrealms.fandom.com/wiki/Quasit
            heights[CreatureConstants.Quasit][GenderConstants.Agender] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.Quasit][CreatureConstants.Quasit] = GetMultiplierFromRange(12, 24);
            //Source: https://www.d20srd.org/srd/monsters/rakshasa.htm Copy from Human
            heights[CreatureConstants.Rakshasa][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Rakshasa][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Rakshasa][CreatureConstants.Rakshasa] = "2d10";
            //Source: https://www.d20srd.org/srd/monsters/rast.htm - body size of "large dog", head almost as large as body, so Riding Dog height x2
            heights[CreatureConstants.Rast][GenderConstants.Agender] = GetBaseFromRange(22 * 2, 30 * 2);
            heights[CreatureConstants.Rast][CreatureConstants.Rast] = GetMultiplierFromRange(22 * 2, 30 * 2);
            //Source: https://www.dimensions.com/element/common-rat
            heights[CreatureConstants.Rat][GenderConstants.Female] = GetBaseFromRange(2, 4);
            heights[CreatureConstants.Rat][GenderConstants.Male] = GetBaseFromRange(2, 4);
            heights[CreatureConstants.Rat][CreatureConstants.Rat] = GetMultiplierFromRange(2, 4);
            //Scaled up from Rat, x6 based on length
            heights[CreatureConstants.Rat_Dire][GenderConstants.Female] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.Rat_Dire][GenderConstants.Male] = GetBaseFromRange(12, 24);
            heights[CreatureConstants.Rat_Dire][CreatureConstants.Rat_Dire] = GetMultiplierFromRange(12, 24);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            heights[CreatureConstants.Rat_Swarm][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Rat_Swarm][CreatureConstants.Rat_Swarm] = "0";
            //Source: https://www.dimensions.com/element/common-raven-corvus-corax
            heights[CreatureConstants.Raven][GenderConstants.Female] = GetBaseFromRange(12, 16);
            heights[CreatureConstants.Raven][GenderConstants.Male] = GetBaseFromRange(12, 16);
            heights[CreatureConstants.Raven][CreatureConstants.Raven] = GetMultiplierFromRange(12, 16);
            //Source: https://www.d20srd.org/srd/monsters/ravid.htm
            heights[CreatureConstants.Ravid][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Ravid][CreatureConstants.Ravid] = "0";
            //Source: https://www.d20srd.org/srd/monsters/razorBoar.htm Copying from Dire Boar
            heights[CreatureConstants.RazorBoar][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.RazorBoar][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.RazorBoar][CreatureConstants.RazorBoar] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/remorhaz.htm
            heights[CreatureConstants.Remorhaz][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Remorhaz][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Remorhaz][CreatureConstants.Remorhaz] = GetMultiplierFromAverage(5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Retriever
            heights[CreatureConstants.Retriever][GenderConstants.Agender] = GetBaseFromAverage(12 * 12);
            heights[CreatureConstants.Retriever][CreatureConstants.Retriever] = GetMultiplierFromAverage(12 * 12);
            //Source: https://www.d20srd.org/srd/monsters/rhinoceros.htm
            heights[CreatureConstants.Rhinoceras][GenderConstants.Female] = GetBaseFromRange(3 * 12, 6 * 12);
            heights[CreatureConstants.Rhinoceras][GenderConstants.Male] = GetBaseFromRange(3 * 12, 6 * 12);
            heights[CreatureConstants.Rhinoceras][CreatureConstants.Rhinoceras] = GetMultiplierFromRange(3 * 12, 6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Roc
            heights[CreatureConstants.Roc][GenderConstants.Female] = "0";
            heights[CreatureConstants.Roc][GenderConstants.Male] = "0";
            heights[CreatureConstants.Roc][CreatureConstants.Roc] = "0";
            //Source: https://www.d20srd.org/srd/monsters/roper.htm
            heights[CreatureConstants.Roper][GenderConstants.Hermaphrodite] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Roper][CreatureConstants.Roper] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.d20srd.org/srd/monsters/rustMonster.htm
            heights[CreatureConstants.RustMonster][GenderConstants.Female] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.RustMonster][GenderConstants.Male] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.RustMonster][CreatureConstants.RustMonster] = GetMultiplierFromAverage(3 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Sahuagin
            heights[CreatureConstants.Sahuagin][GenderConstants.Female] = GetBaseFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin][GenderConstants.Male] = GetBaseFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin][CreatureConstants.Sahuagin] = GetMultiplierFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin_Malenti][GenderConstants.Female] = GetBaseFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin_Malenti][GenderConstants.Male] = GetBaseFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin_Malenti][CreatureConstants.Sahuagin_Malenti] = GetMultiplierFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin_Mutant][GenderConstants.Female] = GetBaseFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin_Mutant][GenderConstants.Male] = GetBaseFromRange(6 * 12, 9 * 12);
            heights[CreatureConstants.Sahuagin_Mutant][CreatureConstants.Sahuagin_Mutant] = GetMultiplierFromRange(6 * 12, 9 * 12);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/salamander-article (average)
            //Scaling down by half for flamebrother, Scaling up x2 for noble. Assuming height is half of length
            heights[CreatureConstants.Salamander_Flamebrother][GenderConstants.Agender] = GetBaseFromAverage(20 * 12 / 4);
            heights[CreatureConstants.Salamander_Flamebrother][CreatureConstants.Salamander_Flamebrother] = GetMultiplierFromAverage(20 * 12 / 4);
            heights[CreatureConstants.Salamander_Average][GenderConstants.Agender] = GetBaseFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Salamander_Average][CreatureConstants.Salamander_Average] = GetMultiplierFromAverage(20 * 12 / 2);
            heights[CreatureConstants.Salamander_Noble][GenderConstants.Agender] = GetBaseFromAverage(20 * 12);
            heights[CreatureConstants.Salamander_Noble][CreatureConstants.Salamander_Noble] = GetMultiplierFromAverage(20 * 12);
            //Source: https://www.d20srd.org/srd/monsters/satyr.htm - copy from Half-Elf
            heights[CreatureConstants.Satyr][GenderConstants.Male] = "4*12+7";
            heights[CreatureConstants.Satyr][CreatureConstants.Satyr] = "2d8";
            heights[CreatureConstants.Satyr_WithPipes][GenderConstants.Male] = "4*12+7";
            heights[CreatureConstants.Satyr_WithPipes][CreatureConstants.Satyr_WithPipes] = "2d8";
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Tiny
            heights[CreatureConstants.Scorpion_Monstrous_Tiny][GenderConstants.Female] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Scorpion_Monstrous_Tiny][GenderConstants.Male] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Scorpion_Monstrous_Tiny][CreatureConstants.Scorpion_Monstrous_Tiny] = GetMultiplierFromRange(1, 2);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Small
            heights[CreatureConstants.Scorpion_Monstrous_Small][GenderConstants.Female] = GetBaseFromAverage(3);
            heights[CreatureConstants.Scorpion_Monstrous_Small][GenderConstants.Male] = GetBaseFromAverage(3);
            heights[CreatureConstants.Scorpion_Monstrous_Small][CreatureConstants.Scorpion_Monstrous_Small] = GetMultiplierFromAverage(3);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Medium
            heights[CreatureConstants.Scorpion_Monstrous_Medium][GenderConstants.Female] = GetBaseFromAverage(6);
            heights[CreatureConstants.Scorpion_Monstrous_Medium][GenderConstants.Male] = GetBaseFromAverage(6);
            heights[CreatureConstants.Scorpion_Monstrous_Medium][CreatureConstants.Scorpion_Monstrous_Medium] = GetMultiplierFromAverage(6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Large
            heights[CreatureConstants.Scorpion_Monstrous_Large][GenderConstants.Female] = GetBaseFromAverage(18);
            heights[CreatureConstants.Scorpion_Monstrous_Large][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Scorpion_Monstrous_Large][CreatureConstants.Scorpion_Monstrous_Large] = GetMultiplierFromAverage(18);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Huge
            heights[CreatureConstants.Scorpion_Monstrous_Huge][GenderConstants.Female] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.Scorpion_Monstrous_Huge][GenderConstants.Male] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.Scorpion_Monstrous_Huge][CreatureConstants.Scorpion_Monstrous_Huge] = GetMultiplierFromAverage(2 * 12 + 6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Gargantuan
            heights[CreatureConstants.Scorpion_Monstrous_Gargantuan][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Scorpion_Monstrous_Gargantuan][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Scorpion_Monstrous_Gargantuan][CreatureConstants.Scorpion_Monstrous_Gargantuan] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Scorpion,_Colossal
            heights[CreatureConstants.Scorpion_Monstrous_Colossal][GenderConstants.Female] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Scorpion_Monstrous_Colossal][GenderConstants.Male] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Scorpion_Monstrous_Colossal][CreatureConstants.Scorpion_Monstrous_Colossal] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Tlincalli
            heights[CreatureConstants.Scorpionfolk][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Scorpionfolk][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Scorpionfolk][CreatureConstants.Scorpionfolk] = GetMultiplierFromAverage(6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Sea_cat
            heights[CreatureConstants.SeaCat][GenderConstants.Female] = "0";
            heights[CreatureConstants.SeaCat][GenderConstants.Male] = "0";
            heights[CreatureConstants.SeaCat][CreatureConstants.SeaCat] = "0";
            //Source: https://www.d20srd.org/srd/monsters/hag.htm - copy from Human
            heights[CreatureConstants.SeaHag][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.SeaHag][CreatureConstants.SeaHag] = "2d10";
            //Source: https://www.d20srd.org/srd/monsters/shadow.htm
            heights[CreatureConstants.Shadow][GenderConstants.Agender] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Shadow][CreatureConstants.Shadow] = GetMultiplierFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Shadow_Greater][GenderConstants.Agender] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Shadow_Greater][CreatureConstants.Shadow_Greater] = GetMultiplierFromRange(5 * 12, 6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/shadowMastiff.htm
            heights[CreatureConstants.ShadowMastiff][GenderConstants.Female] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.ShadowMastiff][GenderConstants.Male] = GetBaseFromAverage(2 * 12);
            heights[CreatureConstants.ShadowMastiff][CreatureConstants.ShadowMastiff] = GetMultiplierFromAverage(2 * 12);
            //Source: https://www.d20srd.org/srd/monsters/shamblingMound.htm
            heights[CreatureConstants.ShamblingMound][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.ShamblingMound][CreatureConstants.ShamblingMound] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.dimensions.com/element/blacktip-shark-carcharhinus-limbatus
            heights[CreatureConstants.Shark_Medium][GenderConstants.Female] = "0";
            heights[CreatureConstants.Shark_Medium][GenderConstants.Male] = "0";
            heights[CreatureConstants.Shark_Medium][CreatureConstants.Shark_Medium] = "0";
            //Source: https://www.dimensions.com/element/thresher-shark
            heights[CreatureConstants.Shark_Large][GenderConstants.Female] = "0";
            heights[CreatureConstants.Shark_Large][GenderConstants.Male] = "0";
            heights[CreatureConstants.Shark_Large][CreatureConstants.Shark_Large] = "0";
            //Source: https://www.dimensions.com/element/great-white-shark
            heights[CreatureConstants.Shark_Huge][GenderConstants.Female] = "0";
            heights[CreatureConstants.Shark_Huge][GenderConstants.Male] = "0";
            heights[CreatureConstants.Shark_Huge][CreatureConstants.Shark_Huge] = "0";
            heights[CreatureConstants.Shark_Dire][GenderConstants.Female] = "0";
            heights[CreatureConstants.Shark_Dire][GenderConstants.Male] = "0";
            heights[CreatureConstants.Shark_Dire][CreatureConstants.Shark_Dire] = "0";
            //Source: https://www.d20srd.org/srd/monsters/shieldGuardian.htm
            heights[CreatureConstants.ShieldGuardian][GenderConstants.Agender] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.ShieldGuardian][CreatureConstants.ShieldGuardian] = GetMultiplierFromAverage(9 * 12);
            //Source: https://www.d20srd.org/srd/monsters/shockerLizard.htm
            heights[CreatureConstants.ShockerLizard][GenderConstants.Female] = GetBaseFromAverage(12);
            heights[CreatureConstants.ShockerLizard][GenderConstants.Male] = GetBaseFromAverage(12);
            heights[CreatureConstants.ShockerLizard][CreatureConstants.ShockerLizard] = GetMultiplierFromAverage(12);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/shrieker-species
            heights[CreatureConstants.Shrieker][GenderConstants.Agender] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.Shrieker][CreatureConstants.Shrieker] = GetMultiplierFromAverage(3 * 12);
            //Source: https://www.d20srd.org/srd/monsters/skum.htm Copying from Human
            heights[CreatureConstants.Skum][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Skum][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Skum][CreatureConstants.Skum] = "2d10";
            //Source: https://forgottenrealms.fandom.com/wiki/Blue_slaad
            heights[CreatureConstants.Slaad_Blue][GenderConstants.Agender] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Slaad_Blue][CreatureConstants.Slaad_Blue] = GetMultiplierFromAverage(10 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Red_slaad
            heights[CreatureConstants.Slaad_Red][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Slaad_Red][CreatureConstants.Slaad_Red] = GetMultiplierFromAverage(8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Green_slaad
            heights[CreatureConstants.Slaad_Green][GenderConstants.Agender] = GetBaseFromAverage(7 * 12);
            heights[CreatureConstants.Slaad_Green][CreatureConstants.Slaad_Green] = GetMultiplierFromAverage(7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Gray_slaad
            heights[CreatureConstants.Slaad_Gray][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Slaad_Gray][CreatureConstants.Slaad_Gray] = GetMultiplierFromAverage(6 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Gray_slaad
            heights[CreatureConstants.Slaad_Death][GenderConstants.Agender] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Slaad_Death][CreatureConstants.Slaad_Death] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.dimensions.com/element/green-tree-python-morelia-viridis
            heights[CreatureConstants.Snake_Constrictor][GenderConstants.Female] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Snake_Constrictor][GenderConstants.Male] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Snake_Constrictor][CreatureConstants.Snake_Constrictor] = GetMultiplierFromRange(1, 2);
            //Source: https://www.dimensions.com/element/burmese-python-python-bivittatus
            heights[CreatureConstants.Snake_Constrictor_Giant][GenderConstants.Female] = GetBaseFromRange(3, 9);
            heights[CreatureConstants.Snake_Constrictor_Giant][GenderConstants.Male] = GetBaseFromRange(3, 9);
            heights[CreatureConstants.Snake_Constrictor_Giant][CreatureConstants.Snake_Constrictor_Giant] = GetMultiplierFromRange(3, 9);
            //Source: https://www.dimensions.com/element/ribbon-snake-thamnophis-saurita 
            heights[CreatureConstants.Snake_Viper_Tiny][GenderConstants.Female] = GetBaseFromAverage(1);
            heights[CreatureConstants.Snake_Viper_Tiny][GenderConstants.Male] = GetBaseFromAverage(1);
            heights[CreatureConstants.Snake_Viper_Tiny][CreatureConstants.Snake_Viper_Tiny] = GetMultiplierFromAverage(1);
            //Source: https://www.dimensions.com/element/copperhead-agkistrodon-contortrix 
            heights[CreatureConstants.Snake_Viper_Small][GenderConstants.Female] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Snake_Viper_Small][GenderConstants.Male] = GetBaseFromRange(1, 2);
            heights[CreatureConstants.Snake_Viper_Small][CreatureConstants.Snake_Viper_Small] = GetMultiplierFromRange(1, 2);
            //Source: https://www.dimensions.com/element/western-diamondback-rattlesnake-crotalus-atrox 
            heights[CreatureConstants.Snake_Viper_Medium][GenderConstants.Female] = GetBaseFromRange(1, 3);
            heights[CreatureConstants.Snake_Viper_Medium][GenderConstants.Male] = GetBaseFromRange(1, 3);
            heights[CreatureConstants.Snake_Viper_Medium][CreatureConstants.Snake_Viper_Medium] = GetMultiplierFromRange(1, 3);
            //Source: https://www.dimensions.com/element/black-mamba-dendroaspis-polylepis 
            heights[CreatureConstants.Snake_Viper_Large][GenderConstants.Female] = GetBaseFromRange(2, 4);
            heights[CreatureConstants.Snake_Viper_Large][GenderConstants.Male] = GetBaseFromRange(2, 4);
            heights[CreatureConstants.Snake_Viper_Large][CreatureConstants.Snake_Viper_Large] = GetMultiplierFromRange(2, 4);
            //Source: https://www.dimensions.com/element/king-cobra-ophiophagus-hannah 
            heights[CreatureConstants.Snake_Viper_Huge][GenderConstants.Female] = GetBaseFromRange(3, 6);
            heights[CreatureConstants.Snake_Viper_Huge][GenderConstants.Male] = GetBaseFromRange(3, 6);
            heights[CreatureConstants.Snake_Viper_Huge][CreatureConstants.Snake_Viper_Huge] = GetMultiplierFromRange(3, 6);
            //Source: https://www.d20srd.org/srd/monsters/spectre.htm Copying from Human
            heights[CreatureConstants.Spectre][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Spectre][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Spectre][CreatureConstants.Spectre] = "2d10";
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Tiny
            heights[CreatureConstants.Spider_Monstrous_Hunter_Tiny][GenderConstants.Female] = GetBaseFromAverage(2);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Tiny][GenderConstants.Male] = GetBaseFromAverage(2);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Tiny][CreatureConstants.Spider_Monstrous_Hunter_Tiny] = GetMultiplierFromAverage(2);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Small
            heights[CreatureConstants.Spider_Monstrous_Hunter_Small][GenderConstants.Female] = GetBaseFromAverage(3);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Small][GenderConstants.Male] = GetBaseFromAverage(3);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Small][CreatureConstants.Spider_Monstrous_Hunter_Small] = GetMultiplierFromAverage(3);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            heights[CreatureConstants.Spider_Monstrous_Hunter_Medium][GenderConstants.Female] = GetBaseFromAverage(6);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Medium][GenderConstants.Male] = GetBaseFromAverage(6);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Medium][CreatureConstants.Spider_Monstrous_Hunter_Medium] = GetMultiplierFromAverage(6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large
            heights[CreatureConstants.Spider_Monstrous_Hunter_Large][GenderConstants.Female] = GetBaseFromAverage(18);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Large][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Large][CreatureConstants.Spider_Monstrous_Hunter_Large] = GetMultiplierFromAverage(18);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Huge
            heights[CreatureConstants.Spider_Monstrous_Hunter_Huge][GenderConstants.Female] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Huge][GenderConstants.Male] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Huge][CreatureConstants.Spider_Monstrous_Hunter_Huge] = GetMultiplierFromAverage(2 * 12 + 6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Gargantuan
            heights[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan][CreatureConstants.Spider_Monstrous_Hunter_Gargantuan] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Colossal
            heights[CreatureConstants.Spider_Monstrous_Hunter_Colossal][GenderConstants.Female] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Colossal][GenderConstants.Male] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Spider_Monstrous_Hunter_Colossal][CreatureConstants.Spider_Monstrous_Hunter_Colossal] = GetMultiplierFromAverage(10 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Tiny
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Tiny][GenderConstants.Female] = GetBaseFromAverage(2);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Tiny][GenderConstants.Male] = GetBaseFromAverage(2);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Tiny][CreatureConstants.Spider_Monstrous_WebSpinner_Tiny] = GetMultiplierFromAverage(2);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Small
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Small][GenderConstants.Female] = GetBaseFromAverage(3);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Small][GenderConstants.Male] = GetBaseFromAverage(3);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Small][CreatureConstants.Spider_Monstrous_WebSpinner_Small] = GetMultiplierFromAverage(3);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Medium][GenderConstants.Female] = GetBaseFromAverage(6);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Medium][GenderConstants.Male] = GetBaseFromAverage(6);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Medium][CreatureConstants.Spider_Monstrous_WebSpinner_Medium] = GetMultiplierFromAverage(6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Large][GenderConstants.Female] = GetBaseFromAverage(18);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Large][GenderConstants.Male] = GetBaseFromAverage(18);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Large][CreatureConstants.Spider_Monstrous_WebSpinner_Large] = GetMultiplierFromAverage(18);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Huge
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Huge][GenderConstants.Female] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Huge][GenderConstants.Male] = GetBaseFromAverage(2 * 12 + 6);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Huge][CreatureConstants.Spider_Monstrous_WebSpinner_Huge] = GetMultiplierFromAverage(2 * 12 + 6);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Gargantuan
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan][GenderConstants.Female] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan][CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Colossal
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal][GenderConstants.Female] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal][GenderConstants.Male] = GetBaseFromAverage(10 * 12);
            heights[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal][CreatureConstants.Spider_Monstrous_WebSpinner_Colossal] = GetMultiplierFromAverage(10 * 12);
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            heights[CreatureConstants.Spider_Swarm][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Spider_Swarm][CreatureConstants.Spider_Swarm] = "0";
            //Source: https://www.d20srd.org/srd/monsters/spiderEater.htm
            heights[CreatureConstants.SpiderEater][GenderConstants.Female] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.SpiderEater][GenderConstants.Male] = GetBaseFromAverage(4 * 12);
            heights[CreatureConstants.SpiderEater][CreatureConstants.SpiderEater] = GetMultiplierFromAverage(4 * 12);
            //Source: https://www.dimensions.com/element/humboldt-squid-dosidicus-gigas
            heights[CreatureConstants.Squid][GenderConstants.Female] = GetBaseFromRange(59, 98);
            heights[CreatureConstants.Squid][GenderConstants.Male] = GetBaseFromRange(59, 98);
            heights[CreatureConstants.Squid][CreatureConstants.Squid] = GetMultiplierFromRange(59, 98);
            //Source: https://www.d20srd.org/srd/monsters/squidGiant.htm
            heights[CreatureConstants.Squid_Giant][GenderConstants.Female] = GetBaseFromUpTo(20 * 20);
            heights[CreatureConstants.Squid_Giant][GenderConstants.Male] = GetBaseFromUpTo(20 * 20);
            heights[CreatureConstants.Squid_Giant][CreatureConstants.Squid_Giant] = GetMultiplierFromUpTo(20 * 20);
            //Source: https://www.d20srd.org/srd/monsters/giantStagBeetle.htm
            //https://www.dimensions.com/element/hercules-beetle-dynastes-hercules scale up: [.47,1.42]*10*12/[2.36,7.09] = [24,24]
            heights[CreatureConstants.StagBeetle_Giant][GenderConstants.Female] = GetBaseFromAverage(24);
            heights[CreatureConstants.StagBeetle_Giant][GenderConstants.Male] = GetBaseFromAverage(24);
            heights[CreatureConstants.StagBeetle_Giant][CreatureConstants.StagBeetle_Giant] = GetMultiplierFromAverage(24);
            heights[CreatureConstants.Stirge][GenderConstants.Female] = "0";
            heights[CreatureConstants.Stirge][GenderConstants.Male] = "0";
            heights[CreatureConstants.Stirge][CreatureConstants.Stirge] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Succubus
            heights[CreatureConstants.Succubus][GenderConstants.Female] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Succubus][GenderConstants.Male] = GetBaseFromAverage(6 * 12);
            heights[CreatureConstants.Succubus][CreatureConstants.Succubus] = GetMultiplierFromAverage(6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/tarrasque.htm
            heights[CreatureConstants.Tarrasque][GenderConstants.Agender] = GetBaseFromAverage(50 * 12);
            heights[CreatureConstants.Tarrasque][CreatureConstants.Tarrasque] = GetMultiplierFromAverage(50 * 12);
            //Source: https://www.d20srd.org/srd/monsters/tendriculos.htm
            heights[CreatureConstants.Tendriculos][GenderConstants.Agender] = GetBaseFromAverage(15 * 12);
            heights[CreatureConstants.Tendriculos][CreatureConstants.Tendriculos] = GetMultiplierFromAverage(15 * 12);
            //Source: https://www.d20srd.org/srd/monsters/thoqqua.htm
            heights[CreatureConstants.Thoqqua][GenderConstants.Agender] = GetBaseFromAverage(12);
            heights[CreatureConstants.Thoqqua][CreatureConstants.Thoqqua] = GetMultiplierFromAverage(12);
            //Source: https://forgottenrealms.fandom.com/wiki/Tiefling
            heights[CreatureConstants.Tiefling][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 11, 6 * 12);
            heights[CreatureConstants.Tiefling][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 11, 6 * 12);
            heights[CreatureConstants.Tiefling][CreatureConstants.Tiefling] = GetMultiplierFromRange(4 * 12 + 11, 6 * 12);
            //Source: https://www.dimensions.com/element/bengal-tiger
            heights[CreatureConstants.Tiger][GenderConstants.Female] = GetBaseFromRange(34, 45);
            heights[CreatureConstants.Tiger][GenderConstants.Male] = GetBaseFromRange(34, 45);
            heights[CreatureConstants.Tiger][CreatureConstants.Tiger] = GetMultiplierFromRange(34, 45);
            //Scaled up from Tiger, x2 based on length
            heights[CreatureConstants.Tiger_Dire][GenderConstants.Female] = GetBaseFromRange(34 * 2, 45 * 2);
            heights[CreatureConstants.Tiger_Dire][GenderConstants.Male] = GetBaseFromRange(34 * 2, 45 * 2);
            heights[CreatureConstants.Tiger_Dire][CreatureConstants.Tiger_Dire] = GetMultiplierFromRange(34 * 2, 45 * 2);
            //Source: https://www.d20srd.org/srd/monsters/titan.htm
            heights[CreatureConstants.Titan][GenderConstants.Female] = GetBaseFromAverage(25 * 12);
            heights[CreatureConstants.Titan][GenderConstants.Male] = GetBaseFromAverage(25 * 12);
            heights[CreatureConstants.Titan][CreatureConstants.Titan] = GetMultiplierFromAverage(25 * 12);
            //Source: https://www.dimensions.com/element/common-toad-bufo-bufo
            heights[CreatureConstants.Toad][GenderConstants.Female] = GetBaseFromRange(2, 3);
            heights[CreatureConstants.Toad][GenderConstants.Male] = GetBaseFromRange(2, 3);
            heights[CreatureConstants.Toad][CreatureConstants.Toad] = GetMultiplierFromRange(2, 3);
            heights[CreatureConstants.Tojanida_Juvenile][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Tojanida_Juvenile][CreatureConstants.Tojanida_Juvenile] = "0";
            heights[CreatureConstants.Tojanida_Adult][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Tojanida_Adult][CreatureConstants.Tojanida_Adult] = "0";
            heights[CreatureConstants.Tojanida_Elder][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Tojanida_Elder][CreatureConstants.Tojanida_Elder] = "0";
            //Source: https://www.d20srd.org/srd/monsters/treant.htm
            heights[CreatureConstants.Treant][GenderConstants.Female] = GetBaseFromAverage(30 * 12);
            heights[CreatureConstants.Treant][GenderConstants.Male] = GetBaseFromAverage(30 * 12);
            heights[CreatureConstants.Treant][CreatureConstants.Treant] = GetMultiplierFromAverage(30 * 12);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Triceratops
            heights[CreatureConstants.Triceratops][GenderConstants.Female] = GetBaseFromAverage(157);
            heights[CreatureConstants.Triceratops][GenderConstants.Male] = GetBaseFromAverage(157);
            heights[CreatureConstants.Triceratops][CreatureConstants.Triceratops] = GetMultiplierFromAverage(157);
            //Source: https://www.d20srd.org/srd/monsters/triton.htm Copying from Human
            heights[CreatureConstants.Triton][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Triton][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Triton][CreatureConstants.Triton] = "2d10";
            //Source: https://forgottenrealms.fandom.com/wiki/Troglodyte
            heights[CreatureConstants.Troglodyte][GenderConstants.Female] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Troglodyte][GenderConstants.Male] = GetBaseFromRange(5 * 12, 6 * 12);
            heights[CreatureConstants.Troglodyte][CreatureConstants.Troglodyte] = GetMultiplierFromRange(5 * 12, 6 * 12);
            //Source: https://www.d20srd.org/srd/monsters/troll.htm Female "slightly larger than males", so 110%
            heights[CreatureConstants.Troll][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Troll][GenderConstants.Female] = GetBaseFromAverage(119, 9 * 12);
            heights[CreatureConstants.Troll][CreatureConstants.Troll] = GetMultiplierFromAverage(9 * 12);
            heights[CreatureConstants.Troll_Scrag][GenderConstants.Male] = GetBaseFromAverage(9 * 12);
            heights[CreatureConstants.Troll_Scrag][GenderConstants.Female] = GetBaseFromAverage(119, 9 * 12);
            heights[CreatureConstants.Troll_Scrag][CreatureConstants.Troll_Scrag] = GetMultiplierFromAverage(9 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Trumpet_archon
            heights[CreatureConstants.TrumpetArchon][GenderConstants.Female] = GetBaseFromRange(6 * 12 + 1, 7 * 12 + 4);
            heights[CreatureConstants.TrumpetArchon][GenderConstants.Male] = GetBaseFromRange(6 * 12 + 6, 7 * 12 + 9);
            heights[CreatureConstants.TrumpetArchon][CreatureConstants.TrumpetArchon] = GetMultiplierFromRange(6 * 12 + 6, 7 * 12 + 9);
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Tyrannosaurus
            heights[CreatureConstants.Tyrannosaurus][GenderConstants.Male] = GetBaseFromAverage(232);
            heights[CreatureConstants.Tyrannosaurus][GenderConstants.Female] = GetBaseFromAverage(232);
            heights[CreatureConstants.Tyrannosaurus][CreatureConstants.Tyrannosaurus] = GetMultiplierFromAverage(232);
            //Source: https://forgottenrealms.fandom.com/wiki/Umber_hulk
            heights[CreatureConstants.UmberHulk][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 6, 5 * 12);
            heights[CreatureConstants.UmberHulk][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 6, 5 * 12);
            heights[CreatureConstants.UmberHulk][CreatureConstants.UmberHulk] = GetMultiplierFromRange(4 * 12 + 6, 5 * 12);
            //Scale up based on length, X2
            heights[CreatureConstants.UmberHulk_TrulyHorrid][GenderConstants.Female] = GetBaseFromRange(9 * 12, 10 * 12);
            heights[CreatureConstants.UmberHulk_TrulyHorrid][GenderConstants.Male] = GetBaseFromRange(9 * 12, 10 * 12);
            heights[CreatureConstants.UmberHulk_TrulyHorrid][CreatureConstants.UmberHulk_TrulyHorrid] = GetMultiplierFromRange(9 * 12, 10 * 12);
            //Source: https://www.d20srd.org/srd/monsters/unicorn.htm Females "slightly smaller", so 90%
            heights[CreatureConstants.Unicorn][GenderConstants.Female] = GetBaseFromAverage(54, 5 * 12);
            heights[CreatureConstants.Unicorn][GenderConstants.Male] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Unicorn][CreatureConstants.Unicorn] = GetMultiplierFromAverage(5 * 12);
            //Source: https://www.d20srd.org/srd/monsters/vampire.htm#vampireSpawn Copying from Human
            heights[CreatureConstants.VampireSpawn][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.VampireSpawn][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.VampireSpawn][CreatureConstants.VampireSpawn] = "2d10";
            //Source: https://www.d20srd.org/srd/monsters/vargouille.htm
            heights[CreatureConstants.Vargouille][GenderConstants.Agender] = GetBaseFromAverage(18);
            heights[CreatureConstants.Vargouille][CreatureConstants.Vargouille] = GetMultiplierFromAverage(18);
            //Source: https://www.worldanvil.com/w/faerun-tatortotzke/a/violet-fungus-species
            heights[CreatureConstants.VioletFungus][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 7 * 12);
            heights[CreatureConstants.VioletFungus][CreatureConstants.VioletFungus] = GetMultiplierFromRange(4 * 12, 7 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Vrock
            heights[CreatureConstants.Vrock][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Vrock][CreatureConstants.Vrock] = GetMultiplierFromAverage(8 * 12);
            //Source: https://www.dimensions.com/element/red-paper-wasp-polistes-carolina
            //https://forgottenrealms.fandom.com/wiki/Giant_wasp scale up: [.23,.33]*5*12/[.94,1.26] = [14,16]
            heights[CreatureConstants.Wasp_Giant][GenderConstants.Male] = GetBaseFromRange(14, 16);
            heights[CreatureConstants.Wasp_Giant][CreatureConstants.Wasp_Giant] = GetMultiplierFromRange(14, 16);
            //Source: https://www.dimensions.com/element/least-weasel-mustela-nivalis
            heights[CreatureConstants.Weasel][GenderConstants.Female] = GetBaseFromRange(2, 3);
            heights[CreatureConstants.Weasel][GenderConstants.Male] = GetBaseFromRange(2, 3);
            heights[CreatureConstants.Weasel][CreatureConstants.Weasel] = GetMultiplierFromRange(2, 3);
            //Scaled up from weasel, x15 based on length
            heights[CreatureConstants.Weasel_Dire][GenderConstants.Female] = GetBaseFromRange(30, 45);
            heights[CreatureConstants.Weasel_Dire][GenderConstants.Male] = GetBaseFromRange(30, 45);
            heights[CreatureConstants.Weasel_Dire][CreatureConstants.Weasel_Dire] = GetMultiplierFromRange(30, 45);
            //Source: https://www.dimensions.com/element/humpback-whale-megaptera-novaeangliae
            heights[CreatureConstants.Whale_Baleen][GenderConstants.Female] = GetBaseFromRange(8 * 12, 9 * 12 + 8);
            heights[CreatureConstants.Whale_Baleen][GenderConstants.Male] = GetBaseFromRange(8 * 12, 9 * 12 + 8);
            heights[CreatureConstants.Whale_Baleen][CreatureConstants.Whale_Baleen] = GetMultiplierFromRange(8 * 12, 9 * 12 + 8);
            //Source: https://www.dimensions.com/element/sperm-whale-physeter-macrocephalus
            heights[CreatureConstants.Whale_Cachalot][GenderConstants.Female] = GetBaseFromRange(6 * 12 + 9, 11 * 12);
            heights[CreatureConstants.Whale_Cachalot][GenderConstants.Male] = GetBaseFromRange(6 * 12 + 9, 11 * 12);
            heights[CreatureConstants.Whale_Cachalot][CreatureConstants.Whale_Cachalot] = GetMultiplierFromRange(6 * 12 + 9, 11 * 12);
            //Source: https://www.dimensions.com/element/orca-killer-whale-orcinus-orca
            heights[CreatureConstants.Whale_Orca][GenderConstants.Female] = GetBaseFromRange(5 * 12 + 3, 7 * 12 + 6);
            heights[CreatureConstants.Whale_Orca][GenderConstants.Male] = GetBaseFromRange(5 * 12 + 3, 7 * 12 + 6);
            heights[CreatureConstants.Whale_Orca][CreatureConstants.Whale_Orca] = GetMultiplierFromRange(5 * 12 + 3, 7 * 12 + 6);
            //Source: https://www.d20srd.org/srd/monsters/wight.htm Copy from Human
            heights[CreatureConstants.Wight][GenderConstants.Female] = "4*12+5";
            heights[CreatureConstants.Wight][GenderConstants.Male] = "4*12+10";
            heights[CreatureConstants.Wight][CreatureConstants.Wight] = "2d10";
            //Source: https://www.d20srd.org/srd/monsters/willOWisp.htm
            heights[CreatureConstants.WillOWisp][GenderConstants.Agender] = GetBaseFromAverage(12);
            heights[CreatureConstants.WillOWisp][CreatureConstants.WillOWisp] = GetMultiplierFromAverage(12);
            //Source: https://www.d20srd.org/srd/monsters/winterWolf.htm
            heights[CreatureConstants.WinterWolf][GenderConstants.Female] = GetBaseFromAverage(4 * 12 + 6);
            heights[CreatureConstants.WinterWolf][GenderConstants.Male] = GetBaseFromAverage(4 * 12 + 6);
            heights[CreatureConstants.WinterWolf][CreatureConstants.WinterWolf] = GetMultiplierFromAverage(4 * 12 + 6);
            //Source: https://www.dimensions.com/element/gray-wolf
            heights[CreatureConstants.Wolf][GenderConstants.Female] = GetBaseFromRange(26, 33);
            heights[CreatureConstants.Wolf][GenderConstants.Male] = GetBaseFromRange(26, 33);
            heights[CreatureConstants.Wolf][CreatureConstants.Wolf] = GetMultiplierFromRange(26, 33);
            //Scaled up from Wolf, x2 based on length
            heights[CreatureConstants.Wolf_Dire][GenderConstants.Female] = GetBaseFromRange(26 * 2, 33 * 2);
            heights[CreatureConstants.Wolf_Dire][GenderConstants.Male] = GetBaseFromRange(26 * 2, 33 * 2);
            heights[CreatureConstants.Wolf_Dire][CreatureConstants.Wolf_Dire] = GetMultiplierFromRange(26 * 2, 33 * 2);
            //Source: https://www.dimensions.com/element/wolverine-gulo-gulo
            heights[CreatureConstants.Wolverine][GenderConstants.Female] = GetBaseFromRange(14, 21);
            heights[CreatureConstants.Wolverine][GenderConstants.Male] = GetBaseFromRange(14, 21);
            heights[CreatureConstants.Wolverine][CreatureConstants.Wolverine] = GetMultiplierFromRange(14, 21);
            //Scaled up from Wolverine, x4 based on length
            heights[CreatureConstants.Wolverine_Dire][GenderConstants.Female] = GetBaseFromRange(14 * 4, 21 * 4);
            heights[CreatureConstants.Wolverine_Dire][GenderConstants.Male] = GetBaseFromRange(14 * 4, 21 * 4);
            heights[CreatureConstants.Wolverine_Dire][CreatureConstants.Wolverine_Dire] = GetMultiplierFromRange(14 * 4, 21 * 4);
            //Source: https://www.d20srd.org/srd/monsters/worg.htm
            heights[CreatureConstants.Worg][GenderConstants.Female] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.Worg][GenderConstants.Male] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.Worg][CreatureConstants.Worg] = GetMultiplierFromAverage(3 * 12);
            //Source: https://www.d20srd.org/srd/monsters/wraith.htm W:Human, DW:Ogre
            heights[CreatureConstants.Wraith][GenderConstants.Agender] = "4*12+10";
            heights[CreatureConstants.Wraith][CreatureConstants.Wraith] = "2d10";
            heights[CreatureConstants.Wraith_Dread][GenderConstants.Agender] = "96";
            heights[CreatureConstants.Wraith_Dread][CreatureConstants.Wraith_Dread] = "2d12";
            //Source: https://forgottenrealms.fandom.com/wiki/Wyvern
            heights[CreatureConstants.Wyvern][GenderConstants.Female] = "0";
            heights[CreatureConstants.Wyvern][GenderConstants.Male] = "0";
            heights[CreatureConstants.Wyvern][CreatureConstants.Wyvern] = "0";
            //Source: https://www.d20srd.org/srd/monsters/xill.htm
            heights[CreatureConstants.Xill][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 5 * 12);
            heights[CreatureConstants.Xill][CreatureConstants.Xill] = GetMultiplierFromRange(4 * 12, 5 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Xorn
            heights[CreatureConstants.Xorn_Minor][GenderConstants.Agender] = GetBaseFromAverage(3 * 12);
            heights[CreatureConstants.Xorn_Minor][CreatureConstants.Xorn_Minor] = GetMultiplierFromAverage(3 * 12);
            heights[CreatureConstants.Xorn_Average][GenderConstants.Agender] = GetBaseFromAverage(5 * 12);
            heights[CreatureConstants.Xorn_Average][CreatureConstants.Xorn_Average] = GetMultiplierFromAverage(5 * 12);
            heights[CreatureConstants.Xorn_Elder][GenderConstants.Agender] = GetBaseFromAverage(8 * 12);
            heights[CreatureConstants.Xorn_Elder][CreatureConstants.Xorn_Elder] = GetMultiplierFromAverage(8 * 12);
            //Source: https://forgottenrealms.fandom.com/wiki/Yeth_hound
            heights[CreatureConstants.YethHound][GenderConstants.Agender] = GetBaseFromRange(4 * 12, 5 * 12);
            heights[CreatureConstants.YethHound][CreatureConstants.YethHound] = GetMultiplierFromRange(4 * 12, 5 * 12);
            //Source: https://www.d20srd.org/srd/monsters/yrthak.htm
            heights[CreatureConstants.Yrthak][GenderConstants.Agender] = "0";
            heights[CreatureConstants.Yrthak][CreatureConstants.Yrthak] = "0";
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_pureblood
            heights[CreatureConstants.YuanTi_Pureblood][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Pureblood][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Pureblood][CreatureConstants.YuanTi_Pureblood] = GetMultiplierFromRange(4 * 12 + 7, 6 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_malison
            heights[CreatureConstants.YuanTi_Halfblood_SnakeArms][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeArms][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeArms][CreatureConstants.YuanTi_Halfblood_SnakeArms] = GetMultiplierFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeHead][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeHead][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeHead][CreatureConstants.YuanTi_Halfblood_SnakeHead] = GetMultiplierFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeTail][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeTail][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeTail][CreatureConstants.YuanTi_Halfblood_SnakeTail] = GetMultiplierFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs][CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs] =
                GetMultiplierFromRange(4 * 12 + 7, 6 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_abomination (assuming height based on other yuan-ti)
            heights[CreatureConstants.YuanTi_Abomination][GenderConstants.Female] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Abomination][GenderConstants.Male] = GetBaseFromRange(4 * 12 + 7, 6 * 12 + 6);
            heights[CreatureConstants.YuanTi_Abomination][CreatureConstants.YuanTi_Abomination] = GetMultiplierFromRange(4 * 12 + 7, 6 * 12 + 6);
            //Source: https://forgottenrealms.fandom.com/wiki/Zelekhut using centaur stats
            heights[CreatureConstants.Zelekhut][GenderConstants.Agender] = GetBaseFromRange(7 * 12, 9 * 12);
            heights[CreatureConstants.Zelekhut][CreatureConstants.Zelekhut] = GetMultiplierFromRange(7 * 12, 9 * 12);

            var templates = CreatureConstants.Templates.GetAll();
            const string BelowAverage = "-1";
            const string NoChange = "0";
            const string AboveAverage = "1";
            foreach (var template in templates)
            {
                heights[template] = [];
                heights[template][template] = NoChange;
            }

            heights[CreatureConstants.Templates.Lycanthrope_Rat_Afflicted][CreatureConstants.Templates.Lycanthrope_Rat_Afflicted] = BelowAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Rat_Natural][CreatureConstants.Templates.Lycanthrope_Rat_Natural] = BelowAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted][CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted] = BelowAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural][CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural] = BelowAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted][CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted] = AboveAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Tiger_Natural][CreatureConstants.Templates.Lycanthrope_Tiger_Natural] = AboveAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Afflicted][CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Afflicted] = AboveAverage;
            heights[CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Natural][CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Natural] = AboveAverage;

            return heights;
        }

        private static string GetBaseFromAverage(int average) => GetBaseFromRange(average * 9 / 10, average * 11 / 10);
        private static string GetBaseFromAverage(int average, int altAverage) => GetBaseFromRange(average - altAverage / 10, average + altAverage / 10);
        private static string GetBaseFromUpTo(int upTo) => GetBaseFromRange(upTo * 9 / 11, upTo);
        private static string GetBaseFromAtLeast(int atLeast) => GetBaseFromRange(atLeast, atLeast * 11 / 9);

        private static string GetMultiplierFromAverage(int average) => GetMultiplierFromRange(average * 9 / 10, average * 11 / 10);
        private static string GetMultiplierFromUpTo(int upTo) => GetMultiplierFromRange(upTo * 9 / 11, upTo);
        private static string GetMultiplierFromAtLeast(int atLeast) => GetMultiplierFromRange(atLeast, atLeast * 11 / 9);

        private static string GetBaseFromRange(int lower, int upper) => GetFromRange(lower, upper, 1);

        private static string GetMultiplierFromRange(int lower, int upper) => GetFromRange(lower, upper, 0);

        private static string GetFromRange(int lower, int upper, int index)
        {
            lower = Math.Max(1, lower);

            var roll = RollHelper.GetRollWithFewestDice(lower, upper);
            var sections = roll.Split('+');
            if (sections.Length - 1 < index)
                return "0";

            return sections[index];
        }

        [TestCase(CreatureConstants.Achaierai, GenderConstants.Male, 15 * 12)]
        [TestCase(CreatureConstants.Achaierai, GenderConstants.Female, 15 * 12)]
        [TestCase(CreatureConstants.Ape_Dire, GenderConstants.Male, 9 * 12)]
        [TestCase(CreatureConstants.Ape_Dire, GenderConstants.Female, 9 * 12)]
        [TestCase(CreatureConstants.Azer, GenderConstants.Agender, 5 * 12)]
        [TestCase(CreatureConstants.Balor, GenderConstants.Agender, 12 * 12)]
        [TestCase(CreatureConstants.DisplacerBeast, GenderConstants.Male, 5 * 12)]
        [TestCase(CreatureConstants.DisplacerBeast, GenderConstants.Female, 5 * 12)]
        [TestCase(CreatureConstants.Lion, GenderConstants.Male, 3 * 12 + 4)]
        [TestCase(CreatureConstants.Lion, GenderConstants.Female, 2 * 12 + 10, 3 * 12 + 4)]
        //https://forgottenrealms.fandom.com/wiki/Locathah
        [TestCase(CreatureConstants.Locathah, GenderConstants.Male, 5 * 12)]
        [TestCase(CreatureConstants.Locathah, GenderConstants.Female, 5 * 12)]
        //Based off of Lion
        [TestCase(CreatureConstants.Manticore, GenderConstants.Male, 3 * 12 + 4)]
        [TestCase(CreatureConstants.Manticore, GenderConstants.Female, 2 * 12 + 10, 3 * 12 + 4)]
        //https://forgottenrealms.fandom.com/wiki/Minotaur
        [TestCase(CreatureConstants.Minotaur, GenderConstants.Male, 9 * 12)]
        [TestCase(CreatureConstants.Minotaur, GenderConstants.Female, 7 * 12, 9 * 12)]
        //https://forgottenrealms.fandom.com/wiki/Oni_mage
        [TestCase(CreatureConstants.OgreMage, GenderConstants.Male, 10 * 12)]
        [TestCase(CreatureConstants.OgreMage, GenderConstants.Female, 10 * 12)]
        [TestCase(CreatureConstants.Salamander_Noble, GenderConstants.Agender, 20 * 12)]
        [TestCase(CreatureConstants.Salamander_Average, GenderConstants.Agender, 10 * 12)]
        [TestCase(CreatureConstants.Salamander_Flamebrother, GenderConstants.Agender, 5 * 12)]
        [TestCase(CreatureConstants.Troll, GenderConstants.Male, 9 * 12)]
        [TestCase(CreatureConstants.Troll, GenderConstants.Female, 9 * 12 * 11 / 10, 9 * 12)]
        [TestCase(CreatureConstants.Troll_Scrag, GenderConstants.Male, 9 * 12)]
        [TestCase(CreatureConstants.Troll_Scrag, GenderConstants.Female, 9 * 12 * 11 / 10, 9 * 12)]
        [TestCase(CreatureConstants.Unicorn, GenderConstants.Male, 5 * 12)]
        [TestCase(CreatureConstants.Unicorn, GenderConstants.Female, 5 * 12 * 9 / 10, 5 * 12)]
        public void RollCalculationsAreAccurate_FromAverage(string creature, string gender, int average, int? altAverage = null)
        {
            Assert.That(creatureHeights, Contains.Key(creature));
            Assert.That(creatureHeights[creature], Contains.Key(creature).And.ContainKey(gender));

            var baseHeight = dice.Roll(creatureHeights[creature][gender]).AsSum();
            var multiplierMin = dice.Roll(creatureHeights[creature][creature]).AsPotentialMinimum();
            var multiplierAvg = dice.Roll(creatureHeights[creature][creature]).AsPotentialAverage();
            var multiplierMax = dice.Roll(creatureHeights[creature][creature]).AsPotentialMaximum();

            var lower = average * 9 / 10;
            var upper = average * 11 / 10;

            if (altAverage.HasValue)
            {
                lower = average - altAverage.Value / 10;
                upper = average + altAverage.Value / 10;
            }

            var theoreticalRoll = RollHelper.GetRollWithFewestDice(lower, upper);

            Assert.That(baseHeight + multiplierMin, Is.EqualTo(lower).Within(1), $"Min (-10%); Theoretical: {theoreticalRoll}");
            Assert.That(baseHeight + multiplierAvg, Is.EqualTo(average).Within(1), $"Average; Theoretical: {theoreticalRoll}");
            Assert.That(baseHeight + multiplierMax, Is.EqualTo(upper).Within(1), $"Max (+10%); Theoretical: {theoreticalRoll}");
        }

        //https://forgottenrealms.fandom.com/wiki/Orc
        [TestCase(CreatureConstants.Orc, GenderConstants.Male, 6 * 12)]
        [TestCase(CreatureConstants.Orc, GenderConstants.Female, 6 * 12)]
        public void RollCalculationsAreAccurate_FromAtLeast(string creature, string gender, int atLeast)
        {
            Assert.That(creatureHeights, Contains.Key(creature));
            Assert.That(creatureHeights[creature], Contains.Key(creature).And.ContainKey(gender));

            var baseHeight = dice.Roll(creatureHeights[creature][gender]).AsSum();
            var multiplierMin = dice.Roll(creatureHeights[creature][creature]).AsPotentialMinimum();
            var multiplierMax = dice.Roll(creatureHeights[creature][creature]).AsPotentialMaximum();

            var lower = atLeast;
            var upper = atLeast * 11 / 9;

            var theoreticalRoll = RollHelper.GetRollWithFewestDice(lower, upper);

            Assert.That(baseHeight + multiplierMin, Is.EqualTo(lower).Within(1), $"Min (-10%); Theoretical: {theoreticalRoll}");
            Assert.That(baseHeight + multiplierMax, Is.EqualTo(upper).Within(1), $"Max (+10%); Theoretical: {theoreticalRoll}");
        }

        //https://forgottenrealms.fandom.com/wiki/Aasimar
        [TestCase(CreatureConstants.Aasimar, GenderConstants.Male, 5 * 12, 6 * 12 + 6)]
        [TestCase(CreatureConstants.Aasimar, GenderConstants.Female, 4 * 12 + 7, 6 * 12 + 1)]
        [TestCase(CreatureConstants.Angel_AstralDeva, GenderConstants.Male, 7 * 12, 7 * 12 + 6)]
        [TestCase(CreatureConstants.Angel_AstralDeva, GenderConstants.Female, 7 * 12, 7 * 12 + 6)]
        [TestCase(CreatureConstants.Angel_Planetar, GenderConstants.Male, 8 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Angel_Planetar, GenderConstants.Female, 8 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Angel_Solar, GenderConstants.Male, 9 * 12, 10 * 12)]
        [TestCase(CreatureConstants.Angel_Solar, GenderConstants.Female, 9 * 12, 10 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny, GenderConstants.Agender, 12, 24)]
        [TestCase(CreatureConstants.AnimatedObject_Small, GenderConstants.Agender, 24, 48)]
        [TestCase(CreatureConstants.AnimatedObject_Medium, GenderConstants.Agender, 48, 96)]
        [TestCase(CreatureConstants.AnimatedObject_Large, GenderConstants.Agender, 8 * 12, 16 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Huge, GenderConstants.Agender, 16 * 12, 32 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan, GenderConstants.Agender, 32 * 12, 64 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal, GenderConstants.Agender, 64 * 12, 128 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden, GenderConstants.Agender, 12 / 2, 24 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wheels_Wooden, GenderConstants.Agender, 24 / 2, 48 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wheels_Wooden, GenderConstants.Agender, 48 / 2, 96 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wheels_Wooden, GenderConstants.Agender, 8 * 12 / 2, 16 * 12 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wheels_Wooden, GenderConstants.Agender, 16 * 12 / 2, 32 * 12 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden, GenderConstants.Agender, 32 * 12 / 2, 64 * 12 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden, GenderConstants.Agender, 64 * 12 / 2, 128 * 12 / 2)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wooden, GenderConstants.Agender, 12, 24)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wooden, GenderConstants.Agender, 24, 48)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wooden, GenderConstants.Agender, 48, 96)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wooden, GenderConstants.Agender, 8 * 12, 16 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wooden, GenderConstants.Agender, 16 * 12, 32 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wooden, GenderConstants.Agender, 32 * 12, 64 * 12)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wooden, GenderConstants.Agender, 64 * 12, 128 * 12)]
        [TestCase(CreatureConstants.Ape, GenderConstants.Male, 5 * 12 + 6, 6 * 12)]
        [TestCase(CreatureConstants.Ape, GenderConstants.Female, 63, 69)]
        [TestCase(CreatureConstants.Babau, GenderConstants.Agender, 6 * 12, 7 * 12)]
        [TestCase(CreatureConstants.Baboon, GenderConstants.Male, 20, 36)]
        [TestCase(CreatureConstants.Baboon, GenderConstants.Female, 20, 36)]
        [TestCase(CreatureConstants.Badger_Dire, GenderConstants.Male, 22, 40)]
        [TestCase(CreatureConstants.Badger_Dire, GenderConstants.Female, 22, 40)]
        [TestCase(CreatureConstants.Bear_Black, GenderConstants.Male, 2 * 12 + 11, 3 * 12 + 5)]
        [TestCase(CreatureConstants.Bear_Black, GenderConstants.Female, 2 * 12 + 6, 3 * 12)]
        [TestCase(CreatureConstants.Bear_Brown, GenderConstants.Male, 3 * 12 + 6, 4 * 12 + 6)]
        [TestCase(CreatureConstants.Bear_Brown, GenderConstants.Female, 3 * 12, 4 * 12)]
        [TestCase(CreatureConstants.Bear_Polar, GenderConstants.Male, 3 * 12 + 7, 5 * 12 + 3)]
        [TestCase(CreatureConstants.Bear_Polar, GenderConstants.Female, 2 * 12 + 8, 4 * 12 + 4)]
        [TestCase(CreatureConstants.Bugbear, GenderConstants.Male, 6 * 12, 8 * 12)]
        [TestCase(CreatureConstants.Bugbear, GenderConstants.Female, 6 * 12, 8 * 12)]
        [TestCase(CreatureConstants.Centaur, GenderConstants.Male, 7 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Centaur, GenderConstants.Female, 7 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Crocodile_Giant, GenderConstants.Male, 10, 30)]
        [TestCase(CreatureConstants.Crocodile_Giant, GenderConstants.Female, 10, 30)]
        [TestCase(CreatureConstants.Dog_Riding, GenderConstants.Male, 22, 30)]
        [TestCase(CreatureConstants.Dog_Riding, GenderConstants.Female, 20, 28)]
        [TestCase(CreatureConstants.Ettin, GenderConstants.Male, 13 * 12, 13 * 12 + 10)]
        [TestCase(CreatureConstants.Ettin, GenderConstants.Female, 12 * 12 + 4, 13 * 12 + 2)]
        [TestCase(CreatureConstants.Ghaele, GenderConstants.Male, 5 * 12 + 2, 7 * 12)]
        [TestCase(CreatureConstants.Ghaele, GenderConstants.Female, 4 * 12 + 7, 6 * 12 + 5)]
        [TestCase(CreatureConstants.Giant_Cloud, GenderConstants.Male, 24 * 12 + 4, 26 * 12 + 8)]
        [TestCase(CreatureConstants.Giant_Cloud, GenderConstants.Female, 22 * 12 + 8, 25 * 12)]
        [TestCase(CreatureConstants.Giant_Fire, GenderConstants.Male, 18 * 12 + 2, 19 * 12 + 8)]
        [TestCase(CreatureConstants.Giant_Fire, GenderConstants.Female, 17 * 12 + 5, 18 * 12 + 11)]
        [TestCase(CreatureConstants.Giant_Frost, GenderConstants.Male, 21 * 12 + 3, 23 * 12 + 6)]
        [TestCase(CreatureConstants.Giant_Frost, GenderConstants.Female, 20 * 12 + 1, 22 * 12 + 4)]
        [TestCase(CreatureConstants.Giant_Hill, GenderConstants.Male, 16 * 12 + 1, 17 * 12)]
        [TestCase(CreatureConstants.Giant_Hill, GenderConstants.Female, 15 * 12 + 5, 16 * 12 + 4)]
        [TestCase(CreatureConstants.Giant_Stone, GenderConstants.Male, 18 * 12 + 2, 19 * 12 + 8)]
        [TestCase(CreatureConstants.Giant_Stone, GenderConstants.Female, 17 * 12 + 5, 18 * 12 + 11)]
        [TestCase(CreatureConstants.Giant_Stone_Elder, GenderConstants.Male, 18 * 12 + 2, 19 * 12 + 8)]
        [TestCase(CreatureConstants.Giant_Stone_Elder, GenderConstants.Female, 17 * 12 + 5, 18 * 12 + 11)]
        //https://forgottenrealms.fandom.com/wiki/Githyanki
        [TestCase(CreatureConstants.Githyanki, GenderConstants.Male, 5 * 12 + 5, 6 * 12 + 11)]
        [TestCase(CreatureConstants.Githyanki, GenderConstants.Female, 5 * 12 + 4, 6 * 12 + 10)]
        //https://forgottenrealms.fandom.com/wiki/Githzerai
        [TestCase(CreatureConstants.Githzerai, GenderConstants.Male, 5 * 12 + 1, 7 * 12)]
        [TestCase(CreatureConstants.Githzerai, GenderConstants.Female, 5 * 12 + 1, 7 * 12)]
        //https://forgottenrealms.fandom.com/wiki/Gnoll
        [TestCase(CreatureConstants.Gnoll, GenderConstants.Male, 7 * 12, 7 * 12 + 6)]
        [TestCase(CreatureConstants.Gnoll, GenderConstants.Female, 7 * 12, 7 * 12 + 6)]
        //https://forgottenrealms.fandom.com/wiki/Goblin
        [TestCase(CreatureConstants.Goblin, GenderConstants.Male, 3 * 12 + 4, 3 * 12 + 8)]
        [TestCase(CreatureConstants.Goblin, GenderConstants.Female, 3 * 12 + 4, 3 * 12 + 8)]
        //https://forgottenrealms.fandom.com/wiki/Hobgoblin
        [TestCase(CreatureConstants.Hobgoblin, GenderConstants.Male, 5 * 12, 6 * 12)]
        [TestCase(CreatureConstants.Hobgoblin, GenderConstants.Female, 5 * 12, 6 * 12)]
        [TestCase(CreatureConstants.Horse_Heavy, GenderConstants.Male, 64, 72)]
        [TestCase(CreatureConstants.Horse_Heavy, GenderConstants.Female, 64, 72)]
        [TestCase(CreatureConstants.Horse_Light, GenderConstants.Male, 57, 61)]
        [TestCase(CreatureConstants.Horse_Light, GenderConstants.Female, 57, 61)]
        //https://forgottenrealms.fandom.com/wiki/Kobold
        [TestCase(CreatureConstants.Kobold, GenderConstants.Male, 2 * 12, 2 * 12 + 6)]
        [TestCase(CreatureConstants.Kobold, GenderConstants.Female, 2 * 12, 2 * 12 + 6)]
        //https://forgottenrealms.fandom.com/wiki/Lizardfolk
        [TestCase(CreatureConstants.Lizardfolk, GenderConstants.Male, 6 * 12, 7 * 12)]
        [TestCase(CreatureConstants.Lizardfolk, GenderConstants.Female, 6 * 12, 7 * 12)]
        [TestCase(CreatureConstants.Nalfeshnee, GenderConstants.Agender, 10 * 12, 20 * 12)]
        //https://forgottenrealms.fandom.com/wiki/Ogre
        [TestCase(CreatureConstants.Ogre, GenderConstants.Male, 10 * 12 + 1, 10 * 12 + 10)]
        [TestCase(CreatureConstants.Ogre, GenderConstants.Female, 9 * 12 + 3, 10 * 12)]
        [TestCase(CreatureConstants.Ogre_Merrow, GenderConstants.Male, 10 * 12 + 1, 10 * 12 + 10)]
        [TestCase(CreatureConstants.Ogre_Merrow, GenderConstants.Female, 9 * 12 + 3, 10 * 12)]
        //Based on Polar Bears
        [TestCase(CreatureConstants.Owlbear, GenderConstants.Male, 3 * 12 + 7, 5 * 12 + 3)]
        [TestCase(CreatureConstants.Owlbear, GenderConstants.Female, 2 * 12 + 8, 4 * 12 + 4)]
        //https://forgottenrealms.fandom.com/wiki/Pixie
        [TestCase(CreatureConstants.Pixie, GenderConstants.Male, 12, 2 * 12 + 6)]
        [TestCase(CreatureConstants.Pixie, GenderConstants.Female, 12, 2 * 12 + 6)]
        [TestCase(CreatureConstants.Pixie_WithIrresistibleDance, GenderConstants.Male, 12, 2 * 12 + 6)]
        [TestCase(CreatureConstants.Pixie_WithIrresistibleDance, GenderConstants.Female, 12, 2 * 12 + 6)]
        [TestCase(CreatureConstants.Quasit, GenderConstants.Agender, 1 * 12, 2 * 12)]
        //https://forgottenrealms.fandom.com/wiki/Sahuagin
        [TestCase(CreatureConstants.Sahuagin, GenderConstants.Male, 6 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Sahuagin, GenderConstants.Female, 6 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Sahuagin_Malenti, GenderConstants.Male, 6 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Sahuagin_Malenti, GenderConstants.Female, 6 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Sahuagin_Mutant, GenderConstants.Male, 6 * 12, 9 * 12)]
        [TestCase(CreatureConstants.Sahuagin_Mutant, GenderConstants.Female, 6 * 12, 9 * 12)]
        //https://forgottenrealms.fandom.com/wiki/Tiefling
        [TestCase(CreatureConstants.Tiefling, GenderConstants.Male, 4 * 12 + 11, 6 * 12)]
        [TestCase(CreatureConstants.Tiefling, GenderConstants.Female, 4 * 12 + 11, 6 * 12)]
        [TestCase(CreatureConstants.TrumpetArchon, GenderConstants.Male, 6 * 12 + 6, 7 * 12 + 9)]
        [TestCase(CreatureConstants.TrumpetArchon, GenderConstants.Female, 6 * 12 + 1, 7 * 12 + 4)]
        [TestCase(CreatureConstants.Wolf, GenderConstants.Male, 26, 33)]
        [TestCase(CreatureConstants.Wolf, GenderConstants.Female, 26, 33)]
        public void RollCalculationsAreAccurate_FromRange(string creature, string gender, int min, int max)
        {
            Assert.That(creatureHeights, Contains.Key(creature));
            Assert.That(creatureHeights[creature], Contains.Key(creature).And.ContainKey(gender));

            var baseHeight = dice.Roll(creatureHeights[creature][gender]).AsSum();
            var multiplierMin = dice.Roll(creatureHeights[creature][creature]).AsPotentialMinimum();
            var multiplierMax = dice.Roll(creatureHeights[creature][creature]).AsPotentialMaximum();
            var theoreticalRoll = RollHelper.GetRollWithFewestDice(min, max);
            var actualRoll = $"{creatureHeights[creature][creature]}+{creatureHeights[creature][gender]}";

            Assert.That(baseHeight + multiplierMin, Is.EqualTo(min), $"Min; Theoretical: {theoreticalRoll}; Actual: {actualRoll}");
            Assert.That(baseHeight + multiplierMax, Is.EqualTo(max), $"Max; Theoretical: {theoreticalRoll}; Actual: {actualRoll}");
        }

        private void AssertIfCreatureHasNoHeight_HasLength(string creature)
        {
            var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, creature);

            foreach (var gender in genders)
            {
                var height = measurementHelper.GetAverageHeight(creature, gender);
                var length = measurementHelper.GetAverageLength(creature, gender);

                if (height == 0)
                    Assert.That(length, Is.Positive, creature + gender);
            }
        }
    }
}