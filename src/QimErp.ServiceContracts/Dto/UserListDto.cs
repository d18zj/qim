using System;
using Qim.Dto;

namespace QimErp.ServiceContracts.Dto
{
    public class UserListDto : BaseDto
    {
        /// <summary>
        ///     用户帐号
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }


        public bool IsActive { get; set; }
    }
}