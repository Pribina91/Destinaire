using System;
using System.Collections.Generic;

namespace Destinataire.FunctionalTests
{
    public class DestinatairePredefinedIds
    {
        public class ContactIds
        {
            public static readonly List<Guid> AllContacts = new List<Guid>()
            {
                AndrejStajerId, SpidermanId, IronmanId, CaptainId
            };

            public static Guid AndrejStajerId => Guid.Parse("6678DACB-6C28-4A73-B6BF-58B99E93E38D");
            public static Guid SpidermanId => Guid.Parse("8656FD28-81CE-4729-B9D6-52CB61A2F675");
            public static Guid IronmanId => Guid.Parse("8AC01A45-4CB8-44A1-9916-9E9B55067248");
            public static Guid CaptainId => Guid.Parse("41B3B04D-13B1-4D95-ABC0-952BDB40B749");
            public static Guid ToBeDeleted => Guid.Parse("2060A26A-7A4C-4A7E-87FE-C7705D6D89D6");
            public static Guid ToBeUpdated => Guid.Parse("3EE5C9A9-9B5F-4EA9-8E4A-93D2A01F662B");
        }

        public class MessageIds
        {
            public static Guid AndrejToSpidermanId => Guid.Parse("8E3AF951-7F28-46A1-9194-6DC51AE850EB");
            public static Guid ToBeDeleted => Guid.Parse("BB505FC0-D8F1-45CA-8F1B-A9BF1A45B89B");
            public static Guid ToBeMarkRead => Guid.Parse("86C32F95-7280-436A-AF1A-D86676D212F6");
            public static Guid ToBeMarkUnread => Guid.Parse("93B5BD4A-8E15-4119-BB9E-5CF55D64F9F5");
            public static Guid TestString => Guid.Parse("103D374A-5FC2-4229-9BA0-7B703349B691");
        }
    }
}