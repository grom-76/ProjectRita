

namespace RitaEngine.Base.Platform.Config; 

using RitaEngine.Base.Math.Color;

//https://github.com/Syncaidius/MoltenEngine/tree/master/Molten.Engine/Settings

public sealed class GraphicDeviceConfig
{
    public string VulkanDllName = "vulkan-1.dll";
    public bool EnableDebugMode = false;
    public string[] ValidationLayerExtensions = new string[]{  
    "VK_LAYER_KHRONOS_validation",
    "VK_LAYER_LUNARG_standard_validation",
    "VK_LAYER_GOOGLE_threading",
    "VK_LAYER_LUNARG_parameter_validation",
    "VK_LAYER_LUNARG_object_tracker",
    "VK_LAYER_LUNARG_core_validation",
    "VK_LAYER_GOOGLE_unique_objects", };


    // public int MAX_FRAMES = 2;
    // // Synchronisation & cache control
    // public Palette BackColorARGB = Palette.Azure;

}

public enum GraphicDeviceClipVolume
{
    NegativeOneToPlusOne,// NO in glm Opengl definition
    ZeroToOne,//ZO in glm Dorect3D definition
}

public enum GraphicDeviceScreenOrigin
{
    lowerLeft,// origin in OpenGL
    UpperLeft,
    Center
}

public enum GraphicDeviceBackend
{
    Directx,
    Opengl,
    Vulkan,
    Api,
}