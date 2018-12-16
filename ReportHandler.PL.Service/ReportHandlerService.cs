using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ReportHandler.PL.Service
{
    public partial class ReportHandlerService : ServiceBase
    {
        public ReportHandlerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            File.AppendAllText(@"D:\TestFolder\1.txt","It works");
        }

        protected override void OnStop()
        {
        }
    }
}
