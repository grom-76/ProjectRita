namespace RitaEngine.Graphics;

using RitaEngine.API;
using RitaEngine.API.Vulkan;
using RitaEngine.Base;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Strings;
using RitaEngine.Graphic;
using RitaEngine.Platform;

//GENERAL
public struct GraphicsData
{
    public DeviceData Device;
    public RenderData Render; 
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

    public DeviceConfig()
    {
    }
}

public unsafe readonly struct DeviceData
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

    public unsafe readonly  delegate* unmanaged< VkQueue,VkResult > vkQueueWaitIdle = null;
	public unsafe readonly  delegate* unmanaged< VkDevice,VkResult > vkDeviceWaitIdle = null;
    public unsafe readonly  delegate* unmanaged< VkQueue,UInt32,VkSubmitInfo*,VkFence,VkResult > vkQueueSubmit = null;
    public unsafe readonly  delegate* unmanaged< VkQueue,VkPresentInfoKHR*,VkResult > vkQueuePresentKHR = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,UInt32,VkQueue*,void > vkGetDeviceQueue = null;

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
  

    public readonly uint[] Device_QueueFamilies = new uint[3]{ uint.MaxValue ,uint.MaxValue,uint.MaxValue };
    public readonly uint Device_ImageCount =0;
    public readonly VkDevice Device = VkDevice.Null;
    public readonly VkQueue Device_GraphicQueue = VkQueue.Null;
    public readonly VkQueue Device_PresentQueue = VkQueue.Null;
    public readonly VkQueue Device_ComputeQueue = VkQueue.Null;
    public readonly VkPhysicalDevice Device_Physical = VkPhysicalDevice.Null;
    public readonly VkPhysicalDeviceProperties Device_Properties = new();
    public readonly VkPhysicalDeviceFeatures Device_Features = new();
    public readonly VkPhysicalDeviceMemoryProperties Device_MemoryProperties = new();
    public readonly VkPresentModeKHR Device_PresentMode = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR ;
    public readonly VkFormat Device_ImageFormat = VkFormat.VK_FORMAT_B8G8R8A8_SRGB;
    public readonly VkFormat Device_DepthBufferImageFormat = VkFormat.VK_FORMAT_D32_SFLOAT;
    public readonly VkExtent2D Device_SurfaceSize = new();
    public readonly VkColorSpaceKHR Device_ImageColor = VkColorSpaceKHR.VK_COLOR_SPACE_SRGB_NONLINEAR_KHR ;
    public readonly VkSurfaceTransformFlagBitsKHR Device_Transform = VkSurfaceTransformFlagBitsKHR.VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR;
    

    public DeviceData( ref DeviceConfig config , Window window)
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
            Device_Physical = devices[i];

            // GetPhysicalDeviceInformations( ref func ,ref data,ref config, in window);
            if ( !Device_Physical.IsNull )
                break;

        }
        Guard.ThrowWhenConditionIsTrue( Device_Physical.IsNull , "Physical device is null ");

        Log.Info($"Select Physical device {Device_Physical}");


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

//    Device = new(vkGetDeviceProcAddr, Device );
        vkGetDeviceQueue = (delegate* unmanaged<VkDevice,UInt32,UInt32,VkQueue*,void>) vkGetDeviceProcAddr(Device,nameof(vkGetDeviceQueue)); 
        vkDeviceWaitIdle = (delegate* unmanaged<VkDevice,VkResult>) vkGetDeviceProcAddr(Device,nameof(vkDeviceWaitIdle)); 
        vkQueuePresentKHR = (delegate* unmanaged<VkQueue,VkPresentInfoKHR*,VkResult>) vkGetDeviceProcAddr(Device,nameof(vkQueuePresentKHR)); 

        fixed(VkQueue* gq =&Device_GraphicQueue)
        {
            vkGetDeviceQueue(Device, Device_QueueFamilies[0], 0,gq);
        }
        fixed(VkQueue* pq = &Device_PresentQueue)
        {
            vkGetDeviceQueue(Device, Device_QueueFamilies[2], 0, pq); 
        }
        fixed(VkQueue* cq = &Device_ComputeQueue)
        {
            vkGetDeviceQueue(Device, Device_QueueFamilies[1], 0, cq); 
        }

        Log.Info($"Graphic Queue : indice :{ Device_QueueFamilies[0]}  Adr[{Device_GraphicQueue}]");
        Log.Info($"Compute Queue : indice :{ Device_QueueFamilies[1]}  Adr[{Device_ComputeQueue}]");
        Log.Info($"Present Queue : indice :{ Device_QueueFamilies[2]}  Adr[{Device_PresentQueue}]");


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

    public unsafe void Wait()
    {
        vkQueueWaitIdle(Device_GraphicQueue);
    }

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

}

public readonly struct RenderData
{
    // SwapChain , depth buffer  => For Image View  
    // internal RecreateSwapChain need curent framebuffer
    // Command Pool , Command buffers
    // Synchronization cache control => Fence , Semaphore


    public RenderData( ref RenderConfig config)
    {
      
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

public static class HelperImplements
{
    public static class Memories
    {
        
    }

    public static class Buffers
    {
        
    }

    public static partial class ResourceDecriptor
    {

    }

    public static partial class ResourceCreation
    {
        
    }

}