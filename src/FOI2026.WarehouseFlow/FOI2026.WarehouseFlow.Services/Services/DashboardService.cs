using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class DashboardService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IOrderRepository _orderRepository;

        public DashboardService(
            IArticleRepository articleRepository,
            IOrderRepository orderRepository)
        {
            _articleRepository = articleRepository;
            _orderRepository = orderRepository;
        }

        // 1. UKUPNO SKLADIŠTE (STVARNI STOCK)
        public async Task<int> GetTotalStockQuantityAsync()
        {
            var articles = await _articleRepository.GetAllWithStockDataAsync();

            int totalStock = 0;

            foreach (var a in articles)
            {
                int delivered = a.DeliveryNoteItems?.Sum(d => d.Quantity) ?? 0;
                int ordered = a.OrderItems?.Sum(o => o.Quantity) ?? 0;

                totalStock += (delivered - ordered);
            }

            return totalStock;
        }

        // 2. VRIJEDNOST ZALIHA / PRODAJE
        public async Task<decimal> GetStockValueAsync()
        {
            var articles = await _articleRepository.GetAllWithStockDataAsync();

            decimal total = 0;

            foreach (var article in articles)
            {
                total += article.OrderItems?
                    .Sum(o => o.Quantity * o.Price) ?? 0;
            }

            return total;
        }

        // 3. TOP 5 NAJPRODAVANIJIH
        public async Task<List<(string Name, int Quantity)>> GetTopSellingArticlesAsync()
        {
            var articles = await _articleRepository.GetAllWithStockDataAsync();

            return articles
                .Select(a => new
                {
                    a.Name,
                    Quantity = a.OrderItems?.Sum(o => o.Quantity) ?? 0
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .Select(x => (x.Name, x.Quantity))
                .ToList();
        }

        // 4. ZADNJA NARUDŽBA
        public async Task<Order?> GetLatestOrderWithDetailsAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.LastOrDefault();
        }
    }
}