using System;
using System.Reflection;

namespace Qim.Configuration.Settings
{
    public class StartupTask
    {
        public string Type { get; set; }

        public string Name { get; set; }
       
    }

    public class Component
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string ServiceType { get; set; }
    }

    public class AppSettings
    {
        public StartupTask[] StartupTasks { get; set; }

        public Component[] Components { get; set; }

        public string ConnectionString { get; set; }
    }
}