using Core.Interfaces;
using Core.Models;
using System.Text.Json;

namespace JSONData
{
    using System.Text.Json;

    public class JsonDataStorage : IDataStorage
    {
        private readonly string _filePath = "shopping_lists.json";

        public async Task SaveDataAsync(IEnumerable<ShoppingList> lists)
        {
            var jsonData = JsonSerializer.Serialize(lists, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, jsonData);
        }

        public async Task<IEnumerable<ShoppingList>> LoadDataAsync()
        {
            if (!File.Exists(_filePath)) return new List<ShoppingList>();

            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<ShoppingList>>(jsonData) ?? new List<ShoppingList>();
        }
    }

}
