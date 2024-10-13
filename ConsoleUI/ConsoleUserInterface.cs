using Core.Models;
using Core.Interfaces;

namespace ConsoleUI
{
    public class ConsoleUserInterface : IUserInterface
    {
        private readonly IShoppingListService _service;

        public ConsoleUserInterface(IShoppingListService service)
        {
            _service = service;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Добро пожаловать в программу для создания и управления списками покупок!");

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Создать новый список покупок");
                Console.WriteLine("2. Просмотреть список текущих списков покупок");
                Console.WriteLine("3. Выйти из программы");
                Console.Write("Выберите действие (1-3): ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateNewShoppingListAsync();
                        break;
                    case "2":
                        await ViewAndManageShoppingListsAsync();
                        break;
                    case "3":
                        Console.WriteLine("Спасибо за использование программы управления списками покупок! Удачных покупок!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        private async Task CreateNewShoppingListAsync()
        {
            Console.Write("Введите название нового списка покупок: ");
            var name = Console.ReadLine();
            await _service.CreateShoppingListAsync(name);

            Console.WriteLine("Добавьте товары в список покупок (для завершения введите 'готово'):");
            while (true)
            {
                Console.Write($"{name}: ");
                var input = Console.ReadLine();
                if (input.ToLower() == "готово") break;

                var parts = input.Split(',');
                var item = new ShoppingItem
                {
                    Name = parts[0].Trim(),
                    Quantity = parts.Length > 1 ? parts[1].Trim() : string.Empty
                };
                await _service.AddItemToListAsync(name, item);
            }
        }

        private async Task ViewAndManageShoppingListsAsync()
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
                await ManageShoppingListAsync(selectedList);
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        private async Task ManageShoppingListAsync(ShoppingList list)
        {
            while (true)
            {
                Console.WriteLine($"\nСписок покупок \"{list.Name}\":");
                int index = 1;
                foreach (var item in list.Items)
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
                        await MarkPurchaseAsync(list.Name);
                        break;
                    case "2":
                        Console.WriteLine("Функция изменения списка пока не реализована.");
                        break;
                    case "3":
                        await ViewPurchaseHistoryAsync(list.Name);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        private async Task MarkPurchaseAsync(string listName)
        {
            Console.Write("Введите номер товара для отметки как купленный: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex))
            {
                var list = await _service.GetAllShoppingListsAsync();
                var selectedList = list.FirstOrDefault(l => l.Name == listName);
                if (selectedList != null && itemIndex > 0 && itemIndex <= selectedList.Items.Count)
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

        private async Task ViewPurchaseHistoryAsync(string listName)
        {
            var history = await _service.GetHistoryAsync(listName);
            Console.WriteLine($"История покупок для списка \"{listName}\":");
            foreach (var item in history)
            {
                Console.WriteLine($"- {item.Name}, {item.Quantity}");
            }
        }
    }

}
