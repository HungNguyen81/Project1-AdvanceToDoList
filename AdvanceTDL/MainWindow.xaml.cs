using AdvanceTDL;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AdvanceTDL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int Stt;
        private StringBuilder csv;
        public static Button btn_Current_Focus;
        private MediaPlayer player;
        string startupPath;
        DispatcherTimer dispatcherTimer;

        DateTime testTime = new DateTime(2020, 10, 5, 9, 13, 0);
        private static bool a = true;
        public const int NUM_ATTR_DATA = 7;  //1. Name  2. Des  3. Date  4.Time  5. id  6. isPast  7. isRemind

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if(DateTime.Compare(now.Date, testTime.Date) == 0 && now.Hour == testTime.Hour && now.Minute == testTime.Minute && a == true)
            {
                startupPath = Environment.CurrentDirectory;
                player = new MediaPlayer();
                player.Open(new Uri("file://" + startupPath + "\\audio_thongbao.mp3"));
                player.Play();
                MessageBoxResult re = MessageBox.Show("Đã đến giờ thưa ngài !!!\n",
                    "Sự kiện bắt đầu", MessageBoxButton.OK, MessageBoxImage.Information);
                if(re == MessageBoxResult.OK)
                {
                    player.Stop();
                }
                a = false;
            }
        }
        public MainWindow()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            InitializeComponent();
            Init();
            UpdateEvents();
            InstallMeOnStartUp();


            //player = new MediaPlayer();

            //startupPath = Environment.CurrentDirectory;

            //player.Open(new Uri("file://" + startupPath + "\\audio_thongbao.mp3"));
            //player.Play();

            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    player.Open(new Uri(openFileDialog.FileName));
            //    player.Play();
            //}
            //MessageBox.Show(Environment.CurrentDirectory);
            //MessageBox.Show(DateTime.Now.ToString());
            //MessageBox.Show(testTime.ToString());


            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("icTDL.ico");
            ni.Visible = true;
            ni.Click +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    pnlSuKien.Children.Clear();
                    UpdateEvents();
                    this.WindowState = WindowState.Normal;
                };
        }

        //protected override void OnStateChanged(EventArgs e)
        //{
        //    if (WindowState == WindowState.Minimized)
        //        this.Hide();

        //    base.OnStateChanged(e);
        //}

        private void InstallMeOnStartUp()
        {
            try
            {
                //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu));
                RegistryKey key =
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Assembly curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            catch
            {
                //do nothing
                MessageBox.Show("err");
            }
        }
        private void Init()
        {
            grid_edit.Visibility = Visibility.Collapsed;

            string[] id_str = File.ReadAllLines("id.csv");
            Stt = int.Parse(id_str[0]);

            int[] hours12 = new int[12];
            int[] hours24 = new int[24];
            int[] mins = new int[60];
            String[] Am_Pm = { "AM", "PM" };
            for (int i = 0; i < 60; i++)
            {
                mins[i] = i;
                if (i < 12)
                    hours12[i] = i + 1;
                if (i < 24)
                    hours24[i] = i;
            }
            btn_Current_Focus = new Button();
            btn_stop.Background = null;
            btn_stop.BorderBrush = null;
            
            cb_hour12.ItemsSource = hours12;
            cb_hour24.ItemsSource = hours24;
            cb_minute12.ItemsSource = mins;
            cb_minute24.ItemsSource = mins;
            cb_AM_PM.ItemsSource = Am_Pm;

            cb_hour12.SelectedIndex = 8;
            cb_hour24.SelectedIndex = 9;
            cb_minute12.SelectedIndex = 0;
            cb_minute24.SelectedIndex = 0;
            cb_AM_PM.SelectedIndex = 0;
            btn_add.Background = null;
            btn_add.BorderBrush = null;
            datePicker.SelectedDate = DateTime.Today;
            btn_edit.Background = null;
            btn_del.Background = null;
            btn_edit.BorderBrush = null;
            btn_del.BorderBrush = null;
            datePicker.SelectedDateFormat = DatePickerFormat.Long;
            //myWindow.Icon = new BitmapImage(new Uri("/VBDAdvertisement;icon.png"));
        }

        private void StoreData(int id, string tenSK, string motaSK, DateTime date, string isPast, string isRemind)
        {
            string newLine = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}",
                id, tenSK, motaSK, date.DayOfWeek, date.Day, date.Month, date.Year, date.Hour, date.Minute, isPast, isRemind);
            csv = new StringBuilder();
            csv.AppendLine(newLine);
            File.AppendAllText("data.csv", csv.ToString());
            File.WriteAllText("id.csv", (id+1) + "");
            csv.Clear();
        }


        private void Add_Event(string id, string TenSK, string MotaSK, DateTime date, int Gio, int Phut, string isPast, string isRemind)
        {
            Border border = new Border();
            Button btn = new Button();
            Canvas canvas = new Canvas();
            TextBlock txTenSK = new TextBlock();
            TextBlock txMoTaSK = new TextBlock();
            TextBlock txNgay = new TextBlock();
            TextBlock txGio = new TextBlock();
            TextBlock event_id = new TextBlock();
            TextBlock event_isPast = new TextBlock();
            TextBlock event_isRemind = new TextBlock();

            event_id.Text = id;
            event_id.Visibility = Visibility.Hidden;
            event_isPast.Text = isPast;                             // isPast = 0: Sự kiện chưa xảy ra, isPast = 1: sự kiện đã xảy ra
            event_isPast.Visibility = Visibility.Hidden;
            event_isRemind.Text = isRemind;                           // isRemind = 0: không nhắc lịch, isRemind = 1: có nhắc lịch
            event_isRemind.Visibility = Visibility.Hidden;
            

            border.BorderThickness = new Thickness(0);
            border.Background = Brushes.White;
            if (isPast.Equals("1"))
            {
                border.Opacity = 0.2f;
            }
            else
            {
                border.Opacity = 0.5f;
            }
            
            border.Margin = new Thickness(5, 5, 5, 0);
            border.CornerRadius = new CornerRadius(15);
            border.BorderBrush = null;

            canvas.Height = btn.Height = 90;
            canvas.Width = 500;
            btn.Background = null;
            btn.Cursor = Cursors.Hand;
            
            if (isPast.Equals("0"))
            {
                btn.GotFocus += new RoutedEventHandler(onGotFocus_event);
                btn.LostFocus += new RoutedEventHandler(onLossFocus_event);
                btn.MouseDoubleClick += new MouseButtonEventHandler(OnMouseDoubleClick);
            }
            else
            {
                btn_edit.IsEnabled = false;
                btn.Focusable = false;
            }
            btn.Foreground = Brushes.DarkViolet;

            Canvas.SetBottom(txNgay, 10);
            Canvas.SetRight(txNgay, 170);
            Canvas.SetTop(txGio, 5);
            Canvas.SetRight(txGio, 170);
            
            txTenSK.Text = TenSK;
            txTenSK.FontSize = 20;
            txTenSK.FontWeight = FontWeights.Bold;
            txTenSK.Margin = new Thickness(10, 5, 0, 0);
            txTenSK.TextWrapping = TextWrapping.WrapWithOverflow;
            txTenSK.Width = 200;
            txTenSK.Height = 30;

            txMoTaSK.Width = 300;
            if (MotaSK.Length > 43)
            {
                canvas.Height = btn.Height = 110 + 30 * (int) (MotaSK.Length / 43);
            }
            txMoTaSK.Text = MotaSK;
            txMoTaSK.TextWrapping = TextWrapping.Wrap;
            txMoTaSK.Margin = new Thickness(10, 40, 0, 0);
            txMoTaSK.FontStyle = FontStyles.Italic;
            txMoTaSK.FontSize = 15;
            txMoTaSK.Foreground = Brushes.DarkBlue;

            txNgay.Text = date.DayOfWeek + ", " + date.Day + "/" + date.Month + "/" + date.Year;
            txNgay.FontSize = 18;
            txNgay.Foreground = Brushes.Black;

            txGio.FontSize = 18;
            string h = (Gio < 10) ? "0" + Gio : "" + Gio;
            string m = (Phut < 10) ? "0" + Phut : "" + Phut;

            txGio.Text = h + ":" + m;
            txGio.FontWeight = FontWeights.SemiBold;
            txGio.HorizontalAlignment = HorizontalAlignment.Left;

            canvas.Children.Add(txTenSK);
            canvas.Children.Add(txMoTaSK);
            canvas.Children.Add(txNgay);
            canvas.Children.Add(txGio);
            canvas.Children.Add(event_id);
            canvas.Children.Add(event_isPast);
            canvas.Children.Add(event_isRemind);

            btn.Content = canvas;

            btn.BorderBrush = null;
            border.Child = btn;
            pnlSuKien.Children.Add(border);
            scroll_sukien.ScrollToEnd();
            btn_Current_Focus = btn;
        }


        private void UpdateEvents()
        {
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 1)
            {
                foreach (string line in lines)
                {
                    //Stt++;
                    string[] data = line.Split('\t');
                    Add_Event(data[0], data[1], data[2], new DateTime(int.Parse(data[6]),int.Parse(data[5]), int.Parse(data[4])), 
                        int.Parse(data[7]), int.Parse(data[8]), data[9], data[10]);
                }
            }

        }

        #region ADD EVENTS

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string remind;
            MessageBoxResult re = MessageBox.Show("Bạn có muốn được nhắc khi tới thời gian xảy ra sự kiện không ?", "Còn một bước để có thể thêm sự kiện",
                                                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (re == MessageBoxResult.Yes)
            {
                remind = "1";
            }
            else if (re == MessageBoxResult.No)
            {
                remind = "0";
            }
            else return;
            string name, des;
            DateTime date;
            int Gio = 0, Phut = 0;

            if (radio_24.IsChecked == true)
            {
                Gio = cb_hour24.SelectedIndex;
                Phut = cb_minute24.SelectedIndex;
            }
            else if (radio_12.IsChecked == true)
            {
                if (cb_AM_PM.SelectedIndex == 1)
                {
                    Gio = (cb_hour12.SelectedIndex + 13) % 24;
                }
                else
                {
                    Gio = cb_hour12.SelectedIndex + 1;
                }
                Phut = cb_minute12.SelectedIndex;
            }


            name = (txtTenSuKien.Text == "") ? "UNNAMED EVENT" : txtTenSuKien.Text;
            des = (txtMota.Text == "") ? "Không có mô tả" : txtMota.Text.Trim();

            date = (DateTime)datePicker.SelectedDate;

            if (DateTime.Compare(date, DateTime.Today) >= 0)
            {
                if (!IsDuplicate(name, des, date.Year, date.Month, date.Day, Gio, Phut))
                {
                    Add_Event(Stt + "", name, des, date, Gio, Phut, "0", remind);
                    StoreData(Stt++, name, des, new DateTime(date.Year, date.Month, date.Day, Gio, Phut, 0), "0", remind);
                }
                else
                {
                    MessageBox.Show("Sự kiện bị trùng lặp", "Hey hey ...", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bạn cần phải chọn ngày trong tương lai ! \n Hôm nay là : " + DateTime.Today.ToString("dd/MM/yyyy"),
                    "Hey hey ...", MessageBoxButton.OK, MessageBoxImage.Warning);
                datePicker.SelectedDate = DateTime.Today;
            }
        }

        bool IsDuplicate(string name, string des, int Day, int Month, int Year, int Gio, int Phut)
        {
            string[] lines = File.ReadAllLines("data.csv");
            foreach (string line in lines)
            {
                string[] data = line.Split('\t');
                if (data[1] == name && data[2] == des && data[4] == Year + "" && data[5] == Month + ""
                    && data[6] == Day + "" && data[7] == Gio + "" && data[8] == Phut + "")
                    return true;
            }
            return false;
        }
        #endregion

        #region Xử lý sự kiện
        private void OpenEditWindow()
        {
            Border b = (Border)btn_Current_Focus.Parent;
            b.Background = Brushes.White;
            EditEvent ed = new EditEvent();
            ed.ShowDialog();
            grid_edit.Visibility = Visibility.Collapsed;
        }
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenEditWindow();
        }
        private void onGotFocus_event(object sender, RoutedEventArgs e)
        {
            btn_edit.IsEnabled = true;
            Button btn = (Button)sender;
            Border b = (Border)btn.Parent;
            b.Background = Brushes.Yellow;
            btn_Current_Focus = btn;

            grid_edit.Visibility = Visibility.Visible;
        }
        private void onLossFocus_event(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Border b = (Border)btn.Parent;
            b.Background = Brushes.White;
        }

        private void input_tenSuKien_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtTenSuKien.Text = "";
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

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            OpenEditWindow();
        }

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            Border b = (Border)btn_Current_Focus.Parent;
            b.Background = Brushes.White;
            grid_edit.Visibility = Visibility.Collapsed;
            MessageBoxResult re = MessageBox.Show("Bạn có muốn xoá sự kiện này ?", "ĐỢI CHÚT ...",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (re == MessageBoxResult.Yes)
            {
                Canvas c = ((Canvas)btn_Current_Focus.Content);
                TextBlock[] id = new TextBlock[NUM_ATTR_DATA];
                int k = 0;
                foreach (object o in c.Children)
                {
                    id[k++] = (TextBlock)o;
                }

                int id_num = int.Parse(id[4].Text);

                StringBuilder sb = new StringBuilder();
                string[] lines = File.ReadAllLines("data.csv");
                if (lines.Length > 1)
                {
                    bool isFound = false;
                    foreach (string line in lines)
                    {
                        string[] s = line.Split('\t');
                        if (int.Parse(s[0]) == id_num)
                        {
                            isFound = true;
                            Stt--;
                            File.WriteAllText("id.csv", Stt + "");
                            try
                            {
                                pnlSuKien.Children.RemoveAt(id_num);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            if (!isFound)
                                sb.AppendLine(line);
                            else
                            {
                                s[0] = (id_num++) + "";
                                sb.AppendLine(string.Join("\t", s));
                            }
                        }
                    }

                    File.WriteAllText("data.csv", sb.ToString());
                }
                else if (lines.Length == 1)
                {
                    File.WriteAllText("data.csv", "");
                    File.WriteAllText("id.csv", "0");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
