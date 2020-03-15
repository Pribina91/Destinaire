using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Destinataire.Core.Extensions;
using Destinataire.Core.Helpers;
using Destinataire.Core.Interfaces;
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

        

        public PagedList<Message> GetMessages(string filter, int pageIndex, int pageSize, string order)
        {
            var query = Get().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(order))
            {
                query = query.SortBy(order);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(m => m.Text.Contains(filter)
                                         || m.Sender.Name.Contains(filter)
                                         || m.Sender.FamilyName.Contains(filter)
                                         || m.Sender.Email.Contains(filter)
                                         || m.Receiver.Name.Contains(filter)
                                         || m.Receiver.FamilyName.Contains(filter)
                                         || m.Receiver.Email.Contains(filter));
            }
           
            return query.ToPagedList(pageIndex, pageSize);
        }
    }
}