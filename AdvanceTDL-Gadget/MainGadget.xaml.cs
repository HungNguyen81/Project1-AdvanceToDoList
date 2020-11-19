using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace AdvanceTDL_Gadget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainGadget : Window
    {
        private Window w;
        public MainGadget(StackPanel pnl, Window w)
        {
            this.w = w;
            InitializeComponent();
            this.Left = SystemParameters.VirtualScreenWidth - 550;
            this.Top = SystemParameters.VirtualScreenHeight/2 - 320;

            Update(pnl);
            foreach(Border b in pnl.Children)
            {
                Button btn = (Button)b.Child;
                btn.MouseDoubleClick += new MouseButtonEventHandler(OnMouseDoubleClick);
            }
            //w.Activate();
        }
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            w.Show();
            w.Activate();
        }

        private void btn_launch_app_Click(object sender, RoutedEventArgs e)
        {            
            w.Show();
            w.WindowState = WindowState.Normal;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("gadgetState.csv", "0");
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void Update(StackPanel pnl)
        {
            pnlSuKien_gadget.Children.Clear();            
            foreach (object o in pnl.Children)
            {
                Border b = (Border)o;
                string stackPnlXaml = XamlWriter.Save(b);

                StringReader stringReader = new StringReader(stackPnlXaml);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                Border br = (Border)XamlReader.Load(xmlReader);
                pnlSuKien_gadget.Children.Add(br);
            }
        }
    }
}
