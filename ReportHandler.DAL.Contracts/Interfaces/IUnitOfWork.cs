using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReportHandler.DAL.Contracts.DTO;

namespace ReportHandler.DAL.Contracts.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void AddCustomer(CustomerDTO item);
        void AddItem(ItemDTO item);
        void AddManager(ManagerDTO item);
        void AddOrder(OrderDTO item);
        IEnumerable<CustomerDTO> GetCustomers(Expression<Func<CustomerDTO, bool>> predicate = null);
        IEnumerable<ItemDTO> GetItems(Expression<Func<ItemDTO, bool>> predicate = null);
        IEnumerable<ManagerDTO> GetManagers(Expression<Func<ManagerDTO, bool>> predicate = null);
        IEnumerable<OrderDTO> GetOrders(Expression<Func<OrderDTO, bool>> predicate = null);
        void RemoveCustomer(CustomerDTO item);
        void RemoveItem(ItemDTO item);
        void RemoveManager(ManagerDTO item);
        void RemoveOrder(OrderDTO item);
        void Save();
        void UpdateCustomer(CustomerDTO item);
        void UpdateItem(ItemDTO item);
        void UpdateManager(ManagerDTO item);
        void UpdateOrder(OrderDTO item);
    }
}