namespace Core.Models
{
    public class ShoppingItem
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public bool IsPurchased { get; set; }
        public DateTime? PurchaseTime { get; set; }

        public void AddOrUpdateParameter(string key, string value)
        {
            Parameters[key] = value;
        }
    }

}
