using System;
using System.Runtime.InteropServices;

namespace Trivial.Utilities
{
    public static partial class Blit
    {
        // public static byte[] StructToByteArr<T>(ref T Value) where T : struct
        // {
        //     var t_ByteArr = new byte[Unsafe.SizeOf<T>()];
        //     MemoryMarshal
        //         .Cast<T, byte>(MemoryMarshal.CreateSpan(ref Value, 1))
        //         .CopyTo(t_ByteArr);

        //     return t_ByteArr;
        // }

        public static void StructToByteArr<T>(ref T Value, ref Span<byte> Arr) where T : struct
        {
            MemoryMarshal
                .Cast<T, byte>(MemoryMarshal.CreateSpan(ref Value, 1))
                .CopyTo(Arr);
        }
    }
}