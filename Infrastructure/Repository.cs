using ApplicationCore.Interfaces.DataAccess;
using AutoMapper;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly ApplicationContext _context;

    public Repository(ApplicationContext context, IMapper mapper, IHttpContextAccessor accessor)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<TEntity>();
    }

    // متد برای گرفتن تمام موجودیت‌ها
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // متد برای گرفتن یک موجودیت بر اساس شناسه
    public async Task<TEntity> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    // متد برای اضافه کردن یک موجودیت
    public async Task<bool> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    // متد برای اضافه کردن چندین موجودیت به صورت همزمان
    public async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    // متد برای به‌روزرسانی یک موجودیت
    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    // متد برای حذف یک موجودیت بر اساس شناسه
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    // متد برای حذف چندین موجودیت به صورت همزمان
    public async Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    // متد برای جستجو در موجودیت‌ها بر اساس شرط خاص
    public async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
    {
        return await Task.Run(() => _dbSet.Where(predicate).ToList());
    }
}
