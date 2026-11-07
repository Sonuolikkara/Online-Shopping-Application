using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        // Get Products
        public async Task<IEnumerable<Product>> GetProducts()
        {
            // Two caching methods

            //return await GetProductsUsingMemoryCache();

             return await GetProductsUsingRedisCache();
        }

        // IN-MEMORY CACHE
        private async Task<IEnumerable<Product>> GetProductsUsingMemoryCache()
        {
            const string cacheKey = "products";

            if (_cache.TryGetValue(
                cacheKey,
                out IEnumerable<Product>? cachedProducts))
            {
                Console.WriteLine("MEMORY CACHE HIT");

                return cachedProducts!;
            }

            Console.WriteLine("MEMORY CACHE MISS - DATABASE");

            var products = await _context.Products
                .ToListAsync();

            //Expiry of Cache
            var cacheOptions =
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                };
            //Storing the products in the cache
            _cache.Set(
                cacheKey,
                products,
                cacheOptions);

            return products;
        }


        // DISTRIBUTED CACHE / REDIS
        private async Task<IEnumerable<Product>> GetProductsUsingRedisCache()
        {
            const string cacheKey = "products";

            var cachedProducts =
                await _distributedCache.GetStringAsync(cacheKey);

            if (cachedProducts != null)
            {
                Console.WriteLine("REDIS CACHE HIT");

                //Converting to C# objects
                return JsonSerializer.Deserialize<IEnumerable<Product>>(
                           cachedProducts)
                       ?? Enumerable.Empty<Product>();
            }

            Console.WriteLine("REDIS CACHE MISS - DATABASE");

            var products = await _context.Products
                .ToListAsync();

            var serializedProducts =
                JsonSerializer.Serialize(products);
            //Expiry of Cache
            var cacheOptions =
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(5)
                };
            //Storing the products in the cache
            await _distributedCache.SetStringAsync(
                cacheKey,
                serializedProducts,
                cacheOptions);

            return products;
        }


        // ==========================================
        // GET PRODUCT BY ID
        // ==========================================
        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
                .FindAsync(id);
        }
    }
}