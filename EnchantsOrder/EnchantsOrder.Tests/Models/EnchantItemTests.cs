using NUnit.Framework;

namespace EnchantsOrder.Models.Tests
{
    /// <summary>
    /// Tests the <see cref="EnchantItem"/> class.
    /// </summary>
    [TestFixture]
    public class EnchantItemTests
    {
        /// <summary>
        /// Tests the <see langword="+"/> operator of <see cref="EnchantItem"/>.
        /// </summary>
        [Test]
        public void AddTest()
        {
            EnchantItem item1 = new(9, 3);
            EnchantItem item2 = new(5, 5);
            EnchantItem result = item1 + item2;

            Assert.That(result.Level, Is.EqualTo(14));
            Assert.That(result.Penalty, Is.EqualTo(6));
            Assert.That(result.StepLevel, Is.EqualTo(43));
            Assert.That(result.HistoryLevel, Is.EqualTo(43));
            Assert.That(result.HistoryExperience, Is.EqualTo(3553));
        }
    }
}
