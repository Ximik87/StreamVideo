using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class CameraInfoParser : ILinkParser
    {
        private readonly IHtmlContentLoader _loader;
        private readonly ILogger<CameraInfoParser> _logger;

        public CameraInfoParser(IHtmlContentLoader loader, ILogger<CameraInfoParser> logger)
        {
            _loader = loader;
            _logger = logger;
        }

        public IEnumerable<CameraInfo> Parse()
        {
            _logger.LogDebug("Parsing html content");

            var cameras = new List<CameraInfo>();
            string htmlContent = _loader.GetHtmlContent();

            int startIndex = htmlContent.IndexOf("<div class=\"row thumbnail");
            var onlyCameraHtmlTags = htmlContent.Substring(startIndex);
            var endIndex = onlyCameraHtmlTags.IndexOf("<div class=\"textcen");
            onlyCameraHtmlTags = onlyCameraHtmlTags.Substring(0, endIndex);
            var doc = XDocument.Parse(onlyCameraHtmlTags);
            var imageTags = doc.Descendants("img");

            _logger.LogDebug($"Parsed {imageTags.Count()} camera sources");

            int number = 1;
            foreach (var tag in imageTags)
            {
                var title = tag.Attribute("title").Value;
                var url = tag.Attribute("src").Value;
                cameras.Add(new CameraInfo
                {
                    Id = number,
                    Title = title,
                    Url = url
                });

                number++;
            }

            return cameras;
        }
    }
}
