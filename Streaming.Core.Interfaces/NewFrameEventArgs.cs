using System;
using System.IO;

namespace Streaming.Core
{
    public delegate void NewFrameEventHandler(object sender, NewFrameEventArgs eventArgs);

    public class NewFrameEventArgs : EventArgs
    {
        public Stream Frame { get; private set; }
        public NewFrameEventArgs(Stream stream)
        {
            Frame = stream;
        }
    }
}
