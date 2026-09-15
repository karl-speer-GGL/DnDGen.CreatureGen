using DnDGen.Infrastructure.Mappers.Percentiles;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables
{
    [TestFixture]
    public abstract class PercentileTests : TableTests
    {
        protected PercentileMapper percentileMapper;

        protected const string EmptyContent = "";

        private Dictionary<int, string> table;

        [SetUp]
        public void PercentileSetup()
        {
            percentileMapper = GetNewInstanceOf<PercentileMapper>();

            table = percentileMapper.Map(Config.Name, TableName);
        }

        public abstract void TableIsComplete();

        protected void AssertTableIsComplete()
        {
            var percentileRolls = Enumerable.Range(1, 100);
            Assert.That(table.Keys, Is.EquivalentTo(percentileRolls), TableName);
        }

        public virtual void Percentile(int lower, int upper, string content)
        {
            for (var roll = 1; roll < lower; roll++)
                Assert.That(table[roll], Is.Not.EqualTo(content), $"Roll: {roll}");

            for (var roll = lower; roll <= upper; roll++)
                Assert.That(table[roll], Is.EqualTo(content), $"Roll: {roll}");

            for (var roll = upper + 1; roll <= 100; roll++)
                Assert.That(table[roll], Is.Not.EqualTo(content), $"Roll: {roll}");
        }
    }
}