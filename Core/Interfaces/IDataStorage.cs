using Core.Models;

namespace Core.Interfaces
{
    public interface IDataStorage
    {
        Task SaveDataAsync(IEnumerable<ShoppingList> lists);
        Task<IEnumerable<ShoppingList>> LoadDataAsync();
    }

}
