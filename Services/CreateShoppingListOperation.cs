using Core.Interfaces;
using Core.Models;

namespace Services
{
    public class CreateShoppingListOperation : IOperation
    {
        private readonly IShoppingListService _service;

        public CreateShoppingListOperation(IShoppingListService service)
        {
            _service = service;
        }

        public async Task ExecuteAsync()
        {
            Console.Write("Введите название нового списка покупок: ");
            var name = Console.ReadLine();
            await _service.CreateShoppingListAsync(name);

            Console.WriteLine("Добавьте товары в список покупок (для завершения введите 'готово'):");
            while (true)
            {
                Console.Write($"{name}: ");
                var input = Console.ReadLine();
                if (input.ToLower() == "готово") break;

                var parts = input.Split(',');
                var item = new ShoppingItem
                {
                    Name = parts[0].Trim(),
                    Quantity = parts.Length > 1 ? parts[1].Trim() : string.Empty
                };
                await _service.AddItemToListAsync(name, item);
            }
        }
    }


}
