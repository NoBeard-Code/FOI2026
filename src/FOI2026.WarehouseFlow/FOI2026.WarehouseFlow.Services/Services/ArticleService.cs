using System;
using System.Collections.Generic;
using System.Text;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class ArticleService
    {

        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesWithStockAsync()
        {
            var articles = await _articleRepository.GetAllWithStockDataAsync();

            foreach (var article in articles)
            {
                article.CurrentStock = CalculateCurrentStock(article);
                article.Status = DetermineStockStatus(article);
            }

            return articles;
        }

        private int CalculateCurrentStock(Article article)
        {
            int totalDelivered = article.DeliveryNoteItems?.Sum(d => d.Quantity) ?? 0;
            int totalOrdered = article.OrderItems?.Sum(o => o.Quantity) ?? 0;

            return totalDelivered - totalOrdered;
        }

        private string DetermineStockStatus(Article article)
        {
            if (article.CurrentStock < article.MinStock)
                return "Kritično";

            return "OK";
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _articleRepository.GetByIdAsync(id);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _articleRepository.AddAsync(article);
        }

    
    }
}
