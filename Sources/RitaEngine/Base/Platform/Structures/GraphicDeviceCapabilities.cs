namespace RitaEngine.Base.Platform.Structures;

using System.Collections.Generic;
using RitaEngine.Base.Platform.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDevicePhysical : IEquatable<GraphicDevicePhysical>
{
    public VkPhysicalDeviceProperties PhysicalDeviceProperties = new();
     #region VKDeivce
    public VkSurfaceCapabilitiesKHR Capabilities;
    public VkSurfaceFormatKHR[] Formats= null!;
    public VkPresentModeKHR[] PresentModes = null!;
    public VkPhysicalDeviceFeatures Features = new();
    public VkPhysicalDevice VkPhysicalDevice = VkPhysicalDevice.Null;
    #endregion
    
    public uint VkGraphicFamilyIndice =0;
    public uint VkPresentFamilyIndice=0;
    public string[] DeviceExtensions = null!;

   
    public GraphicDevicePhysical() 
    { 
        PhysicalDeviceProperties.sparseProperties = new();
        PhysicalDeviceProperties.limits = new();
    }
    public void Release()
    {
        DeviceExtensions = null!;
        Formats= null!;
        PresentModes = null!;
    }

  
        #region OVERRIDE    
    public override string ToString()  
        => string.Format($"DataModule" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicDevicePhysical data && this.Equals(data) ;
    public bool Equals(GraphicDevicePhysical other)=>  false;
    public static bool operator ==(GraphicDevicePhysical left, GraphicDevicePhysical right) => left.Equals(right);
    public static bool operator !=(GraphicDevicePhysical left, GraphicDevicePhysical  right) => !left.Equals(right);
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceApp
{
    
    public string[] ValidationLayers = null!;
    public string[] InstanceExtensions = null!;
    public byte[] GameName = null!;
    // public string GameVersion = string.Empty;
    public unsafe void* Handle =(void*)0;
    public unsafe void* HInstance =(void*)0;
    public int Width =1280;
    public int Height = 720;
    public VkSurfaceKHR VkSurface = VkSurfaceKHR.Null;
    public VkInstance VkInstance = VkInstance.Null;
    public VkDebugUtilsMessengerEXT DebugMessenger = VkDebugUtilsMessengerEXT.Null;
    public uint Version=0;
    public bool EnableDebug = false;

    public GraphicDeviceApp()
    {
    }

    public unsafe void Release()
    {
        // Handle = null!;// do in Window
        // HInstance = null!;
        GameName = null!;
        ValidationLayers = null!;
        InstanceExtensions = null!;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceSwapChain
{


}