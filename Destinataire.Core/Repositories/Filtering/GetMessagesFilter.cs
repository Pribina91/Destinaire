using System;

namespace Destinataire.Core.Repositories.Filtering
{
    public class GetMessagesFilter
    {
        public string Search { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}