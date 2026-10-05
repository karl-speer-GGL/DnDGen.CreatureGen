using DnDGen.CreatureGen.Creatures;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;

namespace DnDGen.CreatureGen.Tests.Unit.Creatures
{
    [TestFixture]
    public class ChallengeRatingConstantsTests
    {
        [TestCase(ChallengeRatingConstants.CR0, "0")]
        [TestCase(ChallengeRatingConstants.CR1_10th, "1/10")]
        [TestCase(ChallengeRatingConstants.CR1_8th, "1/8")]
        [TestCase(ChallengeRatingConstants.CR1_6th, "1/6")]
        [TestCase(ChallengeRatingConstants.CR1_4th, "1/4")]
        [TestCase(ChallengeRatingConstants.CR1_3rd, "1/3")]
        [TestCase(ChallengeRatingConstants.CR1_2nd, "1/2")]
        [TestCase(ChallengeRatingConstants.CR1, "1")]
        [TestCase(ChallengeRatingConstants.CR2, "2")]
        [TestCase(ChallengeRatingConstants.CR3, "3")]
        [TestCase(ChallengeRatingConstants.CR4, "4")]
        [TestCase(ChallengeRatingConstants.CR5, "5")]
        [TestCase(ChallengeRatingConstants.CR6, "6")]
        [TestCase(ChallengeRatingConstants.CR7, "7")]
        [TestCase(ChallengeRatingConstants.CR8, "8")]
        [TestCase(ChallengeRatingConstants.CR9, "9")]
        [TestCase(ChallengeRatingConstants.CR10, "10")]
        [TestCase(ChallengeRatingConstants.CR11, "11")]
        [TestCase(ChallengeRatingConstants.CR12, "12")]
        [TestCase(ChallengeRatingConstants.CR13, "13")]
        [TestCase(ChallengeRatingConstants.CR14, "14")]
        [TestCase(ChallengeRatingConstants.CR15, "15")]
        [TestCase(ChallengeRatingConstants.CR16, "16")]
        [TestCase(ChallengeRatingConstants.CR17, "17")]
        [TestCase(ChallengeRatingConstants.CR18, "18")]
        [TestCase(ChallengeRatingConstants.CR19, "19")]
        [TestCase(ChallengeRatingConstants.CR20, "20")]
        [TestCase(ChallengeRatingConstants.CR21, "21")]
        [TestCase(ChallengeRatingConstants.CR22, "22")]
        [TestCase(ChallengeRatingConstants.CR23, "23")]
        [TestCase(ChallengeRatingConstants.CR24, "24")]
        [TestCase(ChallengeRatingConstants.CR25, "25")]
        [TestCase(ChallengeRatingConstants.CR26, "26")]
        [TestCase(ChallengeRatingConstants.CR27, "27")]
        [TestCase(ChallengeRatingConstants.CR28, "28")]
        [TestCase(ChallengeRatingConstants.CR29, "29")]
        [TestCase(ChallengeRatingConstants.CR30, "30")]
        public void ChallengeRatingConstant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }

