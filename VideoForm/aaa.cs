using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.IO;

namespace VideoForm
{
    public delegate void NewFrameEventHandler(object sender, NewFrameEventArgs eventArgs);
    public delegate void NewTickEventHandler(object sender, NewTickEventArgs eventArgs);

    public class NewFrameEventArgs : EventArgs
    {
        private readonly Stream frame;
        public NewFrameEventArgs(Stream frame)
        {
            this.frame = frame;
        }

        public Stream Frame => frame;
    }

    public class NewTickEventArgs : EventArgs
    {
        private readonly int _tick;
        public int Tick => _tick;
        public NewTickEventArgs(int frame)
        {
            _tick = frame;
        }

    }

    class aaa
    {
        //   private const string cameraUrl = "http://3.16.40.200/mjpg/video.mjpg";
        private const string cameraUrl = "http://89.245.165.210:8888/mjpg/video.mjpg";

        public event NewFrameEventHandler NewFrame;
        private Task _task;

        public void Start()
        {
            _task = Task.Factory.StartNew(() => NewTest(), TaskCreationOptions.LongRunning);
        }

        private async Task NewTest()
        {

            //Create an HTTP request, as long as the request does not end, MJPEG server will always send real-time image content to the response body of the request
            var hwRequest = (HttpWebRequest)WebRequest.Create(cameraUrl);
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
            while (true)
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

                    while (total < imageLength)
                    {
                        if (imageLength - total > readSize)
                        {
                            stream.Read(imageToBytes, total, readSize);
                        }
                        else
                        {
                            stream.Read(imageToBytes, total, imageLength - total);
                        }
                        total += readSize;
                    }


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

                    //using (FileStream fs = File.Create(@"c:\1.bin"))
                    //{
                    //    fs.Write(imageToBytes, 0, imageLength);
                    //}

                    if (imageToBytes[imageLength - 2].ToString("X") != "FF" && imageToBytes[imageLength - 1].ToString("X") != "D9")
                    {
                        Console.WriteLine("--incomplete jpeg");
                    }
                    else
                    {
                        if (NewFrame != null)
                        {
                            //Bitmap bitmap = (Bitmap)Bitmap.FromStream(new MemoryStream(imageToBytes));
                            // notify client
                            NewFrame(this, new NewFrameEventArgs(new MemoryStream(imageToBytes)));
                            // release the image
                            //bitmap.Dispose();
                        }
                        Console.WriteLine("complete jpeg");
                    }

                    await Task.Delay(200);
                }
            }
        }
    }

    class TestTick
    {
        public event NewTickEventHandler NewTick;
        private Task _task;
        private int _tick;

        public void Start()
        {
            _task = Task.Factory.StartNew(() => NewTest(), TaskCreationOptions.LongRunning);

        }

        private async Task NewTest()
        {
            while (true)
            {
                _tick++;
                NewTick?.Invoke(this, new NewTickEventArgs(_tick));
                await Task.Delay(100);
            }
        }
    }
}
