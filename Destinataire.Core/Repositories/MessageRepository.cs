using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Destinataire.Core.Extensions;
using Destinataire.Core.Helpers;
using Destinataire.Core.Interfaces;
using Destinataire.Core.Repositories.Filtering;
using Destinataire.Data;
using Destinataire.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Destinataire.Core.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DestinaireContext context) : base(context)
        {
        }

        public PagedList<Message> GetReceivedMessages(Guid receiverId, int pageIndex, int pageSize)
        {
            return Get(m => m.ReceiverId == receiverId)
                .AsNoTracking()
                .ToPagedList(pageIndex, pageSize);
        }

        public PagedList<Message> GetSentMessages(Guid senderId, int pageIndex, int pageSize)
        {
            return Get(m => m.SenderId == senderId)
                .AsNoTracking()
                .ToPagedList(pageIndex, pageSize);
        }

        

        public PagedList<Message> GetMessages(GetMessagesFilter filter, int pageIndex, int pageSize, string order)
        {
            var query = Get().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(order))
            {
                query = query.SortBy(order);
            }

            if (filter?.From != null)
            {
                query = query.Where(m => m.CreatedAt >= filter.From);

            }
            
            if (filter?.To != null)
            {
                query = query.Where(m => m.CreatedAt <= filter.To);
            }

            if (!string.IsNullOrWhiteSpace(filter?.Search))
            {
                query = query.Where(m => m.Text.Contains(filter.Search)
                                         || m.Sender.Name.Contains(filter.Search)
                                         || m.Sender.FamilyName.Contains(filter.Search)
                                         || m.Sender.Email.Contains(filter.Search)
                                         || m.Receiver.Name.Contains(filter.Search)
                                         || m.Receiver.FamilyName.Contains(filter.Search)
                                         || m.Receiver.Email.Contains(filter.Search));
            }
           
            return query.ToPagedList(pageIndex, pageSize);
        }
    }
}