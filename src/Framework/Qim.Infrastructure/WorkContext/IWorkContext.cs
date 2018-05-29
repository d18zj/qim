namespace Qim.WorkContext
{
    public interface IWorkContext
    {
        /// <summary>
        ///     是否授权
        ///     
        /// </summary>
        /// <param name="programCode">程序代码</param>
        /// <param name="authorityCodes">权限代码列表</param>
        /// <returns>可访问某程序及权限代码列表中的任一权限时，返回:true,否则返回:false</returns>
        bool IsGranted(string programCode, params string[] authorityCodes);
    }
}