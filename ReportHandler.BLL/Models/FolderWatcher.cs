using System;
using System.IO;
using ReportHandler.BLL.Interfaces;

namespace ReportHandler.BLL.Models
{
    public sealed class FolderWatcher : IDisposable
    {
        private readonly string _path;
        private readonly string _filter;
        private readonly string _folderForProcessedFile;

        private readonly IFileHandler _handler;

        private FileSystemWatcher _watcher;


        public FolderWatcher(string path, string filter, string folderForProcessedFile, IFileHandler fileHandler)
        {
            _handler = fileHandler;
            _path = path;
            _filter = filter;
            _folderForProcessedFile = folderForProcessedFile;

            InitializeWatcher();
        }

        public void Start()
        {
            // TODO existing files
            //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            if (_watcher == null)
            {
                InitializeWatcher();
            }

            _watcher.Created += Watcher_Created;
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.Created -= Watcher_Created;

            _watcher.EnableRaisingEvents = false;
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            _handler.ParseFile(e.FullPath, _folderForProcessedFile);
        }

        private void InitializeWatcher()
        {
            _watcher = new FileSystemWatcher(_path, _filter)
            {
                NotifyFilter = NotifyFilters.FileName
                               | NotifyFilters.DirectoryName
            };
        }

        //private void 

        #region IDisposable implemintation

        public void Dispose()
        {
            Stop();
            _watcher.Dispose();
            _watcher = null;
        }

        #endregion
    }
}