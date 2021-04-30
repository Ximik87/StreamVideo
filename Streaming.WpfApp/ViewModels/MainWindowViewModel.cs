using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Streaming.WpfApp.Models;

namespace Streaming.WpfApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CameraData> Cameras { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            //var stream = File.OpenRead(@"d:\qqqq2.jpg");
            //var bytes = new byte[stream.Length];
            //stream.Read(bytes, 0, (int)stream.Length);

            Cameras = new ObservableCollection<CameraData>();
        }
    }
}
