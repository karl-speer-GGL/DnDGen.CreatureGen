using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Tables;
using NUnit.Framework;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures
{
    [TestFixture]
    public class TemplateGroupsTests : CollectionTests
    {
        protected override string TableName => TableNameConstants.Collection.TemplateGroups;

        [Test]
        public void TemplateGroupNames()
        {
            var entries = new[]
            {
                GroupConstants.All,
            };

            AssertCollectionNames(entries);
        }

        [Test]
        public void AllGroup()
        {
            var allTemplates = CreatureConstants.Templates.GetAll().Except([CreatureConstants.Templates.None]);
            AssertDistinctCollection(GroupConstants.All, [.. allTemplates]);
        }
    }
}
