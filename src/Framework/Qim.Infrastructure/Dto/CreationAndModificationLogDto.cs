using System;

namespace Qim.Dto
{
    public class CreationAndModificationLogDto<TPkey> : CreationLogDto<TPkey>, ICreationAndModificationLogDto
    {
        public string LastModifyBy { get; set; }
       

        public DateTime? LastModifyOn { get; set; }
        
    }
}
