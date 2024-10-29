using Core.Interfaces;
using Core.Models;
using Services.Operations;

namespace ConsoleUI.Commands
{
    public class CreateShoppingListCommand : IMenuItem
    {
        public string Name { get; }
        private readonly IShoppingListService _service;

        public CreateShoppingListCommand(IShoppingListService service, string name)
        {
            Name = name;
            _service = service;
        }

        public async Task ExecuteAsync()
        {
            Console.Clear();
            var name = string.Empty;

            while (true)
            {
                Console.Write("Введите название нового списка покупок: ");
                name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    await ConsoleUserInterface.ShowMessageAsync("Название списка не может быть пустым. Пожалуйста, введите снова.", ConsoleColor.Red);
                    continue;
                }
                if (await _service.CheckIfListNameExistsAsync(name))
                {
                    await ConsoleUserInterface.ShowMessageAsync("Список с таким названием уже существует. Пожалуйста, введите другое название.", ConsoleColor.Red);
                    continue;
                }
                break;
            }

            var createOperation = new CreateShoppingListOperation(_service, name);
            await createOperation.ExecuteAsync();

            var addItem = new AddItemToListCommand(_service, "", name);
            await addItem.ExecuteAsync();
        }
    }
}
