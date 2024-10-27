using Core.Interfaces;
using Core.Models;

namespace Services.Operations
{
    public class UpdateItemInListOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;
        private readonly ShoppingItem _item;

        public UpdateItemInListOperation(IShoppingListService service, string listName, ShoppingItem item)
        {
            _service = service;
            _listName = listName;
            _item = item;
        }

        public async Task ExecuteAsync()
        {
            var list = _service.shoppingLists.FirstOrDefault(l => l.Name == _listName);
            var existingItem = list?.Items.FirstOrDefault(i => i.Name == _item.Name);
            if (existingItem != null)
            {
                existingItem.Quantity = _item.Quantity;
                await _service.dataStorage.SaveDataAsync(_service.shoppingLists);
            }
        }
    }
}
