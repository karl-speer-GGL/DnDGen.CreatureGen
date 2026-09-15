using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using NUnit.Framework;
using System.Collections;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateAlignmentCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureGroupNames() => AssertCreatureGroupNames();

        private static IEnumerable TemplatesWithAlignmentFilter
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();

                foreach (var template in templates)
                    foreach (var alignment in Alignments)
                        yield return new TestCaseData(template, alignment);
            }
        }

        [TestCaseSource(nameof(TemplatesWithAlignmentFilter))]
        public void CreatureGroup_Template_ResultsInAlignment(string template, string alignment)
        {
            var sourcePrototypes = GetAllPrototypes();
            var applicator = GetNewInstanceOf<TemplateApplicator>(template);

            var templateCreatures = sourcePrototypes
                .Where(p => applicator.IsCompatible(p, new() { Alignments = [alignment] }))
                .Select(p => p.Name)
                .Distinct();

            var groupName = template + alignment;
            AssertDistinctCollection(groupName, [.. templateCreatures]);
        }
    }
}
