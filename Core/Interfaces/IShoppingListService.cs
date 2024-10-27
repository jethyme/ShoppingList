using Core.Models;

namespace Core.Interfaces
{
    public interface IShoppingListService
    {
        //Task CreateShoppingListAsync(string name);
        //Task AddItemToListAsync(string listName, ShoppingItem item);
        Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync();
        //Task MarkItemAsPurchasedAsync(string listName, string itemName);
        Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName);
        //Task UpdateItemInListAsync(string listName, ShoppingItem item);

        List<ShoppingList>? shoppingLists { get; set; }
        IDataStorage? dataStorage { get; set; }
    }

}
