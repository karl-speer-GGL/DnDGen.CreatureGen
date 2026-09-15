using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Selectors.Collections;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Helpers;
using DnDGen.Infrastructure.Selectors.Collections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Feats.Data
{
    [TestFixture]
    public class SpecialQualityDataTests : CollectionTests
    {
        private IFeatsSelector featsSelector;
        private Dictionary<string, List<string>> templateSpecialQualityData;
        private Dictionary<string, List<string>> creatureSpecialQualityData;
        private Dictionary<string, List<string>> typeSpecialQualityData;
        private Dictionary<string, List<string>> subtypeSpecialQualityData;
        private Dictionary<string, CreatureDataSelection> creatureDataSelections;

        protected override string TableName => TableNameConstants.Collection.SpecialQualityData;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            templateSpecialQualityData = SpecialQualityTestData.GetTemplateData();
            creatureSpecialQualityData = SpecialQualityTestData.GetCreatureData();
            typeSpecialQualityData = SpecialQualityTestData.GetTypeData();
            subtypeSpecialQualityData = SpecialQualityTestData.GetSubtypeData();

            var creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
            creatureDataSelections = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Single());

        }

        [SetUp]
        public void Setup()
        {
            featsSelector = GetNewInstanceOf<IFeatsSelector>();
        }

        [Test]
        public void SpecialQualityDataNames()
        {
            var creatures = CreatureConstants.GetAll();
            var types = CreatureConstants.Types.GetAll();
            var subtypes = CreatureConstants.Types.Subtypes.GetAll();
            var templates = CreatureConstants.Templates.GetAll();

            Assert.That(creatureSpecialQualityData.Keys, Is.EquivalentTo(creatures));
            Assert.That(templateSpecialQualityData.Keys, Is.EquivalentTo(templates));
            Assert.That(typeSpecialQualityData.Keys, Is.EquivalentTo(types));
            Assert.That(subtypeSpecialQualityData.Keys, Is.EquivalentTo(subtypes.Except(creatures)));

            var names = creatures.Union(types).Union(subtypes).Union(templates);
            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void SpecialQualityData_Creature(string creature)
        {
            Assert.That(creatureSpecialQualityData.Keys, Contains.Item(creature));
            AssertSpecialQualityData(creature, creatureSpecialQualityData[creature]);

            NoOverlapBetweenCreatureAndCreatureTypes(creature);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void SpecialQualityData_Template(string template)
        {
            Assert.That(templateSpecialQualityData.Keys, Contains.Item(template));
            AssertSpecialQualityData(template, templateSpecialQualityData[template]);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Types))]
        public void SpecialQualityData_Type(string type)
        {
            Assert.That(typeSpecialQualityData.Keys, Contains.Item(type));
            AssertSpecialQualityData(type, typeSpecialQualityData[type]);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Subtypes))]
        public void SpecialQualityData_Subtype(string subtype)
        {
            var creatures = CreatureConstants.GetAll();
            if (creatures.Contains(subtype))
                Assert.Pass($"{subtype} is duplicate of the creature entry");

            Assert.That(subtypeSpecialQualityData.Keys, Contains.Item(subtype));
            AssertSpecialQualityData(subtype, subtypeSpecialQualityData[subtype]);
        }

        private void AssertSpecialQualityData(string creature, List<string> entries)
        {
            var specialQualities = entries.Select(DataHelper.Parse<SpecialQualityDataSelection>);

            //Bonus Feats Have Correct Data
            var feats = featsSelector.SelectFeats();
            var bonusFeatsData = specialQualities.Where(d => feats.Any(f => f.Feat == d.Feat));

            foreach (var data in bonusFeatsData)
            {
                var matchingFeat = feats.First(f => f.Feat == data.Feat);

                Assert.That(data.FrequencyQuantity, Is.EqualTo(matchingFeat.FrequencyQuantity), $"TEST DATA: {matchingFeat.Feat} - Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.EqualTo(matchingFeat.FrequencyTimePeriod), $"TEST DATA: {matchingFeat.Feat} - Frequency Time Period");
                Assert.That(data.Power, Is.EqualTo(matchingFeat.Power), $"TEST DATA: {matchingFeat.Feat} - Power");
            }

            //Proficiency Feats Have Correct Foci
            var proficiencyFeats = new[]
            {
                FeatConstants.WeaponProficiency_Exotic,
                FeatConstants.WeaponProficiency_Martial,
                FeatConstants.WeaponProficiency_Simple,
            };

            var proficiencyFeatsData = specialQualities.Where(d => proficiencyFeats.Contains(d.Feat) && d.FocusType != GroupConstants.All);

            var weaponFamiliarityData = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.WeaponFamiliarity);
            var weaponFamiliarityFoci = weaponFamiliarityData.Select(d => d.FocusType);

            foreach (var data in proficiencyFeatsData)
            {
                var featName = data.Feat;
                var focus = data.FocusType;

                var featFoci = collectionMapper.Map(Config.Name, TableNameConstants.Collection.FeatFoci);
                var proficiencyFoci = featFoci[featName];

                if (weaponFamiliarityFoci.Contains(focus) && featName == FeatConstants.WeaponProficiency_Martial)
                {
                    Assert.That(featFoci[FeatConstants.WeaponProficiency_Exotic], Contains.Item(focus), $"WEAPON FAMILIARITY: {focus}");
                    proficiencyFoci = proficiencyFoci.Union([focus]);
                }

                Assert.That(proficiencyFoci, Contains.Item(focus), $"TEST DATA: {featName}");
            }

            //Feats Focusing On Weapons Or Armor Require Equipment
            var weaponAndArmorFeats = new[]
            {
                FeatConstants.ArmorProficiency_Heavy,
                FeatConstants.ArmorProficiency_Light,
                FeatConstants.ArmorProficiency_Medium,
                FeatConstants.ShieldBash_Improved,
                FeatConstants.ShieldProficiency,
                FeatConstants.ShieldProficiency_Tower,
                FeatConstants.TwoWeaponDefense,
                FeatConstants.TwoWeaponFighting,
                FeatConstants.TwoWeaponFighting_Greater,
                FeatConstants.TwoWeaponFighting_Improved,
                FeatConstants.WeaponProficiency_Exotic,
                FeatConstants.WeaponProficiency_Martial,
                FeatConstants.WeaponProficiency_Simple,
                FeatConstants.Monster.MultiweaponFighting,
                FeatConstants.Monster.MultiweaponFighting_Greater,
                FeatConstants.Monster.MultiweaponFighting_Improved,
                FeatConstants.SpecialQualities.OversizedWeapon,
                FeatConstants.SpecialQualities.TwoWeaponFighting_Superior,
                FeatConstants.SpecialQualities.WeaponFamiliarity,
            };

            var weaponArmorFeatsData = specialQualities.Where(d => weaponAndArmorFeats.Contains(d.Feat));

            foreach (var data in weaponArmorFeatsData)
            {
                var featName = data.Feat;

                var requiresEquipment = Convert.ToBoolean(data.RequiresEquipment);
                Assert.That(requiresEquipment, Is.True, $"TEST DATA: {featName}");
            }

            //Fast Healing Has Correct Frequency
            var fastHealingData = specialQualities.FirstOrDefault(e => e.Feat == FeatConstants.SpecialQualities.FastHealing);
            if (fastHealingData != null)
            {
                Assert.That(fastHealingData.FrequencyQuantity, Is.EqualTo(1), "TEST DATA: Frequency Quantity");
                Assert.That(fastHealingData.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), "TEST DATA: Frequency Time Period");
            }

            //Regeneration Has Correct Frequency
            var regenerationData = specialQualities.FirstOrDefault(e => e.Feat == FeatConstants.SpecialQualities.Regeneration);
            if (regenerationData != null)
            {
                Assert.That(regenerationData.FrequencyQuantity, Is.EqualTo(1), "TEST DATA: Frequency Quantity");
                Assert.That(regenerationData.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), "TEST DATA: Frequency Time Period");
            }

            //Damage Reduction Has Correct Data
            var damageReductiondatas = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.DamageReduction);

            foreach (var data in damageReductiondatas)
            {
                Assert.That(data.FrequencyQuantity, Is.EqualTo(1), "TEST DATA: Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Hit), "TEST DATA: Frequency Time Period");
                Assert.That(data.FocusType, Is.Not.Empty, "TEST DATA: Focus");
                Assert.That(data.Power, Is.Positive, "TEST DATA: Power");
            }

            //Immunity Has Correct Data
            var immunityDatas = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.Immunity);

            foreach (var data in immunityDatas)
            {
                Assert.That(data.FrequencyQuantity, Is.EqualTo(0), "TEST DATA: Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.Empty, "TEST DATA: Frequency Time Period");
                Assert.That(data.FocusType, Is.Not.Empty, "TEST DATA: Focus");
                Assert.That(data.Power, Is.EqualTo(0), "TEST DATA: Power");
            }

            //Change Shape Has Correct Data
            var changeShapeDatas = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.ChangeShape);

            foreach (var data in changeShapeDatas)
            {
                Assert.That(data.FrequencyQuantity, Is.Not.Negative, "TEST DATA: Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.Not.Empty, "TEST DATA: Frequency Time Period");
                Assert.That(data.FocusType, Is.Not.Empty, "TEST DATA: Focus");
                Assert.That(data.Power, Is.EqualTo(0), "TEST DATA: Power");
            }

            //Spell-Like Ability Has Correct Data
            var spellLikeAbilityDatas = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.SpellLikeAbility);

            foreach (var data in spellLikeAbilityDatas)
            {
                Assert.That(data.FocusType, Is.Not.Empty, "TEST DATA: Focus");
                Assert.That(data.FrequencyQuantity, Is.Not.Negative, $"TEST DATA: {data.FocusType} - Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.Not.Empty, $"TEST DATA: {data.FocusType} - Frequency Time Period");
                Assert.That(data.Power, Is.Zero, $"TEST DATA: {data.FocusType} - Power");
            }

            //Psionic Has Correct Data
            var psionicDatas = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.Psionic);

            foreach (var data in psionicDatas)
            {
                Assert.That(data.FocusType, Is.Not.Empty, "TEST DATA: Focus");
                Assert.That(data.FrequencyQuantity, Is.Not.Negative, $"TEST DATA: {data.FocusType} - Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.Not.Empty, $"TEST DATA: {data.FocusType} - Frequency Time Period");
                Assert.That(data.Power, Is.Zero, $"TEST DATA: {data.FocusType} - Power");
            }

            //Energy Resistance Has Correct Data
            var energies = new[]
            {
                FeatConstants.Foci.Elements.Acid,
                FeatConstants.Foci.Elements.Cold,
                FeatConstants.Foci.Elements.Electricity,
                FeatConstants.Foci.Elements.Fire,
                FeatConstants.Foci.Elements.Sonic,
            };

            var energyResistanceDatas = specialQualities.Where(d => d.Feat == FeatConstants.SpecialQualities.EnergyResistance);

            foreach (var data in energyResistanceDatas)
            {
                Assert.That(data.FocusType, Is.Not.Empty, "TEST DATA: Focus");
                Assert.That(energies, Contains.Item(data.FocusType), $"TEST DATA: Focus");
                Assert.That(data.FrequencyQuantity, Is.EqualTo(1), $"TEST DATA: {data.FocusType} - Frequency Quantity");
                Assert.That(data.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), $"TEST DATA: {data.FocusType} - Frequency Time Period");
                Assert.That(data.Power, Is.Positive, $"TEST DATA: {data.FocusType} - Power");
                Assert.That(data.Power % 5, Is.Zero, $"TEST DATA: {data.FocusType} - Power");
            }

            //Creatures That Can Change Shape Into Humanoid Can Use Equipment
            var allCreatures = CreatureConstants.GetAll();
            if (allCreatures.Contains(creature))
            {
                var changeShapeFeats = new[]
                {
                    FeatConstants.SpecialQualities.ChangeShape,
                    FeatConstants.SpecialQualities.AlternateForm,
                };

                changeShapeDatas = specialQualities.Where(d => changeShapeFeats.Contains(d.Feat));

                var humanoids = new[]
                {
                    CreatureConstants.Goblin, //For Barghest
                    CreatureConstants.Types.Giant,
                    CreatureConstants.Types.Humanoid,
                    CreatureConstants.Types.MonstrousHumanoid,
                };

                foreach (var data in changeShapeDatas)
                {
                    var focus = data.FocusType;
                    var changesIntoHumanoid = humanoids.Any(h => focus.ToLower().Contains(h.ToLower()));

                    if (changesIntoHumanoid)
                    {
                        Assert.That(creatureDataSelections[creature].CanUseEquipment, Is.True, $"TEST DATA: {focus}");
                    }
                }
            }

            AssertCollection(creature, [.. entries]);
        }

        private void NoOverlapBetweenCreatureAndCreatureTypes(string creature)
        {
            var creatureTypes = creatureDataSelections[creature].Types.Except([creature]); //INFO: In case creature name duplicates as type, such as Gnoll

            Assert.That(table.Keys, Is.SupersetOf(creatureTypes));

            foreach (var creatureType in creatureTypes)
            {
                var overlap = table[creatureType].Intersect(creatureSpecialQualityData[creature]);
                Assert.That(overlap, Is.Empty, $"TEST CASE v TEST DATA: {creature} - {creatureType}");

                overlap = table[creatureType].Intersect(table[creature]);
                Assert.That(overlap, Is.Empty, $"XML v TEST DATA: {creature} - {creatureType}");

                if (typeSpecialQualityData.ContainsKey(creatureType))
                {
                    overlap = typeSpecialQualityData[creatureType].Intersect(table[creature]);
                    Assert.That(overlap, Is.Empty, $"XML v TEST CASE: {creature} - {creatureType}");

                    overlap = typeSpecialQualityData[creatureType].Intersect(creatureSpecialQualityData[creature]);
                    Assert.That(overlap, Is.Empty, $"TEST CASE v TEST CASE: {creature} - {creatureType}");
                }
                else if (subtypeSpecialQualityData.ContainsKey(creatureType))
                {
                    overlap = subtypeSpecialQualityData[creatureType].Intersect(table[creature]);
                    Assert.That(overlap, Is.Empty, $"XML v TEST CASE: {creature} - {creatureType}");

                    overlap = subtypeSpecialQualityData[creatureType].Intersect(creatureSpecialQualityData[creature]);
                    Assert.That(overlap, Is.Empty, $"TEST CASE v TEST CASE: {creature} - {creatureType}");
                }
            }
        }
    }
}
