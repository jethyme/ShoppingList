using Core.Interfaces;
using Core.Models;

namespace Services.Operations
{
    public class DeleteParamOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;
        private readonly string _parKey;
        private readonly ShoppingItem _item;

        public DeleteParamOperation(IShoppingListService service, string listName, ShoppingItem item, string parKey)
        {
            _service = service;
            _listName = listName;
            _item = item;
            _parKey = parKey;
        }

        public async Task ExecuteAsync()
        {
            var list = _service.ShoppingLists.FirstOrDefault(l => l.Name == _listName);
            var existingItem = list?.Items.FirstOrDefault(i => i.Name == _item.Name);
            if (existingItem != null)
            {
                existingItem.Parameters.Remove(_parKey);
                await _service.DataStorage.SaveDataAsync(_service.ShoppingLists);
            }
        }
    }
}
