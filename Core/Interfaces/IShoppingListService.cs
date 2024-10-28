using Core.Models;

namespace Core.Interfaces
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync();
        Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName);
        Task<bool> CheckIfListNameExistsAsync(string name);

        List<ShoppingList>? ShoppingLists { get; set; }
        IDataStorage? DataStorage { get; set; }
    }

}
