using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using NUnit.Framework;
using System.Collections;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateTypeCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureGroupNames() => AssertCreatureGroupNames();

        private static IEnumerable TemplatesWithTypeFilter
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();
                var types = CreatureConstants.Types.GetAll();
                var subtypes = CreatureConstants.Types.Subtypes.GetAll();

                foreach (var template in templates)
                {
                    foreach (var type in types)
                        yield return new TestCaseData(template, type);

                    foreach (var subtype in subtypes)
                        yield return new TestCaseData(template, subtype);
                }
            }
        }

        [TestCaseSource(nameof(TemplatesWithTypeFilter))]
        public void CreatureGroup_Template_ResultsInType(string template, string type)
        {
            var sourcePrototypes = GetAllPrototypes();
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { Types = [type] }))
                .Select(p => p.Name)
                .Distinct();

            var groupName = template + type;
            AssertDistinctCollection(groupName, [.. templateCreatures]);
        }
    }
}
