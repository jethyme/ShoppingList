using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ConsoleUI.Commands
{
    [ExcludeFromCodeCoverage]
    public class AddItemToListCommand : IMenuItem
    {
        public string Name { get; }
        private readonly IShoppingListService _service;
        private readonly string _listName;

        public AddItemToListCommand(IShoppingListService service, string name, string listName)
        {
            _service = service;
            _listName = listName;
            Name = name;
        }

        public async Task ExecuteAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добавьте товар в список покупок {_listName} (для завершения нажмите Enter):");
                Console.Write($"{_listName}: ");
                var input = Console.ReadLine();
                if (input.Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    await ConsoleUserInterface.ShowMessageAsync($"Добавление товаров в список \"{_listName}\" закончено", ConsoleColor.Green);
                    return;
                }
                if (await _service.CheckIfItemNameExistsAsync(_listName, input))
                {
                    await ConsoleUserInterface.ShowMessageAsync($"Товар с названием \"{input}\" уже существует. Пожалуйста, введите другое название.", ConsoleColor.Red);
                    continue;
                }               

                var parts = input.Split(',');
                var item = new ShoppingItem
                {
                    Name = parts[0].Trim(),
                    Quantity = parts.Length > 1 ? parts[1].Trim() : string.Empty
                };

                var addParam = new AddParamToItemCommand(_service, _listName, item);
                await addParam.ExecuteAsync();

                var addOperation = new AddItemToListOperation(_service, _listName, item);
                await addOperation.ExecuteAsync();
            }
        }
    }
}
