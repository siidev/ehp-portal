
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data.Models;
using Application = SSOPortalX.Data.Models.Application;

namespace SSOPortalX.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<SsoPortalToken> SsoPortalTokens { get; set; } = null!;
        public DbSet<UserAppAccess> UserAppAccesses { get; set; } = null!;
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; } = null!;
        public DbSet<Vendor> Vendors { get; set; } = null!;
        public DbSet<SiteSettings> SiteSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for UserAppAccess
            modelBuilder.Entity<UserAppAccess>()
                .HasKey(uaa => new { uaa.UserId, uaa.AppId });

            // Configure relationships
            modelBuilder.Entity<UserAppAccess>()
                .HasOne(uaa => uaa.User)
                .WithMany()
                .HasForeignKey(uaa => uaa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAppAccess>()
                .HasOne(uaa => uaa.App)
                .WithMany()
                .HasForeignKey(uaa => uaa.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Vendor relationship
            modelBuilder.Entity<Vendor>()
                .HasOne(v => v.User)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for SsoPortalToken performance
            modelBuilder.Entity<SsoPortalToken>()
                .HasIndex(t => t.Token)
                .HasDatabaseName("IX_sso_portal_tokens_token");

            modelBuilder.Entity<SsoPortalToken>()
                .HasIndex(t => new { t.UserId, t.IsActive, t.ExpiresAt })
                .HasDatabaseName("IX_sso_portal_tokens_user_active_exp");
        }
    }
}
