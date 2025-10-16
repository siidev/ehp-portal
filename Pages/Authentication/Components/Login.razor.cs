
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SSOPortalX.Data.App.User;
using SSOPortalX.Data.App.UserAppAccess;
using SSOPortalX.Data.Security;
using SSOPortalX.Data.Sso;
using SSOPortalX.Global;
using System.Threading.Tasks;

namespace SSOPortalX.Pages.Authentication.Components
{
    public partial class Login
    {
        private bool _show;
        private bool _isLoading;
        public string _email { get; set; } = "";
        public string _password { get; set; } = "";
        public bool _remember { get; set; }
        private string _errorMessage = "";

        [Inject]
        private CaptchaService CaptchaService { get; set; } = default!;

        [Inject]
        private CookieStorage CookieStorage { get; set; } = default!;

        [Inject]
        private UserService UserService { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

        [Inject]
        private SsoTokenService SsoTokenService { get; set; } = default!;

        [Inject]
        private UserAppAccessService UserAppAccessService { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Parameter]
        public bool HideLogo { get; set; }

        [Parameter]
        public double Width { get; set; } = 410;

        [Parameter]
        public StringNumber? Elevation { get; set; }

        [Parameter]
        public string CreateAccountRoute { get; set; } = "pages/authentication/register-v1";

        [Parameter]
        public string ForgotPasswordRoute { get; set; } = "/authentication/forgot-password";

        public string CaptchaInput { get; set; } = "";
        public string? CaptchaImageUrl { get; set; }
        private string? _currentCaptchaCode;

    private bool _captchaInitialized = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !_captchaInitialized)
        {
            _captchaInitialized = true;
            await GenerateCaptcha();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }        private async Task GenerateCaptcha()
        {
            try
            {
                // Always generate fresh captcha
                var (code, image) = CaptchaService.GenerateCaptchaImage(150, 65);
                
                // Store in component memory (primary)
                _currentCaptchaCode = code;
                
                // Also try to store in cookie for backup
                try
                {
                    await CookieStorage.SetAsync("CaptchaCode", code);
                }
                catch (Exception cookieEx)
                {
                    Console.WriteLine($"Warning: Could not store captcha in cookie: {cookieEx.Message}");
                }
                
                // Set image
                CaptchaImageUrl = $"data:image/svg+xml;base64,{Convert.ToBase64String(image)}";
                
                // Debug log
                Console.WriteLine($"Generated CAPTCHA: {code} (stored in component memory)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating captcha: {ex.Message}");
            }
        }

        private async Task HandleLogin()
        {
            _isLoading = true;
            _errorMessage = "";
            StateHasChanged();
            
            try
            {
                // Use component memory first, fallback to cookie
                var storedCode = _currentCaptchaCode;
                if (string.IsNullOrEmpty(storedCode))
                {
                    storedCode = await CookieStorage.GetAsync("CaptchaCode");
                }
                
                // Debug log to check captcha validation
                Console.WriteLine($"Component CAPTCHA: '{_currentCaptchaCode}', Cookie CAPTCHA: '{await CookieStorage.GetAsync("CaptchaCode")}', Input CAPTCHA: '{CaptchaInput}'");

                if (!string.Equals(storedCode, CaptchaInput, StringComparison.OrdinalIgnoreCase))
                {
                    _errorMessage = "Invalid CAPTCHA";
                    CaptchaInput = ""; // Clear input
                    await GenerateCaptcha();
                    StateHasChanged();
                    return;
                }

                var isAuthenticated = await UserService.ValidatePasswordAsync(_email, _password);

            if (isAuthenticated)
            {
                var user = await UserService.GetUserByUsernameAsync(_email);
                if (user != null)
                {
                    // Store user session immediately for faster login
                    await CookieStorage.SetAsync("CurrentUserId", user.Id.ToString());
                    await CookieStorage.SetAsync("CurrentUsername", user.Username);
                    await CookieStorage.SetAsync("CurrentUserRole", user.Role);
                    
                    // Notify authentication state change
                    if (AuthStateProvider is CustomAuthenticationStateProvider customProvider)
                    {
                        customProvider.NotifyUserAuthentication(user.Id.ToString(), user.Username, user.Role);
                    }
                    
                    // Clear captcha after successful login
                    _currentCaptchaCode = null;
                    CaptchaInput = "";
                    await CookieStorage.RemoveAsync("CaptchaCode");
                    
                    // Navigate first, then do background token generation
                    Navigation.NavigateTo("/", forceLoad: true);
                    
                    // Background token processing (non-blocking) dengan pembatasan paralelisme
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await SsoTokenService.ClearUserTokensAsync(user.Id);

                            var appIds = await UserAppAccessService.GetAppIdsForUserAsync(user.Id);

                            var degreeOfParallelism = Math.Max(1, Math.Min(2, appIds.Count));
                            using var semaphore = new SemaphoreSlim(degreeOfParallelism);

                            var tokenTasks = appIds.Select(async appId =>
                            {
                                await semaphore.WaitAsync();
                                try
                                {
                                    var token = await SsoTokenService.GenerateTokenAsync(user.Id, appId);
                                    System.Console.WriteLine($"Generated token for user {user.Username} and app {appId}: {token.Token}");
                                }
                                finally
                                {
                                    semaphore.Release();
                                }
                            }).ToList();

                            await Task.WhenAll(tokenTasks);
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine($"Background token generation error: {ex.Message}");
                        }
                    });
                }
                else
                {
                    _errorMessage = "An error occurred after login.";
                    await GenerateCaptcha();
                }
            }
            else
            {
                _errorMessage = "Invalid username or password.";
                await GenerateCaptcha();
            }
            }
            catch (Exception ex)
            {
                _errorMessage = $"Login error: {ex.Message}";
                await GenerateCaptcha();
                StateHasChanged();
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }
    }
}
