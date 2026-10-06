using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Abilities;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Templates;
using DnDGen.CreatureGen.Verifiers;
using DnDGen.Infrastructure.Factories;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Unit.Verifiers
{
    [TestFixture]
    public class CreatureVerifierTests
    {
        private ICreatureVerifier verifier;
        private Mock<JustInTimeFactory> mockJustInTimeFactory;
        private Mock<ICollectionSelector> mockCollectionSelector;
        private Mock<Dice> mockDice;
        private Mock<ICreaturePrototypeFactory> mockCreaturePrototypeFactory;
        private AbilityRandomizer abilityRandomizer;

        private const string creature = "my creature";
        private static readonly string[] allCreatures = ["character", creature, "wrong creature", "wrong character"];
        private static readonly string[] allCharacters = ["character", creature, "wrong character"];

        [SetUp]
        public void Setup()
        {
            mockJustInTimeFactory = new Mock<JustInTimeFactory>();
            mockCollectionSelector = new Mock<ICollectionSelector>();
            mockDice = new Mock<Dice>();
            mockCreaturePrototypeFactory = new Mock<ICreaturePrototypeFactory>();
            verifier = new CreatureVerifier(mockJustInTimeFactory.Object, mockCollectionSelector.Object, mockDice.Object, mockCreaturePrototypeFactory.Object);

            abilityRandomizer = new("my roll");
            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMinimum<int>()).Returns(1);

            SetUpCreatureGroup(GroupConstants.All, allCreatures);
            SetUpCreatureGroup(GroupConstants.Characters, allCharacters);
            SetUpCreatureGroup(CreatureConstants.Templates.None + true, allCharacters);
            SetUpCreatureGroup(CreatureConstants.Templates.None + false, allCreatures);

            SetupApplicator(CreatureConstants.Templates.None);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyCompatibility_AbilityRandomizer_DefaultIsValid(bool asCharacter)
        {
            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, null);
            Assert.That(isCompatible, Is.True);
        }

        private Mock<TemplateApplicator> SetupApplicator(string template)
        {
            var mockApplicator = new Mock<TemplateApplicator>();
            mockJustInTimeFactory
                .Setup(f => f.Build<TemplateApplicator>(template))
                .Returns(mockApplicator.Object);

            return mockApplicator;
        }

        private Mock<TemplateApplicator> SetupApplicatorWithMinAbility(string template, int minScore = 6)
        {
            var mockApplicator = SetupApplicator(template);
            var minAbility = new Ability("my ability") { BaseScore = minScore };
            mockApplicator.SetupGet(a => a.MinimumAbility).Returns(minAbility);

            return mockApplicator;
        }

        private Mock<TemplateApplicator> SetupStepInApplicatorChain(string template, Filters filters, Func<CreaturePrototype, Filters, bool> compatible = null)
        {
            var mockApplicator = SetupApplicator(template);
            SetupStepInApplicatorChain(mockApplicator, template, filters, compatible);

            return mockApplicator;
        }

        private static void SetupStepInApplicatorChain(
            Mock<TemplateApplicator> mockApplicator,
            string template,
            Filters filters,
            Func<CreaturePrototype, Filters, bool> compatible = null)
        {
            compatible ??= (p, _) => true;

            mockApplicator
                .Setup(a => a.IsCompatible(It.IsAny<CreaturePrototype>(), filters))
                .Returns(compatible);
            mockApplicator
                .Setup(a => a.ApplyTo(It.IsAny<CreaturePrototype>(), filters))
                .Returns((CreaturePrototype p, Filters _) => ApplyTemplate(p, template));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyCompatibility_AbilityRandomizer_Valid(bool asCharacter)
        {
            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, abilityRandomizer);
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyCompatibility_AbilityRandomizer_Invalid(bool asCharacter)
        {
            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMinimum<int>()).Returns(0);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, abilityRandomizer);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void VerifyCompatibility_Creature_WithDefaults(bool asCharacter, bool compatible)
        {
            if (!compatible)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["character", "wrong creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature);
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void VerifyCompatibility_CreatureAnd1Template_Compatible(bool asCharacter, bool compatible)
        {
            SetUpCreatureGroup("template" + asCharacter, compatible ? ["character", creature, "wrong creature"] : ["character", "wrong creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void VerifyCompatibility_CreatureAnd2Templates_Compatible(bool asCharacter, bool compatible)
        {
            SetUpCreatureGroup("template 1" + asCharacter, ["character", creature, "wrong creature"]);

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var mockApplicator1 = SetupStepInApplicatorChain("template 1", null);
            var mockApplicator2 = SetupStepInApplicatorChain("template 2", null, (cp, _) => compatible && cp.Name.Contains(creature));

            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, abilityRandomizer, null, "template 1", "template 2");
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void VerifyCompatibility_CreatureAnd3Templates_Compatible(bool asCharacter, bool compatible)
        {
            SetUpCreatureGroup("template 1" + asCharacter, ["character", creature, "wrong creature"]);

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var mockApplicator1 = SetupStepInApplicatorChain("template 1", null);
            var mockApplicator2 = SetupStepInApplicatorChain("template 2", null);
            var mockApplicator3 = SetupStepInApplicatorChain("template 3", null, (cp, _) => compatible && cp.Name.Contains(creature));

            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, abilityRandomizer, null, "template 1", "template 2", "template 3");
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void BUG_VerifyCompatibility_CreatureAndNoneTemplate_Compatible(bool asCharacter, bool compatible)
        {
            if (!compatible)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["character", "wrong creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, creature, abilityRandomizer, null, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyCompatiblity_CreatureAndTemplateAsCharacter_Compatible(bool compatible)
        {
            SetUpCreatureGroup("template" + true, compatible ? ["character", creature, "wrong creature"] : ["character", "wrong creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(true, creature, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BUG_VerifyCompatiblity_CreatureAndNoneTemplateAsCharacter_Compatible(bool compatible)
        {
            if (!compatible)
                SetUpCreatureGroup(CreatureConstants.Templates.None + true, ["character", "wrong character"]);

            var isCompatible = verifier.VerifyCompatibility(true, creature, abilityRandomizer, null, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.EqualTo(compatible));
        }

        [Test]
        public void VerifyCompatiblity_CreatureAndTemplateAsCharacter_NotCompatible_IfNotCharacter()
        {
            SetUpCreatureGroup(GroupConstants.Characters, allCharacters.Except([creature]));
            SetUpCreatureGroup("template" + true, ["character", creature, "wrong creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(true, creature, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.False);
        }

        [Test]
        public void BUG_VerifyCompatiblity_CreatureAndNoneTemplateAsCharacter_NotCompatible_IfNotCharacter()
        {
            SetUpCreatureGroup(GroupConstants.Characters, allCharacters.Except([creature]));

            var isCompatible = verifier.VerifyCompatibility(true, creature, abilityRandomizer, null, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [Test]
        public void VerifyCompatiblity_Template_Compatible_IfTemplate()
        {
            SetUpCreatureGroup("template" + false, ["character", creature, "template creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(false, null, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.True);
        }

        [Test]
        public void VerifyCompatiblity_TemplateWithMinimumAbility_Compatible()
        {
            SetUpCreatureGroup("template" + false, ["template character", creature, "wrong template creature"]);
            SetUpCreatureGroup("my ability-3", ["ability character", creature, "wrong ability creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("template");

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var isCompatible = verifier.VerifyCompatibility(false, null, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.True);
        }

        [Test]
        public void BUG_VerifyCompatiblity_NoneTemplate_Compatible()
        {
            var isCompatible = verifier.VerifyCompatibility(false, null, abilityRandomizer, null, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyCompatiblity_Template_NotCompatible_IfNotTemplate(bool asCharacter)
        {
            SetUpCreatureGroup("template" + asCharacter, ["template creature", "wrong template creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void VerifyCompatiblity_Template_NotCompatible_IfNotMinimumAbility(bool asCharacter)
        {
            SetUpCreatureGroup("template" + asCharacter, ["template creature", creature, "wrong template creature"]);
            SetUpCreatureGroup("my ability-3", ["ability character", "wrong ability creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("template");

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BUG_VerifyCompatiblity_NoneTemplate_NotCompatible_IfNotTemplate(bool asCharacter)
        {
            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["none creature", "other wrong creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, null, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [Test]
        public void VerifyCompatiblity_TemplateAsCharacter_NotCompatible_IfNotCharacter()
        {
            SetUpCreatureGroup(GroupConstants.Characters, ["character", "wrong character"]);
            SetUpCreatureGroup("template" + true, [creature, "wrong creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(true, null, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.False);
        }

        [Test]
        public void VerifyCompatiblity_TemplateWithMinimumAbilityAsCharacter_NotCompatible_IfNotCharacter()
        {
            SetUpCreatureGroup(GroupConstants.Characters, ["character", "wrong character"]);
            SetUpCreatureGroup("template" + true, [creature, "wrong creature"]);
            SetUpCreatureGroup("my ability-3", ["ability character", "character", creature, "wrong ability creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("template");

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var isCompatible = verifier.VerifyCompatibility(true, null, abilityRandomizer, null, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateAndFilters_Compatible(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateWithMinimumAbilityAndFilters_Compatible(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", creature, "wrong template creature"]);
            SetUpCreatureGroup("my ability-3", ["ability character", creature, "wrong ability creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("template");

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateAndFilters_Compatible(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateAndFilters_NotCompatible_IfNotTemplate(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateAndFilters_NotCompatible_IfNotCR(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateAndFilters_NotCompatible_IfNotType(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateAndFilters_NotCompatible_IfNotAlignment(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", "wrong alignment creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateWithMinimumAbilityAndFilters_NotCompatible_IfNotMinimumAbility(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup("template" + asCharacter, ["template character", creature, "wrong template creature"]);
            SetUpCreatureGroup("my ability-3", ["ability character", "wrong ability creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("template");

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateAndFilters_NotCompatible_IfNotTemplate(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template character", "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateAndFilters_NotCompatible_IfNotCR(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr character", "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateAndFilters_NotCompatible_IfNotType(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type character", "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateAndFilters_NotCompatible_IfNotAlignment(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment character", "wrong alignment creature"]);

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(null, null, null)]
        [TestCase(null, null, "my alignment")]
        [TestCase(null, "my type", null)]
        [TestCase(null, "my type", "my alignment")]
        [TestCase("my challenge rating", null, null)]
        [TestCase("my challenge rating", null, "my alignment")]
        [TestCase("my challenge rating", "my type", null)]
        [TestCase("my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateAndFiltersAsCharacter_NotCompatible_IfNotCharacter(string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(GroupConstants.Characters, ["character", "wrong character"]);
            SetUpCreatureGroup("template" + true, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + true + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            SetupApplicator("template");

            var isCompatible = verifier.VerifyCompatibility(true, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(null, null, null)]
        [TestCase(null, null, "my alignment")]
        [TestCase(null, "my type", null)]
        [TestCase(null, "my type", "my alignment")]
        [TestCase("my challenge rating", null, null)]
        [TestCase("my challenge rating", null, "my alignment")]
        [TestCase("my challenge rating", "my type", null)]
        [TestCase("my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_TemplateWithMinimumAbilityAndFiltersAsCharacter_NotCompatible_IfNotCharacter(string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(GroupConstants.Characters, ["character", "wrong character"]);
            SetUpCreatureGroup("template" + true, ["template character", creature, "wrong template creature"]);
            SetUpCreatureGroup("my ability-3", ["ability character", creature, "wrong ability creature"]);

            if (cr != null)
                SetUpCreatureGroup("template" + true + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup("template" + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup("template" + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("template");

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var isCompatible = verifier.VerifyCompatibility(true, null, abilityRandomizer, filters, "template");
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(null, null, null)]
        [TestCase(null, null, "my alignment")]
        [TestCase(null, "my type", null)]
        [TestCase(null, "my type", "my alignment")]
        [TestCase("my challenge rating", null, null)]
        [TestCase("my challenge rating", null, "my alignment")]
        [TestCase("my challenge rating", "my type", null)]
        [TestCase("my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateAndFiltersAsCharacter_NotCompatible_IfNotCharacter(string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(GroupConstants.Characters, ["character", "wrong character"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + true, ["template character", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + true + cr, ["template cr character", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type character", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment character", creature, "wrong alignment creature"]);

            var isCompatible = verifier.VerifyCompatibility(true, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_WithFilters_Compatible_BaseCreature(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template creature", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr creature", creature, "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type creature", creature, "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment creature", creature, "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
            }

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters);
            Assert.That(isCompatible, Is.True);

            mockCollectionSelector.Verify(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All), Times.Never);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_WithFilters_Compatible_Template(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template creature", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr creature", "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type creature", "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment creature", "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
                var isTemplate = template == "template";

                SetUpCreatureGroup(
                    template + asCharacter,
                    isTemplate ? ["template character", creature, "wrong template creature"] : ["template character", "wrong template creature"]);

                if (cr != null)
                    SetUpCreatureGroup(template + asCharacter + cr, ["template cr creature", creature, "wrong cr creature"]);

                if (type != null)
                    SetUpCreatureGroup(template + type, ["template type creature", creature, "wrong type creature"]);

                if (alignment != null)
                    SetUpCreatureGroup(template + alignment, ["template alignment creature", creature, "wrong alignment creature"]);
            }

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters);
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_WithFilters_Compatible_AnyTemplate(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template creature", creature, "wrong template creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr creature", "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type creature", "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment creature", "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
                var isTemplate = template == "other template";

                SetUpCreatureGroup(
                    template + asCharacter,
                    isTemplate ? ["template character", creature, "wrong template creature"] : ["template character", "wrong template creature"]);

                if (cr != null)
                    SetUpCreatureGroup(template + asCharacter + cr, ["template cr creature", creature, "wrong cr creature"]);

                if (type != null)
                    SetUpCreatureGroup(template + type, ["template type creature", creature, "wrong type creature"]);

                if (alignment != null)
                    SetUpCreatureGroup(template + alignment, ["template alignment creature", creature, "wrong alignment creature"]);
            }

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters);
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_WithFilters_Compatible_BaseCreature_MultipleFilters(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type, "other type"],
                ChallengeRatings = [cr, "other cr"],
                Alignments = [alignment, "other alignment"]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template creature", creature, "wrong template creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + "other cr", ["other template cr creature", creature, "wrong cr creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + "other type", ["other type creature", creature, "wrong type creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + "other alignment", ["other alignment creature", creature, "wrong alignment creature"]);

            if (cr != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + cr, ["template cr creature", "wrong cr creature"]);

            if (type != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + type, ["template type creature", "wrong type creature"]);

            if (alignment != null)
                SetUpCreatureGroup(CreatureConstants.Templates.None + alignment, ["template alignment creature", "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
            }

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters);
            Assert.That(isCompatible, Is.True);

            mockCollectionSelector.Verify(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All), Times.Never);
        }

        [TestCase(null)]
        [TestCase("")]
        public void VerifyCompatiblity_WithFilters_Compatible_Template_IgnoreEmptyTemplates(string empty)
        {
            var filters = new Filters
            {
                Types = ["my type"],
                ChallengeRatings = ["my challenge rating"],
                Alignments = ["my alignment"]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + false, ["template creature", creature, "wrong template creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + false + filters.ChallengeRatings[0], ["template cr creature", "wrong cr creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + filters.Types[0], ["template type creature", "wrong type creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + filters.Alignments[0], ["template alignment creature", "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
                var isTemplate = template == "template";

                SetUpCreatureGroup(
                    template + false,
                    isTemplate ? ["template character", creature, "wrong template creature"] : ["template character", "wrong template creature"]);

                SetUpCreatureGroup(template + false + filters.ChallengeRatings[0], ["template cr creature", creature, "wrong cr creature"]);
                SetUpCreatureGroup(template + filters.Types[0], ["template type creature", creature, "wrong type creature"]);
                SetUpCreatureGroup(template + filters.Alignments[0], ["template alignment creature", creature, "wrong alignment creature"]);
            }

            var isCompatible = verifier.VerifyCompatibility(false, null, abilityRandomizer, filters, empty);
            Assert.That(isCompatible, Is.True);
        }

        [TestCase(true, null, null, null)]
        [TestCase(true, null, null, "my alignment")]
        [TestCase(true, null, "my type", null)]
        [TestCase(true, null, "my type", "my alignment")]
        [TestCase(true, "my challenge rating", null, null)]
        [TestCase(true, "my challenge rating", null, "my alignment")]
        [TestCase(true, "my challenge rating", "my type", null)]
        [TestCase(true, "my challenge rating", "my type", "my alignment")]
        [TestCase(false, null, null, null)]
        [TestCase(false, null, null, "my alignment")]
        [TestCase(false, null, "my type", null)]
        [TestCase(false, null, "my type", "my alignment")]
        [TestCase(false, "my challenge rating", null, null)]
        [TestCase(false, "my challenge rating", null, "my alignment")]
        [TestCase(false, "my challenge rating", "my type", null)]
        [TestCase(false, "my challenge rating", "my type", "my alignment")]
        public void BUG_VerifyCompatiblity_NoneTemplateWithFilters_NotCompatible_DoNotTryRandomTemplates(bool asCharacter, string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter, ["template creature", creature, "wrong template creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + filters.ChallengeRatings[0], ["template cr creature", "wrong cr creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + filters.Types[0], ["template type creature", "wrong type creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + filters.Alignments[0], ["template alignment creature", "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
                SetUpCreatureGroup(template + asCharacter, ["template character", creature, "wrong template creature"]);
                SetUpCreatureGroup(template + asCharacter + filters.ChallengeRatings[0], ["template cr creature", creature, "wrong cr creature"]);
                SetUpCreatureGroup(template + filters.Types[0], ["template type creature", creature, "wrong type creature"]);
                SetUpCreatureGroup(template + filters.Alignments[0], ["template alignment creature", creature, "wrong alignment creature"]);
            }

            var isCompatible = verifier.VerifyCompatibility(asCharacter, null, abilityRandomizer, filters, CreatureConstants.Templates.None);
            Assert.That(isCompatible, Is.False);

            mockCollectionSelector.Verify(c => c.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All), Times.Never);
        }

        [TestCase(null, null, null)]
        [TestCase(null, null, "my alignment")]
        [TestCase(null, "my type", null)]
        [TestCase(null, "my type", "my alignment")]
        [TestCase("my challenge rating", null, null)]
        [TestCase("my challenge rating", null, "my alignment")]
        [TestCase("my challenge rating", "my type", null)]
        [TestCase("my challenge rating", "my type", "my alignment")]
        public void VerifyCompatiblity_WithFiltersAsCharacter_NotCompatible_IfNotCharacter(string cr, string type, string alignment)
        {
            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [cr],
                Alignments = [alignment]
            };

            SetUpCreatureGroup(GroupConstants.Characters, ["character", "wrong character"]);

            SetUpCreatureGroup(CreatureConstants.Templates.None + true, ["template creature", creature, "wrong template creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + true + filters.ChallengeRatings[0], ["template cr creature", creature, "wrong cr creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + filters.Types[0], ["template type creature", creature, "wrong type creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + filters.Alignments[0], ["template alignment creature", creature, "wrong alignment creature"]);

            var templates = new[] { "template", "other template" };
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.TemplateGroups, GroupConstants.All))
                .Returns(templates);

            foreach (var template in templates)
            {
                SetupApplicator(template);
                SetUpCreatureGroup(template + true, ["template character", creature, "wrong template creature"]);
                SetUpCreatureGroup(template + true + filters.ChallengeRatings[0], ["template cr creature", creature, "wrong cr creature"]);
                SetUpCreatureGroup(template + filters.Types[0], ["template type creature", creature, "wrong type creature"]);
                SetUpCreatureGroup(template + filters.Alignments[0], ["template alignment creature", creature, "wrong alignment creature"]);
            }

            var isCompatible = verifier.VerifyCompatibility(true, null, abilityRandomizer, filters);
            Assert.That(isCompatible, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_ReturnCompatibleCreatures_NoFilters(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);

            SetupApplicator("my template");

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.EqualTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_ReturnCompatibleCreatures_NoneMatching(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };

            var templateCreatures = new[] { "my template creature", "something else", "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);

            SetupApplicator("my template");

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_ReturnCompatibleCreatures_EmptyTemplateGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };

            SetUpCreatureGroup("my template" + asCharacter, []);

            SetupApplicator("my template");

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        private void SetUpCreatureGroup(string groupName, IEnumerable<string> group)
        {
            mockCollectionSelector
                .Setup(s => s.IsCollection(Config.Name, TableNameConstants.Collection.CreatureGroups, groupName))
                .Returns(true);
            mockCollectionSelector
                .Setup(s => s.SelectFrom(Config.Name, TableNameConstants.Collection.CreatureGroups, groupName))
                .Returns(group);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithDefaultAbilityRandomizer_ReturnCompatibleCreatures_NoFilters(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup("my ability-5", ["low-ability creature", "my other creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicator("my template");
            var minAbility = new Ability("my ability") { BaseScore = 6 };
            mockApplicator.SetupGet(a => a.MinimumAbility).Returns(minAbility);

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.EqualTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithDefaultAbilityRandomizer_ReturnCompatibleCreatures_EmptyTemplateGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            SetUpCreatureGroup("my template" + asCharacter, []);
            SetUpCreatureGroup("my ability-5", ["low-ability creature", "my other creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithDefaultAbilityRandomizer_ReturnCompatibleCreatures_EmptyAbilityGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup("my ability-5", []);

            var mockApplicator = SetupApplicator("my template");
            var minAbility = new Ability("my ability") { BaseScore = 6 };
            mockApplicator.SetupGet(a => a.MinimumAbility).Returns(minAbility);

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithDefaultAbilityRandomizer_ReturnCompatibleCreatures_NoneMatching(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var templateCreatures = new[] { "my template creature", "something else", "whatever", "my other creature" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup("my ability-5", ["low-ability creature", "my wrong creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true, 4, 14)]
        [TestCase(true, 4, 15)]
        [TestCase(true, 4, 16)]
        [TestCase(true, 4, 17)]
        [TestCase(true, 4, 18)]
        [TestCase(true, 4, 19)]
        [TestCase(true, 4, 20)]
        [TestCase(true, 4, 100)]
        [TestCase(true, 6, 16)]
        [TestCase(true, 6, 17)]
        [TestCase(true, 6, 18)]
        [TestCase(true, 6, 19)]
        [TestCase(true, 6, 20)]
        [TestCase(true, 6, 100)]
        [TestCase(false, 4, 14)]
        [TestCase(false, 4, 15)]
        [TestCase(false, 4, 16)]
        [TestCase(false, 4, 17)]
        [TestCase(false, 4, 18)]
        [TestCase(false, 4, 19)]
        [TestCase(false, 4, 20)]
        [TestCase(false, 4, 100)]
        [TestCase(false, 6, 16)]
        [TestCase(false, 6, 17)]
        [TestCase(false, 6, 18)]
        [TestCase(false, 6, 19)]
        [TestCase(false, 6, 20)]
        [TestCase(false, 6, 100)]
        public void GetCompatibleCreaturesForTemplate_WithAbilityRandomizer_ReturnCompatibleCreatures_LowRequiredAdjustment_Roll(bool asCharacter, int minScore, int maxRoll)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(maxRoll);

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup("my ability-10", ["my other creature", "low-ability creature", "my wrong creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template", minScore);

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, abilityRandomizer);
            Assert.That(compatibleCreatures, Is.EqualTo([creature, "my other creature"]));
        }

        [TestCase(true, 4, 14)]
        [TestCase(true, 4, 15)]
        [TestCase(true, 4, 16)]
        [TestCase(true, 4, 17)]
        [TestCase(true, 4, 18)]
        [TestCase(true, 4, 19)]
        [TestCase(true, 4, 20)]
        [TestCase(true, 4, 100)]
        [TestCase(true, 6, 16)]
        [TestCase(true, 6, 17)]
        [TestCase(true, 6, 18)]
        [TestCase(true, 6, 19)]
        [TestCase(true, 6, 20)]
        [TestCase(true, 6, 100)]
        [TestCase(false, 4, 14)]
        [TestCase(false, 4, 15)]
        [TestCase(false, 4, 16)]
        [TestCase(false, 4, 17)]
        [TestCase(false, 4, 18)]
        [TestCase(false, 4, 19)]
        [TestCase(false, 4, 20)]
        [TestCase(false, 4, 100)]
        [TestCase(false, 6, 16)]
        [TestCase(false, 6, 17)]
        [TestCase(false, 6, 18)]
        [TestCase(false, 6, 19)]
        [TestCase(false, 6, 20)]
        [TestCase(false, 6, 100)]
        public void GetCompatibleCreaturesForTemplate_WithAbilityRandomizer_ReturnCompatibleCreatures_LowRequiredAdjustment_Set(bool asCharacter, int minScore, int setRoll)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            abilityRandomizer.SetRolls["my ability"] = setRoll;

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup("my ability-10", ["my other creature", "low-ability creature", "my wrong creature", "high-ability creature", creature]);

            SetupApplicatorWithMinAbility("my template", minScore);

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, abilityRandomizer);
            Assert.That(compatibleCreatures, Is.EquivalentTo([creature, "my other creature"]));
        }

        [TestCase(true, 4, 1)]
        [TestCase(true, 4, 2)]
        [TestCase(true, 4, 3)]
        [TestCase(true, 4, 4)]
        [TestCase(true, 4, 5)]
        [TestCase(true, 4, 6)]
        [TestCase(true, 4, 7)]
        [TestCase(true, 4, 8)]
        [TestCase(true, 4, 9)]
        [TestCase(true, 4, 10)]
        [TestCase(true, 4, 11)]
        [TestCase(true, 4, 12)]
        [TestCase(true, 4, 13)]
        [TestCase(true, 6, 1)]
        [TestCase(true, 6, 2)]
        [TestCase(true, 6, 3)]
        [TestCase(true, 6, 4)]
        [TestCase(true, 6, 5)]
        [TestCase(true, 6, 6)]
        [TestCase(true, 6, 7)]
        [TestCase(true, 6, 8)]
        [TestCase(true, 6, 9)]
        [TestCase(true, 6, 10)]
        [TestCase(true, 6, 11)]
        [TestCase(true, 6, 12)]
        [TestCase(true, 6, 13)]
        [TestCase(true, 6, 14)]
        [TestCase(true, 6, 15)]
        [TestCase(false, 4, 1)]
        [TestCase(false, 4, 2)]
        [TestCase(false, 4, 3)]
        [TestCase(false, 4, 4)]
        [TestCase(false, 4, 5)]
        [TestCase(false, 4, 6)]
        [TestCase(false, 4, 7)]
        [TestCase(false, 4, 8)]
        [TestCase(false, 4, 9)]
        [TestCase(false, 4, 10)]
        [TestCase(false, 4, 11)]
        [TestCase(false, 4, 12)]
        [TestCase(false, 4, 13)]
        [TestCase(false, 6, 1)]
        [TestCase(false, 6, 2)]
        [TestCase(false, 6, 3)]
        [TestCase(false, 6, 4)]
        [TestCase(false, 6, 5)]
        [TestCase(false, 6, 6)]
        [TestCase(false, 6, 7)]
        [TestCase(false, 6, 8)]
        [TestCase(false, 6, 9)]
        [TestCase(false, 6, 10)]
        [TestCase(false, 6, 11)]
        [TestCase(false, 6, 12)]
        [TestCase(false, 6, 13)]
        [TestCase(false, 6, 14)]
        [TestCase(false, 6, 15)]
        public void GetCompatibleCreaturesForTemplate_WithAbilityRandomizer_ReturnCompatibleCreatures_RequiredAdjustment_Roll(bool asCharacter, int minScore, int maxRoll)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(maxRoll);

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup($"my ability{minScore - maxRoll}", ["low-ability creature", "my other creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template", minScore);

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, abilityRandomizer);
            Assert.That(compatibleCreatures, Is.EqualTo([creature, "my other creature"]));
        }

        [TestCase(true, 4, 1)]
        [TestCase(true, 4, 2)]
        [TestCase(true, 4, 3)]
        [TestCase(true, 4, 4)]
        [TestCase(true, 4, 5)]
        [TestCase(true, 4, 6)]
        [TestCase(true, 4, 7)]
        [TestCase(true, 4, 8)]
        [TestCase(true, 4, 9)]
        [TestCase(true, 4, 10)]
        [TestCase(true, 4, 11)]
        [TestCase(true, 4, 12)]
        [TestCase(true, 4, 13)]
        [TestCase(true, 6, 1)]
        [TestCase(true, 6, 2)]
        [TestCase(true, 6, 3)]
        [TestCase(true, 6, 4)]
        [TestCase(true, 6, 5)]
        [TestCase(true, 6, 6)]
        [TestCase(true, 6, 7)]
        [TestCase(true, 6, 8)]
        [TestCase(true, 6, 9)]
        [TestCase(true, 6, 10)]
        [TestCase(true, 6, 11)]
        [TestCase(true, 6, 12)]
        [TestCase(true, 6, 13)]
        [TestCase(true, 6, 14)]
        [TestCase(true, 6, 15)]
        [TestCase(false, 4, 1)]
        [TestCase(false, 4, 2)]
        [TestCase(false, 4, 3)]
        [TestCase(false, 4, 4)]
        [TestCase(false, 4, 5)]
        [TestCase(false, 4, 6)]
        [TestCase(false, 4, 7)]
        [TestCase(false, 4, 8)]
        [TestCase(false, 4, 9)]
        [TestCase(false, 4, 10)]
        [TestCase(false, 4, 11)]
        [TestCase(false, 4, 12)]
        [TestCase(false, 4, 13)]
        [TestCase(false, 6, 1)]
        [TestCase(false, 6, 2)]
        [TestCase(false, 6, 3)]
        [TestCase(false, 6, 4)]
        [TestCase(false, 6, 5)]
        [TestCase(false, 6, 6)]
        [TestCase(false, 6, 7)]
        [TestCase(false, 6, 8)]
        [TestCase(false, 6, 9)]
        [TestCase(false, 6, 10)]
        [TestCase(false, 6, 11)]
        [TestCase(false, 6, 12)]
        [TestCase(false, 6, 13)]
        [TestCase(false, 6, 14)]
        [TestCase(false, 6, 15)]
        public void GetCompatibleCreaturesForTemplate_WithAbilityRandomizer_ReturnCompatibleCreatures_RequiredAdjustment_Set(bool asCharacter, int minScore, int setRoll)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };
            abilityRandomizer.SetRolls["my ability"] = setRoll;

            var templateCreatures = new[] { "my template creature", "my other creature", "something else", creature, "whatever" };
            SetUpCreatureGroup("my template" + asCharacter, templateCreatures);
            SetUpCreatureGroup($"my ability{minScore - setRoll}", ["low-ability creature", "my other creature", "high-ability creature", creature]);

            SetupApplicatorWithMinAbility("my template", minScore);

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, abilityRandomizer);
            Assert.That(compatibleCreatures, Is.EquivalentTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithAlignment_ReturnCompatibleCreatures(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature"]);
            SetUpCreatureGroup("my template" + "preset alignment", [creature, "my other creature", "alignment creature"]);

            SetupApplicator("my template");

            var filters = new Filters { Alignments = ["preset alignment"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.EqualTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithAlignment_ReturnCompatibleCreatures_EmptyTemplateGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, []);
            SetUpCreatureGroup("my template" + "preset alignment", [creature, "my other creature", "alignment creature"]);

            SetupApplicator("my template");

            var filters = new Filters { Alignments = ["preset alignment"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithAlignment_ReturnCompatibleCreatures_EmptyAlignmentGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature"]);
            SetUpCreatureGroup("my template" + "preset alignment", []);

            SetupApplicator("my template");

            var filters = new Filters { Alignments = ["preset alignment"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithAlignment_ReturnCompatibleCreatures_NoneMatching(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "alignment creature"]);
            SetUpCreatureGroup("my template" + "preset alignment", ["my other creature", "preset alignment creature"]);

            SetupApplicator("my template");

            var filters = new Filters { Alignments = ["preset alignment"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithChallengeRating_ReturnCompatibleCreatures(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature 2", "my other creature", "wrong creature 1", "wrong creature 3" };

            SetUpCreatureGroup("my template" + asCharacter, creatures);
            SetUpCreatureGroup("my template" + asCharacter + "my CR", [creature, "my other creature", "CR creature"]);

            SetupApplicator("my template");

            var filters = new Filters { ChallengeRatings = ["my CR"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.EquivalentTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithChallengeRating_ReturnCompatibleCreatures_EmptyTemplateGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, []);
            SetUpCreatureGroup("my template" + asCharacter + "my CR", [creature, "my other creature", "CR creature"]);

            SetupApplicator("my template");

            var filters = new Filters { ChallengeRatings = ["my CR"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithChallengeRating_ReturnCompatibleCreatures_EmptyChallengeRatingGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature"]);
            SetUpCreatureGroup("my template" + asCharacter + "my CR", []);

            SetupApplicator("my template");

            var filters = new Filters { ChallengeRatings = ["my CR"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithChallengeRating_ReturnCompatibleCreatures_NoneMatching(bool asCharacter)
        {
            var creatures = new[] { creature, "alignment creature", "my other creature", "wrong creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "CR creature"]);
            SetUpCreatureGroup("my template" + asCharacter + "my CR", ["my other creature", "CR creature"]);

            SetupApplicator("my template");

            var filters = new Filters { ChallengeRatings = ["my CR"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithType_ReturnCompatibleCreatures(bool asCharacter)
        {
            var creatures = new[] { creature, "outsider creature", "my other creature", "evil creature", "template creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature"]);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", creature, "type creature"]);

            SetupApplicator("my template");

            var filters = new Filters { Types = ["my type"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.EquivalentTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithType_ReturnCompatibleCreatures_EmptyTemplateGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "outsider creature", "my other creature", "evil creature", "template creature" };

            SetUpCreatureGroup("my template" + asCharacter, []);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", creature, "type creature"]);

            SetupApplicator("my template");

            var filters = new Filters { Types = ["my type"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithType_ReturnCompatibleCreatures_EmptyTypeGroup(bool asCharacter)
        {
            var creatures = new[] { creature, "outsider creature", "my other creature", "evil creature", "template creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature"]);
            SetUpCreatureGroup("my template" + "my type", []);

            SetupApplicator("my template");

            var filters = new Filters { Types = ["my type"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithType_ReturnCompatibleCreatures_NoneMatching(bool asCharacter)
        {
            var creatures = new[] { creature, "outsider creature", "my other creature", "evil creature", "template creature" };

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "type creature"]);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", "type creature"]);

            SetupApplicator("my template");

            var filters = new Filters { Types = ["my type"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, null, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithAllFilters_ReturnCompatibleCreatures(bool asCharacter)
        {
            var creatures = new[] { creature, "wrong creature", "my other creature", "template creature", "alignment creature", "CR creature", "type creature" };
            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature"]);
            SetUpCreatureGroup("my ability-3", ["low-ability creature", "my other creature", "high-ability creature", creature]);
            SetUpCreatureGroup("my template" + "my alignment", ["alignment creature", "my other creature", creature]);
            SetUpCreatureGroup("my template" + asCharacter + "my CR", [creature, "my other creature", "CR creature"]);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", "type creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");

            var filters = new Filters { Alignments = ["my alignment"], ChallengeRatings = ["my CR"], Types = ["my type"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, abilityRandomizer, filters);
            Assert.That(compatibleCreatures, Is.EquivalentTo([creature, "my other creature"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetCompatibleCreaturesForTemplate_WithAllFilters_ReturnCompatibleCreatures_NoneMatch(bool asCharacter)
        {
            var creatures = new[]
            {
                creature,
                "wrong creature",
                "my other creature",
                "another creature",
                "template creature",
                "high-ability creature",
                "alignment creature",
                "CR creature",
                "type creature"
            };
            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature", "another creature"]);
            SetUpCreatureGroup("my ability-3", ["low-ability creature", "my other creature", "high-ability creature", creature]);
            SetUpCreatureGroup("my template" + "my alignment", ["alignment creature", creature, "another creature"]);
            SetUpCreatureGroup("my template" + asCharacter + "my CR", ["my other creature", "CR creature", "another creature"]);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", "type creature", creature, "another creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");

            var filters = new Filters { Alignments = ["my alignment"], ChallengeRatings = ["my CR"], Types = ["my type"] };

            var compatibleCreatures = verifier.GetCompatibleCreaturesForTemplate(creatures, "my template", asCharacter, abilityRandomizer, filters);
            Assert.That(compatibleCreatures, Is.Empty);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_0Templates_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, [], asCharacter);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([creature, "character"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_0TemplatesWithAbilityRandomizer_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, [], asCharacter, abilityRandomizer);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([creature, "character"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_0TemplatesWithFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + "my alignment", ["character", "alignment creature", creature, "another creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + "my challenge rating", ["my other creature", "character", "CR creature", creature]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + "my type", ["my other creature", "type creature", "character", creature, "another creature"]);

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, [], asCharacter, null, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([creature, "character"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_0TemplatesWithAbilityRandomizerAndFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            SetUpCreatureGroup(CreatureConstants.Templates.None + "my alignment", ["character", "alignment creature", creature, "another creature"]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + asCharacter + "my challenge rating", ["my other creature", "character", "CR creature", creature]);
            SetUpCreatureGroup(CreatureConstants.Templates.None + "my type", ["my other creature", "type creature", "character", creature, "another creature"]);

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, [], asCharacter, abilityRandomizer, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([creature, "character"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_1Template_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };

            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature", "character", "another creature"]);
            SetUpCreatureGroup("my ability-5", ["low-ability creature", "character", "my other creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");
            SetupStepInApplicatorChain(mockApplicator, "my template", null, (p, _) => !p.Name.Contains("some creature"));

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template"], asCharacter);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template", "character my template"]));
        }

        private static CreaturePrototype ApplyTemplate(CreaturePrototype prototype, string template)
        {
            if (template != CreatureConstants.Templates.None)
                prototype.Name += " " + template;

            return prototype;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_1TemplateWithAbilityRandomizer_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature", "character", "another creature"]);
            SetUpCreatureGroup("my ability-3", ["low-ability creature", "character", "my other creature", "high-ability creature", creature]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");
            SetupStepInApplicatorChain(mockApplicator, "my template", null, (p, _) => !p.Name.Contains("some creature"));

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template"], asCharacter, abilityRandomizer);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template", "character my template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_1TemplateWithFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature", "character", "another creature"]);
            SetUpCreatureGroup("my ability-5", ["low-ability creature", "character", "my other creature", "high-ability creature", creature]);
            SetUpCreatureGroup("my template" + "my alignment", ["character", "alignment creature", creature, "another creature"]);
            SetUpCreatureGroup("my template" + asCharacter + "my challenge rating", ["my other creature", "character", "CR creature", creature]);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", "type creature", "character", creature, "another creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");
            SetupStepInApplicatorChain(mockApplicator, "my template", filters);

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template"], asCharacter, null, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template", "character my template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_1TemplateWithAbilityRandomizerAndFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            SetUpCreatureGroup("my template" + asCharacter, [creature, "template creature", "my other creature", "character", "another creature"]);
            SetUpCreatureGroup("my ability-3", ["low-ability creature", "character", "my other creature", "high-ability creature", creature]);
            SetUpCreatureGroup("my template" + "my alignment", ["character", "alignment creature", creature, "another creature"]);
            SetUpCreatureGroup("my template" + asCharacter + "my challenge rating", ["my other creature", "character", "CR creature", creature]);
            SetUpCreatureGroup("my template" + "my type", ["my other creature", "type creature", "character", creature, "another creature"]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");
            SetupStepInApplicatorChain(mockApplicator, "my template", filters, (p, _) => !p.Name.Contains("some creature"));

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template"], asCharacter, abilityRandomizer, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template", "character my template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_2Templates_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var mockApplicator1 = SetupStepInApplicatorChain("my template", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("my other template", null, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template", "my other template"], asCharacter);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template my other template", "character my template my other template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_2TemplatesWithAbilityRandomizer_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var mockApplicator1 = SetupStepInApplicatorChain("my template", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("my other template", null, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template", "my other template"], asCharacter, abilityRandomizer);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template my other template", "character my template my other template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_2TemplatesWithFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var mockApplicator1 = SetupStepInApplicatorChain("my template", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("my other template", filters, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template", "my other template"], asCharacter, null, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template my other template", "character my template my other template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_2TemplatesWithAbilityRandomizerAndFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var mockApplicator1 = SetupStepInApplicatorChain("my template", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("my other template", filters, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template", "my other template"], asCharacter, abilityRandomizer, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template my other template", "character my template my other template"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_3Templates_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var mockApplicator1 = SetupStepInApplicatorChain("t1", null, (p, _) => !p.Name.Contains("wrong creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("t2", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator3 = SetupStepInApplicatorChain("t3", null, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["t1", "t2", "t3"], asCharacter);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} t1 t2 t3", "character t1 t2 t3"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_3TemplatesWithAbilityRandomizer_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var mockApplicator1 = SetupStepInApplicatorChain("t1", null, (p, _) => !p.Name.Contains("wrong creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("t2", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator3 = SetupStepInApplicatorChain("t3", null, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["t1", "t2", "t3"], asCharacter, abilityRandomizer);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} t1 t2 t3", "character t1 t2 t3"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_3TemplatesWithFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, null))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(AbilityConstants.RandomizerRolls.Default).AsPotentialMaximum<int>(true)).Returns(11);

            var mockApplicator1 = SetupStepInApplicatorChain("t1", null, (p, _) => !p.Name.Contains("wrong creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("t2", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator3 = SetupStepInApplicatorChain("t3", filters, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["t1", "t2", "t3"], asCharacter, null, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} t1 t2 t3", "character t1 t2 t3"]));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetChainedTemplates_3TemplatesWithAbilityRandomizerAndFilters_ReturnsPrototypes(bool asCharacter)
        {
            var creatures = new[] { "some creature", creature, "a different creature", "character", "wrong creature" };
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            mockCreaturePrototypeFactory
                .Setup(f => f.Build(It.IsAny<IEnumerable<string>>(), asCharacter, abilityRandomizer))
                .Returns((IEnumerable<string> cc, bool _, AbilityRandomizer _) => BuildPrototypes(cc));

            mockDice.Setup(d => d.Roll(abilityRandomizer.Roll).AsPotentialMaximum<int>(true)).Returns(9);

            var mockApplicator1 = SetupStepInApplicatorChain("t1", null, (p, _) => !p.Name.Contains("wrong creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("t2", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator3 = SetupStepInApplicatorChain("t3", filters, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["t1", "t2", "t3"], asCharacter, abilityRandomizer, filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} t1 t2 t3", "character t1 t2 t3"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_0Templates_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes([creature, "character"]);

            SetupStepInApplicatorChain(CreatureConstants.Templates.None, null);

            var prototypes = verifier.GetChainedTemplates(creatures, []);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([creature, "character"]));
        }

        private IEnumerable<CreaturePrototype> BuildPrototypes(IEnumerable<string> creatures) => creatures.Select(BuildPrototype);
        private CreaturePrototype BuildPrototype(string creature) => new() { Name = creature };

        [Test]
        public void GetChainedTemplates_FromPrototypes_0TemplatesWithFilters_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character", "wrong creature"]);
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            SetupStepInApplicatorChain(CreatureConstants.Templates.None, filters, (p, _) => p.Name == creature || p.Name == "character");

            var prototypes = verifier.GetChainedTemplates(creatures, [], filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([creature, "character"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_1Template_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character"]);

            var mockApplicator = SetupApplicatorWithMinAbility("my template");
            SetupStepInApplicatorChain(mockApplicator, "my template", null, (p, _) => p.Name == creature || p.Name == "character");

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template"]);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template", "character my template"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_1TemplateWithFilters_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character", "wrong creature"]);
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            var mockApplicator = SetupApplicatorWithMinAbility("my template");
            SetupStepInApplicatorChain(mockApplicator, "my template", filters, (p, _) => p.Name == creature || p.Name == "character");

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template"], filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template", "character my template"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_2Templates_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character"]);

            var mockApplicator1 = SetupStepInApplicatorChain("my template", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("my other template", null, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template", "my other template"]);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template my other template", "character my template my other template"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_2TemplatesWithFilters_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character"]);
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            var mockApplicator1 = SetupStepInApplicatorChain("my template", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("my other template", filters, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["my template", "my other template"], filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} my template my other template", "character my template my other template"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_3Templates_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character", "wrong creature"]);

            var mockApplicator1 = SetupStepInApplicatorChain("t1", null, (p, _) => !p.Name.Contains("wrong creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("t2", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator3 = SetupStepInApplicatorChain("t3", null, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["t1", "t2", "t3"]);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} t1 t2 t3", "character t1 t2 t3"]));
        }

        [Test]
        public void GetChainedTemplates_FromPrototypes_3TemplatesWithFilters_ReturnsPrototypes()
        {
            var creatures = BuildPrototypes(["some creature", creature, "a different creature", "character", "wrong creature"]);
            var filters = new Filters
            {
                Alignments = ["my alignment"],
                ChallengeRatings = ["my challenge rating"],
                Types = ["my type"]
            };

            var mockApplicator1 = SetupStepInApplicatorChain("t1", null, (p, _) => !p.Name.Contains("wrong creature"));
            var mockApplicator2 = SetupStepInApplicatorChain("t2", null, (p, _) => !p.Name.Contains("some creature"));
            var mockApplicator3 = SetupStepInApplicatorChain("t3", filters, (p, _) => !p.Name.Contains("a different creature"));

            var prototypes = verifier.GetChainedTemplates(creatures, ["t1", "t2", "t3"], filters);
            Assert.That(prototypes.Count(), Is.EqualTo(2));
            Assert.That(prototypes.Select(p => p.Name), Is.EquivalentTo([$"{creature} t1 t2 t3", "character t1 t2 t3"]));
        }
    }
}
