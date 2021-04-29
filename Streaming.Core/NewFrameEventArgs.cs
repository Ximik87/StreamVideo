using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
