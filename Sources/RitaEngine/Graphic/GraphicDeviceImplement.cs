namespace RitaEngine.Graphic;

using RitaEngine.API.Vulkan;
using RitaEngine.Base;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Strings;
using RitaEngine.Platform;
using VkDeviceSize = UInt64;

public struct GraphicDeviceDatas
{
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
    public VkFormat Device_SurfaceFormat = VkFormat.VK_FORMAT_B8G8R8A8_SRGB;
    public VkExtent2D Device_SurfaceSize = new();
    public uint[] Device_QueueFamilies = new uint[4];
    public uint Device_ImageCount =0;
    public VkDevice Device = VkDevice.Null;
    public VkQueue Device_GraphicQueue = VkQueue.Null;
    public VkQueue Device_PresentQueue = VkQueue.Null;
    public VkQueue Device_ComputeQueue = VkQueue.Null;
    public VkQueue Device_TransfertQueue = VkQueue.Null;

    public GraphicDeviceDatas()
    {
    }
}

public struct GraphicsConfig
{
    
    public SwapChainConfig SwapChain = new();
    public DeviceConfig Device = new();
    public BackEndConfig BackEnd = new(); 

    public GraphicsConfig() { }

    public struct DeviceConfig
    {
       
        public bool EnableDebugMode = false;
        public string[] ValidationLayerExtensions = new string[]{  
        "VK_LAYER_KHRONOS_validation",
        "VK_LAYER_LUNARG_standard_validation",
        "VK_LAYER_GOOGLE_threading",
        "VK_LAYER_LUNARG_parameter_validation",
        "VK_LAYER_LUNARG_object_tracker",
        "VK_LAYER_LUNARG_core_validation",
        "VK_LAYER_GOOGLE_unique_objects", };
        public string[] DeviceExtensionsManualAdd = new string[] { VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME  };
        public VkPresentModeKHR PresentModePreferred = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR ;
        public VkFormat SurfaceFormatPreferred = VkFormat.VK_FORMAT_B8G8R8_SRGB;
        public VkColorSpaceKHR ColorFormatPreferred = VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR;

        public void Vsync( bool disable )
        {
            PresentModePreferred = disable ? VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR : VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR ;
        }


        public DeviceConfig() { }
    }        

    public struct SwapChainConfig
    {
        public bool Stereoscopic3DApp = false ;
        public bool Clipped =false;
        public VkCompositeAlphaFlagBitsKHR CompositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;

        public SwapChainConfig() { }
    }        

    public struct BackEndConfig
    {
        public string LibraryName_Vulkan = "vulkan-1.dll";
        public GraphicDeviceBackend GraphicDevice_BackEnd = GraphicDeviceBackend.Vulkan;
        public GraphicDeviceClipVolume GraphicDevice_ClipVolume = GraphicDeviceClipVolume.ZeroToOne;
        public GraphicDeviceScreenOrigin GraphicDevice_ScreenOrigin =GraphicDeviceScreenOrigin.Center_Y_DownAxis;
        public GraphicDeviceNDC GraphicDevice_NDC = GraphicDeviceNDC.RightHand;

