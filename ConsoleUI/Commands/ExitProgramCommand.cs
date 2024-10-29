using Core.Interfaces;
using Core.Models;
using Services.Operations;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleUI.Commands
{
    public class ExitProgramCommand : IMenuItem
    {
        public string Name { get; }

        public ExitProgramCommand(string name)
        {
            Name = name;
        }

        public async Task ExecuteAsync()
        {
            Console.Clear();
            Console.WriteLine("Спасибо за использование программы управления списками покупок! Удачных покупок!");
            Environment.Exit(0);
        }
    }
}
