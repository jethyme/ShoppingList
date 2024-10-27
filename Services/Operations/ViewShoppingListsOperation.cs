using Core.Interfaces;

namespace Services.Operations
{
    public class ViewShoppingListsOperation : IOperation
    {
        private readonly IShoppingListService _service;

        public ViewShoppingListsOperation(IShoppingListService service)
        {
            _service = service;
        }

        public async Task ExecuteAsync()
        {

        }
    }


}
