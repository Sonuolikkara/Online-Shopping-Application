using System.Globalization;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class ProductService
    {
        public async Task<byte[]> ExportProducts()
        {
            var products = await GetProducts();

            var csv = new StringBuilder();

            // CSV header
            csv.AppendLine(
                "ProductName,Description,Price,Stock,CategoryID");

            // Product rows
            foreach (var product in products)
            {
                csv.AppendLine(string.Join(",",
                    EscapeCsvField(product.ProductName),
                    EscapeCsvField(product.Description),
                    product.Price.ToString(
                        CultureInfo.InvariantCulture),
                    product.Stock,
                    product.CategoryID?.ToString() ?? ""
                ));
            }
            //Convert CSV text to bytes
            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        private static string EscapeCsvField(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Contains(',') ||
                value.Contains('"') ||
                value.Contains('\n') ||
                value.Contains('\r'))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            return value;
        }
    }
}