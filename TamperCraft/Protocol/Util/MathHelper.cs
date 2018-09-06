using System;

namespace TamperCraft.Protocol.Util
{
    public static class MathHelper
    {
        public static int NextPowerOfTwo(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException("n", "Must be positive.");
            return (int)Math.Pow(2, Math.Ceiling(Math.Log(n, 2)));
        }
    }
}
