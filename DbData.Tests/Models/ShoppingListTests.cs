using DBData.Models;

namespace DBData.Tests.Models
{
    [TestFixture]
    public class ShoppingListTests
    {
        [Test]
        public void ShoppingList_ShouldInitializeWithDefaultValues()
        {
            var shoppingList = new ShoppingList();

            Assert.AreEqual(0, shoppingList.Id);
            Assert.IsNull(shoppingList.Name);
            Assert.IsNotNull(shoppingList.Items);
            Assert.AreEqual(0, shoppingList.Items.Count);
        }

        [Test]
        public void ShoppingList_ShouldAllowSettingProperties()
        {
            var shoppingList = new ShoppingList
            {
                Id = 1,
                Name = "Weekly Groceries",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem { Id = 1, Name = "Milk", Quantity = "1 liter" },
                    new ShoppingItem { Id = 2, Name = "Bread", Quantity = "2 loaves" }
                }
            };

            Assert.AreEqual(1, shoppingList.Id);
            Assert.AreEqual("Weekly Groceries", shoppingList.Name);
            Assert.AreEqual(2, shoppingList.Items.Count);
            Assert.AreEqual("Milk", shoppingList.Items[0].Name);
            Assert.AreEqual("1 liter", shoppingList.Items[0].Quantity);
            Assert.AreEqual("Bread", shoppingList.Items[1].Name);
            Assert.AreEqual("2 loaves", shoppingList.Items[1].Quantity);
        }

        [Test]
        public void ShoppingList_ShouldAllowAddingItems()
        {
            var shoppingList = new ShoppingList
            {
                Id = 1,
                Name = "Weekend Shopping"
            };

            var item1 = new ShoppingItem { Id = 1, Name = "Eggs", Quantity = "12" };
            var item2 = new ShoppingItem { Id = 2, Name = "Cheese", Quantity = "200 grams" };

            shoppingList.Items.Add(item1);
            shoppingList.Items.Add(item2);

            Assert.AreEqual(2, shoppingList.Items.Count);
            Assert.AreEqual("Eggs", shoppingList.Items[0].Name);
            Assert.AreEqual("Cheese", shoppingList.Items[1].Name);
        }
    }
}
