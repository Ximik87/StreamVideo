using System;
using System.Collections.Generic;

namespace Streaming.Core
{
    public interface ILinkParser
    {
        IEnumerable<CameraInfo> CameraInfos { get; }
        void Parse();
    }
}
