using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streaming.Core.Interfaces;
using Streaming.Core.Properties;

namespace Streaming.Core
{
    public class VideoConsumerStub : IVideoConsumer
    {
        private bool _isWorking = false;
        private Stream _jpegFrame1;
        private Stream _jpegFrame2;
        public event NewFrameEventHandler NewFrame;
        private int _count = 0;
        private int _delay = 1;

        public VideoConsumerStub(string url)
        {
            _delay = new Random(GetHashCode()).Next(20, 1000);
            Init();
        }

        private void Init()
        {
            _jpegFrame1 = new MemoryStream(Resources.frame1); // StreamHelper.GetStream(@"D:\qqqq.jpg"); 
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
