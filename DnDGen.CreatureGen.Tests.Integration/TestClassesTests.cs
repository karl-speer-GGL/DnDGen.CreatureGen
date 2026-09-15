using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DnDGen.CreatureGen.Tests.Integration
{
    [TestFixture]
    public class TestClassesTests
    {
        private const int TestLimit = 2048;

        [Test]
        public void TestClassesDoNotHaveTooManyTests()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var classes = assembly.GetTypes();
            var classesOverLimit = new List<string>();

            foreach (var testClass in classes)
            {
                var methods = testClass.GetMethods();
                var activeTests = methods.Where(m => IsActiveTest(m, testClass));

                if (!activeTests.Any())
                    continue;

                int GetTestCaseSourceCount(TestCaseSourceAttribute tcs) => TestClassesTests.GetTestCaseSourceCount(tcs, testClass);

                var testsCount = activeTests.Sum(m => m.GetCustomAttributes<TestAttribute>(true).Count());
                var testCasesCount = activeTests.Sum(m => m.GetCustomAttributes<TestCaseAttribute>().Count(TestCaseIsActive));
                var testCaseSourcesCount = activeTests.Sum(m => m.GetCustomAttributes<TestCaseSourceAttribute>().Sum(GetTestCaseSourceCount));
                var testsTotal = testsCount + testCasesCount + testCaseSourcesCount;

                if (testsTotal > TestLimit)
                    classesOverLimit.Add($"{testClass.Name}: {testsTotal}");
            }

            Assert.That(classesOverLimit, Is.Empty);
        }

        [Test]
        public void TestCaseSourcesDoNotHaveTooManyTestCases()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var classes = assembly.GetTypes();
            var sourcesOverLimit = new Dictionary<string, int>();

            foreach (var testClass in classes)
            {
                var methods = testClass.GetMethods();
                var activeTests = methods.Where(m => IsActiveTest(m, testClass));

                if (!activeTests.Any())
                    continue;

                var testCaseSources = activeTests.SelectMany(m => m.GetCustomAttributes<TestCaseSourceAttribute>());
                foreach (var testCaseSource in testCaseSources)
                {
                    var key = $"{testCaseSource.SourceType?.Name ?? testClass.Name}:{testCaseSource.SourceName}";
                    if (sourcesOverLimit.ContainsKey(key))
                        continue;

                    var count = GetTestCaseSourceCount(testCaseSource, testClass);
                    if (count > TestLimit)
                        sourcesOverLimit[key] = count;
                }
            }

            Assert.That(sourcesOverLimit, Is.Empty);
        }

        private static bool IsActiveTest(MethodInfo method, Type testClass)
        {
            if (method.GetCustomAttributes<IgnoreAttribute>(true).Any())
                return false;

            return method.GetCustomAttributes<TestAttribute>(true).Any()
                || method.GetCustomAttributes<TestCaseAttribute>(true).Any(TestCaseIsActive)
                || method.GetCustomAttributes<TestCaseSourceAttribute>(true).Any(tcs => TestCaseSourceIsActive(tcs, testClass));
        }

        private static bool TestCaseIsActive(TestCaseAttribute testCase)
        {
            return string.IsNullOrEmpty(testCase.Ignore) && string.IsNullOrEmpty(testCase.IgnoreReason);
        }

        private static bool TestCaseSourceIsActive(TestCaseSourceAttribute testCaseSource, Type testClass)
        {
            var testCases = GetFromSource(testCaseSource, testClass);

            foreach (var _ in testCases)
                return true;

            return false;
        }

        private static int GetTestCaseSourceCount(TestCaseSourceAttribute testCaseSource, Type testClass)
        {
            var testCases = GetFromSource(testCaseSource, testClass);
            var count = 0;

            foreach (var testCase in testCases)
                count++;

            return count;
        }

        private static IEnumerable GetFromSource(TestCaseSourceAttribute testCaseSource, Type testClass)
        {
            var sourceClass = testClass;
            if (testCaseSource.SourceType != null)
            {
                sourceClass = testCaseSource.SourceType;
            }

            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
            var method = sourceClass.GetMethod(testCaseSource.SourceName, flags);
            if (method == null)
            {
                var property = sourceClass.GetProperty(testCaseSource.SourceName, flags);
                method = property?.GetMethod;
            }

            if (method == null)
                throw new InvalidOperationException($"Type '{sourceClass}' lacks a method or property '{testCaseSource.SourceName}'");

            var instance = Activator.CreateInstance(sourceClass);
            var testCases = (IEnumerable)method.Invoke(instance, []);

            return testCases;
        }
    }
}
