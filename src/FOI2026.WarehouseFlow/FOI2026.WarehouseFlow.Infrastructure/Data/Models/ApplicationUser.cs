using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }
       
        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public bool IsActive { get; set; }

    }

}
