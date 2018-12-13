using ReportHandler.BLL.DTO;
using ReportHandler.BLL.Models;
using ReportHandler.DAL;
using ReportHandler.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportHandler.BLL
{
    public class Unity
    {
        private IDbContextFactory _contextFactory;

        // TODO use connection string as parametr to create DbContext
        public Unity()
        {
            _contextFactory = new DAL.Models.DbContextFactory();
        }

        public Task<bool> ParseFile(string path)
        {
            return Task<bool>.Factory.StartNew(() =>
            {
                using (var sr = new StreamReader(path))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        var data = ParseLine(line);

                        // TODO
                        // example how will work
                        if (data.Length < 4)
                        {
                            throw new ArgumentException();
                        }

                        ProcessManager(data[0]);
                    }
                }

                return true;
            }
            );
        }

        private string[] ParseLine(string line)
        {
            return line.Split(',');
        }

        private void ProcessManager(string managerLine)
        {
            using (var context = _contextFactory.CreateInstance())
            {
                Expression<Func<ManagerDTO, bool>> managerSearchCriteria = x => x.LastName == managerLine;

                var managerUnitOfWork = new GenericUnitOfWork<Manager, ManagerDTO>(context, new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion));
                var manager = managerUnitOfWork.TryGet(managerSearchCriteria);

                if (manager == null)
                {
                    manager = new ManagerDTO { LastName = managerLine };
                    managerUnitOfWork.TryAdd(manager, managerSearchCriteria);
                }
            }
        }
    }
}
