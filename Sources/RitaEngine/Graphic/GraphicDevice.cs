namespace RitaEngine.Graphic;

using System.IO;
using RitaEngine.Base.Debug;
using RitaEngine.Math.Color;
using RitaEngine.API.Vulkan;
using RitaEngine.Platform;
using RitaEngine.Base.Strings;
using RitaEngine.Math;
using RitaEngine.Base;

using VkDeviceSize = UInt64;
using RitaEngine.Resources.Images;
using System.Collections.Generic;
using RitaEngine.Graphic;

/// <summary>
/// 
/// </summary>
[SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack =  BaseHelper.FORCE_ALIGNEMENT)]
public struct GraphicDevice : IEquatable<GraphicDevice>
{
    public static string VersionToString( uint version ) => $"{VK.VK_VERSION_MAJOR(version)}.{VK.VK_VERSION_MINOR(version)}.{VK.VK_VERSION_PATCH(version)} ";
    
    private GraphicDeviceFunctions _functions = new();
    private GraphicDeviceData _data =new(); // inside => Instance Device Render Infos 

    public GraphicDevice() { }
   
    public unsafe void Init(in PlatformConfig config, in Window window )
    {
        GraphicDeviceImplement.TransfertToData(in config,in window ,ref _functions , ref _data);   
        // APP  : WSI , DEBUG , INSTANCE        
        GraphicDeviceImplement.CreateInstanceAndDebug(ref _functions,ref _data);
        GraphicDeviceImplement.CreateSurface(ref _functions , ref _data);
        // //DEVICE          
        GraphicDeviceImplement.SelectPhysicalDevice(ref _functions ,ref _data);
        GraphicDeviceImplement.GetPhysicalInformations( ref _functions, ref _data);
        GraphicDeviceImplement.CreateLogicalDevice(ref _functions ,ref _data );
        // //SWAP CHAIN
        GraphicDeviceImplement.CreateSwapChain( ref _functions , ref _data);
        GraphicDeviceImplement.CreateImageViews( ref _functions , ref _data );
    }

    public void BuildRender(in GraphicRenderConfig config)
    {
        GraphicDeviceImplement.TransfertToRender(in config, in _functions , ref _data);
        //GraphicDeviceImplement.BuildRender(ref _functions, ref _data);
        GraphicDeviceImplement.CreateRenderPass(ref _functions,ref _data);
        GraphicDeviceImplement.CreateDepthResources( ref _functions, ref _data);//need for framebuffer
        GraphicDeviceImplement.CreateCommandPool(ref _functions,ref _data);
        GraphicDeviceImplement.CreateCommandBuffer(ref _functions,ref _data);
        //LoadModel => Do in TransfertConfigRenderTo_data load vertex indeices and textures ....
        GraphicDeviceImplement.CreateVertexBuffer( ref _functions , ref _data );
        GraphicDeviceImplement.CreateIndexBuffer( ref _functions , ref _data);
        GraphicDeviceImplement.CreateTextureImage(ref _functions , ref _data);
        GraphicDeviceImplement.CreateTextureImageView(ref _functions , ref _data);
            // _data to transfert at shader
        GraphicDeviceImplement.CreateTextureSampler(ref _functions , ref _data);
        GraphicDeviceImplement.CreateUniformBuffers(ref _functions , ref _data );
        GraphicDeviceImplement.CreateDescriptorSetLayout(ref _functions , ref _data);
        GraphicDeviceImplement.CreateDescriptorPool(ref _functions,ref _data);
        GraphicDeviceImplement.CreateDescriptorSets(ref _functions,ref _data);
        // need to complete descriptor before layout
        GraphicDeviceImplement.CreatePipelineLayout( ref _functions , ref _data);
        GraphicDeviceImplement.CreatePipeline(ref _functions, ref _data , in config);
        GraphicDeviceImplement.CreateFramebuffers(ref _functions,ref _data);
        GraphicDeviceImplement.CreateSyncObjects(ref _functions , ref _data);
    }

    public unsafe void Release()
    {  
        Log.Info("Dispose Graphic Device");

        // GraphicDeviceImplement.DisposeForReCreateSwapChain( _functions , ref __data);
        Pause();
        GraphicDeviceImplement.DisposeDepthResources(  _functions , ref _data);
        GraphicDeviceImplement.DisposeFrameBuffer(_functions, ref _data);
        GraphicDeviceImplement.DisposeSwapChain(_functions , ref _data);
        // GraphicDeviceImplement.DisposeBuildRender(_functions, ref _data);
        GraphicDeviceImplement.DisposePipeline( _functions,ref _data);
        GraphicDeviceImplement.DisposeRenderPass( _functions,ref _data);
        GraphicDeviceImplement.DisposeUniformBuffers( _functions , ref _data);
        GraphicDeviceImplement.DisposeTextureSampler( _functions , ref _data);
        GraphicDeviceImplement.DisposeTextureImage( _functions , ref _data);
        GraphicDeviceImplement.DisposeDescriptorPool( _functions , ref _data);
        GraphicDeviceImplement.DisposeBuffers( _functions , ref _data);
        GraphicDeviceImplement.DisposeSyncObjects( _functions ,ref _data);
        GraphicDeviceImplement.DisposeCommandPool( _functions,ref _data);
        // GraphicDeviceImplement.DisposeInstance(_functions, ref _data);
        GraphicDeviceImplement.DisposeLogicalDevice(in _functions ,ref _data);
        GraphicDeviceImplement.DisposeSurface(_functions, ref _data);
        GraphicDeviceImplement.DisposeInstanceAndDebug(in _functions,ref _data);  
        _data.Release();
        _functions.Release();
    }

    public void Pause() =>  GraphicDeviceImplement.Pause( _functions , ref _data);

    public void UpdateRender(in GraphicRenderConfig config)
    {    
        //TODO => for each Mesh _data.Info.UniformBufferArray = config.Camera.ClipToArray(ModelsWorld);Avec : float[0] = Models[0] -- float[16] = Models[15]
        _data.Info.UniformBufferArray = config.Camera.ToArray;
        _data.Info.PushConstants = config.Mesh;
    }

    public void DrawRender(in GraphicRenderConfig config)
        => GraphicDeviceImplement.DrawPipeline(ref _functions, ref _data);

    #region OVERRIDE    
    public override string ToString() => string.Format($"Data GraphicDevice " );
    public override int GetHashCode() => HashCode.Combine( _functions.GetHashCode(), _data.GetHashCode());
    public override bool Equals(object? obj) => obj is GraphicDevice  window && this.Equals(window) ;
    public bool Equals(GraphicDevice other)=>  _data.Equals(other._data) ;
    public static bool operator ==(GraphicDevice  left,GraphicDevice right) => left.Equals(right);
    public static bool operator !=(GraphicDevice  left,GraphicDevice right) => !left.Equals(right);
    #endregion
}

