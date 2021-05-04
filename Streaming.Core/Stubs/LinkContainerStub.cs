using System.Collections.Generic;
using Streaming.Core.Interfaces;

namespace Streaming.Core.Stubs
{
    public class LinkContainerStub : ILinkContainer
    {
        private readonly List<CameraInfo> _links = new();
        public IEnumerable<CameraInfo> CameraInfos => _links;

        public void GetContent()
        {
            _links.Add(new CameraInfo
            {
                Title = "firstCamera",
                Url = ""
            });
            _links.Add(new CameraInfo
            {
                Title = "secondCamera",
                Url = ""
            });
            _links.Add(new CameraInfo
            {
                Title = "thirdCamera",
                Url = ""
            });
            _links.Add(new CameraInfo
            {
                Title = "4Camera"
            });
        }
    }
}
