using RitaEngine.API.Vulkan;

namespace RitaEngine.Graphics;

using RitaEngine.API;
using RitaEngine.API.Vulkan;
using RitaEngine.Base;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Strings;
using RitaEngine.Graphic;
using RitaEngine.Math.Color;
using RitaEngine.Platform;
using VkDeviceSize = UInt64;


//GENERAL
public struct GraphicsData
{
    public readonly GraphicsMemories Memories;
    public readonly GraphicsQueues Queues;
    public readonly GraphicsCommandBuffers CommandBuffers;
    public readonly GraphicsSwapChains SwapChains;
    public readonly GraphicsFrameBufers FrameBuffers;
    public readonly GraphicsSynchronizationCacheControl SynchronizationCacheControl;
    public readonly GraphicsRenderPass RenderPass ;

    public /*readonly*/ GraphicsDevice Device;

    public PipelineData Pipeline;
}

public struct GraphicsConfig
{
    public DeviceConfig Device;
    public RenderConfig Render;
    public PipelineConfig Pipeline;

}

//------DEVICE ---------------------------------
public struct DeviceConfig
{
    public string LibraryName_Vulkan = "vulkan-1.dll";
    public GraphicDeviceBackend GraphicDevice_BackEnd = GraphicDeviceBackend.Vulkan;
    public GraphicDeviceClipVolume GraphicDevice_ClipVolume = GraphicDeviceClipVolume.ZeroToOne;
    public GraphicDeviceScreenOrigin GraphicDevice_ScreenOrigin =GraphicDeviceScreenOrigin.Center_Y_DownAxis;
    public GraphicDeviceNDC GraphicDevice_NDC = GraphicDeviceNDC.RightHand;
    public bool EnableDebugMode = false;
    public string[] ValidationLayerExtensions = new string[]{  
    "VK_LAYER_KHRONOS_validation",
    "VK_LAYER_LUNARG_standard_validation",
    "VK_LAYER_GOOGLE_threading",
    "VK_LAYER_LUNARG_parameter_validation",
    "VK_LAYER_LUNARG_object_tracker",
    "VK_LAYER_LUNARG_core_validation",
    "VK_LAYER_GOOGLE_unique_objects", };

    public void SetVerticalSynchro(bool activeVSynchro = true )
    {
        PresentModePreferred = activeVSynchro ? VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR : VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR ;
    }
    public string[] DeviceExtensionsManualAdd = new string[] { VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME  };
    public VkPresentModeKHR PresentModePreferred = VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR ;
    public VkFormat SurfaceFormatPreferred = VkFormat.VK_FORMAT_B8G8R8A8_SRGB;
    public VkColorSpaceKHR ColorFormatPreferred = VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR;
    public VkFormat[] DepthFormatCandidat = new VkFormat[] 
    {
        VkFormat.VK_FORMAT_D32_SFLOAT, 
        VkFormat.VK_FORMAT_D32_SFLOAT_S8_UINT, 
        VkFormat.VK_FORMAT_D24_UNORM_S8_UINT,
        VkFormat.VK_FORMAT_D16_UNORM_S8_UINT,
        VkFormat.VK_FORMAT_D16_UNORM
    };
    public VkImageTiling  DepthImageTilingPreferred =   VkImageTiling.VK_IMAGE_TILING_OPTIMAL;
    public VkFormatFeatureFlagBits DepthFormatFeature = VkFormatFeatureFlagBits.VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT;

    public DeviceConfig()
    {
    }
}

