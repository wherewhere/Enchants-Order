using EnchantsOrder.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EnchantsOrder.Tests
{
    /// <summary>
    /// Tests the <see cref="EnchantsOrder"/> class.
    /// </summary>
    [TestFixture]
    public class EnchantsOrderTests
    {
        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void NoneBookOrderTest()
        {
            List<IEnchantment> enchantmentList = null;
            _ = Assert.Throws<ArgumentNullException>(() => _ = enchantmentList.Ordering());
            enchantmentList = new List<IEnchantment>();
            _ = Assert.Throws<ArgumentNullException>(() => _ = enchantmentList.Ordering());
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void OneBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Efficiency", 5, 1)
            };

            OrderingResults results = enchantmentList.Ordering();
            Assert.AreEqual(1, results.Penalty);
            Assert.AreEqual(5, results.MaxExperience);
            Assert.AreEqual(5, results.TotalExperience);
            Assert.AreEqual(1, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Efficiency", results.Steps[0][0].Name);
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void TwoBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1)
            };

            OrderingResults results = enchantmentList.Ordering();

            Assert.AreEqual(2, results.Penalty);
            Assert.AreEqual(3, results.MaxExperience);
            Assert.AreEqual(6, results.TotalExperience);

            Assert.AreEqual(2, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Unbreaking", results.Steps[0][0].Name);
            Assert.AreEqual(1, results.Steps[1].Count);
            Assert.AreEqual("Mending", results.Steps[1][0].Name);
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void ThreeBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Projectile Protection", 4, 1)
            };

            OrderingResults results = enchantmentList.Ordering();

            Assert.AreEqual(2, results.Penalty);
            Assert.AreEqual(7, results.MaxExperience);
            Assert.AreEqual(13, results.TotalExperience);

            Assert.AreEqual(2, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Projectile Protection", results.Steps[0][0].Name);
            Assert.AreEqual(2, results.Steps[1].Count);
            Assert.AreEqual("Unbreaking", results.Steps[1][0].Name);
            Assert.AreEqual("Mending", results.Steps[1][1].Name);
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void FourBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Riptide", 3, 2),
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Impaling", 5, 2)
            };

            OrderingResults results = enchantmentList.Ordering();

            Assert.AreEqual(3, results.Penalty);
            Assert.AreEqual(10, results.MaxExperience);
            Assert.AreEqual(28, results.TotalExperience);

            Assert.AreEqual(3, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Impaling", results.Steps[0][0].Name);
            Assert.AreEqual(2, results.Steps[1].Count);
            Assert.AreEqual("Unbreaking", results.Steps[1][0].Name);
            Assert.AreEqual("Mending", results.Steps[1][1].Name);
            Assert.AreEqual(1, results.Steps[2].Count);
            Assert.AreEqual("Riptide", results.Steps[2][0].Name);
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void FiveBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Punch", 2, 2),
                new Enchantment("Flame", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Infinity", 1, 4),
                new Enchantment("Power", 5, 1)
            };

            OrderingResults results = enchantmentList.Ordering();

            Assert.AreEqual(3, results.Penalty);
            Assert.AreEqual(10, results.MaxExperience);
            Assert.AreEqual(29, results.TotalExperience);

            Assert.AreEqual(3, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Power", results.Steps[0][0].Name);
            Assert.AreEqual(2, results.Steps[1].Count);
            Assert.AreEqual("Punch", results.Steps[1][0].Name);
            Assert.AreEqual("Unbreaking", results.Steps[1][1].Name);
            Assert.AreEqual(2, results.Steps[2].Count);
            Assert.AreEqual("Infinity", results.Steps[2][0].Name);
            Assert.AreEqual("Flame", results.Steps[2][1].Name);
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void SixBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Aqua Affinity", 1, 2),
                new Enchantment("Respiration", 3, 2),
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Blast Protection", 4, 2),
                new Enchantment("Thorns", 3, 4)
            };

            OrderingResults results = enchantmentList.Ordering();

            Assert.AreEqual(3, results.Penalty);
            Assert.AreEqual(17, results.MaxExperience);
            Assert.AreEqual(49, results.TotalExperience);

            Assert.AreEqual(3, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Thorns", results.Steps[0][0].Name);
            Assert.AreEqual(2, results.Steps[1].Count);
            Assert.AreEqual("Blast Protection", results.Steps[1][0].Name);
            Assert.AreEqual("Mending", results.Steps[1][1].Name);
            Assert.AreEqual(3, results.Steps[2].Count);
            Assert.AreEqual("Respiration", results.Steps[2][0].Name);
            Assert.AreEqual("Aqua Affinity", results.Steps[2][1].Name);
            Assert.AreEqual("Unbreaking", results.Steps[2][2].Name);
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void SevenBookOrderTest()
        {
            List<IEnchantment> enchantmentList = new()
            {
                new Enchantment("Knockback", 2, 1),
                new Enchantment("Fire Aspect", 2, 2),
                new Enchantment("Sharpness", 5, 1),
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Looting", 3, 2),
                new Enchantment("Sweeping Edge", 3, 2)
            };

            OrderingResults results = enchantmentList.Ordering();

            Assert.AreEqual(3, results.Penalty);
            Assert.AreEqual(17, results.MaxExperience);
            Assert.AreEqual(52, results.TotalExperience);

            Assert.AreEqual(3, results.Steps.Count);
            Assert.AreEqual(1, results.Steps[0].Count);
            Assert.AreEqual("Sweeping Edge", results.Steps[0][0].Name);
            Assert.AreEqual(2, results.Steps[1].Count);
            Assert.AreEqual("Looting", results.Steps[1][0].Name);
            Assert.AreEqual("Sharpness", results.Steps[1][1].Name);
            Assert.AreEqual(4, results.Steps[2].Count);
            Assert.AreEqual("Fire Aspect", results.Steps[2][0].Name);
            Assert.AreEqual("Unbreaking", results.Steps[2][1].Name);
            Assert.AreEqual("Knockback", results.Steps[2][2].Name);
            Assert.AreEqual("Mending", results.Steps[2][3].Name);
        }
    }
}
