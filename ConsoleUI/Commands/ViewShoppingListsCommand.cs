using Core.Interfaces;
using Core.Models;
using Services.Operations;

namespace ConsoleUI.Commands
{
    public class ViewShoppingListsCommand : IMenuItem
    {
        public string Name { get; set; }
        private readonly IShoppingListService _service;

        public ViewShoppingListsCommand(IShoppingListService service, string name)
        {
            Name = name;
            _service = service;
        }
        public async Task ExecuteAsync()
        {
            var lists = await _service.GetAllShoppingListsAsync();
            if (!lists.Any())
            {
                ConsoleUserInterface.ShowMessage("Нет текущих списков покупок.", ConsoleColor.Red);
                return;
            }
            while (true)
            {
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
                    await ManageShoppingListAsync(_service, selectedList);
                    return;
                }
                else
                {
                    ConsoleUserInterface.ShowMessage("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
                }
            }
        }

        private static async Task ManageShoppingListAsync(IShoppingListService service, ShoppingList list)
        {
            var back = new BackCommand("4. Вернуться в предыдущее меню", true);
            while (back.Back)
            {
                Console.Clear();
                Console.WriteLine($"Список покупок \"{list.Name}\":");
                int index = 1;
                foreach (var item in list.Items)
                {
                    var status = item.IsPurchased ? "(куплен)" : "";
                    Console.WriteLine($"{index}. {item.Name}, {item.Quantity} {status}");
                    index++;
                }

                var menu = new Dictionary<string, IMenuItem>();
                menu.Add("1", new MarkPurchaseCommand(service, "1. Отметить покупку", list));
                menu.Add("2", new UpdateListCommand(service, "2. Изменить список покупок", list));
                menu.Add("3", new ViewPurchaseHistoryCommand(service, "3. Просмотреть историю покупок", list));
                menu.Add("4", back);
                await ConsoleUserInterface.ShowMenu(menu);
            }
        }

        public static Dictionary<string, string> ViewParam(IShoppingListService service, string listName, ShoppingItem item)
        {
            var parameters = new Dictionary<string, string>();
            var list = item.Parameters;
            if (!list.Any())
            {
                ConsoleUserInterface.ShowMessage($"У товара \"{item.Name}\" нет параметров.", ConsoleColor.Red);
                return parameters;
            }
            Console.Clear();
            Console.WriteLine($"Параметры товара \"{item.Name}\":");
            
            int index = 1;
            foreach (var par in list)
            {
                parameters[index.ToString()] = par.Key;
                Console.WriteLine($"{index}. {par.Key} = {par.Value}");
                index++;
            }
            return parameters;
        }
    }
}
