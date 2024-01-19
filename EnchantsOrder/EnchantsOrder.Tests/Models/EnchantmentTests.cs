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
            Assert.That(enchantment1.CompareTo(enchantment2), Is.EqualTo(-1));

            enchantment1.Level = 6;
            enchantment1.Weight = 2;
            Assert.That(enchantment1.CompareTo(enchantment2), Is.EqualTo(1));

            enchantment1.Level = 3;
            enchantment1.Weight = 4;
            Assert.That(enchantment1.CompareTo(enchantment2), Is.EqualTo(-1));

            enchantment1.Name = "Y";
            Assert.That(enchantment1.CompareTo(enchantment2), Is.EqualTo(0));
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
            Assert.That(enchantment.Experience, Is.EqualTo((long)level * weight));
        }
    }
}
