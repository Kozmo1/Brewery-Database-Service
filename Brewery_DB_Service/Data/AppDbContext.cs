using Brewery_DB_Service.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brewery_DB_Service.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Shipping> Shipping { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().OwnsOne(u => u.TasteProfile, tp =>
            {
                tp.Property(t => t.PrimaryFlavor).IsRequired(false);
                tp.Property(t => t.Sweetness).IsRequired(false);
                tp.Property(t => t.Bitterness).IsRequired(false);
            });
            modelBuilder.Entity<Inventory>().OwnsOne(i => i.TasteProfile);
            modelBuilder.Entity<OrderItem>()
                .HasOne<Order>()
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Cart>().HasKey(c => c.Id);

            modelBuilder.Entity<Inventory>().Property(i => i.Cost).HasPrecision(10, 2);
            modelBuilder.Entity<Inventory>().Property(i => i.Price).HasPrecision(10, 2);
        }
    }
}