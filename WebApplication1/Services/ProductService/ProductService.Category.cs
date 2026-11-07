using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task<bool> HasProductsByCategoryId(int categoryId)
        {
            return await _context.Products
                .AnyAsync(p => p.CategoryID == categoryId);
        }
    }
}