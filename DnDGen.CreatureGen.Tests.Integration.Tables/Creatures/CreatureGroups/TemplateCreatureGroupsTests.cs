using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureGroupNames() => AssertCreatureGroupNames();

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_Template(string template)
        {
            var allCreatures = CreatureConstants.GetAll();
            AssertTemplateGroup(template, allCreatures, false);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_TemplateAsCharacter(string template)
        {
            //INFO: We can do this as a shortcut, because if asCharacter = true, then our set of base creatures is only characters.
            //Setting asCharacter = true and generating a creature that can't be a character produces a compatibility error.
            var allCharacters = CreatureConstants.GetAllCharacters();
            AssertTemplateGroup(template, allCharacters, true);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Templates))]
        public void CreatureGroup_Template_HasNonEmptyVariation(string template)
        {
            Assert.That(table, Contains.Key(template + bool.TrueString)
                .And.ContainKey(template + bool.FalseString));

            var allVariations = table[template + bool.FalseString]
                .Union(table[template + bool.TrueString]);
            Assert.That(allVariations, Is.Not.Empty);
        }
    }
}
