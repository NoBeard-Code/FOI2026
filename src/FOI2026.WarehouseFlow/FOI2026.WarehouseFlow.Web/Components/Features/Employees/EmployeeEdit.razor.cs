using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Employees
{
    public partial class EmployeeEdit
    {
        [Parameter]
        public string Id { get; set; }

        [Inject]
        public UserService UserService { get; set; }

        [Inject]
        public RoleService RoleService { get; set; }

        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string username = string.Empty;
        private string email = string.Empty;
        private string selectedRole = string.Empty;
        private bool uspjeh = false;
        private string greska = string.Empty;

        private IEnumerable<ApplicationRole> roles = new List<ApplicationRole>();

        protected override async Task OnInitializedAsync()
        {
            roles = await RoleService.GetAllRolesAsync();

            var user = await UserManager.FindByIdAsync(Id);
            if (user == null)
            {
                Navigation.NavigateTo("/employees");
                return;
            }

            firstName = user.FirstName;
            lastName = user.LastName;
            username = user.UserName ?? string.Empty;
            email = user.Email ?? string.Empty;

            var userRoles = await UserManager.GetRolesAsync(user);
            selectedRole = userRoles.FirstOrDefault() ?? string.Empty;
        }

        private async Task Spremi()
        {
            uspjeh = false;
            greska = string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(selectedRole))
            {
                greska = "Sva polja su obavezna.";
                return;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    greska = "Email adresa nije ispravnog formata.";
                    return;
                }
            }
            catch
            {
                greska = "Email adresa nije ispravnog formata.";
                return;
            }

            var user = await UserManager.FindByIdAsync(Id);
            if (user == null)
            {
                greska = "Zaposlenik nije pronađen.";
                return;
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.UserName = username;
            user.Email = email;

            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                greska = string.Join(", ", result.Errors.Select(e => e.Description));
                return;
            }

            // Ažuriraj ulogu
            var trenutneUloge = await UserManager.GetRolesAsync(user);
            await UserManager.RemoveFromRolesAsync(user, trenutneUloge);
            await UserManager.AddToRoleAsync(user, selectedRole);

            uspjeh = true;
        }
    }
}