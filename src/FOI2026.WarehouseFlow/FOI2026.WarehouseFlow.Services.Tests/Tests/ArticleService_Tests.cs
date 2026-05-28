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
        [Fact]
        public async Task CalculateCurrentStock_ReturnsZero_WhenNoDeliveriesAndNoOrders()
        {
            // Arrange
            var article = BuildArticle(articleId: 1, delivered: 0, ordered: 0, minStock: 5);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert
            Assert.Equal(0, result.CurrentStock);
        }

        [Fact]
        public async Task CalculateCurrentStock_ReturnsDeliveredAmount_WhenNoOrders()
        {
            // Arrange
            var article = BuildArticle(articleId: 1, delivered: 80, ordered: 0, minStock: 5);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert
            Assert.Equal(80, result.CurrentStock);
        }

        [Fact]
        public async Task CalculateCurrentStock_ReturnsNegative_WhenOrdersExceedDeliveries()
        {
            // Arrange 
            var article = BuildArticle(articleId: 1, delivered: 10, ordered: 30, minStock: 5);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert
            Assert.Equal(-20, result.CurrentStock);
        }

        [Fact]
        public async Task CalculateCurrentStock_HandlesNullDeliveryNoteItems_AsZero()
        {
            // Arrange
            var article = new Article
            {
                ArticleId = 1,
                MinStock = 5,
                DeliveryNoteItems = null,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = 10 }
                }
            };

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert
            Assert.Equal(-10, result.CurrentStock);
        }

        [Fact]
        public async Task CalculateCurrentStock_HandlesNullOrderItems_AsZero()
        {
            // Arrange
            var article = new Article
            {
                ArticleId = 1,
                MinStock = 5,
                DeliveryNoteItems = new List<DeliveryNoteItem>
                {
                    new DeliveryNoteItem { Quantity = 50 }
                },
                OrderItems = null
            };

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert 
            Assert.Equal(50, result.CurrentStock);
        }

        [Fact]
        public async Task CalculateCurrentStock_SumsMultipleDeliveryAndOrderItems()
        {
            // Arrange
            var article = new Article
            {
                ArticleId = 1,
                MinStock = 5,
                DeliveryNoteItems = new List<DeliveryNoteItem>
                {
                    new DeliveryNoteItem { Quantity = 40 },
                    new DeliveryNoteItem { Quantity = 60 }   // ukupno isporučeno = 100
                },
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = 15 },
                    new OrderItem { Quantity = 25 }           
                }
            };

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

         
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            
            Assert.Equal(60, result.CurrentStock);
        }


        [Fact]
        public async Task DetermineStockStatus_ReturnsOK_WhenCurrentStockEqualsMinStock()
        {
            // Arrange 
            var article = BuildArticle(articleId: 1, delivered: 10, ordered: 0, minStock: 10);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert
            Assert.Equal("OK", result.Status);
        }

        [Fact]
        public async Task DetermineStockStatus_ReturnsOK_WhenCurrentStockAboveMinStock()
        {
            // Arrange
            var article = BuildArticle(articleId: 1, delivered: 50, ordered: 10, minStock: 10);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert 
            Assert.Equal("OK", result.Status);
        }

        [Fact]
        public async Task DetermineStockStatus_ReturnsKriticno_WhenCurrentStockBelowMinStock()
        {
            // Arrange
            var article = BuildArticle(articleId: 1, delivered: 5, ordered: 0, minStock: 10);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert 
            Assert.Equal("Kritično", result.Status);
        }

        [Fact]
        public async Task DetermineStockStatus_ReturnsKriticno_WhenCurrentStockIsNegative()
        {
            // Arrange
            var article = BuildArticle(articleId: 1, delivered: 0, ordered: 5, minStock: 1);

            A.CallTo(() => _articleRepository.GetAllWithStockDataAsync())
                .Returns(Task.FromResult<IEnumerable<Article>>(new[] { article }));

            // Act
            var result = (await _service.GetAllArticlesWithStockAsync()).Single();

            // Assert 
            Assert.Equal("Kritično", result.Status);
        }

        private static Article BuildArticle(int articleId, int delivered, int ordered, int minStock)
        {
            return new Article
            {
                ArticleId = articleId,
                MinStock = minStock,
                DeliveryNoteItems = delivered == 0
                    ? new List<DeliveryNoteItem>()
                    : new List<DeliveryNoteItem>
                    {
                new DeliveryNoteItem { Quantity = delivered }
                    },
                OrderItems = ordered == 0
                    ? new List<OrderItem>()
                    : new List<OrderItem>
                    {
                new OrderItem { Quantity = ordered }
                    }
            };
        }
    }

}
