using Core.Interfaces;
using Core.Models;
using Services.Operations;

namespace ConsoleUI.Commands
{
    public class MarkPurchaseCommand : IMenuItem
    {
        public string Name { get; }
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public MarkPurchaseCommand(IShoppingListService service, string name, ShoppingList list)
        {
            Name = name;
            _service = service;
            _list = list;
        }

        public async Task ExecuteAsync()
        {
            Console.Write("Введите номер товара для отметки купленным: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                var markPurchaseOperation = new MarkPurchaseOperation(_service, _list.Name, item.Name);
                await markPurchaseOperation.ExecuteAsync();
                Console.WriteLine($"Товар \"{item.Name}\" отмечен купленным.");
            }
            else
            {
                ConsoleUserInterface.ShowMessage("Неверный выбор.", ConsoleColor.Red);
            }
        }
    }
}
