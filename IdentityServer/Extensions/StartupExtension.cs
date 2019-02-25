using System.Globalization;
using IdentityServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Extensions
{
    public static class StartupExtension
    {
        public static void AddDbContexts<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString(ConfigurationConsts.AdminConnectionStringKey);
            services.AddDbContext<TContext>(options => options.UseSqlServer(connectionString));
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