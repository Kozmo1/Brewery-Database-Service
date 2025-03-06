using Brewery_DB_Service.Model;
using Microsoft.EntityFrameworkCore;

namespace Brewery_DB_Service.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Shipping> Shipping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().OwnsOne(u => u.TasteProfile);
            modelBuilder.Entity<Inventory>().OwnsOne(i => i.TasteProfile);
            modelBuilder.Entity<OrderItem>().HasKey(oi => new { oi.OrderId, oi.ProductId });
            modelBuilder.Entity<Cart>().HasKey(c => new { c.UserId, c.InventoryId });
        }
    }
}