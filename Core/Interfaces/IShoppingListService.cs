using Core.Models;

namespace Core.Interfaces
{
    public interface IShoppingListService
    {
        Task CreateShoppingListAsync(string name);
        Task AddItemToListAsync(string listName, ShoppingItem item);
        Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync();
        Task MarkItemAsPurchasedAsync(string listName, string itemName);
        Task<IEnumerable<ShoppingItem>> GetHistoryAsync(string listName);
    }

}
