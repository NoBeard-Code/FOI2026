using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task AddAsync(ApplicationUser applicationUser);
        Task UpdateAsync(ApplicationUser applicationUser);
        Task DeleteAsync(string id);

    }
}
