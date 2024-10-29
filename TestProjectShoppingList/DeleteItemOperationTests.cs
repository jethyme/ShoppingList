using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Moq;
using NUnit.Framework;
using Services.Operations;

namespace TestProjectShoppingList
{
    [TestFixture]
    public class DeleteItemOperationTests
    {
        private Mock<IShoppingListService> _mockShoppingListService;
        private Mock<IDataStorage> _mockDataStorage;
        private DeleteItemOperation _operation;

        [SetUp]
        public void SetUp()
        {
            // Инициализация мока для IShoppingListService и IDataStorage
            _mockDataStorage = new Mock<IDataStorage>();
            _mockShoppingListService = new Mock<IShoppingListService>();
            _mockShoppingListService.Setup(s => s.DataStorage).Returns(_mockDataStorage.Object);
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(new List<ShoppingList>());
        }

        [Test]
        public async Task ExecuteAsync_ShouldRemoveItemFromListAndSaveData()
        {
            // Arrange
            var itemToDelete = "Milk";
            var shoppingList = new ShoppingList
            {
                Name = "Groceries",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem { Name = itemToDelete },
                    new ShoppingItem { Name = "Bread" }
                }
            };
            var shoppingLists = new List<ShoppingList> { shoppingList };
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new DeleteItemOperation(_mockShoppingListService.Object, "Groceries", itemToDelete);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.AreEqual(1, shoppingList.Items.Count);
            Assert.AreEqual("Bread", shoppingList.Items[0].Name);
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(shoppingLists), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_ShouldNotRemoveItemIfListDoesNotExist()
        {
            // Arrange
            var itemToDelete = "Milk";
            var shoppingLists = new List<ShoppingList>(); // Пустой список
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new DeleteItemOperation(_mockShoppingListService.Object, "NonExistentList", itemToDelete);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(It.IsAny<List<ShoppingList>>()), Times.Never);
        }

        [Test]
        public async Task ExecuteAsync_ShouldNotRemoveItemIfItemDoesNotExist()
        {
            // Arrange
            var shoppingList = new ShoppingList
            {
                Name = "Groceries",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem { Name = "Bread" }
                }
            };
            var shoppingLists = new List<ShoppingList> { shoppingList };
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new DeleteItemOperation(_mockShoppingListService.Object, "Groceries", "Milk"); // Элемент не существует
            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.AreEqual(1, shoppingList.Items.Count); // Элемент не должен быть удален
            Assert.AreEqual("Bread", shoppingList.Items[0].Name);
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(It.IsAny<List<ShoppingList>>()), Times.Never);
        }
    }
}
