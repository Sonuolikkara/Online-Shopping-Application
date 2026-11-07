using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;
using Serilog;

namespace WebApplication1.Properties.Controllers.Backend
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // Testing serilog
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception");
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomers();

            return Ok(customers);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound(new
                {
                    message = "Customer not found. Try another customer."
                });
            }

            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(
            Customer customer)
        {
            var createdCustomer =
                await _customerService.AddCustomer(customer);

            return CreatedAtAction(
                nameof(GetCustomer),
                new { id = createdCustomer.CustomerID },
                createdCustomer
            );
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(
            int id,
            Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest(new
                {
                    message = "Customer ID does not match the URL ID."
                });
            }

            await _customerService.UpdateCustomer(customer);

            return NoContent();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var deleted = await _customerService.DeleteCustomer(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message =
                        $"Customer with ID {id} not found. Try another customer."
                });
            }

            return NoContent();
        }
    }
}