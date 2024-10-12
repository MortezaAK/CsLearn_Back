//using ApplicationCore.Interfaces.DataAccess;
//using AutoMapper;
//using Infrastructure.DataAccess;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure
//{
//    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
//    {

//        //private readonly DbSet<TEntity> _dbSet;
//        //private readonly IMapper _mapper;
//        //private readonly IHttpContextAccessor _accessor;
//        //private readonly DbContext _dbContext;
//        //private readonly ApplicationContext _context;
//        //public Repository(ApplicationContext _db, IMapper mapper, IHttpContextAccessor accessor)
//        //{
//        //    _context = _db;
//        //    _accessor = accessor;
//        //    _mapper = mapper;
//        //    _dbSet = _context.Set<TEntity>();
//        //}
//        private readonly DbSet<TEntity> _dbSet;
//        private readonly IMapper _mapper;
//        private readonly IHttpContextAccessor _accessor;
//        private readonly DbContext _dbContext;
//        private readonly ApplicationContext _context;
//        public Repository(ApplicationContext context, IMapper mapper, IHttpContextAccessor accessor)
//        {
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//            _accessor = accessor;
//            _mapper = mapper;
//            _dbSet = context.Set<TEntity>();
//        }
//        public async Task<IEnumerable<TEntity>> GetAllAsync()
//        {
//            return await _dbSet.ToListAsync();
//        }

//        public async Task<TEntity> GetByIdAsync(int id)
//        {
//            return await _dbSet.FindAsync(id);
//        }

//        public async Task<bool> AddAsync(TEntity entity)
//        {
//            await _context.AddAsync(entity);
//            var result = await _context.SaveChangesAsync();
//            return result > 0;
//        }


//        //public async Task<bool> AddAsync(TEntity entity)
//        //{

//        //    await _context.AddAsync(entity);

//        //    var result = await _dbContext.SaveChangesAsync();
//        //    if (result > 0)
//        //    {
//        //        return true;
//        //    }
//        //    return false;
//        //}

//        public async Task UpdateAsync(TEntity entity)
//        {
//            _dbSet.Update(entity);
//            await _dbContext.SaveChangesAsync();
//        }

//        public async Task DeleteAsync(int id)
//        {
//            var entity = await GetByIdAsync(id);
//            _dbSet.Remove(entity);
//            await _dbContext.SaveChangesAsync(); ;
//        }

//        //public void Dispose()
//        //{
//        //    GC.SuppressFinalize(this);
//        //}
//    }
//}
using ApplicationCore.Interfaces.DataAccess;
using AutoMapper;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly ApplicationContext _context;

    public Repository(ApplicationContext context, IMapper mapper, IHttpContextAccessor accessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity); // Use _dbSet
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {

         _dbSet.Update(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        _dbSet.Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
