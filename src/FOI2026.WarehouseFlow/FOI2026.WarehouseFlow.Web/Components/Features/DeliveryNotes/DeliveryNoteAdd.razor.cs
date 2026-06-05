using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOI2026.WarehouseFlow.Web.Components.Features.DeliveryNotes
{
    public class DeliveryNoteStavka
    {
        public int? ArtiklId { get; set; }
        public string ArtiklNaziv { get; set; } = string.Empty;
        public string JedinicaMjere { get; set; } = string.Empty;
        public int Kolicina { get; set; } = 1;
        public bool PrikaziDropdown { get; set; } = false;
        public List<Article> FiltriraniArtikli { get; set; } = new();
    }

    public partial class DeliveryNoteAdd
    {
        [Inject]
        public DeliveryNoteService DeliveryNoteService { get; set; }

        [Inject]
        public PartnerService PartnerService { get; set; }

        [Inject]
        public ApplicationDbContext DbContext { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private string generiraniKod = string.Empty;
        private DateTime datum = DateTime.Today;
        private string status = string.Empty;
        private string trenutniUserId = string.Empty;

        private string partnerSearch = string.Empty;
        private int? odabraniPartnerId;
        private bool pokaziPartnerDropdown = false;
        private List<Partner> sviPartneri = new();
        private List<Partner> filtriraniiPartneri = new();

        private List<Article> sviArtikli = new();
        private List<DeliveryNoteStavka> stavke = new();

        private bool uspjeh = false;
        private string greska = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            // Dohvati trenutnog korisnika
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            trenutniUserId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            sviPartneri = (await PartnerService.GetAllSuppliersAsync()).ToList();
            sviArtikli = await DbContext.Articles.ToListAsync();

            // Auto-generiraj sljedeći broj otpremnice
            var postojeciIds = await DbContext.DeliveryNotes.Select(d => d.DeliveryNoteId).ToListAsync();
            int sljedeciId = postojeciIds.Any() ? postojeciIds.Max() + 1 : 1;
            generiraniKod = sljedeciId.ToString();
        }

        private void OnPartnerSearchChanged(ChangeEventArgs e)
        {
            partnerSearch = e.Value?.ToString() ?? string.Empty;
            odabraniPartnerId = null;

            filtriraniiPartneri = string.IsNullOrWhiteSpace(partnerSearch)
                ? new List<Partner>()
                : sviPartneri
                    .Where(p => p.Name.Contains(partnerSearch, StringComparison.OrdinalIgnoreCase))
                    .Take(8)
                    .ToList();

            pokaziPartnerDropdown = filtriraniiPartneri.Any();
        }

        private void OdaberiPartnera(Partner partner)
        {
            odabraniPartnerId = partner.PartnerId;
            partnerSearch = partner.Name;
            filtriraniiPartneri.Clear();
            pokaziPartnerDropdown = false;
        }

        private async Task SakriPartnerDropdown()
        {
            await Task.Delay(150);
            pokaziPartnerDropdown = false;
        }

        private void DodajStavku()
        {
            stavke.Add(new DeliveryNoteStavka());
        }

        private void UkloniStavku(int index)
        {
            stavke.RemoveAt(index);
        }

        private void OnArtiklSearchChanged(ChangeEventArgs e, int index)
        {
            var tekst = e.Value?.ToString() ?? string.Empty;
            stavke[index].ArtiklNaziv = tekst;
            stavke[index].ArtiklId = null;
            stavke[index].JedinicaMjere = string.Empty;

            stavke[index].FiltriraniArtikli = string.IsNullOrWhiteSpace(tekst)
                ? new List<Article>()
                : sviArtikli
                    .Where(a => a.Name.Contains(tekst, StringComparison.OrdinalIgnoreCase) ||
                                a.Code.Contains(tekst, StringComparison.OrdinalIgnoreCase))
                    .Take(8)
                    .ToList();

            stavke[index].PrikaziDropdown = stavke[index].FiltriraniArtikli.Any();
        }

        private void OdaberiArtikl(int index, Article artikl)
        {
            stavke[index].ArtiklId = artikl.ArticleId;
            stavke[index].ArtiklNaziv = artikl.Name;
            stavke[index].JedinicaMjere = artikl.Unit;
            stavke[index].FiltriraniArtikli.Clear();
            stavke[index].PrikaziDropdown = false;
        }

        private async Task SakriArtiklDropdown(int index)
        {
            await Task.Delay(150);
            if (index < stavke.Count)
                stavke[index].PrikaziDropdown = false;
        }

        private async Task Spremi()
        {
            greska = string.Empty;
            uspjeh = false;

            if (odabraniPartnerId == null)
            {
                greska = "Molimo odaberite kupca s popisa.";
                return;
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                greska = "Status je obavezan.";
                return;
            }

            if (!stavke.Any())
            {
                greska = "Dodajte barem jedan artikl.";
                return;
            }

            if (stavke.Any(s => s.ArtiklId == null))
            {
                greska = "Sve stavke moraju imati odabran artikl s popisa.";
                return;
            }

            if (stavke.Any(s => s.Kolicina <= 0))
            {
                greska = "Sve stavke moraju imati količinu veću od 0.";
                return;
            }

            if (string.IsNullOrEmpty(trenutniUserId))
            {
                greska = "Nije moguće identificirati korisnika. Molimo prijavite se ponovo.";
                return;
            }

            var prvaStavka = stavke.First();

            var otpremnica = new DeliveryNote
            {
                Code = generiraniKod,
                Date = datum,
                PartnerId = odabraniPartnerId.Value,
                ArticleId = prvaStavka.ArtiklId!.Value,
                Status = status,
                UserId = trenutniUserId,
                DeliveryNoteItems = stavke.Select(s => new DeliveryNoteItem
                {
                    ArticleId = s.ArtiklId!.Value,
                    Quantity = s.Kolicina,
                    Price = 0
                }).ToList()
            };

            await DeliveryNoteService.AddDeliveryNoteAsync(otpremnica);

            // Regeneriraj broj za eventualnu sljedeću otpremnicu u istoj sesiji
            var noviIds = await DbContext.DeliveryNotes.Select(d => d.DeliveryNoteId).ToListAsync();
            int sljedeciId = noviIds.Any() ? noviIds.Max() + 1 : 1;
            generiraniKod = sljedeciId.ToString();

            uspjeh = true;
            datum = DateTime.Today;
            status = string.Empty;
            partnerSearch = string.Empty;
            odabraniPartnerId = null;
            stavke.Clear();
        }
    }
}
