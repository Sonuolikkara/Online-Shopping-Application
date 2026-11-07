using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Services;

namespace WebApplication1.Controllers.UserPage
{
    [Authorize(
        AuthenticationSchemes = "Bearer",
        Roles = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserWishlistApiController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public UserWishlistApiController(
            IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }


        // WISHLIST
        // GET: /api/UserWishlistApi

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customerIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (customerIdClaim == null ||
                !int.TryParse(
                    customerIdClaim.Value,
                    out int customerId))
            {
                return Unauthorized();
            }


            var wishlist =
                await _wishlistService
                    .GetWishlistByCustomerId(customerId);

            return Ok(wishlist);
        }


        // ADD TO WISHLIST
        // POST: /api/UserWishlistApi/Add

        [HttpPost("Add")]
        public async Task<IActionResult> Add(
            int productId)
        {
            var customerIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (customerIdClaim == null ||
                !int.TryParse(
                    customerIdClaim.Value,
                    out int customerId))
            {
                return Unauthorized();
            }


            var alreadyExists =
                await _wishlistService
                    .IsProductInWishlist(
                        customerId,
                        productId);


            if (!alreadyExists)
            {
                await _wishlistService
                    .AddToWishlist(
                        customerId,
                        productId);
            }


            return Ok(new
            {
                message =
                    "Product added to wishlist."
            });
        }


        // REMOVE FROM WISHLIST
        // POST: /api/UserWishlistApi/Remove

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(
            int productId)
        {
            var customerIdClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (customerIdClaim == null ||
                !int.TryParse(
                    customerIdClaim.Value,
                    out int customerId))
            {
                return Unauthorized();
            }


            await _wishlistService
                .RemoveFromWishlist(
                    customerId,
                    productId);


            return Ok(new
            {
                message =
                    "Product removed from wishlist."
            });
        }
    }
}