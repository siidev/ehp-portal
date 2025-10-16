using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.Application
{
    public class ApplicationService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public ApplicationService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<SSOPortalX.Data.Models.Application>> GetApplicationsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Applications
                .Where(a => a.DeletedAt == null)
                .Include(a => a.Category)
                .ToListAsync();
        }

        public async Task<SSOPortalX.Data.Models.Application?> GetApplicationByIdAsync(int applicationId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Applications
                .Where(a => a.Id == applicationId && a.DeletedAt == null)
                .Include(a => a.Category)
                .FirstOrDefaultAsync();
        }

        public async Task CreateApplicationAsync(SSOPortalX.Data.Models.Application application)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Applications.Add(application);
            await context.SaveChangesAsync();
        }

        public async Task UpdateApplicationAsync(SSOPortalX.Data.Models.Application application)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Entry(application).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteApplicationAsync(int applicationId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var application = await context.Applications.FindAsync(applicationId);
            if (application != null && application.DeletedAt == null)
            {
                application.DeletedAt = DateTime.UtcNow;
                application.IsActive = false;
                application.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }
    }
}