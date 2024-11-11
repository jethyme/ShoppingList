using Microsoft.EntityFrameworkCore;
using DBData.Models;

namespace DBData.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }
        public DbSet<ShoppingItemParameter> ShoppingItemParameters { get; set; }

        public AppDbContext() : base(new DbContextOptionsBuilder<AppDbContext>()
           .UseNpgsql("Host=localhost;Database=ShoppingListDb;Username=your_username;Password=your_password")
           .Options)
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=ShoppingListDb;Username=your_username;Password=your_password");
            }
        }
    }
}