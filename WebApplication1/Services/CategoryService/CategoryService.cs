using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Data;

namespace WebApplication1.Services
{
    public partial class CategoryService : ICategoryService
    {
        private readonly ShoppingCartDbContext _context;

        public CategoryService(ShoppingCartDbContext context)
        {
            _context = context;
        }
    }
}
