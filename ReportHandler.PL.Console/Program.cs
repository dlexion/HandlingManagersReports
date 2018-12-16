using System;
using System.Configuration;
using ReportHandler.BLL.Models;
using ReportHandler.DAL.AutoMapperSetup;
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
            AutoMapperConfiguration.Configure();

            container = new Container();

            container.RegisterInstance<IUnitOfWorkFactory>(new UnitOfWorkFactory());
            container.Register<FileHandler>();

            container.Verify();
        }

        static void Main(string[] args)
        {
            var folderToWatch = string.Empty;
            var fileFilter = string.Empty;

            try
            {
                folderToWatch = ConfigurationManager.AppSettings["FolderToWatch"];
                fileFilter = ConfigurationManager.AppSettings["FileFilter"];

                var watcher = new FolderWatcher(folderToWatch, fileFilter, container.GetInstance<FileHandler>());
                watcher.Start();

                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.ReadKey();
            }
        }
    }
}
