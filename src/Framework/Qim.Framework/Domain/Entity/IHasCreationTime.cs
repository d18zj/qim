using System;

namespace Qim.Domain.Entity
{
    public interface IHasCreationTime
    {
        /// <summary>
        ///     创建时间
        /// </summary>
        DateTime CreateOn { get; set; }
    }
}