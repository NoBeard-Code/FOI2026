using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class DeliveryNoteItem
    {
        public int DeliveryNoteId { get; set; }
        public int ItemId { get; set; }
        public int itemQuantity { get; set; }
        public DeliveryNote deliveryNote { get; set; }
        public Item item { get; set; }
    }
}
