using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Generators.Abilities;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Skills;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.CreatureGen.Verifiers;
using DnDGen.CreatureGen.Verifiers.Exceptions;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Diagnostics;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Generators
{
    [TestFixture]
    public class CreatureGeneratorTests : IntegrationTests
    {
        private CreatureAsserter creatureAsserter;
        private ICreatureGenerator creatureGenerator;
        private Stopwatch stopwatch;
        private AbilityRandomizerFactory abilityRandomizerFactory;
        private ICreatureVerifier creatureVerifier;

        [SetUp]
        public void Setup()
        {
            creatureAsserter = GetNewInstanceOf<CreatureAsserter>();
            creatureGenerator = GetNewInstanceOf<ICreatureGenerator>();
            creatureVerifier = GetNewInstanceOf<ICreatureVerifier>();
            stopwatch = new Stopwatch();
            abilityRandomizerFactory = GetNewInstanceOf<AbilityRandomizerFactory>();
        }

        [TestCase(CreatureConstants.Androsphinx)]
        [TestCase(CreatureConstants.Angel_Planetar)]
        [TestCase(CreatureConstants.Angel_Solar)]
        [TestCase(CreatureConstants.Aranea)]
        [TestCase(CreatureConstants.Couatl)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_White_Old)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld)]
        [TestCase(CreatureConstants.Dragon_White_Ancient)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Brass_Young)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Brass_Old)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm)]
        [TestCase(CreatureConstants.Drider)]
        [TestCase(CreatureConstants.FormianQueen)]
        [TestCase(CreatureConstants.Lammasu)]
        [TestCase(CreatureConstants.Lillend)]
        [TestCase(CreatureConstants.Naga_Dark)]
        [TestCase(CreatureConstants.Naga_Guardian)]
        [TestCase(CreatureConstants.Naga_Spirit)]
        [TestCase(CreatureConstants.Naga_Water)]
        [TestCase(CreatureConstants.Nymph)]
        [TestCase(CreatureConstants.Rakshasa)]
        [TestCase(CreatureConstants.TrumpetArchon)]
        public void Generate_SpellsForThoseWhoCastAsSpellcaster(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);

            Assert.That(creature.Magic, Is.Not.Null);
            Assert.That(creature.Magic.Caster, Is.Not.Empty);
            CreatureAsserter.AssertMagic(creature, creature.Summary);
        }

        [TestCase(CreatureConstants.Human)]
        [TestCase(CreatureConstants.Dwarf_Hill)]
        [TestCase(CreatureConstants.Elf_Half)]
        [TestCase(CreatureConstants.Elf_High)]
        [TestCase(CreatureConstants.Gnome_Rock)]
        [TestCase(CreatureConstants.Halfling_Lightfoot)]
        [TestCase(CreatureConstants.Orc)]
        [TestCase(CreatureConstants.Orc_Half)]
        [TestCase(CreatureConstants.Goblin)]
        [TestCase(CreatureConstants.Ogre)]
        [TestCase(CreatureConstants.Balor)]
        [TestCase(CreatureConstants.Titan)]
        [TestCase(CreatureConstants.Giant_Cloud)]
        public void Generate_Weapons(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Equipment, Is.Not.Null);
            Assert.That(creature.Equipment.Weapons, Is.Not.Empty.And.All.Not.Null);
        }

        [TestCase(CreatureConstants.Titan)]
        public void BUG_Generate_OversizedWeaponHasCorrectAttackDamage(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Equipment, Is.Not.Null);
            Assert.That(creature.Equipment.Weapons, Is.Not.Empty.And.All.Not.Null);

            var oversizedFeat = creature.SpecialQualities.FirstOrDefault(sq => sq.Name == FeatConstants.SpecialQualities.OversizedWeapon);
            Assert.That(oversizedFeat, Is.Not.Null);
            Assert.That(oversizedFeat.Foci, Is.Not.Empty);
            Assert.That(oversizedFeat.Foci.Count(), Is.EqualTo(1));

            var oversizedSize = oversizedFeat.Foci.First();

            var weaponNames = WeaponConstants.GetAllWeapons(true, false);
            var unnaturalAttacks = creature.Attacks.Where(a => !a.IsNatural && weaponNames.Contains(a.Name));

            foreach (var attack in unnaturalAttacks)
            {
                var weapon = creature.Equipment.Weapons.FirstOrDefault(w => w.Name == attack.Name);
                Assert.That(weapon, Is.Not.Null, $"{creature.Summary}: {attack.Name}");
                Assert.That(weapon.DamageSummary, Is.Not.Empty, $"{creature.Summary}: {weapon.Summary}");
                Assert.That(weaponNames, Contains.Item(weapon.Name), $"{creature.Summary}: {weapon.Summary}");

                Assert.That(attack.Damages, Is.Not.Empty.And.Count.EqualTo(weapon.Damages.Count), $"{creature.Summary}; Weapon: {weapon.Summary}");

                for (var i = 0; i < weapon.Damages.Count; i++)
                {
                    if (i == 0)
                    {
                        Assert.That(attack.DamageSummary, Contains.Substring(weapon.Damages[i].Roll), $"{creature.Summary}; Weapon: {weapon.Summary}");
                        Assert.That(attack.DamageSummary, Contains.Substring(weapon.Damages[i].Type), $"{creature.Summary}; Weapon: {weapon.Summary}");
                        Assert.That(attack.DamageSummary, Contains.Substring(weapon.Damages[i].Condition), $"{creature.Summary}; Weapon: {weapon.Summary}");
                    }
                    else
                    {
                        Assert.That(attack.DamageSummary, Contains.Substring(weapon.Damages[i].Summary), $"{creature.Summary}; Weapon: {weapon.Summary}");
                    }
                }

                if (weapon.Attributes.Contains(AttributeConstants.Melee))
                {
                    Assert.That(attack.AttackType, Contains.Substring("melee"), $"{creature.Summary} ({creature.Size}): {weapon.Summary} ({weapon.Size}) [Oversized: {oversizedSize}]");
                }
                else if (weapon.Attributes.Contains(AttributeConstants.Ranged))
                {
                    Assert.That(attack.AttackType, Contains.Substring("ranged"), $"{creature.Summary} ({creature.Size}): {weapon.Summary} ({weapon.Size}) [Oversized: {oversizedSize}]");
                }
            }
        }

        [TestCase(CreatureConstants.Human)]
        [TestCase(CreatureConstants.Dwarf_Hill)]
        [TestCase(CreatureConstants.Elf_Half)]
        [TestCase(CreatureConstants.Elf_High)]
        [TestCase(CreatureConstants.Gnome_Rock)]
        [TestCase(CreatureConstants.Halfling_Lightfoot)]
        [TestCase(CreatureConstants.Orc)]
        [TestCase(CreatureConstants.Orc_Half)]
        [TestCase(CreatureConstants.Goblin)]
        [TestCase(CreatureConstants.Ogre)]
        public void Generate_Armor(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Equipment, Is.Not.Null);
            Assert.That(creature.Equipment.Armor, Is.Not.Null);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void Generate_Creature(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Characters))]
        public void Generate_CreatureAsCharacter(string creatureName)
        {
            var creature = creatureGenerator.Generate(true, creatureName);
            creatureAsserter.AssertCreatureAsCharacter(creature);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void Generate_BasicTemplate(string template)
        {
            var creatureName = CreatureConstants.Human;
            if (template == CreatureConstants.Templates.Lycanthrope_Rat_Afflicted
                || template == CreatureConstants.Templates.Lycanthrope_Rat_Natural)
            {
                creatureName = CreatureConstants.Kobold;
            }
            else if (template == CreatureConstants.Templates.Lich)
            {
                Assert.Ignore($"Template {template} must be a character");
            }
            else if (template == CreatureConstants.Templates.Vampire)
            {
                creatureName = CreatureConstants.Minotaur;
            }

            var creature = creatureGenerator.Generate(false, creatureName, null, template);
            creatureAsserter.AssertCreature(creature);

            if (template != CreatureConstants.Templates.None)
                Assert.That(creature.Templates, Has.Count.EqualTo(1).And.Contains(template));
            else
                Assert.That(creature.Templates, Is.Empty);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void Generate_BasicTemplateAsCharacter(string template)
        {
            var creatureName = CreatureConstants.Human;
            if (template == CreatureConstants.Templates.Lycanthrope_Rat_Afflicted
                || template == CreatureConstants.Templates.Lycanthrope_Rat_Natural)
            {
                creatureName = CreatureConstants.Kobold;
            }
            else if (template == CreatureConstants.Templates.Skeleton || template == CreatureConstants.Templates.Zombie)
            {
                Assert.Ignore($"Template {template} cannot be a character");
            }

            var creature = creatureGenerator.Generate(true, creatureName, null, template);
            creatureAsserter.AssertCreatureAsCharacter(creature);

            if (template != CreatureConstants.Templates.None)
                Assert.That(creature.Templates, Has.Count.EqualTo(1).And.Contains(template));
            else
                Assert.That(creature.Templates, Is.Empty);
        }

        [TestCase(false, CreatureConstants.Aboleth)]
        [TestCase(true, CreatureConstants.Aboleth)]
        [TestCase(false, CreatureConstants.Ape, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Ape_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Badger, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Badger_Dire, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Basilisk, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Basilisk_Greater, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Bat, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bat, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Bat_Dire, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bat_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Bat_Swarm, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bat_Swarm, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Bear_Black, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bear_Brown, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bear_Polar, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bear_Dire, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bee_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Bison, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Boar, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Boar_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.BombardierBeetle_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(true, CreatureConstants.Bugbear)]
        [TestCase(false, CreatureConstants.Bugbear, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Cat, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Cat, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Centipede_Monstrous_Medium, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Centipede_Monstrous_Large, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Centipede_Monstrous_Huge, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Centipede_Monstrous_Gargantuan, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Centipede_Monstrous_Colossal, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Chimera_Black, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Chimera_Blue, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Chimera_Green, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Chimera_Red, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Chimera_White, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Crocodile, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Crocodile_Giant, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Deinonychus, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Dog, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Dog_Riding, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Dragon_Black_Young, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Dragon_Brass_Young, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Dragon_Brass_Young, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Dragon_Red_Wyrmling, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Dragon_Red_VeryYoung, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Dragon_Red_Young, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Dragon_Red_Juvenile, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Dragon_Red_YoungAdult, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Dragon_White_Old, CreatureConstants.Templates.HalfFiend)]
        [TestCase(false, CreatureConstants.Eagle, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Eagle_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Elasmosaurus, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Elephant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Elf_Half)]
        [TestCase(true, CreatureConstants.Elf_Half)]
        [TestCase(false, CreatureConstants.Elf_Half, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(true, CreatureConstants.Elf_Half, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Elf_Half, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(true, CreatureConstants.Elf_Half, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Elf_Half, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(true, CreatureConstants.Elf_Half, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Elf_Half, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.Elf_Half, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.Elf_Half, CreatureConstants.Templates.Lich)]
        [TestCase(true, CreatureConstants.Elf_Half, CreatureConstants.Templates.Vampire)]
        [TestCase(true, CreatureConstants.Elf_High, CreatureConstants.Templates.Lich)]
        [TestCase(true, CreatureConstants.Elf_High, CreatureConstants.Templates.Vampire)]
        [TestCase(false, CreatureConstants.Ettin, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.FireBeetle_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Gargoyle)]
        [TestCase(true, CreatureConstants.Gargoyle)]
        [TestCase(false, CreatureConstants.Gargoyle, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Gargoyle_Kapoacinth, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Giant_Cloud, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Giant_Frost)]
        [TestCase(true, CreatureConstants.Giant_Frost)]
        [TestCase(false, CreatureConstants.Giant_Hill, CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted)]
        [TestCase(false, CreatureConstants.Giant_Hill, CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural)]
        [TestCase(false, CreatureConstants.Girallon, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.GrayRender, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Griffon, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Grig, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Grig_WithFiddle, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Harpy)]
        [TestCase(true, CreatureConstants.Harpy)]
        [TestCase(false, CreatureConstants.Hawk, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Hawk, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Hieracosphinx, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Hippogriff, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Horse_Heavy, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Horse_Heavy_War, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Horse_Light, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Horse_Light_War, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Human)]
        [TestCase(true, CreatureConstants.Human)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Ghost)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.HalfDragon_Black)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.HalfDragon_Black)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.HalfFiend)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.HalfDragon_Black, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.HalfDragon_Black, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lich)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Boar_Afflicted)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Boar_Afflicted)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Boar_Natural)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Boar_Natural)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Tiger_Natural)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Tiger_Natural)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted, CreatureConstants.Templates.HalfDragon_White)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted, CreatureConstants.Templates.HalfDragon_White)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Natural)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Natural)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Afflicted)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Afflicted)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Natural)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Lycanthrope_Wolf_Dire_Natural)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Skeleton)]
        [TestCase(true, CreatureConstants.Human, CreatureConstants.Templates.Vampire)]
        [TestCase(false, CreatureConstants.Human,
            CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted,
            CreatureConstants.Templates.HalfDragon_Blue,
            CreatureConstants.Templates.Ghost)]
        [TestCase(true, CreatureConstants.Human,
            CreatureConstants.Templates.Lycanthrope_Wolf_Afflicted,
            CreatureConstants.Templates.HalfDragon_Blue,
            CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Human, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Kobold)]
        [TestCase(true, CreatureConstants.Kobold)]
        [TestCase(false, CreatureConstants.Kobold, CreatureConstants.Templates.Zombie)]
        [TestCase(true, CreatureConstants.Kobold, CreatureConstants.Templates.Lycanthrope_Rat_Afflicted)]
        [TestCase(false, CreatureConstants.Kobold, CreatureConstants.Templates.Lycanthrope_Rat_Afflicted)]
        [TestCase(true, CreatureConstants.Kobold, CreatureConstants.Templates.Lycanthrope_Rat_Natural)]
        [TestCase(false, CreatureConstants.Kobold, CreatureConstants.Templates.Lycanthrope_Rat_Natural)]
        [TestCase(true, CreatureConstants.Lammasu)]
        [TestCase(false, CreatureConstants.Lammasu)]
        [TestCase(false, CreatureConstants.Lammasu, CreatureConstants.Templates.HalfDragon_Gold)]
        [TestCase(false, CreatureConstants.Lammasu, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(true, CreatureConstants.Lammasu, CreatureConstants.Templates.CelestialCreature, CreatureConstants.Templates.HalfDragon_Gold)]
        [TestCase(false, CreatureConstants.Lammasu, CreatureConstants.Templates.CelestialCreature, CreatureConstants.Templates.HalfDragon_Gold)]
        [TestCase(true, CreatureConstants.Lammasu, CreatureConstants.Templates.HalfDragon_Gold, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Lammasu, CreatureConstants.Templates.HalfDragon_Gold, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Lion, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Lion_Dire, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Lizard, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Lizard, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(true, CreatureConstants.Medusa, CreatureConstants.Templates.HalfDragon_Red)]
        [TestCase(false, CreatureConstants.Megaraptor, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Megaraptor, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Minotaur)]
        [TestCase(true, CreatureConstants.Minotaur)]
        [TestCase(false, CreatureConstants.Minotaur, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Monkey, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Mummy)]
        [TestCase(true, CreatureConstants.Mummy)]
        [TestCase(false, CreatureConstants.Octopus, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Octopus_Giant, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Ogre)]
        [TestCase(true, CreatureConstants.Ogre)]
        [TestCase(false, CreatureConstants.Ogre, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Orc_Half)]
        [TestCase(true, CreatureConstants.Orc_Half)]
        [TestCase(false, CreatureConstants.Orc_Half, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(true, CreatureConstants.Orc_Half, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Orc_Half, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(true, CreatureConstants.Orc_Half, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Orc_Half, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.Orc_Half, CreatureConstants.Templates.HalfFiend)]
        [TestCase(false, CreatureConstants.Orc_Half, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(true, CreatureConstants.Orc_Half, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Otyugh, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Otyugh, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Owl, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Owl, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Owl_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Owlbear, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Pony, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Pony_War, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Porpoise, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.PrayingMantis_Giant, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Rakshasa)]
        [TestCase(true, CreatureConstants.Rakshasa)]
        [TestCase(false, CreatureConstants.Rat, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Rat, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Rat_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Raven, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Raven, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.RazorBoar, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Rhinoceras, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Roc, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Scorpion_Monstrous_Small, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Scorpion_Monstrous_Medium, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Scorpion_Monstrous_Large, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Scorpion_Monstrous_Huge, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Scorpion_Monstrous_Gargantuan, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.SeaCat, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.ShamblingMound)]
        [TestCase(true, CreatureConstants.ShamblingMound)]
        [TestCase(false, CreatureConstants.ShamblingMound, CreatureConstants.Templates.HalfFiend)]
        [TestCase(true, CreatureConstants.ShamblingMound, CreatureConstants.Templates.HalfFiend)]
        [TestCase(false, CreatureConstants.Shark_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Shark_Huge, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Shark_Large, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Shark_Large, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Shark_Medium, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Shrieker, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Skum)]
        [TestCase(true, CreatureConstants.Skum)]
        [TestCase(false, CreatureConstants.Skum, CreatureConstants.Templates.Ghost)]
        [TestCase(true, CreatureConstants.Skum, CreatureConstants.Templates.Ghost)]
        [TestCase(false, CreatureConstants.Snake_Constrictor, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Constrictor, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Snake_Constrictor_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Constrictor_Giant, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Tiny, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Tiny, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Small, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Small, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Medium, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Medium, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Large, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Large, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Huge, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Snake_Viper_Huge, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_Hunter_Small, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_WebSpinner_Small, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_Hunter_Medium, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_WebSpinner_Medium, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_Hunter_Large, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_WebSpinner_Large, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_Hunter_Huge, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_WebSpinner_Huge, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_Hunter_Gargantuan, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_Hunter_Colossal, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Spider_Monstrous_WebSpinner_Colossal, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Squid, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Squid_Giant, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Squid_Giant, CreatureConstants.Templates.HalfDragon_Blue)]
        [TestCase(false, CreatureConstants.Squid_Giant, CreatureConstants.Templates.HalfDragon_Green)]
        [TestCase(false, CreatureConstants.StagBeetle_Giant, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Tiger, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Tiger_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Toad, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Toad, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Triceratops, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Troglodyte, CreatureConstants.Templates.HalfCelestial)]
        [TestCase(false, CreatureConstants.Troglodyte, CreatureConstants.Templates.Zombie)]
        [TestCase(false, CreatureConstants.Troll)]
        [TestCase(true, CreatureConstants.Troll)]
        [TestCase(false, CreatureConstants.Troll, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Tyrannosaurus, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Unicorn, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Wasp_Giant, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Weasel, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Weasel, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Weasel_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Whale_Baleen, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Whale_Cachalot, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Whale_Orca, CreatureConstants.Templates.CelestialCreature)]
        [TestCase(false, CreatureConstants.Wolf, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Wolf, CreatureConstants.Templates.Skeleton)]
        [TestCase(false, CreatureConstants.Wolf_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Wolverine, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Wolverine_Dire, CreatureConstants.Templates.FiendishCreature)]
        [TestCase(false, CreatureConstants.Wyvern, CreatureConstants.Templates.Zombie)]
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicCreaturesTestCases))]
        public void Generate_CanGenerateTemplate(bool asCharacter, string creatureName, params string[] templates)
        {
            var creature = creatureGenerator.Generate(asCharacter, creatureName, null, templates);
            Assert.That(creature.Name, Is.EqualTo(creatureName));
            Assert.That(creature.Templates, Is.EqualTo(templates));

            creatureAsserter.AssertCreature(creature, asCharacter);
        }

        [TestCase(CreatureConstants.Destrachan)]
        [TestCase(CreatureConstants.Grimlock)]
        [TestCase(CreatureConstants.Yrthak)]
        public void BUG_Generate_DoesNotHaveSight(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);

            Assert.That(creature.SpecialQualities, Is.Not.Empty);

            var specialQualityNames = creature.SpecialQualities.Select(q => q.Name);
            Assert.That(specialQualityNames, Contains.Item(FeatConstants.SpecialQualities.Blind));
            Assert.That(specialQualityNames, Contains.Item(FeatConstants.SpecialQualities.Blindsight));
            Assert.That(specialQualityNames, Does.Not.Contain(FeatConstants.SpecialQualities.AllAroundVision)
                .And.Not.Contain(FeatConstants.SpecialQualities.Darkvision)
                .And.Not.Contain(FeatConstants.SpecialQualities.LowLightVision)
                .And.Not.Contain(FeatConstants.SpecialQualities.LowLightVision_Superior));
        }

        [TestCase(CreatureConstants.Elf_Aquatic)]
        [TestCase(CreatureConstants.Elf_Drow)]
        [TestCase(CreatureConstants.Elf_Gray)]
        [TestCase(CreatureConstants.Elf_Half)]
        [TestCase(CreatureConstants.Elf_High)]
        [TestCase(CreatureConstants.Elf_Wild)]
        [TestCase(CreatureConstants.Elf_Wood)]
        public void BUG_Generate_ElfCanUseShield(string elfName)
        {
            var elf = creatureGenerator.Generate(false, elfName);
            creatureAsserter.AssertCreature(elf);

            Assert.That(elf.SpecialQualities, Is.Not.Empty);

            var specialQualityNames = elf.SpecialQualities.Select(q => q.Name);
            Assert.That(specialQualityNames, Contains.Item(FeatConstants.ShieldProficiency));
        }

        [Test]
        public void BUG_Generate_HalfOrcIsNotSensitiveToLight()
        {
            var halfOrc = creatureGenerator.Generate(false, CreatureConstants.Orc_Half);
            creatureAsserter.AssertCreature(halfOrc);

            Assert.That(halfOrc.SpecialQualities, Is.Not.Empty);

            var specialQualityNames = halfOrc.SpecialQualities.Select(q => q.Name);
            Assert.That(specialQualityNames, Does.Not.Contain(FeatConstants.SpecialQualities.LightSensitivity));
        }

        [Test]
        public void BUG_Generate_NightcrawlerHasConcentration()
        {
            var nightcrawler = creatureGenerator.Generate(false, CreatureConstants.Nightcrawler);
            creatureAsserter.AssertCreature(nightcrawler);

            Assert.That(nightcrawler.Skills, Is.Not.Empty);

            var skillNames = nightcrawler.Skills.Select(q => q.Name);
            Assert.That(skillNames, Contains.Item(SkillConstants.Concentration));
        }

        [TestCase(AbilityConstants.RandomizerRolls.Heroic, 15, 18)]
        [TestCase(AbilityConstants.RandomizerRolls.BestOfFour, 3, 18)]
        [TestCase(AbilityConstants.RandomizerRolls.Default, 10, 11)]
        [TestCase(AbilityConstants.RandomizerRolls.Average, 10, 13)]
        [TestCase(AbilityConstants.RandomizerRolls.Good, 13, 16)]
        [TestCase(AbilityConstants.RandomizerRolls.OnesAsSixes, 6, 18)]
        [TestCase(AbilityConstants.RandomizerRolls.Poor, 3, 9)]
        [TestCase(AbilityConstants.RandomizerRolls.Raw, 3, 18)]
        [TestCase(AbilityConstants.RandomizerRolls.Wild, 2, 20)]
        [TestCase("42d600+9266", 42 + 9266, 42 * 600 + 9266)]
        public void Generate_HumanWithAbilityRandomizer(string roll, int lower, int upper)
        {
            var randomizer = new AbilityRandomizer(roll);

            var creature = creatureGenerator.Generate(false, CreatureConstants.Human, randomizer);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Abilities.Values.Select(v => v.BaseScore), Is.All.InRange(lower, upper));
        }

        [Test]
        public void Generate_HumanWithSetRolls()
        {
            var randomizer = new AbilityRandomizer();
            randomizer.SetRolls[AbilityConstants.Charisma] = 9266;
            randomizer.SetRolls[AbilityConstants.Constitution] = 90210;
            randomizer.SetRolls[AbilityConstants.Dexterity] = 42;
            randomizer.SetRolls[AbilityConstants.Intelligence] = 600;
            randomizer.SetRolls[AbilityConstants.Strength] = 1337;
            randomizer.SetRolls[AbilityConstants.Wisdom] = 1336;

            var creature = creatureGenerator.Generate(false, CreatureConstants.Human, randomizer);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Abilities[AbilityConstants.Charisma].BaseScore, Is.EqualTo(9266));
            Assert.That(creature.Abilities[AbilityConstants.Constitution].BaseScore, Is.EqualTo(90210));
            Assert.That(creature.Abilities[AbilityConstants.Dexterity].BaseScore, Is.EqualTo(42));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].BaseScore, Is.EqualTo(600));
            Assert.That(creature.Abilities[AbilityConstants.Strength].BaseScore, Is.EqualTo(1337));
            Assert.That(creature.Abilities[AbilityConstants.Wisdom].BaseScore, Is.EqualTo(1336));
        }

        [Test]
        public void Generate_HumanWithAdvancementBonuses()
        {
            var randomizer = new AbilityRandomizer();
            randomizer.AbilityAdvancements[AbilityConstants.Charisma] = 9266;
            randomizer.AbilityAdvancements[AbilityConstants.Constitution] = 90210;
            randomizer.AbilityAdvancements[AbilityConstants.Dexterity] = 42;
            randomizer.AbilityAdvancements[AbilityConstants.Intelligence] = 600;
            randomizer.AbilityAdvancements[AbilityConstants.Strength] = 1337;
            randomizer.AbilityAdvancements[AbilityConstants.Wisdom] = 1336;

            var creature = creatureGenerator.Generate(false, CreatureConstants.Human, randomizer);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Abilities[AbilityConstants.Charisma].AdvancementAdjustment, Is.EqualTo(9266));
            Assert.That(creature.Abilities[AbilityConstants.Constitution].AdvancementAdjustment, Is.EqualTo(90210));
            Assert.That(creature.Abilities[AbilityConstants.Dexterity].AdvancementAdjustment, Is.EqualTo(42));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].AdvancementAdjustment, Is.EqualTo(600));
            Assert.That(creature.Abilities[AbilityConstants.Strength].AdvancementAdjustment, Is.EqualTo(1337));
            Assert.That(creature.Abilities[AbilityConstants.Wisdom].AdvancementAdjustment, Is.EqualTo(1336));
        }

        [TestCase(AbilityConstants.Charisma)]
        [TestCase(AbilityConstants.Constitution)]
        [TestCase(AbilityConstants.Dexterity)]
        [TestCase(AbilityConstants.Intelligence)]
        [TestCase(AbilityConstants.Strength)]
        [TestCase(AbilityConstants.Wisdom)]
        public void Generate_HumanWithPriorityAbility(string ability)
        {
            var randomizer = new AbilityRandomizer(AbilityConstants.RandomizerRolls.Wild)
            {
                PriorityAbility = ability,
            };

            var creature = creatureGenerator.Generate(false, CreatureConstants.Human, randomizer);
            creatureAsserter.AssertCreature(creature);
            Assert.That(creature.Abilities.Values.Max(v => v.BaseScore), Is.EqualTo(creature.Abilities[ability].BaseScore));
        }

        [Test]
        [Repeat(10)] //INFO: We have to repeat to ensure we get males, since gender is random
        public void BUG_Generate_MaleSpiderEaterDoesNotHaveImplantAbility()
        {
            var spiderEater = creatureGenerator.Generate(false, CreatureConstants.SpiderEater);
            creatureAsserter.AssertCreature(spiderEater);

            Assert.That(spiderEater.Attacks, Is.Not.Empty);
            Assert.That(spiderEater.Demographics.Gender, Is.EqualTo(GenderConstants.Male).Or.EqualTo(GenderConstants.Female));

            var attackNames = spiderEater.Attacks.Select(q => q.Name);

            if (spiderEater.Demographics.Gender == GenderConstants.Male)
            {
                Assert.That(attackNames, Does.Not.Contain("Implant"));
            }
            else if (spiderEater.Demographics.Gender == GenderConstants.Female)
            {
                Assert.That(attackNames, Contains.Item("Implant"));
            }
        }

        [Test]
        [Repeat(10)] //INFO: We have to repeat to ensure we get males, since gender is random
        public void Generate_GenderSpecificAppearancesAreGenerated()
        {
            var bison = creatureGenerator.Generate(false, CreatureConstants.Bison);
            creatureAsserter.AssertCreature(bison);

            Assert.That(bison.Demographics.Other, Is.Not.Empty);
            Assert.That(bison.Demographics.Gender, Is.EqualTo(GenderConstants.Male).Or.EqualTo(GenderConstants.Female));

            if (bison.Demographics.Gender == GenderConstants.Male)
            {
                Assert.That(bison.Demographics.Other, Contains.Substring("chin with a black beard-like growth"));
            }
            else if (bison.Demographics.Gender == GenderConstants.Female)
            {
                Assert.That(bison.Demographics.Other, Does.Not.Contain("chin with a black beard-like growth"));
            }
        }

        [TestCase(CreatureConstants.Human, true, false)]
        [TestCase(CreatureConstants.Snake_Constrictor, false, true)]
        [TestCase(CreatureConstants.Snake_Constrictor_Giant, false, true)]
        [TestCase(CreatureConstants.WinterWolf, true, true)]
        [TestCase(CreatureConstants.Wolf, true, true)]
        [TestCase(CreatureConstants.Wolf_Dire, true, true)]
        [TestCase(CreatureConstants.Worg, true, true)]
        [TestCase(CreatureConstants.YuanTi_Pureblood, true, false)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeHead, true, false)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeArms, true, false)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTail, true, true)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs, true, true)]
        [TestCase(CreatureConstants.YuanTi_Abomination, true, true)]
        public void Generate_SnakeHasLength(string creatureName, bool hasHeight, bool hasLength)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            creatureAsserter.AssertCreature(creature);

            Assert.That(creature.Demographics.Height, Is.Not.Null);
            Assert.That(creature.Demographics.Height.Unit, Is.EqualTo("inches"));
            Assert.That(creature.Demographics.Height.Description, Is.Not.Empty);
            Assert.That(creature.Demographics.Length, Is.Not.Null);
            Assert.That(creature.Demographics.Length.Unit, Is.EqualTo("inches"));
            Assert.That(creature.Demographics.Length.Description, Is.Not.Empty);

            if (hasHeight)
            {
                Assert.That(creature.Demographics.Height.Value, Is.Positive);
            }

            if (hasLength)
            {
                Assert.That(creature.Demographics.Length.Value, Is.Positive);
            }
        }

        //INFO: Too many problematic test cases for Repeat to be time-efficient
        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicCreaturesTestCases))]
        public void BUG_Generate_ProblematicCreature(bool asCharacter, string creatureName, params string[] templates)
        {
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer(templates);
            GenerateAndAssertCreature(creatureName, asCharacter, randomizer, templates);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicCreaturesTestCases))]
        public void BUG_Generate_ProblematicCreature_DefaultAbilities(bool asCharacter, string creatureName, params string[] templates)
        {
            GenerateAndAssertCreature(creatureName, asCharacter, null, templates);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicCreaturesTestCases))]
        public void BUG_Generate_ProblematicCreature_ProblematicAbilities(bool asCharacter, string creatureName, params string[] templates)
        {
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer(templates, [AbilityConstants.RandomizerRolls.Poor, AbilityConstants.RandomizerRolls.Wild]);
            GenerateAndAssertCreature(creatureName, asCharacter, randomizer, templates);
        }

        private Creature GenerateAndAssertCreature(string creatureName, bool asCharacter, AbilityRandomizer randomizer, params string[] templates)
        {
            stopwatch.Restart();
            var creature = creatureGenerator.Generate(asCharacter, creatureName, randomizer, templates);
            stopwatch.Stop();

            var timeLimit = CreatureAsserter.GetGenerationTimeLimitInSeconds(creature);
            Assert.That(stopwatch.Elapsed.TotalSeconds, Is.LessThan(timeLimit), creature.Summary);
            Assert.That(creature.Name, Is.EqualTo(creatureName), creature.Summary);
            Assert.That(creature.Templates, Is.EqualTo(templates.Where(t => t != CreatureConstants.Templates.None)), creature.Summary);

            if (asCharacter)
                creatureAsserter.AssertCreatureAsCharacter(creature);
            else
                creatureAsserter.AssertCreature(creature);

            return creature;
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicFiltersTestCases))]
        public void BUG_GenerateRandom_WithProblematicFilters(bool asCharacter, string[] templates, Filters filters)
        {
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer(templates);
            GenerateAndAssertRandomCreature(asCharacter, filters, randomizer, templates);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicFiltersTestCases))]
        public void BUG_GenerateRandom_WithProblematicFilters_DefaultAbilities(bool asCharacter, string[] templates, Filters filters)
        {
            GenerateAndAssertRandomCreature(asCharacter, filters, null, templates);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.ProblematicFiltersTestCases))]
        public void BUG_GenerateRandom_WithProblematicFilters_ProblematicAbilities(bool asCharacter, string[] templates, Filters filters)
        {
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer(templates, [AbilityConstants.RandomizerRolls.Poor, AbilityConstants.RandomizerRolls.Wild]);
            GenerateAndAssertRandomCreature(asCharacter, filters, randomizer, templates);
        }

        [Test]
        public void BUG_GenerateRandom_WithProblematicFilters_HalfDragonCelestial()
        {
            var templates = new[] { CreatureConstants.Templates.HalfDragon_Gold, CreatureConstants.Templates.CelestialCreature };
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer(templates);
            GenerateAndAssertRandomCreature(false, null, randomizer, templates);
        }

        [Test]
        public void BUG_Generate_WithProblematicFilters_DriderHalfDragonCelestial()
        {
            var templates = new[] { CreatureConstants.Templates.HalfDragon_Gold, CreatureConstants.Templates.CelestialCreature };
            var creature = creatureGenerator.Generate(false, CreatureConstants.Drider, null, templates);
            creatureAsserter.AssertCreature(creature);
        }

        [Test]
        [Repeat(100)]
        public void BUG_Generate_WithProblematicFilters_OtyughHalfCelestial()
        {
            var templates = new[] { CreatureConstants.Templates.HalfCelestial };
            var randomizer = new AbilityRandomizer(AbilityConstants.RandomizerRolls.Raw);
            var creature = creatureGenerator.Generate(false, CreatureConstants.Otyugh, randomizer, templates);
            creatureAsserter.AssertCreature(creature);
        }

        private Creature GenerateAndAssertRandomCreature(
            bool asCharacter,
            Filters filters,
            AbilityRandomizer randomizer,
            params string[] templates)
        {
            stopwatch.Restart();
            var creature = creatureGenerator.GenerateRandom(asCharacter, randomizer, filters, templates);
            stopwatch.Stop();

            var failure = new InvalidCreatureException(null, asCharacter, creature.Summary, filters, randomizer, templates);

            var timeLimit = CreatureAsserter.GetGenerationTimeLimitInSeconds(creature);
            Assert.That(stopwatch.Elapsed.TotalSeconds, Is.LessThan(timeLimit), failure.Message);

            if (templates.Length > 0)
                Assert.That(creature.Templates, Is.EqualTo(templates), failure.Message);

            //INFO: While we support multiple filters (acting as an OR), our current test cases only use 1 filter each.
            //if we ever add multiple filter values as abilityRandomizerFactory test case, these assertions should fail and should be updated to handle the OR correctly
            if (filters?.Types?.Count > 0)
                CreatureAsserter.AssertCreatureIsType(creature, filters.Types.Single(), failure.Message);

            if (filters?.ChallengeRatings?.Count > 0)
                Assert.That(creature.ChallengeRating, Is.EqualTo(filters.ChallengeRatings.Single()), failure.Message);

            if (filters?.Alignments?.Count > 0)
                Assert.That(creature.Alignment.Full, Is.EqualTo(filters.Alignments.Single()), failure.Message);

            if (asCharacter)
                creatureAsserter.AssertCreatureAsCharacter(creature, failure.Message);
            else
                creatureAsserter.AssertCreature(creature, asCharacter, failure.Message);

            return creature;
        }

        [Test]
        public void Generate_AdvancedCreatureHappens()
        {
            var iterations = 100;
            var advancedHappened = false;
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);

            while (iterations-- > 0 && !advancedHappened)
            {
                var creature = GenerateAndAssertCreature(CreatureConstants.Badger, false, randomizer);
                advancedHappened |= creature.IsAdvanced;
            }

            Assert.That(advancedHappened, Is.True);
        }

        [TestCase(CreatureConstants.Human, 0)]
        [TestCase(CreatureConstants.Lizardfolk, 2)]
        public void BUG_Generate_CreatureAsCharacterHasCorrectHitDice(string creatureName, double hitDiceQuantity)
        {
            var creature = creatureGenerator.Generate(true, creatureName);
            creatureAsserter.AssertCreatureAsCharacter(creature);
            Assert.That(creature.HitPoints.HitDiceQuantity, Is.EqualTo(hitDiceQuantity), creature.Summary);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void BUG_Generate_TemplateOnlyAddsAugmentedIfCreatureTypeChanged(string template)
        {
            var creatureNames = new[] { CreatureConstants.Human, CreatureConstants.Halfling_Lightfoot, CreatureConstants.Dog_Riding };

            foreach (var asCharacter in new[] { true, false })
            {
                foreach (var creatureName in creatureNames)
                {
                    var valid = creatureVerifier.VerifyCompatibility(asCharacter, creatureName);
                    valid &= creatureVerifier.VerifyCompatibility(asCharacter, creatureName, null, null, template);
                    if (!valid)
                        continue;

                    var creature = creatureGenerator.Generate(asCharacter, creatureName);
                    var templatedCreature = creatureGenerator.Generate(asCharacter, creatureName, null, template);

                    if (asCharacter)
                    {
                        creatureAsserter.AssertCreatureAsCharacter(creature);
                        creatureAsserter.AssertCreatureAsCharacter(templatedCreature);
                    }
                    else
                    {
                        creatureAsserter.AssertCreature(creature);
                        creatureAsserter.AssertCreature(templatedCreature);
                    }
                    Assert.That(creature.Templates, Is.Empty);

                    if (template == CreatureConstants.Templates.None)
                    {
                        Assert.That(templatedCreature.Templates, Is.Empty);
                    }
                    else
                    {
                        Assert.That(templatedCreature.Templates, Is.EqualTo([template]));
                    }

                    if (template == CreatureConstants.Templates.Skeleton || template == CreatureConstants.Templates.Zombie)
                    {
                        Assert.That(templatedCreature.Type.Name, Is.EqualTo(CreatureConstants.Types.Undead), templatedCreature.Summary);
                        Assert.That(templatedCreature.Type.SubTypes, Does.Not.Contain(creature.Type.Name), templatedCreature.Summary);
                        Assert.That(templatedCreature.Type.SubTypes, Does.Not.Contain(CreatureConstants.Types.Subtypes.Augmented), templatedCreature.Summary);
                    }
                    else if (creature.Type.Name != templatedCreature.Type.Name)
                    {
                        Assert.That(templatedCreature.Type.SubTypes, Is.SupersetOf([creature.Type.Name, CreatureConstants.Types.Subtypes.Augmented]), templatedCreature.Summary);
                    }
                    else
                    {
                        Assert.That(templatedCreature.Type.SubTypes, Does.Not.Contain(creature.Type.Name), templatedCreature.Summary);
                        Assert.That(templatedCreature.Type.SubTypes, Does.Not.Contain(CreatureConstants.Types.Subtypes.Augmented), templatedCreature.Summary);
                    }
                }
            }
        }
    }
}
