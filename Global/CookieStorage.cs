using Microsoft.JSInterop;

namespace SSOPortalX.Global;

public class CookieStorage
{
    private readonly IJSRuntime jsRuntime;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CookieStorage(IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor)
    {
        this.jsRuntime = jsRuntime;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> GetAsync(string key)
    {
        try
        {
            // Try JavaScript interop first
            var jsValue = await jsRuntime.InvokeAsync<string?>("getCookie", key);
            Console.WriteLine($"Cookie retrieved via JavaScript: {key} = '{jsValue}'");
            return jsValue;
        }
        catch (InvalidOperationException)
        {
            // Fallback to server-side during prerendering
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Request.Cookies.ContainsKey(key))
            {
                var httpValue = httpContext.Request.Cookies[key];
                Console.WriteLine($"Cookie retrieved via HttpContext: {key} = '{httpValue}'");
                return httpValue;
            }
            Console.WriteLine($"Cookie not found in HttpContext: {key}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting cookie: {ex.Message}");
            return null;
        }
    }

    public async Task SetAsync(string key, string value, int hours = 8)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("setCookie", key, value, hours);
            Console.WriteLine($"Cookie set via JavaScript: {key} = {value}");
        }
        catch (InvalidOperationException)
        {
            // Fallback to server-side during prerendering
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddHours(hours),
                    HttpOnly = false, // Allow JavaScript access
                    Secure = httpContext.Request.IsHttps,
                    SameSite = SameSiteMode.Lax
                };
                httpContext.Response.Cookies.Append(key, value, cookieOptions);
                Console.WriteLine($"Cookie set via HttpContext: {key} = {value}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting cookie: {ex.Message}");
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("removeCookie", key);
            Console.WriteLine($"Cookie removed via JavaScript: {key}");
        }
        catch (InvalidOperationException)
        {
            // Fallback to server-side during prerendering
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Delete(key);
                Console.WriteLine($"Cookie removed via HttpContext: {key}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing cookie: {ex.Message}");
        }
    }
}