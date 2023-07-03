namespace RitaEngine.Graphic.GraphicPipeline;

using RitaEngine.API.Vulkan;

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public static class DepthStencil
{
    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct DepthStencilConfigData
    {
        public bool                                  DepthTestEnable = true;
        public bool                                  DepthWriteEnable = true;
        public CompareOp                           DepthCompareOp = CompareOp.VK_COMPARE_OP_LESS;
        public bool                                  DepthBoundsTestEnable = false;
        public bool                                  StencilTestEnable=false;
        public VkStencilOpState                      Front = new();
        public VkStencilOpState                      Back = new();
        public float                                 MinDepthBounds=0.0f;
        public float                                 MaxDepthBounds=1.0f;
        public DepthStencilState    Flags = DepthStencilState.VK_PIPELINE_DEPTH_STENCIL_STATE_CREATE_RASTERIZATION_ORDER_ATTACHMENT_DEPTH_ACCESS_BIT_ARM ;
        public DepthStencilConfigData()  { }
        
        #region OVERRIDE    
        public override string ToString() => string.Format($"Depth Stencil" );
        public override int GetHashCode() => HashCode.Combine(DepthTestEnable,DepthWriteEnable ,DepthBoundsTestEnable, MinDepthBounds ,MaxDepthBounds );
        public override bool Equals(object? obj) => obj is DepthStencilConfigData data && this.Equals(data) ;
        public bool Equals(DepthStencilConfigData other)=>  false;
        public static bool operator ==(DepthStencilConfigData left, DepthStencilConfigData right) => left.Equals(right);
        public static bool operator !=(DepthStencilConfigData left, DepthStencilConfigData  right) => !left.Equals(right);
        #endregion
    }

    public unsafe static void CreateDepthStencil(ref DepthStencilConfigData data , out VkPipelineDepthStencilStateCreateInfo depthStencilState )
    {
        depthStencilState.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
        depthStencilState.pNext = null;

        depthStencilState.depthTestEnable = data.DepthTestEnable ?  VK.VK_TRUE : VK.VK_FALSE;
        depthStencilState.depthWriteEnable =data.DepthWriteEnable  ?  VK.VK_TRUE : VK.VK_FALSE;
        depthStencilState.depthCompareOp = (VkCompareOp)data.DepthCompareOp;
        depthStencilState.depthBoundsTestEnable =data.DepthBoundsTestEnable ?  VK.VK_TRUE : VK.VK_FALSE;
        depthStencilState.stencilTestEnable = data.StencilTestEnable ? VK.VK_TRUE : VK.VK_FALSE;
        depthStencilState.maxDepthBounds = data.MaxDepthBounds;
        depthStencilState.minDepthBounds = data.MinDepthBounds;
        depthStencilState.flags = (uint)data.Flags;
        depthStencilState.front = data.Front ;
        depthStencilState.back = data.Back ;
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
}
