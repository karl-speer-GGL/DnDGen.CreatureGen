using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.Tables.Helpers;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Helpers;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Attacks
{
    [TestFixture]
    public class DamageDataTests : CollectionTests
    {
        private Dictionary<string, IEnumerable<AttackDataSelection>> attackData;
        private Dictionary<string, List<string>> creatureAttackDamageData;
        private Dictionary<string, List<string>> templateAttackDamageData;
        private Dictionary<string, IEnumerable<AdvancementDataSelection>> advancementData;
        private Dictionary<string, CreatureDataSelection> creatureData;
        private DamageHelper damageHelper;
        private Dictionary<string, Dictionary<string, string>> dragonDamages;
        private Dictionary<string, string> damageTypes;

        protected override string TableName => TableNameConstants.Collection.DamageData;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var attackDataSelector = GetNewInstanceOf<ICollectionDataSelector<AttackDataSelection>>();
            attackData = attackDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.AttackData);

            var creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
            creatureData = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Single());

            var advancementsDataSelector = GetNewInstanceOf<ICollectionDataSelector<AdvancementDataSelection>>();
            advancementData = advancementsDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.Advancements);

            damageHelper = GetNewInstanceOf<DamageHelper>();

            dragonDamages = new()
            {
                ["Bite"] = new()
                {
                    [SizeConstants.Tiny] = "1d4",
                    [SizeConstants.Small] = "1d6",
                    [SizeConstants.Medium] = "1d8",
                    [SizeConstants.Large] = "2d6",
                    [SizeConstants.Huge] = "2d8",
                    [SizeConstants.Gargantuan] = "4d6",
                    [SizeConstants.Colossal] = "4d8",
                },
                ["Claw"] = new()
                {
                    [SizeConstants.Tiny] = "1d3",
                    [SizeConstants.Small] = "1d4",
                    [SizeConstants.Medium] = "1d6",
                    [SizeConstants.Large] = "1d8",
                    [SizeConstants.Huge] = "2d6",
                    [SizeConstants.Gargantuan] = "2d8",
                    [SizeConstants.Colossal] = "4d6",
                },
                ["Wing"] = new()
                {
                    [SizeConstants.Medium] = "1d4",
                    [SizeConstants.Large] = "1d6",
                    [SizeConstants.Huge] = "1d8",
                    [SizeConstants.Gargantuan] = "2d6",
                    [SizeConstants.Colossal] = "2d8",
                },
                ["Tail Slap"] = new()
                {
                    [SizeConstants.Large] = "1d8",
                    [SizeConstants.Huge] = "2d6",
                    [SizeConstants.Gargantuan] = "2d8",
                    [SizeConstants.Colossal] = "4d6",
                },
                ["Crush"] = new()
                {
                    [SizeConstants.Huge] = "2d8",
                    [SizeConstants.Gargantuan] = "4d6",
                    [SizeConstants.Colossal] = "4d8",
                },
                ["Tail Sweep"] = new()
                {
                    [SizeConstants.Gargantuan] = "2d6",
                    [SizeConstants.Colossal] = "2d8",
                },
            };

            damageTypes = new Dictionary<string, string>
            {
                ["-Bite-"] = $"{AttributeConstants.DamageTypes.Piercing}/{AttributeConstants.DamageTypes.Slashing}/{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Claw-"] = $"{AttributeConstants.DamageTypes.Piercing}/{AttributeConstants.DamageTypes.Slashing}",
                ["-Talon-"] = $"{AttributeConstants.DamageTypes.Piercing}/{AttributeConstants.DamageTypes.Slashing}",
                ["-Talons-"] = $"{AttributeConstants.DamageTypes.Piercing}/{AttributeConstants.DamageTypes.Slashing}",
                ["-Rake-"] = $"{AttributeConstants.DamageTypes.Piercing}/{AttributeConstants.DamageTypes.Slashing}",
                ["-Rend-"] = $"{AttributeConstants.DamageTypes.Piercing}/{AttributeConstants.DamageTypes.Slashing}",
                ["-Gore-"] = $"{AttributeConstants.DamageTypes.Piercing}",
                ["-Slap-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Tail Slap-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Tail Sweep-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Crush-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Slam-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Sting-"] = $"{AttributeConstants.DamageTypes.Piercing}",
                ["-Tentacle-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Arm-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Wing-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Trample-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Unarmed Strike-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
                ["-Constrict-"] = $"{AttributeConstants.DamageTypes.Bludgeoning}",
            };

            creatureAttackDamageData = DamageTestData.GetCreatureAttackDamageData(attackData, creatureData, advancementData, damageHelper);
            templateAttackDamageData = DamageTestData.GetTemplateDamageData(attackData, damageHelper);
        }

        [Test]
        public void DamageDataNames()
        {
            var creatureKeys = damageHelper.GetAllCreatureDamageKeys();
            var templateKeys = damageHelper.GetAllTemplateDamageKeys();
            var names = creatureKeys.Concat(templateKeys);

            var testKeys = creatureAttackDamageData.Keys.Union(templateAttackDamageData.Keys);
            Assert.That(testKeys, Is.Unique.And.EquivalentTo(names));

            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureAttackDamageData(string creature)
        {
            var keys = damageHelper.GetCreatureDamageKeys(creature);
            foreach (var key in keys)
            {
                AssertCreatureAttackDamages(key, creature);
            }
        }

        private void AssertCreatureAttackDamages(string key, string creature)
        {
            Assert.That(creatureAttackDamageData, Contains.Key(key));

            AssertCommonAttacks(creature, key, creatureAttackDamageData[key]);
            AssertDragonAttacks(creature, key);

            AssertCollection(key, [.. creatureAttackDamageData[key]]);

            CreatureWithUnnaturalAttack_CanUseEquipment(creature, key);
        }

        private void AssertCommonAttacks(string creature, string key, List<string> entries)
        {
            AssertDiseaseAttacksHaveCorrectDamageTypes(creature, key, entries);
            AssertEnergyDrainAttack(creature, key, entries);
            AssertImprovedGrabAttackDamage(key, entries);
            AssertNaturalAttacksHaveCorrectDamageTypes(key, entries);
            AssertPoisonAttacksHaveCorrectDamageTypes(creature, key, entries);
            AssertPsionicAttackDamage(key, entries);
            AssertSpellLikeAbilityAttackDamage(key, entries);
            AssertSpellsAttackDamage(key, entries);
        }

        private void AssertDragonAttacks(string creature, string key)
        {
            if (!creature.Contains("Dragon,"))
                return;

            AssertDragonPhysicalAttack(creature, key, "Bite");
            AssertDragonPhysicalAttack(creature, key, "Claw");
            AssertDragonPhysicalAttack(creature, key, "Wing");
            AssertDragonPhysicalAttack(creature, key, "Tail Slap");
            AssertDragonPhysicalAttack(creature, key, "Crush");
            AssertDragonPhysicalAttack(creature, key, "Tail Sweep");

            AssertDragonBreathWeaponAttacks(creature, key);
            AssertDragonFrightfulPresenceAttack(creature, key);
        }

        private void AssertDragonPhysicalAttack(string creature, string key, string name)
        {
            if (!key.Contains($"-{name}-"))
                return;

            var selection = creatureAttackDamageData[key].Select(DataHelper.Parse<DamageDataSelection>).Single();
            var size = creatureData[creature].Size;
            Assert.That(selection, Is.Not.Null, key);
            Assert.That(selection.Roll, Is.EqualTo(dragonDamages[name][size]), key);
            Assert.That(selection.Type, Is.EqualTo(damageTypes[$"-{name}-"]), key);
            Assert.That(selection.Condition, Is.Empty, key);
        }

        private void AssertDragonBreathWeaponAttacks(string creature, string key)
        {
            if (!key.Contains("-Breath Weapon"))
                return;

            var breathWeaponDamages = creatureAttackDamageData[key].Select(DataHelper.Parse<DamageDataSelection>);
            var ageCategory = GetNumericDragonAgeCategory(creature.Split(',')[1].Trim());

            //INFO: This indicates no effect, so it should have damage
            if (key.EndsWith('-'))
            {
                var selection = breathWeaponDamages.Single();
                Assert.That(selection, Is.Not.Null, key);

                var quantity = Convert.ToInt32(selection.Roll.Split('d')[0]);
                Assert.That(quantity, Is.EqualTo(ageCategory).Or.EqualTo(ageCategory * 2), key);
                Assert.That(selection.Type, Is.Not.Empty, key);
                Assert.That(selection.Condition, Is.Empty, key);
            }
            else
            {
                Assert.That(breathWeaponDamages, Is.Empty);
            }
        }

        private void AssertDragonFrightfulPresenceAttack(string creature, string key)
        {
            if (!key.Contains("-Frightful Presence-"))
                return;

            Assert.That(creatureAttackDamageData[key], Is.Empty, key);
        }

        private int GetNumericDragonAgeCategory(string dragonAge)
        {
            var ages = new[]
            {
                "Wyrmling",
                "Very Young",
                "Young",
                "Juvenile",
                "Young Adult",
                "Adult",
                "Mature Adult",
                "Old",
                "Very Old",
                "Ancient",
                "Wyrm",
                "Great Wyrm",
            };
            return Array.IndexOf(ages, dragonAge) + 1;
        }

        private void AssertEnergyDrainAttack(string creature, string key, List<string> entries)
        {
            if (!key.Contains("-Energy Drain-"))
                return;

            var damageSelections = entries.Select(DataHelper.Parse<DamageDataSelection>).ToArray();
            Assert.That(damageSelections, Has.Length.EqualTo(1), key);
            Assert.That(damageSelections[0].Roll, Is.EqualTo("1").Or.EqualTo("2"), key);
            Assert.That(damageSelections[0].Type, Is.EqualTo("Negative Level"), key);
            Assert.That(damageSelections[0].Condition, Is.Empty, key);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void TemplateAttackDamageData(string template)
        {
            var keys = damageHelper.GetTemplateDamageKeys(template);
            foreach (var key in keys)
            {
                AssertTemplateAttackDamages(key, template);
            }
        }

        [Test]
        [Ignore("Don't run this unless you need to bulk update the damage data file")]
        public void DEBUG_WriteXml()
        {
            var collections = new XElement("collections");

            foreach (var creatureKvp in creatureAttackDamageData)
            {
                var collection = new XElement("collection", new XElement("name", creatureKvp.Key));

                foreach (var damageData in creatureKvp.Value)
                {
                    collection.Add(new XElement("entry", damageData));
                }

                collections.Add(collection);
            }

            foreach (var templateKvp in templateAttackDamageData)
            {
                var collection = new XElement("collection", new XElement("name", templateKvp.Key));

                foreach (var damageData in templateKvp.Value)
                {
                    collection.Add(new XElement("entry", damageData));
                }

                collections.Add(collection);
            }

            var path = Path.Combine(Environment.CurrentDirectory, $"{TableName}.xml");
            collections.Save(path);
        }

        private void AssertTemplateAttackDamages(string key, string template)
        {
            Assert.That(templateAttackDamageData, Contains.Key(key));

            AssertCommonAttacks(template, key, templateAttackDamageData[key]);

            AssertCollection(key, [.. templateAttackDamageData[key]]);
        }

        private void AssertNaturalAttacksHaveCorrectDamageTypes(string key, List<string> entries)
        {
            foreach (var kvp in damageTypes)
            {
                if (!key.Contains(kvp.Key, StringComparison.CurrentCultureIgnoreCase))
                    continue;

                Assert.That(entries, Is.Not.Empty, key);

                var damageType = kvp.Value;
                var firstDamage = DataHelper.Parse<DamageDataSelection>(entries[0]);
                Assert.That(firstDamage.Type, Is.EqualTo(damageType), key);
            }
        }

        private void AssertPoisonAttacksHaveCorrectDamageTypes(string creature, string key, List<string> entries)
        {
            if (!key.Contains("-Poison-"))
                return;

            var damageSelections = entries.Select(DataHelper.Parse<DamageDataSelection>).ToArray();
            Assert.That(damageSelections, Has.Length.AtMost(2).Or.Length.EqualTo(4), key);

            if (damageSelections.Length == 4)
            {
                Assert.That(damageSelections[0].Condition, Is.EqualTo("Initial"));
                Assert.That(damageSelections[1].Condition, Is.EqualTo("Initial"));
                Assert.That(damageSelections[2].Condition, Is.EqualTo("Secondary"));
                Assert.That(damageSelections[3].Condition, Is.EqualTo("Secondary"));
            }
            else if (damageSelections.Length == 2)
            {
                Assert.That(damageSelections[0].Condition, Is.EqualTo("Initial"));
                Assert.That(damageSelections[1].Condition, Is.EqualTo("Secondary"));
            }
            else if (damageSelections.Length == 1)
            {
                Assert.That(damageSelections[0].Condition, Is.EqualTo("Initial"));
            }
            else
            {
                var poisonAttack = attackData[creature].FirstOrDefault(a => a.Name.Equals("poison", StringComparison.CurrentCultureIgnoreCase));

                Assert.That(poisonAttack.DamageEffect, Is.Not.Empty);
            }
        }

        private void AssertDiseaseAttacksHaveCorrectDamageTypes(string creature, string key, List<string> entries)
        {
            var attackSelections = attackData[creature];

            var disease = attackSelections.FirstOrDefault(s => s.Name.Equals("disease", StringComparison.CurrentCultureIgnoreCase));
            if (disease == null)
                return;

            var specificDisease = attackSelections.FirstOrDefault(s => s.Name == disease.DamageEffect);
            if (specificDisease == null)
                Assert.Fail($"Could not find disease '{disease.DamageEffect}'");

            if (!key.Contains(specificDisease.Name))
                return;

            var damageSelections = entries.Select(DataHelper.Parse<DamageDataSelection>);
            foreach (var damageSelection in damageSelections)
            {
                Assert.That(damageSelection.Roll, Is.Not.Empty, key);
                Assert.That(damageSelection.Type, Is.Not.Empty, key);
                Assert.That(damageSelection.Condition, Does.StartWith("Incubation period"), key);
            }
        }

        private void CreatureWithUnnaturalAttack_CanUseEquipment(string creature, string key)
        {
            if (!IsEquipmentAttack(key))
                return;

            //No Damage for Equipment attacks
            Assert.That(creatureAttackDamageData[key], Is.Empty, key);

            //Has Natural Attack with Damage
            var naturalAttacks = attackData[creature].Where(a => a.IsNatural);
            var naturalAttackDamages = damageHelper.GetCreatureDamageKeys(creature)
                .Where(k => naturalAttacks.Any(a => k.Contains(a.Name)))
                .SelectMany(k => creatureAttackDamageData[k]);

            Assert.That(naturalAttackDamages, Is.Not.Empty);
        }

        private bool IsEquipmentAttack(string key) => key.Contains(AttributeConstants.Melee) || key.Contains(AttributeConstants.Ranged);

        private void AssertImprovedGrabAttackDamage(string key, List<string> entries) => AssertEmpty(key, entries, "Improved Grab");
        private void AssertSpellsAttackDamage(string key, List<string> entries) => AssertEmpty(key, entries, "Spells");
        private void AssertSpellLikeAbilityAttackDamage(string key, List<string> entries) => AssertEmpty(key, entries, FeatConstants.SpecialQualities.SpellLikeAbility);
        private void AssertPsionicAttackDamage(string key, List<string> entries) => AssertEmpty(key, entries, FeatConstants.SpecialQualities.Psionic);

        private void AssertEmpty(string key, List<string> entries, string attackName)
        {
            if (key.Contains(attackName))
            {
                Assert.That(entries, Is.Empty, key);
            }
        }

        [TestCase("1d2")]
        [TestCase("1d3")]
        [TestCase("1d4")]
        [TestCase("1d6")]
        [TestCase("1d8")]
        [TestCase("1d10")]
        [TestCase("2d6")]
        [TestCase("2d8")]
        [TestCase("3d6")]
        [TestCase("3d8")]
        public void AdjustNaturalAttackDamageBySize_SameSize(string originalDamage)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Medium, SizeConstants.Medium);
            Assert.That(adjustedDamage, Is.EqualTo(originalDamage));
        }

        [TestCase("1d2", "1d3")]
        [TestCase("1d3", "1d4")]
        [TestCase("1d4", "1d6")]
        [TestCase("1d6", "1d8")]
        [TestCase("1d8", "2d6")]
        [TestCase("1d10", "2d8")]
        [TestCase("2d6", "3d6")]
        [TestCase("2d8", "3d8")]
        [TestCase("3d6", "4d6")]
        [TestCase("3d8", "4d8")]
        public void AdjustNaturalAttackDamageUpBySize_OneSize(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Huge, SizeConstants.Gargantuan);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCase("1d2", "1d4")]
        [TestCase("1d3", "1d6")]
        [TestCase("1d4", "1d8")]
        [TestCase("1d6", "2d6")]
        [TestCase("1d8", "3d6")]
        [TestCase("1d10", "3d8")]
        [TestCase("2d6", "4d6")]
        [TestCase("2d8", "4d8")]
        [TestCase("3d6", "5d6")]
        [TestCase("3d8", "5d8")]
        public void AdjustNaturalAttackDamageUpBySize_MultipleSizes(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Small, SizeConstants.Large);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCase("1d2", "1")]
        [TestCase("1d3", "1d2")]
        [TestCase("1d4", "1d3")]
        [TestCase("1d6", "1d4")]
        [TestCase("1d8", "1d6")]
        [TestCase("1d10", "1d8")]
        [TestCase("2d6", "1d8")]
        [TestCase("2d8", "1d10")]
        [TestCase("3d6", "2d6")]
        [TestCase("3d8", "2d8")]
        public void AdjustNaturalAttackDamageDownBySize_OneSize(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Gargantuan, SizeConstants.Huge);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCase("1d2", "0")]
        [TestCase("1d3", "1")]
        [TestCase("1d4", "1d2")]
        [TestCase("1d6", "1d3")]
        [TestCase("1d8", "1d4")]
        [TestCase("1d10", "1d6")]
        [TestCase("2d6", "1d6")]
        [TestCase("2d8", "1d8")]
        [TestCase("3d6", "1d8")]
        [TestCase("3d8", "1d10")]
        public void AdjustNaturalAttackDamageDownBySize_MultipleSizes(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Large, SizeConstants.Small);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(VerboseDamages))]
        public void AdjustNaturalAttackDamageUpBySize_VerboseRollDamage(string[] originalDamageData, string[] adjustedDamageData)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            for (var i = 0; i < originalDamageData.Length; i++)
            {
                var originalSelection = DataHelper.Parse<DamageDataSelection>(originalDamageData[i]);
                var adjustedSelection = DataHelper.Parse<DamageDataSelection>(adjustedDamageData[i]);
                var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalSelection.Roll, SizeConstants.Medium, SizeConstants.Large);

                Assert.That(adjustedDamage, Is.EqualTo(adjustedSelection.Roll));
                Assert.That(originalSelection.Type, Is.EqualTo(adjustedSelection.Type));
                Assert.That(originalSelection.Condition, Is.EqualTo(adjustedSelection.Condition));
            }
        }

        private static IEnumerable VerboseDamages
        {
            get
            {
                var originalDamagesDatas = new string[][]
                {
                    [DamageTestData.BuildData("1d6", "piercing")],
                    [DamageTestData.BuildData("1d6", "piercing", "sometimes")],
                    [DamageTestData.BuildData("1d6", "bludgeoning", string.Empty), DamageTestData.BuildData("1d4", "acid", "often")],
                    [DamageTestData.BuildData("1d6", "bludgeoning", "sometimes"), DamageTestData.BuildData("1d4", "acid", string.Empty)],
                    [DamageTestData.BuildData("1d6", "bludgeoning", "sometimes"), DamageTestData.BuildData("1d4", "acid", "often")],
                    [DamageTestData.BuildData("1d2", "bludgeoning", string.Empty), DamageTestData.BuildData("1d10", "acid", "often")],
                    [DamageTestData.BuildData("1d2", "bludgeoning", "sometimes"), DamageTestData.BuildData( "1d10", "acid", string.Empty)],
                    [DamageTestData.BuildData("1d2", "bludgeoning", "sometimes"), DamageTestData.BuildData("1d10", "acid", "often")],
                };

                var adjustedDamagesDatas = new string[][]
                {
                    [DamageTestData.BuildData("1d8", "piercing")],
                    [DamageTestData.BuildData("1d8", "piercing", "sometimes")],
                    [DamageTestData.BuildData("1d8", "bludgeoning", string.Empty), DamageTestData.BuildData("1d6", "acid", "often")],
                    [DamageTestData.BuildData("1d8", "bludgeoning", "sometimes"), DamageTestData.BuildData("1d6", "acid", string.Empty)],
                    [DamageTestData.BuildData("1d8", "bludgeoning", "sometimes"), DamageTestData.BuildData("1d6", "acid", "often")],
                    [DamageTestData.BuildData("1d3", "bludgeoning", string.Empty), DamageTestData.BuildData("2d8", "acid", "often")],
                    [DamageTestData.BuildData("1d3", "bludgeoning", "sometimes"), DamageTestData.BuildData("2d8", "acid", string.Empty)],
                    [DamageTestData.BuildData("1d3", "bludgeoning", "sometimes"), DamageTestData.BuildData("2d8", "acid", "often")],
                };

                for (var i = 0; i < originalDamagesDatas.Length; i++)
                {
                    yield return new TestCaseData(originalDamagesDatas[i], adjustedDamagesDatas[i]);
                }
            }
        }

        [TestCase("0", "1")]
        [TestCase("1", "1d2")]
        [TestCase("1d2", "1d3")]
        [TestCase("1d10", "2d8")]
        [TestCase("4d6", "5d6")]
        [TestCase("4d4", "5d4")]
        [TestCase("9266d90210", "9267d90210")]
        [TestCase("1d8+5", "2d6+5")]
        public void AdjustNaturalAttackDamageBySize_NonStandardRollDamage_Up(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Large, SizeConstants.Huge);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCase("0", "0")]
        [TestCase("1", "0")]
        [TestCase("1d2", "1")]
        [TestCase("1d10", "1d8")]
        [TestCase("4d6", "3d6")]
        [TestCase("4d4", "3d4")]
        [TestCase("9266d90210", "9265d90210")]
        [TestCase("1d8+5", "1d6+5")]
        public void AdjustNaturalAttackDamageBySize_NonStandardRollDamage_Down(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Medium, SizeConstants.Small);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCase("4d6", "1d8")]
        public void AdjustNaturalAttackDamageBySize_NonStandardRollDamage_DownToDefault(string originalDamage, string expected)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Huge, SizeConstants.Small);
            Assert.That(adjustedDamage, Is.EqualTo(expected));
        }

        [TestCase("4d4")]
        public void AdjustNaturalAttackDamageBySize_NonStandardRollDamage_DownToZero(string originalDamage)
        {
            var attack = new AttackDataSelection { IsNatural = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, originalDamage, SizeConstants.Colossal, SizeConstants.Fine);
            Assert.That(adjustedDamage, Is.EqualTo("0"));
        }

        [Test]
        public void DoNotAdjustUnnaturalAttackDamageBySize()
        {
            var attack = new AttackDataSelection { IsNatural = false };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, "1d2", SizeConstants.Fine, SizeConstants.Colossal);
            Assert.That(adjustedDamage, Is.EqualTo("1d2"));
        }

        //INFO: We do not want to increase damage for special attacks
        //The Improving Monsters rules state that a "special attack that increases combat effectiveness" would increase the CR even more than normal
        //Since doing more damage would definitely be more combat-effective, it means special attacks should not increase damage
        //There are some oddities to this (such as Constrict attacks not increasing when a Slam does), but we'll accept that for now
        [Test]
        public void DoNotAdjustNaturalSpecialAttackDamageBySize()
        {
            var attack = new AttackDataSelection { IsNatural = true, IsSpecial = true };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, "1d2", SizeConstants.Fine, SizeConstants.Colossal);
            Assert.That(adjustedDamage, Is.EqualTo("1d2"));
        }

        [Test]
        public void DoNotAdjustNaturalMeleeAttackEffectRolls()
        {
            var attack = new AttackDataSelection { IsNatural = true, DamageEffect = "1d2" };
            var adjustedDamage = DamageTestData.GetAdjustedDamage(attack, "1d2", SizeConstants.Fine, SizeConstants.Colossal);
            Assert.That(adjustedDamage, Is.EqualTo("5d6"));
            Assert.That(attack.DamageEffect, Is.EqualTo("1d2"));
        }
    }
}
