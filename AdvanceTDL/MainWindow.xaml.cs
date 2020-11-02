using AdvanceTDL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AdvanceTDL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum myConsts
        {
            I_TENSK = 0,
            I_MOTA = 1,
            I_NGAY = 2,
            I_GIO = 3,
            I_ID = 4,
            I_PAST = 5,
            I_REMIND = 6,
            NUM_ATTR_DATA = 9,
            DELTA_SEC = 1,
            MOTA_LENGTH = 30,
            UPDATE_PASSED = -1,
            UPDATE_MISSED = -2
        }
        const int SK_HOMNAY = 0;
        const int SK_NGAYMAI = 1;
        public static int Stt;          // Id của sự kiện, cũng là số thứ tự ô thông tin trong bảng các sự kiện
        private StringBuilder csv;
        public static Button btn_Current_Focus;
        private MediaPlayer player;
        static string startupPath;
        DispatcherTimer dispatcherTimer;

        // Lưu giữ thời gian của sự kiện gần nhất với thời điểm hiện tại
        static infoSK inf;

        // Trạng thái thông báo sự kiện, TRUE khi đã có sk được reminded, FALSE khi thay đổi giá trị *testTime*
        private static bool isPlayed = false;
        private static bool isUpdated = false;

        // 0. Name  1. Des  2. Date  3.Time  4. id  5. isPast  6. isRemind

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (!isUpdated) UpdateEvents();
            if (inf != null)
            {
                if (DateTime.Compare(now.Date, inf.getDate.Date) == 0 && now.Hour == inf.getDate.Hour
                && now.Minute == inf.getDate.Minute && isPlayed == false)
                {
                    startupPath = Environment.CurrentDirectory;
                    player = new MediaPlayer();
                    player.Open(new Uri("file://" + startupPath + "\\remind_audio.mp3")); //audio_thongbao.mp3
                    player.Play();

                    // Khi đã thông báo remind thì có nghĩa là tăng thêm 1 sự kiện passed => cần cập nhật
                    isUpdated = false;
                    MessageBoxResult re = MessageBox.Show("Đã đến giờ !!!\n",
                        "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Information); //start_winxp.mp3
                    if (re == MessageBoxResult.OK)
                    {
                        player.Stop();
                    }
                    isPlayed = true;
                }
                else
                {
                    if (!(DateTime.Compare(now.Date, inf.getDate.Date) == 0
                        && now.Hour == inf.getDate.Hour
                        && now.Minute == inf.getDate.Minute))
                    {
                        isPlayed = false;
                        SetupInf();
                    }
                }
            }
            //else SetupInf();
        }
        private void SetupInf()
        {
            try
            {
                string[] lines = File.ReadAllLines("data.csv");
                if(lines.Length > 0)
                {
                    foreach (string line in lines)
                    {
                        string[] data = line.Split('\t');
                        if (data[10].Equals("1") && data[9].Equals("0"))
                        {
                            inf = new infoSK(data[0], data[1], data[2],
                                new DateTime(int.Parse(data[6]), int.Parse(data[5]), int.Parse(data[4]),
                                int.Parse(data[7]), int.Parse(data[8]), 0), "0", "1");
                            break;
                        }
                    }
                }
                else
                {
                    inf = new infoSK("", "", "", DateTime.Now, "", "");
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show("Error when opening file!\n" + exp.Message);
            }            
        }
        public MainWindow()
        {
            InitializeComponent();
            Init();
            UpdateEvents();
            SetupInf();
            InstallMeOnStartUp();

            scroll_sukien.ScrollToEnd();

            player = new MediaPlayer();
            startupPath = Environment.CurrentDirectory;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            // Lặp lại sau mỗi DELTA_SEC (giây)
            dispatcherTimer.Interval = new TimeSpan(0, 0, (int)myConsts.DELTA_SEC);
            dispatcherTimer.Start();            
            
            player.Open(new Uri("file://" + startupPath + "\\start_winxp.mp3"));
            player.Play();


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
            string[] Am_Pm = { "AM", "PM" };
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
        private int isNearEvents(DateTime date)
        {
            DateTime today = DateTime.Today;
            if(date.Month == today.Month && date.Year == today.Year)
            {
                if (date.Day - today.Day == 0) return SK_HOMNAY;
                if (date.Day - today.Day == 1) return SK_NGAYMAI;
            }
            return -1;
        }
        private void Add_Event(string id, string TenSK, string MotaSK, DateTime date, int Gio, int Phut, string isPast, string isRemind)
        {
            Border border = new Border();
            Button btn = new Button();
            Canvas canvas = new Canvas();
            TextBlock txTenSK = new TextBlock();            // 0 -> Tên sk
            TextBlock txMoTaSK = new TextBlock();           // 1 -> Mô tả sk
            TextBlock txNgay = new TextBlock();             // 2 -> Ngày
            TextBlock txGio = new TextBlock();              // 3 -> Giờ
            TextBlock event_id = new TextBlock();           // 4 -> ID sk
            TextBlock event_isPast = new TextBlock();       // 5 -> Đã qua hay chưa
            TextBlock event_isRemind = new TextBlock();     // 6 -> có nhắc nhở không
            Image img = new Image();
            string dir = "file://" + Directory.GetCurrentDirectory() + "\\check.png";
            img.Source = new BitmapImage(new Uri(dir));
            img.Width = 20;
            img.Height = 20;

            event_id.Text = id;
            event_id.Visibility = Visibility.Hidden;
            event_isPast.Text = isPast;                             // isPast = 0: Sự kiện chưa xảy ra, isPast = 1: sự kiện đã xảy ra
            event_isPast.Visibility = Visibility.Hidden;
            event_isRemind.Text = isRemind;                           // isRemind = 0: không nhắc lịch, isRemind = 1: có nhắc lịch
            event_isRemind.Visibility = Visibility.Hidden;            

            border.BorderThickness = new Thickness(0);

            if (isNearEvents(date) == SK_HOMNAY)
            {
                border.Background = Brushes.DarkOrange;
            }
            else if(isNearEvents(date) == SK_NGAYMAI)
            {
                border.Background = Brushes.Yellow;
            }
            else
            {
                border.Background = Brushes.White;
            }

            border.Margin = new Thickness(5, 5, 5, 0);
            border.CornerRadius = new CornerRadius(15);
            border.BorderBrush = null;

            canvas.Height = btn.Height = 90;
            canvas.Width = 500;
            btn.Background = null;
            btn.Cursor = Cursors.Hand;

            if (isPast.Equals("0"))   // Nếu sk đã qua thì Opacity = 0.2, Focusable = false
            {
                border.Opacity = 0.8f;
                btn.GotFocus += new RoutedEventHandler(onGotFocus_event);
                btn.LostFocus += new RoutedEventHandler(onLossFocus_event);
                btn.MouseDoubleClick += new MouseButtonEventHandler(OnMouseDoubleClick);
                img.Visibility = Visibility.Hidden;
            }
            else
            {
                border.Opacity = 0.8f;
                border.Background = Brushes.Gray;
                btn.Foreground = Brushes.White;
                btn.GotFocus += new RoutedEventHandler(onGotFocus_Pass_Event);
                btn.LostFocus += new RoutedEventHandler(onLossFocus_Pass_Event);                
                img.Visibility = Visibility.Visible;

                if (isPast.Equals("2"))
                {
                    string miss_dir = "file://" + Directory.GetCurrentDirectory() + "\\miss.png";
                    img.Source = new BitmapImage(new Uri(miss_dir));
                }
            }           

            Canvas.SetBottom(txNgay, 10);
            Canvas.SetRight(txNgay, 170);
            Canvas.SetTop(txGio, 5);
            Canvas.SetRight(txGio, 170);
            Canvas.SetTop(img, 10);
            Canvas.SetRight(img, 220);

            txTenSK.Text = TenSK;
            txTenSK.FontSize = 20;
            txTenSK.FontWeight = FontWeights.Bold;
            txTenSK.Margin = new Thickness(10, 5, 0, 0);
            txTenSK.TextWrapping = TextWrapping.WrapWithOverflow;
            txTenSK.Width = 200;
            txTenSK.Height = 30;            

            TextBlock txMore = new TextBlock();
            txMore.Text = "...";
            txMore.FontSize = 20;
            txMore.FontWeight = FontWeights.Bold;
            txMore.Margin = new Thickness(180, 5, 0, 0);
            txMore.Height = 30;
            txMore.Visibility = Visibility.Collapsed;         

            txMoTaSK.Width = 200;
            if (MotaSK.Length > (int)myConsts.MOTA_LENGTH)
            {
                canvas.Height = btn.Height = 110 + 30 * (int) (MotaSK.Length / (int)myConsts.MOTA_LENGTH);
            }
            if(TenSK.Length > 40) txMore.Visibility = Visibility.Visible;
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

            canvas.Children.Add(txTenSK);           // 0
            canvas.Children.Add(txMoTaSK);          // 1
            canvas.Children.Add(txNgay);            // 2
            canvas.Children.Add(txGio);             // 3
            canvas.Children.Add(event_id);          // 4
            canvas.Children.Add(event_isPast);      // 5
            canvas.Children.Add(event_isRemind);    // 6
            canvas.Children.Add(txMore);            // 7            
            canvas.Children.Add(img);               // tick - passed events

            btn.Content = canvas;

            btn.BorderBrush = null;
            border.Child = btn;
            pnlSuKien.Children.Add(border);
            //scroll_sukien.ScrollToEnd();
            btn_Current_Focus = btn;

            isUpdated = false;
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
            date = new DateTime(date.Year, date.Month, date.Day, Gio, Phut, 0);

            if (DateTime.Compare(date, DateTime.Now) >= 0)
            {
                if (!IsDuplicate(name, des, date.Year, date.Month, date.Day, Gio, Phut))
                {
                    Add_Event(Stt + "", name, des, date, Gio, Phut, "0", remind);
                    StoreData(Stt++, name, des, date, "0", remind);
                    UpdateEvents();
                    SetupInf();
                }
                else
                {
                    MessageBox.Show("Sự kiện bị trùng lặp", "Hey hey ...", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bạn cần phải chọn ngày giờ trong tương lai ! \nHôm nay là : " + DateTime.Today.ToString("dd/MM/yyyy") +
                    "\nGiờ hiện tại: " + DateTime.Now.ToString("hh:mm:ss"),
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
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenEditWindow();
        }
        private void onGotFocus_event(object sender, RoutedEventArgs e)
        {
            btn_edit.IsEnabled = true;
            btn_miss.IsEnabled = true;
            Button btn = (Button)sender;
            Border b = (Border)btn.Parent;
            b.Background = Brushes.Aqua;
            btn_Current_Focus = btn;
            grid_edit.Visibility = Visibility.Visible;
        }
        private void onGotFocus_Pass_Event(object sender, RoutedEventArgs e)
        {
            btn_edit.IsEnabled = false;
            btn_miss.IsEnabled = false;
            Button btn = (Button)sender;
            Border b = (Border)btn.Parent;
            b.Background = Brushes.Cyan;
            btn_Current_Focus = btn;

            grid_edit.Visibility = Visibility.Visible;
        }
        private void onLossFocus_event(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Border border = (Border)btn.Parent;
            Canvas c = (Canvas)btn.Content;
            int i = 0;
            TextBlock[] t = new TextBlock[(int)myConsts.NUM_ATTR_DATA];
            foreach(object o in c.Children)
            {
                if (i < (int)myConsts.NUM_ATTR_DATA - 1)
                {
                    t[i++] = (TextBlock)o;
                }                            
            }
            string[] d = t[(int)myConsts.I_NGAY].Text.Split(',');
            string[] d1 = d[1].Split('/');
            DateTime date = new DateTime(int.Parse(d1[2]), int.Parse(d1[1]), int.Parse(d1[0]));
            
            if (isNearEvents(date) == SK_HOMNAY)
            {
                border.Background = Brushes.DarkOrange;
            }
            else if (isNearEvents(date) == SK_NGAYMAI)
            {
                border.Background = Brushes.Yellow;
            }
            else
            {
                border.Background = Brushes.White;
            }
        }
        private void onLossFocus_Pass_Event(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Border border = (Border)btn.Parent;
            border.Background = Brushes.Gray;
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
        private void OpenEditWindow()
        {
            Border b = (Border)btn_Current_Focus.Parent;
            b.Background = Brushes.White;
            EditEvent ed = new EditEvent(this);
            ed.ShowDialog();
            grid_edit.Visibility = Visibility.Collapsed;
        }

        /* DELETE EVENT'S INFORMATION */

        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            Border b = (Border)btn_Current_Focus.Parent;
            //b.Background = Brushes.White;
            grid_edit.Visibility = Visibility.Collapsed;
            MessageBoxResult re = MessageBox.Show("Bạn có muốn xoá sự kiện này ?", "ĐỢI CHÚT ...",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (re == MessageBoxResult.Yes)
            {
                Canvas c = ((Canvas)btn_Current_Focus.Content);
                TextBlock[] id = new TextBlock[(int)myConsts.NUM_ATTR_DATA-1];
                int k = 0;
                foreach (object o in c.Children)
                {                    
                    if(k < (int)myConsts.NUM_ATTR_DATA-1) id[k++] = (TextBlock)o;
                }

                int id_num = int.Parse(id[(int)myConsts.I_ID].Text);
                StringBuilder sb = new StringBuilder();

                string[] lines = File.ReadAllLines("data.csv");
                try
                {
                    pnlSuKien.Children.RemoveAt(id_num);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                if (lines.Length > 1)
                {
                    bool isFound = false;
                    string[] ids = File.ReadAllLines("id.csv");
                    Stt = int.Parse(ids[0]);

                    foreach (string line in lines)
                    {
                        string[] s = line.Split('\t');
                        if (int.Parse(s[0]) == id_num)
                        {
                            isFound = true;
                            Stt--;
                            File.WriteAllText("id.csv", Stt + "");                            
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
                UpdateEvents();
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

        private void btn_miss_Click(object sender, RoutedEventArgs e)
        {
            // Nếu sk đã qua thì Opacity = 0.2, isEnable = false, Focusable = false
            Canvas c = (Canvas)btn_Current_Focus.Content;
            Border b = (Border)btn_Current_Focus.Parent;
            TextBlock[] texts = new TextBlock[(int)myConsts.NUM_ATTR_DATA];
            int i = 0;
            foreach(object o in c.Children)
            {
                if(i < (int)myConsts.NUM_ATTR_DATA-1)
                {
                    texts[i] = (TextBlock)o;
                    i++;
                }
            }
            b.Opacity = 0.2f;
            btn_Current_Focus.Focusable = false;
            texts[(int)myConsts.I_PAST].Text = "2";
            UpdateData(texts[(int)myConsts.I_ID].Text, (int)myConsts.UPDATE_MISSED);
            isUpdated = false;
        }
        private void UpdateData(string id, int indexOfItem)
        {
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                string temp = null;
                foreach (string line in lines)
                {
                    string[] data = line.Split('\t');
                    switch (indexOfItem)
                    {
                        case (int)myConsts.I_PAST:
                            if (data[0].Equals(id))
                            {
                                data[9] = "1";
                                temp = string.Join("\t", data);
                            }
                            break;
                        case (int)myConsts.UPDATE_MISSED:   //-2
                            if (data[0].Equals(id))
                            {
                                data[9] = "2";
                                temp = string.Join("\t", data);
                            }
                            break;
                        // Kiểm tra sự kiện đã xảy ra;
                        case (int)myConsts.UPDATE_PASSED:   // -1   
                            DateTime date = new DateTime(int.Parse(data[6]), int.Parse(data[5]),
                                int.Parse(data[4]), int.Parse(data[7]), int.Parse(data[8]), 0);
                            if (data[9].Equals("0") && DateTime.Compare(DateTime.Now, date) >= 0)
                            {
                                data[9] = "1";
                                sb.AppendLine(string.Join("\t", data));                                
                            } else
                            {
                                sb.AppendLine(line);
                            }
                            break;
                    }
                }
                if(temp != null)
                {
                    sb.AppendLine(temp);
                    foreach(string line in lines)
                    {
                        string[] data = line.Split('\t');
                        if (data[0].Equals(id) == false)
                        {
                            sb.AppendLine(line);
                        }                      
                    }
                    File.WriteAllText("data.csv", sb.ToString());
                    UpdateID();
                    if(indexOfItem != -1) UpdateEvents();
                } 
                else if(indexOfItem == -1)
                {
                    File.WriteAllText("data.csv", sb.ToString());
                }
            }
        }
        private void UpdateID()   
        {
            int num = 0;
            string[] lines = File.ReadAllLines("data.csv");
            string[] ids = File.ReadAllLines("id.csv");
            foreach(string id in ids)
            {
                num = int.Parse(id);
            }
            if(lines.Length > 1)
            {
                StringBuilder sb = new StringBuilder();
                int newId = 0;
                foreach(string line in lines)
                {
                    string[] data = line.Split('\t');                    
                    if(newId < num)
                    {
                        data[0] = "" + newId;
                        newId++;
                        sb.AppendLine(string.Join("\t", data));
                    }
                }
                File.WriteAllText("data.csv", sb.ToString());
                File.WriteAllText("id.csv", newId+"");
            }
        }
        private void SortEvent()
        {
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 0)
            {
                //List<infoSK> listSK_passed = new List<infoSK>();
                //List<infoSK> listSK_coming = new List<infoSK>();
                List<infoSK> listSK = new List<infoSK>();

                StringBuilder sb = new StringBuilder();
                DateTime[] date = new DateTime[lines.Length];

                foreach (string line in lines)
                {
                    string[] data = line.Split('\t');
                    int i = 0;

                    date[i] = new DateTime(int.Parse(data[6]), int.Parse(data[5]), int.Parse(data[4]), int.Parse(data[7]), int.Parse(data[8]), 0);                   
                    listSK.Add(new infoSK(data[0], data[1], data[2], date[i++], data[9], data[10]));
                }
                //listSK_passed.Sort();
                //listSK_coming.Sort();
                listSK.Sort();
                foreach(infoSK s in listSK)
                {
                    sb.AppendLine(s.ToString());
                }      
                File.WriteAllText("data.csv", sb.ToString());
                UpdateID();
                isUpdated = false;
            }
        }
        public void UpdateEvents()
        {                
            if(pnlSuKien.Children != null) pnlSuKien.Children.Clear();            

            UpdateData(null, (int)myConsts.UPDATE_PASSED);
            SortEvent();
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 0)
            {
                foreach (string line in lines)
                {                    
                    string[] data = line.Split('\t');
                    Add_Event(data[0], data[1], data[2], new DateTime(int.Parse(data[6]), int.Parse(data[5]), int.Parse(data[4])),
                        int.Parse(data[7]), int.Parse(data[8]), data[9], data[10]);                    
                }
            }
            isUpdated = true;
        }
        #endregion
    }
}
