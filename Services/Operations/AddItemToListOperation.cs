using Core.Interfaces;
using Core.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Services.Operations
{ 

    public class AddItemToListOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;
        private readonly ShoppingItem _item;

        public AddItemToListOperation(IShoppingListService service, string listName, ShoppingItem item)
        {
            _service = service;
            _listName = listName;
            _item = item;
        }


        public async Task ExecuteAsync()
        {
            var list = _service.shoppingLists.FirstOrDefault(l => l.Name == _listName);
            if (list != null)
            {
                list.Items.Add(_item);
                await _service.dataStorage.SaveDataAsync(_service.shoppingLists);
            }
        }

    }
}
