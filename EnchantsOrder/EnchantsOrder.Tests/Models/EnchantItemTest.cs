using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var item1 = new EnchantItem(9, 3, 18);
            var item2 = new EnchantItem(5, 5, 7);
            var result = item1 + item2;

            Assert.AreEqual(14, result.Level);
            Assert.AreEqual(6, result.Penalty);
            Assert.AreEqual(43, result.StepLevel);
            Assert.AreEqual(68, result.HistoryLevel);
            Assert.AreEqual(3553, result.HistoryExperience);
        }
    }
}
