using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApplicationCore.Interfaces.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(long id);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate);
        Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities);
        Task<bool> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(params string[] includes);
        IQueryable<TEntity> Include(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
    }
}
