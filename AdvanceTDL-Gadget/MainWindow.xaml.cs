using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace AdvanceTDL_Gadget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window w;
        public MainWindow(StackPanel pnl, Window w)
        {
            this.w = w;
            InitializeComponent();
            //MessageBox.Show(pnl.Children.Count + "");
            foreach(object o in pnl.Children)
            {
                Border b = (Border)o;
                string stackPnlXaml = XamlWriter.Save(b);

                StringReader stringReader = new StringReader(stackPnlXaml);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                Border br = (Border)XamlReader.Load(xmlReader);
                pnlSuKien_gadget.Children.Add(br);
            }
            
        }
        
        private void btn_launch_app_Click(object sender, RoutedEventArgs e)
        {
            w.Show();            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("gadgetState.csv", "0");
            this.Close();
        }
    }
}
