using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class CameraInfoParser : ILinkParser
    {
        private readonly string _url = "http://www.insecam.org/en/bytype/Axis/";
        private readonly List<CameraInfo> _cameras = new List<CameraInfo>();
        public IEnumerable<CameraInfo> CameraInfos { get => _cameras; }


        public void Parse()
        {
            // todo refactor this
            string htmlContent = GetHtmlContent();

            int startIndex = htmlContent.IndexOf("<div class=\"row thumbnail");
            var onlyCameraHtmlTags = htmlContent.Substring(startIndex);
            var endIndex = onlyCameraHtmlTags.IndexOf("<div class=\"textcen");
            onlyCameraHtmlTags = onlyCameraHtmlTags.Substring(0, endIndex);
            var doc = XDocument.Parse(onlyCameraHtmlTags);
            var imageTag = doc.Descendants("img");

            foreach (var item in imageTag)
            {
                var name = item.Attribute("title").Value;
                var url = item.Attribute("src").Value;
                _cameras.Add(new CameraInfo
                {
                    Name = name,
                    Url = url
                });
            }

        }

        private string GetHtmlContent()
        {
            string htmlContent = string.Empty;
            try
            {
                
                var hwRequest = (HttpWebRequest)WebRequest.Create(_url);
                hwRequest.Method = "GET";
                var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                var receiveStream = hwResponse.GetResponseStream();
                var readStream = new StreamReader(receiveStream, Encoding.UTF8);

                htmlContent = readStream.ReadToEnd();

            }
            catch (Exception)
            {
                // todo log this
            }

            return htmlContent;
        }
    }
}
