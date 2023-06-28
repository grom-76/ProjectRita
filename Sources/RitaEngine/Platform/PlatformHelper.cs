namespace RitaEngine.Platform;

using System.IO;
using RitaEngine.Base;

public unsafe delegate nint PFN_WNDPROC(void* hWnd,uint msg,nuint wParam, nint lpParam );
public unsafe delegate void* PFN_GetSymbolPointer(nint module , string name);


[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class PlatformHelper
{

    public enum PlatformName : int
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

    public static string AssetsPath = Path.Combine(AppContext.BaseDirectory, "Assets");
    // public static string RootPath = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location)!; // bug with AOT
    public static string WorkingDirectory = Environment.CurrentDirectory;
    //
    public static PlatformName SelectedPlatform = PlatformName.AutoDetect ;

    public static readonly bool IsBigEndian = !System.BitConverter.IsLittleEndian;

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

    // public static string ToName(PlatformName platform)
    //     => platform switch {
    //         PlatformName.Unknow => StrUnknow,
    //         PlatformName.Window=> StrWindow,
    //         PlatformName.Linux=> StrLinux,
    //         PlatformName.Mac=>StrMac,
    //         PlatformName.IOS=> StrIOS,
    //         PlatformName.Android=>StrAndroid,
    //         PlatformName.WEB=>StrWEB,
    //         PlatformName.PS2=>StrPS2,
    //         PlatformName.NES=>StrNES ,
    //         PlatformName.SWITCH=>StrSWITCH,
    //         PlatformName.MEGADRIVE=>StrMEGADRIVE,
    //         _ => StrUnknow};

    // public static PlatformName ToId( string platformName)
    //     => platformName switch{
    //         StrUnknow => PlatformName.Unknow,
    //         StrWindow =>PlatformName.Window,
    //         StrLinux =>PlatformName.Linux,
    //         StrMac =>PlatformName.Mac,
    //         StrIOS =>PlatformName.IOS,
    //         StrAndroid =>PlatformName.Android,
    //         StrWEB =>PlatformName.WEB,
    //         StrSWITCH  =>PlatformName.SWITCH,
    //         StrNES =>PlatformName.NES,
    //         StrMEGADRIVE => PlatformName.MEGADRIVE,
    //         _ =>PlatformName.Unknow
    //     };

    // private const string StrUnknow ="Unknown";
    // private const string StrWindow ="Window";
    // private const string StrLinux ="Linux";
    // private const string StrMac ="Mac";
    // private const string StrIOS ="IOS";
    // private const string StrAndroid ="Android";
    // private const string StrWEB ="WEB";
    // private const string StrPS2 ="SONY PS2";
    // private const string StrNES ="NES";
    // private const string StrSWITCH ="Switch";
    // private const string StrMEGADRIVE ="SEGA MegaDrive";      

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
    public  static PlatformName AuoDetectPlatformUsed()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return PlatformName.Window;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return PlatformName.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return PlatformName.Mac;
        else if( IsAndroid() )
            return PlatformName.Android;
        else if( IsIOS() )
            return PlatformName.IOS;             
        else 
            return PlatformName.Unknow;
    }


} // CPU  architecture, Platform, ... absolut path
    

