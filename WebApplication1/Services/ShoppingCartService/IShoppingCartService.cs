using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IShoppingCartService
    {
        Task<IEnumerable<ShoppingCart>> GetShoppingCartsAsync();

        Task<ShoppingCart?> GetShoppingCartByIdAsync(int id);

        Task<ShoppingCart> CreateShoppingCartAsync(ShoppingCart cart);

        Task<ShoppingCart?> UpdateShoppingCartAsync(int id, ShoppingCart cart);

        Task<bool> DeleteShoppingCartAsync(int id);
    }
}
