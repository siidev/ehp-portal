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
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");

                using var client = new SmtpClient(smtpHost, smtpPort);
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                client.EnableSsl = enableSsl;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail!, fromName);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = "Password Reset Request - SSO Portal";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = GeneratePasswordResetEmailTemplate(resetLink, userName);

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Failed to send email: {ex.Message}");
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