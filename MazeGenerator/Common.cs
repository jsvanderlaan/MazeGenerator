using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeGenerator
{
    [Flags]
    public enum CellState
    {
        Top = 1,
        TopRight = 2,
        Right = 4,
        BottomRight = 8,
        Bottom = 16,
        Left = 32,
        Active = 64,
        Visited = 128,
        Initial = Top | Right | Bottom | Left | TopRight | BottomRight,
    }

    public struct AdjacentCell
    {
        public Point Position;
        public CellState Wall;
    }

    public struct Cell
    {
        public CellState State;
        public Point Predecessor;
        public Point Position;
        public ICollection<Point> Successors;
        public int Distance;
    }

    public struct ColorRGB
    {
        public byte R;
        public byte G;
        public byte B;

        public ColorRGB(Color value)
        {
            R = value.R;
            G = value.G;
            B = value.B;
        }

        public static implicit operator Color(ColorRGB rgb) => Color.FromArgb(rgb.R, rgb.G, rgb.B);
        public static explicit operator ColorRGB(Color c) => new ColorRGB(c);
    }
}
