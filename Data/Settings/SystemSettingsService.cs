using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;

namespace SSOPortalX.Data.Settings
{
    public class SystemSettingsService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SystemSettingsService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var s = await context.SiteSettings.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            var dict = new Dictionary<string, string>();
            if (s != null)
            {
                dict["site_name"] = s.SiteName;
                dict["site_description"] = s.SiteDescription ?? string.Empty;
                dict["site_logo"] = s.LogoUrl ?? string.Empty;
                dict["site_logo_small"] = s.LogoSmallUrl ?? string.Empty;
                dict["site_favicon"] = s.FaviconUrl ?? string.Empty;
                // theme color removed
                dict["site_keywords"] = s.Keywords ?? string.Empty;
            }
            return dict;
        }

        public async Task<string?> GetSettingAsync(string key)
        {
            using var context = _contextFactory.CreateDbContext();
            var s = await context.SiteSettings.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (s == null) return null;
            return key switch
            {
                "site_name" => s.SiteName,
                "site_description" => s.SiteDescription,
                "site_logo" => s.LogoUrl,
                "site_logo_small" => s.LogoSmallUrl,
                "site_favicon" => s.FaviconUrl,
                // theme color removed
                "site_keywords" => s.Keywords,
                _ => null
            };
        }

        public async Task SetSettingAsync(string key, string value, string type = "text", string? description = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var s = await context.SiteSettings
                .AsTracking() // enable tracking for updates (global default is NoTracking)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (s == null)
            {
                s = new SiteSettings();
                context.SiteSettings.Add(s);
            }
            switch (key)
            {
                case "site_name": s.SiteName = value; break;
                case "site_description": s.SiteDescription = value; break;
                case "site_logo": s.LogoUrl = value; break;
                case "site_logo_small": s.LogoSmallUrl = value; break;
                case "site_favicon": s.FaviconUrl = value; break;
                // theme color removed
                case "site_keywords": s.Keywords = value; break;
                default: break;
            }
            s.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task InitializeDefaultSettingsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var exists = await context.SiteSettings.AnyAsync();
            if (!exists)
            {
                var s = new SiteSettings
                {
                    SiteName = "EHP Portal",
                    SiteDescription = "Enterprise Health Portal",
                    LogoUrl = "/img/mainLayout/eagle-high-logo.png",
                    LogoSmallUrl = "/img/mainLayout/eagle-high-logo-small.png",
                    FaviconUrl = "/favicon.ico",
                    Keywords = "EHP, Portal, Enterprise, Health",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                context.SiteSettings.Add(s);
                await context.SaveChangesAsync();
            }
        }

        public async Task SetSettingsBatchAsync(Dictionary<string, string> values)
        {
            using var context = _contextFactory.CreateDbContext();
            var s = await context.SiteSettings
                .AsTracking() // enable tracking for updates (global default is NoTracking)
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (s == null)
            {
                s = new SiteSettings();
                context.SiteSettings.Add(s);
            }

            if (values.TryGetValue("site_name", out var siteName)) s.SiteName = siteName;
            if (values.TryGetValue("site_description", out var siteDesc)) s.SiteDescription = siteDesc;
            if (values.TryGetValue("site_logo", out var logo)) s.LogoUrl = logo;
            if (values.TryGetValue("site_logo_small", out var logoSmall)) s.LogoSmallUrl = logoSmall;
            if (values.TryGetValue("site_favicon", out var favicon)) s.FaviconUrl = favicon;
            // theme color removed
            if (values.TryGetValue("site_keywords", out var keywords)) s.Keywords = keywords;

            s.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task<SystemSettings?> GetSettingDetailsAsync(string key)
        {
            using var context = _contextFactory.CreateDbContext();
            // Map to a pseudo SystemSettings object for compatibility if needed
            var value = await GetSettingAsync(key);
            if (value == null) return null;
            return new SystemSettings
            {
                SettingKey = key,
                SettingValue = value,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public async Task<List<SystemSettings>> GetAllSettingsDetailsAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            var s = await context.SiteSettings.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            if (s == null) return new List<SystemSettings>();
            var list = new List<SystemSettings>
            {
                new SystemSettings{ SettingKey = "site_name", SettingValue = s.SiteName, IsActive = true },
                new SystemSettings{ SettingKey = "site_description", SettingValue = s.SiteDescription, IsActive = true },
                new SystemSettings{ SettingKey = "site_logo", SettingValue = s.LogoUrl, IsActive = true },
                new SystemSettings{ SettingKey = "site_logo_small", SettingValue = s.LogoSmallUrl, IsActive = true },
                new SystemSettings{ SettingKey = "site_favicon", SettingValue = s.FaviconUrl, IsActive = true },
                // theme color removed
                new SystemSettings{ SettingKey = "site_keywords", SettingValue = s.Keywords, IsActive = true }
            };
            return list;
        }
    }
}
