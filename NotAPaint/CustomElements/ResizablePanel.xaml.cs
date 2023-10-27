
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotAPaint.CustomElements
{
    /// <summary>
    /// Логика взаимодействия для ResizablePanel.xaml
    /// </summary>
    public partial class ResizablePanel : UserControl , INotifyPropertyChanged
    {
        private bool CanResize
        {
            get => canResize;
            set
            {
                canResize = value;
                OnPropertyChanged(nameof(CanResize));
            }
        }
        private bool canResize ;
        private double BorderWidth
        {
            get => borderWidth;
            set
            {
                borderWidth = value;
                OnPropertyChanged(nameof(BorderWidth));
            }
        }
        private double borderWidth;
        private double BorderHeight
        {
            get => borderHeight;
            set
            {
                borderHeight = value;
                OnPropertyChanged(nameof(BorderHeight));
            }
        }
        private double borderHeight;


        public static readonly DependencyProperty ContainerProperty =
            DependencyProperty.Register("Container", typeof(UIElement), typeof(ResizablePanel));
        public static readonly DependencyProperty ContainerWidthProperty =
            DependencyProperty.Register("ContainerWidth", typeof(double), typeof(ResizablePanel));
        public static readonly DependencyProperty ContainerHeightProperty =
            DependencyProperty.Register("ContainerHeight", typeof(double), typeof(ResizablePanel));
        public UIElement Container
        {
            get { return (UIElement)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }

        public double ContainerWidth
        {
            get { return (double)GetValue(ContainerWidthProperty); }
            set { SetValue(ContainerWidthProperty, value); }
        }

        public double ContainerHeight
        {
            get { return (double)GetValue(ContainerHeightProperty); }
            set { SetValue(ContainerHeightProperty, value); }
        }


        public ResizablePanel()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanResize = true;
        }
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CanResize)
            {
                CanResize = false;

                ViewGrid.Width = ViewBorder.Width;
                ViewGrid.Height = ViewBorder.Height;
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (CanResize)
            {
                Point positionInWindow = e.GetPosition(ViewGrid);

                ViewBorder.Width = positionInWindow.X < 0 ? 0 : positionInWindow.X;
                ViewBorder.Height = positionInWindow.Y < 0 ? 0 : positionInWindow.Y;
            }
        }

        public void SetResizableArea(double width, double height)
        {
            ViewGrid.Width = width;
            ViewGrid.Height = height;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CanResize = true;

        }

    }
}
