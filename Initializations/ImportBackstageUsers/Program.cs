using System;
using System.IO;
using System.Linq;
using ImportBackstageUsers.BackstageModels;
using ImportBackstageUsers.SSOModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Z.EntityFramework.Plus;

namespace ImportBackstageUsers
{
    class Program
    {
        private static readonly pwContext _pwContext;
        private static readonly aibol2ssoContext _aibol2ssoContext;
        private static readonly UserManager<ApplicationUser> _userManager;

        static Program()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _pwContext = new pwContext(configuration);
            _aibol2ssoContext = new aibol2ssoContext(configuration);
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(_aibol2ssoContext),
                null,
                new PasswordHasher<ApplicationUser>(),
                null, null, null, null, null, null);
        }

        static void Main(string[] args)
        {
            // delete existed
            var count = _aibol2ssoContext.AspNetUsers.Where(t => t.Source == "pingwei.backstage").Delete();
            Console.WriteLine($"{count} users deleted in aibol2sso");

            // query pwUsers
            var userQuery = from u in _pwContext.AibolUser.Where(t => t.Status != 8)
                            join l in _pwContext.AibolLogin on u.Id equals l.Id
                            select new { u, l };

            var pwUsers = userQuery.GroupBy(t => t.u.Code.ToLower()).ToDictionary(t => t.Key, t => t.ToList());
            Console.WriteLine($"pw users read, count: {pwUsers.Count}");

            // create aibol2sso users
            count = 0;
            foreach (var pwUserGroup in pwUsers)
            {
                count++;
                var pwUser = pwUserGroup.Value.First();
                Console.WriteLine($"{count}:{pwUser.u.Code}");

                var aspnetUser = new ApplicationUser
                {
                    Id = pwUser.u.Id.ToString(),
                    UserName = pwUser.l.UserName,
                    NormalizedUserName = pwUser.l.UserName.ToUpperInvariant(),
                    PasswordHash = _userManager.PasswordHasher.HashPassword(null, "Abc123!"),
                    SecurityStamp = _userManager.GenerateNewAuthenticatorKey(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = pwUser.u.Mobile,
                    LockoutEnabled = true,
                    Source = "pingwei.backstage"
                };

                _aibol2ssoContext.AspNetUsers.Add(aspnetUser);
            }

            _aibol2ssoContext.SaveChanges();
            Console.WriteLine($"{count} users saved to aibol2sso");

            Console.ReadLine();
        }
    }
}
