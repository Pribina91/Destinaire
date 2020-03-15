using System;

namespace Destinataire.Web.Contracts
{
    public class GetMessagesParameters : QueryStringParameters
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}