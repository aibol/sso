using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Agreement")]
        [Range(1, 1, ErrorMessage = "您只有同意本系统的隐私条款才可以注册账号")]
        public bool Agreement { get; set; }
    }
}