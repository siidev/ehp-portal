
using Microsoft.AspNetCore.Components;
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

        protected override async Task OnInitializedAsync()
        {
            await GenerateCaptcha();
        }

        private async Task GenerateCaptcha()
        {
            var (code, image) = CaptchaService.GenerateCaptchaImage(150, 50);
            await CookieStorage.SetAsync("CaptchaCode", code);
            CaptchaImageUrl = $"data:image/png;base64,{Convert.ToBase64String(image)}";
        }

        private async Task HandleLogin()
        {
            _errorMessage = "";
            var storedCode = await CookieStorage.GetAsync("CaptchaCode");

            if (!string.Equals(storedCode, CaptchaInput, StringComparison.OrdinalIgnoreCase))
            {
                _errorMessage = "Invalid CAPTCHA";
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
                    // Clear old tokens untuk user ini (optional - untuk keamanan)
                    await SsoTokenService.ClearUserTokensAsync(user.Id);
                    
                    var appIds = await UserAppAccessService.GetAppIdsForUserAsync(user.Id);
                    foreach (var appId in appIds)
                    {
                        var token = await SsoTokenService.GenerateTokenAsync(user.Id, appId);
                        System.Console.WriteLine($"Generated token for user {user.Username} and app {appId}: {token.Token}");
                    }

                    // Store user session
                    await CookieStorage.SetAsync("CurrentUserId", user.Id.ToString());
                    await CookieStorage.SetAsync("CurrentUsername", user.Username);
                    await CookieStorage.SetAsync("CurrentUserRole", user.Role);
                    
                    Navigation.NavigateTo("/", forceLoad: true);
                }
                else
                {
                    _errorMessage = "An error occurred after login.";
                    await GenerateCaptcha();
                    StateHasChanged();
                }
            }
            else
            {
                _errorMessage = "Invalid username or password.";
                await GenerateCaptcha();
                StateHasChanged();
            }
        }
    }
}
