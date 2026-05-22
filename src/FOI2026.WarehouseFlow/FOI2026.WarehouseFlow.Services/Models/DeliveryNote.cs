using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FOI2026.WarehouseFlow.Infrastructure.Data.Models
{
    public class DeliveryNote
    {
        [Key]
        public int DeliveryNoteId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [ForeignKey("Partner")]
        public int PartnerId { get; set; }
        
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Required, StringLength(100)]
        public string Status { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Partner Partner { get; set; }
        public virtual Article Article { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<DeliveryNoteItem> DeliveryNoteItems { get; set; }


    }
}
