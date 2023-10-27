using NotAPaint.CustomElements.DrawingControl.Brushes.BrushTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotAPaint.CustomElements.DrawingControl
{
    /// <summary>
    /// Логика взаимодействия для DrawingControl.xaml
    /// </summary>
    public partial class DrawingControl : UserControl, INotifyPropertyChanged
    {
        private DrawingControler graphic = new DrawingControler(400, 200);
        private bool isDrawing;
        private int brushSize = 5;
        Random random = new Random();

        private AbstractBrush brush = new PenBrush(5,5,0,Colors.Black,PixelFormats.Bgr32);

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ResizablePanel));
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set {
                brush.Color = value;
                SetValue(SelectedColorProperty, value); 
            }
        }
        public DrawingControl()
        {
            InitializeComponent();

            DataContext = graphic;
            graphic.SetBackground(Colors.White);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                isDrawing = true;
            }
        }
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                //graphic.DrawEllipse(SelectedColor, Mouse.GetPosition(ViewImage), brushSize, brushSize);
                graphic.DrawBrush(brush, Mouse.GetPosition(ViewImage));
            }
        }
        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                graphic.StilusUp();
            }
        }

        public void SetColor(Color color)
        {
            this.SelectedColor = color;
        }
        public void SetBrushSize(int size)
        {
            if (size < 0) size = 0;
            else if (size > 300) size = 300;

            brushSize = size;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            graphic.ResizeBitmap((int)this.ActualWidth, (int)this.ActualHeight);
        }

        private void MouseLeave(object sender, MouseEventArgs e)
        {
            graphic.StilusUp();
        }
    }
}
