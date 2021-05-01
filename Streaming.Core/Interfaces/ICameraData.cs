using System.IO;

namespace Streaming.Core.Interfaces
{
    public interface ICameraData
    {
        string Name { get; set; }
        Stream Image { get; set; }
        string Url { get; set; }
    }
}
