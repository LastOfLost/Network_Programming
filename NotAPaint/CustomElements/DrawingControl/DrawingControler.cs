using NotAPaint.CustomElements.DrawingControl.Brushes.BrushTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace NotAPaint.CustomElements.DrawingControl
{
    public class DrawingControler : INotifyPropertyChanged
    {
        private DataPixelEditor pixelEditor;
        public BitmapSource ImageSource => imageSource;
        private BitmapSource imageSource;

        private static System.Windows.Media.PixelFormat pixelFormat = PixelFormats.Bgra32;

        private int bitmapWidth;
        private int bitmapHeight;

        private Point? thisDrawPoint = null;
        private Point? prevDrawPoint = null;
        public DrawingControler(int width, int height)
        {
            bitmapWidth = width;
            bitmapHeight = height;

            pixelEditor = new DataPixelEditor(width, height, pixelFormat, Colors.White);
            //DrawRectangle(Color.FromScRgb(255, 255, 255, 255), new Point(0, 0), new Point());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void DrawRectangle(Color color, Point startPoint, Point endPoint)
        {

            for (int y = (int)startPoint.Y; y < (int)endPoint.Y && y < bitmapHeight; y++)
            {
                for (int x = (int)startPoint.X; x < (int)endPoint.X && x < bitmapWidth; x++)
                {
                    pixelEditor.SetPixel(color, x, y);
                }
            }

            WritePixels();
        }
        public void DrawEllipse(Color color, Point startPoint, int width, int height)
        {

            for (int y = (int)startPoint.Y; y < startPoint.Y + height && y < this.bitmapHeight; y++)
            {
                for (int x = (int)startPoint.X; x < startPoint.X + width && x < this.bitmapWidth; x++)
                {
                    pixelEditor.SetPixel(color, x, y);
                }
            }

            WritePixels();
        }
        public void DrawBrushOne(AbstractBrush brush, Point startPoint)
        {
            pixelEditor.SetBitmap(brush.GetDrawImage(), startPoint);
        }
        public void DrawBrush(AbstractBrush brush, Point startPoint)
        {
            DrawBrushOne(brush, startPoint);

            prevDrawPoint = thisDrawPoint;
            thisDrawPoint = startPoint;

            if (prevDrawPoint != null) DrawBrushLine(brush, thisDrawPoint.Value, prevDrawPoint.Value);

            WritePixels();
        }
        public void StilusUp()
        {
            prevDrawPoint = null;
            thisDrawPoint = null;
        }
        public void SetBackground(Color color)
        {
            pixelEditor.SetBackground(color);
        }
        public void ResizeBitmap(int newWidth, int newHeight)
        {
            bitmapWidth = newWidth < 1 ? 1 : newWidth;
            bitmapHeight = newHeight < 1 ? 1 : newHeight;

            pixelEditor.ResizeBitmap(newWidth, newHeight);
            WritePixels();
        }
        private void WritePixels()
        {
            imageSource = pixelEditor.GetBitmap();

            OnPropertyChanged(nameof(ImageSource));
        }
        public void DrawBrushLine(AbstractBrush brush, Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint) return;

            int x1 = (int)startPoint.X;
            int y1 = (int)startPoint.Y;
            int x2 = (int)endPoint.X;
            int y2 = (int)endPoint.Y;

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                DrawBrushOne(brush, new Point(x1, y1));

                if (x1 == x2 && y1 == y2) break;

                int err2 = 2 * err;

                if (err2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }

                if (err2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }

                if (x1 < 0 || x1 > bitmapWidth) break;
                else if (y1 < 0 || y1 > bitmapHeight) break;
            }
        }
    }
}
