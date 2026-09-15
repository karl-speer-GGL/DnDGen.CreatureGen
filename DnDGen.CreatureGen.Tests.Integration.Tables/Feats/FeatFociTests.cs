using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Skills;
using DnDGen.CreatureGen.Tables;
using DnDGen.TreasureGen.Items;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Feats
{
    [TestFixture]
    public class FeatFociTests : CollectionTests
    {
        protected override string TableName
        {
            get { return TableNameConstants.Collection.FeatFoci; }
        }

        [Test]
        public void FeatFociNames()
        {
            var names = new[]
            {
                FeatConstants.WeaponProficiency_Exotic,
                FeatConstants.WeaponProficiency_Martial,
                FeatConstants.WeaponProficiency_Simple,
                GroupConstants.Skills,
                "Imp Alternate Form"
            };

            AssertCollectionNames(names);
        }

        [TestCase("Imp Alternate Form",
            CreatureConstants.Spider_Monstrous_Hunter_Small,
            CreatureConstants.Spider_Monstrous_Hunter_Medium,
            CreatureConstants.Spider_Monstrous_WebSpinner_Small,
            CreatureConstants.Spider_Monstrous_WebSpinner_Medium,
            CreatureConstants.Raven,
            CreatureConstants.Rat,
            CreatureConstants.Boar)]
        public void FeatFoci(string feat, params string[] foci)
        {
            base.AssertDistinctCollection(feat, foci);
        }

        [Test]
        public void SimpleWeaponProficiencyFoci()
        {
            var foci = WeaponConstants.GetAllSimple(false, false);
            var ammunition = WeaponConstants.GetAllAmmunition(false, false);
            foci = foci.Except(ammunition);
            base.AssertDistinctCollection(FeatConstants.WeaponProficiency_Simple, foci.ToArray());
        }

        [Test]
        public void MartialWeaponProficiencyFoci()
        {
            var foci = WeaponConstants.GetAllMartial(false, false);
            var ammunition = WeaponConstants.GetAllAmmunition(false, false);
            foci = foci.Except(ammunition);
            base.AssertDistinctCollection(FeatConstants.WeaponProficiency_Martial, foci.ToArray());
        }

        [Test]
        public void ExoticWeaponProficiencyFoci()
        {
            var foci = WeaponConstants.GetAllExotic(false, false);
            base.AssertDistinctCollection(FeatConstants.WeaponProficiency_Exotic, foci.ToArray());
        }

        [Test]
        public void SkillsFoci()
        {
            var foci = new[]
            {
                SkillConstants.Appraise,
                SkillConstants.Balance,
                SkillConstants.Bluff,
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Alchemy),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Armorsmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Blacksmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Bookbinding),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Bowmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Brassmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Brewing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Candlemaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Cloth),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Coppersmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Dyemaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Gemcutting),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Glass),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Goldsmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Hatmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Hornworking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Jewelmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Leather),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Locksmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Mapmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Milling),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Painting),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Parchmentmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Pewtermaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Potterymaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Sculpting),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Shipmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Shoemaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Silversmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Skinning),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Soapmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Stonemasonry),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Tanning),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Trapmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Weaponsmithing),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Weaving),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Wheelmaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Winemaking),
                SkillConstants.Build(SkillConstants.Craft, SkillConstants.Foci.Craft.Woodworking),
                SkillConstants.Climb,
                SkillConstants.Concentration,
                SkillConstants.DecipherScript,
                SkillConstants.Diplomacy,
                SkillConstants.DisableDevice,
                SkillConstants.Disguise,
                SkillConstants.EscapeArtist,
                SkillConstants.Forgery,
                SkillConstants.GatherInformation,
                SkillConstants.HandleAnimal,
                SkillConstants.Heal,
                SkillConstants.Hide,
                SkillConstants.Intimidate,
                SkillConstants.Jump,
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.Arcana),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.ArchitectureAndEngineering),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.Dungeoneering),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.Geography),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.History),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.Local),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.Nature),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.NobilityAndRoyalty),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.Religion),
                SkillConstants.Build(SkillConstants.Knowledge, SkillConstants.Foci.Knowledge.ThePlanes),
                SkillConstants.Listen,
                SkillConstants.MoveSilently,
                SkillConstants.OpenLock,
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.Act),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.Comedy),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.Dance),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.KeyboardInstruments),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.Oratory),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.PercussionInstruments),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.Sing),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.StringInstruments),
                SkillConstants.Build(SkillConstants.Perform, SkillConstants.Foci.Perform.WindInstruments),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Adviser),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Alchemist),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.AnimalGroomer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.AnimalTrainer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Apothecary),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Appraiser),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Architect),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Armorer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Barrister),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Blacksmith),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Bookbinder),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Bowyer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Brazier),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Brewer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Butler),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Carpenter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Cartographer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Cartwright),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Chandler),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.CityGuide),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Clerk),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Cobbler),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Coffinmaker),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Coiffeur),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Cook),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Coppersmith),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Craftsman),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Dowser),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Dyer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Embalmer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Engineer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Entertainer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.ExoticAnimalTrainer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Farmer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Fletcher),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Footman),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Gemcutter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Goldsmith),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Governess),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Haberdasher),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Healer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Horner),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Hunter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Interpreter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Jeweler),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Laborer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Launderer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Limner),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.LocalCourier),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Locksmith),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Maid),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Masseuse),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Matchmaker),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Midwife),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Miller),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Miner),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Navigator),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Nursemaid),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.OutOfTownCourier),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Painter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Parchmentmaker),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Pewterer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Polisher),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Porter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Potter),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Sage),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.SailorCrewmember),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.SailorMate),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Scribe),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Sculptor),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Shepherd),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Shipwright),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Silversmith),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Skinner),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Soapmaker),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Soothsayer),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Tanner),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Teacher),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Teamster),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Trader),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Trapper),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Valet),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Vintner),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Weaponsmith),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Weaver),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.Wheelwright),
                SkillConstants.Build(SkillConstants.Profession, SkillConstants.Foci.Profession.WildernessGuide),
                SkillConstants.Ride,
                SkillConstants.Search,
                SkillConstants.SenseMotive,
                SkillConstants.SleightOfHand,
                SkillConstants.Spellcraft,
                SkillConstants.Spot,
                SkillConstants.Survival,
                SkillConstants.Swim,
                SkillConstants.Tumble,
                SkillConstants.UseMagicDevice,
                SkillConstants.UseRope
            };

            base.AssertDistinctCollection(GroupConstants.Skills, foci);
        }

        [TestCase(SkillConstants.Craft)]
        [TestCase(SkillConstants.Knowledge)]
        [TestCase(SkillConstants.Perform)]
        [TestCase(SkillConstants.Profession)]
        public void FeatFociForSkillsContainsAllFoci(string skill)
        {
            var skillGroups = collectionMapper.Map(Config.Name, TableNameConstants.Collection.SkillGroups);
            var skillFoci = skillGroups[skill];

            var featFoci = skillFoci.Select(f => SkillConstants.Build(skill, f));

            Assert.That(table[GroupConstants.Skills], Is.SupersetOf(featFoci));
        }
    }
}
