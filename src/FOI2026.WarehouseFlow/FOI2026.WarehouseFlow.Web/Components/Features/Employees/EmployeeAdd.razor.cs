using FOI2026.WarehouseFlow.Infrastructure.Data.Models;
using FOI2026.WarehouseFlow.Services.Models;
using FOI2026.WarehouseFlow.Services.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace FOI2026.WarehouseFlow.Web.Components.Features.Employees
{
    public partial class EmployeeAdd
    {
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
        private string email = string.Empty;
        private string password = string.Empty;
        private string selectedRole = string.Empty;
        private bool uspjeh = false;
        private string greska = string.Empty;

        private IEnumerable<ApplicationRole> roles = new List<ApplicationRole>();

        protected override async Task OnInitializedAsync()
        {
            roles = await RoleService.GetAllRolesAsync();
        }

        private async Task Spremi()
        {
            uspjeh = false;
            greska = string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(selectedRole))
            {
                greska = "Sva polja su obavezna.";
                return;
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                EmailConfirmed = true
            };

            var result = await UserManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                greska = string.Join(", ", result.Errors.Select(e => e.Description));
                return;
            }

            await RoleService.AssignRoleAsync(user.Id, selectedRole);

            uspjeh = true;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            password = string.Empty;
            selectedRole = string.Empty;
        }
    }
}