using FOI2026.WarehouseFlow.Services.Tests.Mocks;
using Xunit;

namespace FOI2026.WarehouseFlow.Services.Tests.Tests;

public class ArticleRepositoryTests
{
    [Fact]
    public void ArticleRepository_AddAsync_Init()
    {
        // Arrange

        // declare art repo but with injected mock implementation

        var articleRepository = new MockArticleRepository();


        // Act

        // Assert


    }


}
