using EXE201.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Data
{
    

    public class FurnitureStoreDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public FurnitureStoreDbContext()
        {
        }
        public FurnitureStoreDbContext(DbContextOptions<FurnitureStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Furniture> Furnitures { get; set; }
        public DbSet<FurnitureSizeConfig> FurnitureSizeConfigs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingDetail> ShippingDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int> { Id = 1, Name = "ADMIN", NormalizedName = "Admin" },
            new IdentityRole<int> { Id = 2, Name = "CUSTOMER", NormalizedName = "Customer" },
            new IdentityRole<int> { Id = 3, Name = "STAFF", NormalizedName = "Staff" }
            );
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>()
            //    .Property(u => u.FullName)
            //    .HasMaxLength(100)
            //    .IsRequired();

            //// FurnitureSizeConfig one-to-one with Furniture
            //modelBuilder.Entity<FurnitureSizeConfig>()
            //    .HasOne(f => f.Furniture)
            //    .WithOne(f => f.SizeConfig)
            //    .HasForeignKey<FurnitureSizeConfig>(f => f.FurnitureId);

            //// Order status constraint
            //modelBuilder.Entity<Order>()
            //    .HasCheckConstraint("CK_Orders_Status", "[Status] IN ('Pending', 'Confirmed', 'Shipped', 'Delivered', 'Cancelled')");

            //modelBuilder.Entity<OrderItem>()
            //    .HasCheckConstraint("CK_OrderItems_Quantity", "[Quantity] > 0");

            //modelBuilder.Entity<Review>()
            //    .HasCheckConstraint("CK_Reviews_Rating", "[Rating] BETWEEN 1 AND 5");
        }
    }

}
