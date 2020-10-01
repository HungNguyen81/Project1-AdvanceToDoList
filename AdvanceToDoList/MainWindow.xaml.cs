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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int Stt;
        
        public MainWindow()
        {
            InitializeComponent();
            Stt = 1;

            int[] hours12 = new int[12];
            int[] hours24 = new int[24];
            int[] mins = new int[60];
            String[] Am_Pm = { "AM", "PM" };
            for(int i = 0; i < 60; i++)
            {
                mins[i] = i;
                if(i < 12)
                    hours12[i] = i + 1;
                if (i < 24)
                    hours24[i] = i + 1;
            }
            cb_hour12.ItemsSource = hours12;
            cb_hour24.ItemsSource = hours24;
            cb_minute12.ItemsSource = mins;
            cb_minute24.ItemsSource = mins;
            cb_AM_PM.ItemsSource = Am_Pm;

            cb_hour12.SelectedIndex = 8;
            cb_hour24.SelectedIndex = 8;
            cb_minute12.SelectedIndex = 0;
            cb_minute24.SelectedIndex = 0;
            cb_AM_PM.SelectedIndex = 0;
            btn_add.Background = null;
            btn_add.BorderBrush = null;
            datePicker.SelectedDate = new DateTime(2020, 10, 2);
            //pnlSuKien.Children.RemoveAt();
        }
        #region ADD EVENTS
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Button btn = new Button();
            Canvas c = new Canvas();
            c.Height = 90;
            c.Width = 500;
            Stt++;
            btn.Height = 90;
            btn.Background = null;
            btn.Cursor = Cursors.Hand;
            btn.GotFocus += new RoutedEventHandler(onGotFocus_event);
            btn.LostFocus += new RoutedEventHandler(onLossFocus_event);
            btn.Foreground = Brushes.DarkViolet;

            TextBlock txt = new TextBlock();
            txt.Text = input_tenSuKien.Text;
            txt.FontSize = 20;
            txt.FontWeight = FontWeights.Bold;
            txt.Margin = new Thickness(10, 5, 0, 0);

            TextBlock des = new TextBlock();
            des.Width = 300;

            if(txtMota.Text.Length > 43)
            {
                c.Height = btn.Height = 110 + 15 *((int) txtMota.Text.Length / 43);
            }
            des.Text = (txtMota.Text != "")? txtMota.Text:"Sự kiện " + Stt;
            des.TextWrapping = TextWrapping.Wrap;
            des.Margin = new Thickness(10, 40, 0, 0);
            des.FontStyle = FontStyles.Italic;
            des.FontSize = 15;
            des.Foreground = Brushes.DarkBlue;

            DateTime date = new DateTime();

            TextBlock txtDate = new TextBlock();

            // input ngay
            date = (DateTime) datePicker.SelectedDate;
            txtDate.Text = date.DayOfWeek + ", " + date.Day + "/" + date.Month + "/" + date.Year;
            txtDate.FontSize = 18;
            txtDate.Foreground = Brushes.Black;
            Canvas.SetBottom(txtDate, 10);
            Canvas.SetRight(txtDate, 170);

            TextBlock txtTime = new TextBlock();
            txtTime.FontSize = 18;
            if (radio_24.IsChecked == true)
            {
                String h = (cb_hour24.SelectedIndex < 9) ? "0" + (cb_hour24.SelectedIndex+1) : "" + (cb_hour24.SelectedIndex+1);
                String m = (cb_minute24.SelectedIndex <= 9) ? "0" + (cb_minute24.SelectedIndex) : "" + cb_minute24.SelectedIndex;
                txtTime.Text = h + ":" + m;
            } 
            else if (radio_12.IsChecked == true)
            {
                String h = (cb_hour12.SelectedIndex < 9) ? "0" + (cb_hour12.SelectedIndex + 1) : "" + (cb_hour12.SelectedIndex+1);
                String m = (cb_minute12.SelectedIndex <= 9) ? "0" + (cb_minute12.SelectedIndex) : "" + cb_minute12.SelectedIndex;
                if(cb_AM_PM.SelectedIndex == 1)
                {
                    h = (cb_hour12.SelectedIndex < 9) ? "" + (cb_hour12.SelectedIndex + 13) : "" + (cb_hour12.SelectedIndex + 1);
                }
                txtTime.Text = h + ":" + m;
            }
            txtTime.FontWeight = FontWeights.SemiBold;
            txtTime.HorizontalAlignment = HorizontalAlignment.Left;

            Canvas.SetTop(txtTime, 5);
            Canvas.SetRight(txtTime, 170);


            
            c.Children.Add(txt);
            c.Children.Add(des);
            c.Children.Add(txtDate);
            c.Children.Add(txtTime);
            
            btn.Content = c;
   
            
           
            Border b = new Border();
            b.Child = btn;
            b.BorderThickness = new Thickness(0);
            b.Background = Brushes.White;
            b.Opacity = 0.5f;
            b.Margin = new Thickness(5, 5, 5, 0);
            b.CornerRadius = new CornerRadius(15);
            b.BorderBrush = null;
            pnlSuKien.Children.Add(b);

            scroll_sukien.ScrollToEnd();
        }
#endregion
        private void onGotFocus_event(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Opacity = 0.7f;
        }
        private void onLossFocus_event(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Opacity = 1;
        }

        private void input_tenSuKien_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            input_tenSuKien.Text = "";
        }

        private void Radio_24_Click(object sender, RoutedEventArgs e)
        {
            grid_24.Visibility = Visibility.Visible;
            grid_12.Visibility = Visibility.Collapsed;
        }
        private void Radio_12_Click(object sender, RoutedEventArgs e)
        {
            grid_12.Visibility = Visibility.Visible;
            grid_24.Visibility = Visibility.Collapsed;
        }

        private void btn_add_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.BorderBrush = null;
        }

        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(sender.ToString());
        }

        private void txtMota_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtMota.Text = "";
        }
    }
}
