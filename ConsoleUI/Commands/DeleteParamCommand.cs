using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ConsoleUI.Commands
{
    [ExcludeFromCodeCoverage]
    public class DeleteParamCommand : IMenuItem
    {
        public string Name { get; }
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
                await ConsoleUserInterface.ShowMessageAsync("Неверный выбор.", ConsoleColor.Red);
            }
        }

        private static async Task DeleteParamAsync(IShoppingListService service, string listName, ShoppingItem item)
        {
            while (true)
            {
                var parameters = await ViewShoppingListsCommand.ViewParamAsync(listName, item);
                Console.Write("Выберите параметр для удаления, введите номер (Enter, чтобы закончить): ");
                var choice = Console.ReadLine();
                if (choice.Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    await ConsoleUserInterface.ShowMessageAsync("Удаление закончено.", ConsoleColor.Green);
                    return;
                }
                if (parameters.TryGetValue(choice, out string? parKey))
                {
                    var deleteParamOperation = new DeleteParamOperation(service, listName, item, parKey);
                    await deleteParamOperation.ExecuteAsync();
                }
                else
                {
                    await ConsoleUserInterface.ShowMessageAsync("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
                }
            }
        }
    }
}
