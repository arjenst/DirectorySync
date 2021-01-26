using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using System.Threading;
using System.Configuration;
using System;

namespace DirectorySync
{
    public partial class Service : ServiceBase
    {
        List<ConfigurationObject> _configuration = new List<ConfigurationObject>();
        System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly Utilities _util = new Utilities();
        private static readonly object _intervalSync = new object();

        private static int _threadInterval
        {
            get
            {
                int n;
                if (!int.TryParse(ConfigurationManager.AppSettings["ThreadInterval"], out n))
                {
                    n = 5000;
                }
                return n;
            }
        }

        private static int _maxRetries
        {
            get
            {
                int n;
                if (!int.TryParse(ConfigurationManager.AppSettings["MaxRetries"], out n))
                {
                    n = 60;
                }
                return n;
            }
        }

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _util.Log("Service is started");
            _configuration = _util.ReadConfiguration();
            _timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            _timer.Interval = _threadInterval;
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

        private void StopService()
        {
            ServiceController service = new ServiceController("DirectorySync");
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch
            {
                _util.Log("Unable to stop the service");
            }
        }

        public void SyncDirectories()
        {
            _util.Log("Synchronizing directories");

            if (_configuration.Count() == 0)
            {
                _util.Log("No directories found to process");
                StopService();
            }

            foreach (var item in _configuration)
            {
                _util.Log(item.Name);
                _util.Log(string.Format("Source: {0}", item.Source));
                _util.Log(string.Format("Destination: {0}", item.Destination));
                _util.Log(string.Format("UnavailableCount: {0}", item.UnavailableCount));

                if (!VerifyDirectories(item))
                {
                    item.UnavailableCount++;
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
                _util.Log("Not found");
                if (item.UnavailableCount >= _maxRetries)
                {
                    _util.Log("Removing item. Restart service to retry again!");
                    item = _configuration.First(a => a.Name == item.Name);
                    _configuration.Remove(item);

                }
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
