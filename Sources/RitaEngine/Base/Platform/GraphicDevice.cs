namespace RitaEngine.Base.Platform;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Math.Color;
using RitaEngine.Base.Platform.Config;
using RitaEngine.Base.Platform.Native.Vulkan;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Strings;


/// <summary>
/// 
/// </summary>
[SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack =  BaseHelper.FORCE_ALIGNEMENT)]
public struct GraphicDevice : IEquatable<GraphicDevice>
{
    private GraphicDeviceFunction _device;
    private GraphicDeviceLoaderFunction _loader;
    private GraphicInstanceFunction _instance;

    private GraphicDeviceData _data; // inside => Instance Device Render Infos 

    public GraphicRenderConfig Render =new();
    public  GraphicDeviceConfig Config = new();

    private nint _address = nint.Zero;

    public GraphicDevice() {   _address= AddressOfPtrThis(); }

    public unsafe void Init(Window win )
    {
        _data = new();
        _data.vulkan = Libraries.Load( Config.VulkanDllName);
        _loader = new( Libraries.GetUnsafeSymbol, _data.vulkan);

        _data.Infos.GameName  = win.GetWindowName();
        _data.Infos.Handle = win.GetWindowHandle();
        _data.Infos.HInstance = win.GetWindowHInstance();
        _data.Infos.Width = win.GetWindowWidth();
        _data.Infos.Height = win.GetWindowheight();
        _data.Infos.EnableDebug = Config.EnableDebugMode;

        CreateInstance(ref _loader,ref _data, out VkDebugUtilsMessengerCreateInfoEXT debugCreateInfo);

        _instance = new( GraphicDeviceLoaderFunction.vkGetInstanceProcAddr , _data.VkInstance);
                
        SetupDebugMessenger(ref _instance, ref _data,ref debugCreateInfo);
        
        
        CreateSurface(ref _instance,ref _data);
        
        SelectPhysicalDevice(ref _instance ,ref _data );
        CreateLogicalDevice(ref _instance ,ref _data  );

        _device = new( GraphicDeviceLoaderFunction.vkGetDeviceProcAddr ,_data.VkDevice);

//RENDER
        CreateQueues( ref _device , ref _data);


        CreateSwapChain( ref _device , ref _data);
        CreateImageViews( ref _device , ref _data );
       
        _data.MAX_FRAMES_IN_FLIGHT = Render.MAX_FRAMES_IN_FLIGHT;// only need this config

        CreateCommandPool(ref _device , ref _data);

        CreateSyncObjects(ref _device , ref _data);
    }

    public unsafe void Release()
    {
        Log.Info("Dispose Graphic Win32");

        DisposeFrameBuffer(ref _device, ref _data);
        DisposeSwapChain(ref _device, ref _data);

        DisposeBuildRender(ref _device, ref _data);
        
        Pause(ref _device,ref _data);

        DisposeSyncObjects(ref _device ,ref _data);

        DisposeQueues(ref _device ,ref _data);

        DisposeLogicalDevice(ref _instance,ref _data);
        //  na pas de disposeDisposePhysicalDevice(ref _data);
        DisposeSurface(ref _instance, ref _data);
        DisposeDebug(ref _instance,ref _data);
        DisposeInstance(ref _loader,ref _data);
        Libraries.Unload(_data.vulkan);
    }

    

    public void Pause()=> Pause( ref _device , ref _data);

    public void BuildRender() => BuildRender(ref _device, ref _data, ref Render);

    public void Draw() => DrawPipeline(ref _device, ref _data);
   
    #region private PRUPOSE

    private unsafe static void Pause(ref GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        if ( !data.VkDevice.IsNull){
            func.vkDeviceWaitIdle(data.VkDevice).Check($"WAIT IDLE VkDevice : {data.VkDevice}");
        }
    }

    private unsafe static void BuildRender(ref GraphicDeviceFunction func,ref GraphicDeviceData data,ref GraphicRenderConfig pipeline)
    {
        CreateRenderPass(ref func,ref data, ref pipeline);
        CreateFramebuffers(ref func,ref data);

        data.RenderAreaOffset.x =0;
        data.RenderAreaOffset.y =0;       
        data.ClearColor = new(ColorHelper.ToRGBA( (uint)pipeline.BackColorARGB),00000.0f,0);

        CreatePipeline(ref func, ref data,ref pipeline );
        CreateCommandPool(ref func,ref data);
        CreateCommandBuffer(ref func,ref data,ref pipeline);

    }
    
    private unsafe static void DisposeBuildRender(ref GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        Pause(ref func,ref data);
        
        // DisposeFrameBuffer(ref func,ref data);
        DisposePipeline(ref func,ref data);
        DisposeRenderPass(ref func,ref data);
        DisposeCommandPool(ref func,ref data);
    }

    private unsafe static void ReCreateSwapChain(ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        if ( data.VkDevice == VkDevice.Null)return ;

        // RECT rect = new();
        //  GetClientRect( data.WindowHandle,&rect);
        // uint width = (uint)rect.Right;
        // uint height = (uint)rect.Bottom;
        // data.VkSurfaceArea.width = width;
        // data.VkSurfaceArea.height = height;
        //need uint width, uint height
        Pause(ref func,ref data);

        DisposeSwapChain(ref func,ref data);
        DisposeFrameBuffer(ref func,ref data);
       
        CreateSwapChain(ref func,ref data);
        CreateImageViews(ref func, ref data);
        CreateFramebuffers(ref func,ref data);
    }

    private static string VersionToString( uint version ) => $"{VK.VK_VERSION_MAJOR(version)}.{VK.VK_VERSION_MINOR(version)}.{VK.VK_VERSION_PATCH(version)} ";

    
    #endregion

    #region Instance

    private unsafe static void CreateInstance(ref GraphicDeviceLoaderFunction func,ref GraphicDeviceData data , out VkDebugUtilsMessengerCreateInfoEXT  debugCreateInfo)
    {
        //Enumerate instance layer         
        uint layerCount = 0;

        func.vkEnumerateInstanceLayerProperties(&layerCount, null).Check("Enumerate instance Layer count");
        Guard.ThrowWhenConditionIsTrue( layerCount ==0 );

        VkLayerProperties* layerProperties = stackalloc VkLayerProperties[(int)layerCount];// ReadOnlySpan<VkLayerProperties> pp = stackalloc VkLayerProperties[(int)count];
        func.vkEnumerateInstanceLayerProperties(&layerCount, layerProperties).Check("Enumerate instance Layer list");

        data.Infos.ValidationLayers = new  string[ layerCount ];
        for (int i = 0; i < layerCount; i++) {
            var length = Strings.StrHelper.Strlen( layerProperties[i].layerName );
            data.Infos.ValidationLayers[i] = Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );// new string(layerProperties[i].layerName); //Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );
        }
        // infos.ValidationLayers[layerCount] =  config.Instance.ValidationLayerExtensions[0] ;
        //--------------------------------------------------------------------
        uint ver=0;
        func.vkEnumerateInstanceVersion(&ver).Check("Enumerate Instance Version");
        // infos.VulkanVersion = ver;
        //--------------------------------------------------------------------
        uint extCount = 0;
        func.vkEnumerateInstanceExtensionProperties(null, &extCount, null).Check( "Enumerate Extension Name Count");
        Guard.ThrowWhenConditionIsTrue( extCount == 0);

        VkExtensionProperties* props = stackalloc VkExtensionProperties[(int)extCount];
        func.vkEnumerateInstanceExtensionProperties(null, &extCount, props).Check( "Enumerate Extension Name List");

