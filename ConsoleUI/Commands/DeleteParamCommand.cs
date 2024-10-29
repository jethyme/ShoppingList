using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Xml.Linq;

namespace ConsoleUI.Commands
{
    public class DeleteParamCommand : IMenuItem
    {
        public string Name { get; set; }
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public DeleteParamCommand(IShoppingListService service, string name, ShoppingList list)
        {
            Name = name;
            _service = service;
            _list = list;
        }
        public async Task ExecuteAsync()
        {
            Console.Write("Введите номер товара для просмотра параметров: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                await DeleteParamAsync(_service, _list.Name, item);
                Console.WriteLine($"Товар \"{item.Name}\" просмотрен.");
            }
            else
            {
                ConsoleUserInterface.ShowMessage("Неверный выбор.", ConsoleColor.Red);
            }
        }

        private static async Task DeleteParamAsync(IShoppingListService service, string listName, ShoppingItem item)
        {
            while (true)
            {
                var parameters = ViewShoppingListsCommand.ViewParam(listName, item);
                Console.Write("Выберите параметр для удаления, введите номер (Enter, чтобы закончить): ");
                var choice = Console.ReadLine();
                if (choice.Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    ConsoleUserInterface.ShowMessage("Удаление закончено.", ConsoleColor.Green);
                    return;
                }
                if (parameters.TryGetValue(choice, out string? parKey))
                {
                    var deleteParamOperation = new DeleteParamOperation(service, listName, item, parKey);
                    await deleteParamOperation.ExecuteAsync();
                }
                else
                {
                    ConsoleUserInterface.ShowMessage("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
                }
            }
        }
    }
}
