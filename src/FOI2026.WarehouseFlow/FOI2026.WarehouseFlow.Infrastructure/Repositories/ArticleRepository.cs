using System;
using System.Collections.Generic;
using System.Text;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;

namespace FOI2026.WarehouseFlow.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        public Task<IEnumerable<Article>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

    }
}
