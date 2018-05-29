

namespace Qim.WorkContext
{
    public interface IQimSession
    {

        /// <summary>
        ///     用户Id
        /// </summary>
        string UserId { get; }

        /// <summary>
        ///     租户Id
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        ///  区域名称
        /// </summary>
        string CultureName { get; }
    }
}
