using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Destinataire.Data;
using Destinataire.Data.Model;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Destinataire.FunctionalTests
{
    public class DestinataireContextSeedForTests
    {
        public static void Seed(DestinaireContext context, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                context.Contacts.RemoveRange(context.Contacts);
                context.Messages.RemoveRange(context.Messages);
                context.SaveChanges();

                context.Contacts.AddRange(GetPredefinedContacts());
                var msgs = GetPredefinedMessages();
                context.Messages.AddRange(msgs);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<DestinataireContextSeedForTests>();
                    log.LogError(ex.Message);
                    Seed(context, loggerFactory, retryForAvailability);
                }

                throw;
            }
        }

        public static IEnumerable<Message> GetPredefinedMessages()
        {
            yield return new Message()
            {
                Id = DestinatairePredefinedIds.MessageIds.AndrejToSpidermanId,
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                ReceiverId = DestinatairePredefinedIds.ContactIds.SpidermanId,
                Text = "You are great at making webs",
                CreatedAt = DateTime.UtcNow,
            };

            yield return new Message()
            {
                Id = DestinatairePredefinedIds.MessageIds.ToBeDeleted,
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                ReceiverId = DestinatairePredefinedIds.ContactIds.SpidermanId,
                Text = "Delete this",
                CreatedAt = DateTime.UtcNow,
            };

            yield return new Message()
            {
                Id = DestinatairePredefinedIds.MessageIds.TestString,
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                ReceiverId = DestinatairePredefinedIds.ContactIds.SpidermanId,
                Text = "Test string lorem ipsum",
                CreatedAt = DateTime.UtcNow,
            };
            yield return new Message()
            {
                Id = DestinatairePredefinedIds.MessageIds.ToBeMarkRead,
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                ReceiverId = DestinatairePredefinedIds.ContactIds.SpidermanId,
                Text = "Mark as read",
                CreatedAt = DateTime.UtcNow,
            };
            yield return new Message()
            {
                Id = DestinatairePredefinedIds.MessageIds.ToBeMarkUnread,
                SenderId = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                ReceiverId = DestinatairePredefinedIds.ContactIds.SpidermanId,
                Text = "Mark as unread",
                ReadAt = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow,
            };

            var random = new Random(100);
            var contactCount = DestinatairePredefinedIds.ContactIds.AllContacts.Count;
            for (int i = 0; i < 1000; i++)
            {
                var senderIndex = random.Next(contactCount);
                var receiverIndex = random.Next(contactCount);
                var createdModifier = (int) (random.NextDouble() * 100000);
                yield return new Message()
                {
                    Id = Guid.NewGuid(),
                    SenderId = DestinatairePredefinedIds.ContactIds.AllContacts[senderIndex],
                    ReceiverId = DestinatairePredefinedIds.ContactIds.AllContacts[receiverIndex],
                    Text = $"Lorem ipsum {i}",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-createdModifier),
                };
            }

            for (var i = 0; i < 100; i++)
            {
                var senderIndex = random.Next(contactCount);
                var receiverIndex = random.Next(contactCount);
                var createdModifier = (int) (random.NextDouble() * 100000);
                var dayModifier = (int) (random.NextDouble() * 1000);
                yield return new Message()
                {
                    Id = Guid.NewGuid(),
                    SenderId = DestinatairePredefinedIds.ContactIds.AllContacts[senderIndex],
                    ReceiverId = DestinatairePredefinedIds.ContactIds.AllContacts[receiverIndex],
                    Text = $"Lorem ipsum {i}",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-createdModifier),
                    ReadAt = DateTime.UtcNow.AddMinutes(-createdModifier + dayModifier),
                };
            }
        }

        public static IEnumerable<Contact> GetPredefinedContacts()
        {
            yield return new Contact()
            {
                Id = DestinatairePredefinedIds.ContactIds.ToBeDeleted,
                Name = "Noone",
                FamilyName = "Nobody",
                Email = "tobe@deleted.com",
            };
            yield return new Contact()
            {
                Id = DestinatairePredefinedIds.ContactIds.ToBeUpdated,
                Name = "Update me",
                FamilyName = "Update my family",
                Email = "email@test.com",
            };

            yield return new Contact()
            {
                Id = DestinatairePredefinedIds.ContactIds.AndrejStajerId,
                Name = "Andrej",
                FamilyName = "Stajer",
                Email = "astajer@pribitech.com",
            };
            yield return new Contact()
            {
                Id = DestinatairePredefinedIds.ContactIds.SpidermanId,
                Name = "Peter",
                FamilyName = "Parker",
                Email = "spiderman@pribitech.com",
            };
            yield return new Contact()
            {
                Id = DestinatairePredefinedIds.ContactIds.IronmanId,
                Name = "Tony",
                FamilyName = "Stark",
                Email = "ironman@pribitech.com",
            };
            yield return new Contact()
            {
                Id = DestinatairePredefinedIds.ContactIds.CaptainId,
                Name = "Steve",
                FamilyName = "Rogers",
                Email = "captain@pribitech.com",
            };
        }
    }
}