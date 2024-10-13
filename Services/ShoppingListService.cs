using Core.Models;
using Core.Interfaces;

namespace Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IDataStorage _dataStorage;

        public ShoppingListService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task CreateShoppingListAsync(string name)
        {
            var list = new ShoppingList { Name = name, Items = new List<ShoppingItem>() };
            await _dataStorage.SaveShoppingListAsync(list);
        }

        public async Task AddItemToListAsync(string listName, ShoppingItem item)
        {
            var list = await _dataStorage.LoadShoppingListAsync(listName);
            list?.Items.Add(item);
            await _dataStorage.SaveShoppingListAsync(list);
        }

        public async Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync()
        {
            return await _dataStorage.LoadAllShoppingListsAsync();
        }

        public async Task MarkItemAsPurchasedAsync(string listName, string itemName)
        {
            var list = await _dataStorage.LoadShoppingListAsync(listName);
            var item = list?.Items.FirstOrDefault(i => i.Name == itemName);
            if (item != null)
            {
                item.IsPurchased = true;
                await _dataStorage.SaveShoppingListAsync(list);
            }
        }

        public async Task<IEnumerable<ShoppingItem>> GetHistoryAsync(string listName)
        {
            var list = await _dataStorage.LoadShoppingListAsync(listName);
            return list?.Items.Where(i => i.IsPurchased) ?? Enumerable.Empty<ShoppingItem>();
        }
    }

}