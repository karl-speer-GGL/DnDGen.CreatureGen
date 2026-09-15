using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.Appearances
{
    [TestFixture]
    internal abstract class AppearancesTests : CollectionTests
    {
        /// <summary>
        /// Keys are: CreatureKey (might include gender) -> Category (Skin, Hair, Eyes, Other) -> Rarity -> Appearances
        /// </summary>
        protected Dictionary<string, Dictionary<string, Dictionary<Rarity, IEnumerable<string>>>> creatureAppearances;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            creatureAppearances = GetCreatureAppearances();
        }

        private static Dictionary<string, Dictionary<Rarity, IEnumerable<string>>> InitializeAppearancesCollection()
        {
            return new Dictionary<string, Dictionary<Rarity, IEnumerable<string>>>
            {
                [TableNameConstants.Collection.AppearanceCategories.Skin] = new Dictionary<Rarity, IEnumerable<string>>
                {
                    [Rarity.Common] = [string.Empty],
                    [Rarity.Uncommon] = [],
                    [Rarity.Rare] = [],
                    [Rarity.VeryRare] = [],
                },
                [TableNameConstants.Collection.AppearanceCategories.Hair] = new Dictionary<Rarity, IEnumerable<string>>
                {
                    [Rarity.Common] = [string.Empty],
                    [Rarity.Uncommon] = [],
                    [Rarity.Rare] = [],
                    [Rarity.VeryRare] = [],
                },
                [TableNameConstants.Collection.AppearanceCategories.Eyes] = new Dictionary<Rarity, IEnumerable<string>>
                {
                    [Rarity.Common] = [string.Empty],
                    [Rarity.Uncommon] = [],
                    [Rarity.Rare] = [],
                    [Rarity.VeryRare] = [],
                },
                [TableNameConstants.Collection.AppearanceCategories.Other] = new Dictionary<Rarity, IEnumerable<string>>
                {
                    [Rarity.Common] = [string.Empty],
                    [Rarity.Uncommon] = [],
                    [Rarity.Rare] = [],
                    [Rarity.VeryRare] = [],
                },
            };
        }

        protected static IEnumerable<string> GetCollectionCreatureKeys()
        {
            return CreatureConstants.GetAll()
                .Union(CreatureConstants.Templates.GetAll())
                .Union(
                [
                    CreatureConstants.Bison + GenderConstants.Male,
                    CreatureConstants.Cockatrice + GenderConstants.Male,
                    CreatureConstants.Giant_Frost + GenderConstants.Male,
                    CreatureConstants.Githzerai + GenderConstants.Female,
                    CreatureConstants.Githzerai + GenderConstants.Male,
                    CreatureConstants.Gnome_Forest + GenderConstants.Male,
                    CreatureConstants.Gnome_Rock + GenderConstants.Male,
                    CreatureConstants.Gnome_Svirfneblin + GenderConstants.Female,
                    CreatureConstants.Gnome_Svirfneblin + GenderConstants.Male,
                    CreatureConstants.Halfling_Deep + GenderConstants.Female,
                    CreatureConstants.Halfling_Deep + GenderConstants.Male,
                    CreatureConstants.Halfling_Lightfoot + GenderConstants.Female,
                    CreatureConstants.Halfling_Lightfoot + GenderConstants.Male,
                    CreatureConstants.Halfling_Tallfellow + GenderConstants.Female,
                    CreatureConstants.Halfling_Tallfellow + GenderConstants.Male,
                    CreatureConstants.Hobgoblin + GenderConstants.Male,
                    CreatureConstants.Lion + GenderConstants.Male,
                    CreatureConstants.Lion_Dire + GenderConstants.Male,
                    CreatureConstants.Sahuagin + GenderConstants.Female,
                    CreatureConstants.Sahuagin_Mutant + GenderConstants.Female,
                    CreatureConstants.Troglodyte + GenderConstants.Male,
                    CreatureConstants.TrumpetArchon + GenderConstants.Female,
                    CreatureConstants.TrumpetArchon + GenderConstants.Male,
                    CreatureConstants.Unicorn + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Rat_Afflicted + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Rat_Natural + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted + GenderConstants.Male,
                    CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural + GenderConstants.Male,
                ]);
        }

        protected void WriteXml(string category)
        {
            var collections = new XElement("collections");
            var rarities = new[] { Rarity.Common, Rarity.Uncommon, Rarity.Rare, Rarity.VeryRare };

            foreach (var creature in CreatureConstants.GetAll())
            {
                var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, creature);
                var creatureKeys = GetCollectionCreatureKeys();
                var keys = genders
                    .Select(g => creature + g)
                    .Concat([creature])
                    .Intersect(creatureKeys);

                foreach (var key in keys)
                {
                    foreach (var rarity in rarities)
                    {
                        var collection = new XElement("collection", new XElement("name", key + rarity.ToString()));

                        foreach (var appearance in creatureAppearances[key][category][rarity])
                        {
                            collection.Add(new XElement("entry", appearance));
                        }

                        collections.Add(collection);
                    }
                }
            }

            foreach (var template in CreatureConstants.Templates.GetAll())
            {
                var genders = collectionSelector.SelectFrom(Config.Name, TableNameConstants.Collection.Genders, CreatureConstants.Human);
                var templateKeys = GetCollectionCreatureKeys();
                var keys = genders
                    .Select(g => template + g)
                    .Concat([template])
                    .Intersect(templateKeys);

                foreach (var key in keys)
                {
                    foreach (var rarity in rarities)
                    {
                        var collection = new XElement("collection", new XElement("name", key + rarity.ToString()));

                        foreach (var appearance in creatureAppearances[key][category][rarity])
                        {
                            collection.Add(new XElement("entry", appearance));
                        }

                        collections.Add(collection);
                    }
                }
            }

            var path = Path.Combine(Environment.CurrentDirectory, $"{TableName}.xml");
            collections.Save(path);
        }

        protected void AssertNoAppearancesIncludeTODO(string category)
        {
            var creatureKeys = GetCollectionCreatureKeys();
            foreach (var name in creatureKeys)
            {
                Assert.That(creatureAppearances, Contains.Key(name));
                Assert.That(creatureAppearances[name], Contains.Key(category));
                Assert.That(creatureAppearances[name][category], Is.Not.Empty
                    .And.ContainKey(Rarity.Common)
                    .And.ContainKey(Rarity.Uncommon)
                    .And.ContainKey(Rarity.Rare)
                    .And.ContainKey(Rarity.VeryRare)
                    .And.Count.EqualTo(4));

                Assert.That(creatureAppearances[name][category][Rarity.Common].Where(a => a.Contains("TODO")), Is.Empty, $"{name}: COMMON APPEARANCES TODO");
                Assert.That(creatureAppearances[name][category][Rarity.Uncommon].Where(a => a.Contains("TODO")), Is.Empty, $"{name}: UNCOMMON APPEARANCES TODO");
                Assert.That(creatureAppearances[name][category][Rarity.Rare].Where(a => a.Contains("TODO")), Is.Empty, $"{name}: RARE APPEARANCES TODO");
                Assert.That(creatureAppearances[name][category][Rarity.VeryRare].Where(a => a.Contains("TODO")), Is.Empty, $"{name}: VERY RARE APPEARANCES TODO");
            }
        }

        protected void AssertCreatureAppearance(string category, string key)
        {
            Assert.That(creatureAppearances, Contains.Key(key));
            Assert.That(creatureAppearances[key], Contains.Key(category));
            Assert.That(creatureAppearances[key][category], Is.Not.Empty
                .And.ContainKey(Rarity.Common)
                .And.ContainKey(Rarity.Uncommon)
                .And.ContainKey(Rarity.Rare)
                .And.ContainKey(Rarity.VeryRare)
                .And.Count.EqualTo(4));

            var allValues = creatureAppearances[key][category].SelectMany(kvp => kvp.Value);
            Assert.That(allValues, Is.Not.Empty);
            Assert.That(creatureAppearances[key][category][Rarity.Common], Is.Not.Empty);

            if (allValues.Any(a => a.Contains("TODO")))
            {
                Assert.Warn($"{key} appearance contains a TODO");
                return;
            }

            AssertAppearanceCollection(key, category, Rarity.Common);
            AssertAppearanceCollection(key, category, Rarity.Uncommon);
            AssertAppearanceCollection(key, category, Rarity.Rare);
            AssertAppearanceCollection(key, category, Rarity.VeryRare);
        }

        private void AssertAppearanceCollection(string key, string category, Rarity rarity)
            => AssertCollection(key + rarity.ToString(), [.. creatureAppearances[key][category][rarity]]);

        private Dictionary<string, Dictionary<string, Dictionary<Rarity, IEnumerable<string>>>> GetCreatureAppearances()
        {
            var creatures = GetCollectionCreatureKeys();
            var appearances = new Dictionary<string, Dictionary<string, Dictionary<Rarity, IEnumerable<string>>>>();

            foreach (var creature in creatures)
            {
                appearances[creature] = InitializeAppearancesCollection();
            }

            //Reusable
            //Source: https://forgottenrealms.fandom.com/wiki/Human
            appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Dark brown skin", "Brown skin", "Light brown skin",
                "Dark tan skin", "Tan skin", "Light tan skin",
                "Pink skin", "White skin", "Pale white skin"
            ];
            appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = [
                "Straight red hair", "Straight blond hair", "Straight brown hair", "Straight black hair",
                "Curly red hair", "Curly blond hair", "Curly brown hair", "Curly black hair",
                "Kinky red hair", "Kinky blond hair", "Kinky brown hair", "Kinky black hair"
            ];
            appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = [
                "Blue eyes", "Brown eyes", "Gray eyes", "Green eyes", "Hazel eyes"];

            //Source: https://www.google.com/search?q=draft+horse+breeds
            //https://equineworld.co.uk/about-horses/horse-colours-and-markings/horse-coat-colours-and-patterns
            appearances[CreatureConstants.Horse_Heavy][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Belgian Draught", "Shire horse", "Clydesdale horse", "Percheron", "Suffolk Punch",
                "American Cream Draft", "Ardennais", "Haflinger", "Irish Draught", "Dutch Draft", "Friesian horse", "Fjord horse", "Russian Heavy Draft",
                "Boulonnais horse", "Galineers Cob", "Australian Draught", "North Swedish Horse", "Noriker", "American Belgian Draft", "Breton horse", "Jutland",
                "Comtois horse", "Vladimir Heavy Draft", "Rhenish German Coldblood", "Auxois", "Cleveland Bay", "Lithuanian heavy draft",
                "North American Spotted Draft Horse", "Bashkir horse", "Soviet Heavy Draft", "Italian Heavy Draft", "South German Coldblood", "Hackney horse",
                "Polish Draft", "Dole Gudbrandsdal", "Spotted Draft", "Groninger", "Orlov Trotter", "Gelderlander", "Kabardian", "Karabair", "Kazakh horse",
                "Camargue horse", "Losino horse", "American Quarter Horse", "Zemaitukas", "Vyatka horse", "Estonian Native", "Kisber Felver", "Shagya Arabian",
                "Byelorussian Harness Horse",
                "Spotted (White coat with black spots; black/white mane and tail)", "Spotted (White coat with brown spots; brown/white mane and tail)",
                    "Spotted (White coat with gray spots; Gray/white mane and tail)", "Spotted (White coat with ginger spots; Ginger/white mane and tail)",
                    "Spotted (White coat with golden spots; Golden/white mane and tail)", "Spotted (White coat with cream spots; Cream/white mane and tail)",
                    "Spotted (White coat with light gray spots; Light gray/white mane and tail)",
                    "Spotted (White coat with dark gray spots; Dark gray/white mane and tail)",
                    "Spotted (White coat with creamy golden spots; Creamy golden/white mane and tail)",
                    "Spotted (White coat with light ginger spots; Light ginger/white mane and tail)",
                    "Spotted (White coat with dark ginger spots; Dark ginger/white mane and tail)",
                    "Spotted (Black coat with white spots; White/black mane and tail)",
                    "Spotted (Black coat with brown spots; Brown/black mane and tail)",
                    "Spotted (Black coat with gray spots; Gray/black mane and tail)",
                    "Spotted (Black coat with ginger spots; Ginger/black mane and tail)",
                    "Spotted (Black coat with golden spots; Golden/black mane and tail)",
                    "Spotted (Black coat with cream spots; Cream/black mane and tail)",
                    "Spotted (Black coat with light gray spots; Light gray/black mane and tail)",
                    "Spotted (Black coat with dark gray spots; Dark gray/black mane and tail)",
                    "Spotted (Black coat with creamy golden spots; Creamy golden/black mane and tail)",
                    "Spotted (Black coat with light ginger spots; Light ginger/black mane and tail)",
                    "Spotted (Black coat with dark ginger spots; Dark ginger/black mane and tail)",
                    "Spotted (Brown coat with white spots; White/brown mane and tail)",
                    "Spotted (Brown coat with black spots; Black/brown mane and tail)",
                    "Spotted (Brown coat with gray spots; Gray/brown mane and tail)",
                    "Spotted (Brown coat with ginger spots; Ginger/brown mane and tail)",
                    "Spotted (Brown coat with golden spots; Golden/brown mane and tail)",
                    "Spotted (Brown coat with cream spots; Cream/brown mane and tail)",
                    "Spotted (Brown coat with light gray spots; Light gray/brown mane and tail)",
                    "Spotted (Brown coat with dark gray spots; Dark gray/brown mane and tail)",
                    "Spotted (Brown coat with creamy golden spots; Creamy golden/brown mane and tail)",
                    "Spotted (Brown coat with light ginger spots; Light ginger/brown mane and tail)",
                    "Spotted (Brown coat with dark ginger spots; Dark ginger/brown mane and tail)",
                "Bay (Brown coat; Black legs; Black mane and tail)", "Solid black coat; Black mane and tail",
                "Brown coat; Black lower legs; Light brown mane, tail, and muzzle",
                "Buckskin (Creamy-golden coat with black points; Black mane and tail)", "Buckskin (Rich golden coat with black points; Black mane and tail)",
                "Chestnut (Ginger coat; Ginger mane and tail)", "Chestnut (Ginger coat; Light ginger mane and tail)",
                    "Chestnut (Ginger coat; Dark ginger mane and tail)",
                "Chestnut (Light ginger coat; Ginger mane and tail)", "Chestnut (Light ginger coat; Light ginger mane and tail)",
                    "Chestnut (Light ginger coat; Dark ginger mane and tail)",
                "Chestnut (Dark ginger coat; Ginger mane and tail)", "Chestnut (Dark ginger coat; Light ginger mane and tail)",
                    "Chestnut (Dark ginger coat; Dark ginger mane and tail)",
                "Cremello (Extremely pale white coat, mane, and tail; Blue eyes)", "Cremello (Extremely pale white coat, mane, and tail; Amber eyes)",
                    "Cremello (Extremely pale cream coat, mane, and tail; Blue eyes)", "Cremello (Extremely pale cream coat, mane, and tail; Amber eyes)",
                "Dun (Creamy-golden coat; Black mane and tail; Dark dorsal strip)",
                "Solid white-gray coat, mane, and tail", "Solid light gray coat, mane, and tail", "Solid gray coat, mane, and tail",
                    "Solid dark gray coat, mane, and tail",
                "Overo (Black/white cow-patterned coat; Black mane and tail)", "Overo (Black/brown cow-patterned coat; Black mane and tail)",
                    "Overo (Black/gray cow-patterned coat; Black mane and tail)", "Overo (Black/light gray cow-patterned coat; Black mane and tail)",
                    "Overo (Black/dark gray cow-patterned coat; Black mane and tail)", "Overo (Black/creamy golden cow-patterned coat; Black mane and tail)",
                    "Overo (Black/rich golden cow-patterned coat; Black mane and tail)", "Overo (Black/ginger cow-patterned coat; Black mane and tail)",
                    "Overo (Black/light ginger cow-patterned coat; Black mane and tail)", "Overo (Black/dark ginger cow-patterned coat; Black mane and tail)",
                    "Overo (Black/golden cow-patterned coat; Black mane and tail)",
                "Palomino (Golden coat; White mane and tail)",
                "Roan (White-brown coat; Brown mane, tail, and legs)", "Roan (White-black coat; Black mane, tail, and legs)",
                    "Roan (White-golden coat; Golden mane, tail, and legs)", "Roan (White-ginger coat; Ginger mane, tail, and legs)",
                    "Roan (White-light-ginger coat; Light ginger mane, tail, and legs)", "Roan (White-dark-ginger coat; Dark ginger mane, tail, and legs)",
                    "Roan (White-creamy-golden coat; Creamy golden mane, tail, and legs)", "Roan (White-rich-golden coat; Rich golden mane, tail, and legs)",
                    "Roan (White-gray coat; Creamy golden mane, tail, and legs)", "Roan (White-light-gray coat; Rich golden mane, tail, and legs)",
                    "Roan (White-dark-gray coat; Creamy golden mane, tail, and legs)",
                "Tobiano (White/black cow-patterned coat; White mane and tail)", "Tobiano (White/brown cow-patterned coat; White mane and tail)",
                    "Tobiano (White/gray cow-patterned coat; White mane and tail)", "Tobiano (White/light gray cow-patterned coat; White mane and tail)",
                    "Tobiano (White/dark gray cow-patterned coat; White mane and tail)", "Tobiano (White/creamy golden cow-patterned coat; White mane and tail)",
                    "Tobiano (White/rich golden cow-patterned coat; White mane and tail)", "Tobiano (White/ginger cow-patterned coat; White mane and tail)",
                    "Tobiano (White/light ginger cow-patterned coat; White mane and tail)", "Tobiano (White/dark ginger cow-patterned coat; White mane and tail)",
                    "Tobiano (White/golden cow-patterned coat; White mane and tail)"
            ];

            //Source: https://forgottenrealms.fandom.com/wiki/High_elf
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Brown skin", "Tan skin", "White skin", "Pink skin",
                "Light brown skin", "Light tan skin", "Pale white skin"
            ];
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon] = [
                "Dark brown skin", "Dark tan skin"
            ];
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = [
                "Straight black hair", "Curly black hair", "Kinky black hair",
                "Straight white hair", "Curly white hair", "Kinky white hair",
                "Straight silver hair", "Curly silver hair", "Kinky silver hair",
                "Straight pale gold hair", "Curly pale gold hair", "Kinky pale gold hair"
            ];
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] = Combine(" ",
                appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                ["with silvery hues", "with blond hues", "with copper hues"]);
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Green eyes"];
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] =
                new[] { "Golden eyes", "Blue eyes", "Light blue eyes" }
                .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Select(e => $"{e} speckled with gold"));
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare] = new[]
            {
                "Violet eyes",
            }
                .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Where(e => !e.Contains("speckled") && !e.Contains("Gold"))
                    .Select(e => $"{e} speckled with gold"))
                .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Select(LowercaseFirstCharacter)
                    .Select(e => $"Solid {e}, lacking pupils"));
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare] = new[]
            {
                "Violet eyes speckled with gold",
                "Solid violet eyes, lacking pupils",
                "Solid violet eyes speckled with gold, lacking pupils",
            }
                .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Select(LowercaseFirstCharacter)
                    .Select(e => $"Solid {e}, lacking pupils"))
                .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Select(LowercaseFirstCharacter)
                    .Where(e => !e.Contains("speckled") && !e.Contains("Gold"))
                    .Select(e => $"Solid {e} speckled with gold, lacking pupils"));
            appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Pointed ears"];

            //Source: https://www.d20srd.org/srd/monsters/elf.htm
            //https://forgottenrealms.fandom.com/wiki/Grey_elf
            appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Pale grey skin"];
            appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                ["Straight silver hair", "Curly silver hair", "Kinky silver hair"];
            appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] =
                ["Straight pale-golden hair", "Curly pale-golden hair", "Kinky pale-golden hair"];
            appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Amber eyes"];
            appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] = ["Violet eyes"];
            appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Other] =
                appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Other];

            //Source: https://forgottenrealms.fandom.com/wiki/Drow
            //https://www.d20srd.org/srd/monsters/elf.htm
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Black skin", "Dark blue skin", "Gray skin", "Dark gray skin", "Jet-black skin", "Obsidian-colored skin", "Blue skin",
                "Gray-blue skin", "Black-blue skin" ];
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare] = ["White (albino) skin"];
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = [
                "Straight stark white hair", "Curly stark white hair", "Kinky stark white hair",
                "Straight pale yellow hair", "Curly pale yellow hair", "Kinky pale yellow hair" ];
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] = new[] {
                    "Straight gray hair", "Curly gray hair", "Kinky gray hair",
                    "Straight pale yellow hair", "Curly pale yellow hair", "Kinky pale yellow hair",
                    "Straight silver hair", "Curly silver hair", "Kinky silver hair",
                    "Straight red hair", "Straight blond hair", "Straight brown hair", "Straight black hair", }
                .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common])
                .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]);
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]);
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]);
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Bright red eyes", "Vivid red eyes"];
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] = [
                "Pale white-blue eyes", "Pale white-lilac eyes", "Pale white-pink eyes", "Pale white-silver eyes", "Purple eyes", "Blue eyes"];
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare] = [
                "Green eyes", "Brown eyes", "Black eyes", "Amber eyes", "Rose-hued eyes"];
            appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Other] =
                appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Other];

            //Source: https://forgottenrealms.fandom.com/wiki/Wild_elf
            //https://www.d20srd.org/srd/monsters/elf.htm
            appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] =
                ["Light brown skin", "Brown skin", "Dark brown skin"];
            appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = [
                "Straight black hair", "Curly black hair", "Kinky black hair",
                "Straight dark brown hair", "Curly dark brown hair", "Kinky dark brown hair",
                "Straight brown hair", "Curly brown hair", "Kinky brown hair",
                "Straight light brown hair", "Curly light brown hair", "Kinky light brown hair"
            ];
            appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] = [
                "Straight silver hair", "Curly silver hair", "Kinky silver hair",
                "Straight silvery-white hair", "Curly silvery-white hair", "Kinky silvery-white hair",
                "Straight gray hair", "Curly gray hair", "Kinky gray hair",
                "Straight white hair", "Curly white hair", "Kinky white hair"];
            appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Eyes] =
                appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes];
            appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Other] =
                appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Other];

            //Source: https://forgottenrealms.fandom.com/wiki/Wood_elf
            //https://www.d20srd.org/srd/monsters/elf.htm
            appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Copper skin", "Tan skin"];
            appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = [
                "Straight black hair", "Straight light brown hair", "Straight brown hair",
                "Curly black hair", "Curly light brown hair", "Curly brown hair",
                "Kinky black hair", "Kinky light brown hair", "Kinky brown hair"
            ];
            appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] = [
                "Straight yellow hair", "Straight blond hair", "Straight copper-red hair",
                "Curly yellow hair", "Curly blond hair", "Curly copper-red hair",
                "Kinky yellow hair", "Kinky blond hair", "Kinky copper-red hair"];
            appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Green eyes", "Brown eyes", "Hazel eyes"];
            appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Other] = appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Other];

            //Source: https://www.d20srd.org/srd/monsters/elf.htm
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]);
            appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Other] =
                appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Other];

            //Source: https://www.d20srd.org/srd/monsters/dwarf.htm
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Tan skin", "Brown skin", "Dark tan skin", "Dark brown skin", "Very dark tan skin", "Very dark brown skin"];
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon] = [
                "Tan skin with a reddish tinge", "Brown skin with a reddish tinge", "Dark tan skin with a reddish tinge",
                "Dark brown skin with a reddish tinge", "Very dark tan skin with a reddish tinge", "Very dark brown skin with a reddish tinge"];
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = [
                "Bright red hair", "Straw blond hair", "Muted red hair", "Yellow hair", "Bright yellow hair", "Muted yellow hair", "Brown hair"
            ];
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Washed-out blue eyes"];
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Select(LowercaseFirstCharacter)
                    .Select(e => $"Washed-out {e}");
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                        .Select(LowercaseFirstCharacter)
                        .Select(e => $"Washed-out {e}");
            appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]
                        .Select(LowercaseFirstCharacter)
                        .Select(e => $"Washed-out {e}")
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                        .Select(LowercaseFirstCharacter)
                        .Select(e => $"Washed-out {e}"));

            appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Tan skin", "Brown skin", "Dark tan skin", "Dark brown skin", "Very dark tan skin", "Very dark brown skin"];
            appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Bald"];
            appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] = ["Black hair", "Gray hair", "Brown hair"];
            appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Eyes] = appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes];

            appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Tan skin", "Brown skin", "Dark tan skin", "Dark brown skin", "Very dark tan skin", "Very dark brown skin"];
            appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Black hair", "Gray hair", "Brown hair"];
            appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Eyes] = appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes];

            appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Lightly tan skin", "Light brown skin", "Tan skin", "Brown skin", "Dark tan skin", "Dark brown skin"];
            appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Dark gray hair", "Light gray hair", "Light brown hair"];
            appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Eyes] = appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes];

            //Source: http://www.wlgf.org/hunting_spiders.html
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Common house spider", "Brown recluse spider", "Southern black widow",
                "Hobo spider", "Forked pirate spider", "Black house spider", "Western black widow", "Black-footed yellow sac spider",
                "Brown widow", "Woodlouse spider", "Redback spider", "False widow", "Giant house spider", "Rabid wolf spider", "Goliath birdeater", "Noble false widow",
                "Bold jumper"
            ];

            appearances[CreatureConstants.Dog][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Bulldog", "French Bulldog", "Beagle", "Standard Poodle", "Chihuahua", "Dachsund", "Bichon Frise",
                "Maltese",
                "Chow Chow", "English Cocker Spaniel" , "Pomeranian", "Yorkshire Terrier", "Cavalier King Charles Spaniel", "Pembroke Welsh Corgi", "Basenji", "Havanese",
                "Boston Terrier", "Cairn Terrier", "Brittany", "Sheltie", "Shiba Inu", "Jack Russell Terrier", "Borzoi", "Maltipoo", "Papillon", "Pikanese",
                "Miniature Poodle", "Pikapoo", "Coyote", "Dingo", "Miniature Schnauzer", "Mutt", "English Bulldog", "Cavapoo", "Red Kelpie", "American Foxhound",
                "Brussels Griffon", "Aussie Terrier", "German Spitz", "Pug", "Teacup Pomeranian", "Saluki", "Schnauzer Terrier", "Presa Canario"
            ];
            appearances[CreatureConstants.Dog][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] =
                ["Brown eyes", "Hazel eyes", "Amber eyes", "Yellow eyes", "Blue eyes", "Green eyes"];

            appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] =
                ["Yellow skin", "Red skin", "Orange skin", "Light orange skin", "Dark orange skin", "Deep red skin"];
            appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon] = ["Green skin"];
            appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] =
                ["Dull, glazed, yellow eyes", "Dull, glazed, orange eyes", "Dull, glazed, red eyes"];
            appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Flat face, small fangs, pointed ears, sloped-back forehead, broad noses"];

            //Source: https://forgottenrealms.fandom.com/wiki/Hill_giant
            appearances[CreatureConstants.Giant_Hill][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] =
                ["Tan skin", "Reddish-brown skin", "Brown skin", "Dark tan skin", "Deep ruddy-brown skin", "Light tan skin"];
            appearances[CreatureConstants.Giant_Hill][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                ["Brown hair", "Dark brown hair", "Black hair"];
            appearances[CreatureConstants.Giant_Hill][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] =
                ["Black, red-rimmed eyes", "Dark brown, red-rimmed eyes", "Brown, red-rimmed eyes"];
            appearances[CreatureConstants.Giant_Hill][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Long arms, stooped shoulders, low forehead"];

            //Source: https://forgottenrealms.fandom.com/wiki/Stone_giant
            appearances[CreatureConstants.Giant_Stone][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] =
                ["Gray skin", "Grayish-brown skin"];
            appearances[CreatureConstants.Giant_Stone][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                ["Dark gray hair", "Gray hair", "Bluish-gray hair", "Bald"];
            appearances[CreatureConstants.Giant_Stone][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] =
                ["Silver eyes", "Steel-colored eyes"];

            //Source: https://forgottenrealms.fandom.com/wiki/Orc
            appearances[CreatureConstants.Orc][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Gray skin"];
            appearances[CreatureConstants.Orc][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Coarse black hair"];
            appearances[CreatureConstants.Orc][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Reddish eyes"];
            appearances[CreatureConstants.Orc][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Low forehead, tusks"];

            //Source: https://forgottenrealms.fandom.com/wiki/Lion
            //https://www.lionlandscapes.org/lion-vision
            appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Golden fur", "Tawny fur"];
            appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Large amber eyes"];

            //Source: https://www.google.com/search?q=species+of+eagle
            appearances[CreatureConstants.Eagle][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Golden eagle", "Bald eagle", "Harpy eagle", "White-tailed eagle", "Black kite", "Steller's sea eagle",
                "Philippine eagle", "Haast's eagle", "Wedge-tailed eagle", "Common buzzard", "Black eagle", "Crowned eagle", "Javan hawk-eagle", "Eastern imperial eagle",
                "Indian spotted eagle", "African fish eagle", "Spanish imperial eagle", "Bonelli's eagle", "Bearded vulture", "Black-and-chestnut eagle", "Martial eagle",
                "Black-chested buzzard-eagle", "Eurasian griffon vulture", "Red kite", "Verreaux's eagle", "Crested serpent eagle", "Steppe eagle", "Cinereous vulture",
                "White-bellied sea eagle", "Booted eagle", "Short-toed snake eagle", "Tawny eagle", "Egyptian vulture", "Lesser spotted eagle", "Mountain hawk-eagle",
                "Greater spotted eagle", "Little eagle", "Brahminy kite", "Ornate hawk-eagle", "Black-and-white hawk-eagle", "Changeable hawk-eagle", "Crested eagle",
                "Rough-legged buzzard", "Hen harrier"];
            appearances[CreatureConstants.Eagle][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Pale yellow eyes"];

            //Source: https://forgottenrealms.fandom.com/wiki/Lightfoot_halfling
            //https://www.d20srd.org/srd/monsters/halfling.htm
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Ruddy skin"];
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common];
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Straight black hair", "Straight brown hair"];
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Straight black hair with short sideburns", "Straight brown hair with short sideburns"]);
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(h => $"{h} with short sideburns");
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Brown eyes", "Black eyes", "Hazel eyes"];
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common];

            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin] =
                appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Skin];
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                ["Straight black hair", "Straight black hair with long sideburns", "Straight brown hair", "Straight brown hair with long sideburns"];
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] =
                appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Straight black hair with beard", "Straight brown hair with beard"]);
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare] =
                Combine(" ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                    ["with long sideburns", "with beard", "with mustache"])
                .Concat(["Straight black hair with mustache", "Straight brown hair with mustache"]);
            appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes] =
                appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female][TableNameConstants.Collection.AppearanceCategories.Eyes];

            //Source: https://www.dimensions.com/element/osprey-pandion-haliaetus
            appearances[CreatureConstants.Hawk][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [ "Red-tailed hawk", "Eurasian sparrowhawk", "Sharp-shinned hawk", "Osprey",
                "Harris' hawk", "Eurasian goshawk",
                "Black kite", "Red-shouldered hawk", "Cooper's hawk", "Broad-winged hawk", "Ferruginous hawk", "Swainson's hawk", "Tiny hawk", "White-tailed hawk",
                "Northern harrier", "Roadside hawk", "Great black hawk", "Common black hawk", "Crested goshawk", "Red kite", "Shikra", "Collared sparrowhawk",
                "Japanese sparrowhawk", "Chinese sparrowhawk", "Hen harrier", "Black-winged kite", "White-tailed kite", "Zone-tailed hawk", "Brahminy kite", "Besra",
                "Western marsh harrier", "Red-backed hawk", "Montagu's harrier", "Swamp harrier"
            ];

            appearances[CreatureConstants.VioletFungus][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Deep purple coloration", "Dull gray coloration", "Violet coloration and covered in purple spots"
            ];
            appearances[CreatureConstants.VioletFungus][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [
                "Mass of root-like feelers, four leafy tendrils"
            ];

            //Non-reusable

            //Source: https://forgottenrealms.fandom.com/wiki/Aasimar
            appearances[CreatureConstants.Aasimar] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s} with small iridescent scales")
                    .Concat(["Emerald skin", "Gold skin", "Silver skin"]),
                rareSkin: ["Emerald skin with small iridescent scales", "Gold skin with small iridescent scales", "Silver skin with small iridescent scales"],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Straight silver hair", "Curly silver hair", "Kinky silver hair"]),
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(h => $"{h} with feathers mized in"))
                    .Concat(["Straight silver hair with feathers mixed in", "Curly silver hair with feathers mixed in", "Kinky silver hair with feathers mixed in"]),
                commonEyes: ["Pupil-less pale white eyes", "Pupil-less golden eyes", "Pupil-less gray eyes"],
                uncommonEyes: ["Pupil-less topaz eyes", "Pupil-less pearly opalescent eyes"],
                uncommonOther: ["A light covering of feathers on the shoulders, where an angel's wings might sprout"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Aboleth
            appearances[CreatureConstants.Aboleth] = GetAppearances(
                commonEyes: new[] { "Three red eyes" },
                commonSkin: new[] { "Orange-pink underbelly, sea-green skin topside" },
                uncommonSkin: new[] { "Orange underbelly, sea-green skin topside", "Pink underbelly, sea-green skin topside",
                    "Orange-pink underbelly, green skin topside", "Orange-pink underbelly, blue skin topside" },
                rareSkin: new[] { "Orange underbelly, green skin topside", "Pink underbelly, green skin topside",
                    "Orange underbelly, blue skin topside", "Pink underbelly, blue skin topside" },
                commonOther: new[] { "Resembles a bizarre eel" });
            //Source: https://forgottenrealms.fandom.com/wiki/Achaierai
            appearances[CreatureConstants.Achaierai] = GetAppearances(
                commonSkin: new[] { "Shining metallic blue-gray legs, claws, and beak" },
                commonHair: new[] { "Bright red, soft feathers on its crest, brown soft feathers on its body" },
                rareHair: new[] { "Dim teal, soft feathers on its crest, brown soft feathers on its body",
                    "Shadowed gold, soft feathers on its crest, brown soft feathers on its body",
                    "Burnt russet, soft feathers on its crest, brown soft feathers on its body" },
                commonOther: new[] { "Resembles a quadruped, plump quail" });
            //Source: https://forgottenrealms.fandom.com/wiki/Allip
            appearances[CreatureConstants.Allip][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Spectral variations of the person it once was, with nightmarish, warped features befitting of the madness that possessed it. Its lower portions trail away into a dark fog."];
            //Source: https://forgottenrealms.fandom.com/wiki/Androsphinx
            appearances[CreatureConstants.Androsphinx][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Tawny hair/fur. Lion body, falcon wings."];
            //Source: https://forgottenrealms.fandom.com/wiki/Astral_deva
            appearances[CreatureConstants.Angel_AstralDeva] = GetAppearances(
                commonSkin: new[] { "Golden skin" },
                commonHair: new[] { "Fair hair", "Golden hair" },
                commonEyes: new[] { "Amber eyes" },
                commonOther: new[] { "White wings tinted with gold" });
            //Source: https://forgottenrealms.fandom.com/wiki/Planetar
            appearances[CreatureConstants.Angel_Planetar] = GetAppearances(
                commonSkin: new[] { "Opalescent emerald skin" },
                uncommonSkin: new[] { "Pearly white skin" },
                commonHair: new[] { "Bald; Opalescent white feathers tinted with gold on wings", "Bald; White feathers tinted with gold on wings" },
                uncommonHair: new[] { "Long, flowing blue hair; Opalescent white feathers tinted with gold on wings",
                    "Long, flowing blue hair; White feathers tinted with gold on wings" },
                commonEyes: new[] { "Glowing blue eyes" });
            //Source: https://forgottenrealms.fandom.com/wiki/Solar
            appearances[CreatureConstants.Angel_Solar] = GetAppearances(
                commonSkin: new[] { "Copper skin", "Silver skin", "Golden skin" },
                uncommonSkin: new[] { "Bronze skin", "Brass skin" },
                commonHair: new[] { "Bronze hair; White-feathered wings", "Bronze hair; Coppery-golden-feathered wings" },
                uncommonHair: new[] { "Copper hair; White-feathered wings", "Copper hair; Coppery-golden-feathered wings",
                    "Silver hair; White-feathered wings", "Silver hair; Coppery-golden-feathered wings",
                    "Golden hair; White-feathered wings", "Golden hair; Coppery-golden-feathered wings",
                    "Brass hair; White-feathered wings", "Brass hair; Coppery-golden-feathered wings" },
                commonEyes: new[] { "Radiant topaz eyes" });
            //Source: https://www.d20srd.org/srd/monsters/animatedObject.htm
            //https://forgottenrealms.fandom.com/wiki/Animated_object
            appearances[CreatureConstants.AnimatedObject_Colossal][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub"];
            appearances[CreatureConstants.AnimatedObject_Colossal_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Rope", "Vine", "Chain"];
            appearances[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Table", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Table", "Wooden Dresser", "Wooden Ottoman",];
            appearances[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Chair", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Chair", "Wooden Dresser", "Wooden Stool"];
            appearances[CreatureConstants.AnimatedObject_Colossal_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Carpet", "Tapestry", "Rug", "Blanket"];
            appearances[CreatureConstants.AnimatedObject_Colossal_TwoLegs] = GetAppearances(
                commonOther: [ "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" ],
                uncommonOther: [ "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" ],
                rareOther: [ "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" ]);
            appearances[CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Cart", "Carriage"];
            appearances[CreatureConstants.AnimatedObject_Colossal_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Clock", "Feather Duster", "Broom", "Bucket", "Barrel"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Rope", "Vine", "Chain"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Table", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Table", "Wooden Dresser", "Wooden Ottoman"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Chair", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Chair", "Wooden Dresser", "Wooden Stool"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Carpet", "Tapestry", "Rug", "Blanket"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs] = GetAppearances(
                commonOther: [ "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" ],
                uncommonOther: [ "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" ],
                rareOther: [ "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" ]);
            appearances[CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Cart", "Carriage"];
            appearances[CreatureConstants.AnimatedObject_Gargantuan_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Clock", "Feather Duster", "Broom", "Bucket", "Barrel"];
            appearances[CreatureConstants.AnimatedObject_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub"];
            appearances[CreatureConstants.AnimatedObject_Huge_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Rope", "Vine", "Chain"];
            appearances[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Table", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Table", "Wooden Dresser", "Wooden Ottoman"];
            appearances[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Chair", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Chair", "Wooden Dresser", "Wooden Stool"];
            appearances[CreatureConstants.AnimatedObject_Huge_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Carpet", "Tapestry", "Rug", "Blanket"];
            appearances[CreatureConstants.AnimatedObject_Huge_TwoLegs] = GetAppearances(
                commonOther: new[] { "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" },
                uncommonOther: new[] { "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" },
                rareOther: new[] { "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" });
            appearances[CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Huge_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Cart", "Carriage"];
            appearances[CreatureConstants.AnimatedObject_Huge_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Clock", "Feather Duster", "Broom", "Bucket", "Barrel"];
            appearances[CreatureConstants.AnimatedObject_Large][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub"];
            appearances[CreatureConstants.AnimatedObject_Large_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Rope", "Vine", "Chain"];
            appearances[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Table", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Table", "Wooden Dresser", "Wooden Ottoman"];
            appearances[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Chair", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Wooden Chair", "Wooden Dresser", "Wooden Stool"];
            appearances[CreatureConstants.AnimatedObject_Large_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Carpet", "Tapestry", "Rug", "Blanket"];
            appearances[CreatureConstants.AnimatedObject_Large_TwoLegs] = GetAppearances(
                commonOther: new[] { "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" },
                uncommonOther: new[] { "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" },
                rareOther: new[] { "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" });
            appearances[CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Large_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Cart", "Carriage" };
            appearances[CreatureConstants.AnimatedObject_Large_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Clock", "Feather Duster", "Broom", "Bucket", "Barrel" };
            appearances[CreatureConstants.AnimatedObject_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub" };
            appearances[CreatureConstants.AnimatedObject_Medium_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Rope", "Vine", "Chain" };
            appearances[CreatureConstants.AnimatedObject_Medium_MultipleLegs][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Stone Table", "Stone Chair", "Stone Dresser" };
            appearances[CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Wooden Table", "Wooden Chair", "Wooden Dresser", "Wooden Ottoman",
                "Wooden Stool" };
            appearances[CreatureConstants.AnimatedObject_Medium_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Carpet", "Tapestry", "Rug", "Blanket" };
            appearances[CreatureConstants.AnimatedObject_Medium_TwoLegs] = GetAppearances(
                commonOther: new[] { "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" },
                uncommonOther: new[] { "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" },
                rareOther: new[] { "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" });
            appearances[CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Medium_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Cart", "Carriage" };
            appearances[CreatureConstants.AnimatedObject_Medium_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Clock", "Feather Duster", "Broom", "Bucket", "Barrel" };
            appearances[CreatureConstants.AnimatedObject_Small][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub" };
            appearances[CreatureConstants.AnimatedObject_Small_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Rope", "Vine", "Chain" };
            appearances[CreatureConstants.AnimatedObject_Small_MultipleLegs][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Stone Table", "Stone Chair", "Stone Dresser" };
            appearances[CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Wooden Table", "Wooden Chair", "Wooden Dresser", "Wooden Ottoman",
                "Wooden Stool" };
            appearances[CreatureConstants.AnimatedObject_Small_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Carpet", "Tapestry", "Rug", "Blanket" };
            appearances[CreatureConstants.AnimatedObject_Small_TwoLegs] = GetAppearances(
                commonOther: new[] { "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" },
                uncommonOther: new[] { "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" },
                rareOther: new[] { "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" });
            appearances[CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Small_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Cart", "Carriage" };
            appearances[CreatureConstants.AnimatedObject_Small_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Clock", "Feather Duster", "Broom", "Bucket", "Barrel" };
            appearances[CreatureConstants.AnimatedObject_Tiny][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Candlestick", "Candelabra", "Plate", "Cup", "Tea Pot", "Bath Tub" };
            appearances[CreatureConstants.AnimatedObject_Tiny_Flexible][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Rope", "Vine", "Chain" };
            appearances[CreatureConstants.AnimatedObject_Tiny_MultipleLegs][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Stone Table", "Stone Chair", "Stone Dresser"];
            appearances[CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "Wooden Table", "Wooden Chair", "Wooden Dresser", "Wooden Ottoman",
                "Wooden Stool" ];
            appearances[CreatureConstants.AnimatedObject_Tiny_Sheetlike][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Carpet", "Tapestry", "Rug", "Blanket"];
            appearances[CreatureConstants.AnimatedObject_Tiny_TwoLegs] = GetAppearances(
                commonOther: [ "Human statue", "High Elf statue", "Lightfoot Halfling statue", "Hill Dwarf statue", "Rock Gnome statue", "Half-Elf statue",
                    "Half-Orc statue", "Suit of plate armor" ],
                uncommonOther: [ "Wood Elf statue", "Wild Elf statue", "Gray Elf statue", "Aquatic Elf statue", "Drow statue",
                    "Tallfellow Halfling statue", "Deep Halfling statue",
                    "Mountain Dwarf statue", "Deep Dwarf statue", "Duergar statue",
                    "Forest Gnome statue", "Svirfneblin statue",
                    "Aasimar statue", "Tiefling statue" ],
                rareOther: [ "Orc statue", "Goblin statue", "Hobgoblin statue", "Kobold statue", "Troll statue", "Ogre statue", "Mind Flayer statue",
                    "Hill Giant statue", "Frost Giant statue", "Fire Giant statue", "Stone Giant statue", "Cloud Giant statue", "Storm Giant statue",
                    "Half-Celestial statue", "Half-Fiend statue", "Half-Dragon statue" ]);
            appearances[CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden] = GetAppearances(
                commonOther: [ "Human wooden figurine", "High Elf wooden figurine", "Lightfoot Halfling wooden figurine", "Hill Dwarf wooden figurine",
                    "Rock Gnome wooden figurine", "Half-Elf wooden figurine",
                    "Half-Orc wooden figurine", "Ladder" ],
                uncommonOther: [ "Wood Elf wooden figurine", "Wild Elf wooden figurine", "Gray Elf wooden figurine", "Aquatic Elf wooden figurine", "Drow wooden figurine",
                    "Tallfellow Halfling wooden figurine", "Deep Halfling wooden figurine",
                    "Mountain Dwarf wooden figurine", "Deep Dwarf wooden figurine", "Duergar wooden figurine",
                    "Forest Gnome wooden figurine", "Svirfneblin wooden figurine",
                    "Aasimar wooden figurine", "Tiefling wooden figurine" ],
                rareOther: [ "Orc wooden figurine", "Goblin wooden figurine", "Hobgoblin wooden figurine", "Kobold wooden figurine", "Troll wooden figurine",
                    "Ogre wooden figurine", "Mind Flayer wooden figurine",
                    "Hill Giant wooden figurine", "Frost Giant wooden figurine", "Fire Giant wooden figurine", "Stone Giant wooden figurine", "Cloud Giant wooden figurine",
                    "Storm Giant wooden figurine",
                    "Half-Celestial wooden figurine", "Half-Fiend wooden figurine", "Half-Dragon wooden figurine" ]);
            appearances[CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Cart", "Carriage"];
            appearances[CreatureConstants.AnimatedObject_Tiny_Wooden][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Clock", "Feather Duster", "Broom", "Bucket", "Barrel"];
            //Source: https://forgottenrealms.fandom.com/wiki/Ankheg
            appearances[CreatureConstants.Ankheg] = GetAppearances(
                commonSkin: ["Brown skin", "Yellow skin", "Green skin"],
                commonEyes: ["Black eyes"]);
            //Source: https://www.d20srd.org/srd/monsters/hag.htm#annis
            appearances[CreatureConstants.Annis] = GetAppearances(
                commonSkin: ["Blue-black skin, as if deeply bruised, covered in scars and blemishes"],
                commonHair: ["Black hair"],
                commonEyes: ["Dull, greenish-yellow eyes"],
                commonOther: [ "Lean with long, lanky limbs. Long, spiraling, stylized, black nails",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, black nails",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, rust-colored nails",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, rust-colored nails" ],
                uncommonOther: [ "Lean with long, lanky limbs. Long, spiraling, stylized, black nails. Chewed-through cheeks, etching a gangrenous grin",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, black nails. Chewed-through cheeks, etching a gangrenous grin",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, rust-colored nails. Chewed-through cheeks, etching a gangrenous grin",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, rust-colored nails. Chewed-through cheeks, etching a gangrenous grin",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, black nails. Corpses stitched to her back",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, black nails. Corpses stitched to her back",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, rust-colored nails. Corpses stitched to her back",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, rust-colored nails. Corpses stitched to her back",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, black nails. Right hand is a grafted Troll hand",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, black nails. Right hand is a grafted Troll hand",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, rust-colored nails. Right hand is a grafted Troll hand",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, rust-colored nails. Right hand is a grafted Troll hand",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, black nails. Left hand is a grafted Troll hand",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, black nails. Left hand is a grafted Troll hand",
                    "Lean with long, lanky limbs. Long, spiraling, stylized, rust-colored nails. Left hand is a grafted Troll hand",
                    "Gnarled muscles, jutting bones, humped back, hunched shoulders. Long, spiraling, stylized, rust-colored nails. Left hand is a grafted Troll hand"  ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Giant_ant
            appearances[CreatureConstants.Ant_Giant_Worker][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Red skin", "Brown skin"];
            appearances[CreatureConstants.Ant_Giant_Soldier][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Red skin", "Brown skin"];
            appearances[CreatureConstants.Ant_Giant_Queen][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Red skin", "Brown skin"];
            //Source: https://www.dimensions.com/element/eastern-lowland-gorilla-gorilla-beringei-graueri
            appearances[CreatureConstants.Ape] = GetAppearances(
                commonHair: ["Jet-black fur"],
                uncommonHair: ["Grey fur", "Silver fur"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_ape
            appearances[CreatureConstants.Ape_Dire] = GetAppearances(
                commonHair: ["Jet-black fur"],
                uncommonHair: ["Grey fur", "Silver fur"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Aranea
            appearances[CreatureConstants.Aranea] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                rareSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                veryRareSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]),
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]),
                veryRareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]),
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]),
                rareEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]),
                veryRareEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]));
            //Source: https://www.d20srd.org/srd/monsters/arrowhawk.htm
            appearances[CreatureConstants.Arrowhawk_Juvenile][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Covered in blue scales with occasional tufts of yellow feathers"];
            appearances[CreatureConstants.Arrowhawk_Adult][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Covered in blue scales with occasional tufts of yellow feathers"];
            appearances[CreatureConstants.Arrowhawk_Elder][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Covered in blue scales with occasional tufts of yellow feathers"];
            //Source: https://forgottenrealms.fandom.com/wiki/Assassin_vine
            appearances[CreatureConstants.AssassinVine][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Smaller vines extend from the main branch and bear clusters of grape-like berries. Stringy bark. Hand-shaped leaves"];
            //Source: https://www.d20srd.org/srd/monsters/athach.htm
            appearances[CreatureConstants.Athach][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Extra arm growing out of chest. Large, curved tusks on either side of a wide mouth. Relatively small eyes and nose for the size of the head. One ear significantly bigger than the other. Quite strong stench."];
            //Source: https://forgottenrealms.fandom.com/wiki/Avoral
            appearances[CreatureConstants.Avoral] = GetAppearances(
                commonSkin: ["Dark brown skin", "Brown skin", "Light brown skin",
                    "Dark tan skin", "Tan skin", "Light tan skin",
                    "Pink skin", "White skin", "Pale white skin"],
                commonHair: ["Red feathers", "Blue feathers", "Brown feathers", "Black feathers", "White feathers"],
                commonEyes: ["Bright golden eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Azer
            appearances[CreatureConstants.Azer] = GetAppearances(
                commonSkin: ["Brass-colored skin"],
                commonHair: [ "Brass-colored hair, brass-colored beard", "Brass-colored hair, fiery-colored beard", "Brass-colored hair, literal fire for beard",
                    "Fiery-colored hair, brass-colored beard", "Fiery-colored hair, fiery-colored beard", "Fiery-colored hair, literal fire for beard",
                    "Literal fire for hair, brass-colored beard", "Literal fire for hair, fiery-colored beard", "Literal fire for hair, literal fire for beard" ],
                commonEyes: appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]),
                rareEyes: appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]),
                veryRareEyes: appearances[CreatureConstants.Dwarf_Hill][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Dwarf_Deep][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Dwarf_Duergar][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Dwarf_Mountain][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare])
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Babau
            appearances[CreatureConstants.Babau][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black, leathery skin tight to a gaunt, skeletal frame"];
            appearances[CreatureConstants.Babau][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["White eyes that glow red when glaring."];
            appearances[CreatureConstants.Babau][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Terrible stench. Skull is tall and long with pointed ears and jaws filled with jagged teeth. A single long horn juts from the back of the skull, curving forward and downward."];
            //Source: https://www.dimensions.com/element/rhesus-macaque-macaca-mulatta
            appearances[CreatureConstants.Baboon][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Red face"];
            appearances[CreatureConstants.Baboon][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Brown hair"];
            //Source: https://animals.sandiegozoo.org/animals/honey-badger-ratel
            appearances[CreatureConstants.Badger][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Thick, coarse, black fur, with a wide gray-white stripe that stretches across the back, from the top of the head to the tip of the tail"];
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_badger
            appearances[CreatureConstants.Badger_Dire] = GetAppearances(
                commonHair: ["Thick brown fur"],
                uncommonHair: ["Thick grey fur", "Thick tan fur"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Balor
            appearances[CreatureConstants.Balor][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Dark red skin wrapped in glaring flames"];
            appearances[CreatureConstants.Balor][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Venom-dripping fangs, fearsome claws"];
            //Source: https://forgottenrealms.fandom.com/wiki/Hamatula
            appearances[CreatureConstants.BarbedDevil_Hamatula][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Gleaming, vigilant eyes constantly shifting, making it seem anxious"];
            appearances[CreatureConstants.BarbedDevil_Hamatula][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Body completely covered in barbs. Horned hands ending in strange, lengthy claws. Long, burly tails coated in spines"];
            //Source: https://forgottenrealms.fandom.com/wiki/Barghest
            appearances[CreatureConstants.Barghest] = GetAppearances(
                commonHair: ["Bluish-red fur", "Blue fur"],
                uncommonHair: ["Bluish-red fur, shock of white hair along the back", "Blue fur, shock of white hair along the back"],
                commonEyes: appearances[CreatureConstants.Dog][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => $"{e}, glowing orange when excited")
                    .Concat(appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => $"{e}, glowing orange when excited")),
                uncommonEyes: Combine(" and ",
                    appearances[CreatureConstants.Dog][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "eye"))
                        .Select(e => $"One {e}"),
                    appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(LowercaseFirstCharacter)
                        .Select(e => e.Replace("eyes", "eye"))
                        .Select(e => $"one {e}, both glowing orange when excited")));
            appearances[CreatureConstants.Barghest_Greater] = GetAppearances(
                commonHair: ["Bluish-red fur", "Blue fur"],
                uncommonHair: ["Bluish-red fur, shock of white hair along the back", "Blue fur, shock of white hair along the back"],
                commonEyes: appearances[CreatureConstants.Dog][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => $"{e}, glowing orange when excited")
                    .Concat(appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => $"{e}, glowing orange when excited")),
                uncommonEyes: Combine(" and ",
                    appearances[CreatureConstants.Dog][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "eye"))
                        .Select(e => $"One {e}"),
                    appearances[CreatureConstants.Goblin][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(LowercaseFirstCharacter)
                        .Select(e => e.Replace("eyes", "eye"))
                        .Select(e => $"one {e}, both glowing orange when excited")));
            //Source: https://forgottenrealms.fandom.com/wiki/Basilisk
            appearances[CreatureConstants.Basilisk] = GetAppearances(
                commonSkin: ["Dull brown skin, yellowish underbelly"],
                commonEyes: ["Glowing pale green eyes"],
                uncommonSkin: ["Dark gray skin, yellowish underbelly", "Dark yellow skin, yellowish underbelly", "Dark orange skin, yellowish underbelly"],
                commonOther: ["Single row of bony spikes lining the back"],
                rareOther: ["Single row of bony spikes lining the back. Curved horn atop the nose"]);
            appearances[CreatureConstants.Basilisk_Greater] = GetAppearances(
                commonSkin: ["Dull brown skin, yellowish underbelly"],
                commonEyes: ["Glowing pale green eyes"],
                uncommonSkin: ["Dark gray skin, yellowish underbelly", "Dark yellow skin, yellowish underbelly", "Dark orange skin, yellowish underbelly"],
                commonOther: ["Single row of bony spikes lining the back"],
                rareOther: ["Single row of bony spikes lining the back. Curved horn atop the nose"]);
            //Source: https://www.dimensions.com/element/little-brown-bat-myotis-lucifugus hanging height
            appearances[CreatureConstants.Bat] = GetAppearances(
                commonSkin: ["Pale red fur", "Pale tan fur", "Red fur", "Tan fur", "Brown fur", "Dark brown fur"],
                commonOther: ["Has a short snout and sloped forehead"]);
            appearances[CreatureConstants.Bat_Dire][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                ["Shaggy, pale red fur", "Shaggy, pale tan fur", "Shaggy, red fur", "Shaggy, tan fur", "Shaggy, brown fur", "Shaggy, dark brown fur"];
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            appearances[CreatureConstants.Bat_Swarm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["5,000 bats"];
            //Source: https://forgottenrealms.fandom.com/wiki/Black_bear
            appearances[CreatureConstants.Bear_Black][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Black fur", "Blond fur", "Cinnamon fur"];
            //Source: https://forgottenrealms.fandom.com/wiki/Brown_bear
            appearances[CreatureConstants.Bear_Brown][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Brown fur"];
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_bear
            appearances[CreatureConstants.Bear_Dire][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "Brown fur" };
            appearances[CreatureConstants.Bear_Dire][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = new[] { "Cold, piecing eyes." };
            appearances[CreatureConstants.Bear_Dire][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Bony brow ridges and extra-long claws, feral face" };
            //Source: https://forgottenrealms.fandom.com/wiki/Polar_bear
            appearances[CreatureConstants.Bear_Polar][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "White fur" };
            //Source: https://forgottenrealms.fandom.com/wiki/Barbazu
            appearances[CreatureConstants.BeardedDevil_Barbazu] = GetAppearances(
                commonSkin: new[] { "Green skin covered in scales" },
                uncommonSkin: new[] { "Red skin covered in scales", "Purple skin covered in scales" },
                commonOther: new[] { "Disgusting, wiry beard. Pointed ears and long tail. Clawed hands and feet." },
                uncommonOther: new[] { "Disgusting, wiry beard infested with maggots. Pointed ears and long tail. Clawed hands and feet." });
            appearances[CreatureConstants.Bebilith][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Like a massive, misshapen spider. Tough, chitinous, mottled exoskeleton. Two forelegs end in razor-sharp barbs. Fangs drip globs of odorous liquid." };
            //Source: https://www.d20srd.org/srd/monsters/giantBee.htm
            //https://www.dimensions.com/element/western-honey-bee-apis-mellifera
            appearances[CreatureConstants.Bee_Giant][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = new[] { "Alternating black and yellow stripes" };
            //Source: https://forgottenrealms.fandom.com/wiki/Behir
            appearances[CreatureConstants.Behir][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Deep blue scales, lighter on the underside", "Dark blue scales, lighter on the underside",
                    "Ultramarine scales, lighter on the underside",
                "Deep blue scales with gray-brown bands, lighter on the underside", "Dark blue scales with gray-brown bands, lighter on the underside",
                    "Ultramarine scales with gray-brown bands, lighter on the underside",
                "Deep blue scales with brown bands, lighter on the underside", "Dark blue scales with brown bands, lighter on the underside",
                    "Ultramarine scales with brown bands, lighter on the underside",
                "Deep blue scales with gray bands, lighter on the underside", "Dark blue scales with gray bands, lighter on the underside",
                    "Ultramarine scales with gray bands, lighter on the underside" };
            //Source: https://forgottenrealms.fandom.com/wiki/Beholder
            appearances[CreatureConstants.Beholder] = GetAppearances(
                commonSkin: new[] { "Purple, pebbly-textured skin on top graduating to earth tones further down",
                    "Blue, pebbly-textured skin on top graduating to earth tones further down",
                    "Brown, pebbly-textured skin on top graduating to earth tones further down",
                    "Blue-purple, pebbly-textured skin on top graduating to earth tones further down" },
                uncommonSkin: [ "White, pebbly-textured skin on top graduating to earth tones further down",
                    "Red, pebbly-textured skin on top graduating to earth tones further down",
                    "Grey, pebbly-textured skin on top graduating to earth tones further down",
                    "Green, pebbly-textured skin on top graduating to earth tones further down" ],
                commonOther: ["A giant eye surrounded by smaller eye stalks. Has a massive, gaping maw."],
                uncommonOther: ["A giant eye surrounded by smaller eye stalks. Has a massive, gaping maw. Nostrils and jointed, articulated eyestalks."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Gauth
            appearances[CreatureConstants.Beholder_Gauth] = GetAppearances(
                commonSkin: [ "Brown skin, mottled with purple and gray", "Brown skin, mottled with purple", "Brown skin, mottled with gray",
                    "Purple skin, mottled with brown and gray", "Purple skin, mottled with brown", "Purple skin, mottled with gray",
                    "Gray skin, mottled with purple and brown", "Gray skin, mottled with purple", "Gray skin, mottled with brown" ],
                uncommonSkin: [ "Purple, pebbly-textured skin on top graduating to earth tones further down",
                    "Blue, pebbly-textured skin on top graduating to earth tones further down",
                    "Brown, pebbly-textured skin on top graduating to earth tones further down",
                    "Blue-purple, pebbly-textured skin on top graduating to earth tones further down" ],
                rareSkin: [ "White, pebbly-textured skin on top graduating to earth tones further down",
                    "Red, pebbly-textured skin on top graduating to earth tones further down",
                    "Grey, pebbly-textured skin on top graduating to earth tones further down",
                    "Green, pebbly-textured skin on top graduating to earth tones further down" ],
                commonOther: ["A giant eye surrounded by smaller eye stalks, along with feeding tendrils and sprout from the lower half of its body. The central eye is surrounded by a ridge of flesh and smaller eyes used for sight. Has a massive, gaping maw."],
                uncommonOther: ["A giant eye surrounded by smaller eye stalks, along with feeding tendrils and sprout from the lower half of its body. The central eye is surrounded by a ridge of flesh and smaller eyes used for sight. Has a massive, gaping maw. Nostrils and jointed, articulated eyestalks."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Belker
            appearances[CreatureConstants.Belker][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Primarily composed of smoke, has large, black, bat-like wings, clawed tendrils, and a biting maw. Base form was sort of demonic, but continually shifts and changes shape due to its semi-gaseous nature."];
            //Source: https://forgottenrealms.fandom.com/wiki/Bison
            appearances[CreatureConstants.Bison][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Distinctive broad head that connects to large, humped shoulders with a thick neck. Short, curved, hollow horns growing from the sides of its head. Body covered in fur. Head covered in a mantle of thick fur. Short legs. Small tail ending in a tassel of fur."];
            appearances[CreatureConstants.Bison + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                ["Distinctive broad head that connects to large, humped shoulders with a thick neck. Short, curved, hollow horns growing from the sides of its head. Body covered in fur. Head covered in a mantle of thick fur, chin with a black beard-like growth. Short legs. Small tail ending in a tassel of fur."];
            //Source: https://forgottenrealms.fandom.com/wiki/Black_pudding
            appearances[CreatureConstants.BlackPudding] = GetAppearances(
                commonSkin: ["Inky black skin"],
                uncommonSkin: ["Brown skin", "Grey skin", "White skin"]);
            appearances[CreatureConstants.BlackPudding_Elder] = GetAppearances(
                commonSkin: ["Inky black skin"],
                uncommonSkin: ["Brown skin", "Grey skin", "White skin"]);
            //Source: https://www.d20pfsrd.com/bestiary/monster-listings/magical-beasts/blink-dog
            appearances[CreatureConstants.BlinkDog] = GetAppearances(
                commonHair: ["Yellow-brown fur"],
                uncommonHair: ["Yellow fur", "Brown fur"],
                commonOther: ["Large ears"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Boar
            appearances[CreatureConstants.Boar] = GetAppearances(
                commonHair: ["Thick brown fur", "Thick gray fur"],
                commonOther: ["Sharp tusks jutting out from beneath the snout"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_boar
            appearances[CreatureConstants.Boar_Dire] = GetAppearances(
                commonHair: ["Thick brown fur", "Thick gray fur"],
                uncommonHair: ["Thick black fur"],
                commonOther: ["Sharp tusks jutting out from beneath the snout"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Bodak
            appearances[CreatureConstants.Bodak] = GetAppearances(
                commonSkin: ["Deathly pale white skin", "Deathly gray skin", "Deathly pale gray skin"],
                commonHair: ["No hair of any kind"],
                commonEyes: ["Empty, milky-white eyes outstretched into vertical ovals"],
                commonOther: ["Face twisted into an inhuman visage of sheer madness and horror"]);
            //Source: https://web.stanford.edu/~cbross/bombbeetle.html
            appearances[CreatureConstants.BombardierBeetle_Giant][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Body is dull blue-gray. Head and chest is brownish-orange."];
            //Source: https://forgottenrealms.fandom.com/wiki/Osyluth
            appearances[CreatureConstants.BoneDevil_Osyluth][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Dry, sickly skin that seems tautly stretched over every bone in its body"];
            appearances[CreatureConstants.BoneDevil_Osyluth][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Incredibly gaunt husk of a frame so emaciated that it seems skeletal. Tail of a giant scorpion while head looks like menacing skulls. The putrid stench of rot surrounds it."];
            //Source: https://forgottenrealms.fandom.com/wiki/Bralani
            appearances[CreatureConstants.Bralani] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                rareSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]),
                veryRareSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_Gray][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wild][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare])
                    .Concat(appearances[CreatureConstants.Elf_Wood][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]),
                commonHair: ["Silvery-white hair"],
                commonEyes: ["Eyes are an ever-changing rainbow of colors, depending on its mood"],
                commonOther: ["Broad and stocky"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Bugbear
            appearances[CreatureConstants.Bugbear] = GetAppearances(
                commonSkin: ["Yellow skin", "Reddish-yellow skin", "Yellowish-brown skin", "Reddish-brown skin"],
                uncommonSkin: ["Red skin", "Brown skin"],
                commonHair: ["Brown hair", "Red hair"],
                commonEyes: ["Yellow eyes", "Orange eyes", "Red eyes", "Brown eyes", "Greenish-white eyes"],
                commonOther: ["Large, hairy, wedge-shaped ears; tough hide; claws"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Bulette
            appearances[CreatureConstants.Bulette] = GetAppearances(
                commonSkin: [ "Around the head and rear, the armor is blue-brown. In between, it is gray-blue. Slightly darker skin around the eyes",
                    "Around the head and rear, the armor is blue-brown. In between, it is blue. Slightly darker skin around the eyes",
                    "Around the head and rear, the armor is blue-brown. In between, it is blue-green. Slightly darker skin around the eyes" ],
                commonEyes: ["Yellow eyes with blue-green pupils"],
                commonOther: ["Covered in thick, layered plates. Hea dis bullet-shaped, similar to a shark's, with a massive mouth. Stumpy but powerful legs."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Camel
            appearances[CreatureConstants.Camel_Bactrian] = GetAppearances(
                commonHair: ["White fur", "Pale tan fur", "Deep brown fur", "Tan fur", "Brown fur"],
                commonOther: ["Two humps. Broad feet"]);
            appearances[CreatureConstants.Camel_Dromedary] = GetAppearances(
                commonHair: ["White fur", "Pale tan fur", "Deep brown fur", "Tan fur", "Brown fur"],
                commonOther: ["One hump. Broad feet"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Carrion_crawler
            appearances[CreatureConstants.CarrionCrawler] = GetAppearances(
                commonSkin: ["Pale yellow skin", "Green skin, pale yellow underbelly", "Green skin"],
                commonEyes: ["Two eye stalks"],
                commonOther: ["Eight long tentacles protruding from the side of the head"]);
            //Source: https://g.co/kgs/eqa8L1
            //https://www.omlet.us/guide/cats/choosing_the_right_cat_for_you/cat_coat_colors_and_patterns/
            appearances[CreatureConstants.Cat][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "Siamese", "British Shorthair", "Maine Coon", "Persian", "Ragdoll", "Sphynx", "American Shorthair", "Abyssinian",
                "Exotic Shorthair", "Scottish Fold", "Burmese", "Birman", "Bombay", "Siberian", "Norwegian Forest", "Russian Blue", "American Curl", "Munchkin",
                "American Bobtail", "Devon Rex", "Balinese", "Oriental Shorthair", "Chartreux", "Turkish Angora", "Manx", "Japanese Bobtail", "American Wirehair",
                "Ragamuffin", "Egyptian Mau", "Cornish Rex", "Somali", "Himalayan", "Selkirk Rex", "Korat", "Singapura", "Ocicat", "Tonkinese", "Turkish Van",
                "British Longhair", "LaPerm", "Havana Brown", "Chausie", "Burmilla", "Toyger", "Sokoke", "Colorpoint Shorthair", "Javanese", "Snowshoe", "Australian Mist",
                "Lykoi", "Khao Manee",
                "Solid Black", "Solid White", "Solid Ginger", "Solid Blue/Gray", "Solid Cream", "Solid Brown", "Solid Cinnamon", "Solid Fawn",
                "Black/White Tuxedo", "Black/Gray Tuxedo", "Black/Cream Tuxedo", "Black/Fawn Tuxedo",
                    "Brown/White Tuxedo", "Brown/Gray Tuxedo", "Brown/Cream Tuxedo", "Brown/Fawn Tuxedo",
                    "Cinnamon/White Tuxedo", "Cinnamon/Gray Tuxedo", "Cinnamon/Cream Tuxedo", "Cinnamon/Fawn Tuxedo",
                    "Ginger/White Tuxedo", "Ginger/Gray Tuxedo", "Ginger/Cream Tuxedo", "Ginger/Fawn Tuxedo",
                "Black/White Magpie Bi-color", "Black/Gray Magpie Bi-color", "Black/Cream Magpie Bi-color", "Black/Fawn Magpie Bi-color",
                    "Brown/White Magpie Bi-color", "Brown/Gray Magpie Bi-color", "Brown/Cream Magpie Bi-color", "Brown/Fawn Magpie Bi-color",
                    "Cinnamon/White Magpie Bi-color", "Cinnamon/Gray Magpie Bi-color", "Cinnamon/Cream Magpie Bi-color", "Cinnamon/Fawn Magpie Bi-color",
                    "Ginger/White Magpie Bi-color", "Ginger/Gray Magpie Bi-color", "Ginger/Cream Magpie Bi-color", "Ginger/Fawn Magpie Bi-color",
                "Black/White Harlequin Bi-color", "Black/Gray Harlequin Bi-color", "Black/Cream Harlequin Bi-color", "Black/Fawn Harlequin Bi-color",
                    "Brown/White Harlequin Bi-color", "Brown/Gray Harlequin Bi-color", "Brown/Cream Harlequin Bi-color", "Brown/Fawn Harlequin Bi-color",
                    "Cinnamon/White Harlequin Bi-color", "Cinnamon/Gray Harlequin Bi-color", "Cinnamon/Cream Harlequin Bi-color", "Cinnamon/Fawn Harlequin Bi-color",
                    "Ginger/White Harlequin Bi-color", "Ginger/Gray Harlequin Bi-color", "Ginger/Cream Harlequin Bi-color", "Ginger/Fawn Harlequin Bi-color",
                "Black/White Cap-And-Saddle Bi-color", "Black/Gray Cap-And-Saddle Bi-color", "Black/Cream Cap-And-Saddle Bi-color", "Black/Fawn Cap-And-Saddle Bi-color",
                    "Brown/White Cap-And-Saddle Bi-color", "Brown/Gray Cap-And-Saddle Bi-color", "Brown/Cream Cap-And-Saddle Bi-color", "Brown/Fawn Cap-And-Saddle Bi-color",
                    "Cinnamon/White Cap-And-Saddle Bi-color", "Cinnamon/Gray Cap-And-Saddle Bi-color", "Cinnamon/Cream Cap-And-Saddle Bi-color", "Cinnamon/Fawn Cap-And-Saddle Bi-color",
                    "Ginger/White Cap-And-Saddle Bi-color", "Ginger/Gray Cap-And-Saddle Bi-color", "Ginger/Cream Cap-And-Saddle Bi-color", "Ginger/Fawn Cap-And-Saddle Bi-color",
                "Black/White Van Bi-color", "Black/Gray Van Bi-color", "Black/Cream Van Bi-color", "Black/Fawn Van Bi-color",
                    "Brown/White Van Bi-color", "Brown/Gray Van Bi-color", "Brown/Cream Van Bi-color", "Brown/Fawn Van Bi-color",
                    "Cinnamon/White Van Bi-color", "Cinnamon/Gray Van Bi-color", "Cinnamon/Cream Van Bi-color", "Cinnamon/Fawn Van Bi-color",
                    "Ginger/White Van Bi-color", "Ginger/Gray Van Bi-color", "Ginger/Cream Van Bi-color", "Ginger/Fawn Van Bi-color",
                "Black Striped Tabby", "White Striped Tabby", "Ginger Striped Tabby", "Blue/Gray Striped Tabby", "Cream Striped Tabby", "Brown Striped Tabby",
                    "Cinnamon Striped Tabby", "Fawn Striped Tabby",
                "Black Blotched Tabby", "White Blotched Tabby", "Ginger Blotched Tabby", "Blue/Gray Blotched Tabby", "Cream Blotched Tabby", "Brown Blotched Tabby",
                    "Cinnamon Blotched Tabby", "Fawn Blotched Tabby",
                "Black Spotted Tabby", "White Spotted Tabby", "Ginger Spotted Tabby", "Blue/Gray Spotted Tabby", "Cream Spotted Tabby", "Brown Spotted Tabby",
                    "Cinnamon Spotted Tabby", "Fawn Spotted Tabby",
                "Black Ticked Tabby", "White Ticked Tabby", "Ginger Ticked Tabby", "Blue/Gray Ticked Tabby", "Cream Ticked Tabby", "Brown Ticked Tabby",
                    "Cinnamon Ticked Tabby", "Fawn Ticked Tabby",
                "Black/Ginger Brindled Tortoiseshell", "Blue/Ginger Brindled Tortoiseshell", "Cream/Ginger Brindled Tortoiseshell",
                "Black/Ginger Patchy Tortoiseshell", "Blue/Ginger Patchy Tortoiseshell", "Cream/Ginger Patchy Tortoiseshell",
                "Black/Ginger Brindled Torbie", "Blue/Ginger Brindled Torbie", "Cream/Ginger Brindled Torbie",
                "Black/Ginger Patchy Torbie", "Blue/Ginger Patchy Torbie", "Cream/Ginger Patchy Torbie",
                "Ginger/Black/White Callico", "Ginger/Gray/White Callico", "Ginger/Cream/White Callico",
                "Black Colorpoint", "White Colorpoint", "Ginger Colorpoint", "Blue/Gray Colorpoint", "Cream Colorpoint", "Brown Colorpoint", "Cinnamon Colorpoint",
                    "Fawn Colorpoint" ];
            //Source: https://forgottenrealms.fandom.com/wiki/Centaur
            appearances[CreatureConstants.Centaur] = GetAppearances(
                commonSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                    appearances[CreatureConstants.Horse_Heavy][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                ),
                uncommonSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                    appearances[CreatureConstants.Horse_Heavy][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]
                ),
                rareSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                    appearances[CreatureConstants.Horse_Heavy][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare]
                ),
                veryRareSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                    appearances[CreatureConstants.Horse_Heavy][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]
                ),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon],
                rareEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare],
                veryRareEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
            );
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Tiny
            appearances[CreatureConstants.Centipede_Monstrous_Tiny][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Small
            appearances[CreatureConstants.Centipede_Monstrous_Small][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Medium
            appearances[CreatureConstants.Centipede_Monstrous_Medium][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Large
            appearances[CreatureConstants.Centipede_Monstrous_Large][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Huge
            appearances[CreatureConstants.Centipede_Monstrous_Huge][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Gargantuan
            appearances[CreatureConstants.Centipede_Monstrous_Gargantuan][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Centipede,_Colossal
            appearances[CreatureConstants.Centipede_Monstrous_Colossal][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Black skin", "Brown skin", "Grey skin", "Red skin", "Pale grey skin"];
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            appearances[CreatureConstants.Centipede_Swarm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["1,500 centipedes"];
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#chainDevilKyton
            appearances[CreatureConstants.ChainDevil_Kyton][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Red skin", "Grey skin", "Grey skin with red tattoos"];
            //Source: https://forgottenrealms.fandom.com/wiki/Chaos_beast
            appearances[CreatureConstants.ChaosBeast][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [
                "A towering thing of pulpy flesh and exposed veins, covered in fangs and hooks. Three legs upon which it walks with unsteady and lurching steps. Face is fractured, with bent eyes and nose hooked three times.",
                "A filmy mass of rope-like tentacles with vermilion tips that slither along. Bulbous body ringed by gnashing mouths and topped by a viscous sac holding ten bobbing eyes. Dozens of vestigial wings that flap too feebly to give it flight.",
                "A powerful and muscular beast, armed with claws and alligator jaws.",
                "A graceful six-legged creature with delicate arms and shining skin. Maned head with three flashing eyes. Walks with a smooth gait and noble bearing.",
                "A man-sized creature with stumpy feet - can only shuffle. Hands like crab's claws. Body like dough falling from broken bones. Head is balding and pitted with empty eye sockets.",
                "A slender raptor-like creature with large talons underneath its body and broad wings, all covered with dark fur. Two hateful, coldly-gleaming eyes.",
                "A sac of intestines, warm and wet and steaming. Both deaf and blind. To move, it rolls and flops like limp sausages falling down stairs, becoming looped and draped around all obstacles and lying as a tangled sprawl.",
                "A hollowed-out carcass, swelling with gases into a balloon-like husk, bobbing along in the air with the ends of its body trailing about it and tangling others.",
                "A plump moth with brilliantly colored wings like the stained glass of temples and compound eyes that sparkle like a myriad of jewels.",
                "The person one least suspects.",
                "A writhing mass of teeth and barbed tentacles, grey and rotting. Random clawed hands hide under some of the tentacles.",
                "DM Discretion - come up with something particularly chaotic" ];
            //Source: https://www.dimensions.com/element/cheetahs
            //https://forgottenrealms.fandom.com/wiki/Cheetah
            appearances[CreatureConstants.Cheetah][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] =
                ["Yellow coat with black spots", "Sand-colored coat with black spots", "Yellow coat with brown spotsd", "Sand-colored coat with brown spots"];
            appearances[CreatureConstants.Cheetah][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Slender body, long thin legs, deep chest, small head"];
            //Source: https://forgottenrealms.fandom.com/wiki/Chimera
            appearances[CreatureConstants.Chimera_Black][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Hindquarters of a goat, the forequarters of a lion, and a set of Black Dragon wings. Three heads: that of a horned goat, a lion, and a Black Dragon."];
            appearances[CreatureConstants.Chimera_Blue][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Hindquarters of a goat, the forequarters of a lion, and a set of Blue Dragon wings. Three heads: that of a horned goat, a lion, and a Blue Dragon."];
            appearances[CreatureConstants.Chimera_Green][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Hindquarters of a goat, the forequarters of a lion, and a set of Green Dragon wings. Three heads: that of a horned goat, a lion, and a Green Dragon."];
            appearances[CreatureConstants.Chimera_Red][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Hindquarters of a goat, the forequarters of a lion, and a set of Red Dragon wings. Three heads: that of a horned goat, a lion, and a Red Dragon."];
            appearances[CreatureConstants.Chimera_White][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Hindquarters of a goat, the forequarters of a lion, and a set of White Dragon wings. Three heads: that of a horned goat, a lion, and a White Dragon."];
            //Source: https://forgottenrealms.fandom.com/wiki/Choker
            appearances[CreatureConstants.Choker] = GetAppearances(
                commonSkin: ["Gray, rubbery skin", "Spotted gray, rubbery skin", "Earthy-brown, rubbery skin", "Spotted earthy-brown, rubbery skin"],
                commonOther: ["Roughly humanoid, with a small torso and head. Arms and legs are long and thin, more like tentacles. Hands are shaped somewhat like a starfish, with a spiny surface—stiff cartilage jutting through the skin. Fingers are enormously long. Limbs move with the fluidity of tentacles."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Chuul
            appearances[CreatureConstants.Chuul] = GetAppearances(
                commonSkin: ["Dark yellow-green exoskeleton"],
                commonEyes: ["Black eyes"],
                commonOther: ["Four long legs and two large crustacean-like claws. Fan-like tail. Mass of tentacles around the mouth."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Cloaker
            appearances[CreatureConstants.Cloaker] = GetAppearances(
                commonSkin: ["Black skin, pale belly"],
                commonEyes: ["Red eyes"],
                commonOther: ["Long, whiplike tail. Head set within the \"cowl\" of the cloak-like body. Toothy maw. Bony claws adjacent to the head look almost like clasps for the cloak."],
                uncommonOther: ["Long, whiplike tail. Head set within the \"cowl\" of the cloak-like body. Toothy maw. Bony claws adjacent to the head look almost like clasps for the cloak. A series of spots run in two parallel lines down the backs, giving the appearance of buttons."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Cockatrice
            appearances[CreatureConstants.Cockatrice] = GetAppearances(
                commonSkin: ["Yellow-green scales on the tail, grey skin on the wings, yellow beak and feet"],
                commonHair: ["Golden brown feathers"],
                commonEyes: ["Glowing crimson eyes"],
                commonOther: ["Lizard-like tail, wings of a bat"]);
            appearances[CreatureConstants.Cockatrice + GenderConstants.Male] = GetAppearances(
                commonSkin: ["Yellow-green scales on the tail, grey skin on the wings, yellow beak and feet"],
                commonHair: ["Golden brown feathers"],
                commonEyes: ["Glowing crimson eyes"],
                commonOther: ["Lizard-like tail, wings of a bat, rooster's red wattle and comb"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Couatl
            appearances[CreatureConstants.Couatl][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Long feathered serpent with a pair of rainbow-feathered wings."];
            //Source: https://www.mojobob.com/roleplay/monstrousmanual/s/sphinx.html
            appearances[CreatureConstants.Criosphinx][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Falcon-like wings, ram-like head. Tawny lion body. Mouth full of sharp teeth."];
            //Source: https://en.wikipedia.org/wiki/Nile_crocodile#Characteristics_and_physiology
            appearances[CreatureConstants.Crocodile] = GetAppearances(
                commonSkin: [
                    "Dark bronze colouration above, with faded blackish spots and stripes variably appearing across the back and a dingy off-yellow on the belly. The flanks, which are yellowish-green in colour, have dark patches arranged in oblique stripes",
                    "Dark bronze colouration above, with faded blackish spots and stripes variably appearing across the back and a dingy off-yellow on the belly, but mud obscures its actual colour. The flanks, which are yellowish-green in colour, have dark patches arranged in oblique stripes"],
                uncommonSkin: [
                    "Bronze colouration above, with faded grayish spots and stripes variably appearing across the back and a dingy, light off-yellow on the belly. The flanks, which are light yellowish-green in colour, have semi-dark patches arranged in oblique stripes",
                    "Bronze colouration above, with faded grayish spots and stripes variably appearing across the back and a dingy, light off-yellow on the belly, but mud obscures its actual colour. The flanks, which are light yellowish-green in colour, have semi-dark patches arranged in oblique stripes",
                    "Very dark bronze colouration above, with blackish spots and stripes variably appearing across the back and a dingy, dark off-yellow on the belly. The flanks, which are dark yellowish-green in colour, have very dark patches arranged in oblique stripes",
                    "Very dark bronze colouration above, with blackish spots and stripes variably appearing across the back and a dingy, dark off-yellow on the belly, but mud obscures its actual colour. The flanks, which are dark yellowish-green in colour, have very dark patches arranged in oblique stripes"],
                commonEyes: ["Green eyes"]);
            //Source: https://en.wikipedia.org/wiki/Saltwater_crocodile#Description
            appearances[CreatureConstants.Crocodile_Giant] = GetAppearances(
                commonSkin: [
                    "Darker greenish-drab skin. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Darker greenish-drab skin. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                ],
                uncommonSkin: [
                    "Darker greenish-drab skin, with a few lighter tan areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Darker greenish-drab skin, with a few lighter tan areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Darker greenish-drab skin, with a few lighter grey areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Darker greenish-drab skin, with a few lighter grey areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Pale greenish-drab skin. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Pale greenish-drab skin. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark black-green skin. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark black-green skin. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark blackish skin. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark blackish skin. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                ],
                rareSkin: [
                    "Pale greenish-drab skin, with a few lighter tan areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Pale greenish-drab skin, with a few lighter tan areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Pale greenish-drab skin, with a few lighter grey areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Pale greenish-drab skin, with a few lighter grey areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark black-green skin, with a few lighter tan areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark black-green skin, with a few lighter tan areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark black-green skin, with a few lighter grey areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark black-green skin, with a few lighter grey areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark blackish skin, with a few lighter tan areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark blackish skin, with a few lighter tan areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark blackish skin, with a few lighter grey areas. The ventral surface is white. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                    "Dark blackish skin, with a few lighter grey areas. The ventral surface is yellow. Stripes are present on the lower sides of the body, but do not extend onto the belly. Tail is grey with dark bands.",
                ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Cryohydra
            appearances[CreatureConstants.Cryohydra_5Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_6Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_7Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_8Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_9Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_10Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_11Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Cryohydra_12Heads] = GetAppearances(
                commonSkin: new[] { "Purplish-brown skin", "Brown skin", "Dark brown skin", "Dark purplish-brown skin" },
                commonEyes: new[] { "Amber eyes" });
            //Source: https://forgottenrealms.fandom.com/wiki/Darkmantle
            appearances[CreatureConstants.Darkmantle] = GetAppearances(
                commonSkin: new[] { "Limestone-colored skin" },
                uncommonSkin: new[] { "Dolomite-colored skin", "Gypsum-colored skin", "Marble-colored skin", "Anhydrite-colored skin", "Halite-colored skin" },
                commonEyes: new[] { "Eight eyespots, but not actual eyes" });
            //Source: https://www.dimensions.com/element/deinonychus-deinonychus-antirrhopus
            //https://jurassicworld-evolution.fandom.com/wiki/Deinonychus#Skins
            appearances[CreatureConstants.Deinonychus] = GetAppearances(
                commonSkin: new[] { "Red skin, tan underbelly", "Gray skin, tan underbelly", "Orange skin, mottled yellow-orange underbelly",
                    "Turquoise skin, gray underbelly", "Green skin, yellow underbelly", "Purple skin with pink accents, gray underbelly" },
                commonOther: new[] { "Small body, sleek horizontal posture, ratite-like spine, and enlarged raptorial claws on the feet. A large, sickle-shaped talon is on the second toe of each hind foot." });
            //Source: https://forgottenrealms.fandom.com/wiki/Delver
            appearances[CreatureConstants.Delver][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Huge teardrop-shaped body with rock-like skin, always covered in a mucus-like slime. Almost the entire underside of the body consists of their mouth. Only two flipper-like appendages at the front of the body ending in blunt claws" };
            //Source: https://forgottenrealms.fandom.com/wiki/Derro
            appearances[CreatureConstants.Derro] = GetAppearances(
                commonSkin: new[] { "Pale blue-white skin", "Blue-gray skin" },
                uncommonSkin: new[] { "Pale blue-gray skin", "Blue-white skin" },
                commonHair: new[] { "White hair", "Yellow hair", "Pale tan hair" },
                commonEyes: new[] { "Uniformly pale eyes lacking irisies and pupils" });
            appearances[CreatureConstants.Derro_Sane] = GetAppearances(
                commonSkin: ["Pale blue-white skin", "Blue-gray skin"],
                uncommonSkin: ["Pale blue-gray skin", "Blue-white skin"],
                commonHair: ["White hair", "Yellow hair", "Pale tan hair"],
                commonEyes: ["Uniformly pale eyes lacking irisies and pupils"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Destrachan
            appearances[CreatureConstants.Destrachan][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Bipedal, vaguely reptilian. Head is largely featureless except for the circular mouth and the large ear structures."];
            //Source: https://forgottenrealms.fandom.com/wiki/Devourer
            appearances[CreatureConstants.Devourer][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Tall, gaunt skeletal figure with a smaller figure trapped within its rib cage"];
            //Source: https://forgottenrealms.fandom.com/wiki/Digester
            appearances[CreatureConstants.Digester][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Gray, pebbly skin with dagger-like markings"];
            appearances[CreatureConstants.Digester][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Two powerful hind legs. Long tail. Head is narrow with a sucking mouth, sporting a tube-like orifice on the forehead."];
            //Source: https://forgottenrealms.fandom.com/wiki/Displacer_beast
            appearances[CreatureConstants.DisplacerBeast] = GetAppearances(
                commonHair: ["Blue fur", "Blue-black fur", "Black fur"],
                uncommonHair: ["Dark blue fur", "Purple fur"],
                commonEyes: ["Striking, glowing emerald-green eyes"],
                commonOther: ["Panther-like with six legs and a pair of tentacles sprouting from their shoulders. Tentacles end in pads with sharp, horny, brownish-yellow edges"],
                uncommonOther: ["Panther-like with six legs and a pair of tentacles sprouting from their shoulders. Tentacles end in pads with sharp, horny, brownish-yellow edges. Emaciated-looking."]);
            appearances[CreatureConstants.DisplacerBeast_PackLord] = GetAppearances(
                commonHair: ["Black fur", "Ebony fur"],
                uncommonHair: ["Dark blue fur", "Purple fur", "Blue fur", "Blue-black fur"],
                commonEyes: ["Striking, glowing emerald-green eyes"],
                commonOther: ["Panther-like with six legs and a pair of tentacles sprouting from their shoulders. Tentacles end in pads with sharp, horny, brownish-yellow edges"],
                uncommonOther: ["Panther-like with six legs and a pair of tentacles sprouting from their shoulders. Tentacles end in pads with sharp, horny, brownish-yellow edges. Emaciated-looking."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Djinni
            appearances[CreatureConstants.Djinni] = GetAppearances(
                commonSkin: ["Pale blue skin", "Olive-brown skin", "Blue skin", "Dark blue skin"],
                uncommonSkin: ["Dark tan skin"],
                rareSkin: ["Purple skin", "Dark purple skin", "Beige skin", "Dark red skin"],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Bald", "Black hair"]),
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: ["Brown eyes"],
                rareEyes: ["Blue eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Noble_djinni
            appearances[CreatureConstants.Djinni_Noble] = GetAppearances(
                commonSkin: ["Pale blue skin", "Fair brown skin", "Fair Blue skin"],
                uncommonSkin: ["Fair tan skin"],
                rareSkin: ["Fair purple skin", "Fair beige skin", "Fair red skin"],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Bald", "Black hair"]),
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: ["Brown eyes"],
                rareEyes: ["Blue eyes"]);
            //Source: https://g.co/kgs/q7esXM
            //https://i.redd.it/3lbyepoqhnlb1.jpg
            //https://coatsandcolors.com/dog-eye-colors/
            appearances[CreatureConstants.Dog] = GetAppearances(
                commonSkin: ["Bulldog", "French Bulldog", "Beagle", "Standard Poodle", "Chihuahua", "Dachsund", "Bichon Frise",
                "Maltese",
                "Chow Chow", "English Cocker Spaniel" , "Pomeranian", "Yorkshire Terrier", "Cavalier King Charles Spaniel", "Pembroke Welsh Corgi", "Basenji", "Havanese",
                "Boston Terrier", "Cairn Terrier", "Brittany", "Sheltie", "Shiba Inu", "Jack Russell Terrier", "Borzoi", "Maltipoo", "Papillon", "Pikanese",
                "Miniature Poodle", "Pikapoo", "Coyote", "Dingo", "Miniature Schnauzer", "Mutt", "English Bulldog", "Cavapoo", "Red Kelpie", "American Foxhound",
                "Brussels Griffon", "Aussie Terrier", "German Spitz", "Pug", "Teacup Pomeranian", "Saluki", "Schnauzer Terrier", "Presa Canario"],
                commonEyes: ["Brown eyes", "Hazel eyes", "Amber eyes", "Yellow eyes", "Blue eyes", "Green eyes"]
            );
            //Source: https://www.google.com/search?q=dog+working+breeds
            //https://i.redd.it/3lbyepoqhnlb1.jpg
            //https://coatsandcolors.com/dog-eye-colors/
            appearances[CreatureConstants.Dog_Riding] = GetAppearances(
                commonSkin: ["German Shepherd", "Labrador Retriever", "Golden Retriever", "Siberian Husky", "Beagle",
                "Alaskan Malamute",
                "Border Collie", "Rottweiler", "Australian Shepherd", "Airedale Terrier", "Affenpinscher", "Afghan Hound", "American Eskimo", "Anatolian Shepherd",
                "Basset Hound", "Belgian Malinois", "Boston Terrier", "Bullmastiff", "Black Russian Terrier", "Bedlington Terrier", "American Pit Bull Terrier", "Doberman",
                "Sarabi", "Samoyed", "American Bully", "Borzoi", "Goldendoodle", "Dalmation", "Labradoodle", "Bernese Mountain Dog", "Greater Swiss Mountain Dog",
                "Newfoundland", "Boxer", "Cane Corso", "Boerboel", "Chinook", "Dogue de Bordeaux", "Giant Schnauzer", "Neopolitan Mastiff", "Dogo Argentino",
                "Great Pyrenees", "Great Dane", "Tibetan Mastiff", "Kuvasz", "Komondor", "German Pinscher", "Leonberger", "Saint Bernard", "Portugese Water Dog",
                "Standard Schnauzer", "English Mastiff", "Karelian Bear Dog", "American Bulldog", "Hovawart", "Bloodhound", "Entlebucher Mountain Dog", "Eurasier",
                "Greenland", "Canadian Eskimo", "Canaan", "Caucasian Shepherd", "Blue Lacy", "American Akita", "Akbash", "Central Asian Shepherd", "Australian Cattle",
                "Yakutian Laika", "Mutt", "Blue Heeler", "Red Heeler", "Labrador", "Japanese Akita", "Red/Blue Heeler Mix", "Chocolate Labrador", "Chow Chow", "English Setter",
                "Irish Wolfhound", "Irish Setter", "Mastiff", "Greyhound", "Chocolate Labrador/Rottweiler Mix", "Maremma Sheepdog", "Catahoula Leopard Dog", "Rough Collie"],
                commonEyes: ["Brown eyes", "Hazel eyes", "Amber eyes", "Yellow eyes", "Blue eyes", "Green eyes"]
            );
            //Source: https://www.britannica.com/animal/donkey
            appearances[CreatureConstants.Donkey] = GetAppearances(
                commonHair: [ "White fur with dark stripe from mane to tail and a crosswise stripe on the shoulders",
                    "Light gray fur with dark stripe from mane to tail and a crosswise stripe on the shoulders",
                    "Gray fur with dark stripe from mane to tail and a crosswise stripe on the shoulders",
                    "Dark gray fur with darker stripe from mane to tail and a crosswise stripe on the shoulders",
                    "Black fur with gray stripe from mane to tail and a crosswise stripe on the shoulders" ],
                commonOther: ["Long ears, surefooted"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Doppelganger
            appearances[CreatureConstants.Doppelganger] = GetAppearances(
                commonSkin: ["Gray skin"],
                commonHair: ["Hairless"],
                commonEyes: ["Bulging, pale yellow eyes lacking visible pupils", "Bulging, yellow eyes lacking visible pupils"],
                rareEyes: ["Bulging, green eyes lacking visible pupils", "Bulging, white eyes lacking visible pupils"],
                commonOther: ["Elven ears. Bulbous head with formless face."]);
            //Source: Draconomicon
            appearances[CreatureConstants.Dragon_Black_Wyrmling] = GetAppearances(
                commonSkin: ["Thin, glossy black one-inch scales"],
                commonEyes: ["Deep-socketed eyes"],
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. One inch of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Thin, glossy black two-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Two inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_Young] = GetAppearances(
                commonSkin: new[] { "Thin, glossy black three-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Three inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_Juvenile] = GetAppearances(
                commonSkin: new[] { "Thin, glossy black four-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Four inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Matte black five-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Five inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_Adult] = GetAppearances(
                commonSkin: new[] { "Matte black six-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Six inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Matte black seven-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Seven inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_Old] = GetAppearances(
                commonSkin: new[] { "Matte black eight-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Eight inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_VeryOld] = GetAppearances(
                commonSkin: new[] { "Thick, dull black nine-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Nine inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_Ancient] = GetAppearances(
                commonSkin: new[] { "Thick, dull black ten-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Ten inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_Wyrm] = GetAppearances(
                commonSkin: new[] { "Thick, dull black eleven-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Eleven inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Black_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Thick, dull black twelve-inch scales" },
                commonEyes: new[] { "Deep-socketed eyes" },
                commonOther: new[] { "Broad nasal openings, making its face look like a skull. Segmented horns the curve forward and down, somewhat like a ram's horns, but not as curly. These horns are bone-colored near their bases, but darken to dead black at the tips. Twelve inches of deteriorated flesh around the horns and cheekbones, as though eaten by acid, leaving only thin layers of hide covering the skull. Most teeth protrude when the mouth is closed. Big spikes stud the lower jaw. Small horns jut from the chin, and a row of hornlets crown the head." });
            appearances[CreatureConstants.Dragon_Blue_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Thin, iridescent azure scales", "Thin, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Thin, iridescent azure scales", "Thin, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_Young] = GetAppearances(
                commonSkin: new[] { "Thin, iridescent azure scales", "Thin, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_Juvenile] = GetAppearances(
                commonSkin: new[] { "Thin, iridescent azure scales", "Thin, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Iridescent azure scales", "Glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_Adult] = GetAppearances(
                commonSkin: new[] { "Iridescent azure scales", "Glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Iridescent azure scales", "Glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_Old] = GetAppearances(
                commonSkin: new[] { "Iridescent azure scales", "Glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_VeryOld] = GetAppearances(
                commonSkin: new[] { "Thick, iridescent azure scales", "Thick, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_Ancient] = GetAppearances(
                commonSkin: new[] { "Thick, iridescent azure scales", "Thick, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_Wyrm] = GetAppearances(
                commonSkin: new[] { "Thick, iridescent azure scales", "Thick, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Blue_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Thick, iridescent azure scales", "Thick, glossy, deep indigo scales" },
                commonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has two points, with the primary slightly curved and reaching well forward, with the smaller secondary point behind. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed." },
                uncommonOther: new[] { "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has one point, slightly curved and reaching well forward. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed.",
                    "Dramatic frilled ears. A single horn atop its short, blunt head. The horn juts forward from a base that takes up most of the top of the head. The horn has three points, with the primary slightly curved and reaching well forward, with the smaller second and third points behind in a line. Rows of hornlets line the brow ridges and run back from the nostrils along the entire length of the head. Short snout with underslung lower jaw. Cluster of bladelike scales under its chin and hornlets on its cheeks. Most teeth protrude when its mouth is closed."});
            appearances[CreatureConstants.Dragon_Brass_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Dull, mottled brown scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Dull, mottled brown scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_Young] = GetAppearances(
                commonSkin: new[] { "Dull brown scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_Juvenile] = GetAppearances(
                commonSkin: new[] { "Dull brown scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Brown scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes with small pupils" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Very sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_Adult] = GetAppearances(
                commonSkin: new[] { "Brown scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes with small pupils" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Very sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Brass-colored scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes with small pupils" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Very sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_Old] = GetAppearances(
                commonSkin: new[] { "Brass-colored scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes with very small pupils" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Very sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_VeryOld] = GetAppearances(
                commonSkin: new[] { "Warm, brass-colored scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes with very small pupils" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Extremely sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_Ancient] = GetAppearances(
                commonSkin: new[] { "Warm, brass-colored scales. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Brass-colored eyes with very small pupils" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Extremely sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_Wyrm] = GetAppearances(
                commonSkin: new[] { "Brass-colored scales with a warm, burnished appearance. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Eyes look like pupil-less, molten metal orbs" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Extremely sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Brass_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Brass-colored scales with a warm, burnished appearance. Wings and frills are mottled green where they join the body, and have reddish tints at the outer edges." },
                commonEyes: new[] { "Eyes look like pupil-less, molten metal orbs" },
                commonOther: new[] { "Fluted plate sweeping back from its eye sockets, forehead, and cheeks. The plate is dished like a plowshare. Extremely sharp, bladed chin horns. Supple, expressive lips." });
            appearances[CreatureConstants.Dragon_Bronze_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Smooth, flat, yellow scales tinged with green, showing only a hint of bronze" },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Smooth, flat, yellow scales tinged with green and bronze" },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_Young] = GetAppearances(
                commonSkin: new[] { "Smooth, flat, yellow scales tinged with bronze" },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_Juvenile] = GetAppearances(
                commonSkin: new[] { "Smooth, flat, yellow scales mottled with bronze" },
                commonEyes: new[] { "Green eyes with somewhat-faded pupils" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales" },
                commonEyes: new[] { "Green eyes with somewhat-faded pupils" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_Adult] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales" },
                commonEyes: new[] { "Green eyes with somewhat-faded pupils" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales" },
                commonEyes: new[] { "Green eyes with faded pupils" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Smaller horns have a secondary point. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_Old] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales" },
                commonEyes: new[] { "Green eyes with faded pupils" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Smaller horns have a secondary point. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_VeryOld] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales with a blue-black tint to the edges" },
                commonEyes: new[] { "Green eyes with faded pupils" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Smaller horns have a secondary point. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_Ancient] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales with a blue-black tint to the edges" },
                commonEyes: new[] { "Eyes have no pupils, and resemble glowing green orbs" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Smaller horns have a secondary point. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_Wyrm] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales with a blue-black tint to the edges" },
                commonEyes: new[] { "Eyes have no pupils, and resemble glowing green orbs" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Smaller horns have a secondary point. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Bronze_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Rich, bronze scales with a blue-black tint to the edges" },
                commonEyes: new[] { "Eyes have no pupils, and resemble glowing green orbs" },
                commonOther: new[] { "Ribbed and fluted crest sweeping back from the cheeks and eyes. Ribs in the crest end in curving horns. The horns are smooth, dark, and oval in cross-section, curving slightly inward toward the spine. Large horns on the top of the head. Smaller horns have a secondary point. Beaklike snout. Small head frill and tall neck frill. Webbed feet and webbing behind the forelimbs." });
            appearances[CreatureConstants.Dragon_Copper_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Ruddy brown scales with a metallic tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. One layer of triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Ruddy brown scales with a metallic tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. One layer of triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_Young] = GetAppearances(
                commonSkin: new[] { "Ruddy brown scales with a metallic tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. One layer of triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_Juvenile] = GetAppearances(
                commonSkin: new[] { "Ruddy brown scales with a metallic tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with somewhat-faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. One layer of triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with somewhat-faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Two layers of large, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_Adult] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with somewhat-faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Two layers of large, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Two layers of large, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_Old] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Two layers of large, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_VeryOld] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss and a green tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Three layers of huge, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_Ancient] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss and a green tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with very faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Three layers of huge, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_Wyrm] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss and a green tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Turquoise eyes with very faded pupils" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Three layers of huge, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Copper_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Coppery scales with a soft, warm gloss and a green tint. Green and red mottling along the trailing edges of the wings." },
                commonEyes: new[] { "Eyes have no pupils, and resemble glowing turquoise orbs" },
                commonOther: new[] { "Thick thighs and shoulders. Short face, no beak. Broad, smooth browplates jut over the eyes. Long, flat, coppery horns extend back from the browplates in a series of segments. Backswept cheek ridges and frills on the back of the lower jaw that sweep forward slightly. Three layers of huge, triangular blades point down from the chin." });
            appearances[CreatureConstants.Dragon_Gold_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Dark yellow scales with golden metallic flecks" },
                commonEyes: new[] { "Slanted, very narrow golden eyes" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills." });
            appearances[CreatureConstants.Dragon_Gold_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Dark yellow scales with large golden metallic flecks" },
                commonEyes: new[] { "Slanted, very narrow golden eyes" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Eight whiskers, four on upper jaw and four below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_Young] = GetAppearances(
                commonSkin: new[] { "Dark yellow scales mottled with metallic gold" },
                commonEyes: new[] { "Slanted, very narrow golden eyes" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Eight whiskers, four on upper jaw and four below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_Juvenile] = GetAppearances(
                commonSkin: new[] { "Metallic golden scales with large dark yellow flecks" },
                commonEyes: new[] { "Slanted, very narrow golden eyes with somewhat-faded pupils" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Eight whiskers, four on upper jaw and four below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Metallic golden scales with dark yellow flecks" },
                commonEyes: new[] { "Slanted, very narrow golden eyes with somewhat-faded pupils" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Eight whiskers, four on upper jaw and four below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_Adult] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow golden eyes with somewhat-faded pupils" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Ten whiskers, five on upper jaw and five below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow golden eyes with faded pupils" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Ten whiskers, five on upper jaw and five below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_Old] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow golden eyes with faded pupils" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Ten whiskers, five on upper jaw and five below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_VeryOld] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow golden eyes with faded pupils" },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Ten whiskers, five on upper jaw and five below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_Ancient] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow eyes that lack pupils, resembling pools of molten gold." },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Twelve whiskers, six on upper jaw and six below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_Wyrm] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow eyes that lack pupils, resembling pools of molten gold." },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Twelve whiskers, six on upper jaw and six below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Gold_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Golden metallic scales" },
                commonEyes: new[] { "Slanted, very narrow eyes that lack pupils, resembling pools of molten gold." },
                commonOther: new[] { "Twin horns, smooth and metallic, coming off the head. Twin neck frills. Twelve whiskers, six on upper jaw and six below, around the mouth that look like the barbels of a catfish." });
            appearances[CreatureConstants.Dragon_Green_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Thin, very small, deep black-green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Thin, very small, black-green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_Young] = GetAppearances(
                commonSkin: new[] { "Thin, small, forest green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_Juvenile] = GetAppearances(
                commonSkin: new[] { "Thin, small, forest green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Forest green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_Adult] = GetAppearances(
                commonSkin: new[] { "Forest green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Large, emerald scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_Old] = GetAppearances(
                commonSkin: new[] { "Large, emerald scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: new[] { "Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings." });
            appearances[CreatureConstants.Dragon_Green_VeryOld] = GetAppearances(
                commonSkin: new[] { "Thick, large, emerald scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges." },
                commonEyes: new[] { "Green eyes" },
                commonOther: ["Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings."]);
            appearances[CreatureConstants.Dragon_Green_Ancient] = GetAppearances(
                commonSkin: ["Thick, huge, olive-green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges."],
                commonEyes: ["Green eyes"],
                commonOther: ["Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings."]);
            appearances[CreatureConstants.Dragon_Green_Wyrm] = GetAppearances(
                commonSkin: ["Thick, huge, olive-green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges."],
                commonEyes: ["Green eyes"],
                commonOther: ["Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings."]);
            appearances[CreatureConstants.Dragon_Green_GreatWyrm] = GetAppearances(
                commonSkin: ["Thick, huge, olive-green scales. Wings have a dappled pattern, darker near the leading edges and lighter toward the trailing edges."],
                commonEyes: ["Green eyes"],
                commonOther: ["Heavily curved jawline and a crest that begins near the eyes and continues down most of the spine. The crest reaches its full height just behind the skull. No external ears, just ear openings."]);
            appearances[CreatureConstants.Dragon_Red_Wyrmling] = GetAppearances(
                commonSkin: ["Small, bright, glossy, scarlet scales"],
                commonEyes: ["Red eyes"],
                commonOther: [ "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears." ]);
            appearances[CreatureConstants.Dragon_Red_VeryYoung] = GetAppearances(
                commonSkin: ["Small, glossy, scarlet scales"],
                commonEyes: ["Red eyes"],
                commonOther: [ "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears." ]);
            appearances[CreatureConstants.Dragon_Red_Young] = GetAppearances(
                commonSkin: ["Small scarlet scales"],
                commonEyes: ["Red eyes"],
                commonOther: [ "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears." ]);
            appearances[CreatureConstants.Dragon_Red_Juvenile] = GetAppearances(
                commonSkin: new[] { "Smooth, dull, deep red scales" },
                commonEyes: new[] { "Red eyes with somewhat-faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears." });
            appearances[CreatureConstants.Dragon_Red_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Smooth, dull, deep red scales" },
                commonEyes: new[] { "Red eyes with somewhat-faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears." });
            appearances[CreatureConstants.Dragon_Red_Adult] = GetAppearances(
                commonSkin: new[] { "Smooth, dull, deep red scales" },
                commonEyes: new[] { "Red eyes with somewhat-faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears." });
            appearances[CreatureConstants.Dragon_Red_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Smooth, dull, deep red scales" },
                commonEyes: new[] { "Red eyes with faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns." });
            appearances[CreatureConstants.Dragon_Red_Old] = GetAppearances(
                commonSkin: new[] { "Large, thick red scales" },
                commonEyes: new[] { "Red eyes with faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns." });
            appearances[CreatureConstants.Dragon_Red_VeryOld] = GetAppearances(
                commonSkin: new[] { "Large, thick red scales" },
                commonEyes: new[] { "Red eyes with faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns." });
            appearances[CreatureConstants.Dragon_Red_Ancient] = GetAppearances(
                commonSkin: new[] { "Large, thick red scales" },
                commonEyes: new[] { "Red eyes with heavily-faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns." });
            appearances[CreatureConstants.Dragon_Red_Wyrm] = GetAppearances(
                commonSkin: new[] { "Large, thick red scales" },
                commonEyes: new[] { "Red eyes with heavily-faded pupils" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns." });
            appearances[CreatureConstants.Dragon_Red_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Large, thick red scales" },
                commonEyes: new[] { "Eyes with no pupils, resembling molten lava orbs" },
                commonOther: new[] { "Two straight, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, bone-white horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, light gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, dark gray horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, faded black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two straight, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns.",
                    "Two twisted, night-black horns sweep back atop the head. Rows of small horns run along the top of the head. Small horns on the cheeks and lower jaw as well. Fringed ears that merge with the cheek horns." });
            appearances[CreatureConstants.Dragon_Silver_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Blue-gray scales with silver highlights" },
                commonEyes: new[] { "Silver eyes" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Blue-gray scales with silver highlights" },
                commonEyes: new[] { "Silver eyes" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_Young] = GetAppearances(
                commonSkin: new[] { "Gray scales with silver highlights" },
                commonEyes: new[] { "Silver eyes" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_Juvenile] = GetAppearances(
                commonSkin: new[] { "Gray scales mottled with metallic silver" },
                commonEyes: new[] { "Silver eyes with somewhat-faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Silver scales" },
                commonEyes: new[] { "Silver eyes with somewhat-faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_Adult] = GetAppearances(
                commonSkin: new[] { "Silver scales" },
                commonEyes: new[] { "Silver eyes with somewhat-faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Silver scales" },
                commonEyes: new[] { "Silver eyes with faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_Old] = GetAppearances(
                commonSkin: new[] { "Bright silver scales" },
                commonEyes: new[] { "Silver eyes with faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_VeryOld] = GetAppearances(
                commonSkin: new[] { "Bright silver scales" },
                commonEyes: new[] { "Silver eyes with faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_Ancient] = GetAppearances(
                commonSkin: new[] { "Bright silver scales" },
                commonEyes: new[] { "Silver eyes with heavily-faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_Wyrm] = GetAppearances(
                commonSkin: new[] { "Bright silver scales" },
                commonEyes: new[] { "Silver eyes with heavily-faded pupils" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_Silver_GreatWyrm] = GetAppearances(
                commonSkin: new[] { "Bright silver scales" },
                commonEyes: new[] { "Eyes without pupils, resembling orbs of mercury" },
                commonOther: new[] { "Smooth, shiny plate for a face. A frill rises high over its head and continues down the neck and back to the tip of the tail. The frill is supported by long spines with dark tips. Two smooth, shiny horns with dark tips." });
            appearances[CreatureConstants.Dragon_White_Wyrmling] = GetAppearances(
                commonSkin: new[] { "Glistening pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Glistening pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_VeryYoung] = GetAppearances(
                commonSkin: new[] { "Glistening pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Glistening pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_Young] = GetAppearances(
                commonSkin: new[] { "Glistening pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Glistening pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_Juvenile] = GetAppearances(
                commonSkin: new[] { "Glistening pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Glistening pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_YoungAdult] = GetAppearances(
                commonSkin: new[] { "Glistening pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Glistening pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_Adult] = GetAppearances(
                commonSkin: new[] { "Pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_MatureAdult] = GetAppearances(
                commonSkin: new[] { "Pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_Old] = GetAppearances(
                commonSkin: new[] { "Pure-white scales. Trailing edge of the wings show a pink tinge",
                    "Pure-white scales. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_VeryOld] = GetAppearances(
                commonSkin: new[] { "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a pink tinge",
                    "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: new[] { "Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed." });
            appearances[CreatureConstants.Dragon_White_Ancient] = GetAppearances(
                commonSkin: new[] { "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a pink tinge",
                    "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a blue tinge" },
                commonEyes: new[] { "Gray eyes" },
                commonOther: ["Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed."]);
            appearances[CreatureConstants.Dragon_White_Wyrm] = GetAppearances(
                commonSkin: [ "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a pink tinge",
                    "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a blue tinge" ],
                commonEyes: ["Gray eyes"],
                commonOther: ["Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed."]);
            appearances[CreatureConstants.Dragon_White_GreatWyrm] = GetAppearances(
                commonSkin: [ "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a pink tinge",
                    "White scales with scales of pale blue and light gray mixed in. Trailing edge of the wings show a blue tinge" ],
                commonEyes: ["Gray eyes"],
                commonOther: ["Small, sharp beak at the nose and a pointed chin. A crest supported by a single backward-curving spine tops the head. Scaled cheeks, spiny dewlaps, and a few protruding teeth when its mouth is closed."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Dragon_turtle
            appearances[CreatureConstants.DragonTurtle][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Green skin, with a golden crest down the center of the head."];
            //Source: https://forgottenrealms.fandom.com/wiki/Dragonne
            appearances[CreatureConstants.Dragonne] = GetAppearances(
                commonSkin: ["Brassy scales"],
                commonHair: ["Thick, coarse mane encircling the face. Large feathery eyebrows"],
                commonEyes: ["Big brass-colored eyes"],
                uncommonEyes: ["Big bronze-colored eyes", "Big copper-colored eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Dretch
            appearances[CreatureConstants.Dretch][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Pale, rubbery, white skin", "Pale, rubbery, beige skin", "Pale, rubbery, blue skin"];
            //Source: https://forgottenrealms.fandom.com/wiki/Drider
            appearances[CreatureConstants.Drider] = GetAppearances(
                commonSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                    appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                uncommonSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                    appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                rareSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                    appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                veryRareSkin: Combine(" on top half, bottom half is ",
                    appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                    appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]),
                commonHair: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon],
                rareEyes: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare],
                veryRareEyes: appearances[CreatureConstants.Elf_Drow][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Dryad
            appearances[CreatureConstants.Dryad][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Delicate features seemingly made from soft wood. Hair looks as if made of leaves and foliage that changes color with the seasons."];
            //Source: https://www.d20srd.org/srd/monsters/eagleGiant.htm
            appearances[CreatureConstants.Eagle_Giant] = appearances[CreatureConstants.Eagle];
            //Source: https://forgottenrealms.fandom.com/wiki/Efreeti
            appearances[CreatureConstants.Efreeti][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Red, burning skin", "Black, burning skin"];
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Elasmosaurus
            appearances[CreatureConstants.Elasmosaurus][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Beige skin"];
            //Source: https://forgottenrealms.fandom.com/wiki/Air_elemental
            appearances[CreatureConstants.Elemental_Air_Small][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "An amorphous, ever-shifting cloud" };
            appearances[CreatureConstants.Elemental_Air_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "An amorphous, ever-shifting cloud" };
            appearances[CreatureConstants.Elemental_Air_Large][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "An amorphous, ever-shifting cloud" };
            appearances[CreatureConstants.Elemental_Air_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "An amorphous, ever-shifting cloud" };
            appearances[CreatureConstants.Elemental_Air_Greater][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "An amorphous, ever-shifting cloud" };
            appearances[CreatureConstants.Elemental_Air_Elder][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "An amorphous, ever-shifting cloud" };
            //Source: https://forgottenrealms.fandom.com/wiki/Earth_elemental
            appearances[CreatureConstants.Elemental_Earth_Small] = GetAppearances(
                commonSkin: new[] { "Vaguely humanoid with club-like arms made of jagged stone and a head made of both dirt and stone" },
                uncommonOther: new[] { "Chunks of minerals set within the stony body", "Chunks of gems set within the stony body",
                    "Chunks of metals set within the stony body" },
                rareOther: new[] { "Chunks of minerals and gems set within the stony body", "Chunks of gems and metals set within the stony body",
                    "Chunks of metals and minerals set within the stony body", "Chunks of minerals, gems, and metals set within the stony body" });
            appearances[CreatureConstants.Elemental_Earth_Medium] = GetAppearances(
                commonSkin: new[] { "Vaguely humanoid with club-like arms made of jagged stone and a head made of both dirt and stone" },
                uncommonOther: new[] { "Chunks of minerals set within the stony body", "Chunks of gems set within the stony body",
                    "Chunks of metals set within the stony body" },
                rareOther: new[] { "Chunks of minerals and gems set within the stony body", "Chunks of gems and metals set within the stony body",
                    "Chunks of metals and minerals set within the stony body", "Chunks of minerals, gems, and metals set within the stony body" });
            appearances[CreatureConstants.Elemental_Earth_Large] = GetAppearances(
                commonSkin: new[] { "Vaguely humanoid with club-like arms made of jagged stone and a head made of both dirt and stone" },
                uncommonOther: new[] { "Chunks of minerals set within the stony body", "Chunks of gems set within the stony body",
                    "Chunks of metals set within the stony body" },
                rareOther: new[] { "Chunks of minerals and gems set within the stony body", "Chunks of gems and metals set within the stony body",
                    "Chunks of metals and minerals set within the stony body", "Chunks of minerals, gems, and metals set within the stony body" });
            appearances[CreatureConstants.Elemental_Earth_Huge] = GetAppearances(
                commonSkin: new[] { "Vaguely humanoid with club-like arms made of jagged stone and a head made of both dirt and stone" },
                uncommonOther: new[] { "Chunks of minerals set within the stony body", "Chunks of gems set within the stony body",
                    "Chunks of metals set within the stony body" },
                rareOther: new[] { "Chunks of minerals and gems set within the stony body", "Chunks of gems and metals set within the stony body",
                    "Chunks of metals and minerals set within the stony body", "Chunks of minerals, gems, and metals set within the stony body" });
            appearances[CreatureConstants.Elemental_Earth_Greater] = GetAppearances(
                commonSkin: new[] { "Vaguely humanoid with club-like arms made of jagged stone and a head made of both dirt and stone" },
                uncommonOther: [ "Chunks of minerals set within the stony body", "Chunks of gems set within the stony body",
                    "Chunks of metals set within the stony body" ],
                rareOther: [ "Chunks of minerals and gems set within the stony body", "Chunks of gems and metals set within the stony body",
                    "Chunks of metals and minerals set within the stony body", "Chunks of minerals, gems, and metals set within the stony body" ]);
            appearances[CreatureConstants.Elemental_Earth_Elder] = GetAppearances(
                commonSkin: ["Vaguely humanoid with club-like arms made of jagged stone and a head made of both dirt and stone"],
                uncommonOther: [ "Chunks of minerals set within the stony body", "Chunks of gems set within the stony body",
                    "Chunks of metals set within the stony body" ],
                rareOther: [ "Chunks of minerals and gems set within the stony body", "Chunks of gems and metals set within the stony body",
                    "Chunks of metals and minerals set within the stony body", "Chunks of minerals, gems, and metals set within the stony body" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Fire_elemental
            appearances[CreatureConstants.Elemental_Fire_Small][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "A column of red flames", "A column of yellow flames", "A column of orange flames",
                "A column of red/yellow flames", "A column of yellow/orange flames", "A column of orange/red flames", "A column of red/yellow/orange flames" ];
            appearances[CreatureConstants.Elemental_Fire_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "A column of red flames", "A column of yellow flames", "A column of orange flames",
                "A column of red/yellow flames", "A column of yellow/orange flames", "A column of orange/red flames", "A column of red/yellow/orange flames" ];
            appearances[CreatureConstants.Elemental_Fire_Large][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "A column of red flames", "A column of yellow flames", "A column of orange flames",
                "A column of red/yellow flames", "A column of yellow/orange flames", "A column of orange/red flames", "A column of red/yellow/orange flames" ];
            appearances[CreatureConstants.Elemental_Fire_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "A column of red flames", "A column of yellow flames", "A column of orange flames",
                "A column of red/yellow flames", "A column of yellow/orange flames", "A column of orange/red flames", "A column of red/yellow/orange flames" ];
            appearances[CreatureConstants.Elemental_Fire_Greater][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "A column of red flames", "A column of yellow flames", "A column of orange flames",
                "A column of red/yellow flames", "A column of yellow/orange flames", "A column of orange/red flames", "A column of red/yellow/orange flames" ];
            appearances[CreatureConstants.Elemental_Fire_Elder][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "A column of red flames", "A column of yellow flames", "A column of orange flames",
                "A column of red/yellow flames", "A column of yellow/orange flames", "A column of orange/red flames", "A column of red/yellow/orange flames" ];
            //Source: https://forgottenrealms.fandom.com/wiki/Water_elemental
            appearances[CreatureConstants.Elemental_Water_Small][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["A cresting wave of water"];
            appearances[CreatureConstants.Elemental_Water_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["A cresting wave of water"];
            appearances[CreatureConstants.Elemental_Water_Large][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["A cresting wave of water"];
            appearances[CreatureConstants.Elemental_Water_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["A cresting wave of water"];
            appearances[CreatureConstants.Elemental_Water_Greater][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["A cresting wave of water"];
            appearances[CreatureConstants.Elemental_Water_Elder][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["A cresting wave of water"];
            //Source: https://en.wikipedia.org/wiki/African_bush_elephant#Description
            appearances[CreatureConstants.Elephant][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Gray skin with scanty hairs"];
            appearances[CreatureConstants.Elephant][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Large, pointed, triangular ears cover the whole shoulder."];
            //Source: https://forgottenrealms.fandom.com/wiki/Aquatic_elf
            appearances[CreatureConstants.Elf_Aquatic] = GetAppearances(
                commonSkin: [ "Deep green skin, mottled and striped with brown", "Blue skin with white stripes and patches",
                    "Deep blue skin with white stripes and patches", "Light blue skin with white stripes and patches" ],
                uncommonSkin: ["Pale silver-green skin"],
                commonEyes: ["Turquoise eyes", "White eyes", "Black eyes", "Blue eyes", "Green eyes"],
                rareEyes: ["Silver eyes"],
                commonHair: [ "Thick, somewhat-stringy black hair", "Thick, somewhat-stringy blue hair", "Thick, somewhat-stringy silver-white hair",
                        "Thick, somewhat-stringy blue-green hair", "Thick, somewhat-stringy emerald green hair" ],
                uncommonHair: [ "Thick, somewhat-stringy red hair",
                    "Thick, somewhat-stringy, rough black hair", "Thick, somewhat-stringy, rough blue hair", "Thick, somewhat-stringy, rough silver-white hair",
                        "Thick, somewhat-stringy, rough blue-green hair", "Thick, somewhat-stringy, rough emerald green hair" ],
                rareHair: ["Thick, somewhat-stringy, rough red hair"],
                commonOther: ["Pointed ears. Long fingers and toes with thick webbing. Gills visible on the neck and ribs"]);
            //Source: https://www.d20srd.org/srd/monsters/devil.htm#erinyes
            //https://forgottenrealms.fandom.com/wiki/Erinyes
            appearances[CreatureConstants.Erinyes] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                rareSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                veryRareSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: ["Red eyes"]);
            //Source: https://www.5esrd.com/database/creature/ethereal-filcher/
            appearances[CreatureConstants.EtherealFilcher][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["One foot. Four arms ending in hands with long, spindly fingers. Looks as if it has two heads, one on a long stalk of a neck and another on its abdomen."];
            //Source: https://www.d20srd.org/srd/monsters/etherealMarauder.htm
            appearances[CreatureConstants.EtherealMarauder][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [ "Bright blue skin", "Blue skin", "Deep blue skin",
                "Bright violet skin", "Violet skin", "Deep violet skin",
                "Bright blue-violet skin", "Blue-violet skin", "Deep blue-violet skin" ];
            //Source: https://forgottenrealms.fandom.com/wiki/Ettercap
            appearances[CreatureConstants.Ettercap][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Grey-purplish skin"];
            appearances[CreatureConstants.Ettercap][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Distended underbelly. Spider-like face, with fangs and multiple eyes. Two sharp, black, chitinous claws instead of hands and feet."];
            //Source: https://forgottenrealms.fandom.com/wiki/Ettin
            appearances[CreatureConstants.Ettin] = GetAppearances(
                commonSkin: [ "Brown skin, crusted over with a thick layer of grime", "Olive skin, crusted over with a thick layer of grime",
                        "Pink skin, crusted over with a thick layer of grime", "Gray skin, crusted over with a thick layer of grime",
                    "Deep brown skin, crusted over with a thick layer of grime", "Deep Olive skin, crusted over with a thick layer of grime",
                        "Deep Pink skin, crusted over with a thick layer of grime", "Deep Gray skin, crusted over with a thick layer of grime",
                    "Pale brown skin, crusted over with a thick layer of grime", "Pale Olive skin, crusted over with a thick layer of grime",
                        "Pale Pink skin, crusted over with a thick layer of grime", "Pale Gray skin, crusted over with a thick layer of grime" ],
                commonHair: appearances[CreatureConstants.Giant_Hill][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                        .Select(LowercaseFirstCharacter)
                        .Select(h => $"Long, stringy {h}")
                    .Concat(appearances[CreatureConstants.Giant_Stone][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                        .Select(LowercaseFirstCharacter)
                        .Select(h => $"Long, stringy {h}")),
                commonEyes: appearances[CreatureConstants.Orc][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(LowercaseFirstCharacter)
                        .Select(e => $"Large, watery {e}"),
                commonOther: ["Two heads. Horrid stench. Shovel jaws with lower canine teeth that protrude out like boar tusks."]);
            //Source: https://beetleidentifications.com/fire-beetle/
            //https://forgottenrealms.fandom.com/wiki/Giant_fire_beetle
            appearances[CreatureConstants.FireBeetle_Giant] = GetAppearances(
                commonSkin: ["Brown color"],
                uncommonSkin: [ "Shimmering green color", "Black color with red markings", "Red color", "Orange color",
                    "Orange color with brown flame-like markings", "Purple color with red markings" ],
                commonOther: ["Red bioluminescent glands above each eye and near the abdomen"]);
            //Source: https://www.d20srd.org/srd/monsters/formian.htm
            appearances[CreatureConstants.FormianWorker] = GetAppearances(
                commonSkin: ["Brownish-red carapace"],
                commonOther: ["Hands only suitable for manual labor"]);
            appearances[CreatureConstants.FormianWarrior] = GetAppearances(
                commonSkin: ["Brownish-red carapace"],
                commonOther: ["Sharp mandibles and poison stingers"]);
            appearances[CreatureConstants.FormianTaskmaster] = GetAppearances(
                commonSkin: ["Brownish-red carapace"],
                commonOther: ["Neither mandible nor mouth"]);
            appearances[CreatureConstants.FormianMyrmarch] = GetAppearances(
                commonSkin: ["Brownish-red carapace"],
                commonOther: ["Claws capable of fine manipulation, like human hands"]);
            appearances[CreatureConstants.FormianQueen] = GetAppearances(
                commonSkin: ["Brownish-red carapace"],
                commonOther: ["Body too large to allow for movement on her weak, atrophied legs"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Frost_worm
            appearances[CreatureConstants.FrostWorm] = GetAppearances(
                commonSkin: ["Blue-white skin", "Very pale blue skin"],
                uncommonSkin: ["Blue-white skin with a purple underbelly", "Very pale blue skin with a purple underbelly"],
                commonOther: ["Huge mandibles. Nodule from which it emits a trilling noise."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Gargoyle - adding my own info for skin tones
            appearances[CreatureConstants.Gargoyle] = GetAppearances(
                commonSkin: [ "Brown stone skin", "Dark brown stone skin", "Light brown stone skin",
                    "Gray stone skin", "Dark gray stone skin","Light gray stone skin",
                    "White stone skin", "Marble stone skin", "Black stone skin" ],
                commonOther: ["Demon-like humanoid"]);
            appearances[CreatureConstants.Gargoyle_Kapoacinth] = GetAppearances(
                commonSkin: [ "Brown stone skin", "Dark brown stone skin", "Light brown stone skin",
                    "Gray stone skin", "Dark gray stone skin","Light gray stone skin",
                    "White stone skin", "Marble stone skin", "Black stone skin" ],
                commonOther: ["Demon-like humanoid"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Gelatinous_cube
            appearances[CreatureConstants.GelatinousCube] = GetAppearances(
                commonSkin: [ "Completely transparent cube, with only a glint on the surface to give away its position",
                    "Completely transparent rhombohedron, with only a glint on the surface to give away its position" ],
                uncommonOther: [ "An object floats within the cube's body", "Two objects float within the cube's body",
                    "Three objects float within the cube's body" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Ghaele
            appearances[CreatureConstants.Ghaele] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                rareSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                veryRareSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                commonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: ["Opalescent pearl eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Ghoul
            appearances[CreatureConstants.Ghoul] = GetAppearances(
                commonSkin: [ "Mottled, decaying, gray hide stretched tight over its emaciated body",
                    "Mottled, decaying, purple hide stretched tight over its emaciated body",
                    "Mottled, decaying, pink hide stretched tight over its emaciated body" ],
                commonHair: new[] { "Almost hairless", "Hairless" },
                commonEyes: new[] { "Sunken eyes, burning like hot coals" });
            appearances[CreatureConstants.Ghoul_Ghast] = GetAppearances(
                commonSkin: new[] { "Mottled, decaying, gray hide stretched tight over its emaciated body",
                    "Mottled, decaying, purple hide stretched tight over its emaciated body",
                    "Mottled, decaying, pink hide stretched tight over its emaciated body",
                    "Mottled, decaying, white hide stretched tight over its emaciated body" },
                commonHair: new[] { "Almost hairless", "Hairless" },
                commonEyes: new[] { "Sunken eyes, burning like hot coals" });
            appearances[CreatureConstants.Ghoul_Lacedon] = GetAppearances(
                commonSkin: new[] { "Mottled, decaying, gray hide stretched tight over its emaciated body",
                    "Mottled, decaying, purple hide stretched tight over its emaciated body",
                    "Mottled, decaying, pink hide stretched tight over its emaciated body",
                    "Mottled, decaying, green hide stretched tight over its emaciated body" },
                commonHair: new[] { "Almost hairless", "Hairless" },
                commonEyes: new[] { "Sunken eyes, burning like hot coals" });
            //Source: https://forgottenrealms.fandom.com/wiki/Cloud_giant
            appearances[CreatureConstants.Giant_Cloud] = GetAppearances(
                commonSkin: new[] { "Milky white skin", "Light sky-blue skin" },
                commonHair: new[] { "Straight silvery-white hair", "Curly silvery-white hair", "Kinky silvery-white hair",
                    "Straight brass hair", "Curly brass hair", "Kinky brass hair"},
                uncommonHair: new[] { "Straight sky-blue hair", "Curly sky-blue hair", "Kinky sky-blue hair" },
                commonEyes: new[] { "Iridescent blue eyes" });
            //Source: https://forgottenrealms.fandom.com/wiki/Fire_giant
            appearances[CreatureConstants.Giant_Fire] = GetAppearances(
                commonSkin: new[] { "Coal-black skin", "Coal-gray skin" },
                commonHair: new[] { "Bright orange hair", "Bright red hair", "Flaming orange hair", "Flaming red hair" },
                commonEyes: new[] { "Red eyes", "Orange eyes", "Fiery red eyes", "Fiery orange eyes" });
            //Source: https://forgottenrealms.fandom.com/wiki/Frost_giant
            appearances[CreatureConstants.Giant_Frost] = GetAppearances(
                commonSkin: ["Ivory-white skin", "Glacial-blue skin"],
                commonHair: ["Blue-white hair", "Light blue hair", "Pale white hair", "Dirty yellow hair"],
                commonEyes: ["Pale blue eyes", "Yellow eyes"]);
            appearances[CreatureConstants.Giant_Frost + GenderConstants.Male] = GetAppearances(
                commonSkin: ["Ivory-white skin", "Glacial-blue skin"],
                commonHair: ["Blue-white hair and beard", "Light blue hair and beard", "Pale white hair and beard", "Dirty yellow hair and beard"],
                commonEyes: ["Pale blue eyes", "Yellow eyes"]);
            appearances[CreatureConstants.Giant_Stone_Elder] = appearances[CreatureConstants.Giant_Stone];
            //Source: https://forgottenrealms.fandom.com/wiki/Storm_giant
            appearances[CreatureConstants.Giant_Storm] = GetAppearances(
                commonSkin: new[] { "Pale light-green skin skin" },
                uncommonSkin: new[] { "Pale purple-gray skin" },
                rareSkin: new[] { "Violet skin" },
                commonHair: new[] { "Dark green hair" },
                uncommonHair: new[] { "Pale purple-gray hair", "Deep violet hair" },
                rareHair: new[] { "Dark blue hair", "Blue-black hair" },
                commonEyes: new[] { "Emerald eyes", "Dark green eyes" },
                uncommonEyes: new[] { "Purple eyes" },
                rareEyes: new[] { "Silvery-gray eyes", "Silver eyes" });
            //Source: https://forgottenrealms.fandom.com/wiki/Gibbering_mouther
            appearances[CreatureConstants.GibberingMouther] = GetAppearances(
                commonSkin: new[] { "Bright red skin", "Pink skin", "White-red skin", "Brown skin", "Brown-green skin" },
                commonOther: new[] { "Large blob of mouth and eyes" });
            //Source: https://forgottenrealms.fandom.com/wiki/Girallon
            appearances[CreatureConstants.Girallon] = GetAppearances(
                commonSkin: new[] { "Beige skin", "Light brown skin", "Pink skin" },
                commonHair: new[] { "Thick white fur" },
                commonOther: new[] { "Four arms. Curved tusks in its mouth." });
            //Source: https://forgottenrealms.fandom.com/wiki/Githyanki
            appearances[CreatureConstants.Githyanki] = GetAppearances(
                commonSkin: new[] { "Rough, leathery yellow skin" },
                commonHair: new[] { "Russet hair in a topknot", "Black hair in a topknot", "Red hair in a topknot" },
                commonEyes: new[] { "Bright black eyes sunk deep in the orbits" },
                commonOther: new[] { "Pointed ears, serrated in the back. Long angular skull, with a small and highly-placed flat nose. Pointed teeth." });
            //Source: https://forgottenrealms.fandom.com/wiki/Githzerai
            appearances[CreatureConstants.Githzerai + GenderConstants.Female] = GetAppearances(
                commonSkin: ["Fair yellow skin", "Pale yellow skin"],
                uncommonSkin: ["Pale yellow skin with green tones", "Pale yellow skin with brown tones"],
                commonHair: Combine(" ",
                    ["Russet hair", "Black hair"],
                    ["in buns", "in braids"]),
                uncommonHair: Combine(" ",
                    ["Gray hair"],
                    ["in buns", "in braids"]),
                commonEyes: ["Deep-set yellow eyes"],
                commonOther: ["Long angular skull, flattened nose, long pointed ears"]);
            appearances[CreatureConstants.Githzerai + GenderConstants.Male] = GetAppearances(
                commonSkin: ["Fair yellow skin", "Pale yellow skin"],
                uncommonSkin: ["Pale yellow skin with green tones", "Pale yellow skin with brown tones"],
                commonHair: [ "Russet hair in braids with carefully-groomed facial hair", "Black hair in braids with carefully-groomed facial hair",
                    "Bald (shaved) with carefully-groomed russet facial hair", "Bald (shaved) with carefully-groomed black facial hair"],
                uncommonHair: ["Gray hair in braids with carefully-groomed facial hair", "Bald (shaved) with carefully-groomed gray facial hair"],
                commonEyes: ["Deep-set yellow eyes"],
                commonOther: ["Long angular skull, flattened nose, long pointed ears"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Glabrezu
            appearances[CreatureConstants.Glabrezu] = GetAppearances(
                commonSkin: ["Wrinkly, deep russet skin", "Wrinkly, russet-black skin", "Wrinkly, black skin", "Wrinkly, pitch-black skin"],
                commonEyes: ["Cold, purple, piercing eyes"],
                commonOther: ["Two pairs of arms: main arms (from the shoulders) are larger and end in pincers, smaller pair (from the stomach) is humanoid with clawed fingers. Goat horns atop the canine head. Numerous fangs in the muzzle."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Gnoll
            appearances[CreatureConstants.Gnoll] = GetAppearances(
                commonSkin: ["Greenish-gray skin"],
                commonHair: [ "Light brown fur, dirty yellow crest-like mane", "Light brown fur, reddish-gray crest-like mane",
                    "Light brown fur, dirty yellow-gray crest-like mane", "Light brown fur, dirty yellow-red crest-like mane",
                    "Light brown fur, yellow-gray crest-like mane", "Light brown fur, yellow-red crest-like mane",
                    "Dark brown fur, dirty yellow crest-like mane", "Dark brown fur, reddish-gray crest-like mane",
                    "Dark brown fur, dirty yellow-gray crest-like mane", "Dark brown fur, dirty yellow-red crest-like mane",
                    "Dark brown fur, yellow-gray crest-like mane", "Dark brown fur, yellow-red crest-like mane" ],
                uncommonHair: [ "Light brown fur marked with spots and stripes, dirty yellow crest-like mane",
                    "Light brown fur marked with spots and stripes, reddish-gray crest-like mane",
                    "Light brown fur marked with spots and stripes, dirty yellow-gray crest-like mane",
                    "Light brown fur marked with spots and stripes, dirty yellow-red crest-like mane",
                    "Light brown fur marked with spots and stripes, yellow-gray crest-like mane",
                    "Light brown fur marked with spots and stripes, yellow-red crest-like mane",
                    "Dark brown fur marked with spots and stripes, dirty yellow crest-like mane",
                    "Dark brown fur marked with spots and stripes, reddish-gray crest-like mane",
                    "Dark brown fur marked with spots and stripes, dirty yellow-gray crest-like mane",
                    "Dark brown fur marked with spots and stripes, dirty yellow-red crest-like mane",
                    "Dark brown fur marked with spots and stripes, yellow-gray crest-like mane",
                    "Dark brown fur marked with spots and stripes, yellow-red crest-like mane" ],
                rareHair: [ "Black fur, dirty yellow crest-like mane", "Black fur, reddish-gray crest-like mane",
                    "Black fur, dirty yellow-gray crest-like mane", "Black fur, dirty yellow-red crest-like mane",
                    "Black fur, yellow-gray crest-like mane", "Black fur, yellow-red crest-like mane",
                    "Black fur marked with fiery orange spots and stripes, dirty yellow crest-like mane",
                    "Black fur marked with fiery orange spots and stripes, reddish-gray crest-like mane",
                    "Black fur marked with fiery orange spots and stripes, dirty yellow-gray crest-like mane",
                    "Black fur marked with fiery orange spots and stripes, dirty yellow-red crest-like mane",
                    "Black fur marked with fiery orange spots and stripes, yellow-gray crest-like mane",
                    "Black fur marked with fiery orange spots and stripes, yellow-red crest-like mane" ],
                commonEyes: ["Green eyes", "Brown eyes"],
                rareEyes: ["Gleaming red eyes"],
                commonOther: ["Hyena-like appearance"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Forest_gnome
            appearances[CreatureConstants.Gnome_Forest] = GetAppearances(
                commonSkin: ["Bark-colored skin", "Earthy brown skin"],
                commonHair: ["Long, free black hair", "Long, free brown hair"],
                uncommonHair: ["Long, free gray hair", "Long, free brown-gray hair"],
                rareHair: ["Long, free white hair"],
                commonEyes: ["Blue eyes", "Brown eyes"]);
            appearances[CreatureConstants.Gnome_Forest + GenderConstants.Male] = GetAppearances(
                commonSkin: ["Bark-colored skin", "Earthy brown skin"],
                commonHair: [ "Long, free black hair", "Long, free brown hair",
                    "Long, free black hair with beard trimmed to fine point", "Long, free brown hair with beard trimmed to fine point",
                    "Long, free black hair with beard trimmed into hornlike spikes", "Long, free brown hair with beard trimmed into hornlike spikes"  ],
                uncommonHair: [ "Long, free gray hair",
                    "Long, free gray hair with beard trimmed to fine point",
                    "Long, free gray hair with beard trimmed into hornlike spikes",
                    "Long, free brown-gray hair",
                    "Long, free brown-gray hair with beard trimmed to fine point",
                    "Long, free brown-gray hair with beard trimmed into hornlike spikes" ],
                rareHair: [ "Long, free white hair",
                    "Long, free white hair with beard trimmed to fine point",
                    "Long, free white hair with beard trimmed into hornlike spikes" ],
                commonEyes: ["Blue eyes", "Brown eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Rock_gnome
            //https://forgottenrealms.fandom.com/wiki/Gnome
            appearances[CreatureConstants.Gnome_Rock] = GetAppearances(
                commonSkin: ["Light tan skin", "Tan skin", "Light brown skin", "Brown skin"],
                commonHair: ["Gray hair", "White hair"],
                commonEyes: ["Glittering opaque orbs of black", "Glitering opaque orbs of blue"],
                uncommonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]);
            appearances[CreatureConstants.Gnome_Rock + GenderConstants.Male] = GetAppearances(
                commonSkin: ["Light tan skin", "Tan skin", "Light brown skin", "Brown skin"],
                commonHair: [ "Gray hair", "White hair",
                    "Gray hair with neatly-trimmed beard", "White hair with neatly-trimmed beard"],
                commonEyes: ["Glittering opaque orbs of black", "Glitering opaque orbs of blue"],
                uncommonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]);
            //Source: https://forgottenrealms.fandom.com/wiki/Deep_gnome
            //https://www.d20srd.org/srd/monsters/gnome.htm
            appearances[CreatureConstants.Gnome_Svirfneblin + GenderConstants.Female] = GetAppearances(
                commonSkin: ["Rocky brown skin", "Rocky brownish-gray skin", "Rocky gray skin"],
                commonHair: ["Stringy gray hair"],
                commonEyes: ["Dark gray eyes", "Black eyes"],
                commonOther: ["Gnarled and wiry"]);
            appearances[CreatureConstants.Gnome_Svirfneblin + GenderConstants.Male] = GetAppearances(
                commonSkin: ["Rocky brown skin", "Rocky brownish-gray skin", "Rocky gray skin"],
                commonHair: ["Bald"],
                commonEyes: ["Dark gray eyes", "Black eyes"],
                commonOther: ["Gnarled and wiry"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Goblin
            //https://www.d20srd.org/srd/monsters/goblin.htm
            appearances[CreatureConstants.Goblin] = GetAppearances(
                commonSkin: ["Yellow skin", "Red skin", "Orange skin", "Light orange skin", "Dark orange skin", "Deep red skin"],
                uncommonSkin: ["Green skin"],
                commonEyes: ["Dull, glazed, yellow eyes", "Dull, glazed, orange eyes", "Dull, glazed, red eyes"],
                commonOther: ["Flat face, small fangs, pointed ears, sloped-back forehead, broad noses"]);
            //Source: https://pathfinderwiki.com/wiki/Clay_golem
            //https://forgottenrealms.fandom.com/wiki/Clay_golem
            appearances[CreatureConstants.Golem_Clay][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Humanoid body made from clay"];
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            //https://forgottenrealms.fandom.com/wiki/Flesh_golem
            appearances[CreatureConstants.Golem_Flesh][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Various decaying humanoid body parts stitched and bolted together"];
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            //https://forgottenrealms.fandom.com/wiki/Iron_golem
            appearances[CreatureConstants.Golem_Iron] = GetAppearances(
                commonSkin: ["Reddish-brown color", "Black color", "Black color with golden markings", "Rusted red color", "Shining steel color"],
                commonOther: [ "Resembles a suit of armor, smooth features",
                    "Resembles a suit of armor, smooth features, symbol carved in the chest",
                    "Resembles a suit of armor, smooth features, designs carved into the limbs" ]);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            //https://forgottenrealms.fandom.com/wiki/Stone_golem
            appearances[CreatureConstants.Golem_Stone] = GetAppearances(
                commonSkin: ["Stone gray color", "Sandy brown color"],
                commonOther: [ "Looks like a carved statue", "Looks like a carved statue, appears as if wearing armor",
                    "Looks like a carved statue, appears as if wearing armor and has a symbol carved on the breastplate",
                    "Looks like a carved statue, symbol carved in the chest",
                    "Looks like a carved statue, designs carved into the limbs" ]);
            //Source: https://www.d20srd.org/srd/monsters/golem.htm
            //https://forgottenrealms.fandom.com/wiki/Stone_golem
            appearances[CreatureConstants.Golem_Stone_Greater] = GetAppearances(
                commonSkin: new[] { "Stone gray color", "Sandy brown color" },
                commonOther: new[] { "Looks like a carved statue", "Looks like a carved statue, stylized by its creator" });
            //Source: https://www.d20srd.org/srd/monsters/gorgon.htm
            //https://forgottenrealms.fandom.com/wiki/Gorgon
            appearances[CreatureConstants.Gorgon] = GetAppearances(
                commonSkin: new[] { "Dusky metallic scales" },
                commonOther: new[] { "Resembles a bull" });
            //Source: https://www.d20srd.org/srd/monsters/ooze.htm#grayOoze
            appearances[CreatureConstants.GrayOoze][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "A thick puddle of gray sludge resembling wet stone",
                "A thick puddle of gray sludge resembling an amorphous rock formation" };
            //Source: https://forgottenrealms.fandom.com/wiki/Gray_render
            appearances[CreatureConstants.GrayRender][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = new[] { "Gray hairless skin" };
            appearances[CreatureConstants.GrayRender][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = new[] { "Six yellow eyes in two rows along the sloped forehead" };
            appearances[CreatureConstants.GrayRender][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Extra long arms drag on the ground." };
            //Source: https://forgottenrealms.fandom.com/wiki/Green_hag
            appearances[CreatureConstants.GreenHag] = GetAppearances(
                commonSkin: new[] { "Rough, bark-like, pallid-green skin with knobby, cancerous protrusions. Warts on the face and exaggerated facial features" },
                uncommonSkin: new[] { "Rough, bark-like, pallid-green skin with knobby, cancerous protrusions. Warts on the face",
                    "Rough, bark-like, pallid-green skin with knobby, cancerous protrusions. Exaggerated facial features",
                    "Rough, bark-like, pallid-green skin with knobby, cancerous protrusions" },
                commonHair: new[] { "Tangled, vine-like, black hair", "Tangled, vine-like, gray hair", "Tangled, vine-like, white hair",
                    "Tangled, vine-like, moldy-olive-green hair" },
                commonEyes: new[] { "Orange eyes", "Amber eyes" },
                commonOther: new[] { "Needle-sharp fangs, black talons covered in filth", "Needle-sharp fangs, yellow talons covered in filth" });
            //Source: https://forgottenrealms.fandom.com/wiki/Grick
            appearances[CreatureConstants.Grick] = GetAppearances(
                commonSkin: new[] { "Rubbery, uniform, dark green skin with a pale underbelly", "Rubbery, uniform, dark blue skin with a pale underbelly",
                    "Rubbery, uniform, blue skin with a pale underbelly" },
                commonOther: new[] { "Snapping beak surrounded by large barbed tentacles" });
            //Source: https://forgottenrealms.fandom.com/wiki/Griffon
            appearances[CreatureConstants.Griffon] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Dusky yellow fur"]),
                commonHair: appearances[CreatureConstants.Eagle][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(["Golden feathers"]),
                commonEyes: appearances[CreatureConstants.Eagle][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: ["Body of a lion. Wings, forelegs, and head of an eagle."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Grig
            appearances[CreatureConstants.Grig] = GetAppearances(
                commonSkin: new[] { "Light blue skin on the torso, brown cricket-like body from the waist-down" },
                commonHair: new[] { "Forest green hair" },
                commonOther: new[] { "Gossamer wings, hairy legs" });
            appearances[CreatureConstants.Grig_WithFiddle] = GetAppearances(
                commonSkin: new[] { "Light blue skin on the torso, brown cricket-like body from the waist-down" },
                commonHair: new[] { "Forest green hair" },
                commonOther: new[] { "Gossamer wings, hairy legs" });
            //Source: https://forgottenrealms.fandom.com/wiki/Grimlock
            appearances[CreatureConstants.Grimlock] = GetAppearances(
                commonSkin: new[] { "Slightly-scaled, thick, scarred, gray skin" },
                uncommonSkin: new[] { "Slightly-scaled, thick gray skin", "Slightly-scaled, thick gray skin with decorative designs scarred into the skin" },
                commonHair: new[] { "Black hair, long and unkempt" },
                commonEyes: new[] { "Completely white eyes", "Face devoid of eyes, skin stretching across where its eye sockets should be" },
                commonOther: new[] { "Sharp teeth" });
            //Source: https://forgottenrealms.fandom.com/wiki/Gynosphinx
            appearances[CreatureConstants.Gynosphinx] = GetAppearances(
                commonHair: new[] { "Tawny fur" },
                commonOther: new[] { "Lion body, falcon wings" });
            //Source: https://forgottenrealms.fandom.com/wiki/Lightfoot_halfling
            //https://www.d20srd.org/srd/monsters/halfling.htm
            appearances[CreatureConstants.Halfling_Deep + GenderConstants.Female] = appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female];
            appearances[CreatureConstants.Halfling_Deep + GenderConstants.Male] = appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male];
            //Source: https://forgottenrealms.fandom.com/wiki/Lightfoot_halfling
            //https://www.d20srd.org/srd/monsters/halfling.htm
            appearances[CreatureConstants.Halfling_Tallfellow + GenderConstants.Female] = appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Female];
            appearances[CreatureConstants.Halfling_Tallfellow + GenderConstants.Male] = appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male];
            //Source: https://www.5esrd.com/database/race/harpy/
            appearances[CreatureConstants.Harpy] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Select(LowercaseFirstCharacter)
                    .Select(h => $"Filthy, tangled, {h} crusted with old, dry blood"),
                commonEyes: ["Coal black eyes"],
                commonOther: [ "Scaly legs, clawed feet, and clawed hands with knotty fingers. Leathery wings. Breasts",
                    "Scaly legs, clawed feet, and clawed hands with knotty fingers. Feathery wings. Breasts" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Hellcat
            appearances[CreatureConstants.Hellcat_Bezekira] = GetAppearances(
                commonSkin: new[] { "Form of a lion", "Form of a tiger-sized domestic cat" },
                commonEyes: new[] { "Crimson eyes", "Crimson eyes, backed by the literal fires of Hell" },
                commonOther: new[] { "Faint, wraith-like outlines with bodies composed of bright light and flickers of fire - only visible in total darkness" });
            //Source: https://forgottenrealms.fandom.com/wiki/Hell_hound
            appearances[CreatureConstants.HellHound] = GetAppearances(
                commonHair: new[] { "Rust-red fur with soot-colored markings", "Red-brown fur with soot-colored markings" },
                commonEyes: new[] { "Glowing red eyes" },
                commonOther: new[] { "Resembles monstrous dog. Soot-colored fangs and tongues. Reek of sulfurous smoke." });
            appearances[CreatureConstants.HellHound_NessianWarhound] = GetAppearances(
                commonHair: new[] { "Coal-black fur with soot-colored markings" },
                commonEyes: new[] { "Glowing red eyes" },
                commonOther: new[] { "Resembles monstrous dog. Soot-colored fangs and tongues. Reek of sulfurous smoke." });
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            appearances[CreatureConstants.Hellwasp_Swarm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "5,000 Hellwasps" };
            //Source: https://forgottenrealms.fandom.com/wiki/Hezrou
            appearances[CreatureConstants.Hezrou] = GetAppearances(
                commonSkin: new[] { "Orange skin", "Yellow-green skin", "Mottled brown skin" },
                commonEyes: new[] { "Glowing yellow eyes", "Glowing red eyes" },
                commonOther: new[] { "Resemble a humanoid toad, wide maw with rows of blunt teeth, long spikes running down the back" });
            //Source: https://forgottenrealms.fandom.com/wiki/Hieracosphinx
            appearances[CreatureConstants.Hieracosphinx] = GetAppearances(
                commonHair: Combine(", ",
                    appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                        .Select(h => $"{h} on the body"),
                    appearances[CreatureConstants.Hawk][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(h => $"head and wings of {h}")),
                commonOther: ["Body of a lion, great feathery wings, head of a hawk"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Hippogriff
            appearances[CreatureConstants.Hippogriff] = GetAppearances(
                commonSkin: new[] { "Ivory beak", "Golden yellow beak" },
                commonHair: new[] { "Russet fur, russet feathers", "Golden tan fur, russet feathers", "Brown fur, russet feathers",
                    "Russet fur, golden tan feathers", "Golden tan fur, golden tan feathers", "Brown fur, golden tan feathers",
                    "Russet fur, brown feathers", "Golden tan fur, brown feathers", "Brown fur, brown feathers" },
                commonOther: [ "Body of a horse, wings and head of a hawk, forelegs ending in sharp talons, hind legs end in hooves",
                    "Body of a horse, wings and head of a eagle, forelegs ending in sharp talons, hind legs end in hooves" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Hobgoblin
            //https://www.d20srd.org/srd/monsters/hobgoblin.htm
            appearances[CreatureConstants.Hobgoblin] = GetAppearances(
                commonSkin: ["Orange skin", "Reddish-brown skin", "Orange-brown skin", "Orange-red skin", "Dark orange skin", "Red-orange skin"],
                commonHair: ["Dark brown hair", "Dark gray hair", "Orange hair", "Red hair", "Dark reddish-brown hair", "Reddish-brown hair"],
                commonEyes: ["Yellow eyes", "Dark brown eyes"],
                commonOther: ["Yellow teeth"]);
            appearances[CreatureConstants.Hobgoblin + GenderConstants.Male] = GetAppearances(
                commonSkin: Combine(", ",
                    ["Orange skin", "Reddish-brown skin", "Orange-brown skin", "Orange-red skin", "Dark orange skin", "Red-orange skin"],
                    ["blue nose", "red nose"]),
                commonHair: ["Dark brown hair", "Dark gray hair", "Orange hair", "Red hair", "Dark reddish-brown hair", "Reddish-brown hair"],
                commonEyes: ["Yellow eyes", "Dark brown eyes"],
                commonOther: ["Yellow teeth"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Homunculus
            appearances[CreatureConstants.Homunculus] = GetAppearances(
                commonSkin: ["Beige skin", "Dark gray skin", "Pale green skin", "Yellow-green skin"],
                commonEyes: ["Yellow eyes", "Green eyes", "Blue eyes", "Blue-green eyes", "Red eyes", "Black eyes"],
                commonOther: ["Leathery wings, large bat-like ears"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Cornugon
            appearances[CreatureConstants.HornedDevil_Cornugon] = GetAppearances(
                commonSkin: ["Repulsive red scales", "Repulsive yellow scales", "Repulsive orange scales", "Repulsive black scales", "Repulsive blue scales"],
                commonEyes: ["Yellow eyes", "Green eyes", "Blue eyes", "Blue-green eyes", "Red eyes", "Black eyes"],
                commonOther: ["Sweeping horns, wings, prehensile, serpentine tail"]);
            //Source: https://www.google.com/search?q=horse+breeds
            //https://equineworld.co.uk/about-horses/horse-colours-and-markings/horse-coat-colours-and-patterns
            appearances[CreatureConstants.Horse_Light][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "Arabian", "Friesian", "Mustang", "Thoroughbread", "Appaloosa", "American Quarter Horse",
                "Dutch Warmblood", "American Paint Horse", "Akhal-Teke", "Turkoman horse", "Mangalarga Marchador", "Percheron", "Criollo",
                "Rahvan", "Kandachime", "Morgan horse", "Icelandic horse", "Cob", "Hanoverian", "Andalusian", "Lipizzan", "Lusitano", "Standardbred", "Falabella",
                "Pure Spanish Breed", "Mongolian", "Trakehner", "Knabstupper", "Konik", "Ferghana", "Marwari", "American Saddlebred", "Missouri Fox Trotter",
                "Belgian Warmblood", "Peruvian paso", "Brumby", "Holsteiner", "Welsh Cob", "Black Forest Horse", "Irish Sport Horse",
                "Spotted (White coat with black spots; black/white mane and tail)", "Spotted (White coat with brown spots; brown/white mane and tail)",
                    "Spotted (White coat with gray spots; Gray/white mane and tail)", "Spotted (White coat with ginger spots; Ginger/white mane and tail)",
                    "Spotted (White coat with golden spots; Golden/white mane and tail)", "Spotted (White coat with cream spots; Cream/white mane and tail)",
                    "Spotted (White coat with light gray spots; Light gray/white mane and tail)",
                    "Spotted (White coat with dark gray spots; Dark gray/white mane and tail)",
                    "Spotted (White coat with creamy golden spots; Creamy golden/white mane and tail)",
                    "Spotted (White coat with light ginger spots; Light ginger/white mane and tail)",
                    "Spotted (White coat with dark ginger spots; Dark ginger/white mane and tail)",
                    "Spotted (Black coat with white spots; White/black mane and tail)",
                    "Spotted (Black coat with brown spots; Brown/black mane and tail)",
                    "Spotted (Black coat with gray spots; Gray/black mane and tail)",
                    "Spotted (Black coat with ginger spots; Ginger/black mane and tail)",
                    "Spotted (Black coat with golden spots; Golden/black mane and tail)",
                    "Spotted (Black coat with cream spots; Cream/black mane and tail)",
                    "Spotted (Black coat with light gray spots; Light gray/black mane and tail)",
                    "Spotted (Black coat with dark gray spots; Dark gray/black mane and tail)",
                    "Spotted (Black coat with creamy golden spots; Creamy golden/black mane and tail)",
                    "Spotted (Black coat with light ginger spots; Light ginger/black mane and tail)",
                    "Spotted (Black coat with dark ginger spots; Dark ginger/black mane and tail)",
                    "Spotted (Brown coat with white spots; White/brown mane and tail)",
                    "Spotted (Brown coat with black spots; Black/brown mane and tail)",
                    "Spotted (Brown coat with gray spots; Gray/brown mane and tail)",
                    "Spotted (Brown coat with ginger spots; Ginger/brown mane and tail)",
                    "Spotted (Brown coat with golden spots; Golden/brown mane and tail)",
                    "Spotted (Brown coat with cream spots; Cream/brown mane and tail)",
                    "Spotted (Brown coat with light gray spots; Light gray/brown mane and tail)",
                    "Spotted (Brown coat with dark gray spots; Dark gray/brown mane and tail)",
                    "Spotted (Brown coat with creamy golden spots; Creamy golden/brown mane and tail)",
                    "Spotted (Brown coat with light ginger spots; Light ginger/brown mane and tail)",
                    "Spotted (Brown coat with dark ginger spots; Dark ginger/brown mane and tail)",
                "Bay (Brown coat; Black legs; Black mane and tail)", "Solid black coat; Black mane and tail",
                "Brown coat; Black lower legs; Light brown mane, tail, and muzzle",
                "Buckskin (Creamy-golden coat with black points; Black mane and tail)", "Buckskin (Rich golden coat with black points; Black mane and tail)",
                "Chestnut (Ginger coat; Ginger mane and tail)", "Chestnut (Ginger coat; Light ginger mane and tail)",
                    "Chestnut (Ginger coat; Dark ginger mane and tail)",
                "Chestnut (Light ginger coat; Ginger mane and tail)", "Chestnut (Light ginger coat; Light ginger mane and tail)",
                    "Chestnut (Light ginger coat; Dark ginger mane and tail)",
                "Chestnut (Dark ginger coat; Ginger mane and tail)", "Chestnut (Dark ginger coat; Light ginger mane and tail)",
                    "Chestnut (Dark ginger coat; Dark ginger mane and tail)",
                "Cremello (Extremely pale white coat, mane, and tail; Blue eyes)", "Cremello (Extremely pale white coat, mane, and tail; Amber eyes)",
                    "Cremello (Extremely pale cream coat, mane, and tail; Blue eyes)", "Cremello (Extremely pale cream coat, mane, and tail; Amber eyes)",
                "Dun (Creamy-golden coat; Black mane and tail; Dark dorsal strip)",
                "Solid white-gray coat, mane, and tail", "Solid light gray coat, mane, and tail", "Solid gray coat, mane, and tail",
                    "Solid dark gray coat, mane, and tail",
                "Overo (Black/white cow-patterned coat; Black mane and tail)", "Overo (Black/brown cow-patterned coat; Black mane and tail)",
                    "Overo (Black/gray cow-patterned coat; Black mane and tail)", "Overo (Black/light gray cow-patterned coat; Black mane and tail)",
                    "Overo (Black/dark gray cow-patterned coat; Black mane and tail)", "Overo (Black/creamy golden cow-patterned coat; Black mane and tail)",
                    "Overo (Black/rich golden cow-patterned coat; Black mane and tail)", "Overo (Black/ginger cow-patterned coat; Black mane and tail)",
                    "Overo (Black/light ginger cow-patterned coat; Black mane and tail)", "Overo (Black/dark ginger cow-patterned coat; Black mane and tail)",
                    "Overo (Black/golden cow-patterned coat; Black mane and tail)",
                "Palomino (Golden coat; White mane and tail)",
                "Roan (White-brown coat; Brown mane, tail, and legs)", "Roan (White-black coat; Black mane, tail, and legs)",
                    "Roan (White-golden coat; Golden mane, tail, and legs)", "Roan (White-ginger coat; Ginger mane, tail, and legs)",
                    "Roan (White-light-ginger coat; Light ginger mane, tail, and legs)", "Roan (White-dark-ginger coat; Dark ginger mane, tail, and legs)",
                    "Roan (White-creamy-golden coat; Creamy golden mane, tail, and legs)", "Roan (White-rich-golden coat; Rich golden mane, tail, and legs)",
                    "Roan (White-gray coat; Creamy golden mane, tail, and legs)", "Roan (White-light-gray coat; Rich golden mane, tail, and legs)",
                    "Roan (White-dark-gray coat; Creamy golden mane, tail, and legs)",
                "Tobiano (White/black cow-patterned coat; White mane and tail)", "Tobiano (White/brown cow-patterned coat; White mane and tail)",
                    "Tobiano (White/gray cow-patterned coat; White mane and tail)", "Tobiano (White/light gray cow-patterned coat; White mane and tail)",
                    "Tobiano (White/dark gray cow-patterned coat; White mane and tail)", "Tobiano (White/creamy golden cow-patterned coat; White mane and tail)",
                    "Tobiano (White/rich golden cow-patterned coat; White mane and tail)", "Tobiano (White/ginger cow-patterned coat; White mane and tail)",
                    "Tobiano (White/light ginger cow-patterned coat; White mane and tail)", "Tobiano (White/dark ginger cow-patterned coat; White mane and tail)",
                    "Tobiano (White/golden cow-patterned coat; White mane and tail)",
            ];
            //Source: https://www.google.com/search?q=draft+horse+breeds
            //https://equineworld.co.uk/about-horses/horse-colours-and-markings/horse-coat-colours-and-patterns
            appearances[CreatureConstants.Horse_Heavy_War] = appearances[CreatureConstants.Horse_Heavy];
            //Source: https://www.google.com/search?q=horse+breeds
            //https://equineworld.co.uk/about-horses/horse-colours-and-markings/horse-coat-colours-and-patterns
            appearances[CreatureConstants.Horse_Light_War] = appearances[CreatureConstants.Horse_Light];
            //Source: https://forgottenrealms.fandom.com/wiki/Hound_archon
            appearances[CreatureConstants.HoundArchon] = GetAppearances(
                commonSkin: ["Red skin", "Brown skin", "Light brown skin", "Tan skin", "Cream-colored skin"],
                commonEyes: ["Black eyes", "Yellow eyes"],
                commonOther: ["Humanoid body; head of a dog"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Howler
            appearances[CreatureConstants.Howler] = GetAppearances(
                commonSkin: ["Light brown scales", "Tan scales"],
                commonHair: ["Tangled, red fur, spreading into mane of trembling quills on the back of the neck and surrounding the face"],
                commonEyes: ["Black eyes", "Yellow eyes"],
                commonOther: ["Bruised, crushed digits ending in claws on the front legs. Back legs end in hooves. Back like an ox. Muzzled, simian face."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Hydra
            appearances[CreatureConstants.Hydra_5Heads] = GetAppearances(
                commonSkin: [ "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" ],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Hydra_6Heads] = GetAppearances(
                commonSkin: [ "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" ],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Hydra_7Heads] = GetAppearances(
                commonSkin: [ "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" ],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Hydra_8Heads] = GetAppearances(
                commonSkin: [ "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" ],
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Hydra_9Heads] = GetAppearances(
                commonSkin: new[] { "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Hydra_10Heads] = GetAppearances(
                commonSkin: new[] { "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Hydra_11Heads] = GetAppearances(
                commonSkin: new[] { "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Hydra_12Heads] = GetAppearances(
                commonSkin: new[] { "Gray-brown skin", "Brown skin", "Dark brown skin", "Dark gray-brown skin",
                    "Yellow-green skin", "Green skin, yellow underbelly", "Tan skin" },
                commonEyes: new[] { "Amber eyes" });
            //Source: https://www.dimensions.com/element/striped-hyena-hyaena-hyaena
            //https://en.wikipedia.org/wiki/Striped_hyena#Description
            appearances[CreatureConstants.Hyena] = GetAppearances(
                commonHair: new[] { "Coarse, bristly, light brown fur with darker brown striping. Mane hair is light gray at the base and black at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is light gray at the base and dark brown at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is white at the base and black at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is white at the base and dark brown at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is light gray at the base and black at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is light gray at the base and dark brown at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is white at the base and black at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is white at the base and dark brown at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is light gray at the base and black at the tips. Muzzle is black",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is light gray at the base and dark brown at the tips. Muzzle is black",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is white at the base and black at the tips. Muzzle is black",
                    "Coarse, bristly, light brown fur with darker brown striping. Mane hair is white at the base and dark brown at the tips. Muzzle is black",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is light gray at the base and black at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is light gray at the base and dark brown at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is white at the base and black at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is white at the base and dark brown at the tips. Muzzle is dark, grayish brown",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is light gray at the base and black at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is light gray at the base and dark brown at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is white at the base and black at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is white at the base and dark brown at the tips. Muzzle is brownish-gray",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is light gray at the base and black at the tips. Muzzle is black",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is light gray at the base and dark brown at the tips. Muzzle is black",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is white at the base and black at the tips. Muzzle is black",
                    "Coarse, bristly, dirty-gray fur with darker gray striping. Mane hair is white at the base and dark brown at the tips. Muzzle is black" },
                commonEyes: new[] { "Small eyes" },
                commonOther: new[] { "Short torso, long legs, thick and elongated neck, large and high-set ears" });
            //Source: https://forgottenrealms.fandom.com/wiki/Gelugon
            appearances[CreatureConstants.IceDevil_Gelugon] = GetAppearances(
                commonSkin: new[] { "Blue and white skin", "Blue skin", "White skin" },
                commonEyes: new[] { "Multifaceted yellow eyes", "Multifaceted green eyes" },
                commonOther: new[] { "Insectoid. Hands and feet end in claws. Long thick tail surrounded in spikes. Set of antennae on the head. Sharp mandibles" });
            //Source: https://forgottenrealms.fandom.com/wiki/Imp
            appearances[CreatureConstants.Imp] = GetAppearances(
                commonSkin: new[] { "Dark red skin" },
                uncommonSkin: new[] { "Red skin", "Light red skin", "Orange skin", "Blue skin", "Brown skin", "Green skin", "Purple skin", "White skin", "Gray skin",
                    "Black skin", "Yellow skin" },
                commonEyes: new[] { "Yellow eyes", "White eyes", "Green eyes" },
                commonOther: new[] { "Leathery bat-like wings. Prehensile tail ending in stinger. Small, sharp, twisting, gleaming-white horns. White fangs." });
            //Source: https://www.d20srd.org/srd/monsters/invisibleStalker.htm
            appearances[CreatureConstants.InvisibleStalker][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Amorphous form. See Invisibility reveals a dim outline of a cloud. True Seeing reveals a roiling cloud of vapor" };
            //Source: https://forgottenrealms.fandom.com/wiki/Janni
            appearances[CreatureConstants.Janni] = GetAppearances(
                commonSkin: ["Skin the color of golden sand", "Earth-colored skin"],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]),
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]),
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]),
                veryRareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]),
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Select(e => $"{e} with supernatural intensity")
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => $"{e} with supernatural intensity")),
                uncommonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Select(e => $"{e} with supernatural intensity")
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                        .Select(e => $"{e} with supernatural intensity")),
                rareEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]
                    .Select(e => $"{e} with supernatural intensity")
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]
                        .Select(e => $"{e} with supernatural intensity")),
                veryRareEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                    .Select(e => $"{e} with supernatural intensity")
                    .Concat(appearances[CreatureConstants.Elf_Half][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                        .Select(e => $"{e} with supernatural intensity")));
            //Source: https://forgottenrealms.fandom.com/wiki/Kobold
            appearances[CreatureConstants.Kobold] = GetAppearances(
                commonSkin: [ "Reddish-brown scaled skin", "Rusty black scaled skin", "Rusty brown scaled skin", "Reddish-black scaled skin",
                    "Brown-black scaled skin", "Black scaled skin" ],
                commonEyes: ["Red eyes", "Burnt orange eyes", "Orange-red eyes", "Burnt red eyes"],
                commonOther: [ "Long, clawed fingers. Jaw like a crocodile. Small white horns on the head. Rat-like tail",
                    "Long, clawed fingers. Jaw like a crocodile. Small tan horns on the head. Rat-like tail" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Kolyarut
            appearances[CreatureConstants.Kolyarut][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Red skin", "Black skin"];
            appearances[CreatureConstants.Kolyarut][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Golden-hued banded-mail armor. Flowing red robe."];
            //Source: https://forgottenrealms.fandom.com/wiki/Kraken
            appearances[CreatureConstants.Kraken] = GetAppearances(
                commonSkin: [ "Brown skin", "Light brown skin", "Reddish-brown skin", "Blue skin", "Purple skin", "Yellow-green skin",
                    "Yellow-green skin with a black head, black tips on tentacles"],
                commonEyes: ["Large yellow eyes", "Large red eyes", "Large blue eyes", "Large green eyes"],
                commonOther: ["Squid-like. Two of the ten tentacles are longer and have deadly barbs. Fins protrude from the upper part of the elongated central body"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Krenshar
            appearances[CreatureConstants.Krenshar] = GetAppearances(
                commonHair: [ "Light brown shaggy fur with dark brown spots, dark-red spiky mane", "Light brown shaggy fur with dark brown spots, dark-brown spiky mane",
                    "Light gray shaggy fur with dark brown spots, brown spiky mane", "Light gray shaggy fur with dark brown spots, dark-brown spiky mane",
                    "Light gray shaggy fur with dark gray spots, brown spiky mane", "Light gray shaggy fur with dark gray spots, dark-brown spiky mane" ],
                commonOther: ["Long, bushy tail. Skin over the skull is a sheath that can be pulled back to reveal the underlying muscles and bones"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Kuo-toa
            appearances[CreatureConstants.KuoToa] = GetAppearances(
                commonSkin: [ "Silver-gray scales", "Gray scales", "Gray scales with yellow undertones", "Gray scales with dark red undertones",
                    "Gray scales with ghostly white undertones" ],
                commonEyes: ["Bulging silver-black eyes"],
                commonOther: ["Broad, distended, partially-webbed hands and feet. Four digits per hand and feet. Bullet-shaped, piscine head. Sharp teeth."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Lamia
            appearances[CreatureConstants.Lamia] = GetAppearances(
                commonSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s} waist-up"),
                    appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s} of a lion waist-down")),
                //TODO: When goats and deer are creatures, update these skins
                uncommonSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s} waist-up"),
                    ["fur of a goat waist-down", "fur of a deer waist-down"]),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common].Select(h => $"{h} on head"),
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon].Select(h => $"{h} on head"),
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]);
            //Source: https://forgottenrealms.fandom.com/wiki/Lammasu
            appearances[CreatureConstants.Lammasu] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                commonHair: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(h => $"{h} on head"),
                    Combine(", ",
                        appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(h => $"{h} on body"),
                        appearances[CreatureConstants.Eagle][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(w => $"wings of {w}"))),
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: ["Human from the waist-up, lion from the waist-down, pair of wings on the back"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Lantern_archon
            appearances[CreatureConstants.LanternArchon][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Sphere of soft, glowing light"];
            //Source: https://forgottenrealms.fandom.com/wiki/Lemure
            appearances[CreatureConstants.Lemure] = GetAppearances(
                commonSkin: new[] { "Orange skin", "Pink skin" },
                commonOther: new[] { "Blob of molten, stinking flesh. Formless, shivering mass below the waist. Head and torso is vaguely humanoid." });
            //Source: https://forgottenrealms.fandom.com/wiki/Leonal
            appearances[CreatureConstants.Leonal] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                commonHair: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]));
            //Source: https://forgottenrealms.fandom.com/wiki/Leopard
            appearances[CreatureConstants.Leopard][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "Yellow fur with rosette-shaped spots" };
            //Source: https://forgottenrealms.fandom.com/wiki/Lillend
            appearances[CreatureConstants.Lillend] = GetAppearances(
                commonSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"Waist-up is {s}"),
                    ["waist-down is bright blue scales", "waist-down is bright purple scales", "waist-down is bright red scales", "waist-down is bright orange scales",
                        "waist-down is bright yellow scales", "waist-down is bright green scales"]),
                commonHair: Combine(", ",
                    ["Bright blue hair", "Bright purple hair", "Bright red hair", "Bright orange hair", "Bright yellow hair", "Bright green hair"],
                    ["bright blue feathers", "bright purple feathers", "bright red feathers", "bright orange feathers", "bright yellow feathers", "bright green feathers"]),
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: ["Torso is humanoid, waist-down is serpentine"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Lion
            //https://www.lionlandscapes.org/lion-vision
            appearances[CreatureConstants.Lion + GenderConstants.Male] = GetAppearances(
                commonHair: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(h => $"{h} with brown mane"),
                commonEyes: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_lion
            appearances[CreatureConstants.Lion_Dire] = GetAppearances(
                commonHair: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                commonEyes: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: ["Bony spine and brow ridges"]);
            appearances[CreatureConstants.Lion_Dire + GenderConstants.Male] = GetAppearances(
                commonHair: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(h => $"{h} with brown mane"),
                commonEyes: appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: ["Bony spine and brow ridges"]);
            //Source: https://www.d20srd.org/srd/monsters/lizard.htm
            //https://www.petcoach.co/article/green-iguana-color-change-causes/
            appearances[CreatureConstants.Lizard] = GetAppearances(
                commonSkin: new[] { "Green skin" },
                uncommonSkin: new[] { "Turquoise skin", "Brown skin", "Reddish-green skin" },
                rareSkin: new[] { "White skin (Albino)" });
            //Source: https://azeah.com/lizards/basic-care-tree-monitors
            appearances[CreatureConstants.Lizard_Monitor] = GetAppearances(
                commonSkin: new[] { "Blue skin", "Black skin", "Green skin", "Yellow skin" });
            //Source: https://forgottenrealms.fandom.com/wiki/Lizardfolk
            appearances[CreatureConstants.Lizardfolk] = GetAppearances(
                commonSkin: new[] { "Green scales", "Black scales", "Gray scales", "Brown scales", "Dark green scales", "Dark brown scales", "Dark gray scales",
                    "Green-brown scales", "Green-gray scales" },
                commonEyes: new[] { "Yellow eyes" },
                commonOther: new[] { "Tail, sharp claws and teeth" });
            //Source: https://forgottenrealms.fandom.com/wiki/Locathah
            appearances[CreatureConstants.Locathah] = GetAppearances(
                commonSkin: new[] { "Fine yellow-green scales with sea-green scales on the stomach", "Fine ochre scales with sea-green scales on the stomach",
                        "Fine yellow scales with sea-green scales on the stomach", "Fine olive-green scales with sea-green scales on the stomach",
                    "Fine yellow-green scales with pale-yellow scales on the stomach", "Fine ochre scales with pale-yellow scales on the stomach",
                        "Fine yellow scales with pale-yellow scales on the stomach", "Fine olive-green scales with pale-yellow scales on the stomach",
                    "Fine yellow-green scales with yellow-green scales on the stomach", "Fine ochre scales with yellow-green scales on the stomach",
                        "Fine yellow scales with yellow-green scales on the stomach", "Fine olive-green scales with yellow-green scales on the stomach",
                    "Fine yellow-green scales with yellow scales on the stomach", "Fine ochre scales with yellow scales on the stomach",
                        "Fine yellow scales with yellow scales on the stomach", "Fine olive-green scales with yellow scales on the stomach",
                    "Fine yellow-green scales with green scales on the stomach", "Fine ochre scales with green scales on the stomach",
                        "Fine yellow scales with green scales on the stomach", "Fine olive-green scales with green scales on the stomach" },
                commonEyes: new[] { "All-black eyes", "All-white eyes" },
                commonOther: new[] { "slender, large fins on arms and legs. Toothless mouth" });
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            appearances[CreatureConstants.Locust_Swarm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "5,000 locusts" };
            //Source: https://forgottenrealms.fandom.com/wiki/Magmin
            appearances[CreatureConstants.Magmin][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Seemingly sculpted from hardened magma. Small bursts of flame constantly erupt from its skin" };
            //Source: https://www.scubalibre-adventures.com/giant-manta-ray-vs-reef-manta/
            appearances[CreatureConstants.MantaRay][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = new[] { "Black top with white underbelly" };
            //Source: https://forgottenrealms.fandom.com/wiki/Manticore
            appearances[CreatureConstants.Manticore] = GetAppearances(
                commonSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s} for the face"),
                    ["Blue Dragon wings", "Black Dragon wings", "Green Dragon wings", "Red Dragon wings", "White Dragon wings"]),
                commonHair: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(s => $"{s} on the head"),
                    appearances[CreatureConstants.Lion][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common].Select(s => $"{s} on the body")),
                commonEyes: ["Yellow eyes"],
                commonOther: ["Tail ends in a mass of deadly spikes. Mouth full of rows and rows of razor-sharp teeth, similar to that of a great white shark"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Marilith
            appearances[CreatureConstants.Marilith] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s}, lower half has green scales"),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: ["Top half is human, bottom half is a serpent"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Marut
            appearances[CreatureConstants.Marut][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Resembles a massive statue made entirely of onyx, humanoid in form but composed of mechanical components. Clad in golden armor." };
            //Source: https://www.d20srd.org/srd/monsters/medusa.htm
            appearances[CreatureConstants.Medusa] = GetAppearances(
                commonSkin: new[] { "Pale gray, scaly skin", "Earthy-brown, scaly skin", "Pale-green, scaly skin" },
                commonHair: new[] { "Hair is mass of 1‑foot-long living venomous serpents" },
                commonEyes: new[] { "Intense red eyes" });
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Velociraptor
            appearances[CreatureConstants.Megaraptor] = GetAppearances(
                commonSkin: new[] { "Dull pale green skin", "Ashy gray skin", "Yellow skin", "Orange skin", "Beige skin", "Stone gray skin", "Dark gray skin" },
                uncommonSkin: new[] { "Earth-brown green skin", "Sandy-yellow skin", "Orange skin with dark orange stripes across the back, yellow underbelly",
                    "White skin with dark gray stripes runnings the length of the body, dark gray hands and feet, dark gray circles around the eyes",
                    "Dark gray skin with a white stripe running the length of the body, yellow underbelly", },
                rareSkin: new[] { "Ashy-gray skin with a blue stripe running the length of the back and down the tail",
                    "Forest-green skin with dark green stripes across the back",
                    "Ashy-gray skin", "Sandy-brown skin with turquoise markings" },
                commonOther: new[] { "Large sickle claw on its foot. Large, sharp teeth." });
            //Source: https://forgottenrealms.fandom.com/wiki/Air_mephit
            appearances[CreatureConstants.Mephit_Air] = GetAppearances(
                commonSkin: new[] { "Pale white skin" },
                commonOther: new[] { "Wings, whirlwinds instead of wings" });
            //Source: https://forgottenrealms.fandom.com/wiki/Dust_mephit
            appearances[CreatureConstants.Mephit_Dust] = GetAppearances(
                commonSkin: new[] { "Rough gray skin", "Flaky gray skin" },
                commonOther: new[] { "Wings, emits dust and grit" });
            //Source: https://forgottenrealms.fandom.com/wiki/Earth_mephit
            appearances[CreatureConstants.Mephit_Earth] = GetAppearances(
                commonSkin: new[] { "Bumpy, rough, earthy skin", "Bumpy, rough, rocky skin", "Bumpy, rough, stony skin" },
                commonOther: new[] { "Wings" });
            //Source: https://forgottenrealms.fandom.com/wiki/Fire_mephit
            appearances[CreatureConstants.Mephit_Fire] = GetAppearances(
                commonSkin: new[] { "Red skin", "Red skin streaked with black" },
                uncommonHair: new[] { "Flaming mustache", "Flaming goatee" },
                commonOther: new[] { "Imp-like wings covered in a halo of fire, body covered in flames" },
                uncommonOther: new[] { "Bat-like wings covered in a halo of fire, body covered in flames" });
            //Source: https://forgottenrealms.fandom.com/wiki/Ice_mephit
            appearances[CreatureConstants.Mephit_Ice] = GetAppearances(
                commonSkin: new[] { "Blue skin", "Blue-white skin", "White skin" },
                commonOther: new[] { "Wings, body seemingly composed of ice" });
            //Source: https://forgottenrealms.fandom.com/wiki/Magma_mephit
            appearances[CreatureConstants.Mephit_Magma] = GetAppearances(
                commonSkin: new[] { "Dull red skin" },
                commonOther: new[] { "Wings, body oozes molten lava (like sweat)" });
            //Source: https://forgottenrealms.fandom.com/wiki/Ooze_mephit
            appearances[CreatureConstants.Mephit_Ooze] = GetAppearances(
                commonSkin: new[] { "Ochre skin", "Green skin" },
                commonEyes: new[] { "Dark green eyes", "Red eyes" },
                commonOther: new[] { "Wings are green bubble membranes, body is mucky and filthy, dripping in slime" });
            //Source: https://forgottenrealms.fandom.com/wiki/Salt_mephit
            appearances[CreatureConstants.Mephit_Salt] = GetAppearances(
                commonSkin: new[] { "Pale beige crystalline skin", "White, grainy, crystalline skin" },
                commonEyes: new[] { "Red eyes" },
                commonOther: new[] { "Wings composed of small cubic crystals" });
            //Source: https://forgottenrealms.fandom.com/wiki/Steam_mephit
            appearances[CreatureConstants.Mephit_Steam] = GetAppearances(
                commonSkin: ["Transluscent white skin", "Billowing steamy skin"],
                uncommonHair: ["Flames like hair on the head", "Flames engulfing the head"],
                commonEyes: ["White eyes", "Yellow eyes"],
                commonOther: [ "Wings composed of steam, body dripping with steamy moisture",
                    "Wings composed of steam, body is billowing steam, a flame roars in the stomach", "Wings composed of flames, body dripping with steamy moisture" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Water_mephit
            appearances[CreatureConstants.Mephit_Water] = GetAppearances(
                commonSkin: ["Blue scaly skin", "Sea-green scaly skin", "Light-blue scaly skin"],
                commonEyes: ["Large, bulbous, black eyes", "Large, bulbous, sea-green eyes", "Large, bulbous, red eyes"],
                commonOther: ["Fin-like wings, body dripping with salt water"]);

            //Source: https://forgottenrealms.fandom.com/wiki/Merfolk
            var commonMerfolkSkin = new[] { "Pale pink skin", "Pale tan skin", "Tan skin" };
            var commonMerfolkScales = new[] { "deep kelly-green scales", "brilliant silver scales", "kelly-green scales", "dark silver scales", "green-silver scales",
                "pale green scales", "brilliant green scales" };
            var commonMerfolkSkinAndScales = Combine(", ",
                commonMerfolkSkin,
                commonMerfolkScales);
            var uncommonMerfolkScales = new[] { "orange scales", "yellow scales", "red scales", "blue scales", "purple scales", "copper scales", "bronze scales",
                "golden scales" };
            var uncommonMerfolkScalesDecoration = new[] { "with darker stripes", "with lighter stripes", "with darker speckles", "with lighter speckles" };
            var commonMerfolkOther = new[] { "Fish-like tail instead of legs, gill slits along the neck, slight webbing between fingers" };
            appearances[CreatureConstants.Merfolk] = GetAppearances(
                commonSkin: commonMerfolkSkinAndScales,
                uncommonSkin: Combine(", ",
                        appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                        commonMerfolkScales
                    ).Concat(Combine(", ",
                        commonMerfolkSkin,
                        uncommonMerfolkScales
                    )).Concat(Combine(" ",
                        commonMerfolkSkinAndScales,
                        uncommonMerfolkScalesDecoration
                    )),
                rareSkin: Combine(", ",
                        appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                        uncommonMerfolkScales
                    ).Concat(Combine(" ",
                        Combine(", ",
                            commonMerfolkSkin,
                            uncommonMerfolkScales
                        ),
                        uncommonMerfolkScalesDecoration
                    )).Concat(Combine(" ",
                        Combine(", ",
                            appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                            uncommonMerfolkScales
                        ),
                        uncommonMerfolkScalesDecoration
                    )),
                commonHair: ["Light brown hair", "Blond hair"],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon].Concat(["Blue hair", "Silver hair"]),
                commonEyes: Combine(" ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                    ["with ice-blue pupils", "with pale-silver pupils"]),
                commonOther: commonMerfolkOther,
                uncommonOther: Combine(", ",
                    commonMerfolkOther,
                    ["tattoos on the arms", "tattoos on the chest", "tattoos on the back", "tattoos on the face", "tattoos on the arms and chest",
                        "tattoos on the arms and back", "tattoos on the arms and face", "tattoos on the chest and back", "tattoos on the chest and face",
                        "tattoos on the back and face", "covered in tattoos"]));
            //Source: https://www.d20srd.org/srd/monsters/mimic.htm
            //https://forgottenrealms.fandom.com/wiki/Mimic
            appearances[CreatureConstants.Mimic][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Shaped like a door", "Shaped like a treasure chest, closed and locked", "Shaped like a rug",
                "Shaped like a bookshelf filled with books", "Shaped like a treasure chest, cracked open and glistening with gold",
                "Shaped like a bed", "Shaped like a wooden chair", "Shaped like a table", "Shaped like a trap door in the floor", "Shaped like a raised stone platform",
                "Shaped like a stone pedestal with a glass orb on top", "Shaped like an altar", "Shaped like a barrel filled with gold",
                "Shaped like a barrel filled with gleaming swords", "Shaped like an empty barrel", "Shaped like a bench", "Shaped like a brazier, unlit",
                "Shaped like a candelabra, unlit", "Shaped like a cauldron", "Shaped like a gate with a solid lock", "Shaped like a chest of drawers",
                "Shaped like a storage chest", "Shaped like a coat rack", "Shaped like a cupboard", "Shaped like a crate", "Shaped like a sofa", "Shaped like a divan",
                "Shaped like a wingback, cushioned chair", "Shaped like a fountain without running water, with a long lever on the side", "Shaped like an iron maiden",
                "Shaped like a ladder", "Shaped like a loom", "Shaped like a stove", "Shaped like an oven", "Shaped like a small set of stairs", "Shaped like a ramp",
                "Shaped like a throne", "Shaped like a washbasin", "Shaped like a bathtub", "Shaped like a wardrobe",
                "Shaped like a weapon rack, filled with gleaming blades", "Shaped like a weapon rack, filled with finely-carved staffs and mallets",
                "Shaped like a gleaming suit of armor (full plate)", "Shaped like a gleaming suit of armor (half plate)", "Shaped like a workbench" };
            //Source: https://forgottenrealms.fandom.com/wiki/Mind_flayer
            appearances[CreatureConstants.MindFlayer] = GetAppearances(
                commonSkin: new[] { "Soft, supple, moist, rubbery, mauve skin", "Soft, supple, moist, rubbery, greenish-violet skin",
                    "Soft, supple, moist, rubbery, pale purple skin", "Soft, supple, moist, rubbery, pale violet skin", "Soft, supple, moist, rubbery, violet skin",
                    "Soft, supple, moist, rubbery, purple skin", "Soft, supple, moist, rubbery, greenish-purple skin",
                    "Soft, supple, moist, rubbery, pale greenish-purple skin" },
                commonEyes: new[] { "Solid white eyes" },
                commonOther: new[] { "Four purplish-black facial tentacles" });
            //Source: https://forgottenrealms.fandom.com/wiki/Minotaur
            appearances[CreatureConstants.Minotaur] = GetAppearances(
                commonSkin: new[] { "Black skin", "Brown skin", "Fair skin", "Pale skin" },
                commonHair: new[] { "Black hair and fur", "Brown hair and fur", "Dark brown hair and fur" },
                commonOther: new[] { "Hooves on legs, dark yellow horns", "Hooves on legs, brown horns", "Hooves on legs, yellow horns",
                    "Hooves on legs, yellow-brown horns", "Hooves on legs, black horns", "Hooves on legs, dark brown horns", "Hooves on legs, white horns",
                    "Hooves on legs, beige horns" },
                uncommonOther: new[] { "Hooves on legs, dark yellow horns, cow-like tail", "Hooves on legs, brown horns, cow-like tail",
                    "Hooves on legs, yellow horns, cow-like tail", "Hooves on legs, yellow-brown horns, cow-like tail", "Hooves on legs, black horns, cow-like tail",
                    "Hooves on legs, dark brown horns, cow-like tail", "Hooves on legs, white horns, cow-like tail", "Hooves on legs, beige horns, cow-like tail" });
            //Source: https://forgottenrealms.fandom.com/wiki/Mohrg
            appearances[CreatureConstants.Mohrg][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Skeletal corpse filled with writhing organs" };
            //Source: https://www.d20srd.org/srd/monsters/monkey.htm
            //https://www.google.com/search?q=colobus+monkey+species
            //https://www.google.com/search?q=capuchin+monkey+species
            appearances[CreatureConstants.Monkey][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Tufted capuchin", "Panamanian white-faced capuchin", "Black-and-white colobus", "Black colobus",
                "Mantled guereza", "Red colobus", "Colombian white-faced capuchin", "White-fronted capuchin", "Black capuchin", "Wedge-capped capuchin",
                "Golden-bellied capuchin", "Kaapori capuchin", "Blond capuchin", "Crested capuchin", "Black-striped capuchin", "Marañón white-fronted capuchin",
                "Azara's capuchin", "Varied white-fronted capuchin", "Spix's white-fronted capuchin", "Santa Marta white-fronted capuchin", "Killikaike", "Acrecebus" };
            //Source: https://www.newworldencyclopedia.org/entry/Mule
            appearances[CreatureConstants.Mule] = GetAppearances(
                commonHair: new[] { "Bay coloration", "Sorrel coloration", "Black fur", "Gray fur" },
                uncommonHair: new[] { "White fur", "Blue roan coloration", "Red roan coloration", "Palomino coloration", "Dun coloration", "Buckskin coloration" },
                rareHair: new[] { "Paint coloration", "Tobiano coloration" });
            //Source: https://www.d20srd.org/srd/monsters/mummy.htm
            appearances[CreatureConstants.Mummy] = GetAppearances(
                commonOther: new[] { "Desiccated husk wrapped in linen burial wraps, pungent cinnamon smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent ginger smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent saltpeter smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent roses smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent myrrh smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent frankincense smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent lye smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent wormwood smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent pomegranate smell" },
                uncommonOther: new[] { "Desiccated husk wrapped in linen burial wraps, pungent upturned-earth smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent decay smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent vegetable rot smell" },
                rareOther: new[] { "Desiccated husk wrapped in linen burial wraps, pungent bread smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent yeast smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent wine smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent vinegar smell",
                    "Desiccated husk wrapped in linen burial wraps, pungent roasted meats smell" });
            //Source: https://forgottenrealms.fandom.com/wiki/Dark_naga
            appearances[CreatureConstants.Naga_Dark] = GetAppearances(
                commonSkin: new[] { "Dark blue scales with black frills", "Dark blue patterned scales", "Dark blue patterned scales with black frills",
                    "Purple scales with black frills", "Purple patterned scales", "Purple patterned scales with black frills" },
                uncommonSkin: new[] { "Dark blue scales", "Purple scales" });
            //Source: https://forgottenrealms.fandom.com/wiki/Guardian_naga
            appearances[CreatureConstants.Naga_Guardian] = GetAppearances(
                commonSkin: new[] { "Green-gold scales with silvery triangles down the spine" },
                uncommonSkin: new[] { "Green-gold scales with silvery triangles down the spine, golden frill runs down the length of the spine" },
                commonEyes: new[] { "Bright golden eyes" },
                uncommonOther: new[] { "Smells of flowers" });
            //Source: https://forgottenrealms.fandom.com/wiki/Spirit_naga
            appearances[CreatureConstants.Naga_Spirit] = GetAppearances(
                commonSkin: new[] { "Black scales with red bands" },
                commonHair: new[] { "Stringy black hair" },
                commonOther: new[] { "Emits the stench of carrion" });
            //Source: https://forgottenrealms.fandom.com/wiki/Water_naga
            appearances[CreatureConstants.Naga_Water] = GetAppearances(
                commonSkin: new[] { "Emerald-green skin, with reticulated rings of light olive and dark emerald green",
                    "Turquoise skin, with reticulated rings of light olive and dark emerald green",
                    "Blue-green skin, with reticulated rings of light olive and dark emerald green",
                    "Green skin, with reticulated rings of light olive and dark emerald green",
                    "Blue skin, with reticulated rings of light olive and dark emerald green",
                    "Emerald-green skin, with reticulated rings of dark brown and gray",
                    "Turquoise skin, with reticulated rings of dark brown and gray",
                    "Blue-green skin, with reticulated rings of dark brown and gray",
                    "Green skin, with reticulated rings of dark brown and gray",
                    "Blue skin, with reticulated rings of dark brown and gray"},
                commonEyes: new[] { "Pale green eyes", "Bright amber eyes" },
                commonOther: new[] { "Fiery red and orange spines" });
            //Source: https://forgottenrealms.fandom.com/wiki/Nalfeshnee
            appearances[CreatureConstants.Nalfeshnee][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Corpulent, combining the most abysmal features of a boar and ape into a bipedal monstrosity. Small, feathered wings." };
            //Source: https://forgottenrealms.fandom.com/wiki/Night_hag
            appearances[CreatureConstants.NightHag] = GetAppearances(
                commonSkin: new[] { "Blue-violet skin", "Light blue-violet skin", "Dark blue-violet skin", "Blue-black skin", "Purple-black skin" },
                commonHair: new[] { "Jet-black hair", "Pitch-black hair" },
                commonEyes: new[] { "Red eyes" },
                commonOther: new[] { "Long, night-black nails. Tattoo-like scars. Grotesque warts, open sores, diseased blisters" },
                uncommonOther: new[] { "Long, night-black nails. Tattoo-like scars" });
            //Source: https://forgottenrealms.fandom.com/wiki/Nightcrawler
            appearances[CreatureConstants.Nightcrawler] = GetAppearances(
                commonSkin: ["Solid black skin"],
                commonOther: ["Black teeth"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Nightmare_(creature)
            appearances[CreatureConstants.Nightmare] = GetAppearances(
                commonSkin: ["Night-black coat"],
                commonHair: ["Red and orange flames for mane and tail"],
                commonEyes: ["Glowing red eyes", "Dark eyes illuminated with flames"],
                commonOther: ["Viper fangs"]);
            appearances[CreatureConstants.Nightmare_Cauchemar] = GetAppearances(
                commonSkin: ["Night-black coat"],
                commonHair: ["Red and orange flames for mane and tail"],
                commonEyes: ["Glowing red eyes", "Dark eyes illuminated with flames"],
                commonOther: ["Viper fangs"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Nightwalker
            appearances[CreatureConstants.Nightwalker][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Body looks as if it is made of shadow-stuff, with a vaguely humanoid form. Smooth and hairless."];
            //Source: https://forgottenrealms.fandom.com/wiki/Nightwing
            appearances[CreatureConstants.Nightwing][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Bat-shaped darkness"];
            //Source: https://forgottenrealms.fandom.com/wiki/Nixie
            appearances[CreatureConstants.Nixie] = GetAppearances(
                commonSkin: ["Pale green skin"],
                commonHair: ["Dark green hair"],
                commonEyes: ["Wide, silver eyes"],
                commonOther: ["Webbed fingers and toes, pointed ears"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Nymph
            appearances[CreatureConstants.Nymph] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(["Vibrant green skin", "Autumnal orange skin", "Autumnal red skin", "Brown skin", "Yellow skin"]),
                commonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Green hair", "Orange hair", "Red hair", "Orange-brown hair", "Red-brown hair"]),
                commonEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(["Yellow eyes", "Green eyes"]));
            //Source: https://forgottenrealms.fandom.com/wiki/Ochre_jelly
            appearances[CreatureConstants.OchreJelly] = GetAppearances(
                commonSkin: ["Dark yellow skin"],
                commonOther: ["Resembles giant amoeba"]);
            //Source: https://kids.nationalgeographic.com/animals/invertebrates/facts/octopus
            appearances[CreatureConstants.Octopus] = GetAppearances(
                commonSkin: ["Gray skin", "Brown skin", "Pink skin", "Blue skin", "Green skin", "Reddish-brown skin"]);
            //Source: https://www.nationalgeographic.com/animals/invertebrates/facts/giant-pacific-octopus
            appearances[CreatureConstants.Octopus_Giant] = GetAppearances(
                commonSkin: ["Gray skin", "Brown skin", "Pink skin", "Blue skin", "Green skin", "Reddish-brown skin"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Ogre
            appearances[CreatureConstants.Ogre] = GetAppearances(
                commonSkin: ["Beige skin", "Pale beige skin", "White skin", "Pale gray skin", "Pale pink skin", "Pink skin", "Red-brown skin"],
                commonEyes: ["Yellow eyes", "Green eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Merrow_(ogre)
            appearances[CreatureConstants.Ogre_Merrow] = GetAppearances(
                commonSkin: ["Green scales"],
                commonHair: ["Seaweed-green, slimy hair"],
                commonEyes: new[] { "Dark green eyes", "Green eyes" },
                commonOther: new[] { "Black teeth and nails, webbed hands and feet, underbite, sloping shoulders, covered in tattoos" },
                rareOther: new[] { "Black teeth and nails, webbed hands and feet, underbite, sloping shoulders, ivory horns, covered in tattoos" });
            //Source: https://forgottenrealms.fandom.com/wiki/Oni_mage
            appearances[CreatureConstants.OgreMage] = GetAppearances(
                commonSkin: new[] { "Turquoise skin", "Pale turquoise skin" },
                commonHair: new[] { "White hair", "Light gray hair", "Black hair", "Blue-black hair" },
                commonEyes: new[] { "Yellow eyes", "Green eyes" },
                commonOther: new[] { "Black nails", "Black nails, white horns", "Black nails, black horns" });
            //Source: https://forgottenrealms.fandom.com/wiki/Half-orc
            appearances[CreatureConstants.Orc_Half] = GetAppearances(
                commonSkin: ["Grayish skin"],
                uncommonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                commonHair: ["Coarse black hair on head and body"],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Coarse dark gray hair on head and body", "Coarse gray hair on head and body"]),
                rareHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Orc][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]),
                commonOther: ["Jutting jaw, sloping forehead, small tusks"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Otyugh
            appearances[CreatureConstants.Otyugh] = GetAppearances(
                commonSkin: new[] { "Disgusting light brown rock-like hide", "Disgusting green rock-like hide", "Disgusting brown rock-like hide" },
                commonOther: new[] { "Bloated, oval-shaped body. Three shuffling elephantine legs. Long tentacles bedecked in rough thorny growths ending in leaf-shaped pads with rows of more sharp spikes. Third tentacle sprouts from the top of the body, forming a vine-like stalk, ending in pair of eyes and olfactory organ. Fang-filled mouth." });
            //Source: https://www.dimensions.com/element/great-horned-owl-bubo-virginianus
            //https://www.allaboutbirds.org/guide/Great_Horned_Owl/species-compare/
            //https://carnivora.net/great-horned-owl-v-powerful-owl-t7257.html
            //https://www.allaboutbirds.org/guide/Long-eared_Owl/species-compare/
            appearances[CreatureConstants.Owl][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Barn Owl", "Great Horned Owl", "Snowy Owl", "Barred Owl", "Great Gray Owl", "Long-Eared Owl", "Powerful Owl" };
            //Source: https://www.d20srd.org/srd/monsters/owlGiant.htm 
            appearances[CreatureConstants.Owl_Giant][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
                appearances[CreatureConstants.Owl][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common].Select(o => $"Resembles a {o}");
            //Source: https://forgottenrealms.fandom.com/wiki/Owlbear
            appearances[CreatureConstants.Owlbear] = GetAppearances(
                commonHair: new[] { "Brown-black feathers and fur", "Yellow-brown feathers and fur", "Brown feathers and fur", "Dark brown feathers and fur" },
                commonEyes: new[] { "Eyes rimmed in red with limpid pupils" },
                commonOther: [ "Bear-like body, avian head, hooked yellow beak", "Bear-like body, avian head, serrated yellow beak",
                    "Bear-like body, avian head, hooked yellow-white beak", "Bear-like body, avian head, serrated yellow-white beak",
                    "Bear-like body, avian head, hooked dull ivory beak", "Bear-like body, avian head, serrated dull ivory beak" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Pegasus
            appearances[CreatureConstants.Pegasus] = GetAppearances(
                commonHair: ["White hair on the body, white feathers for mane and tail, white feathered legs"],
                commonOther: ["Resembles horse with bird-like wings"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Phantom_fungus
            appearances[CreatureConstants.PhantomFungus] = GetAppearances(
                commonSkin: ["Brown skin", "Greenish-brown skin"],
                commonOther: ["Fungal mass supported by four stumpy legs. Cluster of nodules atop the body. Gaping maw lined with row of teeth."]);
            //Source: https://forgottenrealms.fandom.com/wiki/Phase_spider
            appearances[CreatureConstants.PhaseSpider] = GetAppearances(
                commonSkin: ["Dark green/white skin", "Green/white skin", "Blue-green/white skin"],
                commonOther: ["Spindly-legged spider"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Phasm
            appearances[CreatureConstants.Phasm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "In its true form, resembles an amorphous, colorful ooze. However, is a natural shapechanger that can take the form of almost any creature or object." };
            //Source: https://forgottenrealms.fandom.com/wiki/Pit_fiend
            appearances[CreatureConstants.PitFiend] = GetAppearances(
                commonSkin: ["Red scales"],
                commonOther: ["Massive bat-like wings. Huge fangs drop with green venom. Prehensile tail."]);
            var weightedPixieAppearances = GetAppearances(
                commonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(["Green skin"]),
                uncommonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                rareSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                veryRareSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                commonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(["Green hair", "Red hair"]),
                uncommonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                rareHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare],
                veryRareHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare],
                commonEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon],
                rareEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare],
                veryRareEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare],
                commonOther: ["Silvery, moth-like wings"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Pixie
            appearances[CreatureConstants.Pixie] = weightedPixieAppearances;
            appearances[CreatureConstants.Pixie_WithIrresistibleDance] = weightedPixieAppearances;
            //Source: https://www.google.com/search?q=breeds+of+pony
            appearances[CreatureConstants.Pony][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "Shetland Pony", "Connemara Pony", "Pony of the Americas", "New Forest Pony", "Dartmoor Pony", "Exmoor Pony",
                "Fell Pony", "Asturcon", "Burmese Pony" ];
            appearances[CreatureConstants.Pony_War][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = appearances[CreatureConstants.Pony][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common];
            //Source: https://www.google.com/search?q=species+of+porpoise
            appearances[CreatureConstants.Porpoise][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "Harbour porpoise", "Vaquita", "Dall's porpoise", "Spectacles porpoise", "Burmeister's porpoise",
                "Indo-Pacific finless porpoise", "Narrow-ridged finless porpoise" ];
            //Source: https://forgottenrealms.fandom.com/wiki/Giant_praying_mantis
            appearances[CreatureConstants.PrayingMantis_Giant] = GetAppearances(
                commonSkin: [ "Brown coloration", "Green coloration", "Yellow coloration", "Yellow-brown coloration", "Yellow-green coloration",
                    "Brown-green coloration" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Pseudodragon
            appearances[CreatureConstants.Pseudodragon] = GetAppearances(
                commonSkin: ["Brownish-red skin"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Purple_worm
            appearances[CreatureConstants.PurpleWorm] = GetAppearances(
                commonSkin: new[] { "Deep purple skin" });
            //Source: https://forgottenrealms.fandom.com/wiki/Pyrohydra
            appearances[CreatureConstants.Pyrohydra_5Heads] = GetAppearances(
                commonSkin: new[] { "Reddish skin, yellow underbelly" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Pyrohydra_6Heads] = GetAppearances(
                commonSkin: new[] { "Reddish skin, yellow underbelly" },
                commonEyes: new[] { "Amber eyes" });
            appearances[CreatureConstants.Pyrohydra_7Heads] = GetAppearances(
                commonSkin: new[] { "Reddish skin, yellow underbelly" },
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Pyrohydra_8Heads] = GetAppearances(
                commonSkin: ["Reddish skin, yellow underbelly"],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Pyrohydra_9Heads] = GetAppearances(
                commonSkin: ["Reddish skin, yellow underbelly"],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Pyrohydra_10Heads] = GetAppearances(
                commonSkin: ["Reddish skin, yellow underbelly"],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Pyrohydra_11Heads] = GetAppearances(
                commonSkin: ["Reddish skin, yellow underbelly"],
                commonEyes: ["Amber eyes"]);
            appearances[CreatureConstants.Pyrohydra_12Heads] = GetAppearances(
                commonSkin: ["Reddish skin, yellow underbelly"],
                commonEyes: ["Amber eyes"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Quasit
            appearances[CreatureConstants.Quasit] = GetAppearances(
                commonSkin: ["Green skin covered in warts and postules"],
                commonOther: ["Tail covered in barbs, spiky horns"],
                uncommonOther: ["Tail covered in barbs, spiky horns, bat-like wings"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Rakshasa
            appearances[CreatureConstants.Rakshasa] = GetAppearances(
                commonSkin: ["Reddish-orange coat with prominent black stripes and a tiger head"],
                uncommonSkin: ["Jet-black fur and an ape head",
                    "Dark bronze colouration on the scaley skin and a crocodile head",
                    "Brown coloration on the exoskeleton and a mantis head", "Green coloration on the exoskeleton and a mantis head",
                        "Yellow coloration on the exoskeleton and a mantis head", "Yellow-brown coloration on the exoskeleton and a mantis head",
                        "Yellow-green coloration on the exoskeleton and a mantis head", "Brown-green coloration on the exoskeleton and a mantis head"],
                rareSkin: ["Grey fur and an ape head", "Silver fur and an ape head",
                    "Bronze colouration on the scaley skin and a crocodile head", "Very dark bronze colouration on the scaley skin and a crocodile head"],
                commonOther: ["Reversed hands"]);
            //Source: https://dungeonsdragons.fandom.com/wiki/Rast
            appearances[CreatureConstants.Rast][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
            [
                "Central body about the size of a large dog, with 10 spindly spider's legs coming out of it. Each of these legs is tipped with a claw. The head is almost exactly the same size as the central body, and resembles a cross between a snake, a vulture and a goblin (some would say it is almost monkey-like). The central body and legs of the Rast are brown, while the head and claws are rust red.",
                "Central body about the size of a large dog, with 11 spindly spider's legs coming out of it. Each of these legs is tipped with a claw. The head is almost exactly the same size as the central body, and resembles a cross between a snake, a vulture and a goblin (some would say it is almost monkey-like). The central body and legs of the Rast are brown, while the head and claws are rust red.",
                "Central body about the size of a large dog, with 12 spindly spider's legs coming out of it. Each of these legs is tipped with a claw. The head is almost exactly the same size as the central body, and resembles a cross between a snake, a vulture and a goblin (some would say it is almost monkey-like). The central body and legs of the Rast are brown, while the head and claws are rust red.",
                "Central body about the size of a large dog, with 13 spindly spider's legs coming out of it. Each of these legs is tipped with a claw. The head is almost exactly the same size as the central body, and resembles a cross between a snake, a vulture and a goblin (some would say it is almost monkey-like). The central body and legs of the Rast are brown, while the head and claws are rust red.",
                "Central body about the size of a large dog, with 14 spindly spider's legs coming out of it. Each of these legs is tipped with a claw. The head is almost exactly the same size as the central body, and resembles a cross between a snake, a vulture and a goblin (some would say it is almost monkey-like). The central body and legs of the Rast are brown, while the head and claws are rust red.",
                "Central body about the size of a large dog, with 15 spindly spider's legs coming out of it. Each of these legs is tipped with a claw. The head is almost exactly the same size as the central body, and resembles a cross between a snake, a vulture and a goblin (some would say it is almost monkey-like). The central body and legs of the Rast are brown, while the head and claws are rust red.",
            ];
            //Source: https://rockypointrattery.com/rat-colors-%26-markings
            appearances[CreatureConstants.Rat][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [ "Standard coat: short, smooth, and glossy black fur",
                "Rex coat: evenly curled black fur, slightly curled white belly", "Rex coat: evenly curled black fur, slightly curled white belly, curled whiskers",
                "Double rex coat: Very short curly white coat", "Double rex coat: peach fuzz white fur", "Double rex coat: mostly hairless, white fur",
                "Silvermane coat: Soft, silvered fur",
                "Velveteen coat: thick, soft black/brown coat, wavy whiskers",
                "Hairless: thin, bright, almost-transluscent skin, very short curly whiskers", "Hairless: thin, bright, almost-transluscent skin, no whiskers",
                "Agouti: rich chestnut fur with dark slate at the base of the hair, silver-gray belly, black eyes",
                "Silver fawn: rich orange fur, white belly, red eyes",
                "Fawn: rich, deep golden orange fur, slightly-lighter belly, dark ruby eyes",
                "American Blue Agouti: Medium-slate fur with a yellow-tan line on top, silver-blue belly, black eyes",
                "Russian Blue Agouti: Dark steel blue fur with a fawn band on top, silver-blue belly, black eyes",
                "Cinnamon: warm russet brown fur with medium slate at the base of the hair, silver-gray belly, black eyes",
                    "Cinnamon: warm russet brown fur with medium slate at the base of the hair, silver-gray belly, ruby eyes",
                "Apricot Agouti: pale apricot fur with dark slate at the base of the hair, pale cream belly, red eyes",
                "Albino: pure clean white fur, red eyes",
                "Cinnamon red-eyed Marten: warm russet brown fur, light heathering, red eyes",
                "Agouti Silvermane: rich chestnut, soft, silky, sheened fur with silver-white tips, appearing heavily silvered, darker fur around the muzzle and eyes, black eyes",
                "Black: solid black fur, black eyes",
                "Beige: warm grayish-tan fur, dark ruby eyes",
                "Champagne: evenly warm beige fur, red eyes",
                "American Blue: slate blue fur, dark ruby eyes", "American Blue: slate blue fur, black eyes",
                "Russian Blue: dark slate blue fur, black eyes",
                "Mink: even mid gray-brown fur with bluish sheen, ruby eyes", "Mink: even mid gray-brown fur with bluish sheen, black eyes",
                "Silver: off-white fur with a cool ice-blue cast, red eyes",
                "Pink-eyed White: pure clean white fur, red eyes",
                "Himalayan: white fur, red eyes",
                "Black Silvermane: solid black, soft, silky, sheened fur with silver-white tips, looking heavily silvered, darker fur around the muzzle and eyes",
                "Russian Blue Silvermane: Dark steel blue, soft, silky, sheened fur with silver-white tips, looking heavily silvered. Darker fur around the muzzle and eyes.",
                "Black red-eyed Marten: light gray fur with light heathering, lighter fur around the muzzle and eyes, red eyes",
                "Berkshire: solid black fur on top, completely white belly, white feet and tail",
                "Hooded: solid black fur on head, neck, shoulders, and extending down the spine; white fur on sides, legs, and feet",
                "Bareback: solid cream-colored fur on head, neck, and shoulders; white fur on spine, sides, legs, and feet",
                "Mismarked Bareback: solid cream-colored fur on head, neck, and shoulders; small cream spot on the spine; white fur on sides, legs, and feet",
                "Variegated: solid gray fur on head, neck, shoulders; patches and flecks of gray fur on the back side; white fur on sides, legs, and feet; white spot on forehead",
                    "Variegated: solid gray fur on head, neck, shoulders; patches and flecks of gray fur on the back side; white fur on sides, legs, and feet; white star on forehead",
                "Blaze: solid gray fur on head and neck; white fur on body; white markings from nose to forehead",
                "English Irish: solid black fur; white equilateral triangle on chest; front feet white; back feet white on lower half",
                "Irish: solid black fur; white marking on belly; white feet; white-tipped tail",
                "English Irish/Irish: solid black fur; white equilateral triangle on chest; white marking on belly; front feet white; back feet white on lower half; white-tipped tail",
                "Spotted Tabby: light black fur with darker dorsal stripe and spots on sides and face",
                    "Spotted Tabby: light brown fur with darker dorsal stripe and spots on sides and face",
                    "Spotted Tabby: light gray fur with darker dorsal stripe and spots on sides and face",
                "Siamese: medium beige fur; darker points on nose, ears, feet, tail, and tail root; red eyes",
                    "Siamese: medium beige fur; darker points on nose, ears, feet, tail, and tail root; black eyes",
                "Himalayan: white fur; rich dark sepia points on nose, ears, feet, tail, and tail root; red eyes",
                "Dalmation: white fur with numerous ragged black splotches",
            ];
            //Source: https://forgottenrealms.fandom.com/wiki/Giant_rat
            appearances[CreatureConstants.Rat_Dire][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "Coarse brown-black fur" };
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            appearances[CreatureConstants.Rat_Swarm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "300 rats" };
            //Source: https://forgottenrealms.fandom.com/wiki/Raven
            appearances[CreatureConstants.Raven][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "Glossy black feathers" };
            //Source: https://eberron.fandom.com/wiki/Ravid
            appearances[CreatureConstants.Ravid][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Long and serpentine, floating above the ground. One single claw juts out of its body just beneath its head. Pale white, scaley skin." };
            //Source: https://www.d20srd.org/srd/monsters/razorBoar.htm 
            appearances[CreatureConstants.RazorBoar][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = ["Eyes are wild and bloodshot"];
            appearances[CreatureConstants.RazorBoar][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Black-bristled hide marked by hundreds of old scars, tusks are more than three feet long, gleaming like polished ivory and sharper than many swords."];
            //Source: https://forgottenrealms.fandom.com/wiki/Remorhaz
            appearances[CreatureConstants.Remorhaz] = GetAppearances(
                commonSkin: ["Ice-blue scales"],
                commonEyes: ["White eyes"],
                commonOther: ["Tough leathery wings, insect-like head, dozens of legs, back glows red, horns along its body, dagger-like teeth"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Retriever
            appearances[CreatureConstants.Retriever][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Resembles a giant spider with four forelegs ending in large cleaver-like blades, and four rear limbs for walking and carrying most of the weight. Two primary eyes and four smaller eyes that gleam malevolently as they peak out from the carapace."];
            //Source: https://www.d20srd.org/srd/monsters/rhinoceros.htm
            //https://denverzoo.org/animals/black-rhinoceros/
            appearances[CreatureConstants.Rhinoceras] = GetAppearances(
                commonSkin: ["Dark gray skin", "Dark brown skin"],
                commonOther: ["Two horns on the nose, with the anterior horn being larger"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Roc
            appearances[CreatureConstants.Roc] = GetAppearances(
                commonHair: ["Dark brown feathers", "Golden feathers", "Golden-brown feathers", "Dark golden-brown feathers", "Dark golden feathers"],
                rareHair: ["Black feathers", "Red feathers", "White feathers"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Roper
            appearances[CreatureConstants.Roper][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Earthy brown skin, resembling a stalagmite"];
            //Source: https://forgottenrealms.fandom.com/wiki/Rust_monster
            appearances[CreatureConstants.RustMonster][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Four insectlike legs and a squat, humped body. Thick, lumpy hide is yellowish-tan on the belly and rust-red on the back. Tail ends in what looks like a double-ended paddle. Two long antennae come out of its head, one under each eye"];

            //Source: https://forgottenrealms.fandom.com/wiki/Sahuagin
            var commonSahuaginSkin = new[] { "Green skin, darker on the back and lighter in the front",
                "Green skin, darker on the back and lighter in the front, with dark stripes",
                "Green skin, darker on the back and lighter in the front, with dark bands", "Green skin, darker on the back and lighter in the front, with dark spots" };
            var commonSahuaginMutantSkin = new[] { "Black skin on the back and gray in the front", "Black skin on the back and gray in the front, with dark stripes",
                    "Black skin on the back and gray in the front, with dark bands",
                    "Black skin on the back and gray in the front, with dark spots" };
            appearances[CreatureConstants.Sahuagin] = GetAppearances(
                commonSkin: commonSahuaginSkin,
                commonEyes: ["Silvery-black eyes"],
                commonOther: ["Webbed feet and hands; gills; finned tail; additional webbing down the back, on elbows, and where ears would be"]);
            appearances[CreatureConstants.Sahuagin + GenderConstants.Female] = GetAppearances(
                commonSkin: Combine(" ",
                    commonSahuaginSkin,
                    ["with speckled yellow tail", "with spotted yellow tail", "with mottled yellow tail", "with mostly yellow tail", "with fully yellow tail"]),
                commonEyes: ["Silvery-black eyes"],
                commonOther: ["Webbed feet and hands; gills; finned tail; additional webbing down the back, on elbows, and where ears would be"]);
            appearances[CreatureConstants.Sahuagin_Malenti] = GetAppearances(
                commonSkin: ["Greenish-silver skin"],
                commonHair: ["Green hair", "Blue hair"],
                commonEyes: ["Silvery-black eyes"],
                commonOther: ["Resembles an Aquatic Elf"]);
            appearances[CreatureConstants.Sahuagin_Mutant] = GetAppearances(
                commonSkin: commonSahuaginMutantSkin,
                commonEyes: ["Silvery-black eyes"],
                commonOther: ["Webbed feet and hands; gills; finned tail; additional webbing down the back, on elbows, and where ears would be; four arms"]);
            appearances[CreatureConstants.Sahuagin_Mutant + GenderConstants.Female] = GetAppearances(
                commonSkin: Combine(" ",
                    commonSahuaginMutantSkin,
                    ["with speckled yellow tail", "with spotted yellow tail", "with mottled yellow tail", "with mostly yellow tail", "with fully yellow tail"]),
                commonEyes: ["Silvery-black eyes"],
                commonOther: ["Webbed feet and hands; gills; finned tail; additional webbing down the back, on elbows, and where ears would be; four arms"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Salamander
            appearances[CreatureConstants.Salamander_Flamebrother][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Serpentine with a humanoid torso, head, and arms, albeit with spines rising from their head, arms, and spine. Red fiery skin"];
            appearances[CreatureConstants.Salamander_Average][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Serpentine with a humanoid torso, head, and arms, albeit with spines rising from their head, arms, and spine. Red fiery skin"];
            appearances[CreatureConstants.Salamander_Noble][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Serpentine with a humanoid torso, head, and arms, albeit with spines rising from their head, arms, and spine. Red fiery skin"];
            //Source: https://forgottenrealms.fandom.com/wiki/Satyr
            appearances[CreatureConstants.Satyr] = GetAppearances(
                //TODO: When goats are creatures, update these skins
                commonSkin: ["Tan skin, goat fur from the waist-down", "Light brown skin, goat fur from the waist-down", "Brown skin, goat fur from the waist-down",
                    "Lightly tan skin, goat fur from the waist-down"],
                commonHair: ["Red hair", "Chestnut brown hair"],
                commonOther: [ "Small nubs for horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Large curling ram horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Small curling ram horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Small straight horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Large straight horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                ]);
            appearances[CreatureConstants.Satyr_WithPipes] = GetAppearances(
                //TODO: When goats are creatures, update these skins
                commonSkin: ["Tan skin, goat fur from the waist-down", "Light brown skin, goat fur from the waist-down", "Brown skin, goat fur from the waist-down",
                    "Lightly tan skin, goat fur from the waist-down"],
                commonHair: ["Red hair", "Chestnut brown hair"],
                commonOther: [ "Small nubs for horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Large curling ram horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Small curling ram horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Small straight horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                    "Large straight horns, fur-covered lower bodies and legs and cloven hooves similar to those of a goat",
                ]);
            //Source: https://a-z-animals.com/animals/scorpion/
            appearances[CreatureConstants.Scorpion_Monstrous_Tiny] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            appearances[CreatureConstants.Scorpion_Monstrous_Small] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            appearances[CreatureConstants.Scorpion_Monstrous_Medium] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            appearances[CreatureConstants.Scorpion_Monstrous_Large] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            appearances[CreatureConstants.Scorpion_Monstrous_Huge] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            appearances[CreatureConstants.Scorpion_Monstrous_Gargantuan] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            appearances[CreatureConstants.Scorpion_Monstrous_Colossal] = GetAppearances(
                commonSkin: new[] { "Yellow exoskeleton", "Black exoskeleton" },
                uncommonSkin: new[] { "Brown exoskeleton", "Red exoskeleton", "White exoskeleton", "Orange exoskeleton" },
                commonOther: new[] { "Front pincers and the tail with a stinger" });
            //Source: https://forgottenrealms.fandom.com/wiki/Tlincalli
            appearances[CreatureConstants.Scorpionfolk] = GetAppearances(
                commonSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                    ["red skin on the scorpion half"]),
                uncommonSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon],
                    ["red skin on the scorpion half"]),
                rareSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare],
                    ["red skin on the scorpion half"]),
                veryRareSkin: Combine(", ",
                    appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare],
                    ["red skin on the scorpion half"]),
                commonHair: new[] { "Completely hairless" },
                commonEyes: new[] { "Red eyes" },
                commonOther: new[] { "Upper body of a human, lower body of a six-legged giant scorpion; Plates of armor made from bone cover the body from the upper abdomen down. Two large fingers and a thumb for hands. Human-like facial features except for segmented, insect-like eyes." });
            //Source: https://forgottenrealms.fandom.com/wiki/Sea_cat
            appearances[CreatureConstants.SeaCat] = GetAppearances(
                commonSkin: new[] { "Tan fur and fish-like scales", "Green skin and reptilian scales" },
                commonHair: new[] { "Mane of silky hair running from the top of its head and down its back to its tail flukes" },
                commonOther: new[]
                {
                    "Head and forepaws of a lion, but a body and tail similar to those of a fish",
                    "Head and forepaws of a lion, but a body and tail similar to those of a porpoise",
                    "Head and forepaws of a lion, but a body and tail similar to those of a small whale",
                });
            //Source: https://forgottenrealms.fandom.com/wiki/Sea_hag
            appearances[CreatureConstants.SeaHag] = GetAppearances(
                commonSkin: new[] { "Complexion of a rotting fish: pallid yellow skin with patches of green, slimy scales and bony protrusions" },
                commonHair: new[] { "Long, limp, hair that resembles rancid seaweed" },
                commonEyes: new[] { "Piscine eyes, devoid of life, with deep black pupils surrounded by red" },
                commonOther: new[] { "Emaciated appearance, flesh was rife with warts and oozing cankers" });
            //Source: https://forgottenrealms.fandom.com/wiki/Shadow
            appearances[CreatureConstants.Shadow][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "A sentient shadow" };
            appearances[CreatureConstants.Shadow_Greater][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "A sentient shadow" };
            //Source: https://forgottenrealms.fandom.com/wiki/Shadow_mastiff
            appearances[CreatureConstants.ShadowMastiff][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Body of a large dog, head more humanoid in shape but monstrous, with mouth full of vicious teeth, coat is smooth and all black" };
            //Source: https://forgottenrealms.fandom.com/wiki/Shambling_mound
            appearances[CreatureConstants.ShamblingMound][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Vaguely man-shaped mound of rotting vegetation" };
            //Source: https://www.dimensions.com/element/blacktip-shark-carcharhinus-limbatus
            //https://www.enchantedlearning.com/subjects/sharks/anatomy/Size.shtml
            appearances[CreatureConstants.Shark_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Blacktip reef shark", "Mako shark", "Pacific Angelshark", "Whitetip reef shark" };
            //Source: https://www.dimensions.com/element/thresher-shark
            appearances[CreatureConstants.Shark_Large][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Thresher shark", "Blue shark", "Bull shark", "Galapagos shark", "Goblin shark", "Lemon shark",
                "Nurse shark", "Short-finned mako shark", "Tiger shark" };
            //Source: https://www.dimensions.com/element/great-white-shark
            appearances[CreatureConstants.Shark_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Great white shark", "Great Hammerhead shark" };
            appearances[CreatureConstants.Shark_Dire][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Megalodon" };
            //Source: https://forgottenrealms.fandom.com/wiki/Shield_guardian
            appearances[CreatureConstants.ShieldGuardian][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Like a large stick figure made of wood with rocky appendages and metal parts" };
            //Source: https://forgottenrealms.fandom.com/wiki/Shocker_lizard
            appearances[CreatureConstants.ShockerLizard][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = new[] { "Blue skin, with light blue belly" };
            //Source: https://forgottenrealms.fandom.com/wiki/Shrieker
            appearances[CreatureConstants.Shrieker] = GetAppearances(
                commonSkin: appearances[CreatureConstants.VioletFungus][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]);
            //Source: https://forgottenrealms.fandom.com/wiki/Skum
            appearances[CreatureConstants.Skum][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Resembles horrific hybrid of human and fish. Spiny frill on the back, bulbous eyes, and a tail." };
            //Source: https://forgottenrealms.fandom.com/wiki/Blue_slaad
            appearances[CreatureConstants.Slaad_Blue][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Bipedal, roughly-humanoid toad with light, electric blue skin marked by grey streaks. Long snout. Two long, scimitar-like hooks of bone protruding from the back of both hands." };
            //Source: https://forgottenrealms.fandom.com/wiki/Red_slaad
            appearances[CreatureConstants.Slaad_Red][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Bipedal, roughly-humanoid, almost neckless toad with huge, flat head. Skin is mostly dull red with specks of gray, lighter around the underside and darker along the back. Fingers and hands are strangely large and, like the feet, end in claws." };
            //Source: https://forgottenrealms.fandom.com/wiki/Green_slaad
            appearances[CreatureConstants.Slaad_Green][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Bipedal, roughly-humanoid, gangly toad. Skin is mostly a pale, mottled green marked with grey streaks. High forehead, wide mouth, and long claws." };
            //Source: https://forgottenrealms.fandom.com/wiki/Gray_slaad
            appearances[CreatureConstants.Slaad_Gray][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] {
                "Frog-like humanoid. Long, clawed fingers. Gray skin is spotted.",
                "Frog-like humanoid. Long, clawed fingers. Evenly light-gray skin.",
            };
            //Source: https://forgottenrealms.fandom.com/wiki/Death_slaad
            appearances[CreatureConstants.Slaad_Death][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] {
                "Frog-like humanoid. Long, clawed fingers. Gray skin is spotted.",
                "Frog-like humanoid. Long, clawed fingers. Evenly light-gray skin.",
            };
            //Source: https://nationalzoo.si.edu/animals/green-tree-python
            //https://www.britannica.com/animal/boa-constrictor
            appearances[CreatureConstants.Snake_Constrictor][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Diamond shaped heads, irregular scales, vibrant green color",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a silvery-gray background.",
            };
            //Source: https://www.dimensions.com/element/burmese-python-python-bivittatus
            //https://myfwc.com/wildlifehabitats/nonnatives/python/identification/
            //https://nationalzoo.si.edu/animals/green-anaconda
            //https://wwf.panda.org/discover/our_focus/wildlife_practice/profiles/reptiles/anaconda/
            appearances[CreatureConstants.Snake_Constrictor_Giant][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [
                "Tan scales with dark blotches along the back and sides. Dark brown blotches are irregularly shaped, fitting together like a puzzle. Dark wedges on top of head, below head, and behind the eye.",
                "Tan scales with dark blotches along the back and sides. Dark brown blotches are irregularly shaped, fitting together like a giraffe pattern. Dark wedges on top of head, below head, and behind the eye.",
                "Olive-green scales with dark oval spots along the spine and similar spots with yellow centers along the sides. The scales on the belly are yellow and black. Two dark stripes stretch from the eyes angling toward the jaw.",
                "Dark green scales with alternating oval black spots. Similar spots with yellow-ochre centres are along the sides of its body. It has a large narrow head. The eyes and nostrils are set on the top of its head.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown-and-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown-and-gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale brown background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a pale gray background. Markings on the tail are red.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, ovals, and joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of triangles, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown-and-black markings in the shape of joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, ovals, and joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of triangles, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep brown markings in the shape of joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, ovals, and joined ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of triangles, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of ovals, against a silvery-gray background.",
                "Long triangular head, with dark streaks from the eyes to the back of the jaw and another dark streak along the top. Deep black markings in the shape of joined ovals, against a silvery-gray background.", ];
            //Source: https://www.dimensions.com/element/ribbon-snake-thamnophis-saurita
            //https://portal.ct.gov/-/media/DEEP/wildlife/pdf_files/outreach/fact_sheets/ribbonsnakepdf.pdf
            appearances[CreatureConstants.Snake_Viper_Tiny][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] {
                "Ribbonsnake: Boldly patterned with three yellow stripes on a reddish-brown background. A distinct dark band separates each side stripe from the belly. One stripe is centered on the body, while the other 2 stripes run down scale rows 3 and 4.",
                "Ribbonsnake: Boldly patterned with three yellow stripes on a dark reddish-brown background. A distinct dark band separates each side stripe from the belly. One stripe is centered on the body, while the other 2 stripes run down scale rows 3 and 4.",
                "Ribbonsnake: Boldly patterned with three yellow stripes on a reddish-black background. A distinct dark band separates each side stripe from the belly. One stripe is centered on the body, while the other 2 stripes run down scale rows 3 and 4.",
                "Ribbonsnake: Boldly patterned with three yellow stripes on a brownish-black background. A distinct dark band separates each side stripe from the belly. One stripe is centered on the body, while the other 2 stripes run down scale rows 3 and 4.",
                "Ribbonsnake: Boldly patterned with three yellow stripes on a black background. A distinct dark band separates each side stripe from the belly. One stripe is centered on the body, while the other 2 stripes run down scale rows 3 and 4.",
            };
            //Source: https://www.dimensions.com/element/copperhead-agkistrodon-contortrix
            //https://www.chesapeakebay.net/discover/field-guide/entry/copperheads
            appearances[CreatureConstants.Snake_Viper_Small][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] {
                "Copperhead: Broad, unmarked, copper-colored head and a reddish-tan colored body. Hourglass-shaped darker marks on its back. Underside is a pinkish color."
            };
            //Source: https://www.dimensions.com/element/western-diamondback-rattlesnake-crotalus-atrox 
            //https://www.desertmuseum.org/kids/oz/long-fact-sheets/Diamondback%20Rattlesnake.php
            appearances[CreatureConstants.Snake_Viper_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] {
                "Diamondback Rattlesnake: Triangular shaped head. Two dark diagonal lines on each side of its face run from the eyes to its jaws. Dark diamond-shaped pattern along its back. The tail has black and white bands just above the rattles."
            };
            //Source: https://www.dimensions.com/element/black-mamba-dendroaspis-polylepis 
            //https://en.wikipedia.org/wiki/Black_mamba#Description
            //https://www.pretoriazoo.org/animals/green-mamba/
            appearances[CreatureConstants.Snake_Viper_Large] = GetAppearances(
                commonSkin: new[] { "Olive-colored scales, greyish-white underbelly", "Yellowish-brown scales, greyish-white underbelly",
                    "Khaki-colored scales, greyish-white underbelly", "Gunmetal-colored scales, greyish-white underbelly",
                    "Bright green scales laid like paving stones, yellow belly",
                    "Bright green scales laid like paving stones, light green belly",
                    "Bright green scales laid like paving stones, yellow-green belly",
                    "Bright green scales laid like paving stones, light yellow-green belly",
                },
                uncommonSkin: new[] { "Olive-colored scales with a purplish sheen, greyish-white underbelly",
                        "Yellowish-brown scales with a purplish sheen, greyish-white underbelly",
                        "Khaki-colored scales with a purplish sheen, greyish-white underbelly",
                        "Gunmetal-colored scales with a purplish sheen, greyish-white underbelly",
                    "Olive-colored scales with dark mottling toward the posterior, greyish-white underbelly",
                        "Yellowish-brown scales with dark mottling toward the posterior, greyish-white underbelly",
                        "Khaki-colored scales with dark mottling toward the posterior, greyish-white underbelly",
                        "Gunmetal-colored scales with dark mottling toward the posterior, greyish-white underbelly",
                    "Olive-colored scales with dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                        "Yellowish-brown scales with dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                        "Khaki-colored scales with dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                        "Gunmetal-colored scales with dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                },
                rareSkin: new[] { "Black scales, greyish-white underbelly", "Black scales with a purplish sheen, greyish-white underbelly",
                    "Olive-colored scales with a purplish sheen and dark mottling toward the posterior, greyish-white underbelly",
                        "Yellowish-brown scales with a purplish sheen and dark mottling toward the posterior, greyish-white underbelly",
                        "Khaki-colored scales with a purplish sheen and dark mottling toward the posterior, greyish-white underbelly",
                        "Gunmetal-colored scales with a purplish sheen and dark mottling toward the posterior, greyish-white underbelly",
                    "Olive-colored scales with a purplish sheen and dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                        "Yellowish-brown scales with a purplish sheen and dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                        "Khaki-colored scales with a purplish sheen and dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                        "Gunmetal-colored scales with a purplish sheen and dark mottling toward the posterior in the form of diagonal crossbands, greyish-white underbelly",
                },
                commonEyes: new[] { "Medium-sized eyes, grayish-brown irises surrounded by silvery-white",
                    "Medium-sized eyes, dark black irises surrounded by silvery-white",
                    "Medium-sized eyes, dark gray irises surrounded by silvery-white", "Medium-sized eyes, gray irises surrounded by silvery-white",
                    "Medium-sized eyes, light gray irises surrounded by silvery-white", "Medium-sized eyes, brown irises surrounded by silvery-white",
                    "Medium-sized eyes, dark brown irises surrounded by silvery-white",
                    "Medium-sized eyes, dark grayish-brown irises surrounded by silvery-white",
                    "Medium-sized eyes, grayish-brown irises surrounded by yellow",
                    "Medium-sized eyes, dark black irises surrounded by yellow",
                    "Medium-sized eyes, dark gray irises surrounded by yellow", "Medium-sized eyes, gray irises surrounded by yellow",
                    "Medium-sized eyes, light gray irises surrounded by yellow", "Medium-sized eyes, brown irises surrounded by yellow",
                    "Medium-sized eyes, dark brown irises surrounded by yellow",
                    "Medium-sized eyes, dark grayish-brown irises surrounded by yellow"},
                commonOther: new[] { "Mamba: Coffin-shaped head with somewhat-pronounced brow ridge" }
            );
            //Source: https://www.dimensions.com/element/king-cobra-ophiophagus-hannah
            //https://nationalzoo.si.edu/animals/king-cobra
            appearances[CreatureConstants.Snake_Viper_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[]
            {
                "King Cobra: Yellow scales. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Green scales. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Green scales. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Green scales. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Green scales. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: brown scales. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: brown scales. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: brown scales. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: brown scales. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: black scales. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: black scales. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: black scales. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: black scales. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Yellow scales with darker yellowish crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales with darker yellowish crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales with darker yellowish crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales with darker yellowish crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Yellow scales with darker yellowish chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales with darker yellowish chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales with darker yellowish chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales with darker yellowish chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Yellow scales with lighter yellowish crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales with lighter yellowish crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales with lighter yellowish crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales with lighter yellowish crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Yellow scales with lighter yellowish chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales with lighter yellowish chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales with lighter yellowish chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales with lighter yellowish chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Yellow scales with white crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales with white crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales with white crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales with white crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Yellow scales with white chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Yellow scales with white chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Yellow scales with white chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Yellow scales with white chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Green scales with yellowish crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Green scales with yellowish crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Green scales with yellowish crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Green scales with yellowish crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Green scales with yellowish chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Green scales with yellowish chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Green scales with yellowish chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Green scales with yellowish chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Green scales with white crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Green scales with white crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Green scales with white crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Green scales with white crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: Green scales with white chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: Green scales with white chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: Green scales with white chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: Green scales with white chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: brown scales with yellowish crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: brown scales with yellowish crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: brown scales with yellowish crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: brown scales with yellowish crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: brown scales with yellowish chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: brown scales with yellowish chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: brown scales with yellowish chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: brown scales with yellowish chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: brown scales with white crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: brown scales with white crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: brown scales with white crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: brown scales with white crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: brown scales with white chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: brown scales with white chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: brown scales with white chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: brown scales with white chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: black scales with yellowish crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: black scales with yellowish crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: black scales with yellowish crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: black scales with yellowish crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: black scales with yellowish chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: black scales with yellowish chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: black scales with yellowish chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: black scales with yellowish chevrons. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: black scales with white crossbars. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: black scales with white crossbars. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: black scales with white crossbars. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: black scales with white crossbars. The belly is ornamented with bars. The throat is cream-colored.",
                "King Cobra: black scales with white chevrons. The belly is uniform in color. The throat is light yellow.",
                "King Cobra: black scales with white chevrons. The belly is uniform in color. The throat is cream-colored.",
                "King Cobra: black scales with white chevrons. The belly is ornamented with bars. The throat is light yellow.",
                "King Cobra: black scales with white chevrons. The belly is ornamented with bars. The throat is cream-colored.",
            };
            //Source: https://forgottenrealms.fandom.com/wiki/Specter
            appearances[CreatureConstants.Spectre][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] =
            [
                "Humanoid, with a mostly transparent and faintly luminous form.",
                "Humanoid, with a mostly transparent and faintly luminous form. The injuries that caused its violent death are visible.",
            ];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Small
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Small] = appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Medium] = appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Large] = appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Huge
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Huge] = appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Gargantuan
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Gargantuan] = appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Colossal
            appearances[CreatureConstants.Spider_Monstrous_Hunter_Colossal] = appearances[CreatureConstants.Spider_Monstrous_Hunter_Tiny];
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Tiny
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Tiny][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Small
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Small][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Medium
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Medium][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Large
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Large][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Huge
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Huge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Gargantuan
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.realmshelps.net/monsters/block/Monstrous_Spider,_Colossal
            appearances[CreatureConstants.Spider_Monstrous_WebSpinner_Colossal][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "European garden spider", "Yellow garden spider", "Golden silk orb-weaver",
                "Sydney funnel-web spider", "Filmy dome spider", "Diving bell spider", "Wheel spider", "Wasp spider", "Barn spider", "Cat-faced spider" };
            //Source: https://www.d20srd.org/srd/monsters/swarm.htm
            appearances[CreatureConstants.Spider_Swarm][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "1,500 spiders" };
            //Source: https://www.d20srd.org/srd/monsters/spiderEater.htm
            appearances[CreatureConstants.SpiderEater][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Resembles a wasp the size of a horse, but with the head of a spider and two long appendages ending in pincers." };
            //Source: https://www.google.com/search?q=squid+species
            appearances[CreatureConstants.Squid][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Humboldt squid", "European squid", "Japanese flying squid", "Caribbean reef squid", "Bigfin reef squid",
                "Vampire squid", "Longfin inshore squid", "Neon flying squid", "Robust clubhook squid", "Angel clubhook squid", "European flying squid"
            };
            appearances[CreatureConstants.Squid_Giant][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Colossal squid" };
            //Source: https://www.uky.edu/Ag/CritterFiles/casefile/insects/beetles/hercules/hercules.htm
            appearances[CreatureConstants.StagBeetle_Giant] = GetAppearances(
                commonSkin: new[] { "Green exoskeleton mottled with black spots", "Gray exoskeleton mottled with black spots",
                    "Tan exoskeleton mottled with black spots", "Black exoskeleton", "Black iridescent exoskeleton" },
                rareSkin: new[] { "Green exoskeleton", "Gray exoskeleton", "Tan exoskeleton" }
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Stirge
            appearances[CreatureConstants.Stirge][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] {
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short and furry body in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short and furry body in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short body covered in feathers in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short body covered in feathers in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short and furry body in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short and furry body in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short body covered in feathers in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short body covered in feathers in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short and furry body in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short and furry body in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short body covered in feathers in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between a large bat and an oversized mosquito. Short body covered in feathers in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short and furry body in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short and furry body in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short body covered in feathers in shades of rusty red. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
                "Resembles a monstrous cross between an oversized mosquito and a bird. Short body covered in feathers in shades of reddish brown. Yellowish eyes. Wings are membranous and bat-like, interlaced with thin-walled blood vessels. Four eight-jointed legs ending in sharp pincers of a similar yellowish hue to the eyes. Long, sharp needle-like proboscis, pink at the tip and fades to grey at the base.",
            };
            //Source: https://forgottenrealms.fandom.com/wiki/Succubus
            appearances[CreatureConstants.Succubus] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => s.Replace("skin", "flawless skin")),
                commonHair: new[] { "Red hair", "Raven-black hair" },
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common].Select(s => s.Replace("eyes", "smoldering eyes")),
                commonOther: new[] { "Clawed fingers, large black bat-like wings", "Clawed fingers, large dark red bat-like wings",
                    "Clawed fingers, large dark gray bat-like wings", "Clawed fingers, large dark green bat-like wings", "Clawed fingers, large dark blue bat-like wings",
                    "Clawed fingers, large dark purple bat-like wings", "Clawed fingers, large reddish bat-like wings"
                },
                uncommonOther: new[] { "Clawed fingers, large black bat-like wings, small horns", "Clawed fingers, large dark red bat-like wings, small horns",
                    "Clawed fingers, large dark gray bat-like wings, small horns", "Clawed fingers, large dark green bat-like wings, small horns",
                    "Clawed fingers, large dark blue bat-like wings, small horns",
                    "Clawed fingers, large dark purple bat-like wings, small horns", "Clawed fingers, large reddish bat-like wings, small horns",
                    "Clawed fingers, large black bat-like wings, tail", "Clawed fingers, large dark red bat-like wings, tail",
                    "Clawed fingers, large dark gray bat-like wings, tail", "Clawed fingers, large dark green bat-like wings, tail",
                    "Clawed fingers, large dark blue bat-like wings, tail",
                    "Clawed fingers, large dark purple bat-like wings, tail", "Clawed fingers, large reddish bat-like wings, tail",
                    "Clawed fingers, large black bat-like wings, small horns, tail", "Clawed fingers, large dark red bat-like wings, small horns, tail",
                    "Clawed fingers, large dark gray bat-like wings, small horns, tail", "Clawed fingers, large dark green bat-like wings, small horns, tail",
                    "Clawed fingers, large dark blue bat-like wings, small horns, tail",
                    "Clawed fingers, large dark purple bat-like wings, small horns, tail", "Clawed fingers, large reddish bat-like wings, small horns, tail"
                }
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Tarrasque
            appearances[CreatureConstants.Tarrasque][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Two long horns extend from its forehead; thick carapace; mighty tail; many spikes cover its large body; wide, toothy maw. Two small eyes."];
            //Source: https://forgottenrealms.fandom.com/wiki/Tendriculos
            appearances[CreatureConstants.Tendriculos][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Mound of vegetation, supported by various branches and vines. Large opening filled with sharp branches and horns, acting sort of like teeth."];
            //Source: https://forgottenrealms.fandom.com/wiki/Thoqqua
            appearances[CreatureConstants.Thoqqua][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Worm-like, made of earth and fire. brown and red skin. Glowing, molten-hot beak."];
            //Source: https://forgottenrealms.fandom.com/wiki/Tiefling
            appearances[CreatureConstants.Tiefling] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => s.Replace("skin", "furred skin"))
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => s.Replace("skin", "leathery skin")))
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => s.Replace("skin", "scaly skin")))
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common].Select(s => $"{s}, unusually warm"))
                    .Concat(["Light red skin", "Red skin", "Dark red skin"]),
                rareSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => s.Replace("skin", "furred skin, unusually warm"))
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => s.Replace("skin", "leathery skin, unusually warm")))
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => s.Replace("skin", "scaly skin, unusually warm")))
                    .Concat(["Light red skin, unusually warm", "Red skin, unusually warm", "Dark red skin, unusually warm",
                        "Light red furred skin", "Red furred skin", "Dark red furred skin",
                        "Light red leathery skin", "Red leathery skin", "Dark red leathery skin",
                        "Light red scaly skin", "Red scaly skin", "Dark red scaly skin",
                        "Light red furred skin, unusually warm", "Red furred skin, unusually warm", "Dark red furred skin, unusually warm",
                        "Light red leathery skin, unusually warm", "Red leathery skin, unusually warm", "Dark red leathery skin, unusually warm",
                        "Light red scaly skin, unusually warm", "Red scaly skin, unusually warm", "Dark red scaly skin, unusually warm"]),
                commonHair: ["Red hair", "Brown hair", "Black hair", "Dark blue hair", "Purple hair"],
                uncommonHair: ["Pale white hair"],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common].Select(e => e.Replace("eyes", "feline eyes"))
                    .Concat(["Solid black eyes with no pupils", "Solid gold eyes with no pupils", "Solid red eyes with no pupils",
                        "Solid silver eyes with no pupils", "Solid white eyes with no pupils"]),
                commonOther: ["Horns", "Prehensile tail", "Non-prehensile tail", "Pointed teeth"],
                uncommonOther: [ "No other demonic qualities",
                    "Sulfurous odor", "Cloven feet",
                    "Horns and prehensile tail", "Horns and non-prehensile tail", "Horns and pointed teeth",
                    "Prehensile tail and pointed teeth", "Non-prehensile tail and pointed teeth",
                    "Forked tongue", "Odor of brimstone", "Goat-like legs", "Hooves",
                    "Antlers",
                ],
                rareOther: [ "Horns, prehensile tail, and pointed teeth", "Horns, non-prehensile tail, and pointed teeth",
                    "Horns and sulfurous odor", "Horns and cloven feet", "Horns, sulfurous odor, and cloven feet",
                    "Prehensile tail and sulfurous odor", "Prehensile tail and cloven feet", "Prehensile tail, sulfurous odor, and cloven feet",
                    "Non-prehensile tail and sulfurous odor", "Non-prehensile tail and cloven feet", "Non-prehensile tail, sulfurous odor, and cloven feet",
                    "Pointed teeth and sulfurous odor", "Pointed teeth and cloven feet", "Pointed teeth, sulfurous odor, and cloven feet",
                    "Sulfurous oder and cloven feet",
                    "Horns, prehensile tail, and sulfurous odor", "Horns, prehensile tail, and cloven feet", "Horns, prehensile tail, sulfurous odor, and cloven feet",
                    "Horns, non-prehensile tail, and sulfurous odor", "Horns, non-prehensile tail, and cloven feet",
                        "Horns, non-prehensile tail, sulfurous odor, and cloven feet",
                    "Horns, pointed teeth, and sulfurous odor", "Horns, pointed teeth, and cloven feet", "Horns, pointed teeth, sulfurous odor, and cloven feet",
                    "Prehensile tail, pointed teeth, and sulfurous odor", "Prehensile tail, pointed teeth, and cloven feet",
                        "Prehensile tail, pointed teeth, sulfurous odor, and cloven feet",
                    "Non-prehensile tail, pointed teeth, and sulfurous odor", "Non-prehensile tail, pointed teeth, and cloven feet",
                        "Non-prehensile tail, pointed teeth, sulfurous odor, and cloven feet",
                    "Forked tongue and odor of brimstone", "Forked tongue and goat-like legs", "Forked tongue and hooves",
                    "Odor of brimstone and goat-like legs", "Odor of brimstone and hooves",
                    "Forked tongue, odor of brimstone, and goat-like legs", "Forked tongue, odor of brimstone, and hooves",
                    "Antlers and prehensile tail", "Antlers and non-prehensile tail", "Antlers and pointed teeth",
                    "Antlers, prehensile tail, and pointed teeth", "Antlers, non-prehensile tail, and pointed teeth",
                    "Antlers and sulfurous odor", "Antlers and cloven feet", "Antlers, sulfurous odor, and cloven feet",
                    "Antlers, prehensile tail, and sulfurous odor", "Antlers, prehensile tail, and cloven feet",
                        "Antlers, prehensile tail, sulfurous odor, and cloven feet",
                    "Antlers, non-prehensile tail, and sulfurous odor", "Antlers, non-prehensile tail, and cloven feet",
                        "Antlers, non-prehensile tail, sulfurous odor, and cloven feet",
                    "Antlers, pointed teeth, and sulfurous odor", "Antlers, pointed teeth, and cloven feet", "Antlers, pointed teeth, sulfurous odor, and cloven feet",
                ]
            );
            //Source: https://nationalzoo.si.edu/animals/tiger
            appearances[CreatureConstants.Tiger][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "Reddish-orange coat with prominent black stripes, white bellies and white spots on the ears." };
            //https://forgottenrealms.fandom.com/wiki/Dire_tiger
            appearances[CreatureConstants.Tiger_Dire][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = new[] { "Reddish-orange coat with prominent black stripes, white bellies and white spots on the ears" };
            appearances[CreatureConstants.Tiger_Dire][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Pair of huge fangs jutting from powerful jaws" };
            //Source: https://forgottenrealms.fandom.com/wiki/Titan
            appearances[CreatureConstants.Titan] = appearances[CreatureConstants.Human];
            //Source: https://www.woodlandtrust.org.uk/trees-woods-and-wildlife/animals/reptiles-and-amphibians/common-toad/
            appearances[CreatureConstants.Toad] = GetAppearances(
                commonSkin: new[] { "Olive brown, dry, warty skin", "Green, dry, warty skin", "Dark brown, dry, warty skin", "Gray, dry, warty skin" },
                uncommonSkin: new[] { "Olive brown, dry, warty skin with dark markings", "Green, dry, warty skin with dark markings",
                    "Dark brown, dry, warty skinn with dark markings", "Gray, dry, warty skin with dark markings" },
                commonEyes: new[] { "Copper-colored eyes with horizontal pupils" }
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Tojanida
            //https://www.d20pfsrd.com/bestiary/monster-listings/outsiders/tojanida/
            appearances[CreatureConstants.Tojanida_Juvenile][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Resemble a cross between a monstrous crab and an enormous snapping turtles. Light green shell, sea-green skin" };
            appearances[CreatureConstants.Tojanida_Adult][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Resemble a cross between a monstrous crab and an enormous snapping turtles. Light green shell, sea-green skin" };
            appearances[CreatureConstants.Tojanida_Elder][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Resemble a cross between a monstrous crab and an enormous snapping turtles. Light green shell, sea-green skin" };
            //Source: https://www.d20srd.org/srd/monsters/treant.htm
            appearances[CreatureConstants.Treant][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Face-like features on their bark, a division between the trunk that forms legs, and long branches that serve as arms; brown bark-like skin" };
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Triceratops
            appearances[CreatureConstants.Triceratops] = GetAppearances(
                commonSkin: new[] { "Dark beige skin", "Pale gray skin with slightly-darker stripes across the back",
                    "Sandy-brown skin with slightly-darker stripes across the back",
                    "Dark gray skin with slightly-lighter stripes across the back, slightly-lighter underbelly",
                    "Earthy, mud-colored skin with slightly-lighter stripes across the back, slightly-lighter underbelly",
                    "Green-brown skin with slightly-lighter stripes across the back, slightly-lighter underbelly" },
                uncommonSkin: new[] { "Gray-tan skin, mottled spots down the back", "Bright green skin, yellow underbelly, mottled spots down the back",
                    "Light gray-green skin, lighter underbelly, mottled spots down the back",
                    "Dull red skin, yellow underbelly, mottled spots down the back",
                    "Orange skin, yellow underbelly, mottled spots down the back",
                    "Sandy-yellow skin, lighter underbelly, mottled spots down the back" },
                commonOther: new[] { "Large bony neck frill and three horns on its head" });
            //Source: https://forgottenrealms.fandom.com/wiki/Triton
            appearances[CreatureConstants.Triton] = GetAppearances(
                commonSkin: new[] { "Silver skin", "Silver-blue skin", "Deep blue skin" },
                uncommonSkin: new[] { "Pearl-colored skin", "Light green skin" },
                rareSkin: new[] { "Dark green skin", "Bright green skin", "Gray skin", "Dark gray skin", "Night-black skin", "Red skin", "Orange skin", "Yellow skin" },
                commonHair: new[] { "Deep blue hair", "Green-blue hair", "Dark blue hair", "Dark green hair" },
                uncommonHair: new[] { "Light blue hair", "Light green hair" },
                commonEyes: new[] { "Brilliant green eyes with a nictitating membrane", "Brilliant blue-green eyes with a nictitating membrane",
                    "Brilliant blue eyes with a nictitating membrane", "Brilliant dark green eyes with a nictitating membrane",
                    "Brilliant dark blue eyes with a nictitating membrane", "Brilliant light green eyes with a nictitating membrane",
                    "Brilliant light blue eyes with a nictitating membrane" },
                commonOther: new[] { "Hands and feet are webbed, minor dorsal fins that run from the mid-calf to the ankle" });
            //Source: https://forgottenrealms.fandom.com/wiki/Troglodyte
            appearances[CreatureConstants.Troglodyte] = GetAppearances(
                commonSkin: new[] { "Grayish-brown leathery scales" },
                commonEyes: new[] { "Black beady eyes" },
                commonOther: new[] { "Spindly but muscular arms, squat legs, long slender tail, lizard-like head, claws and fangs" });
            appearances[CreatureConstants.Troglodyte + GenderConstants.Male] = GetAppearances(
                commonSkin: new[] { "Grayish-brown leathery scales" },
                commonEyes: new[] { "Black beady eyes" },
                commonOther: new[] { "Spindly but muscular arms, squat legs, long slender tail, lizard-like head, claws and fangs, head crowned with frills extending from forehead to neck" });
            //Source: https://forgottenrealms.fandom.com/wiki/Troll
            appearances[CreatureConstants.Troll] = GetAppearances(
                commonSkin: new[] { "Thick, rubbery, moss-green scales", "Thick, rubbery, putrid gray scales", "Thick, rubbery, mottled gray and green scales" },
                commonHair: new[] { "Iron-gray hair", "Greenish-black hair", "Steel gray hair", "Black hair" },
                commonOther: new[] { "Severe hunch, backs of the hands dragging on the ground" });
            //Source: https://forgottenrealms.fandom.com/wiki/Scrag
            appearances[CreatureConstants.Troll_Scrag] = GetAppearances(
                commonSkin: new[] { "Blue-green scales", "Olive-green scales" },
                commonHair: new[] { "Iron-gray hair", "Greenish-black hair", "Steel gray hair", "Black hair" },
                commonOther: new[] { "Severe hunch, backs of the hands dragging on the ground, gills" });
            //Source: https://forgottenrealms.fandom.com/wiki/Trumpet_archon
            appearances[CreatureConstants.TrumpetArchon + GenderConstants.Male] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(["Green skin"])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]),
                rareSkin: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare]),
                veryRareSkin: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]),
                commonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]),
                rareHair: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]),
                veryRareHair: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]),
                commonEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]),
                rareEyes: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]),
                veryRareEyes: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]),
                commonOther: new[] { "Wings" });
            appearances[CreatureConstants.TrumpetArchon + GenderConstants.Female] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                uncommonSkin: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(["Green skin"])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]),
                rareSkin: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare]),
                veryRareSkin: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare]),
                commonHair: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]),
                rareHair: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare]),
                veryRareHair: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare]),
                commonEyes: appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                uncommonEyes: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]),
                rareEyes: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon]
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare]),
                veryRareEyes: appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]
                    .Concat(appearances[CreatureConstants.Halfling_Lightfoot + GenderConstants.Male][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare])
                    .Concat(appearances[CreatureConstants.Elf_High][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare]),
                commonOther: new[] { "Wings" });
            //Source: https://jurassicworld-evolution.fandom.com/wiki/Tyrannosaurus
            appearances[CreatureConstants.Tyrannosaurus] = GetAppearances(
                commonSkin: new[] { "Dark brown skin", "Light brown skin", "White skin with dark gray stripes across the back", "Dark brown-gray skin, lighter underbelly",
                    "Dark gray skin, orange underbelly", "Gray-green skin with gray-yellow stripes across the back, gray-yellow underbelly",
                    "Yellow skin with black stripes across the back" },
                uncommonSkin: new[] { "Gray skin, lighter underbelly", "Dark gray skin, slightly-lighter underbelly", "Turquoise-gray skin, yellow underbelly",
                    "Green skin, yellow underbelly", "Orange skin, white underbelly", "Brown-red skin, yellow underbelly" },
                commonOther: new[] { "Massive head, powerful jaw, rows of large serrated teeth, small non-functional arms" });
            //Source: https://forgottenrealms.fandom.com/wiki/Umber_hulk
            appearances[CreatureConstants.UmberHulk] = GetAppearances(
                commonSkin: new[] { "Black chitinous armor, front is burnt brown", "Black chitinous armor, front is yellowish-gray" },
                commonEyes: new[] { "Two sets of evenly-spaced eyes: compound eyes on the side of the head, forehead eyes that are small and apelike" },
                commonOther: new[] { "Looks like a cross between a gorilla and a beetle, huge mandibles, mouth filled with rows of sharp triangular teeth" });
            appearances[CreatureConstants.UmberHulk_TrulyHorrid] = GetAppearances(
                commonSkin: new[] { "Black chitinous armor, front is burnt brown", "Black chitinous armor, front is yellowish-gray" },
                commonEyes: new[] { "Two sets of evenly-spaced eyes: compound eyes on the side of the head, forehead eyes that are small and apelike" },
                commonOther: new[] { "Looks like a cross between a gorilla and a beetle, huge mandibles, mouth filled with rows of sharp triangular teeth" });
            //Source: https://forgottenrealms.fandom.com/wiki/Unicorn
            appearances[CreatureConstants.Unicorn] = GetAppearances(
                commonHair: ["White coat, mane, and tail"],
                commonEyes: new[] { "Deep sea-blue eyes", "Violet eyes", "Brown eyes", "Golden eyes", "Pink eyes" },
                commonOther: new[] { "Cloven hooves, long ivory-colored horn protruding from the forehead" });
            appearances[CreatureConstants.Unicorn + GenderConstants.Male] = GetAppearances(
                commonHair: ["White coat, mane, tail, and beard"],
                commonEyes: new[] { "Deep sea-blue eyes", "Violet eyes", "Brown eyes", "Golden eyes", "Pink eyes" },
                commonOther: new[] { "Cloven hooves, long ivory-colored horn protruding from the forehead" });
            //Source: https://forgottenrealms.fandom.com/wiki/Vampire_spawn
            appearances[CreatureConstants.VampireSpawn] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common],
                commonOther: new[] { "Hardened features, appearing predatory" });
            //Source: https://forgottenrealms.fandom.com/wiki/Vargouille
            appearances[CreatureConstants.Vargouille] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Select(e => $"{e} with an unnerving green glow"),
                commonOther: new[] { "Grotesquely deformed human-like head, leathery bat-like wings instead of ears, flailing tendrils crown and fringe the head in place of hair, jagged teeth" });
            //Source: https://forgottenrealms.fandom.com/wiki/Violet_fungus
            appearances[CreatureConstants.VioletFungus] = GetAppearances(
                commonSkin: new[] { "Deep purple coloration", "Dull gray coloration", "Violet coloration and covered in purple spots" },
                commonOther: new[] { "Mass of root-like feelers, four leafy tendrils" });
            //Source: https://forgottenrealms.fandom.com/wiki/Vrock
            appearances[CreatureConstants.Vrock][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "A cross between a vulture and a human. Twisted and gnarled with a long neck and limbs covered in sinew. Covered in small gray feathers. Stinks of offal and carrion. Long talons and vulture head." };
            //Source: https://activepestcontrol.com/pest-info/bees-and-hornets/red-paper-wasp/
            //https://forgottenrealms.fandom.com/wiki/Giant_wasp 
            appearances[CreatureConstants.Wasp_Giant][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = ["Dull tan body with yellow stripes", "Nearly black wings, red coloring, yellow face"];
            appearances[CreatureConstants.Wasp_Giant][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Pinched waist"];
            //Source: https://www.woodlandtrust.org.uk/trees-woods-and-wildlife/animals/mammals/weasel/
            appearances[CreatureConstants.Weasel][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = ["Chestnut-brown fur with white-cream underparts"];
            appearances[CreatureConstants.Weasel][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = ["Long, slim body that ends in a short tail."];
            //https://forgottenrealms.fandom.com/wiki/Giant_weasel
            appearances[CreatureConstants.Weasel_Dire] = GetAppearances(
                commonHair: ["Chestnut-brown fur with white-cream underparts"],
                uncommonHair: ["White fur with white-cream underparts"],
                rareHair: ["Black fur with black underparts"],
                commonOther: ["Long, slim body ending in a short tail"]);
            //Source: https://www.d20srd.org/srd/monsters/whale.htm
            //https://www.fisheries.noaa.gov/species/humpback-whale
            //https://www.fisheries.noaa.gov/species/gray-whale
            //https://www.fisheries.noaa.gov/species/north-atlantic-right-whale
            appearances[CreatureConstants.Whale_Baleen] = GetAppearances(
                commonSkin: [
                    "Humpback Whale: black skin with white on the underside of the pectoral fins, belly, and underside of the tail",
                    "Gray Whale: Mottled gray skin with small eyes located just above the corners of the mouth. Broad paddle-shaped pectoral flippers, pointed at the tips. Dorsal hump.",
                    "Right Whale: Stocky black body, V-shaped blow spout, broad and deeply-notched tail, all-black belly",
                    "Right Whale: Stocky black body, V-shaped blow spout, broad and deeply-notched tail, black belly with irregularly-shaped white patches",
                ],
                uncommonSkin: ["Humpback Whale: black skin with white on the underside of the pectoral fins, flanks, belly, and underside of the tail"]);
            //Source: https://www.fisheries.noaa.gov/species/sperm-whale
            appearances[CreatureConstants.Whale_Cachalot] = GetAppearances(
                commonSkin: ["Sperm Whale: Dark grey skin"],
                uncommonSkin: ["Sperm Whale: Dark grey skin with white patches on the belly"],
                rareSkin: ["Sperm Whale (Moby Dick): White (albino) skin"]);
            //Source: https://www.fisheries.noaa.gov/species/killer-whale
            appearances[CreatureConstants.Whale_Orca][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = [
                "Mostly black on top with white underside and white patches near the eyes. Gray saddle patch behind the dorsal fin.",
                "Mostly black on top with white underside and white patches near the eyes. White saddle patch behind the dorsal fin.",
            ];
            //Source: https://forgottenrealms.fandom.com/wiki/Wight
            appearances[CreatureConstants.Wight] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Select(s => s.Replace("skin", "mummified skin")),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: ["Calcified, all-white eyes"],
                commonOther: ["Hands end in claws, teeth are sharp and jagged like needles"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Will-o'-wisp
            appearances[CreatureConstants.WillOWisp] = GetAppearances(
                commonSkin: [ "Hazy ball of white light", "Hazy ball of blue light", "Hazy ball of violet light", "Hazy ball of green light",
                    "Hazy ball of yellow light" ]);
            //Source: https://forgottenrealms.fandom.com/wiki/Winter_wolf
            appearances[CreatureConstants.WinterWolf] = GetAppearances(
                commonHair: ["Glistening white fur", "Glistening silvery fur"],
                commonEyes: ["Blue eyes", "Very pale blue eyes"],
                uncommonEyes: ["Silvery eyes"]);
            //Source: https://seaworld.org/animals/facts/mammals/gray-wolf/
            appearances[CreatureConstants.Wolf] = GetAppearances(
                commonHair: [ "Light gray fur with a thick, dense underfur", "Gray fur with a thick, dense underfur", "Dark gray fur with a thick, dense underfur",
                    "Light gray fur interspersed with black and white with a thick, dense underfur",
                    "Gray fur interspersed with black and white with a thick, dense underfur",
                    "Dark gray fur interspersed with black and white with a thick, dense underfur"
                ],
                uncommonHair: ["Solid black fur with a thick, dense underfur", "Solid white fur with a thick, dense underfur"],
                commonOther: ["High body, long legs, broad skull tapering to a narrow muzzle, bushy tail"]);
            //Source: https://forgottenrealms.fandom.com/wiki/Dire_wolf
            appearances[CreatureConstants.Wolf_Dire] = GetAppearances(
                commonHair: ["Thick mottled gray fur", "Thick black fur", "Thick mottled black fur", "Thick gray fur"],
                commonEyes: ["Eyes like fire"]);
            //Source: https://www.adfg.alaska.gov/index.cfm?adfg=wolverine.printerfriendly
            appearances[CreatureConstants.Wolverine] = GetAppearances(
                commonHair: [
                    "Long, dense, dark brown fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                    "Long, dense, dark brown fur with a gold stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                    "Long, dense, black fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                    "Long, dense, black fur with a gold stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                ],
                uncommonHair: new[] {
                    "Long, dense, dark brown fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail",
                    "Long, dense, dark brown fur with a gold stripe running from each shoulder along the flanks to the base of the tail",
                    "Long, dense, black fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail",
                    "Long, dense, black fur with a gold stripe running from each shoulder along the flanks to the base of the tail",
                },
                commonOther: new[] { "Thick body, short legs, short ears, broad flat head" });
            appearances[CreatureConstants.Wolverine_Dire] = GetAppearances(
                commonHair: new[] {
                    "Long, dense, dark brown fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                    "Long, dense, dark brown fur with a gold stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                    "Long, dense, black fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                    "Long, dense, black fur with a gold stripe running from each shoulder along the flanks to the base of the tail, with a white hair patch on the neck and chest",
                },
                uncommonHair: new[] {
                    "Long, dense, dark brown fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail",
                    "Long, dense, dark brown fur with a gold stripe running from each shoulder along the flanks to the base of the tail",
                    "Long, dense, black fur with a creamy white stripe running from each shoulder along the flanks to the base of the tail",
                    "Long, dense, black fur with a gold stripe running from each shoulder along the flanks to the base of the tail",
                },
                commonOther: new[] { "Thick body, short legs, short ears, broad flat head" });
            //Source: https://forgottenrealms.fandom.com/wiki/Worg
            appearances[CreatureConstants.Worg] = GetAppearances(
                commonHair: new[] { "Gray fur", "Black fur" },
                uncommonHair: new[] { "Gray, particularly-glossy fur", "Black, particularly-glossy fur" },
                commonOther: new[] { "High body, long legs, broad skull tapering to a narrow muzzle, bushy tail" });
            //Source: https://forgottenrealms.fandom.com/wiki/Wraith
            appearances[CreatureConstants.Wraith][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "A sinister, spectral figure robed in darkness, no visual features or appendages, except for their glowing red eyes" };
            appearances[CreatureConstants.Wraith_Dread][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "A sinister, spectral figure robed in darkness, no visual features or appendages, except for their glowing red eyes" };
            //Source: https://forgottenrealms.fandom.com/wiki/Wyvern
            appearances[CreatureConstants.Wyvern] = GetAppearances(
                commonSkin: new[] { "Dark brown scales", "Gray scales", "Dark gray scales", "Gray-brown scales" },
                commonEyes: new[] { "Orange eyes", "Red eyes" },
                commonOther: new[] { "Jaw filled with long, sharp teeth" });
            //Source: https://forgottenrealms.fandom.com/wiki/Xill
            appearances[CreatureConstants.Xill][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = new[] { "Vaguely reptilian with flame-red, leathery skin and solid black eyes. Roughly humanoid body with four clawed arms." };
            //Source: https://forgottenrealms.fandom.com/wiki/Xorn
            appearances[CreatureConstants.Xorn_Minor] = GetAppearances(
                commonSkin: new[] { "Brown stonelike skin", "Brown pebbly, rocky skin", "Gray stonelike skin", "Gray pebbly, rocky skin" },
                uncommonSkin: new[] { "Dark gray, metallic-like skin" },
                commonOther: new[] { "Large mouth on top of head, radially-symmetric body, three arms with talons at the end, three eyes, three legs" });
            appearances[CreatureConstants.Xorn_Average] = GetAppearances(
                commonSkin: new[] { "Brown stonelike skin", "Brown pebbly, rocky skin", "Gray stonelike skin", "Gray pebbly, rocky skin" },
                uncommonSkin: new[] { "Dark gray, metallic-like skin" },
                commonOther: new[] { "Large mouth on top of head, radially-symmetric body, three arms with talons at the end, three eyes, three legs" });
            appearances[CreatureConstants.Xorn_Elder] = GetAppearances(
                commonSkin: new[] { "Brown stonelike skin", "Brown pebbly, rocky skin", "Gray stonelike skin", "Gray pebbly, rocky skin" },
                uncommonSkin: new[] { "Dark gray, metallic-like skin" },
                commonOther: new[] { "Large mouth on top of head, radially-symmetric body, three arms with talons at the end, three eyes, three legs" });
            //Source: https://forgottenrealms.fandom.com/wiki/Yeth_hound
            appearances[CreatureConstants.YethHound] = GetAppearances(
                commonHair: new[] { "Dull, nonreflective night-black fur" },
                commonEyes: new[] { "Cherry-red eyes" },
                commonOther: new[] { "Resembles a greyhound, smells of chilled smoke, oddly human-like visage with protruding nose, short pointed ears that curve upward and away resembling horns" });
            //Source: https://forgottenrealms.fandom.com/wiki/Yrthak
            appearances[CreatureConstants.Yrthak] = GetAppearances(
                commonSkin: new[] { "Tough yellowish-green skin, with yellow leathery wings and yellow back-fin" },
                commonOther: new[] {
                    "Akin to a winged crocodile, eyeless head, disproportionately large mouth filled with yellow teeth, distinctive twisting horn",
                    "Akin to a sickly dragon, eyeless head, disproportionately large mouth filled with yellow teeth, distinctive twisting horn",
                });
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_pureblood
            appearances[CreatureConstants.YuanTi_Pureblood] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => $"{s} with patches of scales")),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                    .Concat(appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "snake-like eyes"))),
                commonOther: new[] { "Fangs", "Forked tongue" },
                uncommonOther: new[] { "Fangs and forked tongue" }
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_malison
            appearances[CreatureConstants.YuanTi_Halfblood_SnakeArms] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => $"{s} with distinctly-patterned scales"),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "snake-like eyes")),
                commonOther: new[] { "Snake arms, fangs and forked tongue" }
            );
            appearances[CreatureConstants.YuanTi_Halfblood_SnakeHead] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => $"{s} with distinctly-patterned scales"),
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "snake-like eyes")),
                commonOther: ["Snake head"],
                uncommonOther: ["Snake head with hood like a cobra"]
            );
            appearances[CreatureConstants.YuanTi_Halfblood_SnakeTail] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => $"{s} with distinctly-patterned scales"),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "snake-like eyes")),
                commonOther: ["Snake tail instead of human legs, fangs and forked tongue"]
            );
            appearances[CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs] = GetAppearances(
                commonSkin: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common]
                        .Select(s => $"{s} with distinctly-patterned scales"),
                commonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common],
                uncommonHair: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon],
                commonEyes: appearances[CreatureConstants.Human][TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common]
                        .Select(e => e.Replace("eyes", "snake-like eyes")),
                commonOther: ["Snake tail and human legs, fangs and forked tongue"]
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Yuan-ti_abomination
            appearances[CreatureConstants.YuanTi_Abomination] = GetAppearances(
                commonSkin: [ "Turquoise scales with pale yellow underbelly", "Bright green scales with yellow underbelly", "Yellow-green scales",
                    "Gray scales with black-outlined red diamonds along the back" ],
                commonEyes: ["Yellow snake-like eyes", "Red snake-like eyes", "Green snake-like eyes"],
                commonOther: ["Resembles massive snake with scale-covered humanoid arms, wedge-shaped head, fangs, forked tongue"]
            );
            //Source: https://forgottenrealms.fandom.com/wiki/Zelekhut
            appearances[CreatureConstants.Zelekhut][TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = [
                "Looks like a mechanical centaur, with skin as white as alabaster and clad in golden plate armor. A pair of golden metallic wings unfold from its back when needed.",
                "Looks like a clockwork centaur, with skin as white as alabaster and clad in golden plate armor. A pair of golden metallic wings unfold from its back when needed.",
            ];

            //Source: https://www.d20srd.org/srd/monsters/celestialCreature.htm
            //https://forgottenrealms.fandom.com/wiki/Celestial_creature#Description
            appearances[CreatureConstants.Templates.CelestialCreature] = GetAppearances(
                commonSkin: ["Gold skin", "Silver skin", "Platinum skin"],
                uncommonSkin: ["Copper skin", "Pristine white skin"],
                commonOther: ["More regal and more beautiful than the earthly counterpart"]
            );
            //Source: https://www.d20srd.org/srd/monsters/fiendishCreature.htm
            //https://forgottenrealms.fandom.com/wiki/Fiendish_creature#Description
            //https://www.reddit.com/r/Pathfinder_RPG/comments/2lt7rw/what_exactly_do_celestial_and_fiendish_creatures/?rdt=42903
            appearances[CreatureConstants.Templates.FiendishCreature] = GetAppearances(
                commonSkin: ["Red skin", "Dark red skin", "Black skin"],
                uncommonOther: ["Sulfur smell", "Mangy appearance", "Diseased appearance"],
                rareOther: ["Diseased appearance, sulfur smell", "Mangy appearance, sulfur smell"]
            );
            //Source: https://www.d20srd.org/srd/monsters/ghost.htm
            //https://forgottenrealms.fandom.com/wiki/Ghost#Description
            appearances[CreatureConstants.Templates.Ghost] = GetAppearances(
                commonSkin: ["Transluscent white skin (and body)"],
                uncommonOther: ["The ghost's appearance reflects how it died", "The ghost's appearance reflects its alignment", "Adorned in chains and weights"]
            );
            //Source: https://www.d20srd.org/srd/monsters/halfCelestial.htm
            //https://forgottenrealms.fandom.com/wiki/Half-celestial#Description
            appearances[CreatureConstants.Templates.HalfCelestial] = GetAppearances(
                commonSkin: ["Golden skin"],
                uncommonEyes: ["Sparkling eyes"],
                commonOther: ["Angelic feathered wings"]
            );
            //Source: https://www.d20srd.org/srd/monsters/halfDragon.htm
            appearances[CreatureConstants.Templates.HalfDragon_Black] = GetAppearances(
                commonSkin: ["Black scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Blue] = GetAppearances(
                commonSkin: ["Blue scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Brass] = GetAppearances(
                commonSkin: ["Brass scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Bronze] = GetAppearances(
                commonSkin: ["Bronze scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Copper] = GetAppearances(
                commonSkin: ["Copper scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Gold] = GetAppearances(
                commonSkin: ["Gold scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Green] = GetAppearances(
                commonSkin: ["Green scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Red] = GetAppearances(
                commonSkin: ["Red scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_Silver] = GetAppearances(
                commonSkin: ["Silver scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            appearances[CreatureConstants.Templates.HalfDragon_White] = GetAppearances(
                commonSkin: ["White scales"],
                commonEyes: ["Reptilian eyes"],
                commonOther: ["Elongated features, exaggerated teeth, claws"]
            );
            //Source: https://www.d20srd.org/srd/monsters/halfFiend.htm
            appearances[CreatureConstants.Templates.HalfFiend] = GetAppearances(
                uncommonSkin: ["Dark red scales", "Black scales"],
                uncommonEyes: ["Glowing red eyes"],
                commonOther: ["Bat wings", "Bat wings, horns", "Bat wings, a fetid odor"],
                uncommonOther: ["Bat wings, horns, and a fetid odor"]
            );
            //Source: https://www.d20srd.org/srd/monsters/lich.htm
            //https://forgottenrealms.fandom.com/wiki/Lich#Description
            appearances[CreatureConstants.Templates.Lich] = GetAppearances(
                commonSkin: ["Withered flesh stretched tight across horribly visible bones"],
                commonHair: ["Hair long ago lost to decay"],
                uncommonHair: ["A hint of the mortal hair remains"],
                commonEyes: ["Eyes long ago lost to decay, but bright pinpoints of crimson light burn on in the empty sockets"],
                uncommonEyes: ["Eyes rotting in their sockets"],
                commonOther: ["Gaunt and skeletal"]
            );
            //Source: https://www.d20srd.org/srd/monsters/lycanthrope.htm
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted] = GetAppearances(
                commonHair: ["Thick black hair", "Thick reddish hair", "Thick blond hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural] = GetAppearances(
                commonHair: ["Thick black hair", "Thick reddish hair", "Thick blond hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted] = GetAppearances(
                commonHair: ["Thick brown hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural] = GetAppearances(
                commonHair: ["Thick brown hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted] = GetAppearances(
                commonHair: ["Thick brown hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural] = GetAppearances(
                commonHair: ["Thick brown hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted] = GetAppearances(
                commonHair: ["Thick ivory hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural] = GetAppearances(
                commonHair: ["Thick ivory hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Black_Afflicted + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick black hair and beard", "Thick reddish hair and beard", "Thick blond hair and beard"],
                uncommonHair: ["Thick black hair, no facial hair", "Thick reddish hair, no facial hair", "Thick blond hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Black_Natural + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick black hair and beard", "Thick reddish hair and beard", "Thick blond hair and beard"],
                uncommonHair: ["Thick black hair, no facial hair", "Thick reddish hair, no facial hair", "Thick blond hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Brown_Afflicted + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick brown hair and beard"],
                uncommonHair: ["Thick brown hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Brown_Natural + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick brown hair and beard"],
                uncommonHair: ["Thick brown hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Dire_Afflicted + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick brown hair and beard"],
                uncommonHair: ["Thick brown hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Dire_Natural + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick brown hair and beard"],
                uncommonHair: ["Thick brown hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Polar_Afflicted + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick ivory hair and beard"],
                uncommonHair: ["Thick ivory hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Bear_Polar_Natural + GenderConstants.Male] = GetAppearances(
                commonHair: ["Thick ivory hair and beard"],
                uncommonHair: ["Thick ivory hair, no facial hair"],
                commonOther: ["Stout, well-muscled, hairy"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Boar_Afflicted] = GetAppearances(
                commonOther: ["Stocky and muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Boar_Natural] = GetAppearances(
                commonOther: ["Stocky and muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Boar_Dire_Afflicted] = GetAppearances(
                commonHair: ["Coarser, stiffer hair"],
                commonOther: ["Stocky and muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Boar_Dire_Natural] = GetAppearances(
                commonHair: ["Coarser, stiffer hair"],
                commonOther: ["Stocky and muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Afflicted] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Natural] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Afflicted + GenderConstants.Male] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonHair: ["Thin, ragged mustache"],
                uncommonHair: ["No facial hair"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Natural + GenderConstants.Male] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonHair: ["Thin, ragged mustache"],
                uncommonHair: ["No facial hair"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Afflicted + GenderConstants.Male] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonHair: ["Thin, ragged mustache"],
                uncommonHair: ["No facial hair"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Rat_Dire_Natural + GenderConstants.Male] = GetAppearances(
                commonEyes: ["Eyes constantly dart around"],
                commonHair: ["Thin, ragged mustache"],
                uncommonHair: ["No facial hair"],
                commonOther: ["Thin and wiry"],
                uncommonOther: ["Thin and wiry, nose and mouth twitch when excited"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Tiger_Afflicted] = GetAppearances(
                commonOther: ["Sleekly muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Tiger_Natural] = GetAppearances(
                commonOther: ["Sleekly muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Afflicted] = GetAppearances(
                commonOther: ["Sleekly muscular"]
            );
            appearances[CreatureConstants.Templates.Lycanthrope_Tiger_Dire_Natural] = GetAppearances(
                commonOther: ["Sleekly muscular"]
            );
            //Werewolves are indistinguishable from non-lycanthropes
            //Source: https://www.d20srd.org/srd/monsters/skeleton.htm
            appearances[CreatureConstants.Templates.Skeleton] = GetAppearances(
                commonOther: ["Animated bones of the dead."]
            );
            //Source: https://www.d20srd.org/srd/monsters/vampire.htm
            //https://forgottenrealms.fandom.com/wiki/Vampire#Description
            appearances[CreatureConstants.Templates.Vampire] = GetAppearances(
                commonSkin: ["Skin paler than it was in life."],
                commonEyes: ["Red eyes."],
                commonOther: ["Features are hardened and feral, with the predatory look of a wolf. Casts no shadows, and throws no reflections in mirrors."]
            );
            //Source: https://www.d20srd.org/srd/monsters/zombie.htm
            //https://forgottenrealms.fandom.com/wiki/Zombie#Description
            appearances[CreatureConstants.Templates.Zombie] = GetAppearances(
                commonOther: ["Reanimated, rotting corpse."],
                uncommonOther: ["Reanimated preserved corpse, with less rotting than most zombies."]
            );

            return appearances;
        }

        private static Dictionary<string, Dictionary<Rarity, IEnumerable<string>>> GetAppearances(
            IEnumerable<string> commonSkin = null, IEnumerable<string> uncommonSkin = null, IEnumerable<string> rareSkin = null, IEnumerable<string> veryRareSkin = null,
            IEnumerable<string> commonHair = null, IEnumerable<string> uncommonHair = null, IEnumerable<string> rareHair = null, IEnumerable<string> veryRareHair = null,
            IEnumerable<string> commonEyes = null, IEnumerable<string> uncommonEyes = null, IEnumerable<string> rareEyes = null, IEnumerable<string> veryRareEyes = null,
            IEnumerable<string> commonOther = null, IEnumerable<string> uncommonOther = null, IEnumerable<string> rareOther = null, IEnumerable<string> veryRareOther = null)
        {
            var appearances = InitializeAppearancesCollection();

            appearances[TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Common] = commonSkin ?? [string.Empty];
            appearances[TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Uncommon] = uncommonSkin ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.Rare] = rareSkin ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Skin][Rarity.VeryRare] = veryRareSkin ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Common] = commonHair ?? [string.Empty];
            appearances[TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Uncommon] = uncommonHair ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.Rare] = rareHair ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Hair][Rarity.VeryRare] = veryRareHair ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Common] = commonEyes ?? [string.Empty];
            appearances[TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Uncommon] = uncommonEyes ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.Rare] = rareEyes ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Eyes][Rarity.VeryRare] = veryRareEyes ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Common] = commonOther ?? [string.Empty];
            appearances[TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Uncommon] = uncommonOther ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Other][Rarity.Rare] = rareOther ?? [];
            appearances[TableNameConstants.Collection.AppearanceCategories.Other][Rarity.VeryRare] = veryRareOther ?? [];

            return appearances;
        }

        private static IEnumerable<string> Combine(string joiner, IEnumerable<string> starts, IEnumerable<string> ends)
        {
            foreach (var start in starts)
            {
                foreach (var end in ends)
                {
                    yield return start + joiner + end;
                }
            }
        }

        private string LowercaseFirstCharacter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}
