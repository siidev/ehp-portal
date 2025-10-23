using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace SSOPortalX.Data.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetLink, string userName = "")
        {
            // Try primary SMTP configuration first
            if (await TrySendEmailAsync(toEmail, resetLink, userName))
            {
                return true;
            }

            // If primary fails, try fallback method (console output for development)
            Console.WriteLine($"⚠️ SMTP failed, using fallback method for: {toEmail}");
            return await SendEmailFallbackAsync(toEmail, resetLink, userName);
        }

        private async Task<bool> TrySendEmailAsync(string toEmail, string resetLink, string userName)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");
                var enableTls = bool.Parse(_configuration["EmailSettings:EnableTls"] ?? "true");
                var requireAuth = bool.Parse(_configuration["EmailSettings:RequireAuthentication"] ?? "true");

                // Set security protocol for Office365
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using var client = new SmtpClient(smtpHost, smtpPort);
                
                if (requireAuth)
                {
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                }
                
                client.EnableSsl = enableSsl;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                
                // Additional settings for Office365
                client.Timeout = 30000; // 30 seconds timeout
                
                // Try different authentication methods for Office365
                try
                {
                    // Method 1: Standard SMTP with SSL
                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(fromEmail!, fromName);
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = "Password Reset Request - SSO Portal";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = GeneratePasswordResetEmailTemplate(resetLink, userName);
                    mailMessage.Priority = MailPriority.Normal;

                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine($"✅ Email sent successfully to: {toEmail}");
                    return true;
                }
                catch (Exception smtpEx)
                {
                    Console.WriteLine($"❌ SMTP Method 1 failed: {smtpEx.Message}");
                    
                    // Method 2: Try with different port (465 for SSL)
                    if (smtpPort == 587)
                    {
                        Console.WriteLine("🔄 Trying alternative SMTP configuration...");
                        return await TryAlternativeSmtpConfigAsync(toEmail, resetLink, userName);
                    }
                    
                    throw smtpEx;
                }
            }
            catch (Exception ex)
            {
                // Log the exception with more details
                Console.WriteLine($"❌ Failed to send email: {ex.Message}");
                Console.WriteLine($"❌ SMTP Host: {_configuration["EmailSettings:SmtpHost"]}");
                Console.WriteLine($"❌ SMTP Port: {_configuration["EmailSettings:SmtpPort"]}");
                Console.WriteLine($"❌ From Email: {_configuration["EmailSettings:FromEmail"]}");
                return false;
            }
        }

        private async Task<bool> TryAlternativeSmtpConfigAsync(string toEmail, string resetLink, string userName)
        {
            try
            {
                // Try with port 465 (SSL) instead of 587 (TLS)
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];

                using var client = new SmtpClient(smtpHost, 465);
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Timeout = 30000;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail!, fromName);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = "Password Reset Request - SSO Portal";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = GeneratePasswordResetEmailTemplate(resetLink, userName);
                mailMessage.Priority = MailPriority.Normal;

                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"✅ Email sent successfully via alternative config to: {toEmail}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Alternative SMTP config also failed: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SendEmailFallbackAsync(string toEmail, string resetLink, string userName)
        {
            try
            {
                // Fallback: Log to console for development/testing
                Console.WriteLine("=".PadRight(80, '='));
                Console.WriteLine("📧 PASSWORD RESET EMAIL (FALLBACK MODE)");
                Console.WriteLine("=".PadRight(80, '='));
                Console.WriteLine($"To: {toEmail}");
                Console.WriteLine($"User: {userName}");
                Console.WriteLine($"Reset Link: {resetLink}");
                Console.WriteLine($"Expires: {DateTime.UtcNow.AddHours(24):yyyy-MM-dd HH:mm:ss} UTC");
                Console.WriteLine("=".PadRight(80, '='));
                Console.WriteLine("📝 Email Content:");
                Console.WriteLine(GeneratePasswordResetEmailTemplate(resetLink, userName));
                Console.WriteLine("=".PadRight(80, '='));
                Console.WriteLine("🔧 OFFICE365 SMTP TROUBLESHOOTING:");
                Console.WriteLine("1. Enable 2FA on Office365 account");
                Console.WriteLine("2. Generate App Password: https://account.microsoft.com/security");
                Console.WriteLine("3. Use App Password instead of regular password");
                Console.WriteLine("4. Ensure 'Less secure app access' is enabled (if available)");
                Console.WriteLine("5. Try port 465 with SSL instead of 587 with TLS");
                Console.WriteLine("=".PadRight(80, '='));
                
                // Simulate async operation
                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Fallback email failed: {ex.Message}");
                return false;
            }
        }

        private string GeneratePasswordResetEmailTemplate(string resetLink, string userName)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: #4318FF; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background: #f9f9f9; }}
        .button {{ background: #4318FF; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; }}
        .footer {{ padding: 20px; text-align: center; font-size: 12px; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Password Reset Request</h2>
        </div>
        <div class='content'>
            <p>Hello {(string.IsNullOrEmpty(userName) ? "" : userName + ",")}</p>
            <p>You have requested to reset your password for your SSO Portal account.</p>
            <p>Click the button below to reset your password:</p>
            <p style='text-align: center; margin: 30px 0;'>
                <a href='{resetLink}' class='button'>Reset Password</a>
            </p>
            <p>If the button above doesn't work, copy and paste this link into your browser:</p>
            <p style='word-break: break-all; background: #eee; padding: 10px;'>{resetLink}</p>
            <p><strong>Important:</strong> This link will expire in 24 hours for security reasons.</p>
            <p>If you did not request a password reset, please ignore this email or contact support if you have concerns.</p>
        </div>
        <div class='footer'>
            <p>This email was sent from SSO Portal. Please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}