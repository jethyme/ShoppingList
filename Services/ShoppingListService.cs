using Core.Models;
using Core.Interfaces;

namespace Services
{
    public class ShoppingListService : IShoppingListService
    {
        public IDataStorage DataStorage { get; set; }
        public List<ShoppingList> ShoppingLists { get; set; }

        public ShoppingListService(IDataStorage storage)
        {
            DataStorage = storage;
            ShoppingLists = new List<ShoppingList>();
        }
        public async Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync()
        {
            ShoppingLists = (await DataStorage.LoadDataAsync()).ToList();
            return ShoppingLists;
        }

        public async Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName)
        {
            var list = ShoppingLists.FirstOrDefault(l => l.Name == listName);
            return list?.Items.Where(i => i.IsPurchased) ?? Enumerable.Empty<ShoppingItem>();
        }

        public async Task<bool> CheckIfListNameExistsAsync(string listName)
        {
            if (!ShoppingLists.Any())
            {
                await GetAllShoppingListsAsync();
            }

            return ShoppingLists.Any(l => l.Name.Equals(listName, StringComparison.OrdinalIgnoreCase));
        }

    }


}