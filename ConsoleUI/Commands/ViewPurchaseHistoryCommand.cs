using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Collections.Generic;

namespace ConsoleUI.Commands
{
    public class ViewPurchaseHistoryCommand : IMenuItem
    {
        public string Name { get; set; }
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public ViewPurchaseHistoryCommand(IShoppingListService service, string name, ShoppingList list)
        {
            Name = name;
            _service = service;
            _list = list;
        }

        public async Task ExecuteAsync()
        {
            var purchasedItems = await _service.GetPurchasedItemsAsync(_list.Name);
            Console.Clear();
            Console.WriteLine($"История покупок для списка \"{_list.Name}\":");
            foreach (var item in purchasedItems)
            {
                Console.WriteLine($"- {item.Name}, {item.Quantity} (отмечен как купленный)");
            }
            Console.WriteLine("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }
    }
}
