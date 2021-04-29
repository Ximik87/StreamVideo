using System;

namespace Streaming.Core
{
    public interface IVideoConsumer
    {
        event NewFrameEventHandler NewFrame;
        void Start();
        void Stop();
    }
}
