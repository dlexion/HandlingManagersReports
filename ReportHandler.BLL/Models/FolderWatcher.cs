using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ReportHandler.BLL.Models
{
    public sealed class FolderWatcher : IDisposable
    {
        private readonly string _path;
        private readonly string _filter;
        private readonly string _folderForProcessedFile;

        // TODO replace with interface
        private readonly FileHandler _handler;

        private FileSystemWatcher _watcher;


        public FolderWatcher(string path, string filter, string folderForProcessedFile, FileHandler fileHandler)
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
            Dispose();
            _watcher = null;
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

        public void Dispose()
        {
            Stop();
            _watcher.Dispose();
        }
    }
}