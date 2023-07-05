namespace RitaEngine.Graphic;

using RitaEngine.Math.Color;
using RitaEngine.Math;
using RitaEngine.API.Vulkan;

/// <summary>
/// RENAME WITH PIPELINE
/// </summary>
public sealed class GraphicRenderConfig :IDisposable
{
    public Pipeline.RasterizationConfigData Pipeline_Rasterization = new();
    public Pipeline.MultisamplingConfigData Pipeline_Multisampling = new();
    public Pipeline.DepthStencilConfigData Pipeline_DepthStencil = new();
    public Pipeline.DynamicStatesConfigData Pipeline_DynamicStates = new();
    public Pipeline.ColorBlendingConfigData Pipeline_ColorBlending = new();

    public GeometricPrimitive Primitive = new();
    public Camera Camera = new();
    public Palette BackColorARGB = Palette.Lavender;
    //Layout 
    public int MAX_FRAMES_IN_FLIGHT = 2;

    public string VertexShaderFileNameSPV ="";
    public string FragmentShaderFileNameSPV ="";
    public string FragmentEntryPoint ="";
    public string VertexEntryPoint="";
    public string TextureName ="wood.png";

    public PushConstantsMesh Mesh = new();
    

    public GraphicRenderConfig() {}

    public void Dispose()
    {
        
        GC.SuppressFinalize(this);
    }

    //Pipeline
    // VK_SAMPLE_COUNT_1_BIT
    //Shader

    //Vertex
}

