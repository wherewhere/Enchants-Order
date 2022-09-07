using NUnit.Framework;

namespace EnchantsOrder.Tests
{
    /// <summary>
    /// Tests the <see cref="Extensions"/> class.
    /// </summary>
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void GetLoumaNumberTest()
        {
            string[] strings = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            for (int i = 1; i <= 10; i++)
            {
                Assert.AreEqual(strings[i - 1], i.GetLoumaNumber());
            }
            Assert.AreEqual("MCCXXXIV", 1234.GetLoumaNumber());
        }
    }
}
