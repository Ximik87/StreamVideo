using System.Collections.Generic;

namespace Streaming.Core.Interfaces
{
    public interface ILinkParser
    {
        IEnumerable<CameraInfo> Parse();
    }
}
