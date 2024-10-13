using Core.Interfaces;
using Core.Models;

namespace Services
{
    public class ManageShoppingListOperation : IOperation
    {
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public ManageShoppingListOperation(IShoppingListService service, ShoppingList list)
        {
            _service = service;
            _list = list;
        }

        public async Task ExecuteAsync()
        {
            while (true)
            {
                Console.WriteLine($"\nСписок покупок \"{_list.Name}\":");
                int index = 1;
                foreach (var item in _list.Items)
                {
                    var status = item.IsPurchased ? "(куплен)" : "";
                    Console.WriteLine($"{index}. {item.Name}, {item.Quantity} {status}");
                    index++;
                }

                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Отметить покупку");
                Console.WriteLine("2. Изменить список покупок");
                Console.WriteLine("3. Просмотреть историю покупок");
                Console.WriteLine("4. Вернуться в предыдущее меню");
                Console.Write("Выберите действие (1-4): ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await MarkPurchaseAsync();
                        break;
                    case "2":
                        await UpdateListAsync();
                        break;
                    case "3":
                        await ViewPurchaseHistoryAsync();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        private async Task MarkPurchaseAsync()
        {
            Console.Write("Введите номер товара для отметки как купленный: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                await _service.MarkItemAsPurchasedAsync(_list.Name, item.Name);
                Console.WriteLine($"Товар \"{item.Name}\" отмечен как купленный.");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        private async Task UpdateListAsync()
        {
            Console.Write("Введите номер товара для изменения: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                Console.Write($"Введите новое количество для \"{item.Name}\": ");
                var newQuantity = Console.ReadLine();
                item.Quantity = newQuantity;
                await _service.UpdateItemInListAsync(_list.Name, item);
                Console.WriteLine($"Товар \"{item.Name}\" обновлен.");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        private async Task ViewPurchaseHistoryAsync()
        {
            var purchasedItems = await _service.GetPurchasedItemsAsync(_list.Name);
            Console.WriteLine($"История покупок для списка \"{_list.Name}\":");
            foreach (var item in purchasedItems)
            {
                Console.WriteLine($"- {item.Name}, {item.Quantity} (отмечен как купленный)");
            }
        }
    }

}
