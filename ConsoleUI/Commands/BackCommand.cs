using Core.Interfaces;
using Core.Models;
using Services.Operations;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleUI.Commands
{
    public class BackCommand : IMenuItem
    {
        public string Name { get; }
        public bool Back {  get; set; } 

        public BackCommand(string name, bool notBack)
        {
            Name = name;
            this.Back = notBack;
        }

        public async Task ExecuteAsync()
        {
            Console.Clear();
            Back = false;
        }
    }
}
