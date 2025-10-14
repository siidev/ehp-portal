using Microsoft.EntityFrameworkCore;
using SSOPortalX.Data;
using SSOPortalX.Data.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMasaBlazor(builder =>
{
    builder.ConfigureTheme(theme =>
    {
        theme.Themes.Light.Primary = "#4318FF";
        theme.Themes.Light.Accent = "#4318FF";
    });
}).AddI18nForServer("wwwroot/i18n");


var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? throw new Exception("Get the assembly root directory exception!");
builder.Services.AddNav(Path.Combine(basePath, $"wwwroot/nav/nav.json"));
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped<ApplicationDbContext>(serviceProvider =>
{
    var factory = serviceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
    return factory.CreateDbContext();
});
builder.Services.AddScoped<CookieStorage>();
builder.Services.AddScoped<GlobalConfig>();
builder.Services.AddSingleton<CaptchaService>();
builder.Services.AddSingleton<PasswordHasherService>();
builder.Services.AddScoped<SSOPortalX.Data.App.User.UserService>();
builder.Services.AddScoped<SSOPortalX.Data.App.Category.CategoryService>();
builder.Services.AddScoped<SSOPortalX.Data.App.Application.ApplicationService>();
builder.Services.AddScoped<SSOPortalX.Data.App.UserAppAccess.UserAppAccessService>();
builder.Services.AddScoped<SSOPortalX.Data.App.Vendor.VendorService>();
builder.Services.AddScoped<SSOPortalX.Data.Sso.SsoTokenService>();
builder.Services.AddScoped<SSOPortalX.Data.Security.PasswordResetService>();
builder.Services.AddScoped<SSOPortalX.Data.Others.AccountSettings.AccountSettingService>();
builder.Services.AddScoped<SSOPortalX.Data.Services.EmailService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<SSOPortalX.Data.Webhook.WebhookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapGet("/validate-token", async (string token, SSOPortalX.Data.Sso.SsoTokenService tokenService) =>
{
    var validatedToken = await tokenService.ValidateTokenAsync(token);
    if (validatedToken != null)
    {
        return Results.Ok(new
        {
            valid = true,
            user = new
            {
                id = validatedToken.User.Id,
                username = validatedToken.User.Username,
                email = validatedToken.User.Email,
                name = validatedToken.User.Name,
                role = validatedToken.User.Role
            },
            app = new
            {
                id = validatedToken.App.Id,
                code = validatedToken.App.Code,
                name = validatedToken.App.Name
            },
            expires_at = validatedToken.ExpiresAt
        });
    }
    else
    {
        return Results.Ok(new { valid = false, error = "Token invalid or expired" });
    }
});

app.MapFallbackToPage("/_Host");

app.Run();