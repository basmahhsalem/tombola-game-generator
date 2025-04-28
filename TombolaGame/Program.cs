using System;
using System.IO;
using SkiaSharp;

class Program
{
    static void Main()
    {
        string backgroundPath = "background.jpg"; // Ensure this exists
        string outputFolder = "GeneratedImages";

        // Ensure output directory exists
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        for (int i = 1; i <= 10; i++)
        {
            using (var input = File.OpenRead(backgroundPath))
            using (var bitmap = SKBitmap.Decode(input))
            using (var surface = new SKCanvas(bitmap))
            {
                // Set text properties
                using (var paint = new SKPaint())
                {
                    paint.TextSize = 200; // Adjust size as needed
                    paint.IsAntialias = true;
                    paint.Color = SKColors.White;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
                    paint.TextAlign = SKTextAlign.Center;

                    // Measure text width & height
                    SKRect textBounds = new SKRect();
                    paint.MeasureText(i.ToString(), ref textBounds);

                    float x = bitmap.Width / 2;
                    float y = (bitmap.Height / 2) - textBounds.MidY; // Center vertically

                    // Draw the text on the image
                    surface.DrawText(i.ToString(), x, y, paint);
                }

                // Save the modified image
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
                using (var stream = File.Create(Path.Combine(outputFolder, $"image_{i}.jpg")))
                {
                    data.SaveTo(stream);
                }

                Console.WriteLine($"Generated: image_{i}.jpg");
            }
        }

        Console.WriteLine("All images generated successfully!");
    }
}
