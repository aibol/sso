using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Models.Account
{
    public class AccountService
    {
        private readonly IClientStore _clientStore;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityServerInteractionService _interaction;

        public AccountService(IIdentityServerInteractionService interaction,
            IHttpContextAccessor httpContext,
            IClientStore clientStore, 
            IAuthenticationSchemeProvider schemeProvider)
        {
            _interaction = interaction;
            _httpContextAccessor = httpContext;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
        }

        public async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                var lvm = new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Email = context.LoginHint,
                    ExternalProviders = new[] {new ExternalProvider(context.IdP)}
                };
                return lvm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null &&
                            !AccountOptions.WindowsAuthenticationSchemes.Contains(x.Name))
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                })
                .ToList();

            if (AccountOptions.WindowsAuthenticationEnabled)
            {
                // this is needed to handle windows auth schemes
                var windowsSchemes = schemes.Where(s =>
                    AccountOptions.WindowsAuthenticationSchemes.Contains(s.Name));
                if (windowsSchemes.Any())
                {
                    var externalProvider = new ExternalProvider
                    {
                        AuthenticationScheme = AccountOptions.WindowsAuthenticationSchemes.First(),
                        DisplayName = AccountOptions.WindowsAuthenticationDisplayName
                    };
                    providers.Add(externalProvider);
                }
            }

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                        providers = providers
                            .Where(provider => client
                                .IdentityProviderRestrictions
                                .Contains(provider.AuthenticationScheme))
                            .ToList();
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        public async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Email = model.Email;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        public async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            var user = _httpContextAccessor.HttpContext.User;
            if (user == null || user.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        public async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            var user = _httpContextAccessor.HttpContext.User;
            var idp = user?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (vm.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                vm.ExternalAuthenticationScheme = idp;
            }

            return vm;
        }
    }
}