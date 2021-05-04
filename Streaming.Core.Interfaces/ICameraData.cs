using System.IO;

namespace Streaming.Core.Interfaces
{
    public interface ICameraData
    {
        int Id { get; set; }
        string Title { get; set; }
        Stream Image { get; set; }
        string Url { get; set; }
    }
}
