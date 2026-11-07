using Microsoft.AspNetCore.Identity;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class CustomerAuthService
        : ICustomerAuthService
    {
        private readonly ShoppingCartDbContext _context;

        private readonly IGuestCartService
            _guestCartService;

        private readonly ICartItemService
            _cartItemService;

        private readonly IProductService
            _productService;

        private readonly PasswordHasher<Customer>
            _passwordHasher;

        private readonly IConfiguration
            _configuration;

        private readonly IHttpContextAccessor 
            _httpContextAccessor;

        public CustomerAuthService(
            ShoppingCartDbContext context,
            IGuestCartService guestCartService,
            ICartItemService cartItemService,
            IProductService productService,
            IConfiguration configuration,
             IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _guestCartService = guestCartService;
            _cartItemService = cartItemService;
            _productService = productService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _passwordHasher =
                new PasswordHasher<Customer>();
        }
    }
}