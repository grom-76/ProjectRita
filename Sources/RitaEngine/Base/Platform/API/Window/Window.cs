namespace RitaEngine.Platform.API.Window;

// HARDWARE , OS BACKEND , INTEROP , NATIVE

public static class Constants
{ 
    public const int CW_USEDEFAULT = unchecked((int)(0x80000000));

    public const uint CS_VREDRAW         = 0x0001;
    public const uint CS_HREDRAW         = 0x0002;
    public const uint CS_DBLCLKS         = 0x0008;
    public const uint CS_OWNDC           = 0x0020;
    public const uint CS_CLASSDC         = 0x0040;
    public const uint CS_PARENTDC        = 0x0080;
    public const uint CS_NOCLOSE         = 0x0200;
    public const uint CS_SAVEBITS        = 0x0800;
    public const uint CS_BYTEALIGNCLIENT = 0x1000;
    public const uint CS_BYTEALIGNWINDOW = 0x2000;
    public const uint CS_GLOBALCLASS     = 0x4000;
    public const uint CS_IME             = 0x00010000;
    public const uint CS_DROPSHADOW      = 0x00020000;

    public const uint WS_OVERLAPPED = 0x0;
    public const uint WS_MAXIMIZEBOX = 0x10000;
    public const uint WS_TABSTOP = WS_MAXIMIZEBOX;
    public const uint WS_GROUP = 0x20000;
    public const uint WS_MINIMIZEBOX = WS_GROUP;
    public const uint WS_SIZEFRAME = 0x40000;
    public const uint WS_SYSMENU = 0x80000;
    public const uint WS_HSCROLL = 0x100000;
    public const uint WS_VSCROLL = 0x200000;
    public const uint WS_DLGFRAME = 0x400000;
    public const uint WS_BORDER = 0x800000;
    public const uint WS_CAPTION = WS_DLGFRAME | WS_BORDER;
    public const uint WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
    public const uint WS_MAXIMIZE = 0x1000000;
    public const uint WS_CLIPCHILDREN = 0x2000000;
    public const uint WS_CLIPSIBLINGS = 0x4000000;
    public const uint WS_DISABLED = 0x8000000;
    public const uint WS_VISIBLE = 0x10000000;
    public const uint WS_MINIMIZE = 0x20000000;
    public const uint WS_CHILD = 0x40000000;
    public const uint WS_POPUP = 0x80000000u;
    public const uint WS_THICKFRAME       = 0x00040000;
    public const uint WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;
    public const uint WS_TILED        = WS_OVERLAPPED;
    public const uint WS_ICONIC       = WS_MINIMIZE;
    public const uint WS_SIZEBOX      = WS_THICKFRAME;
    public const uint WS_TILEDWINDOW      = WS_OVERLAPPEDWINDOW;

