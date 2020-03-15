using System;
using System.Threading.Tasks;
using Destinataire.Core.Interfaces;
using Destinataire.Core.Services;
using Destinataire.Data.Model;
using Moq;
using Xunit;

namespace Destinataire.UnitTests.DestinaireCore.Services
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;

        public MessageServiceTests()
        {
            _messageRepositoryMock = new Mock<IMessageRepository>();
        }
        
        [Fact]
        public async void SendMessage()
        {
            Message inserted = null;
            _messageRepositoryMock
                .Setup(r => r.Insert(It.IsAny<Message>()))
                .Callback<Message>((m) => inserted = m)
                .Returns(Task.CompletedTask);

            var service = new MessageService(_messageRepositoryMock.Object);

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                SenderId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
                Text = Guid.NewGuid().ToString(),
            };
            
            var result = await service.SendMessage(message);

            _messageRepositoryMock.Verify((repository) => repository.Insert(It.IsAny<Message>()), Times.Once);
            
            Assert.NotNull(result);
            Assert.Equal(message.Id, result.Id);
            Assert.Equal(message.SenderId, result.SenderId);
            Assert.Equal(message.ReceiverId, result.ReceiverId);
            Assert.Equal(message.Text, result.Text);
            Assert.NotNull(inserted);
        }
    }
}
