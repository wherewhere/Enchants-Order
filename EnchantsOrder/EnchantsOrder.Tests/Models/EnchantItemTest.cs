using NUnit.Framework;

namespace EnchantsOrder.Models.Tests
{
    /// <summary>
    /// Tests the <see cref="EnchantItem"/> class.
    /// </summary>
    internal class EnchantItemTest
    {
        [Test]
        public void AddTest()
        {
            EnchantItem item1 = new EnchantItem(9, 3, 18);
            EnchantItem item2 = new EnchantItem(5, 5, 7);
            EnchantItem result = item1 + item2;

            Assert.AreEqual(14, result.Level);
            Assert.AreEqual(6, result.Penalty);
            Assert.AreEqual(43, result.StepLevel);
            Assert.AreEqual(68, result.HistoryLevel);
            Assert.AreEqual(3553, result.HistoryExperience);
        }
    }
}
