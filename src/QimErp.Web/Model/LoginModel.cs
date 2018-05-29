using System.ComponentModel.DataAnnotations;

namespace QimErp.Web.Model
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "登录名称")]
        public string UserAccount { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}