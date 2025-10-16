using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendorModel = SSOPortalX.Data.Models.Vendor;

namespace SSOPortalX.Data.App.Vendor
{
    public class VendorService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public VendorService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<VendorModel>> GetVendorsAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Vendors
                .AsNoTracking()
                .Where(v => v.DeletedAt == null)
                .Include(v => v.User)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<List<VendorModel>> GetVendorsByUserIdAsync(int userId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Vendors
                .AsNoTracking()
                .Where(v => v.UserId == userId && v.DeletedAt == null)
                .Include(v => v.User)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<VendorModel?> GetVendorByIdAsync(int vendorId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Vendors
                .AsNoTracking()
                .Where(v => v.Id == vendorId && v.DeletedAt == null)
                .Include(v => v.User)
                .FirstOrDefaultAsync();
        }

        public async Task CreateVendorAsync(VendorModel vendor)
        {
            vendor.CreatedAt = DateTime.UtcNow;
            vendor.UpdatedAt = DateTime.UtcNow;

            using var context = _dbContextFactory.CreateDbContext();
            context.Vendors.Add(vendor);
            await context.SaveChangesAsync();
        }

        public async Task UpdateVendorAsync(VendorModel vendor)
        {
            vendor.UpdatedAt = DateTime.UtcNow;
            using var context = _dbContextFactory.CreateDbContext();
            context.Entry(vendor).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteVendorAsync(int vendorId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var vendor = await context.Vendors.FindAsync(vendorId);
            if (vendor != null && vendor.DeletedAt == null)
            {
                vendor.DeletedAt = DateTime.UtcNow;
                vendor.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> VendorExistsAsync(int vendorId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Vendors.AsNoTracking().AnyAsync(v => v.Id == vendorId && v.DeletedAt == null);
        }

        public async Task<List<VendorModel>> SearchVendorsAsync(string searchTerm)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Vendors
                .AsNoTracking()
                .Where(v => v.DeletedAt == null && 
                           (v.Name.Contains(searchTerm) || 
                           v.Phone!.Contains(searchTerm) ||
                           v.AddressCity!.Contains(searchTerm) ||
                           v.AddressCountry!.Contains(searchTerm)))
                .Include(v => v.User)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }
    }
}
