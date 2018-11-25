using System.Drawing;

namespace MazeGenerator.Drawing
{
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
