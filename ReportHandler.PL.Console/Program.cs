using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportHandler.BLL;
using ReportHandler.BLL.Models;
using ReportHandler.DAL.AutoMapperSetup;
using ReportHandler.DAL.Contracts.DTO;
using ReportHandler.DAL.Models;

namespace ReportHandler.PL.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfiguration.Configure();

            // TODO take parameters from config
            var watcher = new FolderWatcher(@"D:\TestFolder", "*.csv", new FileHandler());
            watcher.Start();

            System.Console.ReadKey();
        }
    }
}
