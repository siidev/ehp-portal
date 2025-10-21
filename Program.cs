using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using SSOPortalX.Data;
using SSOPortalX.Data.Security;

var builder = WebApplication.CreateBuilder(args);

// Force Production environment if not explicitly set
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
{
    builder.Environment.EnvironmentName = "Production";
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Authentication & Authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/authentication/login";
        options.LogoutPath = "/authentication/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
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
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
    {
        mysqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(2), errorNumbersToAdd: null);
        mysqlOptions.CommandTimeout(15);
    });

    // Kurangi biaya tracking untuk query baca
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
builder.Services.AddHttpContextAccessor();
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
builder.Services.AddScoped<SSOPortalX.Data.Settings.SystemSettingsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

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

// Initialize default system settings
using (var scope = app.Services.CreateScope())
{
    try
    {
        var systemSettingsService = scope.ServiceProvider.GetRequiredService<SSOPortalX.Data.Settings.SystemSettingsService>();
        await systemSettingsService.InitializeDefaultSettingsAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing system settings: {ex.Message}");
    }
}

app.Run();