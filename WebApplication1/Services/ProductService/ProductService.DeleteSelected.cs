using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task<bool> DeleteSelectedProducts(IEnumerable<int> productIds)
        {
            var products = await _context.Products
                .Where(p => productIds.Contains(p.ProductID))
                .ToListAsync();

            if (products.Count == 0)
            {
                return false;
            }

            _context.Products.RemoveRange(products);

            await _context.SaveChangesAsync();

            await InvalidateProductCacheAsync();

            return true;
        }
    }
}