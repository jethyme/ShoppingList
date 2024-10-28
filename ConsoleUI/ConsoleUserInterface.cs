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

            IDictionary<string, IMenuItem> menu = new Dictionary<string, IMenuItem>(); ;
            menu.Add("1", new CreateShoppingListCommand(_service, "1. Создать новый список покупок"));
            menu.Add("2", new ViewShoppingListsCommand(_service, "2. Просмотреть список текущих списков покупок"));
            menu.Add("3", new ExitProgramCommand("3. Выйти из программы"));

            Console.Clear();
            while (true) await ShowMenu(menu);
        }

        public static async Task ShowMessage(string message, ConsoleColor color)
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

                

            if (menu.ContainsKey(choice))
            {
                await menu[choice].ExecuteAsync();
            }
            else
            {
                await ShowMessage("Неверный выбор. Попробуйте еще раз.", ConsoleColor.Red);
            }
                            
        }
    }
}