public unsafe readonly struct GraphicsDevice
{
    public unsafe delegate void* PFN_vkGetInstanceProcAddr(VkInstance module , string name);

    public unsafe delegate void* PFN_vkGetDeviceProcAddr(VkDevice module , string name);

    public delegate void PFN_GetFrameBuffer( ref uint x , ref uint y);

    public unsafe void* vkGetInstanceProcAddr(VkInstance instance, string name)
    {
		void*    result = null;
		byte[] bytes = Encoding.ASCII.GetBytes(name);
		fixed( byte* ptr = bytes)
        {
			result =  FuncvkGetInstanceProcAddr(  instance,ptr );
		}
        return result;
    }

    public unsafe void* vkGetDeviceProcAddr(VkDevice device, string name)
    {
		void*    result = null;
		byte[] bytes = Encoding.ASCII.GetBytes(name);
		fixed( byte* ptr = bytes){
			result =  FuncvkGetDeviceProcAddr(  device,ptr );
		}
        return result;
    }
    
    public readonly nint vulkan = nint.Zero;
    
    public unsafe readonly delegate* unmanaged<VkInstance,byte*  , void*> FuncvkGetInstanceProcAddr =null;
	public unsafe readonly delegate* unmanaged<VkDevice,byte*  , void*> FuncvkGetDeviceProcAddr = null;
    public unsafe readonly delegate* unmanaged< UInt32*,VkLayerProperties*,VkResult > vkEnumerateInstanceLayerProperties = null;
    public unsafe readonly delegate* unmanaged< UInt32*,VkResult > vkEnumerateInstanceVersion = null;
    public unsafe readonly delegate* unmanaged< char*,UInt32*,VkExtensionProperties*,VkResult > vkEnumerateInstanceExtensionProperties = null;
    public unsafe readonly delegate* unmanaged< VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*,VkResult > vkCreateInstance = null;
    public unsafe readonly delegate* unmanaged< VkInstance,VkAllocationCallbacks*,void > vkDestroyInstance = null;

    public unsafe readonly  delegate* unmanaged< VkInstance,VkDebugUtilsMessengerCreateInfoEXT*,VkAllocationCallbacks*,VkDebugUtilsMessengerEXT*,VkResult > vkCreateDebugUtilsMessengerEXT = null;
    public unsafe readonly  delegate* unmanaged< VkInstance,UInt32*,VkPhysicalDevice*,VkResult > vkEnumeratePhysicalDevices = null;
    public unsafe readonly  delegate* unmanaged< VkInstance, VkWin32SurfaceCreateInfoKHR*, VkAllocationCallbacks*, VkSurfaceKHR*,VkResult > vkCreateWin32SurfaceKHR = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkDeviceCreateInfo*,VkAllocationCallbacks*,VkDevice*,VkResult > vkCreateDevice = null;    

    public readonly string[] App_ValidationLayers = null!;
    public readonly string[] App_InstanceExtensions = null!;
    public readonly string[] Device_Extensions = null!;
    public readonly uint App_Version=0;
    public readonly VkSurfaceKHR App_Surface = VkSurfaceKHR.Null;
    public readonly VkInstance App_Instance;
    public readonly VkDebugUtilsMessengerEXT App_DebugMessenger = VkDebugUtilsMessengerEXT.Null;
    public readonly VkInstance Instance ;

    public readonly VkDevice Device = VkDevice.Null;

    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkQueueFamilyProperties*,void > vkGetPhysicalDeviceQueueFamilyProperties = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceFeatures*,void > vkGetPhysicalDeviceFeatures = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkFormatProperties*,void > vkGetPhysicalDeviceFormatProperties = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkImageType,VkImageTiling,UInt32,UInt32,VkImageFormatProperties*,VkResult > vkGetPhysicalDeviceImageFormatProperties = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceProperties*,void > vkGetPhysicalDeviceProperties = null;
    
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,char*,UInt32*,VkExtensionProperties*,VkResult > vkEnumerateDeviceExtensionProperties = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkLayerProperties*,VkResult > vkEnumerateDeviceLayerProperties = null;
    
    #region VK_KHR_surface
    public unsafe readonly  delegate* unmanaged< VkInstance,VkSurfaceKHR,VkAllocationCallbacks*,void > vkDestroySurfaceKHR = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,UInt32,VkSurfaceKHR,UInt32*,VkResult > vkGetPhysicalDeviceSurfaceSupportKHR = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,VkSurfaceCapabilitiesKHR*,VkResult > vkGetPhysicalDeviceSurfaceCapabilitiesKHR = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkSurfaceFormatKHR*,VkResult > vkGetPhysicalDeviceSurfaceFormatsKHR = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkPresentModeKHR*,VkResult > vkGetPhysicalDeviceSurfacePresentModesKHR = null;
    #endregion


    public readonly uint[] Device_QueueFamilies = new uint[3]{ uint.MaxValue ,uint.MaxValue,uint.MaxValue };
    public readonly uint Device_ImageCount =0;

    public readonly VkPhysicalDevice Device_Physical = VkPhysicalDevice.Null;
    public readonly VkPhysicalDeviceProperties Device_Properties = new();
    public readonly VkPhysicalDeviceFeatures Device_Features = new();
    
    public readonly VkPresentModeKHR Device_PresentMode = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR ;
    public readonly VkFormat Device_ImageFormat = VkFormat.VK_FORMAT_B8G8R8A8_SRGB;
    public readonly VkFormat Device_DepthBufferImageFormat = VkFormat.VK_FORMAT_D32_SFLOAT;
    public readonly VkExtent2D Device_SurfaceSize = new();
    public readonly VkColorSpaceKHR Device_ImageColor = VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR ;
    public readonly VkSurfaceTransformFlagBitsKHR Device_Transform = VkSurfaceTransformFlagBitsKHR.VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR;
    

    public GraphicsDevice( ref DeviceConfig config , Window window)
    {
        vulkan = Libraries.Load(config.LibraryName_Vulkan);
        Guard.ThrowWhenConditionIsTrue(  vulkan == nint.Zero , "Vulkan Dll not found");

        vkEnumerateInstanceLayerProperties =(delegate* unmanaged< UInt32*,VkLayerProperties*,VkResult>) Libraries.GetUnsafeSymbol( vulkan,nameof(vkEnumerateInstanceLayerProperties)) ;
        vkEnumerateInstanceVersion =  (delegate* unmanaged<UInt32*  , VkResult>)Libraries.GetUnsafeSymbol(vulkan,nameof(vkEnumerateInstanceVersion)) ;
        vkEnumerateInstanceExtensionProperties = (delegate* unmanaged<char*,UInt32*,VkExtensionProperties*,VkResult>)Libraries.GetUnsafeSymbol(vulkan,nameof(vkEnumerateInstanceExtensionProperties)) ;
        vkCreateInstance = (delegate* unmanaged<VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*  , VkResult> )Libraries.GetUnsafeSymbol(vulkan,nameof(vkCreateInstance));
        vkDestroyInstance = (delegate* unmanaged<VkInstance  ,VkAllocationCallbacks*, void> ) Libraries.GetUnsafeSymbol(vulkan,nameof(vkDestroyInstance));
        FuncvkGetInstanceProcAddr = (delegate* unmanaged<VkInstance,byte*  , void*>) Libraries.GetUnsafeSymbol(vulkan,"vkGetInstanceProcAddr");
        FuncvkGetDeviceProcAddr = ( delegate* unmanaged<VkDevice,byte*  , void*>) Libraries.GetUnsafeSymbol(vulkan,"vkGetDeviceProcAddr");

        // VALIDATION LAYER  --------------------------------------------------------------------
        uint layerCount = 0;
        vkEnumerateInstanceLayerProperties(&layerCount, null).Check("Enumerate instance Layer count");
        Guard.ThrowWhenConditionIsTrue( layerCount ==0 );

        VkLayerProperties* layerProperties = stackalloc VkLayerProperties[(int)layerCount];// ReadOnlySpan<VkLayerProperties> pp = stackalloc VkLayerProperties[(int)count];
        vkEnumerateInstanceLayerProperties(&layerCount, layerProperties).Check("Enumerate instance Layer list");

        App_ValidationLayers = new  string[ layerCount ];
        for (int i = 0; i < layerCount; i++) {
            var length = StrHelper.Strlen( layerProperties[i].layerName );
           App_ValidationLayers[i] = Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );// new string(layerProperties[i].layerName); //Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );
        }

        //-- VULKAN API VERSION ------------------------------------------------------------------
        fixed ( uint* ver = &App_Version)
        {
            vkEnumerateInstanceVersion(ver).Check("Enumerate Instance Version");
        }

        //--INSTANCE EXTENSIONS ------------------------------------------------------------------
        uint extCount = 0;
        vkEnumerateInstanceExtensionProperties(null, &extCount, null).Check( "Enumerate Extension Name Count");
        Guard.ThrowWhenConditionIsTrue( extCount == 0);

        VkExtensionProperties* props = stackalloc VkExtensionProperties[(int)extCount];
        vkEnumerateInstanceExtensionProperties(null, &extCount, props).Check( "Enumerate Extension Name List");

        App_InstanceExtensions = new string[extCount ];
        for (int i = 0; i < extCount; i++)
        {
            var length = StrHelper.Strlen( props[i].extensionName);
           App_InstanceExtensions[i] =Encoding.UTF8.GetString(  props[i].extensionName, (int) length );
        }

        // CREATE DEBUG INFO ------------------------------------------------
        VkDebugUtilsMessengerCreateInfoEXT debugCreateInfo = new();
        debugCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT;
        debugCreateInfo.pNext = null;
        debugCreateInfo.messageSeverity = (uint)( VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT );
        debugCreateInfo.messageType = (uint) (VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT);
        debugCreateInfo.pfnUserCallback = &DebugMessengerCallback;
        debugCreateInfo.pUserData = null;
        
        //CREATE APPLICATION INFO ------------------------------------------------
        var EngineName = Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);

        VkApplicationInfo appInfo = new();
        appInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO;
        appInfo.apiVersion =App_Version; 
        appInfo.applicationVersion = VK.VK_MAKE_VERSION(1,0,0);
        appInfo.engineVersion= VK.VK_MAKE_VERSION(1,0,0);
        appInfo.pNext =null;
        var GameName  = window.GetWindowName();
        fixed(byte* ptr = &EngineName[0] ,  app = &GameName[0] )
        {
            appInfo.pApplicationName =app;
            appInfo.pEngineName = ptr;
        }
        //CREATE INSTANCE  INFO   ------------------------------------------------
        using var extNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref App_InstanceExtensions) ;
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref App_ValidationLayers);

        VkInstanceCreateInfo instanceCreateInfo = new();
        instanceCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
        instanceCreateInfo.flags =  (uint)VkInstanceCreateFlagBits.VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR;       
        instanceCreateInfo.pApplicationInfo =&appInfo;
        instanceCreateInfo.pNext= !config.EnableDebugMode ? null :  (VkDebugUtilsMessengerCreateInfoEXT*) &debugCreateInfo;
        instanceCreateInfo.ppEnabledExtensionNames = extNames;
        instanceCreateInfo.enabledExtensionCount =extNames.Count;
        instanceCreateInfo.enabledLayerCount = config.EnableDebugMode ?layerNames.Count : 0;
        instanceCreateInfo.ppEnabledLayerNames =config.EnableDebugMode ? layerNames : null;

        fixed( VkInstance* instance = &App_Instance)
        {
            vkCreateInstance(&instanceCreateInfo, null, instance).Check("failed to create instance!");
        }

        Log.Info($"Create Debug {App_Instance}");

        VK.VK_KHR_swapchain=true;// //Special dont understand pour chage swapchain car nvidia n'a pas l'extension presente
        VkHelper.ValidateExtensionsForLoad(ref App_InstanceExtensions,0 );

        vkCreateDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance,VkDebugUtilsMessengerCreateInfoEXT*,VkAllocationCallbacks*,VkDebugUtilsMessengerEXT*,VkResult>) vkGetInstanceProcAddr(App_Instance,nameof(vkCreateDebugUtilsMessengerEXT)); 
        vkEnumeratePhysicalDevices = (delegate* unmanaged<VkInstance,UInt32*,VkPhysicalDevice*,VkResult>) vkGetInstanceProcAddr(App_Instance,nameof(vkEnumeratePhysicalDevices)); 
        // vkDestroySurfaceKHR=  (delegate* unmanaged<VkInstance, VkSurfaceKHR,VkAllocationCallbacks*  , void>)load(App_Instance,nameof(vkDestroySurfaceKHR));
        vkCreateWin32SurfaceKHR = (delegate* unmanaged<VkInstance,VkWin32SurfaceCreateInfoKHR*, VkAllocationCallbacks*,VkSurfaceKHR*  , VkResult> )vkGetInstanceProcAddr(App_Instance,nameof(vkCreateWin32SurfaceKHR));
        // vkGetPhysicalDeviceQueueFamilyProperties = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkQueueFamilyProperties*,void>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceQueueFamilyProperties)); 
        vkGetPhysicalDeviceSurfaceSupportKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32,VkSurfaceKHR,uint*,VkResult>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceSurfaceSupportKHR)); 
	    vkGetPhysicalDeviceFeatures = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceFeatures*,void>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceFeatures)); 
        vkGetPhysicalDeviceFormatProperties = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkFormatProperties*,void>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceFormatProperties)); 
        vkGetPhysicalDeviceImageFormatProperties = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkImageType,VkImageTiling,UInt32,UInt32,VkImageFormatProperties*,VkResult>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceImageFormatProperties)); 
        vkGetPhysicalDeviceProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceProperties*,void>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceProperties)); 
        vkGetPhysicalDeviceQueueFamilyProperties = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkQueueFamilyProperties*,void>) vkGetInstanceProcAddr(App_Instance,nameof(vkGetPhysicalDeviceQueueFamilyProperties)); 
    
        // CREATE DEBUG ------------------------------------------------------------------------
        if ( !config.EnableDebugMode  )
        {
            fixed(VkDebugUtilsMessengerEXT* dbg = &App_DebugMessenger )
            {
                vkCreateDebugUtilsMessengerEXT(App_Instance, &debugCreateInfo, null, dbg).Check("failed to set up debug messenger!");
            }
            Log.Info($"Create Debug {App_DebugMessenger}");

        }

        // CREATE SURFACE -------------------------------------------------------------------------
      
        VkWin32SurfaceCreateInfoKHR sci = new() ;
        sci .hinstance = window.GetWindowHInstance();
        sci .hwnd = window.GetWindowHandle();
        sci .pNext = null;
        sci .flags = 0;
        sci .sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;

        fixed ( VkSurfaceKHR* surf = &App_Surface)
        {
            vkCreateWin32SurfaceKHR(App_Instance,&sci,null, surf).Check("Create Surface");
        }
        Log.Info($"Create Surface {App_Surface}");

        // SELECT PHYSICAL DEVICE
        uint deviceCount = 0;
        vkEnumeratePhysicalDevices(App_Instance, &deviceCount, null).Check("EnumeratePhysicalDevices Count");
        Guard.ThrowWhenConditionIsTrue(deviceCount == 0,"Vulkan: Failed to find GPUs with Vulkan support");

        VkPhysicalDevice* devices = stackalloc VkPhysicalDevice[(int)deviceCount];
        vkEnumeratePhysicalDevices(App_Instance, &deviceCount, devices).Check("EnumeratePhysicalDevices List");

        for (int i = 0; i < (int)deviceCount; i++)
        {
            Device_Physical =IsSuitable( devices[i] );

            if ( !Device_Physical.IsNull )
                break;

        }
        Guard.ThrowWhenConditionIsTrue( Device_Physical.IsNull , "Physical device is null ");

        Log.Info($"Select Physical device {Device_Physical}");
