using System.Configuration;
using System.ServiceProcess;
using ReportHandler.BLL.Interfaces;
using ReportHandler.BLL.Models;
using ReportHandler.DAL.AutoMapperSetup;
using ReportHandler.DAL.Contracts.Interfaces;
using ReportHandler.DAL.Models;
using SimpleInjector;

namespace ReportHandler.PL.Service
{
    public partial class ReportHandlerService : ServiceBase
    {
        private FolderWatcher watcher;
        private readonly Container container;

        public ReportHandlerService()
        {
            InitializeComponent();

            AutoMapperConfiguration.Configure();

            container = new Container();

            container.RegisterInstance<IUnitOfWorkFactory>(new UnitOfWorkFactory());
            container.Register<IFileHandler, FileHandler>();

            container.Verify();
        }

        protected override void OnStart(string[] args)
        {
            var folderToWatch = ConfigurationManager.AppSettings["FolderToWatch"];
            var fileFilter = ConfigurationManager.AppSettings["FileFilter"];
            var folderForProcessedFiles = ConfigurationManager.AppSettings["FolderForProcessedFiles"];

            watcher = new FolderWatcher(folderToWatch, fileFilter, folderForProcessedFiles, container.GetInstance<IFileHandler>());
            watcher.Start();
        }

        protected override void OnStop()
        {
            watcher.Dispose();
        }
    }
}
