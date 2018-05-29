using AutoMapper;
using Qim.Configuration;
using Qim.Configuration.Settings;
using Qim.Ioc;
using Qim.Reflection;
using IObjectMapper = Qim.Application.IObjectMapper;

namespace Qim.AutoMapper
{
    public static class ConfigurationExtensions
    {
        private static bool _hasCreateMapping = false;
        private static readonly object _syncObj = new object();
        public static IIocAppConfiguration UserAutoMapper(this IIocAppConfiguration configuration)
        {
            Ensure.NotNull(configuration,nameof(configuration));
            var registrar = configuration.Registrar;
            registrar.Register<IObjectMapper, AutoMapperObjectMapper>(LifetimeType.Singleton);
            var typeFinder = configuration.Resolver.GetService<ITypeFinder>();
            CreateMapping(typeFinder);
            registrar.RegisterInstance(Mapper.Instance);
            return configuration;
        }

        private static void CreateMapping(ITypeFinder typeFinder)
        {
            lock (_syncObj)
            {
                if (_hasCreateMapping) return;

                var mapTypes = typeFinder.FindClassesOfType(typeof(Profile));
                Mapper.Initialize(config =>
                {
                    foreach (var type in mapTypes)
                    {
                        config.AddProfile(type);
                    }
                });

                _hasCreateMapping = true;
            }
        }
    }
}