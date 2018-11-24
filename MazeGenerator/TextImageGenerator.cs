using System;
using System.Drawing;

namespace MazeGenerator
{
    public class TextImageGenerator
    {
        public string GenerateTextImage(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public void DrawSpacedText(Graphics g, Font font, Brush brush, PointF point, string text, float spacing)
        {
            //Calculate spacing
            //float widthNeeded = 0;
            //foreach (char c in text)
            //{
            //    widthNeeded += g.MeasureString(c.ToString(), font).Width;
            //}
            //float spacing = (desiredWidth - widthNeeded) / (text.Length - 1);

            //draw text
            float indent = 0;
            foreach (char c in text)
            {
                g.DrawString(c.ToString(), font, brush, new PointF(point.X + indent, point.Y));
                indent += g.MeasureString(c.ToString(), font).Width + spacing;
            }
        }

        public Font GetAdjustedFont(Graphics graphicRef, string graphicString, Font originalFont, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                SizeF adjustedSizeNew = graphicRef.MeasureString(graphicString, testFont);

                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return minimumSize or original?
            if (smallestOnFail)
            {
                return testFont;
            }
            else
            {
                return originalFont;
            }
        }

    }
}
