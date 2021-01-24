using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Threading;

namespace DirectorySync
{
    public partial class Service : ServiceBase
    {
        List<ConfigurationObject> _configuration = new List<ConfigurationObject>();
        System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly Utilities _util = new Utilities();
        private static readonly object _intervalSync = new object();

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _util.Log("Service is started");
            _configuration = _util.ReadConfiguration();

            _timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            _timer.Interval = 5000;
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
            _util.Log("Service is stopped");
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            _util.Log("Service is recalled");
            if (Monitor.TryEnter(_intervalSync))
            {
                try
                {
                    SyncDirectories();
                }
                finally
                {
                    Monitor.Exit(_intervalSync);
                }
            }
            else
            {
                _util.Log("Previous interval is still in progress...");
            }
        }

        public void SyncDirectories()
        {
            _util.Log("Synchronizing directories");

            if (_configuration.Count() == 0)
            {
                _util.Log("No directories found to process");
            }

            foreach (var item in _configuration)
            {
                _util.Log(item.Name);
                _util.Log(string.Format("Source: {0}", item.Source));
                _util.Log(string.Format("Destination: {0}", item.Destination));

                if (!VerifyDirectories(item))
                {
                    break;
                }

                string[] files = Directory.GetFiles(item.Source, "*.*", SearchOption.AllDirectories);
                _util.Log(string.Format("Found {0} files in {1}", files.Length, item.Source));

                foreach (string file in files)
                {
                    var relativeFilePath = file.Replace(item.Source, "");
                    var destinationFilePath = string.Format("{0}{1}", item.Destination, relativeFilePath);
                    _util.Log(string.Format("file: {0}", file));
                    _util.Log(string.Format("dest: {0}", destinationFilePath));

                    if (!File.Exists(destinationFilePath) || _util.IsFileNew(file, destinationFilePath))
                    {
                        _util.Log("Moving file");
                        new FileInfo(destinationFilePath).Directory.Create();
                        File.Copy(file, destinationFilePath, true);
                    }
                }
            }
        }

        private bool VerifyDirectories(ConfigurationObject item)
        {
            _util.Log("Verifying directories:");

            _util.Log(item.Source);
            if (!Directory.Exists(item.Source))
            {
                _util.Log("Not found, skipping this item");
                _configuration.Remove(item);
                return false;
            }

            _util.Log(item.Destination);
            if (!Directory.Exists(item.Destination))
            {
                _util.Log("Creating destination directory");
                Directory.CreateDirectory(item.Destination);
            }

            return true;
        }
    }
}
