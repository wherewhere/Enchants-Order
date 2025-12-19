using EnchantsOrder.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnchantsOrder.Tests
{
    /// <summary>
    /// Tests the <see cref="EnchantsOrder"/> class.
    /// </summary>
    [TestFixture]
    public class EnchantsOrderTests
    {
        /// <summary>
        /// Represents the result of an ordering operation, including penalty and experience metrics.
        /// </summary>
        /// <param name="Penalty">The penalty value associated with the ordering result. Typically represents a cost or score indicating the
        /// quality of the ordering. Must be zero or greater.</param>
        /// <param name="MaxExperience">The maximum experience value found among all items in the ordering. Must be zero or greater.</param>
        /// <param name="TotalExperience">The total combined experience value for all items in the ordering. Must be zero or greater.</param>
        public record struct OrderingResult(int Penalty, int MaxExperience, int TotalExperience);

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Curse of Vanishing", 1, 4)]
                }, new OrderingResult(1, 4, 4))
                {
                    TestName = $"{nameof(OrderingTest)} (Item)"
                };

                #region Trident
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Impaling", 5, 2)],
                    [new Enchantment("Channeling", 1, 4), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Loyalty", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 10, 33))
                {
                    TestName = $"{nameof(OrderingTest)} (Trident - Loyalty)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Impaling", 5, 2)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Riptide", 3, 2)]
                }, new OrderingResult(3, 10, 28))
                {
                    TestName = $"{nameof(OrderingTest)} (Trident - Riptide)"
                };
                #endregion

                #region Crossbow
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Multishot", 1, 2), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Quick Charge", 3, 1)]
                }, new OrderingResult(3, 6, 17))
                {
                    TestName = $"{nameof(OrderingTest)} (Crossbow - Multishot)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Piercing", 4, 1)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Quick Charge", 3, 1)]
                }, new OrderingResult(3, 7, 19))
                {
                    TestName = $"{nameof(OrderingTest)} (Crossbow - Piercing)"
                };
                #endregion

                #region Bow
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Power", 5, 1)],
                    [new Enchantment("Punch", 2, 2), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Infinity", 1, 4), new Enchantment("Flame", 1, 2)]
                }, new OrderingResult(3, 10, 29))
                {
                    TestName = $"{nameof(OrderingTest)} (Bow - Infinity)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Power", 5, 1)],
                    [new Enchantment("Punch", 2, 2), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Flame", 1, 2)]
                }, new OrderingResult(3, 9, 26))
                {
                    TestName = $"{nameof(OrderingTest)} (Bow - Mending)"
                };
                #endregion

                #region Sword
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Sweeping Edge", 3, 2)],
                    [new Enchantment("Looting", 3, 2), new Enchantment("Sharpness/Smite/Bane of Arthropods", 5, 1)],
                    [new Enchantment("Fire Aspect", 2, 2), new Enchantment("Unbreaking", 3, 1), new Enchantment("Knockback", 2, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 17, 52))
                {
                    TestName = $"{nameof(OrderingTest)} (Sword)"
                };
                #endregion

                #region Mace
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Breach", 4, 2)],
                    [new Enchantment("Wind Burst", 3, 2), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Fire Aspect", 2, 2), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 11, 34))
                {
                    TestName = $"{nameof(OrderingTest)} (Mace - Breach)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Wind Burst", 3, 2)],
                    [new Enchantment("Density/Smite/Bane of Arthropods", 5, 1), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Fire Aspect", 2, 2), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 10, 31))
                {
                    TestName = $"{nameof(OrderingTest)} (Mace - Density)"
                };
                #endregion

                #region Fishing Rod
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Lure", 3, 2)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Luck of the Sea", 3, 2)]
                }, new OrderingResult(3, 9, 24))
                {
                    TestName = $"{nameof(OrderingTest)} (Fishing Rod)"
                };
                #endregion

                #region Elytra/Flint and Steel
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(2, 3, 6))
                {
                    TestName = $"{nameof(OrderingTest)} (Elytra)"
                };
                #endregion

                #region Shears
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Efficiency", 5, 1)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(2, 7, 14))
                {
                    TestName = $"{nameof(OrderingTest)} (Shears)"
                };
                #endregion

                #region Axe
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Fortune", 3, 2)],
                    [new Enchantment("Sharpness/Smite/Bane of Arthropods", 5, 1), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Efficiency", 5, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 11, 32))
                {
                    TestName = $"{nameof(OrderingTest)} (Axe - Fortune)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Sharpness/Smite/Bane of Arthropods", 5, 1)],
                    [new Enchantment("Efficiency", 5, 1), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Silk Touch", 1, 4), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 10, 30))
                {
                    TestName = $"{nameof(OrderingTest)} (Axe - Silk Touch)"
                };
                #endregion

                #region Pickaxe/Shovel/Hoe
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Fortune", 3, 2)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Efficiency", 5, 1)]
                }, new OrderingResult(3, 8, 23))
                {
                    TestName = $"{nameof(OrderingTest)} (Pickaxe - Fortune)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Efficiency", 5, 1)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Silk Touch", 1, 4)]
                }, new OrderingResult(3, 7, 21))
                {
                    TestName = $"{nameof(OrderingTest)} (Pickaxe - Silk Touch)"
                };
                #endregion

                #region Helmet
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Blast Protection", 4, 2), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Respiration", 3, 2), new Enchantment("Mending", 1, 2), new Enchantment("Aqua Affinity", 1, 2)]
                }, new OrderingResult(3, 16, 49))
                {
                    TestName = $"{nameof(OrderingTest)} (Helmet - Blast Protection)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Respiration", 3, 2), new Enchantment("Unbreaking", 3, 1)],
                    [new Enchantment("Protection/Projectile Protection/Fire Protection", 4, 1), new Enchantment("Mending", 1, 2), new Enchantment("Aqua Affinity", 1, 2)]
                }, new OrderingResult(3, 14, 45))
                {
                    TestName = $"{nameof(OrderingTest)} (Helmet - Other Protection)"
                };
                #endregion

                #region Chestplate
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Blast Protection", 4, 2)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 12, 32))
                {
                    TestName = $"{nameof(OrderingTest)} (Chestplate - Blast Protection)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Protection/Projectile Protection/Fire Protection", 4, 1)]
                }, new OrderingResult(3, 12, 28))
                {
                    TestName = $"{nameof(OrderingTest)} (Chestplate - Other Protection)"
                };
                #endregion

                #region Leggings
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Swift Sneak", 3, 4), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Blast Protection", 4, 2), new Enchantment("Unbreaking", 3, 1)]
                }, new OrderingResult(3, 16, 48))
                {
                    TestName = $"{nameof(OrderingTest)} (Leggings - Blast Protection)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Swift Sneak", 3, 4), new Enchantment("Mending", 1, 2)],
                    [new Enchantment("Protection/Projectile Protection/Fire Protection", 4, 1), new Enchantment("Unbreaking", 3, 1)]
                }, new OrderingResult(3, 16, 44))
                {
                    TestName = $"{nameof(OrderingTest)} (Leggings - Other Protection)"
                };
                #endregion

                #region Boots
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Soul Speed", 3, 4), new Enchantment("Depth Strider", 3, 2)],
                    [new Enchantment("Blast Protection", 4, 2), new Enchantment("Feather Falling", 4, 1), new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 23, 74))
                {
                    TestName = $"{nameof(OrderingTest)} (Boots - Blast Protection - Depth Strider)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Soul Speed", 3, 4), new Enchantment("Protection/Projectile Protection/Fire Protection", 4, 1)],
                    [new Enchantment("Depth Strider", 3, 2), new Enchantment("Feather Falling", 4, 1), new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 21, 68))
                {
                    TestName = $"{nameof(OrderingTest)} (Boots - Other Protection - Depth Strider)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Soul Speed", 3, 4), new Enchantment("Feather Falling", 4, 1)],
                    [new Enchantment("Blast Protection", 4, 2), new Enchantment("Frost Walker", 2, 2), new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 23, 70))
                {
                    TestName = $"{nameof(OrderingTest)} (Boots - Blast Protection - Frost Walker)"
                };
                yield return new TestCaseData(new Enchantment[][]
                {
                    [new Enchantment("Thorns", 3, 4)],
                    [new Enchantment("Soul Speed", 3, 4), new Enchantment("Protection/Projectile Protection/Fire Protection", 4, 1)],
                    [new Enchantment("Feather Falling", 4, 1), new Enchantment("Frost Walker", 2, 2), new Enchantment("Unbreaking", 3, 1), new Enchantment("Mending", 1, 2)]
                }, new OrderingResult(3, 19, 66))
                {
                    TestName = $"{nameof(OrderingTest)} (Boots - Other Protection - Frost Walker)"
                };
                #endregion
            }
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering{T}(IEnumerable{T}, int)"/> method.
        /// </summary>
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void OrderingTest(Enchantment[][] enchantments, OrderingResult result)
        {
            OrderingResults results = enchantments.Unwrap().Random().Ordering();

            Assert.That(results.Penalty, Is.EqualTo(result.Penalty));
            Assert.That(results.MaxExperience, Is.EqualTo(result.MaxExperience));
            Assert.That(results.TotalExperience, Is.EqualTo(result.TotalExperience));

            Assert.That(results.Steps.Count, Is.EqualTo(enchantments.Length));
            for (int i = 0; i < results.Steps.Count; i++)
            {
                EnchantmentStep step = results.Steps[i];
                Assert.That(step.Count, Is.EqualTo(enchantments[i].Length));
                for (int j = 0; j < step.Count; j++)
                {
                    Assert.That(step[j], Is.EqualTo(enchantments[i][j]));
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="EnchantsOrder.Ordering{T}(IEnumerable{T}, int)"/> method.
        /// </summary>
        [Test]
        public void OrderingEmptyTest()
        {
            IEnchantment[] enchantmentList = null;
            _ = Assert.Throws<ArgumentNullException>(() => _ = enchantmentList.Ordering());
            enchantmentList = [];
            _ = Assert.Throws<ArgumentNullException>(() => _ = enchantmentList.Ordering());
        }
    }

    file static class Enumerable
    {
        public static IEnumerable<T> Unwrap<T>(this T[][] arrays)
        {
            foreach (T[] array in arrays)
            {
                foreach (T enchantment in array)
                {
                    yield return enchantment;
                }
            }
        }

        public static IEnumerable<T> Random<T>(this IEnumerable<T> source)
        {
            Random rand = new();
            return source.OrderBy(_ => rand.Next());
        }
    }
}
