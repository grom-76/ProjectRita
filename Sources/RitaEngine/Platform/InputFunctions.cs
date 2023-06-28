namespace RitaEngine.Platform;

using RitaEngine.Base;
using RitaEngine.API.Window;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public readonly struct InputFunctions : IDisposable
{
    public readonly unsafe delegate* unmanaged<byte*,int> GetKeyboardState = null ;
    public readonly unsafe delegate* unmanaged<int,short> GetAsyncKeyState = null ;
    public readonly unsafe delegate* unmanaged<int,short> GetKeyState = null ;
    public readonly unsafe delegate* unmanaged<POINT* ,int> GetCursorPos = null;
    public readonly unsafe delegate* unmanaged<int,int ,int> SetCursorPos = null;
    public readonly unsafe delegate* unmanaged< CURSORINFO* ,int> GetCursorInfo = null;
    

    /// <summary> The ScreenToClient function converts the screen coordinates of a specified point on the screen to client-area coordinates. </summary>
    public readonly unsafe delegate* unmanaged< void* ,POINT*, int > ScreenToClient = null;
    
    /// <summary> The ClientToScreen function converts the client-area coordinates of a specified point to screen coordinates. </summary>
    public readonly unsafe delegate* unmanaged< void* ,POINT*, int > ClientToScreen = null;

    public unsafe InputFunctions( PFN_GetSymbolPointer load, nint user32 ) 
    { 
        GetKeyboardState = (delegate* unmanaged<byte*,int>) load( user32 ,"GetKeyboardState");
        GetAsyncKeyState =  (delegate* unmanaged<int,short>) load( user32 , "GetAsyncKeyState");
        GetKeyState =  (delegate* unmanaged<int,short>) load (user32 , "GetKeyState");
        GetCursorPos = (delegate* unmanaged<POINT*,int>) load( user32 , "GetCursorPos" );
        SetCursorPos = (delegate* unmanaged<int,int ,int>) load( user32 , "SetCursorPos" );
        GetCursorInfo = (delegate* unmanaged< CURSORINFO* ,int>) load( user32 , "GetCursorInfo" );
        ScreenToClient =(delegate* unmanaged< void* ,POINT*, int >  ) load( user32 , "ScreenToClient" );
        ClientToScreen =(delegate* unmanaged< void* ,POINT*, int >  ) load( user32 , "ClientToScreen" );
    }
    public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)GetAsyncKeyState).ToInt32()  ,  ((nint)GetKeyboardState).ToInt32(),  ((nint)GetKeyState).ToInt32(), ((nint)SetCursorPos).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is InputFunctions context && this.Equals(context) ;
    public unsafe bool Equals(InputFunctions? other)=> other is InputFunctions input && (((nint)GetKeyState).ToInt64()).Equals(((nint)input.GetKeyState).ToInt64() );
    public static bool operator ==(InputFunctions  left, InputFunctions right) => left.Equals(right);
    public static bool operator !=(InputFunctions  left, InputFunctions  right) => !left.Equals(right);
    public void Dispose() {  }
    #endregion
}

