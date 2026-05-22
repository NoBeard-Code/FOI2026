using System;
using System.Collections.Generic;
using System.Text;
using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using Microsoft.EntityFrameworkCore;

namespace FOI2026.WarehouseFlow.Infrastructure.Repositories
{
    public class StockHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public StockHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersByArticleIdAsync(int articleId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Where(o => o.OrderItems.Any(oi => oi.ArticleId == articleId))
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryNote>> GetDeliveryNotesByArticleIdAsync(int articleId)
        {
            return await _context.DeliveryNotes
                .Include(d => d.DeliveryNoteItems)
                .Include(d => d.User)
                .Where(d => d.DeliveryNoteItems.Any(di => di.ArticleId == articleId))
                .ToListAsync();
        }
    }
}

