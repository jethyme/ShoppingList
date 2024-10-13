using Core.Models;
using Core.Interfaces;

namespace Services
{
    public class ShoppingListService : IShoppingListService
    {
        private readonly IDataStorage _dataStorage;
        private List<ShoppingList> _shoppingLists;

        public ShoppingListService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            _shoppingLists = new List<ShoppingList>();
        }

        public async Task CreateShoppingListAsync(string name)
        {
            _shoppingLists.Add(new ShoppingList { Name = name });
            await _dataStorage.SaveDataAsync(_shoppingLists);
        }

        public async Task AddItemToListAsync(string listName, ShoppingItem item)
        {
            var list = _shoppingLists.FirstOrDefault(l => l.Name == listName);
            if (list != null)
            {
                list.Items.Add(item);
                await _dataStorage.SaveDataAsync(_shoppingLists);
            }
        }

        public async Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync()
        {
            _shoppingLists = (await _dataStorage.LoadDataAsync()).ToList();
            return _shoppingLists;
        }

        public async Task MarkItemAsPurchasedAsync(string listName, string itemName)
        {
            var list = _shoppingLists.FirstOrDefault(l => l.Name == listName);
            var item = list?.Items.FirstOrDefault(i => i.Name == itemName);
            if (item != null)
            {
                item.IsPurchased = true;
                await _dataStorage.SaveDataAsync(_shoppingLists);
            }
        }

        public async Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName)
        {
            var list = _shoppingLists.FirstOrDefault(l => l.Name == listName);
            return list?.Items.Where(i => i.IsPurchased) ?? Enumerable.Empty<ShoppingItem>();
        }

        public async Task UpdateItemInListAsync(string listName, ShoppingItem item)
        {
            var list = _shoppingLists.FirstOrDefault(l => l.Name == listName);
            var existingItem = list?.Items.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
                await _dataStorage.SaveDataAsync(_shoppingLists);
            }
        }
    }


}