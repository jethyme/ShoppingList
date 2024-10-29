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
    public class AddItemToListOperationTests
    {
        private Mock<IShoppingListService> _mockShoppingListService;
        private Mock<IDataStorage> _mockDataStorage;
        private AddItemToListOperation _operation;

        [SetUp]
        public void Setup()
        {
            // Инициализация мока для IShoppingListService и IDataStorage
            _mockDataStorage = new Mock<IDataStorage>();
            _mockShoppingListService = new Mock<IShoppingListService>();
            _mockShoppingListService.Setup(s => s.DataStorage).Returns(_mockDataStorage.Object);
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(new List<ShoppingList>());
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddItemToListAndSaveData()
        {
            // Arrange
            var shoppingList = new ShoppingList
            {
                Name = "Groceries",
                Items = new List<ShoppingItem>()
            };
            var itemToAdd = new ShoppingItem { Name = "Milk" };

            // Настройка мока для ShoppingLists
            var shoppingLists = new List<ShoppingList> { shoppingList };
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);

            // Создание операции
            _operation = new AddItemToListOperation(_mockShoppingListService.Object, "Groceries", itemToAdd);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.AreEqual(1, shoppingList.Items.Count);
            Assert.AreEqual("Milk", shoppingList.Items.First().Name);
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(shoppingLists), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_ShouldNotAddItemIfListDoesNotExist()
        {
            // Arrange
            var itemToAdd = new ShoppingItem { Name = "Milk" };

            // Создание операции
            _operation = new AddItemToListOperation(_mockShoppingListService.Object, "NonExistentList", itemToAdd);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            _mockShoppingListService.Verify(s => s.DataStorage.SaveDataAsync(It.IsAny<List<ShoppingList>>()), Times.Never);
        }
    }
}
