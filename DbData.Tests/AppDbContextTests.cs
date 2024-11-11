using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBData.Models;
using DBData.Data;

namespace DbData.Tests
{
    [TestFixture]
    public class AppDbContextTests
    {
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task SaveAndLoadShoppingList()
        {
            var shoppingList = new ShoppingList
            {
                Name = "Test List",
                Items = new List<ShoppingItem>
                {
                    new ShoppingItem
                    {
                        Name = "Item 1",
                        Quantity = "2",
                        IsPurchased = false,
                        Parameters = new List<ShoppingItemParameter>
                        {
                            new ShoppingItemParameter { Key = "Size", Value = "Medium" }
                        }
                    }
                }
            };

            _context.ShoppingLists.Add(shoppingList);
            await _context.SaveChangesAsync();

            var savedList = await _context.ShoppingLists
                .Include(sl => sl.Items)
                .ThenInclude(si => si.Parameters)
                .FirstOrDefaultAsync(sl => sl.Name == "Test List");

            Assert.IsNotNull(savedList);
            Assert.AreEqual("Test List", savedList.Name);
            Assert.AreEqual(1, savedList.Items.Count);
            Assert.AreEqual("Item 1", savedList.Items.First().Name);
            Assert.AreEqual("2", savedList.Items.First().Quantity);
            Assert.IsFalse(savedList.Items.First().IsPurchased);
            Assert.AreEqual("Size", savedList.Items.First().Parameters.First().Key);
            Assert.AreEqual("Medium", savedList.Items.First().Parameters.First().Value);
        }
    }
}
