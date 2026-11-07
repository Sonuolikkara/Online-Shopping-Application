using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();

        Task<Category?> GetCategoryById(int id);

        Task<Category> AddCategory(Category category);

        Task<Category> UpdateCategory(Category category);

        Task<bool> DeleteCategory(int id);
    }
}
