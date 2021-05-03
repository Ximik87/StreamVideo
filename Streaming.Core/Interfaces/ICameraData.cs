using System.IO;

namespace Streaming.Core.Interfaces
{
    public interface ICameraData
    {
        string Title { get; set; }
        Stream Image { get; set; }
        string Url { get; set; }
    }
}
