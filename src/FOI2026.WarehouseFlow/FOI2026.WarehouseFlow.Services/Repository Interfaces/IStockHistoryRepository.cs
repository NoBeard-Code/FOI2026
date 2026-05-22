using System;
using System.Collections.Generic;
using System.Text;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IStockHistoryRepository
    {
        Task<IEnumerable<Order>> GetOrdersByArticleIdAsync(int articleId);
        Task<IEnumerable<DeliveryNote>> GetDeliveryNotesByArticleIdAsync(int articleId);
    }
}
