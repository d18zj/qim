using System;

namespace Qim.Domain.Entity
{
    public abstract class FullLogEntity<TPkey> : CreationAndModificationLogEntity<TPkey>, IFullLog
    {
        /// <summary>
        ///     是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        ///     删除用户Id
        /// </summary>
        public virtual string DeleteBy { get; set; }

        /// <summary>
        ///     删除时间
        /// </summary>
        public virtual DateTime? DeleteOn { get; set; }
    }

    public abstract class FullLogEntity<TPkey, TUser> : CreationAndModificationLogEntity<TPkey, TUser>, IFullLog<TUser>
        where TUser : BaseUser
    {
        /// <summary>
        ///     是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        ///     删除用户Id
        /// </summary>
        public virtual string DeleteBy { get; set; }

        /// <summary>
        ///     删除时间
        /// </summary>
        public virtual DateTime? DeleteOn { get; set; }

        /// <summary>
        ///     删除用户
        /// </summary>
        public virtual TUser DeleterUser { get; set; }
    }


    public abstract class FullLogEntity : FullLogEntity<string>
    {
        
    }
}