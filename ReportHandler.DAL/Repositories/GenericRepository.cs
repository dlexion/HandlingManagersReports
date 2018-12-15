using ReportHandler.DAL.Contracts.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

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
            // TODO convert expression because t is DTO

            return predicate != null 
                ? _dbSet.Where(predicate) 
                : _dbSet;
        }

        public void Add(T item)
        {
            var entity = Mapper.Map<T>(item);

            _dbSet.Add(entity);
        }

        public void Remove(T item)
        {
            var entity = Mapper.Map<T>(item);

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public void Update(T item)
        {
            var entity = Mapper.Map<T>(item);

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
