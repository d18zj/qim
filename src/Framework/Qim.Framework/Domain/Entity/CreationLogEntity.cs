using System;
using Qim.Timing;

namespace Qim.Domain.Entity
{
    public abstract class CreationLogEntity<TPkey> : BaseEntity<TPkey>,ICreationLog
    {
        /// <summary>
        ///     创建人员
        /// </summary>
        public virtual string CreateBy { get; set; }


        /// <summary>
        ///     创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        protected CreationLogEntity()
        {
        }
    }

    public abstract class CreationLogEntity<TPkey, TUser> : CreationLogEntity<TPkey>, ICreationLog<TUser>
        where TUser : BaseUser
    {
        public TUser CreatorUser { get; set; }
    }

    public abstract class CreationLogEntity : CreationLogEntity<string>
    {
        
    }
     
}