using DirectorySync.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace DirectorySync
{
    public class Utilities
    {
        private static readonly bool _enableNotifications = ConfigurationManager.AppSettings["EnableNotifications"] == "true";
        private static readonly bool _enableLogging = ConfigurationManager.AppSettings["EnableLogging"] == "true";

        public List<ConfigurationObject> ReadConfiguration()
        {
            Log("Reading the configuration via App.config");
            List<ConfigurationObject> configuration = new List<ConfigurationObject>();

            var config = (DirectorySyncConfig)ConfigurationManager.GetSection("directorySync");

            if (config == null)
                return new List<ConfigurationObject>();

            foreach (DirectorySyncInstanceElement instance in config.DirectorySyncInstances)
            {
                Log(instance.Name);
                configuration.Add(new ConfigurationObject
                {
                    Name = instance.Name,
                    Source = instance.Source,
                    Destination = instance.Destination
                });
            }

            return configuration;
        }

        public bool IsFileNew(string source, string destination)
        {
            DateTime sourceDateTime = File.GetLastWriteTime(source);
            DateTime destinationDateTime = File.GetLastWriteTime(destination);
            return sourceDateTime > destinationDateTime;
        }

        public void ShowNotification()
        {
            if (!_enableNotifications)
                return;
        }

        public void Log(string message)
        {
            if (!_enableLogging)
                return;

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";

            message = string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), message);

            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                    sw.WriteLine(message);
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                    sw.WriteLine(message);
            }
        }
    }
}
