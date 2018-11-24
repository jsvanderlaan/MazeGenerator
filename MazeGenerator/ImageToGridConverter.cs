using System;
using System.Drawing;

namespace MazeGenerator
{
    public static class ImageToGridConverter
    {
        private static bool[,] output;

        public static Boolean[,] Convert(string imagePath, int outputHeight, double minimalBrightness)
        {
            var timer = new Timer("Processing image");
            var input = new Bitmap(imagePath);

            double gridHeight = input.Height / (double)outputHeight;
            int outputWidth = (int)Math.Round(input.Width / gridHeight);
            output = new bool[outputWidth, outputHeight];

            timer.Start(outputWidth * outputHeight);

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
                            int X = (int)Math.Round(x * gridHeight + i);
                            int Y = (int)Math.Round(y * gridHeight + j);
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

        public static void Display() {
            if(output?.Length > 0)
            {
                for (int i = 0; i < output.GetLength(1); i++)
                {
                    for (int j = 0; j < output.GetLength(0); j++)
                    {
                        Console.Write(output[j, i] ? "x" : " ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
