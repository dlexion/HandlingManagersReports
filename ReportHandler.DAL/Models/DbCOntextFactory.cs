using ReportHandler.DAL.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportHandler.DAL.Models
{
    public class DbContextFactory : IDbContextFactory
    {
        public DbContext GetInstance()
        {
            return new ReportModelContainer();
        }
    }
}
