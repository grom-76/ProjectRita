

namespace RitaEngine.Base
{

    namespace Strings
    {
        // https://kalapos.net/Blog/ShowPost/DotNetConceptOfTheWeek16-RefStruct
        // https://cdiese.fr/csharp7-ref-struct/
        [ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
    public unsafe readonly struct StrUnsafe : IDisposable
    {
        public readonly byte* Pointer;
        public readonly uint Size;

        public StrUnsafe(string chaine)
        {
            Size = (uint)chaine.Length+1;
            Pointer = (byte*)NativeMemory.AllocZeroed( (uint)Size, sizeof(byte));
            byte[] sp = Encoding.UTF8.GetBytes( chaine );
            fixed(byte* ptr = &sp[0] ){ NativeMemory.Copy( ptr ,Pointer, (Size-1) ); }
        }

        public void Dispose()
        {
            NativeMemory.Free(Pointer);
        }
        public static unsafe implicit operator byte*(StrUnsafe value) => value.Pointer;
        public static unsafe implicit operator Char*(StrUnsafe value) => (char*)value.Pointer;
        public static implicit operator string(StrUnsafe value) => Encoding.UTF8.GetString( value.Pointer, (int)value.Size-1);
    }


    }
    
}
