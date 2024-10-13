using Core.Models;

namespace Core.Interfaces
{
    public interface IDataStorage
    {
        Task SaveShoppingListAsync(ShoppingList list);
        Task<ShoppingList> LoadShoppingListAsync(string name);
        Task<IEnumerable<ShoppingList>> LoadAllShoppingListsAsync();
    }

}
