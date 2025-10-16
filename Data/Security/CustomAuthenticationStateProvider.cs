using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SSOPortalX.Data.Security;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly CookieStorage _cookieStorage;

    public CustomAuthenticationStateProvider(CookieStorage cookieStorage)
    {
        _cookieStorage = cookieStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Check if we're in a prerendering context
            var currentUserId = await _cookieStorage.GetAsync("CurrentUserId");
            var currentUsername = await _cookieStorage.GetAsync("CurrentUsername");
            var currentUserRole = await _cookieStorage.GetAsync("CurrentUserRole");

            if (!string.IsNullOrEmpty(currentUserId) && 
                !string.IsNullOrEmpty(currentUsername) && 
                !string.IsNullOrEmpty(currentUserRole))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, currentUserId),
                    new Claim(ClaimTypes.Name, currentUsername),
                    new Claim(ClaimTypes.Role, currentUserRole)
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("JavaScript interop"))
        {
            // During prerendering, JavaScript interop is not available
            // Return anonymous user and let client-side handle authentication
            Console.WriteLine("Prerendering detected, returning anonymous user");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting authentication state: {ex.Message}");
        }

        // Return anonymous user if no valid session or during prerendering
        var anonymousIdentity = new ClaimsIdentity();
        var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
        return new AuthenticationState(anonymousUser);
    }

    public void NotifyUserAuthentication(string userId, string username, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        var anonymousIdentity = new ClaimsIdentity();
        var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }
}