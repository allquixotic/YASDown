using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YASDown
{
    class Utils
    {
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string Utf8ToString(IntPtr aUtf8)
        {
            if (aUtf8 == IntPtr.Zero)
                return null;
            int len = 0;
            while (Marshal.ReadByte(aUtf8, len) != 0)
                len++;
            if (len == 0)
                return "";
            byte[] array = new byte[len];
            Marshal.Copy(aUtf8, array, 0, len);
            return Encoding.UTF8.GetString(array);
        }
    }
}
