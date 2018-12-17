using System;
using System.Configuration;
using ReportHandler.BLL.Interfaces;
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
            container.Register<IFileHandler, FileHandler>();

            container.Verify();
        }

        static void Main(string[] args)
        {
            try
            {
                var folderToWatch = ConfigurationManager.AppSettings["FolderToWatch"];
                var fileFilter = ConfigurationManager.AppSettings["FileFilter"];
                var folderForProcessedFiles = ConfigurationManager.AppSettings["FolderForProcessedFiles"];

                var watcher = new FolderWatcher(folderToWatch, fileFilter, folderForProcessedFiles, container.GetInstance<IFileHandler>());
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
