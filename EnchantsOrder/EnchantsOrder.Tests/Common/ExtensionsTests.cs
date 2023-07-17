using NUnit.Framework;

namespace EnchantsOrder.Common.Tests
{
    /// <summary>
    /// Tests the <see cref="Extensions"/> class.
    /// </summary>
    [TestFixture]
    public class ExtensionsTests
    {
        /// <summary>
        /// Tests the <see cref="Extensions.GetRomanNumber(int, int)"/> method.
        /// </summary>
        [Test]
        public void GetRomanNumberTest()
        {
            string[] strings = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            for (int i = 1; i <= 10; i++)
            {
                Assert.AreEqual(strings[i - 1], i.GetRomanNumber());
            }
            Assert.AreEqual("MCCXXXIV", 1234.GetRomanNumber());
        }

        /// <summary>
        /// Tests the <see cref="Extensions.PenaltyToExperience(long)"/> method.
        /// </summary>
        [Test]
        public void PenaltyToExperience()
        {
            long[] expect = new long[] { 0, 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023 };
            for (int i = 0; i <= 10; i++)
            {
                Assert.AreEqual(expect[i], Extensions.PenaltyToExperience(i));
            }
        }

        /// <summary>
        /// Tests the <see cref="Extensions.LevelToExperience(long)"/> method.
        /// </summary>
        [TestCase(160, 10)]
        [TestCase(550, 20)]
        [TestCase(2920, 40)]
        [TestCase(18020, 80)]
        [TestCase(657220, 400)]
        [TestCase(161027220, 6000)]
        [TestCase(448377220, 10000)]
        public void LevelToExperience(long xp, long level) => Assert.AreEqual(xp, Extensions.LevelToExperience(level));
    }
}
