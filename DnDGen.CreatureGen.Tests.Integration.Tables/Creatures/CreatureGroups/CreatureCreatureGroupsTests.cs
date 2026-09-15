using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.Infrastructure.Selectors.Collections;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class CreatureCreatureGroupsTests : CreatureGroupsTestBase
    {
        private ICollectionTypeAndAmountSelector typeAndAmountSelector;

        [SetUp]
        public void Setup()
        {
            typeAndAmountSelector = GetNewInstanceOf<ICollectionTypeAndAmountSelector>();
        }

        [Test]
        public void CreatureGroupNames() => AssertCreatureGroupNames();

        [Test]
        public void CreatureGroup_All()
        {
            var allCreatures = CreatureConstants.GetAll();
            AssertDistinctCollection(GroupConstants.All, [.. allCreatures]);
        }

        [Test]
        public void CreatureGroup_Characters()
        {
            var allCharacters = CreatureConstants.GetAllCharacters();
            AssertDistinctCollection(GroupConstants.Characters, [.. allCharacters]);
        }

        //INFO: Can't construct this programmatically, because to know the minimum abilities requires getting all template aplicators, which requires non-static construction
        //-10 is lowest adjustment
        //Valid ability randomizer must have minimum roll of 1
        //Ghost has Minimum Charisma 6, so biggest required adjustment is 6 - 1 = 5
        //Half-Celestial and Half-Fiend have Minimum Intelligence 4, so biggest required adjustment is 4 - 1 = 3
        [TestCase(AbilityConstants.Charisma, -10)]
        [TestCase(AbilityConstants.Charisma, -9)]
        [TestCase(AbilityConstants.Charisma, -8)]
        [TestCase(AbilityConstants.Charisma, -7)]
        [TestCase(AbilityConstants.Charisma, -6)]
        [TestCase(AbilityConstants.Charisma, -5)]
        [TestCase(AbilityConstants.Charisma, -4)]
        [TestCase(AbilityConstants.Charisma, -3)]
        [TestCase(AbilityConstants.Charisma, -2)]
        [TestCase(AbilityConstants.Charisma, -1)]
        [TestCase(AbilityConstants.Charisma, 0)]
        [TestCase(AbilityConstants.Charisma, 1)]
        [TestCase(AbilityConstants.Charisma, 2)]
        [TestCase(AbilityConstants.Charisma, 3)]
        [TestCase(AbilityConstants.Charisma, 4)]
        [TestCase(AbilityConstants.Charisma, 5)]
        [TestCase(AbilityConstants.Intelligence, -10)]
        [TestCase(AbilityConstants.Intelligence, -9)]
        [TestCase(AbilityConstants.Intelligence, -8)]
        [TestCase(AbilityConstants.Intelligence, -7)]
        [TestCase(AbilityConstants.Intelligence, -6)]
        [TestCase(AbilityConstants.Intelligence, -5)]
        [TestCase(AbilityConstants.Intelligence, -4)]
        [TestCase(AbilityConstants.Intelligence, -3)]
        [TestCase(AbilityConstants.Intelligence, -2)]
        [TestCase(AbilityConstants.Intelligence, -1)]
        [TestCase(AbilityConstants.Intelligence, 0)]
        [TestCase(AbilityConstants.Intelligence, 1)]
        [TestCase(AbilityConstants.Intelligence, 2)]
        [TestCase(AbilityConstants.Intelligence, 3)]
        public void CreatureGroup_MinimumAbilityAdjustment(string ability, int adjustment)
        {
            var allCreatures = CreatureConstants.GetAll();
            var abilityAdjustments = typeAndAmountSelector.SelectAllFrom(Config.Name, TableNameConstants.TypeAndAmount.AbilityAdjustments)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(d => d.Type, d => d.Amount));
            var abilityCreatures = allCreatures
                .Where(c => abilityAdjustments[c].ContainsKey(ability) && abilityAdjustments[c][ability] >= adjustment);

            var groupName = ability + adjustment;
            AssertDistinctCollection(groupName, [.. abilityCreatures]);
        }
    }
}
