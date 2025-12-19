using NUnit.Framework;

namespace EnchantsOrder.Models.Tests
{
    /// <summary>
    /// Tests the <see cref="EnchantmentStep"/> class.
    /// </summary>
    [TestFixture]
    public class EnchantmentStepTests
    {
        /// <summary>
        /// Tests the <see cref="EnchantmentStep.ToString"/> method.
        /// </summary>
        [Test]
        public void ToStringTest()
        {
            EnchantmentStep step = new(1, new Enchantment("Test", 1, 2));
            Assert.That(step.ToString(), Is.EqualTo("Step 1: Test I"));

            step = new EnchantmentStep(2, new Enchantment("Test1", 1, 2), new Enchantment("Test2", 3, 4));
            Assert.That(step.ToString(), Is.EqualTo("Step 2: Test1 I + Test2 III"));

            step = new EnchantmentStep(3, new Enchantment("Test1", 1, 2), new Enchantment("Test2", 3, 4), new Enchantment("Test3", 5, 6));
            Assert.That(step.ToString(), Is.EqualTo("Step 3: (Test1 I + Test2 III) + Test3 V"));

            step = new EnchantmentStep(4, new Enchantment("Test1", 1, 2), new Enchantment("Test2", 3, 4), new Enchantment("Test3", 5, 6), new Enchantment("Test4", 7, 8));
            Assert.That(step.ToString(), Is.EqualTo("Step 4: (Test1 I + Test2 III) + (Test3 V + Test4 VII)"));
        }
    }
}
