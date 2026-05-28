using System;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using FOI2026.WarehouseFlow.Services.Services;
using FakeItEasy;

namespace FOI2026.WarehouseFlow.Services.Tests.Tests
{
    public class ArticleService_Tests
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ArticleService _service;

        public ArticleService_Tests()
        {
            _articleRepository = A.Fake<IArticleRepository>();
            _service = new ArticleService(_articleRepository);
        }
    }
}
