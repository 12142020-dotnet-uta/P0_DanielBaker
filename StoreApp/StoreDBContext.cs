using Microsoft.EntityFrameworkCore;

namespace StoreApp
{
    public class StoreDBContext : DbContext
    {
        public DbSet<Customer> customers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<StoreLocation> locations { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Inventory> inventories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Targmarttest;Trusted_Connection=True;");
        }
        
    }
}