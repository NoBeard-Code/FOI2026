using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }


        [StringLength(100),Required]
        public string Name { get; set; }
        
        [StringLength(300)]
        public string? Description { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
