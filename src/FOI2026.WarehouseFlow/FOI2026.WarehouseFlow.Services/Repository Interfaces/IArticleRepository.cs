using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Repository_Interfaces
{
    public interface IArticleRepository
    {

        public Task<IEnumerable<Article>> GetAllAsync();




    }
}
