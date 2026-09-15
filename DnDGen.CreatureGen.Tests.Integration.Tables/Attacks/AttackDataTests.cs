using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Defenses;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Selectors.Collections;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.Infrastructure.Helpers;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Attacks
{
    [TestFixture]
    public class AttackDataTests : CollectionTests
    {
        private IFeatsSelector featsSelector;
        private Dictionary<string, List<string>> creatureAttackData;
        private Dictionary<string, List<string>> templateAttackData;
        private Dictionary<string, CreatureDataSelection> creatureData;

        protected override string TableName => TableNameConstants.Collection.AttackData;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            creatureAttackData = AttackTestData.GetCreatureAttackData();
            templateAttackData = AttackTestData.GetTemplateAttackData();

            var creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
            creatureData = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Single());
        }

        [SetUp]
        public void Setup()
        {
            featsSelector = GetNewInstanceOf<IFeatsSelector>();
        }

        [Test]
        public void AttackDataNames()
        {
            var creatures = CreatureConstants.GetAll();
            var templates = CreatureConstants.Templates.GetAll();

            Assert.That(creatureAttackData.Keys, Is.EquivalentTo(creatures));
            Assert.That(templateAttackData.Keys, Is.EquivalentTo(templates));

            var names = creatureAttackData.Keys.Union(templateAttackData.Keys);

            AssertCollectionNames(names);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void CreatureAttackData(string creature)
        {
            if (!creatureAttackData[creature].Any())
                Assert.Fail("Test case did not specify attacks or NONE");

            if (creatureAttackData[creature][0] == AttackTestData.None)
                creatureAttackData[creature].Clear();

            AssertCommonAttacks(creatureAttackData[creature]);
            AssertDragonAttacks(creature);

            AssertCollection(creature, [.. creatureAttackData[creature]]);

            CreatureWithSpellLikeAbilityAttack_HasSpellLikeAbilitySpecialQuality(creature);
            CreatureWithPsionicAttack_HasPsionicSpecialQuality(creature);
            CreatureWithSpellsAttack_HasMagicSpells(creature);
            CreatureWithUnnaturalAttack_CanUseEquipment(creature);
        }

        private void AssertCommonAttacks(List<string> entries)
        {
            AssertConstrictAttack(entries);
            AssertDiseaseAttacks(entries);
            AssertImprovedGrabAttack(entries);
            AssertPoisonAttacks(entries);
            AssertPsionicAttack(entries);
            AssertRageAttack(entries);
            AssertRakeAttack(entries);
            AssertRendAttack(entries);
            AssertSpellLikeAbilityAttack(entries);
            AssertSpellsAttack(entries);

            AssertSpecialAttacks(entries);
            AssertDamageEffectDoesNotHaveDamage(entries);
            AssertDamageEffectAttackIsNotPrimary(entries);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void TemplateAttackData(string template)
        {
            if (!templateAttackData[template].Any())
                Assert.Fail("Test case did not specify attacks or NONE");

            if (templateAttackData[template][0] == AttackTestData.None)
                templateAttackData[template].Clear();

            AssertCommonAttacks(templateAttackData[template]);

            AssertCollection(template, [.. templateAttackData[template]]);
        }

        private void AssertDamageEffectDoesNotHaveDamage(List<string> entries)
        {
            var damageTypes = new[]
            {
                AttributeConstants.DamageTypes.Bludgeoning.ToLower(),
                AttributeConstants.DamageTypes.Piercing.ToLower(),
                AttributeConstants.DamageTypes.Slashing.ToLower(),

                FeatConstants.Foci.Elements.Acid.ToLower(),
                FeatConstants.Foci.Elements.Cold.ToLower(),
                FeatConstants.Foci.Elements.Electricity.ToLower(),
                FeatConstants.Foci.Elements.Fire.ToLower(),
                FeatConstants.Foci.Elements.Sonic.ToLower(),

                AbilityConstants.Charisma.ToLower(),
                AbilityConstants.Constitution.ToLower(),
                AbilityConstants.Dexterity.ToLower(),
                AbilityConstants.Intelligence.ToLower(),
                AbilityConstants.Strength.ToLower(),
                AbilityConstants.Wisdom.ToLower(),
                "str",
                "con",
                "dex",
                "wis",
                "int",
                "cha",

                "level",
                "negative"
            };

            var selections = entries.Select(DataHelper.Parse<AttackDataSelection>);
            var attackNames = selections.Select(s => s.Name);
            var nonDamageEffectAttacks = selections.Where(s => !attackNames.Contains(s.DamageEffect));

            foreach (var selection in nonDamageEffectAttacks)
            {
                var damageEffectWords = selection.DamageEffect.ToLower().Split(' ');
                Assert.That(damageEffectWords.Intersect(damageTypes), Is.Empty, selection.Name);
            }
        }

        private void AssertDamageEffectAttackIsNotPrimary(List<string> entries)
        {
            var selections = entries.Select(DataHelper.Parse<AttackDataSelection>);
            var damageEffects = selections.Select(s => s.DamageEffect);
            var damageEffectAttacks = selections.Where(s => damageEffects.Contains(s.Name));

            foreach (var selection in damageEffectAttacks)
            {
                Assert.That(selection.IsSpecial, Is.True, selection.Name);
                Assert.That(selection.IsPrimary, Is.False, selection.Name);
                Assert.That(selection.IsNatural, Is.True, selection.Name);
            }
        }

        private void AssertDragonAttacks(string creature)
        {
            if (!creature.Contains("Dragon,"))
                return;

            AssertDragonPhysicalAttack(creature, "Bite", SizeConstants.Tiny, 1, true, "melee", 1, false);
            AssertDragonPhysicalAttack(creature, "Claw", SizeConstants.Tiny, 2, false, "melee", 0.5, false);
            AssertDragonPhysicalAttack(creature, "Wing", SizeConstants.Medium, 2, false, "melee", 0.5, false);
            AssertDragonPhysicalAttack(creature, "Tail Slap", SizeConstants.Large, 1, false, "melee", 1.5, false);
            AssertDragonPhysicalAttack(creature, "Crush", SizeConstants.Huge, 1, true, "extraordinary ability", 1.5, true);
            AssertDragonPhysicalAttack(creature, "Tail Sweep", SizeConstants.Gargantuan, 1, true, "extraordinary ability", 1.5, true);

            AssertDragonBreathWeaponAttacks(creature);
            AssertDragonFrightfulPresenceAttack(creature);
        }

        private void AssertDragonPhysicalAttack(string creature, string name, string minimumSize, int frequency, bool primary, string attackType, double multiplier, bool special)
        {
            var selection = creatureAttackData[creature]
                .Select(DataHelper.Parse<AttackDataSelection>)
                .FirstOrDefault(s => s.Name == name);

            if (!SizeIsAtLeast(creature, minimumSize))
            {
                Assert.That(selection, Is.Null);
                return;
            }

            Assert.That(selection, Is.Not.Null, name);
            Assert.That(selection.AttackType, Is.EqualTo(attackType), name);
            Assert.That(selection.DamageBonusMultiplier, Is.EqualTo(multiplier), name);
            Assert.That(selection.DamageEffect, Is.Empty, name);
            Assert.That(selection.FrequencyQuantity, Is.EqualTo(frequency), name);
            Assert.That(selection.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), name);
            Assert.That(selection.IsMelee, Is.True, name);
            Assert.That(selection.IsPrimary, Is.EqualTo(primary), name);
            Assert.That(selection.IsNatural, Is.True, name);
            Assert.That(selection.IsSpecial, Is.EqualTo(special), name);
            Assert.That(selection.RequiredGender, Is.Empty, name);

            if (special)
            {
                Assert.That(selection.Save, Is.EqualTo(SaveConstants.Reflex), name);
                Assert.That(selection.SaveAbility, Is.EqualTo(AbilityConstants.Constitution), name);
                Assert.That(selection.SaveDcBonus, Is.Zero, name);
            }
            else
            {
                Assert.That(selection.Save, Is.Empty, name);
                Assert.That(selection.SaveAbility, Is.Empty, name);
                Assert.That(selection.SaveDcBonus, Is.Zero, name);
            }
        }

        private void AssertDragonBreathWeaponAttacks(string creature)
        {
            var breathWeaponAttacks = creatureAttackData[creature]
                .Select(DataHelper.Parse<AttackDataSelection>)
                .Where(s => s.Name.StartsWith("Breath Weapon"));

            Assert.That(breathWeaponAttacks, Is.Not.Empty.And.All.Not.Null);

            foreach (var breathWeapon in breathWeaponAttacks)
            {
                Assert.That(breathWeapon.AttackType, Is.EqualTo("supernatural ability"), breathWeapon.Name);
                Assert.That(breathWeapon.DamageBonusMultiplier, Is.Zero, breathWeapon.Name);
                Assert.That(breathWeapon.FrequencyQuantity, Is.EqualTo(1), breathWeapon.Name);
                Assert.That(breathWeapon.FrequencyTimePeriod, Is.EqualTo($"1d4 {FeatConstants.Frequencies.Round}"), breathWeapon.Name);
                Assert.That(breathWeapon.IsMelee, Is.False, breathWeapon.Name);
                Assert.That(breathWeapon.IsPrimary, Is.True, breathWeapon.Name);
                Assert.That(breathWeapon.IsNatural, Is.True, breathWeapon.Name);
                Assert.That(breathWeapon.IsSpecial, Is.True, breathWeapon.Name);
                Assert.That(breathWeapon.RequiredGender, Is.Empty, breathWeapon.Name);
                Assert.That(breathWeapon.SaveAbility, Is.EqualTo(AbilityConstants.Constitution), breathWeapon.Name);
                Assert.That(breathWeapon.SaveDcBonus, Is.Zero, breathWeapon.Name);

                //INFO: Indicates this is a damaging breath weapon
                if (string.IsNullOrEmpty(breathWeapon.DamageEffect))
                {
                    Assert.That(breathWeapon.Save, Is.EqualTo(SaveConstants.Reflex).Or.EqualTo(SaveConstants.Fortitude), breathWeapon.Name);
                }
                else
                {
                    Assert.That(breathWeapon.Save, Is.EqualTo(SaveConstants.Will).Or.EqualTo(SaveConstants.Fortitude), breathWeapon.Name);

                    var ageCategory = GetNumericDragonAgeCategory(creature.Split(',')[1].Trim());
                    Assert.That(breathWeapon.DamageEffect, Contains.Substring($"+{ageCategory}"), breathWeapon.Name);
                }
            }
        }

        private void AssertDragonFrightfulPresenceAttack(string creature)
        {
            var frightfulPresence = creatureAttackData[creature]
                .Select(DataHelper.Parse<AttackDataSelection>)
                .FirstOrDefault(s => s.Name == "Frightful Presence");

            if (!DragonAgeIsAtLeast(creature, "Young Adult"))
            {
                Assert.That(frightfulPresence, Is.Null, "Frightful Presence");
                return;
            }

            Assert.That(frightfulPresence, Is.Not.Null);
            Assert.That(frightfulPresence.AttackType, Is.EqualTo("extraordinary ability"), frightfulPresence.Name);
            Assert.That(frightfulPresence.DamageBonusMultiplier, Is.Zero, frightfulPresence.Name);
            Assert.That(frightfulPresence.DamageEffect, Is.Empty, frightfulPresence.Name);
            Assert.That(frightfulPresence.FrequencyQuantity, Is.Zero, frightfulPresence.Name);
            Assert.That(frightfulPresence.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Constant), frightfulPresence.Name);
            Assert.That(frightfulPresence.IsMelee, Is.False, frightfulPresence.Name);
            Assert.That(frightfulPresence.IsPrimary, Is.False, frightfulPresence.Name);
            Assert.That(frightfulPresence.IsNatural, Is.True, frightfulPresence.Name);
            Assert.That(frightfulPresence.IsSpecial, Is.True, frightfulPresence.Name);
            Assert.That(frightfulPresence.RequiredGender, Is.Empty, frightfulPresence.Name);
            Assert.That(frightfulPresence.Save, Is.EqualTo(SaveConstants.Will), frightfulPresence.Name);
            Assert.That(frightfulPresence.SaveAbility, Is.EqualTo(AbilityConstants.Charisma), frightfulPresence.Name);
            Assert.That(frightfulPresence.SaveDcBonus, Is.Zero, frightfulPresence.Name);
        }

        private bool DragonAgeIsAtLeast(string creature, string minimumAge)
        {
            var dragonAge = creature.Split(',')[1].Trim();
            return GetNumericDragonAgeCategory(dragonAge) >= GetNumericDragonAgeCategory(minimumAge);
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

        private bool SizeIsAtLeast(string creature, string minimumSize)
        {
            var size = creatureData[creature].Size;
            var sizes = SizeConstants.GetOrdered();
            return Array.IndexOf(sizes, size) >= Array.IndexOf(sizes, minimumSize);
        }

        private void AssertSpecialAttacks(List<string> entries)
        {
            var attackTypes = new[]
            {
                "spell-like",
                "supernatural",
                "extraordinary"
            };
            var specialAttacks = entries
                .Select(DataHelper.Parse<AttackDataSelection>)
                .Where(s => attackTypes.Any(a => s.AttackType.Contains(a)));

            foreach (var selection in specialAttacks)
            {
                Assert.That(selection.IsSpecial, Is.True, selection.Name);
            }
        }

        private void AssertPoisonAttacks(List<string> entries)
        {
            var poisons = entries
                .Select(DataHelper.Parse<AttackDataSelection>)
                .Where(s => s.Name.Equals("poison", StringComparison.CurrentCultureIgnoreCase));

            foreach (var selection in poisons)
            {
                Assert.That(selection.IsSpecial, Is.True, "Special");
                Assert.That(selection.IsMelee, Is.True, "Melee");
                Assert.That(selection.IsPrimary, Is.False, "Primary");
                Assert.That(selection.Save, Is.Not.Empty.And.EqualTo(SaveConstants.Fortitude));

                if (selection.IsNatural)
                    Assert.That(selection.SaveAbility, Is.Not.Empty);
                else
                    Assert.That(selection.SaveAbility, Is.Empty);
            }
        }

        private void AssertDiseaseAttacks(List<string> entries)
        {
            var selections = entries.Select(DataHelper.Parse<AttackDataSelection>);

            var disease = selections.FirstOrDefault(s => s.Name.Equals("disease", StringComparison.CurrentCultureIgnoreCase));
            if (disease == null)
                return;

            Assert.That(disease.IsSpecial, Is.True);
            Assert.That(disease.IsPrimary, Is.False);
            Assert.That(disease.SaveAbility, Is.Empty);
            Assert.That(disease.Save, Is.Empty);
            Assert.That(disease.DamageEffect, Is.Not.Empty);

            var specificDisease = selections.FirstOrDefault(s => s.Name == disease.DamageEffect);
            if (specificDisease == null)
                Assert.Fail($"Could not find disease '{disease.DamageEffect}'");

            Assert.That(specificDisease.IsSpecial, Is.True);
            Assert.That(specificDisease.IsPrimary, Is.False);
            Assert.That(specificDisease.SaveAbility, Is.Not.Empty);
            Assert.That(specificDisease.Save, Is.Not.Empty);
        }

        private void CreatureWithSpellLikeAbilityAttack_HasSpellLikeAbilitySpecialQuality(string creature)
        {
            Assert.That(table, Contains.Key(creature));

            var creatureType = GetCreatureType(creature);
            var specialQualities = featsSelector.SelectSpecialQualities(creature, creatureType);
            var hasSpellLikeAbilityAttack = table[creature]
                .Select(DataHelper.Parse<AttackDataSelection>)
                .Any(s => s.Name == FeatConstants.SpecialQualities.SpellLikeAbility);

            //INFO: Want to ignore constant effects such as Doppelganger's Detect Thoughts and Copper Dragon's Spider Climb
            var hasSpellLikeAbilitySpecialQuality = specialQualities.Any(q =>
                q.Feat == FeatConstants.SpecialQualities.SpellLikeAbility
                && q.FrequencyTimePeriod != FeatConstants.Frequencies.Constant);
            Assert.That(hasSpellLikeAbilityAttack, Is.EqualTo(hasSpellLikeAbilitySpecialQuality));
        }

        private CreatureType GetCreatureType(string creatureName) => new(creatureData[creatureName].Types);

        private void CreatureWithPsionicAttack_HasPsionicSpecialQuality(string creature)
        {
            Assert.That(table, Contains.Key(creature));

            var creatureType = GetCreatureType(creature);
            var specialQualities = featsSelector.SelectSpecialQualities(creature, creatureType);
            var hasPsionicAttack = table[creature]
                .Select(DataHelper.Parse<AttackDataSelection>)
                .Any(s => s.Name == FeatConstants.SpecialQualities.Psionic);

            var hasPsionicSpecialQuality = specialQualities.Any(q => q.Feat == FeatConstants.SpecialQualities.Psionic);
            Assert.That(hasPsionicAttack, Is.EqualTo(hasPsionicSpecialQuality));
        }

        private void CreatureWithSpellsAttack_HasMagicSpells(string creature)
        {
            Assert.That(table, Contains.Key(creature));

            var hasSpellsAttack = table[creature]
                .Select(DataHelper.Parse<AttackDataSelection>)
                .Any(s => s.Name == "Spells");

            var caster = collectionSelector.SelectFrom(Config.Name, TableNameConstants.TypeAndAmount.Casters, creature);

            if (hasSpellsAttack)
            {
                Assert.That(caster, Is.Not.Empty);
            }
            else
            {
                Assert.That(caster, Is.Empty);
            }
        }

        private void CreatureWithUnnaturalAttack_CanUseEquipment(string creature)
        {
            Assert.That(table, Contains.Key(creature));

            var selections = table[creature].Select(DataHelper.Parse<AttackDataSelection>);
            var unnaturalAttacks = selections.Where(s => !s.IsNatural);

            if (!unnaturalAttacks.Any())
            {
                return;
            }

            Assert.That(unnaturalAttacks.Any(), Is.True.And.EqualTo(creatureData[creature].CanUseEquipment));

            //Has Natural Attack
            var naturalAttack = selections.FirstOrDefault(s => s.IsNatural && !s.IsSpecial);

            Assert.That(naturalAttack, Is.Not.Null);
            Assert.That(naturalAttack.Name, Is.Not.Empty);
            Assert.That(naturalAttack.IsNatural, Is.True, naturalAttack.Name);
            Assert.That(naturalAttack.IsSpecial, Is.False, naturalAttack.Name);
        }

        private void AssertSpecialPhysicalAttack(List<string> entries, string name, double? multiplier, int? frequency)
        {
            var selections = entries.Select(DataHelper.Parse<AttackDataSelection>);

            var selection = selections.FirstOrDefault(s => s.Name == name);
            if (selection == null)
            {
                return;
            }

            Assert.That(selection.AttackType, Is.EqualTo("extraordinary ability"), name);

            if (multiplier.HasValue)
                Assert.That(selection.DamageBonusMultiplier, Is.EqualTo(multiplier), name);

            if (frequency.HasValue)
                Assert.That(selection.FrequencyQuantity, Is.EqualTo(frequency), name);

            Assert.That(selection.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), name);
            Assert.That(selection.IsMelee, Is.True, name);
            Assert.That(selection.IsNatural, Is.True, name);
            Assert.That(selection.IsPrimary, Is.False, name);
            Assert.That(selection.IsSpecial, Is.True, name);
            Assert.That(selection.Name, Is.EqualTo(name), name);
            Assert.That(selection.SaveAbility, Is.Empty, name);
            Assert.That(selection.SaveDcBonus, Is.Zero, name);
            Assert.That(selection.Save, Is.Empty, name);
        }

        private void AssertImprovedGrabAttack(List<string> entries) => AssertSpecialPhysicalAttack(entries, "Improved Grab", 0, 1);
        // INFO: Salamanders constrict with bonus 0.5, while others constrict with bonus 1.5
        private void AssertConstrictAttack(List<string> entries) => AssertSpecialPhysicalAttack(entries, "Constrict", null, 1);
        // INFO: Behir get 6 rake attacks, while other animals only get 2
        // INFO: Annis Rakes with bonus 1, while other animals rake with bonus 0.5
        private void AssertRakeAttack(List<string> entries) => AssertSpecialPhysicalAttack(entries, "Rake", null, null);
        private void AssertRendAttack(List<string> entries) => AssertSpecialPhysicalAttack(entries, "Rend", 1.5, 1);
        private void AssertSneakAttack(List<string> entries) => AssertSpecialPhysicalAttack(entries, "Sneak Attack", 0, 1);

        private void AssertRageAttack(List<string> entries)
        {
            var selections = entries.Select(DataHelper.Parse<AttackDataSelection>);
            var name = "Rage";

            var selection = selections.FirstOrDefault(s => s.Name == name);
            if (selection == null)
            {
                return;
            }

            Assert.That(selection.AttackType, Is.EqualTo("extraordinary ability"), name);
            Assert.That(selection.DamageBonusMultiplier, Is.Zero, name);
            Assert.That(selection.FrequencyQuantity, Is.EqualTo(1), name);
            Assert.That(selection.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), name);
            Assert.That(selection.IsMelee, Is.False, name);
            Assert.That(selection.IsNatural, Is.True, name);
            Assert.That(selection.IsPrimary, Is.False, name);
            Assert.That(selection.IsSpecial, Is.True, name);
            Assert.That(selection.Name, Is.EqualTo(name), name);
            Assert.That(selection.SaveAbility, Is.Empty, name);
            Assert.That(selection.SaveDcBonus, Is.Zero, name);
            Assert.That(selection.Save, Is.Empty, name);
        }

        private void AssertMagicAttack(List<string> entries, string name)
        {
            var selections = entries.Select(DataHelper.Parse<AttackDataSelection>);

            var selection = selections.FirstOrDefault(s => s.Name == name);
            if (selection == null)
            {
                return;
            }

            Assert.That(selection.AttackType, Is.EqualTo("spell-like ability"), name);
            Assert.That(selection.DamageBonusMultiplier, Is.Zero, name);
            Assert.That(selection.DamageEffect, Is.Empty, name);
            Assert.That(selection.FrequencyQuantity, Is.EqualTo(1), name);
            Assert.That(selection.FrequencyTimePeriod, Is.EqualTo(FeatConstants.Frequencies.Round), name);
            Assert.That(selection.IsMelee, Is.False, name);
            Assert.That(selection.IsNatural, Is.True, name);
            Assert.That(selection.IsPrimary, Is.True, name);
            Assert.That(selection.IsSpecial, Is.True, name);
            Assert.That(selection.Name, Is.EqualTo(name), name);
            Assert.That(selection.SaveAbility, Is.Empty, name);
            Assert.That(selection.SaveDcBonus, Is.Zero, name);
            Assert.That(selection.Save, Is.Empty, name);
        }

        private void AssertSpellsAttack(List<string> entries) => AssertMagicAttack(entries, "Spells");
        private void AssertSpellLikeAbilityAttack(List<string> entries) => AssertMagicAttack(entries, FeatConstants.SpecialQualities.SpellLikeAbility);
        private void AssertPsionicAttack(List<string> entries) => AssertMagicAttack(entries, FeatConstants.SpecialQualities.Psionic);
    }
}
