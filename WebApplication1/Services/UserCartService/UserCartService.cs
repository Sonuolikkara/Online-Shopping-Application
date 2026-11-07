using WebApplication1.Data;

namespace WebApplication1.Services
{
    public partial class UserCartService
        : IUserCartService
    {
        private readonly ShoppingCartDbContext _context;

        private readonly ICartItemService _cartItemService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly IGuestCartService _guestCartService;

        public UserCartService(
            ShoppingCartDbContext context,
            ICartItemService cartItemService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IPaymentService paymentService,
            IProductService productService,
            IGuestCartService guestCartService)
        {
            _context = context;

            _cartItemService =
                cartItemService;

            _orderService =
                orderService;

            _orderItemService =
                orderItemService;

            _paymentService =
                paymentService;

            _productService =
                productService;

            _guestCartService =
                guestCartService;
        }
    }
}