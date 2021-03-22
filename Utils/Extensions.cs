using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class Extensions
    {
        private static Random rnd = new Random();
        public static byte[] AddBytes(this byte[] l_component , byte[] r_component)
        {
            int l_length = l_component.Length;
            int r_length = r_component.Length;

            byte[] buffer = new byte[l_length + r_length];

            Buffer.BlockCopy(l_component, 0, buffer, 0, l_length);
            Buffer.BlockCopy(r_component, 0, buffer, l_length, r_length);

            return buffer;
        }

        public static string GetRandomString(int size)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, size)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
