using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NotAPaint.CustomElements.DrawingControl
{
    public class DataPixelEditor
    {
        private const int byteInPixel = 4;

        private System.Windows.Media.PixelFormat pixelFormat;
        private Color? standartBackgroungColor;
        private int width;
        private int height;

        private const int dpiX = 96;
        private const int dpiY = 96;
        public byte[] PixelData
        {
            get => pixelData;
            private set => pixelData = value;
        }
        private byte[] pixelData;

        public DataPixelEditor(int width, int height, PixelFormat pixelFormat, Color? standartBackgroungColor)
        {
            this.width = width;
            this.height = height;
            this.pixelFormat = pixelFormat;
            this.standartBackgroungColor = standartBackgroungColor;

            ResizePixelData();
        }

        public void ResizePixelData()
        {
            Array.Resize(ref pixelData, (int)GetStride() * height);
        }
        public void SetBitmap(BitmapSource bitmap, Point position)
        {
            Color[,] matrix = GetPixelMatrix(GetBytes(bitmap), new Size(bitmap.Width, bitmap.Height));
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    SetPixel(matrix[y, x], (int)position.X + x, (int)position.Y + y);
                }
            }
        }
        public void SetPixel(Color color, int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            if (color.A == 0) return;

            int index = y * (int)GetStride() + byteInPixel * x;

            //if(color.A > 0 && color.A < 255)
            //{
            //    pixelData[index] = (byte)((int)pixelData[index] + color.B * color.A / 255.0);
            //    pixelData[index + 1] = (byte)(int)(color.B * color.A / 255.0);
            //    pixelData[index + 2] = (byte)(int)(color.B * color.A / 255.0);
            //    pixelData[index + 3] = color.A;
            //}

            pixelData[index] = color.B;
            pixelData[index + 1] = color.G;
            pixelData[index + 2] = color.R;
            pixelData[index + 3] = color.A;
        }
        public Color GetPixel(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return standartBackgroungColor != null ? standartBackgroungColor.Value : Colors.Transparent;
            }

            int index = y * (int)GetStride() + byteInPixel * x;
            return Color.FromScRgb(pixelData[index + 3], pixelData[index + 2], pixelData[index + 1], pixelData[index]);
        }
        public Color[,] GetPixelMatrix()
        {
            Color[,] matrix = new Color[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    matrix[y, x] = GetPixel(x, y);
                }
            }

            return matrix;
        }
        public void RecolorByAlfa(Color color)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    PixelRecolorByAlfa(color, x, y);
                }
            }
        }
        public void PixelRecolorByAlfa(Color color, int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            if (color.A == 0) return;

            int index = y * (int)GetStride() + byteInPixel * x;

            if ((int)pixelData[index + 3] == 0) return;

            pixelData[index] = color.B;
            pixelData[index + 1] = color.G;
            pixelData[index + 2] = color.R;
        }


        public Color GetPixel(int x, int y, byte[] pixelbytes, Size bitmapSize)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return standartBackgroungColor != null ? standartBackgroungColor.Value : Colors.Transparent;
            }

            int thisStride = (int)(bitmapSize.Width * pixelFormat.BitsPerPixel + 7) / 8;
            int index = y * thisStride + byteInPixel * x;
            return Color.FromScRgb(pixelbytes[index + 3], pixelbytes[index + 2], pixelbytes[index + 1], pixelbytes[index]);
        }
        public Color[,] GetPixelMatrix(byte[] pixelbytes, Size bitmapSize)
        {
            Color[,] matrix = new Color[(int)bitmapSize.Height, (int)bitmapSize.Width];

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    matrix[y, x] = GetPixel(x, y, pixelbytes, bitmapSize);
                }
            }

            return matrix;
        }


        public void ResizeBitmap(int newWidth, int newHeight)
        {

            Color[,] matrix = GetPixelMatrix();

            int oldBitmapWidth = width;
            int oldBitmapHeight = height;

            width = newWidth < 1 ? 1 : newWidth;
            height = newHeight < 1 ? 1 : newHeight;

            ResizePixelData();

            if (standartBackgroungColor != null)
            {
                SetBackground(standartBackgroungColor.Value);
            }

            for (int y = 0; y < oldBitmapHeight && y < height; y++)
            {
                for (int x = 0; x < oldBitmapWidth && x < width; x++)
                {
                    SetPixel(matrix[y, x], x, y);
                }
            }
        }
        public BitmapSource GetBitmap()
        {

            WriteableBitmap writableBitmap = new WriteableBitmap(width, height, dpiX, dpiY, pixelFormat, null);
            writableBitmap.WritePixels(new Int32Rect(0, 0, width, height),pixelData, (int)GetStride(), 0);

            return writableBitmap;
        }
        public byte[] GetBytes(BitmapSource bitmap)
        {
            double thisStride = (bitmap.Width * pixelFormat.BitsPerPixel + 7) / 8;
            byte[] newDataPixels = new byte[(int)(GetStride() * bitmap.Height)];

            bitmap.CopyPixels(newDataPixels, (int)thisStride, 0);

            return newDataPixels;
        }
        public void SetBackground(Color color)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SetPixel(color, x, y);
                }
            }
        }
        public double GetStride() => (width * pixelFormat.BitsPerPixel + 7) / 8;
    }
}
