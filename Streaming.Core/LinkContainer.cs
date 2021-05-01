using System.Collections.Generic;
using System.Linq;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class LinkContainer : ILinkContainer
    {
        private List<CameraInfo> _cameras;
        private readonly ILinkParser linkParser;
        public IEnumerable<CameraInfo> CameraInfos => _cameras;

        public LinkContainer(ILinkParser parser)
        {
            linkParser = parser;
            _cameras = new List<CameraInfo>();
        }

        public void GetContent()
        {
            _cameras = linkParser.Parse().ToList();
        }
    }
}
