using System;
using Streaming.Core;

namespace Streaming.Core
{
    public class SeparateCameraProcess : ISeparateCameraProcess
    {
        private IVideoConsumer _consumer;
        private readonly ICameraData _camera;

        public SeparateCameraProcess(IVideoConsumer consumer, ICameraData camera)
        {
            _consumer = consumer;
            _camera = camera;
        }

        public void Start()
        {
            _consumer.NewFrame += SetNewFrame;
            _consumer.Start();
        }

        private void SetNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            _camera.Image = eventArgs.Frame;
        }

        public void Stop()
        {
            _consumer.Stop();
            _consumer.NewFrame -= SetNewFrame;
        }
    }
}
