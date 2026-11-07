using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IGuestCartService
    {
        List<GuestCartItem> GetCart();

        void SaveCart(List<GuestCartItem> cart);

        void ClearCart();

        void AddToCart(int productId,int quantity,int stock);
    }
}
