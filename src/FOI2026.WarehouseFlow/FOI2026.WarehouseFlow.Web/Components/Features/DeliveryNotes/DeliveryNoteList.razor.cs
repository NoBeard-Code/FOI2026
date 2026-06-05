using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FOI2026.WarehouseFlow.Web.Components.Features.DeliveryNotes
{
    public partial class DeliveryNoteList
    {
        [Inject]
        public DeliveryNoteService DeliveryNoteService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public ApplicationDbContext DbContext { get; set; }

        private List<DeliveryNote> otpremnice = new();
        private List<DeliveryNote> sveOtpremnice = new();

        private string search = string.Empty;

        private int trenutnaStranica = 1;
        private int poStranici = 10;

        protected override async Task OnInitializedAsync()
        {
            await Ucitaj();
        }

        private async Task Ucitaj()
        {
            sveOtpremnice = await DbContext.DeliveryNotes
                .Include(d => d.Partner)
                .Include(d => d.DeliveryNoteItems)
                .ToListAsync();

            otpremnice = sveOtpremnice;
        }

        private IEnumerable<DeliveryNote> FiltriraneOtpremnice =>
            string.IsNullOrWhiteSpace(search)
                ? otpremnice
                : otpremnice.Where(o =>
                    (o.Code != null && o.Code.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (o.Partner?.Name != null && o.Partner.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));

        private List<DeliveryNote> OtpremniceNaStranici =>
            FiltriraneOtpremnice
                .Skip((trenutnaStranica - 1) * poStranici)
                .Take(poStranici)
                .ToList();

        private int UkupnoStranica =>
            (int)Math.Ceiling((double)FiltriraneOtpremnice.Count() / poStranici);

        private void IdiNaStranicu(int str)
        {
            trenutnaStranica = str;
        }

        private void DodajOtpremnicu()
        {
            Navigation.NavigateTo("/delivery-notes/add");
        }

        private void Uredi(int id)
        {
            Navigation.NavigateTo($"/delivery-notes/edit/{id}");
        }

        private async Task Obrisi(int id)
        {
            await DeliveryNoteService.DeleteDeliveryNoteAsync(id);
            await Ucitaj();
        }
    }
}
