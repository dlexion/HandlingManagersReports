using System;
using System.Linq;
using System.Linq.Expressions;

namespace ReportHandler.DAL.Contracts.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);

        void Add(T item);

        void Remove(T item);

        void Update(T item);

        void Save();
    }
}
