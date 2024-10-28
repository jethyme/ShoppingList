using Core.Models;

namespace Core.Interfaces
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync();
        Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName);
        Task<bool> CheckIfListNameExistsAsync(string name);
        Task<bool> CheckIfItemNameExistsAsync(string listName, string input);

        List<ShoppingList>? ShoppingLists { get; set; }
        IDataStorage? DataStorage { get; set; }
    }

}
