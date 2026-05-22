using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class DeliveryNoteItem
    {
        public int DeliveryNoteId { get; set; }

        public int ArticleId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual DeliveryNote DeliveryNote { get; set; }
        public virtual Article Article { get; set; }
    }
}
