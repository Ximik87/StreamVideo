using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Streaming.Core.Interfaces;

namespace Streaming.Core
{
    public class VideoConsumer : IVideoConsumer
    {
        private bool _isWorking = false;
        private readonly string _url;
        public event NewFrameEventHandler NewFrame;

        public VideoConsumer(string url)
        {
            _url = url;
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
            var hwRequest = (HttpWebRequest)WebRequest.Create(_url);
            hwRequest.Method = "GET";
            var hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            //Read the separator of each image specified by boundary, DroidCam is: - dcmjpeg
            string contentType = hwResponse.Headers["Content-Type"];
            string boundryKey = "boundary=";
            string boundary = contentType.Substring(contentType.IndexOf(boundryKey) + boundryKey.Length);

            //Get response volume flow
            Stream stream = hwResponse.GetResponseStream();
            string headerName = "Content-Length:";
            //Temporary storage of string data
            StringBuilder sb = new StringBuilder();
            int readSize = 1024;
            while (_isWorking)
            {
                //читаем данные построчно
                while (true)
                {
                    char c = (char)stream.ReadByte();
                    //Console.Write(c);
                    //log.Write(c);
                    if (c == '\n')
                    {
                        break;
                    }
                    sb.Append(c);
                }
                string line = sb.ToString();
                Console.WriteLine(line);
                sb.Remove(0, sb.Length);
                // ищем header
                int i = line.IndexOf(headerName);
                if (i != -1)
                {
                    // получим размер jpg
                    // строка для примера: Content-Length: 289476
                    int imageLength = Convert.ToInt32(line.Substring(i + headerName.Length).Trim());
                    //Console.WriteLine(line);

                    // скрипаем символы конеца строки  - \r\n
                    stream.Read(new byte[2], 0, 2);

                    // читаем данные jpeg
                    int total = 0;
                    var imageToBytes = new byte[imageLength];

                    //while (total < imageLength)
                    //{
                    //    if (imageLength - total > readSize)
                    //    {
                    //        stream.Read(imageToBytes, total, readSize);
                    //    }
                    //    else
                    //    {
                    //        stream.Read(imageToBytes, total, imageLength - total);
                    //    }
                    //    total += readSize;
                    //}

                    // alg #2
                    stream.Read(imageToBytes, 0, imageLength);


                    // показать хедер и конец
                    // JPEG The header of the file is: FF D8 FF
                    // tail FF D9
                    //Console.WriteLine("file header: {0}{1}{2}",
                    //    imageToBytes[0].ToString("X"),
                    //    imageToBytes[1].ToString("X"),
                    //    imageToBytes[2].ToString("X"));
                    //Console.WriteLine("tail: {0}{1}",
                    //    imageToBytes[imageLength - 2].ToString("X"),
                    //    imageToBytes[imageLength - 1].ToString("X"));

                    //using (FileStream fs = File.Create(@"c:\example.jpg"))
                    //{
                    //    fs.Write(imageToBytes, 0, imageLength);
                    //}

                    if (imageToBytes[imageLength - 2].ToString("X") != "FF" && imageToBytes[imageLength - 1].ToString("X") != "D9")
                    {
                        char l = '0';
                        while (true)
                        {
                            char c = (char)stream.ReadByte();
                            //Here, only the first two characters in dcmjpeg are judged. When two consecutive characters in the read stream are, it means that the stream has read to the beginning of the next picture
                            if (l == boundary[0] && c == boundary[1])
                            {
                                break;
                            }
                            l = c;
                        }
                        Console.WriteLine("--incomplete jpeg, delay: {0}", compensator.Delay);
                        compensator.SetFail();
                    }
                    else
                    {
                        NewFrame?.Invoke(this, new NewFrameEventArgs(new MemoryStream(imageToBytes)));
                        Console.WriteLine("complete jpeg");
                    }

                    await Task.Delay(compensator.Delay);
                }
            }
        }
    }
}
