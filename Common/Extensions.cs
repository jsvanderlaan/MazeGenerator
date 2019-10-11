using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Common
{
    public static class Extensions
    {
        public static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            var e = source.ToArray();
            for (var i = e.Length - 1; i >= 0; i--)
            {
                var swapIndex = rng.Next(i + 1);
                yield return e[swapIndex];
                e[swapIndex] = e[i];
            }
        }

        public static T Random<T>(this IEnumerable<T> source, Random rng) => source.ElementAt(rng.Next(source.Count()));

        public static double Distance(this Point a, Point b)
        {
            var vector = a.GetVector(b);
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }
        public static PointF GetVector(this Point a, Point b, double distance)
        {
            var vector = a.GetVector(b);
            var length = a.Distance(b);
            return new PointF((float)(vector.X / length * distance), (float)(vector.Y / length * distance));
        }

        public static Point GetVector(this Point a, Point b) => new Point(b.X - a.X, b.Y - a.Y);
        public static Point Move(this Point a, PointF vec) => new Point((int)(a.X + vec.X), (int)(a.Y + vec.Y));

        public static Point GetPoint(double X, double Y) => new Point((int)(X), (int)(Y)); 
        public static Point Point(this Position pos) => new Point(pos.X, pos.Y);

        public static Point TopLeft(this Rectangle rect) => new Point(rect.Left, rect.Top);
        public static Point TopRight(this Rectangle rect) => new Point(rect.Right, rect.Top);
        public static Point BottomLeft(this Rectangle rect) => new Point(rect.Left, rect.Bottom);
        public static Point BottomRight(this Rectangle rect) => new Point(rect.Right, rect.Bottom);

        public static Color GetColorInRange(double num, double max) => HSL2RGB(num / max == 1.0 ? 0.999 : num / max, 0.5, 0.5);

        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            ColorRGB rgb;
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }
    }
}
