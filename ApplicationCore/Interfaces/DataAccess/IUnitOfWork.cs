using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.DataAccess
{
    public interface IUnitOfWork
    {
       
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<bool> Save();
    }
}
