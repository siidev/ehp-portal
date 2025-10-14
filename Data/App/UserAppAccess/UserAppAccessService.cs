
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.UserAppAccess
{
    public class UserAppAccessService
    {
        private readonly ApplicationDbContext _context;

        public UserAppAccessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetAppIdsForUserAsync(int userId)
        {
            return await _context.UserAppAccesses
                .Where(uaa => uaa.UserId == userId)
                .Select(uaa => uaa.AppId)
                .ToListAsync();
        }

        public async Task UpdateUserAccessAsync(int userId, List<int> newAppIds, int? grantedByUserId = null, string? notes = null)
        {
            var currentAccesses = await _context.UserAppAccesses
                .Where(uaa => uaa.UserId == userId)
                .ToListAsync();

            var currentAppIds = currentAccesses.Select(a => a.AppId).ToList();

            var toAdd = newAppIds.Except(currentAppIds);
            var toRemove = currentAppIds.Except(newAppIds);

            // Remove old accesses
            _context.UserAppAccesses.RemoveRange(currentAccesses.Where(a => toRemove.Contains(a.AppId)));

            // Add new accesses
            foreach (var appId in toAdd)
            {
                _context.UserAppAccesses.Add(new Models.UserAppAccess
                {
                    UserId = userId,
                    AppId = appId,
                    GrantedAt = System.DateTime.UtcNow,
                    GrantedBy = grantedByUserId,
                    Notes = notes
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DateTime?> GetEarliestGrantedAtForUserAsync(int userId)
        {
            return await _context.UserAppAccesses
                .Where(uaa => uaa.UserId == userId)
                .Select(uaa => uaa.GrantedAt)
                .OrderBy(grantedAt => grantedAt)
                .FirstOrDefaultAsync();
        }
    }
}
