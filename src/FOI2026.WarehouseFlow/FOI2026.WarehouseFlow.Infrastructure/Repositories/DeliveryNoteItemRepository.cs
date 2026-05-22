using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Repositories
{
    public class DeliveryNoteItemRepository : IDeliveryNoteItemRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryNoteItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryNoteItem>> GetAllAsync()
        {
            return await _context.DeliveryNoteItems.ToListAsync();
        }

        public async Task<DeliveryNoteItem?> GetByIdAsync(int id)
        {
            return await _context.DeliveryNoteItems.FindAsync(id);
        }

        public async Task AddAsync(DeliveryNoteItem deliveryNoteItem)
        {
            await _context.DeliveryNoteItems.AddAsync(deliveryNoteItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryNoteItem deliveryNoteItem)
        {
            _context.DeliveryNoteItems.Update(deliveryNoteItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var deliveryNoteItem = await _context.DeliveryNoteItems.FindAsync(id);
            if (deliveryNoteItem != null)
            {
                _context.DeliveryNoteItems.Remove(deliveryNoteItem);
                await _context.SaveChangesAsync();
            }
        }

    }
}
