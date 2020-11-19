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
using System.Windows.Shapes;

namespace AdvanceTDL
{
    /// <summary>
    /// Interaction logic for RemindDialog.xaml
    /// </summary>
    public partial class RemindDialog : Window
    {
        public RemindDialog(infoSK inf)
        {
            InitializeComponent();
            tx_tenSK.Text = "Sự kiện: " + inf.TenSK;
            tx_thoigian.Text = inf.getTime();
            tx_vitri.Text = "Vị trí: " + inf.MotaSK;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.remindResult = true;
            this.Close();
        }
    }
}
