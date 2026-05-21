using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public decimal price { get; set; }
        public string unit { get; set; }
        public int currentStock { get; set; }
        public int maxStock { get; set; }
        public int minStock { get; set; }
        public string? description { get; set; }
        public string status { get; set; }
        public int idCategory { get; set; }


        public Category category { get; set; }
        public ICollection<OrderItem> orderItems { get; set; }
        public ICollection<DeliveryNoteItem> deliveryNoteItems { get; set; }

    }
}
