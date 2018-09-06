using Craft.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Craft.Net.Packets
{
    public class PacketBuffer
    {
        private MemoryStream baseStream;

        public long Position
        {
            get => baseStream.Position; set => baseStream.Position = value;
        }

        public PacketBuffer(byte[] arr)
        {
            baseStream = new MemoryStream(arr);
        }

        public PacketBuffer()
        {
            baseStream = new MemoryStream();
        }

        public MemoryStream GetStream()
        {
            return baseStream;
        }

        public bool ReadBool()
        {
            return ReadByte() == 0x01;
        }

        public byte ReadByte()
        {
            return ReadRawByteArray(1)[0];
        }

        public byte[] ReadByteArray()
        {
            return ReadRawByteArray(ReadVarInt());
        }

        public float ReadByteEncRot()
        {
            sbyte input = ReadSByte();
            float f = ((float)input * 360) / 256.0F;
            return f;
        }

        public double ReadDouble()
        {
            byte[] contents = ReadRawByteArray(8);
            Array.Reverse(contents);
            return BitConverter.ToDouble(contents, 0);
        }

        public float ReadFloat()
        {
            byte[] contents = ReadRawByteArray(4);
            Array.Reverse(contents);
            return BitConverter.ToSingle(contents, 0);
        }

        public double ReadFractInt()
        {
            return ReadInt() / 32;
        }

        public int ReadInt()
        {
            byte[] contents = ReadRawByteArray(4);
            Array.Reverse(contents);
            return BitConverter.ToInt32(contents, 0);
        }

        public sbyte ReadSByte()
        {
            return ToSbyte(ReadByte());
        }

        public short ReadShort()
        {
            byte[] contents = ReadRawByteArray(2);
            Array.Reverse(contents);
            return BitConverter.ToInt16(contents, 0);
        }

        public string ReadString()
        {
            byte[] contents = ReadRawByteArray(ReadVarInt());
            return Encoding.UTF8.GetString(contents);
        }

        public byte[] ReadToEnd()
        {
            List<byte> bytes = new List<byte>();
            while (true)
            {
                int i = baseStream.ReadByte();
                if (i == -1)
                    return bytes.ToArray();
                bytes.Add((byte)i);
            }
        }

        public ulong ReadULong()
        {
            byte[] contents = ReadRawByteArray(8);
            Array.Reverse(contents);
            return BitConverter.ToUInt64(contents, 0);
        }

        public long ReadLong()
        {
            byte[] contents = ReadRawByteArray(8);
            Array.Reverse(contents);
            return BitConverter.ToInt64(contents, 0);
        }

        public int ReadVarInt()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (true)
            {
                k = ReadByte();
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }

        public byte[] ToArray()
        {
            return baseStream.ToArray();
        }

        public void WriteBool(bool onGround)
        {
            WriteRawByteArray(new byte[] { onGround ? (byte)0x01 : (byte)0x00 });
        }

        public void WriteByte(byte @byte)
        {
            WriteRawByteArray(new byte[] { @byte });
        }

        public void WriteByteArray(byte[] v)
        {
            WriteVarInt(v.Length);
            WriteRawByteArray(v);
        }

        public void WriteDouble(double v)
        {
            byte[] content = BitConverter.GetBytes(v);
            Array.Reverse(content);
            WriteRawByteArray(content);
        }

        public void WriteFloat(float v)
        {
            byte[] content = BitConverter.GetBytes(v);
            Array.Reverse(content);
            WriteRawByteArray(content);
        }

        public void WriteShort(short v)
        {
            byte[] content = BitConverter.GetBytes(v);
            Array.Reverse(content);
            WriteRawByteArray(content);
        }
        public void WriteString(string s)
        {
            byte[] content = Encoding.UTF8.GetBytes(s);
            WriteVarInt(content.Length);
            WriteRawByteArray(content);
        }

        public void WriteULong(ulong v)
        {
            byte[] content = BitConverter.GetBytes(v);
            Array.Reverse(content);
            WriteRawByteArray(content);
        }

        public void WriteLong(long v)
        {
            byte[] content = BitConverter.GetBytes(v);
            Array.Reverse(content);
            WriteRawByteArray(content);
        }

        public void WriteUShort(ushort v)
        {
            byte[] content = BitConverter.GetBytes(v);
            Array.Reverse(content);
            WriteRawByteArray(content);
        }

        public ushort ReadUShort()
        {
            byte[] contents = ReadRawByteArray(2);
            Array.Reverse(contents);
            return BitConverter.ToUInt16(contents, 0);
        }

        public void WriteVarInt(int i)
        {
            WriteRawByteArray(ByteUtils.ToVarInt(i));
        }
        private byte[] ReadRawByteArray(int len)
        {
            byte[] buf = new byte[len];
            baseStream.Read(buf, 0, len);
            return buf;
        }

        private sbyte ToSbyte(byte b)
        {
            return (sbyte)(b < 128 ? b : b - 256);
        }

        public void WriteRawByteArray(byte[] arr)
        {
            baseStream.Write(arr, 0, arr.Length);
        }

        public void Reset()
        {
            baseStream.Position = 0;
        }

        public void Reset(int pos)
        {
            baseStream.Position = pos;
        }
    }
}