    public const uint WS_EX_None = 0;
    public const uint WS_EX_LEFT =WS_EX_None;
    public const uint WS_EX_LTRREADING = WS_EX_None;
    public const uint WS_EX_RIGHTSCROLLBAR = WS_EX_None;
    public const uint WS_EX_DLGMODALFRAME = 0x00000001;
    public const uint WS_EX_NOPARENTNOTIFY = 0x00000004;
    public const uint WS_EX_TOPMOST = 0x00000008;
    public const uint WS_EX_ACCEPTFILES = 0x00000010;
    public const uint WS_EX_TRANSPARENT = 0x00000020;
    public const uint WS_EX_MDICHILD = 0x00000040;
    public const uint WS_EX_TOOLWINDOW = 0x00000080;
    public const uint WS_EX_WINDOWEDGE = 0x00000100;
    public const uint WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST;
    public const uint WS_EX_CLIENTEDGE = 0x00000200;
    public const uint WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE;
    public const uint WS_EX_CONTEXTHELP = 0x00000400;
    public const uint WS_EX_RIGHT = 0x00001000;
    public const uint WS_EX_RTLREADING = 0x00002000;
    public const uint WS_EX_LEFTSCROLLBAR = 0x00004000;
    public const uint WS_EX_WCONTROLPARENT = 0x00010000;
    public const uint WS_EX_STATICEDGE = 0x00020000;
    public const uint WS_EX_APPWINDOW = 0x00040000;
    public const uint WS_EX_LAYERED = 0x00080000;
    public const uint WS_EX_NOINHERITLAYOUT = 0x00100000;
    public const uint WS_EX_LAYOUTRTL = 0x00400000;
    public const uint WS_EX_COMPOSITED = 0x02000000;
    public const uint WS_EX_NOACTIVATE = 0x08000000;
    #region ICON
    public const int IDI_APPLICATION = 32512, IDI_HAND = 32513, IDI_QUESTION = 32514,
        IDI_EXCLAMATION = 32515, IDI_ASTERISK = 32516, IDI_WINLOGO = 32517, IDI_WARNING = IDI_EXCLAMATION,
        IDI_ERROR = IDI_HAND, IDI_INFORMATION = IDI_ASTERISK;
        #endregion
    public const int IDC_ARROW = 32512, IDC_IBEAM = 32513, IDC_WAIT = 32514,
        IDC_CROSS = 32515, IDC_UPARROW = 32516, IDC_SIZE = 32640, IDC_ICON = 32641,
        IDC_SIZENWSE = 32642, IDC_SIZENESW = 32643, IDC_SIZEWE = 32644, IDC_SIZENS = 32645,
        IDC_SIZEALL = 32646, IDC_NO = 32648, IDC_HAND = 32649, IDC_APPSTARTING = 32650, IDC_HELP = 32651;

    public const int SW_HIDE = 0,  SW_SHOWNORMAL =1, SW_NORMAL=1, SW_SHOWMINIMIZED =2,
        SW_SHOWMAXIMIZED =3, SW_MAXIMIZE =3, SW_SHOWNOACTIVATE=4, SW_SHOW=5, SW_MINIMIZE=6,
        SW_SHOWMINNOACTIVE=7, SW_SHOWNA =8, SW_RESTORE=9, SW_SHOWDEFAULT=10,  SW_FORCEMINIMIZE=11;

