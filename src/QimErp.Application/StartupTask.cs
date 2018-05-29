using Qim.Configuration;
using QimErp.Domain.Services;
using QimErp.Domain.Services.Impl;
using QimErp.ServiceContracts;

namespace QimErp.Application
{
    public class StartupTask : IStartupTask
    {
        public int Order => 100;

        public void Execute(IIocAppConfiguration configuration)
        {
            configuration.Registrar.Register<IUserManager, UserManager>();
            configuration.Registrar.Register<IUserService, UserService>();
            
        }
    }
}