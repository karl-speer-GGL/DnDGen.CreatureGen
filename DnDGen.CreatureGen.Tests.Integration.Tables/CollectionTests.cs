using DnDGen.Infrastructure.Mappers.Collections;
using DnDGen.Infrastructure.Selectors.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.Tables
{
    [TestFixture]
    public abstract class CollectionTests : TableTests
    {
        protected CollectionMapper collectionMapper;
        protected ICollectionSelector collectionSelector;

        protected Dictionary<string, IEnumerable<string>> table;

        [SetUp]
        public void CollectionSetup()
        {
            collectionMapper = GetNewInstanceOf<CollectionMapper>();
            collectionSelector = GetNewInstanceOf<ICollectionSelector>();

            table = collectionMapper.Map(Config.Name, TableName);
        }

        protected void AssertCollectionNames(IEnumerable<string> names)
        {
            AssertUniqueCollection(names, "Collection Names");
            AssertCollection(table.Keys, names, "Table Keys");
        }

        protected IEnumerable<string> GetCollection(string name)
        {
            return table[name];
        }

        protected void AssertUniqueCollection(IEnumerable<string> collection, string message)
        {
            Assert.That(collection, Is.Unique, message);
        }

        public void AssertCollection(string name, params string[] collection)
        {
            Assert.That(table, Contains.Key(name), name);
            AssertCollection(table[name], collection, name);
        }

        private void AssertCollection(IEnumerable<string> source, IEnumerable<string> expected, string message = "")
        {
            Assert.That(source.Order(), Is.EquivalentTo(expected.Order()), message);
        }

        public virtual void AssertDistinctCollection(string name, params string[] collection)
        {
            Assert.That(table, Contains.Key(name), name);
            AssertDistinctCollection(table[name], collection, name);
        }

        public virtual void AssertDistinctCollection(IEnumerable<string> actual, IEnumerable<string> expected, string message = "")
        {
            AssertUniqueCollection(expected, $"{message}: Expected");
            AssertCollection(actual, expected, message);
            AssertUniqueCollection(actual, $"{message}: Actual");
        }
    }
}