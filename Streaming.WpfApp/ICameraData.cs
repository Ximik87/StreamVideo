using System;
using System.IO;

namespace Streaming.WpfApp
{
    public interface ICameraData
    {
        string Name { get; set; }
        Stream Image { get; set; }
        string Url { get; set; }
    }
}
