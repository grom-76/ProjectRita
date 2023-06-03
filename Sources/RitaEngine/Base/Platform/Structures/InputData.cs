

namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.Window;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct InputData : IEquatable<InputData>
{

    public readonly byte[] CurrentKeys = new byte[256];
    public readonly byte[] PreviousKeys = new byte[256];
    public readonly byte[] ToggleKeys = new byte[256];


    // public byte[] KeyboardName =null!;

    public nint User32 = nint.Zero;
    public unsafe void* WindowHandle= null;
    private nint _address = nint.Zero;
    public int MouseDPI =0;
    public int MouseState =(int) CursorInfoFlags.Hidden;
    public int Mouse_CurrentFrame_Position_X =0;
    public int Mouse_CurrentFrame_Position_Y =0;
    public int Mouse_PreviousFrame_Position_X =0;
    public int Mouse_PreviousFrame_Position_Y =0;
    public int Mouse_CurrentFrame_Delta_X =0;
    public int Mouse_CurrentFrame_Delta_Y =0;
    public bool IsMouseLeaveWindow = true;

    public InputData() {   _address = AddressOfPtrThis( ) ; }
    public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
    #region OVERRIDE    
    public override string ToString() => string.Format($"Data Input " );
    public unsafe override int GetHashCode() => HashCode.Combine( CurrentKeys, PreviousKeys);
    public override bool Equals(object? obj) => obj is InputData  window && this.Equals(window) ;
    public unsafe bool Equals(InputData other)=>  false;
    public static bool operator ==(InputData  left,InputData right) => left.Equals(right);
    public static bool operator !=(InputData  left,InputData right) => !left.Equals(right);
    #endregion
    public unsafe void Dispose()
    {
        // KeyboardName =null!;
    }

}

