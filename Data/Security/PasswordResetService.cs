
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SSOPortalX.Data.Security
{
    public class PasswordResetService
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PasswordResetToken?> GenerateResetTokenAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return null; // Don't reveal that the user doesn't exist
                }

                // Check if there's already an active reset token for this user
                var existingToken = await _context.PasswordResetTokens
                    .FirstOrDefaultAsync(t => t.UserId == user.Id && !t.IsUsed && t.ExpiresAt > System.DateTime.UtcNow);
                
                if (existingToken != null)
                {
                    // Update existing token instead of creating new one
                    existingToken.Token = GenerateSecureToken();
                    existingToken.ExpiresAt = System.DateTime.UtcNow.AddHours(24);
                    existingToken.CreatedAt = System.DateTime.UtcNow;
                    
                    await _context.SaveChangesAsync();
                    return existingToken;
                }

                var tokenValue = GenerateSecureToken();

                var resetToken = new PasswordResetToken
                {
                    UserId = user.Id,
                    // Don't assign User property to avoid EF tracking conflicts
                    Token = tokenValue,
                    ExpiresAt = System.DateTime.UtcNow.AddHours(24), // Token valid for 24 hours
                    IsUsed = false
                };

                _context.PasswordResetTokens.Add(resetToken);
                await _context.SaveChangesAsync();

                return resetToken;
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                System.Console.WriteLine($"Error generating reset token: {ex.Message}");
                System.Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Don't re-throw to prevent circuit termination
                // Return null instead to allow graceful handling
                return null;
            }
        }

        public async Task<PasswordResetToken?> ValidateResetTokenAsync(string tokenValue)
        {
            var token = await _context.PasswordResetTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == tokenValue);

            if (token == null || token.IsUsed || token.ExpiresAt < System.DateTime.UtcNow)
            {
                return null;
            }

            return token;
        }

        public async Task<bool> ResetPasswordAsync(string tokenValue, string newPassword)
        {
            var token = await ValidateResetTokenAsync(tokenValue);
            if (token == null)
            {
                return false;
            }

            var passwordHasher = new PasswordHasherService();
            token.User.PasswordHash = passwordHasher.HashPassword(newPassword);
            token.IsUsed = true;
            token.UsedAt = System.DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
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
