using Microsoft.EntityFrameworkCore;
using DBData.Models;

namespace DBData.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<ShoppingItemParameter> ShoppingItemParameters { get; set; } // Добавлено

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=ShoppingListDb;Username=your_username;Password=your_password");
        }
    }
}