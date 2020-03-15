using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Destinataire.Core.Extensions;
using Destinataire.Core.Helpers;
using Destinataire.Core.Interfaces;
using Destinataire.Data;
using Destinataire.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Destinataire.Core.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(DestinaireContext context) : base(context)
        {
        }

        public PagedList<Contact> GetAll(int pageIndex, int pageSize, string sortOrder)
        {
            return this.Get()
                .AsNoTracking()
                .ToPagedList(pageIndex, pageSize);
        }
    }
}
