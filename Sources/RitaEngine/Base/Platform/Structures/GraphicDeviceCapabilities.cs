namespace RitaEngine.Base.Platform.Structures;

using System.Collections.Generic;
using RitaEngine.Base.Platform.API.Vulkan;

[ SkipLocalsInit]
public struct GraphicDeviceCapabilities : IEquatable<GraphicDeviceCapabilities>
{
    public VkPhysicalDeviceProperties PhysicalDeviceProperties = new();
     #region VKDeivce
    public VkSurfaceCapabilitiesKHR Capabilities;
    public VkSurfaceFormatKHR[] Formats= null!;
    public VkPresentModeKHR[] PresentModes = null!;
    public VkPhysicalDeviceFeatures Features = new();
    public VkPhysicalDeviceLimits Limits = new();
    

    #endregion
    
    public string[] ValidationLayers = null!;
    public string[] InstanceExtensions = null!;      
    public string[] DeviceExtensions = null!;
    public bool EnableDebug = false;
    public byte[] GameName = null!;
    // public string GameVersion = string.Empty;
    public uint VkVersion =0;

    #region Surface
    public unsafe void* Handle =(void*)0;
    public unsafe void* HInstance =(void*)0;
    public int Width =1280;
    public int Height = 720;

    #endregion

    private nint _address = nint.Zero;

   
    public GraphicDeviceCapabilities() 
    { 
        var sizeEmpty = Unsafe.SizeOf<GraphicDeviceCapabilities>();
        var size =  Marshal.SizeOf(this);
         _address = AddressOfPtrThis( ) ;
        Log.Info($"Create Graphic Device Capabilities => size : {sizeEmpty}, {size } {_address:X} ");
       
        PhysicalDeviceProperties.sparseProperties = new();
        PhysicalDeviceProperties.limits = new();
    }
    public void Release()
    {
        var sizeEmpty = Unsafe.SizeOf<GraphicDeviceCapabilities>();
        var size =  Marshal.SizeOf(this);
        Log.Info($"Release Graphic Device Capabilities => size : {sizeEmpty}, {size } {AddressOfPtrThis( ):X}");
        DeviceExtensions = null!;
        ValidationLayers = null!;
        InstanceExtensions = null!;
        Formats= null!;
        PresentModes = null!;
    }

     public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
        #region OVERRIDE    
    public override string ToString()  
        => string.Format($"DataModule" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicDeviceCapabilities data && this.Equals(data) ;
    public bool Equals(GraphicDeviceCapabilities other)=>  false;
    public static bool operator ==(GraphicDeviceCapabilities left, GraphicDeviceCapabilities right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceCapabilities left, GraphicDeviceCapabilities  right) => !left.Equals(right);
    #endregion
}

