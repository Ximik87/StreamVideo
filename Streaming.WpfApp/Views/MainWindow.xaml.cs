using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Streaming.WpfApp.Interfaces;
using Streaming.WpfApp.ViewModels;

namespace Streaming.WpfApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IBackgroundProcess _background;
        public MainWindow(IMainWindowViewModel viewModel, IBackgroundProcess background)
        {
            InitializeComponent();
            DataContext = viewModel;
            _background = background;
            _background.Start();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _background.Stop();
        }
    }
}
