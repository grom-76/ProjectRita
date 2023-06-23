namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Math;
using RitaEngine.Base.Platform.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceData : IEquatable<GraphicDeviceData>
{
    public GraphicDeviceHandles Handles = new();
    public GraphicDeviceInfo Info = new();
    public GraphicDeviceData() { }
    
    public void Release()
    {
        Info.Release();
        Handles.Release();
    }
 
    #region OVERRIDE    
    public override string ToString() => string.Format($"Graphic Device Data" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicDeviceData data && this.Equals(data) ;
    public bool Equals(GraphicDeviceData other)=> Handles.Equals( other.Handles) ;
    public static bool operator ==(GraphicDeviceData left, GraphicDeviceData right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceData left, GraphicDeviceData  right) => !left.Equals(right);
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceHandles: IEquatable<GraphicDeviceHandles>
{
    public delegate void PFN_GetFrameBuffer( ref uint x , ref uint y);
    public PFN_GetFrameBuffer GetFrameBufferCallback = null!;
    public VkSurfaceKHR Surface = VkSurfaceKHR.Null;
    public VkInstance Instance = VkInstance.Null;
    public VkDebugUtilsMessengerEXT DebugMessenger = VkDebugUtilsMessengerEXT.Null;
    public VkPhysicalDevice PhysicalDevice = VkPhysicalDevice.Null;
    public VkSwapchainKHR SwapChain = VkSwapchainKHR.Null ;
    public VkRenderPass RenderPass = VkRenderPass.Null;
    public VkDevice Device = VkDevice.Null; 
    public VkQueue PresentQueue = VkQueue.Null;// used for draw 
    public VkQueue GraphicQueue = VkQueue.Null;// used for draw
    public VkCommandPool CommandPool = VkCommandPool.Null;// not used for draw but importante??? only need to create command buffer 
    public VkDescriptorSetLayout DescriptorSetLayout = VkDescriptorSetLayout.Null;
    public VkDescriptorPool DescriptorPool = VkDescriptorPool.Null;
    public VkPipelineLayout PipelineLayout = VkPipelineLayout.Null;
    public VkPipeline Pipeline = VkPipeline.Null;
    public VkBuffer VertexBuffer = VkBuffer.Null;
    public VkBuffer IndicesBuffer = VkBuffer.Null;
    public uint IndicesSize = 0;
    public VkImage[] Images = null!;// new VkImage[3]; //for CreateImagesView and RecreateSwapChain ....
    public VkImageView[] SwapChainImageViews = null!;//new VkImageView[3];
    public VkFramebuffer[] Framebuffers = null!;// new VkFramebuffer[3];//need for render => NEEDVALID SWAP CHAIN
    public VkDescriptorSet[] DescriptorSets  =  null!;//new VkDescriptorSet[2];
    public VkCommandBuffer[] CommandBuffers = null!;//new VkCommandBuffer[2];// TODO same number than MAX FRAME FILGHT  TODO MAXFRAME FILGHT in settings ?  
    public VkSemaphore[] ImageAvailableSemaphores = null!;//new VkSemaphore[2];
    public VkSemaphore[] RenderFinishedSemaphores = null!;//new VkSemaphore[2];
    public VkFence[] InFlightFences =null!;// new VkFence[2];

    // public VkPrimitiveTopology DynamicStatee_PolygonTopology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST;
    public float DynamicStatee_LineWidth= 1.0f;
    public GraphicDeviceHandles()
    {

    }

    public void Release()
    {

        ImageAvailableSemaphores = null!;
        RenderFinishedSemaphores = null!;
        InFlightFences = null!;

        DescriptorSets =null!; 
        CommandBuffers = null!;

        Framebuffers = null!;
        SwapChainImageViews = null!;
        Images = null!;
    }

    #region OVERRIDE    
    public override string ToString() => string.Format($"Graphic Device Handles" );
    public override int GetHashCode() => HashCode.Combine(Instance,Device );
    public override bool Equals(object? obj) => obj is GraphicDeviceHandles data && this.Equals(data) ;
    public bool Equals(GraphicDeviceHandles other)=>  Instance.Equals(other.Instance) && Device.Equals(other.Device);
    public static bool operator ==(GraphicDeviceHandles left, GraphicDeviceHandles right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceHandles left, GraphicDeviceHandles  right) => !left.Equals(right);
    #endregion
}

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceInfo : IEquatable<GraphicDeviceInfo>
{
    public unsafe void* Handle =(void*)0;
    public unsafe void* HInstance =(void*)0;
    public int Width =1280;
    public int Height = 720;
    public uint Version=0;
    public bool EnableDebug = false;
    public string[] ValidationLayers = null!;
    public string[] InstanceExtensions = null!;
    public byte[] GameName = null!;

    public VkPhysicalDeviceProperties PhysicalDeviceProperties = new();
    
    #region VKDeivce
    public VkSurfaceCapabilitiesKHR Capabilities;
    public VkSurfaceFormatKHR[] Formats= null!;
    public VkPresentModeKHR[] PresentModes = null!;
    public VkPhysicalDeviceFeatures Features = new();
    #endregion
    
    public uint VkGraphicFamilyIndice =0;
    public uint VkPresentFamilyIndice=0;
    public string[] DeviceExtensions = null!;

    public VkExtent2D VkSurfaceArea = new();
    public VkFormat VkFormat = VkFormat.VK_FORMAT_UNDEFINED;
    public VkPresentModeKHR VkPresentMode = VkPresentModeKHR.VK_PRESENT_MODE_IMMEDIATE_KHR;// v-sync ???
    public VkSurfaceFormatKHR VkSurfaceFormat = new();
    public uint ImageCount =3;
    public int MAX_FRAMES_IN_FLIGHT =2;
    public VkClearValue ClearColor = new();
    public VkClearValue ClearColor2 = new();
    public VkOffset2D  RenderAreaOffset = new();
    public VkViewport Viewport  = new();
    public VkRect2D Scissor = new();
    public VkRect2D RenderArea = new();
    public ulong tick_timeout = ulong.MaxValue;
   
    public VkDeviceMemory VertexBufferMemory = VkDeviceMemory.Null;
    public VkDeviceMemory IndicesBufferMemory = VkDeviceMemory.Null;
    public VkBuffer[] UniformBuffers = null!;
    public VkDeviceMemory[] UniformBuffersMemory = null!;
    // public nint[] UniformBuffersMapped = null!;
    public unsafe void*[] UboMapped = null!;
    public ulong UboSize = 0;
    public VkImage TextureImage = VkImage.Null;
    public VkDeviceMemory TextureImageMemory = VkDeviceMemory.Null;
    public VkImageView TextureImageView = VkImageView.Null; //25 sampler
    public VkSampler TextureSampler =     VkSampler.Null; // 25 sampler
    public VkImage DepthImage = VkImage.Null; //27 depth buffering
    public VkDeviceMemory DepthImageMemory = VkDeviceMemory.Null;
    public VkImageView DepthImageView = VkImageView.Null;

    
    
    public short[] Indices = null!;
    public string VertexShaderFileNameSPV ="";
    public string FragmentShaderFileNameSPV ="";
    public string FragmentEntryPoint ="";
    public string VertexEntryPoint="";
    
    public float[] UniformBufferArray = null!;
    public float[] Vertices = null!;
    public string TextureName ="";

    public GraphicDeviceInfo()
    {
        PhysicalDeviceProperties.sparseProperties = new();
        PhysicalDeviceProperties.limits = new();
    }

    public unsafe void Release()
    {
       Vertices = null!;

        UniformBufferArray = null!;
        UniformBuffers = null!;
        UniformBuffersMemory= null!;
        // UniformBuffersMapped = null!;
        UboMapped = null!;
        DeviceExtensions = null!;
        Formats= null!;
        PresentModes = null!;
        GameName = null!;
        ValidationLayers = null!;
        InstanceExtensions = null!;
    }
    #region OVERRIDE    
    public override string ToString() => string.Format($"Graphic Device Data" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicDeviceInfo data && this.Equals(data) ;
    public bool Equals(GraphicDeviceInfo other)=>  false;
    public static bool operator ==(GraphicDeviceInfo left, GraphicDeviceInfo right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceInfo left, GraphicDeviceInfo  right) => !left.Equals(right);
    #endregion
}