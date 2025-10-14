
namespace SSOPortalX.Data.Security;

public class CaptchaService
{
    private const string CaptchaChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    private readonly Random _random = new Random();

    public (string code, byte[] image) GenerateCaptchaImage(int width, int height)
    {
        var captchaCode = GenerateRandomCode(6);
        
        // Generate SVG-based CAPTCHA for Linux compatibility
        var svg = GenerateSvgCaptcha(captchaCode, width, height);
        var svgBytes = System.Text.Encoding.UTF8.GetBytes(svg);
        
        return (captchaCode, svgBytes);
    }

    private string GenerateRandomCode(int length)
    {
        var result = new System.Text.StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            result.Append(CaptchaChars[_random.Next(CaptchaChars.Length)]);
        }
        return result.ToString();
    }

    private string GenerateSvgCaptcha(string code, int width, int height)
    {
        var colors = new[] { "#FF0000", "#00AA00", "#0000FF", "#FF00FF", "#FFAA00", "#00AAFF" };
        var svg = new System.Text.StringBuilder();
        
        svg.AppendLine($"<svg width=\"{width}\" height=\"{height}\" xmlns=\"http://www.w3.org/2000/svg\">");
        svg.AppendLine($"<rect width=\"{width}\" height=\"{height}\" fill=\"#f8f8f8\"/>");
        
        // Add noise lines
        for (int i = 0; i < 8; i++)
        {
            var x1 = _random.Next(width);
            var y1 = _random.Next(height);
            var x2 = _random.Next(width);
            var y2 = _random.Next(height);
            var color = colors[_random.Next(colors.Length)];
            svg.AppendLine($"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" stroke=\"{color}\" stroke-width=\"1\" opacity=\"0.3\"/>");
        }
        
        // Add characters with random positions and colors
        var fontSize = height * 0.7; // Increase font size
        var charWidth = width / (code.Length + 1);
        
        for (int i = 0; i < code.Length; i++)
        {
            var x = charWidth * (i + 1) + _random.Next(-3, 3);
            var y = height / 2 + fontSize / 3 + _random.Next(-8, 8);
            var rotation = _random.Next(-12, 12);
            var color = colors[_random.Next(colors.Length)];
            
            svg.AppendLine($"<text x=\"{x}\" y=\"{y}\" font-family=\"Arial, sans-serif\" font-size=\"{fontSize}\" " +
                          $"font-weight=\"bold\" fill=\"{color}\" text-anchor=\"middle\" " +
                          $"transform=\"rotate({rotation} {x} {y})\">{code[i]}</text>");
        }
        
        // Add noise dots
        for (int i = 0; i < 50; i++)
        {
            var x = _random.Next(width);
            var y = _random.Next(height);
            var color = colors[_random.Next(colors.Length)];
            svg.AppendLine($"<circle cx=\"{x}\" cy=\"{y}\" r=\"1\" fill=\"{color}\" opacity=\"0.5\"/>");
        }
        
        svg.AppendLine("</svg>");
        
        return svg.ToString();
    }
}
