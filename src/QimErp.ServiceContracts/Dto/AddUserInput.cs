namespace QimErp.ServiceContracts.Dto
{
    public class AddUserInput
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
        ///     密码(加密)
        /// </summary>
        public string Password { get;  set; }

        /// <summary>
        ///     手机号码
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        ///     邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string Description { get; set; }
    }
}