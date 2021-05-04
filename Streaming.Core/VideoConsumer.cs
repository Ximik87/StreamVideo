using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class VideoConsumer : IVideoConsumer
    {
        private readonly ILogger<VideoConsumer> _logger;
        private readonly string _url;
        private readonly int _cameraId;
        private readonly CancellationTokenSource _cts = new();
        public event NewFrameEventHandler NewFrame;

        public VideoConsumer(ILogger<VideoConsumer> logger, ICameraData camera)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera));
            }

            _url = camera.Url;
            _cameraId = camera.Id;
        }

        public void Start()
        {
            Task.Factory.StartNew(
                () => DoWork(_cts.Token),
                _cts.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        private async Task DoWork(CancellationToken token)
        {
            var compensator = new DelayCompensator();
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();

            string contentType = response.Headers["Content-Type"];
            string boundryKey = "boundary=";
            string boundary = contentType.Substring(contentType.IndexOf(boundryKey) + boundryKey.Length);

            Stream stream = response.GetResponseStream();
            string headerName = "Content-Length:";
         
            while (true)
            {
                token.ThrowIfCancellationRequested();

                // read data line
                string line = ReadToLine(stream);

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
                        SetStreamToNextPosition(stream, boundary);
                        _logger.LogDebug("For cameraId: {0} invalid format jpeg, delay: {1}", _cameraId, compensator.Delay);
                        compensator.SetFail();
                    }
                    else
                    {
                        NewFrame?.Invoke(this, new NewFrameEventArgs(new MemoryStream(imageToBytes)));
                    }

                    await Task.Delay(compensator.Delay, token);
                }
            }
        }

        private string ReadToLine(Stream stream)
        {
            var sb = new StringBuilder();
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
            _logger.LogTrace(line);          

            return line;
        }

        private void SetStreamToNextPosition(Stream stream, string boundary)
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
        }
    }
}
