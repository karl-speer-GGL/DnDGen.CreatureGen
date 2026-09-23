using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateTypeCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureAllGroupNames() => AssertAllCreatureGroupNames();

        [Test]
        public void TemplateTypeCreatureGroupNames()
        {
            AssertSubsetCreatureGroupNames(TemplateTypePairs.Select(p => p[0] + p[1]));
        }

        private static IEnumerable<string[]> TemplateTypePairs
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();
                var types = CreatureConstants.Types.GetAll();
                var subtypes = CreatureConstants.Types.Subtypes.GetAll();

                foreach (var template in templates)
                {
                    foreach (var type in types)
                        yield return [template, type];

                    foreach (var subtype in subtypes)
                        yield return [template, subtype];
                }
            }
        }

        private static IEnumerable TemplatesWithTypeFilter => TemplateTypePairs.Select(p => new TestCaseData(p[0], p[1]));

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
