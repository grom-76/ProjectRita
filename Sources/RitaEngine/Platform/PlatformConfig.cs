namespace RitaEngine.Platform;

using System.Runtime;
using RitaEngine.Audio;
using RitaEngine.Graphic;
using RitaEngine.Input;
using static RitaEngine.Base.Log;


public sealed class PlatformConfig : IDisposable
{
    #if WIN64
    
    public string LibraryName_User32 = "User32.dll";
    public string LibraryName_Kernel =  "kernel32.dll";
    public string LibraryName_Gdi =  "Gdi32.dll";
    public string LibraryName_Vulkan = "vulkan-1.dll";
    public string LibraryName_xaudio = "xaudio2_9.dll";
    #endif

    #if WIN64
    public AudioDeviceBackEnd Audio_BackEnd = AudioDeviceBackEnd.Xaudio;
    public InputBackEnd Input_BackEnd = InputBackEnd.XInput;
    public GraphicDeviceBackend GraphicDevice_BackEnd = GraphicDeviceBackend.Vulkan;
    public GraphicDeviceClipVolume GraphicDevice_ClipVolume = GraphicDeviceClipVolume.ZeroToOne;
    public GraphicDeviceScreenOrigin GraphicDevice_ScreenOrigin =GraphicDeviceScreenOrigin.Center_Y_DownAxis;
    public GraphicDeviceNDC GraphicDevice_NDC = GraphicDeviceNDC.RightHand;

    #else
    
    public AudioDeviceBackEnd Audio_BackEnd = AudioDeviceBackEnd.None;

    #endif


    public  AudioChannels Audio_Channels = AudioChannels.stereo;
    public  AudioCategory Audio_Category =  AudioCategory.GameMedia;

  
    
    public ClockLoopMode Clock_LoopMode = ClockLoopMode.Default;
    public double Clock_FixedTimeStep =0.033;

    

    public int Controller_MaxController =  RitaEngine.API.DirectX.XInput.Constants.XUSER_MAX_COUNT;
    public float Controller_DeadZone =0.24f; //Todo change dead zone ?
    public float Controller_Threshold = 0.2f;
    public double Controller_AcquireMiliSec = 0.033;

    
    public bool Input_MappingKeysToAzerty = true;
    public string Input_Langue ="FRA";
    public bool Input_ShowCursor = false;

    public string Game_Title= "My First Game With Rita Engine ";
    public string Game_Version ="1.0.0";
    public WindowResolution Window_Resolution = WindowResolution.HD_720p_1920x720;
    public WindowStyle Window_Style =0;
    public WindowExtraStyle Window_ExtraStyle =0;

    
    public bool GraphicDevice_EnableDebugMode = false;
    public string[] GraphicDevice_ValidationLayerExtensions = new string[]{  
    "VK_LAYER_KHRONOS_validation",
    "VK_LAYER_LUNARG_standard_validation",
    "VK_LAYER_GOOGLE_threading",
    "VK_LAYER_LUNARG_parameter_validation",
    "VK_LAYER_LUNARG_object_tracker",
    "VK_LAYER_LUNARG_core_validation",
    "VK_LAYER_GOOGLE_unique_objects", };

    

    public  static (int width,int height) GetResolution(WindowResolution resolution)
        =>resolution switch{
            WindowResolution.FHD_1080p_1920x1080 => new(1920,1080),
            WindowResolution.HD_720p_1920x720 => new(1280,720),
            WindowResolution.Megadrive_320x224 => new(256,224),
            WindowResolution.SVGA_800x600 => new(800,600),
            WindowResolution.VGA_640x480 => new(640,480),
            WindowResolution.Fullscreen => new(0,0),
            WindowResolution.GameBoy_160x144 => new(1920,1080),
            WindowResolution.GameGear_160x144 => new(1920,1080),
            WindowResolution.QuarterQuarterVGA_160x120 => new(1920,1080),
            WindowResolution.GameBoyAdvance_160x120 => new(1920,1080),
            WindowResolution.AtariST_160x100 => new(1920,1080),
            WindowResolution.AtariLynx_160x102 => new(1920,1080),
            WindowResolution.GameCube_320x240 => new(1920,1080),
            WindowResolution.NEOGEOAES_320x224 => new(1920,1080),
            WindowResolution.SuperNintendo_320x240 => new(1920,1080),
            WindowResolution.NintendoDS_320x240 => new(1920,1080),
            WindowResolution.Nintendo64_320x240 => new(1920,1080),
            WindowResolution.MasterSystem_256x192 => new(1920,1080),
            WindowResolution.NES_256x224 => new(1920,1080),
            WindowResolution.QVGA_320x240 => new(1920,1080),
            WindowResolution.WQVGA_400x240 => new(1920,1080),
            WindowResolution.CGA_640x200 => new(1920,1080),
            WindowResolution.WGA_800x480 => new(1920,1080),
            WindowResolution.WXGA_1280x800 => new(1920,1080),
            WindowResolution._4K_3840x1600 => new(1920,1080),
            _=> new(800,600)     };

    public void Log(Display output,string file="") => RitaEngine.Base.Log.Config(output,file );

    public void AssetPath( string assetpath) => RitaEngine.Platform.PlatformHelper.AssetsPath = assetpath;

    private bool _useMemoryPressure = false;
    private int _memoryPressure = 1024*1024*100;

    public void Garbage( GarbageCollectionPriority GCPriority =  GarbageCollectionPriority.SustainedLowLatency ,int MemoryPressure =  1024*1024*100 )
    {   
        if (  MemoryPressure < (1024*1024))return ;
        
        GCSettings.LatencyMode = (GCLatencyMode)GCPriority;
        GC.AddMemoryPressure(MemoryPressure);
        _useMemoryPressure = true;
        _memoryPressure = MemoryPressure;       
    }

    private  void ReleaseGC()
    {
        if ( _useMemoryPressure ){
            GCSettings.LatencyMode = (GCLatencyMode.Interactive);
            GC.RemoveMemoryPressure( _memoryPressure );
        }
    }
    

    public void Dispose()
    {
        ReleaseGC();
        Base.Log.Release();
        GC.SuppressFinalize(this);
    }
}



