using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistItem>>
            GetWishlistByCustomerId(int customerId);

        Task<bool>
            IsProductInWishlist(
                int customerId,
                int productId);

        Task<WishlistItem>
            AddToWishlist(
                int customerId,
                int productId);

        Task<bool>
            RemoveFromWishlist(
                int customerId,
                int productId);

        Task<bool> AddToWishlistIfNotExists(
                int customerId,
                int productId);
    }
}