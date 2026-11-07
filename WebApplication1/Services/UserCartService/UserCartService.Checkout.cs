using WebApplication1.Models;

namespace WebApplication1.Services
{
    public partial class UserCartService
    {
        public async Task<int> Checkout(
            int customerId,
            CheckoutViewModel model)
        {
            // ========================================================
            // BUY NOW
            // ========================================================

            if (model.ProductId.HasValue)
            {
                var product =
                    await _productService
                        .GetProductById(
                            model.ProductId.Value);

                if (product == null)
                {
                    throw new KeyNotFoundException(
                        "Product not found.");
                }

                var quantity =
                    model.Quantity ?? 1;

                if (quantity <= 0)
                {
                    throw new InvalidOperationException(
                        "Invalid quantity.");
                }

                if (quantity > product.Stock)
                {
                    throw new InvalidOperationException(
                        "Requested quantity is not available.");
                }

                // ----------------------------------------------------
                // Calculate total
                // ----------------------------------------------------

                decimal totalAmount =
                    product.Price * quantity;

                // ----------------------------------------------------
                // Create Order
                // ----------------------------------------------------

                var order =
                    new Order
                    {
                        CustomerID =
                            customerId,

                        OrderDate =
                            DateTime.Now,

                        TotalAmount =
                            totalAmount,

                        Status =
                            "Placed"
                    };

                var createdOrder =
                    await _orderService
                        .AddOrder(order);

                // ----------------------------------------------------
                // Create OrderItem
                // ----------------------------------------------------

                var orderItem =
                    new OrderItem
                    {
                        OrderID =
                            createdOrder.OrderID,

                        ProductID =
                            product.ProductID,

                        Quantity =
                            quantity,

                        Price =
                            product.Price
                    };

                await _orderItemService
                    .AddOrderItem(
                        orderItem);

                // ----------------------------------------------------
                // Create Payment
                // ----------------------------------------------------

                var payment =
                    new Payment
                    {
                        OrderID =
                            createdOrder.OrderID,

                        PaymentMethod =
                            model.PaymentMethod,

                        PaymentStatus =
                            "Pending",

                        PaymentDate =
                            DateTime.Now
                    };

                await _paymentService
                    .AddPayment(
                        payment);

                // IMPORTANT:
                // Do NOT clear cart for BUY NOW.

                return createdOrder.OrderID;
            }

            // ========================================================
            // NORMAL CART CHECKOUT
            // ========================================================

            var cartItems =
                (await _cartItemService
                    .GetCartItemsByCustomerId(
                        customerId))
                .ToList();

            if (!cartItems.Any())
            {
                throw new InvalidOperationException(
                    "Your cart is empty.");
            }

            // --------------------------------------------------------
            // Calculate total
            // --------------------------------------------------------

            decimal cartTotal =
                cartItems.Sum(
                    item =>
                        item.Price *
                        item.Quantity);

            // --------------------------------------------------------
            // Create Order
            // --------------------------------------------------------

            var cartOrder =
                new Order
                {
                    CustomerID =
                        customerId,

                    OrderDate =
                        DateTime.Now,

                    TotalAmount =
                        cartTotal,

                    Status =
                        "Placed"
                };

            var createdCartOrder =
                await _orderService
                    .AddOrder(
                        cartOrder);

            // --------------------------------------------------------
            // Create OrderItems
            // --------------------------------------------------------

            foreach (var item in cartItems)
            {
                var orderItem =
                    new OrderItem
                    {
                        OrderID =
                            createdCartOrder.OrderID,

                        ProductID =
                            item.ProductID,

                        Quantity =
                            item.Quantity,

                        Price =
                            item.Price
                    };

                await _orderItemService
                    .AddOrderItem(
                        orderItem);
            }

            // --------------------------------------------------------
            // Create Payment
            // --------------------------------------------------------

            var cartPayment =
                new Payment
                {
                    OrderID =
                        createdCartOrder.OrderID,

                    PaymentMethod =
                        model.PaymentMethod,

                    PaymentStatus =
                        "Pending",

                    PaymentDate =
                        DateTime.Now
                };

            await _paymentService
                .AddPayment(
                    cartPayment);

            // --------------------------------------------------------
            // Clear cart ONLY for normal checkout
            // --------------------------------------------------------

            await _cartItemService
                .ClearCartByCustomerId(
                    customerId);

            return createdCartOrder.OrderID;
        }
    }
}