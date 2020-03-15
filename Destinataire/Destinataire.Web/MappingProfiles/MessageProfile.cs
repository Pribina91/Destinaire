using AutoMapper;
using Destinataire.Core.Helpers;
using Destinataire.Data.Model;
using Destinataire.Web.Dtos;

namespace Destinataire.Web.MappingProfiles
{
    public class MessageProfile : Profile
    { 
        public MessageProfile()
        {
            CreateMap<Message, MessageDto>()
                .ReverseMap();
            CreateMap<PagedList<Message>, PagedList<MessageDto>>();
        }
    }
}