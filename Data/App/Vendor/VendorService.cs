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
        private readonly ApplicationDbContext _context;

        public VendorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VendorModel>> GetVendorsAsync()
        {
            return await _context.Vendors
                .Include(v => v.User)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<List<VendorModel>> GetVendorsByUserIdAsync(int userId)
        {
            return await _context.Vendors
                .Include(v => v.User)
                .Where(v => v.UserId == userId)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<VendorModel?> GetVendorByIdAsync(int vendorId)
        {
            return await _context.Vendors
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.Id == vendorId);
        }

        public async Task CreateVendorAsync(VendorModel vendor)
        {
            vendor.CreatedAt = DateTime.UtcNow;
            vendor.UpdatedAt = DateTime.UtcNow;
            
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVendorAsync(VendorModel vendor)
        {
            vendor.UpdatedAt = DateTime.UtcNow;
            _context.Entry(vendor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVendorAsync(int vendorId)
        {
            var vendor = await _context.Vendors.FindAsync(vendorId);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> VendorExistsAsync(int vendorId)
        {
            return await _context.Vendors.AnyAsync(v => v.Id == vendorId);
        }

        public async Task<List<VendorModel>> SearchVendorsAsync(string searchTerm)
        {
            return await _context.Vendors
                .Include(v => v.User)
                .Where(v => v.Name.Contains(searchTerm) || 
                           v.Phone!.Contains(searchTerm) ||
                           v.AddressCity!.Contains(searchTerm) ||
                           v.AddressCountry!.Contains(searchTerm))
                .OrderBy(v => v.Name)
                .ToListAsync();
        }
    }
}
