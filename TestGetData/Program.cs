using AForge.Video;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestGetData
{
    class Program
    {
          private const string cameraUrl = "http://3.16.40.200/mjpg/video.mjpg";
        //private const string cameraUrl = "http://89.245.165.210:8888/mjpg/video.mjpg";

        static void Main(string[] args)
        {

            NewTest();
            Console.ReadKey();
        }

        private static void NewTest()
        {
            var log = new Logger();
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

                    // alg #2
                    //stream.Read(imageToBytes, 0, imageLength);


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

                    using (FileStream fs = File.Create(@"c:\1.bin"))
                    {
                        fs.Write(imageToBytes, 0, imageLength);
                    }

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
                        Console.WriteLine("--incomplete jpeg");
                    }
                    else
                    {
                        Console.WriteLine("complete jpeg");
                    }

                    Thread.Sleep(800);
                }
            }
        }

        private static void SampleMethod()
        {
            //Create an HTTP request, as long as the request does not end, MJPEG server will always send real-time image content to the response body of the request
            HttpWebRequest hwRequest = (HttpWebRequest)WebRequest.Create(cameraUrl);
            hwRequest.Method = "GET";
            HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            //Read the separator of each image specified by boundary, DroidCam is: - dcmjpeg
            string contentType = hwResponse.Headers["Content-Type"];
            string boundryKey = "boundary=";
            string boundary = contentType.Substring(contentType.IndexOf(boundryKey) + boundryKey.Length);

            //Get response volume flow
            Stream stream = hwResponse.GetResponseStream();
            string headerName = "Content-Length:";
            //Temporary storage of string data
            StringBuilder sb = new StringBuilder();
            int len = 1024;
            while (true)
            {
                //Read a line of data
                while (true)
                {
                    char c = (char)stream.ReadByte();
                    Console.Write(c);
                    if (c == '\n')
                    {
                        break;
                    }
                    sb.Append(c);
                }
                string line = sb.ToString();
                sb.Remove(0, sb.Length);
                //Whether the current line contains content length:
                int i = line.IndexOf(headerName);
                if (i != -1)
                {
                    //Before each picture, there is a brief introduction to the picture (picture type and length). Here, we only care about the value after the length (content length:), which is used for subsequent reading of the picture
                    int imageFileLength = Convert.ToInt32(line.Substring(i + headerName.Length).Trim());
                    //Content-Length:xxx  After that, there will be a / r/n newline character, which will be the real image data
                    //Skip / r/n here
                    stream.Read(new byte[2], 0, 2);
                    //Start to read the image data. imageFileLength is the length after the content length: read
                    byte[] imageFileBytes = new byte[imageFileLength];
                    stream.Read(imageFileBytes, 0, imageFileBytes.Length);
                    //JPEG The header of the file is: FF D8 FF ，The end of the file is: FF D9，very important，It's better to print when debugging, so as to distinguish whether the read-in data is exactly the same as all the contents of the picture
                    //Console.WriteLine("file header): + imagefilebytes [0]. ToString (" X ") +" + imagefilebytes [1]. ToString ("X") + "+ imagefilebytes [2]. ToString (" X ") +" + imagefilebytes [3]. ToString ("X") + "+ imagefilebytes [4]. ToString (" X "))));
                    //Console.WriteLine (end of file: + imagefilebytes [imagefilelength - 2]. ToString ("X") + "+ imagefilebytes [imagefilelength - 1]. ToString (" X ")));
                    //If the file read in is incomplete, the bigger the picture is, the faster the program cycle read speed is, and the more likely it is to lead to incomplete file read. If there is a good solution, I hope you can give me some advice. Thank you very much!
                    //Is the end of the file FF D9
                    if (imageFileBytes[imageFileLength - 2].ToString("X") != "FF" && imageFileBytes[imageFileLength - 1].ToString("X") != "D9")
                    {
                        //If the content of the read file is incomplete, skip the second file and let the stream position jump to the beginning of the next picture
                        //Console.WriteLine (start correction...);
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
                    }
                    else
                    {
                        //Read the picture successfully!
                        //accessImageHandler is an Action used to write pictures to PictureBox control in real time
                        accessImageHandler(imageFileBytes);
                    }
                    //If you sleep several tens of milliseconds properly here, it will reduce the situation of incomplete picture reading. The reason of incomplete picture random reading has not been found yet
                    Thread.Sleep(20);
                }
            }
            stream.Close();
            hwResponse.Close();
        }

        private static void accessImageHandler(byte[] imageFileBytes)
        {
            Console.WriteLine("Size: {0}", imageFileBytes.Length);
        }
    }
}
