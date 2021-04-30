using System;
using System.Collections.Generic;

namespace Streaming.Core.Interfaces
{
    public interface ILinkParser
    {
        IEnumerable<CameraInfo> CameraInfos { get; }
        void Parse();
    }
}
