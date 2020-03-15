using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Destinataire.Core.Helpers;
using Destinataire.Web.Contracts;
using Destinataire.Web.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Xunit;

namespace Destinataire.FunctionalTests.Controllers
{
    [Collection("Ordered")]
    public class MessagesControllerTests : IClassFixture<WebTestFixture>
    {
        public MessagesControllerTests(WebTestFixture factory)
        {
            Client = factory.CreateClient();
            Client.DefaultRequestHeaders.Add("ContentType", "application/json");
        }

        public HttpClient Client { get; set; }

        [Fact]
        public async Task SendMessage()
        {
            var message = new SendMessageInput()
            {
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                RecipientId = DestinatairePredefinedIds.ContactIds.CaptainId,
                Text = "Lorem ipsum",
            };

            var json = JsonConvert.SerializeObject(message);
            var response =
                await Client.PostAsync("/Messages", new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MessageDto>(stringResponse);

            Assert.Equal(message.RecipientId, model.ReceiverId);
            Assert.Equal(message.SenderId, model.SenderId);
            Assert.Equal(message.Text, model.Text);
        }
        
        [Fact]
        public async Task MarkReadMessages()
        {
            var message = new PatchMessagesParameters()
            {
                MessageIds = new List<Guid>() {DestinatairePredefinedIds.MessageIds.ToBeMarkRead},
            };

            var json = JsonConvert.SerializeObject(message);
            var response =
                await Client.PatchAsync("/Messages/markread", new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    
        [Fact]
        public async Task MarkUnreadMessages()
        {
            var message = new PatchMessagesParameters()
            {
                MessageIds = new List<Guid>() {DestinatairePredefinedIds.MessageIds.ToBeMarkUnread},
            };

            var json = JsonConvert.SerializeObject(message);
            var response =
                await Client.PatchAsync("/Messages/markunread", new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task SendMessage_Fail_WrongSender()
        {
            var message = new SendMessageInput()
            {
                SenderId = Guid.Empty,
                RecipientId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                Text = "Lorem ipsum",
            };
            var response = await Client.PostAsync("/Messages",
                new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SendMessage_Fail_WrongRecipient()
        {
            var message = new SendMessageInput()
            {
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                RecipientId = Guid.Empty,
                Text = "Lorem ipsum",
            };
            var response = await Client.PostAsync("/Messages",
                new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetMessages()
        {
            var parameters = new GetMessagesParameters()
            {
                Search = "Test string",
                Order = nameof(MessageDto.ReadAt),
                
            };

            var response =
                await Client.GetAsync(QueryHelpers.AddQueryString("/Messages", parameters.ToDictionary()));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<PagedList<MessageDto>>(stringResponse);

            Assert.NotEmpty(model.Items);
            Assert.Equal(1, model.Pagination.CurrentCount);
            Assert.Collection(model.Items, dto => Assert.Equal(DestinatairePredefinedIds.MessageIds.TestString, dto.Id));
            Assert.All(model.Items, dto => Assert.Contains(parameters.Search, dto.Text));
        }

        [Fact]
        public async Task GetReceivedMessages()
        {
            var parameters = new GetPersonalMessagesParameters()
            {
                ContactId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
            };

            var response =
                await Client.GetAsync(QueryHelpers.AddQueryString("/Messages/received", parameters.ToDictionary()));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<PagedList<MessageDto>>(stringResponse);

            Assert.NotEmpty(model.Items);
            Assert.All(model.Items, dto => Assert.Equal(parameters.ContactId, dto.ReceiverId));
        }

        [Fact]
        public async Task GetSentMessages()
        {
            var parameters = new GetPersonalMessagesParameters()
            {
                ContactId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
            };
            var response =
                await Client.GetAsync(QueryHelpers.AddQueryString("/Messages/sent", parameters.ToDictionary()));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<PagedList<MessageDto>>(stringResponse);

            Assert.NotEmpty(model.Items);
            Assert.All(model.Items, dto => Assert.Equal(parameters.ContactId, dto.SenderId));
        }

        [Fact]
        public async Task GetMessage()
        {
            var response =
                await Client.GetAsync($"/Messages/{DestinatairePredefinedIds.MessageIds.AndrejToSpidermanId}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MessageDto>(stringResponse);

            Assert.Equal(DestinatairePredefinedIds.MessageIds.AndrejToSpidermanId, model.Id);
        }

        [Fact]
        public async Task DeleteMessage()
        {
            var response = await Client.DeleteAsync($"/Messages/{DestinatairePredefinedIds.MessageIds.ToBeDeleted}");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}