    public const  uint WM_INPUT =  0x00FF;
    public const uint WM_ACTIVATE           = 0x0006;
    public const uint WM_ACTIVATEAPP        = 0x001C;
    public const uint WM_AFXFIRST           = 0x0360;
    public const uint WM_AFXLAST        = 0x037F;
    public const uint WM_APP            = 0x8000;
    public const uint WM_ASKCBFORMATNAME    = 0x030C;
    public const uint WM_CANCELJOURNAL      = 0x004B;
    public const uint WM_CANCELMODE         = 0x001F;
    public const uint WM_CAPTURECHANGED     = 0x0215;
    public const uint WM_CHANGECBCHAIN      = 0x030D;
    public const uint WM_CHANGEUISTATE      = 0x0127;
    public const uint WM_CHAR           = 0x0102;
    public const uint WM_CHARTOITEM         = 0x002F;
    public const uint WM_CHILDACTIVATE      = 0x0022;
    public const uint WM_CLEAR          = 0x0303;
    public const uint WM_CLOSE          = 0x0010;
    public const uint WM_CLIPBOARDUPDATE    = 0x031D;
    public const uint WM_COMMAND        = 0x0111;
    public const uint WM_COMPACTING         = 0x0041;
    public const uint WM_COMPAREITEM        = 0x0039;
    public const uint WM_CONTEXTMENU        = 0x007B;
    public const uint WM_COPY           = 0x0301;
    public const uint WM_COPYDATA           = 0x004A;
    public const uint WM_CREATE         = 0x0001;
    public const uint WM_CTLCOLORBTN        = 0x0135;
    public const uint WM_CTLCOLORDLG        = 0x0136;
    public const uint WM_CTLCOLOREDIT       = 0x0133;
    public const uint WM_CTLCOLORLISTBOX    = 0x0134;
    public const uint WM_CTLCOLORMSGBOX     = 0x0132;
    public const uint WM_CTLCOLORSCROLLBAR      = 0x0137;
    public const uint WM_CTLCOLORSTATIC     = 0x0138;
    public const uint WM_CUT            = 0x0300;
    public const uint WM_DEADCHAR           = 0x0103;
    public const uint WM_DELETEITEM         = 0x002D;
    public const uint WM_DESTROY        = 0x0002;
    public const uint WM_DESTROYCLIPBOARD       = 0x0307;
    public const uint WM_DEVICECHANGE       = 0x0219;
    public const uint WM_DEVMODECHANGE      = 0x001B;
    public const uint WM_DISPLAYCHANGE      = 0x007E;
    public const uint WM_DRAWCLIPBOARD      = 0x0308;
    public const uint WM_DRAWITEM           = 0x002B;
    public const uint WM_DROPFILES          = 0x0233;
    public const uint WM_ENABLE         = 0x000A;
    public const uint WM_ENDSESSION         = 0x0016;
    public const uint WM_ENTERIDLE          = 0x0121;
    public const uint WM_ENTERMENULOOP      = 0x0211;
    public const uint WM_ENTERSIZEMOVE      = 0x0231;
    public const uint WM_ERASEBKGND         = 0x0014;
    public const uint WM_EXITMENULOOP       = 0x0212;
    public const uint WM_EXITSIZEMOVE       = 0x0232;
    public const uint WM_FONTCHANGE         = 0x001D;
    public const uint WM_GETDLGCODE         = 0x0087;
    public const uint WM_GETFONT        = 0x0031;
    public const uint WM_GETHOTKEY          = 0x0033;
    public const uint WM_GETICON        = 0x007F;
    public const uint WM_GETMINMAXINFO      = 0x0024;
    public const uint WM_GETOBJECT          = 0x003D;
    public const uint WM_GETTEXT        = 0x000D;
    public const uint WM_GETTEXTLENGTH      = 0x000E;
    public const uint WM_HANDHELDFIRST      = 0x0358;
    public const uint WM_HANDHELDLAST       = 0x035F;
    public const uint WM_HELP           = 0x0053;
    public const uint WM_HOTKEY         = 0x0312;
    public const uint WM_HSCROLL        = 0x0114;
    public const uint WM_HSCROLLCLIPBOARD       = 0x030E;
    public const uint WM_ICONERASEBKGND     = 0x0027;
    public const uint WM_IME_CHAR           = 0x0286;
    public const uint WM_IME_COMPOSITION    = 0x010F;
    public const uint WM_IME_COMPOSITIONFULL    = 0x0284;
    public const uint WM_IME_CONTROL        = 0x0283;
    public const uint WM_IME_ENDCOMPOSITION     = 0x010E;
    public const uint WM_IME_KEYDOWN        = 0x0290;
    public const uint WM_IME_KEYLAST        = 0x010F;
    public const uint WM_IME_KEYUP          = 0x0291;
    public const uint WM_IME_NOTIFY         = 0x0282;
    public const uint WM_IME_REQUEST        = 0x0288;
    public const uint WM_IME_SELECT         = 0x0285;
    public const uint WM_IME_SETCONTEXT     = 0x0281;
    public const uint WM_IME_STARTCOMPOSITION   = 0x010D;
    public const uint WM_INITDIALOG         = 0x0110;
    public const uint WM_INITMENU           = 0x0116;
    public const uint WM_INITMENUPOPUP      = 0x0117;
    public const uint WM_INPUTLANGCHANGE    = 0x0051;
    public const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
    public const uint WM_KEYDOWN        = 0x0100;
    public const uint WM_KEYFIRST           = 0x0100;
    public const uint WM_KEYLAST        = 0x0108;
    public const uint WM_KEYUP          = 0x0101;
    public const uint WM_KILLFOCUS          = 0x0008;
    public const uint WM_LBUTTONDBLCLK      = 0x0203;
    public const uint WM_LBUTTONDOWN        = 0x0201;
    public const uint WM_LBUTTONUP          = 0x0202;
    public const uint WM_MBUTTONDBLCLK      = 0x0209;
    public const uint WM_MBUTTONDOWN        = 0x0207;
    public const uint WM_MBUTTONUP          = 0x0208;
    public const uint WM_MDIACTIVATE        = 0x0222;
    public const uint WM_MDICASCADE         = 0x0227;
    public const uint WM_MDICREATE          = 0x0220;
    public const uint WM_MDIDESTROY         = 0x0221;
    public const uint WM_MDIGETACTIVE       = 0x0229;
    public const uint WM_MDIICONARRANGE     = 0x0228;
    public const uint WM_MDIMAXIMIZE        = 0x0225;
    public const uint WM_MDINEXT        = 0x0224;
    public const uint WM_MDIREFRESHMENU     = 0x0234;
    public const uint WM_MDIRESTORE         = 0x0223;
    public const uint WM_MDISETMENU         = 0x0230;
    public const uint WM_MDITILE        = 0x0226;
    public const uint WM_MEASUREITEM        = 0x002C;
    public const uint WM_MENUCHAR           = 0x0120;
    public const uint WM_MENUCOMMAND        = 0x0126;
    public const uint WM_MENUDRAG           = 0x0123;
    public const uint WM_MENUGETOBJECT      = 0x0124;
    public const uint WM_MENURBUTTONUP      = 0x0122;
    public const uint WM_MENUSELECT         = 0x011F;
    public const uint WM_MOUSEACTIVATE      = 0x0021;
    public const uint WM_MOUSEFIRST         = 0x0200;
    public const uint WM_MOUSEHOVER         = 0x02A1;
    public const uint WM_MOUSELAST          = 0x020D;
    public const uint WM_MOUSELEAVE         = 0x02A3;
    public const uint WM_MOUSEMOVE          = 0x0200;
    public const uint WM_MOUSEWHEEL         = 0x020A;
    public const uint WM_MOUSEHWHEEL        = 0x020E;
    public const uint WM_MOVE           = 0x0003;
    public const uint WM_MOVING         = 0x0216;
    public const uint WM_NCACTIVATE         = 0x0086;
    public const uint WM_NCCALCSIZE         = 0x0083;
    public const uint WM_NCCREATE           = 0x0081;
    public const uint WM_NCDESTROY          = 0x0082;
    public const uint WM_NCHITTEST          = 0x0084;
    public const uint WM_NCLBUTTONDBLCLK    = 0x00A3;
    public const uint WM_NCLBUTTONDOWN      = 0x00A1;
    public const uint WM_NCLBUTTONUP        = 0x00A2;
    public const uint WM_NCMBUTTONDBLCLK    = 0x00A9;
    public const uint WM_NCMBUTTONDOWN      = 0x00A7;
    public const uint WM_NCMBUTTONUP        = 0x00A8;
    public const uint WM_NCMOUSEHOVER       = 0x02A0;
    public const uint WM_NCMOUSELEAVE       = 0x02A2;
    public const uint WM_NCMOUSEMOVE        = 0x00A0;
    public const uint WM_NCPAINT        = 0x0085;
    public const uint WM_NCRBUTTONDBLCLK    = 0x00A6;
    public const uint WM_NCRBUTTONDOWN      = 0x00A4;
    public const uint WM_NCRBUTTONUP        = 0x00A5;
    public const uint WM_NCXBUTTONDBLCLK    = 0x00AD;
    public const uint WM_NCXBUTTONDOWN      = 0x00AB;
    public const uint WM_NCXBUTTONUP        = 0x00AC;
    public const uint WM_NCUAHDRAWCAPTION       = 0x00AE;
    public const uint WM_NCUAHDRAWFRAME     = 0x00AF;
    public const uint WM_NEXTDLGCTL         = 0x0028;
    public const uint WM_NEXTMENU           = 0x0213;
    public const uint WM_NOTIFY         = 0x004E;
    public const uint WM_NOTIFYFORMAT       = 0x0055;
    public const uint WM_NULL           = 0x0000;
    public const uint WM_PAINT          = 0x000F;
    public const uint WM_PAINTCLIPBOARD     = 0x0309;
    public const uint WM_PAINTICON          = 0x0026;
    public const uint WM_PALETTECHANGED     = 0x0311;
    public const uint WM_PALETTEISCHANGING      = 0x0310;
    public const uint WM_PARENTNOTIFY       = 0x0210;
    public const uint WM_PASTE          = 0x0302;
    public const uint WM_PENWINFIRST        = 0x0380;
    public const uint WM_PENWINLAST         = 0x038F;
    public const uint WM_POWER          = 0x0048;
    public const uint WM_POWERBROADCAST     = 0x0218;
    public const uint WM_PRINT          = 0x0317;
    public const uint WM_PRINTCLIENT        = 0x0318;
    public const uint WM_QUERYDRAGICON      = 0x0037;
    public const uint WM_QUERYENDSESSION    = 0x0011;
    public const uint WM_QUERYNEWPALETTE    = 0x030F;
    public const uint WM_QUERYOPEN          = 0x0013;
    public const uint WM_QUEUESYNC          = 0x0023;
    public const uint WM_QUIT           = 0x0012;
    public const uint WM_RBUTTONDBLCLK      = 0x0206;
    public const uint WM_RBUTTONDOWN        = 0x0204;
    public const uint WM_RBUTTONUP          = 0x0205;
    public const uint WM_RENDERALLFORMATS       = 0x0306;
    public const uint WM_RENDERFORMAT       = 0x0305;
    public const uint WM_SETCURSOR          = 0x0020;
    public const uint WM_SETFOCUS           = 0x0007;
    public const uint WM_SETFONT        = 0x0030;
    public const uint WM_SETHOTKEY          = 0x0032;
    public const uint WM_SETICON        = 0x0080;
    public const uint WM_SETREDRAW          = 0x000B;
    public const uint WM_SETTEXT        = 0x000C;
    public const uint WM_SETTINGCHANGE      = 0x001A;
    public const uint WM_SHOWWINDOW         = 0x0018;
    public const uint WM_SIZE           = 0x0005;
    public const uint WM_SIZECLIPBOARD      = 0x030B;
    public const uint WM_SIZING         = 0x0214;
    public const uint WM_SPOOLERSTATUS      = 0x002A;
    public const uint WM_STYLECHANGED       = 0x007D;
    public const uint WM_STYLECHANGING      = 0x007C;
    public const uint WM_SYNCPAINT          = 0x0088;
    public const uint WM_SYSCHAR        = 0x0106;
    public const uint WM_SYSCOLORCHANGE     = 0x0015;
    public const uint WM_SYSCOMMAND         = 0x0112;
    public const uint WM_SYSDEADCHAR        = 0x0107;
    public const uint WM_SYSKEYDOWN         = 0x0104;
    public const uint WM_SYSKEYUP           = 0x0105;
    public const uint WM_TCARD          = 0x0052;
    public const uint WM_TIMECHANGE         = 0x001E;
    public const uint WM_TIMER          = 0x0113;
    public const uint WM_UNDO           = 0x0304;
    public const uint WM_UNINITMENUPOPUP    = 0x0125;
    public const uint WM_USER           = 0x0400;
    public const uint WM_USERCHANGED        = 0x0054;
    public const uint WM_VKEYTOITEM         = 0x002E;
    public const uint WM_VSCROLL        = 0x0115;
    public const uint WM_VSCROLLCLIPBOARD       = 0x030A;
    public const uint WM_WINDOWPOSCHANGED       = 0x0047;
    public const uint WM_WINDOWPOSCHANGING      = 0x0046;
    public const uint WM_WININICHANGE       = 0x001A;
    public const uint WM_XBUTTONDBLCLK      = 0x020D;
    public const uint WM_XBUTTONDOWN        = 0x020B;
    public const uint WM_XBUTTONUP          = 0x020C;

