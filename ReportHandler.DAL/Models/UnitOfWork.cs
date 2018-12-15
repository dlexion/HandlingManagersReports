using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using ReportHandler.DAL.AutoMapperSetup;
using ReportHandler.DAL.Contracts.DTO;
using ReportHandler.DAL.Contracts.Interfaces;
using ReportHandler.DAL.Extensions;
using ReportHandler.DAL.Repositories;

namespace ReportHandler.DAL.Models
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IGenericRepository<Customer> _customers;
        private readonly IGenericRepository<Item> _items;
        private readonly IGenericRepository<Manager> _managers;
        private readonly IGenericRepository<Order> _orders;

        public UnitOfWork()
        {
            //AutoMapperConfiguration.Configure();

            _context = new ReportModelContainer();

            _customers = new GenericRepository<Customer>(_context);
            _items = new GenericRepository<Item>(_context);
            _managers = new GenericRepository<Manager>(_context);
            _orders = new GenericRepository<Order>(_context);
        }

        #region Items methods

        public IEnumerable<ItemDTO> GetItems(Expression<Func<ItemDTO, bool>> predicate = null)
        {
            var newExpression = predicate?.Project<ItemDTO, Item>();

            var result = _items.Get(newExpression);

            return Mapper.Map<IEnumerable<ItemDTO>>(result);
        }

        public void AddItem(ItemDTO item)
        {
            var entity = Mapper.Map<Item>(item);

            _items.Add(entity);
        }

        public void RemoveItem(ItemDTO item)
        {
            var entity = Mapper.Map<Item>(item);

            _items.Remove(entity);
        }

        public void UpdateItem(ItemDTO item)
        {
            var entity = Mapper.Map<Item>(item);

            _items.Update(entity);
        }

        #endregion

        #region Customers methods

        public IEnumerable<CustomerDTO> GetCustomers(Expression<Func<CustomerDTO, bool>> predicate = null)
        {
            var newExpression = predicate?.Project<CustomerDTO, Customer>();

            var result = _customers.Get(newExpression);

            return Mapper.Map<IEnumerable<CustomerDTO>>(result);
        }

        public void AddCustomer(CustomerDTO item)
        {
            var entity = Mapper.Map<Customer>(item);

            _customers.Add(entity);
        }

        public void RemoveCustomer(CustomerDTO item)
        {
            var entity = Mapper.Map<Customer>(item);

            _customers.Remove(entity);
        }

        public void UpdateCustomer(CustomerDTO item)
        {
            var entity = Mapper.Map<Customer>(item);

            _customers.Update(entity);
        }

        #endregion

        #region Managers methods

        public IEnumerable<ManagerDTO> GetManagers(Expression<Func<ManagerDTO, bool>> predicate = null)
        {
            var newExpression = predicate?.Project<ManagerDTO, Manager>();

            var result = _managers.Get(newExpression);

            return Mapper.Map<IEnumerable<ManagerDTO>>(result);
        }

        public void AddManager(ManagerDTO item)
        {
            var entity = Mapper.Map<Manager>(item);

            _managers.Add(entity);
        }

        public void RemoveManager(ManagerDTO item)
        {
            var entity = Mapper.Map<Manager>(item);

            _managers.Remove(entity);
        }

        public void UpdateManager(ManagerDTO item)
        {
            var entity = Mapper.Map<Manager>(item);

            _managers.Update(entity);
        }

        #endregion

        #region Orders methods

        public IEnumerable<OrderDTO> GetOrders(Expression<Func<OrderDTO, bool>> predicate = null)
        {
            var newExpression = predicate?.Project<OrderDTO, Order>();

            var result = _orders.Get(newExpression);

            return Mapper.Map<IEnumerable<OrderDTO>>(result);
        }

        public void AddOrder(OrderDTO item)
        {
            var entity = Mapper.Map<Order>(item);

            _orders.Add(entity);
        }

        public void RemoveOrder(OrderDTO item)
        {
            var entity = Mapper.Map<Order>(item);

            _orders.Remove(entity);
        }

        public void UpdateOrder(OrderDTO item)
        {
            var entity = Mapper.Map<Order>(item);

            _orders.Update(entity);
        }

        #endregion

        public void Save()
        {
            _items.Save();
            _customers.Save();
            _managers.Save();
            _orders.Save();
        }

        // TODO
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}