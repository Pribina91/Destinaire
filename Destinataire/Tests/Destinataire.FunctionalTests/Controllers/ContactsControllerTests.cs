using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Destinataire.Core.Helpers;
using Destinataire.Data.Model;
using Destinataire.Web.Dtos;
using Newtonsoft.Json;
using Xunit;

namespace Destinataire.FunctionalTests.Controllers
{
    [Collection("Ordered")]
    public class ContactsControllerTests : IClassFixture<WebTestFixture>
    {
        public ContactsControllerTests(WebTestFixture factory)
        {
            Client = factory.CreateClient();
            Client.DefaultRequestHeaders.Add("ContentType", "application/json");
        }

        public HttpClient Client { get; set; }

        [Fact]
        public async Task ReturnFirstContacts()
        {
            var response = await Client.GetAsync("/Contacts");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<PagedList<ContactDto>>(stringResponse);

            Assert.NotEqual(0, model.Pagination.CurrentCount);
            Assert.Equal(model.Pagination.CurrentCount, model.Items.Count());
        }

        [Fact]
        public async Task ReturnSpecificContact()
        {
            var response = await Client.GetAsync($"/Contacts/{DestinatairePredefinedIds.ContactIds.SpidermanId}");
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ContactDto>(stringResponse);

            Assert.Equal("spiderman@pribitech.com", model.Email);
            Assert.Equal("Peter", model.Name);
            Assert.Equal("Parker", model.FamilyName);
        }

        [Fact]
        public async Task CreateContact()
        {
            var newContact = new ContactDto()
            {
                Id = Guid.NewGuid(),
                Email = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                FamilyName = Guid.NewGuid().ToString(),
            };
            var response = await Client.PostAsync($"/Contacts",
                new StringContent(JsonConvert.SerializeObject(newContact), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ContactDto>(stringResponse);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(newContact.Id, model.Id);
            Assert.Equal(newContact.Email, model.Email);
            Assert.Equal(newContact.Name, model.Name);
            Assert.Equal(newContact.FamilyName, model.FamilyName);
        }

        [Fact]
        public async Task UpdateContact()
        {
            var updated = new ContactDto()
            {
                Id = DestinatairePredefinedIds.ContactIds.ToBeUpdated,
                Email = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                FamilyName = Guid.NewGuid().ToString(),
            };
            var response = await Client.PutAsync($"/Contacts/{DestinatairePredefinedIds.ContactIds.ToBeUpdated}",
                new StringContent(JsonConvert.SerializeObject(updated), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }


        [Fact]
        public async Task DeleteContact()
        {
            var response = await Client.DeleteAsync($"/Contacts/{DestinatairePredefinedIds.ContactIds.ToBeDeleted}");
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task FailDeleteContact_404()
        {
            var response = await Client.DeleteAsync($"/Contacts/8656FD28-81CE-4729-B9D6-52CB61A2F677");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}