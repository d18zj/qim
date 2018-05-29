namespace QimErp.Infrastructure.DomainModel
{
    public enum LoginResult
    {
        /// <summary>
        ///     登录成功
        /// </summary>
        Success = 1,

        /// <summary>
        ///     无效的用户帐号或邮件地址
        /// </summary>
        InvalidUserAccountOrEmail,

        /// <summary>
        ///     无效的密码
        /// </summary>
        InvalidPassword,

        /// <summary>
        ///     用户已停用
        /// </summary>
        UserIsNotActive,

        /// <summary>
        ///     租户已停用
        /// </summary>
        TenantIsNotActive,

        /// <summary>
        ///     租户已过期
        /// </summary>
        TenantOutDate
    }
}