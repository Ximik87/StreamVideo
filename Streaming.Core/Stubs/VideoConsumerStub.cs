using System;
using System.IO;
using System.Threading.Tasks;
using Streaming.Core.Interfaces;
using Streaming.Core.Properties;

namespace Streaming.Core.Stubs
{
    public class VideoConsumerStub : IVideoConsumer
    {
        private bool _isWorking = false;
        private Stream _jpegFrame1;
        private Stream _jpegFrame2;
        private int _count = 0;
        private readonly int _delay = 1;
        public event NewFrameEventHandler NewFrame;

        public VideoConsumerStub()
        {
            _delay = new Random(GetHashCode()).Next(20, 1000);
            Init();
        }

        private void Init()
        {
            _jpegFrame1 = new MemoryStream(Resources.frame1);
            _jpegFrame2 = new MemoryStream(Resources.frame2);
        }

        public void Start()
        {
            _isWorking = true;
            Task.Factory.StartNew(() => DoWork(), TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _isWorking = false;
        }

        private async Task DoWork()
        {
            while (_isWorking)
            {
                System.Threading.Interlocked.Increment(ref _count);

                if (_count % 2 == 0)
                {
                    NewFrame?.Invoke(this, new NewFrameEventArgs(_jpegFrame1));
                }
                else
                {
                    NewFrame?.Invoke(this, new NewFrameEventArgs(_jpegFrame2));
                }

                await Task.Delay(TimeSpan.FromMilliseconds(_delay));
            }
        }
    }
}
