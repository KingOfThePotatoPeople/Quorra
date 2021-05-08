using System;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quorra.EntityFramework.Entities;
using Quorra.EntityFramework.Identity;

namespace Quorra.EntityFramework
{
    public class QuorraDbContext : IdentityDbContext<QuorraUser, QuorraRole, Guid, QuorraUserClaim, QuorraUserRole, QuorraUserLogin, QuorraRoleClaim, QuorraUserToken>
    {
      
        public QuorraDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
     
        // Audit / Event Log
        public DbSet<EventLog> EventLog { get; set; }
        
        
        // ----------------------------------------------------------------
        //  ON CONFIGURING
        // ----------------------------------------------------------------

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");

                // Get configuration
                IConfiguration config = configBuilder.Build();

                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                // Useful for debugging (not for prod)
                //optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }
        
        
        // ----------------------------------------------------------------
        //          ON MODEL CREATING
        // ----------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ----------------------------------------------------------------
            // Typical format is to group into specific collections
            // and layout KEY > INDEXES > RELATIONSHIP > FILTERS
            // ----------------------------------------------------------------
            
            base.OnModelCreating(builder);
            
            // ----------------------------------------------------------------
            //  IDENTITY
            // ----------------------------------------------------------------
            
            // QuorraUser
            builder.Entity<QuorraUser>(b =>
            {
                // Primary key
                b.HasKey(u => u.Id);
                // Indexes
                b.HasIndex(x => new {x.UserName, x.Email}).IsUnique();
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
                // Map
                b.ToTable("Users");
                // Properties
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);
                // Query Filters
                //b.HasQueryFilter(x => !x.Deleted);
                // Relationships
                b.HasMany<QuorraUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<QuorraUserLogin>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<QuorraUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany<QuorraUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            });

            // QuorraUserClaim
            builder.Entity<QuorraUserClaim>(b =>
            {
                // Primary key
                b.HasKey(uc => uc.Id);
                // Map
                b.ToTable("UserClaims");
            });

            // QuorraUserLogin
            builder.Entity<QuorraUserLogin>(b =>
            {
                // Primary key
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                // Properties
                b.Property(l => l.LoginProvider).HasMaxLength(128);
                b.Property(l => l.ProviderKey).HasMaxLength(128);
                // Map
                b.ToTable("UserLogins");
            });

            // QuorraUserToken
            builder.Entity<QuorraUserToken>(b =>
            {
                // Primary key
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
                // Properties
                b.Property(t => t.LoginProvider).HasMaxLength(128);
                b.Property(t => t.Name).HasMaxLength(128);
                // Map
                b.ToTable("UserTokens");
            });

            // QuorraUserRole
            builder.Entity<QuorraRole>(b =>
            {
                // Primary key
                b.HasKey(r => r.Id);
                // Indexes
                b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
                // Map
                b.ToTable("Roles");
                // Properties
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);
                // Relationships
                b.HasMany<QuorraUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany<QuorraRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            // QuorraRoleClaims
            builder.Entity<QuorraRoleClaim>(b =>
            {
                // Primary key
                b.HasKey(rc => rc.Id);
                // Map
                b.ToTable("UserRoleClaims");
            });

            // QuorraUserRole
            builder.Entity<QuorraUserRole>(b =>
            {
                // Primary key
                b.HasKey(r => new { r.UserId, r.RoleId });
                // Map
                b.ToTable("UserRoles");
            });
            
            // ----------------------------------------------------------------
            //  ENTITIES
            // ----------------------------------------------------------------
            
            // EventLog
            builder.Entity<EventLog>().HasKey(x => x.Id);
            builder.Entity<EventLog>().HasIndex(x => x.Id).IsUnique();
            builder.Entity<EventLog>().HasIndex(x => new {x.Logged, x.Level, x.UserName});
            

        }

        
    }
}