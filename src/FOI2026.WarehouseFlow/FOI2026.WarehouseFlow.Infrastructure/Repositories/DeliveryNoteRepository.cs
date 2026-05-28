using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Repositories
{
    public class DeliveryNoteRepository : IDeliveryNoteRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryNoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryNote>> GetAllAsync()
        {
            return await _context.DeliveryNotes
                .Include(d => d.DeliveryNoteItems)
                .ToListAsync();
        }

        public async Task<DeliveryNote?> GetByIdAsync(int id)
        {
            return await _context.DeliveryNotes
                .Include(d => d.DeliveryNoteItems)
                .FirstOrDefaultAsync(d => d.DeliveryNoteId == id);
        }

        public async Task AddAsync(DeliveryNote deliveryNote)
        {
            await _context.DeliveryNotes.AddAsync(deliveryNote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DeliveryNote deliveryNote)
        {
            _context.DeliveryNotes.Update(deliveryNote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var deliveryNote = await _context.DeliveryNotes.FindAsync(id);
            if (deliveryNote != null)
            {
                _context.DeliveryNotes.Remove(deliveryNote);
                await _context.SaveChangesAsync();
            }
        }
    }
}
