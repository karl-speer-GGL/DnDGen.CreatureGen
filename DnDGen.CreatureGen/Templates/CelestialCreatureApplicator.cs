using DnDGen.CreatureGen.Abilities;
using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Defenses;
using DnDGen.CreatureGen.Generators.Attacks;
using DnDGen.CreatureGen.Generators.Creatures;
using DnDGen.CreatureGen.Generators.Feats;
using DnDGen.CreatureGen.Generators.Magics;
using DnDGen.CreatureGen.Languages;
using DnDGen.CreatureGen.Tables;
using DnDGen.CreatureGen.Verifiers.Exceptions;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.TreasureGen.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DnDGen.CreatureGen.Templates
{
    internal class CelestialCreatureApplicator(
        IAttacksGenerator attackGenerator,
        IFeatsGenerator featGenerator,
        ICollectionSelector collectionSelector,
        IMagicGenerator magicGenerator,
        IDemographicsGenerator demographicsGenerator) : TemplateApplicator
    {
        private readonly IEnumerable<string> creatureTypes =
            [
                CreatureConstants.Types.Aberration,
                CreatureConstants.Types.Animal,
                CreatureConstants.Types.Dragon,
                CreatureConstants.Types.Fey,
                CreatureConstants.Types.Giant,
                CreatureConstants.Types.Humanoid,
                CreatureConstants.Types.MagicalBeast,
                CreatureConstants.Types.MonstrousHumanoid,
                CreatureConstants.Types.Plant,
                CreatureConstants.Types.Vermin,
            ];

        public Ability MinimumAbility => null;

        public Creature ApplyTo(Creature creature, bool asCharacter, Filters filters = null)
        {
            var (Compatible, Reason) = IsCompatible(
                creature.Type.AllTypes,
                [creature.Alignment.Full],
                creature.ChallengeRating,
                creature.HitPoints.RoundedHitDiceQuantity,
                filters);
            if (!Compatible)
            {
                throw new InvalidCreatureException(
                    Reason,
                    asCharacter,
                    creature.Name,
                    filters,
                    templates: [.. creature.Templates.Concat([CreatureConstants.Templates.CelestialCreature])]);
            }

            // Template
            UpdateCreatureTemplate(creature);

            // Creature type
            UpdateCreatureType(creature);

            // Demographics
            UpdateCreatureDemographics(creature);

            // Challenge ratings
            UpdateCreatureChallengeRating(creature);

            // Abilities
            UpdateCreatureAbilities(creature);

            // Level Adjustment
            UpdateCreatureLevelAdjustment(creature);

            // Alignment
            UpdateCreatureAlignment(creature);

            // Languages
            UpdateCreatureLanguages(creature);

            //INFO: Depends on abilities
            // Attacks
            UpdateCreatureAttacks(creature);

            //INFO: Depends on abilities, alignment
            // Special Qualities
            UpdateCreatureSpecialQualities(creature);

            //INFO: Depends on abilities, alignment
            // Magic
            UpdateCreatureMagic(creature);

            return creature;
        }

        private static void UpdateCreatureTemplate(Creature creature) => creature.Templates.Add(CreatureConstants.Templates.CelestialCreature);
        private static void UpdateCreatureTemplate(CreaturePrototype creature) => creature.Templates.Add(CreatureConstants.Templates.CelestialCreature);

        private static void UpdateCreatureType(Creature creature)
        {
            var adjustedTypes = UpdateCreatureType(creature.Type.Name, creature.Type.SubTypes);
            creature.Type = new CreatureType(adjustedTypes);
        }

        private static void UpdateCreatureType(CreaturePrototype creature)
        {
            var adjustedTypes = UpdateCreatureType(creature.Type.Name, creature.Type.SubTypes);
            creature.Type = new CreatureType(adjustedTypes);
        }

        private static IEnumerable<string> UpdateCreatureType(string creatureType, IEnumerable<string> subtypes)
        {
            var adjustedSubtypes = subtypes.Union(
            [
                CreatureConstants.Types.Subtypes.Extraplanar,
            ]);

            if (creatureType == CreatureConstants.Types.Animal
                || creatureType == CreatureConstants.Types.Vermin)
            {
                return new[] { CreatureConstants.Types.MagicalBeast }.Union(adjustedSubtypes).Union([CreatureConstants.Types.Subtypes.Augmented, creatureType]);
            }

            return new[] { creatureType }.Union(adjustedSubtypes);
        }

        private void UpdateCreatureDemographics(Creature creature)
        {
            creature.Demographics = demographicsGenerator.UpdateByTemplate(creature.Demographics, creature.Name, CreatureConstants.Templates.CelestialCreature);
        }

        private static void UpdateCreatureAbilities(Creature creature)
        {
            if (creature.Abilities[AbilityConstants.Intelligence].FullScore < 3)
                creature.Abilities[AbilityConstants.Intelligence].TemplateScore = 3;
        }

        private static void UpdateCreatureAbilities(CreaturePrototype creature)
        {
            if (creature.Abilities[AbilityConstants.Intelligence].FullScore < 3)
                creature.Abilities[AbilityConstants.Intelligence].TemplateScore = 3;
        }

        private void UpdateCreatureAlignment(Creature creature)
        {
            creature.Alignment = UpdateCreatureAlignment(creature.Alignment);
        }

        private void UpdateCreatureAlignment(CreaturePrototype creature, Filters filters)
        {
            var updatedAlignments = creature.Alignments
                .Where(a => a.Goodness != AlignmentConstants.Evil)
                .Select(UpdateCreatureAlignment);

            if (filters?.Alignments?.Count > 0)
            {
                //INFO: Using Where instead of Intersect to maintain alignment weighting
                updatedAlignments = updatedAlignments.Where(a => filters.Alignments.Contains(a.Full));
            }

            creature.Alignments = [.. updatedAlignments];
        }

        private Alignment UpdateCreatureAlignment(Alignment alignment) => UpdateCreatureAlignment(alignment.Full);
        private Alignment UpdateCreatureAlignment(string alignment) => new(alignment) { Goodness = AlignmentConstants.Good };

        private static void UpdateCreatureChallengeRating(Creature creature)
        {
            creature.ChallengeRating = UpdateCreatureChallengeRating(creature.ChallengeRating, creature.HitPoints.RoundedHitDiceQuantity);
        }

        private static void UpdateCreatureChallengeRating(CreaturePrototype creature)
        {
            var roundedHitDiceQuantity = creature.GetRoundedHitDiceQuantity();
            creature.ChallengeRating = UpdateCreatureChallengeRating(creature.ChallengeRating, roundedHitDiceQuantity);
        }

        private static string UpdateCreatureChallengeRating(string challengeRating, double hitDiceQuantity)
        {
            return UpdateCreatureChallengeRating(challengeRating, HitDice.GetRoundedQuantity(hitDiceQuantity));
        }

        private static string UpdateCreatureChallengeRating(string challengeRating, int hitDiceQuantity)
        {
            if (hitDiceQuantity >= 8)
            {
                return ChallengeRatingConstants.IncreaseChallengeRating(challengeRating, 2);
            }
            else if (hitDiceQuantity >= 4)
            {
                return ChallengeRatingConstants.IncreaseChallengeRating(challengeRating, 1);
            }

            return challengeRating;
        }

        private static void UpdateCreatureLevelAdjustment(Creature creature)
        {
            if (creature.LevelAdjustment.HasValue)
                creature.LevelAdjustment += 2;
        }

        private static void UpdateCreatureLevelAdjustment(CreaturePrototype creature)
        {
            if (creature.LevelAdjustment.HasValue)
                creature.LevelAdjustment += 2;
        }

        private void UpdateCreatureMagic(Creature creature)
        {
            creature.Magic = magicGenerator.GenerateWith(creature.Name, creature.Alignment, creature.Abilities, creature.Equipment);
        }

        private void UpdateCreatureAttacks(Creature creature)
        {
            var attacks = attackGenerator.GenerateAttacks(
                CreatureConstants.Templates.CelestialCreature,
                creature.Size,
                creature.BaseAttackBonus,
                creature.Abilities,
                creature.HitPoints.RoundedHitDiceQuantity,
                creature.Demographics.Gender);

            var smiteEvil = attacks.First(a => a.Name == "Smite Evil");
            smiteEvil.Damages.Add(new Damage
            {
                Roll = Math.Min(creature.HitPoints.RoundedHitDiceQuantity, 20).ToString()
            });

            creature.Attacks = creature.Attacks.Union(attacks);
        }

        private void UpdateCreatureSpecialQualities(Creature creature)
        {
            var specialQualities = featGenerator.GenerateSpecialQualities(
                CreatureConstants.Templates.CelestialCreature,
                creature.Type,
                creature.HitPoints,
                creature.Abilities,
                creature.Skills,
                creature.CanUseEquipment,
                creature.Size,
                creature.Alignment);

            foreach (var sq in specialQualities)
            {
                var matching = creature.SpecialQualities.FirstOrDefault(f =>
                    f.Name == sq.Name
                    && !f.Foci.Except(sq.Foci).Any()
                    && !sq.Foci.Except(f.Foci).Any());

                if (matching == null)
                {
                    creature.SpecialQualities = creature.SpecialQualities.Union([sq]);
                }
                else if (matching.Power < sq.Power)
                {
                    matching.Power = sq.Power;
                }
            }
        }

        private void UpdateCreatureLanguages(Creature creature)
        {
            if (!creature.Languages.Any())
            {
                return;
            }

            var language = collectionSelector.SelectRandomFrom(
                Config.Name,
                TableNameConstants.Collection.LanguageGroups,
                CreatureConstants.Templates.CelestialCreature + LanguageConstants.Groups.Automatic);

            creature.Languages = creature.Languages.Union([language]);
        }

        public async Task<Creature> ApplyToAsync(Creature creature, bool asCharacter, Filters filters = null)
        {
            var (Compatible, Reason) = IsCompatible(
                creature.Type.AllTypes,
                [creature.Alignment.Full],
                creature.ChallengeRating,
                creature.HitPoints.RoundedHitDiceQuantity,
                filters);
            if (!Compatible)
            {
                throw new InvalidCreatureException(
                    Reason,
                    asCharacter,
                    creature.Name,
                    filters,
                    templates: [.. creature.Templates.Concat([CreatureConstants.Templates.CelestialCreature])]);
            }

            var tasks = new List<Task>();

            // Template
            var templateTask = Task.Run(() => UpdateCreatureTemplate(creature));
            tasks.Add(templateTask);

            // Creature type
            var typeTask = Task.Run(() => UpdateCreatureType(creature));
            tasks.Add(typeTask);

            // Demographics
            var demographicsTask = Task.Run(() => UpdateCreatureDemographics(creature));
            tasks.Add(demographicsTask);

            // Challenge ratings
            var challengeRatingTask = Task.Run(() => UpdateCreatureChallengeRating(creature));
            tasks.Add(challengeRatingTask);

            // Abilities
            var abilitiesTask = Task.Run(() => UpdateCreatureAbilities(creature));
            tasks.Add(abilitiesTask);

            // Level Adjustment
            var levelAdjustmentTask = Task.Run(() => UpdateCreatureLevelAdjustment(creature));
            tasks.Add(levelAdjustmentTask);

            // Alignment
            var alignmentTask = Task.Run(() => UpdateCreatureAlignment(creature));
            tasks.Add(alignmentTask);

            // Languages
            var languageTask = Task.Run(() => UpdateCreatureLanguages(creature));
            tasks.Add(languageTask);

            await Task.WhenAll(tasks);
            tasks.Clear();

            //INFO: Depends on abilities
            // Attacks
            var attackTask = Task.Run(() => UpdateCreatureAttacks(creature));
            tasks.Add(attackTask);

            //INFO: Depends on abilities, alignment
            // Special Qualities
            var qualityTask = Task.Run(() => UpdateCreatureSpecialQualities(creature));
            tasks.Add(qualityTask);

            //INFO: Depends on abilities, alignment
            // Magic
            var magicTask = Task.Run(() => UpdateCreatureMagic(creature));
            tasks.Add(magicTask);

            await Task.WhenAll(tasks);
            tasks.Clear();

            return creature;
        }

        private (bool Compatible, string Reason) IsCompatible(
            IEnumerable<string> types,
            IEnumerable<string> alignments,
            string creatureChallengeRating,
            double creatureHitDiceQuantity,
            Filters filters)
        {
            var (Compatible, Reason) = IsCompatible(types, alignments);
            if (!Compatible)
                return (false, Reason);

            return AreFiltersCompatible(types, alignments, creatureChallengeRating, creatureHitDiceQuantity, filters);
        }

        private (bool Compatible, string Reason) AreFiltersCompatible(
            IEnumerable<string> types,
            IEnumerable<string> alignments,
            string creatureChallengeRating,
            double creatureHitDiceQuantity,
            Filters filters)
        {
            if (filters is null)
                return (true, null);

            var updatedAlignments = alignments
                    .Where(a => !a.Contains(AlignmentConstants.Evil))
                    .Select(UpdateCreatureAlignment)
                    .Select(a => a.Full);
            var updatedTypes = UpdateCreatureType(types.First(), types.Skip(1));
            var cr = UpdateCreatureChallengeRating(creatureChallengeRating, creatureHitDiceQuantity);

            return filters.AreCompatible(updatedAlignments, [cr], updatedTypes);
        }

        private (bool Compatible, string Reason) IsCompatible(IEnumerable<string> types, IEnumerable<string> alignments)
        {
            if (types.Contains(CreatureConstants.Types.Subtypes.Incorporeal))
                return (false, "Creature is Incorporeal");

            if (!creatureTypes.Contains(types.First()))
                return (false, $"Type '{types.First()}' is not valid");

            if (!alignments.Any(a => !a.Contains(AlignmentConstants.Evil)))
                return (false, "Creature has no non-evil alignments");

            return (true, null);
        }

        public CreaturePrototype ApplyTo(CreaturePrototype creature, Filters filters = null)
        {
            var (Compatible, Reason) = IsCompatible(
                creature.Type.AllTypes,
                creature.Alignments.Select(a => a.Full),
                creature.ChallengeRating,
                creature.GetRoundedHitDiceQuantity(),
                filters);
            if (!Compatible)
            {
                throw new InvalidCreatureException(
                    Reason,
                    creature.AsCharacter,
                    creature.Name,
                    filters,
                    templates: [.. creature.Templates.Concat([CreatureConstants.Templates.CelestialCreature])]);
            }

            UpdateCreatureAbilities(creature);
            UpdateCreatureAlignment(creature, filters);
            UpdateCreatureChallengeRating(creature);
            UpdateCreatureLevelAdjustment(creature);
            UpdateCreatureType(creature);
            UpdateCreatureTemplate(creature);

            return creature;
        }

        public bool IsCompatible(CreaturePrototype creature, Filters filters = null)
        {
            var (Compatible, Reason) = IsCompatible(
                creature.Type.AllTypes,
                creature.Alignments.Select(a => a.Full),
                creature.ChallengeRating,
                creature.HitDiceQuantity,
                filters);

            return Compatible;
        }
    }
}
