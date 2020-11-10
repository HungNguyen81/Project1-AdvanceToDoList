using System;
using System.IO;
using System.Text;
using System.Threading;
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
        public enum myConsts
        {
            I_TENSK = 0,
            I_MOTA = 1,
            I_NGAY = 2,
            I_GIO = 3,
            I_ID = 4,
            I_PAST = 5,
            I_REMIND = 6,  
            I_TIME_REMIND = 7,
            I_IS_LOOP = 8,
            NUM_ATTR_DATA = 10,
            DELTA_SEC = 1,
            MOTA_LENGTH = 30,
            UPDATE_PASSED = -1,
            UPDATE_MISSED = -2,
            TICKS_PER_SECOND = 1000000
        }
        public enum DATA
        {
            ID = 0,
            TENSK = 1,
            VITRI = 2,
            NGAY_TRONG_TUAN = 3,
            NGAY = 4,
            THANG = 5,
            NAM = 6,
            GIO = 7,
            PHUT = 8,
            IS_PASSED = 9,
            IS_REMIND = 10,
            TIME_TO_REMIND = 11,
            IS_LOOP = 12
        }
        public static int[] TIME_REMIND = { 1, 5, 10, 15, 30, 60, 0 };
        public const string strDataFormat = "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}";
        public static int DemNguoc = -1;

        AdvanceTDL_Gadget.MainWindow mGadget;
        const int SK_HOMNAY = 0;
        const int SK_NGAYMAI = 1;
        public static int Stt;          // Id của sự kiện, cũng là số thứ tự ô thông tin trong bảng các sự kiện
        private StringBuilder csv;
        public static Button btn_Current_Focus;
        private MediaPlayer player;
        static string startupPath;
        DispatcherTimer dispatcherTimer;
        //public static RoutedCommand MyCommand = new RoutedCommand();

        // Lưu giữ thời gian của sự kiện gần nhất với thời điểm hiện tại
        static infoSK inf;

        // Trạng thái thông báo sự kiện, TRUE khi đã có sk được reminded, FALSE khi thay đổi giá trị *testTime*
        private static bool isPlayed = false;
        private static bool isUpdated = false;

        // 0. Name  1. Des  2. Date  3.Time  4. id  5. isPast  6. isRemind

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DemNguoc--;
            if (DemNguoc == 0) UpdateEvents();
            else if (DemNguoc < 0) DemNguoc = DemNguoc % 10 - 1;
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
                    DemNguoc = inf.ThoiGianDemNguoc();
                    if (inf.GetLoop.Equals("1"))
                    {
                        inf.MakeLoop();
                        isUpdated = false;
                    }
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
            string[] gadgetState = File.ReadAllLines("gadgetState.csv");
            if (gadgetState[0].Equals("0")) 
                chk_Gadget.IsChecked = false;
            else chk_Gadget.IsChecked = true;
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
                        if (data[(int)DATA.IS_REMIND].Equals("1") && data[(int)DATA.IS_PASSED].Equals("0"))
                        {
                            int time2rm = TIME_REMIND[int.Parse(data[(int)DATA.TIME_TO_REMIND])];
                            int h = int.Parse(data[(int)DATA.GIO]), m = int.Parse(data[(int)DATA.PHUT]);
                            if (time2rm >= 60)
                            {
                                if(int.Parse(data[(int)DATA.GIO]) == 6)
                                {
                                    h = 24;
                                }
                                else h -= time2rm / 60;
                            }
                            m -= time2rm % 60;
                            if(m < 0)
                            {
                                m += 60;
                                h--;
                            }
                            DateTime date = new DateTime(int.Parse(data[6]), int.Parse(data[5]), int.Parse(data[4]), h, m, 0);
                            if(DateTime.Compare(date, DateTime.Now) <= 0)
                                continue;
                            else
                            {
                                inf = new infoSK(data[(int)DATA.ID], data[1], data[2], date, "0", "1",
                                    int.Parse(data[(int)DATA.TIME_TO_REMIND]), data[(int)DATA.IS_LOOP]);
                                break;
                            }                                                           
                        }
                    }
                }
                if(inf == null)
                {
                    inf = new infoSK("", "", "", new DateTime(1900, 1, 8), "", "");
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show("Error when opening file!\n" + exp.Message);
            }            
        }
        public MainWindow()
        {          
            CreateShortKeys();
            InitializeComponent();
            Init();
            UpdateEvents();
            SetupInf();

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

        //private void InstallMeOnStartUp()
        //{
        //    try
        //    {
        //        //MessageBox.Show(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu));
        //        RegistryKey key =
        //            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        //        Assembly curAssembly = Assembly.GetExecutingAssembly();
        //        key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
        //    }
        //    catch
        //    {
        //        //do nothing
        //        MessageBox.Show("err");
        //    }
        //}

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

            cb_time2remind.SelectedIndex = 6;

            btn_add.Background = null;
            btn_add.BorderBrush = null;
            datePicker.SelectedDate = DateTime.Today;
            btn_edit.Background = null;
            btn_del.Background = null;
            btn_edit.BorderBrush = null;
            btn_del.BorderBrush = null;
            datePicker.SelectedDateFormat = DatePickerFormat.Long;
        }
        private void StoreData(int id, string tenSK, string motaSK, DateTime date, 
            string isPast, string isRemind, int time_to_remind, string isLoop)
        {
            string newLine = string.Format(strDataFormat, id, tenSK, motaSK, date.DayOfWeek,
                date.Day, date.Month, date.Year, date.Hour, date.Minute, 
                isPast, isRemind, time_to_remind, isLoop);
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
       
        private void CreateShortKeys()
        {
            // Ctrl + X: Xoá sự kiện
            RoutedCommand newCmd = new RoutedCommand();
            newCmd.InputGestures.Add(new KeyGesture(Key.X, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newCmd, btn_del_Click));

            // Ctrl + N: Thêm sự kiện
            newCmd = new RoutedCommand();
            newCmd.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newCmd, Btn_Add_Click));

            // Ctrl + M: Bỏ qua sự kiện
            newCmd = new RoutedCommand();
            newCmd.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newCmd, btn_edit_Click));
        }   
    }
}
