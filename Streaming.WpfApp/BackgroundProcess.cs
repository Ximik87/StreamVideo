using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Streaming.Core;
using Streaming.Core.Interfaces;
using Streaming.WpfApp.Interfaces;
using Streaming.WpfApp.Models;
using Streaming.WpfApp.Properties;
using Streaming.WpfApp.ViewModels;

namespace Streaming.WpfApp
{
    class BackgroundProcess : IBackgroundProcess
    {
        private Stream _emptyFrame;
        private readonly ObservableCollection<CameraData> _cameras;
        private readonly List<ISeparateCameraProcess> _consumers;
        private readonly ILinkContainer _linkContainer;

        public BackgroundProcess(IMainWindowViewModel viewModel, ILinkContainer linkContainer)
        {
            _cameras = viewModel.Cameras;
            _linkContainer = linkContainer;
            _consumers = new List<ISeparateCameraProcess>();             
        }

        private void Init()
        {
            _linkContainer.GetContent();
            _emptyFrame = new MemoryStream(Resources.empty);

            foreach (var item in _linkContainer.CameraInfos)
            {
                _cameras.Add(new CameraData
                {
                    Name = item.Name,
                    Url = item.Url,
                    Image = _emptyFrame
                });
            }
        }

        public void Start()
        {
            Init();

            int i = 0;
            foreach (var camera in _cameras)
            {
                IVideoConsumer stub;
                if (i < 5)
                {
                    stub = new VideoConsumer(camera.Url);
                }
                else
                {
                    stub = new VideoConsumerStub(camera.Url);
                }

                var consumer = new SeparateCameraProcess(stub, camera);
                consumer.Start();
                _consumers.Add(consumer);
                i++;
            }
        }
    }
}
