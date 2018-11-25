using System;
using System.Drawing;
using MazeGenerator.Cells;
using MazeGenerator.Common;

namespace MazeGenerator.ImageConverter
{
    public static class ImageToGridConverter<C> where C : Cell, new()
    {
        public static Boolean[,] Convert(string imagePath, int outputHeight, double minimalBrightness)
        {
            var timer = new Timer("Processing image");
            var input = new Bitmap(imagePath);

            C cell = new C();

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
                                totalBrightness += input.GetPixel(X, Y).GetBrightness();
                            }
                        }
                    }

                    output[x, y] = totalBrightness / gridSize < minimalBrightness;
                }
            }

            timer.Stop();

            return output;
        }
    }
}
