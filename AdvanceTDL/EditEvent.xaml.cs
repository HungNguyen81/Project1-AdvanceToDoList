﻿using System;
//using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using AdvanceTDL;

namespace AdvanceTDL
{
    /// <summary>
    /// Interaction logic for EditEvent.xaml
    /// </summary>
    public partial class EditEvent : Window
    {
        private Button btnSender;
        private string remind;
        public EditEvent()
        {
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
            btnSender = MainWindow.btn_Current_Focus;
            Canvas c = (Canvas)btnSender.Content;
            string[] texts = new string[MainWindow.NUM_ATTR_DATA];

            int k = 0;
            foreach (object o in c.Children)
            {
                TextBlock t = (TextBlock)o;
                texts[k++] = t.Text;
            }
            txt_edit_tenSK.Text = texts[0];
            txt_edit_motaSK.Text = texts[1];
            string[] s = texts[2].Split(',');
            string[] s1 = s[1].Split('/');
            date_edit.SelectedDate = new DateTime(int.Parse(s1[2]), int.Parse(s1[1]), int.Parse(s1[0]));

            string[] gioPhut = texts[3].Split(':');
            int gio = int.Parse(gioPhut[0]);
            int phut = int.Parse(gioPhut[1]);
            cb_gio.SelectedIndex = gio;
            cb_phut.SelectedIndex = phut;
            txb_Id.Text = "ID: " + texts[4];

            if (texts[6] == "1") check_remind.IsChecked = true;
            else check_remind.IsChecked = false;
        }



        private void EditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult re = MessageBox.Show("Bạn muốn đóng hộp thoại này chứ?!", "Xác nhận đóng", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if(re == MessageBoxResult.OK)
            {

            }
            else
            {
                e.Cancel = true;
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
            DateTime date = (DateTime)date_edit.SelectedDate;
            if (DateTime.Compare(date, DateTime.Today) >= 0)
            {
                Canvas c = (Canvas)btnSender.Content;
                TextBlock[] children = new TextBlock[MainWindow.NUM_ATTR_DATA];

                int k = 0;
                foreach (object o in c.Children)
                {
                    children[k++] = (TextBlock)o;
                }

                children[0].Text = txt_edit_tenSK.Text;
                children[1].Text = txt_edit_motaSK.Text;
                children[2].Text = date.DayOfWeek + ", " + date.Day + "/" + date.Month + "/" + date.Year;
                string h = (cb_gio.SelectedIndex < 10) ? "0" + cb_gio.SelectedIndex : "" + cb_gio.SelectedIndex;
                string m = (cb_phut.SelectedIndex < 10) ? "0" + cb_phut.SelectedIndex : "" + cb_phut.SelectedIndex;
                children[3].Text = h + ":" + m;

                int id_num = int.Parse(children[4].Text);

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
                        string[] s = line.Split('\t');
                        if (int.Parse(s[0]) == id_num)
                        {
                            string newLine = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}",
                                        id_num, children[0].Text, children[1].Text, date.DayOfWeek, date.Day,
                                        date.Month, date.Year, date.Hour, date.Minute, s[9], remind);
                            sb.AppendLine(newLine);
                        }
                        else
                        {
                            sb.AppendLine(line);
                        }
                    }

                    File.WriteAllText("data.csv", sb.ToString());
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Bạn cần phải chọn ngày trong tương lai ! \n Hôm nay là : " + DateTime.Today.ToString("dd/MM/yyyy"),
                    "Hey hey ...", MessageBoxButton.OK, MessageBoxImage.Warning);
                date_edit.SelectedDate = DateTime.Today;
            }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

//issues: chua luu su kien dc edit vao file data => OK fixed

//issues: khong kiem tra ngay dc sua co som hon ngay hien tai k => OK fixed