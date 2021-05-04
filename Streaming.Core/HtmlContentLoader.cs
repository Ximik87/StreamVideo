using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Streaming.Core.Exceptions;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class HtmlContentLoader : IHtmlContentLoader
    {
        private const string _url = "http://www.insecam.org/en/bytype/Axis/";
        private readonly ILogger<HtmlContentLoader> _logger;

        public HtmlContentLoader(ILogger<HtmlContentLoader> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetHtmlContent()
        {
            string htmlContent = string.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(_url);
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();
                var receiveStream = response.GetResponseStream();
                var readStream = new StreamReader(receiveStream, Encoding.UTF8);

                htmlContent = readStream.ReadToEnd();

            }
            catch (WebException ex)
            {
                _logger.LogError("Exception in {0}", ex.Message);
                throw new NotAvailableWebSourceException();
            }

            return htmlContent;
        }
    }
}
