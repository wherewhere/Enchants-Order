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
            IEnchantment[] enchantmentList = null;
            _ = Assert.Throws<ArgumentNullException>(() => _ = enchantmentList.Ordering());
            enchantmentList = [];
            _ = Assert.Throws<ArgumentNullException>(() => _ = enchantmentList.Ordering());
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void OneBookOrderTest()
        {
            Enchantment[] enchantmentList = [new Enchantment("Efficiency", 5, 1)];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(1));
            Assert.That(results.MaxExperience, Is.EqualTo(5));
            Assert.That(results.TotalExperience, Is.EqualTo(5));

            Assert.That(results.Steps.Count, Is.EqualTo(1));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Efficiency"));
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void TwoBookOrderTest()
        {
            Enchantment[] enchantmentList =
            [
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1)
            ];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(2));
            Assert.That(results.MaxExperience, Is.EqualTo(3));
            Assert.That(results.TotalExperience, Is.EqualTo(6));

            Assert.That(results.Steps.Count, Is.EqualTo(2));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Unbreaking"));

            Assert.That(results.Steps[1].Count, Is.EqualTo(1));
            Assert.That(results.Steps[1][0].Name, Is.EqualTo("Mending"));
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void ThreeBookOrderTest()
        {
            Enchantment[] enchantmentList =
            [
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Projectile Protection", 4, 1)
            ];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(2));
            Assert.That(results.MaxExperience, Is.EqualTo(7));
            Assert.That(results.TotalExperience, Is.EqualTo(13));

            Assert.That(results.Steps.Count, Is.EqualTo(2));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Projectile Protection"));

            Assert.That(results.Steps[1].Count, Is.EqualTo(2));
            Assert.That(results.Steps[1][0].Name, Is.EqualTo("Unbreaking"));
            Assert.That(results.Steps[1][1].Name, Is.EqualTo("Mending"));
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void FourBookOrderTest()
        {
            Enchantment[] enchantmentList =
            [
                new Enchantment("Riptide", 3, 2),
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Impaling", 5, 2)
            ];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(3));
            Assert.That(results.MaxExperience, Is.EqualTo(10));
            Assert.That(results.TotalExperience, Is.EqualTo(28));

            Assert.That(results.Steps.Count, Is.EqualTo(3));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Impaling"));

            Assert.That(results.Steps[1].Count, Is.EqualTo(2));
            Assert.That(results.Steps[1][0].Name, Is.EqualTo("Unbreaking"));
            Assert.That(results.Steps[1][1].Name, Is.EqualTo("Mending"));

            Assert.That(results.Steps[2].Count, Is.EqualTo(1));
            Assert.That(results.Steps[2][0].Name, Is.EqualTo("Riptide"));
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void FiveBookOrderTest()
        {
            Enchantment[] enchantmentList =
            [
                new Enchantment("Punch", 2, 2),
                new Enchantment("Flame", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Infinity", 1, 4),
                new Enchantment("Power", 5, 1)
            ];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(3));
            Assert.That(results.MaxExperience, Is.EqualTo(10));
            Assert.That(results.TotalExperience, Is.EqualTo(29));

            Assert.That(results.Steps.Count, Is.EqualTo(3));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Power"));

            Assert.That(results.Steps[1].Count, Is.EqualTo(2));
            Assert.That(results.Steps[1][0].Name, Is.EqualTo("Punch"));
            Assert.That(results.Steps[1][1].Name, Is.EqualTo("Unbreaking"));

            Assert.That(results.Steps[2].Count, Is.EqualTo(2));
            Assert.That(results.Steps[2][0].Name, Is.EqualTo("Infinity"));
            Assert.That(results.Steps[2][1].Name, Is.EqualTo("Flame"));
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void SixBookOrderTest()
        {
            Enchantment[] enchantmentList =
            [
                new Enchantment("Aqua Affinity", 1, 2),
                new Enchantment("Respiration", 3, 2),
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Blast Protection", 4, 2),
                new Enchantment("Thorns", 3, 4)
            ];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(3));
            Assert.That(results.MaxExperience, Is.EqualTo(17));
            Assert.That(results.TotalExperience, Is.EqualTo(49));

            Assert.That(results.Steps.Count, Is.EqualTo(3));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Thorns"));

            Assert.That(results.Steps[1].Count, Is.EqualTo(2));
            Assert.That(results.Steps[1][0].Name, Is.EqualTo("Blast Protection"));
            Assert.That(results.Steps[1][1].Name, Is.EqualTo("Mending"));

            Assert.That(results.Steps[2].Count, Is.EqualTo(3));
            Assert.That(results.Steps[2][0].Name, Is.EqualTo("Respiration"));
            Assert.That(results.Steps[2][1].Name, Is.EqualTo("Aqua Affinity"));
            Assert.That(results.Steps[2][2].Name, Is.EqualTo("Unbreaking"));
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering(IEnumerable{IEnchantment}, int)"/> method.
        /// </summary>
        [Test]
        public void SevenBookOrderTest()
        {
            Enchantment[] enchantmentList =
            [
                new Enchantment("Knockback", 2, 1),
                new Enchantment("Fire Aspect", 2, 2),
                new Enchantment("Sharpness", 5, 1),
                new Enchantment("Mending", 1, 2),
                new Enchantment("Unbreaking", 3, 1),
                new Enchantment("Looting", 3, 2),
                new Enchantment("Sweeping Edge", 3, 2)
            ];

            OrderingResults results = enchantmentList.Ordering();

            Assert.That(results.Penalty, Is.EqualTo(3));
            Assert.That(results.MaxExperience, Is.EqualTo(17));
            Assert.That(results.TotalExperience, Is.EqualTo(52));

            Assert.That(results.Steps.Count, Is.EqualTo(3));
            Assert.That(results.Steps[0].Count, Is.EqualTo(1));
            Assert.That(results.Steps[0][0].Name, Is.EqualTo("Sweeping Edge"));

            Assert.That(results.Steps[1].Count, Is.EqualTo(2));
            Assert.That(results.Steps[1][0].Name, Is.EqualTo("Looting"));
            Assert.That(results.Steps[1][1].Name, Is.EqualTo("Sharpness"));

            Assert.That(results.Steps[2].Count, Is.EqualTo(4));
            Assert.That(results.Steps[2][0].Name, Is.EqualTo("Fire Aspect"));
            Assert.That(results.Steps[2][1].Name, Is.EqualTo("Unbreaking"));
            Assert.That(results.Steps[2][2].Name, Is.EqualTo("Knockback"));
            Assert.That(results.Steps[2][3].Name, Is.EqualTo("Mending"));
        }
    }
}
