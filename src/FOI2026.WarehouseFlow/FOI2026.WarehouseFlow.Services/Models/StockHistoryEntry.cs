using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Models
{
    public class StockHistoryEntry
    {
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string ChangeType { get; set; }
        public int Quantity { get; set; }
        public int NewStock { get; set; }
    }
}
