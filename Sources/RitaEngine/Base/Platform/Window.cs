namespace RitaEngine.Base.Platform ;

using RitaEngine.Base.Platform.API.Window;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Platform.Config;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct Window : IEquatable<Window>
{
    
    private WindowData _data;
    private WindowFunction _funcs;
    public  WindowConfig Config = new();
    private nint _address = nint.Zero;

    public Window(){   _address = AddressOfPtrThis();  }

    public unsafe nint AddressOfPtrThis( )
    { 
        #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }

    public unsafe void* GetWindowHandle() => _data.Handle ;
    public unsafe void* GetWindowHInstance() => _data.HInstance ;
    public unsafe int GetWindowWidth() => _data.Width ;
    public unsafe int GetWindowheight() => _data.Height ;
    public byte[] GetWindowName() => _data.Title ;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Init()
    {
        //Guard if Title = empty if User32 empty UserPtr = null ....
        
        //LOAD DLL ....
        _data = new();
        _data.User32 =Libraries.Load( Config.System_User32DllName );
        _data.Kernel = Libraries.Load( Config.System_KernelDllName );
        _data.Gdi =  Libraries.Load( Config.System_Gdi32DllName );
        _funcs = new(Libraries.GetUnsafeSymbol, _data.User32 , _data.Kernel, _data.Gdi);
        _data.WndProc = this.WndProc2;
        // _data.Style =  Constants.WS_CAPTION | Constants.WS_SYSMENU /*| Constants.WS_VISIBLE */| Constants.WS_THICKFRAME;
        MonitorsInfo( ref _data , ref _funcs);
        Create(ref _data, ref _funcs , Config);
        //Logically Here Dispose COnfig don''t need anywhere 

        Config.Dispose();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsForeGround() {return Focused; } 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Show() => Show(ref _data , ref _funcs);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DispatchPending() => Update(ref _data , ref _funcs);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ShouldClose() => ShouldClose(ref _data , ref _funcs);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RequestClose() =>RequestClose(ref _data , ref _funcs);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Release() => Release(ref _data , ref _funcs);
    
private static bool Focused = false;

    #region STATIC IMPLEMENT
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // private unsafe nint WndProc( void* hWnd, uint message,  nuint wParam,  nint lParam)
    // {
    //     #if WIN64
    //     switch (message)
    //     {
    //         // case Constants.WM_PAINT:
    //         //     return (_funcs.DefWindowProcA(hWnd, message, wParam, lParam));
    //         case Constants.WM_DESTROY:
    //             return 0;
    //         case Constants.WM_QUIT:
    //             return 0;
    //         case Constants.WM_CLOSE :
    //             _funcs.PostQuitMessage(0);
    //             _data.IsRun = false;
    //             return 0;
    //         // case Constants.WM_SIZE :
    //         //     // Config.GraphicDevice.Surface.Height =  Utils.HIWORD(lParam);
    //         //     // Config.GraphicDevice.Surface.Width =Utils.LOWORD(lParam);
    //         //     // REGraphicDevice.ReCreateSwapChain( ref _data.GraphicDevice,(uint)Utils.LOWORD(lParam),(uint)Utils.HIWORD(lParam));
    //         //      return (_funcs.DefWindowProcA(hWnd, message, wParam, lParam));
    //         case Constants.WM_SETFOCUS :
    //             Focused =true;
    //              return (_funcs.DefWindowProcA(hWnd, message, wParam, lParam));
    //         case Constants.WM_KILLFOCUS :
    //             Focused = false;
    //              return (_funcs.DefWindowProcA(hWnd, message, wParam, lParam));
    //         default:
    //             return (_funcs.DefWindowProcA(hWnd, message, wParam, lParam));
    //     }
    //     #elif LINUX64

    //     #endif

    // } 

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe nint WndProc2( void* hWnd, uint message,  nuint wParam,  nint lParam)
		=>  message switch {
			Constants.WM_QUIT  => Win32OnQuit(hWnd,message,wParam,lParam),//QUIT
			Constants.WM_CLOSE => Win32OnClose(hWnd,message,wParam,lParam),//CLOSE
			Constants.WM_DESTROY => Win32OnDestroy(hWnd,message,wParam,lParam),//DESTROy
			Constants.WM_SETFOCUS => Win32OnSetFocus(hWnd,message,wParam,lParam),//SETFOCUS
			Constants.WM_KILLFOCUS => Win32OnKillFocus(hWnd,message,wParam,lParam),//KILLFOCUS
			Constants.WM_SHOWWINDOW => Win32OnShowWindow(hWnd,message,wParam,lParam),//SHOWWINDOW
			Constants.WM_LBUTTONDOWN or Constants.WM_RBUTTONDOWN or Constants.WM_MBUTTONDOWN or Constants.WM_XBUTTONDOWN or
			Constants.WM_LBUTTONUP or Constants.WM_RBUTTONUP or Constants.WM_MBUTTONUP or 
			Constants.WM_XBUTTONUP => Win32OnMouseButtonClic(hWnd,message,wParam,lParam),
			Constants.WM_KEYDOWN or  Constants.WM_SYSKEYDOWN or Constants.WM_KEYUP or
			Constants.WM_SYSKEYUP=> Win32OnKeyPress(hWnd,message,wParam,lParam),
			Constants.WM_INPUT=> Win32OnInput(hWnd,message,wParam,lParam),
			Constants.WM_SIZE => Win32OnSize(hWnd,message,wParam,lParam),
			_=> _funcs.DefWindowProcA(hWnd, message, wParam, lParam)
		};

	private unsafe nint Win32OnSize(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		// OnSize(wParam,lParam);
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}

	private  unsafe nint Win32OnInput(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		// OnInput(wParam,lParam);
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}
	private  unsafe nint Win32OnKeyPress(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		// OnKeyPress(wParam,lParam);
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}
	private  unsafe nint Win32OnMouseButtonClic(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		// OnMouseButtonClic!(wParam,lParam);
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}
	private   unsafe nint Win32OnQuit(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		// OnQuit!(wParam,lParam);
		return 0;
	}

	private  unsafe nint Win32OnShowWindow(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		// OnShowWindow!(wParam,lParam);
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}

	private  unsafe nint Win32OnSetFocus(void* hWnd, uint message, nuint wParam, nint lParam)
	{
        Focused =true;
		// OnSetFocus!(wParam,lParam);
		// _isfocused =false;
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}

	private  unsafe nint Win32OnKillFocus(void* hWnd, uint message, nuint wParam, nint lParam)
	{
        Focused = false;
		// OnKillFocus!(wParam,lParam);
		// _isfocused = false;
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}

	private  unsafe nint Win32OnDestroy(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		_=hWnd;_=message;
		// OnDestroy!(wParam,lParam);
        _data.IsRun = false;
		_funcs.PostQuitMessage(0);
		return nint.Zero;
	}

	private  unsafe nint Win32OnClose(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		_=hWnd;_=message;
		// OnClose!(wParam,lParam);
        _data.IsRun = false;
		_funcs.PostQuitMessage(0);
		return nint.Zero;
	}

    private unsafe static int Create(ref WindowData data ,ref WindowFunction funcs,  in Config.WindowConfig config)
    {
        #if WIN64
        (data.Width, data.Height) =  WindowConfig.GetResolution( config.Rsolution);
        data.Left = Constants.CW_USEDEFAULT;
        data.Top = Constants.CW_USEDEFAULT;
        data.Style= Constants.WS_CAPTION | Constants.WS_SYSMENU | /*Constants.WS_VISIBLE |*/ Constants.WS_THICKFRAME;
        data.ExStyle = Constants.WS_EX_APPWINDOW | Constants.WS_EX_WINDOWEDGE;
        data.HInstance =funcs.GetModuleHandleA(null);// Marshal.GetHINSTANCE(typeof(WindowData).Module).ToPointer(); 
        var EngineNameChars = Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);
        data.Title =Encoding.UTF8.GetBytes(config.Title);

        WNDCLASSEXA wcex = new ();
        wcex.cbSize = (uint)Marshal.SizeOf( wcex) ;
        wcex.style = Constants.CS_OWNDC  | Constants.CS_HREDRAW | Constants.CS_VREDRAW ;//CS_HREDRAW | CS_VREDRAW | CS_OWNDC
        wcex.lpfnWndProc = data.WndProc;
        wcex.cbClsExtra = 0;
        wcex.cbWndExtra = 0;
        wcex.hIcon =   null;//LoadIconW(null, new IntPtr(32512).ToPointer() /*IDI_APPLICATION*/);
        wcex.hCursor =   null;//LoadCursor(IntPtr.Zero, 32512/*IDC_ARROW*/);
        wcex.hIconSm = null;//IntPtr.Zero;
        wcex.hbrBackground =new IntPtr(6).ToPointer();//(IntPtr)( /*COLOR_WINDOW*/5 + 1);https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsyscolor
        wcex.lpszMenuName = null;//string.Empty;
        wcex.hInstance =  data.HInstance;
        fixed( byte* ptr = &EngineNameChars[0]) { wcex.lpszClassName =ptr;  }
        
        _ = funcs.RegisterClassExA(wcex);
    
        fixed( byte* ptr = &EngineNameChars[0] , app = &data.Title[0] ) 
        {
        data.Handle = funcs.CreateWindowExA(  data.ExStyle,
            ptr,  app,
            data.Style,
            data.Left, data.Top,  data.Width,  data.Height,
            null, null,data.HInstance, null );
        }

        #else

        #endif
    
        return 0;
    }

    private unsafe static void MonitorsInfo(ref WindowData data ,ref WindowFunction funcs)
    {
        // GetMONITOR INFO 
        // ChangeDisplaySettingsExW MonitorFromPoint GetMonitorInfo


        MONITORINFOEX target;
        target.Size = MONITORINFOEX.MonitorInfoSize;

        void* hMon = funcs.MonitorFromWindow( funcs.GetDesktopWindow(), Constants.MONITOR_DEFAULTTOPRIMARY);
        funcs.GetMonitorInfoW(hMon, &target);
        // string asStrin = new string((char *) target.DeviceName, 0, 32);
        data.IsPrimaryMonitor = target.Flags == 1 ;

        DEVMODEW devmod;
        devmod.dmSize = (ushort)Unsafe.SizeOf<DEVMODEW>();
        // int err =_funcs.EnumDisplaySettingsW( (char *) target.DeviceName,Constants.ENUM_CURRENT_SETTINGS,&devmod );

        int err =  funcs.EnumDisplaySettingsW( null,0,&devmod );

        err =  funcs.EnumDisplaySettingsW( null,1,&devmod );
        err =  funcs.EnumDisplaySettingsW( null,2,&devmod );
        //  If the function fails, the return value is zero. The function fails if iDevNum is greater than the largest device index
        //  This setting is the screen DPI, or dots per inch.
        // 
        /* GET SCALING FACTOR 
            int LogicalScreenHeight = GetDeviceCaps( GetDC(hwnd), (int)DeviceCap.VERTRES  10) ;
            int PhysicalScreenHeight = GetDeviceCaps( GetDc(hwnd), (int)DeviceCap.DESKTOPVERTRES 117 ); 
            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
            ScreenScalingFactor; // 1.25 = 125%
            int Xdpi = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSX 88);
            int Ydpi = GetDeviceCaps(desktop, (int)DeviceCap.LOGPIXELSY 90);    
        */
    }
    private static void MonitorInit(ref WindowData data ,ref WindowFunction funcs)
    {
    #if WIN

//         Win32.RECT rect = new((int)data.left, (int)data.top,  (int)(data.Width + data.left) ,(int)(data.Height + data.top) );
        
//         _= Win32.AdjustWindowRect(&rect,data._style,0);
        
//         data.Width = (rect.Right - rect.Left);
//         data.Height = (rect.Bottom - rect.Top);
//         Log.Info($"Adjust Win RECT [Width={ data.Width},Height={ data.Height}, top= { data.top}left={ data.left}, style { config.Style}]");

    #else
    
    #endif        
    }
  //     /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="hwnd"></param>       
    // public  void SetDpiAwarness(in IntPtr hwnd)
    // {
    //     //For other
    //     // log.Test(SetProcessDPIAware(), "For Other Window Process DPI Aware ");
    //     SetProcessDPIAware();
    //     // //For Window 8
    //     // log.Test(SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.ProcessPerMonitorDpiAware), "For Windows 8 Set DPI Awareness ");
    //         SetProcessDpiAwareness( PROCESS_DPI_AWARENESS.ProcessPerMonitorDpiAware);
    //     // //For Win 10
    //     var ptr =  GetWindowDpiAwarenessContext(hwnd);
    //     // log.Test(SetProcessDpiAwarenessContext(ptr + ((IntPtr)DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2).ToInt32() ), "For Windows 10set DPI Awarness context" , Hwnd.ArgOf("Handle"));
    //     // log.Test(SetProcessDpiAwarenessContext(ptr  ), "For Windows 10set DPI Awarness context" , Hwnd.ArgOf("Handle"));
    //         SetProcessDpiAwarenessContext(ptr -4 );
    // }

 
    // /// <summary>
    // /// Get active screen Dimension 
    // /// </summary>
    // public static void GetMonitorViewPort(in System.IntPtr hwnd )
    // {
    //     // var hwnd = new WindowInteropHelper( this ).EnsureHandle();
    //     var monitor =  MonitorFromWindow(hwnd,  MONITOR_DEFAULTTONEAREST );

    //     if ( monitor != System.IntPtr.Zero )
    //     {
    //         MonitorInfo monitorInfo = new  ();

    //         int result = GetMonitorInfoW( monitor.ToPointer(),&monitorInfo );
                
    //         Log.Is( result != 0 ,$"Getmonitor info erro with Adr Ptr of monitor : {monitor}");

    //         // left = monitorInfo.Monitor.Left;
    //         // top = monitorInfo.Monitor.Top;
    //         // width = ( monitorInfo.Monitor.Right - monitorInfo.Monitor.Left );
    //         // height = ( monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top );
            
    //     }
    // }

    // /// <summary>
    // /// 
    // /// </summary>
    // public static void GetVideosMode()
    // {
    //     DEVMODE win32Mode = new  DEVMODE();
    //     win32Mode.dmSize = (short)System.Runtime.InteropServices.Marshal.SizeOf(typeof( DEVMODE));
    //     win32Mode.dmDriverExtra = 0;
    //     // EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS,ref  win32Mode);
    //     int i = 0;
    //     while ( EnumDisplaySettings(null!, i, ref win32Mode))
    //     {
    //         //                         win32Mode.dmPelsWidth,
    //         //                         win32Mode.dmPelsHeight,
    //         //                         win32Mode.dmBitsPerPel,
    //         //                         win32Mode.dmDisplayFrequency
    //     }
    // }
     // /// <summary>
    // /// // To make SetForegroundWindow work as we want, we need to fiddle
    // ///    // with the FOREGROUNDLOCKTIMEOUT system setting (we do this as early
    // ///    // as possible in the hope of still being the foreground process)
    // /// </summary>
    // /// <param name="foregroundLockTimeout"></param>
    // public static void SetForeground(ref System.IntPtr foregroundLockTimeout)
    // {
    //     SystemParametersInfo((int) SPI.GETFOREGROUNDLOCKTIMEOUT, 0, foregroundLockTimeout, 0);
    //     SystemParametersInfo((int) SPI.SETFOREGROUNDLOCKTIMEOUT, 0, System.IntPtr.Zero,  SPIF_SENDCHANGE);
    // }

    // public static  void CenterSurface(in uint width, in uint height, ref uint left , ref uint top)
    // {
    //     //TODO : REDO with no interop Dll import
    //     // Compute position and size
    //     var screenDC =    Monitor.GetDC(System.IntPtr.Zero);
    //     left = (uint)(  Monitor.GetDeviceCaps(screenDC, (int)  DeviceCap.HORZRES) - width) / 2;
    //     top = (uint)(  Monitor.GetDeviceCaps(screenDC, (int)  DeviceCap.VERTRES) - height) / 2;

    //     Monitor.ReleaseDC(System.IntPtr.Zero, screenDC);
    // }
        
//     public static unsafe void AdjustAreaSize(in uint style,in bool menu,ref uint left, ref uint top, ref uint width,ref uint height)
//     {
//         Win32.RECT rect = new Win32.RECT((int)left, (int)top,  (int)(width + left) ,(int)(height + top) );

//         int err= AdjustWindowRect(&rect,style,menu);

//         width = (uint)(rect.Right - rect.Left);
//         height = (uint)(rect.Bottom - rect.Top);
//     }
        // public static void SetForeground(ref System.IntPtr foregroundLockTimeout)
        // {
        //     // To make SetForegroundWindow work as we want, we need to fiddle
        //     // with the FOREGROUNDLOCKTIMEOUT system setting (we do this as early
        //     // as possible in the hope of still being the foreground process)
        //      NativeVideo.SystemParametersInfo((int) NativeVideo.SPI.GETFOREGROUNDLOCKTIMEOUT, 0, foregroundLockTimeout, 0);
        //      NativeVideo.SystemParametersInfo((int) NativeVideo.SPI.SETFOREGROUNDLOCKTIMEOUT, 0, System.IntPtr.Zero,  NativeVideo.SPIF_SENDCHANGE);
        // }

/*
Place le thread qui a créé la fenêtre spécifiée au premier plan et active la fenêtre. La saisie au clavier est dirigée vers la fenêtre et divers repères visuels sont modifiés pour l’utilisateur. Le système attribue une priorité légèrement plus élevée au thread qui a créé la fenêtre de premier plan qu’aux autres threads.


// If the window is invisible we will show it and make it topmost without the
// foreground focus. If the window is visible it will also be made the
// topmost window without the foreground focus. If wParam is TRUE then
// for both cases the window will be forced into the foreground focus

if (uMsg == m_ShowStageMessage) {

    BOOL bVisible = IsWindowVisible(hwnd);
    SetWindowPos(hwnd, HWND_TOP, 0, 0, 0, 0,
                    SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW |
                    (bVisible ? SWP_NOACTIVATE : 0));

    // Should we bring the window to the foreground
    if (wParam == TRUE) {
        SetForegroundWindow(hwnd);
    }
    return (LRESULT) 1;
}
*/


    private unsafe static int Update(ref WindowData data ,ref WindowFunction funcs)
    {
        #if WIN64

        // fixed (MSG* msg = &data.Msg){
        //     data.IsRun = funcs.GetMessageA(msg, null, 0,  0)>0;
        //     
        //     _=funcs.TranslateMessage(msg);
        //     _=funcs.DispatchMessageA(msg);
        // } 

        fixed (MSG* msg = &data.Msg){
            while( funcs.PeekMessageA(msg,null,0,0,1) > 0 )  
            {
                data.IsRun =  msg->message != Constants. WM_QUIT ;
                _=funcs.TranslateMessage(msg);
                _=funcs.DispatchMessageA(msg);
            }
            // data.IsRun = funcs.GetMessageA(msg, null, 0,  0)>0;
            // data.IsRun = msg.Message == WM.QUIT;
            
        } 


        #else

        #endif
        return 0;
    }

    private unsafe static int Release(ref WindowData data ,ref WindowFunction funcs)
    {
        #if WIN64
        if (  data.Handle != null)
        {
            int err = funcs.DestroyWindow(data.Handle);//If the function fails, the return value is zero.
            // Log.WarnWhenConditionIsFalse(err !=0, $" Destroy Window If the function succeeds, the return value is nonzero => {err} whith HWND {new IntPtr(data.Handle)}");
            
            var EngineNameChars =Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);
            fixed( byte* ptr = &EngineNameChars[0]){ err = funcs.UnRegisterClassA(ptr, null );}
            // Log.WarnWhenConditionIsFalse(err !=0 ,  $" Unregister Window If the function succeeds, the return value is nonzero.=> {err}");
        }  
    
        Libraries.Unload( data.User32 );
        Libraries.Unload( data.Kernel );
        // data.Dispose();
        //func.Dispose();
        #else

        #endif
        return 0;
    }

    private unsafe static void Minimize(ref WindowData data ,ref WindowFunction funcs) 
    {
        #if WIN64
        funcs.ShowWindow(data.Handle , Constants.SW_MINIMIZE);
        #else   
            {}
        #endif
    }

    private unsafe static void Show(ref WindowData data ,ref WindowFunction funcs)
    { 
        #if WIN64
        funcs.ShowWindow(data.Handle, Constants.SW_SHOW );
        funcs.UpdateWindow(data.Handle );

        //bring to top  set foreground
        #else

        #endif
    }

    private static bool ShouldClose(ref WindowData data ,ref WindowFunction funcs) =>  data.IsRun;

    private unsafe static void RequestClose(ref WindowData data ,ref WindowFunction funcs )
    {   
        #if WIN64   
        funcs.PostQuitMessage(0);
        data.IsRun = false; 
        #endif
    }
    #endregion

    #region OVERRIDE    
    public override string ToString() => string.Format($"Data Window " );
    public override int GetHashCode() => HashCode.Combine( _data.GetHashCode(), _funcs.GetHashCode());
    public override bool Equals(object? obj) => obj is Window  window && this.Equals(window) ;
    public bool Equals(Window other)=>  _data.Equals(other._data) ;
    public static bool operator ==(Window  left,Window right) => left.Equals(right);
    public static bool operator !=(Window  left,Window right) => !left.Equals(right);
    #endregion
}

