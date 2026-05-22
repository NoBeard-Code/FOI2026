using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public DbSet<DeliveryNoteItem> DeliveryNoteItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderItem>().HasKey(x => new { x.OrderId, x.ArticleId });
            builder.Entity<DeliveryNoteItem>().HasKey(y => new { y.DeliveryNoteId, y.ArticleId });


            builder.Entity<DeliveryNoteItem>().HasOne(x => x.Article).WithMany(x => x.DeliveryNoteItems).HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderItem>().HasOne(x => x.Article).WithMany(x => x.OrderItems).HasForeignKey(x => x.ArticleId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
