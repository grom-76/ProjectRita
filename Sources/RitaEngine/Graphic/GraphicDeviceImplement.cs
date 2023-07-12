namespace RitaEngine.Graphic;

using System.IO;
using RitaEngine.API.Vulkan;
using RitaEngine.Base;
using RitaEngine.Base.Debug;
using RitaEngine.Base.Strings;
using RitaEngine.Math;
using RitaEngine.Math.Color;
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
      
    public VkCommandPool CommandPool = VkCommandPool.Null;
    public VkCommandPool CommandPoolCompute = VkCommandPool.Null;
    public VkCommandBuffer[] CommandBuffers = null!;

    public VkSemaphore[] ImageAvailableSemaphores = null!;
    public VkSemaphore[] RenderFinishedSemaphores = null!;
    public VkFence[] InFlightFences =null!;

    public VkRenderPass RenderPass = VkRenderPass.Null;
    public VkRect2D RenderPass_RenderArea = new();
    public VkClearValue[] RenderPass_ClearColors = new VkClearValue[2];

    public VkDescriptorSetLayout DescriptorSetLayout = VkDescriptorSetLayout.Null;
    public VkDescriptorPool DescriptorPool = VkDescriptorPool.Null;
    public VkPipelineLayout PipelineLayout = VkPipelineLayout.Null;
    public VkPipeline Pipeline = VkPipeline.Null;
    public VkDescriptorSet[] DescriptorSets  =  null!;
    

    
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
    public RenderConfig Render = new();
    public SceneLoadConfig SceneLoad = new();

    public PipelineConfig Pipeline = new();

    public GraphicsConfig() { }

    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct RenderConfig
    {
        public int MAX_FRAMES_IN_FLIGHT = 2;
        public ulong Tick_timeout = ulong.MaxValue;
        public Palette BackGroundColor = Palette.LavenderBlush;
        public RenderConfig() { }

    }

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


    public struct SceneLoadConfig
    {
        public Camera Camera = new();
        public short[] Indices = null!;
        public string VertexShaderFileNameSPV ="";
        public string FragmentShaderFileNameSPV ="";
        public string FragmentEntryPoint ="";
        public string VertexEntryPoint="";
        
        public float[] UniformBufferArray = null!;
        public float[] Vertices = null!;
        public string TextureName ="";

        public PushConstantsMesh PushConstants = new();
        public GeometricPrimitive Primitive = new();

        public SceneLoadConfig()  { }
    }

    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct PipelineConfig
    {
        public UsedGeometry UsedGeometry =  UsedGeometry.Position3f_Color3f_UV2f ;//
        public string VertexShaderFileNameSPV ="";
        public string FragmentShaderFileNameSPV ="";
        public string FragmentEntryPoint ="";
        public string VertexEntryPoint="";
        public VkPrimitiveTopology PrimitiveTopology = VkPrimitiveTopology. VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
        public bool              DepthTestEnable = true;
        public bool              DepthWriteEnable = true;
        public CompareOp        DepthCompareOp = CompareOp.VK_COMPARE_OP_LESS;
        public bool              DepthBoundsTestEnable = false;
        public bool              DepthStencilTestEnable=false;
        public VkStencilOpState  DepthFront = new();
        public VkStencilOpState  DepthBack = new();
        public float             DepthMinDepthBounds=0.0f;
        public float             DepthMaxDepthBounds=1.0f;
        public DepthStencilState    DepthFlags = DepthStencilState.VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_DEPTH_ACCESS_BIT_ARM ;
        
        public bool DynamicStatesWithViewport = true;
        public bool DynamicStatesWithScissor = true;
        public bool DynamicStatesWithLineWidth = false;
        public bool DynamicStatesWithPrimitiveTopology = false;
        public bool DynamicStatesWithCullMode = false;
        public VkViewport DynamicStatesViewport = new();
        public VkRect2D DynamicStatesScissor = new();   
        public float DynamicStatesLineWidth =1.0f;

        public SampleCount  RasterizationSamples = SampleCount.VK_SAMPLE_COUNT_1_BIT;
        public bool                   SampleShadingEnable  = false;
        public float                  MinSampleShading=  0.0f;
        public bool                   AlphaToCoverageEnable = false;
        public bool                   AlphaToOneEnable = false ;

        public FaceCullMode FaceCullMode  = FaceCullMode.Back;
        /// <summary>polygonMode is the triangle rendering mode. See VkPolygonMode.  </summary>
        public PolygonFillMode PolygonFillMode = PolygonFillMode.Solid;
        public float LineWidth =1.0f;
        public FrontFace FrontFace = FrontFace.CounterClockwise;
        /// <summary>is a scalar factor controlling the constant depth value added to each fragment. </summary>
        public float DepthBiasConstantFactor=1.0f;
        /// <summary>is the maximum (or minimum) depth bias of a fragment. </summary>
        public float DepthBiasClamp=0.0f;
        /// <summary>is a scalar factor applied to a fragment’s slope in depth bias calculations </summary>
        public float DepthBiasSlopeFactor=1.0f;
        /// <summary>  permet de contrôler si les valeurs de profondeur du fragment doivent être bloquées, comme décrit dans Depth Test (test de profondeur). 
        ///  Si le pipeline n'est pas créé avec VkPipelineRasterizationDepthClipStateCreateInfoEXT présent, l'activation de l'écrêtage de profondeur désactivera également les primitives d'écrêtage sur les plans z du frustrum, comme décrit dans l'écrêtage des primitives.
        /// Sinon, l'écrêtage en profondeur est contrôlé par l'état défini dans VkPipelineRasterizationDepthClipStateCreateInfoEXT.
        /// </summary>
        public bool DepthClampEnable = false;
        /// <summary> depthBiasEnable controls whether to bias fragment depth values </summary>
        public bool DepthBiasEnable = false;
        /// <summary>controls whether primitives are discarded immediately before the rasterization stage. </summary>
        public bool RasterizerDiscardEnable = false;

        public VkBlendFactor ColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        public VkBlendFactor AlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        public VkBlendOp ColorBlendOperation = VkBlendOp.VK_BLEND_OP_ADD;
        public VkBlendOp AlphaBlendOperation = VkBlendOp.VK_BLEND_OP_ADD;

        public VkBlendFactor[] ColorBlendFactors = null!;
        public VkBlendFactor[] AlphaBlendFactors =null!;
        public VkBlendOp[] ColorBlendOperations = null!;
        public VkBlendOp[] AlphaBlendOperations = null!;

        public PipelineConfig()
        {
        }



        #region OVERRIDE    
        public override string ToString() => string.Format($"Depth Stencil" );
        public override int GetHashCode() => HashCode.Combine(DepthTestEnable,DepthWriteEnable ,DepthBoundsTestEnable, DepthMinDepthBounds ,DepthMaxDepthBounds );
        public override bool Equals(object? obj) => obj is PipelineConfig data && this.Equals(data) ;
        public bool Equals(PipelineConfig other)=>  false;
        public static bool operator ==(PipelineConfig left, PipelineConfig right) => left.Equals(right);
        public static bool operator !=(PipelineConfig left, PipelineConfig  right) => !left.Equals(right);
        #endregion
    }

    public enum UsedGeometry
    {
        GeometryInsideShader = -1,
        Position3f_Color3f_UV2f = 0 ,
        Position2f_Color3f =2,
    }

    public enum DepthStencilState 
    {
    // Provided by VK_EXT_rasterization_order_attachment_access
        VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_DEPTH_ACCESS_BIT_EXT = 0x00000001,
    // Provided by VK_EXT_rasterization_order_attachment_access
        VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_STENCIL_ACCESS_BIT_EXT = 0x00000002,
    // Provided by VK_ARM_rasterization_order_attachment_access
        VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_DEPTH_ACCESS_BIT_ARM = VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_DEPTH_ACCESS_BIT_EXT,
    // Provided by VK_ARM_rasterization_order_attachment_access
        VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_STENCIL_ACCESS_BIT_ARM = VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_STENCIL_ACCESS_BIT_EXT,
    }

    public enum StencilOp 
    {
        VK_STENCIL_OP_KEEP = 0,
        VK_STENCIL_OP_ZERO = 1,
        VK_STENCIL_OP_REPLACE = 2,
        VK_STENCIL_OP_INCREMENT_AND_CLAMP = 3,
        VK_STENCIL_OP_DECREMENT_AND_CLAMP = 4,
        VK_STENCIL_OP_INVERT = 5,
        VK_STENCIL_OP_INCREMENT_AND_WRAP = 6,
        VK_STENCIL_OP_DECREMENT_AND_WRAP = 7,
        VK_STENCIL_OP_MAX_ENUM = 0x7FFFFFFF
    }

    public enum CompareOp {
        VK_COMPARE_OP_NEVER = 0,
        VK_COMPARE_OP_LESS = 1,
        VK_COMPARE_OP_EQUAL = 2,
        VK_COMPARE_OP_LESS_OR_EQUAL = 3,
        VK_COMPARE_OP_GREATER = 4,
        VK_COMPARE_OP_NOT_EQUAL = 5,
        VK_COMPARE_OP_GREATER_OR_EQUAL = 6,
        VK_COMPARE_OP_ALWAYS = 7,
        VK_COMPARE_OP_MAX_ENUM = 0x7FFFFFFF
    }

    public  enum SampleCount {
        VK_SAMPLE_COUNT_1_BIT = 0x00000001,
        VK_SAMPLE_COUNT_2_BIT = 0x00000002,
        VK_SAMPLE_COUNT_4_BIT = 0x00000004,
        VK_SAMPLE_COUNT_8_BIT = 0x00000008,
        VK_SAMPLE_COUNT_16_BIT = 0x00000010,
        VK_SAMPLE_COUNT_32_BIT = 0x00000020,
        VK_SAMPLE_COUNT_64_BIT = 0x00000040,
    } 

        /// <summary>  Indicates which face will be culled. </summary>
    public enum FaceCullMode : uint
    {
        /// <summary> The back face. </summary>
        Back = VkCullModeFlagBits.VK_CULL_MODE_BACK_BIT,
        /// <summary> The front face. </summary>
        Front = VkCullModeFlagBits.VK_CULL_MODE_FRONT_BIT,
        /// <summary>  No face culling. </summary>
        None = VkCullModeFlagBits.VK_CULL_MODE_NONE,

        Both = VkCullModeFlagBits.VK_CULL_MODE_FRONT_AND_BACK
    }

    /// <summary> Indicates how the rasterizer will fill polygons. </summary>
    public enum PolygonFillMode : uint
    {
        /// <summary> Polygons are filled completely. </summary>
        Solid = VkPolygonMode.VK_POLYGON_MODE_FILL,
        /// <summary> Polygons are outlined in a "wireframe" style. </summary>
        Wireframe = VkPolygonMode.VK_POLYGON_MODE_LINE,
        /// <summary> specifies that polygon vertices are drawn as points. </summary>
        Point = VkPolygonMode.VK_POLYGON_MODE_POINT,
    }

    /// <summary>  The winding order used to determine the front face of a primitive. </summary>
    public enum FrontFace :uint
    {
        /// <summary>  Clockwise winding order. specifies that a triangle with negative area is considered front-facing.
        /// any triangle which is not front-facing is back-facing, including zero-area triangles. </summary>
        Clockwise =VkFrontFace.VK_FRONT_FACE_CLOCKWISE,
        /// <summary> Counter-clockwise winding order.specifies that a triangle with positive area is considered front-facing. </summary>
        CounterClockwise = VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
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

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Device
{
    public unsafe static void Init(ref VulkanFunctions funcs , ref GraphicsData data, ref GraphicsConfig config , in Window window  )
    {
        funcs.Loader = new( config.Instance.LibraryName_Vulkan);
        Instance.CreateInstanceDebugAndSurface(ref funcs, ref data , ref config , in window);

        Device.CreateDevicePhysicalLogicQueues(ref funcs, ref data , ref config , in window);        

        data.SwapChain_GetFrameBufferCallback = window.GetFrameBuffer;
        SwapChain.CreateSwapChain(ref funcs,ref data, ref config.SwapChain);
        SwapChain.CreateSwapChainImages(ref funcs, ref data, ref config.SwapChain);
        SwapChain.CreateDepthResources( ref funcs, ref data, ref config.SwapChain);
        // RenderPass. CreateFramebuffers(ref func, ref data);
    }

    public unsafe static void Dispose(ref VulkanFunctions func,ref GraphicsData data)
    {
        if ( data.SwapChain == VkSwapchainKHR.Null)return ;

        SynchronizationCachControl.  Pause(ref func,ref data);
        SwapChain.DisposeDepthResources(ref  func , ref data);
        // RenderPass.DisposeFrameBuffer(ref func, ref data);
        SwapChain.DisposeSwapChain(ref func , ref data);

        Device.DisposeDevicePhysicalLogicQueues(ref func , ref data);

        Instance.DisposeInstanceDebugAndSurface(ref func ,ref data);
        func.Dispose();   
    }

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
        // VkDeviceQueueCreateFlagBits.VK_DEVICE_QUEUE_CREATE_PROTECTED_BIT

// TEST queue Family 2 
        // uint graphic = uint.MaxValue; uint present = uint.MaxValue; uint compute = uint.MaxValue; uint transfert = uint.MaxValue;
        // uint queueFamilyPropertyCount2 = 0;
        // func.Instance.vkGetPhysicalDeviceQueueFamilyProperties2(data.Device_Physical, &queueFamilyPropertyCount2, null);

        // ReadOnlySpan<VkQueueFamilyProperties2> queueFamilyProperties2 = new VkQueueFamilyProperties2[queueFamilyPropertyCount2];
        // fixed (VkQueueFamilyProperties2* queueFamilyPropertiesPtr2 = queueFamilyProperties2){
        //     func.Instance.vkGetPhysicalDeviceQueueFamilyProperties2(data.Device_Physical, &queueFamilyPropertyCount2, queueFamilyPropertiesPtr2);
        // }
        // if( data.Device_Properties.deviceType ==   VkPhysicalDeviceType.VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU ) 
        // {
        //     //Always better
        // }
// ----------------------------------------------
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
        // fixed ( VkPhysicalDeviceMemoryProperties* mem = &data.Device_MemoryProperties )
        // {
        //     func.Instance.vkGetPhysicalDeviceMemoryProperties(data.Device_Physical, mem);
        // }
        // if vulkan >= ver 1.2

        VkPhysicalDeviceMemoryProperties2 mem2 = default;
        func.Instance.vkGetPhysicalDeviceMemoryProperties2(data.Device_Physical, &mem2);
        data.Device_MemoryProperties = mem2.memoryProperties;

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
    public unsafe static void ReCreateSwapChain(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.SwapChainConfig config)
    {
        if ( data.SwapChain == VkSwapchainKHR.Null)return ;

        data.SwapChain_GetFrameBufferCallback(ref data.Device_SurfaceSize.width ,ref data.Device_SurfaceSize.height );

        SynchronizationCachControl. Pause(ref func,ref data);
        DisposeDepthResources(ref  func , ref data);
        RenderPass.DisposeFrameBuffer(ref func, ref data);
        DisposeSwapChain(ref func , ref data);
       
        CreateSwapChain(ref func,ref data, ref config);
        CreateSwapChainImages(ref func, ref data, ref config);
        CreateDepthResources( ref func, ref data, ref config);
        RenderPass.CreateFramebuffers(ref func, ref data);
    }

    public static unsafe void CreateSwapChain(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.SwapChainConfig config)
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

        fixed (VkSwapchainKHR* swapchainPtr = &data.SwapChain)
        {
            func.Device.vkCreateSwapchainKHR(data.Device, &createInfo, null, swapchainPtr).Check("failed to create swap chain!");
        }

        Log.Info($"Create SwapChain {data.SwapChain}");
    }

    public static unsafe void CreateSwapChainImages(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.SwapChainConfig config )
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
            ResourceCreation.ImageViewConfig temp = new( data.SwapChain_Images[i],data.Device_ImageFormat, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT);
            ResourceCreation.CreateImageView( ref func,ref data, in temp, out data.SwapChain_ImageViews[i]);
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

    public unsafe static void CreateDepthResources(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.SwapChainConfig config)
    {

        ResourceCreation.CreateImage2( ref func ,ref data, ref data.SwapChain_DepthBufferImages,ref data.SwapChain_DepthBufferImageMemory,
        new(data.Device_SurfaceSize.width,data.Device_SurfaceSize.height, 1) , data.Device_DepthBufferImageFormat,VkImageTiling.VK_IMAGE_TILING_OPTIMAL  , VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT  
        ,VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT);
        
        // FOR FRAMEBUFFER
        ResourceCreation.ImageViewConfig temp = new( data.SwapChain_DepthBufferImages, data.Device_DepthBufferImageFormat, VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT,0);
        ResourceCreation.CreateImageView( ref func,ref data, in temp, out data.SwapChain_DepthBufferImageViews);
    }

    public unsafe static void DisposeDepthResources(ref VulkanFunctions func,ref GraphicsData data )
    {
        if (  !data.Device.IsNull && data.SwapChain_DepthBufferImageViews != VkImageView.Null )
        {
            func.Device.vkDestroyImageView(data.Device,data.SwapChain_DepthBufferImageViews, null);
        }
        if (  !data.Device.IsNull && data.SwapChain_DepthBufferImageMemory != VkDeviceMemory.Null)
        {
            func.Device.vkFreeMemory(data.Device, data.SwapChain_DepthBufferImageMemory, null);
        }
        if( !data.Device.IsNull && data.SwapChain_DepthBufferImages != VkImage.Null )
        {
            func.Device.vkDestroyImage(data.Device,data.SwapChain_DepthBufferImages, null);
        }
    }

}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Memories
{
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

    public static unsafe void AllocateMemoryForBuffer(ref VulkanFunctions func, ref GraphicsData data, VkMemoryPropertyFlagBits properties,ref  VkBuffer buffer, ref  VkDeviceMemory bufferMemory )
    {
        VkMemoryRequirements memRequirements = new();
        func.Device.vkGetBufferMemoryRequirements(data.Device, buffer, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = Memories.FindMemoryType(ref func, ref data, memRequirements.memoryTypeBits, properties);
        allocInfo.pNext = null;
        fixed(VkDeviceMemory* memory =  &bufferMemory) 
        {
            func.Device.vkAllocateMemory(data.Device, &allocInfo, null, memory).Check("failed to allocate memory!");
        }
    }

    public unsafe static void AllocateMemoryForImage( ref VulkanFunctions func, ref GraphicsData data,
       VkImage Image,VkMemoryPropertyFlagBits MemoryProperties , out VkDeviceMemory imageMemory )
    {
        VkMemoryRequirements memRequirements;
        func.Device.vkGetImageMemoryRequirements(data.Device, Image, &memRequirements);

        VkMemoryAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO;
        allocInfo.allocationSize = memRequirements.size;
        allocInfo.memoryTypeIndex = Memories.FindMemoryType(ref func , ref data, memRequirements.memoryTypeBits,MemoryProperties);

        fixed (VkDeviceMemory* imgMem = &imageMemory  )
        {
            func.Device.vkAllocateMemory(data.Device, &allocInfo, null, imgMem).Check("failed to allocate image memory!");
        }

    }

    public unsafe static void TransfertMemory(ref VulkanFunctions func,ref GraphicsData data,void* ptr , VkDeviceSize bufferSize,ref VkDeviceMemory stagingBufferMemory, bool unmap =true)
    {
        void* indicesdataPtr = null;

        func.Device.vkMapMemory(data.Device, stagingBufferMemory, 0, bufferSize, 0, &indicesdataPtr ).Check("Impossible to map memory  for indice");

        Unsafe.CopyBlock( indicesdataPtr , ptr ,(uint)bufferSize);
        
        if (unmap)func.Device.vkUnmapMemory(data.Device, stagingBufferMemory);
    }
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class CommandBuffers
{

    public static unsafe void CreateCommandPool(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config  ) 
    {
        VkCommandPoolCreateInfo poolInfo = new();
        poolInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
        poolInfo.pNext = null;
        poolInfo.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT;
        poolInfo.queueFamilyIndex =data.Device_QueueFamilies[0];

        /*
        K_COMMAND_POOL_CREATE_TRANSIENT_BIT specifies that command buffers allocated from the pool will be short-lived, meaning that they will be reset or freed in a relatively short timeframe. This flag may be used by the implementation to control memory allocation behavior within the pool.
        spécifie que les tampons de commande alloués à partir du pool seront de courte durée, ce qui signifie qu'ils seront réinitialisés ou libérés dans un délai relativement court. Cet indicateur peut être utilisé par l'implémentation pour contrôler le comportement de l'allocation de mémoire au sein du pool.
VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT allows any command buffer allocated from a pool to be individually reset to the initial state; either by calling vkResetCommandBuffer, or via the implicit reset when calling vkBeginCommandBuffer. If this flag is not set on a pool, then vkResetCommandBuffer must not be called for any command buffer allocated from that pool.
permet à tout tampon de commande alloué à partir d'un pool d'être individuellement réinitialisé à l'état initial, soit en appelant vkResetCommandBuffer, soit via la réinitialisation implicite lors de l'appel à vkBeginCommandBuffer. Si ce drapeau n'est pas activé pour un pool, vkResetCommandBuffer ne doit pas être appelé pour un tampon de commande alloué à partir de ce pool.
VK_COMMAND_POOL_CREATE_PROTECTED_BIT specifies that command buffers allocated from the pool are protected command buffers.
        */

        fixed( VkCommandPool* pool =  &data.CommandPool)
        {
            func.Device.vkCreateCommandPool(data.Device, &poolInfo, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool {data.CommandPool}  with {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");

        VkCommandPoolCreateInfo poolInfoCompute = new();
        poolInfoCompute.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO;
        poolInfoCompute.flags = (uint)VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_TRANSIENT_BIT;
        poolInfoCompute.queueFamilyIndex =data.Device_QueueFamilies[1];

        fixed( VkCommandPool* pool =  &data.CommandPoolCompute)
        {
            func.Device.vkCreateCommandPool(data.Device, &poolInfoCompute, null, pool ).Check("failed to create command pool!");
        }

        Log.Info($"Create Command Pool {data.CommandPoolCompute}  with {VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT}");
    }

    public unsafe static void DisposeCommandPool(ref VulkanFunctions func,ref GraphicsData data  )
    {
        if (!data.Device.IsNull && !data.CommandPool.IsNull)
        {
            Log.Info($"Destroy Command Pool {data.CommandPool}");
            func.Device.vkDestroyCommandPool(data.Device, data.CommandPool , null);
        }
        if (!data.Device.IsNull && !data.CommandPoolCompute.IsNull)
        {
            Log.Info($"Destroy Command Pool {data.CommandPoolCompute}");
            func.Device.vkDestroyCommandPool(data.Device, data.CommandPoolCompute , null);
        }
    }

    public static unsafe void CreateCommandBuffer(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config ) 
    {
        data.CommandBuffers = new VkCommandBuffer[config.Render.MAX_FRAMES_IN_FLIGHT]; 

        VkCommandBufferAllocateInfo allocInfo =new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.commandPool = data.CommandPool;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandBufferCount = (uint)data.CommandBuffers.Length;
        
        fixed(VkCommandBuffer* commandBuffer = &data.CommandBuffers[0] )
        {
            func.Device.vkAllocateCommandBuffers(data.Device, &allocInfo, commandBuffer ).Check("failed to allocate command buffers!"); 
        }

        Log.Info($"Create Allocate Command buffer count : {config.Render.MAX_FRAMES_IN_FLIGHT}");
    }
    
    /// <summary>
    /// // RECORD COMMANDS WICH CAN THEN BE SUBMITED TO DEVICE QUEUE FOR EXECUTION
    /// </summary>
    public static unsafe void RecordCommandBuffer(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.PipelineConfig config, in VkCommandBuffer commandBuffer, uint imageIndex)
    {
// START A THREAD  ?
        func.Device.vkResetCommandBuffer(commandBuffer, (uint)VkCommandBufferResetFlagBits.VK_COMMAND_BUFFER_RESET_RELEASE_NONE);
        
        VkCommandBufferBeginInfo beginInfo = default;
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO; 
        beginInfo.pNext =null;
        beginInfo.flags =(uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT;
        beginInfo.pInheritanceInfo= null;
        
        func.Device.vkBeginCommandBuffer(commandBuffer, &beginInfo).Check("Failed to Begin command buffer");

        RenderPass.DrawRenderPass( ref func , ref data, ref config, commandBuffer, imageIndex);

        func.Device.vkEndCommandBuffer(commandBuffer).Check("Failed to End command buffer ");
    }
    
    public unsafe static VkCommandBuffer BeginSingleTimeCommands(ref VulkanFunctions func,ref GraphicsData data)
    {
        
        VkCommandBufferAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO;
        allocInfo.level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY;
        allocInfo.commandPool = data.CommandPool;
        allocInfo.commandBufferCount = 1;

        VkCommandBuffer commandBuffer = VkCommandBuffer.Null;

        func.Device.vkAllocateCommandBuffers(data.Device, &allocInfo, &commandBuffer);

        VkCommandBufferBeginInfo beginInfo = new();
        beginInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO;
        beginInfo.flags =(uint) VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT;

        func.Device.vkBeginCommandBuffer(commandBuffer, &beginInfo);

        return commandBuffer;
    }

    public unsafe static void EndSingleTimeCommands(ref VulkanFunctions func,ref GraphicsData data, VkCommandBuffer commandBuffer) 
    {
        func.Device.vkEndCommandBuffer(commandBuffer);
       
        VkSubmitInfo submitInfo = new();
        submitInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO;
        submitInfo.commandBufferCount = 1;
        submitInfo.pCommandBuffers =&commandBuffer;
        
        func.Device.vkQueueSubmit(data.Device_GraphicQueue, 1, &submitInfo, VkFence.Null);
        func.Device.vkQueueWaitIdle(data.Device_GraphicQueue);

        func.Device.vkFreeCommandBuffers(data.Device, data.CommandPool, 1, &commandBuffer);
    }

}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class SynchronizationCachControl
{
    public unsafe static void Pause(ref VulkanFunctions func,ref GraphicsData data   )
    {
        if ( !data.Device.IsNull)
        {
            func.Device.vkDeviceWaitIdle(data.Device).Check($"WAIT IDLE VkDevice : {data.Device}");
        }
    }

    /// <summary>
    /// Wait for a queue to become idle . To wait on the host for the completion of outstanding queue operations for a given queue
    /// </summary>
    /// <param name="func"></param>
    /// <param name="data"></param>
    public unsafe static void Wait(ref VulkanFunctions func,ref GraphicsData data)
        => func.Device.vkQueueWaitIdle(data.Device_GraphicQueue);

    public static unsafe void CreateSyncObjects(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config   )
    {
        data.ImageAvailableSemaphores = new VkSemaphore[config.Render.MAX_FRAMES_IN_FLIGHT]; 
        data.RenderFinishedSemaphores = new VkSemaphore[config.Render.MAX_FRAMES_IN_FLIGHT];
        data.InFlightFences = new VkFence[config.Render.MAX_FRAMES_IN_FLIGHT];

        VkSemaphoreCreateInfo semaphoreInfo =new();
        semaphoreInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO;
        semaphoreInfo.flags =0;
        semaphoreInfo.pNext =null;

        VkFenceCreateInfo fenceInfo= new();
        fenceInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_FENCE_CREATE_INFO;
        fenceInfo.flags = (uint)VkFenceCreateFlagBits.VK_FENCE_CREATE_SIGNALED_BIT;


        for (int i = 0; i < config.Render.MAX_FRAMES_IN_FLIGHT; i++)
        {
            fixed( VkSemaphore* imageAvailableSemaphore = &data.ImageAvailableSemaphores[i])
            {
                func.Device.vkCreateSemaphore(data.Device, &semaphoreInfo, null,  imageAvailableSemaphore).Check("Failed to create Semaphore ImageAvailableSemaphore");
            }
            Log.Info($"-{i}  Create Semaphore Image Available : {data.ImageAvailableSemaphores[i]}");
            
            fixed( VkSemaphore* renderFinishedSemaphore = &data.RenderFinishedSemaphores[i])
            {
                func.Device.vkCreateSemaphore(data.Device, &semaphoreInfo, null, renderFinishedSemaphore).Check("Failed to create Semaphore RenderFinishedSemaphore");
            }
            Log.Info($"-{i}  Create Semaphore render Finish : {data.RenderFinishedSemaphores[i]}");
            
            fixed(VkFence*  inFlightFence = &data.InFlightFences[i] )
            {
                func.Device.vkCreateFence(data.Device, &fenceInfo, null, inFlightFence).Check("Failed to create Fence InFlightFence");
            }
            Log.Info($"-{i}  Create Fence  : {data.InFlightFences[i]}");
        }
    }

    public static unsafe void DisposeSyncObjects(ref VulkanFunctions func,ref GraphicsData data)
    {
        if (  !data.Device.IsNull && data.RenderFinishedSemaphores != null){
            for ( int i = 0 ; i< data.RenderFinishedSemaphores.Length ; i++)
            {
                if ( !data.RenderFinishedSemaphores[i].IsNull)
                {
                    Log.Info($"-{i}  Create Semaphore render Finish : {data.RenderFinishedSemaphores[i]}");
                    func.Device.vkDestroySemaphore(data.Device, data.RenderFinishedSemaphores[i], null);
                }
            }
            data.RenderFinishedSemaphores = null!;
        }

        if (  !data.Device.IsNull && data.ImageAvailableSemaphores != null){
            for ( int i = 0 ; i< data.ImageAvailableSemaphores.Length ; i++)
            {
                if ( !data.ImageAvailableSemaphores[i].IsNull)
                {
                    Log.Info($"-{i}  Create Semaphore Image Available : {data.ImageAvailableSemaphores[i]}");
                    func.Device.vkDestroySemaphore(data.Device, data.ImageAvailableSemaphores[i], null);
                }
            }
            data.ImageAvailableSemaphores = null!;
        }

        if (  !data.Device.IsNull && data.InFlightFences != null){
            for ( int i = 0 ; i< data.InFlightFences.Length ; i++)
            {
                if ( !data.InFlightFences[i].IsNull)
                {
                    Log.Info($"-{i}  Create Fence  : {data.InFlightFences[i]}");
                    func.Device.vkDestroyFence(data.Device,data.InFlightFences[i], null);
                }
            }
            data.InFlightFences = null!;
        }
    }

    public readonly struct TransitionImageLayoutConfig
    {
        public readonly VkCommandBuffer CommandBuffer;
        public readonly  VkImage Image;
        public readonly uint BaseMipLevel;
        public readonly uint LevelCount;
        public readonly uint BaseArrayLayer;
        public readonly uint LayerCount;
        public readonly  VkImageAspectFlagBits AspectMask;
        // public readonly  VkImageLayout OldLayout;
        // public readonly  VkImageLayout NewLayout;

        public TransitionImageLayoutConfig( VkCommandBuffer cb,  VkImage image,
        uint baseMipLevel,uint levelCount,uint baseArrayLayer, uint layerCount, VkImageAspectFlagBits aspectMask)
        {
            CommandBuffer = cb;
            Image = image;
            BaseMipLevel = baseMipLevel;
            BaseArrayLayer = baseArrayLayer;
            LevelCount = levelCount;
            LayerCount = layerCount;
            AspectMask = aspectMask;
            // OldLayout = oldLayout;
            // NewLayout = newLayout;

        }
    }

    //  MEMORY BARRIER source from : https://github.com/veldrid/veldrid/blob/master/src/Veldrid/Vk/VulkanUtil.cs
    public unsafe  static void TransitionImageLayout(ref VulkanFunctions func,in TransitionImageLayoutConfig config,  VkImageLayout oldLayout,  VkImageLayout newLayout)
    {
//         Debug.Assert(oldLayout != newLayout);
        VkImageMemoryBarrier barrier = default;
        barrier.sType =VkStructureType. VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
        barrier.pNext = null;
        barrier.oldLayout = oldLayout;
        barrier.newLayout = newLayout;
        barrier.srcQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
        barrier.dstQueueFamilyIndex = VK.VK_QUEUE_FAMILY_IGNORED;
        barrier.image = config.Image;
        barrier.subresourceRange.aspectMask = (uint)config.AspectMask;
        barrier.subresourceRange.baseMipLevel = config.BaseMipLevel;
        barrier.subresourceRange.levelCount = config.LevelCount;
        barrier.subresourceRange.baseArrayLayer = config.BaseArrayLayer;
        barrier.subresourceRange.layerCount = config.LayerCount;

        VkPipelineStageFlagBits srcStageFlags =VkPipelineStageFlagBits. VK_PIPELINE_STAGE_NONE;
        VkPipelineStageFlagBits dstStageFlags = VkPipelineStageFlagBits. VK_PIPELINE_STAGE_NONE;

        if ((oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED || oldLayout == VkImageLayout.VK_IMAGE_LAYOUT_PREINITIALIZED) && newLayout == VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL)
        {
            barrier.srcAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_NONE;
            barrier.dstAccessMask = (uint)VkAccessFlagBits.VK_ACCESS_TRANSFER_WRITE_BIT;
            srcStageFlags = VkPipelineStageFlagBits. VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
            dstStageFlags = VkPipelineStageFlagBits .VK_PIPELINE_STAGE_TRANSFER_BIT;
        }
       
//         else if (oldLayout == VkImageLayout.ShaderReadOnlyOptimal && newLayout == VkImageLayout.TransferSrcOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ShaderRead;
//             barrier.dstAccessMask = VkAccessFlags.TransferRead;
//             srcStageFlags = VkPipelineStageFlags.FragmentShader;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.ShaderReadOnlyOptimal && newLayout == VkImageLayout.TransferDstOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ShaderRead;
//             barrier.dstAccessMask = VkAccessFlags.TransferWrite;
//             srcStageFlags = VkPipelineStageFlags.FragmentShader;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.Preinitialized && newLayout == VkImageLayout.TransferSrcOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.None;
//             barrier.dstAccessMask = VkAccessFlags.TransferRead;
//             srcStageFlags = VkPipelineStageFlags.TopOfPipe;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.Preinitialized && newLayout == VkImageLayout.General)
//         {
//             barrier.srcAccessMask = VkAccessFlags.None;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.TopOfPipe;
//             dstStageFlags = VkPipelineStageFlags.ComputeShader;
//         }
//         else if (oldLayout == VkImageLayout.Preinitialized && newLayout == VkImageLayout.ShaderReadOnlyOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.None;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.TopOfPipe;
//             dstStageFlags = VkPipelineStageFlags.FragmentShader;
//         }
//         else if (oldLayout == VkImageLayout.General && newLayout == VkImageLayout.ShaderReadOnlyOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferRead;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.FragmentShader;
//         }
//         else if (oldLayout == VkImageLayout.ShaderReadOnlyOptimal && newLayout == VkImageLayout.General)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ShaderRead;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.FragmentShader;
//             dstStageFlags = VkPipelineStageFlags.ComputeShader;
//         }

//         else if (oldLayout == VkImageLayout.TransferSrcOptimal && newLayout == VkImageLayout.ShaderReadOnlyOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferRead;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.FragmentShader;
//         }
//         else if (oldLayout == VkImageLayout.TransferDstOptimal && newLayout == VkImageLayout.ShaderReadOnlyOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferWrite;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.FragmentShader;
//         }
//         else if (oldLayout == VkImageLayout.TransferSrcOptimal && newLayout == VkImageLayout.TransferDstOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferRead;
//             barrier.dstAccessMask = VkAccessFlags.TransferWrite;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.TransferDstOptimal && newLayout == VkImageLayout.TransferSrcOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferWrite;
//             barrier.dstAccessMask = VkAccessFlags.TransferRead;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.ColorAttachmentOptimal && newLayout == VkImageLayout.TransferSrcOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ColorAttachmentWrite;
//             barrier.dstAccessMask = VkAccessFlags.TransferRead;
//             srcStageFlags = VkPipelineStageFlags.ColorAttachmentOutput;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.ColorAttachmentOptimal && newLayout == VkImageLayout.TransferDstOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ColorAttachmentWrite;
//             barrier.dstAccessMask = VkAccessFlags.TransferWrite;
//             srcStageFlags = VkPipelineStageFlags.ColorAttachmentOutput;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.ColorAttachmentOptimal && newLayout == VkImageLayout.ShaderReadOnlyOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ColorAttachmentWrite;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.ColorAttachmentOutput;
//             dstStageFlags = VkPipelineStageFlags.FragmentShader;
//         }
//         else if (oldLayout == VkImageLayout.DepthStencilAttachmentOptimal && newLayout == VkImageLayout.ShaderReadOnlyOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.DepthStencilAttachmentWrite;
//             barrier.dstAccessMask = VkAccessFlags.ShaderRead;
//             srcStageFlags = VkPipelineStageFlags.LateFragmentTests;
//             dstStageFlags = VkPipelineStageFlags.FragmentShader;
//         }
//         else if (oldLayout == VkImageLayout.ColorAttachmentOptimal && newLayout == VkImageLayout.PresentSrcKHR)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ColorAttachmentWrite;
//             barrier.dstAccessMask = VkAccessFlags.MemoryRead;
//             srcStageFlags = VkPipelineStageFlags.ColorAttachmentOutput;
//             dstStageFlags = VkPipelineStageFlags.BottomOfPipe;
//         }
//         else if (oldLayout == VkImageLayout.TransferDstOptimal && newLayout == VkImageLayout.PresentSrcKHR)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferWrite;
//             barrier.dstAccessMask = VkAccessFlags.MemoryRead;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.BottomOfPipe;
//         }
//         else if (oldLayout == VkImageLayout.TransferDstOptimal && newLayout == VkImageLayout.ColorAttachmentOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferWrite;
//             barrier.dstAccessMask = VkAccessFlags.ColorAttachmentWrite;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.ColorAttachmentOutput;
//         }
//         else if (oldLayout == VkImageLayout.TransferDstOptimal && newLayout == VkImageLayout.DepthStencilAttachmentOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.TransferWrite;
//             barrier.dstAccessMask = VkAccessFlags.DepthStencilAttachmentWrite;
//             srcStageFlags = VkPipelineStageFlags.Transfer;
//             dstStageFlags = VkPipelineStageFlags.LateFragmentTests;
//         }
//         else if (oldLayout == VkImageLayout.General && newLayout == VkImageLayout.TransferSrcOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ShaderWrite;
//             barrier.dstAccessMask = VkAccessFlags.TransferRead;
//             srcStageFlags = VkPipelineStageFlags.ComputeShader;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.General && newLayout == VkImageLayout.TransferDstOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.ShaderWrite;
//             barrier.dstAccessMask = VkAccessFlags.TransferWrite;
//             srcStageFlags = VkPipelineStageFlags.ComputeShader;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
//         else if (oldLayout == VkImageLayout.PresentSrcKHR && newLayout == VkImageLayout.TransferSrcOptimal)
//         {
//             barrier.srcAccessMask = VkAccessFlags.MemoryRead;
//             barrier.dstAccessMask = VkAccessFlags.TransferRead;
//             srcStageFlags = VkPipelineStageFlags.BottomOfPipe;
//             dstStageFlags = VkPipelineStageFlags.Transfer;
//         }
        else 
        {
            Guard.ThrowWhenConditionIsTrue(true,"unsupported layout transition!");
        }

        func.Device.vkCmdPipelineBarrier(
            config.CommandBuffer,
            (uint)srcStageFlags,
            (uint)dstStageFlags,
            0/*VkDependencyFlagBits.NONE*/,
            0, null,
            0, null,
            1, &barrier);

    }

}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Render
{
    
    public static unsafe void Init(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
        CommandBuffers.CreateCommandPool(ref func , ref data, ref config);
        CommandBuffers.CreateCommandBuffer(ref func , ref data,ref config);
        SynchronizationCachControl.CreateSyncObjects(ref func , ref data,ref config);
    }

    public unsafe static void Dispose(ref VulkanFunctions func,ref GraphicsData data)
    {
        SynchronizationCachControl.DisposeSyncObjects(ref func , ref data);
        CommandBuffers.DisposeCommandPool(ref func , ref data);
    }

    public static unsafe  void Draw(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
// PREPARE A FRAME         
        uint imageIndex=0;
        VkFence CurrentinFlightFence = data.InFlightFences[/*data.*/CurrentFrame];
        VkSemaphore CurrentImageAvailableSemaphore =  data.ImageAvailableSemaphores[/*data.*/CurrentFrame];
        VkSemaphore CurrentRenderFinishedSemaphore = data.RenderFinishedSemaphores[/*data.*/CurrentFrame];
        VkSemaphore* waitSemaphores = stackalloc VkSemaphore[1] {CurrentImageAvailableSemaphore};// VkSemaphore[] waitSemaphores = {CurrentImageAvailableSemaphore};
        UInt32* waitStages  = stackalloc UInt32[1]{(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT }; //*VkPipelineStageFlags*/UInt32[] waitStages = {(uint)VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT};
        VkSemaphore*   signalSemaphores  = stackalloc VkSemaphore[1] {CurrentRenderFinishedSemaphore} ;// VkSemaphore[] signalSemaphores = {CurrentRenderFinishedSemaphore};
        VkSwapchainKHR* swapChains = stackalloc  VkSwapchainKHR[1]{ data.SwapChain };// VkSwapchainKHR[] swapChains = { data.SwapChain };
        VkCommandBuffer commandBuffer =data.CommandBuffers[/*data.*/CurrentFrame];


//SYNCHRONISATION OBJECTS :  USED TO CONTROL THE ORDER OF EXECUTION OF ASYNCHRONOUS OPERATION
        func.Device.vkWaitForFences(data.Device, 1,&CurrentinFlightFence, VK.VK_TRUE, config.Render.Tick_timeout).Check("Acquire Image");

// ACQUIRE NEXT IMAGE 
        VkResult result = func.Device.vkAcquireNextImageKHR(data.Device, data.SwapChain, config.Render.Tick_timeout,CurrentImageAvailableSemaphore, VkFence.Null, &imageIndex);

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
        {
            SwapChain. ReCreateSwapChain( ref func,ref data, ref config.SwapChain);
            return ;
        }
        else if (result != VkResult.VK_SUCCESS && result != VkResult.VK_SUBOPTIMAL_KHR )
        {
            throw new Exception("Failed to acquire swap chain Images");
        }

        // UpdateUniformBuffer( func,ref  data);
                
        func.Device.vkResetFences(data.Device, 1, &CurrentinFlightFence);


// RECORD COMMANDS WICH CAN THEN BE SUBMITED TO DEVICE QUEUE FOR EXECUTION
        CommandBuffers. RecordCommandBuffer( ref func,ref data,ref config.Pipeline, commandBuffer, imageIndex);

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
// SUBMIT GRAPHICS COMMAND BUFFERS        
        func.Device.vkQueueSubmit(data.Device_GraphicQueue, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
        
        VkPresentInfoKHR presentInfo =  default;
        presentInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR; 
        presentInfo.waitSemaphoreCount = 1;
        presentInfo.pWaitSemaphores = signalSemaphores;
        presentInfo.pImageIndices = &imageIndex;
        presentInfo.swapchainCount = 1;
        presentInfo.pSwapchains = swapChains;
        presentInfo.pNext =null;
        presentInfo.pResults = null;
// PRESENT IMAGE        
        result = func.Device.vkQueuePresentKHR(data.Device_PresentQueue, &presentInfo); 

        if ( result == VkResult.VK_ERROR_OUT_OF_DATE_KHR || result == VkResult.VK_SUBOPTIMAL_KHR )
        {
            SwapChain. ReCreateSwapChain( ref func,ref data, ref config.SwapChain);
        }
        else if (result != VkResult.VK_SUCCESS )
        {
            throw new Exception("Failed to  present swap chain Images");
        }
        // FOR NEXT FRAME 
       CurrentFrame = ((CurrentFrame + 1) % config.Render.MAX_FRAMES_IN_FLIGHT);   
    }
   private static int CurrentFrame =0;

}

public unsafe struct QueuesTest
{
    public unsafe readonly  delegate* unmanaged< VkQueue,UInt32,VkSubmitInfo*,VkFence,VkResult > vkQueueSubmit = null;
    public unsafe readonly  delegate* unmanaged< VkQueue,VkPresentInfoKHR*,VkResult > vkQueuePresentKHR = null;
    public unsafe readonly  delegate* unmanaged< VkDevice,UInt32,UInt32,VkQueue*,void > vkGetDeviceQueue = null;
    private readonly VkDevice _device;
    private readonly VkQueue _present;
    private readonly VkQueue _graphic;
    private readonly VkQueue _compute;

    public QueuesTest( VkDevice device , uint[] queueFamiliyIndices )
    {
        _device = device;
        fixed ( VkQueue* graphic = &_graphic){
            vkGetDeviceQueue(_device, queueFamiliyIndices[0], 0, graphic); 
        }
        fixed ( VkQueue* compute = &_compute){
            vkGetDeviceQueue(_device, queueFamiliyIndices[1], 0, compute); 
        }
        fixed ( VkQueue* present = &_present){
            vkGetDeviceQueue(_device, queueFamiliyIndices[2], 0,present); 
        }
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
        vkQueueSubmit(_graphic, 1, &submitInfo,  CurrentinFlightFence ).Check("failed to submit draw command buffer!");
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
        return  vkQueuePresentKHR(_present, &presentInfo); 
    }

    public void Dispose()
    {

    }
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class RenderPass
{
    public unsafe static void DrawRenderPass(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.PipelineConfig config, in VkCommandBuffer commandBuffer , uint imageIndex )
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
        
        func.Device.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);

            Pipelines.DrawPipeline( ref func , ref data , ref config , in commandBuffer);

        func.Device.vkCmdEndRenderPass(commandBuffer);
    //} // END FOREACH RENDER PASS 
        // END RENDER PASS --------------------------------------------------------------------------------------------------  
    }

    public static unsafe void CreateRenderPass(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config) 
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
            func.Device.vkCreateRenderPass(data.Device, &renderPassInfo, null, renderPass).Check("failed to create render pass!");
        }

        Log.Info($"Create Render Pass : {data.RenderPass}");
    }

    public static unsafe void DisposeRenderPass(ref VulkanFunctions func,ref GraphicsData data  )
    {
        if (!data.Device.IsNull && !data.RenderPass.IsNull)
        {
            Log.Info($"Destroy Render Pass : {data.RenderPass}");
            func.Device.vkDestroyRenderPass(data.Device,data.RenderPass,null);
        }
    }

    public static unsafe void CreateFramebuffers( ref VulkanFunctions func,ref GraphicsData data )
    {
        int size= data.SwapChain_ImageViews.Length;
        data.Framebuffers = new VkFramebuffer[size];
    
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

}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class Pipelines
{
    public unsafe static void Load(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
       // Preperae Texture data        
        // VkFormat format = VkFormat.VK_FORMAT_UNDEFINED;
        uint texWidth=512, texHeight=512, texChannels=4;
        // //TODO CreateTextureImage  File.ReadAllBytes( data.Info.TextureName) do this outside 
        // var file = File.ReadAllBytes( data.Info.TextureName);
        // // StbImage.stbi__vertically_flip_on_load_set = 1;
        // ImageResult result = ImageResult.FromMemory(file , ColorComponents.RedGreenBlueAlpha);
        // texWidth = (uint)result.Width;
        // texHeight = (uint)result.Height;
        // texChannels = (uint)result.Comp;
        // if (result.Comp == ColorComponents.RedGreenBlue  )
        //     format = VkFormat.VK_FORMAT_R8G8B8_SRGB;
        // else if (result.Comp == ColorComponents.RedGreenBlueAlpha )    
        //     format = VkFormat.VK_FORMAT_R8G8B8A8_SRGB;

        VkDeviceSize imageSize = (ulong)(texWidth * texHeight * texChannels);
        //--------------------------------------------------   

        // Span<byte> shaderBytes = System.IO.File.ReadAllBytes("");

        //--------------------------------------------------

        data.RenderPass_ClearColors[0] = new(ColorHelper.PaletteToRGBA( config.Render.BackGroundColor));
        data.RenderPass_ClearColors[1] = new(depth:1.0f,stencil:0);
        
        data.RenderPass_RenderArea = new();
        data.RenderPass_RenderArea.offset = new( );
        data.RenderPass_RenderArea.offset.x =0 ;data.RenderPass_RenderArea.offset.x =0 ; //TODO put in config
        data.RenderPass_RenderArea.extent  = data.Device_SurfaceSize;
        // //LoadModel => Do in TransfertConfigRenderTo_data load vertex indeices and textures ....
    }

    public unsafe static void Build(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
        // TODO :  see if redner pass init in Render or pipelines?  
        RenderPass.CreateRenderPass(ref func , ref data, ref config);
        RenderPass.CreateFramebuffers(ref func , ref data);
      
        CommandBuffers.CreateCommandPool( ref func , ref data, ref config);
        CommandBuffers.CreateCommandBuffer( ref func , ref data, ref config);

        ResourceCreation.CreateVertexIndiceBuffer( ref func , ref data , ref config);
        ResourceCreation.CreateTextures( ref func , ref data , ref config);
        // ResourceCreation.CreateTextureImageView
        // ResourceCreation.CreateUniformBuffers( );
        // ResourceCreation.CreateTextureSampler();
   
        ResourceDecriptor.CreateDescriptors( ref func ,ref data , ref config);

        CreatePipeline(ref  func,ref  data , ref config.Pipeline);
    }

    public unsafe static void Dispose(ref VulkanFunctions func,ref GraphicsData data  )
    {
        RenderPass.DisposeFrameBuffer(ref func , ref data);
        RenderPass.DisposeRenderPass(ref func , ref data);

        DisposePipeline(ref  func,ref  data );
    }

    public unsafe static void DrawPipeline(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.PipelineConfig config, in VkCommandBuffer commandBuffer )
    {
        // ALL LINE COMMENT  IS EMPTY PIPELINE 
        // // PUSH CONSTANTS ---------- ( do before bin pipeline)
        // // void* ptr = new IntPtr( data.Info.PushConstants).ToPointer();
        // fixed(void* ptr = &data.Info.PushConstants ){
        //     func.vkCmdPushConstants(commandBuffer,data.Handles.PipelineLayout, (uint) VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT, 0,(uint)sizeof(PushConstantsMesh), ptr );
        // }
        
        // // USE SHADER   FOREACH PIPELINE or FOREACH SHADER
        func.Device.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.Pipeline);
            
        // //  FOREACN  MATERIALS  or FOREACH UNIFORMS & TEXTURES(sampler)
        //     fixed(VkDescriptorSet* desc =  &data.Handles.DescriptorSets[CurrentFrame] )
        //     {
        //         func.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, data.Handles.PipelineLayout, 0, 1, desc, 0, null);
        //     }
            
        // // SET DYNAMIC STATES

        if ( config.DynamicStatesWithViewport && config.DynamicStatesWithScissor ){
            fixed(VkViewport* viewport = &config.DynamicStatesViewport ){ func.Device.vkCmdSetViewport(commandBuffer, 0, 1,viewport); }
            fixed( VkRect2D* scissor = &config.DynamicStatesScissor) { func.Device.vkCmdSetScissor(commandBuffer, 0, 1, scissor); }
        } 
        if ( config.DynamicStatesWithLineWidth){
            func.Device.vkCmdSetLineWidth( commandBuffer,config.DynamicStatesLineWidth);
        } 
      
        // //  FOREACH OBJECT/ GEOMETRY 
        //     VkDeviceSize* offsets = stackalloc VkDeviceSize[]{0};
        //     VkBuffer* vertexBuffers = stackalloc VkBuffer[] { data.Handles.VertexBuffer};
        //     func.vkCmdBindVertexBuffers(commandBuffer, 0, 1, vertexBuffers, offsets);
        //     func.vkCmdBindIndexBuffer(commandBuffer, data.Handles.IndicesBuffer, 0, VkIndexType.VK_INDEX_TYPE_UINT16);
        // // DRAW CALLS  ------------ VERTEX INDEXED  
        //     func.vkCmdDrawIndexed(commandBuffer, data.Handles.IndicesSize, 1, 0, 0, 0);
    }
        
    #region  Compute Pipeline 
    
    public static unsafe void CreatePipeline(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig.PipelineConfig config)
    {
        int shadercount = 0;
        if (  !string.IsNullOrEmpty( config.FragmentShaderFileNameSPV )  ) shadercount++;
        if (  !string.IsNullOrEmpty( config.VertexShaderFileNameSPV )  ) shadercount++;


        // #region SHADERS
        VkShaderModule[] shaderModules = new  VkShaderModule[shadercount];
        VkPipelineShaderStageCreateInfo[] shaderStages = new VkPipelineShaderStageCreateInfo[shadercount] ;

        if (  !string.IsNullOrEmpty( config.FragmentShaderFileNameSPV )  )
        {
            shadercount = shadercount-1;
            ReadOnlySpan<byte> span = File.ReadAllBytes( config.FragmentShaderFileNameSPV  ).AsSpan();
            CreateShaderModule(ref func , ref data , span,out VkShaderModule  shader );
         
            using RitaEngine.Base.Strings.StrUnsafe vertentryPoint = new(config.FragmentEntryPoint);
            shaderStages[shadercount] = new();
            shaderStages[shadercount].sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
            shaderStages[shadercount].stage = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT;
            shaderStages[shadercount].module = shader;
            shaderStages[shadercount].pName = vertentryPoint;
            shaderStages[shadercount].flags =0;
            shaderStages[shadercount].pNext =null;
            shaderStages[shadercount].pSpecializationInfo =null;
        }
        if (  !string.IsNullOrEmpty( config.VertexShaderFileNameSPV )  )
        {
            shadercount = shadercount-1;
            ReadOnlySpan<byte> span = File.ReadAllBytes( config.VertexShaderFileNameSPV ).AsSpan();
            CreateShaderModule(ref func , ref data , span,out VkShaderModule  shader );
         
            using RitaEngine.Base.Strings.StrUnsafe vertentryPoint = new(config.VertexEntryPoint);
            shaderStages[shadercount] = new();
            shaderStages[shadercount].sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO;
            shaderStages[shadercount].stage = VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
            shaderStages[shadercount].module = shader;
            shaderStages[shadercount].pName = vertentryPoint;
            shaderStages[shadercount].flags =0;
            shaderStages[shadercount].pNext =null;
            shaderStages[shadercount].pSpecializationInfo =null;
        }
     
        VkPipelineVertexInputStateCreateInfo vertexInputInfo =new();
        vertexInputInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;
        vertexInputInfo.pNext = null;
        vertexInputInfo.flags =0;


        if ( config.UsedGeometry == 0 )// 0 = PositionColorUV do an enum
        {
             VkVertexInputBindingDescription bindingDescription =new();
            bindingDescription.binding = 0; // layout 
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
          
            vertexInputInfo.pVertexAttributeDescriptions=attributeDescriptions;
            vertexInputInfo.pVertexBindingDescriptions=&bindingDescription;
        }
        else
        {
            vertexInputInfo.vertexBindingDescriptionCount = 0;
            vertexInputInfo.vertexAttributeDescriptionCount = 0;
        }
       
        VkPipelineInputAssemblyStateCreateInfo inputAssembly=new();
        inputAssembly.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
        inputAssembly.topology =config.PrimitiveTopology;
        inputAssembly.primitiveRestartEnable = VK.VK_FALSE;
        inputAssembly.flags =0;
        inputAssembly.pNext =null;       

        VkPipelineColorBlendAttachmentState colorBlendAttachment =new();
        colorBlendAttachment.colorWriteMask = (uint)(VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT);
        colorBlendAttachment.blendEnable = VK.VK_FALSE;
        colorBlendAttachment.srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD;
        colorBlendAttachment.colorBlendOp =  VkBlendOp.VK_BLEND_OP_ADD;
        colorBlendAttachment.dstAlphaBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.dstColorBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        
        VkPipelineColorBlendStateCreateInfo colorBlending=new();
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


        VkPipelineRasterizationStateCreateInfo rasterizer =new();
        rasterizer.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO;
        rasterizer.rasterizerDiscardEnable = config.RasterizerDiscardEnable?   VK.VK_TRUE : VK.VK_FALSE ;// VK.VK_FALSE;
        rasterizer.polygonMode = (VkPolygonMode) config.PolygonFillMode ;//  VkPolygonMode. VK_POLYGON_MODE_FILL;
        rasterizer.lineWidth = config.LineWidth;// 1.0f;
        rasterizer.cullMode =  (uint)config.FaceCullMode ; //( uint)VkCullModeFlagBits.VK_CULL_MODE_BACK_BIT;
        rasterizer.frontFace = (VkFrontFace) config.FrontFace ; //VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE;
        rasterizer.flags =0;
        rasterizer.pNext = null;
        rasterizer.depthBiasEnable = config.DepthBiasEnable? VK.VK_TRUE : VK.VK_FALSE ;// VK.VK_FALSE;
        rasterizer.depthClampEnable = config.DepthClampEnable? VK.VK_TRUE : VK.VK_FALSE ;   // VK.VK_FALSE;
        rasterizer.depthBiasClamp = config.DepthBiasClamp ; // 0.0f;
        rasterizer.depthBiasConstantFactor =config.DepthBiasConstantFactor ;  // 1.0f;
        rasterizer.depthBiasSlopeFactor = config.DepthBiasSlopeFactor ;   //1.0f;

        VkPipelineMultisampleStateCreateInfo multisampling=new();
        multisampling.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO;
        multisampling.pNext = null;
        multisampling.flags =0;
        multisampling.sampleShadingEnable =config.SampleShadingEnable ? VK.VK_TRUE:  VK.VK_FALSE;
        multisampling.rasterizationSamples =(VkSampleCountFlagBits) config.RasterizationSamples ;
        multisampling.alphaToCoverageEnable =  config.AlphaToCoverageEnable ? VK.VK_TRUE:  VK.VK_FALSE;
        multisampling.alphaToOneEnable = config.AlphaToOneEnable  ? VK.VK_TRUE:  VK.VK_FALSE;       
        multisampling.minSampleShading =config.MinSampleShading;
        uint* samplemask = null;
        multisampling.pSampleMask =samplemask;

        VkPipelineDepthStencilStateCreateInfo depthStencilStateCreateInfo=new();
        depthStencilStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
        depthStencilStateCreateInfo.pNext = null;
        depthStencilStateCreateInfo.depthTestEnable = config.DepthTestEnable ?  VK.VK_TRUE : VK.VK_FALSE;
        depthStencilStateCreateInfo.depthWriteEnable =config.DepthWriteEnable  ?  VK.VK_TRUE : VK.VK_FALSE;
        depthStencilStateCreateInfo.depthCompareOp = (VkCompareOp)config.DepthCompareOp;
        depthStencilStateCreateInfo.depthBoundsTestEnable =config.DepthBoundsTestEnable ?  VK.VK_TRUE : VK.VK_FALSE;
        depthStencilStateCreateInfo.stencilTestEnable = config.DepthStencilTestEnable ? VK.VK_TRUE : VK.VK_FALSE;
        depthStencilStateCreateInfo.maxDepthBounds = config.DepthMaxDepthBounds;
        depthStencilStateCreateInfo.minDepthBounds = config.DepthMinDepthBounds;
        depthStencilStateCreateInfo.flags = (uint)config.DepthFlags;
        depthStencilStateCreateInfo.front = config.DepthFront ;
        depthStencilStateCreateInfo.back = config.DepthBack ;

        uint dynamicCount =0 ;
        if ( config.DynamicStatesWithViewport) dynamicCount++;
        if ( config.DynamicStatesWithScissor) dynamicCount++;
        if ( config.DynamicStatesWithLineWidth) dynamicCount++;
        VkPipelineDynamicStateCreateInfo dynamicStateCreateInfo = new();
        dynamicStateCreateInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO;
        dynamicStateCreateInfo.pNext = null;
        dynamicStateCreateInfo.flags =0;
        dynamicStateCreateInfo.dynamicStateCount =dynamicCount;
        dynamicStateCreateInfo.pDynamicStates = null;
        if ( dynamicCount != 0)
        {
            VkDynamicState[] dynamicStates2  = new VkDynamicState[dynamicCount];
            if ( config.DynamicStatesWithViewport) dynamicStates2[--dynamicCount] = VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT;
            if ( config.DynamicStatesWithScissor) dynamicStates2[--dynamicCount] = VkDynamicState.VK_DYNAMIC_STATE_SCISSOR;
            if ( config.DynamicStatesWithLineWidth) dynamicStates2[--dynamicCount] = VkDynamicState.VK_DYNAMIC_STATE_LINE_WIDTH;
            fixed( VkDynamicState* dynamicStates = &dynamicStates2[0] ){
                dynamicStateCreateInfo.pDynamicStates =dynamicStates;
            }
        }

        config.DynamicStatesViewport.x = 0.0f;
        config.DynamicStatesViewport.y = 0.0f;
        config.DynamicStatesViewport.width = (float) data.Device_SurfaceSize.width;
        config.DynamicStatesViewport.height = (float) data.Device_SurfaceSize.height;
        config.DynamicStatesViewport.minDepth = 0.0f;
        config.DynamicStatesViewport.maxDepth = 1.0f;
        config.DynamicStatesScissor.offset = new();
        config.DynamicStatesScissor.extent = data.Device_SurfaceSize ;
        VkPipelineViewportStateCreateInfo viewportState =new();
        viewportState.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO;
        viewportState.viewportCount = 1;
        fixed( VkViewport* viewport = &config.DynamicStatesViewport)
        {    
            viewportState.pViewports =viewport;    
        }
        viewportState.scissorCount = 1;
        fixed( VkRect2D* scissor = &config.DynamicStatesScissor )
        {  
            viewportState.pScissors = scissor;          
        }
        viewportState.flags=0;
        viewportState.pNext = null;
        
        VkPipelineTessellationStateCreateInfo tessellationStateCreateInfo = new();
        tessellationStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_TESSELLATION_STATE_CREATE_INFO;
        tessellationStateCreateInfo.pNext = null;
        tessellationStateCreateInfo.flags =0 ;
        uint numberOfControlPointsPerPatch =0;
        tessellationStateCreateInfo.patchControlPoints = numberOfControlPointsPerPatch;

        VkGraphicsPipelineCreateInfo pipelineInfo =new();
        pipelineInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO;
        pipelineInfo.pNext = null;
        pipelineInfo.flags = (uint)VkPipelineCreateFlagBits.VK_PIPELINE_CREATE_DISABLE_OPTIMIZATION_BIT ;
        pipelineInfo.renderPass = data.RenderPass;
        pipelineInfo.subpass = 0;
        pipelineInfo.stageCount =(uint)shaderStages.Length;
        fixed(VkPipelineShaderStageCreateInfo* ss = &shaderStages[0]){
            pipelineInfo.pStages = ss;
        }
        pipelineInfo.pVertexInputState = &vertexInputInfo;
        pipelineInfo.pInputAssemblyState = &inputAssembly;
        pipelineInfo.pColorBlendState = &colorBlending;
        pipelineInfo.pViewportState = &viewportState;
        pipelineInfo.pRasterizationState = &rasterizer;
        pipelineInfo.pMultisampleState = &multisampling;
        pipelineInfo.layout = data.PipelineLayout;
        pipelineInfo.pTessellationState = &tessellationStateCreateInfo;
        pipelineInfo.pDepthStencilState = &depthStencilStateCreateInfo;
        pipelineInfo.pDynamicState = &dynamicStateCreateInfo;
        pipelineInfo.basePipelineIndex =0;
        pipelineInfo.basePipelineHandle = VkPipeline.Null;
        
        fixed( VkPipeline* gfxpipeline = &data.Pipeline )
        {    
            func.Device.vkCreateGraphicsPipelines(data.Device, VkPipelineCache.Null, 1, &pipelineInfo, null, gfxpipeline).Check("failed to create graphics pipeline!");
        }

        for( int i = 0 ; i< shaderModules.Length ; i++ )
        {
            if(shaderModules[i] != VkShaderModule.Null)
            {
                func.Device.vkDestroyShaderModule(data.Device, shaderModules[i], null);
            }
        }

        Log.Info($"Create PIPELINE : {data.Pipeline}");
    }

    public static unsafe void DisposePipeline(ref VulkanFunctions func,ref GraphicsData data )
    {  
        if (!data.Device.IsNull && !data.Pipeline.IsNull)
        {
            Log.Info($"Destroy PIPELINE : {data.Pipeline}");
            func.Device.vkDestroyPipeline(data.Device,data.Pipeline, null);
        }
    }

    #endregion

    #region Objects geometry 

    //matrix model ?
    public unsafe struct ObjectGeometryConfig
    {
        public readonly VkDeviceSize BufferSize;
        public unsafe readonly void* DataPtr;
      

        public ObjectGeometryConfig(void* dataPtr, VkDeviceSize bufferSize )
        {
            BufferSize = bufferSize; 
            DataPtr = dataPtr;
        }
    }

    public unsafe static void CreateBuffers(ref VulkanFunctions func,ref GraphicsData data , ObjectGeometryConfig config ,ref VkBuffer buffer,ref VkDeviceMemory bufferMemory ) 
    {
        VkBuffer stagingBuffer = new();
        VkDeviceMemory stagingBufferMemory = new();

        ResourceCreation. CreateBuffer(ref func , ref data , config.BufferSize , VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT ,         
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer,  ref stagingBufferMemory);

        Memories.TransfertMemory( ref func,ref data,config.DataPtr ,  config.BufferSize,ref  stagingBufferMemory);

        ResourceCreation. CreateBuffer(ref func , ref data , config.BufferSize, 
            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT  | VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT, 
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT, 
            ref buffer,  ref bufferMemory);

        ResourceCreation.CopyBuffer(ref func , ref data ,stagingBuffer,buffer, config.BufferSize);

        func.Device.vkDestroyBuffer(data.Device, stagingBuffer, null);
        func.Device.vkFreeMemory(data.Device, stagingBufferMemory, null);
    }
    //drawcalls

    #endregion

    #region Materials 

    public unsafe static void GenerateMipmaps(  ref VulkanFunctions func,ref GraphicsData data , ref TextureConfig config, ref VkImage image) 
    {
//         // if (!(formatProperties.optimalTilingFeatures & VK_FORMAT_FEATURE_SAMPLED_IMAGE_FILTER_LINEAR_BIT)) {
//         //     throw std::runtime_error("texture image format does not support linear blitting!");
//         // }
        VkCommandBuffer commandBuffer =CommandBuffers.BeginSingleTimeCommands(ref func , ref data);
        
        int mipWidth = (int)config.Width;
        int mipHeight = (int)config.Height;

        for (uint i = 1; i < config.MipmapLevel ; i++)
        {
            SynchronizationCachControl.TransitionImageLayoutConfig transitionImageLayoutConfig = new(commandBuffer, image,i - 1,0,1,1,VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT );
            SynchronizationCachControl.TransitionImageLayout( ref  func,transitionImageLayoutConfig , VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL ,VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL );

            VkImageBlit blit = new();

            VkOffset3D srcOffset3D = new();
            VkOffset3D srcOffset3D2 = new();srcOffset3D2.x = mipWidth;srcOffset3D2.y = mipHeight; srcOffset3D2.z =1;
            
            blit.srcOffsets[0] = &srcOffset3D;
            blit.srcOffsets[1] = &srcOffset3D2;
            blit.srcSubresource.aspectMask = (uint) VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
            blit.srcSubresource.mipLevel = i - 1;
            blit.srcSubresource.baseArrayLayer = 0;
            blit.srcSubresource.layerCount = 1;

            VkOffset3D dstOffset3D = new();
            VkOffset3D dstOffset3D2 = new();
                dstOffset3D2.x = mipWidth > 1 ? mipWidth / 2 : 1;
                dstOffset3D2.y = mipHeight > 1 ? mipHeight / 2 : 1; 
                dstOffset3D2.z =1;

            blit.dstOffsets[0] = &dstOffset3D;
            blit.dstOffsets[1] = &dstOffset3D2;
            blit.dstSubresource.aspectMask = (uint) VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT;
            blit.dstSubresource.mipLevel = i;
            blit.dstSubresource.baseArrayLayer = 0;
            blit.dstSubresource.layerCount = 1;

            func.Device.vkCmdBlitImage(commandBuffer,
                image,  VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL,
                image,  VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                1, &blit,
                VkFilter. VK_FILTER_LINEAR);
            
            if (mipWidth > 1) mipWidth /= 2;
            if (mipHeight > 1) mipHeight /= 2;
            SynchronizationCachControl.TransitionImageLayout( ref  func,transitionImageLayoutConfig , VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL ,VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL );
        }

        CommandBuffers.EndSingleTimeCommands(ref func , ref data, commandBuffer );

    }
    
    public readonly unsafe struct TextureConfig
    {
        public readonly VkFilter Filter = VkFilter.VK_FILTER_LINEAR;
        public readonly VkSamplerAddressMode SamplerMode =   VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        public readonly VkDeviceSize ImageSize;
        public unsafe readonly void* DataPtr;
        public readonly VkFormat Format;
        public readonly uint Width;
        public readonly uint Height;
        public readonly uint MipmapLevel;
        public readonly bool WithMemorieBarrier;

        public TextureConfig(void* dataPtr, VkDeviceSize imageSize , VkFormat format , uint width, uint height,uint mipmaplevel,  bool withMemorieBarrier= false)
        {
            ImageSize = imageSize; 
            DataPtr = dataPtr;
            Format = format;
            Width =width;
            Height= height;
            MipmapLevel = mipmaplevel;
            WithMemorieBarrier = withMemorieBarrier;

        }
    }

    public unsafe static void CreateTexture(ref VulkanFunctions func,ref GraphicsData data , ref TextureConfig config, ref VkImage image, ref VkDeviceMemory imageMemory )
    {
        VkBuffer stagingBuffer = VkBuffer.Null;
        VkDeviceMemory stagingBufferMemory = VkDeviceMemory.Null;

        ResourceCreation. CreateBuffer(ref func , ref data ,config.ImageSize, VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_SRC_BIT ,         
            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, 
            ref stagingBuffer,  ref stagingBufferMemory);
        
        Memories.TransfertMemory( ref func,ref data,config.DataPtr ,config.ImageSize,ref  stagingBufferMemory);       

        ResourceCreation.CreateImage2( ref func ,ref data, ref image,ref imageMemory,
            new(config.Width,config.Height, 1) , config.Format,VkImageTiling.VK_IMAGE_TILING_OPTIMAL  ,
            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT | VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT  
            ,VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT);

       if( config.WithMemorieBarrier) {
            // SynchronizationCachControl.TransitionImageLayout transitionImageLayout = new(   )
            // Memories.TransitionImageLayout(ref func , ref data, format, VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL);
       }
        
       ResourceCreation.CopyBufferToImage(ref  func,ref  data,ref image , ref stagingBuffer, config.Width,config.Height);
        // TransitionImageLayout(ref func , ref data,format, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL);

        func.Device.vkDestroyBuffer(data.Device, stagingBuffer, null);
        func.Device.vkFreeMemory(data.Device, stagingBufferMemory, null);
    }
    
    public unsafe static void DisposeTextureImage(ref VulkanFunctions func,ref GraphicsData data, ref VkImage image, ref VkDeviceMemory imageMemory  )
    {
        if( !image.IsNull)
        {
            Log.Info($"Create Texture Image {image} ");
            func.Device.vkDestroyImage(data.Device, image, null);
        }
        if( !imageMemory.IsNull)
        {
            Log.Info($"Create Texture Image Memory {imageMemory} ");
            func.Device.vkFreeMemory(data.Device, imageMemory, null);
        }


        //This is for DEscriptor  NOt texture buffer Image
        // if( !data.Info.TextureImageView.IsNull)
        // {
        //     Log.Info($"Destroy Texture Image View {data.Info.TextureImageView}");
        //     func.vkDestroyImageView(data.Handles.Device, data.Info.TextureImageView, null);
        // }
    }

    #endregion

    #region  Shaders

    private unsafe static void CreateShaderModule( ref VulkanFunctions func,ref GraphicsData data , ReadOnlySpan<byte> span ,out VkShaderModule shader  )
    {
        // ReadOnlySpan<byte> span = File.ReadAllBytes( shaderfragmentfileSPV ).AsSpan();
        VkShaderModuleCreateInfo createInfoFrag =new();
        createInfoFrag.sType= VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
        createInfoFrag.codeSize=(int)span.Length;
        createInfoFrag.pCode= (uint*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
        createInfoFrag.pNext = null;
        createInfoFrag.flags =0;

        VkShaderModule fragShaderModule = VkShaderModule.Null;
        func.Device.vkCreateShaderModule(data.Device, &createInfoFrag, null,  &fragShaderModule ).Check($"Create  Shader Module Failed"); 
        shader = fragShaderModule;
    }
    
    #endregion
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class ResourceCreation
{
    #region IMAGE VIEW FOR FRAMEBUFFER : SWAPCHAIN AND DEPTHBUFFER
    public readonly struct ImageViewConfig 
    {
        public readonly VkImage Image;
        public readonly VkImageViewType ImageviewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D;
        public readonly VkFormat Format;
        public readonly VkImageAspectFlagBits Aspect = VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT;
        public readonly int ComponentSwizzle =0;

        public ImageViewConfig( VkImage image,VkFormat format ,   VkImageAspectFlagBits aspect = VkImageAspectFlagBits. VK_IMAGE_ASPECT_COLOR_BIT , VkImageViewType imageviewType = VkImageViewType. VK_IMAGE_VIEW_TYPE_2D, int componentSwizzle =0)
        {
            Image= image; Format=format;Aspect = aspect;ComponentSwizzle =componentSwizzle;ImageviewType = imageviewType;
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
        else
        {

        }

        fixed (VkImageView* imageView = &imageViewResult )
        {
            func.Device.vkCreateImageView(data.Device, &viewInfo, null, imageView).Check("failed to create image view!");
        }
        
    }
    
    #endregion

    #region IMAGE  
    public unsafe static void CreateImage2( ref VulkanFunctions func, ref GraphicsData data,ref  VkImage image,ref VkDeviceMemory imageMemory, 
        VkExtent3D extend3D, VkFormat format, VkImageTiling tiling,VkImageUsageFlagBits usage, VkMemoryPropertyFlagBits MemoryProperties   )
    {
        VkImageCreateInfo imageInfo = new();
        imageInfo.sType =VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
        imageInfo.imageType =VkImageType. VK_IMAGE_TYPE_2D;

        imageInfo.extent = extend3D;
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
            func.Device.vkCreateImage(data.Device, &imageInfo, null,img).Check("failed to create image!");
        }

        Memories.AllocateMemoryForImage(ref func , ref data , image,VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT ,out  imageMemory);
        
        func.Device.vkBindImageMemory(data.Device,  image, imageMemory, 0).Check("Bind Image Memory");
    }

    public unsafe static void CopyBufferToImage(ref VulkanFunctions func,ref GraphicsData data,ref VkImage image,ref  VkBuffer buffer,  uint width, uint height, uint depth = 1) 
    {
        VkCommandBuffer commandBuffer = CommandBuffers. BeginSingleTimeCommands(ref func , ref data);

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
        VkExtent3D extent3D = new(); extent3D.width = width; extent3D.height = height; extent3D.depth =depth;
        region.imageExtent = extent3D;

        func.Device.vkCmdCopyBufferToImage(commandBuffer, buffer, image, VkImageLayout.VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &region);

        CommandBuffers.EndSingleTimeCommands(ref func , ref data, commandBuffer);
    }

    #endregion
    //Dispose
    #region BUFFER

    public unsafe static void CreateBuffer(ref VulkanFunctions func,ref GraphicsData data,
        VkDeviceSize size, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits  properties,ref VkBuffer buffer,ref VkDeviceMemory bufferMemory)
    {
        VkBufferCreateInfo bufferInfo = new();
        bufferInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO;
        bufferInfo.pNext = null;
        bufferInfo.size = size;
        bufferInfo.usage = (uint)usage;
        bufferInfo.sharingMode = VkSharingMode.VK_SHARING_MODE_EXCLUSIVE;

        fixed (VkBuffer* buf = &buffer)
        {
            func.Device.vkCreateBuffer(data.Device, &bufferInfo, null, buf).Check("failed to create  buffer!");
        }
        
        Memories.AllocateMemoryForBuffer(ref func, ref data, properties,ref buffer , ref bufferMemory);

        func.Device.vkBindBufferMemory(data.Device, buffer, bufferMemory, 0).Check("failed to Bind buffer memory!");
    }

    public unsafe static void CopyBuffer(ref VulkanFunctions func,ref GraphicsData data, VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size , ulong offsetSrc =0 , ulong offsetDest =0)
    {
        // BeginSingleTimeCommands ---------------------------------------------------------
        VkCommandBuffer commandBuffer = CommandBuffers. BeginSingleTimeCommands(ref func , ref data);
        
        VkBufferCopy copyRegion = new();
        copyRegion.dstOffset = offsetDest;
        copyRegion.srcOffset = offsetSrc;
        copyRegion.size = size;

        func.Device.vkCmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, &copyRegion);

        // EndSingleTimeCommands --------------------------------------------------
        CommandBuffers.EndSingleTimeCommands(ref func , ref data, commandBuffer);
    }

    #endregion
    
    #region  UNIFORM BUFFER

    public unsafe static void CreateUniformBuffers(ref VulkanFunctions func,ref GraphicsData data,ref VkDeviceSize size  ,ref VkBuffer uniformBuffers,ref VkDeviceMemory uniformBuffersMemory,void* uboMapped, bool unamap =false) 
    {
        ulong uboAlignment  = data.Device_Properties.limits.minUniformBufferOffsetAlignment;
       
        VkDeviceSize bufferSize =((size / uboAlignment) * uboAlignment + ((size % uboAlignment) > 0 ? uboAlignment : 0));
        
        size = (ulong) bufferSize;

        CreateBuffer(ref func, ref data, bufferSize, 
        VkBufferUsageFlagBits.VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT, 
        VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | 
        VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT,
        ref uniformBuffers,ref uniformBuffersMemory);

        Memories.TransfertMemory( ref func,ref data,uboMapped,size,ref  uniformBuffersMemory, false);
        Log.Info($"Create Uniform Buffer : {uniformBuffers} Mem {uniformBuffersMemory}");
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

    public unsafe static void UpdateUniformBuffer(ref VulkanFunctions func,ref GraphicsData data, VkDeviceSize size, float* uboData,void* uboMapped)
    {      
        Unsafe.CopyBlock( uboMapped ,uboData ,(uint)size);
    }

    #endregion

    #region SAMPLER OU DANS RESOURCE DESCRIPTOR ?
    public unsafe static void CreateTextureSampler(ref VulkanFunctions func,ref GraphicsData data,ref VkSampler textureSampler )
    {
        VkSamplerCreateInfo samplerInfo = new();
        samplerInfo.sType =  VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
        samplerInfo.magFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.minFilter = VkFilter.VK_FILTER_LINEAR;
        samplerInfo.addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT;
        samplerInfo.anisotropyEnable = VK.VK_TRUE;
        samplerInfo.maxAnisotropy = data.Device_Properties.limits.maxSamplerAnisotropy;
        samplerInfo.borderColor = VkBorderColor . VK_BORDER_COLOR_INT_OPAQUE_BLACK;
        samplerInfo.unnormalizedCoordinates = VK.VK_FALSE;
        samplerInfo.compareEnable = VK.VK_FALSE;
        samplerInfo.compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS;
        samplerInfo.mipmapMode = VkSamplerMipmapMode .VK_SAMPLER_MIPMAP_MODE_LINEAR;

        fixed(VkSampler* sampler  = &textureSampler)
        {
            func.Device.vkCreateSampler(data.Device, &samplerInfo, null, sampler).Check("failed to create texture sampler!");
        }   
        Log.Info($"Create Texture sampler {textureSampler}");
    }

    public unsafe static void DisposeTextureSampler(ref  VulkanFunctions func,ref GraphicsData data, ref VkSampler textureSampler)
    {
        if(!data.Device.IsNull && !textureSampler.IsNull)
        {
            Log.Info($"Destroy Texture sampler {textureSampler}");
            func.Device.vkDestroySampler(data.Device,textureSampler, null);
        }
    }
    
    #endregion

    internal static void CreateVertexIndiceBuffer(ref VulkanFunctions func, ref GraphicsData data, ref GraphicsConfig config)
    {
            // CreateBuffers(ref  func,ref  data ,  config ,ref  vertexbuffer,ref  vertexbufferMemory );
        // CreateBuffers(ref  func,ref  data ,  config ,ref  indicebuffer,ref  indicebufferMemory );

    }

    internal static void CreateTextures(ref VulkanFunctions func, ref GraphicsData data, ref GraphicsConfig config)
    {
        // CreateTexture(ref  func,ref  data , ref  config, ref  textureimage, ref  textureimageMemory );
        // GraphicDeviceImplement.CreateTextureImageView(ref _functions , ref _data);
    }

    }

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static partial class ResourceDecriptor
{

    public unsafe static void CreateDescriptors(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
        //:KNOW SHADER DESCIRPTION INSIDE 

        // AFTER CREATE   !!! Create  VkDescriptorSet ( need pool ) 
        CreateDDescriptorSetLayout(ref  func,ref  data , ref  config );
// HERE IS IMPORTATN FOR CREATE PIPELINE LAYOUT
        CreateDDescriptorPool(ref  func,ref  data , ref  config );

        CreateDescriptorsSet(ref  func,ref  data , ref  config );
        UpdateDescriptorsSet(ref  func,ref  data , ref  config );

        CreatePipelineLayout( ref  func,ref  data , ref  config  );
    }

    public unsafe static void CreateDDescriptorSetLayout(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
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

        fixed( VkDescriptorSetLayout* layout = &data.DescriptorSetLayout )
        {
            func.Device.vkCreateDescriptorSetLayout(data.Device, &layoutInfo, null, layout).Check("failed to create descriptor set layout!");
        }
        Log.Info($"Create Descriptor Set layout : {data.DescriptorSetLayout}");

    }
    
    public unsafe static void CreateDDescriptorPool(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
         VkDescriptorPoolSize* poolSizes = stackalloc VkDescriptorPoolSize[2];

        poolSizes[0].type =VkDescriptorType. VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
        poolSizes[0].descriptorCount = (uint)(config.Render.MAX_FRAMES_IN_FLIGHT);
        
        poolSizes[1].type = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
        poolSizes[1].descriptorCount =(uint)(config.Render.MAX_FRAMES_IN_FLIGHT);

        VkDescriptorPoolCreateInfo poolInfo= new();
        poolInfo.sType =VkStructureType. VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO;
        poolInfo.poolSizeCount = 2;
        poolInfo.pPoolSizes = poolSizes;
        poolInfo.maxSets = (uint)(config.Render.MAX_FRAMES_IN_FLIGHT);

        fixed(VkDescriptorPool* pool =  &data.DescriptorPool)
        {
            func.Device.vkCreateDescriptorPool(data.Device, &poolInfo, null,pool ).Check("failed to create descriptor pool!");
        }
        Log.Info($"Create Descriptor Pool : {data.DescriptorPool}");

    }

    public unsafe static void CreateDescriptorsSet(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
        // int value = data.Info.MAX_FRAMES_IN_FLIGHT;
        VkDescriptorSetLayout* layouts  =  stackalloc VkDescriptorSetLayout[2] { data.DescriptorSetLayout,data.DescriptorSetLayout };

        VkDescriptorSetAllocateInfo allocInfo = new();
        allocInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO;
        allocInfo.descriptorPool = data.DescriptorPool;
        allocInfo.descriptorSetCount = (uint)(config.Render.MAX_FRAMES_IN_FLIGHT);
        allocInfo.pSetLayouts = layouts;
        allocInfo.pNext = null;

        data.DescriptorSets = new VkDescriptorSet[ config.Render.MAX_FRAMES_IN_FLIGHT];

        fixed(VkDescriptorSet* descriptor =&data.DescriptorSets[0]  )
        {
            func.Device.vkAllocateDescriptorSets(data.Device, &allocInfo, descriptor).Check("failed to allocate descriptor sets!");
        }
    }

    public unsafe static void UpdateDescriptorsSet(ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config )
    {
        /*
        BEFORE UPDATE NEED TO CREATE 
        UNIFORM BUFFER
        SAMPLER 
        IN RESOURCE CREATION 
        */

        VkWriteDescriptorSet* descriptorWrites = stackalloc VkWriteDescriptorSet[2];

        for (int i = 0; i <  config.Render.MAX_FRAMES_IN_FLIGHT; i++) 
        {
            VkDescriptorBufferInfo bufferInfo = new();
            // bufferInfo.buffer = data.Info.UniformBuffers[i];
            bufferInfo.offset = 0;
            bufferInfo.range = (uint) sizeof(float) * 3 * 16;// sizeof UNIFORM_MVP

            VkDescriptorImageInfo imageInfo =new();
            imageInfo.imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
            // imageInfo.imageView = data.Info.TextureImageView;
            // imageInfo.sampler = data.Info.TextureSampler;

            // descriptorWrites[0].sType = VkStructureType. VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
            // descriptorWrites[0].dstSet = data.Handles.DescriptorSets[i];
            // descriptorWrites[0].dstBinding = 0;
            // descriptorWrites[0].dstArrayElement = 0;
            // descriptorWrites[0].descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER;
            // descriptorWrites[0].descriptorCount = 1;
            // descriptorWrites[0].pBufferInfo = &bufferInfo;

            // descriptorWrites[1].sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET;
            // descriptorWrites[1].dstSet = data.Handles.DescriptorSets[i];
            // descriptorWrites[1].dstBinding = 1;
            // descriptorWrites[1].dstArrayElement = 0;
            // descriptorWrites[1].descriptorType =VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER;
            // descriptorWrites[1].descriptorCount = 1;
            // descriptorWrites[1].pImageInfo = &imageInfo;

            func.Device.vkUpdateDescriptorSets(data.Device, 2, descriptorWrites, 0, null);

            // Log.Info($"-{i}  Create Descriptor Sets : {data.Handles.DescriptorSets[i]}");
        }
    }

     public unsafe static void DisposeDescriptors(ref VulkanFunctions func,ref GraphicsData data  )
    {
        //  if ( !data.Handles.DescriptorPool.IsNull)
        // {
        //     Log.Info($"Destroy Descriptor Pool : {data.Handles.DescriptorPool}");
        //     func.vkDestroyDescriptorPool(data.Handles.Device, data.Handles.DescriptorPool, null);
        // }
        
        // if ( !data.Handles.DescriptorSetLayout.IsNull)
        // {
        //     Log.Info($"Destroy Descriptor Set layout : {data.Handles.DescriptorSetLayout}");
        //     func.vkDestroyDescriptorSetLayout(data.Handles.Device, data.Handles.DescriptorSetLayout, null);
        // }
        

    }

    public unsafe static void UpdatePushConstant( ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config)
    {
        // PUSH CONSTANTS ---------- ( do before bin pipeline)
        // void* ptr = new IntPtr( data.Info.PushConstants).ToPointer();
        // fixed(void* ptr = &data.Info.PushConstants ){
        //     func.vkCmdPushConstants(commandBuffer,data.Handles.PipelineLayout, (uint) VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT, 0,(uint)sizeof(PushConstantsMesh), ptr );
        // }
    }
  
    public unsafe static void CreatePushConstant( out VkPushConstantRange push_constant )
    {
        // VkPushConstantRange push_constant;
	    //this push constant range starts at the beginning
	    push_constant.offset = 0;
	    //this push constant range takes up the size of a MeshPushConstants struct
	    push_constant.size = (uint)sizeof(PushConstantsMesh); // 16 * sizeof(float) + 4 * sizeof(float)
	    //this push constant range is accessible only in the vertex shader
	    push_constant.stageFlags = (uint)VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT;
    }

    public unsafe static void CreatePipelineLayout( ref VulkanFunctions func,ref GraphicsData data , ref GraphicsConfig config  )
    {
        VkPipelineLayoutCreateInfo pipelineLayoutInfo=new();
        pipelineLayoutInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO;
        pipelineLayoutInfo.flags =0;
        pipelineLayoutInfo.pNext =null; 

        pipelineLayoutInfo.setLayoutCount = 1;     

        fixed (VkDescriptorSetLayout* layout = &data.DescriptorSetLayout )
        {
            pipelineLayoutInfo.pSetLayouts = layout;
        }         

        CreatePushConstant( out VkPushConstantRange push_constant );
       
        pipelineLayoutInfo.pushConstantRangeCount = 1;    // Optionnel
        pipelineLayoutInfo.pPushConstantRanges = &push_constant; 

        fixed( VkPipelineLayout* layout = &data.PipelineLayout )
        {
            func.Device.vkCreatePipelineLayout(data.Device, &pipelineLayoutInfo, null, layout).Check ("failed to create pipeline layout!");
        }
        Log.Info($"Create Pipeline Layout : {data.PipelineLayout}");
    }

    public static unsafe void DisposePipelineLayout(in GraphicDeviceFunctions  func,ref GraphicDeviceData data)
    {
        if( !data.Handles.Device.IsNull && !data.Handles.PipelineLayout.IsNull)
        {
            Log.Info($"Destroy Pipeline Layout : {data.Handles.PipelineLayout}");
            func.vkDestroyPipelineLayout(data.Handles.Device, data.Handles.PipelineLayout, null);
        }        
        
    }


    public unsafe static void CreateTextureImageViewForResourceDescriptors(ref VulkanFunctions func,ref GraphicsData data ,VkFormat textureFormat,VkImage textureImage , ref VkImageView textureImageView)
    {
        Log.Info($"Create Texture Image View {textureImageView}");
        ResourceCreation.ImageViewConfig temp = new(textureImage ,textureFormat , VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT);
        ResourceCreation.CreateImageView( ref func,ref data, in temp, out textureImageView);
    }

    public unsafe static void DisposeTextureImageViewForResourceDescriptors(ref VulkanFunctions func,ref GraphicsData data, ref VkImageView textureImageView)
    {
        if( !textureImageView.IsNull)
        {
            Log.Info($"Destroy Texture Image View {textureImageView}");
            func.Device.vkDestroyImageView(data.Device, textureImageView, null);
        }
    }

}

}
