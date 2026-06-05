using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Orders
{
    public partial class OrderList
    {
        [Inject]
        public OrderService OrderService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public ApplicationDbContext DbContext { get; set; }

        private List<Order> narudzbe = new();
        private List<Order> sveNarudzbe = new();

        private string search = string.Empty;

        private int trenutnaStranica = 1;
        private int poStranici = 10;

        protected override async Task OnInitializedAsync()
        {
            await Ucitaj();
        }

        private async Task Ucitaj()
        {
            sveNarudzbe = (await DbContext.Orders
                .Include(o => o.partner)
                .Include(o => o.OrderItems)
                .ToListAsync());

            narudzbe = sveNarudzbe;
        }

        private IEnumerable<Order> FiltriranaNarudzbe =>
            string.IsNullOrWhiteSpace(search)
                ? narudzbe
                : narudzbe.Where(n =>
                    (n.Code != null && n.Code.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (n.partner?.Name != null && n.partner.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));

        private List<Order> NarudzbeNaStranici =>
            FiltriranaNarudzbe
                .Skip((trenutnaStranica - 1) * poStranici)
                .Take(poStranici)
                .ToList();

        private int UkupnoStranica =>
            (int)Math.Ceiling((double)FiltriranaNarudzbe.Count() / poStranici);

        private void IdiNaStranicu(int str)
        {
            trenutnaStranica = str;
        }

        private void DodajNarudzbu()
        {
            Navigation.NavigateTo("/orders/add");
        }

        private void Uredi(int id)
        {
            Navigation.NavigateTo($"/orders/edit/{id}");
        }

        private async Task Obrisi(int id)
        {
            await OrderService.DeleteOrderAsync(id);
            await Ucitaj();
        }
    }
}
