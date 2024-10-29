﻿using Core.Interfaces;
using Core.Models;
using Services.Operations;

namespace ConsoleUI.Commands
{
    public class DeleteListsCommand : IMenuItem
    {
        public string Name { get; set; }
        private readonly IShoppingListService _service;

        public DeleteListsCommand(IShoppingListService service, string name)
        {
            Name = name;
            _service = service;
        }
        public async Task ExecuteAsync()
        {
            var lists = await _service.GetAllShoppingListsAsync();
            if (!lists.Any())
            {
                ConsoleUserInterface.ShowMessage("Нет текущих списков покупок.", ConsoleColor.Red);
                return;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Текущие списки покупок:");
                int index = 1;
                foreach (var list in lists)
                {
                    Console.WriteLine($"{index}. {list.Name}");
                    index++;
                }

                Console.Write("Выберите список покупок для удаления (введите номер): ");
                if (int.TryParse(Console.ReadLine(), out int listIndex) && listIndex > 0 && listIndex <= lists.Count())
                {
                    var selectedList = lists.ElementAt(listIndex - 1);
                    var listName = selectedList.Name;
                    var deleteListOperation = new DeleteListOperation(_service, listName);
                    await deleteListOperation.ExecuteAsync();
                    ConsoleUserInterface.ShowMessage($"Список \"{listName}\" удалён.", ConsoleColor.Green);
                    return;
                }
                else
                {
                    ConsoleUserInterface.ShowMessage("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
                }
            }
        }
    }
}
