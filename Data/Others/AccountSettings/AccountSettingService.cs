using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data.Models;
using SSOPortalX.Data.Others.AccountSettings.Dto;

namespace SSOPortalX.Data.Others.AccountSettings
{
    public class AccountSettingService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly SSOPortalX.CookieStorage _cookieStorage;

        public AccountSettingService(IDbContextFactory<ApplicationDbContext> dbContextFactory, SSOPortalX.CookieStorage cookieStorage)
        {
            _dbContextFactory = dbContextFactory;
            _cookieStorage = cookieStorage;
        }

        public async Task<AccountDto> GetAccountAsync()
        {
            try
            {
                var currentUserId = await _cookieStorage.GetAsync("CurrentUserId");
                
                if (!string.IsNullOrEmpty(currentUserId) && int.TryParse(currentUserId, out int userId))
                {
                    using var context = _dbContextFactory.CreateDbContext();
                    var user = await context.Users.FindAsync(userId);
                    
                    if (user != null)
                    {
                        // Get all vendor names for this user
                        var vendors = await context.Vendors
                            .Where(v => v.UserId == userId)
                            .Select(v => v.Name)
                            .ToListAsync();
                        
                        var companyName = vendors.Count > 0 ? string.Join(", ", vendors) : "";
                        
                        return new AccountDto(user.Username, user.Name, user.Email, companyName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting account: {ex.Message}");
            }
            
            // Fallback to default
            return new AccountDto("guest", "Guest User", "guest@example.com", "");
        }

        public static List<CountryDto> GetCountryList() => new()
        {
            new("1", "USA"),
            new("2", "India"),
            new("3", "Canada"),
            new("4", "Indonesia"),
            new("5", "Singapore"),
            new("6", "Malaysia"),
        };

        public async Task<bool> UpdateAccountAsync(AccountDto account)
        {
            try
            {
                var currentUserId = await _cookieStorage.GetAsync("CurrentUserId");
                
                if (!string.IsNullOrEmpty(currentUserId) && int.TryParse(currentUserId, out int userId))
                {
                    using var context = _dbContextFactory.CreateDbContext();
                    var user = await context.Users.FindAsync(userId);
                    
                    if (user != null)
                    {
                        user.Username = account.UserName;
                        user.Name = account.Name;
                        user.Email = account.Email;
                        
                        await context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating account: {ex.Message}");
            }
            
            return false;
        }

        public async Task<VendorInformationDto> GetVendorInformationAsync()
        {
            try
            {
                var currentUserId = await _cookieStorage.GetAsync("CurrentUserId");
                
                if (!string.IsNullOrEmpty(currentUserId) && int.TryParse(currentUserId, out int userId))
                {
                    using var context = _dbContextFactory.CreateDbContext();
                    var vendor = await context.Vendors.FirstOrDefaultAsync(v => v.UserId == userId);
                    
                    if (vendor != null)
                    {
                        return new VendorInformationDto(
                            vendor.Id,
                            vendor.UserId,
                            vendor.Name ?? "",
                            vendor.Phone ?? "",
                            vendor.AddressStreet ?? "",
                            vendor.AddressCity ?? "",
                            vendor.AddressCountry ?? "",
                            vendor.AddressPostalCode ?? ""
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting vendor information: {ex.Message}");
            }
            
            // Return empty vendor if not found
            return new VendorInformationDto();
        }

        public async Task<bool> UpdateVendorInformationAsync(VendorInformationDto vendorInfo)
        {
            try
            {
                var currentUserId = await _cookieStorage.GetAsync("CurrentUserId");
                
                if (!string.IsNullOrEmpty(currentUserId) && int.TryParse(currentUserId, out int userId))
                {
                    using var context = _dbContextFactory.CreateDbContext();
                    var vendor = await context.Vendors.FirstOrDefaultAsync(v => v.UserId == userId);
                    
                    if (vendor != null)
                    {
                        // Update existing vendor
                        vendor.Name = vendorInfo.Name;
                        vendor.Phone = vendorInfo.Phone;
                        vendor.AddressStreet = vendorInfo.AddressStreet;
                        vendor.AddressCity = vendorInfo.AddressCity;
                        vendor.AddressCountry = vendorInfo.AddressCountry;
                        vendor.AddressPostalCode = vendorInfo.AddressPostalCode;
                        vendor.UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        // Create new vendor
                        vendor = new Vendor
                        {
                            UserId = userId,
                            Name = vendorInfo.Name,
                            Phone = vendorInfo.Phone,
                            AddressStreet = vendorInfo.AddressStreet,
                            AddressCity = vendorInfo.AddressCity,
                            AddressCountry = vendorInfo.AddressCountry,
                            AddressPostalCode = vendorInfo.AddressPostalCode,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        
                        context.Vendors.Add(vendor);
                    }
                    
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating vendor information: {ex.Message}");
            }
            
            return false;
        }

        public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            try
            {
                var currentUserId = await _cookieStorage.GetAsync("CurrentUserId");
                
                if (!string.IsNullOrEmpty(currentUserId) && int.TryParse(currentUserId, out int userId))
                {
                    using var context = _dbContextFactory.CreateDbContext();
                    var user = await context.Users.FindAsync(userId);
                    
                    if (user != null)
                    {
                        var passwordHasher = new SSOPortalX.Data.Security.PasswordHasherService();
                        
                        // Verify old password
                        if (!passwordHasher.VerifyPassword(oldPassword, user.PasswordHash))
                        {
                            return false; // Old password is incorrect
                        }
                        
                        // Hash new password and update
                        user.PasswordHash = passwordHasher.HashPassword(newPassword);
                        user.UpdatedAt = DateTime.Now;
                        
                        await context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing password: {ex.Message}");
            }
            
            return false;
        }

        public static InformationDto GetInformation() => new("", DateOnly.FromDateTime(DateTime.Now), "1", "", 6562542568);

        public static SocialDto GetSocial() => new("https://www.twitter.com", "", "", "https://www.linkedin.com", "", "");
    }
}
