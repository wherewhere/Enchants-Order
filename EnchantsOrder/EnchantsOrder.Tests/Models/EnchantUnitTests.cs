using NUnit.Framework;

namespace EnchantsOrder.Models.Tests
{
    /// <summary>
    /// Tests the <see cref="EnchantUnit"/> class.
    /// </summary>
    [TestFixture]
    public class EnchantUnitTests
    {
        /// <summary>
        /// Tests the <see langword="+"/> operator of <see cref="EnchantUnit"/>.
        /// </summary>
        [Test]
        public void AddTest()
        {
            EnchantUnit item1 = new(9, 3);
            EnchantUnit item2 = new(5, 5);
            EnchantUnit result = item1 + item2;

            Assert.That(result.Level, Is.EqualTo(14));
            Assert.That(result.Penalty, Is.EqualTo(6));
            Assert.That(result.StepLevel, Is.EqualTo(43));
            Assert.That(result.HistoryLevel, Is.EqualTo(43));
            Assert.That(result.HistoryExperience, Is.EqualTo(3553));
            Assert.That(result.ToString(), Is.EqualTo("Level:14 Penalty:6"));
        }
    }
}
