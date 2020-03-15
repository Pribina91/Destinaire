using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Destinataire.Core.Helpers;
using Destinataire.Data.Model;
using Destinataire.Web.Dtos;

namespace Destinataire.Web.MappingProfiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDto>()
                .ReverseMap();
            CreateMap<PagedList<Contact>, PagedList<ContactDto>>();
        }
    }
}
