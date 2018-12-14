using System.Data.Entity;

namespace ReportHandler.DAL.Contracts.Interfaces
{
    public interface IDbContextFactory
    {
        DbContext GetInstance();
    }
}
