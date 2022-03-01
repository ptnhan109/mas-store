using Mas.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Core
{
    public interface IAsyncRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);


        Task DeleteAsync(Guid id);

        Task DeleteAsync(TEntity entity);

        Task DeleteRangeAsync(IEnumerable<Guid> ids);

        Task DeleteRangeAsync(IEnumerable<TEntity> entities);


        Task UpdateAsync(TEntity entity);


        Task<TEntity> FindAsync(Guid id, IEnumerable<string> includes = null);

        Task<TEntity> FindAsync(Expression<Func<TEntity,bool>> where = null, IEnumerable<string> includes = null);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where = null, IEnumerable<string> includes = null, Expression<Func<TEntity, TEntity>> selector = null);

        Task<PagedResult<TEntity>> FindPagedAsync(Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, TEntity>> selector = null, IEnumerable<string> includes = null, int pageIndex = 1, int pageSize = 10);

        Task<PagedResult<TEntity>> FindPagedAsync(IQueryable<TEntity> query,Expression<Func<TEntity,bool>> where = null, int pageIndex = 1, int pageSize = 10);

        IQueryable<TEntity> GetQueryable(IEnumerable<string> includes = null);



    }
}
