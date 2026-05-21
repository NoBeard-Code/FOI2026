using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class DeliveryNote
    {
        public int DeliveryNoteId { get; set; }
        public string code { get; set; }
        public DateTime date { get; set; }
        public int idPartner { get; set; }
        public int idItem { get; set; }
        public string status { get; set; }
        public string? description { get; set; }
        public int idUser { get; set; }

        public Partner partner { get; set; }
        public Item item { get; set; }
        public ApplicationUser user { get; set; }
        public ICollection<DeliveryNoteItem> deliveryNoteItems { get; set; }


    }
}
