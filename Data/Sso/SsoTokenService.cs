using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;
using SSOPortalX.Data.Webhook;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SSOPortalX.Data.Sso
{
    public class SsoTokenService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly WebhookService _webhookService;

        public SsoTokenService(IDbContextFactory<ApplicationDbContext> contextFactory, WebhookService webhookService)
        {
            _contextFactory = contextFactory;
            _webhookService = webhookService;
        }

        public async Task<SsoPortalToken> GenerateTokenAsync(int userId, int appId)
        {
            var tokenValue = GenerateSecureToken();

            var token = new SsoPortalToken
            {
                UserId = userId,
                AppId = appId,
                Token = tokenValue,
                ExpiresAt = System.DateTime.UtcNow.AddHours(8), // Token valid for 8 hours
                IsActive = true
            };

            using var context = _contextFactory.CreateDbContext();
            context.SsoPortalTokens.Add(token);
            await context.SaveChangesAsync();

            // Ambil data minimum untuk payload tanpa Include berat
            var user = await context.Users.AsNoTracking().FirstAsync(u => u.Id == userId);
            var app = await context.Applications.AsNoTracking().FirstAsync(a => a.Id == appId);

            if (!string.IsNullOrEmpty(app.WebhookUrl) && !string.IsNullOrEmpty(app.WebhookSecret))
            {
                var payload = new
                {
                    @event = "token.created",
                    timestamp = System.DateTime.UtcNow,
                    data = new
                    {
                        token = token.Token,
                        user = new
                        {
                            id = user.Id,
                            username = user.Username,
                            email = user.Email,
                            name = user.Name,
                            role = user.Role
                        },
                        expires_at = token.ExpiresAt
                    }
                };

                // Non-blocking webhook dengan timeout pendek di dalam service
                _ = _webhookService.SendWebhookAsync(app.WebhookUrl, app.WebhookSecret, payload);
            }

            return token;
        }

        public async Task<SsoPortalToken?> GetOrCreateTokenAsync(int userId, int appId)
        {
            // Cek apakah sudah ada token yang masih valid
            using var context = _contextFactory.CreateDbContext();
            var existingToken = await context.SsoPortalTokens
                .Include(t => t.User)
                .Include(t => t.App)
                .FirstOrDefaultAsync(t => t.UserId == userId && t.AppId == appId && t.IsActive && t.ExpiresAt > System.DateTime.UtcNow);

            if (existingToken != null)
            {
                return existingToken; // Gunakan token yang sudah ada
            }

            // Buat token baru jika belum ada atau sudah expire
            return await GenerateTokenAsync(userId, appId);
        }

        public async Task<SsoPortalToken?> ValidateTokenAsync(string tokenValue)
        {
            using var context = _contextFactory.CreateDbContext();
            var token = await context.SsoPortalTokens
                .Include(t => t.User)
                .Include(t => t.App)
                .FirstOrDefaultAsync(t => t.Token == tokenValue);

            if (token == null || !token.IsActive || token.ExpiresAt < System.DateTime.UtcNow)
            {
                return null;
            }

            return token;
        }

        public async Task ClearUserTokensAsync(int userId)
        {
            using var context = _contextFactory.CreateDbContext();
            // Single batch UPDATE untuk menonaktifkan token aktif milik user
            var sql = "UPDATE sso_portal_tokens SET is_active = 0, updated_at = UTC_TIMESTAMP() WHERE user_id = {0} AND is_active = 1";
            await context.Database.ExecuteSqlRawAsync(sql, userId);
        }

        private string GenerateSecureToken(int length = 64)
        {
            var byteArray = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
            }
            return System.Convert.ToBase64String(byteArray)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }
    }
}