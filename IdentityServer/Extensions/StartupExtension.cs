using System.Globalization;
using System.Reflection;
using IdentityServer.Externals.Weibo;
using IdentityServer.Externals.Weixin;
using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer4;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Extensions
{
    public static class StartupExtension
    {
        public static void AddDbContexts<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString(ConfigurationConsts.AdminConnectionStringKey);
            services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString));

            var operationalStoreOptions = new OperationalStoreOptions();
            services.AddSingleton(operationalStoreOptions);

            var storeOptions = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptions);
        }

        public static void AddAuthenticationServices<TContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, 
            IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILogger logger)
            where TContext : DbContext 
            where TUserIdentity : class 
            where TUserIdentityRole : class
        {
            var connectionString = configuration.GetConnectionString(ConfigurationConsts.AdminConnectionStringKey);
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Add framework services.
            services.AddIdentity<TUserIdentity, TUserIdentityRole>()
                .AddEntityFrameworkStores<TContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<Aibol2IdentityErrorDescriber>();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<TUserIdentity>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                });

            builder.AddCustomSigningCredential(configuration, logger);
            builder.AddCustomValidationKey(configuration, logger);

            // external logins
            services.AddAuthentication()
                .AddWeibo(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "abcd";
                    options.ClientSecret = "23242";
                })
                .AddWeixin(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "abcd";
                    options.ClientSecret = "23242";
                });
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // cookie policy
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public static void AddMvcLocalization(this IServiceCollection services)
        {
            services.AddLocalization(opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = ConfigurationConsts.ResourcesPath; })
                .AddDataAnnotationsLocalization();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new[]
                    {
                        new CultureInfo("en"),
                        new CultureInfo("zh")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });
        }
    }
}