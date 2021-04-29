using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGetData
{
    class Logger
    {
        FileStream fs = File.Create(@"c:\log.txt");

        public void Write(char value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(new[] { value });
            fs.Write(info, 0, info.Length);
        }
        public void Write(string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

    }
}