    #region MONITOR INFO
    public const int MONITOR_DEFAULTTONULL = 0;
    public const int MONITOR_DEFAULTTOPRIMARY = 1;
    public const int MONITOR_DEFAULTTONEAREST = 2;

    public const int ENUM_CURRENT_SETTINGS = -1;
    public const int ENUM_REGISTRY_SETTINGS = -2;

       // dwFlags of EnumDisplaySettingsEx (default is 0)
    public const uint EDS_RAWMODE = 0x00000002;
    public const uint EDS_ROTATEDMODE = 0x00000004;

    // dwflags of ChangeDisplaySettingsEx (default is 0)
    public const uint CDS_UPDATEREGISTRY = 0x00000001;
    public const uint CDS_TEST = 0x00000002;
    public const uint CDS_FULLSCREEN = 0x00000004;
    public const uint CDS_GLOBAL = 0x00000008;
    public const uint CDS_SET_PRIMARY = 0x00000010;
    public const uint CDS_VIDEOPARAMETERS = 0x00000020;
    public const uint CDS_ENABLE_UNSAFE_MODES = 0x00000100;
    public const uint CDS_DISABLE_UNSAFE_MODES = 0x00000200;
    public const uint CDS_RESET = 0x40000000;
    public const uint CDS_RESET_EX = 0x20000000;
    public const uint CDS_NORESET = 0x10000000;

