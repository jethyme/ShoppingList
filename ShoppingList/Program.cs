//using ConsoleUI;
//using Core.Interfaces;
//using JsonData;
//using Services;

//namespace ShoppingLists
//{
//    class Program
//    {
//        static async Task Main(string[] args)
//        {
//            IDataStorage dataStorage = new JsonDataStorage();
//            IShoppingListService shoppingListService = new ShoppingListService(dataStorage);
//            IUserInterface userInterface = new ConsoleUserInterface(shoppingListService);

//            await userInterface.RunAsync();
//        }
//    }


//}
using ConsoleUI;
using Core.Interfaces;
using DBData;
using DBData.Data;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Diagnostics.CodeAnalysis;

namespace ShoppingLists
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "Host=localhost;Database=ShoppingListDb;Username=your_username;Password=your_password";
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            IDataStorage dataStorage = new DbDataStorage(new AppDbContext(optionsBuilder.Options));
            IShoppingListService shoppingListService = new ShoppingListService(dataStorage);
            IUserInterface userInterface = new ConsoleUserInterface(shoppingListService);

            await userInterface.RunAsync();
        }
    }
}