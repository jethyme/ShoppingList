using DBData.Models;

namespace DBData.Tests.Models
{
    [TestFixture]
    public class ShoppingItemParameterTests
    {
        [Test]
        public void ShoppingItemParameter_ShouldInitializeWithDefaultValues()
        {
            var parameter = new ShoppingItemParameter();

            Assert.AreEqual(0, parameter.Id);
            Assert.IsNull(parameter.Key);
            Assert.IsNull(parameter.Value);
            Assert.AreEqual(0, parameter.ShoppingItemId);
            Assert.IsNull(parameter.ShoppingItem);
        }

        [Test]
        public void ShoppingItemParameter_ShouldAllowSettingProperties()
        {
            var parameter = new ShoppingItemParameter
            {
                Id = 1,
                Key = "Brand",
                Value = "Brand A",
                ShoppingItemId = 2
            };

            Assert.AreEqual(1, parameter.Id);
            Assert.AreEqual("Brand", parameter.Key);
            Assert.AreEqual("Brand A", parameter.Value);
            Assert.AreEqual(2, parameter.ShoppingItemId);
        }

        [Test]
        public void ShoppingItemParameter_ShouldHaveNavigationProperty()
        {
            var shoppingItem = new ShoppingItem { Id = 2, Name = "Milk" };
            var parameter = new ShoppingItemParameter
            {
                Id = 1,
                Key = "Brand",
                Value = "Brand A",
                ShoppingItem = shoppingItem
            };

            Assert.IsNotNull(parameter.ShoppingItem);
            Assert.AreEqual(2, parameter.ShoppingItem.Id);
            Assert.AreEqual("Milk", parameter.ShoppingItem.Name);
        }
    }
}