        data.Infos.InstanceExtensions = new string[extCount ];
        for (int i = 0; i < extCount; i++){
            var length = Strings.StrHelper.Strlen( props[i].extensionName);
            data.Infos.InstanceExtensions[i] =Encoding.UTF8.GetString(  props[i].extensionName, (int) length );// new string( props[i].extensionName) ;//Encoding.UTF8.GetString(  props[i].extensionName, (int) length );
        }
        //--------------------CREATE INFO et POPULATE DEBUG------------------------------------------------
        debugCreateInfo = new();
            debugCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT;
            debugCreateInfo.pNext = null;
            debugCreateInfo.messageSeverity = (uint)( VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT );
            debugCreateInfo.messageType = (uint) (VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT);
            debugCreateInfo.pfnUserCallback = &DebugMessengerCallback;
            debugCreateInfo.pUserData = null;
        //--------------------CREATE INFO et POPULATE DEBUG------------------------------------------------
        var EngineName = Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);// RitaEngine.Base.BaseHelper.ENGINE_NAME.ToCharArray();//Encoding.UTF8.GetBytes(RitaEngine.Device.RitaEngineDeviceConfig.EngineName);
        // var GameName = Encoding.UTF8.GetBytes(data.Infos.GameName);// data.Infos.GameName;//
        VkApplicationInfo appInfo=new();
            appInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO;
            appInfo.apiVersion = VK.VK_API_VERSION_1_3; //ver;
            appInfo.applicationVersion = VK.VK_MAKE_VERSION(1,0,0);
            appInfo.engineVersion= VK.VK_MAKE_VERSION(1,0,0);
            appInfo.pNext =null;

        fixed(byte* ptr = &EngineName[0] ,  app = &data.Infos.GameName[0] ) {
            appInfo.pApplicationName =app;
            appInfo.pEngineName = ptr;
        }

        using var extNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.Infos.InstanceExtensions) ;
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.Infos.ValidationLayers);

        VkInstanceCreateInfo instanceCreateInfo = new();
            instanceCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
            instanceCreateInfo.pApplicationInfo =&appInfo;
            fixed(VkDebugUtilsMessengerCreateInfoEXT* dbg =  &debugCreateInfo)
            {
                instanceCreateInfo.pNext= !data.Infos.EnableDebug ? null :  dbg;
            }
           
            instanceCreateInfo.ppEnabledExtensionNames = extNames;
            instanceCreateInfo.enabledExtensionCount =extNames.Count;
            
            if ( data.Infos.EnableDebug)            {

                instanceCreateInfo.enabledLayerCount = layerNames.Count;
                instanceCreateInfo.ppEnabledLayerNames = layerNames;
            }
            else {
                instanceCreateInfo.enabledLayerCount =0;
                instanceCreateInfo.ppEnabledLayerNames =null;
            }
            instanceCreateInfo.flags =  (uint)VkInstanceCreateFlagBits.VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR;       

        fixed( VkInstance* instance = &data.VkInstance) {
            func.vkCreateInstance(&instanceCreateInfo, null, instance).Check("failed to create instance!");
        };
        
        // infos.VkInstanceAdress = data.VkInstance.ToString();

        VK.VK_KHR_swapchain=true;// //Special dont understand pour chage swapchain car nvidia n'a pas l'extension presente
        VkHelper.ValidateExtensionsForLoad(ref data.Infos.InstanceExtensions,ver );
        data.Infos.VkVersion = ver;
    }

    private unsafe static void DisposeInstance(ref GraphicDeviceLoaderFunction func,ref GraphicDeviceData data)
    {
        Log.Info($"Dispose Vulkan Instance");

        if ( !data.VkInstance.IsNull){
            Log.Info($"Release Instance [{data.VkInstance}]");
            func.vkDestroyInstance(data.VkInstance, null);
            data.VkInstance = VkInstance.Null;
        }
    }
  
    #endregion

    #region Debug

    private static unsafe void SetupDebugMessenger(ref GraphicInstanceFunction func,ref GraphicDeviceData data ,ref VkDebugUtilsMessengerCreateInfoEXT debugCreateInfo)
    {
        if ( !data.Infos.EnableDebug  )return ;
        
        fixed (VkDebugUtilsMessengerCreateInfoEXT* dbgInfo =  &debugCreateInfo ){
            fixed(VkDebugUtilsMessengerEXT* dbg = &data.DebugMessenger ){
            func.vkCreateDebugUtilsMessengerEXT(data.VkInstance, dbgInfo, null, dbg).Check("failed to set up debug messenger!");
            }
        }
    }

    private static unsafe void DisposeDebug(ref GraphicInstanceFunction func,ref GraphicDeviceData data)
    {
        Log.Info($"Dispose DEBUG");
        if (!data.VkInstance.IsNull && !data.DebugMessenger.IsNull){
            Log.Info($"Release DebugMessenger [{data.DebugMessenger}]");
            func.vkDestroyDebugUtilsMessengerEXT(data.VkInstance,data.DebugMessenger,null);
        } 
    }

    [UnmanagedCallersOnly] 
    private unsafe static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, 
        uint/*VkDebugUtilsMessageTypeFlagsEXT*/ messageTypes,
        VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {

        string message= System.Text.Encoding.UTF8.GetString(pCallbackData->pMessage,(int) Strings.StrHelper.Strlen(pCallbackData->pMessage) );  //new string(pCallbackData->pMessage); //
        // string validation = (messageTypes == (uint)Vulkan.VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT)? "Validation : " : string.Empty;

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

    #region WSI  Window System Integration
    
    private static unsafe void CreateSurface( ref GraphicInstanceFunction func,ref GraphicDeviceData data )
    {
        #if WIN64
        VkWin32SurfaceCreateInfoKHR sci = default ;
            sci .hinstance = data.Infos.HInstance;
            sci .hwnd = data.Infos.Handle;
            sci .pNext = null;
            sci .flags = 0;
            sci .sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;

        fixed ( VkSurfaceKHR* surf = &data.VkSurface){
            func.vkCreateWin32SurfaceKHR(data.VkInstance,&sci,null, surf).Check("Create Surface");
        }
        // data.WindowHandle = infos.Handle;
        #endif
    }
    
    private static unsafe void DisposeSurface(ref GraphicInstanceFunction func,ref GraphicDeviceData data)
    {
        Log.Info($"Dispose Surface");
        if ( !data.VkInstance.IsNull && !data.VkSurface.IsNull){
            Log.Info($"Release Surface [{data.VkSurface}]");
            func.vkDestroySurfaceKHR(data.VkInstance,data.VkSurface,null);
        }
    }

    #endregion
    
    #region DEvice

    private static unsafe void SelectPhysicalDevice( ref GraphicInstanceFunction func,ref GraphicDeviceData data )
    {
        //Find Queue Family and QueryChainsupport ????
        // (data.VkGraphicFamilyIndice,data.VkPresentFamilyIndice) = FindQueueFamilies(data.VkPhysicalDevice,data.VkSurface);

        uint deviceCount = 0;
        func.vkEnumeratePhysicalDevices(data.VkInstance, &deviceCount, null).Check("EnumeratePhysicalDevices Count");

        Guard.ThrowWhenConditionIsTrue(deviceCount == 0,"Vulkan: Failed to find GPUs with Vulkan support");

        VkPhysicalDevice* devices = stackalloc VkPhysicalDevice[(int)deviceCount];
        func.vkEnumeratePhysicalDevices(data.VkInstance, &deviceCount, devices).Check("EnumeratePhysicalDevices List");

        Log.Info($"Find {deviceCount} Physical Device");
        for (int i = 0; i < (int)deviceCount; i++)
        {
            VkPhysicalDevice physicalDevice = devices[i];

            if ( IsDeviceSuitable(ref func,physicalDevice, data.VkSurface) )
            {
                data.VkPhysicalDevice = physicalDevice;
                break;
            }
        }

        SwapChainSupportDetails swap = QuerySwapChainSupport( ref func , data.VkPhysicalDevice , data.VkSurface  );
        data.Infos.Capabilities = swap.Capabilities;
        data.Infos.PresentModes = swap.PresentModes.ToArray();
        data.Infos.Formats = swap.Formats.ToArray(); 

        Guard.ThrowWhenConditionIsTrue( data.VkPhysicalDevice.IsNull , "Physical device is null ");
        // //FOR INFO .... bool in GraphicDeviceSettings.WithInfo       if ( !settings.Instance.GetDeviceInfo ) return;
        Log.Info($"Create Physical device {data.VkPhysicalDevice}");
    }
    
    private static unsafe void CreateLogicalDevice(ref GraphicInstanceFunction func,ref GraphicDeviceData data )
    {
        var (graphicsFamily, presentFamily) = FindQueueFamilies(ref func,data.VkPhysicalDevice, data.VkSurface);
        data.VkGraphicFamilyIndice = graphicsFamily;
        data.VkPresentFamilyIndice = presentFamily;
        // QUEUE
        VkDeviceQueueCreateInfo* queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[2];

       uint[] uniqueQueueFamilies = new uint[]{
            graphicsFamily,
            presentFamily
        };

        float queuePriority = 1.0f;
        uint queueCount = 0;
        foreach (uint queueFamily in uniqueQueueFamilies)
        {
            queueCreateInfos[queueCount++] = new VkDeviceQueueCreateInfo {
                sType = VkStructureType. VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = queueFamily,
                queueCount = 1,
                pQueuePriorities = &queuePriority
            };
        }

        // EXTENSIONS
        uint propertyCount = 0;
        func.vkEnumerateDeviceExtensionProperties(data.VkPhysicalDevice, null, &propertyCount, null).Check();

        VkExtensionProperties* properties = stackalloc VkExtensionProperties[(int)propertyCount];  
        func.vkEnumerateDeviceExtensionProperties(data.VkPhysicalDevice, null, &propertyCount, properties).Check();

        data.Infos.DeviceExtensions = new string[propertyCount + 1];

        for (int i = 0; i < propertyCount; i++){
            var length =  Strings.StrHelper.Strlen( properties[i].extensionName);
            data.Infos.DeviceExtensions[i] = Encoding.UTF8.GetString( properties[i].extensionName, (int) length ); //new string(properties[i].extensionName); //
        }
        data.Infos.DeviceExtensions[propertyCount] = VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME ;
        

        fixed (VkPhysicalDeviceProperties* phd =   &data.Infos.PhysicalDeviceProperties){
            func.vkGetPhysicalDeviceProperties(data.VkPhysicalDevice ,phd );
        }
        data.Infos.Limits = data.Infos.PhysicalDeviceProperties.limits;
        
        fixed ( VkPhysicalDeviceFeatures* features = &data.Infos.Features)
        {
            func.vkGetPhysicalDeviceFeatures(data.VkPhysicalDevice,features );
        }
            // DEVICE
            using var deviceExtensions = new StrArrayUnsafe(ref data.Infos.DeviceExtensions);
            VkDeviceCreateInfo createInfo = new(){
                sType =  VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
                queueCreateInfoCount = (uint)queueCount,
                pQueueCreateInfos = queueCreateInfos,
                pEnabledFeatures = null,
                enabledExtensionCount = (uint)deviceExtensions.Count,
                ppEnabledExtensionNames = deviceExtensions,
                pNext = null ,
            };
            using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.Infos.ValidationLayers);
            if ( data.Infos.EnableDebug)
            {
                
                createInfo.enabledLayerCount = layerNames.Count ;
                createInfo.ppEnabledLayerNames = layerNames ;
            }
            else
            {
                createInfo.enabledLayerCount = (uint)0  ;
                createInfo.ppEnabledLayerNames = null ;
            }

            fixed(VkDevice* device = &data.VkDevice){
                func.vkCreateDevice(data.VkPhysicalDevice, &createInfo, null, device).Check("Error creation vkDevice");
            }
       
       VkHelper.ValidateExtensionsForLoad(ref data.Infos.DeviceExtensions,data.Infos.VkVersion );

       Log.Info($"Create Device :{data.VkDevice}");

   
    }

    private static unsafe void DisposeLogicalDevice(ref GraphicInstanceFunction func,ref GraphicDeviceData data )
    {
        Log.Info("Dispose Logical Device ");
        if ( !data.VkDevice.IsNull){
            Log.Info($"Release Physical Device [{data.VkPhysicalDevice}]");
            func.vkDestroyDevice(data.VkDevice, null);
        }  
    }

    private unsafe static (uint graphicsFamily, uint presentFamily) FindQueueFamilies(ref GraphicInstanceFunction func,VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        uint queueFamilyPropertyCount = 0;
        func.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyPropertyCount, null);

        ReadOnlySpan<VkQueueFamilyProperties> queueFamilyProperties = new VkQueueFamilyProperties[queueFamilyPropertyCount];
        
        fixed (VkQueueFamilyProperties* queueFamilyPropertiesPtr = queueFamilyProperties){
            func.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyPropertyCount, queueFamilyPropertiesPtr);
        }

        uint graphicsFamily = VK.VK_QUEUE_FAMILY_IGNORED;
        uint presentFamily = VK.VK_QUEUE_FAMILY_IGNORED;
        uint i = 0;

        foreach (VkQueueFamilyProperties queueFamily in queueFamilyProperties)
        {
            
            if ((queueFamily.queueFlags & VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT) != 0)
            {
                graphicsFamily = i;
            }
            uint presentSupport =0;
            func.vkGetPhysicalDeviceSurfaceSupportKHR(device, i, surface, &presentSupport);
            if (presentSupport==VK.VK_TRUE)
            {
                presentFamily = i;
            }
            Log.Info($" Flag : {  queueFamily.queueFlags  }  count = {queueFamily.queueCount }  granularity [{queueFamily.minImageTransferGranularity.width};{queueFamily.minImageTransferGranularity.height} ]  Present Support count = {presentSupport } "); 
            if (graphicsFamily != VK.VK_QUEUE_FAMILY_IGNORED
                && presentFamily != VK.VK_QUEUE_FAMILY_IGNORED)
            {
                break;
            }
            i++;
        }

        return (graphicsFamily, presentFamily);
        
    }

    private static bool IsDeviceSuitable(ref GraphicInstanceFunction func,VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
    {
        var (graphicsFamily, presentFamily) = FindQueueFamilies(ref func,physicalDevice, surface);
        if (  graphicsFamily == VK.VK_QUEUE_FAMILY_IGNORED
            || presentFamily == VK.VK_QUEUE_FAMILY_IGNORED)
        {
            return false;
        }

        SwapChainSupportDetails swapChainSupport = QuerySwapChainSupport(ref func,physicalDevice, surface);
        return !swapChainSupport.Formats.IsEmpty && !swapChainSupport.PresentModes.IsEmpty;
    }
    
    private ref struct SwapChainSupportDetails
    {
        public VkSurfaceCapabilitiesKHR Capabilities;
        public ReadOnlySpan<VkSurfaceFormatKHR> Formats;
        public ReadOnlySpan<VkPresentModeKHR> PresentModes;
    }

    private  unsafe static SwapChainSupportDetails QuerySwapChainSupport(ref GraphicInstanceFunction func,VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
    {
        SwapChainSupportDetails details = new();

        // Capabilities
        func.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, &details.Capabilities ).Check("vkGetPhysicalDeviceSurfaceCapabilitiesKHR");
        Log.Info($"Supported Image  {details.Capabilities.supportedUsageFlags} ");

        // Surface Format 
        uint surfaceFormatCount = 0;
        func.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &surfaceFormatCount, null).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");

        ReadOnlySpan<VkSurfaceFormatKHR> surfaceFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
        fixed (VkSurfaceFormatKHR* surfaceFormatsPtr = surfaceFormats)		{
            func.vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &surfaceFormatCount, surfaceFormatsPtr).Check("vkGetPhysicalDeviceSurfaceFormatsKHR");
        }
        

        //NEW NEW nEW  see : https://registry.khronos.org/vulkan/specs/1.2-extensions/html/vkspec.html#VkPresentModeKHR

        if (VK.VK_KHR_get_surface_capabilities2 )
        {
              VkPhysicalDeviceSurfaceInfo2KHR surfaceInfo2 = new();
                surfaceInfo2.pNext = null;
                surfaceInfo2.sType = VkStructureType.VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_SURFACE_INFO_2_KHR;
                surfaceInfo2.surface = surface;

            uint surfaceFormat2Count = 0;
           func.vkGetPhysicalDeviceSurfaceFormats2KHR(physicalDevice,&surfaceInfo2,&surfaceFormat2Count,null ).Check("vkGetPhysicalDeviceSurfaceFormats2KHR");

            
            ReadOnlySpan<VkSurfaceFormat2KHR> surfaceFormats2 = new VkSurfaceFormat2KHR[surfaceFormat2Count];
            fixed (VkSurfaceFormat2KHR* surfaceFormats2Ptr = surfaceFormats2)		{   
            func.vkGetPhysicalDeviceSurfaceFormats2KHR(physicalDevice,&surfaceInfo2,&surfaceFormat2Count,surfaceFormats2Ptr ).Check("vkGetPhysicalDeviceSurfaceFormats2KHR");
             }
        }
        details.Formats = surfaceFormats;
        // TODO : PRESENT MODE  ENABLED VSYNC ?????
        uint presentModeCount = 0;
        func.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, null).Check("vkGetPhysicalDeviceSurfacePresentModesKHR Count");

        ReadOnlySpan<VkPresentModeKHR> presentModes = new VkPresentModeKHR[presentModeCount];
        fixed (VkPresentModeKHR* presentModesPtr = presentModes)	{
            func.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, presentModesPtr).Check("vkGetPhysicalDeviceSurfacePresentModesKHR List");
        }
        details.PresentModes = presentModes;
        return details;
    }

