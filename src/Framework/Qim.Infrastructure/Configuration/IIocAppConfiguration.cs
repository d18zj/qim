using Qim.Ioc;

namespace Qim.Configuration
{
    public interface IIocAppConfiguration : IAppConfiguration
    {

        /// <summary>
        ///  Ioc Registrar. <see cref="IIocRegistrar"/>
        /// </summary>
        IIocRegistrar Registrar { get; set; }

        /// <summary>
        /// Ioc Resolver. <see cref="IIocResolver"/>
        /// </summary>
        IIocResolver Resolver { get; set; }
    }
}