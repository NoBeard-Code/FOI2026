using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string name { get; set; }

        public string? description { get; set; }

        public ICollection<Item> items { get; set; }
    }
}
