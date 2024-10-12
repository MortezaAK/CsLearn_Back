using ApplicationCore.Interfaces.DataAccess;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> PrivateRepository;
        private readonly ApplicationContext db;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        public UnitOfWork(ApplicationContext context, IMapper mapper, IHttpContextAccessor accessor)
        {
            db = context;
            _mapper = mapper;
            _accessor = accessor;
            PrivateRepository = new Dictionary<Type, object>();
        }

        //public async ValueTask DisposeAsync()
        //{
        //    //if (db != null)
        //    //{
        //    await db.DisposeAsync();
        //    //}
        //}

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            object repository;

            PrivateRepository.TryGetValue(typeof(TEntity), out repository);
            if (repository == null)
            {
                repository = new Repository<TEntity>(db, _mapper, _accessor);
                PrivateRepository.Add(typeof(TEntity), repository);
            }

            return (Repository<TEntity>)repository;
        }

        public async Task<bool> Save()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}
