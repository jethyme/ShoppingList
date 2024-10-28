using Core.Models;
using Core.Interfaces;
using Services.Operations;

namespace ConsoleUI
{
    public class ConsoleUserInterface : IUserInterface
    {
        private readonly IDictionary<string, IOperation> _operations;
        private readonly IShoppingListService _service;

        public ConsoleUserInterface(IShoppingListService service)
        {
            _service = service;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Добро пожаловать в программу для создания и управления списками покупок!");
            Console.WriteLine("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Создать новый список покупок");
                Console.WriteLine("2. Просмотреть список текущих списков покупок");
                Console.WriteLine("3. Выйти из программы");
                Console.Write("Выберите действие (1-3): ");
                var choice = Console.ReadLine();

                if (choice == "3")
                {
                    Console.WriteLine("Спасибо за использование программы управления списками покупок! Удачных покупок!");
                    break;
                }
                switch (choice)
                {
                    case "1":
                        await CreateShoppingListAsync(_service);
                        break;
                    case "2":
                        await ViewShoppingListsAsync(_service);
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие от 1 до 3.");
                        break;
                }
            }
        }
        private static async Task CreateShoppingListAsync(IShoppingListService service)
        {
            Console.Clear();
            Console.Write("Введите название нового списка покупок: ");
            var name = Console.ReadLine();
            var createOperation = new CreateShoppingListOperation(service, name);
            await createOperation.ExecuteAsync();
            
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добавьте товар в список покупок {name} (для завершения нажмите Enter):");
                Console.Write($"{name}: ");
                var input = Console.ReadLine();
                if (input.ToLower() == "") break;

                var parts = input.Split(',');
                var item = new ShoppingItem
                {
                    Name = parts[0].Trim(),
                    Quantity = parts.Length > 1 ? parts[1].Trim() : string.Empty
                };

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Добавьте параметр для товара {item.Name} (формат: ключ=значение, для завершения нажмите Enter):");
                    Console.Write($"{item.Name}: ");
                    var paramInput = Console.ReadLine();
                    if (paramInput.ToLower() == "") break;

                    var paramParts = paramInput.Split('=');
                    if (paramParts.Length == 2)
                    {
                        var key = paramParts[0].Trim();
                        var value = paramParts[1].Trim();
                        item.AddOrUpdateParameter(key, value);
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат. Используйте формат: ключ=значение.");
                    }
                }

                var addOperation = new AddItemToListOperation(service, name, item);
                await addOperation.ExecuteAsync();
            }
        }
        private static async Task ViewShoppingListsAsync(IShoppingListService service)
        {
            var lists = await service.GetAllShoppingListsAsync();
            if (!lists.Any())
            {
                Console.WriteLine("Нет текущих списков покупок.");
                Console.WriteLine("Нажмите любую кнопку для продолжения...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
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
                await ManageShoppingListAsync(service, selectedList);
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }
        private static async Task ManageShoppingListAsync(IShoppingListService service, ShoppingList list)
        {
            while (true)
            {
                Console.Clear ();
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
                        await MarkPurchaseAsync(service, list);
                        break;
                    case "2":
                        await UpdateListAsync(service, list);
                        break;
                    case "3":
                        await ViewPurchaseHistoryAsync(service, list);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }
        private static async Task MarkPurchaseAsync(IShoppingListService service, ShoppingList list)
        {
            Console.Write("Введите номер товара для отметки как купленный: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= list.Items.Count)
            {
                var item = list.Items.ElementAt(itemIndex - 1);
                var markPurchaseOperation = new MarkPurchaseOperation(service, list.Name, item.Name);
                await markPurchaseOperation.ExecuteAsync();
                Console.WriteLine($"Товар \"{item.Name}\" отмечен как купленный.");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        private static async Task UpdateListAsync(IShoppingListService service, ShoppingList list)
        {
            Console.Write("Введите номер товара для изменения: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= list.Items.Count)
            {
                var item = list.Items.ElementAt(itemIndex - 1);
                Console.Write($"Введите новое количество для \"{item.Name}\": ");
                var newQuantity = Console.ReadLine();
                item.Quantity = newQuantity;
                var updateItemInListOperation = new UpdateItemInListOperation(service, list.Name, item);
                await updateItemInListOperation.ExecuteAsync();
                Console.WriteLine($"Товар \"{item.Name}\" обновлен.");
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        private static async Task ViewPurchaseHistoryAsync(IShoppingListService service, ShoppingList list)
        {
            var purchasedItems = await service.GetPurchasedItemsAsync(list.Name);
            Console.Clear();
            Console.WriteLine($"История покупок для списка \"{list.Name}\":");
            foreach (var item in purchasedItems)
            {
                Console.WriteLine($"- {item.Name}, {item.Quantity} (отмечен как купленный)");
            }
            Console.WriteLine("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }
    }
}
