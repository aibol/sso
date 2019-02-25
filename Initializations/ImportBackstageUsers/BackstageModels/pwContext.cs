using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImportBackstageUsers.BackstageModels
{
    public partial class pwContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public pwContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public pwContext(DbContextOptions<pwContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<AibolLogin> AibolLogin { get; set; }
        public virtual DbSet<AibolUser> AibolUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("PwServer"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "pw");

            modelBuilder.Entity<AibolLogin>(entity =>
            {
                entity.ToTable("Aibol.Login");

                entity.HasIndex(e => new { e.Mobile, e.Email, e.UserName })
                    .HasName("UserLogin");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastLogonWhen).HasColumnType("datetime");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordEncrypted)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ResetPasswordToken)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AibolUser>(entity =>
            {
                entity.ToTable("Aibol.User");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Avatar)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedWhen).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdentityCard).HasMaxLength(100);

                entity.Property(e => e.JobPosition).HasMaxLength(50);

                entity.Property(e => e.JobTitle).HasMaxLength(50);

                entity.Property(e => e.LastModifiedWhen).HasColumnType("datetime");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PostId).HasColumnName("PostID");

                entity.Property(e => e.Provider).HasMaxLength(200);

                entity.Property(e => e.ProviderId)
                    .HasColumnName("ProviderID")
                    .HasMaxLength(200);

                entity.Property(e => e.SpellChar)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenantBvalue)
                    .HasColumnName("TenantBValue")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenantId).HasColumnName("TenantID");

                entity.Property(e => e.TenantTypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
