using Core.Models;
using Core.Interfaces;
using Services.Operations;
using ConsoleUI.Commands;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleUI
{
    public class ConsoleUserInterface : IUserInterface
    {
        private readonly IShoppingListService _service;

        public ConsoleUserInterface(IShoppingListService service)
        {
            _service = service;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Добро пожаловать в программу для создания и управления списками покупок!");
            Console.WriteLine("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();

            var menu = new Dictionary<string, IMenuItem>(); ;
            menu.Add("1", new CreateShoppingListCommand(_service, "1. Создать новый список покупок"));
            menu.Add("2", new ViewShoppingListsCommand(_service, "2. Просмотреть список текущих списков покупок"));
            menu.Add("3", new DeleteListsCommand(_service, "3. Удалить список покупок"));
            menu.Add("4", new ExitProgramCommand("4. Выйти из программы"));

            Console.Clear();
            while (true) await ShowMenu(menu);
        }

        public static void ShowMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.WriteLine("Нажмите любую кнопку для продолжения...");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }

        public static async Task ShowMenu(IDictionary<string, IMenuItem> menu)
        {
            Console.WriteLine("Меню:");
            var index = 1;
            while (true)
            {
                if (menu.ContainsKey(index.ToString()))
                {
                    Console.WriteLine(menu[index.ToString()].Name);
                }
                else
                {
                    break;
                }
                index++;
            }
                
            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

                

            if (menu.TryGetValue(choice, out IMenuItem? value))
            {
                await value.ExecuteAsync();
            }
            else
            {
                ShowMessage("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
            }
                            
        }
        
    }
}
