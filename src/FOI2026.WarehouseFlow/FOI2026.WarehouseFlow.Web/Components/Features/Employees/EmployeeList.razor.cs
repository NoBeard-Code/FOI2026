using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Employees
{
    public partial class EmployeeList
    {
        [Inject]
        public UserService UserService { get; set; }

        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        private List<EmployeeViewModel> sviZaposlenici = new();
        private List<EmployeeViewModel> zaposlenici = new();
        private string aktivniTab = "svi";

        protected override async Task OnInitializedAsync()
        {
            await UcitajZaposlenike();
        }

        private async Task UcitajZaposlenike()
        {
            var users = await UserService.GetAllAsync();
            sviZaposlenici = new List<EmployeeViewModel>();

            foreach (var user in users)
            {
                var roles = await UserManager.GetRolesAsync(user);
                sviZaposlenici.Add(new EmployeeViewModel
                {
                    User = user,
                    Role = roles.FirstOrDefault() ?? "Nema uloge"
                });
            }

            PrimijeniFilter();
        }

        private void PromijeniTab(string tab)
        {
            aktivniTab = tab;
            PrimijeniFilter();
        }

        private int trenutnaStranica = 1;
        private int zaposlenikaPoStranici = 10;

        private List<EmployeeViewModel> ZaposleniziNaTrenutnoStranici =>
            zaposlenici
                .Skip((trenutnaStranica - 1) * zaposlenikaPoStranici)
                .Take(zaposlenikaPoStranici)
                .ToList();

        private int UkupnoStranica =>
            (int)Math.Ceiling((double)zaposlenici.Count / zaposlenikaPoStranici);

        private void IdiNaStranicuu(int stranica)
        {
            trenutnaStranica = stranica;
        }

        private void PrimijeniFilter()
        {
            zaposlenici = aktivniTab switch
            {
                "aktivni" => sviZaposlenici.Where(u => u.User.IsActive).ToList(),
                "deaktivirani" => sviZaposlenici.Where(u => !u.User.IsActive).ToList(),
                _ => sviZaposlenici
            };
            trenutnaStranica = 1;
        }

        private async Task Deaktiviraj(string id)
        {
            await UserService.DeactivateAsync(id);
            await UcitajZaposlenike();
        }

        private async Task Aktiviraj(string id)
        {
            await UserService.ActivateAsync(id);
            await UcitajZaposlenike();
        }

        private void DodajZaposlenika()
        {
            Navigation.NavigateTo("/employees/add");
        }

        private void Uredi(string id)
        {
            Navigation.NavigateTo($"/employees/edit/{id}");
        }

        private async Task Obrisi(string id)
        {
            var potvrda = await JS.InvokeAsync<bool>("confirm", "Jeste li sigurni da želite obrisati zaposlenika?");
            if (!potvrda) return;

            await UserService.DeleteAsync(id);
            await UcitajZaposlenike();
        }


    }
}