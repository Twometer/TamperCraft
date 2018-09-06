using System;
using System.Collections.Generic;
using System.Text;

namespace Craft.Net.Util
{
    public static class ByteUtils
    {
        public static byte[] EncodeVarInt(this int paramInt)
        {
            return ToVarInt(paramInt);
        }

        public static byte[] EncodeVarInt(this long paramInt)
        {
            return ToVarInt(paramInt);
        }

        public static byte[] ToVarInt(long paramInt)
        {
            List<byte> bytes = new List<byte>();
            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }

        public static byte[] ToLengthPrefixedString(string theString)
        {
            byte[] str = Encoding.UTF8.GetBytes(theString);
            byte[] len = ToVarInt(str.Length);
            byte[] packet = Concat(len, str);
            return packet;
        }

        public static byte[] Concat(this byte[] a, byte[] b)
        {
            var result = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, result, 0, a.Length);
            Buffer.BlockCopy(b, 0, result, a.Length, b.Length);
            return result;
        }


    }
}