// GET INFOS 
        // VkDeviceQueueCreateFlagBits.VK_DEVICE_QUEUE_CREATE_PROTECTED_BIT

// TEST queue Family 2 
        // uint graphic = uint.MaxValue; uint present = uint.MaxValue; uint compute = uint.MaxValue; uint transfert = uint.MaxValue;
        // uint queueFamilyPropertyCount2 = 0;
        // vkGetPhysicalDeviceQueueFamilyProperties2(data.Device_Physical, &queueFamilyPropertyCount2, null);

        // ReadOnlySpan<VkQueueFamilyProperties2> queueFamilyProperties2 = new VkQueueFamilyProperties2[queueFamilyPropertyCount2];
        // fixed (VkQueueFamilyProperties2* queueFamilyPropertiesPtr2 = queueFamilyProperties2){
        //     vkGetPhysicalDeviceQueueFamilyProperties2(data.Device_Physical, &queueFamilyPropertyCount2, queueFamilyPropertiesPtr2);
        // }
        // if( data.Device_Properties.deviceType ==   VkPhysicalDeviceType.VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU ) 
        // {
        //     //Always better
        // }
// ----------------------------------------------
        // Capabilities
        VkSurfaceCapabilitiesKHR Capabilities;
        vkGetPhysicalDeviceSurfaceCapabilitiesKHR(Device_Physical, App_Surface, &Capabilities ).Check("vkGetPhysicalDeviceSurfaceCapabilitiesKHR");

        if ( Capabilities.currentExtent.width != uint.MaxValue)
        {
            Device_SurfaceSize = Capabilities.currentExtent;
        } else  {
           
            Device_SurfaceSize.width = ClampUInt( (uint)window.GetWindowWidth(), Capabilities.minImageExtent.width, Capabilities.maxImageExtent.width);
            Device_SurfaceSize.height = ClampUInt( (uint)window.GetWindowheight(), Capabilities.minImageExtent.height, Capabilities.maxImageExtent.height);
        }

        Device_ImageCount = Capabilities.minImageCount + 1;
        if (Capabilities.maxImageCount > 0 && Device_ImageCount > Capabilities.maxImageCount)
        {
            Device_ImageCount = Capabilities.maxImageCount;
        }

        Device_Transform = Capabilities.currentTransform;

        // Surface Format -------------------------------------------------------------------------------------------------------------------------------------------
        uint surfaceFormatCount = 0;
        vkGetPhysicalDeviceSurfaceFormatsKHR(Device_Physical, App_Surface,  &surfaceFormatCount, null).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");

        ReadOnlySpan<VkSurfaceFormatKHR> surfaceFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
        fixed (VkSurfaceFormatKHR* surfaceFormatsPtr = surfaceFormats)		{
            vkGetPhysicalDeviceSurfaceFormatsKHR(Device_Physical, App_Surface,  &surfaceFormatCount, surfaceFormatsPtr).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");
        }

        Device_ImageFormat = surfaceFormats[0].format;
        Device_ImageColor =surfaceFormats[0].colorSpace;

        foreach (VkSurfaceFormatKHR availableFormat in surfaceFormats)
        {
            if (availableFormat.format == config.SurfaceFormatPreferred && availableFormat.colorSpace == config.ColorFormatPreferred )
            {
                Device_ImageFormat = availableFormat.format;
                Device_ImageColor = availableFormat.colorSpace  ;
                break;
            }
        }

        
        foreach ( VkFormat format in config.DepthFormatCandidat )
        {
            //Get PhysicalFormatProperties --------------------------------------------------------
            VkFormatProperties formatProperties;
            vkGetPhysicalDeviceFormatProperties(Device_Physical,format, &formatProperties);

            if (config.DepthImageTilingPreferred == VkImageTiling.VK_IMAGE_TILING_LINEAR && (formatProperties.linearTilingFeatures & (uint)config.DepthFormatFeature) == (uint)config.DepthFormatFeature) 
            {
               
                Device_DepthBufferImageFormat = format;
            } 
            else if (config.DepthImageTilingPreferred == VkImageTiling.VK_IMAGE_TILING_OPTIMAL && (formatProperties.optimalTilingFeatures & (uint)config.DepthFormatFeature) == (uint)config.DepthFormatFeature) 
            {
                
                Device_DepthBufferImageFormat = format;
            }
        }
        Log.Info($"Depth Format : { Device_DepthBufferImageFormat.ToString() }");
        
        // Present mode -----------------------------------------------------------------------------------------------------------------------
        uint presentModeCount = 0;
        vkGetPhysicalDeviceSurfacePresentModesKHR(Device_Physical, App_Surface, &presentModeCount, null).Check("vkGetPhysicalDeviceSurfacePresentModesKHR Count");

        ReadOnlySpan<VkPresentModeKHR> presentModes = new VkPresentModeKHR[presentModeCount];
        fixed (VkPresentModeKHR* presentModesPtr = presentModes)	{
            vkGetPhysicalDeviceSurfacePresentModesKHR(Device_Physical, App_Surface, &presentModeCount, presentModesPtr).Check("vkGetPhysicalDeviceSurfacePresentModesKHR List");
        }
        
        Device_PresentMode = VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
        foreach (VkPresentModeKHR availablePresentMode in presentModes)
        {
            if (availablePresentMode == config.PresentModePreferred)
            {
                Device_PresentMode = availablePresentMode;
                break;
            }
        }

        // PHYSICAL DEVICE PROPERTIES -------------------------------------------
        fixed (VkPhysicalDeviceProperties* phd =   &Device_Properties)
        {
            vkGetPhysicalDeviceProperties(Device_Physical ,phd );
        }

        // GET Physical FEATURES ----------------------------------------------------------

        fixed ( VkPhysicalDeviceFeatures* features = &Device_Features)
        {
            vkGetPhysicalDeviceFeatures(Device_Physical,features );
        } 

       
        // DEVICE  EXTENSIONS -------------------------------------------------
        uint propertyCount = 0;
        vkEnumerateDeviceExtensionProperties(Device_Physical, null, &propertyCount, null).Check();

        VkExtensionProperties* properties = stackalloc VkExtensionProperties[(int)propertyCount];  
        vkEnumerateDeviceExtensionProperties(Device_Physical, null, &propertyCount, properties).Check();

        int addext = config.DeviceExtensionsManualAdd is null ? 0 : config.DeviceExtensionsManualAdd.Length;

        Device_Extensions = new string[propertyCount + addext];
        
        for (int i = 0; i < propertyCount; i++){
            var length =  StrHelper.Strlen( properties[i].extensionName);
           Device_Extensions[i] = Encoding.UTF8.GetString( properties[i].extensionName, (int) length ); 
        }

        if ( config.DeviceExtensionsManualAdd is null)return ;

        for( int i = 0 ;i < addext ; i++ )
        {
            Device_Extensions[i+ propertyCount ] = VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME ;
        }

