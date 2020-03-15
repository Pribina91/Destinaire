using Destinataire.Core.Helpers;
using Destinataire.Data.Model;

namespace Destinataire.Core.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        PagedList<Contact> GetAll(int pageIndex, int pageSize, string sortOrder);
    }
}