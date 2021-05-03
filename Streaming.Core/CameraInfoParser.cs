using System.Collections.Generic;
using System.Xml.Linq;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class CameraInfoParser : ILinkParser
    {
        private readonly IHtmlContentLoader _loader;

        public CameraInfoParser(IHtmlContentLoader loader)
        {
            _loader = loader;
        }

        public IEnumerable<CameraInfo> Parse()
        {
            var cameras = new List<CameraInfo>();
            string htmlContent = _loader.GetHtmlContent();

            int startIndex = htmlContent.IndexOf("<div class=\"row thumbnail");
            var onlyCameraHtmlTags = htmlContent.Substring(startIndex);
            var endIndex = onlyCameraHtmlTags.IndexOf("<div class=\"textcen");
            onlyCameraHtmlTags = onlyCameraHtmlTags.Substring(0, endIndex);
            var doc = XDocument.Parse(onlyCameraHtmlTags);
            var imageTags = doc.Descendants("img");

            foreach (var tag in imageTags)
            {
                var name = tag.Attribute("title").Value;
                var url = tag.Attribute("src").Value;
                cameras.Add(new CameraInfo
                {
                    Title = name,
                    Url = url
                });
            }

            return cameras;
        }
    }
}
