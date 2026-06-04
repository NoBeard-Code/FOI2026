using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using FOI2026.WarehouseFlow.Services.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FOI2026.WarehouseFlow.Services.Tests.Tests;

public class ArticleServiceTests
{
    [Fact]
    public async Task ArticleService_AddArticleAsync_Empty()
    {
        // Arrange

        // declare art repo but with injected mock implementation
        // init art service with mocked repo impl

        var articleRepository = new MockArticleRepository();
        var articleService = new ArticleService(articleRepository);
        var article = new Article();

        // Act

        await articleService.AddArticleAsync(article);

        // Assert


    }

}
