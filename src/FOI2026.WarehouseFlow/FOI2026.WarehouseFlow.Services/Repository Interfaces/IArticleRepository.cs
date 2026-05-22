using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IArticleRepository
    {

        Task<IEnumerable<Article>> GetAllAsync();
        Task<IEnumerable<Article>> GetAllWithStockDataAsync();
        Task<Article> GetByIdAsync(int id);
        Task AddAsync(Article article);
        Task UpdateAsync(Article article);
        Task DeleteAsync(int id);
    }
}
