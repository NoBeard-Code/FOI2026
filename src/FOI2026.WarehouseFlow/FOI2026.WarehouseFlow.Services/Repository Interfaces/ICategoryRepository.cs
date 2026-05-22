using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);

    }
}
