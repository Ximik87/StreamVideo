using System;

namespace Streaming.Core.Interfaces
{
    public interface IVideoConsumer
    {
        event NewFrameEventHandler NewFrame;
        void Start();
        void Stop();
    }
}
