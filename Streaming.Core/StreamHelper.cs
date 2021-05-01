using System.IO;

namespace Streaming.Core
{
    public class StreamHelper
    {
        public static Stream GetStream(string path)
        {
            using (var fs = File.OpenRead(path))
            {
                var stream = new MemoryStream();
                fs.CopyTo(stream);
                return stream;
            }
        }
    }
}
