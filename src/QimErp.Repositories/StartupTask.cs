using Qim.Configuration;
using QimErp.Domain.Repositories;

namespace QimErp.Repositories
{
    public class StartupTask : IStartupTask
    {
        public int Order => 10;

        public void Execute(IIocAppConfiguration configuration)
        {
            configuration.Registrar.Register<IUserRepository, UserRepository>();
            configuration.Registrar.Register<IGlobalAccountRepository, GlobalAccountRepository>();
        }
    }
}