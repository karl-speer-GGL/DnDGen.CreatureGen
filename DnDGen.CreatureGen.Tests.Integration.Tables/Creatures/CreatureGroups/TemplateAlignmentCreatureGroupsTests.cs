using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Templates;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables.Creatures.CreatureGroups
{
    [TestFixture]
    public class TemplateAlignmentCreatureGroupsTests : CreatureGroupsTestBase
    {
        [Test]
        public void CreatureAllGroupNames() => AssertAllCreatureGroupNames();

        [Test]
        public void TemplateAlignmentCreatureGroupNames()
        {
            AssertSubsetCreatureGroupNames(TemplateAlignmentPairs.Select(p => p[0] + p[1]));
        }

        private static IEnumerable<string[]> TemplateAlignmentPairs
        {
            get
            {
                var templates = CreatureConstants.Templates.GetAll();

                foreach (var template in templates)
                    foreach (var alignment in Alignments)
                        yield return [template, alignment];
            }
        }

        private static IEnumerable TemplatesWithAlignmentFilter => TemplateAlignmentPairs.Select(p => new TestCaseData(p[0], p[1]));

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
