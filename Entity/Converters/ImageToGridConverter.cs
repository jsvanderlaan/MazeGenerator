﻿using System;
using System.Drawing;
using System.IO;
using Common;
using Entities.Cells;
using Entities.Factories;

namespace Entities.Converters
{
    public static class ImageToGridConverter
    {
        public static Boolean[,] Convert(MemoryStream ms, int outputHeight, double brightness, bool minimal, Shape shape)
        {
            var timer = new Timer("Processing image");
            using (var input = new Bitmap(ms))
            {
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
}
