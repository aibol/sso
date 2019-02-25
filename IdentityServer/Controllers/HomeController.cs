using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Models.Home;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IServiceScopeFactory _builder;

        public HomeController(IIdentityServerInteractionService interaction, IServiceScopeFactory builder)
        {
            _interaction = interaction;
            _builder = builder;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Test()
        {
            var scopes = new [] {"openid", "profile"};
            using (var scope = _builder.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                var query =
                    from identityResource in _context.IdentityResources
                    where scopes.Contains(identityResource.Name)
                    select identityResource;

                var resources = query
                    .Include(x => x.UserClaims);

                var results = resources.ToArray();

                var arra = results.Select(x => x.ToModel()).ToArray().AsEnumerable();
            }

            return Json(200);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}