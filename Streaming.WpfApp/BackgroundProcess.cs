using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Streaming.Core;
using Streaming.Core.Interfaces;
using Streaming.WpfApp.Models;
using Streaming.WpfApp.Properties;

namespace Streaming.WpfApp
{
    class BackgroundProcess
    {
        private Stream _emptyFrame;
        private readonly ObservableCollection<CameraData> _cameras;
        private readonly List<ISeparateCameraProcess> _consumers;
        private readonly ILinkContainer _linkContainer;

        public BackgroundProcess(ObservableCollection<CameraData> cameras, ILinkContainer linkContainer)
        {
            _cameras = cameras;
            _linkContainer = linkContainer;
            _consumers = new List<ISeparateCameraProcess>();
            Init();
            Test();
        }

        private void Init()
        {
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

        private void Test()
        {
            foreach (var camera in _cameras)
            {
                IVideoConsumer stub;


                stub = new VideoConsumer(camera.Url);

                var consumer = new SeparateCameraProcess(stub, camera);
                consumer.Start();
                _consumers.Add(consumer);
            }
        }
    }
}
