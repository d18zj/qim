using System;
using Microsoft.EntityFrameworkCore;

namespace Qim.EntitiFrameworkCore.Configuration
{
    public class DbContextConfigurer
    {
       
        public static readonly DbContextConfigurer Instance = new DbContextConfigurer();

        private DbContextConfigurer()
        {
            
        }

        public string ConnectionString { get;  set; }

        internal Action<DbContextConfiguration> ConfigAction { get; set; }

        internal DbContextOptions GetDbContextOptions(string connectionString)
        {
            if (ConfigAction == null)
            {
                throw new InvalidOperationException("Please call UseEntityFrameworkCore first!");
            }
            if (string.IsNullOrEmpty(connectionString)) connectionString = ConnectionString;

            var config = new DbContextConfiguration(connectionString);

            ConfigAction(config);
            return config.Builder.Options;
        }

        internal void Init(string connectionString, Action<DbContextConfiguration> configAction)
        {
            ConnectionString = connectionString;
            ConfigAction = configAction;
        }

    }
}