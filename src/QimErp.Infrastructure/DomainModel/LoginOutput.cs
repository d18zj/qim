using System;

namespace QimErp.Infrastructure.DomainModel
{
    public class LoginOutput
    {
        /// <summary>
        ///     登录结果
        /// </summary>
        public LoginResult LoginResult { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     用户帐号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        ///     租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        ///     租户名称
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        ///     租户过期日期
        /// </summary>
        public DateTime TenantOutDate { get; set; }
    }
}