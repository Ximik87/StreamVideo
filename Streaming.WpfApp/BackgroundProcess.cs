using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private readonly ISeparateProcessFactory _factory;

        public BackgroundProcess(
            IMainWindowViewModel viewModel,
            ILinkContainer linkContainer,
            ISeparateProcessFactory factory
            )
        {
            _cameras = viewModel.Cameras;
            _linkContainer = linkContainer;
            _consumers = new List<ISeparateCameraProcess>();
            _factory = factory;
        }

        private void Init()
        {
            _linkContainer.GetContent();
            _emptyFrame = new MemoryStream(Resources.empty);

            foreach (var item in _linkContainer.CameraInfos)
            {
                _cameras.Add(new CameraData
                {
                    Id = item.Id,
                    Title = item.Title,
                    Url = item.Url,
                    Image = _emptyFrame
                });
            }
        }

        public void Start()
        {
            Init();

            foreach (var camera in _cameras)
            {
                var separateProcess = _factory.Create(camera);
                separateProcess.Start();
                _consumers.Add(separateProcess);
            }
        }

        public void Stop()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Stop();
            }
        }
    }
}
