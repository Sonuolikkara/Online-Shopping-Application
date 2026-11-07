using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ShoppingCartDbContext _context;

        public ShoppingCartService(ShoppingCartDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync()
        {
            return await _context.ShoppingCart.ToListAsync();
        }

        public async Task<ShoppingCart?> GetShoppingCartByIdAsync(int id)
        {
            return await _context.ShoppingCart.FindAsync(id);
        }

        public async Task<ShoppingCart> CreateShoppingCartAsync(ShoppingCart cart)
        {
            _context.ShoppingCart.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<ShoppingCart?> UpdateShoppingCartAsync(int id, ShoppingCart cart)
        {
            if (id != cart.CartID)
            {
                return null;
            }

            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> DeleteShoppingCartAsync(int id)
        {
            var cart = await _context.ShoppingCart.FindAsync(id);

            if (cart == null)
            {
                return false;
            }

            _context.ShoppingCart.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
