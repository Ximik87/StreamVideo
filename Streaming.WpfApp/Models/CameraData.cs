using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Streaming.Core.Interfaces;

namespace Streaming.WpfApp.Models
{
    public class CameraData : INotifyPropertyChanged, ICameraData
    {
        private Stream _image;
        private string _title;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                OnPropertyChanged(nameof(Title));
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
        public int Id { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
