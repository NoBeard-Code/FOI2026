using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<OrderItem> GetByIdAsync(int id);
        Task AddAsync(OrderItem orderItem);
        Task UpdateAsync(OrderItem orderItem);
        Task DeleteAsync(int id);

    }
}
