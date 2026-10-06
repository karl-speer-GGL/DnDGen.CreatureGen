using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DnDGen.CreatureGen.Tests.Integration.Creatures
{
    [TestFixture]
    public class CreatureTests : IntegrationTests
    {
        private ICreatureGenerator creatureGenerator;

        [SetUp]
        public void Setup()
        {
            creatureGenerator = GetNewInstanceOf<ICreatureGenerator>();
        }

        [Test]
        public void DEBUG_Generate_GenerateBetaCreature()
        {
            var creature = creatureGenerator.Generate(false, CreatureConstants.Lizardfolk);
            Assert.That(creature, Is.Not.Null);
        }

        [TestCaseSource(typeof(CreatureTestData), nameof(CreatureTestData.Creatures))]
        public void BUG_Generate_CanDeserializeCreature(string creatureName)
        {
            var creature = creatureGenerator.Generate(false, creatureName);
            var serialized = JsonConvert.SerializeObject(creature);
            var deserialized = JsonConvert.DeserializeObject<Creature>(serialized);
            Assert.That(deserialized.Summary, Is.EqualTo(creature.Summary));
        }
    }
}
