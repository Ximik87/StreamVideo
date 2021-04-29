using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.WpfApp
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CameraData> Cameras { get; set; }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            var stream = File.OpenRead(@"d:\qqqq2.jpg");
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);

            Cameras = new ObservableCollection<CameraData>
            {
                new CameraData { Name = "firstCamera", Image = bytes },
                new CameraData { Name = "secondCamera", Image = bytes },
                new CameraData { Name = "thirdCamera", Image = bytes },
                new CameraData { Name = "4Camera", Image = bytes },
            };
        }
    }

    public class CameraData : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
