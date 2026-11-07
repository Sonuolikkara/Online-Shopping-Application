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
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        // GET: api/orderitems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            var orderItems = await _orderItemService.GetOrderItems();

            return Ok(orderItems);
        }

        // GET: api/orderitems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _orderItemService.GetOrderItemById(id);

            if (orderItem == null)
            {
                return NotFound(new
                {
                    message = "Order item not found. Try another order item."
                });
            }

            return Ok(orderItem);
        }

        // POST: api/orderitems
        [HttpPost]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(
            OrderItem orderItem)
        {
            var createdOrderItem =
                await _orderItemService.AddOrderItem(orderItem);

            return CreatedAtAction(
                nameof(GetOrderItem),
                new { id = createdOrderItem.OrderItemID },
                createdOrderItem
            );
        }

        // PUT: api/orderitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(
            int id,
            OrderItem orderItem)
        {
            if (id != orderItem.OrderItemID)
            {
                return BadRequest(new
                {
                    message = "Order item ID does not match the URL ID."
                });
            }

            await _orderItemService.UpdateOrderItem(orderItem);

            return NoContent();
        }

        // DELETE: api/orderitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var deleted = await _orderItemService.DeleteOrderItem(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"Order item with ID {id} not found. Try another order item."
                });
            }

            return NoContent();
        }
    }
}
