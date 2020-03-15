using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Destinataire.Data.Model
{
    public class Contact : BaseEntity
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [MinLength(1)]
        public string FamilyName { get; set; }

        public string Email { get; set; }
    }
}
