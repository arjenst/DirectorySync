using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using DirectorySync.Configuration;
using Newtonsoft.Json.Linq;

namespace DirectorySync
{
    public class Utilities
    {
        public List<ConfigurationObject> ReadConfiguration()
        {
            Log("Reading the configuration via App.config");
            List<ConfigurationObject> configuration = new List<ConfigurationObject>();

            var config = (DirectorySyncConfig)ConfigurationManager.GetSection("directorySync");

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
            return sourceDateTime != destinationDateTime && sourceDateTime > destinationDateTime;
        }

        public void Log(string message)
        {
            if (ConfigurationManager.AppSettings["EnableLogging"] != "true")
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
