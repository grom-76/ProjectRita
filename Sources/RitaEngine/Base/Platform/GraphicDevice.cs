namespace RitaEngine.Base.Platform;

using System.IO;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Math.Color;
using RitaEngine.Base.Platform.Config;
using RitaEngine.Base.Platform.API.Vulkan;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Strings;
using RitaEngine.Base.Math.Vertex;

using VkDeviceSize = UInt64;
using RitaEngine.Base.Resources.Images;

/// <summary>
/// 
/// </summary>
[SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack =  BaseHelper.FORCE_ALIGNEMENT)]
public struct GraphicDevice : IEquatable<GraphicDevice>
{
    private GraphicDeviceFunctions _functions;
    private GraphicDeviceData _data; // inside => Instance Device Render Infos 

    public GraphicDevice() 
    {
        _functions = new();
        _data = new();
    }
   
    public unsafe void Init(in GraphicDeviceConfig config, in Window window )
    {
        TransfertToData(in config,in window ,ref _functions , ref _data);   
        GraphicDeviceImplement.Init(ref _functions , ref _data);
    }

    private static unsafe void TransfertToData(in GraphicDeviceConfig config, in Window window ,ref GraphicDeviceFunctions functions , ref GraphicDeviceData data)
    {
        //check if Exist vulkandllname? 
        functions.InitLoaderFunctions( config.VulkanDllName);
        data.App.EnableDebug = config.EnableDebugMode;
        data.App.GameName  = window.GetWindowName();
        data.App.Handle = window.GetWindowHandle();
        data.App.HInstance = window.GetWindowHInstance();
        data.App.Width = window.GetWindowWidth();
        data.App.Height = window.GetWindowheight();
        data.GetFrameBufferCallback = window.GetFrameBuffer;
    }

    public unsafe void Release()
    {  
        GraphicDeviceImplement.Release(in _functions , ref _data);
        _data.Release();
        _functions.Release();
    }

    public void Pause()
    {
        GraphicDeviceImplement.Pause( _functions.DeviceFunction , ref _data);
    }

    public void BuildRender(in GraphicRenderConfig config)
    {
        TransfertToRender(in config, in _functions , ref _data);
        GraphicDeviceImplement.BuildRender(ref _functions, ref _data);
    }

    private void TransfertToRender(in GraphicRenderConfig pipeline, in GraphicDeviceFunctions functions, ref GraphicDeviceData data)
    {
        data.RenderAreaOffset.x =0;
        data.RenderAreaOffset.y =0;       
        data.ClearColor = new(ColorHelper.ToRGBA( (uint)pipeline.BackColorARGB),00000.0f,0);
        data.ubo = pipeline.ubo;
        data.Indices = pipeline.Indices;
        data.Vertices = pipeline.Vertices;
        data.IndicesSize =(uint) pipeline.Indices.Length;
        data.TextureName =  pipeline.TextureName;
        data.MAX_FRAMES_IN_FLIGHT = pipeline.MAX_FRAMES_IN_FLIGHT;
        data.FragmentEntryPoint = pipeline.FragmentEntryPoint;
        data.VertexEntryPoint = pipeline.VertexEntryPoint;
        data.FragmentShaderFileNameSPV = pipeline.FragmentShaderFileNameSPV;
        data.VertexShaderFileNameSPV = pipeline.VertexShaderFileNameSPV;


    }

    public void UpdateRender(in GraphicRenderConfig config)
    {
        TransfertToRender(in config, in _functions , ref _data);
        GraphicDeviceImplement.UpdateUniformBuffer(_functions.DeviceFunction,ref _data);
    }
    /// <summary>
    /// Don't forget to Do Update Before Draw 
    /// </summary>
    public void DrawRender()
    {
        GraphicDeviceImplement.DrawPipeline(ref _functions, ref _data);
    }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Data GraphicDevice " );
    public override int GetHashCode() => HashCode.Combine( _functions.GetHashCode(), _data.GetHashCode());
    public override bool Equals(object? obj) => obj is GraphicDevice  window && this.Equals(window) ;
    public bool Equals(GraphicDevice other)=>  _data.Equals(other._data) ;
    public static bool operator ==(GraphicDevice  left,GraphicDevice right) => left.Equals(right);
    public static bool operator !=(GraphicDevice  left,GraphicDevice right) => !left.Equals(right);
    #endregion
}

public static class GraphicDeviceImplement
{
    #region public PRUPOSE
    public static void Init(ref GraphicDeviceFunctions func , ref GraphicDeviceData data)
    {
// APP  : WSI , DEBUG , INSTANCE        
        CreateInstanceAndDebug(ref func,ref data);
        CreateSurface(ref func.InstanceFunction , ref data);
// //DEVICE          
        SelectPhysicalDevice(ref func.InstanceFunction ,ref data);
        GetPhysicalInformations( ref func, ref data);
        CreateLogicalDevice(ref func ,ref data );
// //SWAP CHAIN
        CreateSwapChain( ref func.DeviceFunction , ref data);
        CreateImageViews( ref func.DeviceFunction , ref data );
    }
    
    public static unsafe void Release(in GraphicDeviceFunctions func , ref GraphicDeviceData data )
    {
        Log.Info("Dispose Graphic Device");

        DisposeForReCreateSwapChain( func , ref data);

        DisposeBuildRender(func, ref data);
        
        DisposeLogicalDevice(in func.InstanceFunction ,ref data);
        DisposeSurface(func.InstanceFunction, ref data);
        DisposeInstanceAndDebug(in func,ref data);  
    }

    public unsafe static void Pause(in GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        if ( !data.VkDevice.IsNull){
            func.vkDeviceWaitIdle(data.VkDevice).Check($"WAIT IDLE VkDevice : {data.VkDevice}");
        }
    }

    public unsafe static void BuildRender(ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        CreateRenderPass(ref func,ref data);
        CreateDescriptorSetLayout(ref func.DeviceFunction , ref data);
       
        CreateCommandPool(ref func.DeviceFunction,ref data);
        CreateDepthResources( ref func, ref data);
        CreateFramebuffers(ref func.DeviceFunction,ref data);
        CreateTextureImage(ref func , ref data);
        CreateTextureImageView(ref func.DeviceFunction , ref data);
        CreateTextureSampler(ref func.DeviceFunction , ref data);
        //LoadModel => Do in TransfertConfigRenderToData 
        CreateVertexBuffer( ref func , ref data );
        CreateIndexBuffer( ref func , ref data);
        CreateUniformBuffers(ref func , ref data );
        CreateDescriptorPool(ref func.DeviceFunction,ref data);
        CreateDescriptorSets(ref func.DeviceFunction,ref data);
        CreateCommandBuffer(ref func.DeviceFunction,ref data);

        CreatePipelineLayout( ref func.DeviceFunction , ref data);
        CreatePipeline(ref func.DeviceFunction, ref data );
        CreateSyncObjects(ref func.DeviceFunction , ref data);
    }
    
    public unsafe static void DisposeBuildRender(in  GraphicDeviceFunctions func,ref GraphicDeviceData data  )
    {
        DisposePipeline( func.DeviceFunction,ref data);
        DisposeRenderPass( func.DeviceFunction,ref data);
        DisposeUniformBuffers( func.DeviceFunction , ref data);
        DisposeTextureSampler( func.DeviceFunction , ref data);
        DisposeTextureImage( func.DeviceFunction , ref data);
        DisposeDescriptorPool( func.DeviceFunction , ref data);
        DisposeBuffers( func.DeviceFunction , ref data);
        DisposeSyncObjects( func.DeviceFunction ,ref data);
        DisposeCommandPool( func.DeviceFunction,ref data);
    }

    public unsafe static void ReCreateSwapChain(ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        if ( data.SwapChain.VkSwapChain == VkSwapchainKHR.Null)return ;

        data.GetFrameBufferCallback(ref data.SwapChain.VkSurfaceArea.width ,ref data.SwapChain.VkSurfaceArea.height );

        DisposeForReCreateSwapChain( func, ref data);
       
        CreateSwapChain(ref func.DeviceFunction,ref data);
        CreateImageViews(ref func.DeviceFunction, ref data);
        CreateDepthResources( ref func, ref data);
        CreateFramebuffers(ref func.DeviceFunction,ref data);
        
    }

    public unsafe static void DisposeForReCreateSwapChain(in GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        Pause( func.DeviceFunction,ref data);
        DisposeDepthResources(  func.DeviceFunction , ref data);
        DisposeFrameBuffer(func.DeviceFunction, ref data);
        DisposeSwapChain(func.DeviceFunction , ref data);
    }

    public static string VersionToString( uint version ) => $"{VK.VK_VERSION_MAJOR(version)}.{VK.VK_VERSION_MINOR(version)}.{VK.VK_VERSION_PATCH(version)} ";

    #endregion

    #region INSTANCE & DEBUG

    private unsafe static void CreateInstanceAndDebug(ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        // VALIDATION LAYER  --------------------------------------------------------------------
        uint layerCount = 0;
        func.LoaderFunction.vkEnumerateInstanceLayerProperties(&layerCount, null).Check("Enumerate instance Layer count");
        Guard.ThrowWhenConditionIsTrue( layerCount ==0 );

        VkLayerProperties* layerProperties = stackalloc VkLayerProperties[(int)layerCount];// ReadOnlySpan<VkLayerProperties> pp = stackalloc VkLayerProperties[(int)count];
        func.LoaderFunction.vkEnumerateInstanceLayerProperties(&layerCount, layerProperties).Check("Enumerate instance Layer list");

       data.App.ValidationLayers = new  string[ layerCount ];
        for (int i = 0; i < layerCount; i++) {
            var length = Strings.StrHelper.Strlen( layerProperties[i].layerName );
           data.App.ValidationLayers[i] = Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );// new string(layerProperties[i].layerName); //Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );
        }

        //-- VULKAN API VERSION ------------------------------------------------------------------
        // uint ver=0;Infos.VkVersion
        fixed ( uint* ver = &data.App.Version){
        func.LoaderFunction.vkEnumerateInstanceVersion(ver).Check("Enumerate Instance Version");}

        //--INSTANCE EXTENSIONS ------------------------------------------------------------------
        uint extCount = 0;
        func.LoaderFunction.vkEnumerateInstanceExtensionProperties(null, &extCount, null).Check( "Enumerate Extension Name Count");
        Guard.ThrowWhenConditionIsTrue( extCount == 0);

        VkExtensionProperties* props = stackalloc VkExtensionProperties[(int)extCount];
        func.LoaderFunction.vkEnumerateInstanceExtensionProperties(null, &extCount, props).Check( "Enumerate Extension Name List");

       data.App.InstanceExtensions = new string[extCount ];
        for (int i = 0; i < extCount; i++){
            var length = Strings.StrHelper.Strlen( props[i].extensionName);
           data.App.InstanceExtensions[i] =Encoding.UTF8.GetString(  props[i].extensionName, (int) length );// new string( props[i].extensionName) ;//Encoding.UTF8.GetString(  props[i].extensionName, (int) length );
        }

        // CREATE DEBUG INFO ------------------------------------------------
        VkDebugUtilsMessengerCreateInfoEXT debugCreateInfo = default;
            debugCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT;
            debugCreateInfo.pNext = null;
            debugCreateInfo.messageSeverity = (uint)( VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT );
            debugCreateInfo.messageType = (uint) (VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VkDebugUtilsMessageTypeFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT);
            debugCreateInfo.pfnUserCallback = &DebugMessengerCallback;
            debugCreateInfo.pUserData = null;
        
        //CREATE APPLICATION INFO ------------------------------------------------
        var EngineName = Encoding.UTF8.GetBytes(RitaEngine.Base.BaseHelper.ENGINE_NAME);

