namespace RitaEngine.Base.Platform.Structures;

using System.Collections.Generic;
using RitaEngine.Base.Platform.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDevicePhysicalData : IEquatable<GraphicDevicePhysicalData>
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

   
    public GraphicDevicePhysicalData() 
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
    public override bool Equals(object? obj) => obj is GraphicDevicePhysicalData data && this.Equals(data) ;
    public bool Equals(GraphicDevicePhysicalData other)=>  false;
    public static bool operator ==(GraphicDevicePhysicalData left, GraphicDevicePhysicalData right) => left.Equals(right);
    public static bool operator !=(GraphicDevicePhysicalData left, GraphicDevicePhysicalData  right) => !left.Equals(right);
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceAppData
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

    public GraphicDeviceAppData()
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
public struct GraphicDeviceSwapChainData
{
    public VkSwapchainKHR VkSwapChain = VkSwapchainKHR.Null ;
    public VkExtent2D VkSurfaceArea = new();
    public VkFormat VkFormat = VkFormat.VK_FORMAT_UNDEFINED;
    public VkPresentModeKHR VkPresentMode = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR;// v-sync ???
    public VkSurfaceFormatKHR VkSurfaceFormat = new();
    public VkImage[] VkImages = new VkImage[3]; //for CreateImagesView and RecreateSwapChain ....
    public VkImageView[] VkSwapChainImageViews = new VkImageView[3];
    public VkFramebuffer[] VkFramebuffers = new VkFramebuffer[3];//need for render => NEEDVALID SWAP CHAIN
    public uint ImageCount =3;

    public GraphicDeviceSwapChainData(int count = 3 )
    {
        VkImages = new VkImage[count];
        VkSwapChainImageViews = new VkImageView[count];
        VkFramebuffers = new VkFramebuffer[count];
    }

    public unsafe void Release()
    {
        VkImages = null!;
        VkSwapChainImageViews = null!;
        VkFramebuffers = null!;
    }
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceRenderData
{
    
    public VkRenderPass VkRenderPass = VkRenderPass.Null;


    public GraphicDeviceRenderData(){ }

    public void Release()
    {

    }
}