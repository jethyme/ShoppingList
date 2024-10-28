﻿using ConsoleUI;
using Core.Interfaces;
using JsonData;
using Services;

namespace ShoppingList
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
