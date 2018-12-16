using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReportHandler.DAL.Contracts.DTO;
using ReportHandler.DAL.Contracts.Interfaces;
using ReportHandler.DAL.Models;

namespace ReportHandler.BLL.Models
{
    public class FileHandler
    {
        private readonly Dictionary<Type, object> _lockers = new Dictionary<Type, object>()
        {
            {typeof(ManagerDTO), new object() },
            {typeof(CustomerDTO), new object() },
            {typeof(ItemDTO), new object() }
        };

        private readonly IUnitOfWorkFactory _factory;

        public FileHandler(IUnitOfWorkFactory factory)
        {
            _factory = factory;
        }

        public Task<bool> ParseFile(string path)
        {
            // TODO check if file exists

            return Task<bool>.Factory.StartNew(() =>
            {
                Console.WriteLine($"Start process {path}");

                using (var sr = new StreamReader(path))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        //Thread.Sleep(100);
                        var data = ParseLine(line);

                        // TODO
                        // example how will work
                        if (data.Length < 4 || data.Length > 4)
                        {
                            throw new ArgumentException();
                        }
                        var order = new OrderDTO();
                        order.Date = DateTime.ParseExact(data[0], "dd.MM.yyyy", null);
                        order.Customer = ProcessCustomer(data[1]);
                        order.Item = ProcessItem(data[2]);
                        order.Cost = Convert.ToDecimal(data[3]);

                        order.Manager = ProcessManager("Ivanov");

                        SaveOrder(order);
                    }
                }
                Console.WriteLine("DONE! {0}", path);
                return true;
            }
            );
        }

        private void SaveOrder(OrderDTO order)
        {
            using (var uow = _factory.GetInstance())
            {
                uow.AddOrder(order);
                uow.Save();
            }
        }

        private string[] ParseLine(string line)
        {
            return line.Split(',');
        }

        private ManagerDTO ProcessManager(string managerLine)
        {
            using (var uow = _factory.GetInstance())
            {
                Expression<Func<ManagerDTO, bool>> managerSearchCriteria = x => x.LastName == managerLine;

                var manager = uow.GetManagers(managerSearchCriteria).FirstOrDefault();

                if (manager == null)
                {
                    manager = new ManagerDTO { LastName = managerLine };

                    lock (_lockers[manager.GetType()])
                    {
                        if (uow.GetManagers(managerSearchCriteria).FirstOrDefault() == null)
                        {
                            uow.AddManager(manager);
                            uow.Save();
                        }
                    }
                }

                return uow.GetManagers(managerSearchCriteria).FirstOrDefault();
            }
        }

        private ItemDTO ProcessItem(string itemLine)
        {
            using (var uow = _factory.GetInstance())
            {
                Expression<Func<ItemDTO, bool>> itemSearchCriteria = x => x.Name == itemLine;

                var item = uow.GetItems(itemSearchCriteria).FirstOrDefault();

                if (item == null)
                {
                    item = new ItemDTO() { Name = itemLine };

                    lock (_lockers[item.GetType()])
                    {
                        if (uow.GetItems(itemSearchCriteria).FirstOrDefault() == null)
                        {
                            uow.AddItem(item);
                            uow.Save();
                        }
                    }
                }

                return uow.GetItems(itemSearchCriteria).FirstOrDefault();
            }
        }

        private CustomerDTO ProcessCustomer(string customerLine)
        {
            using (var uow = _factory.GetInstance())
            {
                var name = ParseFullName(customerLine);
                var first = name[0];
                var last = name[1];

                Expression<Func<CustomerDTO, bool>> customerSearchCriteria = (x => x.FirstName == first && x.LastName == last);

                var customer = uow.GetCustomers(customerSearchCriteria).FirstOrDefault();

                if (customer == null)
                {
                    customer = new CustomerDTO() { FirstName = first, LastName = last };

                    lock (_lockers[customer.GetType()])
                    {
                        if (uow.GetCustomers(customerSearchCriteria).FirstOrDefault() == null)
                        {
                            uow.AddCustomer(customer);
                            uow.Save();
                        }
                    }
                }

                return uow.GetCustomers(customerSearchCriteria).FirstOrDefault();
            }
        }

        private string[] ParseFullName(string fullName)
        {
            return fullName.Split(' ');
        }
    }
}
