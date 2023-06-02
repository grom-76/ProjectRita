namespace RitaEngine.Base.Platform.Config;

[SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential , Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT)]
public sealed class WindowConfig : IDisposable 
{
    public string Title= "My First Game With Rita Engine ";
    public string Version ="1.0.0";
    public WindowResolution Rsolution = WindowResolution.HD_720p_1920x720;
    public WindowStyle Style =0;
    public WindowExtraStyle ExtraStyle =0;

    public string System_User32DllName = "User32.dll";
    public string System_KernelDllName =  "kernel32.dll";
    public string System_Gdi32DllName =  "Gdi32.dll";
    
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
    
    public WindowConfig SetTitle( string title){  Title = title ; return this; }
    public WindowConfig SetResolution( WindowResolution resolution){ Rsolution = resolution ; return this; }
    public WindowConfig ChangeDll( string user32newPath){ System_User32DllName  = user32newPath ; return this; }

    public void Dispose()
    {
        Title=null!;
        GC.SuppressFinalize(this);
    }
}


public enum WindowResolution
{
    Fullscreen=-1,
    FHD_1080p_1920x1080,
    HD_720p_1920x720,
    Megadrive_320x224,
    VGA_640x480,
    SVGA_800x600,
    GameBoy_160x144,
    GameGear_160x144,
    QuarterQuarterVGA_160x120,
    GameBoyAdvance_160x120,
    AtariST_160x100,
    AtariLynx_160x102,
    GameCube_320x240,
    NEOGEOAES_320x224,
    SuperNintendo_320x240,
    NintendoDS_320x240,
    Nintendo64_320x240,
    MasterSystem_256x192,
    NES_256x224,
    QVGA_320x240,
    WQVGA_400x240,
    CGA_640x200,
    WGA_800x480,
    WXGA_1280x800,
    _4K_3840x1600,
}


public enum WindowStyle
{
  Fullscreen=-1,
    Popup,
    TitleBar,
    CloseButtons,
    Resizable,
    Bordered,
    Default=Bordered | Resizable | TitleBar,
}


public enum WindowExtraStyle
{

}
