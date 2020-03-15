using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Destinataire.Data.Model
{
   public class Message : BaseEntity
    {
        [Required]
        public Guid SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public Contact Sender { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public Contact Receiver { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }

    }
}
