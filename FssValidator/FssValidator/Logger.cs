using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RosstatValidator
{
    public class LogEvent
    {
        public static void Write(string s)
        {
            Record(s);
        }

        private static void Record(string str)
        {
            var time = DateTime.Now;
            var file = new StreamWriter("log.txt", true, Encoding.Default);
            file.WriteLine(time + " " + str);
            file.Close();
        }


    }
}
