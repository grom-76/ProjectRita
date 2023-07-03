namespace RitaEngine.Graphic;

using RitaEngine.Math.Color;
using RitaEngine.Math;
using RitaEngine.API.Vulkan;

/// <summary>
/// RENAME WITH PIPELINE
/// </summary>
public sealed class GraphicRenderConfig :IDisposable
{
    public GraphicPipeline.Rasterization.RasterizationConfigData Rasterization = new();
    public GraphicPipeline.Multisampling.MultisamplingConfigData Multisampling = new();
    
    public GeometricPrimitive Primitive = new();
    public Camera Camera = new();
    public Palette BackColorARGB = Palette.Lavender;
    //Layout 
    public int MAX_FRAMES_IN_FLIGHT = 2;

    public string VertexShaderFileNameSPV ="";
    public string FragmentShaderFileNameSPV ="";
    public string FragmentEntryPoint ="";
    public string VertexEntryPoint="";



   
    public int VerticeSize =0;// = Vertices.Length * Vertices.Size ;
    public string TextureName ="wood.png";

    

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

