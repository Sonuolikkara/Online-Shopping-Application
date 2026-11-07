using System.Globalization;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task ImportProducts(IFormFile file)
        {
            // Validate file
            if (file == null || file.Length == 0)
                throw new Exception("CSV file is empty.");

            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
                throw new Exception("Only CSV files are allowed.");
            //Craeting a List
            var products = new List<Product>();

            // Open and read CSV
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            // Skip header
            await reader.ReadLineAsync();


            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            { 

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var values = line.Split(',');

                if (values.Length < 5)
                    throw new Exception($"Invalid row: {line}");

                var product = new Product
                {
                    ProductName = values[0].Trim(),
                    Description = values[1].Trim(),
                    Price = decimal.Parse(
                        values[2],
                        CultureInfo.InvariantCulture),
                    Stock = int.Parse(values[3]),
                    CategoryID = string.IsNullOrWhiteSpace(values[4])
                        ? null
                        : int.Parse(values[4])
                };

                products.Add(product);
            }

            // Save products
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Clear cached product list
            await InvalidateProductCacheAsync();
        }
    }
}