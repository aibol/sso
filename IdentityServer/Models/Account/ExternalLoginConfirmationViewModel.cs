using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}