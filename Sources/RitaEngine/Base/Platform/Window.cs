namespace RitaEngine.Base.Platform ;

using System.Collections.Generic;
using RitaEngine.Base.Platform.API.Window;
using RitaEngine.Base.Platform.Structures;
using static RitaEngine.Base.MemoryHelper;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public struct Window : IEquatable<Window>
{
    public delegate void PFN_Clock( );
    public PFN_Clock clockPause = null!;
    public PFN_Clock clockStart = null!;
    private WindowData _data = new();
    private WindowFunction _funcs;
    public WindowEvent Events = new();
    
    public Window(){  }
    public bool IsLostFocus { get ; private set;} = false;
    public unsafe void* GetWindowHandle() => _data.Handle ;
    public unsafe void* GetWindowHInstance() => _data.HInstance ;
    public unsafe int GetWindowWidth() => _data.Width ;
    public unsafe int GetWindowheight() => _data.Height ;
    public byte[] GetWindowName() => _data.Title ;
    public void ChangeTitleBarCaption(string title) => WindowImplement.UpdateCaptionTitleBar( ref _data , ref _funcs , title);

    public unsafe static int LOWORD( nint lParam ) =>  (int)((nint)lParam & 0xFFFF);
	public unsafe static int HIWORD( nint lParam ) =>  (int)((nint)lParam >> 16);
	public unsafe static int GET_X_LPARAM(nint lp) => (int)(short)LOWORD(lp);
	public unsafe static int GET_Y_LPARAM(nint lp) => (int)(short)HIWORD(lp)   ;
    public static int MakeLong (short lowPart, short highPart) => (int)(((ushort)lowPart) | (uint)(highPart << 16));
    public static int MakeWord (short lowPart, short highPart) => (int)(((ushort)lowPart) | (uint)(highPart << 16));


    public void GetFrameBuffer(ref uint width , ref uint height)
    {
        unsafe{
        RECT rect = new();
        _funcs.GetClientRect( _data.Handle,&rect);
        width = (uint)rect.Right;
        height = (uint)rect.Bottom;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Init(in PlatformConfig config, in Clock clock)
    {
        clockPause = clock.Pause;
        clockStart = clock.Start;

        UpadateData(config);

        WindowImplement.MonitorsInfo(ref _data, ref _funcs);

        WindowImplement.AdjustSize(ref _data, ref _funcs);

        WindowImplement.Create(ref _data, ref _funcs);
    }

    private unsafe void UpadateData(PlatformConfig config)
    {
        _data.User32 = Libraries.Load(config.LibraryName_User32);
        _data.Kernel = Libraries.Load(config.LibraryName_Kernel);
        _data.Gdi = Libraries.Load(config.LibraryName_Gdi);
        _funcs = new(Libraries.GetUnsafeSymbol, _data.User32, _data.Kernel, _data.Gdi);
        _data.WndProc = this.WndProc2;
        (_data.Width, _data.Height) = PlatformConfig.GetResolution(config.Window_Resolution);
        _data.Left = Constants.CW_USEDEFAULT;
        _data.Top = Constants.CW_USEDEFAULT;
        _data.Style = Constants.WS_CAPTION | Constants.WS_SYSMENU | /*Constants.WS_VISIBLE |*/ Constants.WS_THICKFRAME;
        _data.ExStyle = Constants.WS_EX_APPWINDOW | Constants.WS_EX_WINDOWEDGE;
        _data.HInstance = _funcs.GetModuleHandleA(null);// Marshal.GetHINSTANCE(typeof(WindowData).Module).ToPointer(); 
        _data.Title = Encoding.UTF8.GetBytes(config.Game_Title);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Show() => WindowImplement.Show(ref _data , ref _funcs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DispatchPending() => WindowImplement.DispatchPending(ref _data , ref _funcs);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PoolEvents() => WindowImplement.Update(ref _data , ref _funcs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ShouldClose() => WindowImplement.ShouldClose(ref _data );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RequestClose() =>WindowImplement.RequestClose(ref _data , ref _funcs);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Release()
    {
        WindowImplement.Release(ref _data , ref _funcs);
        Events.Release();
    } 

    #region STATIC IMPLEMENT
    
    /// <summary>
	///  How to send a key 
	/// Exemples : SendMessage(EventsMessage.KEYDOWN, (uint)EventsKeyCode.VK_SPACE	,0 );
	/// </summary>
	/// <param name="Wm_messageType"></param>
	/// <param name="highValue"></param>
	/// <param name="lowValue"></param>
	public unsafe void SendMessage( TypeOfMessageToSend Wm_messageType , uint highValue=0 , int lowValue=0)
	{
		_funcs.SendMessageA( _data.Handle ,(uint)Wm_messageType,highValue, lowValue  );
	}

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
		Events.OnSetFocus(wParam,lParam);
		IsLostFocus =false;
        clockStart();
		return _funcs.DefWindowProcA(hWnd, message, wParam, lParam);
	}

	private  unsafe nint Win32OnKillFocus(void* hWnd, uint message, nuint wParam, nint lParam)
	{
		Events.OnKillFocus!(wParam,lParam);
		IsLostFocus = true;
        clockPause();
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

   #endregion

    #region OVERRIDE    
    public override string ToString() => string.Format($"Window " );
    public override int GetHashCode() => HashCode.Combine( _data.GetHashCode(), _funcs.GetHashCode());
    public override bool Equals(object? obj) => obj is Window  window && this.Equals(window) ;
    public bool Equals(Window other)=>  _data.Equals(other._data) ;
    public static bool operator ==(Window  left,Window right) => left.Equals(right);
    public static bool operator !=(Window  left,Window right) => !left.Equals(right);
    #endregion
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class WindowImplement
{
    public unsafe static void UpdateCaptionTitleBar(ref WindowData data ,ref WindowFunction funcs, string title)
    {
        var bytes = Encoding.UTF8.GetBytes(title).AsSpan();// byte* title = RitaEngine.Base.MemoryHelper.AsPointer<byte[]>(ref bytes);
        int result = funcs.SetWindowTextA(data.Handle , bytes.GetPointer() );
        Log.WarnWhenConditionIsFalse( result !=0 ,$"Change caption title to {title}");
    }
    
    public unsafe static int Create(ref WindowData data ,ref WindowFunction funcs)
    {
        #if WIN64

        var EngineNameChars = Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);

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
        
        ushort err = funcs.RegisterClassExA(wcex);
        Log.WarnWhenConditionIsFalse(err !=0 , "Register Class" );
    
        fixed( byte* ptr = &EngineNameChars[0] , app = &data.Title[0] ) 
        {
        data.Handle = funcs.CreateWindowExA(  data.ExStyle,
            ptr,  app,
            data.Style,
            data.Left, data.Top,  data.Width,  data.Height,
            null, null,data.HInstance, null );
        }
        Log.Info($"Create Window :  {(int)data.Handle:X}");
        #else

        #endif
    
        return 0;
    }

    public unsafe static void MonitorsInfo(ref WindowData data ,ref WindowFunction funcs)
    {
        MONITORINFOEX target= new();
        target.Size = MONITORINFOEX.SizeInBytes;

        void* hMon = funcs.MonitorFromWindow( funcs.GetDesktopWindow(), Constants.MONITOR_DEFAULTTOPRIMARY);
        funcs.GetMonitorInfoW(hMon, &target);
        // string asStrin = new string((char *) target.DeviceName, 0, 32);
        string deviceName = new string(target.DeviceName);
        Log.Info($"Monitor Name : {deviceName}");
        data.IsPrimaryMonitor = target.Flags == 1 ;

        DEVMODEW devmod = new();
        devmod.dmSize = DEVMODEW.SizeInBytes;

        // GET CURRENT MODE
        int err = funcs.EnumDisplaySettingsW( (char *) target.DeviceName,Constants.ENUM_CURRENT_SETTINGS,&devmod );
        Log.WarnWhenConditionIsFalse( err != 0 ,"Enum Display GET CURRENT MODE");

        err =  funcs.EnumDisplaySettingsW( (char *) target.DeviceName,0,&devmod );
        Log.WarnWhenConditionIsFalse( err != 0 ,"Enum Display Error Id 0");
        err =  funcs.EnumDisplaySettingsW(  (char *) target.DeviceName,1,&devmod );
        Log.WarnWhenConditionIsFalse( err != 0 ,"Enum Display Error Id 1");
        err =  funcs.EnumDisplaySettingsW( (char *) target.DeviceName,2,&devmod );
        Log.WarnWhenConditionIsFalse( err != 0 ,"Enum Display Error Id 2");
        //  If the function fails, the return value is zero. The function fails if iDevNum is greater than the largest device index
        //  This setting is the screen DPI, or dots per inch.
        // 
        //  GET SCALING FACTOR 
        var hdc =  funcs.GetDC(null) ;   
        int LogicalScreenHeight = funcs.GetDeviceCaps( hdc, (int)DEVICE_CAP.VERTRES  ) ;
        int PhysicalScreenHeight = funcs.GetDeviceCaps( hdc, (int)DEVICE_CAP.DESKTOPVERTRES  ); 
        int LogicalScreenWidth = funcs.GetDeviceCaps( hdc, (int)DEVICE_CAP.HORZRES  ) ;
        int PhysicalScreenWidth = funcs.GetDeviceCaps( hdc, (int)DEVICE_CAP.DESKTOPHORZRES  ); 
        
        float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
        int Xdpi = funcs.GetDeviceCaps(hdc, (int)DEVICE_CAP.LOGPIXELSX );
        int Ydpi = funcs.GetDeviceCaps(hdc, (int)DEVICE_CAP.LOGPIXELSY );    
    
        funcs.ReleaseDC(null, hdc);
            // ScreenScalingFactor; // 1.25 = 125%
            
    }

    
    public unsafe  static void AdjustSize(ref WindowData data ,ref WindowFunction funcs)
    {
    #if WIN64

        RECT rect = new((int)data.Left, (int)data.Top,  (int)(data.Width + data.Left) ,(int)(data.Height + data.Top) );
        
        _= funcs.AdjustWindowRect(&rect,data.Style,0);
        
        data.Width = (rect.Right - rect.Left);
        data.Height = (rect.Bottom - rect.Top);
        Log.Info($"Adjust Win RECT [Width={ data.Width},Height={ data.Height}, top= { data.Top}left={ data.Left}, style { data.Style}]");

    #else
    
    #endif        
    }
 
    // /// <summary>
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

    public unsafe static int Update(ref WindowData data ,ref WindowFunction funcs)
    {
        #if WIN64

        fixed (MSG* msg = &data.Msg){
            data.IsRun = funcs.GetMessageA(msg, null, 0,  0)>0;
            
            _=funcs.TranslateMessage(msg);
            _=funcs.DispatchMessageA(msg);
        } 

        #else

        #endif
        return 0;
    }

    public unsafe static int DispatchPending(ref WindowData data ,ref WindowFunction funcs)
    {
        #if WIN64

        fixed (MSG* msg = &data.Msg){
            while( funcs.PeekMessageA(msg,null,0,0,1) > 0 )  
            {
                data.IsRun =  msg->message != Constants. WM_QUIT ;
                _=funcs.TranslateMessage(msg);
                _=funcs.DispatchMessageA(msg);
            }
            
        } 
        #else

        #endif
        return 0;
    }

    public unsafe static int Release(ref WindowData data ,ref WindowFunction funcs)
    {
        #if WIN64
        if (  data.Handle != null)
        {
            int err = funcs.DestroyWindow(data.Handle);//If the function fails, the return value is zero.
            Log.WarnWhenConditionIsFalse(err !=0, $" Destroy Window If the function succeeds, the return value is nonzero => {err} whith HWND {new IntPtr(data.Handle)}");
            
            var EngineNameChars =Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);
            fixed( byte* ptr = &EngineNameChars[0]){ err = funcs.UnRegisterClassA(ptr, null );}
            Log.WarnWhenConditionIsFalse(err !=0 ,  $" Unregister Window If the function succeeds, the return value is nonzero.=> {err}");
        }  
    
        Libraries.Unload( data.User32 );
        Libraries.Unload( data.Kernel );
        data.Release();
        //func.Dispose();
        #else

        #endif
        return 0;
    }

    public unsafe static void Minimize(ref WindowData data ,ref WindowFunction funcs) 
    {
        #if WIN64
        funcs.ShowWindow(data.Handle , Constants.SW_MINIMIZE);
        #else   
            {}
        #endif
    }

    public unsafe static void Show(ref WindowData data ,ref WindowFunction funcs)
    { 
        #if WIN64
        funcs.ShowWindow(data.Handle, Constants.SW_SHOW );
        funcs.UpdateWindow(data.Handle );

        //bring to top  set foreground
        #else

        #endif
    }

    public static bool ShouldClose(ref WindowData data ) =>  data.IsRun;

    public unsafe static void RequestClose(ref WindowData data ,ref WindowFunction funcs )
    {   
        #if WIN64   
        funcs.PostQuitMessage(0);
        data.IsRun = false; 
        #endif
    }
 
}