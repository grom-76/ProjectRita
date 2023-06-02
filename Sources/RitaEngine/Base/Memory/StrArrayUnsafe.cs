

namespace RitaEngine.Base
{

    namespace Strings
    {
        [StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
    public unsafe struct StrArrayUnsafe : IDisposable
    {
        private StrUnsafe[] _array=null!;
        public uint Count=0;
        public nint Pointer= nint.Zero;
       
        public StrArrayUnsafe( ref string[] list )
        {
            Count = (uint)list.Length;
            Pointer = Marshal.AllocHGlobal(sizeof(IntPtr) *(int)Count);
            _array = new StrUnsafe[Count];
            for (int i =0 ; i < Count ; i++)
			    Add( list[i],i);
        }

        private void Add( string value , int index =-1 )
        {
            if (index < 0 || index >= _array.Length) return ;

            _array[index] = new  StrUnsafe(value);
            ((byte**)Pointer)[index] = _array[index];
        }

        public StrUnsafe this[int index] { get => _array[index]; set => Add(value,index); }

        public void Dispose()
        {
            if (_array == null || _array.Length == 0 || Pointer == nint.Zero) return;

            for( int i =0 ; i < _array.Length ; i++){
			    _array[i].Dispose();}
            
            Marshal.FreeHGlobal(Pointer);
            Pointer = nint.Zero;
		}

        public static implicit operator byte**(StrArrayUnsafe array) => (byte**) array.Pointer;

        public static implicit operator char**(StrArrayUnsafe array) => (char**) array.Pointer;
    }


    }
    
}
