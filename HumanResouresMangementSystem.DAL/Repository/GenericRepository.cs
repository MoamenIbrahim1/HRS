using HumanResouresMangementSystem.Core.Interfaces;
using HumanResouresMangementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HumanResouresMangementSystem.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public IQueryable<T> GetQueryable()
            => _dbSet.AsQueryable();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);
    }
}