//--------------------------------------------------


        // CREATE DEVICE
        float queuePriority = 1.0f;
        
        var queueFamiliesCount = Device_QueueFamilies[0] == Device_QueueFamilies[2] ? Device_QueueFamilies.Length-1 : Device_QueueFamilies.Length ;
        queueFamiliesCount = Device_QueueFamilies[0] == Device_QueueFamilies[1] ? queueFamiliesCount-1 : queueFamiliesCount ;

        VkDeviceQueueCreateInfo* queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[queueFamiliesCount];

        for( uint i = 0; i < queueFamiliesCount ; i++ )
        {
            queueCreateInfos[i] = new VkDeviceQueueCreateInfo {
                sType = VkStructureType. VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = Device_QueueFamilies[i],
                queueCount = 1,
                pQueuePriorities = &queuePriority
            };
        }

        // CREATE DEVICE INFO ---------------------------------------------------------
        using var deviceExtensions = new StrArrayUnsafe(ref Device_Extensions);
        // using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref App_ValidationLayers);
        VkDeviceCreateInfo createInfo = new();
        createInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
        createInfo.queueCreateInfoCount = (uint)queueFamiliesCount;
        createInfo.pQueueCreateInfos = queueCreateInfos;
        fixed ( VkPhysicalDeviceFeatures* features = &Device_Features) {createInfo.pEnabledFeatures = features;}
        createInfo.enabledExtensionCount = (uint)deviceExtensions.Count;
        createInfo.ppEnabledExtensionNames = deviceExtensions;
        createInfo.pNext = null ;
        createInfo.enabledLayerCount = config.EnableDebugMode ? layerNames.Count : 0 ;
        createInfo.ppEnabledLayerNames = config.EnableDebugMode ? layerNames : null ;

        fixed(VkDevice* device = &Device)
        {
            vkCreateDevice(Device_Physical, &createInfo, null, device).Check("Error creation vkDevice");
        }
       
       VkHelper.ValidateExtensionsForLoad(ref Device_Extensions,0 );

       Log.Info($"Create Device :{Device}");
    }

    
    private unsafe VkPhysicalDevice IsSuitable( VkPhysicalDevice Device_Physical)
    {
        // GET QUEUES 
        uint queueFamilyPropertyCount = 0;
        vkGetPhysicalDeviceQueueFamilyProperties(Device_Physical, &queueFamilyPropertyCount, null);

        ReadOnlySpan<VkQueueFamilyProperties> queueFamilyProperties = new VkQueueFamilyProperties[queueFamilyPropertyCount];
        
        fixed (VkQueueFamilyProperties* queueFamilyPropertiesPtr = queueFamilyProperties){
            vkGetPhysicalDeviceQueueFamilyProperties(Device_Physical, &queueFamilyPropertyCount, queueFamilyPropertiesPtr);
        }

        for( uint i = 0 ; i <queueFamilyProperties.Length ; i++ )
        {
            // var flag = queueFamilyProperties[(int)i].queueFlags;
            if ( (queueFamilyProperties[(int)i].queueFlags & VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT) != 0)
            {
                Device_QueueFamilies[0] = i;
                
            }
            if ( (queueFamilyProperties[(int)i].queueFlags & VkQueueFlagBits.VK_QUEUE_COMPUTE_BIT) != 0   && queueFamilyProperties[(int)i].queueCount > 1 )
            {
                Device_QueueFamilies[1] = i;
            }
            if( SupportPresenting( i)  && Device_QueueFamilies[2] == uint.MaxValue )
            {
                Device_QueueFamilies[2] = i;
            }

            if (Device_QueueFamilies[0] != uint.MaxValue && Device_QueueFamilies[2] != uint.MaxValue && Device_QueueFamilies[1] != uint.MaxValue )
            { break; }
        }

        if (Device_QueueFamilies[0] == uint.MaxValue || Device_QueueFamilies[2] == uint.MaxValue || Device_QueueFamilies[1] == uint.MaxValue )
        {
            Device_Physical  = VkPhysicalDevice.Null ;
        }

        return Device_Physical;
    }
    

    private unsafe bool SupportPresenting( uint i)
    {
        uint presentSupport = 0;
        //Querying for WSI Support
        vkGetPhysicalDeviceSurfaceSupportKHR(Device_Physical, i, App_Surface, &presentSupport);

        // #if WIN64 // if not work use directly platform Presentation support method
        // var supported = vkGetPhysicalDeviceWin32PresentationSupportKHR(Device_Physical, i);
        // #endif
        return  (presentSupport == VK.VK_TRUE) ? true : false;
    }

    private static uint ClampUInt(uint value, uint min, uint max) =>value < min ? min : value > max ? max : value;

    [UnmanagedCallersOnly] 
    private unsafe static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, uint messageTypes,
        VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {
        string message= System.Text.Encoding.UTF8.GetString(pCallbackData->pMessage,(int) StrHelper.Strlen(pCallbackData->pMessage) );  

        if (messageSeverity == VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT)
        {
            Log.Error($"{message}" ,"");
        }
        else if (messageSeverity == VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT)
        {
            Log.Warning($"{message}");
        }
        else
        {
            Log.Info($"{message}");
        }
        return 1;
    }
}

//------ RENDER ---------------------------------
public struct RenderConfig
{
    public bool Stereoscopic3DApp = false ;
    public bool Clipped =false;
    public VkCompositeAlphaFlagBitsKHR CompositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;

    public int MAX_FRAMES_IN_FLIGHT = 2;
    public ulong Tick_timeout = ulong.MaxValue;
    public Palette BackGroundColor = Palette.LavenderBlush;

    public RenderConfig()
    {
    }
}

public unsafe readonly struct GraphicsSwapChains
{
    // public PFN_GetFrameBuffer SwapChain_GetFrameBufferCallback = null!;
    public readonly VkSwapchainKHR SwapChain = VkSwapchainKHR.Null ;
    public unsafe readonly  delegate* unmanaged< VkDevice,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult > vkCreateSwapchainKHR = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,VkSwapchainKHR,VkAllocationCallbacks*,void > vkDestroySwapchainKHR = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt64,VkSemaphore,VkFence,UInt32*,VkResult > vkAcquireNextImageKHR = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,VkAcquireNextImageInfoKHR*,UInt32*,VkResult > vkAcquireNextImage2KHR = null;  
	public unsafe readonly  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt32*,VkImage*,VkResult > vkGetSwapchainImagesKHR = null;

    public readonly VkImage[] SwapChain_Images = null!;
    public readonly VkImageView[] SwapChain_ImageViews = null!;
    public readonly VkImage SwapChain_DepthBufferImages = VkImage.Null;
    public readonly VkImageView SwapChain_DepthBufferImageViews = VkImageView.Null;
    public readonly VkDeviceMemory SwapChain_DepthBufferImageMemory = VkDeviceMemory.Null;
    private readonly VkDevice _device = VkDevice.Null;


    public GraphicsSwapChains(ref GraphicsDevice data,  RenderConfig config)
    {
        _device = data.Device;
        if (VK.VK_KHR_swapchain){
			vkCreateSwapchainKHR = (delegate* unmanaged<VkDevice,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkCreateSwapchainKHR)); 
			vkDestroySwapchainKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,VkAllocationCallbacks*,void>) data.vkGetDeviceProcAddr(data.Device,nameof(vkDestroySwapchainKHR)); 
			vkGetSwapchainImagesKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,UInt32*,VkImage*,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkGetSwapchainImagesKHR)); 
			vkAcquireNextImageKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,UInt64,VkSemaphore,VkFence,UInt32*,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkAcquireNextImageKHR)); 
			vkAcquireNextImage2KHR = (delegate* unmanaged<VkDevice,VkAcquireNextImageInfoKHR*,UInt32*,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkAcquireNextImage2KHR)); 
		}

        uint* queueFamilyIndices = stackalloc uint[2]{data.Device_QueueFamilies[0], data.Device_QueueFamilies[2]};

        VkSwapchainCreateInfoKHR createInfo = new();
        createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
        createInfo.pNext = null;
        createInfo.surface = data.App_Surface;
        createInfo.minImageCount = data.Device_ImageCount;
        createInfo.imageFormat =  data.Device_ImageFormat;
        createInfo.imageColorSpace = data.Device_ImageColor;
        createInfo.imageExtent = data.Device_SurfaceSize;
        createInfo.imageArrayLayers = config.Stereoscopic3DApp ? (uint)2 : (uint)1;

        createInfo.imageUsage = (uint)VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;
        
        if (data.Device_QueueFamilies[0] != data.Device_QueueFamilies[2])
        {
            createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
            createInfo.queueFamilyIndexCount = 2;
            createInfo.pQueueFamilyIndices = queueFamilyIndices;
        } else {
            createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
        }
        
        createInfo.preTransform = data.Device_Transform;
        createInfo.compositeAlpha = config.CompositeAlpha;
        createInfo.presentMode = data.Device_PresentMode;
        createInfo.clipped = config.Clipped ? VK.VK_TRUE : VK.VK_FALSE;
        createInfo.oldSwapchain = VkSwapchainKHR.Null;

        fixed (VkSwapchainKHR* swapchainPtr = &SwapChain)
        {
            vkCreateSwapchainKHR(data.Device, &createInfo, null, swapchainPtr).Check("failed to create swap chain!");
        }

        Log.Info($"Create SwapChain {SwapChain}");
          // SWWAP CHAIN IMAGES  ----------------------------------------------------------------------
        uint imageCount = 0 ;
        vkGetSwapchainImagesKHR(data.Device, SwapChain, &imageCount, null);
       
        SwapChain_Images = new VkImage[imageCount];

        fixed (VkImage* swapchainImagesPtr = SwapChain_Images){
            vkGetSwapchainImagesKHR(data.Device,SwapChain, &imageCount, swapchainImagesPtr).Check("vkGetSwapchainImagesKHR");
        }
        
        Log.Info($"Create {SwapChain_Images.Length} SwapChainImages ");

        var Device_ImageCount = imageCount  ;
        // SWWAP CHAIN IMAGES  VIEWS FOR FRAMEBUFFER ----------------------------------------------------------------------
        
        SwapChain_ImageViews = new VkImageView[imageCount ];

        ResourceCreation.ImageViewCreation imgView = new (ref data,data.Device_ImageFormat, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT );
        for (uint i = 0; i < imageCount; i++)
        {
            Log.Info($"\t -[{i}] {SwapChain_Images[i]} : {data.Device_ImageFormat} other info ...."); 
            SwapChain_ImageViews[i] = imgView.CreateImageView(SwapChain_Images[i] );
        }

        //DEPTH RESOURCE ----------------------------------------------------------------------
        
        // ResourceCreation.CreateImage2( ref func ,ref data, ref data.SwapChain_DepthBufferImages,ref data.SwapChain_DepthBufferImageMemory,
        // new(data.Device_SurfaceSize.width,data.Device_SurfaceSize.height, 1) , data.Device_DepthBufferImageFormat,VkImageTiling.VK_IMAGE_TILING_OPTIMAL  , VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT  
        // ,VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT);
        
        // DEPTH RESOURCE FOR FRAMEBUFFER
        ResourceCreation.ImageViewCreation imgView2 = new (ref data,data.Device_DepthBufferImageFormat, VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT,0);
        SwapChain_DepthBufferImageViews = imgView2.CreateImageView(SwapChain_DepthBufferImages );
    }


    public VkResult AcquireNextImage( ulong tick  , VkSemaphore CurrentImageAvailableSemaphore, ref uint imageIndex)
    {
        uint _imageIndex = imageIndex;
        return vkAcquireNextImageKHR(_device, SwapChain, tick ,CurrentImageAvailableSemaphore, VkFence.Null, &_imageIndex);
    }
}

