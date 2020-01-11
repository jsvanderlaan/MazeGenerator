using System;
using System.Drawing;
using Common;
using Common.Enums;
using Entities.Cells;
using Entities.Factories;

namespace Entities.Converters
{
    public static class ImageToGridConverter
    {
        //public static Boolean[,] ConvertThreshold(Bitmap image, int outputHeight, double brightness, bool minimal, Shape shape)
        //{
        //    var imgAttr = new ImageAttributes();
        //    imgAttr.SetColorKey()
        //}


        //public static Boolean[,] ConvertParallel(Bitmap image, int outputHeight, double brightness, bool minimal, Shape shape)
        //{
        //    ICell cell = new CellFactory().CreateCell(shape, -1, -1, false);
        //    double gridHeight = image.Height / (outputHeight * cell.RelativeHeight());
        //    int outputWidth = (int)Math.Round(image.Width / (gridHeight * cell.RelativeWidth()));

        //    bool[,] output = new bool[outputWidth, outputHeight];

        //    unsafe
        //    {






        //        int heightInPixels = bitmapData.Height;
        //        int widthInBytes = bitmapData.Width * bytesPerPixel;
        //        byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

        //        Parallel.For(0, heightInPixels, y =>
        //        {
        //            byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
        //            for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
        //            {
        //                double totalBrightness = 0;
        //                int gridSize = 0;
        //                Parallel.For(0, (int)Math.Ceiling(gridHeight), i =>
        //                    Parallel.For(0, (int)Math.Ceiling(gridHeight), j =>
        //                    {
        //                        int X = (int)Math.Round(x * gridHeight * cell.RelativeWidth() + i);
        //                        int Y = (int)Math.Round(y * gridHeight * cell.RelativeHeight() + j);
        //                        if (X < bitmapData.Width && Y < bitmapData.Height)
        //                        {
        //                            gridSize++;
        //                            //var pix = bitmapData(X, Y);
        //                            //totalBrightness += pix.A == 0 ? 1.0 : pix.GetBrightness();
        //                        }
        //                    })
        //                );
        //                output[x, y] = minimal ? totalBrightness / gridSize < brightness : totalBrightness / gridSize > brightness;
        //            }
        //        }));




        //        image.UnlockBits(bitmapData);
        //    }
        //    return output;
        //}

        public static Boolean[,] Convert(Bitmap bitmap, int outputHeight, double brightness, bool minimal, Shape shape)
        {
            var timer = new TimerLoadingbar("Processing image");
            var input = bitmap;
            ICell cell = new CellFactory().CreateCell(shape, -1, -1, false);

            double gridHeight = input.Height / (outputHeight * cell.RelativeHeight());
            int outputWidth = (int)Math.Round(input.Width / (gridHeight * cell.RelativeWidth()));

            timer.Start(outputWidth * outputHeight);

            bool[,] output = new bool[outputWidth, outputHeight];

            for (int x = 0; x < outputWidth; x++)
            {
                for (int y = 0; y < outputHeight; y++)
                {
                    timer.Next();
                    double totalBrightness = 0;
                    int gridSize = 0;

                    for (int i = 0; i < Math.Ceiling(gridHeight); i++)
                    {
                        for (int j = 0; j < Math.Ceiling(gridHeight); j++)
                        {
                            int X = (int)Math.Round(x * gridHeight * cell.RelativeWidth() + i);
                            int Y = (int)Math.Round(y * gridHeight * cell.RelativeHeight() + j);
                            if (X < input.Width && Y < input.Height)
                            {
                                gridSize++;
                                var pix = input.GetPixel(X, Y);
                                totalBrightness += pix.A == 0 ? 1.0 : pix.GetBrightness();
                            }
                        }
                    }

                    output[x, y] = minimal ? totalBrightness / gridSize < brightness : totalBrightness / gridSize > brightness;
                }
            }

            timer.Stop();

            return output;
        }
    }
}
