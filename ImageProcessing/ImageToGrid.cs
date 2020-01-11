using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace ImageProcessing
{
    public class ImageToGrid
    {
        public Boolean[,] Convert(Stream stream)
        {
            var ms = new MemoryStream();
            using (Image<Rgba32> img = Image.Load<Rgba32>(stream))
            {
                img.Mutate(image =>
                    image
                    .Grayscale()
                    .Resize(100, 50)
                    .BinaryThreshold((float)0.7));
                img.SaveAsPng(ms);
            }
            //ms.

            return null;
        }
    }
}
