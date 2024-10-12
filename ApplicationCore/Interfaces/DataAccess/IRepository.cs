using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
