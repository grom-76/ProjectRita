namespace RitaEngine.Graphic.GraphicPipeline;

using RitaEngine.API.Vulkan;

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public unsafe static class DynamicStates
{
    
    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct DynamicStatesConfigData
    {
        public bool WithViewport = true;
        public bool WithScissor = true;
        public bool WithLineWidth = false;
        public bool WithPrimitiveTopology = false;
        public bool WithCullMode = false;

        public DynamicStatesConfigData()  { }
        
        #region OVERRIDE    
        public override string ToString() => string.Format($"Dynamic states config" );
        public override int GetHashCode() => HashCode.Combine(WithViewport,WithScissor ,WithLineWidth, WithPrimitiveTopology ,WithCullMode );
        public override bool Equals(object? obj) => obj is DynamicStatesConfigData data && this.Equals(data) ;
        public bool Equals(DynamicStatesConfigData other)=>  false;
        public static bool operator ==(DynamicStatesConfigData left, DynamicStatesConfigData right) => left.Equals(right);
        public static bool operator !=(DynamicStatesConfigData left, DynamicStatesConfigData  right) => !left.Equals(right);
        #endregion
    }

    public unsafe static void CreateDynamicStates(ref DynamicStatesConfigData data , out VkPipelineDynamicStateCreateInfo dynamicStateCreateInfo )
    {
        VkDynamicState[ ] dynamicStates2  = new VkDynamicState[2];

        dynamicStates2[0] = VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT;
        dynamicStates2[1] = VkDynamicState.VK_DYNAMIC_STATE_SCISSOR;

    
        dynamicStateCreateInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO;
        dynamicStateCreateInfo.dynamicStateCount = 2;
        fixed( VkDynamicState* dynamicStates = &dynamicStates2[0] ){
            dynamicStateCreateInfo.pDynamicStates =dynamicStates;
        }
        
        dynamicStateCreateInfo.pNext = null;
        dynamicStateCreateInfo.flags =0;
   
    }
}
/*
  // public struct DynamicStateConfig
    // {
    //     private bool UseViewport = true;

    //     public DynamicStateConfig()
    //     {
    //     }
    // }
    // public struct DynamicState
    // {
    //     private int Count =0;
    //     private List<VkDynamicState> states=new(10);
    //     private delegate void PFN_SETVIEWPORT( in VkCommandBuffer commandBuffer,in VkViewport viewport, uint count =1 );
    //     private PFN_SETVIEWPORT SetViewport = SetViewportEmpty;

    //     public DynamicState()   { }

    //     private static void SetViewportEmpty( in VkCommandBuffer commandBuffer,in VkViewport viewport, uint count=1 ){   _=commandBuffer;_=viewport;   }
    // }

    #region DYNAMIC STATES

        VkDynamicState* dynamicStates = stackalloc VkDynamicState[3] 
        {
            VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT,
            VkDynamicState.VK_DYNAMIC_STATE_SCISSOR,
            VkDynamicState.VK_DYNAMIC_STATE_LINE_WIDTH,
            // VkDynamicState.VK_DYNAMIC_STATE_PRIMITIVE_TOPOLOGY,
            //     VkDynamicState.VK_DYNAMIC_STATE_CULL_MODE,
            //     VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT_WITH_COUNT_EXT,
        };
        
        VkPipelineDynamicStateCreateInfo dynamicStateCreateInfo =  new();
        dynamicStateCreateInfo.sType = VkStructureType. VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO;
        dynamicStateCreateInfo.dynamicStateCount = 3;
        dynamicStateCreateInfo.pDynamicStates = dynamicStates;
        
        #endregion
*/