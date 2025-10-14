
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Models;
using SSOPortalX.Data.Security;
using System.Threading.Tasks;

namespace SSOPortalX.Pages
{
    public partial class SetupAdmin : ComponentBase
    {
        [Inject]
        private ApplicationDbContext DbContext { get; set; } = default!;

        [Inject]
        private PasswordHasherService PasswordHasher { get; set; } = default!;

        public string Message { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            var adminUser = await DbContext.Users.FirstOrDefaultAsync(u => u.Username == "admin");

            if (adminUser == null)
            {
                var newUser = new User
                {
                    Name = "Administrator",
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = PasswordHasher.HashPassword("admin"),
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = System.DateTime.UtcNow,
                    UpdatedAt = System.DateTime.UtcNow
                };

                DbContext.Users.Add(newUser);
                await DbContext.SaveChangesAsync();

                Message = "Admin user 'admin' with password 'admin' has been created successfully.";
            }
            else
            {
                Message = "Admin user already exists.";
            }
        }
    }
}
