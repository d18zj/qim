using System;

namespace Qim.Domain.Entity
{
    public abstract class CreationAndModificationLogEntity<TPkey> : CreationLogEntity<TPkey>,
        ICreationAndModificationLog
    {
        /// <summary>
        ///     最后修改用户Id
        /// </summary>
        public virtual string LastModifyBy { get; set; }

        /// <summary>
        ///     最后修改时间
        /// </summary>
        public virtual DateTime? LastModifyOn { get; set; }
    }

    public abstract class CreationAndModificationLogEntity<TPkey, TUser> : CreationAndModificationLogEntity<TPkey>,
        ICreationAndModificationLog<TUser> where TUser : BaseUser
    {
        /// <summary>
        ///     创建用户
        /// </summary>
        public virtual TUser CreatorUser { get; set; }

        /// <summary>
        ///     最后修改用户
        /// </summary>
        public virtual TUser LastModifierUser { get; set; }
    }


    public abstract class CreationAndModificationLogEntity : CreationAndModificationLogEntity<string>
    {
        
    }
}