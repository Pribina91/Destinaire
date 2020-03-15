using System;
using System.Collections.Generic;

namespace Destinataire.Web.Contracts
{
    public class PatchMessagesParameters
    {
        public List<Guid> MessageIds { get; set; }
    }
}