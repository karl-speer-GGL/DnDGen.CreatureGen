using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Abilities;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tests.Integration.TestData;
using DnDGen.CreatureGen.Verifiers.Exceptions;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DnDGen.CreatureGen.Tests.Integration.Stress.Creatures
{
    [TestFixture]
    public class CreatureGeneratorTests : StressTests
    {
        private CreatureAsserter creatureAsserter;
        private ICollectionSelector collectionSelector;
        private ICreatureGenerator creatureGenerator;
        private Stopwatch stopwatch;
        private Dice dice;
        private AbilityRandomizerFactory abilityRandomizerFactory;

        [SetUp]
        public void Setup()
        {
            creatureAsserter = GetNewInstanceOf<CreatureAsserter>();
            collectionSelector = GetNewInstanceOf<ICollectionSelector>();
            creatureGenerator = GetNewInstanceOf<ICreatureGenerator>();
            dice = GetNewInstanceOf<Dice>();
            stopwatch = new Stopwatch();
            abilityRandomizerFactory = GetNewInstanceOf<AbilityRandomizerFactory>();
        }

        [Test]
        public void StressCreature() => stressor.Stress(() => GenerateAndAssertCreature(false, false));

        [Test]
        public void StressCreature_WithTemplate() => stressor.Stress(() => GenerateAndAssertCreature(false, true));

        [Test]
        public void StressCreatureAsCharacter() => stressor.Stress(() => GenerateAndAssertCreature(true, false));

        [Test]
        public void StressCreatureAsCharacter_WithTemplate() => stressor.Stress(() => GenerateAndAssertCreature(true, true));

        private void GenerateAndAssertCreature(bool asCharacter, bool withTemplate)
        {
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var randomCreature = GetCreatureAndTemplate(asCharacter, withTemplate, abilityRandomizer);
            GenerateAndAssertCreature(randomCreature.Creature, asCharacter, abilityRandomizer, randomCreature.Template);
        }

        private (string Creature, string Template) GetCreatureAndTemplate(bool asCharacter, bool withTemplate, AbilityRandomizer abilityRandomizer)
        {
            var template = CreatureConstants.Templates.None;
            if (withTemplate)
            {
                var validTemplates = allTemplates.Where(t => creatureVerifier.VerifyCompatibility(
                    asCharacter,
                    null,
                    abilityRandomizer,
                    null,
                    [t]));

                template = collectionSelector.SelectRandomFrom(validTemplates);
            }

            var validCreatures = allCreatures.Where(c => creatureVerifier.VerifyCompatibility(
                asCharacter,
                c,
                abilityRandomizer,
                null,
                [template]));
            var randomCreatureName = collectionSelector.SelectRandomFrom(validCreatures);

            return (randomCreatureName, template);
        }

        private Creature GenerateAndAssertCreature(string creatureName, bool asCharacter, AbilityRandomizer abilityRandomizer, params string[] templates)
        {
            stopwatch.Restart();
            var creature = creatureGenerator.Generate(asCharacter, creatureName, abilityRandomizer, templates);
            stopwatch.Stop();

            var timeLimit = CreatureAsserter.GetGenerationTimeLimitInSeconds(creature);
            Assert.That(stopwatch.Elapsed.TotalSeconds, Is.LessThan(timeLimit), creature.Summary);
            Assert.That(creature.Name, Is.EqualTo(creatureName), creature.Summary);
            Assert.That(creature.Templates, Is.EqualTo(templates.Where(t => t != CreatureConstants.Templates.None)), creature.Summary);

            if (asCharacter)
                creatureAsserter.AssertCreatureAsCharacter(creature);
            else
                creatureAsserter.AssertCreature(creature);

            return creature;
        }

        [Test]
        public async Task StressCreatureAsync() => await stressor.StressAsync(async () => await GenerateAndAssertCreatureAsync(false, false));

        [Test]
        public async Task StressCreatureAsync_WithTemplate() => await stressor.StressAsync(async () => await GenerateAndAssertCreatureAsync(false, true));

        [Test]
        public async Task StressCreatureAsyncAsCharacter() => await stressor.StressAsync(async () => await GenerateAndAssertCreatureAsync(true, false));

        [Test]
        public async Task StressCreatureAsyncAsCharacter_WithTemplate() => await stressor.StressAsync(async () => await GenerateAndAssertCreatureAsync(true, true));

        private async Task GenerateAndAssertCreatureAsync(bool asCharacter, bool withTemplate)
        {
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var randomCreature = GetCreatureAndTemplate(asCharacter, withTemplate, abilityRandomizer);
            await GenerateAndAssertCreatureAsync(randomCreature.Creature, randomCreature.Template, asCharacter);
        }

        private async Task<Creature> GenerateAndAssertCreatureAsync(string creatureName, string template, bool asCharacter)
        {
            var randomizer = abilityRandomizerFactory.GetAbilityRandomizer([template]);

            stopwatch.Restart();
            var creature = await creatureGenerator.GenerateAsync(asCharacter, creatureName, randomizer, template);
            stopwatch.Stop();

            var timeLimit = CreatureAsserter.GetGenerationTimeLimitInSeconds(creature);
            Assert.That(stopwatch.Elapsed.TotalSeconds, Is.LessThan(timeLimit), creature.Summary);
            Assert.That(creature.Name, Is.EqualTo(creatureName), creature.Summary);

            if (template != CreatureConstants.Templates.None)
                Assert.That(creature.Templates, Is.EqualTo([template]), creature.Summary);

            if (asCharacter)
                creatureAsserter.AssertCreatureAsCharacter(creature);
            else
                creatureAsserter.AssertCreature(creature);

            return creature;
        }

        [Test]
        public void StressRandomCreature()
        {
            stressor.Stress(GenerateAndAssertRandomCreature);
        }

        private void GenerateAndAssertRandomCreature()
        {
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var filters = GetRandomFilters(abilityRandomizer);
            GenerateAndAssertRandomCreature(filters.AsCharacter, filters.Filters, abilityRandomizer, filters.Template);
        }

        private (bool AsCharacter, string Template, Filters Filters) GetRandomFilters(AbilityRandomizer abilityRandomizer)
        {
            var asCharacter = dice.Roll().d2().AsTrueOrFalse();
            var setTemplate = dice.Roll().d2().AsTrueOrFalse();
            var setType = dice.Roll().d2().AsTrueOrFalse();
            var setCr = dice.Roll().d2().AsTrueOrFalse();
            var setAlignment = dice.Roll().d2().AsTrueOrFalse();

            var filters = GetRandomFilters(asCharacter, setTemplate, setType, setCr, setAlignment, abilityRandomizer);
            return (asCharacter, filters.Template, filters.Filters);
        }

        private (string Template, Filters Filters) GetRandomFilters(
            bool asCharacter,
            bool setTemplate,
            bool setType,
            bool setCr,
            bool setAlignment,
            AbilityRandomizer abilityRandomizer)
        {
            string template = null;
            string type = null;
            string cr = null;
            string alignment = null;
            var filters = new Filters();

            if (setTemplate)
            {
                var validTemplates = allTemplates.Where(t => creatureVerifier.VerifyCompatibility(
                    asCharacter,
                    null,
                    abilityRandomizer,
                    null,
                    [t]));

                template = collectionSelector.SelectRandomFrom(validTemplates);
            }

            if (setType)
            {
                var types = CreatureConstants.Types.GetAll();
                var subtypes = CreatureConstants.Types.Subtypes.GetAll();
                var allTypes = types.Union(subtypes);
                var validTypes = allTypes.Where(t => creatureVerifier.VerifyCompatibility(
                    asCharacter,
                    null,
                    abilityRandomizer,
                    new Filters { Types = [t] },
                    [template]));

                type = collectionSelector.SelectRandomFrom(validTypes);
                filters.Types.Add(type);
            }

            if (setCr)
            {
                var challengeRatings = ChallengeRatingConstants.GetOrdered();
                var validChallengeRatings = challengeRatings
                    .Where(c => creatureVerifier.VerifyCompatibility(
                        asCharacter,
                        null,
                        abilityRandomizer,
                        new Filters { Types = [type], ChallengeRatings = [c] },
                        [template]));

                cr = collectionSelector.SelectRandomFrom(validChallengeRatings);
                filters.ChallengeRatings.Add(cr);
            }

            if (setAlignment)
            {
                var alignments = new[]
                {
                    AlignmentConstants.LawfulGood,
                    AlignmentConstants.NeutralGood,
                    AlignmentConstants.ChaoticGood,
                    AlignmentConstants.LawfulNeutral,
                    AlignmentConstants.TrueNeutral,
                    AlignmentConstants.ChaoticNeutral,
                    AlignmentConstants.LawfulEvil,
                    AlignmentConstants.NeutralEvil,
                    AlignmentConstants.ChaoticEvil,
                };
                var validAlignments = alignments
                    .Where(a => creatureVerifier.VerifyCompatibility(
                        asCharacter,
                        null,
                        abilityRandomizer,
                        new Filters { Types = [type], ChallengeRatings = [cr], Alignments = [a] },
                        [template]));

                alignment = collectionSelector.SelectRandomFrom(validAlignments);
                filters.Alignments.Add(alignment);
            }

            return (template, filters);
        }

        private Creature GenerateAndAssertRandomCreature(
            bool asCharacter,
            Filters filters,
            AbilityRandomizer abilityRandomizer,
            params string[] templates)
        {
            stopwatch.Restart();
            var creature = creatureGenerator.GenerateRandom(asCharacter, abilityRandomizer, filters, templates);
            stopwatch.Stop();

            AssertRandomCreature(creature, asCharacter, filters, templates);

            return creature;
        }

        private void AssertRandomCreature(Creature creature, bool asCharacter, Filters filters, params string[] templates)
        {
            var failure = new InvalidCreatureException(null, asCharacter, creature.Summary, filters, null, templates);

            var timeLimit = CreatureAsserter.GetGenerationTimeLimitInSeconds(creature);
            Assert.That(stopwatch.Elapsed.TotalSeconds, Is.LessThan(timeLimit), failure.Message);

            if (templates.Any(t => !string.IsNullOrEmpty(t)))
                Assert.That(creature.Templates, Is.EqualTo(templates.Where(t => t != CreatureConstants.Templates.None)), failure.Message);

            //INFO: While we support multiple filters (acting as an OR), our current test cases only use 1 filter each.
            //if we ever add multiple filter values as abilityRandomizerFactory test case, these assertions should fail and should be updated to handle the OR correctly
            if (filters?.Types?.Count > 0)
                CreatureAsserter.AssertCreatureIsType(creature, filters.Types.Single(), failure.Message);

            if (filters?.ChallengeRatings?.Count > 0)
                Assert.That(creature.ChallengeRating, Is.EqualTo(filters.ChallengeRatings.Single()), failure.Message);

            if (filters?.Alignments?.Count > 0)
                Assert.That(creature.Alignment.Full, Is.EqualTo(filters.Alignments.Single()), failure.Message);

            if (asCharacter)
                creatureAsserter.AssertCreatureAsCharacter(creature, failure.Message);
            else
                creatureAsserter.AssertCreature(creature, asCharacter, failure.Message);
        }

        [Test]
        public async Task StressRandomCreatureAsync()
        {
            await stressor.StressAsync(GenerateAndAssertRandomCreatureAsync);
        }

        private async Task GenerateAndAssertRandomCreatureAsync()
        {
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var filters = GetRandomFilters(abilityRandomizer);
            await GenerateAndAssertRandomCreatureAsync(filters.AsCharacter, filters.Template, filters.Filters, abilityRandomizer);
        }

        private async Task<Creature> GenerateAndAssertRandomCreatureAsync(
            bool asCharacter,
            string template,
            Filters filters,
            AbilityRandomizer abilityRandomizer)
        {
            stopwatch.Restart();
            var creature = await creatureGenerator.GenerateRandomAsync(asCharacter, abilityRandomizer, filters, template);
            stopwatch.Stop();

            AssertRandomCreature(creature, asCharacter, filters, template);

            return creature;
        }

        [Test]
        public void BUG_StressProblematicCreature()
        {
            stressor.Stress(GenerateAndAssertProblematicCreature);
        }

        private void GenerateAndAssertProblematicCreature()
        {
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var randomCreature = collectionSelector.SelectRandomFrom(CreatureTestData.ProblematicCreatures);
            GenerateAndAssertCreature(randomCreature.Creature, randomCreature.AsCharacter, abilityRandomizer, randomCreature.Templates);
        }

        [Test]
        public void BUG_StressProblematicFilters()
        {
            stressor.Stress(GenerateAndAssertProblematicFilters);
        }

        private void GenerateAndAssertProblematicFilters()
        {
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var randomFilters = collectionSelector.SelectRandomFrom(CreatureTestData.ProblematicFilters);
            GenerateAndAssertRandomCreature(
                randomFilters.AsCharacter,
                randomFilters.Filters,
                abilityRandomizer,
                [.. randomFilters.Templates]);
        }

        [TestCase(ChallengeRatingConstants.CR10)]
        public void StressRandomCreature_MinimumCR(string cr)
        {
            stressor.Stress(() => GenerateAndAssertRandomCreatureOfMinimumCR(cr));
        }

        private void GenerateAndAssertRandomCreatureOfMinimumCR(string minimumCr)
        {
            var crs = ChallengeRatingConstants.GetOrdered();
            var minIndex = Array.IndexOf(crs, minimumCr);
            var randomCr = collectionSelector.SelectRandomFrom(crs.Skip(minIndex));
            var abilityRandomizer = abilityRandomizerFactory.GetAbilityRandomizer([]);
            var filters = new Filters { ChallengeRatings = [randomCr] };

            GenerateAndAssertRandomCreature(false, filters, abilityRandomizer);
        }
    }
}