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
        private readonly ApplicationDbContext _context;
        private readonly WebhookService _webhookService;

        public SsoTokenService(ApplicationDbContext context, WebhookService webhookService)
        {
            _context = context;
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

            _context.SsoPortalTokens.Add(token);
            await _context.SaveChangesAsync();

            // Fetch the related entities needed for the webhook payload
            var dbToken = await _context.SsoPortalTokens
                .Include(t => t.User)
                .Include(t => t.App)
                .FirstAsync(t => t.Id == token.Id);

            if (!string.IsNullOrEmpty(dbToken.App.WebhookUrl) && !string.IsNullOrEmpty(dbToken.App.WebhookSecret))
            {
                var payload = new
                {
                    @event = "token.created",
                    timestamp = System.DateTime.UtcNow,
                    data = new
                    {
                        token = dbToken.Token,
                        user = new
                        {
                            id = dbToken.User.Id,
                            username = dbToken.User.Username,
                            email = dbToken.User.Email,
                            name = dbToken.User.Name,
                            role = dbToken.User.Role
                        },
                        expires_at = dbToken.ExpiresAt
                    }
                };

                await _webhookService.SendWebhookAsync(dbToken.App.WebhookUrl, dbToken.App.WebhookSecret, payload);
            }

            return token;
        }

        public async Task<SsoPortalToken?> GetOrCreateTokenAsync(int userId, int appId)
        {
            // Cek apakah sudah ada token yang masih valid
            var existingToken = await _context.SsoPortalTokens
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
            var token = await _context.SsoPortalTokens
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
            var userTokens = await _context.SsoPortalTokens
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();

            foreach (var token in userTokens)
            {
                token.IsActive = false; // Deactivate instead of delete for audit trail
            }

            await _context.SaveChangesAsync();
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