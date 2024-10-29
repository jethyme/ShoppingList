using Core.Interfaces;
using Core.Models;

namespace Services.Operations
{
    public class DeleteListOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;

        public DeleteListOperation(IShoppingListService service, string listName)
        {
            _service = service;
            _listName = listName;
        }

        public async Task ExecuteAsync()
        {
            var list = _service.ShoppingLists.FirstOrDefault(l => l.Name == _listName);
            if (list != null)
            {
                bool removed = _service.ShoppingLists.Remove(list);
                if (removed)
                {
                    await _service.DataStorage.SaveDataAsync(_service.ShoppingLists);
                }
            }
        }

    }

}
