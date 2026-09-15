using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Skills;
using DnDGen.CreatureGen.Tables;
using DnDGen.Infrastructure.Helpers;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Feats.Data
{
    [TestFixture]
    public class FeatDataTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.FeatData;

        private Dictionary<string, Dictionary<string, int>> requiredAbilities;
        private Dictionary<string, IEnumerable<FeatDataSelection.RequiredFeatDataSelection>> requiredFeats;
        private Dictionary<string, IEnumerable<FeatDataSelection.RequiredSkillDataSelection>> requiredSkills;
        private Dictionary<string, string[]> requiredSizes;
        private Dictionary<string, Dictionary<string, int>> requiredSpeeds;
        private IEnumerable<string> featsTakenMultipleTimes;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            requiredAbilities = GetRequiredAbilities();
            requiredFeats = GetRequiredFeats();
            requiredSkills = GetRequiredSkills();
            requiredSizes = GetRequiredSizes();
            requiredSpeeds = GetRequiredSpeeds();

            featsTakenMultipleTimes =
                [//FeatConstants.SpellMastery,
                FeatConstants.Toughness,
                //FeatConstants.Turning_Extra,
                FeatConstants.Monster.NaturalArmor_Improved,
                FeatConstants.SpecialQualities.AttackBonus,
                FeatConstants.SpecialQualities.DodgeBonus];
        }

        [Test]
        public void FeatDataNames()
        {
            var feats = FeatConstants.All();
            var monsterFeats = FeatConstants.Monster.All();
            var craftFeats = FeatConstants.MagicItemCreation.All();
            var metamagicFeats = FeatConstants.Metamagic.All();
            var featsWithFoci = GetFeatsWithFociAbilityRequirementNames();

            var names = feats
                .Union(monsterFeats)
                .Union(craftFeats)
                .Union(metamagicFeats)
                .Union(featsWithFoci);

            AssertCollectionNames(names);
        }

        [TestCase(FeatConstants.Acrobatic, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Agile, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Alertness, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.AnimalAffinity, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.ArmorProficiency_Heavy, 0, "", 0, "", 0, 0, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.ArmorProficiency_Light, 0, "", 0, "", 0, 0, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.ArmorProficiency_Medium, 0, "", 0, "", 0, 0, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.Athletic, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.AugmentSummoning, 0, "", 0, "", 4, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.BlindFight, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.BullRush_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Cleave, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Cleave_Great, 4, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.CombatCasting, 0, "", 0, "", 4, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.CombatExpertise, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.CombatReflexes, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Counterspell_Improved, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Critical_Improved, 8, FeatConstants.Foci.Weapon, 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Deceitful, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.DeflectArrows, 0, "", 1, FeatConstants.Frequencies.Round, 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.DeftHands, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Diehard, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Diligent, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Disarm_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Dodge, 0, "", 0, "", 1, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Endurance, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.EschewMaterials, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        //INFO: Creatures cannot get familiars without being a character class
        //[TestCase(FeatConstants.Familiar_Improved, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.FarShot, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Feint_Improved, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Grapple_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.GreatFortitude, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Initiative_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Investigator, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.IronWill, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        //INFO: Requires character level 6
        //[TestCase(FeatConstants.Leadership, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.LightningReflexes, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.MagicalAptitude, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Manyshot, 6, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Mobility, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.MountedArchery, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.MountedCombat, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        //INFO: Wild Shape, a prerequisite, is only had by Druid classes
        //[TestCase(FeatConstants.NaturalSpell, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Negotiator, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.NimbleFingers, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Overrun_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Persuasive, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.PointBlankShot, 0, "", 0, "", 1, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.PowerAttack, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.PreciseShot, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.PreciseShot_Improved, 11, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.QuickDraw, 1, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.RapidReload, 0, GroupConstants.ManualCrossbows, 0, "", 0, 0, 2, 0, false, false, false, true)]
        [TestCase(FeatConstants.RapidShot, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.RideByAttack, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Run, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SelfSufficient, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.ShieldProficiency, 0, "", 0, "", 0, 0, 1, 0, false, false, false, true)]
        [TestCase(FeatConstants.ShieldProficiency_Tower, 0, "", 0, "", 0, 0, 1, 0, false, false, false, true)]
        [TestCase(FeatConstants.ShieldBash_Improved, 0, "", 0, "", 0, 0, 1, 0, false, false, false, true)]
        [TestCase(FeatConstants.ShotOnTheRun, 4, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SkillFocus, 0, GroupConstants.Skills, 0, "", 3, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SnatchArrows, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SpellFocus, 0, FeatConstants.Foci.School, 0, "", 1, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SpellFocus_Greater, 0, FeatConstants.Foci.School, 0, "", 1, 1, 0, 0, false, false, false, false)]
        //INFO: Spell Mastery requires being a Wizard Level 1
        //[TestCase(FeatConstants.SpellMastery, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SpellPenetration, 0, "", 0, "", 2, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SpellPenetration_Greater, 0, "", 0, "", 2, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SpiritedCharge, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.SpringAttack, 4, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Stealthy, 0, "", 0, "", 2, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.StunningFist, 8, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Sunder_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Toughness, 0, "", 0, "", 3, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Track, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Trample, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Trip_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false, false)]
        //INFO: No monsters can natively turn or rebuke
        //[TestCase(FeatConstants.Turning_Extra, 0, "", 0, "", 4, 0, 0, 0, false, false, false)]
        //[TestCase(FeatConstants.Turning_Improved, 0, "", 0, "", 4, 0, 0, 0, false, false, false)]
        [TestCase(FeatConstants.TwoWeaponFighting_Greater, 11, "", 0, "", 0, 0, 2, 0, false, false, false, true)]
        [TestCase(FeatConstants.TwoWeaponFighting_Improved, 6, "", 0, "", 0, 0, 2, 0, false, false, false, true)]
        [TestCase(FeatConstants.TwoWeaponDefense, 0, "", 0, "", 0, 0, 2, 0, false, false, false, true)]
        [TestCase(FeatConstants.TwoWeaponFighting, 0, "", 0, "", 0, 0, 2, 0, false, false, false, true)]
        [TestCase(FeatConstants.UnarmedStrike_Improved, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.WeaponFinesse, 1, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        //INFO: Being a Fighter is a requirement for these feats
        [TestCase(FeatConstants.WeaponFocus, 0, FeatConstants.Foci.Weapon, 0, "", 0, 0, 0, 0, false, false, false, false)]
        //[TestCase(FeatConstants.WeaponFocus_Greater, 0, FeatConstants.Foci.Weapon, 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.WeaponProficiency_Exotic, 1, GroupConstants.All, 0, "", 0, 0, 1, 0, false, false, false, true)]
        [TestCase(FeatConstants.WeaponProficiency_Martial, 0, GroupConstants.All, 0, "", 0, 0, 1, 0, false, false, false, true)]
        [TestCase(FeatConstants.WeaponProficiency_Simple, 0, GroupConstants.All, 0, "", 0, 0, 1, 0, false, false, false, true)]
        //INFO: Being a Fighter is a requirement for these feats
        //[TestCase(FeatConstants.WeaponSpecialization, 0, FeatConstants.Foci.Weapon, 0, "", 0, 0, 0, 0, false, false, false, true)]
        //[TestCase(FeatConstants.WeaponSpecialization_Greater, 0, FeatConstants.Foci.Weapon, 0, "", 0, 0, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.WhirlwindAttack, 4, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.MagicItemCreation.BrewPotion, 0, "", 0, "", 0, 3, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.CraftMagicArmsAndArmor, 0, "", 0, "", 0, 5, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.CraftRod, 0, "", 0, "", 0, 9, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.CraftStaff, 0, "", 0, "", 0, 12, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.CraftWand, 0, "", 0, "", 0, 5, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.CraftWondrousItem, 0, "", 0, "", 0, 3, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.ForgeRing, 0, "", 0, "", 0, 12, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.MagicItemCreation.ScribeScroll, 0, "", 0, "", 0, 1, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.Metamagic.EmpowerSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.EnlargeSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.ExtendSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.HeightenSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.MaximizeSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.QuickenSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.SilentSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.StillSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Metamagic.WidenSpell, 0, "", 0, "", 0, 1, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Monster.AbilityFocus, 0, "", 0, "", 2, 0, 0, 0, false, true, false, false)]
        [TestCase(FeatConstants.Monster.AwesomeBlow, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Monster.CraftConstruct, 0, "", 0, "", 0, 0, 0, 0, false, false, false, true)]
        [TestCase(FeatConstants.Monster.EmpowerSpellLikeAbility, 0, "", 3, FeatConstants.Frequencies.Day, 0, 6, 0, 0, false, false, true, false)]
        [TestCase(FeatConstants.Monster.FlybyAttack, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Monster.FlybyAttack_Improved, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Monster.Hover, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Monster.Multiattack, 0, "", 0, "", 0, 0, 0, 3, false, false, false, false)]
        [TestCase(FeatConstants.Monster.Multiattack_Improved, 0, "", 0, "", 0, 0, 0, 3, false, false, false, false)]
        [TestCase(FeatConstants.Monster.MultiweaponFighting, 0, "", 0, "", 0, 0, 3, 0, false, false, false, true)]
        [TestCase(FeatConstants.Monster.MultiweaponFighting_Greater, 15, "", 0, "", 0, 0, 3, 0, false, false, false, true)]
        [TestCase(FeatConstants.Monster.MultiweaponFighting_Improved, 9, "", 0, "", 0, 0, 3, 0, false, false, false, true)]
        [TestCase(FeatConstants.Monster.NaturalArmor_Improved, 0, "", 0, "", 1, 0, 0, 0, true, false, false, false)]
        [TestCase(FeatConstants.Monster.NaturalAttack_Improved, 4, "", 0, "", 0, 0, 0, 1, false, false, false, false)]
        [TestCase(FeatConstants.Monster.QuickenSpellLikeAbility, 0, "", 3, FeatConstants.Frequencies.Day, 0, 10, 0, 0, false, false, true, false)]
        [TestCase(FeatConstants.Monster.Snatch, 0, "", 0, "", 0, 0, 0, 0, false, false, false, false)]
        [TestCase(FeatConstants.Monster.Wingover, 0, "", 1, FeatConstants.Frequencies.Round, 0, 0, 0, 0, false, false, false, false)]
        public void FeatData(
            string name,
            int baseAttackRequirement,
            string focusType,
            int frequencyQuantity,
            string frequencyTimePeriod,
            int power,
            int casterLevel,
            int hands,
            int naturalWeapons,
            bool naturalArmor,
            bool specialAttacks,
            bool spellLikeAbility,
            bool equipment)
        {
            var data = DataHelper.Parse(new FeatDataSelection
            {
                Feat = name,
                RequiredBaseAttack = baseAttackRequirement,
                FocusType = focusType,
                FrequencyQuantity = frequencyQuantity,
                FrequencyTimePeriod = frequencyTimePeriod,
                Power = power,
                MinimumCasterLevel = casterLevel,
                RequiredHands = hands,
                RequiredNaturalWeapons = naturalWeapons,
                RequiresNaturalArmor = naturalArmor,
                RequiresSpecialAttack = specialAttacks,
                RequiresSpellLikeAbility = spellLikeAbility,
                RequiresEquipment = equipment,
                RequiredAbilities = requiredAbilities.ContainsKey(name) ? requiredAbilities[name] : [],
                RequiredFeats = requiredFeats.ContainsKey(name) ? requiredFeats[name] : [],
                RequiredSkills = requiredSkills.ContainsKey(name) ? requiredSkills[name] : [],
                RequiredSizes = requiredSizes.ContainsKey(name) ? requiredSizes[name] : [],
                RequiredSpeeds = requiredSpeeds.ContainsKey(name) ? requiredSpeeds[name] : [],
                CanBeTakenMultipleTimes = featsTakenMultipleTimes.Contains(name),
            });

            AssertCollection(name, data);
        }

        [TestCase(FeatConstants.WeaponProficiency_Exotic, WeaponConstants.BastardSword)]
        [TestCase(FeatConstants.WeaponProficiency_Exotic, WeaponConstants.DwarvenWaraxe)]
        [TestCase(FeatConstants.WeaponProficiency_Martial, WeaponConstants.DwarvenWaraxe)]
        public void FeatData_FeatWithFocus(string name, string focus)
        {
            var nameWithFocus = name + focus;
            var selection = DataHelper.Parse<FeatDataSelection>(table[name].Single());
            selection.RequiredAbilities = requiredAbilities.ContainsKey(nameWithFocus) ? requiredAbilities[nameWithFocus] : [];

            var data = DataHelper.Parse(selection);
            AssertCollection(nameWithFocus, data);
        }

        private Dictionary<string, Dictionary<string, int>> GetRequiredAbilities()
        {
            var abilityRequirements = new Dictionary<string, Dictionary<string, int>>
            {
                [FeatConstants.BullRush_Improved] = new() { [AbilityConstants.Strength] = 13 },
                [FeatConstants.Cleave] = new() { [AbilityConstants.Strength] = 13 },
                [FeatConstants.Cleave_Great] = new() { [AbilityConstants.Strength] = 13 },
                [FeatConstants.CombatExpertise] = new() { [AbilityConstants.Intelligence] = 13 },
                [FeatConstants.DeflectArrows] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.Disarm_Improved] = new() { [AbilityConstants.Intelligence] = 13 },
                [FeatConstants.Dodge] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.Feint_Improved] = new() { [AbilityConstants.Intelligence] = 13 },
                [FeatConstants.Grapple_Improved] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.Manyshot] = new() { [AbilityConstants.Dexterity] = 17 },
                [FeatConstants.Mobility] = new() { [AbilityConstants.Dexterity] = 13 },
                //INFO: Natural Spell is only available to Druids
                //testCases[FeatConstants.NaturalSpell][AbilityConstants.Wisdom] = 13;
                [FeatConstants.Overrun_Improved] = new() { [AbilityConstants.Strength] = 13 },
                [FeatConstants.PowerAttack] = new() { [AbilityConstants.Strength] = 13 },
                [FeatConstants.PreciseShot_Improved] = new() { [AbilityConstants.Dexterity] = 19 },
                [FeatConstants.RapidShot] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.ShotOnTheRun] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.SnatchArrows] = new() { [AbilityConstants.Dexterity] = 15 },
                [FeatConstants.SpringAttack] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.StunningFist] = new() { [AbilityConstants.Dexterity] = 13, [AbilityConstants.Wisdom] = 13 },
                [FeatConstants.Sunder_Improved] = new() { [AbilityConstants.Strength] = 13 },
                [FeatConstants.Trip_Improved] = new() { [AbilityConstants.Intelligence] = 13 },
                [FeatConstants.TwoWeaponDefense] = new() { [AbilityConstants.Dexterity] = 15 },
                [FeatConstants.TwoWeaponFighting] = new() { [AbilityConstants.Dexterity] = 15 },
                [FeatConstants.TwoWeaponFighting_Greater] = new() { [AbilityConstants.Dexterity] = 19 },
                [FeatConstants.TwoWeaponFighting_Improved] = new() { [AbilityConstants.Dexterity] = 17 },
                [FeatConstants.WhirlwindAttack] = new() { [AbilityConstants.Dexterity] = 13, [AbilityConstants.Intelligence] = 13 },

                [FeatConstants.Monster.AwesomeBlow] = new() { [AbilityConstants.Strength] = 25 },
                [FeatConstants.Monster.MultiweaponFighting] = new() { [AbilityConstants.Dexterity] = 13 },
                [FeatConstants.Monster.MultiweaponFighting_Greater] = new() { [AbilityConstants.Dexterity] = 19 },
                [FeatConstants.Monster.MultiweaponFighting_Improved] = new() { [AbilityConstants.Dexterity] = 15 },
                [FeatConstants.Monster.NaturalArmor_Improved] = new() { [AbilityConstants.Constitution] = 13 },

                [$"{FeatConstants.WeaponProficiency_Exotic}/{WeaponConstants.BastardSword}"] = new() { [AbilityConstants.Strength] = 13 },
                [$"{FeatConstants.WeaponProficiency_Exotic}/{WeaponConstants.DwarvenWaraxe}"] = new() { [AbilityConstants.Strength] = 13 },
                [$"{FeatConstants.WeaponProficiency_Martial}/{WeaponConstants.DwarvenWaraxe}"] = new() { [AbilityConstants.Strength] = 13 },
            };

            return abilityRequirements;
        }

        private Dictionary<string, IEnumerable<FeatDataSelection.RequiredFeatDataSelection>> GetRequiredFeats()
        {
            var requiredFeats = new Dictionary<string, IEnumerable<FeatDataSelection.RequiredFeatDataSelection>>
            {
                [FeatConstants.ArmorProficiency_Heavy] = [new() { Feat = FeatConstants.ArmorProficiency_Light }, new() { Feat = FeatConstants.ArmorProficiency_Medium }],
                [FeatConstants.ArmorProficiency_Medium] = [new() { Feat = FeatConstants.ArmorProficiency_Light }],
                [FeatConstants.AugmentSummoning] = [new() { Feat = FeatConstants.SpellFocus, Foci = [FeatConstants.Foci.Schools.Conjuration] }],
                [FeatConstants.BullRush_Improved] = [new() { Feat = FeatConstants.PowerAttack }],
                [FeatConstants.Cleave] = [new() { Feat = FeatConstants.PowerAttack }],
                [FeatConstants.Cleave_Great] = [new() { Feat = FeatConstants.Cleave }, new() { Feat = FeatConstants.PowerAttack }],
                [FeatConstants.Critical_Improved] = [new() { Feat = GroupConstants.WeaponProficiency }],
                [FeatConstants.DeflectArrows] = [new() { Feat = FeatConstants.UnarmedStrike_Improved }],
                [FeatConstants.Diehard] = [new() { Feat = FeatConstants.Endurance }],
                [FeatConstants.Disarm_Improved] = [new() { Feat = FeatConstants.CombatExpertise }],
                [FeatConstants.FarShot] = [new() { Feat = FeatConstants.PointBlankShot }],
                [FeatConstants.Feint_Improved] = [new() { Feat = FeatConstants.CombatExpertise }],
                [FeatConstants.Grapple_Improved] = [new() { Feat = FeatConstants.UnarmedStrike_Improved }],
                [FeatConstants.Manyshot] = [new() { Feat = FeatConstants.PointBlankShot }, new() { Feat = FeatConstants.RapidShot }],
                [FeatConstants.Mobility] = [new() { Feat = FeatConstants.Dodge }],
                [FeatConstants.MountedArchery] = [new() { Feat = FeatConstants.MountedCombat }],
                //INFO: Wild Shape is only had by Druid classes
                //testCases[FeatConstants.NaturalSpell] = new string[1] { FeatConstants.WildShape };
                [FeatConstants.Overrun_Improved] = [new() { Feat = FeatConstants.PowerAttack }],
                [FeatConstants.PreciseShot] = [new() { Feat = FeatConstants.PointBlankShot }],
                [FeatConstants.PreciseShot_Improved] = [new() { Feat = FeatConstants.PointBlankShot }, new() { Feat = FeatConstants.PreciseShot }],
                [FeatConstants.RapidReload] = [new() { Feat = GroupConstants.WeaponProficiency }],
                [FeatConstants.RapidShot] = [new() { Feat = FeatConstants.PointBlankShot }],
                [FeatConstants.RideByAttack] = [new() { Feat = FeatConstants.MountedCombat }],
                [FeatConstants.ShieldBash_Improved] = [new() { Feat = FeatConstants.ShieldProficiency }],
                [FeatConstants.ShieldProficiency_Tower] = [new() { Feat = FeatConstants.ShieldProficiency }],
                [FeatConstants.ShotOnTheRun] = [new() { Feat = FeatConstants.Dodge }, new() { Feat = FeatConstants.Mobility }, new() { Feat = FeatConstants.PointBlankShot }],
                [FeatConstants.SnatchArrows] = [new() { Feat = FeatConstants.DeflectArrows }, new() { Feat = FeatConstants.UnarmedStrike_Improved }],
                [FeatConstants.SpellFocus_Greater] = [new() { Feat = FeatConstants.SpellFocus }],
                [FeatConstants.SpellPenetration_Greater] = [new() { Feat = FeatConstants.SpellPenetration }],
                [FeatConstants.SpiritedCharge] = [new() { Feat = FeatConstants.MountedCombat }, new() { Feat = FeatConstants.RideByAttack }],
                [FeatConstants.SpringAttack] = [new() { Feat = FeatConstants.Dodge }, new() { Feat = FeatConstants.Mobility }],
                [FeatConstants.StunningFist] = [new() { Feat = FeatConstants.UnarmedStrike_Improved }],
                [FeatConstants.Sunder_Improved] = [new() { Feat = FeatConstants.PowerAttack }],
                [FeatConstants.Trample] = [new() { Feat = FeatConstants.MountedCombat }],
                [FeatConstants.Trip_Improved] = [new() { Feat = FeatConstants.CombatExpertise }],
                //INFO: No monsters can natively turn or rebuke
                //testCases[FeatConstants.Turning_Extra] = new string[1] { FeatConstants.Turn };
                //testCases[FeatConstants.Turning_Improved] = new string[1] { FeatConstants.Turn };
                [FeatConstants.TwoWeaponDefense] = [new() { Feat = FeatConstants.TwoWeaponFighting }],
                [FeatConstants.TwoWeaponFighting_Greater] = [new() { Feat = FeatConstants.TwoWeaponFighting_Improved }, new() { Feat = FeatConstants.TwoWeaponFighting }],
                [FeatConstants.TwoWeaponFighting_Improved] = [new() { Feat = FeatConstants.TwoWeaponFighting }],
                //INFO: Being a Fighter is a requirement for these feats
                [FeatConstants.WeaponFocus] = [new() { Feat = GroupConstants.WeaponProficiency }],
                //testCases[FeatConstants.WeaponFocus_Greater] = new string[1] { FeatConstants.WeaponFocus };
                //INFO: Being a Fighter is a requirement for these feats
                //testCases[FeatConstants.WeaponSpecialization_Greater] = new string[1] { FeatConstants.WeaponSpecialization };
                [FeatConstants.WhirlwindAttack] =
                    [new() { Feat = FeatConstants.CombatExpertise },
                    new() { Feat = FeatConstants.Dodge },
                    new() { Feat = FeatConstants.Mobility },
                    new() { Feat = FeatConstants.SpringAttack }],

                [FeatConstants.Monster.AwesomeBlow] = [new() { Feat = FeatConstants.PowerAttack }, new() { Feat = FeatConstants.BullRush_Improved }],
                [FeatConstants.Monster.CraftConstruct] =
                    [new() { Feat = FeatConstants.MagicItemCreation.CraftMagicArmsAndArmor },
                    new() { Feat = FeatConstants.MagicItemCreation.CraftWondrousItem }],
                [FeatConstants.Monster.FlybyAttack_Improved] =
                    [new() { Feat = FeatConstants.Dodge },
                    new() { Feat = FeatConstants.Mobility },
                    new() { Feat = FeatConstants.Monster.FlybyAttack }],
                [FeatConstants.Monster.Multiattack_Improved] = [new() { Feat = FeatConstants.Monster.Multiattack }],
                [FeatConstants.Monster.MultiweaponFighting_Greater] =
                    [new() { Feat = FeatConstants.Monster.MultiweaponFighting },
                    new() { Feat = FeatConstants.Monster.MultiweaponFighting_Improved }],
                [FeatConstants.Monster.MultiweaponFighting_Improved] = [new() { Feat = FeatConstants.Monster.MultiweaponFighting }],
            };

            return requiredFeats;
        }

        private Dictionary<string, IEnumerable<FeatDataSelection.RequiredSkillDataSelection>> GetRequiredSkills()
        {
            var requiredSkills = new Dictionary<string, IEnumerable<FeatDataSelection.RequiredSkillDataSelection>>
            {
                [FeatConstants.MountedArchery] = [new() { Skill = SkillConstants.Ride, Ranks = 1 }],
                [FeatConstants.MountedCombat] = [new() { Skill = SkillConstants.Ride, Ranks = 1 }],
                [FeatConstants.RideByAttack] = [new() { Skill = SkillConstants.Ride, Ranks = 1 }],
                [FeatConstants.SpiritedCharge] = [new() { Skill = SkillConstants.Ride, Ranks = 1 }],
                [FeatConstants.Trample] = [new() { Skill = SkillConstants.Ride, Ranks = 1 }]
            };

            return requiredSkills;
        }


        private Dictionary<string, string[]> GetRequiredSizes()
        {
            var requiredSizes = new Dictionary<string, string[]>
            {
                [FeatConstants.Monster.AwesomeBlow] = [SizeConstants.Large, SizeConstants.Huge, SizeConstants.Gargantuan, SizeConstants.Colossal],
                [FeatConstants.Monster.Snatch] = [SizeConstants.Huge, SizeConstants.Gargantuan, SizeConstants.Colossal]
            };

            return requiredSizes;
        }

        private Dictionary<string, Dictionary<string, int>> GetRequiredSpeeds()
        {
            var requiredSpeeds = new Dictionary<string, Dictionary<string, int>>
            {
                [FeatConstants.Monster.FlybyAttack] = new() { [SpeedConstants.Fly] = 1 },
                [FeatConstants.Monster.FlybyAttack_Improved] = new() { [SpeedConstants.Fly] = 1 },
                [FeatConstants.Monster.Hover] = new() { [SpeedConstants.Fly] = 1 },
                [FeatConstants.Monster.Wingover] = new() { [SpeedConstants.Fly] = 1 },
            };

            return requiredSpeeds;
        }

        private IEnumerable<string> GetFeatsWithFociAbilityRequirementNames()
        {
            return
            [
                FeatConstants.WeaponProficiency_Exotic + WeaponConstants.BastardSword,
                FeatConstants.WeaponProficiency_Exotic + WeaponConstants.DwarvenWaraxe,
                FeatConstants.WeaponProficiency_Martial + WeaponConstants.DwarvenWaraxe,
            ];
        }
    }
}