    // Result of ChangeDisplaySettingsEx
    public const int DISP_CHANGE_SUCCESSFUL = 0;   // The settings change was successful.
    public const int DISP_CHANGE_RESTART = 1;      // The computer must be restarted for the graphics mode to work.
    public const int DISP_CHANGE_FAILED = -1;      // The display driver failed the specified graphics mode.
    public const int DISP_CHANGE_BADMODE = -2;     // The graphics mode is not supported.
    public const int DISP_CHANGE_NOTUPDATED = -3;  // Unable to write settings to the registry.
    public const int DISP_CHANGE_BADFLAGS = -4;    // An invalid set of flags was passed in.
    public const int DISP_CHANGE_BADPARAM = -5;    // An invalid parameter was passed in. This can include an invalid flag or combination of flags.
    public const int DISP_CHANGE_BADDUALVIEW = -6; // The settings change was unsuccessful because the system is DualView capable.

    #endregion
}

public enum DMDO : uint
{
    /// <summary> The display orientation is the natural orientation of the display device. </summary>
    DMDO_DEFAULT = 0,
    /// <summary> The display orientation is rotated 90 degrees (measured clockwise) from DMDO_DEFAULT. </summary>
    DMDO_90 = 1,
    /// <summary> The display orientation is rotated 180 degrees (measured clockwise) from DMDO_DEFAULT. </summary>
    DMDO_180 = 2,
    /// <summary> The display orientation is rotated 270 degrees (measured clockwise) from DMDO_DEFAULT. </summary>
    DMDO_270 = 3,
}