        public BackEndConfig() { }
    }
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class GraphicDeviceImplement
{

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class App
{
    public unsafe static void Init(ref VulkanFunctions funcs , ref GraphicDeviceDatas data, ref GraphicsConfig config , in Window window  )
    {
        funcs.Loader = new( config.BackEnd.LibraryName_Vulkan);

    }
    
    #region INSTANCE , DEBUG & SURFACE 

    public unsafe static void CreateInstanceDebugAndSurface(ref VulkanFunctions func,ref GraphicDeviceDatas data , ref GraphicsConfig config , in Window window )
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
        instanceCreateInfo.pNext= !config.Device.EnableDebugMode ? null :  (VkDebugUtilsMessengerCreateInfoEXT*) &debugCreateInfo;
        instanceCreateInfo.ppEnabledExtensionNames = extNames;
        instanceCreateInfo.enabledExtensionCount =extNames.Count;
        instanceCreateInfo.enabledLayerCount = config.Device.EnableDebugMode ?layerNames.Count : 0;
        instanceCreateInfo.ppEnabledLayerNames =config.Device.EnableDebugMode ? layerNames : null;

        fixed( VkInstance* instance = &data.App_Instance)
        {
            func.Loader.vkCreateInstance(&instanceCreateInfo, null, instance).Check("failed to create instance!");
        };

        Log.Info($"Create Debug {data.App_Instance}");

        VK.VK_KHR_swapchain=true;// //Special dont understand pour chage swapchain car nvidia n'a pas l'extension presente
        VkHelper.ValidateExtensionsForLoad(ref data.App_InstanceExtensions,0 );

       func.Instance = new( func.vkGetInstanceProcAddr ,data.App_Instance );

        // CREATE DEBUG ------------------------------------------------------------------------
        if ( !config.Device.EnableDebugMode  )return ;
        
        fixed(VkDebugUtilsMessengerEXT* dbg = &data.App_DebugMessenger )
        {
            func.Instance.vkCreateDebugUtilsMessengerEXT(data.App_Instance, &debugCreateInfo, null, dbg).Check("failed to set up debug messenger!");
        }
        Log.Info($"Create Debug {data.App_DebugMessenger}");

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

    public unsafe static void DisposeInstanceDebugAndSurface(ref VulkanFunctions func,ref GraphicDeviceDatas data)
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

    #region PHYSICAL DEVICE & Device

    public static unsafe void CreateDevice( ref VulkanFunctions func,ref GraphicDeviceDatas data , ref GraphicsConfig config , in Window window  )
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
        
        var queueFamiliesCount = data.Device_QueueFamilies[0] == data.Device_QueueFamilies[3] ? 3 : 4 ;
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
        createInfo.enabledLayerCount = config.Device.EnableDebugMode ? layerNames.Count : 0 ;
        createInfo.ppEnabledLayerNames = config.Device.EnableDebugMode ? layerNames : null ;

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
            func.Device.vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[3], 0, pq); 
        }
        fixed(VkQueue* cq = &data.Device_ComputeQueue)
        {
            func.Device.vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[1], 0, cq); 
        }
        fixed(VkQueue* tq = &data.Device_TransfertQueue)
        {
            func.Device.vkGetDeviceQueue(data.Device, data.Device_QueueFamilies[2], 0, tq); 
        }

        Log.Info($"Graphic Queue : indice :{ data.Device_QueueFamilies[0]}  Adr[{data.Device_GraphicQueue}]");
        Log.Info($"Compute Queue : indice :{ data.Device_QueueFamilies[1]}  Adr[{data.Device_ComputeQueue}]");
        Log.Info($"Transfert Queue : indice :{ data.Device_QueueFamilies[2]}  Adr[{data.Device_TransfertQueue}]");
        Log.Info($"Present Queue : indice :{ data.Device_QueueFamilies[3]}  Adr[{data.Device_PresentQueue}]");
    }

    public static unsafe void DisposeDevice(ref VulkanFunctions func,ref GraphicDeviceDatas data )
    {       
        if ( !data.Device.IsNull)
        {
            Log.Info($"Dispose Logical Device {data.Device}");
            func.Instance.vkDestroyDevice(data.Device, null);
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


    private static unsafe void GetPhysicalDeviceInformations(  ref VulkanFunctions func,ref GraphicDeviceDatas data, ref GraphicsConfig config , in Window window )
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
            var flag = queueFamilyProperties[(int)i].queueFlags;
            switch (flag)
            {
                case VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT:
                    data.Device_QueueFamilies[0] = i;
                    if( SupportPresenting(func, data, i) )
                        data.Device_QueueFamilies[3] = i;
                    break;
                case VkQueueFlagBits.VK_QUEUE_COMPUTE_BIT:
                    if ( queueFamilyProperties[(int)i].queueCount > 1 )
                        data.Device_QueueFamilies[1] = i;
                    break;
                case VkQueueFlagBits.VK_QUEUE_TRANSFER_BIT:
                    // SEE minImageTransferGranularity  is the minimum granularity supported for image transfer operations 
                    data.Device_QueueFamilies[2] =i;
                    break;
            }

            if (data.Device_QueueFamilies[0] != uint.MaxValue && data.Device_QueueFamilies[3] != uint.MaxValue && data.Device_QueueFamilies[1] != uint.MaxValue && data.Device_QueueFamilies[2] != uint.MaxValue)
            { break; }
        }

        if (data.Device_QueueFamilies[0] == uint.MaxValue || data.Device_QueueFamilies[3] == uint.MaxValue || data.Device_QueueFamilies[1] == uint.MaxValue || data.Device_QueueFamilies[2] == uint.MaxValue)
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

        // Surface Format -------------------------------------------------------------------------------------------------------------------------------------------
        uint surfaceFormatCount = 0;
        func.Instance.vkGetPhysicalDeviceSurfaceFormatsKHR(data.Device_Physical, data.App_Surface,  &surfaceFormatCount, null).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");

        ReadOnlySpan<VkSurfaceFormatKHR> surfaceFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
        fixed (VkSurfaceFormatKHR* surfaceFormatsPtr = surfaceFormats)		{
            func.Instance.vkGetPhysicalDeviceSurfaceFormatsKHR(data.Device_Physical, data.App_Surface,  &surfaceFormatCount, surfaceFormatsPtr).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");
        }

        foreach (VkSurfaceFormatKHR availableFormat in surfaceFormats)
        {
            if (availableFormat.format == config.Device.SurfaceFormatPreferred && availableFormat.colorSpace == config.Device.ColorFormatPreferred )
            {
                data.Device_SurfaceFormat = availableFormat.format;
            }
        }
        data.Device_SurfaceFormat = surfaceFormats[0].format;

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

    private static unsafe bool SupportPresenting(VulkanFunctions func, GraphicDeviceDatas data, uint i)
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
    

    public unsafe static void Pause(in GraphicDeviceFunctions func,ref GraphicDeviceData data  )
    {
        if ( !data.Handles.Device.IsNull){
            func.vkDeviceWaitIdle(data.Handles.Device).Check($"WAIT IDLE VkDevice : {data.Handles.Device}");
        }
    }

    public unsafe static void ReCreateSwapChain(ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        if ( data.Handles.SwapChain == VkSwapchainKHR.Null)return ;

        data.Handles.GetFrameBufferCallback(ref data.Info.VkSurfaceArea.width ,ref data.Info.VkSurfaceArea.height );

        Pause( func,ref data);
        DisposeDepthResources(  func , ref data);
        DisposeFrameBuffer(func, ref data);
        DisposeSwapChain(func , ref data);
       
        CreateSwapChain(ref func,ref data);
        CreateImageViews(ref func, ref data);
        CreateDepthResources( ref func, ref data);
        CreateFramebuffers(ref func,ref data);
        
    }

    public static unsafe void CreateSwapChain(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
         // FOR SWAP CHAIN ----------------------------------------------------
        data.Info.VkPresentMode = ChooseSwapPresentMode(ref data);
        data.Info.VkSurfaceFormat  = ChooseSwapSurfaceFormat(ref data);
        VkFormatProperties formatProperties;
        func.vkGetPhysicalDeviceFormatProperties(data.Handles.PhysicalDevice,data.Info.VkSurfaceFormat.format, &formatProperties);
        
        data.Info.VkSurfaceArea = ChooseSwapExtent(ref data);
        data.Info.VkFormat = data.Info.VkSurfaceFormat.format;

        uint imageCount = data.Info.Capabilities.minImageCount + 1;
        if (data.Info.Capabilities.maxImageCount > 0 && data.Info.ImageCount > data.Info.Capabilities.maxImageCount) {
            data.Info.ImageCount = data.Info.Capabilities.maxImageCount;
        }

        uint* queueFamilyIndices = stackalloc uint[2]{data.Info.VkGraphicFamilyIndice, data.Info.VkPresentFamilyIndice};

        VkSwapchainCreateInfoKHR createInfo = new();
        createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
        createInfo.pNext = null;
        createInfo.surface = data.Handles.Surface;
        createInfo.minImageCount = imageCount;
        createInfo.imageFormat =  data.Info.VkSurfaceFormat.format;
        createInfo.imageColorSpace = data.Info.VkSurfaceFormat.colorSpace;
        createInfo.imageExtent = data.Info.VkSurfaceArea;
        createInfo.imageArrayLayers = 1;
        createInfo.imageUsage = (uint)VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;
        if (data.Info.VkGraphicFamilyIndice != data.Info.VkPresentFamilyIndice) {
            createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
            createInfo.queueFamilyIndexCount = 2;
            createInfo.pQueueFamilyIndices = queueFamilyIndices;
        } else {
            createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
        }
        createInfo.preTransform = data.Info.Capabilities.currentTransform;
        createInfo.compositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
        createInfo.presentMode = data.Info.VkPresentMode;
        createInfo.clipped = VK.VK_TRUE;
        createInfo.oldSwapchain = VkSwapchainKHR.Null;

        fixed (VkSwapchainKHR* swapchainPtr = &data.Handles.SwapChain)
        {
            func.vkCreateSwapchainKHR(data.Handles.Device, &createInfo, null, swapchainPtr).Check("failed to create swap chain!");
        }

        Log.Info($"Create SwapChain {data.Handles.SwapChain}\nMode : {data.Info.VkPresentMode}\nSize :[{data.Info.VkSurfaceArea.width},{data.Info.VkSurfaceArea.height}] ");

        // SWWAP CHAIN IMAGES  ----------------------------------------------------------------------

        func.vkGetSwapchainImagesKHR(data.Handles.Device, data.Handles.SwapChain, &imageCount, null);
       
        data.Handles.Images = new VkImage[imageCount];

        fixed (VkImage* swapchainImagesPtr = data.Handles.Images){
            func.vkGetSwapchainImagesKHR(data.Handles.Device,data.Handles.SwapChain, &imageCount, swapchainImagesPtr).Check("vkGetSwapchainImagesKHR");
        }
        
        Log.Info($"Create {data.Handles.Images.Length} SwapChainImages ");
        data.Info.ImageCount = imageCount  ;
    }

    public static unsafe void CreateImageViews(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data )
    {
        uint size  = data.Info.ImageCount;
        data.Handles.SwapChainImageViews = new VkImageView[size ];

        for (uint i = 0; i < size; i++)
        {
            VkImageViewCreateInfo imageViewCreateInfo= new();
            imageViewCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO; 
            imageViewCreateInfo.image = data.Handles.Images[i];
            imageViewCreateInfo.viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;
            imageViewCreateInfo.format = data.Info.VkFormat;
            imageViewCreateInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            imageViewCreateInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            imageViewCreateInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            imageViewCreateInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
            imageViewCreateInfo.subresourceRange.aspectMask = (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
            imageViewCreateInfo.subresourceRange.baseMipLevel = 0;
            imageViewCreateInfo.subresourceRange.levelCount = 1;
            imageViewCreateInfo.subresourceRange.baseArrayLayer = 0;
            imageViewCreateInfo.subresourceRange.layerCount = 1;

            fixed(VkImageView* img =  &data.Handles.SwapChainImageViews[i])
            {
                func.vkCreateImageView(data.Handles.Device, &imageViewCreateInfo, null, img).Check("failed to create image views!");
            }
            Log.Info($"\t -[{i}] {data.Handles.SwapChainImageViews[i]} : {data.Info.VkFormat} other info ...."); 
        }
    }
    
    public static unsafe void DisposeSwapChain(in GraphicDeviceFunctions  func,ref GraphicDeviceData data )
    {
        if (!data.Handles.Device.IsNull && data.Handles.SwapChainImageViews != null) 
        {
            Log.Info($"Release Swap chain Images View [{data.Handles.SwapChainImageViews .Length}]");
            foreach (var imageView in data.Handles.SwapChainImageViews)
            {
                func.vkDestroyImageView(data.Handles.Device, imageView, null); 
            }
        }

        if ( !data.Handles.Device.IsNull && !data.Handles.SwapChain.IsNull )
        {
            Log.Info($"Release Swapchain Instancew [{data.Handles.SwapChain}]");
            func.vkDestroySwapchainKHR(data.Handles.Device, data.Handles.SwapChain, null);
        }
    }

    #region FrameBuffer

    public static unsafe void CreateFramebuffers( ref GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
        int size= data.Handles.SwapChainImageViews.Length;
        data.Handles.Framebuffers = new VkFramebuffer[size];
    
        for (int i = 0; i < size; i++)
        {
            VkImageView[] attachments = new[]{ data.Handles.SwapChainImageViews[i] , data.Info.DepthImageView };

            VkFramebufferCreateInfo framebufferInfo = new();
            framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
            framebufferInfo.renderPass = VkRenderPass.Null; //data.Handles.RenderPass;
            framebufferInfo.attachmentCount = (uint)attachments.Length ;
            fixed( VkImageView* attachmentPtr =&attachments[0] ) 
            {
                framebufferInfo.pAttachments = attachmentPtr; 
            }
            framebufferInfo.width = data.Info.VkSurfaceArea.width;
            framebufferInfo.height = data.Info.VkSurfaceArea.height;
            framebufferInfo.layers = 1;

            fixed( VkFramebuffer* frame = &data.Handles.Framebuffers[i]) 
            {
                func.vkCreateFramebuffer(data.Handles.Device, &framebufferInfo, null, frame).Check("failed to create framebuffer!"); 
            }
            Log.Info($"-{i} Create FrameBuffer {data.Handles.Framebuffers[i] }");
        }
    }

    public unsafe static void DisposeFrameBuffer(in GraphicDeviceFunctions  func,ref GraphicDeviceData data  )
    {
        if (data.Handles.Framebuffers != null)
        {
            for(int i=0 ; i < data.Handles.Framebuffers.Length ; i++) 
            {
                if( !data.Handles.Framebuffers[i].IsNull)
                {
                    Log.Info($"- {i} Dispose FrameBuffer {data.Handles.Framebuffers[i] }");
                    func.vkDestroyFramebuffer(data.Handles.Device, data.Handles.Framebuffers[i], null); 
                }  
            }
        }
    }

    #endregion

    #region DEpth Buffering 

    public unsafe static void CreateDepthResources(ref GraphicDeviceFunctions func, ref GraphicDeviceData data)
    {
        VkFormat depthFormat = FindDepthFormat(func , data.Handles.PhysicalDevice);

        CreateImage(ref func , ref data, ref data.Info.DepthImage , ref data.Info.DepthImageMemory, (uint)data.Info.VkSurfaceArea.width, (uint)data.Info.VkSurfaceArea.height, 
            depthFormat, 
            VkImageTiling.VK_IMAGE_TILING_OPTIMAL, 
            VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT   );
        
        VkImageViewCreateInfo viewInfo = new();
        viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        viewInfo.image = data.Info.DepthImage;
        viewInfo.viewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        viewInfo.format = depthFormat;
        viewInfo.subresourceRange.aspectMask =  (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT;
        viewInfo.subresourceRange.baseMipLevel = 0;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.baseArrayLayer = 0;
        viewInfo.subresourceRange.layerCount = 1;

        fixed (VkImageView* imageView = &data.Info.DepthImageView )
        {
            func.vkCreateImageView(data.Handles.Device, &viewInfo, null, imageView).Check("failed to create image view!");
        }
        Log.Info($"Create Depth Resources: {data.Info.DepthImageView}");
    }
    
    public unsafe static void DisposeDepthResources(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if ( data.Info.DepthImageView != VkImageView.Null )
        {
            Log.Info($"Destroy Depth Image : {data.Info.DepthImageView}");
            func.vkDestroyImageView(data.Handles.Device,data.Info.DepthImageView, null);
        }
        if(data.Info.DepthImage != VkImage.Null )
        {
            Log.Info($"Destroy Depth Image : {data.Info.DepthImage}");
            func.vkDestroyImage(data.Handles.Device, data.Info.DepthImage, null);
        }
        if ( data.Info.DepthImageMemory != VkDeviceMemory.Null)
        {
            Log.Info($"Destroye Depth Image memory : {data.Info.DepthImageMemory}");
            func.vkFreeMemory(data.Handles.Device, data.Info.DepthImageMemory, null);
        }
    }
    
    private static unsafe  VkFormat FindSupportedFormat(
        in GraphicDeviceFunctions func, 
        in VkPhysicalDevice physicalDevice,  
        VkFormat[] candidates,/*valid Formats*/ 
        VkImageTiling tiling, 
        VkFormatFeatureFlagBits features) 
    {

        foreach ( VkFormat format in candidates)
        {
        //Get PhysicalFormatProperties --------------------------------------------------------
        VkFormatProperties formatProperties;
        func.vkGetPhysicalDeviceFormatProperties(physicalDevice,format, &formatProperties);

            if (tiling == VkImageTiling.VK_IMAGE_TILING_LINEAR && (formatProperties.linearTilingFeatures & (uint)features) == (uint)features) 
            {
                Log.Info($"Depth Format : { format.ToString()}");
                return format;
            } 
            else if (tiling == VkImageTiling.VK_IMAGE_TILING_OPTIMAL && (formatProperties.optimalTilingFeatures & (uint)features) == (uint)features) 
            {
                Log.Info($"Depth Format : { format.ToString() }");
                return format;
            }
        }
        return VkFormat.VK_FORMAT_A8B8G8R8_SRGB_PACK32;
        //  throw std::runtime_error("failed to find supported format!");
    }
    
    private static unsafe VkFormat FindDepthFormat( in GraphicDeviceFunctions func, in VkPhysicalDevice physicalDevice ) 
    {
        return FindSupportedFormat(func , physicalDevice,
            new VkFormat[] 
            {
                VkFormat.VK_FORMAT_D32_SFLOAT, 
                VkFormat.VK_FORMAT_D32_SFLOAT_S8_UINT, 
                VkFormat.VK_FORMAT_D24_UNORM_S8_UINT,
                VkFormat.VK_FORMAT_D16_UNORM_S8_UINT,
                VkFormat.VK_FORMAT_D16_UNORM
            },
            VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
            VkFormatFeatureFlagBits.VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT
        );
    }

    #endregion

}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Memories
{
    
    public unsafe static uint FindMemoryType(ref VulkanFunctions func,ref GraphicDeviceDatas data , uint memoryTypeBits, VkMemoryPropertyFlagBits properties)
    {
        uint count = data.Device_MemoryProperties.memoryTypeCount;
        for (uint i = 0; i < count; i++)
        {
            if ( (memoryTypeBits & 1) == 1 && (data.Device_MemoryProperties.memoryTypes[(int)i].propertyFlags & (uint)properties) == (uint)properties)
            {
                Log.Info($"Memory INdex choosen : {i}");
                return i;
            }
            memoryTypeBits >>= 1;
        }

        return uint.MaxValue;
    }
    
    private unsafe static void CreateImage(
        ref GraphicDeviceFunctions func,
        ref VkImage image,ref VkDeviceMemory imageMemory,  uint width, uint height, VkFormat format, VkImageTiling tiling,
        VkImageUsageFlagBits usage, VkMemoryPropertyFlagBits properties) 
    {
        // VkImageCreateInfo imageInfo = new();
        // imageInfo.sType =VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
        // imageInfo.imageType =VkImageType. VK_IMAGE_TYPE_2D;
        
        // imageInfo.extent.width = width;
        // imageInfo.extent.height = height;
        // imageInfo.extent.depth = 1;
        // imageInfo.mipLevels = 1;
        // imageInfo.arrayLayers = 1;
        // imageInfo.format = format;
        // imageInfo.tiling = tiling;
        // imageInfo.initialLayout =VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
        // imageInfo.usage = (uint)usage;
        // imageInfo.samples =VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
        // imageInfo.sharingMode = VkSharingMode. VK_SHARING_MODE_EXCLUSIVE;
        // imageInfo.pNext = null;

        // fixed( VkImage* img = &image)
        // {
        //     func.vkCreateImage(data.Handles.Device, &imageInfo, null,img).Check("failed to create image!");
        // }
        
        // VkMemoryRequirements memRequirements;
        // func.vkGetImageMemoryRequirements(data.Handles.Device, image, &memRequirements);

        // VkMemoryAllocateInfo allocInfo = new();
        // allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        // allocInfo.allocationSize = memRequirements.size;
        // allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data, memRequirements.memoryTypeBits, properties);

        // fixed (VkDeviceMemory* imgMem = &imageMemory  )
        // {
        //     func.vkAllocateMemory(data.Handles.Device, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        // }

        // func.vkBindImageMemory( data.Handles.Device, image,imageMemory, 0).Check("Bind Image Memory");
    }
    // create ImagesView 

    //Dispose

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

     public unsafe static void CreateImageView(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data, out VkImageView imageViewResult)
    {
        VkImageViewCreateInfo viewInfo = new();
        viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        viewInfo.image = data.Info.TextureImage;
        viewInfo.viewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        viewInfo.format = VkFormat. VK_FORMAT_R8G8B8A8_SRGB;
        viewInfo.subresourceRange.aspectMask =  (uint)VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT;
        viewInfo.subresourceRange.baseMipLevel = 0;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.baseArrayLayer = 0;
        viewInfo.subresourceRange.layerCount = 1;

        fixed (VkImageView* imageView = &data.Info.TextureImageView )
        {
            func.vkCreateImageView(data.Handles.Device, &viewInfo, null, imageView).Check("failed to create image view!");
        }
        Log.Info($"Create Texture Image View {data.Info.TextureImageView}");
        imageViewResult = VkImageView.Null;
    }

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

}
