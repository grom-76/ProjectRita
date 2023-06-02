namespace RitaEngine.Base;

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class BytesHelper
{
            /// <summary>
    /// converti une chaine de caractère string en byte ( copy block ) 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    internal static byte[] StringToBytesSafe(string text)
    {
        var bytes = new byte[text.Length];
        System.Buffer.BlockCopy(text.ToCharArray() ,0, bytes,0,bytes.Length);
        return bytes;
    }

    /// <summary>
    /// converti une chaine de caractère string en byte Unsafe ( copy block ) 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    internal unsafe static byte[] StringToBytes(string value)
    {
        byte[] tempByte = ArrayPool<byte>.Shared.Rent(value.Length);;
        // var bytes = new byte[text.Length];
        fixed (void* ptr = value)
        {
            System.Runtime.InteropServices.Marshal.Copy(new IntPtr(ptr), tempByte, 0, value.Length);
        }    
        return tempByte;
    }
    /// <summary>
    /// Convertie un tableau de float en bytes
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static unsafe  byte[] FloatsToBytes(float[] data)
    {
        int n = data.Length;
        byte[] ret = ArrayPool<byte>.Shared.Rent(n * sizeof(float));;
        // var ret = GameMemory.Instance.NewBytes(n * sizeof(float) );
        if (n == 0) return ret;

        // unsafe
        // {
            fixed (byte* pByteArray = &ret[0])
            {
                float* pFloatArray = (float*)pByteArray;
                for (int i = 0; i < n; i++)
                {
                    pFloatArray[i] = data[i];
                }
            }
        // }

        return ret;
    }

    /// <summary>
    /// converti un floatent en byte( 4 bytes )
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static unsafe byte[] FloatToBytes(float data)
    {
        var ret = new  byte[ sizeof(float) ];

        fixed (byte* pByteArray = &ret[0])
        {
            float* pFloatArray = (float*)pByteArray;
            pFloatArray[0] = data;
        }

        return ret;
    }
    /// <summary>
    /// converti un byte en tableau ( tableau de 1 bytes )
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] ByteToBytes(byte v)
        =>new byte[sizeof(byte)]{ v};

    /// <summary>
    /// converti un boolean en tableau de byte( 1 byte )convertion
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] BoolToBytes(bool v)
        =>new byte[sizeof(bool)]{ v ? (byte)0x01 : (byte)0x00 };

    /// <summary>
    /// converti un short en tableau de bytes 2 bytes
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] ShortToBytes(short v)
        => new byte[sizeof(short) ]{  (byte)v , (byte)(v >> 8) }; 

    /// <summary>
    /// converti un short non singé en tableau de bytes 2 bytes
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] UShortToBytes(ushort v)
        => new byte[sizeof(ushort) ]{  (byte)v , (byte)(v >> 8) }; 

    /// <summary>
    /// convert an int into 4 bytes
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] IntToBytes(int v)
        =>new byte[sizeof(int) ]{  (byte)v , (byte)(v >> 8),(byte)(v >> 16),(byte)(v >> 24) };
    
    /// <summary>
    /// converti un entier non signé en 4 bytes
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] UIntToBytes(uint v)
        =>new byte[sizeof(uint) ]{  (byte)v , (byte)(v >> 8),(byte)(v >> 16),(byte)(v >> 24) };
    
    /// <summary>
    /// converti un entier long en 8 bytes
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] LongToBytes(long v)
        => new byte[sizeof(long) ]{  (byte)v , (byte)(v >> 8),(byte)(v >> 16),(byte)(v >> 24),(byte)(v >> 32),(byte)(v >> 40),(byte)(v >> 48),(byte)(v >> 56) };
    
    /// <summary>
    /// converti un entier long non singé en 8 bytes
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static byte[] ULongToBytes(ulong v)
        => new byte[sizeof(long) ]{  (byte)v , (byte)(v >> 8),(byte)(v >> 16),(byte)(v >> 24),(byte)(v >> 32),(byte)(v >> 40),(byte)(v >> 48),(byte)(v >> 56) };

    /// <summary>
    /// converti un tableau de 2 bytes en entier court non signé
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static ushort  BytesToUshort(  byte[] data )
        =>  (ushort)(  ( data[0] << 0) | (ushort) (data[1] << 8) ) ;
    
    /// <summary>
    /// converti un tableau de 2 bytes en entier court
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static  short  BytesToShort(  byte[] data )
        => (short) BytesToUshort(data);
    
    /// <summary>
    /// converti un tableau de 4 bytes en entier non signé
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static uint BytesToUInt(  byte[] data)
        => (uint)( (data[0] << 0) | (data[1] << 8) | (data[2] << 16) | (data[3] << 24)) ;
    
    /// <summary>
    /// converti un tableau de 4 bytes en entier 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static int BytesToInt(  byte[] data)
        =>  ( int) BytesToUInt(data);
    
    /// <summary>
    /// converti un tableau de 8 bytes en entier long non signé
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static ulong BytesToULong(  byte[] data)
        =>  (( ulong)data[0] <<  0) | (( ulong)data[1] <<  8) | (( ulong)data[2] << 16) | (( ulong)data[3] << 24) |
            (( ulong)data[4] << 32) | (( ulong)data[5] << 40) | (( ulong)data[6] << 48) | (( ulong)data[7] << 56);
    
    /// <summary>
    /// converti un tableau de 8 bytes en entier long
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static  long  BytesToLong(  byte[] data )
        =>  ( long) BytesToULong(data);

    /// <summary>
    /// Rotation de bits vers la gauche d'un entier long non signé
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bits"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong RotateLeft(ulong value, int bits)
        => (value << bits) | (value >> (64 - bits));
    
    /// <summary>
    /// Rotation de bits vers la gauche d'un entier non signé
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bits"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint RotateLeft(uint value, int bits)
        => (value << bits) | (value >> (32 - bits));

    /// <summary>
    /// Rotation de bits vers la droite d'un entier non signé
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bits"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint RotateRight(uint value, int bits)
        => (value >> bits) | (value << (32 - bits));
    
    /// <summary>
    /// Rotation de bits vers la droite d'un entier long non signé
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bits"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong RotateRight(ulong value, int bits)
        => (value >> bits) | (value << (64 - bits));

    /// <summary>
    /// Convertie un tableau de 8 bytes en entier long non signé ( méthode plus rapide )
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="leftBytes"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong PartialBytesToUInt64(byte[] buffer, int leftBytes)
    {
        // a switch/case approach is slightly faster than the loop but .net
        // refuses to inline it due to larger code size.
        ulong result = 0;

        // trying to modify leftBytes would invalidate inlining
        // need to use local variable instead
        for (int i = 0; i < leftBytes; i++)
        {
            result |= ((ulong)buffer[i]) << (i << 3);
        }

        return result;
    }
    /// <summary>
    /// Unsafe Convertie un tableau de 8 bytes ( pointeur ) en entier long non signé ( méthode plus rapide )
    /// </summary>
    /// <param name="ptr"></param>
    /// <param name="leftBytes"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe uint PartialBytesToUInt32(byte* ptr, int leftBytes)
    {
        if (leftBytes > 3)
        {
            return *((uint*)ptr);
        }

        // a switch/case approach is slightly faster than the loop but .net
        // refuses to inline it due to larger code size.
        uint result = *ptr;
        if (leftBytes > 1)
        {
            result |= (uint)(ptr[1] << 8);
        }

        if (leftBytes > 2)
        {
            result |= (uint)(ptr[2] << 16);
        }

        return result;
    }

    /// <summary>
    /// Convertie un tableau de 4 bytes en entier non signé ( méthode plus rapide )
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="leftBytes"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint PartialBytesToUInt32(byte[] buffer, int leftBytes)
    {
        if (leftBytes > 3)
        {
            return System.BitConverter.ToUInt32(buffer, 0);
        }

        // a switch/case approach is slightly faster than the loop but .net
        // refuses to inline it due to larger code size.
        uint result = buffer[0];
        if (leftBytes > 1)
        {
            result |= (uint)(buffer[1] << 8);
        }

        if (leftBytes > 2)
        {
            result |= (uint)(buffer[2] << 16);
        }

        return result;
    }

    /// <summary>
    /// swap bits ( conversion big endian little indian ou inversement)
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint SwapBytes32(uint num)
        => (RotateLeft(num, 8) & 0x00FF00FFu)
            | (RotateRight(num, 8) & 0xFF00FF00u);

    /// <summary>
    /// swap bits ( conversion big endian little indian ou inversement)
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong SwapBytes64(ulong num)
    {
        num = (RotateLeft(num, 48) & 0xFFFF0000FFFF0000ul)
            | (RotateLeft(num, 16) & 0x0000FFFF0000FFFFul);

        return (RotateLeft(num, 8) & 0xFF00FF00FF00FF00ul)
            | (RotateRight(num, 8) & 0x00FF00FF00FF00FFul);
    }

    /// <summary>
    /// swap bits ( conversion big endian little indian ou inversement)
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    internal static ushort Swap16bits( ushort n)
        =>  (ushort)( ((n & 0xFF00) >> 8) |  ((n & 0x00FF) << 8) ) ;

    /// <summary>
    /// swap bits ( conversion big endian little indian ou inversement)
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    internal static uint  Swap32bits( uint n)
    =>    ((n & 0xFF000000) >> 24) | ((n & 0x00FF0000) >>  8) 
        | ((n & 0x0000FF00) <<  8) | ((n & 0x000000FF) << 24);
    

    /// <summary>
    /// swap bits ( conversion big endian little indian ou inversement)
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    internal static ulong  Swap64bits( ulong n)
    => ((n & ( ulong)0xFF00000000000000) >> 56) |
            ((n & ( ulong)0x00FF000000000000) >> 40) |
            ((n & ( ulong)0x0000FF0000000000) >> 24) |
            ((n & ( ulong)0x000000FF00000000) >>  8) |
            ((n & ( ulong)0x00000000FF000000) <<  8) |
            ((n & ( ulong)0x0000000000FF0000) << 24) |
            ((n & ( ulong)0x000000000000FF00) << 40) |
            ((n & ( ulong)0x00000000000000FF) << 56);
    
}//convert
    