public enum DEVICE_CAP
{
    /// <summary>
    /// Device driver version
    /// </summary>
    DRIVERVERSION = 0,
    /// <summary>
    /// Device classification
    /// </summary>
    TECHNOLOGY = 2,
    /// <summary>
    /// Horizontal size in millimeters
    /// </summary>
    HORZSIZE = 4,
    /// <summary>
    /// Vertical size in millimeters
    /// </summary>
    VERTSIZE = 6,
    /// <summary>
    /// Horizontal width in pixels
    /// </summary>
    HORZRES = 8,
    /// <summary>
    /// Vertical height in pixels
    /// </summary>
    VERTRES = 10,
    /// <summary>
    /// Number of bits per pixel
    /// </summary>
    BITSPIXEL = 12,
    /// <summary>
    /// Number of planes
    /// </summary>
    PLANES = 14,
    /// <summary>
    /// Number of brushes the device has
    /// </summary>
    NUMBRUSHES = 16,
    /// <summary>
    /// Number of pens the device has
    /// </summary>
    NUMPENS = 18,
    /// <summary>
    /// Number of markers the device has
    /// </summary>
    NUMMARKERS = 20,
    /// <summary>
    /// Number of fonts the device has
    /// </summary>
    NUMFONTS = 22,
    /// <summary>
    /// Number of colors the device supports
    /// </summary>
    NUMCOLORS = 24,
    /// <summary>
    /// Size required for device descriptor
    /// </summary>
    PDEVICESIZE = 26,
    /// <summary>
    /// Curve capabilities
    /// </summary>
    CURVECAPS = 28,
    /// <summary>
    /// Line capabilities
    /// </summary>
    LINECAPS = 30,
    /// <summary>
    /// Polygonal capabilities
    /// </summary>
    POLYGONALCAPS = 32,
    /// <summary>
    /// Text capabilities
    /// </summary>
    TEXTCAPS = 34,
    /// <summary>
    /// Clipping capabilities
    /// </summary>
    CLIPCAPS = 36,
    /// <summary>
    /// Bitblt capabilities
    /// </summary>
    RASTERCAPS = 38,
    /// <summary>
    /// Length of the X leg
    /// </summary>
    ASPECTX = 40,
    /// <summary>
    /// Length of the Y leg
    /// </summary>
    ASPECTY = 42,
    /// <summary>
    /// Length of the hypotenuse
    /// </summary>
    ASPECTXY = 44,
    /// <summary>
    /// Shading and Blending caps
    /// </summary>
    SHADEBLENDCAPS = 45,

