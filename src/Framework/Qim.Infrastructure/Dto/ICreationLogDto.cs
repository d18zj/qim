using System;

namespace Qim.Dto
{
    public interface ICreationLogDto 
    {
        /// <summary>
        ///     创建人员
        /// </summary>
        string CreateBy { get; set; }


        /// <summary>
        ///     创建时间
        /// </summary>
        DateTime CreateOn { get; set; }
    }
}
