namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceData : IEquatable<GraphicDeviceData>
{
    public GraphicDeviceApp  App = new();
    public GraphicDevicePhysical Physical = new();
    public GraphicDeviceSwapChain SwapChain = new();


    public VkFramebuffer[] VkFramebuffers = new VkFramebuffer[3];//need for render => NEEDVALID SWAP CHAIN
    public VkCommandBuffer[] VkCommandBuffers = new VkCommandBuffer[3];// TODO same number than MAX FRAME FILGHT  TODO MAXFRAME FILGHT in settings ?  
    
    public VkImageView[] VkSwapChainImageViews = new VkImageView[3];
    public VkSemaphore[] ImageAvailableSemaphores = new VkSemaphore[2];
    public VkSemaphore[] RenderFinishedSemaphores = new VkSemaphore[2];
    public VkFence[] InFlightFences = new VkFence[2];
    public VkRenderPass VkRenderPass = VkRenderPass.Null;

    
    public VkDevice VkDevice = VkDevice.Null; 
    public VkQueue VkPresentQueue = VkQueue.Null;// used for draw 
    public VkQueue VkGraphicQueue = VkQueue.Null;// used for draw


    // public VkSwapchainKHR VkSwapChain = VkSwapchainKHR.Null ;
    // public VkExtent2D VkSurfaceArea = new();
    // public VkFormat VkFormat = VkFormat.VK_FORMAT_UNDEFINED;
    // public VkImage[] VkImages = new VkImage[3]; //for CreateImagesView and RecreateSwapChain ....
    
    public VkCommandPool VkCommandPool = VkCommandPool.Null;// not used for draw but importante??? only need to create command buffer 
    public VkPipelineLayout VkpipelineLayout = VkPipelineLayout.Null;
    public VkPipeline VkGraphicsPipeline = VkPipeline.Null;
    public VkDescriptorSetLayout DescriptorSetLayout = VkDescriptorSetLayout.Null;
    public VkDescriptorSetLayout[] Layouts = new VkDescriptorSetLayout[2];
    public VkDescriptorSet[] DescriptorSets = new VkDescriptorSet[2]; 
    public VkDescriptorPool DescriptorPool = VkDescriptorPool.Null;
    
    public VkClearValue ClearColor = new();
    public VkOffset2D  RenderAreaOffset = new();
    public VkViewport Viewport  = new();
    public VkRect2D Scissor = new();
    public ulong tick_timeout = ulong.MaxValue;
    
    private nint _address = nint.Zero;
    
    public int MAX_FRAMES_IN_FLIGHT =2;
    public bool VertexOutsideShader = false;
// vertex code 19
    public VkBuffer VertexBuffer = VkBuffer.Null;
    public VkBuffer IndicesBuffer = VkBuffer.Null;
    public uint IndicesSize = 0;

    public VkDeviceMemory VertexBufferMemory = VkDeviceMemory.Null;
    public VkDeviceMemory IndicesBufferMemory = VkDeviceMemory.Null;

    public VkBuffer[] uniformBuffers = new VkBuffer[2];
    public VkDeviceMemory[] uniformBuffersMemory= new VkDeviceMemory[2];
    public nint[] uniformBuffersMapped = new nint[2];

    public VkImage TextureImage = VkImage.Null;
    public VkDeviceMemory TextureImageMemory = VkDeviceMemory.Null;

    public VkImageView TextureImageView = VkImageView.Null; //25 sampler
    public VkSampler TextureSampler =     VkSampler.Null; // 25 sampler

    public VkImage DepthImage = VkImage.Null; //27 depth buffering
    public VkDeviceMemory DepthImageMemory = VkDeviceMemory.Null;
    public VkImageView DepthImageView = VkImageView.Null;

    // public float MaxSamplerAnisotropy;
    public string TextureName ="";
    public GraphicDeviceData()   {  }
    
    public void Release()
    {
        
        VkFramebuffers = null!;
        // VkImages = null!;
        VkSwapChainImageViews = null!;
        VkCommandBuffers = null!;
        ImageAvailableSemaphores = null!;
        RenderFinishedSemaphores = null!;
        InFlightFences = null!;
    }

 
    #region OVERRIDE    
    public override string ToString() => string.Format($"Graphic Device Data" );
    public override int GetHashCode() => (int)0;
    public override bool Equals(object? obj) => obj is GraphicDeviceData data && this.Equals(data) ;
    public bool Equals(GraphicDeviceData other)=>  false;
    public static bool operator ==(GraphicDeviceData left, GraphicDeviceData right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceData left, GraphicDeviceData  right) => !left.Equals(right);
    #endregion
}

