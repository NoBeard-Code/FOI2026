using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Partners
{
    public partial class PartnerAdd
    {
        [Inject]
        public PartnerService PartnerService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }
        private string name = string.Empty;
        private string oib = string.Empty;
        private string address = string.Empty;
        private string contact = string.Empty;
        private string email = string.Empty;

        private bool uspjeh = false;
        private string greska = string.Empty;
        private string? successMessage;
        private string? errorMessage;

        private async Task Spremi()
        {
            greska = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
            {
                greska = "Naziv je obavezan.";
                return;
            }

            if (!int.TryParse(oib, out int parsedOib))
            {
                greska = "OIB mora biti broj.";
                return;
            }

            var partner = new Partner
            {
                Name = name,
                OIB = parsedOib,
                Address = address,
                Contact = contact,
                Email = email,
                IsSupplier = true
            };

            await PartnerService.AddSupplierAsync(partner);

            uspjeh = true;

            await PartnerService.AddSupplierAsync(partner);
        }
    }
}