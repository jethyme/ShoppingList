using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBData.Models
{
    public class ShoppingList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
    }
}