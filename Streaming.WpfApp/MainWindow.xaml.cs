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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Streaming.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundProcess _background;
        public MainWindow()
        {
            InitializeComponent();
            var vm =  new MainWindowViewModel();
            DataContext = vm;
            _background = new BackgroundProcess(vm.Cameras);           
           
        }

        //private void testImg()
        //{
        //    var stream = File.OpenRead(@"d:\empty.jpg");  //File.OpenRead(@"d:\qqqq2.jpg");
        //    var bitMap = new BitmapImage();
        //    bitMap.BeginInit();
        //    bitMap.StreamSource = stream;
        //    bitMap.EndInit();
        //    imgTest.Source = bitMap;          
        //}

    }
}
