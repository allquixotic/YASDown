using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YASDown
{
    public class Log
    {
        public static readonly StreamWriter sw = null;
        public static bool gui = false;

        static Log()
        {
            try
            {
                sw = new StreamWriter(new FileStream(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "log.txt"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8);
            }
            catch (Exception) { }
        }

        public static void Debug(string o1, params object[] o2)
        {
            if (sw != null && sw.BaseStream.CanWrite)
            {
                sw.WriteLine(o1, o2);
                sw.Flush();
            }

            Console.WriteLine(o1, o2);
        }

        public static void Debug(string o1)
        {
            if (sw != null && sw.BaseStream.CanWrite)
            {
                sw.WriteLine(o1);
                sw.Flush();
            }

            Console.WriteLine(o1);
            Console.Out.Flush();
        }

        public static void Error(string o1, params object[] o2)
        {
            Debug(o1, o2);
        }

        public static void Error(string o1)
        {
            Debug(o1);
            if (gui)
            {
                MessageBox.Show(o1, "Error");
            }
        }
    }
}
