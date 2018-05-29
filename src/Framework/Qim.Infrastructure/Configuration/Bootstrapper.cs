using System;

namespace Qim.Configuration
{
    public class Bootstrapper
    {
        private static IIocAppConfiguration _configuration;
        private static IAppConfiguration _appConfiguration;
        //public static Bootstrapper Instance { get; } = new Bootstrapper();

        public static IIocAppConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    throw new AppException($"Please call Start method first!");
                }
                return _configuration;
            }
        }

        public static IIocAppConfiguration Start(Func<IAppConfiguration, IIocAppConfiguration> configAction)
        {

            Ensure.NotNull(configAction, nameof(configAction));
            if (_configuration != null) return _configuration; //确保只执行一次
            _configuration = configAction(AppConfiguration);

            _configuration.Registrar.RegisterInstance(Configuration);
            return _configuration;
        }


        private static IAppConfiguration AppConfiguration => _appConfiguration ?? (_appConfiguration = new AppConfiguration());
    }
}