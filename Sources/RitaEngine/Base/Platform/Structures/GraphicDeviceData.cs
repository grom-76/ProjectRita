namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.Native.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public unsafe struct GraphicDeviceData : IEquatable<GraphicDeviceData>
{

/*
struct Instance  => GraphicDeviceConfig => ? layer, dll , 
*/
    public GraphicDeviceCapabilities Infos = new();
    
    private nint _address = nint.Zero;

    public VkInstance VkInstance = VkInstance.Null;
    public VkDebugUtilsMessengerEXT DebugMessenger = VkDebugUtilsMessengerEXT.Null;
    // Surface
    public VkSurfaceKHR VkSurface = VkSurfaceKHR.Null;
    public VkPhysicalDevice VkPhysicalDevice = VkPhysicalDevice.Null;



    public VkImage[] VkImages = null!; //for CreateImagesView and RecreateSwapChain ....
    public VkDevice VkDevice = VkDevice.Null; 


    public VkQueue VkPresentQueue = VkQueue.Null;// used for draw 
    public VkQueue VkGraphicQueue = VkQueue.Null;// used for draw

    public VkSwapchainKHR VkSwapChain = VkSwapchainKHR.Null ;
    public VkImageView[] VkSwapChainImageViews = null!;  
    public VkExtent2D VkSurfaceArea = new();
    public VkFormat VkFormat = VkFormat.VK_FORMAT_UNDEFINED;

    public uint VkGraphicFamilyIndice =0;
    public uint VkPresentFamilyIndice=0;

    //RenderPass
    public VkRenderPass VkRenderPass = VkRenderPass.Null;

    //FrameBuffer
    public VkFramebuffer[] VkFramebuffers = null!;//need for render => NEEDVALID SWAP CHAIN

    //Command Pool 
    public VkCommandBuffer[] VkCommandBuffers = null!;// TODO same number than MAX FRAME FILGHT  TODO MAXFRAME FILGHT in settings ?
    public VkCommandPool VkCommandPool = VkCommandPool.Null;// not used for draw but importante??? only need to create command buffer 
    public int MAX_FRAMES_IN_FLIGHT =2;

    //Synchronisation & cache control
    public VkSemaphore[] ImageAvailableSemaphores = null!;
    public VkSemaphore[] RenderFinishedSemaphores = null!;
    public VkFence[] InFlightFences = null!;

    // PIPELINE
    public VkPipelineLayout VkpipelineLayout = VkPipelineLayout.Null;
    public VkPipeline VkGraphicsPipeline = VkPipeline.Null;
    
    // public VkClearValue ClearColor= new VkClearValue();
    public VkClearValue ClearColor = new();
    public VkOffset2D  RenderAreaOffset = new();

    //dynamic state
    public VkViewport Viewport  = new();
    public VkRect2D Scissor = new();

    //TODO : check in select physical device to get all info 

    public ulong tick_timeout = ulong.MaxValue;
    public nint vulkan = nint.Zero;
    public bool VertexOutsideShader = false;

    public GraphicDeviceData() {_address = AddressOfPtrThis( ) ;}
    public void Release()
    {
        // DebugMessenger = VkDebugUtilsMessengerEXT.Null;
        // VkSurface = VkSurfaceKHR.Null;
        // VkPhysicalDevice = VkPhysicalDevice.Null;
        // VkDevice = VkDevice.Null; 
        // VkPresentQueue = VkQueue.Null;// used for draw 
        // VkGraphicQueue = VkQueue.Null;// used for draw
        // VkSwapChain = VkSwapchainKHR.Null ;
        // VkSwapChainImageViews = null!;  
        // VkSurfaceArea = new();
        // VkFormat = VkFormat.VK_FORMAT_UNDEFINED;
        // VkGraphicFamilyIndice =0;
        // VkPresentFamilyIndice=0;
        // VkRenderPass = VkRenderPass.Null;
        // VkSwapChainFramebuffers = null!;
        // VkCommandBuffers = null!;// TODO same number than MAX FRAME FILGHT  TODO MAXFRAME FILGHT in settings ?
        // VkCommandPool = VkCommandPool.Null;// not used for draw but importante??? only need to create command buffer 
        // MAX_FRAMES_IN_FLIGHT =2;
        // ImageAvailableSemaphores = null!;
        // RenderFinishedSemaphores = null!;
        // InFlightFences = null!;
        // VkpipelineLayout = VkPipelineLayout.Null;
        // VkGraphicsPipeline = VkPipeline.Null;
        // ClearColor= new VkClearValue();
        // RenderAreaOffset = new();
        // VkInstance = VkInstance.Null;

    }

    public unsafe nint AddressOfPtrThis( ) { 
            #pragma warning disable CS8500
        fixed (void* pointer = &this )  { return((nint) pointer ) ; }  
        #pragma warning restore
    }
        #region OVERRIDE    
    public override string ToString()  
        => string.Format($"DataModule" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicDeviceData data && this.Equals(data) ;
    public bool Equals(GraphicDeviceData other)=>  false;
    public static bool operator ==(GraphicDeviceData left, GraphicDeviceData right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceData left, GraphicDeviceData  right) => !left.Equals(right);
    #endregion
}

