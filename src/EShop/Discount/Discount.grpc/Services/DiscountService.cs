using Discount.grpc.Data;
using Discount.grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext
               .Coupons
               .FirstOrDefaultAsync(x => x.ProductName.ToLower() == request.ProductName.ToLower());

            if (coupon is null)
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };

            }

            logger.LogInformation("Discount is retrieved for Product Name: {ProductName}", coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }
        public override  async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();

            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }

            dbContext.Add(coupon);
            await dbContext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {


            var coupon = await  dbContext.Coupons.Where(x => x.ProductName == request.Coupon.ProductName).FirstOrDefaultAsync();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }

            coupon.ProductName = request.Coupon.ProductName;
            coupon.Amount = request.Coupon.Amount;
            coupon.Description = request.Coupon.Description;

            await dbContext.SaveChangesAsync();

            return coupon.Adapt<CouponModel>();
        }

        public override  async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.Where(x => x.ProductName == request.ProductName).FirstOrDefaultAsync();

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
            }

             dbContext.Remove(coupon);

            await dbContext.SaveChangesAsync();

            return new DeleteDiscountResponse { Success = true };

        }
    }
}
