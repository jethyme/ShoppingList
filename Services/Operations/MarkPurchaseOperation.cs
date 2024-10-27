using Core.Interfaces;
using Core.Models;

namespace Services.Operations
{
    public class MarkPurchaseOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;
        private readonly string _itemName;

        public MarkPurchaseOperation(IShoppingListService service, string listName, string itemName)
        {
            _service = service;
            _listName = listName;
            _itemName = itemName;
        }

        public async Task ExecuteAsync()
        {
            var list = _service.shoppingLists.FirstOrDefault(l => l.Name == _listName);
            var item = list?.Items.FirstOrDefault(i => i.Name == _itemName);
            if (item != null)
            {
                item.IsPurchased = true;
                //item.PurchaseTime = DateTime.Now;
                await _service.dataStorage.SaveDataAsync(_service.shoppingLists);
            }
        }

    }

}
