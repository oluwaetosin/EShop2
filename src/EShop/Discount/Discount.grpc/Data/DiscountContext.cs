using Discount.grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;

        public DiscountContext(DbContextOptions<DiscountContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, ProductName = "IPhoneX", Description = "IPhone Discount", Amount = 150},
                 new Coupon { Id = 2, ProductName = "IPhoneX-23", Description = "IPhone Discount-23", Amount = 100 }
                );
        }
    }
}
