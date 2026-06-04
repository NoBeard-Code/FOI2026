using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class DeliveryNoteService
    {
        private readonly IDeliveryNoteRepository _deliveryNoteRepository;

        public DeliveryNoteService(IDeliveryNoteRepository deliveryNoteRepository)
        {
            _deliveryNoteRepository = deliveryNoteRepository;
        }

        public async Task<IEnumerable<DeliveryNote>> GetDeliveryNotesAsync()
        {
           var deliveryNotes =  await _deliveryNoteRepository.GetAllAsync();
            return deliveryNotes;
        }

        public async Task<DeliveryNote?> GetDeliveryNoteByIdAsync(int id)
        {
            if (id < 0)
                return null;

            var deliveryNote = await _deliveryNoteRepository.GetByIdAsync(id);
            return deliveryNote;

        }

        public async Task AddDeliveryNoteAsync(DeliveryNote deliveryNote)
        {
            if(deliveryNote == null)
                return;

           await _deliveryNoteRepository.AddAsync(deliveryNote);
        }

        public async Task UpdateDeliveryNoteAsync(DeliveryNote deliveryNote)
        {
            if(deliveryNote == null)
                return;

            await _deliveryNoteRepository.UpdateAsync(deliveryNote);
        }

        public async Task DeleteDeliveryNoteAsync(int id)
        {
            if(id < 0)
                return;
            await _deliveryNoteRepository.DeleteAsync(id);
        }


    }
}
