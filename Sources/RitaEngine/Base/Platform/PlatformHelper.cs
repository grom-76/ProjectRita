

namespace RitaEngine.Base.Platform;

using System.IO;
using System.Runtime;

public unsafe delegate nint PFN_WNDPROC(void* hWnd,uint msg,nuint wParam, nint lpParam );
public unsafe delegate void* PFN_GetSymbolPointer(nint module , string name);


[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class PlatformHelper
{
    public enum GarbageCollectionPriority
    {
        Normal =1 ,
        High =3,
        Desactive=0,
        Middle =2 ,
    }

    public enum PlatformDevice : int
    {
        AutoDetect = -1,
        Unknow =0,
        Window =1,
        Linux =2,
        Mac =3,
        IOS =4,
        Android =5,
        WEB =6,
        PS2 =7,
        NES =8,
        SWITCH =9,
        MEGADRIVE =10,
        UWP=11,
        ATARIST,

    }


    public static GarbageCollectionPriority GCPriority = GarbageCollectionPriority.High;
    public static string AssetsPath = Path.Combine(AppContext.BaseDirectory, "Assets");
    // public static string RootPath = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location)!; // bug with AOT
    public static string WorkingDirectory = Environment.CurrentDirectory;
    public static int MemoryPressure = 1024*1024*40;
    public static PlatformDevice SelectedPlatform = PlatformDevice.AutoDetect ;
    public static bool UseMemoryPressure = false;

        
     // // #define LOBYTE(w)              ((BYTE)((DWORD_PTR)(w) & 0xFF))
// // #define HIBYTE(w)              ((BYTE)((DWORD_PTR)(w) >> 8))
// // #define MAKEWORD(low,high)     ((WORD)(((BYTE)((DWORD_PTR)(low) & 0xFF)) | ((WORD)((BYTE)((DWORD_PTR)(high) & 0xFF))) << 8))
// // #define MAKELONG(low,high)     ((LONG)(((WORD)((DWORD_PTR)(low) & 0xFFFF)) | ((DWORD)((WORD)((DWORD_PTR)(high) & 0xFFFF))) << 16))
    public unsafe static int LOWORD( nint lParam ) =>  (int)((nint)lParam & 0xFFFF);
	public unsafe static int HIWORD( nint lParam ) =>  (int)((nint)lParam >> 16);
	public unsafe static int GET_X_LPARAM(nint lp) => (int)(short)LOWORD(lp);
	public unsafe static int GET_Y_LPARAM(nint lp) => (int)(short)HIWORD(lp)   ;
    public static int MakeLong (short lowPart, short highPart) => (int)(((ushort)lowPart) | (uint)(highPart << 16));
    public static int MakeWord (short lowPart, short highPart) => (int)(((ushort)lowPart) | (uint)(highPart << 16));

    public static void Init( )
    {   
        if ( UseMemoryPressure && MemoryPressure > (1024*1024)){
            GCSettings.LatencyMode = (GCLatencyMode)GCPriority;
            GC.AddMemoryPressure(MemoryPressure);
        }
        else
        {
            MemoryPressure = 0;
        }
    }

    public static void Release()
    {
        if ( MemoryPressure > 0){
            GCSettings.LatencyMode = (GCLatencyMode.SustainedLowLatency);
            GC.RemoveMemoryPressure(MemoryPressure );
        }

        
    }

    public static bool IsBigEndian = !System.BitConverter.IsLittleEndian;

    public static string GetNativePath(string asset, string libraryName)
    {
        // string osPlatform = GetPlatform();

        // string architecture = GetArchitecture(); // use Files 

        string[] paths = new[]{
            System.IO.Path.Combine(asset, libraryName),
            // Path.Combine(EngineFolder, "libs",osPlatform, libraryName),
            // Path.Combine(EngineFolder, "runtimes", osPlatform, "native", libraryName),
            // Path.Combine(EngineFolder, "runtimes", $"{osPlatform}-{architecture}", "native", libraryName),
            // Path.Combine(EngineFolder, "native", $"{osPlatform}-{architecture}", libraryName),

            System.IO.Path.Combine(Environment.SystemDirectory, "" ,libraryName ),
            System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64" ,libraryName )

            };

        // foreach (string path in paths)
        // {
        //     // if (File.Exists(path))
        //     // {
        //     //     return path;
        //     // }
        // }

        return libraryName;
    }

        /// <summary> Get the App Archtecture used </summary>
        /// <returns></returns>
    public static string GetArchitecture()
        => RuntimeInformation.ProcessArchitecture switch{
                Architecture.X86 => "x86",
                Architecture.X64 => "x64",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => throw new ArgumentException("Unsupported architecture."),
        };
    
   

    public static string ToName(PlatformDevice platform)
        => platform switch {
            PlatformDevice.Unknow => StrUnknow,
            PlatformDevice.Window=> StrWindow,
            PlatformDevice.Linux=> StrLinux,
            PlatformDevice.Mac=>StrMac,
            PlatformDevice.IOS=> StrIOS,
            PlatformDevice.Android=>StrAndroid,
            PlatformDevice.WEB=>StrWEB,
            PlatformDevice.PS2=>StrPS2,
            PlatformDevice.NES=>StrNES ,
            PlatformDevice.SWITCH=>StrSWITCH,
            PlatformDevice.MEGADRIVE=>StrMEGADRIVE,
            _ => StrUnknow};

    public static PlatformDevice ToId( string platformName)
        => platformName switch{
            StrUnknow => PlatformDevice.Unknow,
            StrWindow =>PlatformDevice.Window,
            StrLinux =>PlatformDevice.Linux,
            StrMac =>PlatformDevice.Mac,
            StrIOS =>PlatformDevice.IOS,
            StrAndroid =>PlatformDevice.Android,
            StrWEB =>PlatformDevice.WEB,
            StrSWITCH  =>PlatformDevice.SWITCH,
            StrNES =>PlatformDevice.NES,
            StrMEGADRIVE => PlatformDevice.MEGADRIVE,
            _ =>PlatformDevice.Unknow
        };

    private const string StrUnknow ="Unknown";
    private const string StrWindow ="Window";
    private const string StrLinux ="Linux";
    private const string StrMac ="Mac";
    private const string StrIOS ="IOS";
    private const string StrAndroid ="Android";
    private const string StrWEB ="WEB";
    private const string StrPS2 ="SONY PS2";
    private const string StrNES ="NES";
    private const string StrSWITCH ="Switch";
    private const string StrMEGADRIVE ="SEGA MegaDrive";      

    public static bool IsAndroid()
    {
        if (System.IO.File.Exists(@"/proc/sys/kernel/ostype"))
        {
            string osType = System.IO.File.ReadAllText(@"/proc/sys/kernel/ostype");
            if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
            {
                var arch = RuntimeInformation.OSArchitecture;
                switch (arch)
                {
                    case Architecture.Arm:
                    case Architecture.Arm64:
                        return true;
                }
            }
        }

        return false;
    }

    public static bool IsIOS()
    {
        if (System.IO.File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
        {
            var arch = RuntimeInformation.OSArchitecture;
            switch (arch)
            {
                case Architecture.Arm:
                case Architecture.Arm64:
                    return true;
            }

        }

        return false;
    }
    /// <summary> Get the os platform using </summary>
    /// <returns></returns>
    public  static PlatformDevice AuoDetectPlatformUsed()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return PlatformDevice.Window;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return PlatformDevice.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return PlatformDevice.Mac;
        else if( IsAndroid() )
            return PlatformDevice.Android;
        else if( IsIOS() )
            return PlatformDevice.IOS;             
    else 
            return PlatformDevice.Unknow;
    }


} // CPU ,BigEndian,MemoryPressure, GarbageCollector,  architecture, Platform, ... absolut path
    

