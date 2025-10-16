
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.UserAppAccess
{
    public class UserAppAccessService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public UserAppAccessService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<int>> GetAppIdsForUserAsync(int userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.UserAppAccesses
                .Where(uaa => uaa.UserId == userId)
                .Select(uaa => uaa.AppId)
                .ToListAsync();
        }

        public async Task UpdateUserAccessAsync(int userId, List<int> newAppIds, int? grantedByUserId = null, string? notes = null)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var currentAccesses = await context.UserAppAccesses
                .Where(uaa => uaa.UserId == userId)
                .ToListAsync();

            var currentAppIds = currentAccesses.Select(a => a.AppId).ToList();

            var toAdd = newAppIds.Except(currentAppIds);
            var toRemove = currentAppIds.Except(newAppIds);

            // Remove old accesses
            context.UserAppAccesses.RemoveRange(currentAccesses.Where(a => toRemove.Contains(a.AppId)));

            // Add new accesses
            foreach (var appId in toAdd)
            {
                context.UserAppAccesses.Add(new Models.UserAppAccess
                {
                    UserId = userId,
                    AppId = appId,
                    GrantedAt = System.DateTime.UtcNow,
                    GrantedBy = grantedByUserId,
                    Notes = notes
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task<DateTime?> GetEarliestGrantedAtForUserAsync(int userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.UserAppAccesses
                .Where(uaa => uaa.UserId == userId)
                .Select(uaa => uaa.GrantedAt)
                .OrderBy(grantedAt => grantedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<int, (int applicationsCount, DateTime? earliestGrantedAt)>> GetAccessSummaryForUsersAsync(List<int> userIds, HashSet<int>? filterActiveAppIds = null)
        {
            using var context = _dbContextFactory.CreateDbContext();

            var query = context.UserAppAccesses
                .AsNoTracking()
                .Where(uaa => userIds.Contains(uaa.UserId));

            if (filterActiveAppIds != null && filterActiveAppIds.Count > 0)
            {
                query = query.Where(uaa => filterActiveAppIds.Contains(uaa.AppId));
            }

            var grouped = await query
                .GroupBy(uaa => uaa.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count(), Earliest = g.Min(x => x.GrantedAt) })
                .ToListAsync();

            var result = new Dictionary<int, (int, DateTime?)>();
            foreach (var item in grouped)
            {
                result[item.UserId] = (item.Count, item.Earliest);
            }

            // Pastikan semua userId ada di dictionary (default 0, null)
            foreach (var uid in userIds)
            {
                if (!result.ContainsKey(uid))
                {
                    result[uid] = (0, null);
                }
            }

            return result;
        }
    }
}
