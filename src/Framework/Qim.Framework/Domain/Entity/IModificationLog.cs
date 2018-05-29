using System;

namespace Qim.Domain.Entity
{
    /// <summary>
    ///     修改日志
    /// </summary>
    public interface IModificationLog : IHasModificationTime
    {
        /// <summary>
        ///     最后修改用户Id
        /// </summary>
        string LastModifyBy { get; set; }


    }

    public interface IModificationLog<TUser> : IModificationLog
        where TUser : IEntity<string>
    {
        /// <summary>
        ///     最后修改用户
        /// </summary>
        TUser LastModifierUser { get; set; }
    }
}