public unsafe readonly struct GraphicsQueues
{
    public unsafe readonly  delegate* unmanaged< VkQueue,VkResult > vkQueueWaitIdle = null;
    public unsafe readonly  delegate* unmanaged< VkQueue,UInt32,VkSubmitInfo*,VkFence,VkResult > vkQueueSubmit = null;
    public unsafe readonly  delegate* unmanaged< VkQueue,VkPresentInfoKHR*,VkResult > vkQueuePresentKHR = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,UInt32,VkQueue*,void > vkGetDeviceQueue = null;
    public readonly VkQueue Device_GraphicQueue = VkQueue.Null;
    public readonly VkQueue Device_PresentQueue = VkQueue.Null;
    public readonly VkQueue Device_ComputeQueue = VkQueue.Null;

    public GraphicsQueues(ref GraphicsDevice data)
    {
        vkGetDeviceQueue = (delegate* unmanaged<VkDevice,UInt32,UInt32,VkQueue*,void>) data.vkGetDeviceProcAddr(data.Device,nameof(vkGetDeviceQueue)); 
        vkQueuePresentKHR = (delegate* unmanaged<VkQueue,VkPresentInfoKHR*,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkQueuePresentKHR)); 
        vkQueueSubmit = (delegate* unmanaged<VkQueue,UInt32,VkSubmitInfo*,VkFence,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkQueueSubmit)); 
        vkQueueWaitIdle = (delegate* unmanaged<VkQueue,VkResult>) data.vkGetDeviceProcAddr(data.Device,nameof(vkQueueWaitIdle)); 

        fixed(VkQueue* gq =&Device_GraphicQueue)
        {
            vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[0], 0,gq);
        }
        fixed(VkQueue* pq = &Device_PresentQueue)
        {
            vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[2], 0, pq); 
        }
        fixed(VkQueue* cq = &Device_ComputeQueue)
        {
            vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[1], 0, cq); 
        }

        Log.Info($"Graphic Queue : indice :{ data.Device_QueueFamilies[0]}  Adr[{Device_GraphicQueue}]");
        Log.Info($"Compute Queue : indice :{ data.Device_QueueFamilies[1]}  Adr[{Device_ComputeQueue}]");
        Log.Info($"Present Queue : indice :{ data.Device_QueueFamilies[2]}  Adr[{Device_PresentQueue}]");
    }

    // SUBMIT GRAPHICS COMMAND BUFFERS
    public unsafe void SubmitGraphicCommandBuffer(VkCommandBuffer commandBuffer, VkSemaphore* waitSemaphores,UInt32* waitStages,VkSemaphore*   signalSemaphores , VkFence CurrentinFlightFence)
    {
        VkSubmitInfo submitInfo = default;
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.waitSemaphoreCount = 1;
        submitInfo.pWaitSemaphores = waitSemaphores;
        submitInfo.pWaitDstStageMask =  waitStages;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers = &commandBuffer;      
        submitInfo.signalSemaphoreCount = 1;
        submitInfo.pSignalSemaphores = signalSemaphores ;
        submitInfo.pNext = null;    
        vkQueueSubmit(Device_GraphicQueue, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
    }

    public unsafe void SubmitSingleTimeCommandBuffer(VkCommandBuffer commandBuffer, VkFence CurrentinFlightFence  )
    {
        VkSubmitInfo submitInfo = default;
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers = &commandBuffer;      
        submitInfo.pNext = null;    
        vkQueueSubmit(Device_GraphicQueue, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
        vkQueueWaitIdle(Device_GraphicQueue);
    }

    public unsafe void Wait() => vkQueueWaitIdle(Device_GraphicQueue);

    public unsafe VkResult PresentImage(VkSwapchainKHR* swapChains , VkSemaphore* signalSemaphores, uint imageIndex )
    {
        VkPresentInfoKHR presentInfo =  default;
        presentInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR; 
        presentInfo.waitSemaphoreCount = 1;
        presentInfo.pWaitSemaphores = signalSemaphores;
        presentInfo.pImageIndices = &imageIndex;
        presentInfo.swapchainCount = 1;
        presentInfo.pSwapchains = swapChains;
        presentInfo.pNext =null;
        presentInfo.pResults = null;   
        return  vkQueuePresentKHR(Device_PresentQueue, &presentInfo); 
    }

}

public unsafe readonly struct GraphicsMemories
{
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceMemoryProperties*,void > vkGetPhysicalDeviceMemoryProperties = null;
    public unsafe readonly  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceMemoryProperties2*,void > vkGetPhysicalDeviceMemoryProperties2 = null;

    public unsafe readonly  delegate* unmanaged< VkDevice,VkMemoryAllocateInfo*,VkAllocationCallbacks*,VkDeviceMemory*,VkResult > vkAllocateMemory = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkDeviceMemory,VkAllocationCallbacks*,void > vkFreeMemory = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkDeviceMemory,UInt64,UInt64,UInt32,void**,VkResult > vkMapMemory = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkDeviceMemory,void > vkUnmapMemory = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,VkMappedMemoryRange*,VkResult > vkFlushMappedMemoryRanges = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,VkMappedMemoryRange*,VkResult > vkInvalidateMappedMemoryRanges = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkDeviceMemory,UInt64*,void > vkGetDeviceMemoryCommitment = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkBuffer,VkDeviceMemory,UInt64,VkResult > vkBindBufferMemory = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkImage,VkDeviceMemory,UInt64,VkResult > vkBindImageMemory = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkBuffer,VkMemoryRequirements*,void > vkGetBufferMemoryRequirements = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkImage,VkMemoryRequirements*,void > vkGetImageMemoryRequirements = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkImage,UInt32*,VkSparseImageMemoryRequirements*,void > vkGetImageSparseMemoryRequirements = null;

    private readonly VkDevice Device = VkDevice.Null;
    public readonly VkPhysicalDeviceMemoryProperties Device_MemoryProperties = new();

    public GraphicsMemories(ref GraphicsDevice data)
    {
        Device = data.Device;
        vkGetPhysicalDeviceMemoryProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceMemoryProperties*,void>) data.vkGetInstanceProcAddr(data.App_Instance,nameof(vkGetPhysicalDeviceMemoryProperties)); 
		vkGetPhysicalDeviceMemoryProperties2 = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceMemoryProperties2*,void>) data.vkGetInstanceProcAddr(data.App_Instance,nameof(vkGetPhysicalDeviceMemoryProperties2)); 

        vkAllocateMemory = (delegate* unmanaged<VkDevice,VkMemoryAllocateInfo*,VkAllocationCallbacks*,VkDeviceMemory*,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkAllocateMemory)); 
        vkFreeMemory = (delegate* unmanaged<VkDevice,VkDeviceMemory,VkAllocationCallbacks*,void>) data.vkGetDeviceProcAddr(Device,nameof(vkFreeMemory)); 
        vkMapMemory = (delegate* unmanaged<VkDevice,VkDeviceMemory,UInt64,UInt64,UInt32,void**,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkMapMemory)); 
        vkUnmapMemory = (delegate* unmanaged<VkDevice,VkDeviceMemory,void>) data.vkGetDeviceProcAddr(Device,nameof(vkUnmapMemory)); 
        vkFlushMappedMemoryRanges = (delegate* unmanaged<VkDevice,UInt32,VkMappedMemoryRange*,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkFlushMappedMemoryRanges)); 
        vkInvalidateMappedMemoryRanges = (delegate* unmanaged<VkDevice,UInt32,VkMappedMemoryRange*,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkInvalidateMappedMemoryRanges)); 
        vkGetDeviceMemoryCommitment = (delegate* unmanaged<VkDevice,VkDeviceMemory,UInt64*,void>) data.vkGetDeviceProcAddr(Device,nameof(vkGetDeviceMemoryCommitment)); 
        vkBindBufferMemory = (delegate* unmanaged<VkDevice,VkBuffer,VkDeviceMemory,UInt64,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkBindBufferMemory)); 
        vkBindImageMemory = (delegate* unmanaged<VkDevice,VkImage,VkDeviceMemory,UInt64,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkBindImageMemory)); 
        vkGetBufferMemoryRequirements = (delegate* unmanaged<VkDevice,VkBuffer,VkMemoryRequirements*,void>) data.vkGetDeviceProcAddr(Device,nameof(vkGetBufferMemoryRequirements)); 
        vkGetImageMemoryRequirements = (delegate* unmanaged<VkDevice,VkImage,VkMemoryRequirements*,void>) data.vkGetDeviceProcAddr(Device,nameof(vkGetImageMemoryRequirements)); 
        vkGetImageSparseMemoryRequirements = (delegate* unmanaged<VkDevice,VkImage,UInt32*,VkSparseImageMemoryRequirements*,void>) data.vkGetDeviceProcAddr(Device,nameof(vkGetImageSparseMemoryRequirements));
         // GET MEMORY PROPERTIES -------------------------------------------------------------------
        // fixed ( VkPhysicalDeviceMemoryProperties* mem = &Device_MemoryProperties )
        // {
        //     vkGetPhysicalDeviceMemoryProperties(Device_Physical, mem);
        // }
        // if vulkan >= ver 1.2

        VkPhysicalDeviceMemoryProperties2 mem2 = default;
        vkGetPhysicalDeviceMemoryProperties2(data.Device_Physical, &mem2);
        Device_MemoryProperties = mem2.memoryProperties;

    }

    public unsafe uint FindMemoryType( uint memoryTypeBits, VkMemoryPropertyFlagBits properties)
    {
        uint count = Device_MemoryProperties.memoryTypeCount;
        for (uint i = 0; i < count; i++)
        {
            if ( (memoryTypeBits & 1) == 1 && (Device_MemoryProperties.memoryTypes[(int)i].propertyFlags & (uint)properties) == (uint)properties)
            {
                return i;
            }
            memoryTypeBits >>= 1;
        }

        return uint.MaxValue;
    }

    public void AllocateMemoryForBuffer( VkMemoryPropertyFlagBits properties,ref  VkBuffer buffer, ref  VkDeviceMemory bufferMemory )
    {
        VkMemoryRequirements memRequirements = new();
        vkGetBufferMemoryRequirements(Device, buffer, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = FindMemoryType(  memRequirements.memoryTypeBits, properties);
        allocInfo.pNext = null;
        fixed(VkDeviceMemory* memory =  &bufferMemory) 
        {
            vkAllocateMemory(Device, &allocInfo, null, memory).Check("failed to allocate memory!");
        }
    }

    public unsafe void AllocateMemoryForImage( VkImage Image,VkMemoryPropertyFlagBits MemoryProperties , out VkDeviceMemory imageMemory )
    {
        VkMemoryRequirements memRequirements;
        vkGetImageMemoryRequirements(Device, Image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = FindMemoryType( memRequirements.memoryTypeBits,MemoryProperties);

        fixed (VkDeviceMemory* imgMem = &imageMemory  )
        {
            vkAllocateMemory(Device, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        }

    }

    public unsafe void TransfertMemory(void* ptr , VkDeviceSize bufferSize,ref VkDeviceMemory stagingBufferMemory, bool unmap =true)
    {
        void* indicesdataPtr = null;

        vkMapMemory(Device, stagingBufferMemory, 0, bufferSize, 0, &indicesdataPtr ).Check("Impossible to map memory  for indice");

        Unsafe.CopyBlock( indicesdataPtr , ptr ,(uint)bufferSize);
        
        if (unmap)vkUnmapMemory(Device, stagingBufferMemory);
    }
}

public unsafe readonly struct GraphicsCommandBuffers
{
    
	public unsafe readonly  delegate* unmanaged< VkDevice,VkCommandPoolCreateInfo*,VkAllocationCallbacks*,VkCommandPool*,VkResult > vkCreateCommandPool = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkCommandPool,VkAllocationCallbacks*,void > vkDestroyCommandPool = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkCommandPool,UInt32,VkResult > vkResetCommandPool = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkCommandBufferAllocateInfo*,VkCommandBuffer*,VkResult > vkAllocateCommandBuffers = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkCommandPool,UInt32,VkCommandBuffer*,void > vkFreeCommandBuffers = null;
	public unsafe readonly  delegate* unmanaged< VkCommandBuffer,VkCommandBufferBeginInfo*,VkResult > vkBeginCommandBuffer = null;
	public unsafe readonly  delegate* unmanaged< VkCommandBuffer,VkResult > vkEndCommandBuffer = null;
	public unsafe readonly  delegate* unmanaged< VkCommandBuffer,UInt32,VkResult > vkResetCommandBuffer = null;
    private readonly VkCommandPool _commandPool = VkCommandPool.Null;
    private readonly VkCommandPool _commandPoolCompute = VkCommandPool.Null;
    private readonly VkCommandBuffer[] _commandBuffers = null!;
    private readonly VkDevice _device = VkDevice.Null;


    public unsafe GraphicsCommandBuffers( ref GraphicsDevice data, ref RenderConfig config ) 
    {
        _device = data.Device;

        vkBeginCommandBuffer = (delegate* unmanaged<VkCommandBuffer,VkCommandBufferBeginInfo*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkBeginCommandBuffer)); 
        vkEndCommandBuffer = (delegate* unmanaged<VkCommandBuffer,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkEndCommandBuffer)); 
        vkResetCommandBuffer = (delegate* unmanaged<VkCommandBuffer,UInt32,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkResetCommandBuffer)); 
        vkCreateCommandPool = (delegate* unmanaged<VkDevice,VkCommandPoolCreateInfo*,VkAllocationCallbacks*,VkCommandPool*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkCreateCommandPool)); 
        vkDestroyCommandPool = (delegate* unmanaged<VkDevice,VkCommandPool,VkAllocationCallbacks*,void>) data.vkGetDeviceProcAddr(_device,nameof(vkDestroyCommandPool)); 
        vkResetCommandPool = (delegate* unmanaged<VkDevice,VkCommandPool,UInt32,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkResetCommandPool)); 

        vkAllocateCommandBuffers = (delegate* unmanaged<VkDevice,VkCommandBufferAllocateInfo*,VkCommandBuffer*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkAllocateCommandBuffers)); 
        vkFreeCommandBuffers = (delegate* unmanaged<VkDevice,VkCommandPool,UInt32,VkCommandBuffer*,void>) data.vkGetDeviceProcAddr(_device,nameof(vkFreeCommandBuffers)); 
  
        VkCommandPoolCreateInfo poolInfo = new();
        poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
        poolInfo.pNext = null;
        poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
        poolInfo.queueFamilyIndex = data.Device_QueueFamilies[0];

        /*
        K_COMMAND_POOL_CREATE_TRANSIENT_BIT specifies that command buffers allocated from the pool will be short-lived, meaning that they will be reset or freed in a relatively short timeframe. This flag may be used by the implementation to control memory allocation behavior within the pool.
        spécifie que les tampons de commande alloués à partir du pool seront de courte durée, ce qui signifie qu'ils seront réinitialisés ou libérés dans un délai relativement court. Cet indicateur peut être utilisé par l'implémentation pour contrôler le comportement de l'allocation de mémoire au sein du pool.
VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT allows any command buffer allocated from a pool to be individually reset to the initial state; either by calling vkResetCommandBuffer, or via the implicit reset when calling vkBeginCommandBuffer. If this flag is not set on a pool, then vkResetCommandBuffer must not be called for any command buffer allocated from that pool.
permet à tout tampon de commande alloué à partir d'un pool d'être individuellement réinitialisé à l'état initial, soit en appelant vkResetCommandBuffer, soit via la réinitialisation implicite lors de l'appel à vkBeginCommandBuffer. Si ce drapeau n'est pas activé pour un pool, vkResetCommandBuffer ne doit pas être appelé pour un tampon de commande alloué à partir de ce pool.
VK_COMMAND_POOL_CREATE_PROTECTED_BIT specifies that command buffers allocated from the pool are protected command buffers.
        */

        fixed( VkCommandPool* pool =  &_commandPool)
        {
            vkCreateCommandPool(_device, &poolInfo, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool {_commandPool}  with {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");

        VkCommandPoolCreateInfo poolInfoCompute = new();
        poolInfoCompute.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
        poolInfoCompute.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_TRANSIENT_BIT;
        poolInfoCompute.queueFamilyIndex =data.Device_QueueFamilies[1];

        fixed( VkCommandPool* pool =  &_commandPoolCompute)
        {
            vkCreateCommandPool(_device, &poolInfoCompute, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool {_commandPoolCompute}  with {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");

         _commandBuffers = new VkCommandBuffer[config.MAX_FRAMES_IN_FLIGHT]; 

        VkCommandBufferAllocateInfo allocInfo =new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.commandPool = _commandPool;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandBufferCount = (uint)_commandBuffers.Length;
        
        fixed(VkCommandBuffer* commandBuffer = &_commandBuffers[0] )
        {
            vkAllocateCommandBuffers(_device, &allocInfo, commandBuffer ).Check("failed to allocate command buffers!"); 
        }

        Log.Info($"Create Allocate Command buffer count : {config.MAX_FRAMES_IN_FLIGHT}");
    }

    public unsafe void Dispose(  )
    {
        if (!_device.IsNull && !_commandPool.IsNull)
        {
            Log.Info($"Destroy Command Pool {_commandPool}");
            vkDestroyCommandPool(_device, _commandPool , null);
        }
        if (!_device.IsNull && !_commandPoolCompute.IsNull)
        {
            Log.Info($"Destroy Command Pool {_commandPoolCompute}");
            vkDestroyCommandPool(_device, _commandPoolCompute , null);
        }
    }
   
    /// <summary>
    /// // RECORD COMMANDS WICH CAN THEN BE SUBMITED TO DEVICE QUEUE FOR EXECUTION
    /// </summary>
    public  unsafe void RecordCommandBuffer(int CurrentFrame /*, Render PAss for draw*/)
    {
// START A THREAD  ?
        vkResetCommandBuffer(_commandBuffers[CurrentFrame], (uint)VkCommandBufferResetFlagBits.VK_COMMAND_BUFFER_RESET_RELEASE_NONE);
        
        VkCommandBufferBeginInfo beginInfo = default;
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO; 
        beginInfo.pNext =null;
        beginInfo.flags =(uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT;
        beginInfo.pInheritanceInfo= null;
        
        vkBeginCommandBuffer(_commandBuffers[CurrentFrame], &beginInfo).Check("Failed to Begin command buffer");

        // RenderPass.DrawRenderPass( );

        vkEndCommandBuffer(_commandBuffers[CurrentFrame]).Check("Failed to End command buffer ");
    }
    
    public unsafe VkCommandBuffer BeginSingleTimeCommands()
    {
        
        VkCommandBufferAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandPool = _commandPool;
        allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer = VkCommandBuffer.Null;

        vkAllocateCommandBuffers(_device, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
        beginInfo.flags =(uint) VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        vkBeginCommandBuffer(commandBuffer, &beginInfo);

        return commandBuffer;
    }

    public unsafe  void EndSingleTimeCommands(ref GraphicsQueues queues, VkCommandBuffer commandBuffer) 
    {
        vkEndCommandBuffer(commandBuffer);
               
        queues.SubmitSingleTimeCommandBuffer( commandBuffer , VkFence.Null);

        vkFreeCommandBuffers(_device, _commandPool, 1, &commandBuffer);
    }

}

public unsafe readonly struct GraphicsSynchronizationCacheControl
{
    public unsafe readonly  delegate* unmanaged< VkDevice,VkFenceCreateInfo*,VkAllocationCallbacks*,VkFence*,VkResult > vkCreateFence = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkFence,VkAllocationCallbacks*,void > vkDestroyFence = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,VkFence*,VkResult > vkResetFences = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkFence,VkResult > vkGetFenceStatus = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,VkFence*,UInt32,UInt64,VkResult > vkWaitForFences = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkSemaphoreCreateInfo*,VkAllocationCallbacks*,VkSemaphore*,VkResult > vkCreateSemaphore = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkSemaphore,VkAllocationCallbacks*,void > vkDestroySemaphore = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkEventCreateInfo*,VkAllocationCallbacks*,VkEvent*,VkResult > vkCreateEvent = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkEvent,VkAllocationCallbacks*,void > vkDestroyEvent = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkEvent,VkResult > vkGetEventStatus = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkEvent,VkResult > vkSetEvent = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkEvent,VkResult > vkResetEvent = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,VkResult > vkDeviceWaitIdle = null;
    public readonly VkSemaphore[] ImageAvailableSemaphores = null!;//new VkSemaphore[2];
    public readonly VkSemaphore[] RenderFinishedSemaphores = null!;//new VkSemaphore[2];
    public readonly VkFence[] InFlightFences =null!;// new VkFence[2];
    public readonly VkDevice _device = VkDevice.Null;
    
  
    public GraphicsSynchronizationCacheControl(   ref GraphicsDevice data, ref RenderConfig config )
    {
        _device = data.Device;
        vkCreateFence = (delegate* unmanaged<VkDevice,VkFenceCreateInfo*,VkAllocationCallbacks*,VkFence*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkCreateFence)); 
        vkDestroyFence = (delegate* unmanaged<VkDevice,VkFence,VkAllocationCallbacks*,void>) data.vkGetDeviceProcAddr(_device,nameof(vkDestroyFence)); 
        vkResetFences = (delegate* unmanaged<VkDevice,UInt32,VkFence*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkResetFences)); 
        vkGetFenceStatus = (delegate* unmanaged<VkDevice,VkFence,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkGetFenceStatus)); 
        vkWaitForFences = (delegate* unmanaged<VkDevice,UInt32,VkFence*,uint,UInt64,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkWaitForFences)); 
        vkCreateSemaphore = (delegate* unmanaged<VkDevice,VkSemaphoreCreateInfo*,VkAllocationCallbacks*,VkSemaphore*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkCreateSemaphore));   
        vkDestroySemaphore = (delegate* unmanaged<VkDevice,VkSemaphore,VkAllocationCallbacks*,void>) data.vkGetDeviceProcAddr(_device,nameof(vkDestroySemaphore)); 
        vkCreateEvent = (delegate* unmanaged<VkDevice,VkEventCreateInfo*,VkAllocationCallbacks*,VkEvent*,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkCreateEvent)); 
        vkDestroyEvent = (delegate* unmanaged<VkDevice,VkEvent,VkAllocationCallbacks*,void>) data.vkGetDeviceProcAddr(_device,nameof(vkDestroyEvent)); 
        vkGetEventStatus = (delegate* unmanaged<VkDevice,VkEvent,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkGetEventStatus)); 
        vkSetEvent = (delegate* unmanaged<VkDevice,VkEvent,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkSetEvent)); 
        vkResetEvent = (delegate* unmanaged<VkDevice,VkEvent,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkResetEvent)); 
        vkDeviceWaitIdle = (delegate* unmanaged<VkDevice,VkResult>) data.vkGetDeviceProcAddr(_device,nameof(vkDeviceWaitIdle)); 
        
        ImageAvailableSemaphores = new VkSemaphore[config.MAX_FRAMES_IN_FLIGHT]; 
        RenderFinishedSemaphores = new VkSemaphore[config.MAX_FRAMES_IN_FLIGHT];
        InFlightFences = new VkFence[config.MAX_FRAMES_IN_FLIGHT];

        VkSemaphoreCreateInfo semaphoreInfo =new();
        semaphoreInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO;
        semaphoreInfo.flags =0;
        semaphoreInfo.pNext =null;

        VkFenceCreateInfo fenceInfo= new();
        fenceInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FENCE_CREATE_INFO;
        fenceInfo.flags = (uint)VkFenceCreateFlagBits.VK_FENCE_CREATE_SIGNALED_BIT;


        for (int i = 0; i < config.MAX_FRAMES_IN_FLIGHT; i++)
        {
            fixed( VkSemaphore* imageAvailableSemaphore = &ImageAvailableSemaphores[i])
            {
                vkCreateSemaphore(_device, &semaphoreInfo, null,  imageAvailableSemaphore).Check("Failed to create Semaphore ImageAvailableSemaphore");
            }
            Log.Info($"-{i}  Create Semaphore Image Available : {ImageAvailableSemaphores[i]}");
            
            fixed( VkSemaphore* renderFinishedSemaphore = &RenderFinishedSemaphores[i])
            {
                vkCreateSemaphore(_device, &semaphoreInfo, null, renderFinishedSemaphore).Check("Failed to create Semaphore RenderFinishedSemaphore");
            }
            Log.Info($"-{i}  Create Semaphore render Finish : {RenderFinishedSemaphores[i]}");
            
            fixed(VkFence*  inFlightFence = &InFlightFences[i] )
            {
                vkCreateFence(_device, &fenceInfo, null, inFlightFence).Check("Failed to create Fence InFlightFence");
            }
            Log.Info($"-{i}  Create Fence  : {InFlightFences[i]}");
        }
    }

    public unsafe void Pause(   )
    {
        if ( !_device.IsNull)
        {
            vkDeviceWaitIdle(_device).Check($"WAIT IDLE VkDevice : {_device}");
        }
    }

    public void Dispose()
    {
        if (  !_device.IsNull && RenderFinishedSemaphores != null){
            for ( int i = 0 ; i< RenderFinishedSemaphores.Length ; i++)
            {
                if ( !RenderFinishedSemaphores[i].IsNull)
                {
                    Log.Info($"-{i}  Create Semaphore render Finish : {RenderFinishedSemaphores[i]}");
                    vkDestroySemaphore(_device, RenderFinishedSemaphores[i], null);
                }
            }

        }

        if (  !_device.IsNull && ImageAvailableSemaphores != null){
            for ( int i = 0 ; i< ImageAvailableSemaphores.Length ; i++)
            {
                if ( !ImageAvailableSemaphores[i].IsNull)
                {
                    Log.Info($"-{i}  Create Semaphore Image Available : {ImageAvailableSemaphores[i]}");
                    vkDestroySemaphore(_device, ImageAvailableSemaphores[i], null);
                }
            }

        }

        if (  !_device.IsNull && InFlightFences != null){
            for ( int i = 0 ; i< InFlightFences.Length ; i++)
            {
                if ( !InFlightFences[i].IsNull)
                {
                    Log.Info($"-{i}  Create Fence  : {InFlightFences[i]}");
                    vkDestroyFence(_device,InFlightFences[i], null);
                }
            }

        }
    }

}

