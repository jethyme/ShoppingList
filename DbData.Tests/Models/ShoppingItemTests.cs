using DBData.Models;

namespace DbData.Tests.Models
{
    [TestFixture]
    public class ShoppingItemTests
    {
        [Test]
        public void ShoppingItem_ShouldInitializeWithDefaultValues()
        {
            var shoppingItem = new ShoppingItem();

            Assert.AreEqual(0, shoppingItem.Id);
            Assert.IsNull(shoppingItem.Name);
            Assert.IsNull(shoppingItem.Quantity);
            Assert.IsFalse(shoppingItem.IsPurchased);
            Assert.IsNull(shoppingItem.PurchaseTime);
            Assert.AreEqual(0, shoppingItem.ShoppingListId);
            Assert.IsNotNull(shoppingItem.Parameters);
            Assert.AreEqual(0, shoppingItem.Parameters.Count);
        }

        [Test]
        public void ShoppingItem_ShouldAllowSettingProperties()
        {
            var shoppingItem = new ShoppingItem
            {
                Id = 1,
                Name = "Milk",
                Quantity = "2 liters",
                IsPurchased = false,
                PurchaseTime = null,
                ShoppingListId = 1,
                Parameters = new List<ShoppingItemParameter>
                {
                    new ShoppingItemParameter { Key = "Brand", Value = "Brand A" }
                }
            };

            Assert.AreEqual(1, shoppingItem.Id);
            Assert.AreEqual("Milk", shoppingItem.Name);
            Assert.AreEqual("2 liters", shoppingItem.Quantity);
            Assert.IsFalse(shoppingItem.IsPurchased);
            Assert.IsNull(shoppingItem.PurchaseTime);
            Assert.AreEqual(1, shoppingItem.ShoppingListId);
            Assert.AreEqual(1, shoppingItem.Parameters.Count);
            Assert.AreEqual("Brand", shoppingItem.Parameters[0].Key);
            Assert.AreEqual("Brand A", shoppingItem.Parameters[0].Value);
        }

        [Test]
        public void ShoppingItem_ShouldHaveNavigationProperty()
        {
            var shoppingList = new ShoppingList { Id = 1, Name = "Grocery List" };
            var shoppingItem = new ShoppingItem
            {
                Id = 1,
                Name = "Bread",
                ShoppingList = shoppingList
            };

            Assert.IsNotNull(shoppingItem.ShoppingList);
            Assert.AreEqual(1, shoppingItem.ShoppingList.Id);
            Assert.AreEqual("Grocery List", shoppingItem.ShoppingList.Name);
        }
    }
}
