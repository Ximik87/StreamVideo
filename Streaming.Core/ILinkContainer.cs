using System;
using System.Collections.Generic;

namespace Streaming.Core
{
    public interface ILinkContainer
    {
        IEnumerable<string> CameraLinks { get; }
        void Parse();
    }
}
