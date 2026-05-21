using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class Partner
    {
        public int PartnerId { get; set; }
        public int oib { get; set; }
        public string? address { get; set; }
        public string? contact { get; set; }
        public string? email { get; set; }
        public string name { get; set; }

        public ICollection<Order> orders { get; set; }
        public ICollection<DeliveryNote> deliveryNotes { get; set; }
    }
}