public unsafe readonly struct GraphicsFrameBufers
{
    public readonly VkFramebuffer[] Framebuffers = null!;

    public GraphicsFrameBufers( ref GraphicsData data )
    {
        
        int size= data.SwapChain_ImageViews.Length;
        Framebuffers = new VkFramebuffer[size];
    
        for (int i = 0; i < size; i++)
        {
            VkImageView[] attachments = new[]{ data.SwapChain_ImageViews[i] , data.SwapChain_DepthBufferImageViews };

            VkFramebufferCreateInfo framebufferInfo = new();
            framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
            framebufferInfo.renderPass =data.RenderPass; //VkRenderPass.Null; work when nul ?  // TODO RenderPass check why is not important n Framebuffer?
            framebufferInfo.attachmentCount = (uint)attachments.Length ;
            fixed( VkImageView* attachmentPtr =&attachments[0] ) 
            {
                framebufferInfo.pAttachments = attachmentPtr; 
            }
            framebufferInfo.width = data.Device_SurfaceSize.width;
            framebufferInfo.height = data.Device_SurfaceSize.height;
            framebufferInfo.layers = 1;
            framebufferInfo.pNext = null;
            framebufferInfo.flags =(uint)0;
            // framebufferInfo.flags =(uint)VkFramebufferCreateFlagBits.VK_FRAMEBUFFER_CREATE_FLAG_BITS_MAX_ENUM;

            fixed( VkFramebuffer* frame = &data.Framebuffers[i]) 
            {
                vkCreateFramebuffer(data.Device, &framebufferInfo, null, frame).Check("failed to create framebuffer!"); 
            }
            Log.Info($"-{i} Create FrameBuffer {data.Framebuffers[i] }");
        }
    }

    public unsafe  void Dispose( )
    {
        if ( !data.Device.IsNull && data.Framebuffers != null)
        {
            for(int i=0 ; i < data.Framebuffers.Length ; i++) 
            {
                if( !data.Framebuffers[i].IsNull)
                {
                    Log.Info($"- {i} Dispose FrameBuffer {data.Framebuffers[i] }");
                    vkDestroyFramebuffer(data.Device, data.Framebuffers[i], null); 
                }  
            }
        }
    }
}