    /// <summary>
    /// Logical pixels inch in X
    /// </summary>
    LOGPIXELSX = 88,
    /// <summary>
    /// Logical pixels inch in Y
    /// </summary>
    LOGPIXELSY = 90,

    /// <summary>
    /// Number of entries in physical palette
    /// </summary>
    SIZEPALETTE = 104,
    /// <summary>
    /// Number of reserved entries in palette
    /// </summary>
    NUMRESERVED = 106,
    /// <summary>
    /// Actual color resolution
    /// </summary>
    COLORRES = 108,

    // Printing related DeviceCaps. These replace the appropriate Escapes
    /// <summary>
    /// Physical Width in device units
    /// </summary>
    PHYSICALWIDTH = 110,
    /// <summary>
    /// Physical Height in device units
    /// </summary>
    PHYSICALHEIGHT = 111,
    /// <summary>
    /// Physical Printable Area x margin
    /// </summary>
    PHYSICALOFFSETX = 112,
    /// <summary>
    /// Physical Printable Area y margin
    /// </summary>
    PHYSICALOFFSETY = 113,
    /// <summary>
    /// Scaling factor x
    /// </summary>
    SCALINGFACTORX = 114,
    /// <summary>
    /// Scaling factor y
    /// </summary>
    SCALINGFACTORY = 115,

    /// <summary>
    /// Current vertical refresh rate of the display device (for displays only) in Hz
    /// </summary>
    VREFRESH = 116,
    /// <summary>
    /// Vertical height of entire desktop in pixels
    /// </summary>
    DESKTOPVERTRES = 117,
    /// <summary>
    /// Horizontal width of entire desktop in pixels
    /// </summary>
    DESKTOPHORZRES = 118,
    /// <summary>
    /// Preferred blt alignment
    /// </summary>
    BLTALIGNMENT = 119
}

