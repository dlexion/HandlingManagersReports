using ReportHandler.DAL.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace ReportHandler.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        private DbContext _context;

        public GenericRepository(DbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null 
                ? _dbSet.Where(predicate) 
                : _dbSet;
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
        }

        public void Remove(T item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _dbSet.Attach(item);
            }
            _dbSet.Remove(item);
        }

        public void Update(T item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
