using Microsoft.EntityFrameworkCore;

namespace Discount.grpc.Data

{
    public static class Extentions
    {
        public async static Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices .CreateScope();

            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            await dbContext.Database.MigrateAsync();

            return app;

        }
    }
}
