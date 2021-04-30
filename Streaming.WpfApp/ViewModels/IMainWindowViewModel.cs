using System;
using System.Collections.ObjectModel;
using Streaming.WpfApp.Models;

namespace Streaming.WpfApp.ViewModels
{
    public interface IMainWindowViewModel
    {
        ObservableCollection<CameraData> Cameras { get; set; }
    }
}
