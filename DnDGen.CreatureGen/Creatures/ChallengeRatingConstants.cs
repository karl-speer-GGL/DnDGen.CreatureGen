using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.CreatureGen.Creatures
{
    public static class ChallengeRatingConstants
    {
        public const string CR0 = "0";
        public const string CR1_10th = "1/10";
        public const string CR1_8th = "1/8";
        public const string CR1_6th = "1/6";
        public const string CR1_4th = "1/4";
        public const string CR1_3rd = "1/3";
        public const string CR1_2nd = "1/2";
        public const string CR1 = "1";
        public const string CR2 = "2";
        public const string CR3 = "3";
        public const string CR4 = "4";
        public const string CR5 = "5";
        public const string CR6 = "6";
        public const string CR7 = "7";
        public const string CR8 = "8";
        public const string CR9 = "9";
        public const string CR10 = "10";
        public const string CR11 = "11";
        public const string CR12 = "12";
        public const string CR13 = "13";
        public const string CR14 = "14";
        public const string CR15 = "15";
        public const string CR16 = "16";
        public const string CR17 = "17";
        public const string CR18 = "18";
        public const string CR19 = "19";
        public const string CR20 = "20";
        public const string CR21 = "21";
        public const string CR22 = "22";
        public const string CR23 = "23";
        public const string CR24 = "24";
        public const string CR25 = "25";
        public const string CR26 = "26";
        public const string CR27 = "27";
        public const string CR28 = "28";
        public const string CR29 = "29";
        public const string CR30 = "30";

        public static string[] GetOrdered()
        {
            return
            [
                CR0,
                CR1_10th,
                CR1_8th,
                CR1_6th,
                CR1_4th,
                CR1_3rd,
                CR1_2nd,
                CR1,
                CR2,
                CR3,
                CR4,
                CR5,
                CR6,
                CR7,
                CR8,
                CR9,
                CR10,
                CR11,
                CR12,
                CR13,
                CR14,
                CR15,
                CR16,
                CR17,
                CR18,
                CR19,
                CR20,
                CR21,
                CR22,
                CR23,
                CR24,
                CR25,
                CR26,
                CR27,
                CR28,
                CR29,
                CR30,
            ];
        }

        public static IEnumerable<string> Fractional => GetOrdered().Skip(1).Take(6);

        public static string IncreaseChallengeRating(string challengeRating, int increaseAmount)
        {
            if (Fractional.Contains(challengeRating))
            {
                var ordered = GetOrdered();
                var index = Array.IndexOf(ordered, challengeRating);

                if (index + increaseAmount < ordered.Length)
                    return ordered[index + increaseAmount];

                challengeRating = ordered.Last();
                increaseAmount += index - ordered.Length + 1;
            }

            var cr = Convert.ToInt32(challengeRating);
            cr += increaseAmount;

            return cr.ToString();
        }

        public static bool IsGreaterThan(string source, string target)
        {
            var ordered = GetOrdered();
            var sourceIndex = Array.IndexOf(ordered, source);
            var targetIndex = Array.IndexOf(ordered, target);

            if (sourceIndex == -1 && targetIndex == -1)
            {
                return Convert.ToInt32(source) > Convert.ToInt32(target);
            }
            else if (sourceIndex == -1)
            {
                return true;
            }
            else if (targetIndex == -1)
            {
                return false;
            }

            return sourceIndex > targetIndex;
        }
    }
}
