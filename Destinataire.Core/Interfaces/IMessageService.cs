using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Destinataire.Data.Model;

namespace Destinataire.Core.Interfaces
{
    public interface IMessageService
    {
        Task MarkMessagesAsNotRead(List<Guid> messageIds);
        Task MarkMessagesAsRead(List<Guid> messageIds);
        Task<Message> SendMessage(Guid senderId, Guid recipientId, string text);
        Task<Message> SendMessage(Message message);
    }
}