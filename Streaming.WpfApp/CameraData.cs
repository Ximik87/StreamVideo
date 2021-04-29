using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Streaming.Core;

namespace Streaming.WpfApp
{
    public class CameraData : INotifyPropertyChanged, ICameraData
    {
        private Stream _image;
        private string _name;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Stream Image
        {
            get => _image;
            set
            {
                if (_image == value)
                {
                    return;
                }

                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public string Url { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
