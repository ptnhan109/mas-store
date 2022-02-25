using Mas.Common;
using Mas.Core.AppDbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Core
{
    public class AsyncRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;

        public AsyncRepository(AppDbContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(c => c.Id == id);
            if (!Equals(entity, null))
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entities = _context.Set<TEntity>().Where(c => ids.Contains(c.Id));
            if(entities.Count() > 0)
            {
                _context.Set<TEntity>().RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null)
        {
            var query = GetQueryable(includes);
            if(where != null)
            {
                query = query.Where(where);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> FindAsync(Guid id, IEnumerable<string> includes = null)
        {
            var query = GetQueryable(includes);

            return await query.FirstOrDefaultAsync();
        }

        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null)
        {
            var query = GetQueryable(includes).Where(where);
        }

        public virtual Task<PagedResult<TEntity>> FindPagedAsync(Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, TEntity>> selector = null, IEnumerable<string> includes = null, int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PagedResult<TEntity>> FindPagedAsync(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> where = null, int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetQueryable(IEnumerable<string> includes = null)
        {
            var queryable = _context.Set<TEntity>();
            if (includes != null)
            {
                foreach(var include in includes)
                {
                    queryable.Include(include);
                }
            }

            return queryable;
        }
    }
}
