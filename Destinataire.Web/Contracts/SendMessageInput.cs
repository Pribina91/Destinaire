using System;
using System.ComponentModel.DataAnnotations;

namespace Destinataire.Web.Contracts
{
    public class SendMessageInput
    {
        [Required]
        public Guid SenderId { get; set; }
        [Required]
        public Guid RecipientId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}