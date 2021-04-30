using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class LinkContainer : ILinkContainer
    {

        public IEnumerable<CameraInfo> CameraInfos => linkParser.CameraInfos;
        private ILinkParser linkParser;

        public LinkContainer(ILinkParser parser)
        {
            linkParser = parser;
        }

        public void GetContent()
        {
            linkParser.Parse();
        }
    }
}
