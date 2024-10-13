namespace Core.Models
{
    public class ShoppingList
    {
        public string Name { get; set; }
        public List<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
    }

}
