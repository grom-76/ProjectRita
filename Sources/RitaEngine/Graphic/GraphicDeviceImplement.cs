namespace RitaEngine.Graphic;

using RitaEngine.API.Vulkan;
using RitaEngine.Base;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Strings;
using RitaEngine.Platform;
using VkDeviceSize = UInt64;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicsData : IEquatable<GraphicsData>
{
    public delegate void PFN_GetFrameBuffer( ref uint x , ref uint y);

    public string[] App_ValidationLayers = null!;
    public string[] App_InstanceExtensions = null!;
    public string[] Device_Extensions = null!;
    public uint App_Version=0;
    public VkSurfaceKHR App_Surface = VkSurfaceKHR.Null;
    public VkInstance App_Instance = VkInstance.Null;
    public VkDebugUtilsMessengerEXT App_DebugMessenger = VkDebugUtilsMessengerEXT.Null;

    public VkPhysicalDevice Device_Physical = VkPhysicalDevice.Null;
    public VkPhysicalDeviceProperties Device_Properties = new();
    public VkPhysicalDeviceFeatures Device_Features = new();
    public VkPhysicalDeviceMemoryProperties Device_MemoryProperties = new();
    public VkPresentModeKHR Device_PresentMode = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR ;
    public VkFormat Device_ImageFormat = VkFormat.VK_FORMAT_B8G8R8A8_SRGB;
    public VkFormat Device_DepthBufferImageFormat = VkFormat.VK_FORMAT_D32_SFLOAT;
    public VkExtent2D Device_SurfaceSize = new();
    public VkColorSpaceKHR Device_ImageColor = VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR ;
    public VkSurfaceTransformFlagBitsKHR Device_Transform = VkSurfaceTransformFlagBitsKHR.VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR;
    public uint[] Device_QueueFamilies = new uint[3]{ uint.MaxValue ,uint.MaxValue,uint.MaxValue };
    public uint Device_ImageCount =0;
    public VkDevice Device = VkDevice.Null;
    public VkQueue Device_GraphicQueue = VkQueue.Null;
    public VkQueue Device_PresentQueue = VkQueue.Null;
    public VkQueue Device_ComputeQueue = VkQueue.Null;

    public VkSwapchainKHR SwapChain = VkSwapchainKHR.Null ;
    
    public VkImage[] SwapChain_Images = null!;
    public VkImageView[] SwapChain_ImageViews = null!;
    public VkImage SwapChain_DepthBufferImages = VkImage.Null;
    public VkImageView SwapChain_DepthBufferImageViews = VkImageView.Null;
    public VkDeviceMemory SwapChain_DepthBufferImageMemory = VkDeviceMemory.Null;
    public PFN_GetFrameBuffer SwapChain_GetFrameBufferCallback = null!;
    public VkFramebuffer[] Framebuffers = null!;
      
    
    public GraphicsData() { }
    #region OVERRIDE    
    public override string ToString() => string.Format($"Graphics  Data" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicsData data && this.Equals(data) ;
    public bool Equals(GraphicsData other)=> false ;
    public static bool operator ==(GraphicsData left, GraphicsData right) => left.Equals(right);
    public static bool operator !=(GraphicsData left, GraphicsData  right) => !left.Equals(right);
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicsConfig : IEquatable<GraphicsConfig>
{
    public InstanceConfig Instance = new();
    public SwapChainConfig SwapChain = new();
    public DeviceConfig Device = new();

    public GraphicsConfig() { }

    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct InstanceConfig
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

        public InstanceConfig() { }
    }

    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct DeviceConfig
    {

        public string[] DeviceExtensionsManualAdd = new string[] { VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME  };
        public VkPresentModeKHR PresentModePreferred = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR ;
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

        public void Vsync( bool disable )
        {
            PresentModePreferred = disable ? VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR : VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR ;
        }


        public DeviceConfig() { }
    }        

    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct SwapChainConfig
    {
        
        public bool Stereoscopic3DApp = false ;
        public bool Clipped =false;
        public VkCompositeAlphaFlagBitsKHR CompositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;

        public SwapChainConfig() { }
    }        

     #region OVERRIDE    
    public override string ToString() => string.Format($"Graphics Config" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicsConfig data && this.Equals(data) ;
    public bool Equals(GraphicsConfig other)=>false;
    public static bool operator ==(GraphicsConfig left, GraphicsConfig right) => left.Equals(right);
    public static bool operator !=(GraphicsConfig left, GraphicsConfig  right) => !left.Equals(right);
    #endregion
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class GraphicsImplement
{

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Instance
{
    public unsafe static void Init(ref VulkanFunctions funcs , ref GraphicsData data, ref GraphicsConfig config , in Window window  )
    {
        funcs.Loader = new( config.Instance.LibraryName_Vulkan);
        CreateInstanceDebugAndSurface(ref funcs, ref data , ref config , in window);
    }

    public unsafe static void Dispose(ref VulkanFunctions func,ref GraphicsData data)
    {
        DisposeInstanceDebugAndSurface(ref func ,ref data);
        func.Dispose();   
    }
    
    #region INSTANCE , DEBUG & SURFACE 

    public unsafe static void CreateInstanceDebugAndSurface(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config , in Window window )
    {
        // VALIDATION LAYER  --------------------------------------------------------------------
        uint layerCount = 0;
        func.Loader.vkEnumerateInstanceLayerProperties(&layerCount, null).Check("Enumerate instance Layer count");
        Guard.ThrowWhenConditionIsTrue( layerCount ==0 );

        VkLayerProperties* layerProperties = stackalloc VkLayerProperties[(int)layerCount];// ReadOnlySpan<VkLayerProperties> pp = stackalloc VkLayerProperties[(int)count];
        func.Loader.vkEnumerateInstanceLayerProperties(&layerCount, layerProperties).Check("Enumerate instance Layer list");

       data.App_ValidationLayers = new  string[ layerCount ];
        for (int i = 0; i < layerCount; i++) {
            var length = StrHelper.Strlen( layerProperties[i].layerName );
           data.App_ValidationLayers[i] = Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );// new string(layerProperties[i].layerName); //Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );
        }

        //-- VULKAN API VERSION ------------------------------------------------------------------
        fixed ( uint* ver = &data.App_Version)
        {
            func.Loader.vkEnumerateInstanceVersion(ver).Check("Enumerate Instance Version");
        }

        //--INSTANCE EXTENSIONS ------------------------------------------------------------------
        uint extCount = 0;
        func.Loader.vkEnumerateInstanceExtensionProperties(null, &extCount, null).Check( "Enumerate Extension Name Count");
        Guard.ThrowWhenConditionIsTrue( extCount == 0);

        VkExtensionProperties* props = stackalloc VkExtensionProperties[(int)extCount];
        func.Loader.vkEnumerateInstanceExtensionProperties(null, &extCount, props).Check( "Enumerate Extension Name List");

        data.App_InstanceExtensions = new string[extCount ];
        for (int i = 0; i < extCount; i++)
        {
            var length = StrHelper.Strlen( props[i].extensionName);
           data.App_InstanceExtensions[i] =Encoding.UTF8.GetString(  props[i].extensionName, (int) length );
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
        appInfo.apiVersion =data.App_Version; 
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
        using var extNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.App_InstanceExtensions) ;
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.App_ValidationLayers);

        VkInstanceCreateInfo instanceCreateInfo = new();
        instanceCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
        instanceCreateInfo.flags =  (uint)VkInstanceCreateFlagBits.VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR;       
        instanceCreateInfo.pApplicationInfo =&appInfo;
        instanceCreateInfo.pNext= !config.Instance.EnableDebugMode ? null :  (VkDebugUtilsMessengerCreateInfoEXT*) &debugCreateInfo;
        instanceCreateInfo.ppEnabledExtensionNames = extNames;
        instanceCreateInfo.enabledExtensionCount =extNames.Count;
        instanceCreateInfo.enabledLayerCount = config.Instance.EnableDebugMode ?layerNames.Count : 0;
        instanceCreateInfo.ppEnabledLayerNames =config.Instance.EnableDebugMode ? layerNames : null;

        fixed( VkInstance* instance = &data.App_Instance)
        {
            func.Loader.vkCreateInstance(&instanceCreateInfo, null, instance).Check("failed to create instance!");
        }

        Log.Info($"Create Debug {data.App_Instance}");

        VK.VK_KHR_swapchain=true;// //Special dont understand pour chage swapchain car nvidia n'a pas l'extension presente
        VkHelper.ValidateExtensionsForLoad(ref data.App_InstanceExtensions,0 );

       func.Instance = new( func.vkGetInstanceProcAddr ,data.App_Instance );

        // CREATE DEBUG ------------------------------------------------------------------------
        if ( !config.Instance.EnableDebugMode  )
        {
            fixed(VkDebugUtilsMessengerEXT* dbg = &data.App_DebugMessenger )
            {
                func.Instance.vkCreateDebugUtilsMessengerEXT(data.App_Instance, &debugCreateInfo, null, dbg).Check("failed to set up debug messenger!");
            }
            Log.Info($"Create Debug {data.App_DebugMessenger}");

        }
        
        
        // CREATE SURFACE -------------------------------------------------------------------------
        #if WIN64
        VkWin32SurfaceCreateInfoKHR sci = new() ;
        sci .hinstance = window.GetWindowHInstance();
        sci .hwnd = window.GetWindowHandle();
        sci .pNext = null;
        sci .flags = 0;
        sci .sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;

        fixed ( VkSurfaceKHR* surf = &data.App_Surface)
        {
            func.Instance.vkCreateWin32SurfaceKHR(data.App_Instance,&sci,null, surf).Check("Create Surface");
        }
        Log.Info($"Create Surface {data.App_Surface}");
        #endif
    }

    public unsafe static void DisposeInstanceDebugAndSurface(ref VulkanFunctions func,ref GraphicsData data)
    {
        if ( !data.App_Instance.IsNull && !data.App_Surface.IsNull)
        {
            Log.Info($"Release Surface [{data.App_Surface}]");
            func.Instance.vkDestroySurfaceKHR(data.App_Instance,data.App_Surface,null);
        }

        if (!data.App_Instance.IsNull && !data.App_DebugMessenger.IsNull){
            Log.Info($"Release DebugMessenger [{data.App_DebugMessenger}]");
            func.Instance.vkDestroyDebugUtilsMessengerEXT(data.App_Instance,data.App_DebugMessenger,null);
        } 

        if ( !data.App_Instance.IsNull){
            Log.Info($"Release Instance [{data.App_Instance}]");
            func.Loader.vkDestroyInstance(data.App_Instance, null);
            data.App_Instance = VkInstance.Null;
        }
    }
  
    #endregion
    #region HELPER

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


    #endregion
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Device
{
    public unsafe static void Init(ref VulkanFunctions funcs , ref GraphicsData data, ref GraphicsConfig config , in Window window  )
    {
        CreateDevicePhysicalLogicQueues(ref funcs, ref data , ref config , in window);        
    }

    public unsafe static void Dispose(ref VulkanFunctions func,ref GraphicsData data)
    {
        DisposeDevicePhysicalLogicQueues(ref func , ref data);
    }
    
    #region PHYSICAL DEVICE & Device

    public static unsafe void CreateDevicePhysicalLogicQueues( ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config , in Window window  )
    {
        // SELECT PHYSICAL DEVICE
        uint deviceCount = 0;
        func.Instance.vkEnumeratePhysicalDevices(data.App_Instance, &deviceCount, null).Check("EnumeratePhysicalDevices Count");
        Guard.ThrowWhenConditionIsTrue(deviceCount == 0,"Vulkan: Failed to find GPUs with Vulkan support");

        VkPhysicalDevice* devices = stackalloc VkPhysicalDevice[(int)deviceCount];
        func.Instance.vkEnumeratePhysicalDevices(data.App_Instance, &deviceCount, devices).Check("EnumeratePhysicalDevices List");

        for (int i = 0; i < (int)deviceCount; i++)
        {
            data.Device_Physical = devices[i];

            GetPhysicalDeviceInformations( ref func ,ref data,ref config, in window);
            if ( !data.Device_Physical.IsNull )
                break;

        }
        Guard.ThrowWhenConditionIsTrue( data.Device_Physical.IsNull , "Physical device is null ");

        Log.Info($"Select Physical device {data.Device_Physical}");


        // CREATE DEVICE
        float queuePriority = 1.0f;
        
        var queueFamiliesCount = data.Device_QueueFamilies[0] == data.Device_QueueFamilies[2] ? data.Device_QueueFamilies.Length-1 : data.Device_QueueFamilies.Length ;
        queueFamiliesCount = data.Device_QueueFamilies[0] == data.Device_QueueFamilies[1] ? queueFamiliesCount-1 : queueFamiliesCount ;

        VkDeviceQueueCreateInfo* queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[queueFamiliesCount];

        for( uint i = 0; i < queueFamiliesCount ; i++ )
        {
            queueCreateInfos[i] = new VkDeviceQueueCreateInfo {
                sType = VkStructureType. VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = data.Device_QueueFamilies[i],
                queueCount = 1,
                pQueuePriorities = &queuePriority
            };
        }

        // CREATE DEVICE INFO ---------------------------------------------------------
        using var deviceExtensions = new StrArrayUnsafe(ref data.Device_Extensions);
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.App_ValidationLayers);
        VkDeviceCreateInfo createInfo = new();
        createInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
        createInfo.queueCreateInfoCount = (uint)queueFamiliesCount;
        createInfo.pQueueCreateInfos = queueCreateInfos;
        fixed ( VkPhysicalDeviceFeatures* features = &data.Device_Features) {createInfo.pEnabledFeatures = features;}
        createInfo.enabledExtensionCount = (uint)deviceExtensions.Count;
        createInfo.ppEnabledExtensionNames = deviceExtensions;
        createInfo.pNext = null ;
        createInfo.enabledLayerCount = config.Instance.EnableDebugMode ? layerNames.Count : 0 ;
        createInfo.ppEnabledLayerNames = config.Instance.EnableDebugMode ? layerNames : null ;

        fixed(VkDevice* device = &data.Device)
        {
            func.Instance.vkCreateDevice(data.Device_Physical, &createInfo, null, device).Check("Error creation vkDevice");
        }
       
       VkHelper.ValidateExtensionsForLoad(ref data.Device_Extensions,0 );

       func.Device = new(func.vkGetDeviceProcAddr, data.Device );

       Log.Info($"Create Device :{data.Device}");

        fixed(VkQueue* gq =&data.Device_GraphicQueue)
        {
            func.Device.vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[0], 0,gq);
        }
        fixed(VkQueue* pq = &data.Device_PresentQueue)
        {
            func.Device.vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[2], 0, pq); 
        }
        fixed(VkQueue* cq = &data.Device_ComputeQueue)
        {
            func.Device.vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[1], 0, cq); 
        }

        Log.Info($"Graphic Queue : indice :{ data.Device_QueueFamilies[0]}  Adr[{data.Device_GraphicQueue}]");
        Log.Info($"Compute Queue : indice :{ data.Device_QueueFamilies[1]}  Adr[{data.Device_ComputeQueue}]");
        Log.Info($"Present Queue : indice :{ data.Device_QueueFamilies[2]}  Adr[{data.Device_PresentQueue}]");
    }

    public static unsafe void DisposeDevicePhysicalLogicQueues(ref VulkanFunctions func,ref GraphicsData data )
    {       
        if ( !data.Device.IsNull)
        {
            Log.Info($"Dispose Logical Device {data.Device}");
            func.Instance.vkDestroyDevice(data.Device, null);
        }  
    }

    #endregion

    #region Helper

    
    private static unsafe void GetPhysicalDeviceInformations(  ref VulkanFunctions func,ref GraphicsData data, ref GraphicsConfig config , in Window window )
    {
        // GET QUEUES 
        uint queueFamilyPropertyCount = 0;
        func.Instance.vkGetPhysicalDeviceQueueFamilyProperties(data.Device_Physical, &queueFamilyPropertyCount, null);

        ReadOnlySpan<VkQueueFamilyProperties> queueFamilyProperties = new VkQueueFamilyProperties[queueFamilyPropertyCount];
        
        fixed (VkQueueFamilyProperties* queueFamilyPropertiesPtr = queueFamilyProperties){
            func.Instance.vkGetPhysicalDeviceQueueFamilyProperties(data.Device_Physical, &queueFamilyPropertyCount, queueFamilyPropertiesPtr);
        }

        for( uint i = 0 ; i <queueFamilyProperties.Length ; i++ )
        {
            // var flag = queueFamilyProperties[(int)i].queueFlags;
            if ( (queueFamilyProperties[(int)i].queueFlags & VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT) != 0)
            {
                data.Device_QueueFamilies[0] = i;
                
            }
            if ( (queueFamilyProperties[(int)i].queueFlags & VkQueueFlagBits.VK_QUEUE_COMPUTE_BIT) != 0   && queueFamilyProperties[(int)i].queueCount > 1 )
            {
                data.Device_QueueFamilies[1] = i;
            }
            if( SupportPresenting(ref func,ref data, i)  && data.Device_QueueFamilies[2] == uint.MaxValue )
            {
                data.Device_QueueFamilies[2] = i;
            }

            if (data.Device_QueueFamilies[0] != uint.MaxValue && data.Device_QueueFamilies[2] != uint.MaxValue && data.Device_QueueFamilies[1] != uint.MaxValue )
            { break; }
        }

        if (data.Device_QueueFamilies[0] == uint.MaxValue || data.Device_QueueFamilies[2] == uint.MaxValue || data.Device_QueueFamilies[1] == uint.MaxValue )
        {
            data.Device_Physical  = VkPhysicalDevice.Null ;
            return;
        }

        // if( data.Device_Properties.deviceType ==   VkPhysicalDeviceType.VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU ) 
        // {
        //     //Always better
        // }

        // Capabilities
        VkSurfaceCapabilitiesKHR Capabilities;
        func.Instance.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(data.Device_Physical, data.App_Surface, &Capabilities ).Check("vkGetPhysicalDeviceSurfaceCapabilitiesKHR");

        if ( Capabilities.currentExtent.width != uint.MaxValue)
        {
            data.Device_SurfaceSize = Capabilities.currentExtent;
        } else  {
           
            data.Device_SurfaceSize.width = ClampUInt( (uint)window.GetWindowWidth(), Capabilities.minImageExtent.width, Capabilities.maxImageExtent.width);
            data.Device_SurfaceSize.height = ClampUInt( (uint)window.GetWindowheight(), Capabilities.minImageExtent.height, Capabilities.maxImageExtent.height);
          
        }

        data.Device_ImageCount = Capabilities.minImageCount + 1;
        if (Capabilities.maxImageCount > 0 && data.Device_ImageCount > Capabilities.maxImageCount)
        {
            data.Device_ImageCount = Capabilities.maxImageCount;
        }

        data.Device_Transform = Capabilities.currentTransform;

        // Surface Format -------------------------------------------------------------------------------------------------------------------------------------------
        uint surfaceFormatCount = 0;
        func.Instance.vkGetPhysicalDeviceSurfaceFormatsKHR(data.Device_Physical, data.App_Surface,  &surfaceFormatCount, null).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");

        ReadOnlySpan<VkSurfaceFormatKHR> surfaceFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
        fixed (VkSurfaceFormatKHR* surfaceFormatsPtr = surfaceFormats)		{
            func.Instance.vkGetPhysicalDeviceSurfaceFormatsKHR(data.Device_Physical, data.App_Surface,  &surfaceFormatCount, surfaceFormatsPtr).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");
        }

        data.Device_ImageFormat = surfaceFormats[0].format;
        data.Device_ImageColor =surfaceFormats[0].colorSpace;

        foreach (VkSurfaceFormatKHR availableFormat in surfaceFormats)
        {
            if (availableFormat.format == config.Device.SurfaceFormatPreferred && availableFormat.colorSpace == config.Device.ColorFormatPreferred )
            {
                data.Device_ImageFormat = availableFormat.format;
                data.Device_ImageColor = availableFormat.colorSpace  ;
                break;
            }
        }

        
        foreach ( VkFormat format in config.Device.DepthFormatCandidat )
        {
            //Get PhysicalFormatProperties --------------------------------------------------------
            VkFormatProperties formatProperties;
            func.Instance.vkGetPhysicalDeviceFormatProperties(data.Device_Physical,format, &formatProperties);

            if (config.Device.DepthImageTilingPreferred == VkImageTiling.VK_IMAGE_TILING_LINEAR && (formatProperties.linearTilingFeatures & (uint)config.Device.DepthFormatFeature) == (uint)config.Device.DepthFormatFeature) 
            {
               
                data.Device_DepthBufferImageFormat = format;
            } 
            else if (config.Device.DepthImageTilingPreferred == VkImageTiling.VK_IMAGE_TILING_OPTIMAL && (formatProperties.optimalTilingFeatures & (uint)config.Device.DepthFormatFeature) == (uint)config.Device.DepthFormatFeature) 
            {
                
                data.Device_DepthBufferImageFormat = format;
            }
        }
        Log.Info($"Depth Format : { data.Device_DepthBufferImageFormat.ToString() }");
        
        // Present mode -----------------------------------------------------------------------------------------------------------------------
        uint presentModeCount = 0;
        func.Instance.vkGetPhysicalDeviceSurfacePresentModesKHR(data.Device_Physical, data.App_Surface, &presentModeCount, null).Check("vkGetPhysicalDeviceSurfacePresentModesKHR Count");

        ReadOnlySpan<VkPresentModeKHR> presentModes = new VkPresentModeKHR[presentModeCount];
        fixed (VkPresentModeKHR* presentModesPtr = presentModes)	{
            func.Instance.vkGetPhysicalDeviceSurfacePresentModesKHR(data.Device_Physical, data.App_Surface, &presentModeCount, presentModesPtr).Check("vkGetPhysicalDeviceSurfacePresentModesKHR List");
        }
        
        data.Device_PresentMode = VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
        foreach (VkPresentModeKHR availablePresentMode in presentModes)
        {
            if (availablePresentMode == config.Device.PresentModePreferred)
            {
                data.Device_PresentMode = availablePresentMode;
                break;
            }
        }

        // PHYSICAL DEVICE PROPERTIES -------------------------------------------
        fixed (VkPhysicalDeviceProperties* phd =   &data.Device_Properties)
        {
            func.Instance.vkGetPhysicalDeviceProperties(data.Device_Physical ,phd );
        }

        // GET Physical FEATURES ----------------------------------------------------------

        fixed ( VkPhysicalDeviceFeatures* features = &data.Device_Features)
        {
            func.Instance.vkGetPhysicalDeviceFeatures(data.Device_Physical,features );
        } 

        // GET MEMORY PROPERTIES -------------------------------------------------------------------
        fixed ( VkPhysicalDeviceMemoryProperties* mem = &data.Device_MemoryProperties )
        {
            func.Instance.vkGetPhysicalDeviceMemoryProperties(data.Device_Physical, mem);
        }

        // DEVICE  EXTENSIONS -------------------------------------------------
        uint propertyCount = 0;
        func.Instance.vkEnumerateDeviceExtensionProperties(data.Device_Physical, null, &propertyCount, null).Check();

        VkExtensionProperties* properties = stackalloc VkExtensionProperties[(int)propertyCount];  
        func.Instance.vkEnumerateDeviceExtensionProperties(data.Device_Physical, null, &propertyCount, properties).Check();

        int addext = config.Device.DeviceExtensionsManualAdd is null ? 0 : config.Device.DeviceExtensionsManualAdd.Length;

        data.Device_Extensions = new string[propertyCount + addext];
        
        for (int i = 0; i < propertyCount; i++){
            var length =  StrHelper.Strlen( properties[i].extensionName);
           data.Device_Extensions[i] = Encoding.UTF8.GetString( properties[i].extensionName, (int) length ); 
        }

        if ( config.Device.DeviceExtensionsManualAdd is null)return ;

        for( int i = 0 ;i < addext ; i++ )
        {
            data.Device_Extensions[i+ propertyCount ] = VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME ;
        }

    }

    private static unsafe bool SupportPresenting(ref VulkanFunctions func,ref GraphicsData data, uint i)
    {
        uint presentSupport = 0;
        //Querying for WSI Support
        func.Instance.vkGetPhysicalDeviceSurfaceSupportKHR(data.Device_Physical, i, data.App_Surface, &presentSupport);
        // #if WIN64 // if not work use directly platform Presentation support method
        // var supported = func.Instance.vkGetPhysicalDeviceWin32PresentationSupportKHR(data.Device_Physical, i);
        // #endif
        return  (presentSupport == VK.VK_TRUE) ? true : false;
    }

    private static uint ClampUInt(uint value, uint min, uint max) =>value < min ? min : value > max ? max : value;

    
    #endregion
   
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class SwapChain
{
    public unsafe static void Init( ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config , in Window window)
    {
        data.SwapChain_GetFrameBufferCallback = window.GetFrameBuffer;
        CreateSwapChain(ref func,ref data, ref config);
        CreateSwapChainImages(ref func, ref data, ref config);
        CreateDepthResources( ref func, ref data, ref config);
        CreateFramebuffers(ref func, ref data);
    }

    public unsafe static void Dispose(ref VulkanFunctions func,ref GraphicsData data  )
    {
        if ( data.SwapChain == VkSwapchainKHR.Null)return ;

        Pause(ref func,ref data);
        DisposeDepthResources(ref  func , ref data);
        DisposeFrameBuffer(ref func, ref data);
        DisposeSwapChain(ref func , ref data);
    }

    public unsafe static void Pause(ref VulkanFunctions func,ref GraphicsData data   )
    {
        if ( !data.Device.IsNull)
        {
            func.Device.vkDeviceWaitIdle(data.Device).Check($"WAIT IDLE VkDevice : {data.Device}");
        }
    }

    public unsafe static void ReCreateSwapChain(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config)
    {
        if ( data.SwapChain == VkSwapchainKHR.Null)return ;

        data.SwapChain_GetFrameBufferCallback(ref data.Device_SurfaceSize.width ,ref data.Device_SurfaceSize.height );

        Pause(ref func,ref data);
        DisposeDepthResources(ref  func , ref data);
        DisposeFrameBuffer(ref func, ref data);
        DisposeSwapChain(ref func , ref data);
       
        CreateSwapChain(ref func,ref data, ref config);
        CreateSwapChainImages(ref func, ref data, ref config);
        CreateDepthResources( ref func, ref data, ref config);
        CreateFramebuffers(ref func, ref data);
    }

    public static unsafe void CreateSwapChain(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config)
    {

        uint* queueFamilyIndices = stackalloc uint[2]{data.Device_QueueFamilies[0], data.Device_QueueFamilies[2]};

        VkSwapchainCreateInfoKHR createInfo = new();
        createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
        createInfo.pNext = null;
        createInfo.surface = data.App_Surface;
        createInfo.minImageCount = data.Device_ImageCount;
        createInfo.imageFormat =  data.Device_ImageFormat;
        createInfo.imageColorSpace = data.Device_ImageColor;
        createInfo.imageExtent = data.Device_SurfaceSize;
        createInfo.imageArrayLayers = config.SwapChain.Stereoscopic3DApp ? (uint)2 : (uint)1;

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
        createInfo.compositeAlpha = config.SwapChain.CompositeAlpha;
        createInfo.presentMode = data.Device_PresentMode;
        createInfo.clipped = config.SwapChain.Clipped ? VK.VK_TRUE : VK.VK_FALSE;
        createInfo.oldSwapchain = VkSwapchainKHR.Null;

        fixed (VkSwapchainKHR* swapchainPtr = &data.SwapChain)
        {
            func.Device.vkCreateSwapchainKHR(data.Device, &createInfo, null, swapchainPtr).Check("failed to create swap chain!");
        }

        Log.Info($"Create SwapChain {data.SwapChain}");
    }

    public static unsafe void CreateSwapChainImages(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
        // SWWAP CHAIN IMAGES  ----------------------------------------------------------------------
        uint imageCount = 0 ;
        func.Device.vkGetSwapchainImagesKHR(data.Device, data.SwapChain, &imageCount, null);
       
        data.SwapChain_Images = new VkImage[imageCount];

        fixed (VkImage* swapchainImagesPtr = data.SwapChain_Images){
            func.Device.vkGetSwapchainImagesKHR(data.Device,data.SwapChain, &imageCount, swapchainImagesPtr).Check("vkGetSwapchainImagesKHR");
        }
        
        Log.Info($"Create {data.SwapChain_Images.Length} SwapChainImages ");

        data.Device_ImageCount = imageCount  ;
        // SWWAP CHAIN IMAGES  VIEWS ----------------------------------------------------------------------
        
        data.SwapChain_ImageViews = new VkImageView[imageCount ];

        for (uint i = 0; i < imageCount; i++)
        {
            Memories.ImageViewConfig temp = new( data.SwapChain_Images[i],data.Device_ImageFormat, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT);
            Memories.CreateImageView( ref func,ref data, in temp, out data.SwapChain_ImageViews[i]);
            Log.Info($"\t -[{i}] {data.SwapChain_Images[i]} : {data.Device_ImageFormat} other info ...."); 
        }
    }
    
    public static unsafe void DisposeSwapChain(ref VulkanFunctions func,ref GraphicsData data  )
    {
        // if(  !data.Device.IsNull && data.SwapChain_Images != null)
        // {
        //     Log.Info($"dispose swap chain Image {data.SwapChain_Images } ");
        //     foreach (var image in data.SwapChain_Images)
        //     {
        //         func.Device.vkDestroyImage(data.Device, image, null); 
        //     }
        // }

        if (!data.Device.IsNull && data.SwapChain_ImageViews != null) 
        {
            Log.Info($"Release Swap chain Images View [{data.SwapChain_ImageViews.Length}]");
            foreach (var imageView in data.SwapChain_ImageViews)
            {
                func.Device.vkDestroyImageView(data.Device, imageView, null); 
            }
        }

        if ( !data.Device.IsNull && !data.SwapChain.IsNull )
        {
            Log.Info($"Release Swapchain Instancew [{data.SwapChain}]");
            func.Device.vkDestroySwapchainKHR(data.Device, data.SwapChain, null);
        }
    }

    #region DEpth Buffering 

    public unsafe static void CreateDepthResources(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config)
    {
       //Create IMAge
        Memories.ImageConfig imageconfig = new(  new(data.Device_SurfaceSize.width,data.Device_SurfaceSize.height, 1) ,data.Device_DepthBufferImageFormat,VkImageTiling.VK_IMAGE_TILING_OPTIMAL  , VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT );
        Memories.CreateImage( ref func , ref data , imageconfig , out data.SwapChain_DepthBufferImages);

        Memories.ImageMemoryConfig imageMemoryConfig = new( data.SwapChain_DepthBufferImages,VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT  );
        Memories.CreateImageMemory(ref func , ref data , imageMemoryConfig , out data.SwapChain_DepthBufferImageMemory);
        
        Memories.ImageViewConfig temp = new( data.SwapChain_DepthBufferImages, data.Device_DepthBufferImageFormat, VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT);
        Memories.CreateImageView( ref func,ref data, in temp, out data.SwapChain_DepthBufferImageViews);
    }

    public unsafe static void DisposeDepthResources(ref VulkanFunctions func,ref GraphicsData data )
    {
        Memories.DisposeImageImageViewImageMemoryConfig config = new( data.SwapChain_DepthBufferImageViews ,data.SwapChain_DepthBufferImages , data.SwapChain_DepthBufferImageMemory );
        Memories.DisposeImageImageViewImageMemory( ref func , ref data , in config);
    }

    #endregion

    #region FrameBuffer

    public static unsafe void CreateFramebuffers( ref VulkanFunctions func,ref GraphicsData data )
    {
        int size= data.SwapChain_ImageViews.Length;
        data.Framebuffers = new VkFramebuffer[size];
    
        for (int i = 0; i < size; i++)
        {
            VkImageView[] attachments = new[]{ data.SwapChain_ImageViews[i] , data.SwapChain_DepthBufferImageViews };

            VkFramebufferCreateInfo framebufferInfo = new();
            framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
            framebufferInfo.renderPass = VkRenderPass.Null; //data.Handles.RenderPass; // TODO RenderPass check why is not important n Framebuffer?
            framebufferInfo.attachmentCount = (uint)attachments.Length ;
            fixed( VkImageView* attachmentPtr =&attachments[0] ) 
            {
                framebufferInfo.pAttachments = attachmentPtr; 
            }
            framebufferInfo.width = data.Device_SurfaceSize.width;
            framebufferInfo.height = data.Device_SurfaceSize.height;
            framebufferInfo.layers = 1;

            fixed( VkFramebuffer* frame = &data.Framebuffers[i]) 
            {
                func.Device.vkCreateFramebuffer(data.Device, &framebufferInfo, null, frame).Check("failed to create framebuffer!"); 
            }
            Log.Info($"-{i} Create FrameBuffer {data.Framebuffers[i] }");
        }
    }

    public unsafe static void DisposeFrameBuffer(ref VulkanFunctions func,ref GraphicsData data )
    {
        if ( !data.Device.IsNull && data.Framebuffers != null)
        {
            for(int i=0 ; i < data.Framebuffers.Length ; i++) 
            {
                if( !data.Framebuffers[i].IsNull)
                {
                    Log.Info($"- {i} Dispose FrameBuffer {data.Framebuffers[i] }");
                    func.Device.vkDestroyFramebuffer(data.Device, data.Framebuffers[i], null); 
                }  
            }
        }
    }

    #endregion

}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Memories
{
    #region  HELPER    
    public unsafe static uint FindMemoryType(ref VulkanFunctions func,ref GraphicsData data , uint memoryTypeBits, VkMemoryPropertyFlagBits properties)
    {
        uint count = data.Device_MemoryProperties.memoryTypeCount;
        for (uint i = 0; i < count; i++)
        {
            if ( (memoryTypeBits & 1) == 1 && (data.Device_MemoryProperties.memoryTypes[(int)i].propertyFlags & (uint)properties) == (uint)properties)
            {
                return i;
            }
            memoryTypeBits >>= 1;
        }

        return uint.MaxValue;
    }
    #endregion

    #region IMAGE
    public readonly struct ImageViewConfig 
    {
        public readonly VkImage Image;
        public readonly VkImageViewType ImageviewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        public readonly VkFormat Format;
        public readonly VkImageAspectFlagBits Aspect = VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT;
        public readonly int ComponentSwizzle =0;

        public ImageViewConfig( VkImage image,VkFormat format ,   VkImageAspectFlagBits aspect = VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT , int componentSwizzle =0)
        {
            Image= image; Format=format;Aspect = aspect;ComponentSwizzle =componentSwizzle;
        }
    }

    public unsafe static void CreateImageView( ref VulkanFunctions  func, ref GraphicsData data, in ImageViewConfig config, out VkImageView imageViewResult )
    {
        VkImageViewCreateInfo viewInfo = new();
        viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        viewInfo.pNext = null;
        // viewInfo.flags =0; ? 

        viewInfo.image = config.Image;
        viewInfo.viewType =config.ImageviewType;
        viewInfo.format = config.Format;
        viewInfo.subresourceRange.aspectMask =  (uint)config.Aspect;
        viewInfo.subresourceRange.baseMipLevel = 0;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.baseArrayLayer = 0;
        viewInfo.subresourceRange.layerCount = 1;
        if ( config.ComponentSwizzle == 0)
        {
            viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
        }
        else if (config.ComponentSwizzle == 1)
        {
            viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
            viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
            viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
            viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ZERO;
        }
        else if ( config.ComponentSwizzle == 2)
        {
            viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
            viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
            viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
            viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_ONE;
        }
        else if ( config.ComponentSwizzle == 3)
        {
            viewInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_R;
            viewInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_G;
            viewInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_B;
            viewInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_A;
        }
        

        fixed (VkImageView* imageView = &imageViewResult )
        {
            func.Device.vkCreateImageView(data.Device, &viewInfo, null, imageView).Check("failed to create image view!");
        }
        
    }
    
    public readonly struct ImageConfig
    {
        public readonly VkExtent3D Extend3D;
        public readonly VkFormat Format;
        public readonly VkImageTiling  Tiling;
        public readonly VkImageUsageFlagBits Usage ;    

        public ImageConfig(  VkExtent3D extend3D,VkFormat format, VkImageTiling  tiling , VkImageUsageFlagBits usage )
        {
            Extend3D= extend3D; Format=format;Tiling = tiling; Usage = usage;
        }
    }

    public unsafe static void CreateImage( ref VulkanFunctions func, ref GraphicsData data, in ImageConfig config, out VkImage imageResult )
    {
        VkImageCreateInfo imageInfo = new();
        imageInfo.sType =VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
        imageInfo.imageType =VkImageType. VK_IMAGE_TYPE_2D;

        imageInfo.extent = config.Extend3D;
        imageInfo.mipLevels = 1;
        imageInfo.arrayLayers = 1;
        imageInfo.format = config.Format;
        imageInfo.tiling = config.Tiling;
        imageInfo.initialLayout =VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        imageInfo.usage = (uint)config.Usage;
        imageInfo.samples =VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        imageInfo.sharingMode = VkSharingMode. VK_SHARING_MODE_EXCLUSIVE;
        imageInfo.pNext = null;

        fixed( VkImage* img = &imageResult)
        {
            func.Device.vkCreateImage(data.Device, &imageInfo, null,img).Check("failed to create image!");
        }
    }

    public readonly struct ImageMemoryConfig
    {
        public readonly VkImage Image;
        public readonly VkMemoryPropertyFlagBits MemoryProperties;

        public ImageMemoryConfig(  VkImage image,VkMemoryPropertyFlagBits memoryProperties  )
        {
            Image = image;MemoryProperties = memoryProperties;
        }
    }

    public unsafe static void CreateImageMemory( ref VulkanFunctions func, ref GraphicsData data, in ImageMemoryConfig config, out VkDeviceMemory imageMemory )
    {
        VkMemoryRequirements memRequirements;
        func.Device.vkGetImageMemoryRequirements(data.Device, config.Image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = Memories.FindMemoryType(ref func , ref data, memRequirements.memoryTypeBits,config.MemoryProperties);

        fixed (VkDeviceMemory* imgMem = &imageMemory  )
        {
            func.Device.vkAllocateMemory(data.Device, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        }

        func.Device.vkBindImageMemory(data.Device, config.Image, imageMemory, 0).Check("Bind Image Memory");
    }
    public readonly struct DisposeImageImageViewImageMemoryConfig
    {
        public readonly VkImageView ImageView ;
        public readonly VkImage Image ;
        public readonly VkDeviceMemory DeviceMemory ;

        public DisposeImageImageViewImageMemoryConfig(VkImageView imageView, VkImage image,  VkDeviceMemory deviceMemory )
            =>(ImageView,Image,DeviceMemory)=(imageView,image,deviceMemory );
    }

    public unsafe static void DisposeImageImageViewImageMemory(ref VulkanFunctions func,ref GraphicsData data , in DisposeImageImageViewImageMemoryConfig  disposeConfig )
    {
        if (  !data.Device.IsNull && disposeConfig.ImageView != VkImageView.Null )
        {
            func.Device.vkDestroyImageView(data.Device,disposeConfig.ImageView, null);
        }
        if (  !data.Device.IsNull && disposeConfig.DeviceMemory != VkDeviceMemory.Null)
        {
            func.Device.vkFreeMemory(data.Device, disposeConfig.DeviceMemory, null);
        }
        if( !data.Device.IsNull && disposeConfig.Image != VkImage.Null )
        {
            func.Device.vkDestroyImage(data.Device,disposeConfig.Image, null);
        }
    }

    #endregion
    //Dispose
    #region BUFFER
    private unsafe static void CreateBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data ,
        VkDeviceSize size, VkBufferUsageFlagBits usage, 
        VkMemoryPropertyFlagBits  properties,ref VkBuffer buffer,ref VkDeviceMemory bufferMemory) 
    {
        // VkBufferCreateInfo bufferInfo = new();
        // bufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
        // bufferInfo.size = size;
        // bufferInfo.usage = (uint)usage;
        // bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

        // fixed(VkBuffer* buf =  &buffer)
        // {
        //     func.vkCreateBuffer(data.Handles.Device, &bufferInfo, null, buf).Check("failed to create  buffer!");
        // }
        
        // VkMemoryRequirements memRequirements = new();
        // func.vkGetBufferMemoryRequirements(data.Handles.Device, buffer, &memRequirements);

        // VkMemoryAllocateInfo allocInfo = new();
        // allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        // allocInfo.allocationSize = memRequirements.size;
        // allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data,memRequirements.memoryTypeBits, properties);
        // allocInfo.pNext = null;

        // fixed(VkDeviceMemory* memory =  &bufferMemory) 
        // {
        //     func.vkAllocateMemory(data.Handles.Device, &allocInfo, null, memory).Check("failed to allocate memory!");
        // }
        
        // func.vkBindBufferMemory(data.Handles.Device, buffer, bufferMemory, 0).Check("failed to Bind buffer memory!");
    }

    private unsafe static void CopyBuffer(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size)
    {
        // BeginSingleTimeCommands ---------------------------------------------------------
        // 
        VkCommandBufferAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandPool = data.Handles.CommandPool;
        allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer = VkCommandBuffer.Null;
        func.vkAllocateCommandBuffers(data.Handles.Device, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
        beginInfo.flags = (uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        func.vkBeginCommandBuffer(commandBuffer, &beginInfo);
        //
        // BeginSingleTimeCommands ---------------------------------------------------------

        
        VkBufferCopy copyRegion = new();
        copyRegion.size = size;
        func.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);


        // EndSingleTimeCommands --------------------------------------------------
        
        func.vkEndCommandBuffer(commandBuffer);

        VkSubmitInfo submitInfo = new();
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers = &commandBuffer;

        func.vkQueueSubmit(data.Handles.GraphicQueue, 1, &submitInfo, VkFence.Null);
        func.vkQueueWaitIdle(data.Handles.GraphicQueue);

        func.vkFreeCommandBuffers(data.Handles.Device, data.Handles.CommandPool, 1, &commandBuffer);
    }

    
    private unsafe static void CopyBufferToImage(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data,VkBuffer buffer,  uint width, uint height) 
    {
        VkCommandBuffer commandBuffer = BeginSingleTimeCommands(ref func , ref data);

        VkBufferImageCopy region = new();
        region.bufferOffset = 0;
        region.bufferRowLength = 0;
        region.bufferImageHeight = 0;
        region.imageSubresource.aspectMask = (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
        region.imageSubresource.mipLevel = 0;
        region.imageSubresource.baseArrayLayer = 0;
        region.imageSubresource.layerCount = 1;
        
        VkOffset3D offset3D = new(); offset3D.x=0;offset3D.y=0;offset3D.z =0;
        region.imageOffset = offset3D;
        VkExtent3D extent3D = new(); extent3D.width = width; extent3D.height = height; extent3D.depth =1;
        region.imageExtent = extent3D;

        func.vkCmdCopyBufferToImage(commandBuffer, buffer, data.Info.TextureImage, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &region);

        EndSingleTimeCommands(ref func , ref data, commandBuffer);
    }

    #endregion

    #region  MEMORY BARRIER

    private unsafe static void TransitionImageLayout(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data, VkFormat format, VkImageLayout oldLayout, VkImageLayout newLayout) 
    {
        VkCommandBuffer commandBuffer = BeginSingleTimeCommands(ref func , ref data);

        VkImageMemoryBarrier barrier = new();
        barrier.sType =VkStructureType. VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
        barrier.oldLayout = oldLayout;
        barrier.newLayout = newLayout;
        barrier.srcQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
        barrier.dstQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
        barrier.image = data.Info.TextureImage;
        barrier.subresourceRange.aspectMask =(uint) VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
        barrier.subresourceRange.baseMipLevel = 0;
        barrier.subresourceRange.levelCount = 1;
        barrier.subresourceRange.baseArrayLayer = 0;
        barrier.subresourceRange.layerCount = 1;

        VkPipelineStageFlagBits sourceStage=VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
        VkPipelineStageFlagBits destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;

        if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED && newLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
        {
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;

            sourceStage = VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
            destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
        } 
        else if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL && newLayout ==VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL) 
        {
            barrier.srcAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_SHADER_READ_BIT;

            sourceStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
            destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
        } else 
        {
            Guard.ThrowWhenConditionIsTrue(true,"unsupported layout transition!");
        }

        func.vkCmdPipelineBarrier( commandBuffer,
            (uint)sourceStage, (uint)destinationStage,
            0, 0, null, 0, null, 1, &barrier   );

        EndSingleTimeCommands(ref func , ref data ,commandBuffer);
    }

    private unsafe static VkCommandBuffer BeginSingleTimeCommands(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
        
        VkCommandBufferAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandPool = data.Handles.CommandPool;
        allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer = VkCommandBuffer.Null;

        func.vkAllocateCommandBuffers(data.Handles.Device, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
        beginInfo.flags =(uint) VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        func.vkBeginCommandBuffer(commandBuffer, &beginInfo);

        return commandBuffer;
    }

    private unsafe static void EndSingleTimeCommands(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data, VkCommandBuffer commandBuffer) 
    {
        func.vkEndCommandBuffer(commandBuffer);
       
        VkSubmitInfo submitInfo = new();
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers =&commandBuffer;
        
        func.vkQueueSubmit(data.Handles.GraphicQueue, 1, &submitInfo, VkFence.Null);
        func.vkQueueWaitIdle(data.Handles.GraphicQueue);

        func.vkFreeCommandBuffers(data.Handles.Device, data.Handles.CommandPool, 1, &commandBuffer);
    }

    #endregion
    
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Render_Commands
{
    
    #region COMMAND BUFFERS 

    public static unsafe void CreateCommandPool(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data  ) 
    {
        VkCommandPoolCreateInfo poolInfo = new();
        poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
        poolInfo.pNext = null;
        poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
        poolInfo.queueFamilyIndex =data.Info.VkGraphicFamilyIndice;

        /*
        K_COMMAND_POOL_CREATE_TRANSIENT_BIT specifies that command buffers allocated from the pool will be short-lived, meaning that they will be reset or freed in a relatively short timeframe. This flag may be used by the implementation to control memory allocation behavior within the pool.
        spécifie que les tampons de commande alloués à partir du pool seront de courte durée, ce qui signifie qu'ils seront réinitialisés ou libérés dans un délai relativement court. Cet indicateur peut être utilisé par l'implémentation pour contrôler le comportement de l'allocation de mémoire au sein du pool.
VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT allows any command buffer allocated from a pool to be individually reset to the initial state; either by calling vkResetCommandBuffer, or via the implicit reset when calling vkBeginCommandBuffer. If this flag is not set on a pool, then vkResetCommandBuffer must not be called for any command buffer allocated from that pool.
permet à tout tampon de commande alloué à partir d'un pool d'être individuellement réinitialisé à l'état initial, soit en appelant vkResetCommandBuffer, soit via la réinitialisation implicite lors de l'appel à vkBeginCommandBuffer. Si ce drapeau n'est pas activé pour un pool, vkResetCommandBuffer ne doit pas être appelé pour un tampon de commande alloué à partir de ce pool.
VK_COMMAND_POOL_CREATE_PROTECTED_BIT specifies that command buffers allocated from the pool are protected command buffers.
        */

        fixed( VkCommandPool* pool =  &data.Handles.CommandPool)
        {
            func.vkCreateCommandPool(data.Handles.Device, &poolInfo, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool {data.Handles.CommandPool}  with {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");
    }

     public static unsafe void CreateCommandPoolForCompute(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data  ) 
    {
        
        VkCommandPoolCreateInfo poolInfo = new();
        poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
        poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_TRANSIENT_BIT;
        poolInfo.queueFamilyIndex =data.Info.VkGraphicFamilyIndice;

        fixed( VkCommandPool* pool =  &data.Handles.CommandPoolForCompute)
        {
            func.vkCreateCommandPool(data.Handles.Device, &poolInfo, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool {data.Handles.CommandPoolForCompute}  with {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");
    }

    public static unsafe void CreateCommandBuffer(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data ) 
    {
        data.Handles.CommandBuffers = new VkCommandBuffer[data.Info.MAX_FRAMES_IN_FLIGHT]; 

        VkCommandBufferAllocateInfo allocInfo =new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.commandPool = data.Handles.CommandPool;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandBufferCount = (uint)data.Handles.CommandBuffers.Length;
        
        fixed(VkCommandBuffer* commandBuffer = &data.Handles.CommandBuffers[0] )
        {
            func.vkAllocateCommandBuffers(data.Handles.Device, &allocInfo, commandBuffer ).Check("failed to allocate command buffers!"); 
        }

        Log.Info($"Create Allocate Command buffer count : {data.Info.MAX_FRAMES_IN_FLIGHT}");
    }

    public unsafe static void DisposeCommandPool(in GraphicDeviceFunctions  func,ref GraphicDeviceData data )
    {
        if (!data.Handles.Device.IsNull && !data.Handles.CommandPool.IsNull)
        {
            Log.Info($"Destroy Command Pool {data.Handles.CommandPool}");
            func.vkDestroyCommandPool(data.Handles.Device, data.Handles.CommandPool , null);
        }
    }

        public unsafe static void DisposeCommandPoolForCompute(in GraphicDeviceFunctions  func,ref GraphicDeviceData data )
    {
        if (!data.Handles.Device.IsNull && !data.Handles.CommandPoolForCompute.IsNull)
        {
            Log.Info($"Destroy Command Pool {data.Handles.CommandPoolForCompute}");
            func.vkDestroyCommandPool(data.Handles.Device, data.Handles.CommandPoolForCompute , null);
        }
    }
   
    #endregion


    private static int CurrentFrame =0;
    public static unsafe  void Draw(ref GraphicDeviceFunctions func, ref GraphicDeviceData data )
    {
        uint imageIndex=0;
        VkFence CurrentinFlightFence = data.Handles.InFlightFences[/*data.*/CurrentFrame];
        VkSemaphore CurrentImageAvailableSemaphore =  data.Handles.ImageAvailableSemaphores[/*data.*/CurrentFrame];
        VkSemaphore CurrentRenderFinishedSemaphore = data.Handles.RenderFinishedSemaphores[/*data.*/CurrentFrame];
        VkSemaphore* waitSemaphores = stackalloc VkSemaphore[1] {CurrentImageAvailableSemaphore};// VkSemaphore[] waitSemaphores = {CurrentImageAvailableSemaphore};
        UInt32* waitStages  = stackalloc UInt32[1]{(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT }; //*VkPipelineStageFlags*/UInt32[] waitStages = {(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT};
        VkSemaphore*   signalSemaphores  = stackalloc VkSemaphore[1] {CurrentRenderFinishedSemaphore} ;// VkSemaphore[] signalSemaphores = {CurrentRenderFinishedSemaphore};
        VkSwapchainKHR* swapChains = stackalloc  VkSwapchainKHR[1]{ data.Handles.SwapChain };// VkSwapchainKHR[] swapChains = { data.Handles.SwapChain };
        VkCommandBuffer commandBuffer =data.Handles.CommandBuffers[/*data.*/CurrentFrame];

        func.vkWaitForFences(data.Handles.Device, 1,&CurrentinFlightFence, VK.VK_TRUE, data.Info.tick_timeout).Check("Acquire Image");

        VkResult result = func.vkAcquireNextImageKHR(data.Handles.Device, data.Handles.SwapChain, data.Info.tick_timeout,CurrentImageAvailableSemaphore, VkFence.Null, &imageIndex);

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
        {
            // ReCreateSwapChain( ref func,ref data);
            return ;
        }
        else if (result != VkResult.VK_SUCCESS && result != VkResult.VK_SUBOPTIMAL_KHR )
        {
            throw new Exception("Failed to acquire swap chain Images");
        }

        // UpdateUniformBuffer( func,ref  data);
                
        func.vkResetFences(data.Handles.Device, 1, &CurrentinFlightFence);

 
        RecordCommandBuffer(  func,ref data, in  commandBuffer, imageIndex);


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
        
        func.vkQueueSubmit(data.Handles.GraphicQueue, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
        
        VkPresentInfoKHR presentInfo =  default;
        presentInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR; 
        presentInfo.waitSemaphoreCount = 1;
        presentInfo.pWaitSemaphores = signalSemaphores;
        presentInfo.pImageIndices = &imageIndex;
        presentInfo.swapchainCount = 1;
        presentInfo.pSwapchains = swapChains;
        presentInfo.pNext =null;
        presentInfo.pResults = null;
        
        result = func.vkQueuePresentKHR(data.Handles.GraphicQueue, &presentInfo); 

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR || result == VkResult.VK_SUBOPTIMAL_KHR )
        {
        //    ReCreateSwapChain( ref func,ref data);
        }
        else if (result != VkResult.VK_SUCCESS )
        {
            throw new Exception("Failed to  present swap chain Images");
        }

       CurrentFrame = ((CurrentFrame + 1) % data.Info.MAX_FRAMES_IN_FLIGHT);   
    }

    private static unsafe void RecordCommandBuffer(in GraphicDeviceFunctions  func,ref GraphicDeviceData data, in VkCommandBuffer commandBuffer, uint imageIndex)
    {
        func.vkResetCommandBuffer(commandBuffer, (uint)VkCommandBufferResetFlagBits.VK_COMMAND_BUFFER_RESET_RELEASE_NONE);
    // COMMAND BUFFER ----------------------------------------------------------------------------------------------------
        VkCommandBufferBeginInfo beginInfo = default;
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO; 
        beginInfo.pNext =null;
        beginInfo.flags =(uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT;
        beginInfo.pInheritanceInfo= null;
        
        func.vkBeginCommandBuffer(commandBuffer, &beginInfo).Check("Failed to Begin command buffer");

            VkClearValue* clearValues = stackalloc VkClearValue[2] {data.Info.ClearColor,data.Info.ClearColor2 };
        // RENDER PASS --------------------------------------------------------------------------------------------------
            VkRenderPassBeginInfo renderPassInfo = default;
            renderPassInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO; 
            renderPassInfo.renderPass = data.Handles.RenderPass;
            renderPassInfo.framebuffer = data.Handles.Framebuffers[imageIndex];
            renderPassInfo.clearValueCount = 2;
            renderPassInfo.pClearValues = clearValues;
            renderPassInfo.pNext = null;
            renderPassInfo.renderArea =data.Info.RenderArea;
            
            func.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);

// ALL LINE COMMENT  IS EMPTY PIPELINE 
            // // PUSH CONSTANTS ---------- ( do before bin pipeline)
            // // void* ptr = new IntPtr( data.Info.PushConstants).ToPointer();
            // fixed(void* ptr = &data.Info.PushConstants ){
            //     func.vkCmdPushConstants(commandBuffer,data.Handles.PipelineLayout, (uint) VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT, 0,(uint)sizeof(PushConstantsMesh), ptr );
            // }
            
            // // USE SHADER   FOREACH PIPELINE or FOREACH SHADER
            //     func.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.Handles.Pipeline);
                
            // //  FOREACN  MATERIALS  or FOREACH UNIFORMS & TEXTURES(sampler)
            //     fixed(VkDescriptorSet* desc =  &data.Handles.DescriptorSets[CurrentFrame] )
            //     {
            //         func.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.Handles.PipelineLayout, 0, 1, desc, 0, null);
            //     }
                
            // // SET DYNAMIC STATES
            //     fixed(VkViewport* viewport = &data.Info.Viewport ){ func.vkCmdSetViewport(commandBuffer, 0, 1,viewport); }
            //     fixed( VkRect2D* scissor = &data.Info.Scissor) { func.vkCmdSetScissor(commandBuffer, 0, 1, scissor); }
            //     func.vkCmdSetLineWidth( commandBuffer,data.Handles.DynamicStatee_LineWidth);
               

            // //  FOREACH OBJECT/ GEOMETRY 
            //     VkDeviceSize* offsets = stackalloc VkDeviceSize[]{0};
            //     VkBuffer* vertexBuffers = stackalloc VkBuffer[] { data.Handles.VertexBuffer};
            //     func.vkCmdBindVertexBuffers(commandBuffer, 0, 1, vertexBuffers, offsets);
            //     func.vkCmdBindIndexBuffer(commandBuffer, data.Handles.IndicesBuffer, 0, VkIndexType.VK_INDEX_TYPE_UINT16);
            // // DRAW CALLS  ------------ VERTEX INDEXED  
            //     func.vkCmdDrawIndexed(commandBuffer, data.Handles.IndicesSize, 1, 0, 0, 0);

            func.vkCmdEndRenderPass(commandBuffer);
        // RENDER PASS --------------------------------------------------------------------------------------------------        
        func.vkEndCommandBuffer(commandBuffer).Check("Failed to End command buffer ");
    // COMMAND BUFFER ----------------------------------------------------------------------------------------------------    
    }
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Render_Pipeline
{

}

}
