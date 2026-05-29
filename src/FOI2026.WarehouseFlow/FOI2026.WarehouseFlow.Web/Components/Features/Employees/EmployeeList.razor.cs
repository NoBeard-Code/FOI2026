using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Employees
{
    public partial class EmployeeList
    {
        [Inject]
        public UserService UserService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private IEnumerable<ApplicationUser> sviZaposlenici = new List<ApplicationUser>();
        private IEnumerable<ApplicationUser> zaposlenici = new List<ApplicationUser>();
        private string aktivniTab = "svi";

        protected override async Task OnInitializedAsync()
        {
            await UcitajZaposlenike();
        }

        private async Task UcitajZaposlenike()
        {
            sviZaposlenici = await UserService.GetAllAsync();
            PrimijeniFilter();
        }

        private void PromijeniTab(string tab)
        {
            aktivniTab = tab;
            PrimijeniFilter();
        }

        private void PrimijeniFilter()
        {
            zaposlenici = aktivniTab switch
            {
                "aktivni" => sviZaposlenici.Where(u => u.IsActive),
                "deaktivirani" => sviZaposlenici.Where(u => !u.IsActive),
                _ => sviZaposlenici
            };
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
    }
}