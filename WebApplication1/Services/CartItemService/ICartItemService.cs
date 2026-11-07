using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItem>> GetCartItems();

        Task<CartItem?> GetCartItemById(int id);

        Task<CartItem> AddCartItem(CartItem cartItem);

        Task<CartItem> UpdateCartItem(CartItem cartItem);


        // Customer Cart Selection
        Task<int?> GetCartIdByCustomerId(int customerId);

        Task<CartItem?> GetCartItemByProductId(
            int cartId,
            int productId);

        // Cart page Viewing
        Task<IEnumerable<CartItemViewModel>>
            GetCartItemsByCustomerId(int customerId);

        //Seq Dlt OR Minus Dlt
        Task<bool> DecreaseCartItemQuantity(int cartItemId);

        Task AddToCart(int customerId,int productId,int quantity,int stock);

        //Cart Count
        Task<int> GetCartItemCountByCustomerId(int customerId);

        //Clearing Cart Items
        Task ClearCartByCustomerId(int customerId);
    }
}