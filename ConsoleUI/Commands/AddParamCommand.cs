using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ConsoleUI.Commands
{
    [ExcludeFromCodeCoverage]
    public class AddParamCommand : IMenuItem
    {
        public string Name { get; }
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public AddParamCommand(IShoppingListService service, string name, ShoppingList list)
        {
            Name = name;
            _service = service;
            _list = list;
        }
        public async Task ExecuteAsync()
        {
            Console.Write("Введите номер товара для добавления параметров: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                await AddParamAsync(_service, _list.Name, item);
                Console.WriteLine($"Товар \"{item.Name}\" просмотрен.");
            }
            else
            {
                await ConsoleUserInterface.ShowMessageAsync("Неверный выбор.", ConsoleColor.Red);
            }
        }

        private static async Task AddParamAsync(IShoppingListService service, string listName, ShoppingItem item)
        {
            
            var parameters = await ViewShoppingListsCommand.ViewParamAsync(listName, item);

            var addParam = new AddParamToItemCommand(service, listName, item);
            await addParam.ExecuteAsync();

            var updateOperation = new UpdateItemInListOperation(service, listName, item);
            await updateOperation.ExecuteAsync();

            
        }
    }
}
