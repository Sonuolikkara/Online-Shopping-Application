using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class AdminDashboardViewModel
    {
        // Summary counts
        public int ProductCount { get; set; }

        public int CategoryCount { get; set; }

        public int CustomerCount { get; set; }

        public int OrderCount { get; set; }


        // Recent orders
        public IEnumerable<Order> RecentOrders { get; set; }
            = new List<Order>();


        // Low stock products
        public IEnumerable<Product> LowStockProducts { get; set; }
            = new List<Product>();

        // Order status

        public int DeliveredOrderCount { get; set; }

        public int ProcessingOrderCount { get; set; }


        public int PendingOrderCount { get; set; }
    }


}
