using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AdvanceTDL
{
    public partial class MainWindow
    {
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
            foreach (object o in c.Children)
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
                TextBlock[] id = new TextBlock[(int)myConsts.NUM_ATTR_DATA - 1];
                int k = 0;
                foreach (object o in c.Children)
                {
                    if (k < (int)myConsts.NUM_ATTR_DATA - 1) id[k++] = (TextBlock)o;
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
            foreach (object o in c.Children)
            {
                if (i < (int)myConsts.NUM_ATTR_DATA - 1)
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

        private void chk_Gadget_Checked(object sender, RoutedEventArgs e)
        {
            mGadget = new AdvanceTDL_Gadget.MainWindow(pnlSuKien, this);
            mGadget.Show();
            File.WriteAllText("gadgetState.csv", "1");
        }

        private void chk_Gadget_Unchecked(object sender, RoutedEventArgs e)
        {
            mGadget.Hide();
            File.WriteAllText("gadgetState.csv", "0");
        }
        #endregion
    }
}
