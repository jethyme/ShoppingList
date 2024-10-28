using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Collections.Generic;

namespace ConsoleUI.Commands
{
    public class UpdateListCommand : IMenuItem
    {
        public string Name { get; set; }
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public UpdateListCommand(IShoppingListService service, string name, ShoppingList list)
        {
            Name = name;
            _service = service;
            _list = list;
        }

        public async Task ExecuteAsync()
        {
            Console.Write("Введите номер товара для изменения: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                Console.Write($"Введите новое количество для \"{item.Name}\": ");
                var newQuantity = Console.ReadLine();
                item.Quantity = newQuantity;
                var updateItemInListOperation = new UpdateItemInListOperation(_service, _list.Name, item);
                await updateItemInListOperation.ExecuteAsync();
                Console.WriteLine($"Товар \"{item.Name}\" обновлен.");
            }
            else
            {
                await ConsoleUserInterface.ShowMessage("Неверный выбор.", ConsoleColor.Red);
            }
        }
    }
}
