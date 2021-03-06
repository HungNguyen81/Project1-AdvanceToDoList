﻿using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdvanceTDL
{
    /// <summary>
    /// Interaction logic for EditEvent.xaml
    /// </summary>
    public partial class EditEvent : Window
    {
        private Button btnSender;
        private string remind;
        private bool isSave;
        private MainWindow main;
        public EditEvent(MainWindow main)
        {
            this.main = main;
            isSave = false;
            InitializeComponent();
            Init_EditWindow();            
        }

        private void Init_EditWindow()
        {
            int[] hours = new int[24];
            int[] mins = new int[60];
            for (int i = 0; i < 60; i++)
            {
                mins[i] = i;
                if (i < 24)
                    hours[i] = i;
            }
            cb_gio.ItemsSource = hours;
            cb_phut.ItemsSource = mins;
            cb_numLoop.ItemsSource = MainWindow.numLoop;

            btnSender = MainWindow.btn_Current_Focus;
            Canvas c = (Canvas)btnSender.Content;
            string[] texts = new string[(int)MainWindow.myConsts.NUM_ATTR_DATA];

            int k = 0;
            foreach (object o in c.Children)
            {
                if (k < (int)MainWindow.myConsts.NUM_ATTR_DATA - 1)
                {
                    TextBlock t = (TextBlock)o;
                    texts[k++] = t.Text;
                }                
            }
            txt_edit_tenSK.Text = texts[(int)MainWindow.myConsts.I_TENSK];
            txt_edit_motaSK.Text = texts[(int)MainWindow.myConsts.I_MOTA];
            string[] s = texts[(int)MainWindow.myConsts.I_NGAY].Split(',');
            string[] s1 = s[1].Split('/');
            date_edit.SelectedDate = new DateTime(int.Parse(s1[2]), int.Parse(s1[1]), int.Parse(s1[0]));

            string[] gioPhut = texts[(int)MainWindow.myConsts.I_GIO].Split(':');
            int gio = int.Parse(gioPhut[0]);
            int phut = int.Parse(gioPhut[1]);
            cb_gio.SelectedIndex = gio;
            cb_phut.SelectedIndex = phut;
            cb_time2remind.SelectedIndex = int.Parse(texts[(int)MainWindow.myConsts.I_TIME_REMIND]);
            if (texts[(int)MainWindow.myConsts.I_IS_LOOP].Equals("0"))
            {
                chk_loop.IsChecked = false;
                grid_loopOption.Visibility = Visibility.Collapsed;
            }
            else
            {
                chk_loop.IsChecked = true;
                grid_loopOption.Visibility = Visibility.Visible;
                cb_typeLoop.SelectedIndex = int.Parse(texts[(int)MainWindow.myConsts.I_TYPE_LOOP]);
                cb_numLoop.SelectedIndex = int.Parse(texts[(int)MainWindow.myConsts.I_NUM_LOOP]);
            }            

            txb_Id.Text = "ID: " + texts[(int)MainWindow.myConsts.I_ID];

            if (texts[(int)MainWindow.myConsts.I_REMIND] == "1") check_remind.IsChecked = true;
            else check_remind.IsChecked = false;            
        }

        private void EditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult re = MessageBox.Show("Bạn muốn đóng hộp thoại này chứ?!", "Xác nhận đóng",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (re == MessageBoxResult.OK)
            {
                if (isSave)
                {
                    SaveEdited();
                    main.UpdateEvents();
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void SaveEdited()
        {
            DateTime date = (DateTime)date_edit.SelectedDate;
            if (DateTime.Compare(date, DateTime.Today) >= 0)
            {
                Canvas c = (Canvas)btnSender.Content;
                TextBlock[] children = new TextBlock[(int)MainWindow.myConsts.NUM_ATTR_DATA];

                int k = 0;
                foreach (object o in c.Children)
                {
                    if(k < (int)MainWindow.myConsts.NUM_ATTR_DATA-1) children[k++] = (TextBlock)o;
                }

                children[0].Text = (txt_edit_tenSK.Text == "") ? "UNNAMED EVENT" : txt_edit_tenSK.Text;
                children[1].Text = txt_edit_motaSK.Text;
                children[2].Text = date.DayOfWeek + ", " + date.Day + "/" + date.Month + "/" + date.Year;
                string h = (cb_gio.SelectedIndex < 10) ? "0" + cb_gio.SelectedIndex : "" + cb_gio.SelectedIndex;
                string m = (cb_phut.SelectedIndex < 10) ? "0" + cb_phut.SelectedIndex : "" + cb_phut.SelectedIndex;
                children[3].Text = h + ":" + m;

                int id_num = int.Parse(children[(int)MainWindow.myConsts.I_ID].Text);

                StringBuilder sb = new StringBuilder();
                string[] lines = File.ReadAllLines("data.csv");

                if (check_remind.IsChecked == true)
                    remind = "1";
                else
                    remind = "0";
                if (lines.Length >= 1)
                {
                    foreach (string line in lines)
                    {
                        string[] s = line.Split(',');
                        if (int.Parse(s[0]) == id_num)
                        {
                            int isLoop, typeLoop, numLoop;
                            if (chk_loop.IsChecked == true)
                            {
                                isLoop = 1;
                                typeLoop = cb_typeLoop.SelectedIndex;
                                numLoop = cb_numLoop.SelectedIndex;
                            }
                            else
                            {
                                isLoop = 0;
                                typeLoop = 0;
                                numLoop = 0;
                            }
                            string newLine = string.Format(MainWindow.strDataFormat,
                                        id_num, children[0].Text, children[1].Text, date.DayOfWeek, date.Day,
                                        date.Month, date.Year, h, m, s[9], remind, cb_time2remind.SelectedIndex, isLoop, typeLoop, numLoop);
                            sb.AppendLine(newLine);
                        }
                        else
                        {
                            sb.AppendLine(line);
                        }
                    }
                    File.WriteAllText("data.csv", sb.ToString());
                }
            }
            else
            {
                MessageBox.Show("Bạn cần phải chọn ngày trong tương lai ! \n Hôm nay là : " + DateTime.Today.ToString("dd/MM/yyyy"),
                    "Hey hey ...", MessageBoxButton.OK, MessageBoxImage.Warning);
                date_edit.SelectedDate = DateTime.Today;
            }
        }

        private void txt_edit_tenSK_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txt_edit_tenSK.Text = "";
        }

        private void txt_edit_motaSK_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txt_edit_motaSK.Text = "";
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            isSave = true;
            this.Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            isSave = false;
            this.Close();
        }

        private void chk_loop_Checked(object sender, RoutedEventArgs e)
        {
            grid_loopOption.Visibility = Visibility.Visible;
        }

        private void chk_loop_Unchecked(object sender, RoutedEventArgs e)
        {
            grid_loopOption.Visibility = Visibility.Collapsed;
        }
    }
}

