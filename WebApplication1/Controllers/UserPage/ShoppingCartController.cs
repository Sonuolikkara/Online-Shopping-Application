using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.UserPage
{
    // /api/ShoppingCart
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // Get all shopping carts
        // GET: api/shoppingcart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        {
            return Ok(await _shoppingCartService.GetShoppingCartsAsync());
        }

        // GET: api/shoppingcart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(int id)
        {
            //Searches for CartID
            var cart = await _shoppingCartService.GetShoppingCartByIdAsync(id);

            if (cart == null)
                return NotFound();

            return cart;
        }

        //Create Cart
        // POST: api/shoppingcart
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> CreateShoppingCart(
            ShoppingCart cart)
        {
            await _shoppingCartService.CreateShoppingCartAsync(cart);

            return CreatedAtAction(
                nameof(GetShoppingCart),
                new { id = cart.CartID },
                cart
            );
        }

        //Update Cart
        // PUT: api/shoppingcart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoppingCart(
            int id,
            ShoppingCart cart)
        {
            var updated = await _shoppingCartService.UpdateShoppingCartAsync(id, cart);

            if (updated == null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE: api/shoppingcart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            var deleted = await _shoppingCartService.DeleteShoppingCartAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
