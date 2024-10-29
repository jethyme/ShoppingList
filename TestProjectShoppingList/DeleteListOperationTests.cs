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
    public class DeleteListOperationTests
    {
        private Mock<IShoppingListService> _mockShoppingListService;
        private Mock<IDataStorage> _mockDataStorage;
        private DeleteListOperation _operation;

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
        public async Task ExecuteAsync_ShouldRemoveShoppingListAndSaveData()
        {
            // Arrange
            var shoppingListName = "Groceries";
            var shoppingList = new ShoppingList { Name = shoppingListName };
            var shoppingLists = new List<ShoppingList> { shoppingList };
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new DeleteListOperation(_mockShoppingListService.Object, shoppingListName);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.IsFalse(shoppingLists.Any(l => l.Name == shoppingListName)); // Проверяем, что список был удален
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(shoppingLists), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_ShouldNotRemoveListIfListDoesNotExist()
        {
            // Arrange
            var shoppingListName = "NonExistentList";
            var shoppingLists = new List<ShoppingList>(); // Пустой список
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);
            _operation = new DeleteListOperation(_mockShoppingListService.Object, shoppingListName);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(It.IsAny<List<ShoppingList>>()), Times.Never); // Проверяем, что SaveDataAsync не был вызван
        }
    }
}
