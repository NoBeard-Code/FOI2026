using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Code { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public int MaxStock { get; set; }
        [Required]
        public int MinStock { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
        [Required]
        public string Status { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [NotMapped]
        public int CurrentStock { get; set; }

        public Category Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<DeliveryNoteItem> DeliveryNoteItems { get; set; }

    }
}
