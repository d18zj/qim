using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Qim.Configuration.Settings;
using Qim.Ioc;
using Qim.Logging;
using Qim.Reflection;
using Qim.Runtime.Serialization;

namespace Qim.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IIocAppConfiguration CreateIocAppConfiguration(this IAppConfiguration configuration,
            IIocRegistrar registrar, IIocResolver resolver)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            var appConfiguration = configuration as AppConfiguration;
            if (appConfiguration == null)
            {
                throw new InvalidOperationException($"The method only support for AppConfiguration.");
            }
            RegisterCommonComponents(registrar);
            var result = new IocAppConfiguration(appConfiguration.ConfigDictionary, registrar, resolver);
            registrar.RegisterInstance<IIocAppConfiguration>(result);
            registrar.RegisterInstance<IAppConfiguration>(result);
            return result;
        }

        public static IIocAppConfiguration LoadSettings(this IIocAppConfiguration configuration, string settingText)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            Ensure.NotNullOrWhiteSpace(settingText, nameof(settingText));
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(settingText, new Runtime.Serialization.StringTypeConverter());
            if (appSettings == null)
            {
                throw new AppException($"Load app settings failed.The setting text is: {settingText}");
            }
            return LoadSettings(configuration, appSettings);
        }

        public static IIocAppConfiguration LoadSettings(this IIocAppConfiguration configuration, AppSettings appSettings)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            Ensure.NotNull(appSettings, nameof(appSettings));
            configuration.DefaultNameOrConnectionString = appSettings.ConnectionString;
            var taskInterface = typeof(IStartupTask);
            if (appSettings.StartupTasks != null)
            {
                foreach (var task in appSettings.StartupTasks)
                {
                    var taskType = GetTypeFromString(task.Type);
                    if (!taskInterface.IsAssignableFrom(taskType))
                    {
                        throw new InvalidOperationException($"The type must be assigned from IStartupTask. ");
                    }
                    configuration.Registrar.RegisterType(taskInterface, taskType, task.Name);
                }
            }
            if (appSettings.Components != null)
            {
                foreach (var item in appSettings.Components)
                {
                    var type = GetTypeFromString(item.Type);

                    if (!string.IsNullOrWhiteSpace(item.ServiceType))
                    {
                        var serviceType = GetTypeFromString(item.ServiceType);
                        configuration.Registrar.RegisterType(serviceType, type, item.Name);
                    }
                    else
                    {
                        configuration.Registrar.RegisterType(type, item.Name);
                    }

                }
            }
            return configuration;

        }


        public static IIocAppConfiguration RunStartupTask(this IIocAppConfiguration configuration)
        {
            var tasks = configuration.Resolver.GetAllServices<IStartupTask>().OrderBy(a => a.Order);
            foreach (var task in tasks)
            {
                task.Execute(configuration);
            }

            return configuration;
        }


        public static IIocAppConfiguration UseEmptyLogger(this IIocAppConfiguration configuration)
        {
            configuration.Registrar.Register<ILoggerFactory, EmptyLoggerFactory>(LifetimeType.Singleton);
            return configuration;
        }

        private static Type GetTypeFromString(string typeName)
        {
            Ensure.NotNullOrWhiteSpace(typeName,nameof(typeName));
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw  new ArgumentException($"Can not convert '{typeName}' to type.");
            }
            return type;
        }
        
        private static void RegisterCommonComponents(IIocRegistrar registrar)
        {
            registrar.Register<IObjectSerializer, JsonNetSerializer>(LifetimeType.Singleton);
            registrar.Register<IAssemblyProvider, AssemblyProvider>(LifetimeType.Singleton);
            registrar.Register<ITypeFinder, AppTypeFinder>(LifetimeType.Singleton);
            registrar.RegisterType(typeof(ILogger<>), typeof(Logger<>));
        }

    }
}