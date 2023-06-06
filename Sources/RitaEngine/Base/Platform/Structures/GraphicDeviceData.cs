namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.Vulkan;

[/*StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT),*/ SkipLocalsInit]
public struct GraphicDeviceData : IEquatable<GraphicDeviceData>
{
    // public GraphicDeviceCapabilities Infos=new();
    
    public VkFramebuffer[] VkFramebuffers = new VkFramebuffer[3];//need for render => NEEDVALID SWAP CHAIN
    public VkCommandBuffer[] VkCommandBuffers = new VkCommandBuffer[3];// TODO same number than MAX FRAME FILGHT  TODO MAXFRAME FILGHT in settings ?  
    public VkImage[] VkImages = new VkImage[3]; //for CreateImagesView and RecreateSwapChain ....
    public VkImageView[] VkSwapChainImageViews = new VkImageView[3];
    public VkSemaphore[] ImageAvailableSemaphores = new VkSemaphore[2];
    public VkSemaphore[] RenderFinishedSemaphores = new VkSemaphore[2];
    public VkFence[] InFlightFences = new VkFence[2];
    public VkRenderPass VkRenderPass = VkRenderPass.Null;

    public VkInstance VkInstance = VkInstance.Null;
    public VkDebugUtilsMessengerEXT DebugMessenger = VkDebugUtilsMessengerEXT.Null;
    public VkSurfaceKHR VkSurface = VkSurfaceKHR.Null;
    public VkPhysicalDevice VkPhysicalDevice = VkPhysicalDevice.Null;
    public VkDevice VkDevice = VkDevice.Null; 
    public VkQueue VkPresentQueue = VkQueue.Null;// used for draw 
    public VkQueue VkGraphicQueue = VkQueue.Null;// used for draw
    public VkSwapchainKHR VkSwapChain = VkSwapchainKHR.Null ;
    
    public VkCommandPool VkCommandPool = VkCommandPool.Null;// not used for draw but importante??? only need to create command buffer 
    public VkPipelineLayout VkpipelineLayout = VkPipelineLayout.Null;
    public VkPipeline VkGraphicsPipeline = VkPipeline.Null;

    public VkExtent2D VkSurfaceArea = new();
    public VkClearValue ClearColor = new();
    public VkOffset2D  RenderAreaOffset = new();
    public VkViewport Viewport  = new();
    public VkRect2D Scissor = new();
    public VkFormat VkFormat = VkFormat.VK_FORMAT_UNDEFINED;
    public ulong tick_timeout = ulong.MaxValue;
    
    private nint _address = nint.Zero;
    public uint VkGraphicFamilyIndice =0;
    public uint VkPresentFamilyIndice=0;
    public int MAX_FRAMES_IN_FLIGHT =2;
    public bool VertexOutsideShader = false;
// vertex code 19
    public VkBuffer VertexBuffer = VkBuffer.Null;
    public VkDeviceMemory VertexBufferMemory = VkDeviceMemory.Null;

    public GraphicDeviceData()
    {
        _address = AddressOfPtrThis( ) ;
        var sizeEmpty = Unsafe.SizeOf<GraphicDeviceCapabilities>();
        var size =  Marshal.SizeOf(this);
        Log.Info($"Create Graphic Device DATA => size : {sizeEmpty}, {size } {_address:X}");
    }
    
    public void Release()
    {
        var sizeEmpty = Unsafe.SizeOf<GraphicDeviceCapabilities>();
        var size =  Marshal.SizeOf(this);
        Log.Info($"Release Graphic Device DATA => size : {sizeEmpty}, {size } {AddressOfPtrThis( ):X}");
        VkFramebuffers = null!;
        VkImages = null!;
        VkSwapChainImageViews = null!;
        VkCommandBuffers = null!;
        ImageAvailableSemaphores = null!;
        RenderFinishedSemaphores = null!;
        InFlightFences = null!;
        // Infos.Release();
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

