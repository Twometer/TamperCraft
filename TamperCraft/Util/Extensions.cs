using Craft.Net.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Util
{
    public static class Extensions
    {
        public static byte[] ToBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static byte[] Hash(this byte[] b)
        {
            var provider = SHA256.Create();
            return provider.ComputeHash(b);
        }

        public static bool EqualsArray(this byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            for(int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }

        public static T AsPacket<T>(this PacketBuffer buffer)
        {
            var obj = Activator.CreateInstance<T>();
            if(obj is IPacket packet)
            {
                ((IPacket)obj).Receive(buffer);
            }
            return obj;
        }
    }
}