        [Test]
        public void OrderedChallengeRatings()
        {
            var orderedChallengeRatings = ChallengeRatingConstants.GetOrdered();

            Assert.That(orderedChallengeRatings[0], Is.EqualTo(ChallengeRatingConstants.CR0));
            Assert.That(orderedChallengeRatings[1], Is.EqualTo(ChallengeRatingConstants.CR1_10th));
            Assert.That(orderedChallengeRatings[2], Is.EqualTo(ChallengeRatingConstants.CR1_8th));
            Assert.That(orderedChallengeRatings[3], Is.EqualTo(ChallengeRatingConstants.CR1_6th));
            Assert.That(orderedChallengeRatings[4], Is.EqualTo(ChallengeRatingConstants.CR1_4th));
            Assert.That(orderedChallengeRatings[5], Is.EqualTo(ChallengeRatingConstants.CR1_3rd));
            Assert.That(orderedChallengeRatings[6], Is.EqualTo(ChallengeRatingConstants.CR1_2nd));
            Assert.That(orderedChallengeRatings[7], Is.EqualTo(ChallengeRatingConstants.CR1));
            Assert.That(orderedChallengeRatings[8], Is.EqualTo(ChallengeRatingConstants.CR2));
            Assert.That(orderedChallengeRatings[9], Is.EqualTo(ChallengeRatingConstants.CR3));
            Assert.That(orderedChallengeRatings[10], Is.EqualTo(ChallengeRatingConstants.CR4));
            Assert.That(orderedChallengeRatings[11], Is.EqualTo(ChallengeRatingConstants.CR5));
            Assert.That(orderedChallengeRatings[12], Is.EqualTo(ChallengeRatingConstants.CR6));
            Assert.That(orderedChallengeRatings[13], Is.EqualTo(ChallengeRatingConstants.CR7));
            Assert.That(orderedChallengeRatings[14], Is.EqualTo(ChallengeRatingConstants.CR8));
            Assert.That(orderedChallengeRatings[15], Is.EqualTo(ChallengeRatingConstants.CR9));
            Assert.That(orderedChallengeRatings[16], Is.EqualTo(ChallengeRatingConstants.CR10));
            Assert.That(orderedChallengeRatings[17], Is.EqualTo(ChallengeRatingConstants.CR11));
            Assert.That(orderedChallengeRatings[18], Is.EqualTo(ChallengeRatingConstants.CR12));
            Assert.That(orderedChallengeRatings[19], Is.EqualTo(ChallengeRatingConstants.CR13));
            Assert.That(orderedChallengeRatings[20], Is.EqualTo(ChallengeRatingConstants.CR14));
            Assert.That(orderedChallengeRatings[21], Is.EqualTo(ChallengeRatingConstants.CR15));
            Assert.That(orderedChallengeRatings[22], Is.EqualTo(ChallengeRatingConstants.CR16));
            Assert.That(orderedChallengeRatings[23], Is.EqualTo(ChallengeRatingConstants.CR17));
            Assert.That(orderedChallengeRatings[24], Is.EqualTo(ChallengeRatingConstants.CR18));
            Assert.That(orderedChallengeRatings[25], Is.EqualTo(ChallengeRatingConstants.CR19));
            Assert.That(orderedChallengeRatings[26], Is.EqualTo(ChallengeRatingConstants.CR20));
            Assert.That(orderedChallengeRatings[27], Is.EqualTo(ChallengeRatingConstants.CR21));
            Assert.That(orderedChallengeRatings[28], Is.EqualTo(ChallengeRatingConstants.CR22));
            Assert.That(orderedChallengeRatings[29], Is.EqualTo(ChallengeRatingConstants.CR23));
            Assert.That(orderedChallengeRatings[30], Is.EqualTo(ChallengeRatingConstants.CR24));
            Assert.That(orderedChallengeRatings[31], Is.EqualTo(ChallengeRatingConstants.CR25));
            Assert.That(orderedChallengeRatings[32], Is.EqualTo(ChallengeRatingConstants.CR26));
            Assert.That(orderedChallengeRatings[33], Is.EqualTo(ChallengeRatingConstants.CR27));
            Assert.That(orderedChallengeRatings[34], Is.EqualTo(ChallengeRatingConstants.CR28));
            Assert.That(orderedChallengeRatings[35], Is.EqualTo(ChallengeRatingConstants.CR29));
            Assert.That(orderedChallengeRatings[36], Is.EqualTo(ChallengeRatingConstants.CR30));
            Assert.That(orderedChallengeRatings, Has.Length.EqualTo(37));
        }

        [Test]
        public void FractionalChallengeRatings()
        {
            Assert.That(ChallengeRatingConstants.Fractional, Contains.Item(ChallengeRatingConstants.CR1_10th)
                .And.Contains(ChallengeRatingConstants.CR1_8th)
                .And.Contains(ChallengeRatingConstants.CR1_6th)
                .And.Contains(ChallengeRatingConstants.CR1_4th)
                .And.Contains(ChallengeRatingConstants.CR1_3rd)
                .And.Contains(ChallengeRatingConstants.CR1_2nd));
            Assert.That(ChallengeRatingConstants.Fractional.Count(), Is.EqualTo(6));
        }

        [TestCaseSource(nameof(FractionalIncreases))]
        public void Increase_Fractional(string challengeRating, int increase, string expected)
        {
            var newCr = ChallengeRatingConstants.IncreaseChallengeRating(challengeRating, increase);
            Assert.That(newCr, Is.EqualTo(expected));
        }

        private static IEnumerable FractionalIncreases
        {
            get
            {
                var challengeRatings = ChallengeRatingConstants.Fractional;
                var increases = Enumerable.Range(1, 10);
                var ordered = ChallengeRatingConstants.GetOrdered();

                foreach (var increase in increases)
                {
                    foreach (var cr in challengeRatings)
                    {
                        var index = Array.IndexOf(ordered, cr);

                        yield return new TestCaseData(cr, increase, ordered[index + increase]);
                    }
                }
            }
        }

        [TestCaseSource(nameof(WholeIncreases))]
        public void Increase_WholeNumber(string challengeRating, int increase, string expected)
        {
            var newCr = ChallengeRatingConstants.IncreaseChallengeRating(challengeRating, increase);
            Assert.That(newCr, Is.EqualTo(expected));
        }

        private static IEnumerable WholeIncreases
        {
            get
            {
                var increases = Enumerable.Range(1, 10);
                var ordered = ChallengeRatingConstants.GetOrdered().Except(ChallengeRatingConstants.Fractional).ToArray();

                foreach (var increase in increases)
                {
                    foreach (var cr in ordered)
                    {
                        var expected = Convert.ToInt32(cr) + increase;
                        yield return new TestCaseData(cr, increase, expected.ToString());
                    }
                }
            }
        }

        [Test]
        public void IncreaseOutOfRange_Whole()
        {
            var newCr = ChallengeRatingConstants.IncreaseChallengeRating("9266", 90210);
            Assert.That(newCr, Is.EqualTo(99476.ToString()));
        }

