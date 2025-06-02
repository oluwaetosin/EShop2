
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string username, CancellationToken cancellation = default)
        {
           await repository.DeleteBasket(username, cancellation);
           await cache.RemoveAsync(username, cancellation);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string username, CancellationToken cancellation = default)
        {
            var cachedBasket = await cache.GetStringAsync(username, cancellation);

            if (!string.IsNullOrEmpty(cachedBasket)) {
               var deserializedBasket =  JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

                return deserializedBasket!; 
            }
            var basket = await repository.GetBasket(username, cancellation);

            await cache.SetStringAsync(username, JsonSerializer.Serialize(basket), cancellation);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellation = default)
        {
                await repository.StoreBasket(basket, cancellation);

                await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellation);

            return basket;
        }
    }
}
