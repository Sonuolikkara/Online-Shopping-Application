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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _paymentService.GetPayments();

            return Ok(payments);
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentById(id);

            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Payment not found. Try another payment."
                });
            }

            return Ok(payment);
        }

        // POST: api/payments
        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment(
            Payment payment)
        {
            var createdPayment =
                await _paymentService.AddPayment(payment);

            return CreatedAtAction(
                nameof(GetPayment),
                new { id = createdPayment.PaymentID },
                createdPayment
            );
        }

        // PUT: api/payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(
            int id,
            Payment payment)
        {
            if (id != payment.PaymentID)
            {
                return BadRequest(new
                {
                    message = "Payment ID does not match the URL ID."
                });
            }

            await _paymentService.UpdatePayment(payment);

            return NoContent();
        }

        // DELETE: api/payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var deleted = await _paymentService.DeletePayment(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"Payment with ID {id} not found. Try another payment."
                });
            }

            return NoContent();
        }
    }
}