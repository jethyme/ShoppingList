using ConsoleUI;
using Core.Interfaces;
using JSONData;
using Services;

namespace Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IDataStorage dataStorage = new JsonDataStorage();
            IShoppingListService shoppingListService = new ShoppingListService(dataStorage);
            IUserInterface userInterface = new ConsoleUserInterface(shoppingListService);

            await userInterface.RunAsync();
        }
    }
}
