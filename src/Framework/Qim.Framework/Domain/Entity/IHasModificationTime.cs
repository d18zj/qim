using System;

namespace Qim.Domain.Entity
{
    public interface IHasModificationTime
    {
        /// <summary>
        ///     最后修改时间
        /// </summary>
        DateTime? LastModifyOn { get; set; }
    }
}