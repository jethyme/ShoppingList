using Core.Interfaces;

namespace Services
{
    public class MarkItemAsPurchasedOperation : IOperation
    {
        private readonly IShoppingListService _service;

        public MarkItemAsPurchasedOperation(IShoppingListService service)
        {
            _service = service;
        }

        public async Task ExecuteAsync()
        {
            Console.Write("Введите название списка покупок: ");
            var listName = Console.ReadLine();

            var list = await _service.GetAllShoppingListsAsync();
            var selectedList = list.FirstOrDefault(l => l.Name == listName);
            if (selectedList == null)
            {
                Console.WriteLine("Список не найден.");
                return;
            }

            Console.WriteLine($"Список покупок \"{listName}\":");
            int index = 1;
            foreach (var item in selectedList.Items)
            {
                var status = item.IsPurchased ? "(куплен)" : "";
                Console.WriteLine($"{index}. {item.Name}, {item.Quantity} {status}");
                index++;
            }

            Console.Write("Введите номер товара для отметки как купленный: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= selectedList.Items.Count)
            {
                var item = selectedList.Items.ElementAt(itemIndex - 1);
                await _service.MarkItemAsPurchasedAsync(listName, item.Name);
                Console.WriteLine($"Товар \"{item.Name}\" отмечен как купленный.");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }
    }

}
