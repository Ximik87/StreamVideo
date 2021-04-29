using System;
using System.Collections.Generic;

namespace Streaming.Core.Interfaces
{
    public interface ILinkContainer
    {
        IEnumerable<CameraInfo> CameraInfos { get; }
        void Parse();
    }
}
