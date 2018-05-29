using Qim.Ioc;

namespace Qim.Configuration
{
    /// <summary>
    ///     Interface which should be implemented by tasks run on startup
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        ///     Order
        /// </summary>
        int Order { get; }

        /// <summary>
        ///     Execute task
        /// </summary>
        /// <param name="configuration"></param>
        void Execute(IIocAppConfiguration configuration);
    }
}