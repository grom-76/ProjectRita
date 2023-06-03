namespace RitaEngine.Base.Platform.Structures;

using System.Collections.Generic;
using RitaEngine.Base.Platform.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public unsafe struct GraphicDeviceCapabilities : IEquatable<GraphicDeviceCapabilities>
{
    public VkPhysicalDeviceProperties PhysicalDeviceProperties = new();
    private nint _address = nint.Zero;
    public string[] ValidationLayers = null!;
    public string[] InstanceExtensions = null!;      
    public string[] DeviceExtensions = null!;
    public bool EnableDebug = false;
    
    public byte[] GameName = null!;
    // public string GameVersion = string.Empty;
    public uint VkVersion =0;

    #region Surface
    public void* Handle =(void*)0;
    public void* HInstance =(void*)0;
    public int Width =1280;
    public int Height = 720;

    #endregion

 

    #region VKDeivce
    public VkSurfaceCapabilitiesKHR Capabilities;
    public VkSurfaceFormatKHR[] Formats= null!;
    public VkPresentModeKHR[] PresentModes = null!;
    public VkPhysicalDeviceFeatures Features = new();
    public VkPhysicalDeviceLimits Limits = new();
    

    #endregion
    public GraphicDeviceCapabilities() { _address = AddressOfPtrThis( ) ;
        PhysicalDeviceProperties.sparseProperties = new();
        PhysicalDeviceProperties.limits = new();
    }
    public void Release()
    {
        DeviceExtensions = null!;
        ValidationLayers = null!;
        InstanceExtensions = null!;
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

