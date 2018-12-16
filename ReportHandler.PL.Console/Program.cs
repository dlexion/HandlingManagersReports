using ReportHandler.BLL.Models;
using ReportHandler.DAL.AutoMapperSetup;
using ReportHandler.DAL.Contracts.DTO;
using ReportHandler.DAL.Contracts.Interfaces;
using ReportHandler.DAL.Models;
using SimpleInjector;

namespace ReportHandler.PL.Console
{
    class Program
    {
        static readonly Container container;

        static Program()
        {
            container = new Container();

            container.RegisterInstance<IUnitOfWorkFactory>(new UnitOfWorkFactory());
            container.Register<FileHandler>();

            container.Verify();
        }

        static void Main(string[] args)
        {
            AutoMapperConfiguration.Configure();

            // TODO take parameters from config
            var watcher = new FolderWatcher(@"D:\TestFolder", "*.csv", container.GetInstance<FileHandler>());
            watcher.Start();

            System.Console.ReadKey();
        }
    }
}
