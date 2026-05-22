using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ArticleId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; } 

        public virtual Order Order { get; set; }

        public virtual Article Article { get; set; }
    }
}
