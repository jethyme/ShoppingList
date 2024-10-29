using Core.Interfaces;
using Core.Models;
using Services.Operations;

namespace ConsoleUI.Commands
{
    public class DeleteListsCommand : IMenuItem
    {
        public string Name { get; }
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
                await ConsoleUserInterface.ShowMessageAsync("Нет текущих списков покупок.", ConsoleColor.Red);
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

                Console.Write("Выберите список покупок для удаления (Enter - вернуться назад): ");
                var choice = Console.ReadLine();
                if (int.TryParse(choice, out int listIndex) && listIndex > 0 && listIndex <= lists.Count())
                {
                    var selectedList = lists.ElementAt(listIndex - 1);
                    var listName = selectedList.Name;
                    var deleteListOperation = new DeleteListOperation(_service, listName);
                    await deleteListOperation.ExecuteAsync();
                    await ConsoleUserInterface.ShowMessageAsync($"Список \"{listName}\" удалён.", ConsoleColor.Green);
                    return;
                }
                else
                {
                    if (choice.Equals("", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.Clear();
                        return;
                    }
                    await ConsoleUserInterface.ShowMessageAsync("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
                }
            }
        }
    }
}
