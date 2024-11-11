using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBData.Models
{
    public class ShoppingItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public bool IsPurchased { get; set; }
        public DateTime? PurchaseTime { get; set; }
        public int ShoppingListId { get; set; } // Foreign key
        public ShoppingList ShoppingList { get; set; } // Navigation property
        public List<ShoppingItemParameter> Parameters { get; set; } = new List<ShoppingItemParameter>(); // Navigation property for parameters
    }
}