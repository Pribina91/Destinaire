using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Destinataire.Data.Model;

namespace Destinataire.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task Delete(TEntity entityToDelete);
        Task Delete(Guid id);
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<TEntity> GetByID(Guid id);

        Task Insert(TEntity entity);
    }
}