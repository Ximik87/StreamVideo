using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class VideoConsumer : IVideoConsumer
    {
        private readonly ILogger<VideoConsumer> _logger;
        private bool _isWorking = false;
        private readonly string _url;
        private readonly int _cameraId;
        public event NewFrameEventHandler NewFrame;

        public VideoConsumer(ICameraData camera, ILogger<VideoConsumer> logger)
        {
            _url = camera.Url;
            _logger = logger;
            _cameraId = camera.Id;
        }

        public void Start()
        {
            _isWorking = true;
            Task.Factory.StartNew(() => DoWork(), TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _isWorking = false;
        }

        private async Task DoWork()
        {
            // todo cancelation toekn ?
            var compensator = new DelayCompensator();
            //Create an HTTP request, as long as the request does not end, MJPEG server will always send real-time image content to the response body of the request
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            string contentType = response.Headers["Content-Type"];
            string boundryKey = "boundary=";
            string boundary = contentType.Substring(contentType.IndexOf(boundryKey) + boundryKey.Length);

            //Get response volume flow
            Stream stream = response.GetResponseStream();
            string headerName = "Content-Length:";
            //Temporary storage of string data
            var sb = new StringBuilder();
            while (_isWorking)
            {
                // read data line
                while (true)
                {
                    char c = (char)stream.ReadByte();
                    //_logger.LogTrace(c);
                    if (c == '\n')
                    {
                        break;
                    }
                    sb.Append(c);
                }
                string line = sb.ToString();
                Console.WriteLine(line);
                sb.Remove(0, sb.Length);
                // find header
                int i = line.IndexOf(headerName);
                if (i != -1)
                {
                    // get size jpg
                    // for example: Content-Length: 289476
                    int imageLength = Convert.ToInt32(line.Substring(i + headerName.Length).Trim());
                    _logger.LogTrace(line);

                    // skip end line - \r\n
                    stream.Read(new byte[2], 0, 2);

                    // read data jpeg                  
                    var imageToBytes = new byte[imageLength];
                    stream.Read(imageToBytes, 0, imageLength);

                    if (imageToBytes[imageLength - 2].ToString("X") != "FF"
                        && imageToBytes[imageLength - 1].ToString("X") != "D9")
                    {
                        char l = '0';
                        while (true)
                        {
                            char c = (char)stream.ReadByte();
                            // read stream to next picture
                            if (l == boundary[0] && c == boundary[1])
                            {
                                break;
                            }
                            l = c;
                        }
                        _logger.LogDebug("For cameraId: {0} invalid jpeg, delay: {1}", _cameraId, compensator.Delay);
                        compensator.SetFail();
                    }
                    else
                    {
                        NewFrame?.Invoke(this, new NewFrameEventArgs(new MemoryStream(imageToBytes)));
                    }

                    await Task.Delay(compensator.Delay);
                }
            }
        }
    }
}
