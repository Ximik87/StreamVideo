using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace TestGetData
{
    class Program
    {
        // http://www.insecam.org/en/bytype/Axis/
        //   private const string cameraUrl = "http://3.16.40.200/mjpg/video.mjpg";
        // private const string cameraUrl = "http://89.245.165.210:8888/mjpg/video.mjpg";
         private const string cameraUrl = "http://77.22.100.19:88/mjpg/video.mjpg";
    //    private const string cameraUrl = "http://109.229.70.17:8080/?action=stream";

        static void Main(string[] args)
        {

            NewTest();
            Console.ReadKey();
        }

        private static void NewTest()
        {
            var compensator = new DelayCompensator();
         
            var request = (HttpWebRequest)WebRequest.Create(cameraUrl);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            // прочитаем разделитель
            string contentType = response.Headers["Content-Type"];
            string boundryKey = "boundary=";
            string boundary = contentType.Substring(contentType.IndexOf(boundryKey) + boundryKey.Length);
          
            Stream stream = response.GetResponseStream();
            string headerName = "Content-Length:";
           
            StringBuilder sb = new StringBuilder();        
            while (true)
            {
                //читаем данные построчно
                while (true)
                {
                    char c = (char)stream.ReadByte();
                    //Console.Write(c);
                  
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
                    var imageToBytes = new byte[imageLength];               
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

                    // for debug
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
                            // проматываем стрим до следующего фрейма
                            if (l == boundary[0] && c == boundary[1])
                            {
                                break;
                            }
                            l = c;
                        }
                        Console.WriteLine("--invalid jpeg, delay: {0}", compensator.Delay);
                        compensator.SetFail();
                    }
                    else
                    {
                        Console.WriteLine("complete jpeg");
                    }

                    Thread.Sleep(compensator.Delay);
                }
            }
        }
    }

    public class DelayCompensator
    {
        private int _currentDelay = 100;
        private int _retryCount = 0;
        public int Delay => _currentDelay;

        public void SetFail()
        {
            if (_retryCount >= 3)
            {
                _currentDelay += 50;
                _retryCount = 0;
            }
            else
            {
                _retryCount++;
            }

        }
    }
}
