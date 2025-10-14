using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.Application
{
    public class ApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SSOPortalX.Data.Models.Application>> GetApplicationsAsync()
        {
            return await _context.Applications.Include(a => a.Category).ToListAsync();
        }

        public async Task<SSOPortalX.Data.Models.Application?> GetApplicationByIdAsync(int applicationId)
        {
            return await _context.Applications.Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public async Task CreateApplicationAsync(SSOPortalX.Data.Models.Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplicationAsync(SSOPortalX.Data.Models.Application application)
        {
            _context.Entry(application).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteApplicationAsync(int applicationId)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application != null)
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            }
        }
    }
}