#endregion

    #region QUEUE
    private static unsafe void CreateQueues(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
    // QUEUE
            fixed(VkQueue* gq =&data.VkGraphicQueue){
                func.vkGetDeviceQueue(data.VkDevice, data.VkGraphicFamilyIndice, 0,gq);
            }
            fixed(VkQueue* pq = &data.VkPresentQueue){
                func.vkGetDeviceQueue(data.VkDevice, data.VkPresentFamilyIndice, 0, pq); 
            }

            Log.Info($"Graphic Queues : indice :{ data.VkGraphicFamilyIndice} Adr[{data.VkGraphicQueue}]\nPresent : indice :{data.VkPresentFamilyIndice} Adr[{data.VkPresentQueue}]");
    }

     private static unsafe void DisposeQueues(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        Log.Info("Dispose Queues ");
        if ( !data.VkDevice.IsNull){
            
        }  
    }
    #endregion
    
    #region SWAP CHAIN
    private static uint ClampUInt(uint value, uint min, uint max) =>value < min ? min : value > max ? max : value;
    
    private static unsafe void CreateSwapChain(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        VkSurfaceFormatKHR surfaceFormat = ChooseSwapSurfaceFormat(data.Infos.Formats);
        VkPresentModeKHR presentMode = ChooseSwapPresentMode(data.Infos.PresentModes);

        // Choose Swap Extend
        //need surface ..........

        data.VkSurfaceArea = new VkExtent2D(){
            width = ClampUInt( (uint)data.Infos.Width, data.Infos.Capabilities.minImageExtent.width, data.Infos.Capabilities.maxImageExtent.width),
            height = ClampUInt( (uint)data.Infos.Height, data.Infos.Capabilities.minImageExtent.height, data.Infos.Capabilities.maxImageExtent.height),
        };

        uint imageCount = data.Infos.Capabilities.minImageCount + 1;
        if (data.Infos.Capabilities.maxImageCount > 0 && imageCount > data.Infos.Capabilities.maxImageCount) {
            imageCount = data.Infos.Capabilities.maxImageCount;
        }

        // (data.VkGraphicFamilyIndice,data.VkPresentFamilyIndice) = FindQueueFamilies(data.VkPhysicalDevice,data.VkSurface);
        uint* queueFamilyIndices = stackalloc uint[2]{data.VkGraphicFamilyIndice, data.VkPresentFamilyIndice};

        VkSwapchainCreateInfoKHR createInfo = default;
            createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
            createInfo.surface = data.VkSurface;
            createInfo.minImageCount = imageCount;
            createInfo.imageFormat = surfaceFormat.format;
            createInfo.imageColorSpace = surfaceFormat.colorSpace;
            createInfo.imageExtent = data.VkSurfaceArea;
            createInfo.imageArrayLayers = 1;
            createInfo.imageUsage = (uint)VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;
            
            if (data.VkGraphicFamilyIndice != data.VkPresentFamilyIndice) {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
                createInfo.queueFamilyIndexCount = 2;
                createInfo.pQueueFamilyIndices = queueFamilyIndices;
            } else {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
            }
            
            createInfo.preTransform = data.Infos.Capabilities.currentTransform;
            createInfo.compositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
            createInfo.presentMode = presentMode;
            createInfo.clipped = VK.VK_TRUE;
            createInfo.oldSwapchain = VkSwapchainKHR.Null;

        fixed (VkSwapchainKHR* swapchainPtr = &data.VkSwapChain){
            func.vkCreateSwapchainKHR(data.VkDevice, &createInfo, null, swapchainPtr).Check("failed to create swap chain!");
        }

        Log.Info($"Create SwapChain {data.VkSwapChain}\nMode : {presentMode}\nSize :[{data.VkSurfaceArea.width},{data.VkSurfaceArea.height}] ");

        // SWWAP CHAIN IMAGES

        func.vkGetSwapchainImagesKHR(data.VkDevice, data.VkSwapChain, &imageCount, null);

        data.VkImages = new VkImage[imageCount];

        fixed (VkImage* swapchainImagesPtr = data.VkImages){
            func.vkGetSwapchainImagesKHR(data.VkDevice,data.VkSwapChain, &imageCount, swapchainImagesPtr).Check("vkGetSwapchainImagesKHR");
        }
        
        Log.Info($"Create {data.VkImages.Length} SwapChainImages ");

        data.VkFormat  = surfaceFormat.format;
   
    }

    private static unsafe void CreateImageViews(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        int size  = data.VkImages.Length;
        data.VkSwapChainImageViews = new VkImageView[size ];

        Log.Info($"Create {size} Image View use Format : {data.VkFormat}");
        for (int i = 0; i < size; i++)
        {
            VkImageViewCreateInfo imageViewCreateInfo= new();//New<VkImageViewCreateInfo>();
                imageViewCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO; //VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
                imageViewCreateInfo.image = data.VkImages[i];
                imageViewCreateInfo.viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;// VK_IMAGE_VIEW_TYPE_2D ;//VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;// VK_IMAGE_VIEW_TYPE_2D;
                imageViewCreateInfo.format = data.VkFormat;

                imageViewCreateInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                imageViewCreateInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                imageViewCreateInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                imageViewCreateInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                
                imageViewCreateInfo.subresourceRange.aspectMask = (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;;
                imageViewCreateInfo.subresourceRange.baseMipLevel = 0;
                imageViewCreateInfo.subresourceRange.levelCount = 1;
                imageViewCreateInfo.subresourceRange.baseArrayLayer = 0;
                imageViewCreateInfo.subresourceRange.layerCount = 1;

            fixed(VkImageView* img =  &data.VkSwapChainImageViews[i]){
                func.vkCreateImageView(data.VkDevice, &imageViewCreateInfo, null, img).Check("failed to create image views!");
            }
            // Log.Info($"\t -[{i}] {data.VkSwapChainImageViews[i]}  :{config.SwapChain.ImageViewType},{config.SwapChain.ImageViewComponentSwizzle},{ config.SwapChain.ImageViewImageAspect}"); 
        }
    }
    
    private static unsafe void DisposeSwapChain(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        Log.Info("Dispose SWAP CHAIN ");
        if (!data.VkDevice.IsNull && data.VkSwapChainImageViews != null) {
            Log.Info($"ReleaseSwap chain Images viewe [{data.VkSwapChainImageViews .Length}]");
            foreach (var imageView in data.VkSwapChainImageViews) {
                func.vkDestroyImageView(data.VkDevice, imageView, null); }
        }

        if ( !data.VkDevice.IsNull && !data.VkSwapChain.IsNull ){
            Log.Info($"Release SwapChain [{data.VkPhysicalDevice}]");
            func.vkDestroySwapchainKHR(data.VkDevice, data.VkSwapChain, null);
        }
    }

    private  static VkSurfaceFormatKHR ChooseSwapSurfaceFormat(ReadOnlySpan<VkSurfaceFormatKHR> availableFormats)
    {
        foreach (VkSurfaceFormatKHR availableFormat in availableFormats)
        {
            if (availableFormat.format == VkFormat.VK_FORMAT_B8G8R8A8_SRGB && availableFormat.colorSpace == VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR )
            {
                return availableFormat;
            }
        }
        return availableFormats[0];
    }

    // private static  VkExtent2D ChooseSwapExtent(in VkSurfaceCapabilitiesKHR capabilities) 
    // {
    //     if (capabilities.currentExtent.width != uint.MaxValue /*std::numeric_limits<uint32_t>::max()*/) {
    //         return capabilities.currentExtent;
    //     } else {
    //         int width=1920, height=720;
    //         // glfwGetFramebufferSize(surface, &width, &height);

    //         VkExtent2D actualExtent =new() {
    //             width = (uint)(width),
    //             height = (uint)(height)
    //         };

    //         // actualExtent.width = std::clamp(actualExtent.width, capabilities.minImageExtent.width, capabilities.maxImageExtent.width);
    //         // actualExtent.height = std::clamp(actualExtent.height, capabilities.minImageExtent.height, capabilities.maxImageExtent.height);

    //         return actualExtent;
    //     }
    // }

    private  static VkPresentModeKHR ChooseSwapPresentMode(ReadOnlySpan<VkPresentModeKHR> availablePresentModes)
    {
        foreach (VkPresentModeKHR availablePresentMode in availablePresentModes)
        {
            if (availablePresentMode == VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR)
            {
                return availablePresentMode;
            }
        }

        return VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
    }

    #endregion

    #region  FrameBuffer

    private static unsafe void CreateFramebuffers( ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        var size= data.VkSwapChainImageViews.Length;
        data.VkFramebuffers = new VkFramebuffer[size] ;// swapChainFramebuffers.resize(swapChainImageViews.size());

        for (int i = 0; i < size; i++)
        {
            VkImageView[] attachments = new[]{ data.VkSwapChainImageViews[i] };

            VkFramebufferCreateInfo framebufferInfo = new();
                framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
                framebufferInfo.renderPass = data.VkRenderPass;
                framebufferInfo.attachmentCount = (uint)attachments.Length ;
                fixed( VkImageView* attachmentPtr =&attachments[0] ) {
                    framebufferInfo.pAttachments = attachmentPtr; 
                }
                framebufferInfo.width = data.VkSurfaceArea.width;
                framebufferInfo.height = data.VkSurfaceArea.height;
                framebufferInfo.layers = 1;

            fixed( VkFramebuffer* frame = &data.VkFramebuffers[i]) {
                func.vkCreateFramebuffer(data.VkDevice, &framebufferInfo, null, frame).Check("failed to create framebuffer!"); 
            }
        }
        Log.Info($"Create {size} FrameBuffer ");
    }

    private unsafe static void DisposeFrameBuffer(ref GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        if (!data.VkDevice.IsNull && data.VkFramebuffers != null)
        {
            foreach (var framebuffer in  data.VkFramebuffers) {
                if( !framebuffer.IsNull)
                    func.vkDestroyFramebuffer(data.VkDevice, framebuffer, null); 
            }
        }
    }

    #endregion
 
    #region COMMAND BUFFERS 

    private static unsafe void CreateCommandPool(ref GraphicDeviceFunction func,ref GraphicDeviceData data  ) 
    {
        
        VkCommandPoolCreateInfo poolInfo = new();
            poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;//VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
            poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;// VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
            poolInfo.queueFamilyIndex =data.VkGraphicFamilyIndice;

        fixed( VkCommandPool* pool =  &data.VkCommandPool){
            func.vkCreateCommandPool(data.VkDevice, &poolInfo, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool With : {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");
    }

    private static unsafe void CreateCommandBuffer(ref GraphicDeviceFunction func,ref GraphicDeviceData data ,ref GraphicRenderConfig pipeline) 
    {
        data.VkCommandBuffers = new VkCommandBuffer[pipeline.MAX_FRAMES_IN_FLIGHT]; 

        VkCommandBufferAllocateInfo allocInfo =new();// New<VkCommandBufferAllocateInfo>();
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
            allocInfo.commandPool = data.VkCommandPool;
            allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
            allocInfo.commandBufferCount = (uint)data.VkCommandBuffers.Length;
        
        fixed(VkCommandBuffer* commandBuffer = &data.VkCommandBuffers[0] ){
            func.vkAllocateCommandBuffers(data.VkDevice, &allocInfo, commandBuffer ).Check("failed to allocate command buffers!"); 
        }

        Log.Info($"Create Allocate Command buffer count : {pipeline.MAX_FRAMES_IN_FLIGHT}");
    }

    private unsafe static void DisposeCommandPool(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        if (!data.VkDevice.IsNull && !data.VkCommandPool .IsNull){
            func.vkDestroyCommandPool(data.VkDevice, data.VkCommandPool , null);
        }
    }

    #endregion

    #region Synchronisation & cache control (  Fence = memory barrier )
    private static unsafe void CreateSyncObjects(ref GraphicDeviceFunction func,ref GraphicDeviceData data   )
    {

        data.ImageAvailableSemaphores = new VkSemaphore[data.MAX_FRAMES_IN_FLIGHT]; // imageAvailableSemaphores = //.resize(MAX_FRAMES_IN_FLIGHT);
        data.RenderFinishedSemaphores = new VkSemaphore[data.MAX_FRAMES_IN_FLIGHT];// renderFinishedSemaphores.resize(MAX_FRAMES_IN_FLIGHT);
        data.InFlightFences = new VkFence[data.MAX_FRAMES_IN_FLIGHT];// inFlightFences.resize(MAX_FRAMES_IN_FLIGHT);

        VkSemaphoreCreateInfo semaphoreInfo =new();// New<VkSemaphoreCreateInfo>();
            semaphoreInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO;
            semaphoreInfo.flags =0;
            semaphoreInfo.pNext =null;

        VkFenceCreateInfo fenceInfo= new();//New<VkFenceCreateInfo>();
            fenceInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FENCE_CREATE_INFO;
            fenceInfo.flags = (uint)VkFenceCreateFlagBits.VK_FENCE_CREATE_SIGNALED_BIT;

        for (int i = 0; i < data.MAX_FRAMES_IN_FLIGHT; i++)
        {
            fixed( VkSemaphore* imageAvailableSemaphore = &data.ImageAvailableSemaphores[i]){
                func.vkCreateSemaphore(data.VkDevice, &semaphoreInfo, null,  imageAvailableSemaphore).Check("Failed to create Semaphore ImageAvailableSemaphore");
            }
            fixed( VkSemaphore* renderFinishedSemaphore = &data.RenderFinishedSemaphores[i]){
                func.vkCreateSemaphore(data.VkDevice, &semaphoreInfo, null, renderFinishedSemaphore).Check("Failed to create Semaphore RenderFinishedSemaphore");
            }
            fixed(VkFence*  inFlightFence = &data.InFlightFences[i] ){
                func.vkCreateFence(data.VkDevice, &fenceInfo, null, inFlightFence).Check("Failed to create Fence InFlightFence");
            }
        }
    }

    private static unsafe void DisposeSyncObjects(ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        if (  !data.VkDevice.IsNull && data.RenderFinishedSemaphores != null){
            for ( int i = 0 ; i< data.MAX_FRAMES_IN_FLIGHT ; i++){
                if ( !data.RenderFinishedSemaphores[i].IsNull){
                    func.vkDestroySemaphore(data.VkDevice, data.RenderFinishedSemaphores[i], null);
                }
            }
        }

        if (  !data.VkDevice.IsNull && data.ImageAvailableSemaphores != null){
            for ( int i = 0 ; i< data.MAX_FRAMES_IN_FLIGHT ; i++){
                if ( !data.ImageAvailableSemaphores[i].IsNull){
                    func.vkDestroySemaphore(data.VkDevice, data.ImageAvailableSemaphores[i], null);
                }
            }
        }

        if (  !data.VkDevice.IsNull && data.InFlightFences != null){
            for ( int i = 0 ; i< data.MAX_FRAMES_IN_FLIGHT ; i++){
                if ( !data.InFlightFences[i].IsNull){
                    func.vkDestroyFence(data.VkDevice,data.InFlightFences[i], null);
                }
            }
        }
    }

    #endregion

    #region RenderPass

    private static unsafe void CreateRenderPass(ref GraphicDeviceFunction func,ref GraphicDeviceData data,ref GraphicRenderConfig pipeline) 
    {
        // COLOR 
        VkAttachmentDescription colorAttachment =new();// New<VkAttachmentDescription>(); //new(){
            colorAttachment.format = data.VkFormat;
            colorAttachment.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            colorAttachment.loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR;
            colorAttachment.storeOp = VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_STORE;
            colorAttachment.stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE;
            colorAttachment.stencilStoreOp =VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_DONT_CARE;
            colorAttachment.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            colorAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            colorAttachment.flags =0;
            colorAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;

        // POST PROCESSING       
         VkAttachmentReference colorAttachmentRef = new();// New<VkAttachmentReference>();//new() {
            colorAttachmentRef.attachment = 0;
            colorAttachmentRef.layout =VkImageLayout. VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;

        VkSubpassDescription subpass = new() ;//New<VkSubpassDescription>(); //new(){
            subpass.pipelineBindPoint = VkPipelineBindPoint. VK_PIPELINE_BIND_POINT_GRAPHICS;
            subpass.colorAttachmentCount = 1;
            subpass.pColorAttachments = &colorAttachmentRef;
            // subpass.flags =0;
            // subpass.inputAttachmentCount=0;
            // subpass.pDepthStencilAttachment = null;
            // subpass.pInputAttachments = null;
            // subpass.pPreserveAttachments = null;
            // subpass.preserveAttachmentCount=0;

        VkSubpassDependency dependency =new(); //  New <VkSubpassDependency>();
            dependency.srcSubpass = VK.VK_SUBPASS_EXTERNAL;
            dependency.dstSubpass = 0;
            dependency.srcStageMask = (uint)VkPipelineStageFlagBits. VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT;
            dependency.srcAccessMask = 0;
            dependency.dstStageMask =(uint) VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT;
            dependency.dstAccessMask =(uint)VkAccessFlagBits. VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT;
            dependency.dependencyFlags =0;

        //RENDER PASS 
        VkRenderPassCreateInfo renderPassInfo = new();// New<VkRenderPassCreateInfo>();//new()
            renderPassInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO;
            renderPassInfo.attachmentCount = 1;
            renderPassInfo.pAttachments = &colorAttachment;
            renderPassInfo.subpassCount = 1;
            renderPassInfo.pSubpasses = &subpass;
            renderPassInfo.dependencyCount = 1;
            renderPassInfo.pDependencies = &dependency;
            renderPassInfo.flags =0;
            renderPassInfo.pNext =null;

        fixed( VkRenderPass* renderPass= &data.VkRenderPass ){
            func.vkCreateRenderPass(data.VkDevice, &renderPassInfo, null, renderPass).Check("failed to create render pass!");
        }

        Log.Info($"Create Render Pass : {data.VkRenderPass}");
    }

    private static unsafe void DisposeRenderPass(ref GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        if (!data.VkDevice.IsNull && !data.VkRenderPass.IsNull)
            func.vkDestroyRenderPass(data.VkDevice,data.VkRenderPass,null);
    }

    #endregion

    #region Pipeline

    #region FIXED FUNCTIONS
    private unsafe static void Pipeline_CreateShaderModule( out VkShaderModule shader , ref GraphicDeviceFunction func, ref GraphicDeviceData data , string shaderfragmentfileSPV)
    {
        ReadOnlySpan<byte> span = File.ReadAllBytes( shaderfragmentfileSPV ).AsSpan();
         
        VkShaderModuleCreateInfo createInfoFrag = new();
            createInfoFrag.sType= VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
            createInfoFrag.codeSize=(int)span.Length;
            createInfoFrag.pCode= (uint*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
            // createInfoFrag.pNext = null;
            // createInfoFrag.flags =0;

        VkShaderModule fragShaderModule = VkShaderModule.Null;
        func.vkCreateShaderModule(data.VkDevice, &createInfoFrag, null,  &fragShaderModule ).Check($"Create Fragment Shader Module ; {shaderfragmentfileSPV}"); 
        shader = fragShaderModule;
    }

    private unsafe static void Pipeline_CreateDescriptorSet(ref GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
         //UNFORM MVP 
        VkPipelineLayoutCreateInfo pipelineLayoutInfo=new();
            pipelineLayoutInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO;
            pipelineLayoutInfo.setLayoutCount = 0;            // Optionnel
            // fixed (VkDescriptorSetLayout* layout = &vk.descriptorSetLayout ){pipelineLayoutInfo.pSetLayouts = layout;}         // Optionnel
            pipelineLayoutInfo.pushConstantRangeCount = 0;    // Optionnel
            pipelineLayoutInfo.pPushConstantRanges = null; // Optionnel
            pipelineLayoutInfo.flags =0;
            pipelineLayoutInfo.pNext =null;

        fixed( VkPipelineLayout* layout = &data.VkpipelineLayout ){
            func.vkCreatePipelineLayout(data.VkDevice, &pipelineLayoutInfo, null, layout).Check ("failed to create pipeline layout!");
        }
    }

    #endregion

    private static unsafe void CreatePipeline(ref GraphicDeviceFunction func,ref GraphicDeviceData data , ref GraphicRenderConfig pipeline )
    {
        Pipeline_CreateDescriptorSet(ref func,ref data);

        #region SHADERS
       
       
        Pipeline_CreateShaderModule( out VkShaderModule  vertShaderModule , ref func, ref data , pipeline.VertexShaderFileNameSPV);
        Pipeline_CreateShaderModule( out VkShaderModule  fragShaderModule , ref func, ref data , pipeline.FragmentShaderFileNameSPV);
        using RitaEngine.Base.Strings.StrUnsafe fragentryPoint = new(pipeline.FragmentEntryPoint);
        using RitaEngine.Base.Strings.StrUnsafe vertentryPoint = new(pipeline.VertexEntryPoint);
       
        VkPipelineShaderStageCreateInfo* shaderStages = stackalloc VkPipelineShaderStageCreateInfo[2];        
            shaderStages[0] = new();
            shaderStages[0].sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
            shaderStages[0].stage = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
            shaderStages[0].module = vertShaderModule;
            shaderStages[0].pName = vertentryPoint;
            shaderStages[0].flags =0;
            shaderStages[0].pNext =null;
            shaderStages[0].pSpecializationInfo =null;

            shaderStages[1] = new();
            shaderStages[1].sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
            shaderStages[1].stage = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT;
            shaderStages[1].module = fragShaderModule;
            shaderStages[1].pName = fragentryPoint;
            shaderStages[1].flags =0;
            shaderStages[1].pNext =null;
            shaderStages[1].pSpecializationInfo =null;
      
        #endregion


        #region VERTEX BUFFER

        VkPipelineVertexInputStateCreateInfo vertexInputInfo = new();
        // if ( pipeline.VertexOutsideShader)
        // {
  
        // VkVertexInputBindingDescription bindingDescription =new();// GetBindingDescription( ref bindingDescription);
        //     bindingDescription.binding = 0;
        //     bindingDescription.stride =(uint) Marshal.SizeOf<Vertex>();
        //     bindingDescription.inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_VERTEX;

        // VkVertexInputAttributeDescription* attributeDescriptions = stackalloc VkVertexInputAttributeDescription[2] ; // GetAttributeDescriptions(  attributeDescriptions);
        //     attributeDescriptions[0].binding = 0;
        //     attributeDescriptions[0].location = 0;
        //     attributeDescriptions[0].format = VkFormat.VK_FORMAT_R32G32_SFLOAT;
        //     attributeDescriptions[0].offset = 0; //offsetof(Vertex, pos);

        //     attributeDescriptions[1].binding = 0;
        //     attributeDescriptions[1].location = 1;
        //     attributeDescriptions[1].format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
        //     attributeDescriptions[1].offset =  (uint)Marshal.OffsetOf( typeof(Vertex), "Color" ) ;//2;//offsetof(Vertex, color);
               
        //     vertexInputInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;
        //     vertexInputInfo.vertexBindingDescriptionCount = 1;
        //     vertexInputInfo.vertexAttributeDescriptionCount = 2;
        //     vertexInputInfo.pNext = null;
        //     vertexInputInfo.flags =0;
        //     vertexInputInfo.pVertexAttributeDescriptions=attributeDescriptions;
        //     vertexInputInfo.pVertexBindingDescriptions=&bindingDescription;

        //     data.VertexOutsideShader = true;
        // }
        // else //ELSE vertex inside shader
        // {   
            vertexInputInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;
            vertexInputInfo.vertexBindingDescriptionCount = 0;
            vertexInputInfo.vertexAttributeDescriptionCount = 0;
            // vertexInputInfo.pNext = null;
            // vertexInputInfo.flags =0;
            // vertexInputInfo.pVertexAttributeDescriptions=null;
            // vertexInputInfo.pVertexBindingDescriptions=null;
        // }

       
        #endregion

        #region INPUT ASSEMBLY
        VkPipelineInputAssemblyStateCreateInfo inputAssembly= new();
        inputAssembly.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
        inputAssembly.topology = VkPrimitiveTopology. VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
        inputAssembly.primitiveRestartEnable = VK.VK_FALSE;
        inputAssembly.flags =0;
        inputAssembly.pNext =null;       
        #endregion

        #region COLOR BLENDING
         //COLOR BLENDING
        VkPipelineColorBlendAttachmentState colorBlendAttachment =new();//New<VkPipelineColorBlendAttachmentState>(); //default;//new ();// New<VkPipelineColorBlendAttachmentState>();
            colorBlendAttachment.colorWriteMask = (uint)(VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT);
            colorBlendAttachment.blendEnable = VK.VK_FALSE;
            // colorBlendAttachment.srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
            // colorBlendAttachment.srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
            // colorBlendAttachment.alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD;
            // colorBlendAttachment.colorBlendOp =  VkBlendOp.VK_BLEND_OP_ADD;
            // colorBlendAttachment.dstAlphaBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
            // colorBlendAttachment.dstColorBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        
        VkPipelineColorBlendStateCreateInfo colorBlending=new();//New<VkPipelineColorBlendStateCreateInfo>();// default;//new ();//New<VkPipelineColorBlendStateCreateInfo>();
            colorBlending.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO;
            colorBlending.logicOpEnable = VK.VK_FALSE;
            colorBlending.logicOp = VkLogicOp. VK_LOGIC_OP_COPY;
            colorBlending.attachmentCount = 1;
            colorBlending.pAttachments = &colorBlendAttachment;
            colorBlending.blendConstants[0] = 0.0f;
            colorBlending.blendConstants[1] = 0.0f;
            colorBlending.blendConstants[2] = 0.0f;
            colorBlending.blendConstants[3] = 0.0f;
            colorBlending.flags =0;
            colorBlending.pNext=null;
        #endregion

        #region VIEWPORT
         // ViewPort et Scissor
        // VkViewport viewport=new();
            data.Viewport.x = 0.0f;
            data.Viewport.y = 0.0f;
            data.Viewport.width = (float) data.VkSurfaceArea.width;
            data.Viewport.height = (float) data.VkSurfaceArea.height;
            data.Viewport.minDepth = 0.0f;
            data.Viewport.maxDepth = 1.0f;

        VkOffset2D offset = new();
            offset.x = 0;
            offset.y = 0;
        // VkRect2D scissor = new();
            data.Scissor.offset = offset;
            data.Scissor.extent = data.VkSurfaceArea;

        VkPipelineViewportStateCreateInfo viewportState =new();
            viewportState.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO;
            viewportState.viewportCount = 1;
            fixed( VkViewport* viewport = &data.Viewport){    viewportState.pViewports =viewport;    }
            viewportState.scissorCount = 1;
            fixed( VkRect2D* scissor = &data.Scissor ){  viewportState.pScissors = scissor;          }
            viewportState.flags=0;
            viewportState.pNext = null;
        #endregion

        #region  RASTERIZATION
        VkPipelineRasterizationStateCreateInfo rasterizer =new();
            rasterizer.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO;
            rasterizer.rasterizerDiscardEnable = VK.VK_FALSE;
            rasterizer.polygonMode =VkPolygonMode. VK_POLYGON_MODE_FILL;
            rasterizer.lineWidth = 1.0f;
            rasterizer.cullMode = (uint)VkCullModeFlagBits.VK_CULL_MODE_BACK_BIT;
            rasterizer.frontFace =VkFrontFace. VK_FRONT_FACE_CLOCKWISE;
            rasterizer.flags =0;
            rasterizer.pNext = null;
            rasterizer.depthBiasEnable = VK.VK_FALSE;
            rasterizer.depthClampEnable = VK.VK_FALSE;
            rasterizer.depthBiasClamp =0.0f;
            rasterizer.depthBiasConstantFactor =1.0f;
            rasterizer.depthBiasSlopeFactor =1.0f;
            rasterizer.flags =0;
        #endregion

        #region MULTISAMPLING
        VkPipelineMultisampleStateCreateInfo multisampling=new();
            multisampling.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO;
            multisampling.sampleShadingEnable = VK.VK_FALSE;
            multisampling.rasterizationSamples =VkSampleCountFlagBits. VK_SAMPLE_COUNT_1_BIT;
            multisampling.alphaToCoverageEnable =0;
            multisampling.alphaToOneEnable =0;
            multisampling.flags =0;
            multisampling.minSampleShading =0.0f;
            multisampling.pNext = null;
            multisampling.pSampleMask =null;
        #endregion
        
        #region DEPTh & STENCIL
           //not used 
        VkPipelineTessellationStateCreateInfo tessellationStateCreateInfo = new();
            tessellationStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_TESSELLATION_STATE_CREATE_INFO;

        VkPipelineDepthStencilStateCreateInfo depthStencilStateCreateInfo = new();
            depthStencilStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
        #endregion
        
        #region DYNAMIC STATES
          // VkDynamicState* dynamicStates = stackalloc VkDynamicState[5] {
        //     VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT,
        //     VkDynamicState.VK_DYNAMIC_STATE_LINE_WIDTH,
        //     VkDynamicState.VK_DYNAMIC_STATE_PRIMITIVE_TOPOLOGY,
        //     VkDynamicState.VK_DYNAMIC_STATE_CULL_MODE,
        //     VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT_WITH_COUNT_EXT,
        // };

        
            // DynamicStateConfig cfg = new();
            // DynamicState dynstate = new();

            // if ( cfg.UseViewport )
            // {
            //     dynstate.SetViewport = null!;
            //     dynstate.states.Add(VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT );
            // }
       
        // VkDynamicState* dyn = dynstate.states.ToArray();
        VkDynamicState* dynamicStates = stackalloc VkDynamicState[2] {
            VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT,
            VkDynamicState.VK_DYNAMIC_STATE_SCISSOR,
        };
           VkPipelineDynamicStateCreateInfo dynamicStateCreateInfo = new();
            dynamicStateCreateInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO;
            dynamicStateCreateInfo.dynamicStateCount = 2;
            dynamicStateCreateInfo.pDynamicStates = dynamicStates;
        #endregion

        VkGraphicsPipelineCreateInfo pipelineInfo =new();
            pipelineInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO;
            pipelineInfo.flags = (uint)VkPipelineCreateFlagBits.VK_PIPELINE_CREATE_DISABLE_OPTIMIZATION_BIT ;

            pipelineInfo.renderPass = data.VkRenderPass;
            pipelineInfo.subpass = 0;
            pipelineInfo.basePipelineHandle = VkPipeline.Null;
            
            pipelineInfo.stageCount =2;
            pipelineInfo.pStages = shaderStages;//fixed( VkPipelineShaderStageCreateInfo* shaderStages = &shaderStagesArray[0]){  pipelineInfo.pStages = shaderStages ;}
       
            pipelineInfo.pVertexInputState = &vertexInputInfo;
            pipelineInfo.pInputAssemblyState = &inputAssembly;
            pipelineInfo.pColorBlendState = &colorBlending;
            pipelineInfo.pViewportState = &viewportState;
            pipelineInfo.pRasterizationState = &rasterizer;
            pipelineInfo.pMultisampleState = &multisampling;
            pipelineInfo.layout = data.VkpipelineLayout;
            pipelineInfo.pTessellationState = &tessellationStateCreateInfo;
            pipelineInfo.pDepthStencilState = &depthStencilStateCreateInfo;
            pipelineInfo.pDynamicState = &dynamicStateCreateInfo;
            pipelineInfo.pNext = null;
            pipelineInfo.basePipelineIndex =0;
            
        fixed( VkPipeline* gfxpipeline = &data.VkGraphicsPipeline ) {    
            func.vkCreateGraphicsPipelines(data.VkDevice, VkPipelineCache.Null, 1, &pipelineInfo, null, gfxpipeline).Check("failed to create graphics pipeline!");
        }

        if( fragShaderModule != VkShaderModule.Null)
            func.vkDestroyShaderModule(data.VkDevice, fragShaderModule, null);
        if( vertShaderModule != VkShaderModule.Null)
            func.vkDestroyShaderModule(data.VkDevice, vertShaderModule, null);
        // fragentryPoint.Dispose();
        // vertentryPoint.Dispose();
    }

    private static unsafe void RecordCommandBuffer(ref GraphicDeviceFunction func,ref GraphicDeviceData data, in VkCommandBuffer commandBuffer, uint imageIndex)
    {
         //----------Début de l'enregistrement des commandes--------------------------------------------------       
        func.vkResetCommandBuffer(commandBuffer, /*VkCommandBufferResetFlagBits*/ 0);

      { //BEGIN COMMAND BUFFER
         VkCommandBufferBeginInfo beginInfo =default;
            beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO; //VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
            beginInfo.pNext =null;
            beginInfo.flags =(uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT;
            beginInfo.pInheritanceInfo= null;
        
        func.vkBeginCommandBuffer(commandBuffer, &beginInfo).Check("Failed to Begin command buffer");}
        
        if( data.VkPresentFamilyIndice != data.VkGraphicFamilyIndice ) {
            VkImageSubresourceRange image_subresource_range = default;
                image_subresource_range.aspectMask=(uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
                image_subresource_range.baseMipLevel=0;
                image_subresource_range.levelCount=1;
                image_subresource_range.baseArrayLayer=0;
                image_subresource_range.layerCount=     1;

            VkImageMemoryBarrier barrier_from_present_to_draw = default ;
                barrier_from_present_to_draw.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;     // VkStructureType                sType
                barrier_from_present_to_draw.pNext = null;                                    // const void                    *pNext
                barrier_from_present_to_draw.srcAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_MEMORY_READ_BIT;                  // VkAccessFlags                  srcAccessMask
                barrier_from_present_to_draw.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT;       // VkAccessFlags                  dstAccessMask
                barrier_from_present_to_draw.oldLayout=VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;                  // VkImageLayout                  oldLayout
                barrier_from_present_to_draw.newLayout=VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;            // VkImageLayout                  newLayout
                barrier_from_present_to_draw.srcQueueFamilyIndex=data.VkPresentFamilyIndice;              // uint32_t                       srcQueueFamilyIndex
                barrier_from_present_to_draw.dstQueueFamilyIndex=data.VkGraphicFamilyIndice;             // uint32_t                       dstQueueFamilyIndex
                barrier_from_present_to_draw.image= data.VkImages[CurrentFrame];                // VkImage                        image
                barrier_from_present_to_draw.subresourceRange=image_subresource_range;                     // VkImageSubresourceRange        subresourceRange
        
            func.vkCmdPipelineBarrier( commandBuffer, (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT, (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT, 0, 0, null, 0, null, 1, &barrier_from_present_to_draw );
        }

       { //BEGIN RENDER PASS
         VkRenderPassBeginInfo renderPassInfo = default;
            renderPassInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO; //VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO;
            renderPassInfo.renderPass = data.VkRenderPass;
            renderPassInfo.framebuffer = data.VkFramebuffers[imageIndex];
            renderPassInfo.renderArea.offset = data.RenderAreaOffset;
            renderPassInfo.renderArea.extent = data.VkSurfaceArea;
            renderPassInfo.clearValueCount = 1;
            // renderPassInfo.pClearValues =  data.ClearColor;//
            fixed(VkClearValue* clearColor = &data.ClearColor ) {  renderPassInfo.pClearValues =  clearColor; }
        func.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo,VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);}
       
        func.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.VkGraphicsPipeline);

        // SET DYNAMIC STATES
        fixed(VkViewport* viewport = &data.Viewport ){ func.vkCmdSetViewport(commandBuffer, 0, 1,viewport);  }
        fixed( VkRect2D* scissor = &data.Scissor){ func.vkCmdSetScissor(commandBuffer, 0, 1, scissor); }
        //--------------------------------------------------------------------------------------
       
        
        func.vkCmdDraw(commandBuffer, 3,(uint)(data.VertexOutsideShader?3:1), 0, 0);       

        func.vkCmdEndRenderPass(commandBuffer);
       
        if( data.VkPresentFamilyIndice != data.VkGraphicFamilyIndice ) {
            VkImageSubresourceRange image_subresource_range = default;
                image_subresource_range.aspectMask=(uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
                image_subresource_range.baseMipLevel=0;
                image_subresource_range.levelCount=1;
                image_subresource_range.baseArrayLayer=0;
                image_subresource_range.layerCount=     1;

            VkImageMemoryBarrier barrier_from_draw_to_present = default ;
                barrier_from_draw_to_present.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;     // VkStructureType                sType
                barrier_from_draw_to_present.pNext = null;                                    // const void                    *pNext
                barrier_from_draw_to_present.srcAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT;                  // VkAccessFlags                  srcAccessMask
                barrier_from_draw_to_present.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_MEMORY_READ_BIT;       // VkAccessFlags                  dstAccessMask
                barrier_from_draw_to_present.oldLayout=VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;                  // VkImageLayout                  oldLayout
                barrier_from_draw_to_present.newLayout=VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;            // VkImageLayout                  newLayout
                barrier_from_draw_to_present.srcQueueFamilyIndex=data.VkGraphicFamilyIndice;              // uint32_t                       srcQueueFamilyIndex
                barrier_from_draw_to_present.dstQueueFamilyIndex=data.VkPresentFamilyIndice;             // uint32_t                       dstQueueFamilyIndex
                barrier_from_draw_to_present.image= data.VkImages[CurrentFrame];                // VkImage                        image
                barrier_from_draw_to_present.subresourceRange=image_subresource_range;                     // VkImageSubresourceRange        subresourceRange
        
            func.vkCmdPipelineBarrier( commandBuffer, (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT, (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT, 0, 0, null, 0, null, 1, &barrier_from_draw_to_present );
        }
        
        func.vkEndCommandBuffer(commandBuffer).Check("Failed to End command buffer ");
    }
    private static int CurrentFrame =0;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static unsafe  void DrawPipeline( ref GraphicDeviceFunction func, ref GraphicDeviceData data )
    {
        uint imageIndex=0;
        VkFence CurrentinFlightFence = data.InFlightFences[/*data.*/CurrentFrame];
        VkSemaphore CurrentImageAvailableSemaphore =  data.ImageAvailableSemaphores[/*data.*/CurrentFrame];
        VkSemaphore CurrentRenderFinishedSemaphore = data.RenderFinishedSemaphores[/*data.*/CurrentFrame];
        VkSemaphore* waitSemaphores = stackalloc VkSemaphore[1] {CurrentImageAvailableSemaphore};// VkSemaphore[] waitSemaphores = {CurrentImageAvailableSemaphore};
        UInt32* waitStages  = stackalloc UInt32[1]{(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT }; //*VkPipelineStageFlags*/UInt32[] waitStages = {(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT};
        VkSemaphore*   signalSemaphores  = stackalloc VkSemaphore[1] {CurrentRenderFinishedSemaphore} ;// VkSemaphore[] signalSemaphores = {CurrentRenderFinishedSemaphore};
        VkSwapchainKHR* swapChains = stackalloc  VkSwapchainKHR[1]{ data.VkSwapChain };// VkSwapchainKHR[] swapChains = { data.VkSwapChain };
        VkCommandBuffer commandBuffer =data.VkCommandBuffers[/*data.*/CurrentFrame];

        func.vkWaitForFences(data.VkDevice, 1,&CurrentinFlightFence, VK.VK_TRUE, data.tick_timeout).Check("Acquire Image");

        VkResult result = func.vkAcquireNextImageKHR(data.VkDevice, data.VkSwapChain, data.tick_timeout,CurrentImageAvailableSemaphore, VkFence.Null, &imageIndex);

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
        {
            ReCreateSwapChain(ref func,ref data);
            // RecreatePipeline(ref data ,in pipeline);
            return ;
        }
        else if (result != VkResult.VK_SUCCESS && result != VkResult.VK_SUBOPTIMAL_KHR )
        {
            throw new Exception("Failed to acquire swap chain Images");
        }

        // UpdateUniform(ref vk , ref gfx);

        func.vkResetFences(data.VkDevice, 1, &CurrentinFlightFence);

        //----------Début de l'enregistrement des commandes--------------------------------------------------   
        RecordCommandBuffer( ref func,ref data, in  commandBuffer, imageIndex);
        //----------Fin de l'enregistrement des commandes--------------------------------------------------    


        VkSubmitInfo submitInfo = default;
            submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
            submitInfo.waitSemaphoreCount = 1;
            submitInfo.pWaitSemaphores = waitSemaphores;// fixed(VkSemaphore* waitSemaphoresPtr = &waitSemaphores[0]   ){   submitInfo.pWaitSemaphores = waitSemaphoresPtr;        }
            submitInfo.pWaitDstStageMask =  waitStages;// fixed(/*VkPipelineStageFlags*/UInt32* waitStagesPtr = &waitStages[0]   ){ submitInfo.pWaitDstStageMask = waitStagesPtr;    }
            submitInfo.commandBufferCount = 1;
            submitInfo.pCommandBuffers = &commandBuffer;      
            submitInfo.signalSemaphoreCount = 1;
            submitInfo.pSignalSemaphores = signalSemaphores ;// fixed( VkSemaphore* signalSemaphoresPtr =&signalSemaphores[0]){ submitInfo.pSignalSemaphores = signalSemaphoresPtr; }
        
        func.vkQueueSubmit(data.VkGraphicQueue, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
        
        VkPresentInfoKHR presentInfo=default;
            presentInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR; //VK_STRUCTURE_TYPE_PRESENT_INFO_KHR;
            presentInfo.waitSemaphoreCount = 1;
            presentInfo.pWaitSemaphores = signalSemaphores;//fixed( VkSemaphore* signalSemaphoresPtr =&signalSemaphores[0]){ presentInfo.pWaitSemaphores = signalSemaphoresPtr;}
            presentInfo.pImageIndices = &imageIndex;
            presentInfo.swapchainCount = 1;
            presentInfo.pSwapchains = swapChains;//fixed( VkSwapchainKHR* swapChainsPtr = &swapChains[0]){ presentInfo.pSwapchains = swapChainsPtr; }
        
        result = func.vkQueuePresentKHR(data.VkPresentQueue, &presentInfo); //.Check("Present Image");  

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR || result == VkResult.VK_SUBOPTIMAL_KHR )
        {
            ReCreateSwapChain(ref func,ref data);
            //  RecreatePipeline(ref data ,in pipeline);
        }
        else if (result != VkResult.VK_SUCCESS )
        {
            throw new Exception("Failed to  present swap chain Images");
        }
       /*data.*/CurrentFrame = ((/*data.*/CurrentFrame + 1) % data.MAX_FRAMES_IN_FLIGHT);   
    }

    private static unsafe void DisposePipeline(ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        if( !data.VkDevice.IsNull && !data.VkpipelineLayout.IsNull){
            func.vkDestroyPipelineLayout(data.VkDevice, data.VkpipelineLayout, null);
        }        
        if (!data.VkDevice.IsNull && !data.VkGraphicsPipeline.IsNull){
            func.vkDestroyPipeline(data.VkDevice,data.VkGraphicsPipeline, null);
        }
    }

    #endregion

    #region OVERRIDE    
    public unsafe nint AddressOfPtrThis( )
    { 
        #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
    public override string ToString() => string.Format($"Data GraphicDevice " );
    public override int GetHashCode() => HashCode.Combine( _loader.GetHashCode(), _loader.GetHashCode());
    public override bool Equals(object? obj) => obj is GraphicDevice  window && this.Equals(window) ;
    public bool Equals(GraphicDevice other)=>  _data.Equals(other._data) ;
    public static bool operator ==(GraphicDevice  left,GraphicDevice right) => left.Equals(right);
    public static bool operator !=(GraphicDevice  left,GraphicDevice right) => !left.Equals(right);
    #endregion
}