        [TestCase(ChallengeRatingConstants.CR1_10th, 6, 1)]
        [TestCase(ChallengeRatingConstants.CR1_10th, 10, 5)]
        [TestCase(ChallengeRatingConstants.CR1_10th, 20, 15)]
        [TestCase(ChallengeRatingConstants.CR1_10th, 90210, 90205)]
        [TestCase(ChallengeRatingConstants.CR1_8th, 6, 2)]
        [TestCase(ChallengeRatingConstants.CR1_8th, 10, 6)]
        [TestCase(ChallengeRatingConstants.CR1_8th, 20, 16)]
        [TestCase(ChallengeRatingConstants.CR1_8th, 90210, 90206)]
        [TestCase(ChallengeRatingConstants.CR1_6th, 6, 3)]
        [TestCase(ChallengeRatingConstants.CR1_6th, 10, 7)]
        [TestCase(ChallengeRatingConstants.CR1_6th, 20, 17)]
        [TestCase(ChallengeRatingConstants.CR1_6th, 90210, 90207)]
        [TestCase(ChallengeRatingConstants.CR1_4th, 6, 4)]
        [TestCase(ChallengeRatingConstants.CR1_4th, 10, 8)]
        [TestCase(ChallengeRatingConstants.CR1_4th, 20, 18)]
        [TestCase(ChallengeRatingConstants.CR1_4th, 90210, 90208)]
        [TestCase(ChallengeRatingConstants.CR1_3rd, 6, 5)]
        [TestCase(ChallengeRatingConstants.CR1_3rd, 10, 9)]
        [TestCase(ChallengeRatingConstants.CR1_3rd, 20, 19)]
        [TestCase(ChallengeRatingConstants.CR1_3rd, 90210, 90209)]
        [TestCase(ChallengeRatingConstants.CR1_2nd, 6, 6)]
        [TestCase(ChallengeRatingConstants.CR1_2nd, 10, 10)]
        [TestCase(ChallengeRatingConstants.CR1_2nd, 20, 20)]
        [TestCase(ChallengeRatingConstants.CR1_2nd, 90210, 90210)]
        public void IncreaseOutOfRange_Fractional(string cr, int increase, int expected)
        {
            var newCr = ChallengeRatingConstants.IncreaseChallengeRating(cr, increase);
            Assert.That(newCr, Is.EqualTo(expected.ToString()));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(90210)]
        public void IncreaseOutOfRange_Zero(int increase)
        {
            var newCr = ChallengeRatingConstants.IncreaseChallengeRating(ChallengeRatingConstants.CR0, increase);
            Assert.That(newCr, Is.EqualTo(increase.ToString()));
        }

        [TestCaseSource(nameof(IsGreaterThanComparisons))]
        public void IsGreaterThan_ComparesChallengeRatings(string source, string target, bool greater)
        {
            var isGreater = ChallengeRatingConstants.IsGreaterThan(source, target);
            Assert.That(isGreater, Is.EqualTo(greater));
        }

        private static IEnumerable IsGreaterThanComparisons
        {
            get
            {
                var ordered = ChallengeRatingConstants.GetOrdered();

                for (var i = 0; i < ordered.Length; i++)
                {
                    for (var j = 0; j < ordered.Length; j++)
                    {
                        yield return new TestCaseData(ordered[i], ordered[j], i > j);
                    }
                }
            }
        }

        [TestCase(9266, 9266, false)]
        [TestCase(9266, 90210, false)]
        [TestCase(9266, 42, true)]
        [TestCase(90210, 9266, true)]
        [TestCase(90210, 90210, false)]
        [TestCase(90210, 42, true)]
        [TestCase(42, 9266, false)]
        [TestCase(42, 90210, false)]
        [TestCase(42, 42, false)]
        public void IsGreaterThan_ComparesChallengeRatings_OutsideConstants(int source, int target, bool greater)
        {
            var isGreater = ChallengeRatingConstants.IsGreaterThan(source.ToString(), target.ToString());
            Assert.That(isGreater, Is.EqualTo(greater));
        }

        [TestCase(ChallengeRatingConstants.CR0, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_10th, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_10th, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_8th, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_6th, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_4th, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_3rd, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1_2nd, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR1, "9266", false)]
        [TestCase(ChallengeRatingConstants.CR27, "9266", false)]
        [TestCase("9266", ChallengeRatingConstants.CR0, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_10th, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_10th, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_8th, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_6th, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_4th, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_3rd, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1_2nd, true)]
        [TestCase("9266", ChallengeRatingConstants.CR1, true)]
        [TestCase("9266", ChallengeRatingConstants.CR27, true)]
        public void IsGreaterThan_ComparesChallengeRatings_OneOutsideConstants(string source, string target, bool greater)
        {
            var isGreater = ChallengeRatingConstants.IsGreaterThan(source, target);
            Assert.That(isGreater, Is.EqualTo(greater));
        }
    }
}
