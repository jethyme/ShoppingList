using Core.Interfaces;

namespace Services
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
            var lists = await _service.GetAllShoppingListsAsync();
            if (!lists.Any())
            {
                Console.WriteLine("Нет текущих списков покупок.");
                return;
            }

            Console.WriteLine("Текущие списки покупок:");
            int index = 1;
            foreach (var list in lists)
            {
                Console.WriteLine($"{index}. {list.Name}");
                index++;
            }

            Console.Write("Выберите список покупок для управления (введите номер): ");
            if (int.TryParse(Console.ReadLine(), out int listIndex) && listIndex > 0 && listIndex <= lists.Count())
            {
                var selectedList = lists.ElementAt(listIndex - 1);
                var manageOperation = new ManageShoppingListOperation(_service, selectedList);
                await manageOperation.ExecuteAsync();
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }
    }


}
