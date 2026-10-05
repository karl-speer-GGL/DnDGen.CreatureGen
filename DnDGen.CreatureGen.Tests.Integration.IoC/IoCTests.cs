using NUnit.Framework;

namespace DnDGen.CreatureGen.Tests.Integration.IoC
{
    [TestFixture]
    public abstract class IoCTests : IntegrationTests
    {
        protected void AssertNotSingleton<T>()
            where T : class
        {
            var first = GetNewInstanceOf<T>();
            var second = GetNewInstanceOf<T>();
            Assert.That(first, Is.Not.SameAs(second));
        }

        protected void AssertSingleton<T>()
            where T : class
        {
            var first = GetNewInstanceOf<T>();
            var second = GetNewInstanceOf<T>();
            Assert.That(first, Is.SameAs(second));
        }

        protected void AssertNotSingleton<T>(string name)
            where T : class
        {
            var first = GetNewInstanceOf<T>(name);
            var second = GetNewInstanceOf<T>(name);
            Assert.That(first, Is.Not.SameAs(second));
        }

        protected I AssertNamedIsInstanceOf<I, T>(string name)
            where T : I
        {
            var item = GetNewInstanceOf<I>(name);
            Assert.That(item, Is.InstanceOf<T>());

            return item;
        }
    }
}