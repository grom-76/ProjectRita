namespace RitaEngine.Base.Platform; //https://github.com/Syncaidius/MoltenEngine/tree/master/Molten.Engine/Settings

using System.Collections.Generic;
using RitaEngine.Base.Math.Color;
using RitaEngine.Base.Math.Vertex;
using RitaEngine.Base.Graphic;
using RitaEngine.Base.Platform.API.Vulkan;

/// <summary>
/// RENAME WITH PIPELINE
/// </summary>
public sealed class GraphicRenderConfig :IDisposable
{
    // RenderPass
    public Palette BackColorARGB = Palette.Lavender;
    //Layout 
    public int MAX_FRAMES_IN_FLIGHT = 2;

    public string VertexShaderFileNameSPV ="";
    public string FragmentShaderFileNameSPV ="";
    public string FragmentEntryPoint ="";
    public string VertexEntryPoint="";

    public bool VertexOutsideShader = false;
    public uint AttributeDescription = 2;

    public Position3f_Color3f_UV2f[] Vertices = new Position3f_Color3f_UV2f[] 
    {
        new(-0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f),
        new(0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f),
        new(0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f),
        new(-0.5f, 0.5f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f)
    };
    public short[] Indices = new short[] { 0, 1, 2, 2, 3, 0};

   
    public int VerticeSize =0;// = Vertices.Length * Vertices.Size ;
    public string TextureName ="wood.png";

    public Camera Camera = new();

    public GraphicRenderConfig() {}

    public void Dispose()
    {
        Indices = null!;
        Vertices = null!;
        GC.SuppressFinalize(this);
    }

    //Pipeline
    // VK_SAMPLE_COUNT_1_BIT
    //Shader

    //Vertex
}


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
    