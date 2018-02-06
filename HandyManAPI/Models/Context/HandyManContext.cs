using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using HandyManAPI.Models;

namespace HandyManAPI.Schema.Context
{
    public class HandyManContext : DbContext
    {
        public DbSet<OrderRecord> Orders { get; set; }
        public DbSet<OrderCategory> OrderCategory { get; set; }
        public DbSet<OrderSubCategory> OrderSubCategory { get; set; }

        public DbSet<ImageModel> ImageModel { get; set; }
        public DbSet<UserRecord> Users { get; set; }
        
        public HandyManContext(DbContextOptions<HandyManContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<OrderRecord>()
                .HasKey(c => new { c.OrderGuid });

            builder.Entity<ImageModel>()
                .ToTable("ImageAttachments")
                .HasKey(c => new { c.ImageModelID});

            //builder.Entity<OrdersRecord>().ToTable("Orders");
            builder.Entity<OrderCategory>().ToTable("OrderCategory");
            builder.Entity<OrderSubCategory>().ToTable("OrderSubCategory");
            //builder.Entity<OrderContactOption>().ToTable("OrderContactOption");
        }
    }
}