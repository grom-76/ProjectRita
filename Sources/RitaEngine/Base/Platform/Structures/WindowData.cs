namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.Native.Window;

[StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct WindowData : IEquatable<WindowData>
{
    public MSG Msg = new();
    public PFN_WNDPROC WndProc = null!;
    private nint _address = nint.Zero;
    public unsafe void* Handle =null;
    public unsafe void* HInstance = null;
    public int Width = 0;
    public int Height = 0;
    public int Left=0;
    public int Top=0;
    public uint Style = 0;
    public uint ExStyle = 0;
    public nint User32 = nint.Zero;
    public nint Kernel = nint.Zero;
    public nint Gdi = nint.Zero;
    public bool IsRun = true;
    public bool IsFocused = false;
    public bool IsPrimaryMonitor = false;
    
    public byte[] Title = null!;
    // public int WindowState =0 ; // 0 = none, 1= Normal , 2 =sized, 3= maximized , 4= minimized ,...

    public WindowData() {   _address = AddressOfPtrThis( ) ;   }

    public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Data Window " );
    public unsafe override int GetHashCode() => HashCode.Combine( (nint)Handle, (nint)HInstance);
    public override bool Equals(object? obj) => obj is WindowData  window && this.Equals(window) ;
    public unsafe bool Equals(WindowData other)=>  Handle == other.Handle && HInstance == other.HInstance ;
    public static bool operator ==(WindowData  left,WindowData right) => left.Equals(right);
    public static bool operator !=(WindowData  left,WindowData right) => !left.Equals(right);
    #endregion
}
