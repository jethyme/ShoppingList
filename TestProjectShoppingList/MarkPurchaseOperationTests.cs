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
    public class MarkPurchaseOperationTests
    {
        private Mock<IShoppingListService> _mockShoppingListService;
        private Mock<IDataStorage> _mockDataStorage;
        private MarkPurchaseOperation _operation;

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
        public async Task ExecuteAsync_ShouldMarkItemAsPurchasedAndSaveData()
        {
            // Arrange
            var shoppingListName = "Groceries";
            var shoppingItem = new ShoppingItem
            {
                Name = "Milk",
                IsPurchased = false
            };

            var shoppingList = new ShoppingList
            {
                Name = shoppingListName,
                Items = new List<ShoppingItem> { shoppingItem }
            };

            var shoppingLists = new List<ShoppingList> { shoppingList };
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new MarkPurchaseOperation(_mockShoppingListService.Object, shoppingListName, "Milk");

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.IsTrue(shoppingItem.IsPurchased); // Проверяем, что элемент помечен как купленный
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(shoppingLists), Times.Once); // Проверяем, что данные сохранены
        }

        [Test]
        public async Task ExecuteAsync_ShouldNotMarkItemIfListDoesNotExist()
        {
            // Arrange
            var shoppingItem = new ShoppingItem
            {
                Name = "Milk",
                IsPurchased = false
            };

            var shoppingLists = new List<ShoppingList>(); // Пустой список
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new MarkPurchaseOperation(_mockShoppingListService.Object, "NonExistentList", "Milk");

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.IsFalse(shoppingItem.IsPurchased); // Элемент не должен быть помечен как купленный
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(It.IsAny<List<ShoppingList>>()), Times.Never); // Проверяем, что SaveDataAsync не был вызван
        }

        [Test]
        public async Task ExecuteAsync_ShouldNotMarkItemIfItemDoesNotExist()
        {
            // Arrange
            var shoppingListName = "Groceries";
            var shoppingItem = new ShoppingItem
            {
                Name = "Milk",
                IsPurchased = false
            };

            var shoppingList = new ShoppingList
            {
                Name = shoppingListName,
                Items = new List<ShoppingItem> { shoppingItem }
            };

            var shoppingLists = new List<ShoppingList> { shoppingList };
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new MarkPurchaseOperation(_mockShoppingListService.Object, shoppingListName, "Bread"); // Элемент не существует

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.IsFalse(shoppingItem.IsPurchased); // Элемент не должен быть помечен как купленный
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(It.IsAny<List<ShoppingList>>()), Times.Never); // Проверяем, что SaveDataAsync не был вызван
        }
    }
}
