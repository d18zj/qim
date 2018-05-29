using System;

namespace Qim.Domain.Entity
{
    /// <summary>
    ///     创建日志
    /// </summary>
    public interface ICreationLog : IHasCreationTime
    {
        /// <summary>
        ///     创建用户Id
        /// </summary>
        string CreateBy { get; set; }

      
    }


    public interface ICreationLog<TUser> : ICreationLog
        where TUser : BaseUser
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        TUser CreatorUser { get; set; }
    }
}