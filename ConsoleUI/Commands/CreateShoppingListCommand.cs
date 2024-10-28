using Core.Interfaces;
using Core.Models;
using Services.Operations;

namespace ConsoleUI.Commands
{
    public class CreateShoppingListCommand : IMenuItem
    {
        public string Name { get; set; }
        private readonly IShoppingListService _service;

        public CreateShoppingListCommand(IShoppingListService service, string name)
        {
            Name = name;
            _service = service;
        }

        public async Task ExecuteAsync()
        {
            Console.Clear();
            Console.Write("Введите название нового списка покупок: ");
            var name = Console.ReadLine();
            var createOperation = new CreateShoppingListOperation(_service, name);
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
                        await ConsoleUserInterface.ShowMessage("Неверный формат. Используйте формат: ключ=значение.", ConsoleColor.Red);
                    }
                }

                var addOperation = new AddItemToListOperation(_service, name, item);
                await addOperation.ExecuteAsync();
            }
        }
    }
}
