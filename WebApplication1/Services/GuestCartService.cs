using System.Text.Json;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class GuestCartService : IGuestCartService
    {
        //Session key
        private const string GuestCartSessionKey = "GuestCart";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public GuestCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<GuestCartItem> GetCart()
        {
            //Get the current session
            var session = _httpContextAccessor.HttpContext?.Session;

            //Return an Empty Guest cart
            if (session == null)
                return new List<GuestCartItem>();

            var json = session.GetString(GuestCartSessionKey);

            if (string.IsNullOrEmpty(json))
                return new List<GuestCartItem>();

            //Deserialize JSON
            return JsonSerializer.Deserialize<List<GuestCartItem>>(json)
                   ?? new List<GuestCartItem>();
        }

        public void SaveCart(List<GuestCartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session == null)
                return;

            //Serialize Cart
            session.SetString(
                GuestCartSessionKey,
                JsonSerializer.Serialize(cart));
        }

        public void AddToCart(
               int productId,
               int quantity,
               int stock)
        {
            var cart = GetCart();

            var existingItem =
                cart.FirstOrDefault(
                    x => x.ProductID == productId);

            if (existingItem != null)
            {
                var newQuantity =
                    existingItem.Quantity + quantity;

                if (newQuantity > stock)
                {
                    throw new InvalidOperationException(
                        "Requested quantity exceeds available stock.");
                }

                existingItem.Quantity = newQuantity;
            }
            else
            {
                if (quantity > stock)
                {
                    throw new InvalidOperationException(
                        "Requested quantity exceeds available stock.");
                }

                cart.Add(
                    new GuestCartItem
                    {
                        ProductID = productId,
                        Quantity = quantity
                    });
            }

            SaveCart(cart);
        }

        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            session?.Remove(GuestCartSessionKey);
        }
    }
}