using Core.Models;

namespace Core.Interfaces
{
    public interface IShoppingListService
    {
        Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync();
        Task<IEnumerable<ShoppingItem>> GetPurchasedItemsAsync(string listName);

        List<ShoppingList>? shoppingLists { get; set; }
        IDataStorage? dataStorage { get; set; }
    }

}
