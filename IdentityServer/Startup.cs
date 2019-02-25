using System.Linq;
using System.Reflection;
using IdentityServer.Data;
using IdentityServer.Extensions;
using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContexts<ApplicationDbContext>(Configuration);

            services.AddAuthenticationServices<ApplicationDbContext, UserIdentity, UserIdentityRole>(Environment, Configuration, Logger);

            services.AddMvcLocalization();


            /**************************/

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Add framework services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<Aibol2IdentityErrorDescriber>();


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                // this adds the config data from DB (clients, resources)
                //      .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //      .AddInMemoryClients(Config.GetClients())
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(conn, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(conn, sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600;
                })
                .AddAspNetIdentity<ApplicationUser>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // this will do the initial DB population
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();

            loggerFactory.AddLog4Net();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var userContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                userContext.Database.Migrate();

                var configContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configContext.Database.Migrate();

                if (!userContext.Users.Any())
                {
                    foreach (var user in Config.GetUsers())
                        userContext.Users.Add(user);
                    userContext.SaveChanges();
                }

                if (!configContext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                        configContext.Clients.Add(client.ToEntity());
                    configContext.SaveChanges();
                }

                if (!configContext.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                        configContext.IdentityResources.Add(resource.ToEntity());
                    configContext.SaveChanges();
                }

                if (!configContext.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                        configContext.ApiResources.Add(resource.ToEntity());
                    configContext.SaveChanges();
                }
            }
        }
    }
}