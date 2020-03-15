using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Destinataire.Core.Helpers;
using Destinataire.Core.Repositories.Filtering;
using Destinataire.Data.Model;

namespace Destinataire.Core.Interfaces
{
    public interface IMessageRepository :  IRepository<Message>
    {
        PagedList<Message> GetReceivedMessages(Guid receiverId, int pageIndex, int pageSize);
        PagedList<Message> GetSentMessages(Guid senderId, int pageIndex, int pageSize);
        PagedList<Message> GetMessages(GetMessagesFilter filter, int pageIndex, int pageSize, string order);
    }
}
