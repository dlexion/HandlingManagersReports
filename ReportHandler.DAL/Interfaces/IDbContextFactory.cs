using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportHandler.DAL.Interfaces
{
    public interface IDbContextFactory
    {
        DbContext CreateInstance();
    }
}
