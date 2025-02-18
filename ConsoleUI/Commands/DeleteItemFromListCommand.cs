﻿using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ConsoleUI.Commands
{
    [ExcludeFromCodeCoverage]
    public class DeleteItemFromListCommand : IMenuItem
    {
        public string Name { get; }
        private readonly IShoppingListService _service;
        private readonly ShoppingList _list;

        public DeleteItemFromListCommand(IShoppingListService service, string name, ShoppingList list)
        {
            _service = service;
            Name = name;
            _list = list;
        }

        public async Task ExecuteAsync()
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

            Console.Write("Введите номер товара для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= _list.Items.Count)
            {
                var item = _list.Items.ElementAt(itemIndex - 1);
                var deleteItemOperation = new DeleteItemOperation(_service, _list.Name, item.Name);
                await deleteItemOperation.ExecuteAsync();
            }
            else
            {
                await ConsoleUserInterface.ShowMessageAsync("Неверный выбор.", ConsoleColor.Red);
            }
        }
    }
}
