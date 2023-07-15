using NUnit.Framework;
using System;

namespace EnchantsOrder.Models.Tests
{
    /// <summary>
    /// Tests the <see cref="Enchantment"/> class.
    /// </summary>
    [TestFixture]
    public class EnchantmentTests
    {
        /// <summary>
        /// Tests the <see cref="Enchantment.CompareTo(IEnchantment)"/> method.
        /// </summary>
        [Test]
        public void CompareToTest()
        {
            Enchantment enchantment1 = new("X", 1, 2);
            Enchantment enchantment2 = new("Y", 3, 4);
            Assert.AreEqual(-1, enchantment1.CompareTo(enchantment2));

            enchantment1.Level = 6;
            enchantment1.Weight = 2;
            Assert.AreEqual(1, enchantment1.CompareTo(enchantment2));

            enchantment1.Level = 3;
            enchantment1.Weight = 4;
            Assert.AreEqual(-1, enchantment1.CompareTo(enchantment2));

            enchantment1.Name = "Y";
            Assert.AreEqual(0, enchantment1.CompareTo(enchantment2));
        }

        /// <summary>
        /// Tests the <see cref="Enchantment.Experience"/> method.
        /// </summary>
        [Test]
        public void ExperienceTest()
        {
            Random random = new();
            int level = random.Next();
            int weight = random.Next();
            string name = "ExperienceTest";
            Enchantment enchantment = new(name, level, weight);
            Assert.AreEqual((long)level * weight, enchantment.Experience);
        }
    }
}
