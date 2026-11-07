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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            return Ok(orders);
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderById(id);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found. Try another order."
                });
            }

            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            var createdOrder = await _orderService.AddOrder(order);

            return CreatedAtAction(
                nameof(GetOrder),
                new { id = createdOrder.OrderID },
                createdOrder
            );
        }

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(
            int id,
            Order order)
        {
            if (id != order.OrderID)
            {
                return BadRequest(new
                {
                    message = "Order ID does not match the URL ID."
                });
            }

            await _orderService.UpdateOrder(order);

            return NoContent();
        }

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _orderService.DeleteOrder(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"Order with ID {id} not found. Try another order."
                });
            }

            return NoContent();
        }
    }
}
