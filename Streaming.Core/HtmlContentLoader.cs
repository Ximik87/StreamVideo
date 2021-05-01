using System;
using System.IO;
using System.Net;
using System.Text;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class HtmlContentLoader : IHtmlContentLoader
    {
        private readonly string _url = "http://www.insecam.org/en/bytype/Axis/";

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
            catch (Exception)
            {
                // todo log this
            }

            return htmlContent;
        }
    }
}
