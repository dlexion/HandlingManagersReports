using System;
using System.IO;
using ReportHandler.BLL.Interfaces;

namespace ReportHandler.BLL.Models
{
    public sealed class FolderWatcher : IDisposable
    {
        private readonly string _folderToWatch;
        private readonly string _filter;
        private readonly string _folderForProcessedFiles;

        private readonly IFileHandler _handler;

        private FileSystemWatcher _watcher;

        public FolderWatcher(string folderToWatch, string filter, string folderForProcessedFiles, IFileHandler fileHandler)
        {
            _handler = fileHandler;
            _folderToWatch = folderToWatch;
            _filter = filter;
            _folderForProcessedFiles = folderForProcessedFiles;

            InitializeWatcher();
        }

        public void Start()
        {
            ProcessExistingFiles();

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
            _handler.ParseFile(e.FullPath, _folderForProcessedFiles);
        }

        private void InitializeWatcher()
        {
            if (!Directory.Exists(_folderToWatch))
            {
                throw new ArgumentException("Directory does not exist.", nameof(_folderToWatch));
            }

            _watcher = new FileSystemWatcher(_folderToWatch, _filter)
            {
                NotifyFilter = NotifyFilters.FileName
                               | NotifyFilters.DirectoryName
            };
        }

        private void ProcessExistingFiles()
        {
            string[] fileList = Directory.GetFiles(_folderToWatch, _filter);

            foreach (var filePath in fileList)
            {
                _handler.ParseFile(filePath, _folderForProcessedFiles);
            }
        }

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