using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Destinataire.Core.Interfaces;
using Destinataire.Data;
using Destinataire.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Destinataire.Core.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected internal DestinaireContext Context;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(DestinaireContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual async Task<TEntity> GetByID(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task Delete(Guid id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual async Task Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }
    }
}