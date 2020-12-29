using Microsoft.EntityFrameworkCore;

namespace StoreApp
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Customer> customers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<StoreLocation> locations { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }
        public StoreDbContext(){}
        public StoreDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if(!options.IsConfigured)
            {
                options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Targmarttest;Trusted_Connection=True;");
            }
            
        }
        
    }
}