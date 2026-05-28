using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FOI2026.WarehouseFlow.Services.Services
{
    public class PartnerService
    {
        private readonly IPartnerRepository _partnerRepository;

        public PartnerService(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }

        public async Task<IEnumerable<Partner>> GetAllSuppliersAsync()
        {
            return await _partnerRepository.GetAllAsync();
        }

        public async Task AddSupplierAsync(Partner partner)
        {
            partner.IsSupplier = true;
            await _partnerRepository.AddAsync(partner);
        }

        public async Task UpdateSupplierAsync(Partner partner)
        {
            partner.IsSupplier = true;
            await _partnerRepository.UpdateAsync(partner);
        }
    }
}
