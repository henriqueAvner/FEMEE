using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FEMEE.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly FemeeDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(FemeeDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            if(pageNumber < 1) pageNumber = 1;
            if(pageSize < 1) pageSize = 10;
            if(pageSize > 100) pageSize = 100;

            var skip = (pageNumber - 1) * pageSize;
            return await _dbSet.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            await _dbSet.AddRangeAsync(entities);

        }
        public async Task UpdateAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if(entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

    }
}