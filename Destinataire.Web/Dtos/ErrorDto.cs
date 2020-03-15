using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Destinataire.Web.Dtos
{
    public class ErrorDto
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
