using RitaEngine.API.Vulkan;

namespace RitaEngine.Graphic.GraphicPipeline;


//INSPIRED BY : https://github.com/veldrid/veldrid/blob/master/src/Veldrid

/// <summary>
/// VkPipelineRasterizationStateCreateInfo - Structure specifying parameters of a newly created pipeline rasterization state
/// </summary>
[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]
public static class Rasterization
{
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
}

