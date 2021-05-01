using System.Collections.Generic;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class LinkContainerStub : ILinkContainer
    {
        private readonly List<CameraInfo> _links = new List<CameraInfo>();
        public IEnumerable<CameraInfo> CameraInfos => _links;

        public void GetContent()
        {
            _links.Add(new CameraInfo
            {
                Name = "firstCamera",
                Url = ""
            });
            _links.Add(new CameraInfo
            {
                Name = "secondCamera",
                Url = ""
            });
            _links.Add(new CameraInfo
            {
                Name = "thirdCamera",
                Url = ""
            });
            _links.Add(new CameraInfo
            {
                Name = "4Camera"
            });
        }
    }
}
