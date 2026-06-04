using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class StockHistoryService
    {
        private readonly IStockHistoryRepository _stockHistoryRepository;

        public StockHistoryService(IStockHistoryRepository stockHistoryRepository)
        {
            _stockHistoryRepository = stockHistoryRepository;
        }

        public async Task<IEnumerable<StockHistoryEntry>> GetStockHistoryAsync(int articleId)
        {
            var entries = new List<StockHistoryEntry>();

            entries.AddRange(await GetOrderEntriesAsync(articleId));
            entries.AddRange(await GetDeliveryNoteEntriesAsync(articleId));

            return CalculateRunningStock(entries);
        }

        public async Task<IEnumerable<StockHistoryEntry>> FilterByUserAsync(int articleId, string userId)
        {
            var history = await GetStockHistoryAsync(articleId);
            return history.Where(e => e.UserId == userId);
        }

        private async Task<IEnumerable<StockHistoryEntry>> GetOrderEntriesAsync(int articleId)
        {
            var orders = await _stockHistoryRepository.GetOrdersByArticleIdAsync(articleId);

            return orders.Select(order => new StockHistoryEntry
            {
                Date = order.Date,
                UserId = order.UserId,
                UserFullName = $"{order.User.FirstName} {order.User.LastName}",
                ChangeType = "-",
                ChangeTypeName = "Otpis",
                Quantity = order.OrderItems.Where(oi => oi.ArticleId == articleId).Sum(oi => oi.Quantity),
                NewStock = 0
            });
        }

        private async Task<IEnumerable<StockHistoryEntry>> GetDeliveryNoteEntriesAsync(int articleId)
        {
            var deliveryNotes = await _stockHistoryRepository.GetDeliveryNotesByArticleIdAsync(articleId);

            return deliveryNotes.Select(deliveryNote => new StockHistoryEntry
            {
                Date = deliveryNote.Date,
                UserId = deliveryNote.UserId,
                UserFullName = $"{deliveryNote.User.FirstName} {deliveryNote.User.LastName}",
                ChangeType = "+",
                ChangeTypeName = "Dodavanje",
                Quantity = deliveryNote.DeliveryNoteItems.Where(di => di.ArticleId == articleId).Sum(di => di.Quantity),
                NewStock = 0
            });
        }

        private IEnumerable<StockHistoryEntry> CalculateRunningStock(List<StockHistoryEntry> entries)
        {
            var sortedEntries = entries.OrderBy(e => e.Date).ToList();

            int runningStock = 0;
            foreach (var entry in sortedEntries)
            {
                runningStock = entry.ChangeType == "+"
                    ? runningStock + entry.Quantity
                    : runningStock - entry.Quantity;

                entry.NewStock = runningStock;
            }

            return sortedEntries.OrderByDescending(e => e.Date);
        }
    }
}