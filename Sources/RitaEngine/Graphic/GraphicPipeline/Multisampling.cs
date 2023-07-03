

namespace RitaEngine.Graphic.GraphicPipeline;

using RitaEngine.API.Vulkan;

[StructLayout(LayoutKind.Sequential, Pack = RitaEngine.Base.BaseHelper.FORCE_ALIGNEMENT), SkipLocalsInit]    
public static class Multisampling
{

    public struct MultisamplingConfigData
    {
        public SampleCount  RasterizationSamples = SampleCount.VK_SAMPLE_COUNT_1_BIT;
        public bool                   SampleShadingEnable  = false;
        public float                  MinSampleShading=  0.0f;
        public bool                   AlphaToCoverageEnable = false;
        public bool                   AlphaToOneEnable = false ;

        public MultisamplingConfigData()  { }
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
}


