using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Partners
{
    public partial class PartnerList
    {
        [Inject]
        public PartnerService PartnerService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private List<Partner> partneri = new();
        private List<Partner> sviPartneri = new();

        private string search = string.Empty;

        private int trenutnaStranica = 1;
        private int poStranici = 10;

        protected override async Task OnInitializedAsync()
        {
            await Ucitaj();
        }

        private async Task Ucitaj()
        {
            sviPartneri = (await PartnerService.GetAllSuppliersAsync())
                .ToList();

            partneri = sviPartneri;
        }

        private IEnumerable<Partner> FiltriraniPartneri =>
            string.IsNullOrWhiteSpace(search)
                ? partneri
                : partneri.Where(p =>
                    (p.Name != null && p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));

        private List<Partner> PartneriNaStranici =>
            FiltriraniPartneri
                .Skip((trenutnaStranica - 1) * poStranici)
                .Take(poStranici)
                .ToList();

        private int UkupnoStranica =>
            (int)Math.Ceiling((double)FiltriraniPartneri.Count() / poStranici);

        private void IdiNaStranicu(int str)
        {
            trenutnaStranica = str;
        }

        private void DodajPartnera()
        {
            Navigation.NavigateTo("/suppliers/add");
        }

        private void Uredi(int id)
        {
            Navigation.NavigateTo($"/suppliers/edit/{id}");
        }

        private async Task Obrisi(int id)
        {
            await PartnerService.DeleteSupplierAsync(id);

            sviPartneri = (await PartnerService.GetAllSuppliersAsync()).ToList();
            partneri = sviPartneri;
        }
    }
}