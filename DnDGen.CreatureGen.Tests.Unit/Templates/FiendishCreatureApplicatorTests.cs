using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Attacks;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Feats;
using DnDGen.CreatureGen.Generators.Attacks;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Generators.Feats;
using DnDGen.CreatureGen.Generators.Magics;
using DnDGen.CreatureGen.Languages;
using DnDGen.CreatureGen.Magics;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Templates;
using DnDGen.CreatureGen.Tests.Unit.TestCaseSources;
using DnDGen.CreatureGen.Verifiers.Exceptions;
using DnDGen.Infrastructure.Selectors.Collections;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace DnDGen.CreatureGen.Tests.Unit.Templates
{
    [TestFixture]
    public class FiendishCreatureApplicatorTests
    {
        private TemplateApplicator applicator;
        private Creature baseCreature;
        private Mock<IAttacksGenerator> mockAttackGenerator;
        private Mock<IFeatsGenerator> mockFeatsGenerator;
        private Mock<ICollectionSelector> mockCollectionSelector;
        private Mock<IMagicGenerator> mockMagicGenerator;
        private Mock<IDemographicsGenerator> mockDemographicsGenerator;

        private static readonly string[] AllAlignments =
            [
                AlignmentConstants.ChaoticEvil,
                AlignmentConstants.ChaoticGood,
                AlignmentConstants.ChaoticNeutral,
                AlignmentConstants.LawfulEvil,
                AlignmentConstants.LawfulGood,
                AlignmentConstants.LawfulNeutral,
                AlignmentConstants.NeutralEvil,
                AlignmentConstants.NeutralGood,
                AlignmentConstants.TrueNeutral,
            ];

        [SetUp]
        public void SetUp()
        {
            mockAttackGenerator = new Mock<IAttacksGenerator>();
            mockFeatsGenerator = new Mock<IFeatsGenerator>();
            mockCollectionSelector = new Mock<ICollectionSelector>();
            mockMagicGenerator = new Mock<IMagicGenerator>();
            mockDemographicsGenerator = new Mock<IDemographicsGenerator>();

            applicator = new FiendishCreatureApplicator(
                mockAttackGenerator.Object,
                mockFeatsGenerator.Object,
                mockCollectionSelector.Object,
                mockMagicGenerator.Object,
                mockDemographicsGenerator.Object);

            baseCreature = new CreatureBuilder()
                .WithTestValues()
                .WithCreatureType(CreatureConstants.Types.Humanoid)
                .Build();

            mockDemographicsGenerator
                .Setup(s => s.UpdateByTemplate(baseCreature.Demographics, baseCreature.Name, CreatureConstants.Templates.FiendishCreature, false, false))
                .Returns(baseCreature.Demographics);
        }

        [Test]
        public void ApplyTo_ThrowsException_WhenCreatureNotCompatible()
        {
            baseCreature.Type.Name = CreatureConstants.Types.Outsider;

            var expected = new InvalidCreatureException("Type 'Outsider' is not valid", false, baseCreature.Name, templates: [CreatureConstants.Templates.FiendishCreature]);
            var func = () => applicator.ApplyTo(baseCreature, false);
            Assert.That(func, Throws.InstanceOf<InvalidCreatureException>().With.Message.EqualTo(expected.Message));
        }

        [TestCase(false, "subtype 1", ChallengeRatingConstants.CR1, AlignmentConstants.NeutralEvil)]
        [TestCase(false, "subtype 1", ChallengeRatingConstants.CR2, AlignmentConstants.LawfulEvil)]
        [TestCase(false, "wrong subtype", ChallengeRatingConstants.CR1, AlignmentConstants.LawfulEvil)]
        [TestCase(true, "subtype 1", ChallengeRatingConstants.CR1, AlignmentConstants.NeutralEvil)]
        [TestCase(true, "subtype 1", ChallengeRatingConstants.CR2, AlignmentConstants.LawfulEvil)]
        [TestCase(true, "wrong subtype", ChallengeRatingConstants.CR1, AlignmentConstants.LawfulEvil)]
        public void ApplyTo_ThrowsException_WhenCreatureNotCompatible_WithFilters(bool asCharacter, string type, string challengeRating, string alignment)
        {
            baseCreature.Type.Name = CreatureConstants.Types.Humanoid;
            baseCreature.Type.SubTypes = ["subtype 1", "subtype 2"];
            baseCreature.HitPoints.HitDice[0].Quantity = 1;
            baseCreature.ChallengeRating = ChallengeRatingConstants.CR1;
            baseCreature.Alignment = new Alignment(AlignmentConstants.LawfulNeutral);

            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [challengeRating],
                Alignments = [alignment]
            };
            var (Compatible, Reason) = filters.AreCompatible(
                [AlignmentConstants.LawfulEvil],
                [ChallengeRatingConstants.CR1],
                [CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar]);

            var expected = new InvalidCreatureException(Reason, asCharacter, baseCreature.Name, filters, null, CreatureConstants.Templates.FiendishCreature);
            var func = () => applicator.ApplyTo(baseCreature, asCharacter, filters);
            Assert.That(func, Throws.InstanceOf<InvalidCreatureException>().With.Message.EqualTo(expected.Message));
        }

        [Test]
        public void ApplyTo_ReturnsCreature_WithOtherTemplate()
        {
            baseCreature.Templates.Add("other template");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature.Templates, Has.Count.EqualTo(2));
            Assert.That(creature.Templates[0], Is.EqualTo("other template"));
            Assert.That(creature.Templates[1], Is.EqualTo(CreatureConstants.Templates.FiendishCreature));
        }

        private void SetUpAttack(Attack attack, string gender = null)
        {
            gender ??= baseCreature.Demographics.Gender;

            mockAttackGenerator
                .Setup(g => g.GenerateAttacks(
                    CreatureConstants.Templates.FiendishCreature,
                    baseCreature.Size,
                    baseCreature.BaseAttackBonus,
                    baseCreature.Abilities,
                    baseCreature.HitPoints.RoundedHitDiceQuantity,
                    gender))
                .Returns([attack]);
        }

        [Test]
        public void ApplyTo_ReturnsCreature_WithFilters()
        {
            baseCreature.Type.Name = CreatureConstants.Types.Humanoid;
            baseCreature.Type.SubTypes = ["subtype 1", "subtype 2"];
            baseCreature.HitPoints.HitDice[0].Quantity = 1;
            baseCreature.ChallengeRating = ChallengeRatingConstants.CR1;
            baseCreature.Alignment = new Alignment(AlignmentConstants.LawfulNeutral);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var filters = new Filters
            {
                Types = ["subtype 1"],
                ChallengeRatings = [ChallengeRatingConstants.CR1],
                Alignments = [AlignmentConstants.LawfulEvil]
            };

            var creature = applicator.ApplyTo(baseCreature, false, filters);
            Assert.That(creature.Templates.Single(), Is.EqualTo(CreatureConstants.Templates.FiendishCreature));
        }

        [TestCase(CreatureConstants.Types.Aberration, CreatureConstants.Types.Aberration)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.Dragon, CreatureConstants.Types.Dragon)]
        [TestCase(CreatureConstants.Types.Fey, CreatureConstants.Types.Fey)]
        [TestCase(CreatureConstants.Types.Giant, CreatureConstants.Types.Giant)]
        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Humanoid)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid, CreatureConstants.Types.MonstrousHumanoid)]
        [TestCase(CreatureConstants.Types.Plant, CreatureConstants.Types.Plant)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.MagicalBeast)]
        public void ApplyTo_CreatureTypeIsAdjusted(string original, string adjusted)
        {
            baseCreature.Type.Name = original;
            baseCreature.Type.SubTypes =
            [
                "subtype 1",
                "subtype 2",
            ];

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Type.Name, Is.EqualTo(adjusted));
            if (original == adjusted)
            {
                Assert.That(creature.Type.SubTypes, Is.EquivalentTo(["subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar]));
            }
            else
            {
                Assert.That(creature.Type.SubTypes, Is.EquivalentTo(["subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar,
                    original, CreatureConstants.Types.Subtypes.Augmented]));
            }
        }

        [Test]
        public void ApplyTo_DemographicsAreAdjusted()
        {
            var templateDemographics = new Demographics
            {
                Skin = "fiery",
                Gender = "hellish gender"
            };
            mockDemographicsGenerator
                .Setup(s => s.UpdateByTemplate(baseCreature.Demographics, baseCreature.Name, CreatureConstants.Templates.FiendishCreature, false, false))
                .Returns(templateDemographics);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood, templateDemographics.Gender);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Demographics, Is.EqualTo(templateDemographics));
        }

        [Test]
        public void ApplyTo_CreatureSizeIsNotAdjusted()
        {
            baseCreature.Size = "my size";

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Size, Is.EqualTo("my size"));
        }

        [TestCase(.1, 1)]
        [TestCase(.25, 1)]
        [TestCase(.5, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        [TestCase(6, 6)]
        [TestCase(7, 7)]
        [TestCase(8, 8)]
        [TestCase(9, 9)]
        [TestCase(10, 10)]
        [TestCase(11, 11)]
        [TestCase(12, 12)]
        [TestCase(13, 13)]
        [TestCase(14, 14)]
        [TestCase(15, 15)]
        [TestCase(16, 16)]
        [TestCase(17, 17)]
        [TestCase(18, 18)]
        [TestCase(19, 19)]
        [TestCase(20, 20)]
        [TestCase(21, 20)]
        [TestCase(22, 20)]
        [TestCase(42, 20)]
        public void CreatureGainssmiteGoodSpecialAttack(double hitDiceQuantity, int smiteDamage)
        {
            baseCreature.HitPoints.HitDice[0].Quantity = hitDiceQuantity;

            var originalAttacks = baseCreature.Attacks
                .Select(a => JsonConvert.SerializeObject(a))
                .Select(a => JsonConvert.DeserializeObject<Attack>(a))
                .ToArray();
            var originalSpecialAttacks = baseCreature.SpecialAttacks
                .Select(a => JsonConvert.SerializeObject(a))
                .Select(a => JsonConvert.DeserializeObject<Attack>(a))
                .ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Attacks.Count(), Is.EqualTo(originalAttacks.Length + 1));
            Assert.That(creature.Attacks.Select(a => a.Name), Is.SupersetOf(originalAttacks.Select(a => a.Name)));
            Assert.That(creature.Attacks, Contains.Item(smiteGood));
            Assert.That(creature.SpecialAttacks.Count(), Is.EqualTo(originalSpecialAttacks.Length + 1));
            Assert.That(creature.SpecialAttacks, Contains.Item(smiteGood));

            Assert.That(smiteGood.DamageSummary, Is.EqualTo(smiteDamage.ToString()));
        }

        [Test]
        public void CreatureGainSpecialQualities()
        {
            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
        }

        [Test]
        public void IfCreatureHasWeakerSpellResistance_Replace()
        {
            var spellResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.SpellResistance,
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([spellResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.Darkvision), Is.EqualTo(1));
            Assert.That(spellResistance.Power, Is.EqualTo(5));
        }

        [Test]
        public void IfCreatureHasStrongerSpellResistance_DoNotReplace()
        {
            var spellResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.SpellResistance,
                Power = 10
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([spellResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Except([specialQualities[4]]))
                .And.Not.Contains(specialQualities[4])
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(spellResistance.Power, Is.EqualTo(10));
        }

        [Test]
        public void IfCreatureHasWeakerDarkvision_Replace()
        {
            var darkvision = new Feat
            {
                Name = FeatConstants.SpecialQualities.Darkvision,
                Power = 30
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([darkvision]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.Darkvision), Is.EqualTo(1));
            Assert.That(darkvision.Power, Is.EqualTo(60));
        }

        [Test]
        public void IfCreatureHasStrongerDarkvision_DoNotReplace()
        {
            var darkvision = new Feat
            {
                Name = FeatConstants.SpecialQualities.Darkvision,
                Power = 90
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([darkvision]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Skip(1))
                .And.Not.Contains(specialQualities[0])
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(darkvision.Power, Is.EqualTo(90));
        }

        [TestCase(FeatConstants.Foci.Elements.Cold)]
        [TestCase(FeatConstants.Foci.Elements.Fire)]
        public void IfCreatureHasWeakerEnergyResistance_Replace(string energy)
        {
            var energyResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.EnergyResistance,
                Foci = [energy],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([energyResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var fiendishSpecialQuality = specialQualities.First(f =>
                f.Name == FeatConstants.SpecialQualities.EnergyResistance
                && f.Foci.Contains(energy));

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.EnergyResistance), Is.EqualTo(2));
            Assert.That(energyResistance.Power, Is.EqualTo(5));
        }

        [TestCase(FeatConstants.Foci.Elements.Cold)]
        [TestCase(FeatConstants.Foci.Elements.Fire)]
        public void IfCreatureHasStrongerEnergyResistance_DoNotReplace(string energy)
        {
            var energyResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.EnergyResistance,
                Foci = [energy],
                Power = 15
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([energyResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var fiendishSpecialQuality = specialQualities.First(f =>
                f.Name == FeatConstants.SpecialQualities.EnergyResistance
                && f.Foci.Contains(energy));

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Except([fiendishSpecialQuality]))
                .And.Not.Contains(fiendishSpecialQuality)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(energyResistance.Power, Is.EqualTo(15));
        }

        [TestCase(FeatConstants.Foci.Elements.Acid)]
        [TestCase(FeatConstants.Foci.Elements.Electricity)]
        [TestCase(FeatConstants.Foci.Elements.Sonic)]
        public void IfCreatureHasEnergyResistanceToDifferentEnergy_DoNotReplace(string energy)
        {
            var energyResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.EnergyResistance,
                Foci = [energy],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([energyResistance]);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(energyResistance.Power, Is.EqualTo(2));
        }

        [Test]
        public void IfCreatureHasWeakerDamageReduction_Replace()
        {
            var damageReduction = new Feat
            {
                Name = FeatConstants.SpecialQualities.DamageReduction,
                Foci = ["Vulnerable to magic"],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([damageReduction]);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.SpellResistance), Is.EqualTo(1));
            Assert.That(damageReduction.Power, Is.EqualTo(5));
        }

        [Test]
        public void IfCreatureHasStrongerDamageReduction_DoNotReplace()
        {
            var damageReduction = new Feat
            {
                Name = FeatConstants.SpecialQualities.DamageReduction,
                Foci = ["Vulnerable to magic"],
                Power = 10
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([damageReduction]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Except([specialQualities[3]]))
                .And.Not.Contains(specialQualities[3])
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(damageReduction.Power, Is.EqualTo(10));
        }

        [Test]
        public void IfCreatureHasDamageReductionWithDifferentVulnerability_DoNotReplace()
        {
            var damageReduction = new Feat
            {
                Name = FeatConstants.SpecialQualities.DamageReduction,
                Foci = ["Vulnerable to magic, adamantine"],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([damageReduction]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(damageReduction.Power, Is.EqualTo(2));
        }

        [TestCaseSource(nameof(AbilityAdjustments))]
        public void CreatureIntelligenceAdvancedToAtLeast3(int raceAdjust, int baseScore, int advanced, int adjusted)
        {
            baseCreature.Abilities[AbilityConstants.Intelligence].BaseScore = baseScore;
            baseCreature.Abilities[AbilityConstants.Intelligence].RacialAdjustment = raceAdjust;
            baseCreature.Abilities[AbilityConstants.Intelligence].AdvancementAdjustment = advanced;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(adjusted).And.AtLeast(3));

            if (baseScore + raceAdjust + advanced < 3)
            {
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(3));
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateAdjustment, Is.Zero);
            }
            else
            {
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(-1));
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateAdjustment, Is.Zero);
            }
        }

        private static IEnumerable AbilityAdjustments
        {
            get
            {
                //INFO: Don't need every combination, just want to test the 3 threshold
                yield return new TestCaseData(0, 10, 0, 10);
                yield return new TestCaseData(0, 10, 2, 12);
                yield return new TestCaseData(-2, 10, 0, 8);
                yield return new TestCaseData(-8, 10, 0, 3);
                yield return new TestCaseData(-10, 10, 0, 3);
                yield return new TestCaseData(-8, 10, 2, 4);
            }
        }

        [Test]
        public void IfCreatureDoesNotHaveIntelligence_GainIntelligenceOf3()
        {
            baseCreature.Abilities[AbilityConstants.Intelligence].BaseScore = 0;
            baseCreature.Abilities[AbilityConstants.Intelligence].RacialAdjustment = 0;
            baseCreature.Abilities[AbilityConstants.Intelligence].AdvancementAdjustment = 0;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(3));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].BaseScore, Is.Zero);
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(3));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateAdjustment, Is.Zero);
        }

        [TestCaseSource(nameof(ChallengeRatingAdjustments))]
        public void ApplyTo_ChallengeRatingAdjusted(double hitDiceQuantity, string original, string adjusted)
        {
            baseCreature.HitPoints.HitDice[0].Quantity = hitDiceQuantity;
            baseCreature.ChallengeRating = original;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.HitPoints.HitDiceQuantity, Is.EqualTo(hitDiceQuantity));
            Assert.That(creature.ChallengeRating, Is.EqualTo(adjusted));
        }

        private static IEnumerable ChallengeRatingAdjustments
        {
            get
            {
                var hitDice = new[] { 0.5, 1, 2, 3, 4, 7, 8 };

                //INFO: Don't need to test every CR, since it is the basic Increase functionality, which is tested separately
                //So, we only need to test the amount it is increased, not every CR permutation
                var challengeRating = ChallengeRatingConstants.CR1;

                foreach (var hitDie in hitDice)
                {
                    var increase = 0;

                    if (hitDie > 7)
                    {
                        increase = 2;
                    }
                    else if (hitDie > 3)
                    {
                        increase = 1;
                    }

                    var newCr = ChallengeRatingConstants.IncreaseChallengeRating(challengeRating, increase);
                    yield return new TestCaseData(hitDie, challengeRating, newCr);
                }
            }
        }

        [TestCase(AlignmentConstants.Chaotic, AlignmentConstants.Neutral, AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Chaotic, AlignmentConstants.Evil, AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Neutral, AlignmentConstants.Neutral, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Neutral, AlignmentConstants.Evil, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Lawful, AlignmentConstants.Neutral, AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.Lawful, AlignmentConstants.Evil, AlignmentConstants.LawfulEvil)]
        public void AlignmentAdjusted(string lawfulness, string goodness, string adjusted)
        {
            baseCreature.Alignment.Lawfulness = lawfulness;
            baseCreature.Alignment.Goodness = goodness;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Alignment.Full, Is.EqualTo(adjusted));
        }

        [Test]
        public void ApplyTo_GetPresetAlignment()
        {
            baseCreature.Alignment.Lawfulness = "preset";
            baseCreature.Alignment.Goodness = "alignment";

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var filters = new Filters { Alignments = ["preset Evil"] };

            var creature = applicator.ApplyTo(baseCreature, false, filters);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Alignment.Full, Is.EqualTo("preset Evil"));
        }

        [TestCase(null, null)]
        [TestCase(0, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 4)]
        [TestCase(10, 12)]
        [TestCase(42, 44)]
        public void LevelAdjustmentIncreased(int? adjustment, int? adjusted)
        {
            baseCreature.LevelAdjustment = adjustment;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.LevelAdjustment, Is.EqualTo(adjusted));
        }

        [Test]
        public async Task ApplyToAsync_ThrowsException_WhenCreatureNotCompatible()
        {
            baseCreature.Type.Name = CreatureConstants.Types.Outsider;

            var expected = new InvalidCreatureException("Type 'Outsider' is not valid", false, baseCreature.Name, templates: [CreatureConstants.Templates.FiendishCreature]);
            await Assert.ThatAsync(async () => await applicator.ApplyToAsync(baseCreature, false),
                Throws.InstanceOf<InvalidCreatureException>().With.Message.EqualTo(expected.Message));
        }

        [TestCase(false, "subtype 1", ChallengeRatingConstants.CR1, AlignmentConstants.NeutralEvil)]
        [TestCase(false, "subtype 1", ChallengeRatingConstants.CR2, AlignmentConstants.LawfulEvil)]
        [TestCase(false, "wrong subtype", ChallengeRatingConstants.CR1, AlignmentConstants.LawfulEvil)]
        [TestCase(true, "subtype 1", ChallengeRatingConstants.CR1, AlignmentConstants.NeutralEvil)]
        [TestCase(true, "subtype 1", ChallengeRatingConstants.CR2, AlignmentConstants.LawfulEvil)]
        [TestCase(true, "wrong subtype", ChallengeRatingConstants.CR1, AlignmentConstants.LawfulEvil)]
        public async Task ApplyToAsync_ThrowsException_WhenCreatureNotCompatible_WithFilters(bool asCharacter, string type, string challengeRating, string alignment)
        {
            baseCreature.Type.Name = CreatureConstants.Types.Humanoid;
            baseCreature.Type.SubTypes = ["subtype 1", "subtype 2"];
            baseCreature.HitPoints.HitDice[0].Quantity = 1;
            baseCreature.ChallengeRating = ChallengeRatingConstants.CR1;
            baseCreature.Alignment = new Alignment(AlignmentConstants.LawfulNeutral);

            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [challengeRating],
                Alignments = [alignment]
            };
            var (Compatible, Reason) = filters.AreCompatible(
                [AlignmentConstants.LawfulEvil],
                [ChallengeRatingConstants.CR1],
                [CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar]);

            var expected = new InvalidCreatureException(Reason, asCharacter, baseCreature.Name, filters, null, CreatureConstants.Templates.FiendishCreature);

            await Assert.ThatAsync(async () => await applicator.ApplyToAsync(baseCreature, asCharacter, filters),
                Throws.InstanceOf<InvalidCreatureException>().With.Message.EqualTo(expected.Message));
        }

        [Test]
        public async Task ApplyToAsync_ReturnsCreature_WithOtherTemplate()
        {
            baseCreature.Templates.Add("other template");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature.Templates, Has.Count.EqualTo(2));
            Assert.That(creature.Templates[0], Is.EqualTo("other template"));
            Assert.That(creature.Templates[1], Is.EqualTo(CreatureConstants.Templates.FiendishCreature));
        }

        [Test]
        public async Task ApplyToAsync_ReturnsCreature_WithFilters()
        {
            baseCreature.Type.Name = CreatureConstants.Types.Humanoid;
            baseCreature.Type.SubTypes = ["subtype 1", "subtype 2"];
            baseCreature.HitPoints.HitDice[0].Quantity = 1;
            baseCreature.ChallengeRating = ChallengeRatingConstants.CR1;
            baseCreature.Alignment = new Alignment(AlignmentConstants.LawfulNeutral);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var filters = new Filters
            {
                Types = ["subtype 1"],
                ChallengeRatings = [ChallengeRatingConstants.CR1],
                Alignments = [AlignmentConstants.LawfulEvil]
            };

            var creature = await applicator.ApplyToAsync(baseCreature, false, filters);
            Assert.That(creature.Templates.Single(), Is.EqualTo(CreatureConstants.Templates.FiendishCreature));
        }

        [TestCase(CreatureConstants.Types.Aberration, CreatureConstants.Types.Aberration)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.Dragon, CreatureConstants.Types.Dragon)]
        [TestCase(CreatureConstants.Types.Fey, CreatureConstants.Types.Fey)]
        [TestCase(CreatureConstants.Types.Giant, CreatureConstants.Types.Giant)]
        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Humanoid)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid, CreatureConstants.Types.MonstrousHumanoid)]
        [TestCase(CreatureConstants.Types.Plant, CreatureConstants.Types.Plant)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.MagicalBeast)]
        public async Task ApplyToAsync_CreatureTypeIsAdjusted(string original, string adjusted)
        {
            baseCreature.Type.Name = original;
            baseCreature.Type.SubTypes =
            [
                "subtype 1",
                "subtype 2",
            ];

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Type.Name, Is.EqualTo(adjusted));
            if (original == adjusted)
            {
                Assert.That(creature.Type.SubTypes, Is.EquivalentTo(["subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar]));
            }
            else
            {
                Assert.That(creature.Type.SubTypes, Is.EquivalentTo(["subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar,
                    original, CreatureConstants.Types.Subtypes.Augmented]));
            }
        }

        [Test]
        public async Task ApplyToAsync_DemographicsAreAdjusted()
        {
            var templateDemographics = new Demographics
            {
                Skin = "fiery",
                Gender = "hellish gender"
            };
            mockDemographicsGenerator
                .Setup(s => s.UpdateByTemplate(baseCreature.Demographics, baseCreature.Name, CreatureConstants.Templates.FiendishCreature, false, false))
                .Returns(templateDemographics);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood, templateDemographics.Gender);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Demographics, Is.EqualTo(templateDemographics));
        }

        [Test]
        public async Task ApplyToAsync_CreatureSizeIsNotAdjusted()
        {
            baseCreature.Size = "my size";

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Size, Is.EqualTo("my size"));
        }

        [TestCase(.1, 1)]
        [TestCase(.25, 1)]
        [TestCase(.5, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(5, 5)]
        [TestCase(6, 6)]
        [TestCase(7, 7)]
        [TestCase(8, 8)]
        [TestCase(9, 9)]
        [TestCase(10, 10)]
        [TestCase(11, 11)]
        [TestCase(12, 12)]
        [TestCase(13, 13)]
        [TestCase(14, 14)]
        [TestCase(15, 15)]
        [TestCase(16, 16)]
        [TestCase(17, 17)]
        [TestCase(18, 18)]
        [TestCase(19, 19)]
        [TestCase(20, 20)]
        [TestCase(21, 20)]
        [TestCase(22, 20)]
        [TestCase(42, 20)]
        public async Task ApplyToAsync_CreatureGainssmiteGoodSpecialAttack(double hitDiceQuantity, int smiteDamage)
        {
            baseCreature.HitPoints.HitDice[0].Quantity = hitDiceQuantity;

            var originalAttacks = baseCreature.Attacks
                .Select(a => JsonConvert.SerializeObject(a))
                .Select(a => JsonConvert.DeserializeObject<Attack>(a))
                .ToArray();
            var originalSpecialAttacks = baseCreature.SpecialAttacks
                .Select(a => JsonConvert.SerializeObject(a))
                .Select(a => JsonConvert.DeserializeObject<Attack>(a))
                .ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Attacks.Count(), Is.EqualTo(originalAttacks.Length + 1));
            Assert.That(creature.Attacks.Select(a => a.Name), Is.SupersetOf(originalAttacks.Select(a => a.Name)));
            Assert.That(creature.Attacks, Contains.Item(smiteGood));
            Assert.That(creature.SpecialAttacks.Count(), Is.EqualTo(originalSpecialAttacks.Length + 1));
            Assert.That(creature.SpecialAttacks, Contains.Item(smiteGood));

            Assert.That(smiteGood.DamageSummary, Is.EqualTo(smiteDamage.ToString()));
        }

        [Test]
        public async Task ApplyToAsync_CreatureGainSpecialQualities()
        {
            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasWeakerSpellResistance_Replace()
        {
            var spellResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.SpellResistance,
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([spellResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.Darkvision), Is.EqualTo(1));
            Assert.That(spellResistance.Power, Is.EqualTo(5));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasStrongerSpellResistance_DoNotReplace()
        {
            var spellResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.SpellResistance,
                Power = 10
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([spellResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Except([specialQualities[4]]))
                .And.Not.Contains(specialQualities[4])
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(spellResistance.Power, Is.EqualTo(10));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasWeakerDarkvision_Replace()
        {
            var darkvision = new Feat
            {
                Name = FeatConstants.SpecialQualities.Darkvision,
                Power = 30
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([darkvision]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.Darkvision), Is.EqualTo(1));
            Assert.That(darkvision.Power, Is.EqualTo(60));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasStrongerDarkvision_DoNotReplace()
        {
            var darkvision = new Feat
            {
                Name = FeatConstants.SpecialQualities.Darkvision,
                Power = 90
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([darkvision]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Skip(1))
                .And.Not.Contains(specialQualities[0])
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(darkvision.Power, Is.EqualTo(90));
        }

        [TestCase(FeatConstants.Foci.Elements.Cold)]
        [TestCase(FeatConstants.Foci.Elements.Fire)]
        public async Task ApplyToAsync_IfCreatureHasWeakerEnergyResistance_Replace(string energy)
        {
            var energyResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.EnergyResistance,
                Foci = [energy],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([energyResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var fiendishSpecialQuality = specialQualities.First(f =>
                f.Name == FeatConstants.SpecialQualities.EnergyResistance
                && f.Foci.Contains(energy));

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.EnergyResistance), Is.EqualTo(2));
            Assert.That(energyResistance.Power, Is.EqualTo(5));
        }

        [TestCase(FeatConstants.Foci.Elements.Cold)]
        [TestCase(FeatConstants.Foci.Elements.Fire)]
        public async Task ApplyToAsync_IfCreatureHasStrongerEnergyResistance_DoNotReplace(string energy)
        {
            var energyResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.EnergyResistance,
                Foci = [energy],
                Power = 15
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([energyResistance]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var fiendishSpecialQuality = specialQualities.First(f =>
                f.Name == FeatConstants.SpecialQualities.EnergyResistance
                && f.Foci.Contains(energy));

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Except([fiendishSpecialQuality]))
                .And.Not.Contains(fiendishSpecialQuality)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(energyResistance.Power, Is.EqualTo(15));
        }

        [TestCase(FeatConstants.Foci.Elements.Acid)]
        [TestCase(FeatConstants.Foci.Elements.Electricity)]
        [TestCase(FeatConstants.Foci.Elements.Sonic)]
        public async Task ApplyToAsync_IfCreatureHasEnergyResistanceToDifferentEnergy_DoNotReplace(string energy)
        {
            var energyResistance = new Feat
            {
                Name = FeatConstants.SpecialQualities.EnergyResistance,
                Foci = [energy],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([energyResistance]);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 0 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(energyResistance.Power, Is.EqualTo(2));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasWeakerDamageReduction_Replace()
        {
            var damageReduction = new Feat
            {
                Name = FeatConstants.SpecialQualities.DamageReduction,
                Foci = ["Vulnerable to magic"],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([damageReduction]);

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(creature.SpecialQualities.Count(sq => sq.Name == FeatConstants.SpecialQualities.SpellResistance), Is.EqualTo(1));
            Assert.That(damageReduction.Power, Is.EqualTo(5));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasStrongerDamageReduction_DoNotReplace()
        {
            var damageReduction = new Feat
            {
                Name = FeatConstants.SpecialQualities.DamageReduction,
                Foci = ["Vulnerable to magic"],
                Power = 10
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([damageReduction]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length - 1));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities.Except([specialQualities[3]]))
                .And.Not.Contains(specialQualities[3])
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(damageReduction.Power, Is.EqualTo(10));
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureHasDamageReductionWithDifferentVulnerability_DoNotReplace()
        {
            var damageReduction = new Feat
            {
                Name = FeatConstants.SpecialQualities.DamageReduction,
                Foci = ["Vulnerable to magic, adamantine"],
                Power = 2
            };
            baseCreature.SpecialQualities = baseCreature.SpecialQualities
                .Union([damageReduction]);

            var originalSpecialQualities = baseCreature.SpecialQualities.ToArray();
            var originalSubtypes = baseCreature.Type.SubTypes.ToArray();

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var specialQualities = new[]
            {
                new Feat { Name = FeatConstants.SpecialQualities.Darkvision, Power = 60 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Cold], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.EnergyResistance, Foci = [FeatConstants.Foci.Elements.Fire], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.DamageReduction, Foci = ["Vulnerable to magic"], Power = 5 },
                new Feat { Name = FeatConstants.SpecialQualities.SpellResistance, Power = 5 },
            };

            mockFeatsGenerator
                .Setup(g => g.GenerateSpecialQualities(
                    CreatureConstants.Templates.FiendishCreature,
                    It.Is<CreatureType>(ct => ct.Name == CreatureConstants.Types.Humanoid
                        && ct.SubTypes.IsEquivalentTo(originalSubtypes.Union(new[]
                        {
                            CreatureConstants.Types.Subtypes.Extraplanar,
                        }))),
                    baseCreature.HitPoints,
                    baseCreature.Abilities,
                    baseCreature.Skills,
                    baseCreature.CanUseEquipment,
                    baseCreature.Size,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil && a.Lawfulness == baseCreature.Alignment.Lawfulness)))
                .Returns(specialQualities);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.SpecialQualities.Count(), Is.GreaterThan(originalSpecialQualities.Length)
                .And.EqualTo(originalSpecialQualities.Length + specialQualities.Length));
            Assert.That(creature.SpecialQualities, Is.SupersetOf(specialQualities)
                .And.SupersetOf(originalSpecialQualities));
            Assert.That(damageReduction.Power, Is.EqualTo(2));
        }

        [TestCaseSource(nameof(AbilityAdjustments))]
        public async Task ApplyToAsync_CreatureIntelligenceAdvancedToAtLeast3(int raceAdjust, int baseScore, int advanced, int adjusted)
        {
            baseCreature.Abilities[AbilityConstants.Intelligence].BaseScore = baseScore;
            baseCreature.Abilities[AbilityConstants.Intelligence].RacialAdjustment = raceAdjust;
            baseCreature.Abilities[AbilityConstants.Intelligence].AdvancementAdjustment = advanced;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(adjusted).And.AtLeast(3));

            if (baseScore + raceAdjust + advanced < 3)
            {
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(3));
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateAdjustment, Is.Zero);
            }
            else
            {
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(-1));
                Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateAdjustment, Is.Zero);
            }
        }

        [Test]
        public async Task ApplyToAsync_IfCreatureDoesNotHaveIntelligence_GainIntelligenceOf3()
        {
            baseCreature.Abilities[AbilityConstants.Intelligence].BaseScore = 0;
            baseCreature.Abilities[AbilityConstants.Intelligence].RacialAdjustment = 0;
            baseCreature.Abilities[AbilityConstants.Intelligence].AdvancementAdjustment = 0;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(3));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].BaseScore, Is.Zero);
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(3));
            Assert.That(creature.Abilities[AbilityConstants.Intelligence].TemplateAdjustment, Is.Zero);
        }

        [TestCaseSource(nameof(ChallengeRatingAdjustments))]
        public async Task ApplyToAsync_ChallengeRatingAdjusted(double hitDiceQuantity, string original, string adjusted)
        {
            baseCreature.HitPoints.HitDice[0].Quantity = hitDiceQuantity;
            baseCreature.ChallengeRating = original;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.HitPoints.HitDiceQuantity, Is.EqualTo(hitDiceQuantity));
            Assert.That(creature.ChallengeRating, Is.EqualTo(adjusted));
        }

        [TestCase(AlignmentConstants.Chaotic, AlignmentConstants.Neutral, AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Chaotic, AlignmentConstants.Evil, AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.Neutral, AlignmentConstants.Neutral, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Neutral, AlignmentConstants.Evil, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.Lawful, AlignmentConstants.Neutral, AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.Lawful, AlignmentConstants.Evil, AlignmentConstants.LawfulEvil)]
        public async Task ApplyToAsync_AlignmentAdjusted(string lawfulness, string goodness, string adjusted)
        {
            baseCreature.Alignment.Lawfulness = lawfulness;
            baseCreature.Alignment.Goodness = goodness;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Alignment.Full, Is.EqualTo(adjusted));
        }

        [Test]
        public async Task ApplyToAsync_GetPresetAlignment()
        {
            baseCreature.Alignment.Lawfulness = "preset";
            baseCreature.Alignment.Goodness = "alignment";

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var filters = new Filters { Alignments = ["preset Evil"] };

            var creature = await applicator.ApplyToAsync(baseCreature, false, filters);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Alignment.Full, Is.EqualTo("preset Evil"));
        }

        [TestCase(null, null)]
        [TestCase(0, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 4)]
        [TestCase(10, 12)]
        [TestCase(42, 44)]
        public async Task ApplyToAsync_LevelAdjustmentIncreased(int? adjustment, int? adjusted)
        {
            baseCreature.LevelAdjustment = adjustment;

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.LevelAdjustment, Is.EqualTo(adjusted));
        }

        [Test]
        public void ApplyTo_GainARandomLanguage()
        {
            var originalLanguages = baseCreature.Languages.ToArray();

            mockCollectionSelector
                .Setup(s => s.SelectRandomFrom(
                    Config.Name,
                    TableNameConstants.Collection.LanguageGroups,
                    CreatureConstants.Templates.FiendishCreature + LanguageConstants.Groups.Automatic))
                .Returns("Mordor");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Languages.Count(), Is.EqualTo(originalLanguages.Length + 1));
            Assert.That(creature.Languages, Is.SupersetOf(originalLanguages)
                .And.Contains("Mordor"));
        }

        [Test]
        public void ApplyTo_GainALanguage_NoLanguages()
        {
            baseCreature.Languages = [];

            mockCollectionSelector
                .Setup(s => s.SelectRandomFrom(
                    Config.Name,
                    TableNameConstants.Collection.LanguageGroups,
                    CreatureConstants.Templates.FiendishCreature + LanguageConstants.Groups.Automatic))
                .Returns("Mordor");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Languages, Is.Empty);
        }

        [Test]
        public void ApplyTo_GainALanguage_AlreadyHas()
        {
            baseCreature.Languages = baseCreature.Languages.Union(["Mordor"]);
            var originalLanguages = baseCreature.Languages.ToArray();

            mockCollectionSelector
                .Setup(s => s.SelectRandomFrom(
                    Config.Name,
                    TableNameConstants.Collection.LanguageGroups,
                    CreatureConstants.Templates.FiendishCreature + LanguageConstants.Groups.Automatic))
                .Returns("Mordor");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Languages.Count(), Is.EqualTo(originalLanguages.Length));
            Assert.That(creature.Languages, Is.SupersetOf(originalLanguages)
                .And.Contains("Mordor"));
        }

        [Test]
        public async Task ApplyToAsync_GainARandomLanguage()
        {
            var originalLanguages = baseCreature.Languages.ToArray();

            mockCollectionSelector
                .Setup(s => s.SelectRandomFrom(
                    Config.Name,
                    TableNameConstants.Collection.LanguageGroups,
                    CreatureConstants.Templates.FiendishCreature + LanguageConstants.Groups.Automatic))
                .Returns("Mordor");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Languages.Count(), Is.EqualTo(originalLanguages.Length + 1));
            Assert.That(creature.Languages, Is.SupersetOf(originalLanguages)
                .And.Contains("Mordor"));
        }

        [Test]
        public async Task ApplyToAsync_GainALanguage_NoLanguages()
        {
            baseCreature.Languages = [];

            mockCollectionSelector
                .Setup(s => s.SelectRandomFrom(
                    Config.Name,
                    TableNameConstants.Collection.LanguageGroups,
                    CreatureConstants.Templates.FiendishCreature + LanguageConstants.Groups.Automatic))
                .Returns("Mordor");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Languages, Is.Empty);
        }

        [Test]
        public async Task ApplyToAsync_GainALanguage_AlreadyHas()
        {
            baseCreature.Languages = baseCreature.Languages.Union(["Mordor"]);
            var originalLanguages = baseCreature.Languages.ToArray();

            mockCollectionSelector
                .Setup(s => s.SelectRandomFrom(
                    Config.Name,
                    TableNameConstants.Collection.LanguageGroups,
                    CreatureConstants.Templates.FiendishCreature + LanguageConstants.Groups.Automatic))
                .Returns("Mordor");

            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Languages.Count(), Is.EqualTo(originalLanguages.Length));
            Assert.That(creature.Languages, Is.SupersetOf(originalLanguages)
                .And.Contains("Mordor"));
        }

        [Test]
        public void ApplyTo_RegenerateMagic()
        {
            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var newMagic = new Magic();
            mockMagicGenerator
                .Setup(g => g.GenerateWith(
                    baseCreature.Name,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil),
                    baseCreature.Abilities,
                    baseCreature.Equipment))
                .Returns(newMagic);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Magic, Is.EqualTo(newMagic));
        }

        [Test]
        public async Task ApplyToAsync_RegenerateMagic()
        {
            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var newMagic = new Magic();
            mockMagicGenerator
                .Setup(g => g.GenerateWith(
                    baseCreature.Name,
                    It.Is<Alignment>(a => a.Goodness == AlignmentConstants.Evil),
                    baseCreature.Abilities,
                    baseCreature.Equipment))
                .Returns(newMagic);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Magic, Is.EqualTo(newMagic));
        }

        [Test]
        public void ApplyTo_SetsTemplate()
        {
            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = applicator.ApplyTo(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Templates.Single(), Is.EqualTo(CreatureConstants.Templates.FiendishCreature));
        }

        [Test]
        public async Task ApplyToAsync_SetsTemplate()
        {
            var smiteGood = new Attack
            {
                Name = "Smite Good",
                IsSpecial = true
            };
            SetUpAttack(smiteGood);

            var creature = await applicator.ApplyToAsync(baseCreature, false);
            Assert.That(creature, Is.EqualTo(baseCreature));
            Assert.That(creature.Templates.Single(), Is.EqualTo(CreatureConstants.Templates.FiendishCreature));
        }

        [Test]
        public void IsCompatible_ReturnsTrue()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var compatible = applicator.IsCompatible(creature);
            Assert.That(compatible, Is.True);
        }

        [TestCase(CreatureConstants.Types.Aberration)]
        [TestCase(CreatureConstants.Types.Animal)]
        [TestCase(CreatureConstants.Types.Construct)]
        [TestCase(CreatureConstants.Types.Dragon)]
        [TestCase(CreatureConstants.Types.Elemental)]
        [TestCase(CreatureConstants.Types.Fey)]
        [TestCase(CreatureConstants.Types.Giant)]
        [TestCase(CreatureConstants.Types.Humanoid)]
        [TestCase(CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid)]
        [TestCase(CreatureConstants.Types.Ooze)]
        [TestCase(CreatureConstants.Types.Outsider)]
        [TestCase(CreatureConstants.Types.Plant)]
        [TestCase(CreatureConstants.Types.Undead)]
        [TestCase(CreatureConstants.Types.Vermin)]
        public void IsCompatible_ReturnsFalse_IfIncorporeal(string creatureType)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(creatureType, "subtype 1", CreatureConstants.Types.Subtypes.Incorporeal, "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var compatible = applicator.IsCompatible(creature);
            Assert.That(compatible, Is.False);
        }

        [TestCase(CreatureConstants.Types.Aberration, true)]
        [TestCase(CreatureConstants.Types.Animal, true)]
        [TestCase(CreatureConstants.Types.Construct, false)]
        [TestCase(CreatureConstants.Types.Dragon, true)]
        [TestCase(CreatureConstants.Types.Elemental, false)]
        [TestCase(CreatureConstants.Types.Fey, true)]
        [TestCase(CreatureConstants.Types.Giant, true)]
        [TestCase(CreatureConstants.Types.Humanoid, true)]
        [TestCase(CreatureConstants.Types.MagicalBeast, true)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid, true)]
        [TestCase(CreatureConstants.Types.Ooze, true)]
        [TestCase(CreatureConstants.Types.Outsider, false)]
        [TestCase(CreatureConstants.Types.Plant, true)]
        [TestCase(CreatureConstants.Types.Undead, false)]
        [TestCase(CreatureConstants.Types.Vermin, true)]
        public void IsCompatible_ReturnsCompatibility_BasedOnCreatureType(string creatureType, bool expected)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(creatureType, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var compatible = applicator.IsCompatible(creature);
            Assert.That(compatible, Is.EqualTo(expected));
        }

        [TestCase(AlignmentConstants.LawfulGood, false)]
        [TestCase(AlignmentConstants.NeutralGood, false)]
        [TestCase(AlignmentConstants.ChaoticGood, false)]
        [TestCase(AlignmentConstants.LawfulNeutral, true)]
        [TestCase(AlignmentConstants.TrueNeutral, true)]
        [TestCase(AlignmentConstants.ChaoticNeutral, true)]
        [TestCase(AlignmentConstants.LawfulEvil, true)]
        [TestCase(AlignmentConstants.NeutralEvil, true)]
        [TestCase(AlignmentConstants.ChaoticEvil, true)]
        public void IsCompatible_ReturnsCompatibility_MustHaveNonGoodAlignment(string alignment, bool expected)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(alignment, "other Good")
                .Build();

            var compatible = applicator.IsCompatible(creature);
            Assert.That(compatible, Is.EqualTo(expected));
        }

        [TestCase(AlignmentConstants.LawfulGood, false)]
        [TestCase(AlignmentConstants.NeutralGood, false)]
        [TestCase(AlignmentConstants.ChaoticGood, false)]
        [TestCase(AlignmentConstants.LawfulNeutral, true)]
        [TestCase(AlignmentConstants.TrueNeutral, true)]
        [TestCase(AlignmentConstants.ChaoticNeutral, true)]
        [TestCase(AlignmentConstants.LawfulEvil, true)]
        [TestCase(AlignmentConstants.NeutralEvil, true)]
        [TestCase(AlignmentConstants.ChaoticEvil, true)]
        public void IsCompatible_ReturnsCompatibility_MustHaveAnyNonGoodAlignment(string alignment, bool expected)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments("other Good", alignment)
                .Build();

            var compatible = applicator.IsCompatible(creature);
            Assert.That(compatible, Is.EqualTo(expected));
        }

        [TestCase(AlignmentConstants.ChaoticGood)]
        [TestCase(AlignmentConstants.ChaoticNeutral)]
        [TestCase(AlignmentConstants.LawfulGood)]
        [TestCase(AlignmentConstants.LawfulNeutral)]
        [TestCase(AlignmentConstants.NeutralGood)]
        [TestCase(AlignmentConstants.TrueNeutral)]
        public void IsCompatible_WithAlignment_ReturnsFalse_WhenAlignmentFilterInvalid(string alignment)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AllAlignments)
                .Build();

            var filters = new Filters { Alignments = [alignment] };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.False);
        }

        [TestCase(AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.NeutralEvil)]
        public void IsCompatible_WithAlignment_ReturnsTrue_WhenAlignmentFilterValid(string alignment)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AllAlignments)
                .Build();

            var filters = new Filters { Alignments = [alignment] };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.True);
        }

        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulGood, false)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.NeutralGood, false)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.ChaoticGood, false)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulNeutral, true)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.TrueNeutral, false)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.ChaoticNeutral, false)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulEvil, true)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.NeutralEvil, false)]
        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.ChaoticEvil, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.LawfulGood, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.NeutralGood, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.ChaoticGood, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.LawfulNeutral, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.TrueNeutral, true)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.ChaoticNeutral, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.LawfulEvil, false)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.NeutralEvil, true)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.ChaoticEvil, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.LawfulGood, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.NeutralGood, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.ChaoticGood, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.LawfulNeutral, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.TrueNeutral, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.ChaoticNeutral, true)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.LawfulEvil, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.NeutralEvil, false)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.ChaoticEvil, true)]
        public void IsCompatible_WithAlignment_ReturnsCompatibility_AdjustedAlignmentMustMatch(string alignmentFilter, string creatureAlignment, bool expected)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments("other Evil", creatureAlignment)
                .Build();

            var filters = new Filters { Alignments = [alignmentFilter] };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.EqualTo(expected));
        }

        [TestCase(0.5, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1_2nd, true)]
        [TestCase(0.5, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR2, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR3, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR1, true)]
        [TestCase(0.5, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR2, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR3, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR4, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR2, true)]
        [TestCase(0.5, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR3, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR4, false)]
        [TestCase(0.5, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR5, false)]
        [TestCase(1, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1_2nd, true)]
        [TestCase(1, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1, false)]
        [TestCase(1, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR2, false)]
        [TestCase(1, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR3, false)]
        [TestCase(1, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR1, true)]
        [TestCase(1, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR2, false)]
        [TestCase(1, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR3, false)]
        [TestCase(1, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR4, false)]
        [TestCase(1, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR2, true)]
        [TestCase(1, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR3, false)]
        [TestCase(1, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR4, false)]
        [TestCase(1, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR5, false)]
        [TestCase(4, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1_2nd, false)]
        [TestCase(4, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1, true)]
        [TestCase(4, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR2, false)]
        [TestCase(4, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR3, false)]
        [TestCase(4, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR1, false)]
        [TestCase(4, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR2, true)]
        [TestCase(4, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR3, false)]
        [TestCase(4, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR4, false)]
        [TestCase(4, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR2, false)]
        [TestCase(4, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR3, true)]
        [TestCase(4, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR4, false)]
        [TestCase(4, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR5, false)]
        [TestCase(8, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1_2nd, false)]
        [TestCase(8, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1, false)]
        [TestCase(8, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR2, true)]
        [TestCase(8, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR3, false)]
        [TestCase(8, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR1, false)]
        [TestCase(8, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR2, false)]
        [TestCase(8, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR3, true)]
        [TestCase(8, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR4, false)]
        [TestCase(8, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR2, false)]
        [TestCase(8, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR3, false)]
        [TestCase(8, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR4, true)]
        [TestCase(8, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR5, false)]
        [TestCase(20, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1_2nd, false)]
        [TestCase(20, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR1, false)]
        [TestCase(20, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR2, true)]
        [TestCase(20, ChallengeRatingConstants.CR1_2nd, ChallengeRatingConstants.CR3, false)]
        [TestCase(20, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR1, false)]
        [TestCase(20, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR2, false)]
        [TestCase(20, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR3, true)]
        [TestCase(20, ChallengeRatingConstants.CR1, ChallengeRatingConstants.CR4, false)]
        [TestCase(20, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR2, false)]
        [TestCase(20, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR3, false)]
        [TestCase(20, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR4, true)]
        [TestCase(20, ChallengeRatingConstants.CR2, ChallengeRatingConstants.CR5, false)]
        public void IsCompatible_WithChallengeRating_ReturnsCompatibility_AdjustedChallengeRatingMustMatch(
            double hitDiceQuantity,
            string original,
            string challengeRating,
            bool expected)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .WithChallengeRating(original)
                .WithHitDiceQuantity(hitDiceQuantity)
                .Build();

            var filters = new Filters { ChallengeRatings = [challengeRating] };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.EqualTo(expected));
        }

        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Subtypes.Extraplanar)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.Subtypes.Augmented)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.Subtypes.Extraplanar)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.Subtypes.Augmented)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.Subtypes.Extraplanar)]
        public void IsCompatible_WithType_ReturnsTrue(string type, string filter)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(type, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var filters = new Filters { Types = [filter] };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.True);
        }

        [TestCase(CreatureConstants.Types.Humanoid, null, true)]
        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Humanoid, true)]
        [TestCase(CreatureConstants.Types.Humanoid, "subtype 1", true)]
        [TestCase(CreatureConstants.Types.Humanoid, "subtype 2", true)]
        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Subtypes.Extraplanar, true)]
        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Subtypes.Augmented, false)]
        [TestCase(CreatureConstants.Types.Humanoid, "wrong type", false)]
        [TestCase(CreatureConstants.Types.Animal, null, true)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.Animal, true)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.MagicalBeast, true)]
        [TestCase(CreatureConstants.Types.Animal, "subtype 1", true)]
        [TestCase(CreatureConstants.Types.Animal, "subtype 2", true)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.Subtypes.Extraplanar, true)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.Subtypes.Augmented, true)]
        [TestCase(CreatureConstants.Types.Animal, "wrong type", false)]
        [TestCase(CreatureConstants.Types.Vermin, null, true)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.Vermin, true)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.MagicalBeast, true)]
        [TestCase(CreatureConstants.Types.Vermin, "subtype 1", true)]
        [TestCase(CreatureConstants.Types.Vermin, "subtype 2", true)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.Subtypes.Extraplanar, true)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.Subtypes.Augmented, true)]
        [TestCase(CreatureConstants.Types.Vermin, "wrong type", false)]
        [TestCase(CreatureConstants.Types.MagicalBeast, null, true)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.MagicalBeast, true)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.Vermin, false)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.Animal, false)]
        [TestCase(CreatureConstants.Types.MagicalBeast, "subtype 1", true)]
        [TestCase(CreatureConstants.Types.MagicalBeast, "subtype 2", true)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.Subtypes.Extraplanar, true)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.Subtypes.Augmented, false)]
        [TestCase(CreatureConstants.Types.MagicalBeast, "wrong type", false)]
        public void IsCompatible_WithType_ReturnsCompatibility_AdjustedTypeMustMatch(string originalType, string filterType, bool expected)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(originalType, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var filters = new Filters { Types = [filterType] };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.EqualTo(expected));
        }

        [Test]
        public void IsCompatible_WithAllFilters_ReturnsTrue()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Animal, "subtype 1", "subtype 2")
                .WithAlignments("wrong Good", AlignmentConstants.TrueNeutral, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR2)
                .WithHitDiceQuantity(8)
                .Build();

            var filters = new Filters
            {
                Alignments = [AlignmentConstants.NeutralEvil],
                ChallengeRatings = [ChallengeRatingConstants.CR4],
                Types = [CreatureConstants.Types.MagicalBeast],
            };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.True);
        }

        [Test]
        public void IsCompatible_WithAllFilters_ReturnsFalse_BecausePrototype()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Outsider, "subtype 1", "subtype 2")
                .WithAlignments("wrong Good", AlignmentConstants.TrueNeutral, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR2)
                .WithHitDiceQuantity(8)
                .Build();

            var filters = new Filters
            {
                Alignments = [AlignmentConstants.NeutralEvil],
                ChallengeRatings = [ChallengeRatingConstants.CR4],
                Types = [CreatureConstants.Types.MagicalBeast],
            };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.False);
        }

        [Test]
        public void IsCompatible_WithAllFilters_ReturnsFalse_BecauseAlignment()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Animal, "subtype 1", "subtype 2")
                .WithAlignments("wrong Good", AlignmentConstants.TrueNeutral, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR2)
                .WithHitDiceQuantity(8)
                .Build();

            var filters = new Filters
            {
                Alignments = [AlignmentConstants.ChaoticEvil],
                ChallengeRatings = [ChallengeRatingConstants.CR4],
                Types = [CreatureConstants.Types.MagicalBeast],
            };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.False);
        }

        [Test]
        public void IsCompatible_WithAllFilters_ReturnsFalse_BecauseChallengeRating()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Animal, "subtype 1", "subtype 2")
                .WithAlignments("wrong Good", AlignmentConstants.TrueNeutral, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR2)
                .WithHitDiceQuantity(8)
                .Build();

            var filters = new Filters
            {
                Alignments = [AlignmentConstants.NeutralEvil],
                ChallengeRatings = [ChallengeRatingConstants.CR5],
                Types = [CreatureConstants.Types.MagicalBeast],
            };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.False);
        }

        [Test]
        public void IsCompatible_WithAllFilters_ReturnsFalse_BecauseType()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Animal, "subtype 1", "subtype 2")
                .WithAlignments("wrong Good", AlignmentConstants.TrueNeutral, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR2)
                .WithHitDiceQuantity(8)
                .Build();

            var filters = new Filters
            {
                Alignments = [AlignmentConstants.NeutralEvil],
                ChallengeRatings = [ChallengeRatingConstants.CR4],
                Types = [CreatureConstants.Types.Humanoid],
            };

            var compatible = applicator.IsCompatible(creature, filters);
            Assert.That(compatible, Is.False);
        }

        [Test]
        public void ApplyTo_Prototype_ThrowsException_WhenCreatureNotCompatible()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Outsider, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var expected = new InvalidCreatureException("Type 'Outsider' is not valid", false, creature.Name, templates: [CreatureConstants.Templates.FiendishCreature]);
            var func = () => applicator.ApplyTo(creature);
            Assert.That(func, Throws.InstanceOf<InvalidCreatureException>().With.Message.EqualTo(expected.Message));
        }

        [TestCase(false, "subtype 1", ChallengeRatingConstants.CR1, AlignmentConstants.NeutralEvil)]
        [TestCase(false, "subtype 1", ChallengeRatingConstants.CR2, AlignmentConstants.LawfulEvil)]
        [TestCase(false, "wrong subtype", ChallengeRatingConstants.CR1, AlignmentConstants.LawfulEvil)]
        [TestCase(true, "subtype 1", ChallengeRatingConstants.CR1, AlignmentConstants.NeutralEvil)]
        [TestCase(true, "subtype 1", ChallengeRatingConstants.CR2, AlignmentConstants.LawfulEvil)]
        [TestCase(true, "wrong subtype", ChallengeRatingConstants.CR1, AlignmentConstants.LawfulEvil)]
        public void ApplyTo_Prototype_ThrowsException_WhenCreatureNotCompatible_WithFilters(bool asCharacter, string type, string challengeRating, string alignment)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulNeutral)
                .WithHitDiceQuantity(1)
                .WithChallengeRating(ChallengeRatingConstants.CR1)
                .Build();
            creature.AsCharacter = asCharacter;

            var filters = new Filters
            {
                Types = [type],
                ChallengeRatings = [challengeRating],
                Alignments = [alignment]
            };
            var (Compatible, Reason) = filters.AreCompatible(
                [AlignmentConstants.LawfulEvil],
                [ChallengeRatingConstants.CR1],
                [CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar]);

            var expected = new InvalidCreatureException(Reason, asCharacter, creature.Name, filters, null, CreatureConstants.Templates.FiendishCreature);
            var func = () => applicator.ApplyTo(creature, filters);
            Assert.That(func, Throws.InstanceOf<InvalidCreatureException>().With.Message.EqualTo(expected.Message));
        }

        [Test]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR1)
                .WithCasterLevel(9266)
                .WithLevelAdjustment(90210)
                .WithHitDiceQuantity(9)
                .WithoutAbility(AbilityConstants.Strength)
                .WithAbility(AbilityConstants.Constitution, 90210)
                .WithAbility(AbilityConstants.Dexterity, 42)
                .WithAbility(AbilityConstants.Intelligence, 600)
                .WithAbility(AbilityConstants.Wisdom, 1337)
                .WithAbility(AbilityConstants.Charisma, 1336)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Type, Is.Not.Null);
            Assert.That(updatedPrototype.Type.Name, Is.EqualTo(CreatureConstants.Types.Humanoid));
            Assert.That(updatedPrototype.Type.SubTypes, Is.EqualTo(
            [
                "subtype 1",
                "subtype 2",
                CreatureConstants.Types.Subtypes.Extraplanar,
            ]));
            Assert.That(updatedPrototype.Abilities, Has.Count.EqualTo(6));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Strength].FullScore, Is.EqualTo(0));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Strength].TemplateScore, Is.EqualTo(-1));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Charisma].FullScore, Is.EqualTo(1346));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Charisma].TemplateScore, Is.EqualTo(-1));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(610));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(-1));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Wisdom].FullScore, Is.EqualTo(1347));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Wisdom].TemplateScore, Is.EqualTo(-1));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Dexterity].FullScore, Is.EqualTo(52));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Dexterity].TemplateScore, Is.EqualTo(-1));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Constitution].FullScore, Is.EqualTo(90220));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Constitution].TemplateScore, Is.EqualTo(-1));
            Assert.That(updatedPrototype.Alignments, Is.EqualTo(
            [
                new Alignment(AlignmentConstants.LawfulEvil),
                new Alignment("other Evil"),
            ]));
            Assert.That(updatedPrototype.CasterLevel, Is.EqualTo(9266 + 0));
            Assert.That(updatedPrototype.ChallengeRating, Is.EqualTo(ChallengeRatingConstants.CR3));
            Assert.That(updatedPrototype.LevelAdjustment, Is.EqualTo(90210 + 2));
            Assert.That(updatedPrototype.HitDiceQuantity, Is.EqualTo(9 + 0));
            Assert.That(updatedPrototype.Templates, Is.EqualTo([CreatureConstants.Templates.FiendishCreature]));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_WithSetIntelligence(int lowScore)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR1)
                .WithCasterLevel(9266)
                .WithLevelAdjustment(90210)
                .WithHitDiceQuantity(9)
                .WithAbility(AbilityConstants.Intelligence, lowScore - 10)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Abilities, Has.Count.EqualTo(6));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(3));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(3));
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        [TestCase(18)]
        [TestCase(20)]
        [TestCase(100)]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_WithUnalteredIntelligence(int score)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .WithChallengeRating(ChallengeRatingConstants.CR1)
                .WithCasterLevel(9266)
                .WithLevelAdjustment(90210)
                .WithHitDiceQuantity(9)
                .WithAbility(AbilityConstants.Intelligence, score - 10)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Abilities, Has.Count.EqualTo(6));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Intelligence].FullScore, Is.EqualTo(score));
            Assert.That(updatedPrototype.Abilities[AbilityConstants.Intelligence].TemplateScore, Is.EqualTo(-1));
        }

        [Test]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_FilteringOutGoodAlignments()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other Good", "other alignment", AlignmentConstants.NeutralGood)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Alignments, Is.EqualTo(
            [
                new Alignment(AlignmentConstants.LawfulEvil),
                new Alignment("other Evil"),
            ]));
        }

        [Test]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_PreserveAlignmentWeighting()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulNeutral, AlignmentConstants.NeutralEvil)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Alignments, Is.EquivalentTo(
            [
                new Alignment(AlignmentConstants.LawfulEvil),
                new Alignment(AlignmentConstants.LawfulEvil),
                new Alignment(AlignmentConstants.LawfulEvil),
                new Alignment(AlignmentConstants.NeutralEvil),
            ]));
        }

        [TestCase(AlignmentConstants.LawfulEvil, AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.NeutralEvil, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.ChaoticEvil, AlignmentConstants.ChaoticEvil)]
        [TestCase(AlignmentConstants.LawfulNeutral, AlignmentConstants.LawfulEvil)]
        [TestCase(AlignmentConstants.TrueNeutral, AlignmentConstants.NeutralEvil)]
        [TestCase(AlignmentConstants.ChaoticNeutral, AlignmentConstants.ChaoticEvil)]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_AlignmentAdjusted(string creatureAlignment, string adjustedAlignment)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments("other Good", creatureAlignment)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Alignments, Is.EqualTo([new Alignment(adjustedAlignment)]));
        }

        [Test]
        public void ApplyTo_PrototypeWithAlignment_ReturnsUpdatedPrototype()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "my alignment")
                .Build();

            var filters = new Filters { Alignments = ["my Evil"] };

            var updatedPrototype = applicator.ApplyTo(creature, filters);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Alignments, Is.EquivalentTo(
            [
                new Alignment("my Evil"),
            ]));
        }

        [Test]
        public void ApplyTo_PrototypeWithAlignment_ReturnsUpdatedPrototype_FilteringOutGoodAlignments()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "my Good", "my alignment", AlignmentConstants.NeutralGood)
                .Build();

            var filters = new Filters { Alignments = ["my Evil"] };

            var updatedPrototype = applicator.ApplyTo(creature, filters);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Alignments, Is.EqualTo(
            [
                new Alignment("my Evil"),
            ]));
        }

        [Test]
        public void ApplyTo_PrototypeWithAlignment_ReturnsUpdatedPrototype_PreserveAlignmentWeighting()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "my alignment", "my alignment", AlignmentConstants.NeutralEvil)
                .Build();

            var filters = new Filters { Alignments = ["my Evil"] };

            var updatedPrototype = applicator.ApplyTo(creature, filters);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Alignments, Is.EqualTo(
            [
                new Alignment("my Evil"),
                new Alignment("my Evil"),
            ]));
        }

        [Test]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_NoLevelAdjustment()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .WithLevelAdjustment(null)
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.LevelAdjustment, Is.Null);
        }

        [TestCase(CreatureConstants.Types.Aberration, CreatureConstants.Types.Aberration)]
        [TestCase(CreatureConstants.Types.Animal, CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.Dragon, CreatureConstants.Types.Dragon)]
        [TestCase(CreatureConstants.Types.Fey, CreatureConstants.Types.Fey)]
        [TestCase(CreatureConstants.Types.Giant, CreatureConstants.Types.Giant)]
        [TestCase(CreatureConstants.Types.Humanoid, CreatureConstants.Types.Humanoid)]
        [TestCase(CreatureConstants.Types.MagicalBeast, CreatureConstants.Types.MagicalBeast)]
        [TestCase(CreatureConstants.Types.MonstrousHumanoid, CreatureConstants.Types.MonstrousHumanoid)]
        [TestCase(CreatureConstants.Types.Plant, CreatureConstants.Types.Plant)]
        [TestCase(CreatureConstants.Types.Vermin, CreatureConstants.Types.MagicalBeast)]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_TypeAdjusted(string originalType, string adjustedType)
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(originalType, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Type.Name, Is.EqualTo(adjustedType));
            Assert.That(updatedPrototype.Type.SubTypes, Is.SupersetOf(["subtype 1", "subtype 2", CreatureConstants.Types.Subtypes.Extraplanar]));

            if (originalType != adjustedType)
                Assert.That(updatedPrototype.Type.SubTypes, Is.SupersetOf([CreatureConstants.Types.Subtypes.Augmented, originalType]));
        }

        [Test]
        public void ApplyTo_Prototype_ReturnsUpdatedPrototype_WithAdditionalTemplates()
        {
            var creature = new CreaturePrototypeBuilder()
                .WithTestValues()
                .WithName("my creature")
                .WithCreatureType(CreatureConstants.Types.Humanoid, "subtype 1", "subtype 2")
                .WithAlignments(AlignmentConstants.LawfulEvil, "other alignment")
                .Build();
            creature.Templates.Add("my other template");

            var updatedPrototype = applicator.ApplyTo(creature);
            Assert.That(updatedPrototype.Name, Is.EqualTo("my creature"));
            Assert.That(updatedPrototype.Templates, Is.EqualTo(["my other template", CreatureConstants.Templates.FiendishCreature]));
        }
    }
}
