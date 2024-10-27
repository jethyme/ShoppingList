using Core.Interfaces;
using Core.Models;
using System.Xml.Linq;

namespace Services.Operations
{
    public class CreateShoppingListOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly string _listName;

        public CreateShoppingListOperation(IShoppingListService service, string listName)
        {
            _service = service;
            _listName = listName;
        }

        public async Task ExecuteAsync()
        {
            _service.shoppingLists.Add(new ShoppingList { Name = _listName });
            await _service.dataStorage.SaveDataAsync(_service.shoppingLists);
        }

    }

}
