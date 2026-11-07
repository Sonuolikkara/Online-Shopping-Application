using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace WebApplication1.Services
{
    public partial class CustomerService
    {
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            const string cacheKey = "customers";

            var cachedCustomers = await _cache.GetStringAsync(cacheKey);

            if (cachedCustomers != null)
            {
                Console.WriteLine("REDIS CACHE HIT");

                return JsonSerializer.Deserialize<IEnumerable<Customer>>(cachedCustomers)
                       ?? Enumerable.Empty<Customer>();
            }

            Console.WriteLine("REDIS CACHE MISS - DATABASE");

            var customers = await _context.Customers
                .OrderBy(c => c.CustomerID)
                .ToListAsync();

            var serializedCustomers = JsonSerializer.Serialize(customers);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            await _cache.SetStringAsync(
                cacheKey,
                serializedCustomers,
                cacheOptions);

            return customers;
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var customer = await connection.QueryFirstOrDefaultAsync<Customer>(
                "GetCustomerById",
                new { CustomerID = id },
                commandType: CommandType.StoredProcedure
            );

            return customer;
        }
    }
}