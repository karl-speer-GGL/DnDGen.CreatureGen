using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Magics
{
    [TestFixture]
    public class SpellLevelsTests : TypesAndAmountsTests
    {
        protected override string TableName => TableNameConstants.TypeAndAmount.SpellLevels;

        [Test]
        public void SpellLevelsHaveAllNames()
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

            var names = casters.Union(domains);
            AssertCollectionNames(names);
        }

        [Test]
        public void BardSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            //0-Level Bard Spells (Cantrips)
            spellLevels[SpellConstants.DancingLights] = 0;
            spellLevels[SpellConstants.Daze] = 0;
            spellLevels[SpellConstants.DetectMagic] = 0;
            spellLevels[SpellConstants.Flare] = 0;
            spellLevels[SpellConstants.GhostSound] = 0;
            spellLevels[SpellConstants.KnowDirection] = 0;
            spellLevels[SpellConstants.Light] = 0;
            spellLevels[SpellConstants.Lullaby] = 0;
            spellLevels[SpellConstants.MageHand] = 0;
            spellLevels[SpellConstants.Mending] = 0;
            spellLevels[SpellConstants.Message] = 0;
            spellLevels[SpellConstants.OpenClose] = 0;
            spellLevels[SpellConstants.Prestidigitation] = 0;
            spellLevels[SpellConstants.ReadMagic] = 0;
            spellLevels[SpellConstants.Resistance] = 0;
            spellLevels[SpellConstants.SummonInstrument] = 0;

            //1st-Level Bard Spells
            spellLevels[SpellConstants.Alarm] = 1;
            spellLevels[SpellConstants.AnimateRope] = 1;
            spellLevels[SpellConstants.CauseFear] = 1;
            spellLevels[SpellConstants.CharmPerson] = 1;
            spellLevels[SpellConstants.ComprehendLanguages] = 1;
            spellLevels[SpellConstants.Confusion_Lesser] = 1;
            spellLevels[SpellConstants.CureLightWounds] = 1;
            spellLevels[SpellConstants.DetectSecretDoors] = 1;
            spellLevels[SpellConstants.DisguiseSelf] = 1;
            spellLevels[SpellConstants.Erase] = 1;
            spellLevels[SpellConstants.ExpeditiousRetreat] = 1;
            spellLevels[SpellConstants.FeatherFall] = 1;
            spellLevels[SpellConstants.Grease] = 1;
            spellLevels[SpellConstants.HideousLaughter] = 1;
            spellLevels[SpellConstants.Hypnotism] = 1;
            spellLevels[SpellConstants.Identify] = 1;
            spellLevels[SpellConstants.MagicMouth] = 1;
            spellLevels[SpellConstants.MagicAura] = 1;
            spellLevels[SpellConstants.ObscureObject] = 1;
            spellLevels[SpellConstants.RemoveFear] = 1;
            spellLevels[SpellConstants.SilentImage] = 1;
            spellLevels[SpellConstants.Sleep] = 1;
            spellLevels[SpellConstants.SummonMonsterI] = 1;
            spellLevels[SpellConstants.UndetectableAlignment] = 1;
            spellLevels[SpellConstants.UnseenServant] = 1;
            spellLevels[SpellConstants.Ventriloquism] = 1;

            //2nd-Level Bard Spells
            spellLevels[SpellConstants.AlterSelf] = 2;
            spellLevels[SpellConstants.AnimalMessenger] = 2;
            spellLevels[SpellConstants.AnimalTrance] = 2;
            spellLevels[SpellConstants.BlindnessDeafness] = 2;
            spellLevels[SpellConstants.Blur] = 2;
            spellLevels[SpellConstants.CalmEmotions] = 2;
            spellLevels[SpellConstants.CatsGrace] = 2;
            spellLevels[SpellConstants.CureModerateWounds] = 2;
            spellLevels[SpellConstants.Darkness] = 2;
            spellLevels[SpellConstants.DazeMonster] = 2;
            spellLevels[SpellConstants.DelayPoison] = 2;
            spellLevels[SpellConstants.DetectThoughts] = 2;
            spellLevels[SpellConstants.EaglesSplendor] = 2;
            spellLevels[SpellConstants.Enthrall] = 2;
            spellLevels[SpellConstants.FoxsCunning] = 2;
            spellLevels[SpellConstants.Glitterdust] = 2;
            spellLevels[SpellConstants.Heroism] = 2;
            spellLevels[SpellConstants.HoldPerson] = 2;
            spellLevels[SpellConstants.HypnoticPattern] = 2;
            spellLevels[SpellConstants.Invisibility] = 2;
            spellLevels[SpellConstants.LocateObject] = 2;
            spellLevels[SpellConstants.MinorImage] = 2;
            spellLevels[SpellConstants.MirrorImage] = 2;
            spellLevels[SpellConstants.Misdirection] = 2;
            spellLevels[SpellConstants.Pyrotechnics] = 2;
            spellLevels[SpellConstants.Rage] = 2;
            spellLevels[SpellConstants.Scare] = 2;
            spellLevels[SpellConstants.Shatter] = 2;
            spellLevels[SpellConstants.Silence] = 2;
            spellLevels[SpellConstants.SoundBurst] = 2;
            spellLevels[SpellConstants.Suggestion] = 2;
            spellLevels[SpellConstants.SummonMonsterII] = 2;
            spellLevels[SpellConstants.SummonSwarm] = 2;
            spellLevels[SpellConstants.Tongues] = 2;
            spellLevels[SpellConstants.WhisperingWind] = 2;

            //3rd-Level Bard Spells
            spellLevels[SpellConstants.Blink] = 3;
            spellLevels[SpellConstants.CharmMonster] = 3;
            spellLevels[SpellConstants.ClairaudienceClairvoyance] = 3;
            spellLevels[SpellConstants.Confusion] = 3;
            spellLevels[SpellConstants.CrushingDespair] = 3;
            spellLevels[SpellConstants.CureSeriousWounds] = 3;
            spellLevels[SpellConstants.Daylight] = 3;
            spellLevels[SpellConstants.DeepSlumber] = 3;
            spellLevels[SpellConstants.DispelMagic] = 3;
            spellLevels[SpellConstants.Displacement] = 3;
            spellLevels[SpellConstants.Fear] = 3;
            spellLevels[SpellConstants.GaseousForm] = 3;
            spellLevels[SpellConstants.Geas_Lesser] = 3;
            spellLevels[SpellConstants.Glibness] = 3;
            spellLevels[SpellConstants.GoodHope] = 3;
            spellLevels[SpellConstants.Haste] = 3;
            spellLevels[SpellConstants.IllusoryScript] = 3;
            spellLevels[SpellConstants.InvisibilitySphere] = 3;
            spellLevels[SpellConstants.MajorImage] = 3;
            spellLevels[SpellConstants.PhantomSteed] = 3;
            spellLevels[SpellConstants.RemoveCurse] = 3;
            spellLevels[SpellConstants.Scrying] = 3;
            spellLevels[SpellConstants.SculptSound] = 3;
            spellLevels[SpellConstants.SecretPage] = 3;
            spellLevels[SpellConstants.SeeInvisibility] = 3;
            spellLevels[SpellConstants.SepiaSnakeSigil] = 3;
            spellLevels[SpellConstants.Slow] = 3;
            spellLevels[SpellConstants.SpeakWithAnimals] = 3;
            spellLevels[SpellConstants.SummonMonsterIII] = 3;
            spellLevels[SpellConstants.TinyHut] = 3;

            //4th-Level Bard Spells
            spellLevels[SpellConstants.BreakEnchantment] = 4;
            spellLevels[SpellConstants.CureCriticalWounds] = 4;
            spellLevels[SpellConstants.DetectScrying] = 4;
            spellLevels[SpellConstants.DimensionDoor] = 4;
            spellLevels[SpellConstants.DominatePerson] = 4;
            spellLevels[SpellConstants.FreedomOfMovement] = 4;
            spellLevels[SpellConstants.HallucinatoryTerrain] = 4;
            spellLevels[SpellConstants.HoldMonster] = 4;
            spellLevels[SpellConstants.Invisibility_Greater] = 4;
            spellLevels[SpellConstants.LegendLore] = 4;
            spellLevels[SpellConstants.LocateCreature] = 4;
            spellLevels[SpellConstants.ModifyMemory] = 4;
            spellLevels[SpellConstants.NeutralizePoison] = 4;
            spellLevels[SpellConstants.RainbowPattern] = 4;
            spellLevels[SpellConstants.RepelVermin] = 4;
            spellLevels[SpellConstants.SecureShelter] = 4;
            spellLevels[SpellConstants.ShadowConjuration] = 4;
            spellLevels[SpellConstants.Shout] = 4;
            spellLevels[SpellConstants.SpeakWithPlants] = 4;
            spellLevels[SpellConstants.SummonMonsterIV] = 4;
            spellLevels[SpellConstants.ZoneOfSilence] = 4;

            //5th - Level Bard Spells
            spellLevels[SpellConstants.CureLightWounds_Mass] = 5;
            spellLevels[SpellConstants.DispelMagic_Greater] = 5;
            spellLevels[SpellConstants.Dream] = 5;
            spellLevels[SpellConstants.FalseVision] = 5;
            spellLevels[SpellConstants.Heroism_Greater] = 5;
            spellLevels[SpellConstants.MindFog] = 5;
            spellLevels[SpellConstants.MirageArcana] = 5;
            spellLevels[SpellConstants.Mislead] = 5;
            spellLevels[SpellConstants.Nightmare] = 5;
            spellLevels[SpellConstants.PersistentImage] = 5;
            spellLevels[SpellConstants.Seeming] = 5;
            spellLevels[SpellConstants.ShadowEvocation] = 5;
            spellLevels[SpellConstants.ShadowWalk] = 5;
            spellLevels[SpellConstants.SongOfDiscord] = 5;
            spellLevels[SpellConstants.Suggestion_Mass] = 5;
            spellLevels[SpellConstants.SummonMonsterV] = 5;

            //6th - Level Bard Spells
            spellLevels[SpellConstants.AnalyzeDweomer] = 6;
            spellLevels[SpellConstants.AnimateObjects] = 6;
            spellLevels[SpellConstants.CatsGrace_Mass] = 6;
            spellLevels[SpellConstants.CharmMonster_Mass] = 6;
            spellLevels[SpellConstants.CureModerateWounds_Mass] = 6;
            spellLevels[SpellConstants.EaglesSplendor_Mass] = 6;
            spellLevels[SpellConstants.Eyebite] = 6;
            spellLevels[SpellConstants.FindThePath] = 6;
            spellLevels[SpellConstants.FoxsCunning_Mass] = 6;
            spellLevels[SpellConstants.GeasQuest] = 6;
            spellLevels[SpellConstants.HeroesFeast] = 6;
            spellLevels[SpellConstants.IrresistibleDance] = 6;
            spellLevels[SpellConstants.PermanentImage] = 6;
            spellLevels[SpellConstants.ProgrammedImage] = 6;
            spellLevels[SpellConstants.ProjectImage] = 6;
            spellLevels[SpellConstants.Scrying_Greater] = 6;
            spellLevels[SpellConstants.Shout_Greater] = 6;
            spellLevels[SpellConstants.SummonMonsterVI] = 6;
            spellLevels[SpellConstants.SympatheticVibration] = 6;
            spellLevels[SpellConstants.Veil] = 6;

            AssertTypesAndAmounts(SpellConstants.Casters.Bard, spellLevels);
        }

        [Test]
        public void ClericSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            //0 - Level Cleric Spells(Orisons)
            spellLevels[SpellConstants.CreateWater] = 0;
            spellLevels[SpellConstants.CureMinorWounds] = 0;
            spellLevels[SpellConstants.InflictMinorWounds] = 0;
            spellLevels[SpellConstants.DetectMagic] = 0;
            spellLevels[SpellConstants.DetectPoison] = 0;
            spellLevels[SpellConstants.Guidance] = 0;
            spellLevels[SpellConstants.Light] = 0;
            spellLevels[SpellConstants.Mending] = 0;
            spellLevels[SpellConstants.PurifyFoodAndDrink] = 0;
            spellLevels[SpellConstants.ReadMagic] = 0;
            spellLevels[SpellConstants.Resistance] = 0;
            spellLevels[SpellConstants.Virtue] = 0;

            //1st - Level Cleric Spells
            spellLevels[SpellConstants.Bane] = 1;
            spellLevels[SpellConstants.Bless] = 1;
            spellLevels[SpellConstants.BlessWater] = 1;
            spellLevels[SpellConstants.CauseFear] = 1;
            spellLevels[SpellConstants.Command] = 1;
            spellLevels[SpellConstants.ComprehendLanguages] = 1;
            spellLevels[SpellConstants.CureLightWounds] = 1;
            spellLevels[SpellConstants.InflictLightWounds] = 1;
            spellLevels[SpellConstants.CurseWater] = 1;
            spellLevels[SpellConstants.Deathwatch] = 1;
            spellLevels[SpellConstants.DetectChaos] = 1;
            spellLevels[SpellConstants.DetectEvil] = 1;
            spellLevels[SpellConstants.DetectGood] = 1;
            spellLevels[SpellConstants.DetectLaw] = 1;
            spellLevels[SpellConstants.DetectUndead] = 1;
            spellLevels[SpellConstants.DivineFavor] = 1;
            spellLevels[SpellConstants.Doom] = 1;
            spellLevels[SpellConstants.EndureElements] = 1;
            spellLevels[SpellConstants.EntropicShield] = 1;
            spellLevels[SpellConstants.HideFromUndead] = 1;
            spellLevels[SpellConstants.MagicStone] = 1;
            spellLevels[SpellConstants.MagicWeapon] = 1;
            spellLevels[SpellConstants.ObscuringMist] = 1;
            spellLevels[SpellConstants.ProtectionFromChaos] = 1;
            spellLevels[SpellConstants.ProtectionFromEvil] = 1;
            spellLevels[SpellConstants.ProtectionFromGood] = 1;
            spellLevels[SpellConstants.ProtectionFromLaw] = 1;
            spellLevels[SpellConstants.RemoveFear] = 1;
            spellLevels[SpellConstants.Sanctuary] = 1;
            spellLevels[SpellConstants.ShieldOfFaith] = 1;
            spellLevels[SpellConstants.SummonMonsterI] = 1;

            //2nd - Level Cleric Spells
            spellLevels[SpellConstants.Aid] = 2;
            spellLevels[SpellConstants.AlignWeapon] = 2;
            spellLevels[SpellConstants.Augury] = 2;
            spellLevels[SpellConstants.BearsEndurance] = 2;
            spellLevels[SpellConstants.BullsStrength] = 2;
            spellLevels[SpellConstants.CalmEmotions] = 2;
            spellLevels[SpellConstants.Consecrate] = 2;
            spellLevels[SpellConstants.CureModerateWounds] = 2;
            spellLevels[SpellConstants.InflictModerateWounds] = 2;
            spellLevels[SpellConstants.Darkness] = 2;
            spellLevels[SpellConstants.DeathKnell] = 2;
            spellLevels[SpellConstants.DelayPoison] = 2;
            spellLevels[SpellConstants.Desecrate] = 2;
            spellLevels[SpellConstants.EaglesSplendor] = 2;
            spellLevels[SpellConstants.Enthrall] = 2;
            spellLevels[SpellConstants.FindTraps] = 2;
            spellLevels[SpellConstants.GentleRepose] = 2;
            spellLevels[SpellConstants.HoldPerson] = 2;
            spellLevels[SpellConstants.MakeWhole] = 2;
            spellLevels[SpellConstants.OwlsWisdom] = 2;
            spellLevels[SpellConstants.RemoveParalysis] = 2;
            spellLevels[SpellConstants.ResistEnergy] = 2;
            spellLevels[SpellConstants.Restoration_Lesser] = 2;
            spellLevels[SpellConstants.Shatter] = 2;
            spellLevels[SpellConstants.ShieldOther] = 2;
            spellLevels[SpellConstants.Silence] = 2;
            spellLevels[SpellConstants.SoundBurst] = 2;
            spellLevels[SpellConstants.SpiritualWeapon] = 2;
            spellLevels[SpellConstants.Status] = 2;
            spellLevels[SpellConstants.SummonMonsterII] = 2;
            spellLevels[SpellConstants.UndetectableAlignment] = 2;
            spellLevels[SpellConstants.ZoneOfTruth] = 2;

            //3rd - Level Cleric Spells
            spellLevels[SpellConstants.AnimateDead] = 3;
            spellLevels[SpellConstants.BestowCurse] = 3;
            spellLevels[SpellConstants.BlindnessDeafness] = 3;
            spellLevels[SpellConstants.Contagion] = 3;
            spellLevels[SpellConstants.ContinualFlame] = 3;
            spellLevels[SpellConstants.CreateFoodAndWater] = 3;
            spellLevels[SpellConstants.CureSeriousWounds] = 3;
            spellLevels[SpellConstants.InflictSeriousWounds] = 3;
            spellLevels[SpellConstants.Daylight] = 3;
            spellLevels[SpellConstants.DeeperDarkness] = 3;
            spellLevels[SpellConstants.DispelMagic] = 3;
            spellLevels[SpellConstants.GlyphOfWarding] = 3;
            spellLevels[SpellConstants.HelpingHand] = 3;
            spellLevels[SpellConstants.InvisibilityPurge] = 3;
            spellLevels[SpellConstants.LocateObject] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstChaos] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstEvil] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstGood] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstLaw] = 3;
            spellLevels[SpellConstants.MagicVestment] = 3;
            spellLevels[SpellConstants.MeldIntoStone] = 3;
            spellLevels[SpellConstants.ObscureObject] = 3;
            spellLevels[SpellConstants.Prayer] = 3;
            spellLevels[SpellConstants.ProtectionFromEnergy] = 3;
            spellLevels[SpellConstants.RemoveBlindnessDeafness] = 3;
            spellLevels[SpellConstants.RemoveCurse] = 3;
            spellLevels[SpellConstants.RemoveDisease] = 3;
            spellLevels[SpellConstants.SearingLight] = 3;
            spellLevels[SpellConstants.SpeakWithDead] = 3;
            spellLevels[SpellConstants.StoneShape] = 3;
            spellLevels[SpellConstants.SummonMonsterIII] = 3;
            spellLevels[SpellConstants.WaterBreathing] = 3;
            spellLevels[SpellConstants.WaterWalk] = 3;
            spellLevels[SpellConstants.WindWall] = 3;

            //4th - Level Cleric Spells
            spellLevels[SpellConstants.AirWalk] = 4;
            spellLevels[SpellConstants.ControlWater] = 4;
            spellLevels[SpellConstants.CureCriticalWounds] = 4;
            spellLevels[SpellConstants.InflictCriticalWounds] = 4;
            spellLevels[SpellConstants.DeathWard] = 4;
            spellLevels[SpellConstants.DimensionalAnchor] = 4;
            spellLevels[SpellConstants.DiscernLies] = 4;
            spellLevels[SpellConstants.Dismissal] = 4;
            spellLevels[SpellConstants.Divination] = 4;
            spellLevels[SpellConstants.DivinePower] = 4;
            spellLevels[SpellConstants.FreedomOfMovement] = 4;
            spellLevels[SpellConstants.GiantVermin] = 4;
            spellLevels[SpellConstants.ImbueWithSpellAbility] = 4;
            spellLevels[SpellConstants.MagicWeapon_Greater] = 4;
            spellLevels[SpellConstants.NeutralizePoison] = 4;
            spellLevels[SpellConstants.PlanarAlly_Lesser] = 4;
            spellLevels[SpellConstants.Poison] = 4;
            spellLevels[SpellConstants.RepelVermin] = 4;
            spellLevels[SpellConstants.Restoration] = 4;
            spellLevels[SpellConstants.Sending] = 4;
            spellLevels[SpellConstants.SpellImmunity] = 4;
            spellLevels[SpellConstants.SummonMonsterIV] = 4;
            spellLevels[SpellConstants.Tongues] = 4;

            //5th - Level Cleric Spells
            spellLevels[SpellConstants.Atonement] = 5;
            spellLevels[SpellConstants.BreakEnchantment] = 5;
            spellLevels[SpellConstants.Command_Greater] = 5;
            spellLevels[SpellConstants.Commune] = 5;
            spellLevels[SpellConstants.InflictLightWounds_Mass] = 5;
            spellLevels[SpellConstants.CureLightWounds_Mass] = 5;
            spellLevels[SpellConstants.DispelChaos] = 5;
            spellLevels[SpellConstants.DispelEvil] = 5;
            spellLevels[SpellConstants.DispelGood] = 5;
            spellLevels[SpellConstants.DispelLaw] = 5;
            spellLevels[SpellConstants.DisruptingWeapon] = 5;
            spellLevels[SpellConstants.FlameStrike] = 5;
            spellLevels[SpellConstants.Hallow] = 5;
            spellLevels[SpellConstants.InsectPlague] = 5;
            spellLevels[SpellConstants.MarkOfJustice] = 5;
            spellLevels[SpellConstants.PlaneShift] = 5;
            spellLevels[SpellConstants.RaiseDead] = 5;
            spellLevels[SpellConstants.RighteousMight] = 5;
            spellLevels[SpellConstants.Scrying] = 5;
            spellLevels[SpellConstants.SlayLiving] = 5;
            spellLevels[SpellConstants.SpellResistance] = 5;
            spellLevels[SpellConstants.SummonMonsterV] = 5;
            spellLevels[SpellConstants.SymbolOfPain] = 5;
            spellLevels[SpellConstants.SymbolOfSleep] = 5;
            spellLevels[SpellConstants.TrueSeeing] = 5;
            spellLevels[SpellConstants.Unhallow] = 5;
            spellLevels[SpellConstants.WallOfStone] = 5;

            //6th - Level Cleric Spells
            spellLevels[SpellConstants.AnimateObjects] = 6;
            spellLevels[SpellConstants.AntilifeShell] = 6;
            spellLevels[SpellConstants.Banishment] = 6;
            spellLevels[SpellConstants.BearsEndurance_Mass] = 6;
            spellLevels[SpellConstants.BladeBarrier] = 6;
            spellLevels[SpellConstants.BullsStrength_Mass] = 6;
            spellLevels[SpellConstants.CreateUndead] = 6;
            spellLevels[SpellConstants.CureModerateWounds_Mass] = 6;
            spellLevels[SpellConstants.InflictModerateWounds_Mass] = 6;
            spellLevels[SpellConstants.DispelMagic_Greater] = 6;
            spellLevels[SpellConstants.EaglesSplendor_Mass] = 6;
            spellLevels[SpellConstants.FindThePath] = 6;
            spellLevels[SpellConstants.Forbiddance] = 6;
            spellLevels[SpellConstants.GeasQuest] = 6;
            spellLevels[SpellConstants.GlyphOfWarding_Greater] = 6;
            spellLevels[SpellConstants.Heal] = 6;
            spellLevels[SpellConstants.Harm] = 6;
            spellLevels[SpellConstants.HeroesFeast] = 6;
            spellLevels[SpellConstants.OwlsWisdom_Mass] = 6;
            spellLevels[SpellConstants.PlanarAlly] = 6;
            spellLevels[SpellConstants.SummonMonsterVI] = 6;
            spellLevels[SpellConstants.SymbolOfFear] = 6;
            spellLevels[SpellConstants.SymbolOfPersuasion] = 6;
            spellLevels[SpellConstants.UndeathToDeath] = 6;
            spellLevels[SpellConstants.WindWalk] = 6;
            spellLevels[SpellConstants.WordOfRecall] = 6;

            //7th - Level Cleric Spells
            spellLevels[SpellConstants.Blasphemy] = 7;
            spellLevels[SpellConstants.ControlWeather] = 7;
            spellLevels[SpellConstants.CureSeriousWounds_Mass] = 7;
            spellLevels[SpellConstants.InflictSeriousWounds_Mass] = 7;
            spellLevels[SpellConstants.Destruction] = 7;
            spellLevels[SpellConstants.Dictum] = 7;
            spellLevels[SpellConstants.EtherealJaunt] = 7;
            spellLevels[SpellConstants.HolyWord] = 7;
            spellLevels[SpellConstants.Refuge] = 7;
            spellLevels[SpellConstants.Regenerate] = 7;
            spellLevels[SpellConstants.Repulsion] = 7;
            spellLevels[SpellConstants.Restoration_Greater] = 7;
            spellLevels[SpellConstants.Resurrection] = 7;
            spellLevels[SpellConstants.Scrying_Greater] = 7;
            spellLevels[SpellConstants.SummonMonsterVII] = 7;
            spellLevels[SpellConstants.SymbolOfStunning] = 7;
            spellLevels[SpellConstants.SymbolOfWeakness] = 7;
            spellLevels[SpellConstants.WordOfChaos] = 7;

            //8th - Level Cleric Spells
            spellLevels[SpellConstants.AntimagicField] = 8;
            spellLevels[SpellConstants.CloakOfChaos] = 8;
            spellLevels[SpellConstants.CreateGreaterUndead] = 8;
            spellLevels[SpellConstants.CureCriticalWounds_Mass] = 8;
            spellLevels[SpellConstants.InflictCriticalWounds_Mass] = 8;
            spellLevels[SpellConstants.DimensionalLock] = 8;
            spellLevels[SpellConstants.DiscernLocation] = 8;
            spellLevels[SpellConstants.Earthquake] = 8;
            spellLevels[SpellConstants.FireStorm] = 8;
            spellLevels[SpellConstants.HolyAura] = 8;
            spellLevels[SpellConstants.PlanarAlly_Greater] = 8;
            spellLevels[SpellConstants.ShieldOfLaw] = 8;
            spellLevels[SpellConstants.SpellImmunity_Greater] = 8;
            spellLevels[SpellConstants.SummonMonsterVIII] = 8;
            spellLevels[SpellConstants.SymbolOfDeath] = 8;
            spellLevels[SpellConstants.SymbolOfInsanity] = 8;
            spellLevels[SpellConstants.UnholyAura] = 8;

            //9th - Level Cleric Spells
            spellLevels[SpellConstants.AstralProjection] = 9;
            spellLevels[SpellConstants.EnergyDrain] = 9;
            spellLevels[SpellConstants.Etherealness] = 9;
            spellLevels[SpellConstants.Gate] = 9;
            spellLevels[SpellConstants.Heal_Mass] = 9;
            spellLevels[SpellConstants.Implosion] = 9;
            spellLevels[SpellConstants.Miracle] = 9;
            spellLevels[SpellConstants.SoulBind] = 9;
            spellLevels[SpellConstants.StormOfVengeance] = 9;
            spellLevels[SpellConstants.SummonMonsterIX] = 9;
            spellLevels[SpellConstants.TrueResurrection] = 9;

            AssertTypesAndAmounts(SpellConstants.Casters.Cleric, spellLevels);
        }

        [Test]
        public void DruidSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            //0 - Level Druid Spells(Orisons)
            spellLevels[SpellConstants.CreateWater] = 0;
            spellLevels[SpellConstants.CureMinorWounds] = 0;
            spellLevels[SpellConstants.DetectMagic] = 0;
            spellLevels[SpellConstants.DetectPoison] = 0;
            spellLevels[SpellConstants.Flare] = 0;
            spellLevels[SpellConstants.Guidance] = 0;
            spellLevels[SpellConstants.KnowDirection] = 0;
            spellLevels[SpellConstants.Light] = 0;
            spellLevels[SpellConstants.Mending] = 0;
            spellLevels[SpellConstants.PurifyFoodAndDrink] = 0;
            spellLevels[SpellConstants.ReadMagic] = 0;
            spellLevels[SpellConstants.Resistance] = 0;
            spellLevels[SpellConstants.Virtue] = 0;

            //1st - Level Druid Spells
            spellLevels[SpellConstants.CalmAnimals] = 1;
            spellLevels[SpellConstants.CharmAnimal] = 1;
            spellLevels[SpellConstants.CureLightWounds] = 1;
            spellLevels[SpellConstants.DetectAnimalsOrPlants] = 1;
            spellLevels[SpellConstants.DetectSnaresAndPits] = 1;
            spellLevels[SpellConstants.EndureElements] = 1;
            spellLevels[SpellConstants.Entangle] = 1;
            spellLevels[SpellConstants.FaerieFire] = 1;
            spellLevels[SpellConstants.Goodberry] = 1;
            spellLevels[SpellConstants.HideFromAnimals] = 1;
            spellLevels[SpellConstants.Jump] = 1;
            spellLevels[SpellConstants.Longstrider] = 1;
            spellLevels[SpellConstants.MagicFang] = 1;
            spellLevels[SpellConstants.MagicStone] = 1;
            spellLevels[SpellConstants.ObscuringMist] = 1;
            spellLevels[SpellConstants.PassWithoutTrace] = 1;
            spellLevels[SpellConstants.ProduceFlame] = 1;
            spellLevels[SpellConstants.Shillelagh] = 1;
            spellLevels[SpellConstants.SpeakWithAnimals] = 1;
            spellLevels[SpellConstants.SummonNaturesAllyI] = 1;

            //2nd - Level Druid Spells
            spellLevels[SpellConstants.AnimalMessenger] = 2;
            spellLevels[SpellConstants.AnimalTrance] = 2;
            spellLevels[SpellConstants.Barkskin] = 2;
            spellLevels[SpellConstants.BearsEndurance] = 2;
            spellLevels[SpellConstants.BullsStrength] = 2;
            spellLevels[SpellConstants.CatsGrace] = 2;
            spellLevels[SpellConstants.ChillMetal] = 2;
            spellLevels[SpellConstants.DelayPoison] = 2;
            spellLevels[SpellConstants.FireTrap] = 2;
            spellLevels[SpellConstants.FlameBlade] = 2;
            spellLevels[SpellConstants.FlamingSphere] = 2;
            spellLevels[SpellConstants.FogCloud] = 2;
            spellLevels[SpellConstants.GustOfWind] = 2;
            spellLevels[SpellConstants.HeatMetal] = 2;
            spellLevels[SpellConstants.HoldAnimal] = 2;
            spellLevels[SpellConstants.OwlsWisdom] = 2;
            spellLevels[SpellConstants.ReduceAnimal] = 2;
            spellLevels[SpellConstants.ResistEnergy] = 2;
            spellLevels[SpellConstants.Restoration_Lesser] = 2;
            spellLevels[SpellConstants.SoftenEarthAndStone] = 2;
            spellLevels[SpellConstants.SpiderClimb] = 2;
            spellLevels[SpellConstants.SummonNaturesAllyII] = 2;
            spellLevels[SpellConstants.SummonSwarm] = 2;
            spellLevels[SpellConstants.TreeShape] = 2;
            spellLevels[SpellConstants.WarpWood] = 2;
            spellLevels[SpellConstants.WoodShape] = 2;

            //3rd - Level Druid Spells
            spellLevels[SpellConstants.CallLightning] = 3;
            spellLevels[SpellConstants.Contagion] = 3;
            spellLevels[SpellConstants.CureModerateWounds] = 3;
            spellLevels[SpellConstants.Daylight] = 3;
            spellLevels[SpellConstants.DiminishPlants] = 3;
            spellLevels[SpellConstants.DominateAnimal] = 3;
            spellLevels[SpellConstants.MagicFang_Greater] = 3;
            spellLevels[SpellConstants.MeldIntoStone] = 3;
            spellLevels[SpellConstants.NeutralizePoison] = 3;
            spellLevels[SpellConstants.PlantGrowth] = 3;
            spellLevels[SpellConstants.Poison] = 3;
            spellLevels[SpellConstants.ProtectionFromEnergy] = 3;
            spellLevels[SpellConstants.Quench] = 3;
            spellLevels[SpellConstants.RemoveDisease] = 3;
            spellLevels[SpellConstants.SleetStorm] = 3;
            spellLevels[SpellConstants.Snare] = 3;
            spellLevels[SpellConstants.SpeakWithPlants] = 3;
            spellLevels[SpellConstants.SpikeGrowth] = 3;
            spellLevels[SpellConstants.StoneShape] = 3;
            spellLevels[SpellConstants.SummonNaturesAllyIII] = 3;
            spellLevels[SpellConstants.WaterBreathing] = 3;
            spellLevels[SpellConstants.WindWall] = 3;

            //4th - Level Druid Spells
            spellLevels[SpellConstants.AirWalk] = 4;
            spellLevels[SpellConstants.AntiplantShell] = 4;
            spellLevels[SpellConstants.Blight] = 4;
            spellLevels[SpellConstants.CommandPlants] = 4;
            spellLevels[SpellConstants.ControlWater] = 4;
            spellLevels[SpellConstants.CureSeriousWounds] = 4;
            spellLevels[SpellConstants.DispelMagic] = 4;
            spellLevels[SpellConstants.FlameStrike] = 4;
            spellLevels[SpellConstants.FreedomOfMovement] = 4;
            spellLevels[SpellConstants.GiantVermin] = 4;
            spellLevels[SpellConstants.IceStorm] = 4;
            spellLevels[SpellConstants.Reincarnate] = 4;
            spellLevels[SpellConstants.RepelVermin] = 4;
            spellLevels[SpellConstants.RustingGrasp] = 4;
            spellLevels[SpellConstants.Scrying] = 4;
            spellLevels[SpellConstants.SpikeStones] = 4;
            spellLevels[SpellConstants.SummonNaturesAllyIV] = 4;

            //5th - Level Druid Spells
            spellLevels[SpellConstants.AnimalGrowth] = 5;
            spellLevels[SpellConstants.Atonement] = 5;
            spellLevels[SpellConstants.Awaken] = 5;
            spellLevels[SpellConstants.BalefulPolymorph] = 5;
            spellLevels[SpellConstants.CallLightningStorm] = 5;
            spellLevels[SpellConstants.CommuneWithNature] = 5;
            spellLevels[SpellConstants.ControlWinds] = 5;
            spellLevels[SpellConstants.CureCriticalWounds] = 5;
            spellLevels[SpellConstants.DeathWard] = 5;
            spellLevels[SpellConstants.Hallow] = 5;
            spellLevels[SpellConstants.InsectPlague] = 5;
            spellLevels[SpellConstants.Stoneskin] = 5;
            spellLevels[SpellConstants.SummonNaturesAllyV] = 5;
            spellLevels[SpellConstants.TransmuteMudToRock] = 5;
            spellLevels[SpellConstants.TransmuteRockToMud] = 5;
            spellLevels[SpellConstants.TreeStride] = 5;
            spellLevels[SpellConstants.Unhallow] = 5;
            spellLevels[SpellConstants.WallOfFire] = 5;
            spellLevels[SpellConstants.WallOfThorns] = 5;

            //6th - Level Druid Spells
            spellLevels[SpellConstants.AntilifeShell] = 6;
            spellLevels[SpellConstants.BearsEndurance_Mass] = 6;
            spellLevels[SpellConstants.BullsStrength_Mass] = 6;
            spellLevels[SpellConstants.CatsGrace_Mass] = 6;
            spellLevels[SpellConstants.CureLightWounds_Mass] = 6;
            spellLevels[SpellConstants.DispelMagic_Greater] = 6;
            spellLevels[SpellConstants.FindThePath] = 6;
            spellLevels[SpellConstants.FireSeeds] = 6;
            spellLevels[SpellConstants.Ironwood] = 6;
            spellLevels[SpellConstants.Liveoak] = 6;
            spellLevels[SpellConstants.MoveEarth] = 6;
            spellLevels[SpellConstants.OwlsWisdom_Mass] = 6;
            spellLevels[SpellConstants.RepelWood] = 6;
            spellLevels[SpellConstants.Spellstaff] = 6;
            spellLevels[SpellConstants.StoneTell] = 6;
            spellLevels[SpellConstants.SummonNaturesAllyVI] = 6;
            spellLevels[SpellConstants.TransportViaPlants] = 6;
            spellLevels[SpellConstants.WallOfStone] = 6;

            //7th - Level Druid Spells
            spellLevels[SpellConstants.AnimatePlants] = 7;
            spellLevels[SpellConstants.Changestaff] = 7;
            spellLevels[SpellConstants.ControlWeather] = 7;
            spellLevels[SpellConstants.CreepingDoom] = 7;
            spellLevels[SpellConstants.CureModerateWounds_Mass] = 7;
            spellLevels[SpellConstants.FireStorm] = 7;
            spellLevels[SpellConstants.Heal] = 7;
            spellLevels[SpellConstants.Scrying_Greater] = 7;
            spellLevels[SpellConstants.SummonNaturesAllyVII] = 7;
            spellLevels[SpellConstants.Sunbeam] = 7;
            spellLevels[SpellConstants.TransmuteMetalToWood] = 7;
            spellLevels[SpellConstants.TrueSeeing] = 7;
            spellLevels[SpellConstants.WindWalk] = 7;

            //8th - Level Druid Spells
            spellLevels[SpellConstants.AnimalShapes] = 8;
            spellLevels[SpellConstants.ControlPlants] = 8;
            spellLevels[SpellConstants.CureSeriousWounds_Mass] = 8;
            spellLevels[SpellConstants.Earthquake] = 8;
            spellLevels[SpellConstants.FingerOfDeath] = 8;
            spellLevels[SpellConstants.RepelMetalOrStone] = 8;
            spellLevels[SpellConstants.ReverseGravity] = 8;
            spellLevels[SpellConstants.SummonNaturesAllyVIII] = 8;
            spellLevels[SpellConstants.Sunburst] = 8;
            spellLevels[SpellConstants.Whirlwind] = 8;
            spellLevels[SpellConstants.WordOfRecall] = 8;

            //9th - Level Druid Spells
            spellLevels[SpellConstants.Antipathy] = 9;
            spellLevels[SpellConstants.CureCriticalWounds_Mass] = 9;
            spellLevels[SpellConstants.ElementalSwarm] = 9;
            spellLevels[SpellConstants.Foresight] = 9;
            spellLevels[SpellConstants.Regenerate] = 9;
            spellLevels[SpellConstants.Shambler] = 9;
            spellLevels[SpellConstants.Shapechange] = 9;
            spellLevels[SpellConstants.StormOfVengeance] = 9;
            spellLevels[SpellConstants.SummonNaturesAllyIX] = 9;
            spellLevels[SpellConstants.Sympathy] = 9;

            AssertTypesAndAmounts(SpellConstants.Casters.Druid, spellLevels);
        }

        [Test]
        public void SorcererSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            //0 - Level Sorcerer / Wizard Spells(Cantrips)
            //Abjur
            spellLevels[SpellConstants.Resistance] = 0;
            //Conj
            spellLevels[SpellConstants.AcidSplash] = 0;
            //Div
            spellLevels[SpellConstants.DetectPoison] = 0;
            spellLevels[SpellConstants.DetectMagic] = 0;
            spellLevels[SpellConstants.ReadMagic] = 0;
            //Ench
            spellLevels[SpellConstants.Daze] = 0;
            //Evoc
            spellLevels[SpellConstants.DancingLights] = 0;
            spellLevels[SpellConstants.Flare] = 0;
            spellLevels[SpellConstants.Light] = 0;
            spellLevels[SpellConstants.RayOfFrost] = 0;
            //Illus
            spellLevels[SpellConstants.GhostSound] = 0;
            //Necro
            spellLevels[SpellConstants.DisruptUndead] = 0;
            spellLevels[SpellConstants.TouchOfFatigue] = 0;
            //Trans
            spellLevels[SpellConstants.MageHand] = 0;
            spellLevels[SpellConstants.Mending] = 0;
            spellLevels[SpellConstants.Message] = 0;
            spellLevels[SpellConstants.OpenClose] = 0;
            //Univ
            spellLevels[SpellConstants.ArcaneMark] = 0;
            spellLevels[SpellConstants.Prestidigitation] = 0;

            //1st - Level Sorcerer / Wizard Spells
            //Abjur
            spellLevels[SpellConstants.Alarm] = 1;
            spellLevels[SpellConstants.EndureElements] = 1;
            spellLevels[SpellConstants.HoldPortal] = 1;
            spellLevels[SpellConstants.ProtectionFromChaos] = 1;
            spellLevels[SpellConstants.ProtectionFromEvil] = 1;
            spellLevels[SpellConstants.ProtectionFromGood] = 1;
            spellLevels[SpellConstants.ProtectionFromLaw] = 1;
            spellLevels[SpellConstants.Shield] = 1;
            //Conj
            spellLevels[SpellConstants.Grease] = 1;
            spellLevels[SpellConstants.MageArmor] = 1;
            spellLevels[SpellConstants.Mount] = 1;
            spellLevels[SpellConstants.ObscuringMist] = 1;
            spellLevels[SpellConstants.SummonMonsterI] = 1;
            spellLevels[SpellConstants.UnseenServant] = 1;
            //Div
            spellLevels[SpellConstants.ComprehendLanguages] = 1;
            spellLevels[SpellConstants.DetectSecretDoors] = 1;
            spellLevels[SpellConstants.DetectUndead] = 1;
            spellLevels[SpellConstants.Identify] = 1;
            spellLevels[SpellConstants.TrueStrike] = 1;
            //Ench
            spellLevels[SpellConstants.CharmPerson] = 1;
            spellLevels[SpellConstants.Hypnotism] = 1;
            spellLevels[SpellConstants.Sleep] = 1;
            //Evoc
            spellLevels[SpellConstants.BurningHands] = 1;
            spellLevels[SpellConstants.FloatingDisk] = 1;
            spellLevels[SpellConstants.MagicMissile] = 1;
            spellLevels[SpellConstants.ShockingGrasp] = 1;
            //Illus
            spellLevels[SpellConstants.ColorSpray] = 1;
            spellLevels[SpellConstants.DisguiseSelf] = 1;
            spellLevels[SpellConstants.MagicAura] = 1;
            spellLevels[SpellConstants.SilentImage] = 1;
            spellLevels[SpellConstants.Ventriloquism] = 1;
            //Necro
            spellLevels[SpellConstants.CauseFear] = 1;
            spellLevels[SpellConstants.ChillTouch] = 1;
            spellLevels[SpellConstants.RayOfEnfeeblement] = 1;
            //Trans
            spellLevels[SpellConstants.AnimateRope] = 1;
            spellLevels[SpellConstants.EnlargePerson] = 1;
            spellLevels[SpellConstants.Erase] = 1;
            spellLevels[SpellConstants.ExpeditiousRetreat] = 1;
            spellLevels[SpellConstants.FeatherFall] = 1;
            spellLevels[SpellConstants.Jump] = 1;
            spellLevels[SpellConstants.MagicWeapon] = 1;
            spellLevels[SpellConstants.ReducePerson] = 1;

            //2nd - Level Sorcerer / Wizard Spells
            //Abjur
            spellLevels[SpellConstants.ArcaneLock] = 2;
            spellLevels[SpellConstants.ObscureObject] = 2;
            spellLevels[SpellConstants.ProtectionFromArrows] = 2;
            spellLevels[SpellConstants.ResistEnergy] = 2;
            //Conj
            spellLevels[SpellConstants.AcidArrow] = 2;
            spellLevels[SpellConstants.FogCloud] = 2;
            spellLevels[SpellConstants.Glitterdust] = 2;
            spellLevels[SpellConstants.SummonMonsterII] = 2;
            spellLevels[SpellConstants.SummonSwarm] = 2;
            spellLevels[SpellConstants.Web] = 2;
            //Div
            spellLevels[SpellConstants.DetectThoughts] = 2;
            spellLevels[SpellConstants.LocateObject] = 2;
            spellLevels[SpellConstants.SeeInvisibility] = 2;
            //Ench
            spellLevels[SpellConstants.DazeMonster] = 2;
            spellLevels[SpellConstants.HideousLaughter] = 2;
            spellLevels[SpellConstants.TouchOfIdiocy] = 2;
            //Evoc
            spellLevels[SpellConstants.ContinualFlame] = 2;
            spellLevels[SpellConstants.Darkness] = 2;
            spellLevels[SpellConstants.FlamingSphere] = 2;
            spellLevels[SpellConstants.GustOfWind] = 2;
            spellLevels[SpellConstants.ScorchingRay] = 2;
            spellLevels[SpellConstants.Shatter] = 2;
            //Illus
            spellLevels[SpellConstants.Blur] = 2;
            spellLevels[SpellConstants.HypnoticPattern] = 2;
            spellLevels[SpellConstants.Invisibility] = 2;
            spellLevels[SpellConstants.MagicMouth] = 2;
            spellLevels[SpellConstants.MinorImage] = 2;
            spellLevels[SpellConstants.MirrorImage] = 2;
            spellLevels[SpellConstants.Misdirection] = 2;
            spellLevels[SpellConstants.PhantomTrap] = 2;
            //Necro
            spellLevels[SpellConstants.BlindnessDeafness] = 2;
            spellLevels[SpellConstants.CommandUndead] = 2;
            spellLevels[SpellConstants.FalseLife] = 2;
            spellLevels[SpellConstants.GhoulTouch] = 2;
            spellLevels[SpellConstants.Scare] = 2;
            spellLevels[SpellConstants.SpectralHand] = 2;
            //Trans
            spellLevels[SpellConstants.AlterSelf] = 2;
            spellLevels[SpellConstants.BearsEndurance] = 2;
            spellLevels[SpellConstants.BullsStrength] = 2;
            spellLevels[SpellConstants.CatsGrace] = 2;
            spellLevels[SpellConstants.Darkvision] = 2;
            spellLevels[SpellConstants.EaglesSplendor] = 2;
            spellLevels[SpellConstants.FoxsCunning] = 2;
            spellLevels[SpellConstants.Knock] = 2;
            spellLevels[SpellConstants.Levitate] = 2;
            spellLevels[SpellConstants.OwlsWisdom] = 2;
            spellLevels[SpellConstants.Pyrotechnics] = 2;
            spellLevels[SpellConstants.RopeTrick] = 2;
            spellLevels[SpellConstants.SpiderClimb] = 2;
            spellLevels[SpellConstants.WhisperingWind] = 2;

            //3rd-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.DispelMagic] = 3;
            spellLevels[SpellConstants.ExplosiveRunes] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstChaos] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstEvil] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstGood] = 3;
            spellLevels[SpellConstants.MagicCircleAgainstLaw] = 3;
            spellLevels[SpellConstants.Nondetection] = 3;
            spellLevels[SpellConstants.ProtectionFromEnergy] = 3;
            //Conj
            spellLevels[SpellConstants.PhantomSteed] = 3;
            spellLevels[SpellConstants.SepiaSnakeSigil] = 3;
            spellLevels[SpellConstants.SleetStorm] = 3;
            spellLevels[SpellConstants.StinkingCloud] = 3;
            spellLevels[SpellConstants.SummonMonsterIII] = 3;
            //Div
            spellLevels[SpellConstants.ArcaneSight] = 3;
            spellLevels[SpellConstants.ClairaudienceClairvoyance] = 3;
            spellLevels[SpellConstants.Tongues] = 3;
            //Ench
            spellLevels[SpellConstants.DeepSlumber] = 3;
            spellLevels[SpellConstants.Heroism] = 3;
            spellLevels[SpellConstants.HoldPerson] = 3;
            spellLevels[SpellConstants.Rage] = 3;
            spellLevels[SpellConstants.Suggestion] = 3;
            //Evoc
            spellLevels[SpellConstants.Daylight] = 3;
            spellLevels[SpellConstants.Fireball] = 3;
            spellLevels[SpellConstants.LightningBolt] = 3;
            spellLevels[SpellConstants.TinyHut] = 3;
            spellLevels[SpellConstants.WindWall] = 3;
            //Illus
            spellLevels[SpellConstants.Displacement] = 3;
            spellLevels[SpellConstants.IllusoryScript] = 3;
            spellLevels[SpellConstants.InvisibilitySphere] = 3;
            spellLevels[SpellConstants.MajorImage] = 3;
            //Necro
            spellLevels[SpellConstants.GentleRepose] = 3;
            spellLevels[SpellConstants.HaltUndead] = 3;
            spellLevels[SpellConstants.RayOfExhaustion] = 3;
            spellLevels[SpellConstants.VampiricTouch] = 3;
            //Trans
            spellLevels[SpellConstants.Blink] = 3;
            spellLevels[SpellConstants.FlameArrow] = 3;
            spellLevels[SpellConstants.Fly] = 3;
            spellLevels[SpellConstants.GaseousForm] = 3;
            spellLevels[SpellConstants.Haste] = 3;
            spellLevels[SpellConstants.KeenEdge] = 3;
            spellLevels[SpellConstants.MagicWeapon_Greater] = 3;
            spellLevels[SpellConstants.SecretPage] = 3;
            spellLevels[SpellConstants.ShrinkItem] = 3;
            spellLevels[SpellConstants.Slow] = 3;
            spellLevels[SpellConstants.WaterBreathing] = 3;

            //4th-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.DimensionalAnchor] = 4;
            spellLevels[SpellConstants.FireTrap] = 4;
            spellLevels[SpellConstants.GlobeOfInvulnerability_Lesser] = 4;
            spellLevels[SpellConstants.RemoveCurse] = 4;
            spellLevels[SpellConstants.Stoneskin] = 4;
            //Conj
            spellLevels[SpellConstants.BlackTentacles] = 4;
            spellLevels[SpellConstants.DimensionDoor] = 4;
            spellLevels[SpellConstants.MinorCreation] = 4;
            spellLevels[SpellConstants.SecureShelter] = 4;
            spellLevels[SpellConstants.SolidFog] = 4;
            spellLevels[SpellConstants.SummonMonsterIV] = 4;
            //Div
            spellLevels[SpellConstants.ArcaneEye] = 4;
            spellLevels[SpellConstants.DetectScrying] = 4;
            spellLevels[SpellConstants.LocateCreature] = 4;
            spellLevels[SpellConstants.Scrying] = 4;
            //Ench
            spellLevels[SpellConstants.CharmMonster] = 4;
            spellLevels[SpellConstants.Confusion] = 4;
            spellLevels[SpellConstants.CrushingDespair] = 4;
            spellLevels[SpellConstants.Geas_Lesser] = 4;
            //Evoc
            spellLevels[SpellConstants.FireShield] = 4;
            spellLevels[SpellConstants.IceStorm] = 4;
            spellLevels[SpellConstants.ResilientSphere] = 4;
            spellLevels[SpellConstants.Shout] = 4;
            spellLevels[SpellConstants.WallOfFire] = 4;
            spellLevels[SpellConstants.WallOfIce] = 4;
            //Illus
            spellLevels[SpellConstants.HallucinatoryTerrain] = 4;
            spellLevels[SpellConstants.IllusoryWall] = 4;
            spellLevels[SpellConstants.Invisibility_Greater] = 4;
            spellLevels[SpellConstants.PhantasmalKiller] = 4;
            spellLevels[SpellConstants.RainbowPattern] = 4;
            spellLevels[SpellConstants.ShadowConjuration] = 4;
            //Necro
            spellLevels[SpellConstants.AnimateDead] = 4;
            spellLevels[SpellConstants.BestowCurse] = 4;
            spellLevels[SpellConstants.Contagion] = 4;
            spellLevels[SpellConstants.Enervation] = 4;
            spellLevels[SpellConstants.Fear] = 4;
            //Trans
            spellLevels[SpellConstants.EnlargePerson_Mass] = 4;
            spellLevels[SpellConstants.MnemonicEnhancer] = 4;
            spellLevels[SpellConstants.Polymorph] = 4;
            spellLevels[SpellConstants.ReducePerson_Mass] = 4;
            spellLevels[SpellConstants.StoneShape] = 4;

            //5th-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.BreakEnchantment] = 5;
            spellLevels[SpellConstants.Dismissal] = 5;
            spellLevels[SpellConstants.MagesPrivateSanctum] = 5;
            //Conj
            spellLevels[SpellConstants.Cloudkill] = 5;
            spellLevels[SpellConstants.MagesFaithfulHound] = 5;
            spellLevels[SpellConstants.MajorCreation] = 5;
            spellLevels[SpellConstants.PlanarBinding_Lesser] = 5;
            spellLevels[SpellConstants.SecretChest] = 5;
            spellLevels[SpellConstants.SummonMonsterV] = 5;
            spellLevels[SpellConstants.Teleport] = 5;
            spellLevels[SpellConstants.WallOfStone] = 5;
            //Div
            spellLevels[SpellConstants.ContactOtherPlane] = 5;
            spellLevels[SpellConstants.PryingEyes] = 5;
            spellLevels[SpellConstants.TelepathicBond] = 5;
            //Ench
            spellLevels[SpellConstants.DominatePerson] = 5;
            spellLevels[SpellConstants.Feeblemind] = 5;
            spellLevels[SpellConstants.HoldMonster] = 5;
            spellLevels[SpellConstants.MindFog] = 5;
            spellLevels[SpellConstants.SymbolOfSleep] = 5;
            //Evoc
            spellLevels[SpellConstants.ConeOfCold] = 5;
            spellLevels[SpellConstants.InterposingHand] = 5;
            spellLevels[SpellConstants.Sending] = 5;
            spellLevels[SpellConstants.WallOfForce] = 5;
            //Illus
            spellLevels[SpellConstants.Dream] = 5;
            spellLevels[SpellConstants.FalseVision] = 5;
            spellLevels[SpellConstants.MirageArcana] = 5;
            spellLevels[SpellConstants.Nightmare] = 5;
            spellLevels[SpellConstants.PersistentImage] = 5;
            spellLevels[SpellConstants.Seeming] = 5;
            spellLevels[SpellConstants.ShadowEvocation] = 5;
            //Necro
            spellLevels[SpellConstants.Blight] = 5;
            spellLevels[SpellConstants.MagicJar] = 5;
            spellLevels[SpellConstants.SymbolOfPain] = 5;
            spellLevels[SpellConstants.WavesOfFatigue] = 5;
            //Trans
            spellLevels[SpellConstants.AnimalGrowth] = 5;
            spellLevels[SpellConstants.BalefulPolymorph] = 5;
            spellLevels[SpellConstants.Fabricate] = 5;
            spellLevels[SpellConstants.OverlandFlight] = 5;
            spellLevels[SpellConstants.Passwall] = 5;
            spellLevels[SpellConstants.Telekinesis] = 5;
            spellLevels[SpellConstants.TransmuteMudToRock] = 5;
            spellLevels[SpellConstants.TransmuteRockToMud] = 5;
            //Univ
            spellLevels[SpellConstants.Permanency] = 5;

            //6th-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.AntimagicField] = 6;
            spellLevels[SpellConstants.DispelMagic_Greater] = 6;
            spellLevels[SpellConstants.GlobeOfInvulnerability] = 6;
            spellLevels[SpellConstants.GuardsAndWards] = 6;
            spellLevels[SpellConstants.Repulsion] = 6;
            //Conj
            spellLevels[SpellConstants.AcidFog] = 6;
            spellLevels[SpellConstants.PlanarBinding] = 6;
            spellLevels[SpellConstants.SummonMonsterVI] = 6;
            spellLevels[SpellConstants.WallOfIron] = 6;
            //Div
            spellLevels[SpellConstants.AnalyzeDweomer] = 6;
            spellLevels[SpellConstants.LegendLore] = 6;
            spellLevels[SpellConstants.TrueSeeing] = 6;
            //Ench
            spellLevels[SpellConstants.GeasQuest] = 6;
            spellLevels[SpellConstants.Heroism_Greater] = 6;
            spellLevels[SpellConstants.Suggestion_Mass] = 6;
            spellLevels[SpellConstants.SymbolOfPersuasion] = 6;
            //Evoc
            spellLevels[SpellConstants.ChainLightning] = 6;
            spellLevels[SpellConstants.Contingency] = 6;
            spellLevels[SpellConstants.ForcefulHand] = 6;
            spellLevels[SpellConstants.FreezingSphere] = 6;
            //Illus
            spellLevels[SpellConstants.Mislead] = 6;
            spellLevels[SpellConstants.PermanentImage] = 6;
            spellLevels[SpellConstants.ProgrammedImage] = 6;
            spellLevels[SpellConstants.ShadowWalk] = 6;
            spellLevels[SpellConstants.Veil] = 6;
            //Necro
            spellLevels[SpellConstants.CircleOfDeath] = 6;
            spellLevels[SpellConstants.CreateUndead] = 6;
            spellLevels[SpellConstants.Eyebite] = 6;
            spellLevels[SpellConstants.SymbolOfFear] = 6;
            spellLevels[SpellConstants.UndeathToDeath] = 6;
            //Trans
            spellLevels[SpellConstants.BearsEndurance_Mass] = 6;
            spellLevels[SpellConstants.BullsStrength_Mass] = 6;
            spellLevels[SpellConstants.CatsGrace_Mass] = 6;
            spellLevels[SpellConstants.ControlWater] = 6;
            spellLevels[SpellConstants.Disintegrate] = 6;
            spellLevels[SpellConstants.EaglesSplendor_Mass] = 6;
            spellLevels[SpellConstants.FleshToStone] = 6;
            spellLevels[SpellConstants.FoxsCunning_Mass] = 6;
            spellLevels[SpellConstants.MagesLucubration] = 6;
            spellLevels[SpellConstants.MoveEarth] = 6;
            spellLevels[SpellConstants.OwlsWisdom_Mass] = 6;
            spellLevels[SpellConstants.StoneToFlesh] = 6;
            spellLevels[SpellConstants.Transformation] = 6;

            //7th-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.Banishment] = 7;
            spellLevels[SpellConstants.Sequester] = 7;
            spellLevels[SpellConstants.SpellTurning] = 7;
            //Conj
            spellLevels[SpellConstants.InstantSummons] = 7;
            spellLevels[SpellConstants.MagesMagnificentMansion] = 7;
            spellLevels[SpellConstants.PhaseDoor] = 7;
            spellLevels[SpellConstants.PlaneShift] = 7;
            spellLevels[SpellConstants.SummonMonsterVII] = 7;
            spellLevels[SpellConstants.Teleport_Greater] = 7;
            spellLevels[SpellConstants.TeleportObject] = 7;
            //Div
            spellLevels[SpellConstants.ArcaneSight_Greater] = 7;
            spellLevels[SpellConstants.Scrying_Greater] = 7;
            spellLevels[SpellConstants.Vision] = 7;
            //Ench
            spellLevels[SpellConstants.HoldPerson_Mass] = 7;
            spellLevels[SpellConstants.Insanity] = 7;
            spellLevels[SpellConstants.PowerWordBlind] = 7;
            spellLevels[SpellConstants.SymbolOfStunning] = 7;
            //Evoc
            spellLevels[SpellConstants.DelayedBlastFireball] = 7;
            spellLevels[SpellConstants.Forcecage] = 7;
            spellLevels[SpellConstants.GraspingHand] = 7;
            spellLevels[SpellConstants.MagesSword] = 7;
            spellLevels[SpellConstants.PrismaticSpray] = 7;
            //Illus
            spellLevels[SpellConstants.Invisibility_Mass] = 7;
            spellLevels[SpellConstants.ProjectImage] = 7;
            spellLevels[SpellConstants.ShadowConjuration_Greater] = 7;
            spellLevels[SpellConstants.Simulacrum] = 7;
            //Necro
            spellLevels[SpellConstants.ControlUndead] = 7;
            spellLevels[SpellConstants.FingerOfDeath] = 7;
            spellLevels[SpellConstants.SymbolOfWeakness] = 7;
            spellLevels[SpellConstants.WavesOfExhaustion] = 7;
            //Trans
            spellLevels[SpellConstants.ControlWeather] = 7;
            spellLevels[SpellConstants.EtherealJaunt] = 7;
            spellLevels[SpellConstants.ReverseGravity] = 7;
            spellLevels[SpellConstants.Statue] = 7;
            //Univ
            spellLevels[SpellConstants.LimitedWish] = 7;

            //8th-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.DimensionalLock] = 8;
            spellLevels[SpellConstants.MindBlank] = 8;
            spellLevels[SpellConstants.PrismaticWall] = 8;
            spellLevels[SpellConstants.ProtectionFromSpells] = 8;
            //Conj
            spellLevels[SpellConstants.IncendiaryCloud] = 8;
            spellLevels[SpellConstants.Maze] = 8;
            spellLevels[SpellConstants.PlanarBinding_Greater] = 8;
            spellLevels[SpellConstants.SummonMonsterVIII] = 8;
            spellLevels[SpellConstants.TrapTheSoul] = 8;
            //Div
            spellLevels[SpellConstants.DiscernLocation] = 8;
            spellLevels[SpellConstants.MomentOfPrescience] = 8;
            spellLevels[SpellConstants.PryingEyes_Greater] = 8;
            //Ench
            spellLevels[SpellConstants.Antipathy] = 8;
            spellLevels[SpellConstants.Binding] = 8;
            spellLevels[SpellConstants.CharmMonster_Mass] = 8;
            spellLevels[SpellConstants.Demand] = 8;
            spellLevels[SpellConstants.IrresistibleDance] = 8;
            spellLevels[SpellConstants.PowerWordStun] = 8;
            spellLevels[SpellConstants.SymbolOfInsanity] = 8;
            spellLevels[SpellConstants.Sympathy] = 8;
            //Evoc
            spellLevels[SpellConstants.ClenchedFist] = 8;
            spellLevels[SpellConstants.PolarRay] = 8;
            spellLevels[SpellConstants.Shout_Greater] = 8;
            spellLevels[SpellConstants.Sunburst] = 8;
            spellLevels[SpellConstants.TelekineticSphere] = 8;
            //Illus
            spellLevels[SpellConstants.ScintillatingPattern] = 8;
            spellLevels[SpellConstants.Screen] = 8;
            spellLevels[SpellConstants.ShadowEvocation_Greater] = 8;
            //Necro
            spellLevels[SpellConstants.Clone] = 8;
            spellLevels[SpellConstants.CreateGreaterUndead] = 8;
            spellLevels[SpellConstants.HorridWilting] = 8;
            spellLevels[SpellConstants.SymbolOfDeath] = 8;
            //Trans
            spellLevels[SpellConstants.IronBody] = 8;
            spellLevels[SpellConstants.PolymorphAnyObject] = 8;
            spellLevels[SpellConstants.TemporalStasis] = 8;

            //9th-Level Sorcerer/Wizard Spells
            //Abjur
            spellLevels[SpellConstants.Freedom] = 9;
            spellLevels[SpellConstants.Imprisonment] = 9;
            spellLevels[SpellConstants.MagesDisjunction] = 9;
            spellLevels[SpellConstants.PrismaticSphere] = 9;
            //Conj
            spellLevels[SpellConstants.Gate] = 9;
            spellLevels[SpellConstants.Refuge] = 9;
            spellLevels[SpellConstants.SummonMonsterIX] = 9;
            spellLevels[SpellConstants.TeleportationCircle] = 9;
            //Div
            spellLevels[SpellConstants.Foresight] = 9;
            //Ench
            spellLevels[SpellConstants.DominateMonster] = 9;
            spellLevels[SpellConstants.HoldMonster_Mass] = 9;
            spellLevels[SpellConstants.PowerWordKill] = 9;
            //Evoc
            spellLevels[SpellConstants.CrushingHand] = 9;
            spellLevels[SpellConstants.MeteorSwarm] = 9;
            //Illus
            spellLevels[SpellConstants.Shades] = 9;
            spellLevels[SpellConstants.Weird] = 9;
            //Necro
            spellLevels[SpellConstants.AstralProjection] = 9;
            spellLevels[SpellConstants.EnergyDrain] = 9;
            spellLevels[SpellConstants.SoulBind] = 9;
            spellLevels[SpellConstants.WailOfTheBanshee] = 9;
            //Trans
            spellLevels[SpellConstants.Etherealness] = 9;
            spellLevels[SpellConstants.Shapechange] = 9;
            spellLevels[SpellConstants.TimeStop] = 9;
            //Univ
            spellLevels[SpellConstants.Wish] = 9;

            AssertTypesAndAmounts(SpellConstants.Casters.Sorcerer, spellLevels);
        }

        [Test]
        public void AirSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.ObscuringMist] = 1;
            spellLevels[SpellConstants.WindWall] = 2;
            spellLevels[SpellConstants.GaseousForm] = 3;
            spellLevels[SpellConstants.AirWalk] = 4;
            spellLevels[SpellConstants.ControlWinds] = 5;
            spellLevels[SpellConstants.ChainLightning] = 6;
            spellLevels[SpellConstants.ControlWeather] = 7;
            spellLevels[SpellConstants.Whirlwind] = 8;
            spellLevels[SpellConstants.ElementalSwarm] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Air, spellLevels);
        }

        [Test]
        public void AnimalSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.CalmAnimals] = 1;
            spellLevels[SpellConstants.HoldAnimal] = 2;
            spellLevels[SpellConstants.DominateAnimal] = 3;
            spellLevels[SpellConstants.SummonNaturesAllyIV] = 4;
            spellLevels[SpellConstants.CommuneWithNature] = 5;
            spellLevels[SpellConstants.AntilifeShell] = 6;
            spellLevels[SpellConstants.AnimalShapes] = 7;
            spellLevels[SpellConstants.SummonNaturesAllyVIII] = 8;
            spellLevels[SpellConstants.Shapechange] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Animal, spellLevels);
        }

        [Test]
        public void ChaosSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.ProtectionFromLaw] = 1;
            spellLevels[SpellConstants.Shatter] = 2;
            spellLevels[SpellConstants.MagicCircleAgainstLaw] = 3;
            spellLevels[SpellConstants.ChaosHammer] = 4;
            spellLevels[SpellConstants.DispelLaw] = 5;
            spellLevels[SpellConstants.AnimateObjects] = 6;
            spellLevels[SpellConstants.WordOfChaos] = 7;
            spellLevels[SpellConstants.CloakOfChaos] = 8;
            spellLevels[SpellConstants.SummonMonsterIX] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Chaos, spellLevels);
        }

        [Test]
        public void DestructionSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.InflictLightWounds] = 1;
            spellLevels[SpellConstants.Shatter] = 2;
            spellLevels[SpellConstants.Contagion] = 3;
            spellLevels[SpellConstants.InflictCriticalWounds] = 4;
            spellLevels[SpellConstants.InflictLightWounds_Mass] = 5;
            spellLevels[SpellConstants.Harm] = 6;
            spellLevels[SpellConstants.Disintegrate] = 7;
            spellLevels[SpellConstants.Earthquake] = 8;
            spellLevels[SpellConstants.Implosion] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Destruction, spellLevels);
        }

        [Test]
        public void EarthSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.MagicStone] = 1;
            spellLevels[SpellConstants.SoftenEarthAndStone] = 2;
            spellLevels[SpellConstants.StoneShape] = 3;
            spellLevels[SpellConstants.SpikeStones] = 4;
            spellLevels[SpellConstants.WallOfStone] = 5;
            spellLevels[SpellConstants.Stoneskin] = 6;
            spellLevels[SpellConstants.Earthquake] = 7;
            spellLevels[SpellConstants.IronBody] = 8;
            spellLevels[SpellConstants.ElementalSwarm] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Earth, spellLevels);
        }

        [Test]
        public void EnchantmentSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            //0 - Level Sorcerer / Wizard Spells(Cantrips)
            //Ench
            spellLevels[SpellConstants.Daze] = 0;

            //1st - Level Sorcerer / Wizard Spells
            //Ench
            spellLevels[SpellConstants.CharmPerson] = 1;
            spellLevels[SpellConstants.Hypnotism] = 1;
            spellLevels[SpellConstants.Sleep] = 1;

            //2nd - Level Sorcerer / Wizard Spells
            //Ench
            spellLevels[SpellConstants.DazeMonster] = 2;
            spellLevels[SpellConstants.HideousLaughter] = 2;
            spellLevels[SpellConstants.TouchOfIdiocy] = 2;

            //3rd-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.DeepSlumber] = 3;
            spellLevels[SpellConstants.Heroism] = 3;
            spellLevels[SpellConstants.HoldPerson] = 3;
            spellLevels[SpellConstants.Rage] = 3;
            spellLevels[SpellConstants.Suggestion] = 3;

            //4th-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.CharmMonster] = 4;
            spellLevels[SpellConstants.Confusion] = 4;
            spellLevels[SpellConstants.CrushingDespair] = 4;
            spellLevels[SpellConstants.Geas_Lesser] = 4;

            //5th-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.DominatePerson] = 5;
            spellLevels[SpellConstants.Feeblemind] = 5;
            spellLevels[SpellConstants.HoldMonster] = 5;
            spellLevels[SpellConstants.MindFog] = 5;
            spellLevels[SpellConstants.SymbolOfSleep] = 5;

            //6th-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.GeasQuest] = 6;
            spellLevels[SpellConstants.Heroism_Greater] = 6;
            spellLevels[SpellConstants.Suggestion_Mass] = 6;
            spellLevels[SpellConstants.SymbolOfPersuasion] = 6;

            //7th-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.HoldPerson_Mass] = 7;
            spellLevels[SpellConstants.Insanity] = 7;
            spellLevels[SpellConstants.PowerWordBlind] = 7;
            spellLevels[SpellConstants.SymbolOfStunning] = 7;

            //8th-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.Antipathy] = 8;
            spellLevels[SpellConstants.Binding] = 8;
            spellLevels[SpellConstants.CharmMonster_Mass] = 8;
            spellLevels[SpellConstants.Demand] = 8;
            spellLevels[SpellConstants.IrresistibleDance] = 8;
            spellLevels[SpellConstants.PowerWordStun] = 8;
            spellLevels[SpellConstants.SymbolOfInsanity] = 8;
            spellLevels[SpellConstants.Sympathy] = 8;

            //9th-Level Sorcerer/Wizard Spells
            //Ench
            spellLevels[SpellConstants.DominateMonster] = 9;
            spellLevels[SpellConstants.HoldMonster_Mass] = 9;
            spellLevels[SpellConstants.PowerWordKill] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Enchantment, spellLevels);
        }

        [Test]
        public void EvilSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.ProtectionFromGood] = 1;
            spellLevels[SpellConstants.Desecrate] = 2;
            spellLevels[SpellConstants.MagicCircleAgainstGood] = 3;
            spellLevels[SpellConstants.UnholyBlight] = 4;
            spellLevels[SpellConstants.DispelGood] = 5;
            spellLevels[SpellConstants.CreateUndead] = 6;
            spellLevels[SpellConstants.Blasphemy] = 7;
            spellLevels[SpellConstants.UnholyAura] = 8;
            spellLevels[SpellConstants.SummonMonsterIX] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Evil, spellLevels);
        }

        [Test]
        public void FireSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.BurningHands] = 1;
            spellLevels[SpellConstants.ProduceFlame] = 2;
            spellLevels[SpellConstants.ResistEnergy] = 3;
            spellLevels[SpellConstants.WallOfFire] = 4;
            spellLevels[SpellConstants.FireShield] = 5;
            spellLevels[SpellConstants.FireSeeds] = 6;
            spellLevels[SpellConstants.FireStorm] = 7;
            spellLevels[SpellConstants.IncendiaryCloud] = 8;
            spellLevels[SpellConstants.ElementalSwarm] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Fire, spellLevels);
        }

        [Test]
        public void GoodSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.ProtectionFromEvil] = 1;
            spellLevels[SpellConstants.Aid] = 2;
            spellLevels[SpellConstants.MagicCircleAgainstEvil] = 3;
            spellLevels[SpellConstants.HolySmite] = 4;
            spellLevels[SpellConstants.DispelEvil] = 5;
            spellLevels[SpellConstants.BladeBarrier] = 6;
            spellLevels[SpellConstants.HolyWord] = 7;
            spellLevels[SpellConstants.HolyAura] = 8;
            spellLevels[SpellConstants.SummonMonsterIX] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Good, spellLevels);
        }

        [Test]
        public void HealingSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.CureLightWounds] = 1;
            spellLevels[SpellConstants.CureModerateWounds] = 2;
            spellLevels[SpellConstants.CureSeriousWounds] = 3;
            spellLevels[SpellConstants.CureCriticalWounds] = 4;
            spellLevels[SpellConstants.CureLightWounds_Mass] = 5;
            spellLevels[SpellConstants.Heal] = 6;
            spellLevels[SpellConstants.Regenerate] = 7;
            spellLevels[SpellConstants.CureCriticalWounds_Mass] = 8;
            spellLevels[SpellConstants.Heal_Mass] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Healing, spellLevels);
        }

        [Test]
        public void IllusionSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            //0 - Level Sorcerer / Wizard Spells(Cantrips)
            //Illus
            spellLevels[SpellConstants.GhostSound] = 0;

            //1st - Level Sorcerer / Wizard Spells
            //Illus
            spellLevels[SpellConstants.ColorSpray] = 1;
            spellLevels[SpellConstants.DisguiseSelf] = 1;
            spellLevels[SpellConstants.MagicAura] = 1;
            spellLevels[SpellConstants.SilentImage] = 1;
            spellLevels[SpellConstants.Ventriloquism] = 1;

            //2nd - Level Sorcerer / Wizard Spells
            //Illus
            spellLevels[SpellConstants.Blur] = 2;
            spellLevels[SpellConstants.HypnoticPattern] = 2;
            spellLevels[SpellConstants.Invisibility] = 2;
            spellLevels[SpellConstants.MagicMouth] = 2;
            spellLevels[SpellConstants.MinorImage] = 2;
            spellLevels[SpellConstants.MirrorImage] = 2;
            spellLevels[SpellConstants.Misdirection] = 2;
            spellLevels[SpellConstants.PhantomTrap] = 2;

            //3rd-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.Displacement] = 3;
            spellLevels[SpellConstants.IllusoryScript] = 3;
            spellLevels[SpellConstants.InvisibilitySphere] = 3;
            spellLevels[SpellConstants.MajorImage] = 3;

            //4th-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.HallucinatoryTerrain] = 4;
            spellLevels[SpellConstants.IllusoryWall] = 4;
            spellLevels[SpellConstants.Invisibility_Greater] = 4;
            spellLevels[SpellConstants.PhantasmalKiller] = 4;
            spellLevels[SpellConstants.RainbowPattern] = 4;
            spellLevels[SpellConstants.ShadowConjuration] = 4;

            //5th-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.Dream] = 5;
            spellLevels[SpellConstants.FalseVision] = 5;
            spellLevels[SpellConstants.MirageArcana] = 5;
            spellLevels[SpellConstants.Nightmare] = 5;
            spellLevels[SpellConstants.PersistentImage] = 5;
            spellLevels[SpellConstants.Seeming] = 5;
            spellLevels[SpellConstants.ShadowEvocation] = 5;

            //6th-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.Mislead] = 6;
            spellLevels[SpellConstants.PermanentImage] = 6;
            spellLevels[SpellConstants.ProgrammedImage] = 6;
            spellLevels[SpellConstants.ShadowWalk] = 6;
            spellLevels[SpellConstants.Veil] = 6;

            //7th-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.Invisibility_Mass] = 7;
            spellLevels[SpellConstants.ProjectImage] = 7;
            spellLevels[SpellConstants.ShadowConjuration_Greater] = 7;
            spellLevels[SpellConstants.Simulacrum] = 7;

            //8th-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.ScintillatingPattern] = 8;
            spellLevels[SpellConstants.Screen] = 8;
            spellLevels[SpellConstants.ShadowEvocation_Greater] = 8;

            //9th-Level Sorcerer/Wizard Spells
            //Illus
            spellLevels[SpellConstants.Shades] = 9;
            spellLevels[SpellConstants.Weird] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Illusion, spellLevels);
        }

        [Test]
        public void KnowledgeSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.DetectSecretDoors] = 1;
            spellLevels[SpellConstants.DetectThoughts] = 2;
            spellLevels[SpellConstants.ClairaudienceClairvoyance] = 3;
            spellLevels[SpellConstants.Divination] = 4;
            spellLevels[SpellConstants.TrueSeeing] = 5;
            spellLevels[SpellConstants.FindThePath] = 6;
            spellLevels[SpellConstants.LegendLore] = 7;
            spellLevels[SpellConstants.DiscernLocation] = 8;
            spellLevels[SpellConstants.Foresight] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Knowledge, spellLevels);
        }

        [Test]
        public void LawSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.ProtectionFromChaos] = 1;
            spellLevels[SpellConstants.CalmEmotions] = 2;
            spellLevels[SpellConstants.MagicCircleAgainstChaos] = 3;
            spellLevels[SpellConstants.OrdersWrath] = 4;
            spellLevels[SpellConstants.DispelChaos] = 5;
            spellLevels[SpellConstants.HoldMonster] = 6;
            spellLevels[SpellConstants.Dictum] = 7;
            spellLevels[SpellConstants.ShieldOfLaw] = 8;
            spellLevels[SpellConstants.SummonMonsterIX] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Law, spellLevels);
        }

        [Test]
        public void LuckSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.EntropicShield] = 1;
            spellLevels[SpellConstants.Aid] = 2;
            spellLevels[SpellConstants.ProtectionFromEnergy] = 3;
            spellLevels[SpellConstants.FreedomOfMovement] = 4;
            spellLevels[SpellConstants.BreakEnchantment] = 5;
            spellLevels[SpellConstants.Mislead] = 6;
            spellLevels[SpellConstants.SpellTurning] = 7;
            spellLevels[SpellConstants.MomentOfPrescience] = 8;
            spellLevels[SpellConstants.Miracle] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Luck, spellLevels);
        }

        [Test]
        public void PlantSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.Entangle] = 1;
            spellLevels[SpellConstants.Barkskin] = 2;
            spellLevels[SpellConstants.PlantGrowth] = 3;
            spellLevels[SpellConstants.CommandPlants] = 4;
            spellLevels[SpellConstants.WallOfThorns] = 5;
            spellLevels[SpellConstants.RepelWood] = 6;
            spellLevels[SpellConstants.AnimatePlants] = 7;
            spellLevels[SpellConstants.ControlPlants] = 8;
            spellLevels[SpellConstants.Shambler] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Plant, spellLevels);
        }

        [Test]
        public void ProtectionSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.Sanctuary] = 1;
            spellLevels[SpellConstants.ShieldOther] = 2;
            spellLevels[SpellConstants.ProtectionFromEnergy] = 3;
            spellLevels[SpellConstants.SpellImmunity] = 4;
            spellLevels[SpellConstants.SpellResistance] = 5;
            spellLevels[SpellConstants.AntimagicField] = 6;
            spellLevels[SpellConstants.Repulsion] = 7;
            spellLevels[SpellConstants.MindBlank] = 8;
            spellLevels[SpellConstants.PrismaticSphere] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Protection, spellLevels);
        }

        [Test]
        public void SunSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.EndureElements] = 1;
            spellLevels[SpellConstants.HeatMetal] = 2;
            spellLevels[SpellConstants.SearingLight] = 3;
            spellLevels[SpellConstants.FireShield] = 4;
            spellLevels[SpellConstants.FlameStrike] = 5;
            spellLevels[SpellConstants.FireSeeds] = 6;
            spellLevels[SpellConstants.Sunbeam] = 7;
            spellLevels[SpellConstants.Sunburst] = 8;
            spellLevels[SpellConstants.PrismaticSphere] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Sun, spellLevels);
        }

        [Test]
        public void TrickerySpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.DisguiseSelf] = 1;
            spellLevels[SpellConstants.Invisibility] = 2;
            spellLevels[SpellConstants.Nondetection] = 3;
            spellLevels[SpellConstants.Confusion] = 4;
            spellLevels[SpellConstants.FalseVision] = 5;
            spellLevels[SpellConstants.Mislead] = 6;
            spellLevels[SpellConstants.Screen] = 7;
            spellLevels[SpellConstants.PolymorphAnyObject] = 8;
            spellLevels[SpellConstants.TimeStop] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Trickery, spellLevels);
        }

        [Test]
        public void WarSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.MagicWeapon] = 1;
            spellLevels[SpellConstants.SpiritualWeapon] = 2;
            spellLevels[SpellConstants.MagicVestment] = 3;
            spellLevels[SpellConstants.DivinePower] = 4;
            spellLevels[SpellConstants.FlameStrike] = 5;
            spellLevels[SpellConstants.BladeBarrier] = 6;
            spellLevels[SpellConstants.PowerWordBlind] = 7;
            spellLevels[SpellConstants.PowerWordStun] = 8;
            spellLevels[SpellConstants.PowerWordKill] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.War, spellLevels);
        }

        [Test]
        public void WaterSpellLevels()
        {
            var spellLevels = new Dictionary<string, int>();

            spellLevels[SpellConstants.ObscuringMist] = 1;
            spellLevels[SpellConstants.FogCloud] = 2;
            spellLevels[SpellConstants.WaterBreathing] = 3;
            spellLevels[SpellConstants.ControlWater] = 4;
            spellLevels[SpellConstants.IceStorm] = 5;
            spellLevels[SpellConstants.ConeOfCold] = 6;
            spellLevels[SpellConstants.AcidFog] = 7;
            spellLevels[SpellConstants.HorridWilting] = 8;
            spellLevels[SpellConstants.ElementalSwarm] = 9;

            AssertTypesAndAmounts(SpellConstants.Domains.Water, spellLevels);
        }
    }
}
