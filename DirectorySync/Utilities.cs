using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DirectorySync
{
    public class Utilities
    {
        public List<Configuration> ReadConfig()
        {
            Log("Reading the configuration");
            List<Configuration> configuration = new List<Configuration>();
            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Configuration.json";
            JObject configurationFile = JObject.Parse(File.ReadAllText(folder));

            foreach (var item in configurationFile["directories"])
            {
                Log((string)item["name"]);
                configuration.Add(new Configuration
                {
                    Name = (string)item["name"],
                    Source = (string)item["source"],
                    Destination = (string)item["destination"]
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
