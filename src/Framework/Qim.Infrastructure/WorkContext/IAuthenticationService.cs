using System.Threading.Tasks;

namespace Qim.WorkContext
{
    /// <summary>
    ///     认证服务
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        ///     签入
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="tenantId">租户Id</param>
        /// <param name="isPersistent">关闭浏览器后cookie是否保留</param>
        Task SignInAsync(string userId, int? tenantId = null, bool isPersistent = false);

        /// <summary>
        ///     签出
        /// </summary>
        Task SignOutAsync();

        /// <summary>
        ///     是否签入
        /// </summary>
        /// <returns></returns>
        bool IsSignedIn();
    }
}