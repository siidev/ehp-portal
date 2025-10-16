
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSOPortalX.Data.App.Category
{
    public class CategoryService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public CategoryService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<Models.Category>> GetCategoriesAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Categories
                .AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<Models.Category?> GetCategoryByIdAsync(int categoryId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Categories
                .AsNoTracking()
                .Where(c => c.Id == categoryId && c.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task CreateCategoryAsync(Models.Category category)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Models.Category category)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Entry(category).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var category = await context.Categories.FindAsync(categoryId);
            if (category != null && category.DeletedAt == null)
            {
                category.DeletedAt = DateTime.UtcNow;
                category.UpdatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }
    }
}
