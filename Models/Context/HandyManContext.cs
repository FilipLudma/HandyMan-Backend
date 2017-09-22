using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using WebAPI.Models;

namespace WebAPI.Schema.Context
{
    public class HandyManContext : DbContext
    {
        public DbSet<OrdersRecord> Orders { get; set; }
        public DbSet<OrderCategory> OrderCategory { get; set; }
        public DbSet<OrderSubCategory> OrderSubCategory { get; set; }
        public DbSet<OrderContactOption> OrderContactOption { get; set; }

        public HandyManContext(DbContextOptions<HandyManContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<OrdersRecord>()
                .HasKey(c => new { c.OrderGuid });
            
            //builder.Entity<OrdersRecord>().ToTable("Orders");
            builder.Entity<OrderCategory>().ToTable("OrderCategory");
            builder.Entity<OrderSubCategory>().ToTable("OrderSubCategory");
            //builder.Entity<OrderContactOption>().ToTable("OrderContactOption");
        }
    }
}