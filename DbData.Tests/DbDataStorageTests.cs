using Core.Interfaces;
using DBData;
using DBData.Data;
using Microsoft.EntityFrameworkCore;
using ShoppingItem = Core.Models.ShoppingItem;
using ShoppingList = Core.Models.ShoppingList;

namespace DbData.Tests
{
    [TestFixture]
    public class DbDataStorageTests
    {
        private DbContextOptions<AppDbContext> _options;
        private IDataStorage _dataStorage;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dataStorage = new DbDataStorage(new AppDbContext(_options));
        }

        [TearDown]
        public void TearDown()
        {
            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Test]
        public async Task SaveDataAsync_ShouldSaveShoppingLists()
        {
            var listsToSave = new List<ShoppingList>
            {
                new ShoppingList
                {
                    Name = "Grocery List",
                    Items = new List<ShoppingItem>
                    {
                        new ShoppingItem
                        {
                            Name = "Milk",
                            Quantity = "2 liters",
                            IsPurchased = false,
                            PurchaseTime = null,
                            Parameters = new Dictionary<string, string> 
                            {
                                { "Brand", "Brand A" }
                            }
                        }
                    }
                }
            };

            await _dataStorage.SaveDataAsync(listsToSave);

            using (var context = new AppDbContext(_options))
            {
                var savedLists = await context.ShoppingLists
                    .Include(sl => sl.Items)
                    .ThenInclude(si => si.Parameters)
                    .ToListAsync();

                Assert.AreEqual(1, savedLists.Count);
                Assert.AreEqual("Grocery List", savedLists[0].Name);
                Assert.AreEqual(1, savedLists[0].Items.Count);
                Assert.AreEqual("Milk", savedLists[0].Items[0].Name);
                Assert.AreEqual("2 liters", savedLists[0].Items[0].Quantity);
                Assert.IsFalse(savedLists[0].Items[0].IsPurchased);
                                Assert.AreEqual("Brand", savedLists[0].Items[0].Parameters.First().Key);
                Assert.AreEqual("Brand A", savedLists[0].Items[0].Parameters.First().Value);
            }
        }

        [Test]
        public async Task LoadDataAsync_ShouldLoadShoppingLists()
        {
            var shoppingList = new ShoppingList
            {
                Name = "Weekly Groceries",
                Items = new List<ShoppingItem>
        {
            new ShoppingItem
            {
                Name = "Eggs",
                Quantity = "12",
                IsPurchased = false,
                PurchaseTime = null,
                Parameters = new Dictionary<string, string>
                {
                    { "Size", "Large" }
                }
            }
        }
            };

            await _dataStorage.SaveDataAsync(new List<ShoppingList> { shoppingList });

            var loadedLists = await _dataStorage.LoadDataAsync();

            var loadedListsList = loadedLists.ToList();

            Assert.AreEqual(1, loadedListsList.Count);
            Assert.AreEqual("Weekly Groceries", loadedListsList[0].Name);
            Assert.AreEqual(1, loadedListsList[0].Items.Count);
            Assert.AreEqual("Eggs", loadedListsList[0].Items[0].Name);
            Assert.AreEqual("12", loadedListsList[0].Items[0].Quantity);
            Assert.IsFalse(loadedListsList[0].Items[0].IsPurchased);
            Assert.AreEqual("Size", loadedListsList[0].Items[0].Parameters.First().Key);
            Assert.AreEqual("Large", loadedListsList[0].Items[0].Parameters.First().Value);
        }

    }
}
