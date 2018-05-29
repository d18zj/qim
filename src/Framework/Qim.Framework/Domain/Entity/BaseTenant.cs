using System;
using Qim.MultiTenancy;

namespace Qim.Domain.Entity
{
    /// <summary>
    ///     租户基类
    /// </summary>
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class BaseTenant : CreationAndModificationLogEntity<int>, IPassivable
    {
        /// <summary>
        ///     租户名称
        /// </summary>
        public virtual string TenantName { get; set; }

        /// <summary>
        ///     开始时间
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        ///     结束时间
        /// </summary>
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        ///     是否激活
        /// </summary>
        public virtual bool IsActive { get; set; }
    }
}