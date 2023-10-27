using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NotAPaint.CustomElements.DrawingControl.Brushes.BrushTypes
{
    public class PenBrush : AbstractBrush
    {
        public PenBrush(double width, double height, double angle, Color color, PixelFormat pixelFormat) : 
            base(width, height, angle, color, pixelFormat)
        {

        }

        private bool isFirstLoading = true;
        private void RasterizeEllipse()
        {
            int a = (int)width / 2;
            int b = (int)height / 2;
            int a2 = a * a;
            int b2 = b * b;
            int twoA2 = 2 * a2;
            int twoB2 = 2 * b2;

            RasterizeEllipseSegment(new Point(width / 2, height / 2), a, b, a2, b2, twoA2, twoB2);
            RasterizeEllipseSegment(new Point(width / 2, height / 2), b, a, b2, a2, twoB2, twoA2);
        }
        private void RasterizeEllipseSegment(Point center, int a, int b, int a2, int b2, int twoA2, int twoB2)
        {
            int x = 0;
            int y = b;
            int xChange = b2 * (1 - 2 * a) + a2;
            int yChange = a2 * (1 - 2 * b) + b2;
            int error = b * a2 - a * b2;

            while (x * b2 <= y * a2)
            {

                pixelEditor.SetPixel(color, (int)center.X + x, (int)center.Y + y);
                pixelEditor.SetPixel(color, (int)center.X - x, (int)center.Y + y);
                pixelEditor.SetPixel(color, (int)center.X + x, (int)center.Y - y);
                pixelEditor.SetPixel(color, (int)center.X - x, (int)center.Y - y);
                x++;

                if (error < 0)
                {
                    error += xChange;
                    xChange += twoB2;
                }
                else
                {
                    y--;
                    xChange += twoB2;
                    error += xChange - yChange;
                    yChange += twoA2;
                }
            }
        }
        public override BitmapSource GetDrawImage()
        {
            if(isFirstLoading) RasterizeEllipse();

            isFirstLoading = false;
            return pixelEditor.GetBitmap();
        }
    }
}
