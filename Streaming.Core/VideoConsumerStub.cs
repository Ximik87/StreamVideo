using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streaming.Core
{
    public class VideoConsumerStub : IVideoConsumer
    {
        private bool _isWorking = false;
        private Stream _jpegFrame;
        private Stream _jpegFrame2;
        public event NewFrameEventHandler NewFrame;
        private int _count = 0;

        public VideoConsumerStub()
        {
            Init();
        }

        private void Init()
        {
            _jpegFrame = File.OpenRead(@"D:\qqqq.jpg");
            _jpegFrame2 = File.OpenRead(@"D:\qqqq2.jpg");
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
                    NewFrame?.Invoke(this, new NewFrameEventArgs(_jpegFrame));
                }
                else
                {
                    NewFrame?.Invoke(this, new NewFrameEventArgs(_jpegFrame2));
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
