using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.UserPage
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCartApiController : ControllerBase
    {
        private readonly IUserCartService _userCartService;

        public UserCartApiController(
            IUserCartService userCartService)
        {
            _userCartService = userCartService;
        }

        // ============================================================
        // GET CART
        // ============================================================

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCart()
        {
            var customerId = GetCustomerId();

            try
            {
                var cart =
                    await _userCartService
                        .GetCart(customerId);

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ============================================================
        // REMOVE / DECREASE CART ITEM
        // ============================================================

        [HttpPost("Remove")]
        [AllowAnonymous]
        public async Task<IActionResult> Remove(
            int cartItemId)
        {
            var customerId = GetCustomerId();

            try
            {
                var result =
                    await _userCartService
                        .Remove(
                            customerId,
                            cartItemId);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = "Cart item not found."
                    });
                }

                return Ok(new
                {
                    message =
                        "Cart item updated successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // ============================================================
        // GET CHECKOUT
        // ============================================================

        [HttpGet("Checkout")]
        [Authorize(
           AuthenticationSchemes = "Bearer",
           Roles = "Customer")]
        public async Task<IActionResult> GetCheckout(
            int? productId,
            int? quantity)
        {
            var customerId = GetCustomerId();

            if (!customerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Customer login required."
                });
            }

            try
            {
                var checkout =
                    await _userCartService
                        .GetCheckout(
                            customerId.Value,
                            productId,
                            quantity);

                return Ok(checkout);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
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

        // ============================================================
        // CHECKOUT /Place Order
        // ============================================================
        //Actual Order Creation
        // POST: /api/UserCartApi/Checkout
        [HttpPost("Checkout")]
        [Authorize(
          AuthenticationSchemes = "Bearer",
           Roles = "Customer")]
        public async Task<IActionResult> Checkout(
            CheckoutViewModel model)
        {
            var customerId = GetCustomerId();

            if (!customerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Customer login required."
                });
            }

            try
            {
                var orderId =
                    await _userCartService
                        .Checkout(
                            customerId.Value,
                            model);

                return Ok(new
                {
                    orderId
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
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

        // ============================================================
        // ORDER PLACED
        // ============================================================
        // GET: /api/UserCartApi/OrderPlaced/{id}
        [HttpGet("OrderPlaced/{id}")]
        [Authorize(
          AuthenticationSchemes = "Bearer",
          Roles = "Customer")]
        public async Task<IActionResult> OrderPlaced(
            int id)
        {
            var customerId = GetCustomerId();

            if (!customerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Customer login required."
                });
            }

            var order =
                await _userCartService
                    .GetOrderPlaced(id);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            return Ok(order);
        }

        // ============================================================
        // GET CUSTOMER ID
        // CUSTOMER ID FROM JWT Used in First Statement
        // ============================================================

        private int? GetCustomerId()
        {
            var claim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return null;
            }

            if (int.TryParse(
                claim.Value,
                out var customerId))
            {
                return customerId;
            }

            return null;
        }
    }
}