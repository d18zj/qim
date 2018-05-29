using System;

namespace Qim.Domain.Entity
{
    /// <summary>
    ///     删除日志
    /// </summary>
    public interface IDeletionLog : ISoftDelete
    {
        /// <summary>
        ///     删除用户Id
        /// </summary>
        string DeleteBy { get; set; }

        /// <summary>
        ///     删除时间
        /// </summary>
        DateTime? DeleteOn { get; set; }
    }

    public interface IDeletionLog<TUser> : IDeletionLog
       where TUser : IEntity<string>
    {
        /// <summary>
        /// Reference to the deleter user of this entity.
        /// </summary>
        TUser DeleterUser { get; set; }
    }
}