
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.Category
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Models.Category?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task CreateCategoryAsync(Models.Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Models.Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
