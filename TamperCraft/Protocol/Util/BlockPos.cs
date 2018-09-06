using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Protocol.Util;

namespace Craft.Net.Util
{
    public class BlockPos
    {
        private static int NUM_X_BITS = 1 + MathCustom.CalculateLogBaseTwo(MathHelper.NextPowerOfTwo(30000000));
        private static int NUM_Z_BITS = NUM_X_BITS;
        private static int NUM_Y_BITS = 64 - NUM_X_BITS - NUM_Z_BITS;
        private static int Y_SHIFT = 0 + NUM_Z_BITS;
        private static int X_SHIFT = Y_SHIFT + NUM_Y_BITS;
        private static long X_MASK = (1L << NUM_X_BITS) - 1L;
        private static long Y_MASK = (1L << NUM_Y_BITS) - 1L;
        private static long Z_MASK = (1L << NUM_Z_BITS) - 1L;

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public BlockPos(long val)
        {
            long x = val >> 38;
            long y = (val >> 26) & 0xFFF;
            long z = val << 38 >> 38;
            if (x >= (2 ^ 25)) x -= 2 ^ 26;
            if (y >= (2 ^ 11)) y -= 2 ^ 12;
            if (z >= (2 ^ 25)) z -= 2 ^ 26;
            X = (int)x;
            Y = (int)y;
            Z = (int)z;
        }

        public BlockPos(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static BlockPos FromLong(ulong serialized)
        {
            int x = (int)(serialized << 64 - X_SHIFT - NUM_X_BITS >> 64 - NUM_X_BITS);
            int y = (int)(serialized << 64 - Y_SHIFT - NUM_Y_BITS >> 64 - NUM_Y_BITS);
            int z = (int)(serialized << 64 - NUM_Z_BITS >> 64 - NUM_Z_BITS);
            if (x >= 33554432) x -= 67108864;
            if (y >= 2048) y -= 4096;
            if (z >= 33554432) z -= 67108864;
            return new BlockPos(x, y, z);
        }

        public long ToLong()
        {
            return (((long)X & 0x3FFFFFF) << 38) | (((long)Y & 0xFFF) << 26) | ((long)Z & 0x3FFFFFF);
        }

    }

    public class MathCustom
    {

        private static int[] multiplyDeBruijnBitPosition = new int[] { 0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8, 31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9 };

        private static bool IsPowerOfTwo(int value)
        {
            return value != 0 && (value & value - 1) == 0;
        }

        public static int CalculateLogBaseTwo(int value)
        {
            return CalculateLogBaseTwoDeBruijn(value) - (IsPowerOfTwo(value) ? 0 : 1);
        }

        private static int CalculateLogBaseTwoDeBruijn(int value)
        {
            value = IsPowerOfTwo(value) ? value : MathHelper.NextPowerOfTwo(value);
            return multiplyDeBruijnBitPosition[(int)(value * 125613361L >> 27) & 31];
        }

    }
}
