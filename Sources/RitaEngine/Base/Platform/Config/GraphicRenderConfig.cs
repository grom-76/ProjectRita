namespace RitaEngine.Base.Platform.Config; //https://github.com/Syncaidius/MoltenEngine/tree/master/Molten.Engine/Settings

using System.Collections.Generic;
using RitaEngine.Base.Math.Color;
using RitaEngine.Base.Platform.Native.Vulkan;

/// <summary>
/// RENAME WITH PIPELINE
/// </summary>
public sealed class GraphicRenderConfig 
{
    // RenderPass
    public Palette BackColorARGB = Palette.Lavender;
    //Layout 
    public int MAX_FRAMES_IN_FLIGHT = 2;

    public string VertexShaderFileNameSPV ="";
    public string FragmentShaderFileNameSPV ="";
    public string FragmentEntryPoint ="";
    public string VertexEntryPoint="";
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
    