using NotAPaint.CustomElements.DrawingControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace NotAPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrawingControl drawingControl = new DrawingControl();
        public MainWindow()
        {
            InitializeComponent();

            ResizableDrawPanel.Container = drawingControl;
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (drawingControl == null) return;

            ColorPicker colorPicker = (ColorPicker)sender;

            if (colorPicker == null) return;

            if (colorPicker.SelectedColor == null) return;

            drawingControl.SetColor(colorPicker.SelectedColor.Value);
        }
    }
}
