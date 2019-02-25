using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 该用户的来源
        /// </summary>
        /// <remarks>
        ///     aibol2_portal: 公司官网
        ///     aibol2_develop: 开发测试
        /// </remarks>
        public string Source { get; set; }
    }
}
