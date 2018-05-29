using System.Collections.Generic;
using Qim.Ioc;

namespace Qim.Configuration
{
    public interface IAppConfiguration
    {

        //IDictionary<string,object> ConfigDictionary { get; }

        /// <summary>
        /// Gets/sets default connection string used by ORM module.
        /// It can be name of a connection string in application's config file or can be full connection string.
        /// </summary>
        string DefaultNameOrConnectionString { get; set; }

        /// <summary>
        ///     基础App当前路径
        /// </summary>
        string BaseAppPath { get; set; }

      
    }


    
}