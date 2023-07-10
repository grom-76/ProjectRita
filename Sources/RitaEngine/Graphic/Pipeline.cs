namespace RitaEngine.Graphic;

using RitaEngine.API.Vulkan;

//INSPIRED BY : https://github.com/veldrid/veldrid/blob/master/src/Veldrid



[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public static class Pipeline
{
#region Depth Stencil    
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

#endregion


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

    
    
    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct MultisamplingConfigData
    {
        public SampleCount  RasterizationSamples = SampleCount.VK_SAMPLE_COUNT_1_BIT;
        public bool                   SampleShadingEnable  = false;
        public float                  MinSampleShading=  0.0f;
        public bool                   AlphaToCoverageEnable = false;
        public bool                   AlphaToOneEnable = false ;

        public MultisamplingConfigData()  { }
        
        #region OVERRIDE    
        public override string ToString() => string.Format($"Multisampling" );
        public override int GetHashCode() => HashCode.Combine(MinSampleShading,AlphaToCoverageEnable ,AlphaToOneEnable, SampleShadingEnable  );
        public override bool Equals(object? obj) => obj is MultisamplingConfigData data && this.Equals(data) ;
        public bool Equals(MultisamplingConfigData other)=>  false;
        public static bool operator ==(MultisamplingConfigData left, MultisamplingConfigData right) => left.Equals(right);
        public static bool operator !=(MultisamplingConfigData left, MultisamplingConfigData  right) => !left.Equals(right);
        #endregion
    }


    public unsafe static void CreateMultisampling(ref MultisamplingConfigData data , out VkPipelineMultisampleStateCreateInfo multisampling , uint* samplemask = null)
    {
        // VkPipelineMultisampleStateCreateInfo multisampling=new();
        multisampling.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO;
        multisampling.pNext = null;
        multisampling.flags =0;
        multisampling.sampleShadingEnable =data.SampleShadingEnable ? VK.VK_TRUE:  VK.VK_FALSE;
        multisampling.rasterizationSamples =(VkSampleCountFlagBits) data.RasterizationSamples ;
        multisampling.alphaToCoverageEnable =  data.AlphaToCoverageEnable ? VK.VK_TRUE:  VK.VK_FALSE;
        multisampling.alphaToOneEnable = data.AlphaToOneEnable  ? VK.VK_TRUE:  VK.VK_FALSE;       
        multisampling.minSampleShading =data.MinSampleShading;
        multisampling.pSampleMask =samplemask;
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



    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct RasterizationConfigData : IEquatable<RasterizationConfigData>
    {
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
        public RasterizationConfigData() {  }
        public void Release()
        {
        }

        #region OVERRIDE    
        public override string ToString() => string.Format($"RasterizationConfigData" );
        public override int GetHashCode() => HashCode.Combine(LineWidth,DepthBiasSlopeFactor ,DepthBiasClamp, DepthBiasConstantFactor  );
        public override bool Equals(object? obj) => obj is RasterizationConfigData data && this.Equals(data) ;
        public bool Equals(RasterizationConfigData other)=>  false;
        public static bool operator ==(RasterizationConfigData left, RasterizationConfigData right) => left.Equals(right);
        public static bool operator !=(RasterizationConfigData left, RasterizationConfigData  right) => !left.Equals(right);
        #endregion

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

    public unsafe static void CreateRasterization( ref RasterizationConfigData data ,out VkPipelineRasterizationStateCreateInfo rasterizer)
    {
        // VkPipelineRasterizationStateCreateInfo rasterizer =new();
        rasterizer.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO;
        rasterizer.rasterizerDiscardEnable = data.RasterizerDiscardEnable?   VK.VK_TRUE : VK.VK_FALSE ;// VK.VK_FALSE;
        rasterizer.polygonMode = (VkPolygonMode) data.PolygonFillMode ;//  VkPolygonMode. VK_POLYGON_MODE_FILL;
        rasterizer.lineWidth = data.LineWidth;// 1.0f;
        rasterizer.cullMode =  (uint)data.FaceCullMode ; //( uint)VkCullModeFlagBits.VK_CULL_MODE_BACK_BIT;
        rasterizer.frontFace = (VkFrontFace) data.FrontFace ; //VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE;
        rasterizer.flags =0;
        rasterizer.pNext = null;
        rasterizer.depthBiasEnable = data.DepthBiasEnable? VK.VK_TRUE : VK.VK_FALSE ;// VK.VK_FALSE;
        rasterizer.depthClampEnable = data.DepthClampEnable? VK.VK_TRUE : VK.VK_FALSE ;   // VK.VK_FALSE;
        rasterizer.depthBiasClamp = data.DepthBiasClamp ; // 0.0f;
        rasterizer.depthBiasConstantFactor =data.DepthBiasConstantFactor ;  // 1.0f;
        rasterizer.depthBiasSlopeFactor = data.DepthBiasSlopeFactor ;   //1.0f;
    }

    public static void ReleaseRasterization( ref RasterizationConfigData data)
    {
        data.Release();
    }


    public unsafe static void CreateTessellation(out VkPipelineTessellationStateCreateInfo tessellationStateCreateInfo , uint numberOfControlPointsPerPatch =0)
    {
            
        #region Tesslation
           //not used 
        // VkPipelineTessellationStateCreateInfo tessellationStateCreateInfo = new();
        tessellationStateCreateInfo.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_TESSELLATION_STATE_CREATE_INFO;
        tessellationStateCreateInfo.pNext = null;
        tessellationStateCreateInfo.flags =0 ;
        tessellationStateCreateInfo.patchControlPoints = numberOfControlPointsPerPatch;
        #endregion
    }

    [StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
    public struct ColorBlendingConfigData
    {
        public VkBlendFactor ColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        public VkBlendFactor AlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        public VkBlendOp ColorBlendOperation = VkBlendOp.VK_BLEND_OP_ADD;
        public VkBlendOp AlphaBlendOperation = VkBlendOp.VK_BLEND_OP_ADD;

        public VkBlendFactor[] ColorBlendFactors = null!;
        public VkBlendFactor[] AlphaBlendFactors =null!;
        public VkBlendOp[] ColorBlendOperations = null!;
        public VkBlendOp[] AlphaBlendOperations = null!;
      
        public ColorBlendingConfigData()  { }
        
        #region OVERRIDE    
        public override string ToString() => string.Format($"Multisampling" );
        public override int GetHashCode() => HashCode.Combine(ColorBlendFactor, AlphaBlendFactor );
        public override bool Equals(object? obj) => obj is ColorBlendingConfigData data && this.Equals(data) ;
        public bool Equals(ColorBlendingConfigData other)=>  false;
        public static bool operator ==(ColorBlendingConfigData left, ColorBlendingConfigData right) => left.Equals(right);
        public static bool operator !=(ColorBlendingConfigData left, ColorBlendingConfigData  right) => !left.Equals(right);
        #endregion
    }

    public unsafe static void CreateColorBlending(ref ColorBlendingConfigData data, out VkPipelineColorBlendStateCreateInfo colorBlending , out VkPipelineColorBlendAttachmentState colorBlendAttachment)
    {
        // VkPipelineColorBlendAttachmentState colorBlendAttachment =new();
        colorBlendAttachment.colorWriteMask = (uint)(VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT);
        colorBlendAttachment.blendEnable = VK.VK_FALSE;
        colorBlendAttachment.srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD;
        colorBlendAttachment.colorBlendOp =  VkBlendOp.VK_BLEND_OP_ADD;
        colorBlendAttachment.dstAlphaBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.dstColorBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        
        // VkPipelineColorBlendStateCreateInfo colorBlending=new();
        colorBlending.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO;
        colorBlending.logicOpEnable = VK.VK_FALSE;
        colorBlending.logicOp = VkLogicOp. VK_LOGIC_OP_COPY;
        colorBlending.attachmentCount = 1;
        fixed(VkPipelineColorBlendAttachmentState* cb =  &colorBlendAttachment  )
        {
            colorBlending.pAttachments =cb ;
        }
        
        colorBlending.blendConstants[0] = 0.0f;
        colorBlending.blendConstants[1] = 0.0f;
        colorBlending.blendConstants[2] = 0.0f;
        colorBlending.blendConstants[3] = 0.0f;
        colorBlending.flags =0;
        colorBlending.pNext=null;

    }

    public unsafe static VkPipelineColorBlendStateCreateInfo  CreateColorBlendingRef(ref ColorBlendingConfigData data)
    {
        VkPipelineColorBlendAttachmentState colorBlendAttachment =new();
        colorBlendAttachment.colorWriteMask = (uint)(VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT);
        colorBlendAttachment.blendEnable = VK.VK_FALSE;
        colorBlendAttachment.srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD;
        colorBlendAttachment.colorBlendOp =  VkBlendOp.VK_BLEND_OP_ADD;
        colorBlendAttachment.dstAlphaBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        colorBlendAttachment.dstColorBlendFactor =VkBlendFactor.VK_BLEND_FACTOR_ZERO;
        
        VkPipelineColorBlendStateCreateInfo colorBlending=default;
        colorBlending.sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO;
        colorBlending.logicOpEnable = VK.VK_FALSE;
        colorBlending.logicOp = VkLogicOp. VK_LOGIC_OP_COPY;
        colorBlending.attachmentCount = 1;
      
        colorBlending.pAttachments =&colorBlendAttachment ;
        colorBlending.blendConstants[0] = 0.0f;
        colorBlending.blendConstants[1] = 0.0f;
        colorBlending.blendConstants[2] = 0.0f;
        colorBlending.blendConstants[3] = 0.0f;
        colorBlending.flags =0;
        colorBlending.pNext=null;
        return colorBlending;
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