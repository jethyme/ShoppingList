using Core.Models;
using Core.Interfaces;
using Services;

namespace ConsoleUI
{
    public class ConsoleUserInterface : IUserInterface
{
    private readonly IDictionary<string, IOperation> _operations;

    public ConsoleUserInterface(IShoppingListService service)
    {
        _operations = new Dictionary<string, IOperation>
        {
            { "1", new CreateShoppingListOperation(service) },
            { "2", new ViewShoppingListsOperation(service) }
        };
    }

    public async Task RunAsync()
    {
        Console.WriteLine("Добро пожаловать в программу для создания и управления списками покупок!");

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Создать новый список покупок");
            Console.WriteLine("2. Просмотреть список текущих списков покупок");
            Console.WriteLine("3. Выйти из программы");
            Console.Write("Выберите действие (1-3): ");
            var choice = Console.ReadLine();

            if (choice == "3")
            {
                Console.WriteLine("Спасибо за использование программы управления списками покупок! Удачных покупок!");
                break;
            }

            if (_operations.ContainsKey(choice))
            {
                await _operations[choice].ExecuteAsync();
            }
            else
            {
                Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
            }
        }
    }
}



}
