using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        ///// <summary>
        /////     Gets or sets the Microsoft.EntityFrameworkCore.DbSet`1 of ApplicationClients.
        ///// </summary>
        //public DbSet<ApplicationClient> ApplicationClients { get; set; }
    }
}