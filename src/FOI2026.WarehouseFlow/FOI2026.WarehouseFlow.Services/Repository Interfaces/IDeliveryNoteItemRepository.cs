using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IDeliveryNoteItemRepository
    {
        Task<IEnumerable<DeliveryNoteItem>> GetAllAsync();
        Task<DeliveryNoteItem> GetByIdAsync(int id);
        Task AddAsync(DeliveryNoteItem deliveryNoteItem);
        Task UpdateAsync(DeliveryNoteItem deliveryNoteItem);
        Task DeleteAsync(int id);

    }
}
