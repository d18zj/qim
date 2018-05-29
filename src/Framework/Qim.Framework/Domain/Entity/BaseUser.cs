using System;

namespace Qim.Domain.Entity
{
    /// <summary>
    ///  基类用户
    /// </summary>

    public abstract class BaseUser : CreationAndModificationLogEntity<string>, IPassivable, IMustHaveTenant
    {
        /// <summary>
        ///     用户帐号
        /// </summary>
        public virtual string UserAccount { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     最后登录时间
        /// </summary>
        public virtual DateTime? LastLoginTime { get; protected set; }

        /// <summary>
        ///     登录次数
        /// </summary>
        public virtual int LoginCount { get; protected set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///     租户Id
        /// </summary>
        public int TenantId { get; set; }
    }
}