public enum CursorInfoFlags : uint
{
    Hidden =0,
    CURSOR_SHOWING = 0x00000001,
    CURSOR_SUPPRESSED =0x00000002,
}

//Structures
[ StructLayout( LayoutKind.Sequential, Pack = 4)]
public struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT( int left, int top,int right, int bottom )
    {
        this.Left = left;
        this.Top = top;
        this.Right = right;
        this.Bottom = bottom;
    }
}

[StructLayout(LayoutKind.Sequential, Pack =4),SkipLocalsInit]
public unsafe struct WNDCLASSEXA
{
    public uint cbSize;
    public uint style;   
    public unsafe  PFN_WNDPROC lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public void* hInstance;
    public void* hIcon;
    public void* hCursor;
    public void* hbrBackground;
    public byte* lpszMenuName;
    public byte* lpszClassName;  // public fixed char lpszClassName[256];
    public void* hIconSm;
}

[ StructLayout( LayoutKind.Sequential),SkipLocalsInit]
public unsafe struct MSG
{

    public  void* hwnd;
    public  uint message;
    public  uint* wParam;
    public  long* lParam;
    public  ulong time;
    public POINT pt;
    public ulong lPrivate;
}

[ StructLayout( LayoutKind.Sequential, Pack = 4)]
public unsafe struct POINT
{
    public int X;
    public int Y;
    public POINT(int x, int y)  =>( this.X,this.Y ) = (x,y);
} 

[ StructLayout( LayoutKind.Sequential, Pack = 4)]
public unsafe struct CURSORINFO 
{
    public static readonly int CursorInfoSize = Unsafe.SizeOf<CURSORINFO>(); // System.Runtime.InteropServices.Marshal.SizeOf( typeof( CURSORINFO ) );
    public uint   cbSize;//MarshalSizeof<CURSORINFO>
    public CursorInfoFlags   flags;//0 MASKED , 1 SHOWING , 2 SUPRESSED
    public void* hCursor;
    public POINT   ptScreenPos;
}

[ StructLayout( LayoutKind.Sequential)]
public unsafe struct DEVMODEW
{
    public static readonly ushort SizeInBytes = (ushort)Unsafe.SizeOf<DEVMODEW>();
    public fixed ushort dmDeviceName[32];
    public ushort dmSpecVersion;
    public ushort dmDriverVersion;
    public ushort dmSize;
    public ushort dmDriverExtra;
    public uint dmFields;
    public int dmPositionX;
    public int dmPositionY;
    public int dmDisplayOrientation;
    public int dmDisplayFixedOutput;
    public short dmColor;
    public short dmDuplex;
    public short dmYResolution;
    public short dmTTOption;
    public short dmCollate;
    public fixed ushort dmFormName[32];
    public short dmLogPixels;
    public int dmBitsPerPel;
    public int dmPelsWidth;
    public int dmPelsHeight;
    public int dmDisplayFlags;
    public int dmDisplayFrequency;
    public int dmICMMethod;
    public int dmICMIntent;
    public int dmMediaType;
    public int dmDitherType;
    public int dmReserved1;
    public int dmReserved2;
    public int dmPanningWidth;
    public int dmPanningHeight;
}

[StructLayout( LayoutKind.Sequential )]
public struct MONITORINFOEX
{
    public static readonly uint SizeInBytes = (uint)  Unsafe.SizeOf<MONITORINFOEX>();
    public uint Size ;
    public RECT Monitor;
    public RECT Work;
    /// <summary>
    ///  Si = 1 This member can be the following value:   1 : MONITORINFOF_PRIMARY
    /// </summary>
    public uint Flags;
    public unsafe fixed char DeviceName[32];
}
