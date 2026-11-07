using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService : IProductService
    {
        private readonly ShoppingCartDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedCache;

        public ProductService(
            ShoppingCartDbContext context,
            IMemoryCache cache,
            IDistributedCache distributedCache)
        {
            _context = context;
            _cache = cache;
            _distributedCache = distributedCache;
        }

        private async Task InvalidateProductCacheAsync()
        {
            _cache.Remove("products");
            await _distributedCache.RemoveAsync("products");
        }


    }
}