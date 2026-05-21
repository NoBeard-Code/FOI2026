using Microsoft.AspNetCore.Identity;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool isActive { get; set; }

    }

}
