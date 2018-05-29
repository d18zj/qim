using System.ComponentModel.DataAnnotations;

namespace QimErp.ServiceContracts.Dto
{
    public class RegisterUserInput
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        //[Required]
        //[Display(Name = "手机号码")]
        public string CellPhone { get; set; }

        /// <summary>
        ///     联系人
        /// </summary>
        //[Display(Name = "联系人")]
        //[Required]
        public string UserName { get; set; }

        /// <summary>
        ///     企业名称
        /// </summary>
        //[Display(Name = "企业名称")]
        //[Required]
        public string TenantName { get; set; }


        //public void Test()
        //{
        //}

    }
}