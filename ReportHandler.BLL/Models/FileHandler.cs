using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReportHandler.DAL.Contracts.DTO;
using ReportHandler.DAL.Contracts.Interfaces;

namespace ReportHandler.BLL.Models
{
    public class FileHandler
    {
        private const char CsvSeparator = ',';
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

        public Task ParseFile(string path, string folderForProcessedFile)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                return null;
            }

            return Task.Factory.StartNew(() =>
            {
                using (var sr = new StreamReader(path))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        var data = ParseLine(line, CsvSeparator);

                        if (data.Length < 4 || data.Length > 4)
                        {
                            break;
                        }

                        var order = new OrderDTO();
                        DateTime.TryParseExact(data[0], "dd.MM.yyyy", null,
                            DateTimeStyles.None, out var dateTime);
                        order.Date = dateTime;
                        order.Customer = ProcessCustomer(data[1]);
                        order.Item = ProcessItem(data[2]);
                        order.Cost = Convert.ToDecimal(data[3]);
                        order.Manager = ProcessManager(fileInfo.Name);

                        SaveOrder(order);
                    }
                }

                MoveFile(path, folderForProcessedFile);
            });
        }

        private void MoveFile(string path, string destinationFolder)
        {
            var sourceInfo = new FileInfo(path);

            var destinationPath = destinationFolder + $"\\{sourceInfo.Name}";

            var destinationInfo = new FileInfo(destinationPath);

            if (destinationInfo.Exists)
            {
                destinationInfo.Delete();
            }

            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            sourceInfo.MoveTo(destinationPath);
        }

        private void SaveOrder(OrderDTO order)
        {
            using (var unitOfWork = _factory.GetInstance())
            {
                unitOfWork.AddOrder(order);
                unitOfWork.Save();
            }
        }

        private string[] ParseLine(string line, char separator)
        {
            return line.Split(separator);
        }

        private ManagerDTO ProcessManager(string managerLine)
        {
            using (var unitOfWork = _factory.GetInstance())
            {
                var customerInfo = ParseLine(managerLine, '_');
                var lastName = customerInfo[0];

                Expression<Func<ManagerDTO, bool>> managerSearchCriteria = x => x.LastName == lastName;

                var manager = unitOfWork.GetManagers(managerSearchCriteria).FirstOrDefault();

                if (manager == null)
                {
                    manager = new ManagerDTO { LastName = lastName };

                    lock (_lockers[manager.GetType()])
                    {
                        if (unitOfWork.GetManagers(managerSearchCriteria).FirstOrDefault() == null)
                        {
                            unitOfWork.AddManager(manager);
                            unitOfWork.Save();
                        }
                    }
                }

                return unitOfWork.GetManagers(managerSearchCriteria).FirstOrDefault();
            }
        }

        private ItemDTO ProcessItem(string itemLine)
        {
            using (var unitOfWork = _factory.GetInstance())
            {
                Expression<Func<ItemDTO, bool>> itemSearchCriteria = x => x.Name == itemLine;

                var item = unitOfWork.GetItems(itemSearchCriteria).FirstOrDefault();

                if (item != null)
                {
                    return item;
                }

                item = new ItemDTO() { Name = itemLine };

                lock (_lockers[item.GetType()])
                {
                    if (unitOfWork.GetItems(itemSearchCriteria).FirstOrDefault() == null)
                    {
                        unitOfWork.AddItem(item);
                        unitOfWork.Save();
                    }
                }

                return unitOfWork.GetItems(itemSearchCriteria).FirstOrDefault();
            }
        }

        private CustomerDTO ProcessCustomer(string customerLine)
        {
            using (var unitOfWork = _factory.GetInstance())
            {
                var fullName = ParseLine(customerLine, ' ');
                var firstName = fullName[0];
                var lastName = fullName[1];

                Expression<Func<CustomerDTO, bool>> customerSearchCriteria = (x => x.FirstName == firstName && x.LastName == lastName);

                var customer = unitOfWork.GetCustomers(customerSearchCriteria).FirstOrDefault();

                if (customer == null)
                {
                    customer = new CustomerDTO() { FirstName = firstName, LastName = lastName };

                    lock (_lockers[customer.GetType()])
                    {
                        if (unitOfWork.GetCustomers(customerSearchCriteria).FirstOrDefault() == null)
                        {
                            unitOfWork.AddCustomer(customer);
                            unitOfWork.Save();
                        }
                    }
                }

                return unitOfWork.GetCustomers(customerSearchCriteria).FirstOrDefault();
            }
        }
    }
}