public unsafe readonly struct GraphicsRenderPass
{
    
    public unsafe void DrawRenderPass( in VkCommandBuffer commandBuffer , uint imageIndex )
    {
    // FOREACH RENDER PASS 
    // for ( int i = 0 ; i < renderPasses; i++){        
        // RENDER PASS --------------------------------------------------------------------------------------------------
        VkRenderPassBeginInfo renderPassInfo = default;
        renderPassInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO; 
        renderPassInfo.pNext = null;
        renderPassInfo.renderArea =data.RenderPass_RenderArea;
        renderPassInfo.renderPass = data.RenderPass;
        renderPassInfo.framebuffer = data.Framebuffers[imageIndex];
        renderPassInfo.clearValueCount = (uint)data.RenderPass_ClearColors.Length;
        fixed(VkClearValue* clearValues = &data.RenderPass_ClearColors[0] ){
            renderPassInfo.pClearValues = clearValues;
        }           
        
        vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);

            Pipelines.DrawPipeline( ref func , ref data , ref config , in commandBuffer);

        vkCmdEndRenderPass(commandBuffer);
    //} // END FOREACH RENDER PASS 
        // END RENDER PASS --------------------------------------------------------------------------------------------------  
    }

    
    public GraphicsRenderPass(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config) 
    {
       
        // COLOR 
        VkAttachmentDescription colorAttachment =new();
        colorAttachment.format = data.Device_ImageFormat;
        colorAttachment.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        colorAttachment.loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR;
        colorAttachment.storeOp = VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_STORE;
        colorAttachment.stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE;
        colorAttachment.stencilStoreOp =VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_DONT_CARE;
        colorAttachment.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        colorAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
        colorAttachment.flags = (uint)VkAttachmentDescriptionFlagBits.VK_ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT;

        //ONLY IF DEPTH RESOURCE
        VkAttachmentDescription depthAttachment =new();
        depthAttachment.format = data.Device_DepthBufferImageFormat;
        depthAttachment.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        depthAttachment.loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR;
        depthAttachment.storeOp = VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_STORE;
        depthAttachment.stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE;
        depthAttachment.stencilStoreOp =VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_DONT_CARE;
        depthAttachment.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        depthAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL;
        depthAttachment.flags = (uint)VkAttachmentDescriptionFlagBits.VK_ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT;

        VkAttachmentDescription* attachments = stackalloc VkAttachmentDescription[] { colorAttachment, depthAttachment };

        // SUBPASS  -> COLOR POST PROCESSING       
        VkAttachmentReference colorAttachmentRef = new();
        colorAttachmentRef.attachment =0 ;
        colorAttachmentRef.layout =VkImageLayout. VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;
         
        // SUBPASS -> DEPTH POST PROCESSING
        VkAttachmentReference depthAttachmentRef = new();
        depthAttachmentRef.attachment =1;
        depthAttachmentRef.layout =VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL;

        // SUBPASS
        VkSubpassDescription subpass = new() ;
        subpass.pipelineBindPoint = VkPipelineBindPoint. VK_PIPELINE_BIND_POINT_GRAPHICS;
        subpass.colorAttachmentCount = 1;
        subpass.pColorAttachments = &colorAttachmentRef;
        subpass.pDepthStencilAttachment =&depthAttachmentRef;
        subpass.flags =0;
        subpass.inputAttachmentCount=0;
        subpass.pInputAttachments = null;
        subpass.pPreserveAttachments = null;
        subpass.preserveAttachmentCount=0;


        VkSubpassDependency dependency =new();
        dependency.srcSubpass = VK.VK_SUBPASS_EXTERNAL;
        dependency.dstSubpass = 0;
        dependency.srcStageMask = (uint)VkPipelineStageFlagBits. VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT | (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT;
        dependency.srcAccessMask = 0;
        dependency.dstStageMask =(uint) VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT| (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT;
        dependency.dstAccessMask =(uint)VkAccessFlagBits. VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT| (uint)VkAccessFlagBits.VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT;
        dependency.dependencyFlags =0;

        VkRenderPassCreateInfo renderPassInfo = new();
        renderPassInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO;
        renderPassInfo.attachmentCount = 2 ;
        renderPassInfo.pAttachments = attachments ;
        renderPassInfo.subpassCount = 1;
        renderPassInfo.pSubpasses = &subpass;
        renderPassInfo.dependencyCount = 1;
        renderPassInfo.pDependencies = &dependency;
        renderPassInfo.flags =0;
        renderPassInfo.pNext =null;

        fixed( VkRenderPass* renderPass= &data.RenderPass )
        {
            vkCreateRenderPass(data.Device, &renderPassInfo, null, renderPass).Check("failed to create render pass!");
        }

        Log.Info($"Create Render Pass : {data.RenderPass}");
    }

    public unsafe void Dispose(  )
    {
        if (!data.Device.IsNull && !data.RenderPass.IsNull)
        {
            Log.Info($"Destroy Render Pass : {data.RenderPass}");
            vkDestroyRenderPass(data.Device,data.RenderPass,null);
        }
    }
}


public static class ResourceCreation
{

    public unsafe readonly struct ImageViewCreation 
    {
        public unsafe readonly  delegate* unmanaged< VkDevice,VkImageViewCreateInfo*,VkAllocationCallbacks*,VkImageView*,VkResult > vkCreateImageView = null;
        public readonly VkDevice Device;
        public readonly VkImageViewType ImageviewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        public readonly VkFormat Format;
        public readonly VkImageAspectFlagBits Aspect = VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT;
        public readonly int ComponentSwizzle =0;

        public ImageViewCreation( ref GraphicsDevice data ,VkFormat format ,   VkImageAspectFlagBits aspect = VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT , VkImageViewType imageviewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D, int componentSwizzle =0)
        {
            Device= data.Device; Format=format;Aspect = aspect;ComponentSwizzle =componentSwizzle;ImageviewType = imageviewType;
            vkCreateImageView = (delegate* unmanaged<VkDevice,VkImageViewCreateInfo*,VkAllocationCallbacks*,VkImageView*,VkResult>) data.vkGetDeviceProcAddr(Device,nameof(vkCreateImageView)); 
        }

        public unsafe VkImageView CreateImageView(VkImage image  )
        {
            VkImageViewCreateInfo viewInfo = new();
            viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
            viewInfo.pNext = null;
            // viewInfo.flags =0; ? 

            viewInfo.image = image;
            viewInfo.viewType =ImageviewType;
            viewInfo.format = Format;
            viewInfo.subresourceRange.aspectMask =  (uint)Aspect;
            viewInfo.subresourceRange.baseMipLevel = 0;
            viewInfo.subresourceRange.levelCount = 1;
            viewInfo.subresourceRange.baseArrayLayer = 0;
            viewInfo.subresourceRange.layerCount = 1;
            if ( ComponentSwizzle == 0)
            {
                viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            }
            else if (ComponentSwizzle == 1)
            {
                viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
                viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
                viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
                viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
            }
            else if ( ComponentSwizzle == 2)
            {
                viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
                viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
                viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
                viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
            }
            else if ( ComponentSwizzle == 3)
            {
                viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_R;
                viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_G;
                viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_B;
                viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_A;
            }
            else
            {

            }

            VkImageView imageView = VkImageView.Null;
           
            vkCreateImageView(Device, &viewInfo, null, &imageView).Check("failed to create image view!");
           
            return imageView;
        }

    }

    
}

//------ PIPELINE ---------------------------------
public struct PipelineConfig
{

}

public readonly struct PipelineData
{
   

    public PipelineData( ref PipelineConfig config)
    {
      
    }
   
}

//------ IMPLEMENTS ---------------------------------
public static class DeviceImplements
{
    public static void Init(ref GraphicsData data, ref GraphicsConfig config, Window window)
    {
        data.Device = new( ref config.Device, window);
    }

    public static void Dispose(ref GraphicsData data, ref GraphicsConfig config)
    {
        
    }
}

public static class RenderImplements
{
    public static void Init(ref GraphicsData data, ref GraphicsConfig config)
    {



    }

    public static void Dispose(ref GraphicsData data, ref GraphicsConfig config)
    {
        
    }

    public static void Draw(ref GraphicsData data, ref GraphicsConfig config )
    {
        
    }
}

public struct SceneConfig
{

}

public static class PipelineImplements
{
    public static void Load(ref GraphicsData data, ref GraphicsConfig config, ref SceneConfig scene)
    {
        // Convert Scene to Data for pipeline
    }

    public static void Build(ref GraphicsData data, ref GraphicsConfig config, ref SceneConfig scene)
    {
        // Resource Creation descriptor
    }

    
    public static void Dispose(ref GraphicsData data, ref GraphicsConfig config)
    {
        
    }
}