        VkApplicationInfo appInfo = default;
            appInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO;
            appInfo.apiVersion =data.App.Version; 
            appInfo.applicationVersion = VK.VK_MAKE_VERSION(1,0,0);
            appInfo.engineVersion= VK.VK_MAKE_VERSION(1,0,0);
            appInfo.pNext =null;

        fixed(byte* ptr = &EngineName[0] ,  app = &data.App.GameName[0] ) {
            appInfo.pApplicationName =app;
            appInfo.pEngineName = ptr;
        }
        //CREATE INSTANCE  INFO  et POPULATE DEBUG ------------------------------------------------
        using var extNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.App.InstanceExtensions) ;
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.App.ValidationLayers);

        VkInstanceCreateInfo instanceCreateInfo = default;
            instanceCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
            instanceCreateInfo.pApplicationInfo =&appInfo;
            instanceCreateInfo.pNext= !data.App.EnableDebug ? null :   &debugCreateInfo;
            instanceCreateInfo.ppEnabledExtensionNames = extNames;
            instanceCreateInfo.enabledExtensionCount =extNames.Count;
            
            if (data.App.EnableDebug) {
                instanceCreateInfo.enabledLayerCount = layerNames.Count;
                instanceCreateInfo.ppEnabledLayerNames = layerNames;
            }
            else {
                instanceCreateInfo.enabledLayerCount =0;
                instanceCreateInfo.ppEnabledLayerNames =null;
            }
            instanceCreateInfo.flags =  (uint)VkInstanceCreateFlagBits.VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR;       

        fixed( VkInstance* instance = &data.App.VkInstance) {
            func.LoaderFunction.vkCreateInstance(&instanceCreateInfo, null, instance).Check("failed to create instance!");
        };

        VK.VK_KHR_swapchain=true;// //Special dont understand pour chage swapchain car nvidia n'a pas l'extension presente
        VkHelper.ValidateExtensionsForLoad(ref data.App.InstanceExtensions,0 );

        func.InitInstancesFunctions( GraphicDeviceLoaderFunction.vkGetInstanceProcAddr ,data.App.VkInstance );

        // CREATE DEBUG ------------------------------------------------------------------------
        if ( !data.App.EnableDebug  )return ;
        
        fixed(VkDebugUtilsMessengerEXT* dbg = &data.App.DebugMessenger )
        {
            func.InstanceFunction.vkCreateDebugUtilsMessengerEXT(data.App.VkInstance, &debugCreateInfo, null, dbg).Check("failed to set up debug messenger!");
        }
    }

    private unsafe static void DisposeInstanceAndDebug(in GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        Log.Info($"Dispose DEBUG");
        if (!data.App.VkInstance.IsNull && !data.App.DebugMessenger.IsNull){
            Log.Info($"Release DebugMessenger [{data.App.DebugMessenger}]");
            func.InstanceFunction.vkDestroyDebugUtilsMessengerEXT(data.App.VkInstance,data.App.DebugMessenger,null);
        } 
        Log.Info($"Dispose Vulkan Instance");

        if ( !data.App.VkInstance.IsNull){
            Log.Info($"Release Instance [{data.App.VkInstance}]");
            func.LoaderFunction.vkDestroyInstance(data.App.VkInstance, null);
            data.App.VkInstance = VkInstance.Null;
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
            sci .hinstance = data.App.HInstance;
            sci .hwnd = data.App.Handle;
            sci .pNext = null;
            sci .flags = 0;
            sci .sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;

        fixed ( VkSurfaceKHR* surf = &data.App.VkSurface){
            func.vkCreateWin32SurfaceKHR(data.App.VkInstance,&sci,null, surf).Check("Create Surface");
        }
        #endif
    }
    
    private static unsafe void DisposeSurface(in GraphicInstanceFunction func,ref GraphicDeviceData data)
    {
        Log.Info($"Dispose Surface");
        if ( !data.App.VkInstance.IsNull && !data.App.VkSurface.IsNull){
            Log.Info($"Release Surface [{data.App.VkSurface}]");
            func.vkDestroySurfaceKHR(data.App.VkInstance,data.App.VkSurface,null);
        }
    }

    #endregion
    
    #region PHYSICAL DEVICE 

    private static unsafe void SelectPhysicalDevice( ref GraphicInstanceFunction func,ref GraphicDeviceData data )
    {
        uint deviceCount = 0;
        func.vkEnumeratePhysicalDevices(data.App.VkInstance, &deviceCount, null).Check("EnumeratePhysicalDevices Count");
        Guard.ThrowWhenConditionIsTrue(deviceCount == 0,"Vulkan: Failed to find GPUs with Vulkan support");

        VkPhysicalDevice* devices = stackalloc VkPhysicalDevice[(int)deviceCount];
        func.vkEnumeratePhysicalDevices(data.App.VkInstance, &deviceCount, devices).Check("EnumeratePhysicalDevices List");

        for (int i = 0; i < (int)deviceCount; i++)
        {
            VkPhysicalDevice physicalDevice = devices[i];

            if ( IsDeviceSuitable(ref func,physicalDevice, data.App.VkSurface) )
            {
                data.Physical.VkPhysicalDevice = physicalDevice;
                break;
            }
        }
        Guard.ThrowWhenConditionIsTrue( data.Physical.VkPhysicalDevice.IsNull , "Physical device is null ");

        Log.Info($"Create Physical device {data.Physical.VkPhysicalDevice}");
    }

    private static unsafe void GetPhysicalInformations( ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        SwapChainSupportDetails swap = QuerySwapChainSupport( ref func.InstanceFunction , data.Physical.VkPhysicalDevice , data.App.VkSurface  );
        data.Physical.Capabilities = swap.Capabilities;
        data.Physical.PresentModes = swap.PresentModes.ToArray();
        data.Physical.Formats = swap.Formats.ToArray(); 

        (data.Physical.VkGraphicFamilyIndice,data.Physical.VkPresentFamilyIndice) = FindQueueFamilies(ref func.InstanceFunction , data.Physical.VkPhysicalDevice,data.App.VkSurface);
        
        // //FOR INFO .... bool in GraphicDeviceSettings.WithInfo       if ( !settings.Instance.GetDeviceInfo ) return;

         //Display enumeration
        // uint physicalDeviceDisplayPropertiesCount =0;
       
        // func.vkGetPhysicalDeviceDisplayPropertiesKHR(data.VkPhysicalDevice, &physicalDeviceDisplayPropertiesCount, null );
        // Guard.IsEmpty(physicalDeviceDisplayPropertiesCount);

        // VkDisplayPropertiesKHR*  displayPropertiesKHR = stackalloc VkDisplayPropertiesKHR[(int)physicalDeviceDisplayPropertiesCount];
        // func.vkGetPhysicalDeviceDisplayPropertiesKHR(data.VkPhysicalDevice, &physicalDeviceDisplayPropertiesCount, displayPropertiesKHR );
        // // if( IsFeatures2Capabilities)
        // // {
        // //     func.vkGetPhysicalDeviceDisplayProperties2KHR
        // // }

        // // Display PLanes
        // uint physicalDeviceDisplayPlanePropertiesCount =0;
       
        // func.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(data.VkPhysicalDevice, &physicalDeviceDisplayPlanePropertiesCount, null );
        // Guard.IsEmpty(physicalDeviceDisplayPlanePropertiesCount);

        // VkDisplayPlanePropertiesKHR*  physicalDeviceDisplayPlanes = stackalloc VkDisplayPlanePropertiesKHR[(int)physicalDeviceDisplayPlanePropertiesCount];
        // func.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(data.VkPhysicalDevice, &physicalDeviceDisplayPlanePropertiesCount,physicalDeviceDisplayPlanes );
        // if( IsFeatures2Capabilities)
        // {
        //     func.vkGetPhysicalDeviceDisplayPlaneProperties2KHR
        //      VkDisplayPlaneProperties2KHR*
        // }
        
        // func.vkGetDisplayPlaneCapabilitiesKHR( data.VkPhysicalDevice , )

        //DISPLAY MODE
        // VkDisplayModeParametersKHR displayModeParameter = new();
        // VkDisplayModeCreateInfoKHR displayModeCreateInfo = default;
        // displayModeCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DISPLAY_MODE_CREATE_INFO_KHR;
        // displayModeCreateInfo.flags =0;
        // displayModeCreateInfo.pNext = null;
        // displayModeCreateInfo.parameters = displayModeParameter;
        // VkDisplayKHR displayKHR = VkDisplayKHR.Null;
        // func.vkCreateDisplayModeKHR(data.VkPhysicalDevice,displayModeCreateInfo, displayKHR, null )

        // uint displayModeProperties =0;
       
        // func.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(data.VkPhysicalDevice, &displayModeProperties, null );
        // Guard.IsEmpty(displayModeProperties);

        // VkDisplayModePropertiesKHR*  displayModes = stackalloc VkDisplayModePropertiesKHR[(int)displayModeProperties];
        // func.vkGetDisplayModePropertiesKHR(data.VkPhysicalDevice, &displayModeProperties, displayModes );

        // PHYSICAL DEVICE PROPERTIES -------------------------------------------
        fixed (VkPhysicalDeviceProperties* phd =   &data.Physical.PhysicalDeviceProperties)
        {
            func.InstanceFunction.vkGetPhysicalDeviceProperties(data.Physical.VkPhysicalDevice ,phd );
        }

        // GET Physical FEATURES ----------------------------------------------------------

        fixed ( VkPhysicalDeviceFeatures* features = &data.Physical.Features)
        {
            func.InstanceFunction.vkGetPhysicalDeviceFeatures(data.Physical.VkPhysicalDevice,features );
        } 

    
        // DZEVICE  EXTENSIONS -------------------------------------------------
        uint propertyCount = 0;
        func.InstanceFunction.vkEnumerateDeviceExtensionProperties(data.Physical.VkPhysicalDevice, null, &propertyCount, null).Check();

        VkExtensionProperties* properties = stackalloc VkExtensionProperties[(int)propertyCount];  
        func.InstanceFunction.vkEnumerateDeviceExtensionProperties(data.Physical.VkPhysicalDevice, null, &propertyCount, properties).Check();

        data.Physical.DeviceExtensions = new string[propertyCount + 1];

        for (int i = 0; i < propertyCount; i++){
            var length =  Strings.StrHelper.Strlen( properties[i].extensionName);
           data.Physical.DeviceExtensions[i] = Encoding.UTF8.GetString( properties[i].extensionName, (int) length ); //new string(properties[i].extensionName); //
        }
        data.Physical.DeviceExtensions[propertyCount] = VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME ;

        // FOR SWAP CHAIN ----------------------------------------------------
        data.SwapChain.VkPresentMode = ChooseSwapPresentMode(ref data);
        data.SwapChain.VkSurfaceFormat  = ChooseSwapSurfaceFormat(ref data);
        data.SwapChain.VkSurfaceArea = ChooseSwapExtent(ref data);
        data.SwapChain.VkFormat = data.SwapChain.VkSurfaceFormat.format;

        data.SwapChain.ImageCount = data.Physical.Capabilities.minImageCount + 1;
        if (data.Physical.Capabilities.maxImageCount > 0 && data.SwapChain.ImageCount > data.Physical.Capabilities.maxImageCount) {
            data.SwapChain.ImageCount = data.Physical.Capabilities.maxImageCount;
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
            //Querying for WSI Support
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
        #if WIN64
        // vkGetPhysicalDeviceWin32PresentationSupportKHR
        #endif
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
        // if (VK.VK_KHR_get_surface_capabilities2 )
        // {
        //       VkPhysicalDeviceSurfaceInfo2KHR surfaceInfo2 = new();
        //         surfaceInfo2.pNext = null;
        //         surfaceInfo2.sType = VkStructureType.VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_SURFACE_INFO_2_KHR;
        //         surfaceInfo2.surface = surface;

        //     uint surfaceFormat2Count = 0;
        //    func.vkGetPhysicalDeviceSurfaceFormats2KHR(physicalDevice,&surfaceInfo2,&surfaceFormat2Count,null ).Check("vkGetPhysicalDeviceSurfaceFormats2KHR");

        //     ReadOnlySpan<VkSurfaceFormat2KHR> surfaceFormats2 = new VkSurfaceFormat2KHR[surfaceFormat2Count];
        //     fixed (VkSurfaceFormat2KHR* surfaceFormats2Ptr = surfaceFormats2)		{   
        //     func.vkGetPhysicalDeviceSurfaceFormats2KHR(physicalDevice,&surfaceInfo2,&surfaceFormat2Count,surfaceFormats2Ptr ).Check("vkGetPhysicalDeviceSurfaceFormats2KHR");
        //      }
        // }
        details.Formats = surfaceFormats;
       
        uint presentModeCount = 0;
        func.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, null).Check("vkGetPhysicalDeviceSurfacePresentModesKHR Count");

        ReadOnlySpan<VkPresentModeKHR> presentModes = new VkPresentModeKHR[presentModeCount];
        fixed (VkPresentModeKHR* presentModesPtr = presentModes)	{
            func.vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, presentModesPtr).Check("vkGetPhysicalDeviceSurfacePresentModesKHR List");
        }
        details.PresentModes = presentModes;
        return details;
    }

    private  static VkSurfaceFormatKHR ChooseSwapSurfaceFormat( ref GraphicDeviceData data)
    {
        foreach (VkSurfaceFormatKHR availableFormat in data.Physical.Formats)
        {
            if (availableFormat.format == VkFormat.VK_FORMAT_B8G8R8A8_SRGB && availableFormat.colorSpace == VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR )
            {
                return availableFormat;
            }
        }
        return data.Physical.Formats[0];
    }

    private static  VkExtent2D ChooseSwapExtent( ref GraphicDeviceData data) 
    {
        if ( data.Physical.Capabilities.currentExtent.width != uint.MaxValue /*std::numeric_limits<uint32_t>::max()*/) {
            return data.Physical.Capabilities.currentExtent;
        } else {
            VkExtent2D actualExtent =new() {
                width = ClampUInt( (uint)data.App.Width, data.Physical.Capabilities.minImageExtent.width, data.Physical.Capabilities.maxImageExtent.width),
                height = ClampUInt( (uint)data.App.Height, data.Physical.Capabilities.minImageExtent.height, data.Physical.Capabilities.maxImageExtent.height),
            };

            return actualExtent;
        }
    }

    private  static VkPresentModeKHR ChooseSwapPresentMode( ref GraphicDeviceData data )
    {
        foreach (VkPresentModeKHR availablePresentMode in data.Physical.PresentModes)
        {
            if (availablePresentMode == VkPresentModeKHR.VK_PRESENT_MODE_MAILBOX_KHR)
            {
                return availablePresentMode;
            }
        }

        return VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
    }

    private static uint ClampUInt(uint value, uint min, uint max) =>value < min ? min : value > max ? max : value;

    #endregion

    #region DEVICE
    
    private static unsafe void CreateLogicalDevice(ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        VkDeviceQueueCreateInfo* queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[2];

       uint[] uniqueQueueFamilies = new uint[]{
            data.Physical.VkGraphicFamilyIndice,
            data.Physical.VkPresentFamilyIndice
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

        // CREATE DEVICE INFO ---------------------------------------------------------
        using var deviceExtensions = new StrArrayUnsafe(ref data.Physical.DeviceExtensions);
        VkDeviceCreateInfo createInfo = default;
        createInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
        createInfo.queueCreateInfoCount = (uint)queueCount;
        createInfo.pQueueCreateInfos = queueCreateInfos;
        fixed ( VkPhysicalDeviceFeatures* features = &data.Physical.Features) {createInfo.pEnabledFeatures = features;}
        createInfo.enabledExtensionCount = (uint)deviceExtensions.Count;
        createInfo.ppEnabledExtensionNames = deviceExtensions;
        createInfo.pNext = null ;
        
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.App.ValidationLayers);
        if ( data.App.EnableDebug)
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
            func.InstanceFunction.vkCreateDevice(data.Physical.VkPhysicalDevice, &createInfo, null, device).Check("Error creation vkDevice");
        }
       
       VkHelper.ValidateExtensionsForLoad(ref data.Physical.DeviceExtensions,0 );

       func.InitDevicesFunctions( GraphicDeviceLoaderFunction.vkGetDeviceProcAddr , data.VkDevice);

       Log.Info($"Create Device :{data.VkDevice}");

        fixed(VkQueue* gq =&data.VkGraphicQueue)
        {
            func.DeviceFunction.vkGetDeviceQueue(data.VkDevice, data.Physical.VkGraphicFamilyIndice, 0,gq);
        }
        fixed(VkQueue* pq = &data.VkPresentQueue)
        {
            func.DeviceFunction.vkGetDeviceQueue(data.VkDevice, data.Physical.VkPresentFamilyIndice, 0, pq); 
        }

        Log.Info($"Graphic Queues : indice :{ data.Physical.VkGraphicFamilyIndice} Adr[{data.VkGraphicQueue}]\nPresent : indice :{data.Physical.VkPresentFamilyIndice} Adr[{data.VkPresentQueue}]");
    }

    private static unsafe void DisposeLogicalDevice(in GraphicInstanceFunction func,ref GraphicDeviceData data )
    {
        Log.Info("Dispose Logical Device ");
        if ( !data.VkDevice.IsNull){
            func.vkDestroyDevice(data.VkDevice, null);
        }  
    }


    #endregion

    #region SWAP CHAIN
    
    private static unsafe void CreateSwapChain(ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
       uint imageCount  = data.SwapChain.ImageCount;

        uint* queueFamilyIndices = stackalloc uint[2]{data.Physical.VkGraphicFamilyIndice, data.Physical.VkPresentFamilyIndice};

        VkSwapchainCreateInfoKHR createInfo = default;
            createInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR;
            createInfo.surface = data.App.VkSurface;
            createInfo.minImageCount = imageCount;
            createInfo.imageFormat =  data.SwapChain.VkSurfaceFormat.format;
            createInfo.imageColorSpace = data.SwapChain.VkSurfaceFormat.colorSpace;
            createInfo.imageExtent = data.SwapChain.VkSurfaceArea;
            createInfo.imageArrayLayers = 1;
            createInfo.imageUsage = (uint)VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT;
            
            if (data.Physical.VkGraphicFamilyIndice != data.Physical.VkPresentFamilyIndice) {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_CONCURRENT;
                createInfo.queueFamilyIndexCount = 2;
                createInfo.pQueueFamilyIndices = queueFamilyIndices;
            } else {
                createInfo.imageSharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;
            }
            
            createInfo.preTransform = data.Physical.Capabilities.currentTransform;
            createInfo.compositeAlpha = VkCompositeAlphaFlagBitsKHR.VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR;
            createInfo.presentMode = data.SwapChain.VkPresentMode;
            createInfo.clipped = VK.VK_TRUE;
            createInfo.oldSwapchain = VkSwapchainKHR.Null;

        fixed (VkSwapchainKHR* swapchainPtr = &data.SwapChain.VkSwapChain){
            func.vkCreateSwapchainKHR(data.VkDevice, &createInfo, null, swapchainPtr).Check("failed to create swap chain!");
        }

        Log.Info($"Create SwapChain {data.SwapChain.VkSwapChain}\nMode : {data.SwapChain.VkPresentMode}\nSize :[{data.SwapChain.VkSurfaceArea.width},{data.SwapChain.VkSurfaceArea.height}] ");

        // SWWAP CHAIN IMAGES ----------------------------------------------------------------------

        func.vkGetSwapchainImagesKHR(data.VkDevice, data.SwapChain.VkSwapChain, &imageCount, null);

        data.SwapChain.VkImages = new VkImage[imageCount];

        fixed (VkImage* swapchainImagesPtr = data.SwapChain.VkImages){
            func.vkGetSwapchainImagesKHR(data.VkDevice,data.SwapChain.VkSwapChain, &imageCount, swapchainImagesPtr).Check("vkGetSwapchainImagesKHR");
        }
        
        Log.Info($"Create {data.SwapChain.VkImages.Length} SwapChainImages ");
    }

    private static unsafe void CreateImageViews(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        uint size  = data.SwapChain.ImageCount;
        data.SwapChain.VkSwapChainImageViews = new VkImageView[size ];

        for (uint i = 0; i < size; i++)
        {
            VkImageViewCreateInfo imageViewCreateInfo= default;//New<VkImageViewCreateInfo>();
                imageViewCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO; //VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
                imageViewCreateInfo.image = data.SwapChain.VkImages[i];
                imageViewCreateInfo.viewType = VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;// VK_IMAGE_VIEW_TYPE_2D ;//VkImageViewType.VK_IMAGE_VIEW_TYPE_2D;// VK_IMAGE_VIEW_TYPE_2D;
                imageViewCreateInfo.format = data.SwapChain.VkFormat;

                imageViewCreateInfo.components.r = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                imageViewCreateInfo.components.g = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                imageViewCreateInfo.components.b = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                imageViewCreateInfo.components.a = VkComponentSwizzle.VK_COMPONENT_SWIZZLE_IDENTITY;
                
                imageViewCreateInfo.subresourceRange.aspectMask = (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
                imageViewCreateInfo.subresourceRange.baseMipLevel = 0;
                imageViewCreateInfo.subresourceRange.levelCount = 1;
                imageViewCreateInfo.subresourceRange.baseArrayLayer = 0;
                imageViewCreateInfo.subresourceRange.layerCount = 1;

            fixed(VkImageView* img =  &data.SwapChain.VkSwapChainImageViews[i]){
                func.vkCreateImageView(data.VkDevice, &imageViewCreateInfo, null, img).Check("failed to create image views!");
            }
            Log.Info($"\t -[{i}] {data.SwapChain.VkSwapChainImageViews[i]} : {data.SwapChain.VkFormat}"); 
        }
    }
    
    private static unsafe void DisposeSwapChain(in GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        if (!data.VkDevice.IsNull && data.SwapChain.VkSwapChainImageViews != null) {
            Log.Info($"Release Swap chain Images View [{data.SwapChain.VkSwapChainImageViews .Length}]");
            foreach (var imageView in data.SwapChain.VkSwapChainImageViews) {
                func.vkDestroyImageView(data.VkDevice, imageView, null); }
        }

        if ( !data.VkDevice.IsNull && !data.SwapChain.VkSwapChain.IsNull )
        {
           Log.Info($"Release Swapchain Instancew [{data.SwapChain.VkSwapChain}]");
            func.vkDestroySwapchainKHR(data.VkDevice, data.SwapChain.VkSwapChain, null);
        }
    }

    #endregion
    
    #region RenderPass

    private static unsafe void CreateRenderPass(ref GraphicDeviceFunctions func,ref GraphicDeviceData data) 
    {
        // COLOR 
        VkAttachmentDescription colorAttachment =default;// New<VkAttachmentDescription>(); //new(){
            colorAttachment.format = data.SwapChain.VkFormat;
            colorAttachment.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            colorAttachment.loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR;
            colorAttachment.storeOp = VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_STORE;
            colorAttachment.stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE;
            colorAttachment.stencilStoreOp =VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_DONT_CARE;
            colorAttachment.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            colorAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            colorAttachment.flags =0;
            colorAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;

        //ONLY IF DEPTH RESOURCE
          VkAttachmentDescription depthAttachment =default;// New<VkAttachmentDescription>(); //new(){
            depthAttachment.format = FindDepthFormat(func.InstanceFunction, data.Physical.VkPhysicalDevice);
            depthAttachment.samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            depthAttachment.loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR;
            depthAttachment.storeOp = VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_STORE;
            depthAttachment.stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE;
            depthAttachment.stencilStoreOp =VkAttachmentStoreOp. VK_ATTACHMENT_STORE_OP_DONT_CARE;
            depthAttachment.initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            depthAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            depthAttachment.flags =0;
            depthAttachment.finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;

        // POST PROCESSING       
         VkAttachmentReference colorAttachmentRef = default;// New<VkAttachmentReference>();//new() {
            colorAttachmentRef.attachment = 1;// TODO voir pourquoi 0 dans tutorial
            colorAttachmentRef.layout =VkImageLayout. VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;
            
         
         VkAttachmentReference depthAttachmentRef = default;// New<VkAttachmentReference>();//new() {
            depthAttachmentRef.attachment =1;
            depthAttachmentRef.layout =VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL;

        
        VkSubpassDescription subpass = default ;//New<VkSubpassDescription>(); //new(){
            subpass.pipelineBindPoint = VkPipelineBindPoint. VK_PIPELINE_BIND_POINT_GRAPHICS;
            subpass.colorAttachmentCount = 1;
            subpass.pColorAttachments = &colorAttachmentRef;
            subpass.pDepthStencilAttachment =&depthAttachmentRef;
            // subpass.flags =0;
            // subpass.inputAttachmentCount=0;
            // subpass.pDepthStencilAttachment = null;
            // subpass.pInputAttachments = null;
            // subpass.pPreserveAttachments = null;
            // subpass.preserveAttachmentCount=0;

        VkSubpassDependency dependency =default; //  New <VkSubpassDependency>();
            dependency.srcSubpass = VK.VK_SUBPASS_EXTERNAL;
            dependency.dstSubpass = 0;
            dependency.srcStageMask = (uint)VkPipelineStageFlagBits. VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT | (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT;
            dependency.srcAccessMask = 0;
            dependency.dstStageMask =(uint) VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT| (uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_EARLY_FRAGMENT_TESTS_BIT;
            dependency.dstAccessMask =(uint)VkAccessFlagBits. VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT| (uint)VkAccessFlagBits.VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT;
            dependency.dependencyFlags =0;

        //RENDER PASS 
        VkAttachmentDescription* attachments = stackalloc VkAttachmentDescription[] { colorAttachment, depthAttachment };

        VkRenderPassCreateInfo renderPassInfo = default;// New<VkRenderPassCreateInfo>();//new()
            renderPassInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO;
            renderPassInfo.attachmentCount = 2;
            renderPassInfo.pAttachments = attachments;
            renderPassInfo.subpassCount = 1;
            renderPassInfo.pSubpasses = &subpass;
            renderPassInfo.dependencyCount = 1;
            renderPassInfo.pDependencies = &dependency;
            renderPassInfo.flags =0;
            renderPassInfo.pNext =null;

        fixed( VkRenderPass* renderPass= &data.Render.VkRenderPass ){
            func.DeviceFunction.vkCreateRenderPass(data.VkDevice, &renderPassInfo, null, renderPass).Check("failed to create render pass!");
        }

        Log.Info($"Create Render Pass : {data.Render.VkRenderPass}");
    }

    private static unsafe void DisposeRenderPass(in GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        if (!data.VkDevice.IsNull && !data.Render.VkRenderPass.IsNull)
            func.vkDestroyRenderPass(data.VkDevice,data.Render.VkRenderPass,null);
    }

    #endregion

    #region  Descirptor Set

    private unsafe static void CreateDescriptorSetLayout(ref GraphicDeviceFunction func, ref GraphicDeviceData data  ) 
    {
        VkDescriptorSetLayoutBinding uboLayoutBinding = default;
            uboLayoutBinding.binding = 0;
            uboLayoutBinding.descriptorCount = 1;
            uboLayoutBinding.descriptorType = VkDescriptorType. VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
            uboLayoutBinding.pImmutableSamplers = null;
            uboLayoutBinding.stageFlags =(uint) VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
        
        VkDescriptorSetLayoutBinding samplerLayoutBinding =default;
        samplerLayoutBinding.binding = 1;
        samplerLayoutBinding.descriptorCount = 1;
        samplerLayoutBinding.descriptorType =  VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
        samplerLayoutBinding.pImmutableSamplers = null;
        samplerLayoutBinding.stageFlags = (uint) VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT;

        
        VkDescriptorSetLayoutBinding* bindings = stackalloc VkDescriptorSetLayoutBinding[2] {uboLayoutBinding, samplerLayoutBinding };
        VkDescriptorSetLayoutCreateInfo layoutInfo = default;
        layoutInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO;
        layoutInfo.bindingCount = 2;
        layoutInfo.pBindings = bindings;

        fixed( VkDescriptorSetLayout* layout = &data.DescriptorSetLayout ){
            func.vkCreateDescriptorSetLayout(data.VkDevice, &layoutInfo, null, layout).Check("failed to create descriptor set layout!");
        }
    }

    private unsafe static void CreateUniformBuffers(ref GraphicDeviceFunctions func, ref GraphicDeviceData data   ) 
    {
        uint taille =  (uint)(Marshal.SizeOf<Uniform_MVP>() );
        VkDeviceSize bufferSize = (uint)(sizeof(float) * 3 * 16 );

        // data.uniformBuffers = new VkBuffer[ data.MAX_FRAMES_IN_FLIGHT];
        // data.uniformBuffersMemory = new VkDeviceMemory[data.MAX_FRAMES_IN_FLIGHT];
        // data.uniformBuffersMapped = new nint[ data.MAX_FRAMES_IN_FLIGHT] ;
        
        for (int i = 0; i < data.MAX_FRAMES_IN_FLIGHT; i++) 
        {
            data.uniformBuffersMapped[i] =Marshal.AllocHGlobal( (int)bufferSize);

            CreateStagingBuffer(ref func, ref data, bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT,ref data.uniformBuffers[i],ref data.uniformBuffersMemory[i]);
            
            //TODO if void ** is oK ?
            fixed(void* p = &data.uniformBuffersMapped[i])
            {
                func.DeviceFunction.vkMapMemory(data.VkDevice,data.uniformBuffersMemory[i], 0, bufferSize, 0, &p).Check("Map Memeory Unifommr pb");
            }
            
        }   
    }

    private unsafe static void DisposeUniformBuffers(in GraphicDeviceFunction func, ref GraphicDeviceData data ) 
    {
        for (nint i = 0; i < data.MAX_FRAMES_IN_FLIGHT; i++) 
        {
            func.vkDestroyBuffer(data.VkDevice, data.uniformBuffers[i], null);
            func.vkFreeMemory(data.VkDevice, data.uniformBuffersMemory[i], null);
        }
    }

    public unsafe static void UpdateUniformBuffer(in GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        // nint ptr = nint.Zero;
        // Marshal.StructureToPtr<Uniform_MVP>( data.ubo,ptr,false );
        int taille = (int)Marshal.SizeOf<Uniform_MVP>();
        int size = sizeof(float) * 16 *3 ;
        nint uiform =   data.uniformBuffersMapped[CurrentFrame];
        void* ubo = data.ubo.AddressOfPtrThis();

        Unsafe.CopyBlock(uiform.ToPointer() , ubo ,(uint)size);
        
    }

    #endregion
  
    #region  FrameBuffer

    private static unsafe void CreateFramebuffers( ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        int size= data.SwapChain.VkSwapChainImageViews.Length;
        data.SwapChain.VkFramebuffers = new VkFramebuffer[size];
        // create frameubuufer array in data max 6  3 swapchain (triple bufer x 2 for stereoscopic// data.VkFramebuffers = new VkFramebuffer[size] ;// swapChainFramebuffers.resize(swapChainImageViews.size());

        for (int i = 0; i < size; i++)
        {
            VkImageView[] attachments = new[]{ data.SwapChain.VkSwapChainImageViews[i] };

            VkFramebufferCreateInfo framebufferInfo = default;
                framebufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO;
                framebufferInfo.renderPass = data.Render.VkRenderPass;
                framebufferInfo.attachmentCount = (uint)attachments.Length ;
                fixed( VkImageView* attachmentPtr =&attachments[0] ) {
                    framebufferInfo.pAttachments = attachmentPtr; 
                }
                framebufferInfo.width = data.SwapChain.VkSurfaceArea.width;
                framebufferInfo.height = data.SwapChain.VkSurfaceArea.height;
                framebufferInfo.layers = 1;

            fixed( VkFramebuffer* frame = &data.SwapChain.VkFramebuffers[i]) {
                func.vkCreateFramebuffer(data.VkDevice, &framebufferInfo, null, frame).Check("failed to create framebuffer!"); 
            }
        }
        Log.Info($"Create  FrameBuffer ");
    }

    private unsafe static void DisposeFrameBuffer(in GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
        if (data.SwapChain.VkFramebuffers != null)
        {
            foreach (var framebuffer in  data.SwapChain.VkFramebuffers) {
                if( !framebuffer.IsNull)
                    func.vkDestroyFramebuffer(data.VkDevice, framebuffer, null); 
            }
        }
    }

    #endregion

    #region DEpth Buffering 
    private unsafe static void CreateDepthResources(ref GraphicDeviceFunctions func, ref GraphicDeviceData data)
    {
        VkFormat depthFormat = FindDepthFormat(func.InstanceFunction , data.Physical.VkPhysicalDevice);

        CreateImage(ref func , ref data, ref data.DepthImage , ref data.DepthImageMemory, (uint)data.SwapChain.VkSurfaceArea.width, (uint)data.SwapChain.VkSurfaceArea.height, 
            depthFormat, 
            VkImageTiling.VK_IMAGE_TILING_OPTIMAL, 
            VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT   );
        
        VkImageViewCreateInfo viewInfo = default;
        viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        viewInfo.image = data.DepthImage;
        viewInfo.viewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        viewInfo.format = VkFormat. VK_FORMAT_R8G8B8A8_SRGB;
        viewInfo.subresourceRange.aspectMask =  (uint)VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT;
        viewInfo.subresourceRange.baseMipLevel = 0;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.baseArrayLayer = 0;
        viewInfo.subresourceRange.layerCount = 1;

        fixed (VkImageView* imageView = &data.DepthImageView ){
            func.DeviceFunction.vkCreateImageView(data.VkDevice, &viewInfo, null, imageView).Check("failed to create image view!");
        }
    }
    
    private static unsafe  VkFormat FindSupportedFormat(in GraphicInstanceFunction func, in VkPhysicalDevice physicalDevice,  VkFormat[] candidates, VkImageTiling tiling, VkFormatFeatureFlagBits features) 
    {

        foreach ( VkFormat format in candidates)
        {
        //Get PhysicalFormatProperties --------------------------------------------------------
        VkFormatProperties formatProperties;
        func.vkGetPhysicalDeviceFormatProperties(physicalDevice,format, &formatProperties);

            if (tiling == VkImageTiling.VK_IMAGE_TILING_LINEAR && (formatProperties.linearTilingFeatures & (uint)features) == (uint)features) 
            {
                return format;
            } 
            else if (tiling == VkImageTiling.VK_IMAGE_TILING_OPTIMAL && (formatProperties.optimalTilingFeatures & (uint)features) == (uint)features) 
            {
                return format;
            }
        }
        return VkFormat.VK_FORMAT_A8B8G8R8_SRGB_PACK32;
        //  throw std::runtime_error("failed to find supported format!");
    }
    
    private static unsafe VkFormat FindDepthFormat( in GraphicInstanceFunction func, in VkPhysicalDevice physicalDevice ) 
    {
        return FindSupportedFormat(func , physicalDevice,
            new VkFormat[] {VkFormat.VK_FORMAT_D32_SFLOAT, VkFormat.VK_FORMAT_D32_SFLOAT_S8_UINT, VkFormat.VK_FORMAT_D24_UNORM_S8_UINT},
            VkImageTiling.VK_IMAGE_TILING_OPTIMAL,
             VkFormatFeatureFlagBits.VK_FORMAT_FEATURE_DEPTH_STENCIL_ATTACHMENT_BIT
        );
    }

    private unsafe static void DisposeDepthResources(in GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        if ( data.DepthImageView != VkImageView.Null )
        {
            func.vkDestroyImageView(data.VkDevice,data.DepthImageView, null);
        }
        if(data.DepthImage != VkImage.Null )
        {
            func.vkDestroyImage(data.VkDevice, data.DepthImage, null);
        }
        if ( data.DepthImageMemory != VkDeviceMemory.Null)
        {
            func.vkFreeMemory(data.VkDevice, data.DepthImageMemory, null);
        }
        
    }

    #endregion
 
    #region COMMAND BUFFERS 

    private static unsafe void CreateCommandPool(ref GraphicDeviceFunction func,ref GraphicDeviceData data  ) 
    {
        
        VkCommandPoolCreateInfo poolInfo = default;
            poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;//VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
            poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;// VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
            poolInfo.queueFamilyIndex =data.Physical.VkGraphicFamilyIndice;

        fixed( VkCommandPool* pool =  &data.VkCommandPool){
            func.vkCreateCommandPool(data.VkDevice, &poolInfo, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool With : {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");
    }

    private static unsafe void CreateCommandBuffer(ref GraphicDeviceFunction func,ref GraphicDeviceData data ) 
    {
        // data.VkCommandBuffers = new VkCommandBuffer[pipeline.MAX_FRAMES_IN_FLIGHT]; 

        VkCommandBufferAllocateInfo allocInfo =default;// New<VkCommandBufferAllocateInfo>();
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
            allocInfo.commandPool = data.VkCommandPool;
            allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
            allocInfo.commandBufferCount = (uint)data.VkCommandBuffers.Length;
        
        fixed(VkCommandBuffer* commandBuffer = &data.VkCommandBuffers[0] ){
            func.vkAllocateCommandBuffers(data.VkDevice, &allocInfo, commandBuffer ).Check("failed to allocate command buffers!"); 
        }

        Log.Info($"Create Allocate Command buffer count : {data.MAX_FRAMES_IN_FLIGHT}");
    }

    private unsafe static void DisposeCommandPool(in GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        if (!data.VkDevice.IsNull && !data.VkCommandPool .IsNull){
            func.vkDestroyCommandPool(data.VkDevice, data.VkCommandPool , null);
        }
    }
   
    #endregion

    #region Vertex & Index
 
    private unsafe static void CreateVertexBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data  ) 
    {
        VkDeviceSize bufferSize = (uint)( Position3f_Color3f_UV2f.Stride * data.Vertices.Length);

        VkBuffer stagingBuffer = new();
        VkDeviceMemory stagingBufferMemory = new();
        
        CreateStagingBuffer(ref func, ref data,bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT 
            | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer, 
            ref stagingBufferMemory);
        
        void* vertexdata;

        func.DeviceFunction.vkMapMemory(data.VkDevice, stagingBufferMemory, 0, bufferSize, 0, &vertexdata);

        fixed (void* p = &data.Vertices[0]){ 
            // memcpy(vertexdata, p,  bufferSize); 
            Unsafe.CopyBlock(vertexdata , p ,(uint)bufferSize);    
        }
        
        func.DeviceFunction.vkUnmapMemory(data.VkDevice, stagingBufferMemory);

        CreateStagingBuffer(ref func, ref data,bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT 
            | VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, 
            ref  data.VertexBuffer, 
            ref data.VertexBufferMemory);

        CopyStagingBuffer(ref func.DeviceFunction, ref data,stagingBuffer, data.VertexBuffer, bufferSize);

        func.DeviceFunction.vkDestroyBuffer(data.VkDevice, stagingBuffer, null);
        func.DeviceFunction.vkFreeMemory(data.VkDevice, stagingBufferMemory, null);
    }

    private unsafe static void CreateIndexBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data   ) 
    {
        VkDeviceSize bufferSize = (uint)(sizeof(short) * data.Indices.Length);

        VkBuffer stagingBuffer = new();
        VkDeviceMemory stagingBufferMemory = new();

        CreateStagingBuffer(ref func , ref data , bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT , 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT 
            | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer, 
            ref stagingBufferMemory);
        
        void* indicesdata;
        func.DeviceFunction.vkMapMemory(data.VkDevice, stagingBufferMemory, 0, bufferSize, 0, &indicesdata);
        fixed (void* p = &data.Indices[0])
        {
            //  memcpy(indicesdata, p,  bufferSize); 
             Unsafe.CopyBlock( indicesdata , p ,(uint)bufferSize);
        }  
        func.DeviceFunction.vkUnmapMemory(data.VkDevice, stagingBufferMemory);

        CreateStagingBuffer(ref func , ref data , bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT 
            | VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, 
            ref data.IndicesBuffer, 
            ref data.IndicesBufferMemory);

        CopyStagingBuffer(ref func.DeviceFunction , ref data ,stagingBuffer, data.IndicesBuffer, bufferSize);

        func.DeviceFunction.vkDestroyBuffer(data.VkDevice, stagingBuffer, null);
        func.DeviceFunction.vkFreeMemory(data.VkDevice, stagingBufferMemory, null);
    }

    private unsafe static void DisposeBuffers(in GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        if( !data.IndicesBuffer.IsNull)
        {
            func.vkDestroyBuffer(data.VkDevice, data.IndicesBuffer, null);
        }
        if( !data.IndicesBufferMemory.IsNull)
        {
            func.vkFreeMemory(data.VkDevice, data.IndicesBufferMemory, null);
        }
        if( !data.VertexBuffer.IsNull)
        {
            func.vkDestroyBuffer(data.VkDevice, data.VertexBuffer, null);
        }
        if( !data.VertexBufferMemory.IsNull)
        {
            func.vkFreeMemory(data.VkDevice, data.VertexBufferMemory, null);
        }
        
    }

    private unsafe static void CreateStagingBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data ,VkDeviceSize size, VkBufferUsageFlagBits usage, 
        VkMemoryPropertyFlagBits  properties,ref VkBuffer buffer,ref VkDeviceMemory bufferMemory) 
    {
        VkBufferCreateInfo bufferInfo =new();
        bufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
        bufferInfo.size = size;
        bufferInfo.usage = (uint)usage;
        bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

        fixed(VkBuffer* buf =  &buffer){
            func.DeviceFunction.vkCreateBuffer(data.VkDevice, &bufferInfo, null, buf).Check("failed to create vertex buffer!");
        }
        
        VkMemoryRequirements memRequirements = new();
        func.DeviceFunction.vkGetBufferMemoryRequirements(data.VkDevice, buffer, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
            allocInfo.allocationSize = memRequirements.size;
            allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data,memRequirements.memoryTypeBits, properties);
            allocInfo.pNext = null;

        fixed(VkDeviceMemory* memory =  &bufferMemory) {
            func.DeviceFunction.vkAllocateMemory(data.VkDevice, &allocInfo, null, memory).Check("failed to allocate vertex buffer memory!");
        }
        
        func.DeviceFunction.vkBindBufferMemory(data.VkDevice, buffer, bufferMemory, 0);
    }

    private unsafe static void CopyStagingBuffer(ref GraphicDeviceFunction func, ref GraphicDeviceData data, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size)
    {
        VkCommandBufferAllocateInfo allocInfo = new();
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
            allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
            allocInfo.commandPool = data.VkCommandPool;
            allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer;
        func.vkAllocateCommandBuffers(data.VkDevice, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
            beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
            beginInfo.flags = (uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        func.vkBeginCommandBuffer(commandBuffer, &beginInfo);

            VkBufferCopy copyRegion = new();
            copyRegion.size = size;
            func.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);

        func.vkEndCommandBuffer(commandBuffer);

        VkSubmitInfo submitInfo = new();
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers = &commandBuffer;

        func.vkQueueSubmit(data.VkGraphicQueue, 1, &submitInfo, VkFence.Null);
        func.vkQueueWaitIdle(data.VkGraphicQueue);

        func.vkFreeCommandBuffers(data.VkDevice, data.VkCommandPool, 1, &commandBuffer);
    }

    // private unsafe static void memcpy(void* a, void* b, ulong size)
	// {
	// 	var ap = (byte*)a;
	// 	var bp = (byte*)b;
	// 	for (ulong i = 0; i < size; ++i)
	// 		*ap++ = *bp++;
	// }

    public unsafe static uint FindMemoryType(ref GraphicDeviceFunctions func, ref GraphicDeviceData data , uint memoryTypeBits, VkMemoryPropertyFlagBits properties)
    {
        VkPhysicalDeviceMemoryProperties memoryProperties = new();
            // memProperties.
        func.InstanceFunction.vkGetPhysicalDeviceMemoryProperties(data.Physical.VkPhysicalDevice, &memoryProperties);

        uint count = memoryProperties.memoryTypeCount;
        for (uint i = 0; i < count; i++)
        {
            if ( (memoryTypeBits & 1) == 1 && (memoryProperties.memoryTypes[(int)i].propertyFlags & (uint)properties) == (uint)properties)
            {
                return i;
            }
            memoryTypeBits >>= 1;
        }

        return uint.MaxValue;
    }

    // [Obsolete]
    //   private unsafe static void CreateVertexBufferWithoutStaging(ref GraphicDeviceFunction func, ref GraphicDeviceData data ,ref GraphicRenderConfig pipeline ) 
    // {
        
    //     VkBufferCreateInfo bufferInfo = default;
    //         bufferInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
    //         bufferInfo.size = (uint)(Marshal.SizeOf<Position2f_Color3f>() * data.Vertices.Length);
    //         bufferInfo.usage = (uint)VkBufferUsageFlagBits. VK_BUFFER_USAGE_VERTEX_BUFFER_BIT;
    //         bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

    //     fixed(VkBuffer* buffer = &data.VertexBuffer) {
    //         func.vkCreateBuffer( data.VkDevice, &bufferInfo, null, buffer).Check("failed to create vertex buffer!");
    //     }
        
    //     VkMemoryRequirements memRequirements = new();
    //     func.vkGetBufferMemoryRequirements(data.VkDevice, data.VertexBuffer, &memRequirements);


    //     uint memoryTypeIndexForCoherentVisibleBit = FindMemoryType(  ref func , ref data , memRequirements.memoryTypeBits ,
    //         (VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT));
        
    //     VkMemoryAllocateInfo allocInfo = default;
    //         allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
    //         allocInfo.allocationSize = memRequirements.size;
    //         allocInfo.memoryTypeIndex = memoryTypeIndexForCoherentVisibleBit;
    //         allocInfo.pNext = null;

    //     fixed(VkDeviceMemory* memory =  &data.VertexBufferMemory) {
    //         func.vkAllocateMemory(data.VkDevice, &allocInfo, null, memory).Check("failed to allocate vertex buffer memory!");
    //     }
        
    //     func.vkBindBufferMemory(data.VkDevice,  data.VertexBuffer,data.VertexBufferMemory, 0);

    //     void* vertexdata;

    //     func.vkMapMemory(data.VkDevice,data.VertexBufferMemory, 0, bufferInfo.size, 0, &vertexdata);
        
    //     fixed (void* p = &data.Vertices[0]){ 
    //         memcpy(vertexdata, p,  bufferInfo.size); 
    //     }  
    //     func.vkUnmapMemory(data.VkDevice, data.VertexBufferMemory);
    // }

    #endregion
  
    #region Desciptor pool descriptor set
    private static unsafe void CreateDescriptorPool(ref GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        VkDescriptorPoolSize* poolSizes = stackalloc VkDescriptorPoolSize[2];

        poolSizes[0].type =VkDescriptorType. VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
        poolSizes[0].descriptorCount = (uint)(data.MAX_FRAMES_IN_FLIGHT);
        poolSizes[1].type = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
        poolSizes[1].descriptorCount =(uint)(data.MAX_FRAMES_IN_FLIGHT);

        VkDescriptorPoolCreateInfo poolInfo= default;
            poolInfo.sType =VkStructureType. VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO;
            poolInfo.poolSizeCount = 2;
            poolInfo.pPoolSizes = poolSizes;
            poolInfo.maxSets = (uint)(data.MAX_FRAMES_IN_FLIGHT);

        fixed(VkDescriptorPool* pool =  &data.DescriptorPool){
            func.vkCreateDescriptorPool(data.VkDevice, &poolInfo, null,pool ).Check("failed to create descriptor pool!");
        }
    }

    private static unsafe void CreateDescriptorSets(ref GraphicDeviceFunction func, ref GraphicDeviceData data) 
    {
        // int value = data.MAX_FRAMES_IN_FLIGHT;
        VkDescriptorSetLayout* layouts  =  stackalloc VkDescriptorSetLayout[2] { data.DescriptorSetLayout,data.DescriptorSetLayout };

        VkDescriptorSetAllocateInfo allocInfo = default;
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO;
            allocInfo.descriptorPool = data.DescriptorPool;
            allocInfo.descriptorSetCount = (uint)(data.MAX_FRAMES_IN_FLIGHT);
            allocInfo.pSetLayouts = layouts;
            allocInfo.pNext = null;

        // data.DescriptorSets = new VkDescriptorSet[ data.MAX_FRAMES_IN_FLIGHT];

        fixed(VkDescriptorSet* descriptor =&data.DescriptorSets[0]  )
        {
            func.vkAllocateDescriptorSets(data.VkDevice, &allocInfo, descriptor).Check("failed to allocate descriptor sets!");
        }
        VkWriteDescriptorSet* descriptorWrites = stackalloc VkWriteDescriptorSet[2];

        for (int i = 0; i <  data.MAX_FRAMES_IN_FLIGHT; i++) {
            VkDescriptorBufferInfo bufferInfo = default;
            bufferInfo.buffer = data.uniformBuffers[i];
            bufferInfo.offset = 0;
            bufferInfo.range = (uint)Marshal.SizeOf<Uniform_MVP>();

            VkDescriptorImageInfo imageInfo =default;
            imageInfo.imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            imageInfo.imageView = data.TextureImageView;
            imageInfo.sampler = data.TextureSampler;

            descriptorWrites[0].sType = VkStructureType. VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
            descriptorWrites[0].dstSet = data.DescriptorSets[i];
            descriptorWrites[0].dstBinding = 0;
            descriptorWrites[0].dstArrayElement = 0;
            descriptorWrites[0].descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
            descriptorWrites[0].descriptorCount = 1;
            descriptorWrites[0].pBufferInfo = &bufferInfo;

            descriptorWrites[1].sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
            descriptorWrites[1].dstSet = data.DescriptorSets[i];
            descriptorWrites[1].dstBinding = 1;
            descriptorWrites[1].dstArrayElement = 0;
            descriptorWrites[1].descriptorType =VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
            descriptorWrites[1].descriptorCount = 1;
            descriptorWrites[1].pImageInfo = &imageInfo;

            func.vkUpdateDescriptorSets(data.VkDevice, 2, descriptorWrites, 0, null);
        }
    }

    private static unsafe void DisposeDescriptorPool(in GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        if ( !data.DescriptorPool.IsNull)
        {
            func.vkDestroyDescriptorPool(data.VkDevice, data.DescriptorPool, null);
        }
        
        if ( !data.DescriptorSetLayout.IsNull)
        {
            func.vkDestroyDescriptorSetLayout(data.VkDevice, data.DescriptorSetLayout, null);
        }
        
    }
    #endregion

    #region Texture

    private unsafe static void CreateTextureImage(ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        uint texWidth=512, texHeight=512;
        uint texChannels=4;

        // stbi_uc* pixels = stbi_load("textures/texture.jpg", &texWidth, &texHeight, &texChannels, STBI_rgb_alpha);
        var file = File.ReadAllBytes( data.TextureName);
        ImageResult result = ImageResult.FromMemory(file );
        texWidth = (uint)result.Width;
        texHeight = (uint)result.Height;
        texChannels = (uint)result.Comp;

        VkDeviceSize imageSize = (ulong)(texWidth * texHeight * texChannels);

        // if (!pixels) {         throw std::runtime_error("failed to load texture image!");        }

        VkBuffer stagingBuffer = VkBuffer.Null;
        VkDeviceMemory stagingBufferMemory = VkDeviceMemory.Null;
        CreateStagingBuffer(ref func , ref data , imageSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer, 
            ref stagingBufferMemory);

        
        void* ptr;
        func.DeviceFunction.vkMapMemory(data.VkDevice, stagingBufferMemory, 0, imageSize, 0, &ptr);
        fixed (void* p = &result.Data[0] )
        { 
            // memcpy(ptr, p,  imageSize ); 
             Unsafe.CopyBlock( ptr , p ,(uint)imageSize);
        }  
        func.DeviceFunction.vkUnmapMemory(data.VkDevice, stagingBufferMemory);

        // // stbi_image_free(pixels);
        file = null!;
        result.Data = null!;

        CreateImage(ref func , ref data, ref data.TextureImage, ref data.TextureImageMemory, texWidth, texHeight, 
        VkFormat.VK_FORMAT_R8G8B8A8_SRGB, 
        VkImageTiling.VK_IMAGE_TILING_OPTIMAL, 
        VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT, 
        VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT   );

        TransitionImageLayout(ref func.DeviceFunction , ref data,  VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            CopyBufferToImage(ref  func.DeviceFunction,ref  data,stagingBuffer, (texWidth), (texHeight));
        TransitionImageLayout(ref func.DeviceFunction , ref data, VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

        func.DeviceFunction.vkDestroyBuffer(data.VkDevice, stagingBuffer, null);
        func.DeviceFunction.vkFreeMemory(data.VkDevice, stagingBufferMemory, null);
    }

    private unsafe static void DisposeTextureImage(in GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        if( !data.TextureImage.IsNull)
        {
            func.vkDestroyImage(data.VkDevice, data.TextureImage, null);
        }
        if( !data.TextureImageMemory.IsNull)
        {
            func.vkFreeMemory(data.VkDevice, data.TextureImageMemory, null);
        }
        
    }
    private unsafe static void CreateImage(ref GraphicDeviceFunctions func,ref GraphicDeviceData data, ref VkImage image,ref VkDeviceMemory imageMemory,  uint width, uint height, VkFormat format, VkImageTiling tiling,
         VkImageUsageFlagBits usage, VkMemoryPropertyFlagBits properties) 
    {
        VkImageCreateInfo imageInfo = new();
            imageInfo.sType =VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
            imageInfo.imageType =VkImageType. VK_IMAGE_TYPE_2D;
            imageInfo.extent.width = width;
            imageInfo.extent.height = height;
            imageInfo.extent.depth = 1;
            imageInfo.mipLevels = 1;
            imageInfo.arrayLayers = 1;
            imageInfo.format = format;
            imageInfo.tiling = tiling;
            imageInfo.initialLayout =VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            imageInfo.usage = (uint)usage;
            imageInfo.samples =VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT;
            imageInfo.sharingMode = VkSharingMode. VK_SHARING_MODE_EXCLUSIVE;

        fixed( VkImage* img = &image) {
            func.DeviceFunction.vkCreateImage(data.VkDevice, &imageInfo, null,img).Check("failed to create image!");
        }
        
        VkMemoryRequirements memRequirements;
        func.DeviceFunction.vkGetImageMemoryRequirements(data.VkDevice, image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = default;
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data, memRequirements.memoryTypeBits, properties);

        fixed (VkDeviceMemory* imgMem = &imageMemory  ){
            func.DeviceFunction.vkAllocateMemory(data.VkDevice, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        }

        func.DeviceFunction.vkBindImageMemory( data.VkDevice, image,imageMemory, 0).Check("Bind Image Memory");
    }

    private unsafe static void TransitionImageLayout(ref GraphicDeviceFunction func,ref GraphicDeviceData data, VkFormat format, VkImageLayout oldLayout, VkImageLayout newLayout) 
    {
        VkCommandBuffer commandBuffer = BeginSingleTimeCommands(ref func , ref data);

        VkImageMemoryBarrier barrier = default;
            barrier.sType =VkStructureType. VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
            barrier.oldLayout = oldLayout;
            barrier.newLayout = newLayout;
            barrier.srcQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
            barrier.dstQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
            barrier.image = data.TextureImage;
            barrier.subresourceRange.aspectMask =(uint) VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
            barrier.subresourceRange.baseMipLevel = 0;
            barrier.subresourceRange.levelCount = 1;
            barrier.subresourceRange.baseArrayLayer = 0;
            barrier.subresourceRange.layerCount = 1;

        VkPipelineStageFlagBits sourceStage=VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
        VkPipelineStageFlagBits destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;

        if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED && newLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL) {
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;

            sourceStage = VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
            destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
        } else if (oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL && newLayout ==VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL) {
            barrier.srcAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_SHADER_READ_BIT;

            sourceStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
            destinationStage = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
        } else {
            Guard.ThrowWhenConditionIsTrue(true,"unsupported layout transition!");
        }

        func.vkCmdPipelineBarrier(
            commandBuffer,
            (uint)sourceStage, (uint)destinationStage,
            0,
            0, null,
            0, null,
            1, &barrier
        );

        EndSingleTimeCommands(ref func , ref data ,commandBuffer);
    }

    private unsafe static void CopyBufferToImage(ref GraphicDeviceFunction func,ref GraphicDeviceData data,VkBuffer buffer,  uint width, uint height) 
    {
        VkCommandBuffer commandBuffer = BeginSingleTimeCommands(ref func , ref data);

        VkBufferImageCopy region = default;
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

        func.vkCmdCopyBufferToImage(commandBuffer, buffer, data.TextureImage, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &region);

        EndSingleTimeCommands(ref func , ref data, commandBuffer);
    }

    private unsafe static VkCommandBuffer BeginSingleTimeCommands(ref GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        
        VkCommandBufferAllocateInfo allocInfo = default;
            allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
            allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
            allocInfo.commandPool = data.VkCommandPool;
            allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer = VkCommandBuffer.Null;

        func.vkAllocateCommandBuffers(data.VkDevice, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
            beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
            beginInfo.flags =(uint) VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        func.vkBeginCommandBuffer(commandBuffer, &beginInfo);

        return commandBuffer;
    }

    private unsafe static void EndSingleTimeCommands(ref GraphicDeviceFunction func,ref GraphicDeviceData data, VkCommandBuffer commandBuffer) 
    {
        func.vkEndCommandBuffer(commandBuffer);
       
        VkSubmitInfo submitInfo = default;
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers =&commandBuffer;
        
        func.vkQueueSubmit(data.VkGraphicQueue, 1, &submitInfo, VkFence.Null);
        func.vkQueueWaitIdle(data.VkGraphicQueue);

        func.vkFreeCommandBuffers(data.VkDevice, data.VkCommandPool, 1, &commandBuffer);
       
    }

    #endregion

    #region SAMPLER
    private unsafe static void CreateTextureSampler(ref GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        
        VkSamplerCreateInfo samplerInfo = default;
        samplerInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
        samplerInfo.magFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.minFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.anisotropyEnable = VK.VK_TRUE;
        samplerInfo.maxAnisotropy = data.Physical.PhysicalDeviceProperties.limits.maxSamplerAnisotropy;
        samplerInfo.borderColor = VkBorderColor . VK_BORDER_COLOR_INT_OPAQUE_BLACK;
        samplerInfo.unnormalizedCoordinates = VK.VK_FALSE;
        samplerInfo.compareEnable = VK.VK_FALSE;
        samplerInfo.compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS;
        samplerInfo.mipmapMode = VkSamplerMipmapMode .VK_SAMPLER_MIPMAP_MODE_LINEAR;

        fixed(VkSampler* sampler  = &data.TextureSampler){
            func.vkCreateSampler(data.VkDevice, &samplerInfo, null, sampler).Check("failed to create texture sampler!");
        }        
    }

    private unsafe static void CreateTextureImageView(ref GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
         VkImageViewCreateInfo viewInfo = default;
        viewInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO;
        viewInfo.image = data.TextureImage;
        viewInfo.viewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        viewInfo.format = VkFormat. VK_FORMAT_R8G8B8A8_SRGB;
        viewInfo.subresourceRange.aspectMask =  (uint)VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT;
        viewInfo.subresourceRange.baseMipLevel = 0;
        viewInfo.subresourceRange.levelCount = 1;
        viewInfo.subresourceRange.baseArrayLayer = 0;
        viewInfo.subresourceRange.layerCount = 1;

        fixed (VkImageView* imageView = &data.TextureImageView ){
            func.vkCreateImageView(data.VkDevice, &viewInfo, null, imageView).Check("failed to create image view!");
        }
       
    }

  

    private unsafe static void DisposeTextureSampler(in GraphicDeviceFunction func, ref GraphicDeviceData data)
    {
        if( !data.TextureSampler.IsNull)
        {
            func.vkDestroySampler(data.VkDevice,data.TextureSampler, null);
        }
        if( !data.TextureImageView.IsNull)
        {
            func.vkDestroyImageView(data.VkDevice, data.TextureImageView, null);
        }
    }

    #endregion

    #region Mipmaping
// //29_mipmapping.cpp
// // generateMipmaps(textureImage, VK_FORMAT_R8G8B8A8_SRGB, texWidth, texHeight, mipLevels);
//     }

//     private void generateMipmaps(VkImage image, VkFormat imageFormat, int32_t texWidth, int32_t texHeight, uint32_t mipLevels) 
//      {
//         // Check if image format supports linear blitting
//         // VkFormatProperties formatProperties;
//         // vkGetPhysicalDeviceFormatProperties(physicalDevice, imageFormat, &formatProperties);

//         // if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT)) {
//         //     throw std::runtime_error("texture image format does not support linear blitting!");
//         // }

//         // VkCommandBuffer commandBuffer = beginSingleTimeCommands();

//         // VkImageMemoryBarrier barrier{};
//         // barrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
//         // barrier.image = image;
//         // barrier.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED;
//         // barrier.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED;
//         // barrier.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//         // barrier.subresourceRange.baseArrayLayer = 0;
//         // barrier.subresourceRange.layerCount = 1;
//         // barrier.subresourceRange.levelCount = 1;

//         // int32_t mipWidth = texWidth;
//         // int32_t mipHeight = texHeight;

//         // for (uint32_t i = 1; i < mipLevels; i++) {
//         //     barrier.subresourceRange.baseMipLevel = i - 1;
//         //     barrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
//         //     barrier.newLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
//         //     barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
//         //     barrier.dstAccessMask = VK_ACCESS_TRANSFER_READ_BIT;

//         //     vkCmdPipelineBarrier(commandBuffer,
//         //         VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, 0,
//         //         0, nullptr,
//         //         0, nullptr,
//         //         1, &barrier);

//         //     VkImageBlit blit{};
//         //     blit.srcOffsets[0] = {0, 0, 0};
//         //     blit.srcOffsets[1] = {mipWidth, mipHeight, 1};
//         //     blit.srcSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//         //     blit.srcSubresource.mipLevel = i - 1;
//         //     blit.srcSubresource.baseArrayLayer = 0;
//         //     blit.srcSubresource.layerCount = 1;
//         //     blit.dstOffsets[0] = {0, 0, 0};
//         //     blit.dstOffsets[1] = { mipWidth > 1 ? mipWidth / 2 : 1, mipHeight > 1 ? mipHeight / 2 : 1, 1 };
//         //     blit.dstSubresource.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
//         //     blit.dstSubresource.mipLevel = i;
//         //     blit.dstSubresource.baseArrayLayer = 0;
//         //     blit.dstSubresource.layerCount = 1;

//         //     vkCmdBlitImage(commandBuffer,
//         //         image, VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL,
//         //         image, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
//         //         1, &blit,
//         //         VK_FILTER_LINEAR);

//         //     barrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL;
//         //     barrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
//         //     barrier.srcAccessMask = VK_ACCESS_TRANSFER_READ_BIT;
//         //     barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

//         //     vkCmdPipelineBarrier(commandBuffer,
//         //         VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0,
//         //         0, nullptr,
//         //         0, nullptr,
//         //         1, &barrier);

//         //     if (mipWidth > 1) mipWidth /= 2;
//         //     if (mipHeight > 1) mipHeight /= 2;
//         // }

//         // barrier.subresourceRange.baseMipLevel = mipLevels - 1;
//         // barrier.oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL;
//         // barrier.newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
//         // barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
//         // barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;

//         // vkCmdPipelineBarrier(commandBuffer,
//         //     VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, 0,
//         //     0, nullptr,
//         //     0, nullptr,
//         //     1, &barrier);

//         // endSingleTimeCommands(commandBuffer);
//     }
    
    #endregion

    #region Synchronisation & cache control (  Fence = memory barrier )
    private static unsafe void CreateSyncObjects(ref GraphicDeviceFunction func,ref GraphicDeviceData data   )
    {

        // data.ImageAvailableSemaphores = new VkSemaphore[data.MAX_FRAMES_IN_FLIGHT]; // imageAvailableSemaphores = //.resize(MAX_FRAMES_IN_FLIGHT);
        // data.RenderFinishedSemaphores = new VkSemaphore[data.MAX_FRAMES_IN_FLIGHT];// renderFinishedSemaphores.resize(MAX_FRAMES_IN_FLIGHT);
        // data.InFlightFences = new VkFence[data.MAX_FRAMES_IN_FLIGHT];// inFlightFences.resize(MAX_FRAMES_IN_FLIGHT);

        VkSemaphoreCreateInfo semaphoreInfo =default;// New<VkSemaphoreCreateInfo>();
            semaphoreInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO;
            semaphoreInfo.flags =0;
            semaphoreInfo.pNext =null;

        VkFenceCreateInfo fenceInfo= default;//New<VkFenceCreateInfo>();
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

    private static unsafe void DisposeSyncObjects(in GraphicDeviceFunction func,ref GraphicDeviceData data)
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

    #region Pipeline

    #region FIXED FUNCTIONS
    private unsafe static void Pipeline_CreateShaderModule( out VkShaderModule shader , ref GraphicDeviceFunction func, ref GraphicDeviceData data , string shaderfragmentfileSPV)
    {
        ReadOnlySpan<byte> span = File.ReadAllBytes( shaderfragmentfileSPV ).AsSpan();
         
        VkShaderModuleCreateInfo createInfoFrag =default;
            createInfoFrag.sType= VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
            createInfoFrag.codeSize=(int)span.Length;
            createInfoFrag.pCode= (uint*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
            // createInfoFrag.pNext = null;
            // createInfoFrag.flags =0;

        VkShaderModule fragShaderModule = VkShaderModule.Null;
        func.vkCreateShaderModule(data.VkDevice, &createInfoFrag, null,  &fragShaderModule ).Check($"Create Fragment Shader Module ; {shaderfragmentfileSPV}"); 
        shader = fragShaderModule;
    }

    private unsafe static void CreatePipelineLayout(ref GraphicDeviceFunction func,ref GraphicDeviceData data  )
    {
         //UNFORM MVP 
        VkPipelineLayoutCreateInfo pipelineLayoutInfo=default;
            pipelineLayoutInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO;
            pipelineLayoutInfo.pPushConstantRanges = null; // Optionnel
            pipelineLayoutInfo.flags =0;
            pipelineLayoutInfo.pNext =null;
        if ( data.DescriptorSetLayout.IsNull)
        {
            pipelineLayoutInfo.setLayoutCount = 0;            // Optionnel
            pipelineLayoutInfo.pushConstantRangeCount = 0;    // Optionnel
        }
        else
        {
            pipelineLayoutInfo.setLayoutCount = 1;            // Optionnel
            fixed (VkDescriptorSetLayout* layout = &data.DescriptorSetLayout )
            {pipelineLayoutInfo.pSetLayouts = layout;}         // Optionnel
            pipelineLayoutInfo.pushConstantRangeCount = 0;    // Optionnel
        }
            
        fixed( VkPipelineLayout* layout = &data.VkpipelineLayout )
        {
            func.vkCreatePipelineLayout(data.VkDevice, &pipelineLayoutInfo, null, layout).Check ("failed to create pipeline layout!");
        }
    }

    #endregion


    private static unsafe void CreatePipeline(ref GraphicDeviceFunction func,ref GraphicDeviceData data )
    {
        #region SHADERS

        Pipeline_CreateShaderModule( out VkShaderModule  vertShaderModule , ref func, ref data , data.VertexShaderFileNameSPV);
        Pipeline_CreateShaderModule( out VkShaderModule  fragShaderModule , ref func, ref data , data.FragmentShaderFileNameSPV);
        using RitaEngine.Base.Strings.StrUnsafe fragentryPoint = new(data.FragmentEntryPoint);
        using RitaEngine.Base.Strings.StrUnsafe vertentryPoint = new(data.VertexEntryPoint);
       
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

        VkPipelineVertexInputStateCreateInfo vertexInputInfo =default;
        vertexInputInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;

  
        VkVertexInputBindingDescription bindingDescription =new();// GetBindingDescription( ref bindingDescription);
            bindingDescription.binding = 0;
            bindingDescription.stride =(uint) Position3f_Color3f_UV2f.Stride;
            bindingDescription.inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_VERTEX;

        VkVertexInputAttributeDescription* attributeDescriptions = stackalloc VkVertexInputAttributeDescription[3] ; // GetAttributeDescriptions(  attributeDescriptions);
            attributeDescriptions[0].binding = 0;
            attributeDescriptions[0].location = 0;
            attributeDescriptions[0].format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
            attributeDescriptions[0].offset = (uint)Position3f_Color3f_UV2f.OffsetPosition;// (uint)Marshal.OffsetOf( typeof(Position2f_Color3f), "Position" ) ;; //offsetof(Vertex, pos);

            attributeDescriptions[1].binding = 0;
            attributeDescriptions[1].location = 1;
            attributeDescriptions[1].format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
            attributeDescriptions[1].offset =   (uint)Position3f_Color3f_UV2f.OffsetColor;//(uint)Marshal.OffsetOf( typeof(Position2f_Color3f), "Color" ) ;//2;//offsetof(Vertex, color);
            
            attributeDescriptions[2].binding = 0;
            attributeDescriptions[2].location = 2;
            attributeDescriptions[2].format = VkFormat.VK_FORMAT_R32G32_SFLOAT;
            attributeDescriptions[2].offset =   (uint)Position3f_Color3f_UV2f.OffsetUV; //(uint)Marshal.OffsetOf( typeof(Position2f_Color3f), "Color" ) ;//2;//offsetof(Vertex, color);
            
            vertexInputInfo.vertexBindingDescriptionCount = 1;
            vertexInputInfo.vertexAttributeDescriptionCount = 3;
            vertexInputInfo.pNext = null;
            vertexInputInfo.flags =0;
            vertexInputInfo.pVertexAttributeDescriptions=attributeDescriptions;
            vertexInputInfo.pVertexBindingDescriptions=&bindingDescription;

        #endregion

        #region INPUT ASSEMBLY
        VkPipelineInputAssemblyStateCreateInfo inputAssembly=default;
        inputAssembly.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
        inputAssembly.topology = VkPrimitiveTopology. VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
        inputAssembly.primitiveRestartEnable = VK.VK_FALSE;
        inputAssembly.flags =0;
        inputAssembly.pNext =null;       
        #endregion

        #region COLOR BLENDING
         //COLOR BLENDING
        VkPipelineColorBlendAttachmentState colorBlendAttachment =default;//New<VkPipelineColorBlendAttachmentState>(); //default;//new ();// New<VkPipelineColorBlendAttachmentState>();
            colorBlendAttachment.colorWriteMask = (uint)(VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT);
            colorBlendAttachment.blendEnable = VK.VK_FALSE;
            // colorBlendAttachment.srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
            // colorBlendAttachment.srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
            // colorBlendAttachment.alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD;
            // colorBlendAttachment.colorBlendOp =  VkBlendOp.VK_BLEND_OP_ADD;
            // colorBlendAttachment.dstAlphaBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
            // colorBlendAttachment.dstColorBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        
        VkPipelineColorBlendStateCreateInfo colorBlending=default;//New<VkPipelineColorBlendStateCreateInfo>();// default;//new ();//New<VkPipelineColorBlendStateCreateInfo>();
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
            data.Viewport.width = (float) data.SwapChain.VkSurfaceArea.width;
            data.Viewport.height = (float) data.SwapChain.VkSurfaceArea.height;
            data.Viewport.minDepth = 0.0f;
            data.Viewport.maxDepth = 1.0f;

            VkOffset2D offset = default;
                offset.x = 0;
                offset.y = 0;

            data.Scissor.offset = offset;
            data.Scissor.extent = data.SwapChain.VkSurfaceArea;

        VkPipelineViewportStateCreateInfo viewportState =default;
            viewportState.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO;
            viewportState.viewportCount = 1;
            fixed( VkViewport* viewport = &data.Viewport){    viewportState.pViewports =viewport;    }
            viewportState.scissorCount = 1;
            fixed( VkRect2D* scissor = &data.Scissor ){  viewportState.pScissors = scissor;          }
            viewportState.flags=0;
            viewportState.pNext = null;
        #endregion

        #region  RASTERIZATION
        VkPipelineRasterizationStateCreateInfo rasterizer =default;
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
        VkPipelineMultisampleStateCreateInfo multisampling=default;
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
        VkPipelineTessellationStateCreateInfo tessellationStateCreateInfo = default;
            tessellationStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_TESSELLATION_STATE_CREATE_INFO;

        VkPipelineDepthStencilStateCreateInfo depthStencilStateCreateInfo = default;
            depthStencilStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
            // depthStencilStateCreateInfo.depthTestEnable =  VK.VK_TRUE;
            // depthStencilStateCreateInfo.depthWriteEnable = VK.VK_TRUE;
            // depthStencilStateCreateInfo.depthCompareOp = VkCompareOp.VK_COMPARE_OP_LESS;
            // depthStencilStateCreateInfo.depthBoundsTestEnable = VK.VK_FALSE;
            // depthStencilStateCreateInfo.stencilTestEnable = VK.VK_FALSE;
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

        VkGraphicsPipelineCreateInfo pipelineInfo =default;
            pipelineInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO;
            pipelineInfo.flags = (uint)VkPipelineCreateFlagBits.VK_PIPELINE_CREATE_DISABLE_OPTIMIZATION_BIT ;

            pipelineInfo.renderPass = data.Render.VkRenderPass;
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

    private static unsafe void RecordCommandBuffer(in GraphicDeviceFunction func,ref GraphicDeviceData data, in VkCommandBuffer commandBuffer, uint imageIndex)
    {
         //----------Début de l'enregistrement des commandes--------------------------------------------------       
        func.vkResetCommandBuffer(commandBuffer, /*VkCommandBufferResetFlagBits*/ 0);

       //BEGIN COMMAND BUFFER
        VkCommandBufferBeginInfo beginInfo =default;
            beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO; //VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
            beginInfo.pNext =null;
            beginInfo.flags =(uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT;
            beginInfo.pInheritanceInfo= null;
        
        func.vkBeginCommandBuffer(commandBuffer, &beginInfo).Check("Failed to Begin command buffer");

        //BEGIN RENDER PASS
         VkRenderPassBeginInfo renderPassInfo = default;
            renderPassInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO; //VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO;
            renderPassInfo.renderPass = data.Render.VkRenderPass;
            renderPassInfo.framebuffer = data.SwapChain.VkFramebuffers[imageIndex];
            renderPassInfo.renderArea.offset = data.RenderAreaOffset;
            renderPassInfo.renderArea.extent = data.SwapChain.VkSurfaceArea;
            renderPassInfo.clearValueCount = 1;
            // renderPassInfo.pClearValues =  data.ClearColor;//
            fixed(VkClearValue* clearColor = &data.ClearColor ) {  renderPassInfo.pClearValues =  clearColor; }
        func.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo,VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
       
        func.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.VkGraphicsPipeline);

        // SET DYNAMIC STATES
        fixed(VkViewport* viewport = &data.Viewport ){ func.vkCmdSetViewport(commandBuffer, 0, 1,viewport);  }
        fixed( VkRect2D* scissor = &data.Scissor){ func.vkCmdSetScissor(commandBuffer, 0, 1, scissor); }
        //--------------------------------------------------------------------------------------
       
        // if ( data.IndicesBuffer != VkBuffer.Null )
        // {
            VkBuffer* vertexBuffers = stackalloc VkBuffer[] { data.VertexBuffer};
            VkDeviceSize* offsets = stackalloc VkDeviceSize[]{0};
            func.vkCmdBindVertexBuffers(commandBuffer, 0, 1, vertexBuffers, offsets);

            func.vkCmdBindIndexBuffer(commandBuffer, data.IndicesBuffer, 0, VkIndexType.VK_INDEX_TYPE_UINT16);

            fixed(VkDescriptorSet* desc =  &data.DescriptorSets[CurrentFrame] ){
                func.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.VkpipelineLayout, 0, 1, desc, 0, null);
            }
            

            func.vkCmdDrawIndexed(commandBuffer, data.IndicesSize, 1, 0, 0, 0);

     

        func.vkCmdEndRenderPass(commandBuffer);
        
        func.vkEndCommandBuffer(commandBuffer).Check("Failed to End command buffer ");
    }
    private static int CurrentFrame =0;

    // [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static unsafe  void DrawPipeline(ref GraphicDeviceFunctions func, ref GraphicDeviceData data )
    {
        uint imageIndex=0;
        VkFence CurrentinFlightFence = data.InFlightFences[/*data.*/CurrentFrame];
        VkSemaphore CurrentImageAvailableSemaphore =  data.ImageAvailableSemaphores[/*data.*/CurrentFrame];
        VkSemaphore CurrentRenderFinishedSemaphore = data.RenderFinishedSemaphores[/*data.*/CurrentFrame];
        VkSemaphore* waitSemaphores = stackalloc VkSemaphore[1] {CurrentImageAvailableSemaphore};// VkSemaphore[] waitSemaphores = {CurrentImageAvailableSemaphore};
        UInt32* waitStages  = stackalloc UInt32[1]{(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT }; //*VkPipelineStageFlags*/UInt32[] waitStages = {(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT};
        VkSemaphore*   signalSemaphores  = stackalloc VkSemaphore[1] {CurrentRenderFinishedSemaphore} ;// VkSemaphore[] signalSemaphores = {CurrentRenderFinishedSemaphore};
        VkSwapchainKHR* swapChains = stackalloc  VkSwapchainKHR[1]{ data.SwapChain.VkSwapChain };// VkSwapchainKHR[] swapChains = { data.VkSwapChain };
        VkCommandBuffer commandBuffer =data.VkCommandBuffers[/*data.*/CurrentFrame];

        func.DeviceFunction.vkWaitForFences(data.VkDevice, 1,&CurrentinFlightFence, VK.VK_TRUE, data.tick_timeout).Check("Acquire Image");

        VkResult result = func.DeviceFunction.vkAcquireNextImageKHR(data.VkDevice, data.SwapChain.VkSwapChain, data.tick_timeout,CurrentImageAvailableSemaphore, VkFence.Null, &imageIndex);

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
        {
            ReCreateSwapChain( ref func,ref data);
            
            return ;
        }
        else if (result != VkResult.VK_SUCCESS && result != VkResult.VK_SUBOPTIMAL_KHR )
        {
            throw new Exception("Failed to acquire swap chain Images");
        }

        // UpdateUniformBuffer(ref func,ref  data);

        func.DeviceFunction.vkResetFences(data.VkDevice, 1, &CurrentinFlightFence);

        //----------Début de l'enregistrement des commandes--------------------------------------------------   
        RecordCommandBuffer(  func.DeviceFunction,ref data, in  commandBuffer, imageIndex);
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
        
        func.DeviceFunction.vkQueueSubmit(data.VkGraphicQueue, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
        
        VkPresentInfoKHR presentInfo=default;
            presentInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR; //VK_STRUCTURE_TYPE_PRESENT_INFO_KHR;
            presentInfo.waitSemaphoreCount = 1;
            presentInfo.pWaitSemaphores = signalSemaphores;//fixed( VkSemaphore* signalSemaphoresPtr =&signalSemaphores[0]){ presentInfo.pWaitSemaphores = signalSemaphoresPtr;}
            presentInfo.pImageIndices = &imageIndex;
            presentInfo.swapchainCount = 1;
            presentInfo.pSwapchains = swapChains;//fixed( VkSwapchainKHR* swapChainsPtr = &swapChains[0]){ presentInfo.pSwapchains = swapChainsPtr; }
        
        result = func.DeviceFunction.vkQueuePresentKHR(data.VkPresentQueue, &presentInfo); //.Check("Present Image");  

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR || result == VkResult.VK_SUBOPTIMAL_KHR )
        {
           ReCreateSwapChain( ref func,ref data);
        }
        else if (result != VkResult.VK_SUCCESS )
        {
            throw new Exception("Failed to  present swap chain Images");
        }
       /*data.*/CurrentFrame = ((/*data.*/CurrentFrame + 1) % data.MAX_FRAMES_IN_FLIGHT);   
    }

    private static unsafe void DisposePipeline(in GraphicDeviceFunction func,ref GraphicDeviceData data)
    {
        if( !data.VkDevice.IsNull && !data.VkpipelineLayout.IsNull){
            func.vkDestroyPipelineLayout(data.VkDevice, data.VkpipelineLayout, null);
        }        
        if (!data.VkDevice.IsNull && !data.VkGraphicsPipeline.IsNull){
            func.vkDestroyPipeline(data.VkDevice,data.VkGraphicsPipeline, null);
        }
    }

    #endregion

}