using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IPartnerRepository
    {
        Task<IEnumerable<Partner>> GetAllAsync();
        Task<Partner> GetByIdAsync(int id);
        Task AddAsync(Partner partner);
        Task UpdateAsync(Partner Pprtner);
        Task DeleteAsync(int id);
    }
}
