using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Moq;
using NUnit.Framework;
using Services;

namespace TestProjectShoppingList
{
    [TestFixture]
    public class ShoppingListServiceTests
    {
        private Mock<IDataStorage> _mockDataStorage;
        private ShoppingListService _service;

        [SetUp]
        public void SetUp()
        {
            // »нициализаци€ мока и сервиса перед каждым тестом
            _mockDataStorage = new Mock<IDataStorage>();
            _service = new ShoppingListService(_mockDataStorage.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // ќчистка ресурсов после каждого теста (если требуетс€)
            _mockDataStorage = null;
            _service = null;
        }

        [Test]
        public async Task GetAllShoppingListsAsync_ShouldLoadShoppingLists()
        {
            // Arrange
            var expectedLists = new List<ShoppingList>
            {
                new ShoppingList { Name = "Groceries" },
                new ShoppingList { Name = "Electronics" }
            };
            _mockDataStorage.Setup(ds => ds.LoadDataAsync()).ReturnsAsync(expectedLists);

            // Act
            var result = await _service.GetAllShoppingListsAsync();

            // Assert
            Assert.AreEqual(expectedLists.Count, result.Count());
            Assert.AreEqual(expectedLists[0].Name, result.First().Name);
        }

        [Test]
        public async Task GetPurchasedItemsAsync_ShouldReturnPurchasedItems()
        {
            // Arrange
            var list = new ShoppingList
            {
                Name = "Groceries",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem { Name = "Milk", IsPurchased = true },
                    new ShoppingItem { Name = "Bread", IsPurchased = false }
                }
            };
            _service.ShoppingLists.Add(list);

            // Act
            var result = await _service.GetPurchasedItemsAsync("Groceries");

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Milk", result.First().Name);
        }

        [Test]
        public async Task CheckIfListNameExistsAsync_ShouldReturnTrueIfListExists()
        {
            // Arrange
            var list = new ShoppingList { Name = "Groceries" };
            _service.ShoppingLists.Add(list);

            // Act
            var result = await _service.CheckIfListNameExistsAsync("Groceries");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CheckIfListNameExistsAsync_ShouldReturnFalseIfListDoesNotExist()
        {
            // Act
            var result = await _service.CheckIfListNameExistsAsync("NonExistentList");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task CheckIfItemNameExistsAsync_ShouldReturnTrueIfItemExists()
        {
            // Arrange
            var list = new ShoppingList
            {
                Name = "Groceries",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem { Name = "Milk" },
                    new ShoppingItem { Name = "Bread" }
                }
            };
            _service.ShoppingLists.Add(list);

            // Act
            var result = await _service.CheckIfItemNameExistsAsync("Groceries", "Milk");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CheckIfItemNameExistsAsync_ShouldReturnFalseIfItemDoesNotExist()
        {
            // Arrange
            var list = new ShoppingList
            {
                Name = "Groceries",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem { Name = "Milk" }
                }
            };
            _service.ShoppingLists.Add(list);

            // Act
            var result = await _service.CheckIfItemNameExistsAsync("Groceries", "Bread");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
