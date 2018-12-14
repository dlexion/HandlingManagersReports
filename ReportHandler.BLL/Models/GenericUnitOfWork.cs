using AutoMapper;
using ReportHandler.BLL.Extensions;
using ReportHandler.DAL;
using ReportHandler.DAL.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportHandler.BLL.Models
{
    public class GenericUnitOfWork<TEntity, TModel> where TEntity : class
    {
        private DbContext _context;
        private ReaderWriterLockSlim _locker;
        private IGenericRepository<TEntity> _repository;

        public GenericUnitOfWork(DbContext context, ReaderWriterLockSlim locker)
        {
            _context = context;
            _locker = locker;
            // TODO Factory
            //_repository = new GenericRepository<TEntity>(_context);
        }

        public TModel TryGet(Expression<Func<TModel, bool>> searchExpression)
        {
            var newExpression = searchExpression.Project<TModel, TEntity>();

            _locker.EnterReadLock();

            TEntity user = null;

            try
            {
                user = _repository.Get().FirstOrDefault(newExpression);
                return Mapper.Map<TModel>(user);
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public void TryAdd(TModel customer, Expression<Func<TModel, bool>> searchExpression)
        {
            _locker.EnterWriteLock();
            try
            {
                if (TryGet(searchExpression) == null)
                {
                    _repository.Add(Mapper.Map<TEntity>(customer));
                    _repository.Save();
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
    }
}
