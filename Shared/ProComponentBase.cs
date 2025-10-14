namespace SSOPortalX;

public abstract class ProComponentBase : ComponentBase
{
    [Inject]
    protected I18n I18n { get; set; } = null!;

    [CascadingParameter(Name = "CultureName")]
    protected string? Culture { get; set; }

    protected string T(string? key, params object[] args)
    {
        return I18n.T(key, args: args);
    }

    /// <summary>
    /// Determines if the given icon URL is a Material Design icon name or a URL to an image
    /// </summary>
    protected static bool IsIconName(string? iconUrl)
    {
        if (string.IsNullOrEmpty(iconUrl))
            return true;
        
        // Check if it's a Material Design icon or Font Awesome icon
        return iconUrl.StartsWith("mdi-") || iconUrl.StartsWith("fa") || 
               iconUrl.StartsWith("fas ") || iconUrl.StartsWith("far ") || 
               iconUrl.StartsWith("fab ") || iconUrl.StartsWith("fal ");
    }

    /// <summary>
    /// Gets the appropriate fallback icon for an application
    /// </summary>
    protected static string GetFallbackIcon(string? iconUrl)
    {
        if (string.IsNullOrEmpty(iconUrl))
            return "mdi-application";
        
        return IsIconName(iconUrl) ? iconUrl : "mdi-application";
    }
}
