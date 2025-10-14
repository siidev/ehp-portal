using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data.Models;
using SSOPortalX.Data.Security;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.User
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasherService _passwordHasher;

        public UserService(ApplicationDbContext context, PasswordHasherService passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Models.User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == username);
        }

                                public async Task UpdateUserAsync(UserDto userDto)
        {
            if (!int.TryParse(userDto.Id, out int userId))
            {
                return;
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Name = userDto.FullName;
                user.Username = userDto.UserName!;
                user.Email = userDto.Email!;
                user.Role = userDto.Role;
                user.IsActive = userDto.Status == "Active";
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Models.User> CreateUserAsync(UserDto userDto, string password)
        {
            var newUser = new Models.User
            {
                Name = userDto.FullName,
                Username = userDto.UserName!,
                Email = userDto.Email!,
                Role = userDto.Role,
                IsActive = userDto.Status == "Active",
                PasswordHash = _passwordHasher.HashPassword(password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<Models.User> CreateUserAsync(Models.User user)
        {
            // Ensure timestamps are set
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> ValidatePasswordAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }

        // The methods below are still using mock data and will be updated later.
                public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user == null ? null : new UserDto(user);
        }

        public async Task<List<UserDto>> GetListAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(u => new UserDto(u)).ToList();
        }


        public static List<string> GetRoleList() => new List<string>
        {
            "Admin", "User",
        };

        public static Dictionary<string, string> GetRoleIconMap() => new()
        {
            ["Admin"] = "mdi-account-key,error",
            ["User"] = "mdi-account,info",
        };

        public static List<string> GetPlanList() => new List<string>
        {
            "Basic", "Company", "Enterprise", "Team",
        };

        public static List<string> GetStatusList() => new List<string>
        {
            "Active", "Inactive",
        };

        public static List<string> GetLanguageList() => new List<string>
        {
            "English", "Spanish", "French", "Russian", "German", "Arabic","Sanskrit",
        };

        public static List<PermissionDto> GetPermissionsList() => new List<PermissionDto>()
        {
            new PermissionDto() { Module="Admin", Read = true },
            new PermissionDto() { Module="Staff", Write = true },
            new PermissionDto() { Module="Author", Read = true, Create = true },
            new PermissionDto() { Module="Contributor" },
            new PermissionDto() { Module="User", Delete = true },
        };
    }
}