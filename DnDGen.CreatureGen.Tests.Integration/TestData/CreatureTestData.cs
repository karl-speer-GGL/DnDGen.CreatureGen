using DnDGen.CreatureGen.Alignments;
using DnDGen.CreatureGen.Creatures;
using DnDGen.CreatureGen.Generators.Creatures;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Integration.TestData
{
    public class CreatureTestData
    {
        public static IEnumerable Creatures => CreatureConstants.GetAll().Select(c => new TestCaseData(c));
        public static IEnumerable Characters => CreatureConstants.GetAllCharacters().Select(c => new TestCaseData(c));
        public static IEnumerable Templates => CreatureConstants.Templates.GetAll().Select(t => new TestCaseData(t));
        public static IEnumerable Types => CreatureConstants.Types.GetAll().Select(t => new TestCaseData(t));
        public static IEnumerable Subtypes => CreatureConstants.Types.Subtypes.GetAll().Select(t => new TestCaseData(t));

        public static IEnumerable<(bool AsCharacter, string Creature, string[] Templates)> ProblematicCreatures =>
            [
                (false, CreatureConstants.Chimera_Green, [CreatureConstants.Templates.HalfCelestial]),
                (true, CreatureConstants.Chimera_Red, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Chimera_White, [CreatureConstants.Templates.Skeleton]),
                (false, CreatureConstants.Choker, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Criosphinx, [CreatureConstants.Templates.Zombie]),
                (false, CreatureConstants.DisplacerBeast_PackLord, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Dragon_Brass_Young, [CreatureConstants.Templates.Ghost]),
                (false, CreatureConstants.Dragon_Brass_Young, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.Dragon_Bronze_GreatWyrm, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.Dragon_Copper_Adult, [CreatureConstants.Templates.Skeleton]),
                (false, CreatureConstants.Dragon_Silver_Ancient, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.Dragon_White_Old, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Elemental_Air_Small, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.Elemental_Fire_Medium, [CreatureConstants.Templates.HalfFiend]),
                (true, CreatureConstants.Gargoyle, [CreatureConstants.Templates.HalfCelestial]),
                (true, CreatureConstants.Gargoyle, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Gargoyle_Kapoacinth, [CreatureConstants.Templates.Ghost]),
                (true, CreatureConstants.Gargoyle_Kapoacinth, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.GibberingMouther, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.GibberingMouther, [CreatureConstants.Templates.HalfFiend]),
                (true, CreatureConstants.Gnoll, [CreatureConstants.Templates.Ghost]),
                (true, CreatureConstants.GrayRender, []),
                (false, CreatureConstants.Hieracosphinx, [CreatureConstants.Templates.Skeleton]),
                (false, CreatureConstants.Human, [CreatureConstants.Templates.Ghost]),
                (false, CreatureConstants.Kobold, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Lizardfolk, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.Lizardfolk, [CreatureConstants.Templates.HalfFiend]),
                (false, CreatureConstants.Mimic, [CreatureConstants.Templates.Ghost]),
                (false, CreatureConstants.Otyugh, [CreatureConstants.Templates.Ghost]),
                (false, CreatureConstants.Otyugh, [CreatureConstants.Templates.Zombie]),
                (false, CreatureConstants.RazorBoar, [CreatureConstants.Templates.Ghost]),
                (true, CreatureConstants.ShamblingMound, [CreatureConstants.Templates.HalfFiend]),
                (true, CreatureConstants.Skum, [CreatureConstants.Templates.Ghost]),
                (false, CreatureConstants.Troglodyte, [CreatureConstants.Templates.HalfCelestial]),
                (false, CreatureConstants.Troglodyte, [CreatureConstants.Templates.HalfFiend]),
                (true, CreatureConstants.Xill, []),
            ];

        public static IEnumerable ProblematicCreaturesTestCases => ProblematicCreatures.Select(pc => new TestCaseData(pc.AsCharacter, pc.Creature, pc.Templates));

        public static IEnumerable<(bool AsCharacter, string[] Templates, Filters Filters)> ProblematicFilters =>
        [
            (true, [], new Filters()),
            (true, [], new Filters { Alignments = [AlignmentConstants.LawfulEvil], Types = [CreatureConstants.Types.Plant] }),
            (true, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR0], Types = [CreatureConstants.Types.Subtypes.Reptilian], }),
            (true, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR5], Types = [CreatureConstants.Types.Subtypes.Earth], }),
            (true, [CreatureConstants.Templates.Ghost], new Filters { Alignments = [AlignmentConstants.LawfulEvil] }),
            (true, [CreatureConstants.Templates.Ghost], new Filters { Types = [CreatureConstants.Types.Subtypes.Gnoll] }),
            (true, [CreatureConstants.Templates.HalfCelestial], new Filters
            {
                ChallengeRatings = [ChallengeRatingConstants.CR5],
                Types = [CreatureConstants.Types.Subtypes.Earth],
            }),
            (true, [CreatureConstants.Templates.HalfFiend], new Filters
            {
                Alignments = [AlignmentConstants.LawfulEvil],
                Types = [CreatureConstants.Types.Plant],
            }),
            (true, [CreatureConstants.Templates.HalfFiend], new Filters
            {
                ChallengeRatings = [ChallengeRatingConstants.CR5],
                Types = [CreatureConstants.Types.Subtypes.Earth],
            }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR1_4th], Types = [CreatureConstants.Types.Subtypes.Reptilian], }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR1_3rd], Types = [CreatureConstants.Types.Subtypes.Reptilian], }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR1], Types = [CreatureConstants.Types.Subtypes.Reptilian], }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR2], Types = [CreatureConstants.Types.Subtypes.Reptilian], }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR3], Types = [CreatureConstants.Types.Aberration], }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR6], Types = [CreatureConstants.Types.Aberration], }),
            (false, [], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR15] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Dragon] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Giant] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Humanoid] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.MagicalBeast] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Outsider] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Undead] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Subtypes.Augmented] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Subtypes.Incorporeal] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Subtypes.Native] }),
            (false, [], new Filters { Types = [CreatureConstants.Types.Subtypes.Shapechanger] }),
            (false, [CreatureConstants.Templates.Ghost], new Filters { Alignments = [AlignmentConstants.ChaoticNeutral] }),
            (false, [CreatureConstants.Templates.Ghost], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR6], Types = [CreatureConstants.Types.Aberration], }),
            (false, [CreatureConstants.Templates.Ghost], new Filters { Types = [CreatureConstants.Types.Undead] }),
            (false, [CreatureConstants.Templates.HalfCelestial], new Filters
            {
                ChallengeRatings = [ChallengeRatingConstants.CR2],
                Types = [CreatureConstants.Types.Subtypes.Reptilian]
            }),
            (false, [CreatureConstants.Templates.HalfCelestial], new Filters { Types = [CreatureConstants.Types.Aberration] }),
            (false, [CreatureConstants.Templates.HalfCelestial], new Filters { Types = [CreatureConstants.Types.Subtypes.Native] }),
            (false, [CreatureConstants.Templates.HalfFiend], new Filters
            {
                ChallengeRatings = [ChallengeRatingConstants.CR1_3rd],
                Types = [CreatureConstants.Types.Subtypes.Reptilian]
            }),
            (false, [CreatureConstants.Templates.HalfFiend], new Filters
            {
                ChallengeRatings = [ChallengeRatingConstants.CR2],
                Types = [CreatureConstants.Types.Subtypes.Reptilian]
            }),
            (false, [CreatureConstants.Templates.HalfFiend], new Filters
            {
                ChallengeRatings = [ChallengeRatingConstants.CR3],
                Types = [CreatureConstants.Types.Aberration]
            }),
            (false, [CreatureConstants.Templates.HalfFiend], new Filters { ChallengeRatings = [ChallengeRatingConstants.CR15] }),
            (false, [CreatureConstants.Templates.HalfFiend], new Filters { Types = [CreatureConstants.Types.Aberration] }),
            (false, [CreatureConstants.Templates.HalfFiend], new Filters { Types = [CreatureConstants.Types.Subtypes.Native] }),
            (false, [CreatureConstants.Templates.Skeleton], null),
            (false, [CreatureConstants.Templates.Zombie], null),
        ];

        public static IEnumerable ProblematicFiltersTestCases
        {
            get
            {
                foreach (var pf in ProblematicFilters)
                {
                    var testCase = new TestCaseData(pf.AsCharacter, pf.Templates, pf.Filters);
                    var asCharacterArgName = pf.AsCharacter.ToString();
                    var templatesArgName = $"[{string.Join(",", pf.Templates)}]";
                    var filtersArgName = pf.Filters?.GetDescription() ?? string.Empty;
                    filtersArgName = filtersArgName.Replace(Environment.NewLine, ";");

                    testCase.SetArgDisplayNames([asCharacterArgName, templatesArgName, filtersArgName]);

                    yield return testCase;
                }
            }
        }
    }
}
