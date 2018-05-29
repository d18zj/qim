using System;

namespace Qim.Dto
{
    public interface IModificationLogDto
    {
        /// <summary>
        ///     最后修改用户
        /// </summary>
        string LastModifyBy { get; set; }

        /// <summary>
        ///     最后修改时间
        /// </summary>
        DateTime? LastModifyOn { get; set; }
    }
}
