using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Destinataire.Core.Interfaces;
using Destinataire.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Destinataire.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task MarkMessagesAsNotRead(List<Guid> messageIds)
        {
            await _messageRepository.Get(m => messageIds.Contains(m.Id))
                .ForEachAsync(m => m.ReadAt = null);
        }

        public async Task MarkMessagesAsRead(List<Guid> messageIds)
        {
            await _messageRepository.Get(m => messageIds.Contains(m.Id))
                .ForEachAsync(m => m.ReadAt = DateTime.UtcNow);
        }

        public async Task<Message> SendMessage(Guid senderId, Guid recipientId, string text)
        {
            return await SendMessage(new Message()
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                ReceiverId = recipientId,
                CreatedAt = DateTime.UtcNow,
                Text = text,
            });
        }

        public async Task<Message> SendMessage(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            await _messageRepository.Insert(message);

            return message;
        }
    }
}