public static partial class GraphicDeviceImplement
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

  

    public static unsafe void TransfertToData(in PlatformConfig config, in Window window ,ref GraphicDeviceFunctions functions , ref GraphicDeviceData data)
    {
        //check if Exist vulkandllname? 
        functions.InitLoaderFunctions( config.GraphicDevice.LibraryName_Vulkan);
        data.Info.EnableDebug = config.GraphicDevice.EnableDebugMode;
        data.Info.GameName  = window.GetWindowName();
        data.Info.Handle = window.GetWindowHandle();
        data.Info.HInstance = window.GetWindowHInstance();
        data.Info.Width = window.GetWindowWidth();
        data.Info.Height = window.GetWindowheight();
        data.Handles.GetFrameBufferCallback = window.GetFrameBuffer;
    }

    public static void TransfertToRender(in GraphicRenderConfig pipeline, in GraphicDeviceFunctions functions, ref GraphicDeviceData data)
    {
        // pipeline.Camera.AddLookAkCamera(new( 0.0f,-0.12f,-2.0f),new(0.0f, 45.0f, 00.0f), new(0.0f,1.0f,0.0f), 45.0f,( 1280/720) , 0.1f, 100.0f);
        data.Info.UniformBufferArray =  pipeline.Camera.ToArray; 
        data.Info.RenderAreaOffset.x =0;
        data.Info.RenderAreaOffset.y =0;       
        data.Info.ClearColor = new(ColorHelper.PaletteToRGBA( pipeline.BackColorARGB));
        data.Info.ClearColor2 = new(depth:1.0f,stencil:0);
        
        data.Info.Indices = pipeline.Primitive.IndicesToArray();
        int sizeVertices = pipeline.Primitive.Vertices.Length  ;
        data.Info.Vertices = pipeline.Primitive.VertexToArray();
        data.Handles.IndicesSize =(uint) pipeline.Primitive.Indices.Length;

        data.Info.TextureName =  RitaEngine.Platform.PlatformHelper.AssetsPath + pipeline.TextureName;
        data.Info.MAX_FRAMES_IN_FLIGHT = pipeline.MAX_FRAMES_IN_FLIGHT;
        data.Info.FragmentEntryPoint = pipeline.FragmentEntryPoint;
        data.Info.VertexEntryPoint = pipeline.VertexEntryPoint;
        data.Info.FragmentShaderFileNameSPV =RitaEngine.Platform.PlatformHelper.AssetsPath + pipeline.FragmentShaderFileNameSPV;
        data.Info.VertexShaderFileNameSPV =RitaEngine.Platform.PlatformHelper.AssetsPath + pipeline.VertexShaderFileNameSPV;

        data.Info.RenderArea.extent = data.Info.VkSurfaceArea;
        data.Info.RenderArea.offset = data.Info.RenderAreaOffset;
        data.Info.PushConstants = pipeline.Mesh ;
    }   

    #region INSTANCE & DEBUG

    public unsafe static void CreateInstanceAndDebug(ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        // VALIDATION LAYER  --------------------------------------------------------------------
        uint layerCount = 0;
        func.vkEnumerateInstanceLayerProperties(&layerCount, null).Check("Enumerate instance Layer count");
        Guard.ThrowWhenConditionIsTrue( layerCount ==0 );

        VkLayerProperties* layerProperties = stackalloc VkLayerProperties[(int)layerCount];// ReadOnlySpan<VkLayerProperties> pp = stackalloc VkLayerProperties[(int)count];
        func.vkEnumerateInstanceLayerProperties(&layerCount, layerProperties).Check("Enumerate instance Layer list");

       data.Info.ValidationLayers = new  string[ layerCount ];
        for (int i = 0; i < layerCount; i++) {
            var length = StrHelper.Strlen( layerProperties[i].layerName );
           data.Info.ValidationLayers[i] = Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );// new string(layerProperties[i].layerName); //Encoding.UTF8.GetString(  layerProperties[i].layerName, (int) length );
        }

        //-- VULKAN API VERSION ------------------------------------------------------------------
        fixed ( uint* ver = &data.Info.Version)
        {
            func.vkEnumerateInstanceVersion(ver).Check("Enumerate Instance Version");
        }

        //--INSTANCE EXTENSIONS ------------------------------------------------------------------
        uint extCount = 0;
        func.vkEnumerateInstanceExtensionProperties(null, &extCount, null).Check( "Enumerate Extension Name Count");
        Guard.ThrowWhenConditionIsTrue( extCount == 0);

        VkExtensionProperties* props = stackalloc VkExtensionProperties[(int)extCount];
        func.vkEnumerateInstanceExtensionProperties(null, &extCount, props).Check( "Enumerate Extension Name List");

        data.Info.InstanceExtensions = new string[extCount ];
        for (int i = 0; i < extCount; i++)
        {
            var length = StrHelper.Strlen( props[i].extensionName);
           data.Info.InstanceExtensions[i] =Encoding.UTF8.GetString(  props[i].extensionName, (int) length );
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
        appInfo.apiVersion =data.Info.Version; 
        appInfo.applicationVersion = VK.VK_MAKE_VERSION(1,0,0);
        appInfo.engineVersion= VK.VK_MAKE_VERSION(1,0,0);
        appInfo.pNext =null;
        fixed(byte* ptr = &EngineName[0] ,  app = &data.Info.GameName[0] )
        {
            appInfo.pApplicationName =app;
            appInfo.pEngineName = ptr;
        }
        //CREATE INSTANCE  INFO   ------------------------------------------------
        using var extNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.Info.InstanceExtensions) ;
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.Info.ValidationLayers);

        VkInstanceCreateInfo instanceCreateInfo = new();
        instanceCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
        instanceCreateInfo.flags =  (uint)VkInstanceCreateFlagBits.VK_INSTANCE_CREATE_ENUMERATE_PORTABILITY_BIT_KHR;       
        instanceCreateInfo.pApplicationInfo =&appInfo;
        instanceCreateInfo.pNext= !data.Info.EnableDebug ? null :  (VkDebugUtilsMessengerCreateInfoEXT*) &debugCreateInfo;
        instanceCreateInfo.ppEnabledExtensionNames = extNames;
        instanceCreateInfo.enabledExtensionCount =extNames.Count;
        instanceCreateInfo.enabledLayerCount = data.Info.EnableDebug ?layerNames.Count : 0;
        instanceCreateInfo.ppEnabledLayerNames = data.Info.EnableDebug ? layerNames : null;

        fixed( VkInstance* instance = &data.Handles.Instance)
        {
            func.vkCreateInstance(&instanceCreateInfo, null, instance).Check("failed to create instance!");
        };

        Log.Info($"Create Debug {data.Handles.Instance}");

        VK.VK_KHR_swapchain=true;// //Special dont understand pour chage swapchain car nvidia n'a pas l'extension presente
        VkHelper.ValidateExtensionsForLoad(ref data.Info.InstanceExtensions,0 );

        func.InitInstancesFunctions( GraphicDeviceFunctions.vkGetInstanceProcAddr ,data.Handles.Instance );

        // CREATE DEBUG ------------------------------------------------------------------------
        if ( !data.Info.EnableDebug  )return ;
        
        fixed(VkDebugUtilsMessengerEXT* dbg = &data.Handles.DebugMessenger )
        {
            func.vkCreateDebugUtilsMessengerEXT(data.Handles.Instance, &debugCreateInfo, null, dbg).Check("failed to set up debug messenger!");
        }
        Log.Info($"Create Debug {data.Handles.DebugMessenger}");
    }

    public unsafe static void DisposeInstanceAndDebug(in GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        if (!data.Handles.Instance.IsNull && !data.Handles.DebugMessenger.IsNull){
            Log.Info($"Release DebugMessenger [{data.Handles.DebugMessenger}]");
            func.vkDestroyDebugUtilsMessengerEXT(data.Handles.Instance,data.Handles.DebugMessenger,null);
        } 

        if ( !data.Handles.Instance.IsNull){
            Log.Info($"Release Instance [{data.Handles.Instance}]");
            func.vkDestroyInstance(data.Handles.Instance, null);
            data.Handles.Instance = VkInstance.Null;
        }
    }
  
    [UnmanagedCallersOnly] 
    private unsafe static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity, 
        uint/*VkDebugUtilsMessageTypeFlagsEXT*/ messageTypes,
        VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* pUserData)
    {

        string message= System.Text.Encoding.UTF8.GetString(pCallbackData->pMessage,(int) StrHelper.Strlen(pCallbackData->pMessage) );  //new string(pCallbackData->pMessage); //
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
    
    public static unsafe void CreateSurface( ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        #if WIN64
        VkWin32SurfaceCreateInfoKHR sci = new() ;
        sci .hinstance = data.Info.HInstance;
        sci .hwnd = data.Info.Handle;
        sci .pNext = null;
        sci .flags = 0;
        sci .sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;

        fixed ( VkSurfaceKHR* surf = &data.Handles.Surface)
        {
            func.vkCreateWin32SurfaceKHR(data.Handles.Instance,&sci,null, surf).Check("Create Surface");
        }
        Log.Info($"Create Surface {data.Handles.Surface}");
        #endif
    }
    
    public static unsafe void DisposeSurface(in GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        if ( !data.Handles.Instance.IsNull && !data.Handles.Surface.IsNull)
        {
            Log.Info($"Release Surface [{data.Handles.Surface}]");
            func.vkDestroySurfaceKHR(data.Handles.Instance,data.Handles.Surface,null);
        }
    }

    #endregion
    
    #region PHYSICAL DEVICE 

    public static unsafe void SelectPhysicalDevice( ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        uint deviceCount = 0;
        func.vkEnumeratePhysicalDevices(data.Handles.Instance, &deviceCount, null).Check("EnumeratePhysicalDevices Count");
        Guard.ThrowWhenConditionIsTrue(deviceCount == 0,"Vulkan: Failed to find GPUs with Vulkan support");

        VkPhysicalDevice* devices = stackalloc VkPhysicalDevice[(int)deviceCount];
        func.vkEnumeratePhysicalDevices(data.Handles.Instance, &deviceCount, devices).Check("EnumeratePhysicalDevices List");

        for (int i = 0; i < (int)deviceCount; i++)
        {
            VkPhysicalDevice physicalDevice = devices[i];

            if ( IsDeviceSuitable(ref func,physicalDevice, data.Handles.Surface) )
            {
                data.Handles.PhysicalDevice = physicalDevice;
                break;
            }
        }
        Guard.ThrowWhenConditionIsTrue( data.Handles.PhysicalDevice.IsNull , "Physical device is null ");

        Log.Info($"Select Physical device {data.Handles.PhysicalDevice}");
    }

    public static unsafe void GetPhysicalInformations( ref GraphicDeviceFunctions func,ref GraphicDeviceData data)
    {
        SwapChainSupportDetails swap = QuerySwapChainSupport( ref func , data.Handles.PhysicalDevice , data.Handles.Surface  );
        data.Info.Capabilities = swap.Capabilities;
        data.Info.PresentModes = swap.PresentModes.ToArray();
        data.Info.Formats = swap.Formats.ToArray(); 

        (data.Info.VkGraphicFamilyIndice,data.Info.VkPresentFamilyIndice) = FindQueueFamilies(ref func , data.Handles.PhysicalDevice,data.Handles.Surface);
        
        // //FOR INFO .... bool in GraphicDeviceSettings.WithInfo       if ( !settings.Instance.GetDeviceInfo ) return;
         //Display enumeration
        // uint physicalDeviceDisplayPropertiesCount =0;
               // func.vkGetPhysicalDeviceDisplayPropertiesKHR(data.Handles.PhysicalDevice, &physicalDeviceDisplayPropertiesCount, null );
        // Guard.IsEmpty(physicalDeviceDisplayPropertiesCount);
        // VkDisplayPropertiesKHR*  displayPropertiesKHR = stackalloc VkDisplayPropertiesKHR[(int)physicalDeviceDisplayPropertiesCount];
        // func.vkGetPhysicalDeviceDisplayPropertiesKHR(data.Handles.PhysicalDevice, &physicalDeviceDisplayPropertiesCount, displayPropertiesKHR );
        // // if( IsFeatures2Capabilities)
        // // {
        // //     func.vkGetPhysicalDeviceDisplayProperties2KHR
        // // }
        // // Display PLanes
        // uint physicalDeviceDisplayPlanePropertiesCount =0;
               // func.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(data.Handles.PhysicalDevice, &physicalDeviceDisplayPlanePropertiesCount, null );
        // Guard.IsEmpty(physicalDeviceDisplayPlanePropertiesCount);
        // VkDisplayPlanePropertiesKHR*  physicalDeviceDisplayPlanes = stackalloc VkDisplayPlanePropertiesKHR[(int)physicalDeviceDisplayPlanePropertiesCount];
        // func.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(data.Handles.PhysicalDevice, &physicalDeviceDisplayPlanePropertiesCount,physicalDeviceDisplayPlanes );
        // if( IsFeatures2Capabilities)
        // {
        //     func.vkGetPhysicalDeviceDisplayPlaneProperties2KHR
        //      VkDisplayPlaneProperties2KHR*
        // }
                // func.vkGetDisplayPlaneCapabilitiesKHR( data.Handles.PhysicalDevice , )
        //DISPLAY MODE        // VkDisplayModeParametersKHR displayModeParameter = new();
        // VkDisplayModeCreateInfoKHR displayModeCreateInfo = new();
        // displayModeCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DISPLAY_MODE_CREATE_INFO_KHR;
        // displayModeCreateInfo.flags =0;
        // displayModeCreateInfo.pNext = null;
        // displayModeCreateInfo.parameters = displayModeParameter;
        // VkDisplayKHR displayKHR = VkDisplayKHR.Null;
        // func.vkCreateDisplayModeKHR(data.Handles.PhysicalDevice,displayModeCreateInfo, displayKHR, null )
        // uint displayModeProperties =0;
        // func.vkGetPhysicalDeviceDisplayPlanePropertiesKHR(data.Handles.PhysicalDevice, &displayModeProperties, null );
        // Guard.IsEmpty(displayModeProperties);
        // VkDisplayModePropertiesKHR*  displayModes = stackalloc VkDisplayModePropertiesKHR[(int)displayModeProperties];
        // func.vkGetDisplayModePropertiesKHR(data.Handles.PhysicalDevice, &displayModeProperties, displayModes );

        // PHYSICAL DEVICE PROPERTIES -------------------------------------------
        fixed (VkPhysicalDeviceProperties* phd =   &data.Info.PhysicalDeviceProperties)
        {
            func.vkGetPhysicalDeviceProperties(data.Handles.PhysicalDevice ,phd );
        }

        // GET Physical FEATURES ----------------------------------------------------------

        fixed ( VkPhysicalDeviceFeatures* features = &data.Info.Features)
        {
            func.vkGetPhysicalDeviceFeatures(data.Handles.PhysicalDevice,features );
        } 

        // DZEVICE  EXTENSIONS -------------------------------------------------
        uint propertyCount = 0;
        func.vkEnumerateDeviceExtensionProperties(data.Handles.PhysicalDevice, null, &propertyCount, null).Check();

        VkExtensionProperties* properties = stackalloc VkExtensionProperties[(int)propertyCount];  
        func.vkEnumerateDeviceExtensionProperties(data.Handles.PhysicalDevice, null, &propertyCount, properties).Check();

        data.Info.DeviceExtensions = new string[propertyCount + 1];

        for (int i = 0; i < propertyCount; i++){
            var length =  StrHelper.Strlen( properties[i].extensionName);
           data.Info.DeviceExtensions[i] = Encoding.UTF8.GetString( properties[i].extensionName, (int) length ); //new string(properties[i].extensionName); //
        }
        data.Info.DeviceExtensions[propertyCount] = VK.VK_KHR_SWAPCHAIN_EXTENSION_NAME ;
       
    }

    private ref struct SwapChainSupportDetails
    {
        public VkSurfaceCapabilitiesKHR Capabilities;
        public ReadOnlySpan<VkSurfaceFormatKHR> Formats;
        public ReadOnlySpan<VkPresentModeKHR> PresentModes;
    }

    private struct QueueFamilyIndices 
    {
        public uint? graphicsFamily = null!;
        public uint? presentFamily = null!;
        public uint? ComputeFamily = null!;

        public QueueFamilyIndices() { }

        public bool IsComplete() => graphicsFamily.HasValue && presentFamily.HasValue;
    }

    private unsafe static (uint graphicsFamily, uint presentFamily) FindQueueFamilies(ref GraphicDeviceFunctions func,VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        uint queueFamilyPropertyCount = 0;
        func.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyPropertyCount, null);

        ReadOnlySpan<VkQueueFamilyProperties> queueFamilyProperties = new VkQueueFamilyProperties[queueFamilyPropertyCount];
        
        fixed (VkQueueFamilyProperties* queueFamilyPropertiesPtr = queueFamilyProperties){
            func.vkGetPhysicalDeviceQueueFamilyProperties(device, &queueFamilyPropertyCount, queueFamilyPropertiesPtr);
        }
        QueueFamilyIndices indices =new();
        
        for( uint i = 0 ; i <queueFamilyProperties.Length ; i++ )
        {
            
            if ( (queueFamilyProperties[(int)i].queueFlags & VkQueueFlagBits.VK_QUEUE_GRAPHICS_BIT) != 0)
            {
                indices.graphicsFamily = i;
            }
            uint presentSupport =0;
            //Querying for WSI Support
            func.vkGetPhysicalDeviceSurfaceSupportKHR(device, i, surface, &presentSupport);
            
            if (presentSupport==VK.VK_TRUE)
            {
                indices.presentFamily = i;
            }
            
            if (indices.IsComplete())  {  break; }
            
        }
        #if WIN64
        // vkGetPhysicalDeviceWin32PresentationSupportKHR
        #endif

        return ((uint)indices.graphicsFamily! , (uint)indices.presentFamily! );
        
    }

    private static bool IsDeviceSuitable(ref GraphicDeviceFunctions func,VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
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
    
    private  unsafe static SwapChainSupportDetails QuerySwapChainSupport(ref GraphicDeviceFunctions func,VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
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
        foreach (VkSurfaceFormatKHR availableFormat in data.Info.Formats)
        {
            if (availableFormat.format == VkFormat.VK_FORMAT_B8G8R8A8_SRGB && availableFormat.colorSpace == VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR )
            {
               

                return availableFormat;
            }
        }


        return data.Info.Formats[0];
    }

    private static  VkExtent2D ChooseSwapExtent( ref GraphicDeviceData data) 
    {
        if ( data.Info.Capabilities.currentExtent.width != uint.MaxValue /*std::numeric_limits<uint32_t>::max()*/) {
            return data.Info.Capabilities.currentExtent;
        } else {
            VkExtent2D actualExtent =new() {
                width = ClampUInt( (uint)data.Info.Width, data.Info.Capabilities.minImageExtent.width, data.Info.Capabilities.maxImageExtent.width),
                height = ClampUInt( (uint)data.Info.Height, data.Info.Capabilities.minImageExtent.height, data.Info.Capabilities.maxImageExtent.height),
            };

            return actualExtent;
        }
    }

    private  static VkPresentModeKHR ChooseSwapPresentMode( ref GraphicDeviceData data )
    {
        foreach (VkPresentModeKHR availablePresentMode in data.Info.PresentModes)
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
    
    public static unsafe void CreateLogicalDevice(ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        (data.Info.VkGraphicFamilyIndice,data.Info.VkPresentFamilyIndice) = FindQueueFamilies(ref func , data.Handles.PhysicalDevice,data.Handles.Surface);

        VkDeviceQueueCreateInfo* queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[2];

        HashSet<uint> uniqueQueueFamilies = new();
        uniqueQueueFamilies.Add(data.Info.VkGraphicFamilyIndice);
        uniqueQueueFamilies.Add(data.Info.VkPresentFamilyIndice);
    
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
        using var deviceExtensions = new StrArrayUnsafe(ref data.Info.DeviceExtensions);
        using var layerNames = new RitaEngine.Base.Strings.StrArrayUnsafe(ref data.Info.ValidationLayers);
        VkDeviceCreateInfo createInfo = new();
        createInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
        createInfo.queueCreateInfoCount = (uint)queueCount;
        createInfo.pQueueCreateInfos = queueCreateInfos;
        fixed ( VkPhysicalDeviceFeatures* features = &data.Info.Features) {createInfo.pEnabledFeatures = features;}
        createInfo.enabledExtensionCount = (uint)deviceExtensions.Count;
        createInfo.ppEnabledExtensionNames = deviceExtensions;
        createInfo.pNext = null ;
        createInfo.enabledLayerCount = data.Info.EnableDebug ? layerNames.Count : 0 ;
        createInfo.ppEnabledLayerNames = data.Info.EnableDebug ? layerNames : null ;

        fixed(VkDevice* device = &data.Handles.Device)
        {
            func.vkCreateDevice(data.Handles.PhysicalDevice, &createInfo, null, device).Check("Error creation vkDevice");
        }
       
       VkHelper.ValidateExtensionsForLoad(ref data.Info.DeviceExtensions,0 );

       func.InitDevicesFunctions( GraphicDeviceFunctions.vkGetDeviceProcAddr , data.Handles.Device);

       Log.Info($"Create Device :{data.Handles.Device}");

        //CREATE QUEUES 
        fixed(VkQueue* gq =&data.Handles.GraphicQueue)
        {
            func.vkGetDeviceQueue(data.Handles.Device, data.Info.VkGraphicFamilyIndice, 0,gq);
        }
        fixed(VkQueue* pq = &data.Handles.PresentQueue)
        {
            func.vkGetDeviceQueue(data.Handles.Device, data.Info.VkPresentFamilyIndice, 0, pq); 
        }

        Log.Info($"Graphic Queues : indice :{ data.Info.VkGraphicFamilyIndice} Adr[{data.Handles.GraphicQueue}]\nPresent : indice :{data.Info.VkPresentFamilyIndice} Adr[{data.Handles.PresentQueue}]");
    }

    public static unsafe void DisposeLogicalDevice(in GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {       
        if ( !data.Handles.Device.IsNull)
        {
            Log.Info($"Dispose Logical Device {data.Handles.Device}");
            func.vkDestroyDevice(data.Handles.Device, null);
        }  
    }

    #endregion

    #region SWAP CHAIN


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

    #endregion
    
    #region RenderPass

    public static unsafe void CreateRenderPass(ref GraphicDeviceFunctions func,ref GraphicDeviceData data) 
    {
        // COLOR 
        VkAttachmentDescription colorAttachment =new();
        colorAttachment.format = data.Info.VkFormat;
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
        depthAttachment.format = FindDepthFormat(func, data.Handles.PhysicalDevice);
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

        fixed( VkRenderPass* renderPass= &data.Handles.RenderPass )
        {
            func.vkCreateRenderPass(data.Handles.Device, &renderPassInfo, null, renderPass).Check("failed to create render pass!");
        }

        Log.Info($"Create Render Pass : {data.Handles.RenderPass}");
    }

    public static unsafe void DisposeRenderPass(in GraphicDeviceFunctions  func,ref GraphicDeviceData data  )
    {
        if (!data.Handles.Device.IsNull && !data.Handles.RenderPass.IsNull)
        {
            Log.Info($"Destroy Render Pass : {data.Handles.RenderPass}");
            func.vkDestroyRenderPass(data.Handles.Device,data.Handles.RenderPass,null);
        }
    }

    #endregion

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
        poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
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

    #region Vertex & Index
 
    public unsafe static void CreateVertexBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data  ) 
    {
        VkDeviceSize bufferSize =  (uint)data.Info.Vertices.Length * sizeof(float) ;//(uint)( Position3f_Color3f_UV2f.Stride * data.Info.Vertices.Length);

        VkBuffer stagingBuffer = new();
        VkDeviceMemory stagingBufferMemory = new();
        
        CreateStagingBuffer(ref func, ref data,bufferSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer, ref stagingBufferMemory);
        

        void* vertexdataPtr = null; //

        func.vkMapMemory(data.Handles.Device, stagingBufferMemory, 0, bufferSize, 0, &vertexdataPtr ).Check("Impossible to map memory  for vertex");

            void* ptr =   Unsafe.AsPointer( ref data.Info.Vertices[0] );
            Unsafe.CopyBlock(vertexdataPtr , ptr ,(uint)bufferSize);    

        func.vkUnmapMemory(data.Handles.Device, stagingBufferMemory);

        CreateStagingBuffer(ref func, ref data,bufferSize,   VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT | VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, 
            ref  data.Handles.VertexBuffer, ref data.Info.VertexBufferMemory);

        CopyStagingBuffer(ref func, ref data,stagingBuffer, data.Handles.VertexBuffer, bufferSize);

        func.vkDestroyBuffer(data.Handles.Device, stagingBuffer, null);
        func.vkFreeMemory(data.Handles.Device, stagingBufferMemory, null);

        // Marshal.FreeHGlobal( (nint)vertexdata);
        Log.Info($"Create Vertex Buffer {data.Handles.VertexBuffer} ");
        Log.Info($"Create Vertex Buffer Memory {data.Info.VertexBufferMemory} ");
    }

    public unsafe static void CreateIndexBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data   ) 
    {
        VkDeviceSize bufferSize = (uint)(sizeof(short) * data.Info.Indices.Length);

        VkBuffer stagingBuffer = new();
        VkDeviceMemory stagingBufferMemory = new();

        CreateStagingBuffer(ref func , ref data , bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT , 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT 
            | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer, 
            ref stagingBufferMemory);
        

        void* indicesdataPtr = null;

        func.vkMapMemory(data.Handles.Device, stagingBufferMemory, 0, bufferSize, 0, &indicesdataPtr ).Check("Impossible to map memory  for indice");

            void* ptr =Unsafe.AsPointer( ref data.Info.Indices[0] );
            Unsafe.CopyBlock( indicesdataPtr , ptr ,(uint)bufferSize);
        
        func.vkUnmapMemory(data.Handles.Device, stagingBufferMemory);

        CreateStagingBuffer(ref func , ref data , bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT 
            | VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, 
            ref data.Handles.IndicesBuffer, 
            ref data.Info.IndicesBufferMemory);

        CopyStagingBuffer(ref func , ref data ,stagingBuffer, data.Handles.IndicesBuffer, bufferSize);

        func.vkDestroyBuffer(data.Handles.Device, stagingBuffer, null);
        func.vkFreeMemory(data.Handles.Device, stagingBufferMemory, null);

        // Marshal.FreeHGlobal( indicesdata);
        Log.Info($"Create Vertex Buffer {data.Handles.IndicesBuffer} ");
        Log.Info($"Create Vertex Buffer Memory {data.Info.IndicesBufferMemory} ");
    }

    public unsafe static void DisposeBuffers(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if( !data.Handles.IndicesBuffer.IsNull)
        {
            Log.Info($"Destroy Vertex Buffer {data.Handles.IndicesBuffer} ");
            func.vkDestroyBuffer(data.Handles.Device, data.Handles.IndicesBuffer, null);
        }
        if( !data.Info.IndicesBufferMemory.IsNull)
        {
            Log.Info($"Destroy Vertex Buffer Memory {data.Info.IndicesBufferMemory} ");
            func.vkFreeMemory(data.Handles.Device, data.Info.IndicesBufferMemory, null);
        }
        if( !data.Handles.VertexBuffer.IsNull)
        {
            Log.Info($"Destroy Vertex Buffer {data.Handles.VertexBuffer} ");
            func.vkDestroyBuffer(data.Handles.Device, data.Handles.VertexBuffer, null);
        }
        if( !data.Info.VertexBufferMemory.IsNull)
        {
            Log.Info($"Destroy Vertex Buffer Memory {data.Info.VertexBufferMemory} ");
            func.vkFreeMemory(data.Handles.Device, data.Info.VertexBufferMemory, null);
        }
        
    }

    private unsafe static void CreateStagingBuffer(ref GraphicDeviceFunctions func, ref GraphicDeviceData data ,
        VkDeviceSize size, VkBufferUsageFlagBits usage, 
        VkMemoryPropertyFlagBits  properties,ref VkBuffer buffer,ref VkDeviceMemory bufferMemory) 
    {
        VkBufferCreateInfo bufferInfo = new();
        bufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
        bufferInfo.size = size;
        bufferInfo.usage = (uint)usage;
        bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

        fixed(VkBuffer* buf =  &buffer)
        {
            func.vkCreateBuffer(data.Handles.Device, &bufferInfo, null, buf).Check("failed to create  buffer!");
        }
        
        VkMemoryRequirements memRequirements = new();
        func.vkGetBufferMemoryRequirements(data.Handles.Device, buffer, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data,memRequirements.memoryTypeBits, properties);
        allocInfo.pNext = null;

        fixed(VkDeviceMemory* memory =  &bufferMemory) 
        {
            func.vkAllocateMemory(data.Handles.Device, &allocInfo, null, memory).Check("failed to allocate memory!");
        }
        
        func.vkBindBufferMemory(data.Handles.Device, buffer, bufferMemory, 0).Check("failed to Bind buffer memory!");
    }

    private unsafe static void CopyStagingBuffer(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size)
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

        func.vkQueueSubmit(data.Handles.GraphicQueue, 1, &submitInfo, VkFence.Null);
        func.vkQueueWaitIdle(data.Handles.GraphicQueue);

        func.vkFreeCommandBuffers(data.Handles.Device, data.Handles.CommandPool, 1, &commandBuffer);
    }

    public unsafe static uint FindMemoryType(ref GraphicDeviceFunctions func, ref GraphicDeviceData data , uint memoryTypeBits, VkMemoryPropertyFlagBits properties)
    {
        VkPhysicalDeviceMemoryProperties memoryProperties = new();
        func.vkGetPhysicalDeviceMemoryProperties(data.Handles.PhysicalDevice, &memoryProperties);

        uint count = memoryProperties.memoryTypeCount;
        for (uint i = 0; i < count; i++)
        {
            if ( (memoryTypeBits & 1) == 1 && (memoryProperties.memoryTypes[(int)i].propertyFlags & (uint)properties) == (uint)properties)
            {
                Log.Info($"Memory INdex choosen : {i}");
                return i;
            }
            memoryTypeBits >>= 1;
        }

        return uint.MaxValue;
    }

    // [Obsolete]
    //   private unsafe static void CreateVertexBufferWithoutStaging(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data ,ref GraphicRenderConfig pipeline ) 
    // {
        
    //     VkBufferCreateInfo bufferInfo = new();
    //         bufferInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
    //         bufferInfo.size = (uint)(Marshal.SizeOf<Position2f_Color3f>() * data.Info.Vertices.Length);
    //         bufferInfo.usage = (uint)VkBufferUsageFlagBits. VK_BUFFER_USAGE_VERTEX_BUFFER_BIT;
    //         bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

    //     fixed(VkBuffer* buffer = &data.Info.VertexBuffer) {
    //         func.vkCreateBuffer( data.Handles.Device, &bufferInfo, null, buffer).Check("failed to create vertex buffer!");
    //     }
        
    //     VkMemoryRequirements memRequirements = new();
    //     func.vkGetBufferMemoryRequirements(data.Handles.Device, data.Info.VertexBuffer, &memRequirements);


    //     uint memoryTypeIndexForCoherentVisibleBit = FindMemoryType(  ref func , ref data , memRequirements.memoryTypeBits ,
    //         (VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT));
        
    //     VkMemoryAllocateInfo allocInfo = new();
    //         allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
    //         allocInfo.allocationSize = memRequirements.size;
    //         allocInfo.memoryTypeIndex = memoryTypeIndexForCoherentVisibleBit;
    //         allocInfo.pNext = null;

    //     fixed(VkDeviceMemory* memory =  &data.Info.VertexBufferMemory) {
    //         func.vkAllocateMemory(data.Handles.Device, &allocInfo, null, memory).Check("failed to allocate vertex buffer memory!");
    //     }
        
    //     func.vkBindBufferMemory(data.Handles.Device,  data.Info.VertexBuffer,data.Info.VertexBufferMemory, 0);

    //     void* vertexdata;

    //     func.vkMapMemory(data.Handles.Device,data.Info.VertexBufferMemory, 0, bufferInfo.size, 0, &vertexdata);
        
    //     fixed (void* p = &data.Info.Vertices[0]){ 
    //         memcpy(vertexdata, p,  bufferInfo.size); 
    //     }  
    //     func.vkUnmapMemory(data.Handles.Device, data.Info.VertexBufferMemory);
    // }

    #endregion

    #region Texture

    public unsafe static void CreateTextureImage(ref GraphicDeviceFunctions func,ref GraphicDeviceData data )
    {
        VkFormat format = VkFormat.VK_FORMAT_UNDEFINED;
        uint texWidth=512, texHeight=512;
        uint texChannels=4;
        //TODO CreateTextureImage  File.ReadAllBytes( data.Info.TextureName) do this outside 
        var file = File.ReadAllBytes( data.Info.TextureName);
        // StbImage.stbi__vertically_flip_on_load_set = 1;
        ImageResult result = ImageResult.FromMemory(file , ColorComponents.RedGreenBlueAlpha);
        texWidth = (uint)result.Width;
        texHeight = (uint)result.Height;
        texChannels = (uint)result.Comp;

        VkDeviceSize imageSize = (ulong)(texWidth * texHeight * texChannels);

        VkBuffer stagingBuffer = VkBuffer.Null;
        VkDeviceMemory stagingBufferMemory = VkDeviceMemory.Null;

        CreateStagingBuffer(ref func , ref data , imageSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer, 
            ref stagingBufferMemory);

        byte* imgPtr = null;
        func.vkMapMemory(data.Handles.Device, stagingBufferMemory, 0, imageSize, 0, (void**)&imgPtr).Check("Impossible to map memory for texture");
       
            // void* ptr = Unsafe.AsPointer( ref  result.Data[0]);
        fixed( byte* tex2D = &result.Data[0])
        {
            Unsafe.CopyBlock(imgPtr  ,tex2D ,(uint)imageSize);
        }

          
        func.vkUnmapMemory(data.Handles.Device, stagingBufferMemory);

        if (result.Comp == ColorComponents.RedGreenBlue  )
            format = VkFormat.VK_FORMAT_R8G8B8_SRGB;
        else if (result.Comp == ColorComponents.RedGreenBlueAlpha )    
            format = VkFormat.VK_FORMAT_R8G8B8A8_SRGB;

        CreateImage(ref func , ref data, ref data.Info.TextureImage, ref data.Info.TextureImageMemory, texWidth, texHeight, 
            format,//VkFormat.VK_FORMAT_R8G8B8A8_SRGB, 
            VkImageTiling.VK_IMAGE_TILING_OPTIMAL, 
            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT   );

        TransitionImageLayout(ref func , ref data, format, VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
            CopyBufferToImage(ref  func,ref  data,stagingBuffer, (texWidth), (texHeight));
        TransitionImageLayout(ref func , ref data,format, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

        func.vkDestroyBuffer(data.Handles.Device, stagingBuffer, null);
        func.vkFreeMemory(data.Handles.Device, stagingBufferMemory, null);
        
        file = null!;
        result.Data = null!;
        
        Log.Info($"Create Texture Image {data.Info.TextureImage} ");
        Log.Info($"Create Texture Image Memory {data.Info.TextureImageMemory} ");
    }

    public unsafe static void CreateTextureImageView(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data)
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
    }

    public unsafe static void DisposeTextureImage(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if( !data.Info.TextureImage.IsNull)
        {
            Log.Info($"Create Texture Image {data.Info.TextureImage} ");
            func.vkDestroyImage(data.Handles.Device, data.Info.TextureImage, null);
        }
        if( !data.Info.TextureImageMemory.IsNull)
        {
            Log.Info($"Create Texture Image Memory {data.Info.TextureImageMemory} ");
            func.vkFreeMemory(data.Handles.Device, data.Info.TextureImageMemory, null);
        }
        if( !data.Info.TextureImageView.IsNull)
        {
            Log.Info($"Destroy Texture Image View {data.Info.TextureImageView}");
            func.vkDestroyImageView(data.Handles.Device, data.Info.TextureImageView, null);
        }
    }
    
    private unsafe static void CreateImage(ref GraphicDeviceFunctions func,ref GraphicDeviceData data,
        ref VkImage image,ref VkDeviceMemory imageMemory,  uint width, uint height, VkFormat format, VkImageTiling tiling,
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
        imageInfo.pNext = null;

        fixed( VkImage* img = &image)
        {
            func.vkCreateImage(data.Handles.Device, &imageInfo, null,img).Check("failed to create image!");
        }
        
        VkMemoryRequirements memRequirements;
        func.vkGetImageMemoryRequirements(data.Handles.Device, image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = FindMemoryType(ref func , ref data, memRequirements.memoryTypeBits, properties);

        fixed (VkDeviceMemory* imgMem = &imageMemory  )
        {
            func.vkAllocateMemory(data.Handles.Device, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        }

        func.vkBindImageMemory( data.Handles.Device, image,imageMemory, 0).Check("Bind Image Memory");
    }

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

    #region  UNIFORM BUFFER  NEED TO DESCRIPTOR SET

    public unsafe static void CreateUniformBuffers(ref GraphicDeviceFunctions func, ref GraphicDeviceData data   ) 
    {
        ulong uboAlignment  = data.Info.PhysicalDeviceProperties.limits.minUniformBufferOffsetAlignment;
        ulong uboSize = (ulong)data.Info.UniformBufferArray.Length * sizeof(float);
        var align = (((ulong)uboSize / uboAlignment) * uboAlignment + (((ulong)uboSize % uboAlignment) > 0 ? uboAlignment : 0));
        
        VkDeviceSize bufferSize = (uint)align;
        
        data.Info.UboSize = (ulong) bufferSize;
        
        data.Info.UniformBuffers = new VkBuffer[ data.Info.MAX_FRAMES_IN_FLIGHT];
        data.Info.UniformBuffersMemory = new VkDeviceMemory[data.Info.MAX_FRAMES_IN_FLIGHT];
        data.Info.UboMapped = new void* [ data.Info.MAX_FRAMES_IN_FLIGHT] ;
        
        for (int i = 0; i < data.Info.MAX_FRAMES_IN_FLIGHT; i++) 
        {
            CreateStagingBuffer(ref func, ref data, bufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT,
            ref data.Info.UniformBuffers[i],ref data.Info.UniformBuffersMemory[i]);

            void* ptr = null;
            func.vkMapMemory(data.Handles.Device,data.Info.UniformBuffersMemory[i], 0, bufferSize, 0, &ptr ).Check("Map Memeory Unifommr pb");
            Guard.ThrowWhenConditionIsTrue( ptr == null);
            data.Info.UboMapped[i] = ptr;
          
            Log.Info($"-[{i}] Create Uniform Buffer : {data.Info.UniformBuffers[i]} Mem {data.Info.UniformBuffersMemory[i]}");
            func.vkUnmapMemory(data.Handles.Device,data.Info.UniformBuffersMemory[i]);
        }   
       
    }

    public unsafe static void DisposeUniformBuffers(in GraphicDeviceFunctions  func, ref GraphicDeviceData data ) 
    {
        for (nint i = 0; i < data.Info.MAX_FRAMES_IN_FLIGHT; i++) 
        {
           
            if ( data.Info.UniformBuffers != null! && !data.Info.UniformBuffers[i].IsNull )
            {
                Log.Info($"-[{i}] Destroy Uniform Buffer : {data.Info.UniformBuffers[i]}");
                func.vkDestroyBuffer(data.Handles.Device, data.Info.UniformBuffers[i], null);
            }
            if( data.Info.UniformBuffersMemory != null! &&!data.Info.UniformBuffersMemory[i].IsNull)
            {
                func.vkUnmapMemory(data.Handles.Device, data.Info.UniformBuffersMemory[i] );
                Log.Info($"-[{i}] Destroy Uniform Buffer Memory : {data.Info.UniformBuffersMemory[i]}");
                func.vkFreeMemory(data.Handles.Device, data.Info.UniformBuffersMemory[i], null);
            } 
            if ( data.Info.UboMapped != null )
            {
                Log.Info($"-[{i}] Destroy Uniform Buffer Memory Mapped: { new IntPtr(data.Info.UboMapped[i]) }");
                data.Info.UboMapped[i] = null!;
            }
            
        }
    }

    public unsafe static void UpdateUniformBuffer(in GraphicDeviceFunctions  func,ref GraphicDeviceData data )
    {
        fixed( void* local  = &data.Info.UniformBufferArray[0] )
        {
            Unsafe.CopyBlock( data.Info.UboMapped[CurrentFrame] , local ,(uint) data.Info.UboSize);
        }
    }

    #endregion

    #region SAMPLER NEED TO DESCRIPTOR SET
    public unsafe static void CreateTextureSampler(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        VkSamplerCreateInfo samplerInfo = new();
        samplerInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
        samplerInfo.magFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.minFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.anisotropyEnable = VK.VK_TRUE;
        samplerInfo.maxAnisotropy = data.Info.PhysicalDeviceProperties.limits.maxSamplerAnisotropy;
        samplerInfo.borderColor = VkBorderColor . VK_BORDER_COLOR_INT_OPAQUE_BLACK;
        samplerInfo.unnormalizedCoordinates = VK.VK_FALSE;
        samplerInfo.compareEnable = VK.VK_FALSE;
        samplerInfo.compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS;
        samplerInfo.mipmapMode = VkSamplerMipmapMode .VK_SAMPLER_MIPMAP_MODE_LINEAR;

        fixed(VkSampler* sampler  = &data.Info.TextureSampler)
        {
            func.vkCreateSampler(data.Handles.Device, &samplerInfo, null, sampler).Check("failed to create texture sampler!");
        }   
        Log.Info($"Create Texture sampler {data.Info.TextureSampler}");
    }

    public unsafe static void DisposeTextureSampler(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if( !data.Info.TextureSampler.IsNull)
        {
            Log.Info($"Destroy Texture sampler {data.Info.TextureSampler}");
            func.vkDestroySampler(data.Handles.Device,data.Info.TextureSampler, null);
        }
      
    }

    #endregion      
    
    #region Desciptor pool descriptor set and layout 

    public static unsafe void CreateDescriptorPool(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        VkDescriptorPoolSize* poolSizes = stackalloc VkDescriptorPoolSize[2];

        poolSizes[0].type =VkDescriptorType. VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
        poolSizes[0].descriptorCount = (uint)(data.Info.MAX_FRAMES_IN_FLIGHT);
        
        poolSizes[1].type = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
        poolSizes[1].descriptorCount =(uint)(data.Info.MAX_FRAMES_IN_FLIGHT);

        VkDescriptorPoolCreateInfo poolInfo= new();
        poolInfo.sType =VkStructureType. VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO;
        poolInfo.poolSizeCount = 2;
        poolInfo.pPoolSizes = poolSizes;
        poolInfo.maxSets = (uint)(data.Info.MAX_FRAMES_IN_FLIGHT);

        fixed(VkDescriptorPool* pool =  &data.Handles.DescriptorPool)
        {
            func.vkCreateDescriptorPool(data.Handles.Device, &poolInfo, null,pool ).Check("failed to create descriptor pool!");
        }
        Log.Info($"Create Descriptor Pool : {data.Handles.DescriptorPool}");
    }
    
    public unsafe static void CreateDescriptorSetLayout(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data  ) 
    {
        VkDescriptorSetLayoutBinding uboLayoutBinding = new();
        uboLayoutBinding.binding = 0;
        uboLayoutBinding.descriptorCount = 1;
        uboLayoutBinding.descriptorType = VkDescriptorType. VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
        uboLayoutBinding.pImmutableSamplers = null;
        uboLayoutBinding.stageFlags =(uint) VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
        
        VkDescriptorSetLayoutBinding samplerLayoutBinding =new();
        samplerLayoutBinding.binding = 1;
        samplerLayoutBinding.descriptorCount = 1;
        samplerLayoutBinding.descriptorType =  VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
        samplerLayoutBinding.pImmutableSamplers = null;
        samplerLayoutBinding.stageFlags = (uint) VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT;
        
        VkDescriptorSetLayoutBinding* bindings = stackalloc VkDescriptorSetLayoutBinding[2] {uboLayoutBinding, samplerLayoutBinding };
        
        VkDescriptorSetLayoutCreateInfo layoutInfo = new();
        layoutInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO;
        layoutInfo.bindingCount = 2;
        layoutInfo.pBindings = bindings;

        fixed( VkDescriptorSetLayout* layout = &data.Handles.DescriptorSetLayout )
        {
            func.vkCreateDescriptorSetLayout(data.Handles.Device, &layoutInfo, null, layout).Check("failed to create descriptor set layout!");
        }
        Log.Info($"Create Descriptor Set layout : {data.Handles.DescriptorSetLayout}");
    }
   
    public static unsafe void CreateDescriptorSets(ref GraphicDeviceFunctions  func, ref GraphicDeviceData data) 
    {
        // int value = data.Info.MAX_FRAMES_IN_FLIGHT;
        VkDescriptorSetLayout* layouts  =  stackalloc VkDescriptorSetLayout[2] { data.Handles.DescriptorSetLayout,data.Handles.DescriptorSetLayout };

        VkDescriptorSetAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO;
        allocInfo.descriptorPool = data.Handles.DescriptorPool;
        allocInfo.descriptorSetCount = (uint)(data.Info.MAX_FRAMES_IN_FLIGHT);
        allocInfo.pSetLayouts = layouts;
        allocInfo.pNext = null;

        data.Handles.DescriptorSets = new VkDescriptorSet[ data.Info.MAX_FRAMES_IN_FLIGHT];

        fixed(VkDescriptorSet* descriptor =&data.Handles.DescriptorSets[0]  )
        {
            func.vkAllocateDescriptorSets(data.Handles.Device, &allocInfo, descriptor).Check("failed to allocate descriptor sets!");
        }

        VkWriteDescriptorSet* descriptorWrites = stackalloc VkWriteDescriptorSet[2];

        for (int i = 0; i <  data.Info.MAX_FRAMES_IN_FLIGHT; i++) 
        {
            VkDescriptorBufferInfo bufferInfo = new();
            bufferInfo.buffer = data.Info.UniformBuffers[i];
            bufferInfo.offset = 0;
            bufferInfo.range = (uint) sizeof(float) * 3 * 16;// sizeof UNIFORM_MVP

            VkDescriptorImageInfo imageInfo =new();
            imageInfo.imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            imageInfo.imageView = data.Info.TextureImageView;
            imageInfo.sampler = data.Info.TextureSampler;

            descriptorWrites[0].sType = VkStructureType. VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
            descriptorWrites[0].dstSet = data.Handles.DescriptorSets[i];
            descriptorWrites[0].dstBinding = 0;
            descriptorWrites[0].dstArrayElement = 0;
            descriptorWrites[0].descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
            descriptorWrites[0].descriptorCount = 1;
            descriptorWrites[0].pBufferInfo = &bufferInfo;

            descriptorWrites[1].sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
            descriptorWrites[1].dstSet = data.Handles.DescriptorSets[i];
            descriptorWrites[1].dstBinding = 1;
            descriptorWrites[1].dstArrayElement = 0;
            descriptorWrites[1].descriptorType =VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
            descriptorWrites[1].descriptorCount = 1;
            descriptorWrites[1].pImageInfo = &imageInfo;

            func.vkUpdateDescriptorSets(data.Handles.Device, 2, descriptorWrites, 0, null);
            Log.Info($"-{i}  Create Descriptor Sets : {data.Handles.DescriptorSets[i]}");
        }
    }

    public static unsafe void DisposeDescriptorPool(in GraphicDeviceFunctions  func, ref GraphicDeviceData data)
    {
        if ( !data.Handles.DescriptorPool.IsNull)
        {
            Log.Info($"Destroy Descriptor Pool : {data.Handles.DescriptorPool}");
            func.vkDestroyDescriptorPool(data.Handles.Device, data.Handles.DescriptorPool, null);
        }
        
        if ( !data.Handles.DescriptorSetLayout.IsNull)
        {
            Log.Info($"Destroy Descriptor Set layout : {data.Handles.DescriptorSetLayout}");
            func.vkDestroyDescriptorSetLayout(data.Handles.Device, data.Handles.DescriptorSetLayout, null);
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
    public static unsafe void CreateSyncObjects(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data   )
    {
        data.Handles.ImageAvailableSemaphores = new VkSemaphore[data.Info.MAX_FRAMES_IN_FLIGHT]; 
        data.Handles.RenderFinishedSemaphores = new VkSemaphore[data.Info.MAX_FRAMES_IN_FLIGHT];
        data.Handles.InFlightFences = new VkFence[data.Info.MAX_FRAMES_IN_FLIGHT];

        VkSemaphoreCreateInfo semaphoreInfo =new();
        semaphoreInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO;
        semaphoreInfo.flags =0;
        semaphoreInfo.pNext =null;

        VkFenceCreateInfo fenceInfo= new();
        fenceInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FENCE_CREATE_INFO;
        fenceInfo.flags = (uint)VkFenceCreateFlagBits.VK_FENCE_CREATE_SIGNALED_BIT;


        for (int i = 0; i < data.Info.MAX_FRAMES_IN_FLIGHT; i++)
        {
            fixed( VkSemaphore* imageAvailableSemaphore = &data.Handles.ImageAvailableSemaphores[i])
            {
                func.vkCreateSemaphore(data.Handles.Device, &semaphoreInfo, null,  imageAvailableSemaphore).Check("Failed to create Semaphore ImageAvailableSemaphore");
            }
            Log.Info($"-{i}  Create Semaphore Image Available : {data.Handles.ImageAvailableSemaphores[i]}");
            fixed( VkSemaphore* renderFinishedSemaphore = &data.Handles.RenderFinishedSemaphores[i])
            {
                func.vkCreateSemaphore(data.Handles.Device, &semaphoreInfo, null, renderFinishedSemaphore).Check("Failed to create Semaphore RenderFinishedSemaphore");
            }
            Log.Info($"-{i}  Create Semaphore render Finish : {data.Handles.RenderFinishedSemaphores[i]}");
            fixed(VkFence*  inFlightFence = &data.Handles.InFlightFences[i] )
            {
                func.vkCreateFence(data.Handles.Device, &fenceInfo, null, inFlightFence).Check("Failed to create Fence InFlightFence");
            }
            Log.Info($"-{i}  Create Fence  : {data.Handles.InFlightFences[i]}");
        }
    }

    public static unsafe void DisposeSyncObjects(in GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
        if (  !data.Handles.Device.IsNull && data.Handles.RenderFinishedSemaphores != null){
            for ( int i = 0 ; i< data.Info.MAX_FRAMES_IN_FLIGHT ; i++)
            {
                if ( !data.Handles.RenderFinishedSemaphores[i].IsNull)
                {
                    Log.Info($"-{i}  Create Semaphore render Finish : {data.Handles.RenderFinishedSemaphores[i]}");
                    func.vkDestroySemaphore(data.Handles.Device, data.Handles.RenderFinishedSemaphores[i], null);
                }
            }
        }

        if (  !data.Handles.Device.IsNull && data.Handles.ImageAvailableSemaphores != null){
            for ( int i = 0 ; i< data.Info.MAX_FRAMES_IN_FLIGHT ; i++)
            {
                if ( !data.Handles.ImageAvailableSemaphores[i].IsNull)
                {
                    Log.Info($"-{i}  Create Semaphore Image Available : {data.Handles.ImageAvailableSemaphores[i]}");
                    func.vkDestroySemaphore(data.Handles.Device, data.Handles.ImageAvailableSemaphores[i], null);
                }
            }
        }

        if (  !data.Handles.Device.IsNull && data.Handles.InFlightFences != null){
            for ( int i = 0 ; i< data.Info.MAX_FRAMES_IN_FLIGHT ; i++)
            {
                if ( !data.Handles.InFlightFences[i].IsNull)
                {
                    Log.Info($"-{i}  Create Fence  : {data.Handles.InFlightFences[i]}");
                    func.vkDestroyFence(data.Handles.Device,data.Handles.InFlightFences[i], null);
                }
            }
        }
    }

    #endregion

    #region Pipeline

    #region FIXED FUNCTIONS
    private unsafe static void Pipeline_CreateShaderModule( out VkShaderModule shader , ref GraphicDeviceFunctions  func, ref GraphicDeviceData data , string shaderfragmentfileSPV)
    {
        ReadOnlySpan<byte> span = File.ReadAllBytes( shaderfragmentfileSPV ).AsSpan();
         
        VkShaderModuleCreateInfo createInfoFrag =new();
        createInfoFrag.sType= VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
        createInfoFrag.codeSize=(int)span.Length;
        createInfoFrag.pCode= (uint*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
        createInfoFrag.pNext = null;
        createInfoFrag.flags =0;

        VkShaderModule fragShaderModule = VkShaderModule.Null;
        func.vkCreateShaderModule(data.Handles.Device, &createInfoFrag, null,  &fragShaderModule ).Check($"Create Fragment Shader Module ; {shaderfragmentfileSPV}"); 
        shader = fragShaderModule;
    }

    public unsafe static void CreatePipelineLayout(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data  )
    {
        VkPipelineLayoutCreateInfo pipelineLayoutInfo=new();
        pipelineLayoutInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO;
        pipelineLayoutInfo.flags =0;
        pipelineLayoutInfo.pNext =null; 

        pipelineLayoutInfo.setLayoutCount = 1;           
        fixed (VkDescriptorSetLayout* layout = &data.Handles.DescriptorSetLayout )
        {
            pipelineLayoutInfo.pSetLayouts = layout;
        }         


        VkPushConstantRange push_constant;
	    //this push constant range starts at the beginning
	    push_constant.offset = 0;
	    //this push constant range takes up the size of a MeshPushConstants struct
	    push_constant.size = (uint)sizeof(PushConstantsMesh); // 16 * sizeof(float) + 4 * sizeof(float)
	    //this push constant range is accessible only in the vertex shader
	    push_constant.stageFlags = (uint)VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;


        pipelineLayoutInfo.pushConstantRangeCount = 1;    // Optionnel
        pipelineLayoutInfo.pPushConstantRanges = &push_constant; 


        fixed( VkPipelineLayout* layout = &data.Handles.PipelineLayout )
        {
            func.vkCreatePipelineLayout(data.Handles.Device, &pipelineLayoutInfo, null, layout).Check ("failed to create pipeline layout!");
        }
        Log.Info($"Create Pipeline Layout : {data.Handles.PipelineLayout}");
    }

    #endregion

    private unsafe static void CreateShaderStages(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data ,out VkShaderModule[] shaderModules,  out VkPipelineShaderStageCreateInfo[] shaderStages )
    {
        shaderModules = new  VkShaderModule[2];
        Pipeline_CreateShaderModule( out   shaderModules[0] , ref func, ref data , data.Info.VertexShaderFileNameSPV);
        Pipeline_CreateShaderModule( out shaderModules[1]   , ref func, ref data , data.Info.FragmentShaderFileNameSPV);
        using RitaEngine.Base.Strings.StrUnsafe fragentryPoint = new(data.Info.FragmentEntryPoint);
        using RitaEngine.Base.Strings.StrUnsafe vertentryPoint = new(data.Info.VertexEntryPoint);
       
        // VkPipelineShaderStageCreateInfo* shaderStages = stackalloc VkPipelineShaderStageCreateInfo[2];        
        shaderStages = new VkPipelineShaderStageCreateInfo[2]; // stackalloc VkPipelineShaderStageCreateInfo[2];        
        shaderStages[0] = new();
        shaderStages[0].sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
        shaderStages[0].stage = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
        shaderStages[0].module =  shaderModules[0];
        shaderStages[0].pName = vertentryPoint;
        shaderStages[0].flags =0;
        shaderStages[0].pNext =null;
        shaderStages[0].pSpecializationInfo =null;

        shaderStages[1] = new();
        shaderStages[1].sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
        shaderStages[1].stage = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT;
        shaderStages[1].module =  shaderModules[1];
        shaderStages[1].pName = fragentryPoint;
        shaderStages[1].flags =0;
        shaderStages[1].pNext =null;
        shaderStages[1].pSpecializationInfo =null;
      
    }

    public static unsafe void CreatePipeline(ref GraphicDeviceFunctions  func,ref GraphicDeviceData data , in GraphicRenderConfig renderConfig)
    {
        

        #region VERTEX BUFFER

        VkPipelineVertexInputStateCreateInfo vertexInputInfo =new();
        vertexInputInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;

  
        VkVertexInputBindingDescription bindingDescription =new();
        bindingDescription.binding = 0;
        bindingDescription.stride =(uint)Vertex.Stride;
        bindingDescription.inputRate = VkVertexInputRate.VK_VERTEX_INPUT_RATE_VERTEX;

        VkVertexInputAttributeDescription* attributeDescriptions = stackalloc VkVertexInputAttributeDescription[3] ;
        attributeDescriptions[0].binding = 0;
        attributeDescriptions[0].location = 0;
        attributeDescriptions[0].format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
        attributeDescriptions[0].offset = (uint)Vertex.OffsetPosition;

        attributeDescriptions[1].binding = 0;
        attributeDescriptions[1].location = 1;
        attributeDescriptions[1].format = VkFormat.VK_FORMAT_R32G32B32_SFLOAT;
        attributeDescriptions[1].offset =   (uint)Vertex.FormatNormal;
        
        attributeDescriptions[2].binding = 0;
        attributeDescriptions[2].location = 2;
        attributeDescriptions[2].format = VkFormat.VK_FORMAT_R32G32_SFLOAT;
        attributeDescriptions[2].offset =   (uint)Vertex.OffsetTexCoord; 
            
        vertexInputInfo.vertexBindingDescriptionCount = 1;
        vertexInputInfo.vertexAttributeDescriptionCount = 3;
        vertexInputInfo.pNext = null;
        vertexInputInfo.flags =0;
        vertexInputInfo.pVertexAttributeDescriptions=attributeDescriptions;
        vertexInputInfo.pVertexBindingDescriptions=&bindingDescription;

        #endregion

        #region INPUT ASSEMBLY
        VkPipelineInputAssemblyStateCreateInfo inputAssembly=new();
        inputAssembly.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
        inputAssembly.topology = VkPrimitiveTopology. VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
        inputAssembly.primitiveRestartEnable = VK.VK_FALSE;
        inputAssembly.flags =0;
        inputAssembly.pNext =null;       
        #endregion

        #region VIEWPORT

        data.Info.Viewport.x = 0.0f;
        data.Info.Viewport.y = 0.0f;
        data.Info.Viewport.width = (float) data.Info.VkSurfaceArea.width;
        data.Info.Viewport.height = (float) data.Info.VkSurfaceArea.height;
        data.Info.Viewport.minDepth = 0.0f;
        data.Info.Viewport.maxDepth = 1.0f;

        VkOffset2D offset = new();
        offset.x = 0;
        offset.y = 0;
        data.Info.Scissor.offset = offset;
        data.Info.Scissor.extent = data.Info.VkSurfaceArea;

        VkPipelineViewportStateCreateInfo viewportState =new();
        viewportState.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO;
        viewportState.viewportCount = 1;
        // fixed( VkViewport* viewport = &data.Info.Viewport)
        // {    
        //     viewportState.pViewports =viewport;    
        // }
        viewportState.scissorCount = 1;
        // fixed( VkRect2D* scissor = &data.Info.Scissor )
        // {  
        //     viewportState.pScissors = scissor;          
        // }
        viewportState.flags=0;
        viewportState.pNext = null;
        #endregion

        CreateShaderStages(ref   func,ref data , out VkShaderModule[] shaderModules,out VkPipelineShaderStageCreateInfo[] shaderStages );
        Pipeline.ColorBlendingConfigData cb = new( );
        Pipeline.CreateColorBlending(ref cb, out VkPipelineColorBlendStateCreateInfo colorBlending,out VkPipelineColorBlendAttachmentState colorBlendAttachment);
        Pipeline.CreateRasterization( ref renderConfig.Pipeline_Rasterization , out VkPipelineRasterizationStateCreateInfo rasterizer) ;
        Pipeline.CreateMultisampling(ref renderConfig.Pipeline_Multisampling, out VkPipelineMultisampleStateCreateInfo multisampling );
        Pipeline.CreateDepthStencil( ref renderConfig.Pipeline_DepthStencil , out VkPipelineDepthStencilStateCreateInfo depthStencilStateCreateInfo);
        Pipeline.CreateDynamicStates( ref renderConfig.Pipeline_DynamicStates , out  VkPipelineDynamicStateCreateInfo dynamicStateCreateInfo );
        Pipeline.CreateTessellation(out VkPipelineTessellationStateCreateInfo tessellationStateCreateInfo);
    

        VkGraphicsPipelineCreateInfo pipelineInfo =new();
        pipelineInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO;
        pipelineInfo.pNext = null;
        pipelineInfo.flags = (uint)VkPipelineCreateFlagBits.VK_PIPELINE_CREATE_DISABLE_OPTIMIZATION_BIT ;
        pipelineInfo.renderPass = data.Handles.RenderPass;
        pipelineInfo.layout = data.Handles.PipelineLayout;

        pipelineInfo.subpass = 0;
        pipelineInfo.stageCount =2;
        fixed(VkPipelineShaderStageCreateInfo*ss = &shaderStages[0] )
        {
            pipelineInfo.pStages = ss;
        }
        pipelineInfo.pVertexInputState = &vertexInputInfo;
        pipelineInfo.pInputAssemblyState = &inputAssembly;
        pipelineInfo.pColorBlendState = &colorBlending;
        pipelineInfo.pViewportState = &viewportState;
        pipelineInfo.pRasterizationState = &rasterizer;
        pipelineInfo.pMultisampleState = &multisampling;
        
        pipelineInfo.pTessellationState = &tessellationStateCreateInfo;
        pipelineInfo.pDepthStencilState = &depthStencilStateCreateInfo;
        pipelineInfo.pDynamicState = &dynamicStateCreateInfo;
        
        pipelineInfo.basePipelineIndex =0;
        pipelineInfo.basePipelineHandle = VkPipeline.Null;
        
        fixed( VkPipeline* gfxpipeline = &data.Handles.Pipeline )
        {    
            func.vkCreateGraphicsPipelines(data.Handles.Device, VkPipelineCache.Null, 1, &pipelineInfo, null, gfxpipeline).Check("failed to create graphics pipeline!");
        }

        for( int i = 0 ; i< shaderModules.Length ; i++ )
        {
            if(shaderModules[i] != VkShaderModule.Null)
            {
                func.vkDestroyShaderModule(data.Handles.Device, shaderModules[i], null);
            }
        }
  
        Log.Info($"Create PIPELINE : {data.Handles.Pipeline}");
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

            // PUSH CONSTANTS ---------- ( do before bin pipeline)
            // void* ptr = new IntPtr( data.Info.PushConstants).ToPointer();
            fixed(void* ptr = &data.Info.PushConstants ){
                func.vkCmdPushConstants(commandBuffer,data.Handles.PipelineLayout, (uint) VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT, 0,(uint)sizeof(PushConstantsMesh), ptr );
            }
            
            // USE SHADER 
                func.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.Handles.Pipeline);
                
            // SEND DATA To SHADER
                fixed(VkDescriptorSet* desc =  &data.Handles.DescriptorSets[CurrentFrame] )
                {
                    func.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.Handles.PipelineLayout, 0, 1, desc, 0, null);
                }
                
            // SET DYNAMIC STATES
                fixed(VkViewport* viewport = &data.Info.Viewport ){ func.vkCmdSetViewport(commandBuffer, 0, 1,viewport); }
                fixed( VkRect2D* scissor = &data.Info.Scissor) { func.vkCmdSetScissor(commandBuffer, 0, 1, scissor); }
                func.vkCmdSetLineWidth( commandBuffer,data.Handles.DynamicStatee_LineWidth);
               

            // BIND VERTEX AND INDICES
                VkDeviceSize* offsets = stackalloc VkDeviceSize[]{0};
                VkBuffer* vertexBuffers = stackalloc VkBuffer[] { data.Handles.VertexBuffer};
                func.vkCmdBindVertexBuffers(commandBuffer, 0, 1, vertexBuffers, offsets);
                func.vkCmdBindIndexBuffer(commandBuffer, data.Handles.IndicesBuffer, 0, VkIndexType.VK_INDEX_TYPE_UINT16);
            // DRAW CALLS  ------------ VERTEX INDEXED  
                func.vkCmdDrawIndexed(commandBuffer, data.Handles.IndicesSize, 1, 0, 0, 0);

            func.vkCmdEndRenderPass(commandBuffer);
        // RENDER PASS --------------------------------------------------------------------------------------------------        
        func.vkEndCommandBuffer(commandBuffer).Check("Failed to End command buffer ");
    // COMMAND BUFFER ----------------------------------------------------------------------------------------------------    
    }
    private static int CurrentFrame =0;

    // [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static unsafe  void DrawPipeline(ref GraphicDeviceFunctions func, ref GraphicDeviceData data )
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
            ReCreateSwapChain( ref func,ref data);
            return ;
        }
        else if (result != VkResult.VK_SUCCESS && result != VkResult.VK_SUBOPTIMAL_KHR )
        {
            throw new Exception("Failed to acquire swap chain Images");
        }

        UpdateUniformBuffer( func,ref  data);
                
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
           ReCreateSwapChain( ref func,ref data);
        }
        else if (result != VkResult.VK_SUCCESS )
        {
            throw new Exception("Failed to  present swap chain Images");
        }

       CurrentFrame = ((CurrentFrame + 1) % data.Info.MAX_FRAMES_IN_FLIGHT);   
    }

    public static unsafe void DisposePipeline(in GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
        if( !data.Handles.Device.IsNull && !data.Handles.PipelineLayout.IsNull)
        {
            Log.Info($"Destroy Pipeline Layout : {data.Handles.PipelineLayout}");
            func.vkDestroyPipelineLayout(data.Handles.Device, data.Handles.PipelineLayout, null);
        }        
        if (!data.Handles.Device.IsNull && !data.Handles.Pipeline.IsNull)
        {
            Log.Info($"Destroy PIPELINE : {data.Handles.Pipeline}");
            func.vkDestroyPipeline(data.Handles.Device,data.Handles.Pipeline, null);
        }
    }

    #endregion

}


