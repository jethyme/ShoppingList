using System.ComponentModel.DataAnnotations;

namespace DBData.Models
{
    public class ShoppingItemParameter
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ShoppingItemId { get; set; } 
        public ShoppingItem ShoppingItem { get; set; } 
    }
}