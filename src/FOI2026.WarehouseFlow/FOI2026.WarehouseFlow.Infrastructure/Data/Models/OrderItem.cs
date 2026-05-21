using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int itemQuantity { get; set; }

        public Order order { get; set; }

        public Item item { get; set; }
    }
}
