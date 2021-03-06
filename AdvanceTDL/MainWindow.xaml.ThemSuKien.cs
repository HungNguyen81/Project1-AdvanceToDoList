﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AdvanceTDL
{    
    public partial class MainWindow
    {
        private Brush passedBrush = Brushes.Violet;
        private void Add_Event(string id, string TenSK, string MotaSK, DateTime date, int Gio, 
            int Phut, string isPast, string isRemind, string time2remind, string isLoop, string typeOfLoop, string numOfLoop, bool needToUpdate)
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
            TextBlock txTimeRm = new TextBlock();           // 7 -> 
            TextBlock txLoop = new TextBlock();             // 8 -> 
            TextBlock txTypeLoop = new TextBlock();
            TextBlock txNumLoop = new TextBlock();
            Image img = new Image();

            string dir = "file://" + Directory.GetCurrentDirectory() + "\\check.png";
            img.Source = new BitmapImage(new Uri(dir));
            img.Width = 20;
            img.Height = 20;

            event_id.Text = id;
            event_id.Visibility = Visibility.Hidden;

            // isPast = 0: Sự kiện chưa xảy ra, isPast = 1: sự kiện đã xảy ra
            event_isPast.Text = isPast;                             
            event_isPast.Visibility = Visibility.Hidden;

            // isRemind = 0: không nhắc lịch, isRemind = 1: có nhắc lịch
            event_isRemind.Text = isRemind;                                      
            event_isRemind.Visibility = Visibility.Hidden;

            border.BorderThickness = new Thickness(0);

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
                border.Background = passedBrush;
                btn.Foreground = Brushes.White;
                btn.GotFocus += new RoutedEventHandler(onGotFocus_Pass_Event);
                btn.LostFocus += new RoutedEventHandler(onLossFocus_Pass_Event);
                img.Visibility = Visibility.Visible;

                // isPast = 2 => sự kiện đã bị bỏ qua.
                if (isPast.Equals("2"))
                {
                    string miss_dir = "file://" + Directory.GetCurrentDirectory() + "\\miss.png";
                    img.Source = new BitmapImage(new Uri(miss_dir));
                }
            }

            Canvas.SetBottom(txNgay, 10);
            Canvas.SetRight(txNgay, 100);
            Canvas.SetTop(txGio, 5);
            Canvas.SetRight(txGio, 100);
            Canvas.SetTop(img, 10);
            Canvas.SetRight(img, 150);

            txTenSK.Text = TenSK;
            txTenSK.FontSize = 20;
            txTenSK.FontWeight = FontWeights.Bold;
            txTenSK.Margin = new Thickness(10, 5, 0, 0);
            txTenSK.TextWrapping = TextWrapping.WrapWithOverflow;
            txTenSK.Width = 250;
            txTenSK.Height = 30;

            TextBlock txMore = new TextBlock();
            txMore.Text = "...";
            txMore.FontSize = 20;
            txMore.FontWeight = FontWeights.Bold;
            txMore.Margin = new Thickness(235, 5, 0, 0);
            txMore.Height = 30;
            txMore.Visibility = Visibility.Collapsed;

            txMoTaSK.Width = 200;

            // Nếu mota quá dài => tăng chiều cao của ô SK
            if (MotaSK.Length > (int)myConsts.MOTA_LENGTH)
            {
                canvas.Height = btn.Height = 110 + 30 * (int)(MotaSK.Length / (int)myConsts.MOTA_LENGTH);
            }
            if (TenSK.Length > 20) txMore.Visibility = Visibility.Visible;
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

            txTimeRm.Visibility = Visibility.Collapsed;
            txTimeRm.Text = time2remind + "";

            txLoop.Visibility = Visibility.Collapsed;
            txLoop.Text = isLoop;
            
            txTypeLoop.Visibility = Visibility.Collapsed;
            txTypeLoop.Text = typeOfLoop;

            txNumLoop.Visibility = Visibility.Collapsed;
            txNumLoop.Text = numOfLoop;

            canvas.Children.Add(txTenSK);           // 0 -> Tên sự kiện
            canvas.Children.Add(txMoTaSK);          // 1 -> Vị trí
            canvas.Children.Add(txNgay);            // 2 -> Ngày tháng năm
            canvas.Children.Add(txGio);             // 3 -> Giờ phút
            canvas.Children.Add(event_id);          // 4 -> ID sự kiện <stt trong danh sách>
            canvas.Children.Add(event_isPast);      // 5 -> Trạng thái sự kiện đã qua/missed
            canvas.Children.Add(event_isRemind);    // 6 -> IsRemind?                        
            canvas.Children.Add(txTimeRm);          // 7 -> Time to remind
            canvas.Children.Add(txLoop);            // 8 -> isLoop?
            canvas.Children.Add(txTypeLoop);        // 9 -> type of loop (theo tuần/tháng/năm)
            canvas.Children.Add(txNumLoop);         // 10 -> số lần lặp

            canvas.Children.Add(txMore);            // 11 -> Khi tên sự kiện quá dài
            canvas.Children.Add(img);               // 12 -> tick - passed events

            btn.Content = canvas;
            //border.Style = Resources["MyBorder"] as Style;

            btn.BorderBrush = null;
            border.Child = btn;

            pnlSuKien.Children.Add(border);            
            //scroll_sukien.ScrollToEnd();
            btn_Current_Focus = btn;

            isUpdated = !needToUpdate;
        }


        #region ADD EVENTS

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string remind, name, des;
            DateTime date;
            int Gio = 0, Phut = 0;
            int time2remind;
            string isLoop;
            string typeOfLoop, numOfLoop;
            if (chk_remind.IsChecked == true){
                remind = "1";
            } else { 
                remind = "0"; 
            }

            if (radio_24.IsChecked == true){
                Gio = cb_hour24.SelectedIndex;
                Phut = cb_minute24.SelectedIndex;
            } else if (radio_12.IsChecked == true){
                // PM
                if (cb_AM_PM.SelectedIndex == 1){
                    if(cb_hour12.SelectedIndex < 11) Gio = (cb_hour12.SelectedIndex + 13) % 24;
                    else Gio = cb_hour12.SelectedIndex + 1;
                // AM
                } else {
                    if (cb_hour12.SelectedIndex < 11) Gio = cb_hour12.SelectedIndex + 1;
                    else Gio = (cb_hour12.SelectedIndex + 13) % 24;
                }
                Phut = cb_minute12.SelectedIndex;
            }
            name = (txtTenSuKien.Text == "") ? "UNNAMED EVENT" : txtTenSuKien.Text;
            des = (txtMota.Text == "") ? "Không có vị trí" : txtMota.Text.Trim();

            date = (DateTime)datePicker.SelectedDate;
            date = new DateTime(date.Year, date.Month, date.Day, Gio, Phut, 0);

            time2remind = cb_time2remind.SelectedIndex;
            
            if (chk_loop.IsChecked == true)
            {
                isLoop = "1";

                // index = 0 => lặp theo tuần
                // index = 1 => lặp theo tháng
                // index = 2 => lặp theo năm
                typeOfLoop = "" + cb_loopType.SelectedIndex;
                numOfLoop = "" + (cb_loopNum.SelectedIndex + 1);
             }
            else
            {
                isLoop = "0";
                typeOfLoop = "0";
                numOfLoop = "0";
            }
            
            if (DateTime.Compare(date, DateTime.Now) >= 0)
            {
                if (!IsDuplicate(name, des, date.Year, date.Month, date.Day, Gio, Phut))
                {
                    Add_Event(Stt + "", name, des, date, Gio, Phut, "0", remind,""+time2remind, isLoop, typeOfLoop, numOfLoop, true);
                    StoreData(Stt++, name, des, date, "0", remind, time2remind, isLoop, typeOfLoop, numOfLoop);
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

        // KIỂM TRA SỰ KIỆN TRÙNG LẶP <<LẶP TẤT CẢ THÔNG TIN>>
        bool IsDuplicate(string name, string des, int Day, int Month, int Year, int Gio, int Phut)
        {
            string[] lines = File.ReadAllLines("data.csv");
            foreach (string line in lines)
            {
                string[] data = line.Split(',');
                if (data[1] == name && data[2] == des && data[4] == Year + "" && data[5] == Month + ""
                    && data[6] == Day + "" && data[7] == Gio + "" && data[8] == Phut + "")
                    return true;
            }
            return false;
        }
        #endregion
    }
}
