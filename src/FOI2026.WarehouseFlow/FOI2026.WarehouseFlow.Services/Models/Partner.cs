using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class Partner
    {
        [Key]
        public int PartnerId { get; set; }

        [Required, MinLength(11), MaxLength(11)]
        public int OIB { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? Contact { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsSupplier { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<DeliveryNote> DeliveryNotes { get; set; }
    }
}
