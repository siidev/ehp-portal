
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace SSOPortalX.Data.Security;

public class CaptchaService
{
    private const string CaptchaChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

    public (string code, byte[] image) GenerateCaptchaImage(int width, int height)
    {
        var captchaCode = GenerateRandomCode(6);
        var image = new Bitmap(width, height);
        var graphics = Graphics.FromImage(image);

        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Color.White);

        // Draw distortion lines
        var random = new Random();
        for (var i = 0; i < 10; i++)
        {
            var x1 = random.Next(width);
            var y1 = random.Next(height);
            var x2 = random.Next(width);
            var y2 = random.Next(height);
            graphics.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
        }

        // Draw CAPTCHA code
        var fontSize = height - 10;
        var font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        var brush = new SolidBrush(Color.Black);
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        // Apply transformations for each character
        var charPositions = new float[captchaCode.Length];
        var charWidth = width / (float)captchaCode.Length;

        for (int i = 0; i < captchaCode.Length; i++)
        {
            var gPath = new GraphicsPath();
            var charRect = new RectangleF(i * charWidth, 0, charWidth, height);
            gPath.AddString(captchaCode[i].ToString(), font.FontFamily, (int)font.Style, font.Size, charRect, format);

            // Add some random rotation and translation
            var matrix = new Matrix();
            matrix.Translate(0, random.Next(-5, 5));
            matrix.RotateAt(random.Next(-15, 15), new PointF(charRect.X + charRect.Width / 2, charRect.Y + charRect.Height / 2));
            gPath.Transform(matrix);

            graphics.FillPath(brush, gPath);
        }

        // Add noise
        for (var i = 0; i < 100; i++)
        {
            var x = random.Next(width);
            var y = random.Next(height);
            image.SetPixel(x, y, Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
        }

        graphics.Flush();

        using var ms = new MemoryStream();
        image.Save(ms, ImageFormat.Png);

        return (captchaCode, ms.ToArray());
    }

    private string GenerateRandomCode(int length)
    {
        var random = new Random();
        var result = new StringBuilder(length);
        for (var i = 0; i < length; i++)
        {
            result.Append(CaptchaChars[random.Next(CaptchaChars.Length)]);
        }
        return result.ToString();
    }
}
