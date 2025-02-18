﻿using Core.Interfaces;
using Core.Models;
using Services.Operations;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleUI.Commands
{
    [ExcludeFromCodeCoverage]
    public class AddParamToItemCommand : IMenuItem
    {
        public string Name { get; }
        private readonly IShoppingListService _service;
        private readonly ShoppingItem _item;

        public AddParamToItemCommand(IShoppingListService service, string name, ShoppingItem item)
        {
            Name = name;
            _service = service;
            _item = item;
        }

        public async Task ExecuteAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Добавьте параметр для товара {_item.Name} (формат: ключ=значение, для завершения нажмите Enter):");
                Console.Write($"{_item.Name}: ");
                var paramInput = Console.ReadLine();
                if (paramInput.Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    await ConsoleUserInterface.ShowMessageAsync($"Добавление параметров для товара \"{_item.Name}\" закончено", ConsoleColor.Green);
                    return;
                }

                var paramParts = paramInput.Split('=');
                if (paramParts.Length == 2)
                {
                    var key = paramParts[0].Trim();
                    var value = paramParts[1].Trim();
                    _item.AddOrUpdateParameter(key, value);
                }
                else
                {
                    await ConsoleUserInterface.ShowMessageAsync("Неверный формат. Используйте формат: ключ=значение.", ConsoleColor.Red);
                }
            }
        }
    }
}
