namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Math.Vertex;
using RitaEngine.Base.Platform.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public struct GraphicDeviceData : IEquatable<GraphicDeviceData>
{
    public delegate void PFN_GetFrameBuffer( ref uint x , ref uint y);

    public PFN_GetFrameBuffer GetFrameBufferCallback = null!;

    public GraphicDeviceAppData  App = new();
    public GraphicDevicePhysicalData Physical = new();
    public GraphicDeviceSwapChainData SwapChain = new();
    public GraphicDeviceRenderData Render = new();

    public int MAX_FRAMES_IN_FLIGHT =2;



    public VkCommandBuffer[] VkCommandBuffers = new VkCommandBuffer[3];// TODO same number than MAX FRAME FILGHT  TODO MAXFRAME FILGHT in settings ?  
    
   //SYNC
    public VkSemaphore[] ImageAvailableSemaphores = new VkSemaphore[2];
    public VkSemaphore[] RenderFinishedSemaphores = new VkSemaphore[2];
    public VkFence[] InFlightFences = new VkFence[2];
    //---------        

    
    public VkDevice VkDevice = VkDevice.Null; 
    public VkQueue VkPresentQueue = VkQueue.Null;// used for draw 
    public VkQueue VkGraphicQueue = VkQueue.Null;// used for draw


    
    public VkCommandPool VkCommandPool = VkCommandPool.Null;// not used for draw but importante??? only need to create command buffer 
    public VkPipelineLayout VkpipelineLayout = VkPipelineLayout.Null;
    public VkPipeline VkGraphicsPipeline = VkPipeline.Null;


    public VkDescriptorSetLayout DescriptorSetLayout = VkDescriptorSetLayout.Null;
    public VkDescriptorSetLayout[] Layouts = new VkDescriptorSetLayout[2];
    public VkDescriptorSet[] DescriptorSets =null!; 
    public VkDescriptorPool DescriptorPool = VkDescriptorPool.Null;
    
    public VkClearValue ClearColor = new();
    public VkOffset2D  RenderAreaOffset = new();
    public VkViewport Viewport  = new();
    public VkRect2D Scissor = new();
    public ulong tick_timeout = ulong.MaxValue;
    
  
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

    public Position3f_Color3f_UV2f[] Vertices = null!;
    public short[] Indices = null!;
    public string VertexShaderFileNameSPV ="";
    public string FragmentShaderFileNameSPV ="";
    public string FragmentEntryPoint ="";
    public string VertexEntryPoint="";
    public Uniform_MVP ubo = new();

    // public float MaxSamplerAnisotropy;
    public string TextureName ="";
    public GraphicDeviceData()   {  }
    
    public void Release()
    {
        DescriptorSets =null!; 
        VkCommandBuffers = null!;
        ImageAvailableSemaphores = null!;
        RenderFinishedSemaphores = null!;
        InFlightFences = null!;

        SwapChain.Release();
        Physical.Release();
        App.Release();
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

