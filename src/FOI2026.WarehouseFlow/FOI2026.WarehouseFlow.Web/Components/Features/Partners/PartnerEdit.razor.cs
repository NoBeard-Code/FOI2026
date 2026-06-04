using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Partners
{
    public partial class PartnerEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public PartnerService PartnerService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private Partner partner = new();
        private string greska = string.Empty;
        private bool uspjeh = false;

        protected override async Task OnInitializedAsync()
        {
            partner = (await PartnerService.GetAllSuppliersAsync())
                .FirstOrDefault(x => x.PartnerId == Id);

            if (partner == null)
            {
                Navigation.NavigateTo("/suppliers");
            }
        }

        private async Task Spremi()
        {
            greska = string.Empty;

            if (string.IsNullOrWhiteSpace(partner.Name))
            {
                greska = "Naziv je obavezan.";
                return;
            }

            await PartnerService.UpdateSupplierAsync(partner);

            uspjeh = true;

            Navigation.NavigateTo("/suppliers");
        }
    }
}