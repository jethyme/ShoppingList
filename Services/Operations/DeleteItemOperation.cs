using Core.Interfaces;
using Core.Models;

namespace Services.Operations
{
    public class DeleteItemOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;
        private readonly string _itemName;

        public DeleteItemOperation(IShoppingListService service, string listName, string itemName)
        {
            _service = service;
            _listName = listName;
            _itemName = itemName;
        }

        public async Task ExecuteAsync()
        {
            var list = _service.ShoppingLists.FirstOrDefault(l => l.Name == _listName);
            var item = list?.Items.FirstOrDefault(i => i.Name == _itemName);
            if (item != null)
            {
                bool v = list.Items.Remove(item);
                await _service.DataStorage.SaveDataAsync(_service.ShoppingLists);
            }
        }

    }

}
