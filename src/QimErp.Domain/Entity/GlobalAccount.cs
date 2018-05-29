using Qim.Domain.Entity;
using Qim.MultiTenancy;

namespace QimErp.Domain.Entity
{
    /// <summary>
    ///     全局帐户
    /// </summary>
    [MultiTenancySide(MultiTenancySides.Host)]
    public class GlobalAccount: CreationAndModificationLogEntity<int>
    {
        /// <summary>
        ///     租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        ///     用户Id(全局唯一)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     邮件地址(全局唯一)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     手机号码（全局唯一)
        /// </summary>
        public string CellPhone { get; set; }


        #region 导航属性
        /// <summary>
        ///     租户
        /// </summary>
        public virtual Tenant Tenant { get; set; }

        #endregion

        #region Method

        public static GlobalAccount Create(int tenantId, string userId, string cellPhone, string email)
        {
            return new GlobalAccount
            {
                TenantId = tenantId,
                CellPhone = cellPhone,
                UserId = userId,
                Email = email,
                CreateBy = userId
            };
        }


        #endregion
    }
}