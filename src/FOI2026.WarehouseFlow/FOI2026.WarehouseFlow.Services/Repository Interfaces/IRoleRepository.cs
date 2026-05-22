using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetAllAsync();
        Task<ApplicationRole> GetByIdAsync(string id);
        Task AddAsync(ApplicationRole applicationRole);
        Task UpdateAsync(ApplicationRole applicationRole);
        Task DeleteAsync(string id);

    }
}
