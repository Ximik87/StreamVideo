using System;
using System.IO;

namespace Streaming.Core
{
    public interface ICameraData
    {
        string Name { get; set; }
        Stream Image { get; set; }
        string Url { get; set; }
    }
}
