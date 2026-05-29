using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOI2026.WarehouseFlow.Services.Tests.Mocks;

internal class MockArticleRepository : IArticleRepository
{
    public Task AddAsync(Article article)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Article>> GetAllAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<Article>> GetAllWithStockDataAsync()
    {
        throw new System.NotImplementedException();
    }

    public Task<Article> GetByIdAsync(int id)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateAsync(Article article)
    {
        throw new System.NotImplementedException();
    }
}
