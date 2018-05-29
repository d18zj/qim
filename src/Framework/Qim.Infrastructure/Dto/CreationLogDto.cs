using System;

namespace Qim.Dto
{
    public abstract class CreationLogDto<TPkey> : BaseDto<TPkey>,ICreationLogDto
    {
        /// <summary>
        ///     创建人员
        /// </summary>
        public string CreateBy { get; set; }


        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreateOn { get; set; }
    }


    public abstract class CreationLogDto : CreationLogDto<string>
    {

    }
}
