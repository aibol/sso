using Microsoft.AspNetCore.Identity;

namespace ImportBackstageUsers.SSOModels
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string Source { get; set; }
    }
}