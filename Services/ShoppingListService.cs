using Core.Models;
using Core.Interfaces;

namespace Services
{
    public class ShoppingListService : IShoppingListService
    {
        public IDataStorage dataStorage { get; set; }
        public List<ShoppingList> shoppingLists { get; set; }

        public ShoppingListService(IDataStorage storage)
        {
            dataStorage = storage;
            shoppingLists = new List<ShoppingList>();
        }
        public async Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync()
        {
            shoppingLists = (await dataStorage.LoadDataAsync()).ToList();
            return shoppingLists;
        }

        public async Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName)
        {
            var list = shoppingLists.FirstOrDefault(l => l.Name == listName);
            return list?.Items.Where(i => i.IsPurchased) ?? Enumerable.Empty<ShoppingItem>();
        }
    }


}