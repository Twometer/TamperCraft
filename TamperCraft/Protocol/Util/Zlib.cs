using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Util
{
    public static class Zlib
    {
        /// <summary>
        /// Compress a byte array into another bytes array using Zlib compression
        /// </summary>
        /// <param name="toCompress">Data to compress</param>
        /// <returns>Compressed data as a byte array</returns>
        public static byte[] Compress(byte[] toCompress)
        {
            ZlibStream stream = new ZlibStream(new MemoryStream(toCompress, false), CompressionMode.Compress);
            MemoryStream outputStream = new MemoryStream();
            var buffer = new byte[32768];
            while (true)
            {
                var read = stream.Read(buffer, 0, buffer.Length);
                if (read > 0) outputStream.Write(buffer, 0, read);
                else break;
            }
            return outputStream.ToArray();
        }

        /// <summary>
        /// Decompress a byte array into another byte array of the specified size
        /// </summary>
        /// <param name="toDecompress">Data to decompress</param>
        /// <param name="uncompressedSize">Size of the data once decompressed</param>
        /// <returns>Decompressed data as a byte array</returns>
        public static byte[] Decompress(byte[] toDecompress, int uncompressedSize)
        {
            ZlibStream stream = new ZlibStream(new MemoryStream(toDecompress, false), CompressionMode.Decompress);
            byte[] decompressed = new byte[uncompressedSize];
            stream.Read(decompressed, 0, uncompressedSize);
            stream.Close();
            return decompressed;
        }
    }
}
