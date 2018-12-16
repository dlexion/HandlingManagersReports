namespace ReportHandler.DAL.Contracts.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetInstance();
    }
}