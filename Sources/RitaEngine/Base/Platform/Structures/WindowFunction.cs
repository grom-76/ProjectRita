namespace RitaEngine.Base.Platform.Structures; // OS SYSTEMS// LAUNCHER CONTEXT DEVICE MACHINE , User Interface , Output

using RitaEngine.Base.Platform.API.Window;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public readonly struct WindowFunction : IEquatable<WindowFunction>
{
    // public nint User32 = nint.Zero;
    // public nint Kernel = nint.Zero;
    // public nint Gdi = nint.Zero;
    public readonly unsafe delegate* unmanaged<WNDCLASSEXA, ushort> RegisterClassExA = null;
    public readonly unsafe delegate* unmanaged<byte*, void*,int> UnRegisterClassA = null;
    public readonly unsafe delegate* unmanaged<void*,int> DestroyWindow = null;
    public readonly unsafe delegate* unmanaged<uint, byte*, byte*,uint,int,int,int,int,void*,void*,void*,void*,void*> CreateWindowExA = null;
    public readonly unsafe delegate* unmanaged<void*,int,int > ShowWindow=null;
    public readonly unsafe delegate* unmanaged<MSG*,void*,uint,uint, int> GetMessageA = null;
    public readonly unsafe delegate* unmanaged<MSG*,void*,uint,uint,uint ,int> PeekMessageA = null;
    public readonly unsafe delegate* unmanaged<MSG* ,int> TranslateMessage = null;
    public readonly unsafe delegate* unmanaged<MSG* ,long*> DispatchMessageA = null;
    public readonly unsafe delegate* unmanaged<int ,void> PostQuitMessage = null; 
    public readonly unsafe delegate* unmanaged<void*,uint,nuint/*WPARM*/,nint/*LPARAM*/,nint/*LRESULT*/> DefWindowProcA = null; 
    public readonly unsafe delegate* unmanaged<void*,int > UpdateWindow=null;
    public readonly unsafe delegate* unmanaged<RECT*,uint,int,int> AdjustWindowRect = null;
    public readonly unsafe delegate* unmanaged<void*,uint,nuint,nint,nint> SendMessageA= null;
    public readonly unsafe delegate* unmanaged<void*, byte*,int> SetWindowTextA = null;  
    public readonly unsafe delegate* unmanaged<void*,int, nint> GetWindowLongPtrW = null;
    public readonly unsafe delegate* unmanaged<void*,RECT*,int>  GetClientRect = null;
    public readonly unsafe delegate* unmanaged<byte*,void*> GetModuleHandleA = null;

    public unsafe WindowFunction(PFN_GetSymbolPointer load, nint user32 , nint kernel, nint gdi)
    { 
        DefWindowProcA = (delegate* unmanaged<void*,uint,nuint/*WPARM*/,nint/*LPARAM*/,nint/*LRESULT*/>) load( user32, "DefWindowProcA");
        GetMessageA = (delegate* unmanaged<MSG*,void*,uint,uint, int>) load( user32, "GetMessageA");
        PeekMessageA = (delegate* unmanaged<MSG*,void*,uint,uint,uint ,int>)load( user32, "PeekMessageA");
        DispatchMessageA = (delegate* unmanaged<MSG* ,long*>)load( user32, "DispatchMessageW");
        TranslateMessage = (delegate* unmanaged<MSG* ,int>) load( user32, "TranslateMessage");
        PostQuitMessage = (delegate* unmanaged<int ,void>)load( user32, "PostQuitMessage"); 
        RegisterClassExA   = (delegate* unmanaged<WNDCLASSEXA, ushort>) load( user32, "RegisterClassExA");
        CreateWindowExA = (delegate* unmanaged<uint, byte*, byte*,uint,int,int,int,int,void*,void*,void*,void*,void*>) load( user32, "CreateWindowExA");
        ShowWindow = (delegate* unmanaged<void*,int,int >)load( user32, "ShowWindow");
        DestroyWindow = (delegate* unmanaged<void*,int>)load( user32, "DestroyWindow");
        UnRegisterClassA = (delegate* unmanaged<byte*, void*,int >) load( user32, "UnregisterClassA");
        UpdateWindow = (delegate* unmanaged<void*,int >)load( user32, "UpdateWindow"); 
        AdjustWindowRect =  (delegate* unmanaged<RECT*,uint,int,int>) load(user32, "AdjustWindowRect");
        SendMessageA = (delegate* unmanaged<void*,uint,nuint,nint,nint>)load(user32,"SendMessageA"); 
        GetWindowLongPtrW =( delegate* unmanaged<void*,int, nint>)load(user32, "GetWindowLongPtrW");
        SetWindowTextA =( delegate* unmanaged<void*,byte*,int>)load(user32, "SetWindowTextA");

        GetModuleHandleA = (delegate* unmanaged<byte*,void*> )load( kernel, "GetModuleHandleA" );
        GetClientRect = ( delegate* unmanaged<void*,RECT*,int>)load(user32,"GetClientRect");

        GetDC=( delegate* unmanaged<void*,void*>) load(user32, "GetDC");
        ReleaseDC=( delegate* unmanaged<void*,void*,int>)load(user32, "ReleaseDC");
        GetMonitorInfoW =   (delegate* unmanaged<void* ,  MONITORINFOEX*, int>) load(user32, "GetMonitorInfoW");
        MonitorFromWindow = (delegate* unmanaged<void*,uint,void*>)  load(user32, "MonitorFromWindow");
        EnumDisplaySettingsW = (delegate* unmanaged<char* ,int, DEVMODEW*, int>) load( user32 , "EnumDisplaySettingsW" );
        SetProcessDpiAwarenessContext=(delegate* unmanaged<void*, int>)load( user32, "SetProcessDpiAwarenessContext" );
        GetWindowDpiAwarenessContext = ( delegate* unmanaged<void*, nuint>) load( user32, "GetWindowDpiAwarenessContext" );

        GetDesktopWindow= (delegate* unmanaged<void* > ) load(user32, "GetDesktopWindow");

        GetDeviceCaps=( delegate* unmanaged<void*,int, int>)load(gdi, "GetDeviceCaps");

    }

    public readonly unsafe delegate* unmanaged<void*,uint,void*> MonitorFromWindow = null;// ????
    public readonly unsafe  delegate* unmanaged<void* ,  MONITORINFOEX*, int> GetMonitorInfoW=null;
    public readonly unsafe  delegate* unmanaged<void* > GetDesktopWindow=null;
    public readonly unsafe  delegate* unmanaged<void*,int, int> GetDeviceCaps=null;//gdi       
    public readonly unsafe  delegate* unmanaged<void*,void*> GetDC=null;
    // public readonly unsafe delegate* unmanaged<void*,void*> GetWindowDC=null;
    public readonly unsafe delegate* unmanaged<void*/*HWND*/,void*/*HDC*/,int> ReleaseDC=null;

    public readonly unsafe delegate* unmanaged<char* ,int, DEVMODEW*,int> EnumDisplaySettingsW = null;
    public readonly unsafe delegate* unmanaged<void*,int> SetProcessDpiAwarenessContext=null;
    // public readonly unsafe delegate* unmanaged<BOOL> SetProcessDPIAware=null;
    // public readonly unsafe delegate* unmanaged<uint,uint,void*,uint,BOOL> SystemParametersInfo=null;

    public  readonly unsafe delegate* unmanaged<void*, nuint> GetWindowDpiAwarenessContext=null ;
    // public readonly unsafe delegate* unmanaged<int GetDpiForWindow(System.IntPtr hWnd);
    // public readonly unsafe delegate* unmanaged<int GetAwarenessFromDpiAwarenessContext(System.IntPtr dpiContext);
    // public readonly unsafe delegate* unmanaged<bool AreDpiAwarenessContextsEqual(System.IntPtr dpiContextA, System.IntPtr dpiContextB);
    // public readonly unsafe delegate* unmanaged<bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness); //shcore
    // public readonly unsafe delegate* unmanaged<void GetProcessDpiAwareness(System.IntPtr hprocess, out PROCESS_DPI_AWARENESS awareness);

    public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }

    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)UpdateWindow).ToInt32()  ,  ((nint)ShowWindow).ToInt32(),  ((nint)SetWindowTextA).ToInt32(), ((nint)GetClientRect).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is WindowFunction context && this.Equals(context) ;
    public unsafe bool Equals(WindowFunction other)=> other is WindowFunction win && (((nint)UpdateWindow).ToInt64()).Equals(((nint)win.UpdateWindow).ToInt64() );
    public static bool operator ==(WindowFunction  left, WindowFunction right) => left.Equals(right);
    public static bool operator !=(WindowFunction  left, WindowFunction  right) => !left.Equals(right);
    #endregion
}


