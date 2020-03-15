using System;
using System.Collections.Generic;

namespace Destinataire.Web.Contracts
{
    public class GetPersonalMessagesParameters : QueryStringParameters
    {
        public Guid ContactId { get; set; }

        public override Dictionary<string, string> ToDictionary()
        {
            var dic = base.ToDictionary();
            dic.Add(nameof(ContactId), ContactId.ToString());

            return dic;
        }
    }
}