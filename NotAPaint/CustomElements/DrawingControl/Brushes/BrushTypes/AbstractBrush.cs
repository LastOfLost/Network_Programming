using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NotAPaint.CustomElements.DrawingControl.Brushes.BrushTypes
{
    public abstract class AbstractBrush
    {
        protected DataPixelEditor pixelEditor;
        protected double width;
        protected double height;
        protected double angle;
        protected Color color;

        public double Width
        {
            get => width;
            set => width = value;
        }
        public double Height
        {
            get => height;
            set => height = value;
        }
        public Color Color
        {
            get => color;
            set {
                pixelEditor.RecolorByAlfa(value);
                color = value;
            }
        }
        public AbstractBrush(double width, double height, double angle, Color color, PixelFormat pixelFormat)
        {
            this.width = width;
            this.height = height;
            this.angle = angle;
            this.color = color;
            pixelEditor = new DataPixelEditor((int)width, (int)height, pixelFormat, null);
        }
        public abstract BitmapSource GetDrawImage();
        
    }
}
