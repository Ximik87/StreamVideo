using System;
using System.Collections.Generic;

namespace Streaming.Core
{
    public class LinkContainerStub : ILinkContainer
    {
        private List<string> _links = new List<string>();
        public IEnumerable<string> CameraLinks => _links;

        public void Parse()
        {
            _links.Add("http://3.16.40.200/mjpg/video.mjpg");
        }
    }
}
