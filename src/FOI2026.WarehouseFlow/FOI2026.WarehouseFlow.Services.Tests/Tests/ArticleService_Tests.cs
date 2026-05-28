using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using FOI2026.WarehouseFlow.Services.Services;
using Xunit;

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

        [Fact]
        public async Task GetAllArticlesWithStockAsync_CallsRepository_Once()
        {
            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new List<Article>()));

            await _service.GetAllArticlesWithStockAsync();

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetAllArticlesWithStockAsync_ReturnsEmptyList_WhenRepositoryReturnsNoArticles()
        {
            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new List<Article>()));

            var result = await _service.GetAllArticlesWithStockAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllArticlesWithStockAsync_SetsCurrentStock_ForEachArticle()
        {
            var articles = new List<Article>
            {
                new Article
                {
                    ArticleId = 1,
                    MinStock = 10,
                    DeliveryNoteItems = new List<DeliveryNoteItem> { new DeliveryNoteItem { Quantity = 100 } },
                    OrderItems       = new List<OrderItem>         { new OrderItem         { Quantity = 30  } }
                },
                new Article
                {
                    ArticleId = 2,
                    MinStock = 5,
                    DeliveryNoteItems = new List<DeliveryNoteItem> { new DeliveryNoteItem { Quantity = 50 } },
                    OrderItems       = new List<OrderItem>         { new OrderItem         { Quantity = 20 } }
                }
            };

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(articles));

            var result = (await _service.GetAllArticlesWithStockAsync()).ToList();

            Assert.Equal(70, result[0].CurrentStock); // 100 - 30
            Assert.Equal(30, result[1].CurrentStock); // 50  - 20
        }

        [Fact]
        public async Task GetAllArticlesWithStockAsync_SetsStatus_ForEachArticle()
        {
            var articles = new List<Article>
            {
                new Article
                {
                    ArticleId = 1,
                    MinStock = 10,
                    DeliveryNoteItems = new List<DeliveryNoteItem> { new DeliveryNoteItem { Quantity = 100 } },
                    OrderItems       = new List<OrderItem>         { new OrderItem         { Quantity = 30  } }
                    // stock = 70 → OK
                },
                new Article
                {
                    ArticleId = 2,
                    MinStock = 5,
                    DeliveryNoteItems = new List<DeliveryNoteItem> { new DeliveryNoteItem { Quantity = 10 } },
                    OrderItems       = new List<OrderItem>         { new OrderItem         { Quantity = 9  } }
                    // stock = 1 → Kritično
                }
            };

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(articles));

            var result = (await _service.GetAllArticlesWithStockAsync()).ToList();

            Assert.Equal("OK", result[0].Status);
            Assert.Equal("Kritično", result[1].Status);
        }

    }
}
