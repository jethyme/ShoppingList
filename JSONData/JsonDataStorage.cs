namespace JSONData
{
    using Core.Interfaces;
    using Core.Models;
    using System.Text.Json;

    public class JsonDataStorage : IDataStorage
    {
        private readonly string _filePath = "shoppingLists.json";

        public async Task SaveShoppingListAsync(ShoppingList list)
        {
            var lists = await LoadAllShoppingListsAsync();
            var updatedLists = lists.ToList();
            updatedLists.RemoveAll(l => l.Name == list.Name);
            updatedLists.Add(list);

            var json = JsonSerializer.Serialize(updatedLists);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<ShoppingList> LoadShoppingListAsync(string name)
        {
            var lists = await LoadAllShoppingListsAsync();
            return lists.FirstOrDefault(l => l.Name == name);
        }

        public async Task<IEnumerable<ShoppingList>> LoadAllShoppingListsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<ShoppingList>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<IEnumerable<ShoppingList>>(json) ?? new List<ShoppingList>();
        }
    }

}
