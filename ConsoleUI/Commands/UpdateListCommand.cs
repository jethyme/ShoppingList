using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Collections.Generic;
using System;
using System.Xml.Linq;

namespace ConsoleUI.Commands
{
    public class UpdateListCommand : IMenuItem
    {
        public string Name { get; }
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
            var back = new BackCommand("5. Вернуться в предыдущее меню", true);
            while (back.Back)
            {
                Console.Clear();
                Console.WriteLine($"Список покупок \"{_list.Name}\":");
                int index = 1;
                foreach (var item in _list.Items)
                {
                    var status = item.IsPurchased ? "(куплен)" : "";
                    Console.WriteLine($"{index}. {item.Name}, {item.Quantity} {status}");
                    index++;
                }

                var menu = new Dictionary<string, IMenuItem>();
                menu.Add("1", new AddItemToListCommand(_service, "1. Добавить товары", _list.Name));
                menu.Add("2", new DeleteItemFromListCommand(_service, "2. Удалить товары", _list));
                menu.Add("3", new AddParamCommand(_service, "3. Добавить параметры товару", _list));
                menu.Add("4", new DeleteParamCommand(_service, "4. Удалить параметры товара", _list));
                menu.Add("5", back);
                await ConsoleUserInterface.ShowMenu(menu);
            }
        }

    }
}
