using ReportHandler.DAL.Contracts.Interfaces;

namespace ReportHandler.DAL.Models
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork GetInstance()
        {
            return new UnitOfWork();
        }
    }
}