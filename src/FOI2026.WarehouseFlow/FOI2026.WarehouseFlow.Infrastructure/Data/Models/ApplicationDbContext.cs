using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<ApplicationRole> Role { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Partner> Partner { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<DeliveryNote> DeliveryNote { get; set; }
        public DbSet<DeliveryNoteItem> DeliveryNoteItem { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderItem>().HasKey(x => new { x.OrderId, x.ItemId });
            builder.Entity<DeliveryNoteItem>().HasKey(y => new { y.DeliveryNoteId, y.ItemId });


            builder.Entity<DeliveryNoteItem>().HasOne(x => x.item).WithMany(x => x.deliveryNoteItems).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderItem>().HasOne(x => x.item).WithMany(x => x.orderItems).HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
