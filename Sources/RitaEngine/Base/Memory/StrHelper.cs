

namespace RitaEngine.Base
{

    namespace Strings
    {
        /// <summary>
        /// replace string (for unmanaged struct ?)
        /// </summary>
        [SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
    public unsafe static class StrHelper
    {
        public static ReadOnlySpan<byte> GetSpanBytes(string chaine)
            => Encoding.UTF8.GetBytes(chaine).AsSpan();
        
        public static string GetString( byte[] bytes) => Encoding.UTF8.GetString( bytes);

        public static byte[] GetBytes(string chaine) =>Encoding.UTF8.GetBytes(chaine);

        /// <summary>
        /// renvoi la taille d'uen chaine sans le caractere null 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public unsafe static uint Strlen(byte* str)
        {
            var ptr = str;

            while (*ptr != '\0')
                ptr++;

            return (uint)ptr - (uint)str ;
        }

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


    }


    }
    
}
