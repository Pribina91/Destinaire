using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Destinataire.Web.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Destinataire.Web.Controllers
{
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public abstract class DestinaireBaseController : ControllerBase
    {
    }
}