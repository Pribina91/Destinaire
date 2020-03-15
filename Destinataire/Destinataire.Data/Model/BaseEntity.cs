using System;
using System.ComponentModel.DataAnnotations;

namespace Destinataire.Data.Model
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}