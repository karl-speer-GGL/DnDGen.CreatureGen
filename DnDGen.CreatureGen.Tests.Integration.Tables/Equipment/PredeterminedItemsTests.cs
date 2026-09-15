using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Selectors;
using DnDGen.CreatureGen.Selectors.Collections;
using DnDGen.CreatureGen.Selectors.Selections;
using DnDGen.CreatureGen.Tables;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.TreasureGen.Items;
using DnDGen.TreasureGen.Items.Magical;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Equipment
{
    [TestFixture]
    public class PredeterminedItemsTests : CollectionTests
    {
        private IItemSelector itemSelector;
        private IFeatsSelector featsSelector;
        private ICollectionDataSelector<CreatureDataSelection> creatureDataSelector;

        protected override string TableName => TableNameConstants.Collection.PredeterminedItems;

        [SetUp]
        public void Setup()
        {
            itemSelector = GetNewInstanceOf<IItemSelector>();
            featsSelector = GetNewInstanceOf<IFeatsSelector>();
            creatureDataSelector = GetNewInstanceOf<ICollectionDataSelector<CreatureDataSelection>>();
        }

        [Test]
        public void PredeterminedItemNamesAreComplete()
        {
            var allCreatures = CreatureConstants.GetAll();
            AssertCollectionNames(allCreatures);
        }

        [TestCase(CreatureConstants.Aasimar)]
        [TestCase(CreatureConstants.Aboleth)]
        [TestCase(CreatureConstants.Achaierai)]
        [TestCase(CreatureConstants.Allip)]
        [TestCase(CreatureConstants.Androsphinx)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Long_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_MultipleLegs_Tall_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Colossal_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Long_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_MultipleLegs_Tall_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Gargantuan_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Huge)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Long_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_MultipleLegs_Tall_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Huge_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Large)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Long_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall)]
        [TestCase(CreatureConstants.AnimatedObject_Large_MultipleLegs_Tall_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Large_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Large_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Large_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Medium)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_MultipleLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_MultipleLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Medium_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Small)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Small_MultipleLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Small_MultipleLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Small_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Small_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Small_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Flexible)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_MultipleLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_MultipleLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Sheetlike)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_TwoLegs)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_TwoLegs_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wheels_Wooden)]
        [TestCase(CreatureConstants.AnimatedObject_Tiny_Wooden)]
        [TestCase(CreatureConstants.Ankheg)]
        [TestCase(CreatureConstants.Annis)]
        [TestCase(CreatureConstants.SeaHag)]
        [TestCase(CreatureConstants.Ant_Giant_Soldier)]
        [TestCase(CreatureConstants.Ant_Giant_Worker)]
        [TestCase(CreatureConstants.Ant_Giant_Queen)]
        [TestCase(CreatureConstants.Ape)]
        [TestCase(CreatureConstants.Ape_Dire)]
        [TestCase(CreatureConstants.Aranea)]
        [TestCase(CreatureConstants.Arrowhawk_Adult)]
        [TestCase(CreatureConstants.Arrowhawk_Elder)]
        [TestCase(CreatureConstants.Arrowhawk_Juvenile)]
        [TestCase(CreatureConstants.AssassinVine)]
        [TestCase(CreatureConstants.Athach)]
        [TestCase(CreatureConstants.Avoral)]
        [TestCase(CreatureConstants.Azer)]
        [TestCase(CreatureConstants.Babau)]
        [TestCase(CreatureConstants.Baboon)]
        [TestCase(CreatureConstants.Badger)]
        [TestCase(CreatureConstants.Badger_Dire)]
        [TestCase(CreatureConstants.Barghest)]
        [TestCase(CreatureConstants.Barghest_Greater)]
        [TestCase(CreatureConstants.Basilisk)]
        [TestCase(CreatureConstants.Basilisk_Greater)]
        [TestCase(CreatureConstants.Bat)]
        [TestCase(CreatureConstants.Bat_Dire)]
        [TestCase(CreatureConstants.Bat_Swarm)]
        [TestCase(CreatureConstants.Bear_Black)]
        [TestCase(CreatureConstants.Bear_Brown)]
        [TestCase(CreatureConstants.Bear_Dire)]
        [TestCase(CreatureConstants.Bear_Polar)]
        [TestCase(CreatureConstants.Bebilith)]
        [TestCase(CreatureConstants.Bee_Giant)]
        [TestCase(CreatureConstants.Behir)]
        [TestCase(CreatureConstants.Beholder)]
        [TestCase(CreatureConstants.Beholder_Gauth)]
        [TestCase(CreatureConstants.Belker)]
        [TestCase(CreatureConstants.Bison)]
        [TestCase(CreatureConstants.BlackPudding)]
        [TestCase(CreatureConstants.BlackPudding_Elder)]
        [TestCase(CreatureConstants.BlinkDog)]
        [TestCase(CreatureConstants.Boar)]
        [TestCase(CreatureConstants.Boar_Dire)]
        [TestCase(CreatureConstants.Bodak)]
        [TestCase(CreatureConstants.BombardierBeetle_Giant)]
        [TestCase(CreatureConstants.Bugbear)]
        [TestCase(CreatureConstants.Bulette)]
        [TestCase(CreatureConstants.Camel_Bactrian)]
        [TestCase(CreatureConstants.Camel_Dromedary)]
        [TestCase(CreatureConstants.CarrionCrawler)]
        [TestCase(CreatureConstants.Cat)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Colossal)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Gargantuan)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Huge)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Large)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Medium)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Small)]
        [TestCase(CreatureConstants.Centipede_Monstrous_Tiny)]
        [TestCase(CreatureConstants.Centipede_Swarm)]
        [TestCase(CreatureConstants.ChaosBeast)]
        [TestCase(CreatureConstants.Cheetah)]
        [TestCase(CreatureConstants.Chimera_Black)]
        [TestCase(CreatureConstants.Chimera_Blue)]
        [TestCase(CreatureConstants.Chimera_Green)]
        [TestCase(CreatureConstants.Chimera_Red)]
        [TestCase(CreatureConstants.Chimera_White)]
        [TestCase(CreatureConstants.Choker)]
        [TestCase(CreatureConstants.Chuul)]
        [TestCase(CreatureConstants.Cloaker)]
        [TestCase(CreatureConstants.Cockatrice)]
        [TestCase(CreatureConstants.Couatl)]
        [TestCase(CreatureConstants.Criosphinx)]
        [TestCase(CreatureConstants.Crocodile)]
        [TestCase(CreatureConstants.Crocodile_Giant)]
        [TestCase(CreatureConstants.Cryohydra_10Heads)]
        [TestCase(CreatureConstants.Cryohydra_11Heads)]
        [TestCase(CreatureConstants.Cryohydra_12Heads)]
        [TestCase(CreatureConstants.Cryohydra_5Heads)]
        [TestCase(CreatureConstants.Cryohydra_6Heads)]
        [TestCase(CreatureConstants.Cryohydra_7Heads)]
        [TestCase(CreatureConstants.Cryohydra_8Heads)]
        [TestCase(CreatureConstants.Cryohydra_9Heads)]
        [TestCase(CreatureConstants.Darkmantle)]
        [TestCase(CreatureConstants.Deinonychus)]
        [TestCase(CreatureConstants.Delver)]
        [TestCase(CreatureConstants.Destrachan)]
        [TestCase(CreatureConstants.Devourer)]
        [TestCase(CreatureConstants.Digester)]
        [TestCase(CreatureConstants.DisplacerBeast)]
        [TestCase(CreatureConstants.DisplacerBeast_PackLord)]
        [TestCase(CreatureConstants.Djinni)]
        [TestCase(CreatureConstants.Djinni_Noble)]
        [TestCase(CreatureConstants.Dog)]
        [TestCase(CreatureConstants.Dog_Riding)]
        [TestCase(CreatureConstants.Donkey)]
        [TestCase(CreatureConstants.Doppelganger)]
        [TestCase(CreatureConstants.Dragon_Black_Adult)]
        [TestCase(CreatureConstants.Dragon_Black_Ancient)]
        [TestCase(CreatureConstants.Dragon_Black_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Black_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Black_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Black_Old)]
        [TestCase(CreatureConstants.Dragon_Black_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Black_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Black_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Black_Young)]
        [TestCase(CreatureConstants.Dragon_Black_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Blue_Adult)]
        [TestCase(CreatureConstants.Dragon_Blue_Ancient)]
        [TestCase(CreatureConstants.Dragon_Blue_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Blue_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Blue_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Blue_Old)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Blue_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Blue_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Blue_Young)]
        [TestCase(CreatureConstants.Dragon_Blue_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Green_Adult)]
        [TestCase(CreatureConstants.Dragon_Green_Ancient)]
        [TestCase(CreatureConstants.Dragon_Green_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Green_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Green_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Green_Old)]
        [TestCase(CreatureConstants.Dragon_Green_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Green_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Green_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Green_Young)]
        [TestCase(CreatureConstants.Dragon_Green_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Red_Adult)]
        [TestCase(CreatureConstants.Dragon_Red_Ancient)]
        [TestCase(CreatureConstants.Dragon_Red_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Red_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Red_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Red_Old)]
        [TestCase(CreatureConstants.Dragon_Red_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Red_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Red_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Red_Young)]
        [TestCase(CreatureConstants.Dragon_Red_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_White_Adult)]
        [TestCase(CreatureConstants.Dragon_White_Ancient)]
        [TestCase(CreatureConstants.Dragon_White_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_White_Juvenile)]
        [TestCase(CreatureConstants.Dragon_White_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_White_Old)]
        [TestCase(CreatureConstants.Dragon_White_VeryOld)]
        [TestCase(CreatureConstants.Dragon_White_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_White_Wyrm)]
        [TestCase(CreatureConstants.Dragon_White_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_White_Young)]
        [TestCase(CreatureConstants.Dragon_White_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Bronze_Adult)]
        [TestCase(CreatureConstants.Dragon_Bronze_Ancient)]
        [TestCase(CreatureConstants.Dragon_Bronze_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Bronze_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Bronze_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Bronze_Old)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Bronze_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Bronze_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Bronze_Young)]
        [TestCase(CreatureConstants.Dragon_Bronze_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Copper_Adult)]
        [TestCase(CreatureConstants.Dragon_Copper_Ancient)]
        [TestCase(CreatureConstants.Dragon_Copper_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Copper_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Copper_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Copper_Old)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Copper_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Copper_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Copper_Young)]
        [TestCase(CreatureConstants.Dragon_Copper_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Silver_Adult)]
        [TestCase(CreatureConstants.Dragon_Silver_Ancient)]
        [TestCase(CreatureConstants.Dragon_Silver_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Silver_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Silver_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Silver_Old)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Silver_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Silver_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Silver_Young)]
        [TestCase(CreatureConstants.Dragon_Silver_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Gold_Adult)]
        [TestCase(CreatureConstants.Dragon_Gold_Ancient)]
        [TestCase(CreatureConstants.Dragon_Gold_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Gold_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Gold_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Gold_Old)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Gold_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Gold_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Gold_Young)]
        [TestCase(CreatureConstants.Dragon_Gold_YoungAdult)]
        [TestCase(CreatureConstants.Dragon_Brass_Adult)]
        [TestCase(CreatureConstants.Dragon_Brass_Ancient)]
        [TestCase(CreatureConstants.Dragon_Brass_GreatWyrm)]
        [TestCase(CreatureConstants.Dragon_Brass_Juvenile)]
        [TestCase(CreatureConstants.Dragon_Brass_MatureAdult)]
        [TestCase(CreatureConstants.Dragon_Brass_Old)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryOld)]
        [TestCase(CreatureConstants.Dragon_Brass_VeryYoung)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrm)]
        [TestCase(CreatureConstants.Dragon_Brass_Wyrmling)]
        [TestCase(CreatureConstants.Dragon_Brass_Young)]
        [TestCase(CreatureConstants.Dragon_Brass_YoungAdult)]
        [TestCase(CreatureConstants.DragonTurtle)]
        [TestCase(CreatureConstants.Dragonne)]
        [TestCase(CreatureConstants.Dretch)]
        [TestCase(CreatureConstants.Eagle)]
        [TestCase(CreatureConstants.Eagle_Giant)]
        [TestCase(CreatureConstants.Efreeti)]
        [TestCase(CreatureConstants.Elasmosaurus)]
        [TestCase(CreatureConstants.Elephant)]
        [TestCase(CreatureConstants.Elemental_Air_Elder)]
        [TestCase(CreatureConstants.Elemental_Air_Greater)]
        [TestCase(CreatureConstants.Elemental_Air_Huge)]
        [TestCase(CreatureConstants.Elemental_Air_Large)]
        [TestCase(CreatureConstants.Elemental_Air_Medium)]
        [TestCase(CreatureConstants.Elemental_Air_Small)]
        [TestCase(CreatureConstants.Elemental_Earth_Elder)]
        [TestCase(CreatureConstants.Elemental_Earth_Greater)]
        [TestCase(CreatureConstants.Elemental_Earth_Huge)]
        [TestCase(CreatureConstants.Elemental_Earth_Large)]
        [TestCase(CreatureConstants.Elemental_Earth_Medium)]
        [TestCase(CreatureConstants.Elemental_Earth_Small)]
        [TestCase(CreatureConstants.Elemental_Fire_Elder)]
        [TestCase(CreatureConstants.Elemental_Fire_Greater)]
        [TestCase(CreatureConstants.Elemental_Fire_Huge)]
        [TestCase(CreatureConstants.Elemental_Fire_Large)]
        [TestCase(CreatureConstants.Elemental_Fire_Medium)]
        [TestCase(CreatureConstants.Elemental_Fire_Small)]
        [TestCase(CreatureConstants.Elemental_Water_Elder)]
        [TestCase(CreatureConstants.Elemental_Water_Greater)]
        [TestCase(CreatureConstants.Elemental_Water_Huge)]
        [TestCase(CreatureConstants.Elemental_Water_Large)]
        [TestCase(CreatureConstants.Elemental_Water_Medium)]
        [TestCase(CreatureConstants.Elemental_Water_Small)]
        [TestCase(CreatureConstants.EtherealFilcher)]
        [TestCase(CreatureConstants.EtherealMarauder)]
        [TestCase(CreatureConstants.Ettercap)]
        [TestCase(CreatureConstants.FireBeetle_Giant)]
        [TestCase(CreatureConstants.FormianQueen)]
        [TestCase(CreatureConstants.FormianTaskmaster)]
        [TestCase(CreatureConstants.FormianMyrmarch)]
        [TestCase(CreatureConstants.FormianWarrior)]
        [TestCase(CreatureConstants.FormianWorker)]
        [TestCase(CreatureConstants.FrostWorm)]
        [TestCase(CreatureConstants.Gargoyle)]
        [TestCase(CreatureConstants.Gargoyle_Kapoacinth)]
        [TestCase(CreatureConstants.GelatinousCube)]
        [TestCase(CreatureConstants.Ghoul)]
        [TestCase(CreatureConstants.Ghoul_Lacedon)]
        [TestCase(CreatureConstants.Ghoul_Ghast)]
        [TestCase(CreatureConstants.GibberingMouther)]
        [TestCase(CreatureConstants.Girallon)]
        [TestCase(CreatureConstants.Glabrezu)]
        [TestCase(CreatureConstants.Golem_Clay)]
        [TestCase(CreatureConstants.Golem_Flesh)]
        [TestCase(CreatureConstants.Golem_Iron)]
        [TestCase(CreatureConstants.Golem_Stone)]
        [TestCase(CreatureConstants.Golem_Stone_Greater)]
        [TestCase(CreatureConstants.Gorgon)]
        [TestCase(CreatureConstants.GrayRender)]
        [TestCase(CreatureConstants.GreenHag)]
        [TestCase(CreatureConstants.Grick)]
        [TestCase(CreatureConstants.Griffon)]
        [TestCase(CreatureConstants.Gynosphinx)]
        [TestCase(CreatureConstants.BarbedDevil_Hamatula)]
        [TestCase(CreatureConstants.Harpy)]
        [TestCase(CreatureConstants.Hawk)]
        [TestCase(CreatureConstants.Hellcat_Bezekira)]
        [TestCase(CreatureConstants.HellHound)]
        [TestCase(CreatureConstants.Hellwasp_Swarm)]
        [TestCase(CreatureConstants.Hezrou)]
        [TestCase(CreatureConstants.Hieracosphinx)]
        [TestCase(CreatureConstants.Hippogriff)]
        [TestCase(CreatureConstants.Homunculus)]
        [TestCase(CreatureConstants.Horse_Heavy)]
        [TestCase(CreatureConstants.Horse_Heavy_War)]
        [TestCase(CreatureConstants.Horse_Light)]
        [TestCase(CreatureConstants.Horse_Light_War)]
        [TestCase(CreatureConstants.Howler)]
        [TestCase(CreatureConstants.Hydra_10Heads)]
        [TestCase(CreatureConstants.Hydra_11Heads)]
        [TestCase(CreatureConstants.Hydra_12Heads)]
        [TestCase(CreatureConstants.Hydra_5Heads)]
        [TestCase(CreatureConstants.Hydra_6Heads)]
        [TestCase(CreatureConstants.Hydra_7Heads)]
        [TestCase(CreatureConstants.Hydra_8Heads)]
        [TestCase(CreatureConstants.Hydra_9Heads)]
        [TestCase(CreatureConstants.Hyena)]
        [TestCase(CreatureConstants.Imp)]
        [TestCase(CreatureConstants.InvisibleStalker)]
        [TestCase(CreatureConstants.Kolyarut)]
        [TestCase(CreatureConstants.Kraken)]
        [TestCase(CreatureConstants.Krenshar)]
        [TestCase(CreatureConstants.KuoToa)]
        [TestCase(CreatureConstants.Lamia)]
        [TestCase(CreatureConstants.Lammasu)]
        [TestCase(CreatureConstants.LanternArchon)]
        [TestCase(CreatureConstants.Lemure)]
        [TestCase(CreatureConstants.Leonal)]
        [TestCase(CreatureConstants.Leopard)]
        [TestCase(CreatureConstants.Lillend)]
        [TestCase(CreatureConstants.Lion)]
        [TestCase(CreatureConstants.Lion_Dire)]
        [TestCase(CreatureConstants.Lizard)]
        [TestCase(CreatureConstants.Lizard_Monitor)]
        [TestCase(CreatureConstants.Locust_Swarm)]
        [TestCase(CreatureConstants.Magmin)] //Nonflammable
        [TestCase(CreatureConstants.MantaRay)]
        [TestCase(CreatureConstants.Manticore)]
        [TestCase(CreatureConstants.Marut)]
        [TestCase(CreatureConstants.Megaraptor)]
        [TestCase(CreatureConstants.Mephit_Air)]
        [TestCase(CreatureConstants.Mephit_Dust)]
        [TestCase(CreatureConstants.Mephit_Earth)]
        [TestCase(CreatureConstants.Mephit_Fire)]
        [TestCase(CreatureConstants.Mephit_Ice)]
        [TestCase(CreatureConstants.Mephit_Magma)]
        [TestCase(CreatureConstants.Mephit_Ooze)]
        [TestCase(CreatureConstants.Mephit_Salt)]
        [TestCase(CreatureConstants.Mephit_Steam)]
        [TestCase(CreatureConstants.Mephit_Water)]
        [TestCase(CreatureConstants.Mimic)]
        [TestCase(CreatureConstants.MindFlayer)]
        [TestCase(CreatureConstants.Minotaur)]
        [TestCase(CreatureConstants.Mohrg)]
        [TestCase(CreatureConstants.Monkey)]
        [TestCase(CreatureConstants.Mule)]
        [TestCase(CreatureConstants.Mummy)]
        [TestCase(CreatureConstants.Naga_Dark)]
        [TestCase(CreatureConstants.Naga_Guardian)]
        [TestCase(CreatureConstants.Naga_Spirit)]
        [TestCase(CreatureConstants.Naga_Water)]
        [TestCase(CreatureConstants.Nalfeshnee)]
        [TestCase(CreatureConstants.NightHag)]
        [TestCase(CreatureConstants.Nightcrawler)]
        [TestCase(CreatureConstants.Nightmare)]
        [TestCase(CreatureConstants.Nightmare_Cauchemar)]
        [TestCase(CreatureConstants.Nightwalker)]
        [TestCase(CreatureConstants.Nightwing)]
        [TestCase(CreatureConstants.Nymph)]
        [TestCase(CreatureConstants.Octopus)]
        [TestCase(CreatureConstants.Octopus_Giant)]
        [TestCase(CreatureConstants.GrayOoze)]
        [TestCase(CreatureConstants.OchreJelly)]
        [TestCase(CreatureConstants.Otyugh)]
        [TestCase(CreatureConstants.BoneDevil_Osyluth)]
        [TestCase(CreatureConstants.Owl)]
        [TestCase(CreatureConstants.Owl_Giant)]
        [TestCase(CreatureConstants.Owlbear)]
        [TestCase(CreatureConstants.Pegasus)]
        [TestCase(CreatureConstants.PhantomFungus)]
        [TestCase(CreatureConstants.PhaseSpider)]
        [TestCase(CreatureConstants.Phasm)]
        [TestCase(CreatureConstants.PitFiend)]
        [TestCase(CreatureConstants.Pony)]
        [TestCase(CreatureConstants.Pony_War)]
        [TestCase(CreatureConstants.Porpoise)]
        [TestCase(CreatureConstants.PrayingMantis_Giant)]
        [TestCase(CreatureConstants.Pseudodragon)]
        [TestCase(CreatureConstants.PurpleWorm)]
        [TestCase(CreatureConstants.Pyrohydra_10Heads)]
        [TestCase(CreatureConstants.Pyrohydra_11Heads)]
        [TestCase(CreatureConstants.Pyrohydra_12Heads)]
        [TestCase(CreatureConstants.Pyrohydra_5Heads)]
        [TestCase(CreatureConstants.Pyrohydra_6Heads)]
        [TestCase(CreatureConstants.Pyrohydra_7Heads)]
        [TestCase(CreatureConstants.Pyrohydra_8Heads)]
        [TestCase(CreatureConstants.Pyrohydra_9Heads)]
        [TestCase(CreatureConstants.Quasit)]
        [TestCase(CreatureConstants.Rakshasa)]
        [TestCase(CreatureConstants.Rast)]
        [TestCase(CreatureConstants.Rat)]
        [TestCase(CreatureConstants.Rat_Dire)]
        [TestCase(CreatureConstants.Rat_Swarm)]
        [TestCase(CreatureConstants.Raven)]
        [TestCase(CreatureConstants.Ravid)]
        [TestCase(CreatureConstants.RazorBoar)]
        [TestCase(CreatureConstants.Remorhaz)]
        [TestCase(CreatureConstants.Retriever)]
        [TestCase(CreatureConstants.Rhinoceras)]
        [TestCase(CreatureConstants.Roc)]
        [TestCase(CreatureConstants.Roper)]
        [TestCase(CreatureConstants.RustMonster)]
        [TestCase(CreatureConstants.Salamander_Average)]
        [TestCase(CreatureConstants.Salamander_Flamebrother)]
        [TestCase(CreatureConstants.Salamander_Noble)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Colossal)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Gargantuan)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Huge)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Large)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Medium)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Small)]
        [TestCase(CreatureConstants.Scorpion_Monstrous_Tiny)]
        [TestCase(CreatureConstants.SeaCat)]
        [TestCase(CreatureConstants.Shadow)]
        [TestCase(CreatureConstants.Shadow_Greater)]
        [TestCase(CreatureConstants.ShadowMastiff)]
        [TestCase(CreatureConstants.ShamblingMound)]
        [TestCase(CreatureConstants.Shark_Dire)]
        [TestCase(CreatureConstants.Shark_Huge)]
        [TestCase(CreatureConstants.Shark_Large)]
        [TestCase(CreatureConstants.Shark_Medium)]
        [TestCase(CreatureConstants.ShieldGuardian)]
        [TestCase(CreatureConstants.ShockerLizard)]
        [TestCase(CreatureConstants.Shrieker)]
        [TestCase(CreatureConstants.Skum)]
        [TestCase(CreatureConstants.Slaad_Blue)]
        [TestCase(CreatureConstants.Slaad_Death)]
        [TestCase(CreatureConstants.Slaad_Gray)]
        [TestCase(CreatureConstants.Slaad_Green)]
        [TestCase(CreatureConstants.Slaad_Red)]
        [TestCase(CreatureConstants.Snake_Constrictor)]
        [TestCase(CreatureConstants.Snake_Constrictor_Giant)]
        [TestCase(CreatureConstants.Snake_Viper_Huge)]
        [TestCase(CreatureConstants.Snake_Viper_Large)]
        [TestCase(CreatureConstants.Snake_Viper_Medium)]
        [TestCase(CreatureConstants.Snake_Viper_Small)]
        [TestCase(CreatureConstants.Snake_Viper_Tiny)]
        [TestCase(CreatureConstants.Spectre)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Colossal)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Gargantuan)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Huge)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Large)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Medium)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Small)]
        [TestCase(CreatureConstants.Spider_Monstrous_Hunter_Tiny)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Colossal)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Gargantuan)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Huge)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Large)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Medium)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Small)]
        [TestCase(CreatureConstants.Spider_Monstrous_WebSpinner_Tiny)]
        [TestCase(CreatureConstants.Spider_Swarm)]
        [TestCase(CreatureConstants.SpiderEater)]
        [TestCase(CreatureConstants.Squid)]
        [TestCase(CreatureConstants.Squid_Giant)]
        [TestCase(CreatureConstants.StagBeetle_Giant)]
        [TestCase(CreatureConstants.Stirge)]
        [TestCase(CreatureConstants.Succubus)]
        [TestCase(CreatureConstants.Tarrasque)]
        [TestCase(CreatureConstants.Tendriculos)]
        [TestCase(CreatureConstants.Thoqqua)]
        [TestCase(CreatureConstants.Tiger)]
        [TestCase(CreatureConstants.Tiger_Dire)]
        [TestCase(CreatureConstants.Toad)]
        [TestCase(CreatureConstants.Tojanida_Adult)]
        [TestCase(CreatureConstants.Tojanida_Elder)]
        [TestCase(CreatureConstants.Tojanida_Juvenile)]
        [TestCase(CreatureConstants.Treant)]
        [TestCase(CreatureConstants.Triceratops)]
        [TestCase(CreatureConstants.Troll)]
        [TestCase(CreatureConstants.Troll_Scrag)]
        [TestCase(CreatureConstants.Tyrannosaurus)]
        [TestCase(CreatureConstants.UmberHulk)]
        [TestCase(CreatureConstants.UmberHulk_TrulyHorrid)]
        [TestCase(CreatureConstants.Unicorn)]
        [TestCase(CreatureConstants.VampireSpawn)]
        [TestCase(CreatureConstants.Vargouille)]
        [TestCase(CreatureConstants.VioletFungus)]
        [TestCase(CreatureConstants.Vrock)]
        [TestCase(CreatureConstants.Wasp_Giant)]
        [TestCase(CreatureConstants.Weasel)]
        [TestCase(CreatureConstants.Weasel_Dire)]
        [TestCase(CreatureConstants.Whale_Baleen)]
        [TestCase(CreatureConstants.Whale_Cachalot)]
        [TestCase(CreatureConstants.Whale_Orca)]
        [TestCase(CreatureConstants.Wight)]
        [TestCase(CreatureConstants.WillOWisp)]
        [TestCase(CreatureConstants.WinterWolf)]
        [TestCase(CreatureConstants.Wolf)]
        [TestCase(CreatureConstants.Wolf_Dire)]
        [TestCase(CreatureConstants.Wolverine)]
        [TestCase(CreatureConstants.Wolverine_Dire)]
        [TestCase(CreatureConstants.Worg)]
        [TestCase(CreatureConstants.Wraith)]
        [TestCase(CreatureConstants.Wraith_Dread)]
        [TestCase(CreatureConstants.Wyvern)]
        [TestCase(CreatureConstants.Xill)]
        [TestCase(CreatureConstants.Xorn_Average)]
        [TestCase(CreatureConstants.Xorn_Elder)]
        [TestCase(CreatureConstants.Xorn_Minor)]
        [TestCase(CreatureConstants.YethHound)]
        [TestCase(CreatureConstants.Yrthak)]
        [TestCase(CreatureConstants.Centaur)]
        [TestCase(CreatureConstants.Derro)]
        [TestCase(CreatureConstants.Derro_Sane)]
        [TestCase(CreatureConstants.Dwarf_Deep)]
        [TestCase(CreatureConstants.Dwarf_Duergar)]
        [TestCase(CreatureConstants.Dwarf_Hill)]
        [TestCase(CreatureConstants.Dwarf_Mountain)]
        [TestCase(CreatureConstants.Elf_Aquatic)]
        [TestCase(CreatureConstants.Elf_Drow)]
        [TestCase(CreatureConstants.Elf_Gray)]
        [TestCase(CreatureConstants.Elf_Half)]
        [TestCase(CreatureConstants.Elf_High)]
        [TestCase(CreatureConstants.Elf_Wild)]
        [TestCase(CreatureConstants.Elf_Wood)]
        [TestCase(CreatureConstants.Human)]
        [TestCase(CreatureConstants.Orc)]
        [TestCase(CreatureConstants.Orc_Half)]
        [TestCase(CreatureConstants.Halfling_Deep)]
        [TestCase(CreatureConstants.Halfling_Lightfoot)]
        [TestCase(CreatureConstants.Halfling_Tallfellow)]
        [TestCase(CreatureConstants.Gnome_Forest)]
        [TestCase(CreatureConstants.Gnome_Rock)]
        [TestCase(CreatureConstants.Gnome_Svirfneblin)]
        [TestCase(CreatureConstants.Hobgoblin)]
        [TestCase(CreatureConstants.Goblin)]
        [TestCase(CreatureConstants.Kobold)]
        [TestCase(CreatureConstants.Merfolk)]
        [TestCase(CreatureConstants.Tiefling)]
        [TestCase(CreatureConstants.Scorpionfolk)]
        [TestCase(CreatureConstants.YuanTi_Pureblood)]
        [TestCase(CreatureConstants.YuanTi_Abomination)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeArms)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeHead)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTail)]
        [TestCase(CreatureConstants.YuanTi_Halfblood_SnakeTailAndHumanLegs)]
        [TestCase(CreatureConstants.Drider)]
        [TestCase(CreatureConstants.Dryad)]
        [TestCase(CreatureConstants.Gnoll)]
        [TestCase(CreatureConstants.Janni)]
        [TestCase(CreatureConstants.Lizardfolk)]
        [TestCase(CreatureConstants.Medusa)]
        [TestCase(CreatureConstants.Nixie)]
        [TestCase(CreatureConstants.OgreMage)]
        [TestCase(CreatureConstants.Sahuagin)]
        [TestCase(CreatureConstants.Sahuagin_Malenti)]
        [TestCase(CreatureConstants.Sahuagin_Mutant)]
        [TestCase(CreatureConstants.Triton)]
        public void CreatureHasNoPredeterminedItems(string entry)
        {
            base.AssertDistinctCollection(entry);
        }

        [TestCase(CreatureConstants.Angel_AstralDeva, WeaponConstants.HeavyMace, ItemTypeConstants.Weapon, 3, SpecialAbilityConstants.Disruption)]
        [TestCase(CreatureConstants.BeardedDevil_Barbazu, WeaponConstants.Glaive, ItemTypeConstants.Weapon, 0, "", "Saw-toothed")]
        [TestCase(CreatureConstants.Ghaele, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 4, SpecialAbilityConstants.Holy)]
        [TestCase(CreatureConstants.Giant_Cloud, WeaponConstants.Morningstar, ItemTypeConstants.Weapon, 0, "")]
        [TestCase(CreatureConstants.Grimlock, WeaponConstants.Battleaxe, ItemTypeConstants.Weapon, 0, "", "Stone")]
        [TestCase(CreatureConstants.HornedDevil_Cornugon, WeaponConstants.SpikedChain, ItemTypeConstants.Weapon, 0, "")]
        [TestCase(CreatureConstants.HoundArchon, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 0, "")]
        [TestCase(CreatureConstants.IceDevil_Gelugon, WeaponConstants.Spear, ItemTypeConstants.Weapon, 0, "")]
        [TestCase(CreatureConstants.HellHound_NessianWarhound, ArmorConstants.ChainShirt, ItemTypeConstants.Armor, 2, "", "Barding")]
        [TestCase(CreatureConstants.Angel_Planetar, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 3, "")]
        [TestCase(CreatureConstants.TrumpetArchon, WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 4, "", "The Archon Trumpet")]
        public void CreatureHasOnePresetItem(string entry, string itemName, string itemType, int itemBonus, string abilityName, params string[] traits)
        {
            var isMagic = itemBonus > 0;
            var setItem = FormatSetItem(itemName, itemType, itemBonus, abilityName, 0, isMagic, traits);
            base.AssertDistinctCollection(entry, new[] { setItem });
        }

        private string FormatSetItem(string itemName, string itemType, int itemBonus = 0, string abilityName = "", int abilityBonus = 0, bool isMagic = false, params string[] traits)
        {
            var item = new Item();
            item.Name = itemName;
            item.ItemType = itemType;
            item.Magic.Bonus = itemBonus;

            if (!string.IsNullOrEmpty(abilityName))
            {
                var ability = new SpecialAbility();
                ability.Name = abilityName;
                ability.BonusEquivalent = abilityBonus;

                item.Magic.SpecialAbilities = new[] { ability };
            }

            item.IsMagical = isMagic;

            foreach (var trait in traits)
                item.Traits.Add(trait);

            return itemSelector.SelectFrom(item);
        }

        [Test]
        public void BalorItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Vorpal, 0, true);
            var whip = FormatSetItem(WeaponConstants.Whip, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Flaming, 0, true, "tipped with hooks, spikes, and balls, dealing slashing and bludgeoning damage");

            base.AssertCollection(CreatureConstants.Balor, new[] { longsword, whip });
        }

        [Test]
        public void BralaniItems()
        {
            var scimitar = FormatSetItem(WeaponConstants.Scimitar, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Holy, 0, true);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Holy, 0, true, "+4 Strength bonus");
            base.AssertCollection(CreatureConstants.Bralani, new[] { scimitar, longbow });
        }

        [Test]
        public void ChainDevilItems()
        {
            var spikedChains = new[]
            {
                FormatSetItem(WeaponConstants.SpikedChain, ItemTypeConstants.Weapon),
                FormatSetItem(WeaponConstants.SpikedChain, ItemTypeConstants.Weapon),
            };

            base.AssertCollection(CreatureConstants.ChainDevil_Kyton, spikedChains);
        }

        [Test]
        public void ErinyesItems()
        {
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 1, SpecialAbilityConstants.Flaming, 0, true, "+5 Strength bonus");
            var rope = FormatSetItem("Rope, 50 ft.", ItemTypeConstants.Tool);

            base.AssertCollection(CreatureConstants.Erinyes, new[] { longbow, rope });
        }

        [Test]
        public void EttinItems()
        {
            var morningstar = FormatSetItem(WeaponConstants.Morningstar, ItemTypeConstants.Weapon);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon);

            base.AssertCollection(CreatureConstants.Ettin, new[] { morningstar, morningstar, javelin, javelin });
        }

        [Test]
        public void FireGiantItems()
        {
            var armor = FormatSetItem(ArmorConstants.HalfPlate, ItemTypeConstants.Armor, 0, string.Empty, 0, false, "Blackened steel");

            base.AssertCollection(CreatureConstants.Giant_Fire, new[] { armor });
        }

        [Test]
        public void FrostGiantItems()
        {
            var greataxe = FormatSetItem(WeaponConstants.Greataxe, ItemTypeConstants.Weapon);
            var armor = FormatSetItem(ArmorConstants.ChainShirt, ItemTypeConstants.Armor);

            base.AssertCollection(CreatureConstants.Giant_Frost, new[] { greataxe, armor });
        }

        [Test]
        public void HillGiantItems()
        {
            var greatclub = FormatSetItem(WeaponConstants.Greatclub, ItemTypeConstants.Weapon);
            var armor = FormatSetItem(ArmorConstants.HideArmor, ItemTypeConstants.Armor);

            base.AssertCollection(CreatureConstants.Giant_Hill, new[] { greatclub, armor });
        }

        [TestCase(CreatureConstants.Grig)]
        [TestCase(CreatureConstants.Grig_WithFiddle)]
        public void GrigItems(string creature)
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon);

            base.AssertCollection(creature, new[] { shortSword, longbow });
        }

        [Test]
        public void MarilithItems()
        {
            var longsword = FormatSetItem(WeaponConstants.Longsword, ItemTypeConstants.Weapon);

            AssertCollection(CreatureConstants.Marilith,
            [
                longsword,
                longsword,
                longsword,
                longsword,
                longsword,
                longsword
            ]);
        }

        [Test]
        public void OgreItems()
        {
            var greatclub = FormatSetItem(WeaponConstants.Greatclub, ItemTypeConstants.Weapon);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon);

            base.AssertCollection(CreatureConstants.Ogre, new[] { greatclub, javelin });
        }

        [Test]
        public void OgreMerrowItems()
        {
            var longspear = FormatSetItem(WeaponConstants.Longspear, ItemTypeConstants.Weapon);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon);

            base.AssertCollection(CreatureConstants.Ogre_Merrow, new[] { longspear, javelin });
        }

        [TestCase(CreatureConstants.Pixie)]
        [TestCase(CreatureConstants.Pixie_WithIrresistibleDance)]
        public void PixieItems(string creatureName)
        {
            var shortSword = FormatSetItem(WeaponConstants.ShortSword, ItemTypeConstants.Weapon);
            var longbow = FormatSetItem(WeaponConstants.Longbow, ItemTypeConstants.Weapon);
            var memoryArrow = FormatSetItem(WeaponConstants.Arrow, ItemTypeConstants.Weapon, 0, string.Empty, 0, true, "Causes memory loss (see special attack)");
            var sleepArrow = FormatSetItem(WeaponConstants.Arrow, ItemTypeConstants.Weapon, 0, string.Empty, 0, true, "Causes sleep (see special attack)");

            base.AssertCollection(creatureName, new[] { shortSword, longbow, memoryArrow, sleepArrow });
        }

        [Test]
        public void SatyrItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon);

            base.AssertCollection(CreatureConstants.Satyr, new[] { dagger, shortbow });
        }

        [Test]
        public void SatyrWithPipesItems()
        {
            var dagger = FormatSetItem(WeaponConstants.Dagger, ItemTypeConstants.Weapon);
            var shortbow = FormatSetItem(WeaponConstants.Shortbow, ItemTypeConstants.Weapon);
            var pipes = FormatSetItem("Pipes", ItemTypeConstants.Tool);

            base.AssertCollection(CreatureConstants.Satyr_WithPipes, new[] { dagger, shortbow, pipes });
        }

        [Test]
        public void SolarItems()
        {
            var greatsword = FormatSetItem(WeaponConstants.Greatsword, ItemTypeConstants.Weapon, 5, SpecialAbilityConstants.Dancing, 0, true);
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 2, "Creates slaying arrows keyed to any creature type or subtype", 5, true, "+5 Strength bonus");

            base.AssertCollection(CreatureConstants.Angel_Solar, new[] { greatsword, longbow });
        }

        [TestCase(CreatureConstants.Giant_Stone)]
        [TestCase(CreatureConstants.Giant_Stone_Elder)]
        public void StoneGiantItems(string creatureName)
        {
            var club = FormatSetItem(WeaponConstants.Greatclub, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, "Stone");
            var armor = FormatSetItem(ArmorConstants.HideArmor, ItemTypeConstants.Armor, 0, string.Empty, 0, false, "dyed in shades of brown and gray to match the stone around them");

            base.AssertCollection(creatureName, new[] { club, armor });
        }

        [Test]
        public void StormGiantItems()
        {
            var longbow = FormatSetItem(WeaponConstants.CompositeLongbow, ItemTypeConstants.Weapon, 0, string.Empty, 0, false, "+14 Strength bonus");

            base.AssertCollection(CreatureConstants.Giant_Storm, new[] { longbow });
        }

        [Test]
        public void TitanItems()
        {
            var warhammer = FormatSetItem(WeaponConstants.Warhammer, ItemTypeConstants.Weapon, 3, string.Empty, 0, true, TraitConstants.SpecialMaterials.Adamantine);
            var armor = FormatSetItem(ArmorConstants.HalfPlate, ItemTypeConstants.Armor, 4, string.Empty, 0, true);

            base.AssertCollection(CreatureConstants.Titan, new[] { warhammer, armor });
        }

        [Test]
        public void TroglodyteItems()
        {
            var club = FormatSetItem(WeaponConstants.Club, ItemTypeConstants.Weapon);
            var javelin = FormatSetItem(WeaponConstants.Javelin, ItemTypeConstants.Weapon);

            base.AssertCollection(CreatureConstants.Troglodyte, new[] { club, javelin });
        }

        [Test]
        public void ZelekhutItems()
        {
            var chain = FormatSetItem(WeaponConstants.SpikedChain, ItemTypeConstants.Weapon);

            AssertCollection(CreatureConstants.Zelekhut, [chain, chain]);
        }

        [Test]
        public void BUG_IfCreatureHasOversizedFeat_WeaponsDoNotHaveSize()
        {
            var creatures = CreatureConstants.GetAll();
            var creatureDatas = creatureDataSelector.SelectAllFrom(Config.Name, TableNameConstants.Collection.CreatureData);

            foreach (var creature in creatures)
            {
                Assert.That(table, Contains.Key(creature));

                if (!table[creature].Any())
                {
                    continue;
                }

                var creatureType = new CreatureType(creatureDatas[creature].Single().Types);
                var feats = featsSelector.SelectSpecialQualities(creature, creatureType);
                if (!feats.Any(f => f.Feat == FeatConstants.SpecialQualities.OversizedWeapon))
                {
                    continue;
                }

                var sizes = SizeConstants.GetOrdered();

                foreach (var itemTemplate in table[creature])
                {
                    var item = itemSelector.SelectFrom(itemTemplate);
                    if (item.ItemType != ItemTypeConstants.Weapon)
                        continue;

                    var sizeTraits = item.Traits.Intersect(sizes);
                    Assert.That(sizeTraits, Is.Empty, item.Name);
                }
            }
        }
    }
}
