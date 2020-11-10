using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdvanceTDL
{
    /// <summary>
    /// Interaction logic for myGadget.xaml
    /// </summary>
    public partial class myGadget : Window
    {
        
        public myGadget()
        {
            InitializeComponent();

            btnClose.BorderBrush = null;
            Image img = new Image();
            string dir = "file://" + Directory.GetCurrentDirectory() + "\\miss.png";
            img.Source = new BitmapImage(new Uri(dir));
            img.Width = 10;
            img.Height = 10;
            btnClose.Content = img;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
