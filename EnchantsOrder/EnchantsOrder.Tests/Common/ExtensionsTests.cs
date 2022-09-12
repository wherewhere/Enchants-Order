using NUnit.Framework;

namespace EnchantsOrder.Common.Tests
{
    /// <summary>
    /// Tests the <see cref="Extensions"/> class.
    /// </summary>
    [TestFixture]
    internal class ExtensionsTests
    {
        [Test]
        public void GetLoumaNumberTest()
        {
            string[] strings = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            for (int i = 1; i <= 10; i++)
            {
                Assert.AreEqual(strings[i - 1], i.GetLoumaNumber());
            }
            Assert.AreEqual("MCCXXXIV", 1234.GetLoumaNumber());
        }

        [Test]
        public void PenaltyToExperience()
        {
            long[] expect = new long[] { 0, 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023 };
            for (int i = 0; i <= 10; i++)
            {
                Assert.AreEqual(expect[i], Extensions.PenaltyToExperience(i));
            }
        }

        [Test]
        public void LevelToExperience()
        {
            Assert.AreEqual(160, Extensions.LevelToExperience(10));
            Assert.AreEqual(550, Extensions.LevelToExperience(20));
            Assert.AreEqual(2920, Extensions.LevelToExperience(40));
            Assert.AreEqual(18020, Extensions.LevelToExperience(80));
            Assert.AreEqual(657220, Extensions.LevelToExperience(400));
            Assert.AreEqual(161027220, Extensions.LevelToExperience(6000));
            Assert.AreEqual(448377220, Extensions.LevelToExperience(10000));
        }
    }
}
