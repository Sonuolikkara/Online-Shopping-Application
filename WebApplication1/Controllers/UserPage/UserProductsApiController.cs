using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.UserPage
{
    // /api/UserProductsApi
    [ApiController]
    [Route("api/[controller]")]
    public class UserProductsApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICartItemService _cartItemService;
        private readonly IWishlistService _wishlistService;
        private readonly IGuestCartService _guestCartService;

        public UserProductsApiController(
            IProductService productService,
            ICartItemService cartItemService,
            IWishlistService wishlistService,
            IGuestCartService guestCartService)
        {
            _productService = productService;
            _cartItemService = cartItemService;
            _wishlistService = wishlistService;
            _guestCartService = guestCartService;
        }


        // GET ALL PRODUCTS
        // GET: /api/UserProductsApi
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =
                await _productService.GetProducts();

            return Ok(products);//200
        }


        // GET PRODUCT DETAILS
        // GET: /api/UserProductsApi/{id}

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product =
                await _productService
                    .GetProductById(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(product);
        }


        // ADD TO CART
        // POST: /api/UserProductsApi/AddToCart
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(
            [FromForm] int productId,
            [FromForm] int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest(new
                {
                    message = "Quantity must be at least 1."
                });
            }

            var product =
                await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            if (product.Stock < quantity)
            {
                return BadRequest(new
                {
                    message = "Requested quantity is not available."
                });
            }

            var customerIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier);

            // GUEST
            if (customerIdClaim == null ||
                !int.TryParse(
                    customerIdClaim.Value,
                    out int customerId))
            {
                try
                {
                    _guestCartService.AddToCart(
                        productId,
                        quantity,
                        product.Stock);

                    return Ok(new
                    {
                        message = "Product added to cart."
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new
                    {
                        message = ex.Message
                    });
                }
            }

            // LOGGED-IN CUSTOMER
            try
            {
                await _cartItemService.AddToCart(
                    customerId,
                    productId,
                    quantity,
                    product.Stock);

                return Ok(new
                {
                    message = "Product added to cart."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // ADD TO WISHLIST
        // POST: /api/UserProductsApi/AddToWishlist
        [Authorize(
                 AuthenticationSchemes = "Bearer",
                 Roles = "Customer")]
        [HttpPost("AddToWishlist")]
        public async Task<IActionResult> AddToWishlist(
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

            var product =
                await _productService
                    .GetProductById(productId);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            await _wishlistService
                .AddToWishlistIfNotExists(
                    customerId,
                    productId);

            return Ok(new
            {
                message = "Product added to wishlist."
            });
        }


        // REMOVE FROM WISHLIST
        // POST: /api/UserProductsApi/RemoveFromWishlist
        [Authorize(
            AuthenticationSchemes = "Bearer",
            Roles = "Customer")]
        [HttpPost("RemoveFromWishlist")]
        public async Task<IActionResult> RemoveFromWishlist(
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


        // CHECK WISHLIST STATUS
        // GET: /api/UserProductsApi/WishlistStatus/{productId}
        [Authorize(
            AuthenticationSchemes = "Bearer",
            Roles = "Customer")]
        [HttpGet("WishlistStatus/{productId:int}")]
        public async Task<IActionResult> WishlistStatus(
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


            var isInWishlist =
                await _wishlistService
                    .IsProductInWishlist(
                        customerId,
                        productId);


            return Ok(new
            {
                isInWishlist
            });
        }
    }
}