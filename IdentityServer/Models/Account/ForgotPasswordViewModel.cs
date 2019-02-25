using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}