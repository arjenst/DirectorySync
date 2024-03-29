﻿namespace DirectorySync
{
    public class ConfigurationObject
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int UnavailableCount { get; set; } = 0;
    }
}
