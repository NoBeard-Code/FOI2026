using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IDeliveryNoteRepository
    {
        Task<IEnumerable<DeliveryNote>> GetAllAsync();
        Task<DeliveryNote> GetByIdAsync(int id);
        Task AddAsync(DeliveryNote deliveryNote);
        Task UpdateAsync(DeliveryNote deliveryNote);
        Task DeleteAsync(int id);

    }
}
