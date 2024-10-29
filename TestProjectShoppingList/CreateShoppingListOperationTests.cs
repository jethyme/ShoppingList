using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Moq;
using NUnit.Framework;
using Services.Operations;

namespace TestProjectShoppingList
{
    [TestFixture]
    public class CreateShoppingListOperationTests
    {
        private Mock<IShoppingListService> _mockShoppingListService;
        private Mock<IDataStorage> _mockDataStorage;
        private CreateShoppingListOperation _operation;

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
        public async Task ExecuteAsync_ShouldAddNewShoppingListAndSaveData()
        {
            // Arrange
            var newListName = "Groceries";
            var shoppingLists = new List<ShoppingList>();
            _mockShoppingListService.Setup(s => s.ShoppingLists).Returns(shoppingLists);

            // Создание операции
            _operation = new CreateShoppingListOperation(_mockShoppingListService.Object, newListName);

            // Act
            await _operation.ExecuteAsync();

            // Assert
            Assert.AreEqual(1, shoppingLists.Count);
            Assert.AreEqual(newListName, shoppingLists[0].Name);
            _mockDataStorage.Verify(ds => ds.SaveDataAsync(shoppingLists), Times.Once);
        }
    }
}
