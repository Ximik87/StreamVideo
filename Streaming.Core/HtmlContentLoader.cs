using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class HtmlContentLoader : IHtmlContentLoader
    {
        private readonly string _url = "http://www.insecam.org/en/bytype/Axis/";
        private readonly ILogger<HtmlContentLoader> _logger;

        public HtmlContentLoader(ILogger<HtmlContentLoader> logger)
        {
            _logger = logger;
        }

        public string GetHtmlContent()
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
            catch (Exception ex)
            {
                _logger.LogError("Exception in {0}", ex.Message);
            }

            return htmlContent;
        }
    }
}
