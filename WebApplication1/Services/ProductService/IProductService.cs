using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IProductService
    {
        //CRUD Operations
        Task<IEnumerable<Product>> GetProducts();

        Task<Product?> GetProductById(int id);

        Task<Product> AddProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task<bool> DeleteProduct(int id);

        // Bulk Delete Products
        Task<bool> DeleteSelectedProducts(IEnumerable<int> productIds);

        Task<bool> HasProductsByCategoryId(int categoryId);

        //Import Products from CSV
        Task ImportProducts(IFormFile file);

        //Export Products to CSV
        Task<byte[]> ExportProducts();
    }
}
