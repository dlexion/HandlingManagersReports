using System;
using System.IO;

namespace ReportHandler.BLL.Models
{
    public class FolderWatcher : IDisposable
    {
        private readonly string _path;
        private readonly string _filter;
        // TODO replace with interface
        private readonly FileHandler _handler;

        private FileSystemWatcher _watcher;

        public FolderWatcher(string path, string filter, FileHandler fileHandler)
        {
            _handler = fileHandler;
            _path = path;
            _filter = filter;

            InitializeWatcher();
        }

        public void Start()
        {
            if (_watcher == null)
            {
                InitializeWatcher();
            }

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
            _handler.ParseFile(e.FullPath);
        }

        private void InitializeWatcher()
        {
            _watcher = new FileSystemWatcher(_path, _filter)
            {
                NotifyFilter = NotifyFilters.FileName
                               | NotifyFilters.DirectoryName
            };

            _watcher.Created += Watcher_Created;
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}