using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Properties.Controllers.Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemsController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        // GET: api/cartitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            var cartItems = await _cartItemService.GetCartItems();

            return Ok(cartItems);
        }

        // GET: api/cartitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
            var cartItem = await _cartItemService.GetCartItemById(id);

            if (cartItem == null)
            {
                return NotFound(new
                {
                    message = "Cart item not found. Try another cart item."
                });
            }

            return Ok(cartItem);
        }

        // POST: api/cartitems
        [HttpPost]
        public async Task<ActionResult<CartItem>> CreateCartItem(
            CartItem cartItem)
        {
            var createdCartItem =
                await _cartItemService.AddCartItem(cartItem);

            return CreatedAtAction(
                nameof(GetCartItem),
                new { id = createdCartItem.CartItemID },
                createdCartItem
            );
        }

        // PUT: api/cartitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(
            int id,
            CartItem cartItem)
        {
            if (id != cartItem.CartItemID)
            {
                return BadRequest(new
                {
                    message = "Cart item ID does not match the URL ID."
                });
            }

            await _cartItemService.UpdateCartItem(cartItem);

            return NoContent();
        }

       
    }
}