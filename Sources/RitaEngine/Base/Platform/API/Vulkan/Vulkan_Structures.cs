namespace RitaEngine.Base.Platform.API.Vulkan;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using System.Security;

using ConstChar = System.Byte;
using System.Diagnostics.CodeAnalysis;

#region Unions
#region VK_VERSION_1_0
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkClearColorValue {
	[FieldOffset(0)]public fixed float  float32[4];
	[FieldOffset(0)]public fixed Int32  int32[4];
	[FieldOffset(0)]public fixed UInt32  uint32[4];
	
	public VkClearColorValue(float r, float g, float b, float a = 1.0f): this()
    {     float32[0] = r;  float32[1] = g;   float32[2] = b;     float32[3] = a;    }
  	
	public VkClearColorValue(float[] color): this()
    {     float32[0] = color[0];  float32[1] = color[1];   float32[2] = color[2];     float32[3] = color[3];    }

	public VkClearColorValue(int r, int g, int b, int a = 255): this()
    {     int32[0] =r ;    int32[1] =g ; int32[2] =b ; int32[3] =a ;}

	public VkClearColorValue(uint r, uint g, uint b, uint a = 255): this()
    {     uint32[0] =r ;    uint32[1] =g ; uint32[2] =b ; uint32[3] =a ;}
    
	public VkClearColorValue(int[] color): this()
    {     int32[0] = color[0];  int32[1] = color[1];   int32[2] = color[2];    int32[3] = 255;    }

	public VkClearColorValue(uint[] color): this()
    {     uint32[0] = color[0];  uint32[1] = color[1];   uint32[2] = color[2];    uint32[3] = 255;    }
}

[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkClearValue {
	[FieldOffset(0)]public  VkClearColorValue  color;
	[FieldOffset(0)]public  VkClearDepthStencilValue  depthStencil;

	
	public VkClearValue( float depth =0.0f, uint stencil=0)
    {
        depthStencil =new(depth,stencil);
    }

	public VkClearValue(float[] rgba  )
	{
        color = new(rgba);
	}

	public VkClearValue(int[] rgba  )
	{
        color = new(rgba);
	}

	public VkClearValue(uint[] rgba  )
	{
        color = new(rgba);
	}
}

#endregion
#region VK_KHR_performance_query
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkPerformanceCounterResultKHR {
	[FieldOffset(0)]public  Int32  int32;
	[FieldOffset(0)]public  Int64  int64;
	[FieldOffset(0)]public  UInt32  uint32;
	[FieldOffset(0)]public  UInt64  uint64;
	[FieldOffset(0)]public  float  float32;
	[FieldOffset(0)]public  double  float64;
}

#endregion
#region VK_KHR_pipeline_executable_properties
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkPipelineExecutableStatisticValueKHR {
	[FieldOffset(0)]public  UInt32  b32;
	[FieldOffset(0)]public  Int64  i64;
	[FieldOffset(0)]public  UInt64  u64;
	[FieldOffset(0)]public  double  f64;
}

#endregion
#region VK_INTEL_performance_query
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkPerformanceValueDataINTEL {
	[FieldOffset(0)]public  UInt32  value32;
	[FieldOffset(0)]public  UInt64  value64;
	[FieldOffset(0)]public  float  valueFloat;
	[FieldOffset(0)]public  UInt32  valueBool;
	[FieldOffset(0)]public  ConstChar*  valueString;
}

#endregion
#region VK_NV_ray_tracing_motion_blur
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkDeviceOrHostAddressConstKHR {
	[FieldOffset(0)]public  UInt64  deviceAddress;
	[FieldOffset(0)]public  void*  hostAddress;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkAccelerationStructureMotionInstanceDataNV {
	[FieldOffset(0)]public  VkAccelerationStructureInstanceKHR  staticInstance;
	[FieldOffset(0)]public  VkAccelerationStructureMatrixMotionInstanceNV  matrixMotionInstance;
	[FieldOffset(0)]public  VkAccelerationStructureSRTMotionInstanceNV  srtMotionInstance;
}

#endregion
#region VK_EXT_opacity_micromap
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkDeviceOrHostAddressKHR {
	[FieldOffset(0)]public  UInt64  deviceAddress;
	[FieldOffset(0)]public  void*  hostAddress;
}

#endregion
#region VK_KHR_acceleration_structure
[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkAccelerationStructureGeometryDataKHR {
	[FieldOffset(0)]public  VkAccelerationStructureGeometryTrianglesDataKHR  triangles;
	[FieldOffset(0)]public  VkAccelerationStructureGeometryAabbsDataKHR  aabbs;
	[FieldOffset(0)]public  VkAccelerationStructureGeometryInstancesDataKHR  instances;
}

#endregion
#endregion

#region Structures
#region VK_VERSION_1_0
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExtent2D {
	public  UInt32  width;
	public  UInt32  height;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExtent3D {
	public  UInt32  width;
	public  UInt32  height;
	public  UInt32  depth;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOffset2D {
	public  Int32  x;
	public  Int32  y;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOffset3D {
	public  Int32  x;
	public  Int32  y;
	public  Int32  z;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRect2D {
	public  VkOffset2D  offset;
	public  VkExtent2D  extent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBaseInStructure {
	public  VkStructureType  sType;
	public  VkBaseInStructure*  pNext;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBaseOutStructure {
	public  VkStructureType  sType;
	public  VkBaseOutStructure*  pNext;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferMemoryBarrier {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  srcAccessMask;
	public  UInt32  dstAccessMask;
	public  UInt32  srcQueueFamilyIndex;
	public  UInt32  dstQueueFamilyIndex;
	public  VkBuffer  buffer;
	public  UInt64  offset;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDispatchIndirectCommand {
	public  UInt32  x;
	public  UInt32  y;
	public  UInt32  z;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrawIndexedIndirectCommand {
	public  UInt32  indexCount;
	public  UInt32  instanceCount;
	public  UInt32  firstIndex;
	public  Int32  vertexOffset;
	public  UInt32  firstInstance;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrawIndirectCommand {
	public  UInt32  vertexCount;
	public  UInt32  instanceCount;
	public  UInt32  firstVertex;
	public  UInt32  firstInstance;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageSubresourceRange {
	public  UInt32  aspectMask;
	public  UInt32  baseMipLevel;
	public  UInt32  levelCount;
	public  UInt32  baseArrayLayer;
	public  UInt32  layerCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageMemoryBarrier {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  srcAccessMask;
	public  UInt32  dstAccessMask;
	public  VkImageLayout  oldLayout;
	public  VkImageLayout  newLayout;
	public  UInt32  srcQueueFamilyIndex;
	public  UInt32  dstQueueFamilyIndex;
	public  VkImage  image;
	public  VkImageSubresourceRange  subresourceRange;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryBarrier {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  srcAccessMask;
	public  UInt32  dstAccessMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCacheHeaderVersionOne {
	public  UInt32  headerSize;
	public  VkPipelineCacheHeaderVersion  headerVersion;
	public  UInt32  vendorID;
	public  UInt32  deviceID;
	public fixed ConstChar  pipelineCacheUUID[(int)VK.VK_UUID_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAllocationCallbacks {
	public  void*  pUserData;
	public  delegate* unmanaged< void*,nuint,nuint,VkSystemAllocationScope,void* >  pfnAllocation;
	public  delegate* unmanaged< void*,void*,nuint,nuint,VkSystemAllocationScope,void* >  pfnReallocation;
	public  delegate* unmanaged< void*,void*,void >  pfnFree;
	public  delegate* unmanaged< void*,nuint,VkInternalAllocationType,VkSystemAllocationScope,void >  pfnInternalAllocation;
	public  delegate* unmanaged< void*,nuint,VkInternalAllocationType,VkSystemAllocationScope,void >  pfnInternalFree;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkApplicationInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  ConstChar*  pApplicationName;
	public  UInt32  applicationVersion;
	public  ConstChar*  pEngineName;
	public  UInt32  engineVersion;
	public  UInt32  apiVersion;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFormatProperties {
	public  UInt32  linearTilingFeatures;
	public  UInt32  optimalTilingFeatures;
	public  UInt32  bufferFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageFormatProperties {
	public  VkExtent3D  maxExtent;
	public  UInt32  maxMipLevels;
	public  UInt32  maxArrayLayers;
	public  UInt32  sampleCounts;
	public  UInt64  maxResourceSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkInstanceCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkApplicationInfo*  pApplicationInfo;
	public  UInt32  enabledLayerCount;
	public  ConstChar**  ppEnabledLayerNames;
	public  UInt32  enabledExtensionCount;
	public  ConstChar**  ppEnabledExtensionNames;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryHeap {
	public  UInt64  size;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryType {
	public  UInt32  propertyFlags;
	public  UInt32  heapIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFeatures {
	public  UInt32  robustBufferAccess;
	public  UInt32  fullDrawIndexUint32;
	public  UInt32  imageCubeArray;
	public  UInt32  independentBlend;
	public  UInt32  geometryShader;
	public  UInt32  tessellationShader;
	public  UInt32  sampleRateShading;
	public  UInt32  dualSrcBlend;
	public  UInt32  logicOp;
	public  UInt32  multiDrawIndirect;
	public  UInt32  drawIndirectFirstInstance;
	public  UInt32  depthClamp;
	public  UInt32  depthBiasClamp;
	public  UInt32  fillModeNonSolid;
	public  UInt32  depthBounds;
	public  UInt32  wideLines;
	public  UInt32  largePoints;
	public  UInt32  alphaToOne;
	public  UInt32  multiViewport;
	public  UInt32  samplerAnisotropy;
	public  UInt32  textureCompressionETC2;
	public  UInt32  textureCompressionASTC_LDR;
	public  UInt32  textureCompressionBC;
	public  UInt32  occlusionQueryPrecise;
	public  UInt32  pipelineStatisticsQuery;
	public  UInt32  vertexPipelineStoresAndAtomics;
	public  UInt32  fragmentStoresAndAtomics;
	public  UInt32  shaderTessellationAndGeometryPointSize;
	public  UInt32  shaderImageGatherExtended;
	public  UInt32  shaderStorageImageExtendedFormats;
	public  UInt32  shaderStorageImageMultisample;
	public  UInt32  shaderStorageImageReadWithoutFormat;
	public  UInt32  shaderStorageImageWriteWithoutFormat;
	public  UInt32  shaderUniformBufferArrayDynamicIndexing;
	public  UInt32  shaderSampledImageArrayDynamicIndexing;
	public  UInt32  shaderStorageBufferArrayDynamicIndexing;
	public  UInt32  shaderStorageImageArrayDynamicIndexing;
	public  UInt32  shaderClipDistance;
	public  UInt32  shaderCullDistance;
	public  UInt32  shaderFloat64;
	public  UInt32  shaderInt64;
	public  UInt32  shaderInt16;
	public  UInt32  shaderResourceResidency;
	public  UInt32  shaderResourceMinLod;
	public  UInt32  sparseBinding;
	public  UInt32  sparseResidencyBuffer;
	public  UInt32  sparseResidencyImage2D;
	public  UInt32  sparseResidencyImage3D;
	public  UInt32  sparseResidency2Samples;
	public  UInt32  sparseResidency4Samples;
	public  UInt32  sparseResidency8Samples;
	public  UInt32  sparseResidency16Samples;
	public  UInt32  sparseResidencyAliased;
	public  UInt32  variableMultisampleRate;
	public  UInt32  inheritedQueries;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceLimits {
	public  UInt32  maxImageDimension1D;
	public  UInt32  maxImageDimension2D;
	public  UInt32  maxImageDimension3D;
	public  UInt32  maxImageDimensionCube;
	public  UInt32  maxImageArrayLayers;
	public  UInt32  maxTexelBufferElements;
	public  UInt32  maxUniformBufferRange;
	public  UInt32  maxStorageBufferRange;
	public  UInt32  maxPushConstantsSize;
	public  UInt32  maxMemoryAllocationCount;
	public  UInt32  maxSamplerAllocationCount;
	public  UInt64  bufferImageGranularity;
	public  UInt64  sparseAddressSpaceSize;
	public  UInt32  maxBoundDescriptorSets;
	public  UInt32  maxPerStageDescriptorSamplers;
	public  UInt32  maxPerStageDescriptorUniformBuffers;
	public  UInt32  maxPerStageDescriptorStorageBuffers;
	public  UInt32  maxPerStageDescriptorSampledImages;
	public  UInt32  maxPerStageDescriptorStorageImages;
	public  UInt32  maxPerStageDescriptorInputAttachments;
	public  UInt32  maxPerStageResources;
	public  UInt32  maxDescriptorSetSamplers;
	public  UInt32  maxDescriptorSetUniformBuffers;
	public  UInt32  maxDescriptorSetUniformBuffersDynamic;
	public  UInt32  maxDescriptorSetStorageBuffers;
	public  UInt32  maxDescriptorSetStorageBuffersDynamic;
	public  UInt32  maxDescriptorSetSampledImages;
	public  UInt32  maxDescriptorSetStorageImages;
	public  UInt32  maxDescriptorSetInputAttachments;
	public  UInt32  maxVertexInputAttributes;
	public  UInt32  maxVertexInputBindings;
	public  UInt32  maxVertexInputAttributeOffset;
	public  UInt32  maxVertexInputBindingStride;
	public  UInt32  maxVertexOutputComponents;
	public  UInt32  maxTessellationGenerationLevel;
	public  UInt32  maxTessellationPatchSize;
	public  UInt32  maxTessellationControlPerVertexInputComponents;
	public  UInt32  maxTessellationControlPerVertexOutputComponents;
	public  UInt32  maxTessellationControlPerPatchOutputComponents;
	public  UInt32  maxTessellationControlTotalOutputComponents;
	public  UInt32  maxTessellationEvaluationInputComponents;
	public  UInt32  maxTessellationEvaluationOutputComponents;
	public  UInt32  maxGeometryShaderInvocations;
	public  UInt32  maxGeometryInputComponents;
	public  UInt32  maxGeometryOutputComponents;
	public  UInt32  maxGeometryOutputVertices;
	public  UInt32  maxGeometryTotalOutputComponents;
	public  UInt32  maxFragmentInputComponents;
	public  UInt32  maxFragmentOutputAttachments;
	public  UInt32  maxFragmentDualSrcAttachments;
	public  UInt32  maxFragmentCombinedOutputResources;
	public  UInt32  maxComputeSharedMemorySize;
	public fixed UInt32  maxComputeWorkGroupCount[3];
	public  UInt32  maxComputeWorkGroupInvocations;
	public fixed UInt32  maxComputeWorkGroupSize[3];
	public  UInt32  subPixelPrecisionBits;
	public  UInt32  subTexelPrecisionBits;
	public  UInt32  mipmapPrecisionBits;
	public  UInt32  maxDrawIndexedIndexValue;
	public  UInt32  maxDrawIndirectCount;
	public  float  maxSamplerLodBias;
	public  float  maxSamplerAnisotropy;
	public  UInt32  maxViewports;
	public fixed UInt32  maxViewportDimensions[2];
	public fixed float  viewportBoundsRange[2];
	public  UInt32  viewportSubPixelBits;
	public  Int32  minMemoryMapAlignment;
	public  UInt64  minTexelBufferOffsetAlignment;
	public  UInt64  minUniformBufferOffsetAlignment;
	public  UInt64  minStorageBufferOffsetAlignment;
	public  Int32  minTexelOffset;
	public  UInt32  maxTexelOffset;
	public  Int32  minTexelGatherOffset;
	public  UInt32  maxTexelGatherOffset;
	public  float  minInterpolationOffset;
	public  float  maxInterpolationOffset;
	public  UInt32  subPixelInterpolationOffsetBits;
	public  UInt32  maxFramebufferWidth;
	public  UInt32  maxFramebufferHeight;
	public  UInt32  maxFramebufferLayers;
	public  UInt32  framebufferColorSampleCounts;
	public  UInt32  framebufferDepthSampleCounts;
	public  UInt32  framebufferStencilSampleCounts;
	public  UInt32  framebufferNoAttachmentsSampleCounts;
	public  UInt32  maxColorAttachments;
	public  UInt32  sampledImageColorSampleCounts;
	public  UInt32  sampledImageIntegerSampleCounts;
	public  UInt32  sampledImageDepthSampleCounts;
	public  UInt32  sampledImageStencilSampleCounts;
	public  UInt32  storageImageSampleCounts;
	public  UInt32  maxSampleMaskWords;
	public  UInt32  timestampComputeAndGraphics;
	public  float  timestampPeriod;
	public  UInt32  maxClipDistances;
	public  UInt32  maxCullDistances;
	public  UInt32  maxCombinedClipAndCullDistances;
	public  UInt32  discreteQueuePriorities;
	public fixed float  pointSizeRange[2];
	public fixed float  lineWidthRange[2];
	public  float  pointSizeGranularity;
	public  float  lineWidthGranularity;
	public  UInt32  strictLines;
	public  UInt32  standardSampleLocations;
	public  UInt64  optimalBufferCopyOffsetAlignment;
	public  UInt64  optimalBufferCopyRowPitchAlignment;
	public  UInt64  nonCoherentAtomSize;
}

// [StructLayout(LayoutKind.Sequential)]
// public unsafe  struct VkPhysicalDeviceMemoryProperties {
// 	public  UInt32  memoryTypeCount;
// 	public VkMemoryType**  memoryTypes;//[(int)VK_MAX_MEMORY_TYPES];
// 	public  UInt32  memoryHeapCount;
// 	public VkMemoryHeap**  memoryHeaps;//[(int)VK_MAX_MEMORY_HEAPS];
// }

/// <summary>
/// Courtesy of : https://github.com/amerkoleci/Vortice.Vulkan/blob/main/src/Vortice.Vulkan/Generated/Structures.cs
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public partial struct VkPhysicalDeviceMemoryProperties
{
	public uint memoryTypeCount;
	public memoryTypes__FixedBuffer memoryTypes;

	public unsafe struct memoryTypes__FixedBuffer
	{
		public VkMemoryType e0;
		public VkMemoryType e1;
		public VkMemoryType e2;
		public VkMemoryType e3;
		public VkMemoryType e4;
		public VkMemoryType e5;
		public VkMemoryType e6;
		public VkMemoryType e7;
		public VkMemoryType e8;
		public VkMemoryType e9;
		public VkMemoryType e10;
		public VkMemoryType e11;
		public VkMemoryType e12;
		public VkMemoryType e13;
		public VkMemoryType e14;
		public VkMemoryType e15;
		public VkMemoryType e16;
		public VkMemoryType e17;
		public VkMemoryType e18;
		public VkMemoryType e19;
		public VkMemoryType e20;
		public VkMemoryType e21;
		public VkMemoryType e22;
		public VkMemoryType e23;
		public VkMemoryType e24;
		public VkMemoryType e25;
		public VkMemoryType e26;
		public VkMemoryType e27;
		public VkMemoryType e28;
		public VkMemoryType e29;
		public VkMemoryType e30;
		public VkMemoryType e31;

		[UnscopedRef]
		public ref VkMemoryType this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref AsSpan()[index];
			}
		}

		[UnscopedRef]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<VkMemoryType> AsSpan()
		{
			return MemoryMarshal.CreateSpan(ref e0, 32);
		}
	}
	public uint memoryHeapCount;
	public memoryHeaps__FixedBuffer memoryHeaps;

	public unsafe struct memoryHeaps__FixedBuffer
	{
		public VkMemoryHeap e0;
		public VkMemoryHeap e1;
		public VkMemoryHeap e2;
		public VkMemoryHeap e3;
		public VkMemoryHeap e4;
		public VkMemoryHeap e5;
		public VkMemoryHeap e6;
		public VkMemoryHeap e7;
		public VkMemoryHeap e8;
		public VkMemoryHeap e9;
		public VkMemoryHeap e10;
		public VkMemoryHeap e11;
		public VkMemoryHeap e12;
		public VkMemoryHeap e13;
		public VkMemoryHeap e14;
		public VkMemoryHeap e15;

		[UnscopedRef]
		public ref VkMemoryHeap this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref AsSpan()[index];
			}
		}

		[UnscopedRef]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<VkMemoryHeap> AsSpan()
		{
			return MemoryMarshal.CreateSpan(ref e0, 16);
		}
	}
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSparseProperties {
	public  UInt32  residencyStandard2DBlockShape;
	public  UInt32  residencyStandard2DMultisampleBlockShape;
	public  UInt32  residencyStandard3DBlockShape;
	public  UInt32  residencyAlignedMipSize;
	public  UInt32  residencyNonResidentStrict;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceProperties {
	public  UInt32  apiVersion;
	public  UInt32  driverVersion;
	public  UInt32  vendorID;
	public  UInt32  deviceID;
	public  VkPhysicalDeviceType  deviceType;
	public fixed ConstChar  deviceName[(int)VK.VK_MAX_PHYSICAL_DEVICE_NAME_SIZE];
	public fixed ConstChar  pipelineCacheUUID[(int)VK.VK_UUID_SIZE];
	public  VkPhysicalDeviceLimits  limits;
	public  VkPhysicalDeviceSparseProperties  sparseProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueueFamilyProperties {
	public  VkQueueFlagBits  queueFlags;
	public  UInt32  queueCount;
	public  UInt32  timestampValidBits;
	public  VkExtent3D  minImageTransferGranularity;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceQueueCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  queueFamilyIndex;
	public  UInt32  queueCount;
	public  float*  pQueuePriorities;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  queueCreateInfoCount;
	public  VkDeviceQueueCreateInfo*  pQueueCreateInfos;
	public  UInt32  enabledLayerCount;
	public  ConstChar**  ppEnabledLayerNames;
	public  UInt32  enabledExtensionCount;
	public  ConstChar**  ppEnabledExtensionNames;
	public  VkPhysicalDeviceFeatures*  pEnabledFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExtensionProperties {
	public fixed ConstChar  extensionName[(int)VK.VK_MAX_EXTENSION_NAME_SIZE];
	public  UInt32  specVersion;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkLayerProperties {
	public fixed ConstChar  layerName[(int)VK.VK_MAX_EXTENSION_NAME_SIZE];
	public  UInt32  specVersion;
	public  UInt32  implementationVersion;
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubmitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  waitSemaphoreCount;
	public  VkSemaphore*  pWaitSemaphores;
	public  UInt32*  pWaitDstStageMask;
	public  UInt32  commandBufferCount;
	public  VkCommandBuffer*  pCommandBuffers;
	public  UInt32  signalSemaphoreCount;
	public  VkSemaphore*  pSignalSemaphores;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMappedMemoryRange {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceMemory  memory;
	public  UInt64  offset;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  allocationSize;
	public  UInt32  memoryTypeIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryRequirements {
	public  UInt64  size;
	public  UInt64  alignment;
	public  UInt32  memoryTypeBits;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseMemoryBind {
	public  UInt64  resourceOffset;
	public  UInt64  size;
	public  VkDeviceMemory  memory;
	public  UInt64  memoryOffset;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseBufferMemoryBindInfo {
	public  VkBuffer  buffer;
	public  UInt32  bindCount;
	public  VkSparseMemoryBind*  pBinds;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageOpaqueMemoryBindInfo {
	public  VkImage  image;
	public  UInt32  bindCount;
	public  VkSparseMemoryBind*  pBinds;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageSubresource {
	public  UInt32  aspectMask;
	public  UInt32  mipLevel;
	public  UInt32  arrayLayer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageMemoryBind {
	public  VkImageSubresource  subresource;
	public  VkOffset3D  offset;
	public  VkExtent3D  extent;
	public  VkDeviceMemory  memory;
	public  UInt64  memoryOffset;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageMemoryBindInfo {
	public  VkImage  image;
	public  UInt32  bindCount;
	public  VkSparseImageMemoryBind*  pBinds;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindSparseInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  waitSemaphoreCount;
	public  VkSemaphore*  pWaitSemaphores;
	public  UInt32  bufferBindCount;
	public  VkSparseBufferMemoryBindInfo*  pBufferBinds;
	public  UInt32  imageOpaqueBindCount;
	public  VkSparseImageOpaqueMemoryBindInfo*  pImageOpaqueBinds;
	public  UInt32  imageBindCount;
	public  VkSparseImageMemoryBindInfo*  pImageBinds;
	public  UInt32  signalSemaphoreCount;
	public  VkSemaphore*  pSignalSemaphores;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageFormatProperties {
	public  UInt32  aspectMask;
	public  VkExtent3D  imageGranularity;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageMemoryRequirements {
	public  VkSparseImageFormatProperties  formatProperties;
	public  UInt32  imageMipTailFirstLod;
	public  UInt64  imageMipTailSize;
	public  UInt64  imageMipTailOffset;
	public  UInt64  imageMipTailStride;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFenceCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSemaphoreCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkEventCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueryPoolCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkQueryType  queryType;
	public  UInt32  queryCount;
	public  UInt32  pipelineStatistics;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt64  size;
	public  UInt32  usage;
	public  VkSharingMode  sharingMode;
	public  UInt32  queueFamilyIndexCount;
	public  UInt32*  pQueueFamilyIndices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferViewCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkBuffer  buffer;
	public  VkFormat  format;
	public  UInt64  offset;
	public  UInt64  range;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkImageType  imageType;
	public  VkFormat  format;
	public  VkExtent3D  extent;
	public  UInt32  mipLevels;
	public  UInt32  arrayLayers;
	public  VkSampleCountFlagBits  samples;
	public  VkImageTiling  tiling;
	public  UInt32  usage;
	public  VkSharingMode  sharingMode;
	public  UInt32  queueFamilyIndexCount;
	public  UInt32*  pQueueFamilyIndices;
	public  VkImageLayout  initialLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubresourceLayout {
	public  UInt64  offset;
	public  UInt64  size;
	public  UInt64  rowPitch;
	public  UInt64  arrayPitch;
	public  UInt64  depthPitch;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkComponentMapping {
	public  VkComponentSwizzle  r;
	public  VkComponentSwizzle  g;
	public  VkComponentSwizzle  b;
	public  VkComponentSwizzle  a;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkImage  image;
	public  VkImageViewType  viewType;
	public  VkFormat  format;
	public  VkComponentMapping  components;
	public  VkImageSubresourceRange  subresourceRange;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkShaderModuleCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  nint  codeSize;
	public  UInt32*  pCode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCacheCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  Int32  initialDataSize;
	public  void*  pInitialData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSpecializationMapEntry {
	public  UInt32  antID;
	public  UInt32  offset;
	public  Int32  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSpecializationInfo {
	public  UInt32  mapEntryCount;
	public  VkSpecializationMapEntry*  pMapEntries;
	public  Int32  dataSize;
	public  void*  pData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineShaderStageCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkShaderStageFlagBits  stage;
	public  VkShaderModule  module;
	public  ConstChar*  pName;
	public  VkSpecializationInfo*  pSpecializationInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkComputePipelineCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkPipelineShaderStageCreateInfo  stage;
	public  VkPipelineLayout  layout;
	public  VkPipeline  basePipelineHandle;
	public  Int32  basePipelineIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkVertexInputBindingDescription {
	public  UInt32  binding;
	public  UInt32  stride;
	public  VkVertexInputRate  inputRate;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkVertexInputAttributeDescription {
	public  UInt32  location;
	public  UInt32  binding;
	public  VkFormat  format;
	public  UInt32  offset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineVertexInputStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  vertexBindingDescriptionCount;
	public  VkVertexInputBindingDescription*  pVertexBindingDescriptions;
	public  UInt32  vertexAttributeDescriptionCount;
	public  VkVertexInputAttributeDescription*  pVertexAttributeDescriptions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineInputAssemblyStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkPrimitiveTopology  topology;
	public  UInt32  primitiveRestartEnable;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineTessellationStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  patchControlPoints;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkViewport {
	public  float  x;
	public  float  y;
	public  float  width;
	public  float  height;
	public  float  minDepth;
	public  float  maxDepth;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  viewportCount;
	public  VkViewport*  pViewports;
	public  UInt32  scissorCount;
	public  VkRect2D*  pScissors;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  depthClampEnable;
	public  UInt32  rasterizerDiscardEnable;
	public  VkPolygonMode  polygonMode;
	public  UInt32  cullMode;
	public  VkFrontFace  frontFace;
	public  UInt32  depthBiasEnable;
	public  float  depthBiasConstantFactor;
	public  float  depthBiasClamp;
	public  float  depthBiasSlopeFactor;
	public  float  lineWidth;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineMultisampleStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkSampleCountFlagBits  rasterizationSamples;
	public  UInt32  sampleShadingEnable;
	public  float  minSampleShading;
	public  UInt32*  pSampleMask;
	public  UInt32  alphaToCoverageEnable;
	public  UInt32  alphaToOneEnable;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkStencilOpState {
	public  VkStencilOp  failOp;
	public  VkStencilOp  passOp;
	public  VkStencilOp  depthFailOp;
	public  VkCompareOp  compareOp;
	public  UInt32  compareMask;
	public  UInt32  writeMask;
	public  UInt32  reference;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineDepthStencilStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  depthTestEnable;
	public  UInt32  depthWriteEnable;
	public  VkCompareOp  depthCompareOp;
	public  UInt32  depthBoundsTestEnable;
	public  UInt32  stencilTestEnable;
	public  VkStencilOpState  front;
	public  VkStencilOpState  back;
	public  float  minDepthBounds;
	public  float  maxDepthBounds;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineColorBlendAttachmentState {
	public  UInt32  blendEnable;
	public  VkBlendFactor  srcColorBlendFactor;
	public  VkBlendFactor  dstColorBlendFactor;
	public  VkBlendOp  colorBlendOp;
	public  VkBlendFactor  srcAlphaBlendFactor;
	public  VkBlendFactor  dstAlphaBlendFactor;
	public  VkBlendOp  alphaBlendOp;
	public  UInt32  colorWriteMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineColorBlendStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  logicOpEnable;
	public  VkLogicOp  logicOp;
	public  UInt32  attachmentCount;
	public  VkPipelineColorBlendAttachmentState*  pAttachments;
	public fixed float  blendConstants[4];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineDynamicStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  dynamicStateCount;
	public  VkDynamicState*  pDynamicStates;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGraphicsPipelineCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  stageCount;
	public  VkPipelineShaderStageCreateInfo*  pStages;
	public  VkPipelineVertexInputStateCreateInfo*  pVertexInputState;
	public  VkPipelineInputAssemblyStateCreateInfo*  pInputAssemblyState;
	public  VkPipelineTessellationStateCreateInfo*  pTessellationState;
	public  VkPipelineViewportStateCreateInfo*  pViewportState;
	public  VkPipelineRasterizationStateCreateInfo*  pRasterizationState;
	public  VkPipelineMultisampleStateCreateInfo*  pMultisampleState;
	public  VkPipelineDepthStencilStateCreateInfo*  pDepthStencilState;
	public  VkPipelineColorBlendStateCreateInfo*  pColorBlendState;
	public  VkPipelineDynamicStateCreateInfo*  pDynamicState;
	public  VkPipelineLayout  layout;
	public  VkRenderPass  renderPass;
	public  UInt32  subpass;
	public  VkPipeline  basePipelineHandle;
	public  Int32  basePipelineIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPushConstantRange {
	public  UInt32  stageFlags;
	public  UInt32  offset;
	public  UInt32  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineLayoutCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  setLayoutCount;
	public  VkDescriptorSetLayout*  pSetLayouts;
	public  UInt32  pushConstantRangeCount;
	public  VkPushConstantRange*  pPushConstantRanges;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkFilter  magFilter;
	public  VkFilter  minFilter;
	public  VkSamplerMipmapMode  mipmapMode;
	public  VkSamplerAddressMode  addressModeU;
	public  VkSamplerAddressMode  addressModeV;
	public  VkSamplerAddressMode  addressModeW;
	public  float  mipLodBias;
	public  UInt32  anisotropyEnable;
	public  float  maxAnisotropy;
	public  UInt32  compareEnable;
	public  VkCompareOp  compareOp;
	public  float  minLod;
	public  float  maxLod;
	public  VkBorderColor  borderColor;
	public  UInt32  unnormalizedCoordinates;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyDescriptorSet {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDescriptorSet  srcSet;
	public  UInt32  srcBinding;
	public  UInt32  srcArrayElement;
	public  VkDescriptorSet  dstSet;
	public  UInt32  dstBinding;
	public  UInt32  dstArrayElement;
	public  UInt32  descriptorCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorBufferInfo {
	public  VkBuffer  buffer;
	public  UInt64  offset;
	public  UInt64  range;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorImageInfo {
	public  VkSampler  sampler;
	public  VkImageView  imageView;
	public  VkImageLayout  imageLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorPoolSize {
	public  VkDescriptorType  type;
	public  UInt32  descriptorCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorPoolCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  maxSets;
	public  UInt32  poolSizeCount;
	public  VkDescriptorPoolSize*  pPoolSizes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDescriptorPool  descriptorPool;
	public  UInt32  descriptorSetCount;
	public  VkDescriptorSetLayout*  pSetLayouts;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetLayoutBinding {
	public  UInt32  binding;
	public  VkDescriptorType  descriptorType;
	public  UInt32  descriptorCount;
	public  UInt32  stageFlags;
	public  VkSampler*  pImmutableSamplers;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetLayoutCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  bindingCount;
	public  VkDescriptorSetLayoutBinding*  pBindings;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkWriteDescriptorSet {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDescriptorSet  dstSet;
	public  UInt32  dstBinding;
	public  UInt32  dstArrayElement;
	public  UInt32  descriptorCount;
	public  VkDescriptorType  descriptorType;
	public  VkDescriptorImageInfo*  pImageInfo;
	public  VkDescriptorBufferInfo*  pBufferInfo;
	public  VkBufferView*  pTexelBufferView;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentDescription {
	public  UInt32  flags;
	public  VkFormat  format;
	public  VkSampleCountFlagBits  samples;
	public  VkAttachmentLoadOp  loadOp;
	public  VkAttachmentStoreOp  storeOp;
	public  VkAttachmentLoadOp  stencilLoadOp;
	public  VkAttachmentStoreOp  stencilStoreOp;
	public  VkImageLayout  initialLayout;
	public  VkImageLayout  finalLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentReference {
	public  UInt32  attachment;
	public  VkImageLayout  layout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFramebufferCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkRenderPass  renderPass;
	public  UInt32  attachmentCount;
	public  VkImageView*  pAttachments;
	public  UInt32  width;
	public  UInt32  height;
	public  UInt32  layers;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassDescription {
	public  UInt32  flags;
	public  VkPipelineBindPoint  pipelineBindPoint;
	public  UInt32  inputAttachmentCount;
	public  VkAttachmentReference*  pInputAttachments;
	public  UInt32  colorAttachmentCount;
	public  VkAttachmentReference*  pColorAttachments;
	public  VkAttachmentReference*  pResolveAttachments;
	public  VkAttachmentReference*  pDepthStencilAttachment;
	public  UInt32  preserveAttachmentCount;
	public  UInt32*  pPreserveAttachments;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassDependency {
	public  UInt32  srcSubpass;
	public  UInt32  dstSubpass;
	public  UInt32  srcStageMask;
	public  UInt32  dstStageMask;
	public  UInt32  srcAccessMask;
	public  UInt32  dstAccessMask;
	public  UInt32  dependencyFlags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  attachmentCount;
	public  VkAttachmentDescription*  pAttachments;
	public  UInt32  subpassCount;
	public  VkSubpassDescription*  pSubpasses;
	public  UInt32  dependencyCount;
	public  VkSubpassDependency*  pDependencies;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandPoolCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  queueFamilyIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkCommandPool  commandPool;
	public  VkCommandBufferLevel  level;
	public  UInt32  commandBufferCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferInheritanceInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRenderPass  renderPass;
	public  UInt32  subpass;
	public  VkFramebuffer  framebuffer;
	public  UInt32  occlusionQueryEnable;
	public  UInt32  queryFlags;
	public  UInt32  pipelineStatistics;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferBeginInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkCommandBufferInheritanceInfo*  pInheritanceInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferCopy {
	public  UInt64  srcOffset;
	public  UInt64  dstOffset;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageSubresourceLayers {
	public  UInt32  aspectMask;
	public  UInt32  mipLevel;
	public  UInt32  baseArrayLayer;
	public  UInt32  layerCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferImageCopy {
	public  UInt64  bufferOffset;
	public  UInt32  bufferRowLength;
	public  UInt32  bufferImageHeight;
	public  VkImageSubresourceLayers  imageSubresource;
	public  VkOffset3D  imageOffset;
	public  VkExtent3D  imageExtent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkClearDepthStencilValue {
	public  float  depth;
	public  uint  stencil;
	public VkClearDepthStencilValue() {depth =0.0f; stencil =0;}
	public VkClearDepthStencilValue(float pdepth , uint pstencil) {depth =pdepth; stencil = pstencil;}
	
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkClearAttachment {
	public  UInt32  aspectMask;
	public  UInt32  colorAttachment;
	public  VkClearValue  clearValue;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkClearRect {
	public  VkRect2D  rect;
	public  UInt32  baseArrayLayer;
	public  UInt32  layerCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageBlit {
	public  VkImageSubresourceLayers  srcSubresource;
	public VkOffset3D**  srcOffsets;//[2];
	public  VkImageSubresourceLayers  dstSubresource;
	public VkOffset3D**  dstOffsets;//[2];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageCopy {
	public  VkImageSubresourceLayers  srcSubresource;
	public  VkOffset3D  srcOffset;
	public  VkImageSubresourceLayers  dstSubresource;
	public  VkOffset3D  dstOffset;
	public  VkExtent3D  extent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageResolve {
	public  VkImageSubresourceLayers  srcSubresource;
	public  VkOffset3D  srcOffset;
	public  VkImageSubresourceLayers  dstSubresource;
	public  VkOffset3D  dstOffset;
	public  VkExtent3D  extent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassBeginInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRenderPass  renderPass;
	public  VkFramebuffer  framebuffer;
	public  VkRect2D  renderArea;
	public  UInt32  clearValueCount;
	public  VkClearValue*  pClearValues;
}

#endregion
#region VK_VERSION_1_1
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSubgroupProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  subgroupSize;
	public  UInt32  supportedStages;
	public  UInt32  supportedOperations;
	public  UInt32  quadOperationsInAllStages;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindBufferMemoryInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  buffer;
	public  VkDeviceMemory  memory;
	public  UInt64  memoryOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindImageMemoryInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  image;
	public  VkDeviceMemory  memory;
	public  UInt64  memoryOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevice16BitStorageFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  storageBuffer16BitAccess;
	public  UInt32  uniformAndStorageBuffer16BitAccess;
	public  UInt32  storagePushConstant16;
	public  UInt32  storageInputOutput16;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryDedicatedRequirements {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  prefersDedicatedAllocation;
	public  UInt32  requiresDedicatedAllocation;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryDedicatedAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  image;
	public  VkBuffer  buffer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct UInt32Info {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  deviceMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupRenderPassBeginInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceMask;
	public  UInt32  deviceRenderAreaCount;
	public  VkRect2D*  pDeviceRenderAreas;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupCommandBufferBeginInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupSubmitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  waitSemaphoreCount;
	public  UInt32*  pWaitSemaphoreDeviceIndices;
	public  UInt32  commandBufferCount;
	public  UInt32*  pCommandBufferDeviceMasks;
	public  UInt32  signalSemaphoreCount;
	public  UInt32*  pSignalSemaphoreDeviceIndices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupBindSparseInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  resourceDeviceIndex;
	public  UInt32  memoryDeviceIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindBufferMemoryDeviceGroupInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceIndexCount;
	public  UInt32*  pDeviceIndices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindImageMemoryDeviceGroupInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceIndexCount;
	public  UInt32*  pDeviceIndices;
	public  UInt32  splitInstanceBindRegionCount;
	public  VkRect2D*  pSplitInstanceBindRegions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceGroupProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  physicalDeviceCount;
	public VkPhysicalDevice**  physicalDevices;//[(int)VK_MAX_DEVICE_GROUP_SIZE];
	public  UInt32  subsetAllocation;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupDeviceCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  physicalDeviceCount;
	public  VkPhysicalDevice*  pPhysicalDevices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferMemoryRequirementsInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  buffer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageMemoryRequirementsInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  image;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageSparseMemoryRequirementsInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  image;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryRequirements2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkMemoryRequirements  memoryRequirements;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageMemoryRequirements2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSparseImageMemoryRequirements  memoryRequirements;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFeatures2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPhysicalDeviceFeatures  features;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceProperties2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPhysicalDeviceProperties  properties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFormatProperties2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormatProperties  formatProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageFormatProperties2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageFormatProperties  imageFormatProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageFormatInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormat  format;
	public  VkImageType  type;
	public  VkImageTiling  tiling;
	public  UInt32  usage;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueueFamilyProperties2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkQueueFamilyProperties  queueFamilyProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMemoryProperties2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPhysicalDeviceMemoryProperties  memoryProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSparseImageFormatProperties2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSparseImageFormatProperties  properties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSparseImageFormatInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormat  format;
	public  VkImageType  type;
	public  VkSampleCountFlagBits  samples;
	public  UInt32  usage;
	public  VkImageTiling  tiling;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePointClippingProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPointClippingBehavior  pointClippingBehavior;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkInputAttachmentAspectReference {
	public  UInt32  subpass;
	public  UInt32  inputAttachmentIndex;
	public  UInt32  aspectMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassInputAttachmentAspectCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  aspectReferenceCount;
	public  VkInputAttachmentAspectReference*  pAspectReferences;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewUsageCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  usage;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineTessellationDomainOriginStateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkTessellationDomainOrigin  domainOrigin;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassMultiviewCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  subpassCount;
	public  UInt32*  pViewMasks;
	public  UInt32  dependencyCount;
	public  Int32*  pViewOffsets;
	public  UInt32  correlationMaskCount;
	public  UInt32*  pCorrelationMasks;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMultiviewFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  multiview;
	public  UInt32  multiviewGeometryShader;
	public  UInt32  multiviewTessellationShader;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMultiviewProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxMultiviewViewCount;
	public  UInt32  maxMultiviewInstanceIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVariablePointersFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  variablePointersStorageBuffer;
	public  UInt32  variablePointers;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceProtectedMemoryFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  protectedMemory;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceProtectedMemoryProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  protectedNoFault;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceQueueInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  queueFamilyIndex;
	public  UInt32  queueIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkProtectedSubmitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  protectedSubmit;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerYcbcrConversionCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormat  format;
	public  VkSamplerYcbcrModelConversion  ycbcrModel;
	public  VkSamplerYcbcrRange  ycbcrRange;
	public  VkComponentMapping  components;
	public  VkChromaLocation  xChromaOffset;
	public  VkChromaLocation  yChromaOffset;
	public  VkFilter  chromaFilter;
	public  UInt32  forceExplicitReruction;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerYcbcrConversionInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSamplerYcbcrConversion  conversion;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindImagePlaneMemoryInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageAspectFlagBits  planeAspect;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImagePlaneMemoryRequirementsInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageAspectFlagBits  planeAspect;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSamplerYcbcrConversionFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  samplerYcbcrConversion;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerYcbcrConversionImageFormatProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  combinedImageSamplerDescriptorCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorUpdateTemplateEntry {
	public  UInt32  dstBinding;
	public  UInt32  dstArrayElement;
	public  UInt32  descriptorCount;
	public  VkDescriptorType  descriptorType;
	public  Int32  offset;
	public  Int32  stride;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorUpdateTemplateCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  descriptorUpdateEntryCount;
	public  VkDescriptorUpdateTemplateEntry*  pDescriptorUpdateEntries;
	public  VkDescriptorUpdateTemplateType  templateType;
	public  VkDescriptorSetLayout  descriptorSetLayout;
	public  VkPipelineBindPoint  pipelineBindPoint;
	public  VkPipelineLayout  pipelineLayout;
	public  UInt32  set;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalMemoryProperties {
	public  UInt32  externalMemoryFeatures;
	public  UInt32  exportFromImportedHandleTypes;
	public  UInt32  compatibleHandleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExternalImageFormatInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalMemoryHandleTypeFlagBits  handleType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalImageFormatProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalMemoryProperties  externalMemoryProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExternalBufferInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  usage;
	public  VkExternalMemoryHandleTypeFlagBits  handleType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalBufferProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalMemoryProperties  externalMemoryProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceIDProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  deviceUUID[(int)VK.VK_UUID_SIZE];
	public fixed ConstChar  driverUUID[(int)VK.VK_UUID_SIZE];
	public fixed ConstChar  deviceLUID[(int)VK.VK_LUID_SIZE];
	public  UInt32  deviceNodeMask;
	public  UInt32  deviceLUIDValid;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalMemoryImageCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalMemoryBufferCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExportMemoryAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExternalFenceInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalFenceHandleTypeFlagBits  handleType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalFenceProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  exportFromImportedHandleTypes;
	public  UInt32  compatibleHandleTypes;
	public  UInt32  externalFenceFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExportFenceCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExportSemaphoreCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExternalSemaphoreInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalSemaphoreHandleTypeFlagBits  handleType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalSemaphoreProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  exportFromImportedHandleTypes;
	public  UInt32  compatibleHandleTypes;
	public  UInt32  externalSemaphoreFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMaintenance3Properties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxPerSetDescriptors;
	public  UInt64  maxMemoryAllocationSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetLayoutSupport {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  supported;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderDrawParametersFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderDrawParameters;
}

#endregion
#region VK_VERSION_1_2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkan11Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  storageBuffer16BitAccess;
	public  UInt32  uniformAndStorageBuffer16BitAccess;
	public  UInt32  storagePushConstant16;
	public  UInt32  storageInputOutput16;
	public  UInt32  multiview;
	public  UInt32  multiviewGeometryShader;
	public  UInt32  multiviewTessellationShader;
	public  UInt32  variablePointersStorageBuffer;
	public  UInt32  variablePointers;
	public  UInt32  protectedMemory;
	public  UInt32  samplerYcbcrConversion;
	public  UInt32  shaderDrawParameters;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkan11Properties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  deviceUUID[(int)VK.VK_UUID_SIZE];
	public fixed ConstChar  driverUUID[(int)VK.VK_UUID_SIZE];
	public fixed ConstChar  deviceLUID[(int)VK.VK_LUID_SIZE];
	public  UInt32  deviceNodeMask;
	public  UInt32  deviceLUIDValid;
	public  UInt32  subgroupSize;
	public  UInt32  subgroupSupportedStages;
	public  UInt32  subgroupSupportedOperations;
	public  UInt32  subgroupQuadOperationsInAllStages;
	public  VkPointClippingBehavior  pointClippingBehavior;
	public  UInt32  maxMultiviewViewCount;
	public  UInt32  maxMultiviewInstanceIndex;
	public  UInt32  protectedNoFault;
	public  UInt32  maxPerSetDescriptors;
	public  UInt64  maxMemoryAllocationSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkan12Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  samplerMirrorClampToEdge;
	public  UInt32  drawIndirectCount;
	public  UInt32  storageBuffer8BitAccess;
	public  UInt32  uniformAndStorageBuffer8BitAccess;
	public  UInt32  storagePushConstant8;
	public  UInt32  shaderBufferInt64Atomics;
	public  UInt32  shaderSharedInt64Atomics;
	public  UInt32  shaderFloat16;
	public  UInt32  shaderInt8;
	public  UInt32  descriptorIndexing;
	public  UInt32  shaderInputAttachmentArrayDynamicIndexing;
	public  UInt32  shaderUniformTexelBufferArrayDynamicIndexing;
	public  UInt32  shaderStorageTexelBufferArrayDynamicIndexing;
	public  UInt32  shaderUniformBufferArrayNonUniformIndexing;
	public  UInt32  shaderSampledImageArrayNonUniformIndexing;
	public  UInt32  shaderStorageBufferArrayNonUniformIndexing;
	public  UInt32  shaderStorageImageArrayNonUniformIndexing;
	public  UInt32  shaderInputAttachmentArrayNonUniformIndexing;
	public  UInt32  shaderUniformTexelBufferArrayNonUniformIndexing;
	public  UInt32  shaderStorageTexelBufferArrayNonUniformIndexing;
	public  UInt32  descriptorBindingUniformBufferUpdateAfterBind;
	public  UInt32  descriptorBindingSampledImageUpdateAfterBind;
	public  UInt32  descriptorBindingStorageImageUpdateAfterBind;
	public  UInt32  descriptorBindingStorageBufferUpdateAfterBind;
	public  UInt32  descriptorBindingUniformTexelBufferUpdateAfterBind;
	public  UInt32  descriptorBindingStorageTexelBufferUpdateAfterBind;
	public  UInt32  descriptorBindingUpdateUnusedWhilePending;
	public  UInt32  descriptorBindingPartiallyBound;
	public  UInt32  descriptorBindingVariableDescriptorCount;
	public  UInt32  runtimeDescriptorArray;
	public  UInt32  samplerFilterMinmax;
	public  UInt32  scalarBlockLayout;
	public  UInt32  imagelessFramebuffer;
	public  UInt32  uniformBufferStandardLayout;
	public  UInt32  shaderSubgroupExtendedTypes;
	public  UInt32  separateDepthStencilLayouts;
	public  UInt32  hostQueryReset;
	public  UInt32  timelineSemaphore;
	public  UInt32  bufferDeviceAddress;
	public  UInt32  bufferDeviceAddressCaptureReplay;
	public  UInt32  bufferDeviceAddressMultiDevice;
	public  UInt32  vulkanMemoryModel;
	public  UInt32  vulkanMemoryModelDeviceScope;
	public  UInt32  vulkanMemoryModelAvailabilityVisibilityChains;
	public  UInt32  shaderOutputViewportIndex;
	public  UInt32  shaderOutputLayer;
	public  UInt32  subgroupBroadcastDynamicId;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkConformanceVersion {
	public  ConstChar  major;
	public  ConstChar  minor;
	public  ConstChar  subminor;
	public  ConstChar  patch;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkan12Properties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDriverId  driverID;
	public fixed ConstChar  driverName[(int)VK.VK_MAX_DRIVER_NAME_SIZE];
	public fixed ConstChar  driverInfo[(int)VK.VK_MAX_DRIVER_INFO_SIZE];
	public  VkConformanceVersion  conformanceVersion;
	public  VkShaderFloatControlsIndependence  denormBehaviorIndependence;
	public  VkShaderFloatControlsIndependence  roundingModeIndependence;
	public  UInt32  shaderSignedZeroInfNanPreserveFloat16;
	public  UInt32  shaderSignedZeroInfNanPreserveFloat32;
	public  UInt32  shaderSignedZeroInfNanPreserveFloat64;
	public  UInt32  shaderDenormPreserveFloat16;
	public  UInt32  shaderDenormPreserveFloat32;
	public  UInt32  shaderDenormPreserveFloat64;
	public  UInt32  shaderDenormFlushToZeroFloat16;
	public  UInt32  shaderDenormFlushToZeroFloat32;
	public  UInt32  shaderDenormFlushToZeroFloat64;
	public  UInt32  shaderRoundingModeRTEFloat16;
	public  UInt32  shaderRoundingModeRTEFloat32;
	public  UInt32  shaderRoundingModeRTEFloat64;
	public  UInt32  shaderRoundingModeRTZFloat16;
	public  UInt32  shaderRoundingModeRTZFloat32;
	public  UInt32  shaderRoundingModeRTZFloat64;
	public  UInt32  maxUpdateAfterBindDescriptorsInAllPools;
	public  UInt32  shaderUniformBufferArrayNonUniformIndexingNative;
	public  UInt32  shaderSampledImageArrayNonUniformIndexingNative;
	public  UInt32  shaderStorageBufferArrayNonUniformIndexingNative;
	public  UInt32  shaderStorageImageArrayNonUniformIndexingNative;
	public  UInt32  shaderInputAttachmentArrayNonUniformIndexingNative;
	public  UInt32  robustBufferAccessUpdateAfterBind;
	public  UInt32  quadDivergentImplicitLod;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindSamplers;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindUniformBuffers;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindStorageBuffers;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindSampledImages;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindStorageImages;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindInputAttachments;
	public  UInt32  maxPerStageUpdateAfterBindResources;
	public  UInt32  maxDescriptorSetUpdateAfterBindSamplers;
	public  UInt32  maxDescriptorSetUpdateAfterBindUniformBuffers;
	public  UInt32  maxDescriptorSetUpdateAfterBindUniformBuffersDynamic;
	public  UInt32  maxDescriptorSetUpdateAfterBindStorageBuffers;
	public  UInt32  maxDescriptorSetUpdateAfterBindStorageBuffersDynamic;
	public  UInt32  maxDescriptorSetUpdateAfterBindSampledImages;
	public  UInt32  maxDescriptorSetUpdateAfterBindStorageImages;
	public  UInt32  maxDescriptorSetUpdateAfterBindInputAttachments;
	public  UInt32  supportedDepthResolveModes;
	public  UInt32  supportedStencilResolveModes;
	public  UInt32  independentResolveNone;
	public  UInt32  independentResolve;
	public  UInt32  filterMinmaxSingleComponentFormats;
	public  UInt32  filterMinmaxImageComponentMapping;
	public  UInt64  maxTimelineSemaphoreValueDifference;
	public  UInt32  framebufferIntegerColorSampleCounts;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageFormatListCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  viewFormatCount;
	public  VkFormat*  pViewFormats;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentDescription2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkFormat  format;
	public  VkSampleCountFlagBits  samples;
	public  VkAttachmentLoadOp  loadOp;
	public  VkAttachmentStoreOp  storeOp;
	public  VkAttachmentLoadOp  stencilLoadOp;
	public  VkAttachmentStoreOp  stencilStoreOp;
	public  VkImageLayout  initialLayout;
	public  VkImageLayout  finalLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentReference2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  attachment;
	public  VkImageLayout  layout;
	public  UInt32  aspectMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassDescription2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkPipelineBindPoint  pipelineBindPoint;
	public  UInt32  viewMask;
	public  UInt32  inputAttachmentCount;
	public  VkAttachmentReference2*  pInputAttachments;
	public  UInt32  colorAttachmentCount;
	public  VkAttachmentReference2*  pColorAttachments;
	public  VkAttachmentReference2*  pResolveAttachments;
	public  VkAttachmentReference2*  pDepthStencilAttachment;
	public  UInt32  preserveAttachmentCount;
	public  UInt32*  pPreserveAttachments;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassDependency2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  srcSubpass;
	public  UInt32  dstSubpass;
	public  UInt32  srcStageMask;
	public  UInt32  dstStageMask;
	public  UInt32  srcAccessMask;
	public  UInt32  dstAccessMask;
	public  UInt32  dependencyFlags;
	public  Int32  viewOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassCreateInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  attachmentCount;
	public  VkAttachmentDescription2*  pAttachments;
	public  UInt32  subpassCount;
	public  VkSubpassDescription2*  pSubpasses;
	public  UInt32  dependencyCount;
	public  VkSubpassDependency2*  pDependencies;
	public  UInt32  correlatedViewMaskCount;
	public  UInt32*  pCorrelatedViewMasks;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassBeginInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSubpassContents  contents;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassEndInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevice8BitStorageFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  storageBuffer8BitAccess;
	public  UInt32  uniformAndStorageBuffer8BitAccess;
	public  UInt32  storagePushConstant8;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDriverProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDriverId  driverID;
	public fixed ConstChar  driverName[(int)VK.VK_MAX_DRIVER_NAME_SIZE];
	public fixed ConstChar  driverInfo[(int)VK.VK_MAX_DRIVER_INFO_SIZE];
	public  VkConformanceVersion  conformanceVersion;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderAtomicInt64Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderBufferInt64Atomics;
	public  UInt32  shaderSharedInt64Atomics;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderFloat16Int8Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderFloat16;
	public  UInt32  shaderInt8;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFloatControlsProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkShaderFloatControlsIndependence  denormBehaviorIndependence;
	public  VkShaderFloatControlsIndependence  roundingModeIndependence;
	public  UInt32  shaderSignedZeroInfNanPreserveFloat16;
	public  UInt32  shaderSignedZeroInfNanPreserveFloat32;
	public  UInt32  shaderSignedZeroInfNanPreserveFloat64;
	public  UInt32  shaderDenormPreserveFloat16;
	public  UInt32  shaderDenormPreserveFloat32;
	public  UInt32  shaderDenormPreserveFloat64;
	public  UInt32  shaderDenormFlushToZeroFloat16;
	public  UInt32  shaderDenormFlushToZeroFloat32;
	public  UInt32  shaderDenormFlushToZeroFloat64;
	public  UInt32  shaderRoundingModeRTEFloat16;
	public  UInt32  shaderRoundingModeRTEFloat32;
	public  UInt32  shaderRoundingModeRTEFloat64;
	public  UInt32  shaderRoundingModeRTZFloat16;
	public  UInt32  shaderRoundingModeRTZFloat32;
	public  UInt32  shaderRoundingModeRTZFloat64;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetLayoutBindingFlagsCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  bindingCount;
	public  UInt32*  pBindingFlags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDescriptorIndexingFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderInputAttachmentArrayDynamicIndexing;
	public  UInt32  shaderUniformTexelBufferArrayDynamicIndexing;
	public  UInt32  shaderStorageTexelBufferArrayDynamicIndexing;
	public  UInt32  shaderUniformBufferArrayNonUniformIndexing;
	public  UInt32  shaderSampledImageArrayNonUniformIndexing;
	public  UInt32  shaderStorageBufferArrayNonUniformIndexing;
	public  UInt32  shaderStorageImageArrayNonUniformIndexing;
	public  UInt32  shaderInputAttachmentArrayNonUniformIndexing;
	public  UInt32  shaderUniformTexelBufferArrayNonUniformIndexing;
	public  UInt32  shaderStorageTexelBufferArrayNonUniformIndexing;
	public  UInt32  descriptorBindingUniformBufferUpdateAfterBind;
	public  UInt32  descriptorBindingSampledImageUpdateAfterBind;
	public  UInt32  descriptorBindingStorageImageUpdateAfterBind;
	public  UInt32  descriptorBindingStorageBufferUpdateAfterBind;
	public  UInt32  descriptorBindingUniformTexelBufferUpdateAfterBind;
	public  UInt32  descriptorBindingStorageTexelBufferUpdateAfterBind;
	public  UInt32  descriptorBindingUpdateUnusedWhilePending;
	public  UInt32  descriptorBindingPartiallyBound;
	public  UInt32  descriptorBindingVariableDescriptorCount;
	public  UInt32  runtimeDescriptorArray;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDescriptorIndexingProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxUpdateAfterBindDescriptorsInAllPools;
	public  UInt32  shaderUniformBufferArrayNonUniformIndexingNative;
	public  UInt32  shaderSampledImageArrayNonUniformIndexingNative;
	public  UInt32  shaderStorageBufferArrayNonUniformIndexingNative;
	public  UInt32  shaderStorageImageArrayNonUniformIndexingNative;
	public  UInt32  shaderInputAttachmentArrayNonUniformIndexingNative;
	public  UInt32  robustBufferAccessUpdateAfterBind;
	public  UInt32  quadDivergentImplicitLod;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindSamplers;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindUniformBuffers;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindStorageBuffers;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindSampledImages;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindStorageImages;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindInputAttachments;
	public  UInt32  maxPerStageUpdateAfterBindResources;
	public  UInt32  maxDescriptorSetUpdateAfterBindSamplers;
	public  UInt32  maxDescriptorSetUpdateAfterBindUniformBuffers;
	public  UInt32  maxDescriptorSetUpdateAfterBindUniformBuffersDynamic;
	public  UInt32  maxDescriptorSetUpdateAfterBindStorageBuffers;
	public  UInt32  maxDescriptorSetUpdateAfterBindStorageBuffersDynamic;
	public  UInt32  maxDescriptorSetUpdateAfterBindSampledImages;
	public  UInt32  maxDescriptorSetUpdateAfterBindStorageImages;
	public  UInt32  maxDescriptorSetUpdateAfterBindInputAttachments;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetVariableDescriptorCountAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  descriptorSetCount;
	public  UInt32*  pDescriptorCounts;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetVariableDescriptorCountLayoutSupport {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxVariableDescriptorCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassDescriptionDepthStencilResolve {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkResolveModeFlagBits  depthResolveMode;
	public  VkResolveModeFlagBits  stencilResolveMode;
	public  VkAttachmentReference2*  pDepthStencilResolveAttachment;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDepthStencilResolveProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  supportedDepthResolveModes;
	public  UInt32  supportedStencilResolveModes;
	public  UInt32  independentResolveNone;
	public  UInt32  independentResolve;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceScalarBlockLayoutFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  scalarBlockLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageStencilUsageCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  stencilUsage;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerReductionModeCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSamplerReductionMode  reductionMode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSamplerFilterMinmaxProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  filterMinmaxSingleComponentFormats;
	public  UInt32  filterMinmaxImageComponentMapping;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkanMemoryModelFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  vulkanMemoryModel;
	public  UInt32  vulkanMemoryModelDeviceScope;
	public  UInt32  vulkanMemoryModelAvailabilityVisibilityChains;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImagelessFramebufferFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  imagelessFramebuffer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFramebufferAttachmentImageInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  usage;
	public  UInt32  width;
	public  UInt32  height;
	public  UInt32  layerCount;
	public  UInt32  viewFormatCount;
	public  VkFormat*  pViewFormats;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFramebufferAttachmentsCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  attachmentImageInfoCount;
	public  VkFramebufferAttachmentImageInfo*  pAttachmentImageInfos;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassAttachmentBeginInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  attachmentCount;
	public  VkImageView*  pAttachments;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceUniformBufferStandardLayoutFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  uniformBufferStandardLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderSubgroupExtendedTypesFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderSubgroupExtendedTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSeparateDepthStencilLayoutsFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  separateDepthStencilLayouts;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentReferenceStencilLayout {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageLayout  stencilLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentDescriptionStencilLayout {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageLayout  stencilInitialLayout;
	public  VkImageLayout  stencilFinalLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceHostQueryResetFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  hostQueryReset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTimelineSemaphoreFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  timelineSemaphore;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTimelineSemaphoreProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  maxTimelineSemaphoreValueDifference;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSemaphoreTypeCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSemaphoreType  semaphoreType;
	public  UInt64  initialValue;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkTimelineSemaphoreSubmitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  waitSemaphoreValueCount;
	public  UInt64*  pWaitSemaphoreValues;
	public  UInt32  signalSemaphoreValueCount;
	public  UInt64*  pSignalSemaphoreValues;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSemaphoreWaitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  semaphoreCount;
	public  VkSemaphore*  pSemaphores;
	public  UInt64*  pValues;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSemaphoreSignalInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSemaphore  semaphore;
	public  UInt64  value;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceBufferDeviceAddressFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  bufferDeviceAddress;
	public  UInt32  bufferDeviceAddressCaptureReplay;
	public  UInt32  bufferDeviceAddressMultiDevice;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferDeviceAddressInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  buffer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferOpaqueCaptureAddressCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  opaqueCaptureAddress;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryOpaqueCaptureAddressAllocateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  opaqueCaptureAddress;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceMemoryOpaqueCaptureAddressInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceMemory  memory;
}

#endregion
#region VK_VERSION_1_3
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkan13Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  robustImageAccess;
	public  UInt32  inlineUniformBlock;
	public  UInt32  descriptorBindingInlineUniformBlockUpdateAfterBind;
	public  UInt32  pipelineCreationCacheControl;
	public  UInt32  privateData;
	public  UInt32  shaderDemoteToHelperInvocation;
	public  UInt32  shaderTerminateInvocation;
	public  UInt32  subgroupSizeControl;
	public  UInt32  computeFullSubgroups;
	public  UInt32  synchronization2;
	public  UInt32  textureCompressionASTC_HDR;
	public  UInt32  shaderZeroInitializeWorkgroupMemory;
	public  UInt32  dynamicRendering;
	public  UInt32  shaderIntegerDotProduct;
	public  UInt32  maintenance4;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVulkan13Properties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  minSubgroupSize;
	public  UInt32  maxSubgroupSize;
	public  UInt32  maxComputeWorkgroupSubgroups;
	public  UInt32  requiredSubgroupSizeStages;
	public  UInt32  maxInlineUniformBlockSize;
	public  UInt32  maxPerStageDescriptorInlineUniformBlocks;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindInlineUniformBlocks;
	public  UInt32  maxDescriptorSetInlineUniformBlocks;
	public  UInt32  maxDescriptorSetUpdateAfterBindInlineUniformBlocks;
	public  UInt32  maxInlineUniformTotalSize;
	public  UInt32  integerDotProduct8BitUnsignedAccelerated;
	public  UInt32  integerDotProduct8BitSignedAccelerated;
	public  UInt32  integerDotProduct8BitMixedSignednessAccelerated;
	public  UInt32  integerDotProduct4x8BitPackedUnsignedAccelerated;
	public  UInt32  integerDotProduct4x8BitPackedSignedAccelerated;
	public  UInt32  integerDotProduct4x8BitPackedMixedSignednessAccelerated;
	public  UInt32  integerDotProduct16BitUnsignedAccelerated;
	public  UInt32  integerDotProduct16BitSignedAccelerated;
	public  UInt32  integerDotProduct16BitMixedSignednessAccelerated;
	public  UInt32  integerDotProduct32BitUnsignedAccelerated;
	public  UInt32  integerDotProduct32BitSignedAccelerated;
	public  UInt32  integerDotProduct32BitMixedSignednessAccelerated;
	public  UInt32  integerDotProduct64BitUnsignedAccelerated;
	public  UInt32  integerDotProduct64BitSignedAccelerated;
	public  UInt32  integerDotProduct64BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating8BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating8BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating8BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating4x8BitPackedUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating4x8BitPackedSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating4x8BitPackedMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating16BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating16BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating16BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating32BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating32BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating32BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating64BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating64BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating64BitMixedSignednessAccelerated;
	public  UInt64  storageTexelBufferOffsetAlignmentConstChars;
	public  UInt32  storageTexelBufferOffsetSingleTexelAlignment;
	public  UInt64  uniformTexelBufferOffsetAlignmentConstChars;
	public  UInt32  uniformTexelBufferOffsetSingleTexelAlignment;
	public  UInt64  maxBufferSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCreationFeedback {
	public  UInt32  flags;
	public  UInt64  duration;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCreationFeedbackCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipelineCreationFeedback*  pPipelineCreationFeedback;
	public  UInt32  pipelineStageCreationFeedbackCount;
	public  VkPipelineCreationFeedback*  pPipelineStageCreationFeedbacks;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderTerminateInvocationFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderTerminateInvocation;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceToolProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  name[(int)VK.VK_MAX_EXTENSION_NAME_SIZE];
	public fixed ConstChar  version[(int)VK.VK_MAX_EXTENSION_NAME_SIZE];
	public  UInt32  purposes;
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public fixed ConstChar  layer[(int)VK.VK_MAX_EXTENSION_NAME_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderDemoteToHelperInvocationFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderDemoteToHelperInvocation;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePrivateDataFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  privateData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDevicePrivateDataCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  privateDataSlotRequestCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPrivateDataSlotCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePipelineCreationCacheControlFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pipelineCreationCacheControl;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryBarrier2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  srcStageMask;
	public  UInt64  srcAccessMask;
	public  UInt64  dstStageMask;
	public  UInt64  dstAccessMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferMemoryBarrier2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  srcStageMask;
	public  UInt64  srcAccessMask;
	public  UInt64  dstStageMask;
	public  UInt64  dstAccessMask;
	public  UInt32  srcQueueFamilyIndex;
	public  UInt32  dstQueueFamilyIndex;
	public  VkBuffer  buffer;
	public  UInt64  offset;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageMemoryBarrier2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  srcStageMask;
	public  UInt64  srcAccessMask;
	public  UInt64  dstStageMask;
	public  UInt64  dstAccessMask;
	public  VkImageLayout  oldLayout;
	public  VkImageLayout  newLayout;
	public  UInt32  srcQueueFamilyIndex;
	public  UInt32  dstQueueFamilyIndex;
	public  VkImage  image;
	public  VkImageSubresourceRange  subresourceRange;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDependencyInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dependencyFlags;
	public  UInt32  memoryBarrierCount;
	public  VkMemoryBarrier2*  pMemoryBarriers;
	public  UInt32  bufferMemoryBarrierCount;
	public  VkBufferMemoryBarrier2*  pBufferMemoryBarriers;
	public  UInt32  imageMemoryBarrierCount;
	public  VkImageMemoryBarrier2*  pImageMemoryBarriers;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSemaphoreSubmitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSemaphore  semaphore;
	public  UInt64  value;
	public  UInt64  stageMask;
	public  UInt32  deviceIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferSubmitInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkCommandBuffer  commandBuffer;
	public  UInt32  deviceMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubmitInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  waitSemaphoreInfoCount;
	public  VkSemaphoreSubmitInfo*  pWaitSemaphoreInfos;
	public  UInt32  commandBufferInfoCount;
	public  VkCommandBufferSubmitInfo*  pCommandBufferInfos;
	public  UInt32  signalSemaphoreInfoCount;
	public  VkSemaphoreSubmitInfo*  pSignalSemaphoreInfos;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSynchronization2Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  synchronization2;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceZeroInitializeWorkgroupMemoryFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderZeroInitializeWorkgroupMemory;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageRobustnessFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  robustImageAccess;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferCopy2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  srcOffset;
	public  UInt64  dstOffset;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyBufferInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  srcBuffer;
	public  VkBuffer  dstBuffer;
	public  UInt32  regionCount;
	public  VkBufferCopy2*  pRegions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageCopy2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageSubresourceLayers  srcSubresource;
	public  VkOffset3D  srcOffset;
	public  VkImageSubresourceLayers  dstSubresource;
	public  VkOffset3D  dstOffset;
	public  VkExtent3D  extent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyImageInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  srcImage;
	public  VkImageLayout  srcImageLayout;
	public  VkImage  dstImage;
	public  VkImageLayout  dstImageLayout;
	public  UInt32  regionCount;
	public  VkImageCopy2*  pRegions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferImageCopy2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  bufferOffset;
	public  UInt32  bufferRowLength;
	public  UInt32  bufferImageHeight;
	public  VkImageSubresourceLayers  imageSubresource;
	public  VkOffset3D  imageOffset;
	public  VkExtent3D  imageExtent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyBufferToImageInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  srcBuffer;
	public  VkImage  dstImage;
	public  VkImageLayout  dstImageLayout;
	public  UInt32  regionCount;
	public  VkBufferImageCopy2*  pRegions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyImageToBufferInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  srcImage;
	public  VkImageLayout  srcImageLayout;
	public  VkBuffer  dstBuffer;
	public  UInt32  regionCount;
	public  VkBufferImageCopy2*  pRegions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageBlit2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageSubresourceLayers  srcSubresource;
	public VkOffset3D**  srcOffsets;//[2];
	public  VkImageSubresourceLayers  dstSubresource;
	public VkOffset3D  dstOffsets;//[2];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBlitImageInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  srcImage;
	public  VkImageLayout  srcImageLayout;
	public  VkImage  dstImage;
	public  VkImageLayout  dstImageLayout;
	public  UInt32  regionCount;
	public  VkImageBlit2*  pRegions;
	public  VkFilter  filter;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageResolve2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageSubresourceLayers  srcSubresource;
	public  VkOffset3D  srcOffset;
	public  VkImageSubresourceLayers  dstSubresource;
	public  VkOffset3D  dstOffset;
	public  VkExtent3D  extent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkResolveImageInfo2 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  srcImage;
	public  VkImageLayout  srcImageLayout;
	public  VkImage  dstImage;
	public  VkImageLayout  dstImageLayout;
	public  UInt32  regionCount;
	public  VkImageResolve2*  pRegions;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSubgroupSizeControlFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  subgroupSizeControl;
	public  UInt32  computeFullSubgroups;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSubgroupSizeControlProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  minSubgroupSize;
	public  UInt32  maxSubgroupSize;
	public  UInt32  maxComputeWorkgroupSubgroups;
	public  UInt32  requiredSubgroupSizeStages;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineShaderStageRequiredSubgroupSizeCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  requiredSubgroupSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceInlineUniformBlockFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  inlineUniformBlock;
	public  UInt32  descriptorBindingInlineUniformBlockUpdateAfterBind;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceInlineUniformBlockProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxInlineUniformBlockSize;
	public  UInt32  maxPerStageDescriptorInlineUniformBlocks;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindInlineUniformBlocks;
	public  UInt32  maxDescriptorSetInlineUniformBlocks;
	public  UInt32  maxDescriptorSetUpdateAfterBindInlineUniformBlocks;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkWriteDescriptorSetInlineUniformBlock {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dataSize;
	public  void*  pData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorPoolInlineUniformBlockCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxInlineUniformBlockBindings;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTextureCompressionASTCHDRFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  textureCompressionASTC_HDR;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderingAttachmentInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageView  imageView;
	public  VkImageLayout  imageLayout;
	public  VkResolveModeFlagBits  resolveMode;
	public  VkImageView  resolveImageView;
	public  VkImageLayout  resolveImageLayout;
	public  VkAttachmentLoadOp  loadOp;
	public  VkAttachmentStoreOp  storeOp;
	public  VkClearValue  clearValue;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderingInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkRect2D  renderArea;
	public  UInt32  layerCount;
	public  UInt32  viewMask;
	public  UInt32  colorAttachmentCount;
	public  VkRenderingAttachmentInfo*  pColorAttachments;
	public  VkRenderingAttachmentInfo*  pDepthAttachment;
	public  VkRenderingAttachmentInfo*  pStencilAttachment;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRenderingCreateInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  viewMask;
	public  UInt32  colorAttachmentCount;
	public  VkFormat*  pColorAttachmentFormats;
	public  VkFormat  depthAttachmentFormat;
	public  VkFormat  stencilAttachmentFormat;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDynamicRenderingFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dynamicRendering;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferInheritanceRenderingInfo {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  viewMask;
	public  UInt32  colorAttachmentCount;
	public  VkFormat*  pColorAttachmentFormats;
	public  VkFormat  depthAttachmentFormat;
	public  VkFormat  stencilAttachmentFormat;
	public  VkSampleCountFlagBits  rasterizationSamples;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderIntegerDotProductFeatures {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderIntegerDotProduct;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderIntegerDotProductProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  integerDotProduct8BitUnsignedAccelerated;
	public  UInt32  integerDotProduct8BitSignedAccelerated;
	public  UInt32  integerDotProduct8BitMixedSignednessAccelerated;
	public  UInt32  integerDotProduct4x8BitPackedUnsignedAccelerated;
	public  UInt32  integerDotProduct4x8BitPackedSignedAccelerated;
	public  UInt32  integerDotProduct4x8BitPackedMixedSignednessAccelerated;
	public  UInt32  integerDotProduct16BitUnsignedAccelerated;
	public  UInt32  integerDotProduct16BitSignedAccelerated;
	public  UInt32  integerDotProduct16BitMixedSignednessAccelerated;
	public  UInt32  integerDotProduct32BitUnsignedAccelerated;
	public  UInt32  integerDotProduct32BitSignedAccelerated;
	public  UInt32  integerDotProduct32BitMixedSignednessAccelerated;
	public  UInt32  integerDotProduct64BitUnsignedAccelerated;
	public  UInt32  integerDotProduct64BitSignedAccelerated;
	public  UInt32  integerDotProduct64BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating8BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating8BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating8BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating4x8BitPackedUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating4x8BitPackedSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating4x8BitPackedMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating16BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating16BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating16BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating32BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating32BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating32BitMixedSignednessAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating64BitUnsignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating64BitSignedAccelerated;
	public  UInt32  integerDotProductAccumulatingSaturating64BitMixedSignednessAccelerated;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTexelBufferAlignmentProperties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  storageTexelBufferOffsetAlignmentConstChars;
	public  UInt32  storageTexelBufferOffsetSingleTexelAlignment;
	public  UInt64  uniformTexelBufferOffsetAlignmentConstChars;
	public  UInt32  uniformTexelBufferOffsetSingleTexelAlignment;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFormatProperties3 {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  linearTilingFeatures;
	public  UInt64  optimalTilingFeatures;
	public  UInt64  bufferFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMaintenance4Features {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maintenance4;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMaintenance4Properties {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  maxBufferSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceBufferMemoryRequirements {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBufferCreateInfo*  pCreateInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceImageMemoryRequirements {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageCreateInfo*  pCreateInfo;
	public  VkImageAspectFlagBits  planeAspect;
}

#endregion
#region VK_KHR_surface
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceCapabilitiesKHR {
	public  UInt32  minImageCount;
	public  UInt32  maxImageCount;
	public  VkExtent2D  currentExtent;
	public  VkExtent2D  minImageExtent;
	public  VkExtent2D  maxImageExtent;
	public  UInt32  maxImageArrayLayers;
	public  UInt32  supportedTransforms;
	public  VkSurfaceTransformFlagBitsKHR  currentTransform;
	public  UInt32  supportedCompositeAlpha;
	public  UInt32  supportedUsageFlags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceFormatKHR {
	public  VkFormat  format;
	public  VkColorSpaceKHR  colorSpace;
}

#endregion
#region VK_KHR_swapchain
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSwapchainCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkSurfaceKHR  surface;
	public  UInt32  minImageCount;
	public  VkFormat  imageFormat;
	public  VkColorSpaceKHR  imageColorSpace;
	public  VkExtent2D  imageExtent;
	public  UInt32  imageArrayLayers;
	public  UInt32  imageUsage;
	public  VkSharingMode  imageSharingMode;
	public  UInt32  queueFamilyIndexCount;
	public  UInt32*  pQueueFamilyIndices;
	public  VkSurfaceTransformFlagBitsKHR  preTransform;
	public  VkCompositeAlphaFlagBitsKHR  compositeAlpha;
	public  VkPresentModeKHR  presentMode;
	public  UInt32  clipped;
	public  VkSwapchainKHR  oldSwapchain;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPresentInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  waitSemaphoreCount;
	public  VkSemaphore*  pWaitSemaphores;
	public  UInt32  swapchainCount;
	public  VkSwapchainKHR*  pSwapchains;
	public  UInt32*  pImageIndices;
	public  VkResult*  pResults;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageSwapchainCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSwapchainKHR  swapchain;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindImageMemorySwapchainInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSwapchainKHR  swapchain;
	public  UInt32  imageIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAcquireNextImageInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSwapchainKHR  swapchain;
	public  UInt64  timeout;
	public  VkSemaphore  semaphore;
	public  VkFence  fence;
	public  UInt32  deviceMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupPresentCapabilitiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed UInt32  presentMask[(int)VK.VK_MAX_DEVICE_GROUP_SIZE];
	public  UInt32  modes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupPresentInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  swapchainCount;
	public  UInt32*  pDeviceMasks;
	public  VkDeviceGroupPresentModeFlagBitsKHR  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceGroupSwapchainCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  modes;
}

#endregion
#region VK_KHR_display
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayModeParametersKHR {
	public  VkExtent2D  visibleRegion;
	public  UInt32  refreshRate;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayModeCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkDisplayModeParametersKHR  parameters;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayModePropertiesKHR {
	public  VkDisplayModeKHR  displayMode;
	public  VkDisplayModeParametersKHR  parameters;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPlaneCapabilitiesKHR {
	public  UInt32  supportedAlpha;
	public  VkOffset2D  minSrcPosition;
	public  VkOffset2D  maxSrcPosition;
	public  VkExtent2D  minSrcExtent;
	public  VkExtent2D  maxSrcExtent;
	public  VkOffset2D  minDstPosition;
	public  VkOffset2D  maxDstPosition;
	public  VkExtent2D  minDstExtent;
	public  VkExtent2D  maxDstExtent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPlanePropertiesKHR {
	public  VkDisplayKHR  currentDisplay;
	public  UInt32  currentStackIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPropertiesKHR {
	public  VkDisplayKHR  display;
	public  ConstChar*  displayName;
	public  VkExtent2D  physicalDimensions;
	public  VkExtent2D  physicalResolution;
	public  UInt32  supportedTransforms;
	public  UInt32  planeReorderPossible;
	public  UInt32  persistentContent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplaySurfaceCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkDisplayModeKHR  displayMode;
	public  UInt32  planeIndex;
	public  UInt32  planeStackIndex;
	public  VkSurfaceTransformFlagBitsKHR  transform;
	public  float  globalAlpha;
	public  VkDisplayPlaneAlphaFlagBitsKHR  alphaMode;
	public  VkExtent2D  imageExtent;
}

#endregion
#region VK_KHR_display_swapchain
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPresentInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRect2D  srcRect;
	public  VkRect2D  dstRect;
	public  UInt32  persistent;
}

#endregion
#region VK_KHR_dynamic_rendering
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderingFragmentShadingRateAttachmentInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageView  imageView;
	public  VkImageLayout  imageLayout;
	public  VkExtent2D  shadingRateAttachmentTexelSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderingFragmentDensityMapAttachmentInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageView  imageView;
	public  VkImageLayout  imageLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentSampleCountInfoAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  colorAttachmentCount;
	public  VkSampleCountFlagBits*  pColorAttachmentSamples;
	public  VkSampleCountFlagBits  depthStencilAttachmentSamples;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMultiviewPerViewAttributesInfoNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  perViewAttributes;
	public  UInt32  perViewAttributesPositionXOnly;
}

#endregion
#region VK_KHR_external_memory_fd
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImportMemoryFdInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalMemoryHandleTypeFlagBits  handleType;
	public  int  fd;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryFdPropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  memoryTypeBits;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryGetFdInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceMemory  memory;
	public  VkExternalMemoryHandleTypeFlagBits  handleType;
}

#endregion
#region VK_KHR_external_semaphore_fd
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImportSemaphoreFdInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSemaphore  semaphore;
	public  UInt32  flags;
	public  VkExternalSemaphoreHandleTypeFlagBits  handleType;
	public  int  fd;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSemaphoreGetFdInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSemaphore  semaphore;
	public  VkExternalSemaphoreHandleTypeFlagBits  handleType;
}

#endregion
#region VK_KHR_push_descriptor
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePushDescriptorPropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxPushDescriptors;
}

#endregion
#region VK_KHR_incremental_present
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRectLayerKHR {
	public  VkOffset2D  offset;
	public  VkExtent2D  extent;
	public  UInt32  layer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPresentRegionKHR {
	public  UInt32  rectangleCount;
	public  VkRectLayerKHR*  pRectangles;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPresentRegionsKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  swapchainCount;
	public  VkPresentRegionKHR*  pRegions;
}

#endregion
#region VK_KHR_shared_presentable_image
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSharedPresentSurfaceCapabilitiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  sharedPresentSupportedUsageFlags;
}

#endregion
#region VK_KHR_external_fence_fd
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImportFenceFdInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFence  fence;
	public  UInt32  flags;
	public  VkExternalFenceHandleTypeFlagBits  handleType;
	public  int  fd;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFenceGetFdInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFence  fence;
	public  VkExternalFenceHandleTypeFlagBits  handleType;
}

#endregion
#region VK_KHR_performance_query
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePerformanceQueryFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  performanceCounterQueryPools;
	public  UInt32  performanceCounterMultipleQueryPools;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePerformanceQueryPropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  allowCommandBufferQueryCopies;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceCounterKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPerformanceCounterUnitKHR  unit;
	public  VkPerformanceCounterScopeKHR  scope;
	public  VkPerformanceCounterStorageKHR  storage;
	public fixed ConstChar  uuid[(int)VK.VK_UUID_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceCounterDescriptionKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public fixed ConstChar  name[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public fixed ConstChar  category[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueryPoolPerformanceCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  queueFamilyIndex;
	public  UInt32  counterIndexCount;
	public  UInt32*  pCounterIndices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAcquireProfilingLockInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt64  timeout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceQuerySubmitInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  counterPassIndex;
}

#endregion
#region VK_KHR_get_surface_capabilities2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSurfaceInfo2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSurfaceKHR  surface;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceCapabilities2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSurfaceCapabilitiesKHR  surfaceCapabilities;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceFormat2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSurfaceFormatKHR  surfaceFormat;
}

#endregion
#region VK_KHR_get_display_properties2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayProperties2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayPropertiesKHR  displayProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPlaneProperties2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayPlanePropertiesKHR  displayPlaneProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayModeProperties2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayModePropertiesKHR  displayModeProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPlaneInfo2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayModeKHR  mode;
	public  UInt32  planeIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPlaneCapabilities2KHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayPlaneCapabilitiesKHR  capabilities;
}

#endregion
#region VK_KHR_shader_clock
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderClockFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderSubgroupClock;
	public  UInt32  shaderDeviceClock;
}

#endregion
#region VK_KHR_global_priority
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceQueueGlobalPriorityCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkQueueGlobalPriorityKHR  globalPriority;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceGlobalPriorityQueryFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  globalPriorityQuery;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueueFamilyGlobalPriorityPropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  priorityCount;
	public VkQueueGlobalPriorityKHR**  priorities;//[(int)VK_MAX_GLOBAL_PRIORITY_SIZE_KHR];
}

#endregion
#region VK_KHR_fragment_shading_rate
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFragmentShadingRateAttachmentInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAttachmentReference2*  pFragmentShadingRateAttachment;
	public  VkExtent2D  shadingRateAttachmentTexelSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineFragmentShadingRateStateCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent2D  fragmentSize;
	public VkFragmentShadingRateCombinerOpKHR**  combinerOps;//[2];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShadingRateFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pipelineFragmentShadingRate;
	public  UInt32  primitiveFragmentShadingRate;
	public  UInt32  attachmentFragmentShadingRate;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShadingRatePropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent2D  minFragmentShadingRateAttachmentTexelSize;
	public  VkExtent2D  maxFragmentShadingRateAttachmentTexelSize;
	public  UInt32  maxFragmentShadingRateAttachmentTexelSizeAspectRatio;
	public  UInt32  primitiveFragmentShadingRateWithMultipleViewports;
	public  UInt32  layeredShadingRateAttachments;
	public  UInt32  fragmentShadingRateNonTrivialCombinerOps;
	public  VkExtent2D  maxFragmentSize;
	public  UInt32  maxFragmentSizeAspectRatio;
	public  UInt32  maxFragmentShadingRateCoverageSamples;
	public  VkSampleCountFlagBits  maxFragmentShadingRateRasterizationSamples;
	public  UInt32  fragmentShadingRateWithShaderDepthStencilWrites;
	public  UInt32  fragmentShadingRateWithSampleMask;
	public  UInt32  fragmentShadingRateWithShaderSampleMask;
	public  UInt32  fragmentShadingRateWithConservativeRasterization;
	public  UInt32  fragmentShadingRateWithFragmentShaderInterlock;
	public  UInt32  fragmentShadingRateWithCustomSampleLocations;
	public  UInt32  fragmentShadingRateStrictMultiplyCombiner;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShadingRateKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  sampleCounts;
	public  VkExtent2D  fragmentSize;
}

#endregion
#region VK_KHR_surface_protected_capabilities
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceProtectedCapabilitiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  supportsProtected;
}

#endregion
#region VK_KHR_present_wait
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePresentWaitFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  presentWait;
}

#endregion
#region VK_KHR_pipeline_executable_properties
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePipelineExecutablePropertiesFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pipelineExecutableInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipeline  pipeline;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineExecutablePropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  stages;
	public fixed ConstChar  name[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public  UInt32  subgroupSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineExecutableInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipeline  pipeline;
	public  UInt32  executableIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineExecutableStatisticKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  name[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public  VkPipelineExecutableStatisticFormatKHR  format;
	public  VkPipelineExecutableStatisticValueKHR  value;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineExecutableInternalRepresentationKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  name[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public  UInt32  isText;
	public  Int32  dataSize;
	public  void*  pData;
}

#endregion
#region VK_KHR_pipeline_library
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineLibraryCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  libraryCount;
	public  VkPipeline*  pLibraries;
}

#endregion
#region VK_KHR_present_id
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPresentIdKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  swapchainCount;
	public  UInt64*  pPresentIds;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePresentIdFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  presentId;
}

#endregion
#region VK_KHR_synchronization2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueueFamilyCheckpointProperties2NV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  checkpointExecutionStageMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCheckpointData2NV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  stage;
	public  void*  pCheckpointMarker;
}

#endregion
#region VK_KHR_fragment_shader_barycentric
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShaderBarycentricFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentShaderBarycentric;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShaderBarycentricPropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  triStripVertexOrderIndependentOfProvokingVertex;
}

#endregion
#region VK_KHR_shader_subgroup_uniform_control_flow
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderSubgroupUniformControlFlowFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderSubgroupUniformControlFlow;
}

#endregion
#region VK_KHR_workgroup_memory_explicit_layout
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceWorkgroupMemoryExplicitLayoutFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  workgroupMemoryExplicitLayout;
	public  UInt32  workgroupMemoryExplicitLayoutScalarBlockLayout;
	public  UInt32  workgroupMemoryExplicitLayout8BitAccess;
	public  UInt32  workgroupMemoryExplicitLayout16BitAccess;
}

#endregion
#region VK_KHR_ray_tracing_maintenance1
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingMaintenance1FeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rayTracingMaintenance1;
	public  UInt32  rayTracingPipelineTraceRaysIndirect2;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkTraceRaysIndirectCommand2KHR {
	public  UInt64  raygenShaderRecordAddress;
	public  UInt64  raygenShaderRecordSize;
	public  UInt64  missShaderBindingTableAddress;
	public  UInt64  missShaderBindingTableSize;
	public  UInt64  missShaderBindingTableStride;
	public  UInt64  hitShaderBindingTableAddress;
	public  UInt64  hitShaderBindingTableSize;
	public  UInt64  hitShaderBindingTableStride;
	public  UInt64  callableShaderBindingTableAddress;
	public  UInt64  callableShaderBindingTableSize;
	public  UInt64  callableShaderBindingTableStride;
	public  UInt32  width;
	public  UInt32  height;
	public  UInt32  depth;
}

#endregion
#region VK_EXT_debug_report
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugReportCallbackCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  delegate* unmanaged< UInt32,VkDebugReportObjectTypeEXT,UInt64,Int32,Int32,char*,char*,void*,UInt32 >  pfnCallback;
	public  void*  pUserData;
}

#endregion
#region VK_AMD_rasterization_order
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationStateRasterizationOrderAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRasterizationOrderAMD  rasterizationOrder;
}

#endregion
#region VK_EXT_debug_marker
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugMarkerObjectNameInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDebugReportObjectTypeEXT  @objectType;
	public  UInt64  @object;
	public  ConstChar*  pObjectName;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugMarkerObjectTagInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDebugReportObjectTypeEXT  @objectType;
	public  UInt64  @object;
	public  UInt64  tagName;
	public  Int32  tagSize;
	public  void*  pTag;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugMarkerMarkerInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  ConstChar*  pMarkerName;
	public fixed float  color[4];
}

#endregion
#region VK_NV_dedicated_allocation
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDedicatedAllocationImageCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dedicatedAllocation;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDedicatedAllocationBufferCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dedicatedAllocation;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDedicatedAllocationMemoryAllocateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImage  image;
	public  VkBuffer  buffer;
}

#endregion
#region VK_EXT_transform_feedback
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTransformFeedbackFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  transformFeedback;
	public  UInt32  geometryStreams;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTransformFeedbackPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxTransformFeedbackStreams;
	public  UInt32  maxTransformFeedbackBuffers;
	public  UInt64  maxTransformFeedbackBufferSize;
	public  UInt32  maxTransformFeedbackStreamDataSize;
	public  UInt32  maxTransformFeedbackBufferDataSize;
	public  UInt32  maxTransformFeedbackBufferDataStride;
	public  UInt32  transformFeedbackQueries;
	public  UInt32  transformFeedbackStreamsLinesTriangles;
	public  UInt32  transformFeedbackRasterizationStreamSelect;
	public  UInt32  transformFeedbackDraw;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationStateStreamCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  rasterizationStream;
}

#endregion
#region VK_NVX_binary_import
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCuModuleCreateInfoNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  Int32  dataSize;
	public  void*  pData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCuFunctionCreateInfoNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkCuModuleNVX  module;
	public  ConstChar*  pName;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCuLaunchInfoNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkCuFunctionNVX  function;
	public  UInt32  gridDimX;
	public  UInt32  gridDimY;
	public  UInt32  gridDimZ;
	public  UInt32  blockDimX;
	public  UInt32  blockDimY;
	public  UInt32  blockDimZ;
	public  UInt32  sharedMemConstChars;
	public  Int32  paramCount;
	public  void**  pParams;
	public  Int32  extraCount;
	public  void**  pExtras;
}

#endregion
#region VK_NVX_image_view_handle
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewHandleInfoNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageView  imageView;
	public  VkDescriptorType  descriptorType;
	public  VkSampler  sampler;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewAddressPropertiesNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  deviceAddress;
	public  UInt64  size;
}

#endregion
#region VK_AMD_texture_gather_bias_lod
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkTextureLODGatherFormatPropertiesAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  supportsTextureGatherLODBiasAMD;
}

#endregion
#region VK_AMD_shader_info
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkShaderResourceUsageAMD {
	public  UInt32  numUsedVgprs;
	public  UInt32  numUsedSgprs;
	public  UInt32  ldsSizePerLocalWorkGroup;
	public  Int32  ldsUsageSizeInConstChars;
	public  Int32  scratchMemUsageInConstChars;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkShaderStatisticsInfoAMD {
	public  UInt32  shaderStageMask;
	public  VkShaderResourceUsageAMD  resourceUsage;
	public  UInt32  numPhysicalVgprs;
	public  UInt32  numPhysicalSgprs;
	public  UInt32  numAvailableVgprs;
	public  UInt32  numAvailableSgprs;
	public fixed UInt32  computeWorkGroupSize[3];
}

#endregion
#region VK_NV_corner_sampled_image
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCornerSampledImageFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  cornerSampledImage;
}

#endregion
#region VK_NV_external_memory_capabilities
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalImageFormatPropertiesNV {
	public  VkImageFormatProperties  imageFormatProperties;
	public  UInt32  externalMemoryFeatures;
	public  UInt32  exportFromImportedHandleTypes;
	public  UInt32  compatibleHandleTypes;
}

#endregion
#region VK_NV_external_memory
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExternalMemoryImageCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkExportMemoryAllocateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  handleTypes;
}

#endregion
#region VK_EXT_validation_flags
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkValidationFlagsEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  disabledValidationCheckCount;
	public  VkValidationCheckEXT*  pDisabledValidationChecks;
}

#endregion
#region VK_EXT_astc_decode_mode
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewASTCDecodeModeEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormat  decodeMode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceASTCDecodeFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  decodeModeSharedExponent;
}

#endregion
#region VK_EXT_pipeline_robustness
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePipelineRobustnessFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pipelineRobustness;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePipelineRobustnessPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipelineRobustnessBufferBehaviorEXT  defaultRobustnessStorageBuffers;
	public  VkPipelineRobustnessBufferBehaviorEXT  defaultRobustnessUniformBuffers;
	public  VkPipelineRobustnessBufferBehaviorEXT  defaultRobustnessVertexInputs;
	public  VkPipelineRobustnessImageBehaviorEXT  defaultRobustnessImages;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRobustnessCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipelineRobustnessBufferBehaviorEXT  storageBuffers;
	public  VkPipelineRobustnessBufferBehaviorEXT  uniformBuffers;
	public  VkPipelineRobustnessBufferBehaviorEXT  vertexInputs;
	public  VkPipelineRobustnessImageBehaviorEXT  images;
}

#endregion
#region VK_EXT_conditional_rendering
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkConditionalRenderingBeginInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  buffer;
	public  UInt64  offset;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceConditionalRenderingFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  conditionalRendering;
	public  UInt32  inheritedConditionalRendering;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferInheritanceConditionalRenderingInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  conditionalRenderingEnable;
}

#endregion
#region VK_NV_clip_space_w_scaling
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkViewportWScalingNV {
	public  float  xcoeff;
	public  float  ycoeff;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportWScalingStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  viewportWScalingEnable;
	public  UInt32  viewportCount;
	public  VkViewportWScalingNV*  pViewportWScalings;
}

#endregion
#region VK_EXT_display_surface_counter
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceCapabilities2EXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  minImageCount;
	public  UInt32  maxImageCount;
	public  VkExtent2D  currentExtent;
	public  VkExtent2D  minImageExtent;
	public  VkExtent2D  maxImageExtent;
	public  UInt32  maxImageArrayLayers;
	public  UInt32  supportedTransforms;
	public  VkSurfaceTransformFlagBitsKHR  currentTransform;
	public  UInt32  supportedCompositeAlpha;
	public  UInt32  supportedUsageFlags;
	public  UInt32  supportedSurfaceCounters;
}

#endregion
#region VK_EXT_display_control
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayPowerInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayPowerStateEXT  powerState;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceEventInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceEventTypeEXT  deviceEvent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayEventInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDisplayEventTypeEXT  displayEvent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSwapchainCounterCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  surfaceCounters;
}

#endregion
#region VK_GOOGLE_display_timing
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRefreshCycleDurationGOOGLE {
	public  UInt64  refreshDuration;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPastPresentationTimingGOOGLE {
	public  UInt32  presentID;
	public  UInt64  desiredPresentTime;
	public  UInt64  actualPresentTime;
	public  UInt64  earliestPresentTime;
	public  UInt64  presentMargin;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPresentTimeGOOGLE {
	public  UInt32  presentID;
	public  UInt64  desiredPresentTime;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPresentTimesInfoGOOGLE {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  swapchainCount;
	public  VkPresentTimeGOOGLE*  pTimes;
}

#endregion
#region VK_NVX_multiview_per_view_attributes
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMultiviewPerViewAttributesPropertiesNVX {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  perViewPositionAllComponents;
}

#endregion
#region VK_NV_viewport_swizzle
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkViewportSwizzleNV {
	public  VkViewportCoordinateSwizzleNV  x;
	public  VkViewportCoordinateSwizzleNV  y;
	public  VkViewportCoordinateSwizzleNV  z;
	public  VkViewportCoordinateSwizzleNV  w;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportSwizzleStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  viewportCount;
	public  VkViewportSwizzleNV*  pViewportSwizzles;
}

#endregion
#region VK_EXT_discard_rectangles
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDiscardRectanglePropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxDiscardRectangles;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineDiscardRectangleStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkDiscardRectangleModeEXT  discardRectangleMode;
	public  UInt32  discardRectangleCount;
	public  VkRect2D*  pDiscardRectangles;
}

#endregion
#region VK_EXT_conservative_rasterization
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceConservativeRasterizationPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  float  primitiveOverestimationSize;
	public  float  maxExtraPrimitiveOverestimationSize;
	public  float  extraPrimitiveOverestimationSizeGranularity;
	public  UInt32  primitiveUnderestimation;
	public  UInt32  conservativePointAndLineRasterization;
	public  UInt32  degenerateTrianglesRasterized;
	public  UInt32  degenerateLinesRasterized;
	public  UInt32  fullyCoveredFragmentShaderInputVariable;
	public  UInt32  conservativeRasterizationPostDepthCoverage;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationConservativeStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkConservativeRasterizationModeEXT  conservativeRasterizationMode;
	public  float  extraPrimitiveOverestimationSize;
}

#endregion
#region VK_EXT_depth_clip_enable
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDepthClipEnableFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  depthClipEnable;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationDepthClipStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  depthClipEnable;
}

#endregion
#region VK_EXT_hdr_metadata
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkXYColorEXT {
	public  float  x;
	public  float  y;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkHdrMetadataEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkXYColorEXT  displayPrimaryRed;
	public  VkXYColorEXT  displayPrimaryGreen;
	public  VkXYColorEXT  displayPrimaryBlue;
	public  VkXYColorEXT  whitePoint;
	public  float  maxLuminance;
	public  float  minLuminance;
	public  float  maxContentLightLevel;
	public  float  maxFrameAverageLightLevel;
}

#endregion
#region VK_EXT_debug_utils
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugUtilsLabelEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  ConstChar*  pLabelName;
	public fixed float  color[4];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugUtilsObjectNameInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkObjectType  @objectType;
	public  UInt64  @objectHandle;
	public  ConstChar*  pObjectName;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugUtilsMessengerCallbackDataEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  ConstChar*  pMessageIdName;
	public  Int32  messageIdNumber;
	public  ConstChar*  pMessage;
	public  UInt32  queueLabelCount;
	public  VkDebugUtilsLabelEXT*  pQueueLabels;
	public  UInt32  cmdBufLabelCount;
	public  VkDebugUtilsLabelEXT*  pCmdBufLabels;
	public  UInt32  @objectCount;
	public  VkDebugUtilsObjectNameInfoEXT*  pObjects;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe ref struct VkDebugUtilsMessengerCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  messageSeverity;
	public  UInt32  messageType;
	public  delegate* unmanaged< VkDebugUtilsMessageSeverityFlagBitsEXT,UInt32,VkDebugUtilsMessengerCallbackDataEXT*,void*,UInt32 >  pfnUserCallback;
	public  void*  pUserData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDebugUtilsObjectTagInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkObjectType  @objectType;
	public  UInt64  @objectHandle;
	public  UInt64  tagName;
	public  Int32  tagSize;
	public  void*  pTag;
}

#endregion
#region VK_EXT_sample_locations
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSampleLocationEXT {
	public  float  x;
	public  float  y;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSampleLocationsInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSampleCountFlagBits  sampleLocationsPerPixel;
	public  VkExtent2D  sampleLocationGridSize;
	public  UInt32  sampleLocationsCount;
	public  VkSampleLocationEXT*  pSampleLocations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAttachmentSampleLocationsEXT {
	public  UInt32  attachmentIndex;
	public  VkSampleLocationsInfoEXT  sampleLocationsInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassSampleLocationsEXT {
	public  UInt32  subpassIndex;
	public  VkSampleLocationsInfoEXT  sampleLocationsInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassSampleLocationsBeginInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  attachmentInitialSampleLocationsCount;
	public  VkAttachmentSampleLocationsEXT*  pAttachmentInitialSampleLocations;
	public  UInt32  postSubpassSampleLocationsCount;
	public  VkSubpassSampleLocationsEXT*  pPostSubpassSampleLocations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineSampleLocationsStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  sampleLocationsEnable;
	public  VkSampleLocationsInfoEXT  sampleLocationsInfo;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSampleLocationsPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  sampleLocationSampleCounts;
	public  VkExtent2D  maxSampleLocationGridSize;
	public fixed float  sampleLocationCoordinateRange[2];
	public  UInt32  sampleLocationSubPixelBits;
	public  UInt32  variableSampleLocations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMultisamplePropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent2D  maxSampleLocationGridSize;
}

#endregion
#region VK_EXT_blend_operation_advanced
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceBlendOperationAdvancedFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  advancedBlendCoherentOperations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceBlendOperationAdvancedPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  advancedBlendMaxColorAttachments;
	public  UInt32  advancedBlendIndependentBlend;
	public  UInt32  advancedBlendNonPremultipliedSrcColor;
	public  UInt32  advancedBlendNonPremultipliedDstColor;
	public  UInt32  advancedBlendCorrelatedOverlap;
	public  UInt32  advancedBlendAllOperations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineColorBlendAdvancedStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  srcPremultiplied;
	public  UInt32  dstPremultiplied;
	public  VkBlendOverlapEXT  blendOverlap;
}

#endregion
#region VK_NV_fragment_coverage_to_color
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCoverageToColorStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  coverageToColorEnable;
	public  UInt32  coverageToColorLocation;
}

#endregion
#region VK_NV_framebuffer_mixed_samples
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCoverageModulationStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkCoverageModulationModeNV  coverageModulationMode;
	public  UInt32  coverageModulationTableEnable;
	public  UInt32  coverageModulationTableCount;
	public  float*  pCoverageModulationTable;
}

#endregion
#region VK_NV_shader_sm_builtins
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderSMBuiltinsPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderSMCount;
	public  UInt32  shaderWarpsPerSM;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderSMBuiltinsFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderSMBuiltins;
}

#endregion
#region VK_EXT_image_drm_format_modifier
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrmFormatModifierPropertiesEXT {
	public  UInt64  drmFormatModifier;
	public  UInt32  drmFormatModifierPlaneCount;
	public  UInt32  drmFormatModifierTilingFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrmFormatModifierPropertiesListEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  drmFormatModifierCount;
	public  VkDrmFormatModifierPropertiesEXT*  pDrmFormatModifierProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageDrmFormatModifierInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  drmFormatModifier;
	public  VkSharingMode  sharingMode;
	public  UInt32  queueFamilyIndexCount;
	public  UInt32*  pQueueFamilyIndices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageDrmFormatModifierListCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  drmFormatModifierCount;
	public  UInt64*  pDrmFormatModifiers;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageDrmFormatModifierExplicitCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  drmFormatModifier;
	public  UInt32  drmFormatModifierPlaneCount;
	public  VkSubresourceLayout*  pPlaneLayouts;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageDrmFormatModifierPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  drmFormatModifier;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrmFormatModifierProperties2EXT {
	public  UInt64  drmFormatModifier;
	public  UInt32  drmFormatModifierPlaneCount;
	public  UInt64  drmFormatModifierTilingFeatures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrmFormatModifierPropertiesList2EXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  drmFormatModifierCount;
	public  VkDrmFormatModifierProperties2EXT*  pDrmFormatModifierProperties;
}

#endregion
#region VK_EXT_validation_cache
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkValidationCacheCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  Int32  initialDataSize;
	public  void*  pInitialData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkShaderModuleValidationCacheCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkValidationCacheEXT  validationCache;
}

#endregion
#region VK_NV_shading_rate_image
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkShadingRatePaletteNV {
	public  UInt32  shadingRatePaletteEntryCount;
	public  VkShadingRatePaletteEntryNV*  pShadingRatePaletteEntries;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportShadingRateImageStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shadingRateImageEnable;
	public  UInt32  viewportCount;
	public  VkShadingRatePaletteNV*  pShadingRatePalettes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShadingRateImageFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shadingRateImage;
	public  UInt32  shadingRateCoarseSampleOrder;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShadingRateImagePropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent2D  shadingRateTexelSize;
	public  UInt32  shadingRatePaletteSize;
	public  UInt32  shadingRateMaxCoarseSamples;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCoarseSampleLocationNV {
	public  UInt32  pixelX;
	public  UInt32  pixelY;
	public  UInt32  sample;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCoarseSampleOrderCustomNV {
	public  VkShadingRatePaletteEntryNV  shadingRate;
	public  UInt32  sampleCount;
	public  UInt32  sampleLocationCount;
	public  VkCoarseSampleLocationNV*  pSampleLocations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportCoarseSampleOrderStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkCoarseSampleOrderTypeNV  sampleOrderType;
	public  UInt32  customSampleOrderCount;
	public  VkCoarseSampleOrderCustomNV*  pCustomSampleOrders;
}

#endregion
#region VK_NV_ray_tracing
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRayTracingShaderGroupCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRayTracingShaderGroupTypeKHR  type;
	public  UInt32  generalShader;
	public  UInt32  closestHitShader;
	public  UInt32  anyHitShader;
	public  UInt32  intersectionShader;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRayTracingPipelineCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  stageCount;
	public  VkPipelineShaderStageCreateInfo*  pStages;
	public  UInt32  groupCount;
	public  VkRayTracingShaderGroupCreateInfoNV*  pGroups;
	public  UInt32  maxRecursionDepth;
	public  VkPipelineLayout  layout;
	public  VkPipeline  basePipelineHandle;
	public  Int32  basePipelineIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGeometryTrianglesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  vertexData;
	public  UInt64  vertexOffset;
	public  UInt32  vertexCount;
	public  UInt64  vertexStride;
	public  VkFormat  vertexFormat;
	public  VkBuffer  indexData;
	public  UInt64  indexOffset;
	public  UInt32  indexCount;
	public  VkIndexType  indexType;
	public  VkBuffer  transformData;
	public  UInt64  transformOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGeometryAABBNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkBuffer  aabbData;
	public  UInt32  numAABBs;
	public  UInt32  stride;
	public  UInt64  offset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGeometryDataNV {
	public  VkGeometryTrianglesNV  triangles;
	public  VkGeometryAABBNV  aabbs;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGeometryNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkGeometryTypeKHR  geometryType;
	public  VkGeometryDataNV  geometry;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureTypeKHR  type;
	public  UInt32  flags;
	public  UInt32  instanceCount;
	public  UInt32  geometryCount;
	public  VkGeometryNV*  pGeometries;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  compactedSize;
	public  VkAccelerationStructureInfoNV  info;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindAccelerationStructureMemoryInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureNV  accelerationStructure;
	public  VkDeviceMemory  memory;
	public  UInt64  memoryOffset;
	public  UInt32  deviceIndexCount;
	public  UInt32*  pDeviceIndices;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkWriteDescriptorSetAccelerationStructureNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  accelerationStructureCount;
	public  VkAccelerationStructureNV*  pAccelerationStructures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureMemoryRequirementsInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureMemoryRequirementsTypeNV  type;
	public  VkAccelerationStructureNV  accelerationStructure;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderGroupHandleSize;
	public  UInt32  maxRecursionDepth;
	public  UInt32  maxShaderGroupStride;
	public  UInt32  shaderGroupBaseAlignment;
	public  UInt64  maxGeometryCount;
	public  UInt64  maxInstanceCount;
	public  UInt64  maxTriangleCount;
	public  UInt32  maxDescriptorSetAccelerationStructures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkTransformMatrixKHR {
	public fixed float  matrix[12];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAabbPositionsKHR {
	public  float  minX;
	public  float  minY;
	public  float  minZ;
	public  float  maxX;
	public  float  maxY;
	public  float  maxZ;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe partial struct VkAccelerationStructureInstanceKHR {
    [FieldOffset(0)]public VkTransformMatrixKHR transform;
    [FieldOffset(48)]public uint instanceCustomIndex;
    [FieldOffset(51)]public uint mask;
    [FieldOffset(52)]public uint instanceShaderBindingTableRecordOffset;
    [FieldOffset(55)]public UInt32 flags;
    [FieldOffset(56)]public ulong accelerationStructureReference;
}
#endregion
#region VK_NV_representative_fragment_test
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRepresentativeFragmentTestFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  representativeFragmentTest;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRepresentativeFragmentTestStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  representativeFragmentTestEnable;
}

#endregion
#region VK_EXT_filter_cubic
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageViewImageFormatInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageViewType  imageViewType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFilterCubicImageViewImageFormatPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  filterCubic;
	public  UInt32  filterCubicMinmax;
}

#endregion
#region VK_EXT_external_memory_host
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImportMemoryHostPointerInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExternalMemoryHandleTypeFlagBits  handleType;
	public  void*  pHostPointer;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryHostPointerPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  memoryTypeBits;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExternalMemoryHostPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  minImportedHostPointerAlignment;
}

#endregion
#region VK_AMD_pipeline_compiler_control
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCompilerControlCreateInfoAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  compilerControlFlags;
}

#endregion
#region VK_EXT_calibrated_timestamps
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCalibratedTimestampInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkTimeDomainEXT  timeDomain;
}

#endregion
#region VK_AMD_shader_core_properties
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderCorePropertiesAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderEngineCount;
	public  UInt32  shaderArraysPerEngineCount;
	public  UInt32  computeUnitsPerShaderArray;
	public  UInt32  simdPerComputeUnit;
	public  UInt32  wavefrontsPerSimd;
	public  UInt32  wavefrontSize;
	public  UInt32  sgprsPerSimd;
	public  UInt32  minSgprAllocation;
	public  UInt32  maxSgprAllocation;
	public  UInt32  sgprAllocationGranularity;
	public  UInt32  vgprsPerSimd;
	public  UInt32  minVgprAllocation;
	public  UInt32  maxVgprAllocation;
	public  UInt32  vgprAllocationGranularity;
}

#endregion
#region VK_AMD_memory_overallocation_behavior
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceMemoryOverallocationCreateInfoAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkMemoryOverallocationBehaviorAMD  overallocationBehavior;
}

#endregion
#region VK_EXT_vertex_attribute_divisor
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVertexAttributeDivisorPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxVertexAttribDivisor;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkVertexInputBindingDivisorDescriptionEXT {
	public  UInt32  binding;
	public  UInt32  divisor;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineVertexInputDivisorStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  vertexBindingDivisorCount;
	public  VkVertexInputBindingDivisorDescriptionEXT*  pVertexBindingDivisors;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVertexAttributeDivisorFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  vertexAttributeInstanceRateDivisor;
	public  UInt32  vertexAttributeInstanceRateZeroDivisor;
}

#endregion
#region VK_NV_compute_shader_derivatives
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceComputeShaderDerivativesFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  computeDerivativeGroupQuads;
	public  UInt32  computeDerivativeGroupLinear;
}

#endregion
#region VK_NV_mesh_shader
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMeshShaderFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  taskShader;
	public  UInt32  meshShader;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMeshShaderPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxDrawMeshTasksCount;
	public  UInt32  maxTaskWorkGroupInvocations;
	public fixed UInt32  maxTaskWorkGroupSize[3];
	public  UInt32  maxTaskTotalMemorySize;
	public  UInt32  maxTaskOutputCount;
	public  UInt32  maxMeshWorkGroupInvocations;
	public fixed UInt32  maxMeshWorkGroupSize[3];
	public  UInt32  maxMeshTotalMemorySize;
	public  UInt32  maxMeshOutputVertices;
	public  UInt32  maxMeshOutputPrimitives;
	public  UInt32  maxMeshMultiviewViewCount;
	public  UInt32  meshOutputPerVertexGranularity;
	public  UInt32  meshOutputPerPrimitiveGranularity;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrawMeshTasksIndirectCommandNV {
	public  UInt32  taskCount;
	public  UInt32  firstTask;
}

#endregion
#region VK_NV_shader_image_footprint
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderImageFootprintFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  imageFootprint;
}

#endregion
#region VK_NV_scissor_exclusive
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportExclusiveScissorStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  exclusiveScissorCount;
	public  VkRect2D*  pExclusiveScissors;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExclusiveScissorFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  exclusiveScissor;
}

#endregion
#region VK_NV_device_diagnostic_checkpoints
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueueFamilyCheckpointPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  checkpointExecutionStageMask;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCheckpointDataNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipelineStageFlagBits  stage;
	public  void*  pCheckpointMarker;
}

#endregion
#region VK_INTEL_shader_integer_functions2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderIntegerFunctions2FeaturesINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderIntegerFunctions2;
}

#endregion
#region VK_INTEL_performance_query
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceValueINTEL {
	public  VkPerformanceValueTypeINTEL  type;
	public  VkPerformanceValueDataINTEL  data;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkInitializePerformanceApiInfoINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  void*  pUserData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkQueryPoolPerformanceQueryCreateInfoINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkQueryPoolSamplingModeINTEL  performanceCountersSampling;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceMarkerInfoINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  marker;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceStreamMarkerInfoINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  marker;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceOverrideInfoINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPerformanceOverrideTypeINTEL  type;
	public  UInt32  enable;
	public  UInt64  parameter;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPerformanceConfigurationAcquireInfoINTEL {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPerformanceConfigurationTypeINTEL  type;
}

#endregion
#region VK_EXT_pci_bus_info
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePCIBusInfoPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pciDomain;
	public  UInt32  pciBus;
	public  UInt32  pciDevice;
	public  UInt32  pciFunction;
}

#endregion
#region VK_AMD_display_native_hdr
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDisplayNativeHdrSurfaceCapabilitiesAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  localDimmingSupport;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSwapchainDisplayNativeHdrCreateInfoAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  localDimmingEnable;
}

#endregion
#region VK_EXT_fragment_density_map
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentDensityMapFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentDensityMap;
	public  UInt32  fragmentDensityMapDynamic;
	public  UInt32  fragmentDensityMapNonSubsampledImages;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentDensityMapPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent2D  minFragmentDensityTexelSize;
	public  VkExtent2D  maxFragmentDensityTexelSize;
	public  UInt32  fragmentDensityInvocations;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassFragmentDensityMapCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAttachmentReference  fragmentDensityMapAttachment;
}

#endregion
#region VK_AMD_shader_core_properties2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderCoreProperties2AMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderCoreFeatures;
	public  UInt32  activeComputeUnitCount;
}

#endregion
#region VK_AMD_device_coherent_memory
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCoherentMemoryFeaturesAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceCoherentMemory;
}

#endregion
#region VK_EXT_shader_image_atomic_int64
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderImageAtomicInt64FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderImageInt64Atomics;
	public  UInt32  sparseImageInt64Atomics;
}

#endregion
#region VK_EXT_memory_budget
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMemoryBudgetPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed UInt64  heapBudget[(int)VK.VK_MAX_MEMORY_HEAPS];
	public fixed UInt64  heapUsage[(int)VK.VK_MAX_MEMORY_HEAPS];
}

#endregion
#region VK_EXT_memory_priority
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMemoryPriorityFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  memoryPriority;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryPriorityAllocateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  float  priority;
}

#endregion
#region VK_NV_dedicated_allocation_image_aliasing
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDedicatedAllocationImageAliasingFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dedicatedAllocationImageAliasing;
}

#endregion
#region VK_EXT_buffer_device_address
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceBufferDeviceAddressFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  bufferDeviceAddress;
	public  UInt32  bufferDeviceAddressCaptureReplay;
	public  UInt32  bufferDeviceAddressMultiDevice;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBufferDeviceAddressCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  deviceAddress;
}

#endregion
#region VK_EXT_validation_features
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkValidationFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  enabledValidationFeatureCount;
	public  VkValidationFeatureEnableEXT*  pEnabledValidationFeatures;
	public  UInt32  disabledValidationFeatureCount;
	public  VkValidationFeatureDisableEXT*  pDisabledValidationFeatures;
}

#endregion
#region VK_NV_cooperative_matrix
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCooperativeMatrixPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  MSize;
	public  UInt32  NSize;
	public  UInt32  KSize;
	public  VkComponentTypeNV  AType;
	public  VkComponentTypeNV  BType;
	public  VkComponentTypeNV  CType;
	public  VkComponentTypeNV  DType;
	public  VkScopeNV  scope;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCooperativeMatrixFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  cooperativeMatrix;
	public  UInt32  cooperativeMatrixRobustBufferAccess;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCooperativeMatrixPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  cooperativeMatrixSupportedStages;
}

#endregion
#region VK_NV_coverage_reduction_mode
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCoverageReductionModeFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  coverageReductionMode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineCoverageReductionStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkCoverageReductionModeNV  coverageReductionMode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkFramebufferMixedSamplesCombinationNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkCoverageReductionModeNV  coverageReductionMode;
	public  VkSampleCountFlagBits  rasterizationSamples;
	public  UInt32  depthStencilSamples;
	public  UInt32  colorSamples;
}

#endregion
#region VK_EXT_fragment_shader_interlock
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShaderInterlockFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentShaderSampleInterlock;
	public  UInt32  fragmentShaderPixelInterlock;
	public  UInt32  fragmentShaderShadingRateInterlock;
}

#endregion
#region VK_EXT_ycbcr_image_arrays
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceYcbcrImageArraysFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  ycbcrImageArrays;
}

#endregion
#region VK_EXT_provoking_vertex
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceProvokingVertexFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  provokingVertexLast;
	public  UInt32  transformFeedbackPreservesProvokingVertex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceProvokingVertexPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  provokingVertexModePerPipeline;
	public  UInt32  transformFeedbackPreservesTriangleFanProvokingVertex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationProvokingVertexStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkProvokingVertexModeEXT  provokingVertexMode;
}

#endregion
#region VK_EXT_headless_surface
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkHeadlessSurfaceCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

#endregion
#region VK_EXT_line_rasterization
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceLineRasterizationFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rectangularLines;
	public  UInt32  bresenhamLines;
	public  UInt32  smoothLines;
	public  UInt32  stippledRectangularLines;
	public  UInt32  stippledBresenhamLines;
	public  UInt32  stippledSmoothLines;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceLineRasterizationPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  lineSubPixelPrecisionBits;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineRasterizationLineStateCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkLineRasterizationModeEXT  lineRasterizationMode;
	public  UInt32  stippledLineEnable;
	public  UInt32  lineStippleFactor;
	public  UInt16  lineStipplePattern;
}

#endregion
#region VK_EXT_shader_atomic_float
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderAtomicFloatFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderBufferFloat32Atomics;
	public  UInt32  shaderBufferFloat32AtomicAdd;
	public  UInt32  shaderBufferFloat64Atomics;
	public  UInt32  shaderBufferFloat64AtomicAdd;
	public  UInt32  shaderSharedFloat32Atomics;
	public  UInt32  shaderSharedFloat32AtomicAdd;
	public  UInt32  shaderSharedFloat64Atomics;
	public  UInt32  shaderSharedFloat64AtomicAdd;
	public  UInt32  shaderImageFloat32Atomics;
	public  UInt32  shaderImageFloat32AtomicAdd;
	public  UInt32  sparseImageFloat32Atomics;
	public  UInt32  sparseImageFloat32AtomicAdd;
}

#endregion
#region VK_EXT_index_type_uint8
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceIndexTypeUint8FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  indexTypeUint8;
}

#endregion
#region VK_EXT_extended_dynamic_state
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExtendedDynamicStateFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  extendedDynamicState;
}

#endregion
#region VK_EXT_shader_atomic_float2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderAtomicFloat2FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderBufferFloat16Atomics;
	public  UInt32  shaderBufferFloat16AtomicAdd;
	public  UInt32  shaderBufferFloat16AtomicMinMax;
	public  UInt32  shaderBufferFloat32AtomicMinMax;
	public  UInt32  shaderBufferFloat64AtomicMinMax;
	public  UInt32  shaderSharedFloat16Atomics;
	public  UInt32  shaderSharedFloat16AtomicAdd;
	public  UInt32  shaderSharedFloat16AtomicMinMax;
	public  UInt32  shaderSharedFloat32AtomicMinMax;
	public  UInt32  shaderSharedFloat64AtomicMinMax;
	public  UInt32  shaderImageFloat32AtomicMinMax;
	public  UInt32  sparseImageFloat32AtomicMinMax;
}

#endregion
#region VK_NV_device_generated_commands
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDeviceGeneratedCommandsPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxGraphicsShaderGroupCount;
	public  UInt32  maxIndirectSequenceCount;
	public  UInt32  maxIndirectCommandsTokenCount;
	public  UInt32  maxIndirectCommandsStreamCount;
	public  UInt32  maxIndirectCommandsTokenOffset;
	public  UInt32  maxIndirectCommandsStreamStride;
	public  UInt32  minSequencesCountBufferOffsetAlignment;
	public  UInt32  minSequencesIndexBufferOffsetAlignment;
	public  UInt32  minIndirectCommandsBufferOffsetAlignment;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDeviceGeneratedCommandsFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceGeneratedCommands;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGraphicsShaderGroupCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  stageCount;
	public  VkPipelineShaderStageCreateInfo*  pStages;
	public  VkPipelineVertexInputStateCreateInfo*  pVertexInputState;
	public  VkPipelineTessellationStateCreateInfo*  pTessellationState;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGraphicsPipelineShaderGroupsCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  groupCount;
	public  VkGraphicsShaderGroupCreateInfoNV*  pGroups;
	public  UInt32  pipelineCount;
	public  VkPipeline*  pPipelines;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindShaderGroupIndirectCommandNV {
	public  UInt32  groupIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindIndexBufferIndirectCommandNV {
	public  UInt64  bufferAddress;
	public  UInt32  size;
	public  VkIndexType  indexType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkBindVertexBufferIndirectCommandNV {
	public  UInt64  bufferAddress;
	public  UInt32  size;
	public  UInt32  stride;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSetStateFlagsIndirectCommandNV {
	public  UInt32  data;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkIndirectCommandsStreamNV {
	public  VkBuffer  buffer;
	public  UInt64  offset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkIndirectCommandsLayoutTokenNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkIndirectCommandsTokenTypeNV  tokenType;
	public  UInt32  stream;
	public  UInt32  offset;
	public  UInt32  vertexBindingUnit;
	public  UInt32  vertexDynamicStride;
	public  VkPipelineLayout  pushantPipelineLayout;
	public  UInt32  pushantShaderStageFlags;
	public  UInt32  pushantOffset;
	public  UInt32  pushantSize;
	public  UInt32  indirectStateFlags;
	public  UInt32  indexTypeCount;
	public  VkIndexType*  pIndexTypes;
	public  UInt32*  pIndexTypeValues;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkIndirectCommandsLayoutCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkPipelineBindPoint  pipelineBindPoint;
	public  UInt32  tokenCount;
	public  VkIndirectCommandsLayoutTokenNV*  pTokens;
	public  UInt32  streamCount;
	public  UInt32*  pStreamStrides;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGeneratedCommandsInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipelineBindPoint  pipelineBindPoint;
	public  VkPipeline  pipeline;
	public  VkIndirectCommandsLayoutNV  indirectCommandsLayout;
	public  UInt32  streamCount;
	public  VkIndirectCommandsStreamNV*  pStreams;
	public  UInt32  sequencesCount;
	public  VkBuffer  preprocessBuffer;
	public  UInt64  preprocessOffset;
	public  UInt64  preprocessSize;
	public  VkBuffer  sequencesCountBuffer;
	public  UInt64  sequencesCountOffset;
	public  VkBuffer  sequencesIndexBuffer;
	public  UInt64  sequencesIndexOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGeneratedCommandsMemoryRequirementsInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkPipelineBindPoint  pipelineBindPoint;
	public  VkPipeline  pipeline;
	public  VkIndirectCommandsLayoutNV  indirectCommandsLayout;
	public  UInt32  maxSequencesCount;
}

#endregion
#region VK_NV_inherited_viewport_scissor
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceInheritedViewportScissorFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  inheritedViewportScissor2D;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferInheritanceViewportScissorInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  viewportScissor2D;
	public  UInt32  viewportDepthCount;
	public  VkViewport*  pViewportDepths;
}

#endregion
#region VK_EXT_texel_buffer_alignment
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTexelBufferAlignmentFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  texelBufferAlignment;
}

#endregion
#region VK_QCOM_render_pass_transform
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassTransformBeginInfoQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSurfaceTransformFlagBitsKHR  transform;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCommandBufferInheritanceRenderPassTransformInfoQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSurfaceTransformFlagBitsKHR  transform;
	public  VkRect2D  renderArea;
}

#endregion
#region VK_EXT_device_memory_report
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDeviceMemoryReportFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceMemoryReport;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceMemoryReportCallbackDataEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  VkDeviceMemoryReportEventTypeEXT  type;
	public  UInt64  memoryObjectId;
	public  UInt64  size;
	public  VkObjectType  @objectType;
	public  UInt64  @objectHandle;
	public  UInt32  heapIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceDeviceMemoryReportCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  delegate* unmanaged< VkDeviceMemoryReportCallbackDataEXT*,void*,void >  pfnUserCallback;
	public  void*  pUserData;
}

#endregion
#region VK_EXT_robustness2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRobustness2FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  robustBufferAccess2;
	public  UInt32  robustImageAccess2;
	public  UInt32  nullDescriptor;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRobustness2PropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  robustStorageBufferAccessSizeAlignment;
	public  UInt64  robustUniformBufferAccessSizeAlignment;
}

#endregion
#region VK_EXT_custom_border_color
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerCustomBorderColorCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkClearColorValue  customBorderColor;
	public  VkFormat  format;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCustomBorderColorPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxCustomBorderColorSamplers;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCustomBorderColorFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  customBorderColors;
	public  UInt32  customBorderColorWithoutFormat;
}

#endregion
#region VK_NV_present_barrier
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePresentBarrierFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  presentBarrier;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSurfaceCapabilitiesPresentBarrierNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  presentBarrierSupported;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSwapchainPresentBarrierCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  presentBarrierEnable;
}

#endregion
#region VK_NV_device_diagnostics_config
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDiagnosticsConfigFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  diagnosticsConfig;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceDiagnosticsConfigCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

#endregion
#region VK_EXT_graphics_pipeline_library
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceGraphicsPipelineLibraryFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  graphicsPipelineLibrary;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceGraphicsPipelineLibraryPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  graphicsPipelineLibraryFastLinking;
	public  UInt32  graphicsPipelineLibraryIndependentInterpolationDecoration;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkGraphicsPipelineLibraryCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
}

#endregion
#region VK_AMD_shader_early_and_late_fragment_tests
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderEarlyAndLateFragmentTestsFeaturesAMD {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderEarlyAndLateFragmentTests;
}

#endregion
#region VK_NV_fragment_shading_rate_enums
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShadingRateEnumsFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentShadingRateEnums;
	public  UInt32  supersampleFragmentShadingRates;
	public  UInt32  noInvocationFragmentShadingRates;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentShadingRateEnumsPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSampleCountFlagBits  maxFragmentShadingRateInvocationCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineFragmentShadingRateEnumStateCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFragmentShadingRateTypeNV  shadingRateType;
	public  VkFragmentShadingRateNV  shadingRate;
	public VkFragmentShadingRateCombinerOpKHR  combinerOps;//[2];
}

#endregion
#region VK_NV_ray_tracing_motion_blur
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureGeometryMotionTrianglesDataNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceOrHostAddressConstKHR  vertexData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureMotionInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxInstances;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkAccelerationStructureMatrixMotionInstanceNV {
	[FieldOffset(0)] public  VkTransformMatrixKHR  transformT0;
	[FieldOffset(64)]public  VkTransformMatrixKHR  transformT1;
	[FieldOffset(128)]public  UInt32  instanceCustomIndex;
	[FieldOffset(131)]public  UInt32  mask;
	[FieldOffset(132)]public  UInt32  instanceShaderBindingTableRecordOffset;
	[FieldOffset(133)]public  UInt32  flags;
	[FieldOffset(136)]public  UInt64  accelerationStructureReference;
}
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSRTDataNV {
	public  float  sx;
	public  float  a;
	public  float  b;
	public  float  pvx;
	public  float  sy;
	public  float  c;
	public  float  pvy;
	public  float  sz;
	public  float  pvz;
	public  float  qx;
	public  float  qy;
	public  float  qz;
	public  float  qw;
	public  float  tx;
	public  float  ty;
	public  float  tz;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe  struct VkAccelerationStructureSRTMotionInstanceNV {
	[FieldOffset(0)]public  VkSRTDataNV  transformT0;
	[FieldOffset(0)]public  VkSRTDataNV  transformT1;
	[FieldOffset(0)]public  UInt32  instanceCustomIndex;
	[FieldOffset(0)]public  UInt32  mask;
	[FieldOffset(0)]public  UInt32  instanceShaderBindingTableRecordOffset;
	[FieldOffset(0)]public  UInt32  flags;
	[FieldOffset(0)]public  UInt64  accelerationStructureReference;
}
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureMotionInstanceNV {
	public  VkAccelerationStructureMotionInstanceTypeNV  type;
	public  UInt32  flags;
	public  VkAccelerationStructureMotionInstanceDataNV  data;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingMotionBlurFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rayTracingMotionBlur;
	public  UInt32  rayTracingMotionBlurPipelineTraceRaysIndirect;
}

#endregion
#region VK_EXT_ycbcr_2plane_444_formats
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceYcbcr2Plane444FormatsFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  ycbcr2plane444Formats;
}

#endregion
#region VK_EXT_fragment_density_map2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentDensityMap2FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentDensityMapDeferred;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentDensityMap2PropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  subsampledLoads;
	public  UInt32  subsampledCoarseReructionEarlyAccess;
	public  UInt32  maxSubsampledArrayLayers;
	public  UInt32  maxDescriptorSetSubsampledSamplers;
}

#endregion
#region VK_QCOM_rotated_copy_commands
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyCommandTransformInfoQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSurfaceTransformFlagBitsKHR  transform;
}

#endregion
#region VK_EXT_image_compression_control
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageCompressionControlFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  imageCompressionControl;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageCompressionControlEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  compressionControlPlaneCount;
	public  UInt32*  pFixedRateFlags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubresourceLayout2EXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkSubresourceLayout  subresourceLayout;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageSubresource2EXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkImageSubresource  imageSubresource;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageCompressionPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  imageCompressionFlags;
	public  UInt32  imageCompressionFixedRateFlags;
}

#endregion
#region VK_EXT_attachment_feedback_loop_layout
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceAttachmentFeedbackLoopLayoutFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  attachmentFeedbackLoopLayout;
}

#endregion
#region VK_EXT_4444_formats
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevice4444FormatsFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  formatA4R4G4B4;
	public  UInt32  formatA4B4G4R4;
}

#endregion
#region VK_EXT_device_fault
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFaultFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  deviceFault;
	public  UInt32  deviceFaultVendorBinary;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceFaultCountsEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  addressInfoCount;
	public  UInt32  vendorInfoCount;
	public  UInt64  vendorBinarySize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceFaultAddressInfoEXT {
	public  VkDeviceFaultAddressTypeEXT  addressType;
	public  UInt64  reportedAddress;
	public  UInt64  addressPrecision;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceFaultVendorInfoEXT {
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public  UInt64  vendorFaultCode;
	public  UInt64  vendorFaultData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceFaultInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public  VkDeviceFaultAddressInfoEXT*  pAddressInfos;
	public  VkDeviceFaultVendorInfoEXT*  pVendorInfos;
	public  void*  pVendorBinaryData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDeviceFaultVendorBinaryHeaderVersionOneEXT {
	public  UInt32  headerSize;
	public  VkDeviceFaultVendorBinaryHeaderVersionEXT  headerVersion;
	public  UInt32  vendorID;
	public  UInt32  deviceID;
	public  UInt32  driverVersion;
	public fixed ConstChar  pipelineCacheUUID[(int)VK.VK_UUID_SIZE];
	public  UInt32  applicationNameOffset;
	public  UInt32  applicationVersion;
	public  UInt32  engineNameOffset;
}

#endregion
#region VK_ARM_rasterization_order_attachment_access
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRasterizationOrderAttachmentAccessFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rasterizationOrderColorAttachmentAccess;
	public  UInt32  rasterizationOrderDepthAttachmentAccess;
	public  UInt32  rasterizationOrderStencilAttachmentAccess;
}

#endregion
#region VK_EXT_rgba10x6_formats
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRGBA10X6FormatsFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  formatRgba10x6WithoutYCbCrSampler;
}

#endregion
#region VK_VALVE_mutable_descriptor_type
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMutableDescriptorTypeFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  mutableDescriptorType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMutableDescriptorTypeListEXT {
	public  UInt32  descriptorTypeCount;
	public  VkDescriptorType*  pDescriptorTypes;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMutableDescriptorTypeCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  mutableDescriptorTypeListCount;
	public  VkMutableDescriptorTypeListEXT*  pMutableDescriptorTypeLists;
}

#endregion
#region VK_EXT_vertex_input_dynamic_state
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceVertexInputDynamicStateFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  vertexInputDynamicState;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkVertexInputBindingDescription2EXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  binding;
	public  UInt32  stride;
	public  VkVertexInputRate  inputRate;
	public  UInt32  divisor;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkVertexInputAttributeDescription2EXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  location;
	public  UInt32  binding;
	public  VkFormat  format;
	public  UInt32  offset;
}

#endregion
#region VK_EXT_physical_device_drm
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDrmPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  hasPrimary;
	public  UInt32  hasRender;
	public  Int64  primaryMajor;
	public  Int64  primaryMinor;
	public  Int64  renderMajor;
	public  Int64  renderMinor;
}

#endregion
#region VK_EXT_device_address_binding_report
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceAddressBindingReportFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  reportAddressBinding;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct UInt64BindingCallbackDataEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  flags;
	public  UInt64  baseAddress;
	public  UInt64  size;
	public  UInt64BindingTypeEXT  bindingType;
}

#endregion
#region VK_EXT_depth_clip_control
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDepthClipControlFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  depthClipControl;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineViewportDepthClipControlCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  negativeOneToOne;
}

#endregion
#region VK_EXT_primitive_topology_list_restart
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePrimitiveTopologyListRestartFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  primitiveTopologyListRestart;
	public  UInt32  primitiveTopologyPatchListRestart;
}

#endregion
#region VK_HUAWEI_subpass_shading
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassShadingPipelineCreateInfoHUAWEI {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRenderPass  renderPass;
	public  UInt32  subpass;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSubpassShadingFeaturesHUAWEI {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  subpassShading;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSubpassShadingPropertiesHUAWEI {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxSubpassShadingWorkgroupSizeAspectRatio;
}

#endregion
#region VK_HUAWEI_invocation_mask
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceInvocationMaskFeaturesHUAWEI {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  invocationMask;
}

#endregion
#region VK_NV_external_memory_rdma
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMemoryGetRemoteAddressInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceMemory  memory;
	public  VkExternalMemoryHandleTypeFlagBits  handleType;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExternalMemoryRDMAFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  externalMemoryRDMA;
}

#endregion
#region VK_EXT_pipeline_properties
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelinePropertiesIdentifierEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  pipelineIdentifier[(int)VK.VK_UUID_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePipelinePropertiesFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pipelinePropertiesIdentifier;
}

#endregion
#region VK_EXT_multisampled_render_to_single_sampled
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMultisampledRenderToSingleSampledFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  multisampledRenderToSingleSampled;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassResolvePerformanceQueryEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  optimal;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMultisampledRenderToSingleSampledInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  multisampledRenderToSingleSampledEnable;
	public  VkSampleCountFlagBits  rasterizationSamples;
}

#endregion
#region VK_EXT_extended_dynamic_state2
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExtendedDynamicState2FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  extendedDynamicState2;
	public  UInt32  extendedDynamicState2LogicOp;
	public  UInt32  extendedDynamicState2PatchControlPoints;
}

#endregion
#region VK_EXT_color_write_enable
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceColorWriteEnableFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  colorWriteEnable;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineColorWriteCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  attachmentCount;
	public  UInt32*  pColorWriteEnables;
}

#endregion
#region VK_EXT_primitives_generated_query
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePrimitivesGeneratedQueryFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  primitivesGeneratedQuery;
	public  UInt32  primitivesGeneratedQueryWithRasterizerDiscard;
	public  UInt32  primitivesGeneratedQueryWithNonZeroStreams;
}

#endregion
#region VK_EXT_image_view_min_lod
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageViewMinLodFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  minLod;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewMinLodCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  float  minLod;
}

#endregion
#region VK_EXT_multi_draw
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMultiDrawFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  multiDraw;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMultiDrawPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxMultiDrawCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMultiDrawInfoEXT {
	public  UInt32  firstVertex;
	public  UInt32  vertexCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMultiDrawIndexedInfoEXT {
	public  UInt32  firstIndex;
	public  UInt32  indexCount;
	public  Int32  vertexOffset;
}

#endregion
#region VK_EXT_image_2d_view_of_3d
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImage2DViewOf3DFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  image2DViewOf3D;
	public  UInt32  sampler2DViewOf3D;
}

#endregion
#region VK_EXT_opacity_micromap
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMicromapUsageEXT {
	public  UInt32  count;
	public  UInt32  subdivisionLevel;
	public  UInt32  format;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMicromapBuildInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkMicromapTypeEXT  type;
	public  UInt32  flags;
	public  VkBuildMicromapModeEXT  mode;
	public  VkMicromapEXT  dstMicromap;
	public  UInt32  usageCountsCount;
	public  VkMicromapUsageEXT*  pUsageCounts;
	public  VkMicromapUsageEXT**  ppUsageCounts;
	public  VkDeviceOrHostAddressConstKHR  data;
	public  VkDeviceOrHostAddressKHR  scratchData;
	public  VkDeviceOrHostAddressConstKHR  triangleArray;
	public  UInt64  triangleArrayStride;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMicromapCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  createFlags;
	public  VkBuffer  buffer;
	public  UInt64  offset;
	public  UInt64  size;
	public  VkMicromapTypeEXT  type;
	public  UInt64  deviceAddress;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceOpacityMicromapFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  micromap;
	public  UInt32  micromapCaptureReplay;
	public  UInt32  micromapHostCommands;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceOpacityMicromapPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxOpacity2StateSubdivisionLevel;
	public  UInt32  maxOpacity4StateSubdivisionLevel;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMicromapVersionInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  ConstChar*  pVersionData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyMicromapToMemoryInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkMicromapEXT  src;
	public  VkDeviceOrHostAddressKHR  dst;
	public  VkCopyMicromapModeEXT  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyMemoryToMicromapInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceOrHostAddressConstKHR  src;
	public  VkMicromapEXT  dst;
	public  VkCopyMicromapModeEXT  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyMicromapInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkMicromapEXT  src;
	public  VkMicromapEXT  dst;
	public  VkCopyMicromapModeEXT  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMicromapBuildSizesInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  micromapSize;
	public  UInt64  buildScratchSize;
	public  UInt32  discardable;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureTrianglesOpacityMicromapEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkIndexType  indexType;
	public  VkDeviceOrHostAddressConstKHR  indexBuffer;
	public  UInt64  indexStride;
	public  UInt32  baseTriangle;
	public  UInt32  usageCountsCount;
	public  VkMicromapUsageEXT*  pUsageCounts;
	public  VkMicromapUsageEXT**  ppUsageCounts;
	public  VkMicromapEXT  micromap;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkMicromapTriangleEXT {
	public  UInt32  dataOffset;
	public  UInt16  subdivisionLevel;
	public  UInt16  format;
}

#endregion
#region VK_EXT_border_color_swizzle
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceBorderColorSwizzleFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  borderColorSwizzle;
	public  UInt32  borderColorSwizzleFromImage;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSamplerBorderColorComponentMappingCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkComponentMapping  components;
	public  UInt32  srgb;
}

#endregion
#region VK_EXT_pageable_device_local_memory
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePageableDeviceLocalMemoryFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pageableDeviceLocalMemory;
}

#endregion
#region VK_VALVE_descriptor_set_host_mapping
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDescriptorSetHostMappingFeaturesVALVE {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  descriptorSetHostMapping;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetBindingReferenceVALVE {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDescriptorSetLayout  descriptorSetLayout;
	public  UInt32  binding;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDescriptorSetLayoutHostMappingInfoVALVE {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  Int32  descriptorOffset;
	public  UInt32  descriptorSize;
}

#endregion
#region VK_EXT_depth_clamp_zero_one
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceDepthClampZeroOneFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  depthClampZeroOne;
}

#endregion
#region VK_EXT_non_seamless_cube_map
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceNonSeamlessCubeMapFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  nonSeamlessCubeMap;
}

#endregion
#region VK_QCOM_fragment_density_map_offset
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentDensityMapOffsetFeaturesQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentDensityMapOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceFragmentDensityMapOffsetPropertiesQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent2D  fragmentDensityOffsetGranularity;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkSubpassFragmentDensityMapOffsetEndInfoQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  fragmentDensityOffsetCount;
	public  VkOffset2D*  pFragmentDensityOffsets;
}

#endregion
#region VK_NV_copy_memory_indirect
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyMemoryIndirectCommandNV {
	public  UInt64  srcAddress;
	public  UInt64  dstAddress;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyMemoryToImageIndirectCommandNV {
	public  UInt64  srcAddress;
	public  UInt32  bufferRowLength;
	public  UInt32  bufferImageHeight;
	public  VkImageSubresourceLayers  imageSubresource;
	public  VkOffset3D  imageOffset;
	public  VkExtent3D  imageExtent;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCopyMemoryIndirectFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  indirectCopy;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceCopyMemoryIndirectPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  supportedQueues;
}

#endregion
#region VK_NV_memory_decompression
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDecompressMemoryRegionNV {
	public  UInt64  srcAddress;
	public  UInt64  dstAddress;
	public  UInt64  compressedSize;
	public  UInt64  decompressedSize;
	public  UInt64  decompressionMethod;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMemoryDecompressionFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  memoryDecompression;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMemoryDecompressionPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  decompressionMethods;
	public  UInt64  maxDecompressionIndirectCount;
}

#endregion
#region VK_NV_linear_color_attachment
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceLinearColorAttachmentFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  linearColorAttachment;
}

#endregion
#region VK_EXT_image_compression_control_swapchain
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageCompressionControlSwapchainFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  imageCompressionControlSwapchain;
}

#endregion
#region VK_QCOM_image_processing
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkImageViewSampleWeightCreateInfoQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkOffset2D  filterCenter;
	public  VkExtent2D  filterSize;
	public  UInt32  numPhases;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageProcessingFeaturesQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  textureSampleWeighted;
	public  UInt32  textureBoxFilter;
	public  UInt32  textureBlockMatch;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceImageProcessingPropertiesQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxWeightFilterPhases;
	public  VkExtent2D  maxWeightFilterDimension;
	public  VkExtent2D  maxBlockMatchRegion;
	public  VkExtent2D  maxBoxFilterBlockSize;
}

#endregion
#region VK_EXT_extended_dynamic_state3
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExtendedDynamicState3FeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  extendedDynamicState3TessellationDomainOrigin;
	public  UInt32  extendedDynamicState3DepthClampEnable;
	public  UInt32  extendedDynamicState3PolygonMode;
	public  UInt32  extendedDynamicState3RasterizationSamples;
	public  UInt32  extendedDynamicState3SampleMask;
	public  UInt32  extendedDynamicState3AlphaToCoverageEnable;
	public  UInt32  extendedDynamicState3AlphaToOneEnable;
	public  UInt32  extendedDynamicState3LogicOpEnable;
	public  UInt32  extendedDynamicState3ColorBlendEnable;
	public  UInt32  extendedDynamicState3ColorBlendEquation;
	public  UInt32  extendedDynamicState3ColorWriteMask;
	public  UInt32  extendedDynamicState3RasterizationStream;
	public  UInt32  extendedDynamicState3ConservativeRasterizationMode;
	public  UInt32  extendedDynamicState3ExtraPrimitiveOverestimationSize;
	public  UInt32  extendedDynamicState3DepthClipEnable;
	public  UInt32  extendedDynamicState3SampleLocationsEnable;
	public  UInt32  extendedDynamicState3ColorBlendAdvanced;
	public  UInt32  extendedDynamicState3ProvokingVertexMode;
	public  UInt32  extendedDynamicState3LineRasterizationMode;
	public  UInt32  extendedDynamicState3LineStippleEnable;
	public  UInt32  extendedDynamicState3DepthClipNegativeOneToOne;
	public  UInt32  extendedDynamicState3ViewportWScalingEnable;
	public  UInt32  extendedDynamicState3ViewportSwizzle;
	public  UInt32  extendedDynamicState3CoverageToColorEnable;
	public  UInt32  extendedDynamicState3CoverageToColorLocation;
	public  UInt32  extendedDynamicState3CoverageModulationMode;
	public  UInt32  extendedDynamicState3CoverageModulationTableEnable;
	public  UInt32  extendedDynamicState3CoverageModulationTable;
	public  UInt32  extendedDynamicState3CoverageReductionMode;
	public  UInt32  extendedDynamicState3RepresentativeFragmentTestEnable;
	public  UInt32  extendedDynamicState3ShadingRateImageEnable;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceExtendedDynamicState3PropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  dynamicPrimitiveTopologyUnrestricted;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkColorBlendEquationEXT {
	public  VkBlendFactor  srcColorBlendFactor;
	public  VkBlendFactor  dstColorBlendFactor;
	public  VkBlendOp  colorBlendOp;
	public  VkBlendFactor  srcAlphaBlendFactor;
	public  VkBlendFactor  dstAlphaBlendFactor;
	public  VkBlendOp  alphaBlendOp;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkColorBlendAdvancedEXT {
	public  VkBlendOp  advancedBlendOp;
	public  UInt32  srcPremultiplied;
	public  UInt32  dstPremultiplied;
	public  VkBlendOverlapEXT  blendOverlap;
	public  UInt32  clampResults;
}

#endregion
#region VK_EXT_subpass_merge_feedback
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceSubpassMergeFeedbackFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  subpassMergeFeedback;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassCreationControlEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  disallowMerging;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassCreationFeedbackInfoEXT {
	public  UInt32  postMergeSubpassCount;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassCreationFeedbackCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRenderPassCreationFeedbackInfoEXT*  pRenderPassFeedback;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassSubpassFeedbackInfoEXT {
	public  VkSubpassMergeStatusEXT  subpassMergeStatus;
	public fixed ConstChar  description[(int)VK.VK_MAX_DESCRIPTION_SIZE];
	public  UInt32  postMergeIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRenderPassSubpassFeedbackCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRenderPassSubpassFeedbackInfoEXT*  pSubpassFeedback;
}

#endregion
#region VK_EXT_shader_module_identifier
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderModuleIdentifierFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderModuleIdentifier;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderModuleIdentifierPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public fixed ConstChar  shaderModuleIdentifierAlgorithmUUID[(int)VK.VK_UUID_SIZE];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPipelineShaderStageModuleIdentifierCreateInfoEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  identifierSize;
	public  ConstChar*  pIdentifier;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkShaderModuleIdentifierEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  identifierSize;
	public fixed ConstChar  identifier[(int)VK.VK_MAX_SHADER_MODULE_IDENTIFIER_SIZE_EXT];
}

#endregion
#region VK_NV_optical_flow
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceOpticalFlowFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  opticalFlow;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceOpticalFlowPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  supportedOutputGridSizes;
	public  UInt32  supportedHintGridSizes;
	public  UInt32  hintSupported;
	public  UInt32  costSupported;
	public  UInt32  bidirectionalFlowSupported;
	public  UInt32  globalFlowSupported;
	public  UInt32  minWidth;
	public  UInt32  minHeight;
	public  UInt32  maxWidth;
	public  UInt32  maxHeight;
	public  UInt32  maxNumRegionsOfInterest;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOpticalFlowImageFormatInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  usage;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOpticalFlowImageFormatPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormat  format;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOpticalFlowSessionCreateInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  width;
	public  UInt32  height;
	public  VkFormat  imageFormat;
	public  VkFormat  flowVectorFormat;
	public  VkFormat  costFormat;
	public  UInt32  outputGridSize;
	public  UInt32  hintGridSize;
	public  VkOpticalFlowPerformanceLevelNV  performanceLevel;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOpticalFlowSessionCreatePrivateDataInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  id;
	public  UInt32  size;
	public  void*  pPrivateData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkOpticalFlowExecuteInfoNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  regionCount;
	public  VkRect2D*  pRegions;
}

#endregion
#region VK_EXT_legacy_dithering
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceLegacyDitheringFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  legacyDithering;
}

#endregion
#region VK_EXT_pipeline_protected_access
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDevicePipelineProtectedAccessFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  pipelineProtectedAccess;
}

#endregion
#region VK_QCOM_tile_properties
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceTilePropertiesFeaturesQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  tileProperties;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkTilePropertiesQCOM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkExtent3D  tileSize;
	public  VkExtent2D  apronSize;
	public  VkOffset2D  origin;
}

#endregion
#region VK_SEC_amigo_profiling
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceAmigoProfilingFeaturesSEC {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  amigoProfiling;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAmigoProfilingSubmitInfoSEC {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  firstDrawTimestamp;
	public  UInt64  swapBufferTimestamp;
}

#endregion
#region VK_NV_ray_tracing_invocation_reorder
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingInvocationReorderPropertiesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRayTracingInvocationReorderModeNV  rayTracingInvocationReorderReorderingHint;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingInvocationReorderFeaturesNV {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rayTracingInvocationReorder;
}

#endregion
#region VK_ARM_shader_core_builtins
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderCoreBuiltinsFeaturesARM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderCoreBuiltins;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceShaderCoreBuiltinsPropertiesARM {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  shaderCoreMask;
	public  UInt32  shaderCoreCount;
	public  UInt32  shaderWarpsPerCore;
}

#endregion
#region VK_KHR_acceleration_structure
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureBuildRangeInfoKHR {
	public  UInt32  primitiveCount;
	public  UInt32  primitiveOffset;
	public  UInt32  firstVertex;
	public  UInt32  transformOffset;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureGeometryTrianglesDataKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkFormat  vertexFormat;
	public  VkDeviceOrHostAddressConstKHR  vertexData;
	public  UInt64  vertexStride;
	public  UInt32  maxVertex;
	public  VkIndexType  indexType;
	public  VkDeviceOrHostAddressConstKHR  indexData;
	public  VkDeviceOrHostAddressConstKHR  transformData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureGeometryAabbsDataKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceOrHostAddressConstKHR  data;
	public  UInt64  stride;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureGeometryInstancesDataKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  arrayOfPointers;
	public  VkDeviceOrHostAddressConstKHR  data;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureGeometryKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkGeometryTypeKHR  geometryType;
	public  VkAccelerationStructureGeometryDataKHR  geometry;
	public  UInt32  flags;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureBuildGeometryInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureTypeKHR  type;
	public  UInt32  flags;
	public  VkBuildAccelerationStructureModeKHR  mode;
	public  VkAccelerationStructureKHR  srcAccelerationStructure;
	public  VkAccelerationStructureKHR  dstAccelerationStructure;
	public  UInt32  geometryCount;
	public  VkAccelerationStructureGeometryKHR*  pGeometries;
	public  VkAccelerationStructureGeometryKHR**  ppGeometries;
	public  VkDeviceOrHostAddressKHR  scratchData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  createFlags;
	public  VkBuffer  buffer;
	public  UInt64  offset;
	public  UInt64  size;
	public  VkAccelerationStructureTypeKHR  type;
	public  UInt64  deviceAddress;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkWriteDescriptorSetAccelerationStructureKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  accelerationStructureCount;
	public  VkAccelerationStructureKHR*  pAccelerationStructures;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceAccelerationStructureFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  accelerationStructure;
	public  UInt32  accelerationStructureCaptureReplay;
	public  UInt32  accelerationStructureIndirectBuild;
	public  UInt32  accelerationStructureHostCommands;
	public  UInt32  descriptorBindingAccelerationStructureUpdateAfterBind;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceAccelerationStructurePropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  maxGeometryCount;
	public  UInt64  maxInstanceCount;
	public  UInt64  maxPrimitiveCount;
	public  UInt32  maxPerStageDescriptorAccelerationStructures;
	public  UInt32  maxPerStageDescriptorUpdateAfterBindAccelerationStructures;
	public  UInt32  maxDescriptorSetAccelerationStructures;
	public  UInt32  maxDescriptorSetUpdateAfterBindAccelerationStructures;
	public  UInt32  minAccelerationStructureScratchOffsetAlignment;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureDeviceAddressInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureKHR  accelerationStructure;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureVersionInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  ConstChar*  pVersionData;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyAccelerationStructureToMemoryInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureKHR  src;
	public  VkDeviceOrHostAddressKHR  dst;
	public  VkCopyAccelerationStructureModeKHR  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyMemoryToAccelerationStructureInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkDeviceOrHostAddressConstKHR  src;
	public  VkAccelerationStructureKHR  dst;
	public  VkCopyAccelerationStructureModeKHR  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkCopyAccelerationStructureInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkAccelerationStructureKHR  src;
	public  VkAccelerationStructureKHR  dst;
	public  VkCopyAccelerationStructureModeKHR  mode;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkAccelerationStructureBuildSizesInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt64  accelerationStructureSize;
	public  UInt64  updateScratchSize;
	public  UInt64  buildScratchSize;
}

#endregion
#region VK_KHR_ray_tracing_pipeline
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRayTracingShaderGroupCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  VkRayTracingShaderGroupTypeKHR  type;
	public  UInt32  generalShader;
	public  UInt32  closestHitShader;
	public  UInt32  anyHitShader;
	public  UInt32  intersectionShader;
	public  void*  pShaderGroupCaptureReplayHandle;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRayTracingPipelineInterfaceCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxPipelineRayPayloadSize;
	public  UInt32  maxPipelineRayHitAttributeSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkRayTracingPipelineCreateInfoKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  flags;
	public  UInt32  stageCount;
	public  VkPipelineShaderStageCreateInfo*  pStages;
	public  UInt32  groupCount;
	public  VkRayTracingShaderGroupCreateInfoKHR*  pGroups;
	public  UInt32  maxPipelineRayRecursionDepth;
	public  VkPipelineLibraryCreateInfoKHR*  pLibraryInfo;
	public  VkRayTracingPipelineInterfaceCreateInfoKHR*  pLibraryInterface;
	public  VkPipelineDynamicStateCreateInfo*  pDynamicState;
	public  VkPipelineLayout  layout;
	public  VkPipeline  basePipelineHandle;
	public  Int32  basePipelineIndex;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingPipelineFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rayTracingPipeline;
	public  UInt32  rayTracingPipelineShaderGroupHandleCaptureReplay;
	public  UInt32  rayTracingPipelineShaderGroupHandleCaptureReplayMixed;
	public  UInt32  rayTracingPipelineTraceRaysIndirect;
	public  UInt32  rayTraversalPrimitiveCulling;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayTracingPipelinePropertiesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  shaderGroupHandleSize;
	public  UInt32  maxRayRecursionDepth;
	public  UInt32  maxShaderGroupStride;
	public  UInt32  shaderGroupBaseAlignment;
	public  UInt32  shaderGroupHandleCaptureReplaySize;
	public  UInt32  maxRayDispatchInvocationCount;
	public  UInt32  shaderGroupHandleAlignment;
	public  UInt32  maxRayHitAttributeSize;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkStridedDeviceAddressRegionKHR {
	public  UInt64  deviceAddress;
	public  UInt64  stride;
	public  UInt64  size;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkTraceRaysIndirectCommandKHR {
	public  UInt32  width;
	public  UInt32  height;
	public  UInt32  depth;
}

#endregion
#region VK_KHR_ray_query
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceRayQueryFeaturesKHR {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  rayQuery;
}

#endregion
#region VK_EXT_mesh_shader
[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMeshShaderFeaturesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  taskShader;
	public  UInt32  meshShader;
	public  UInt32  multiviewMeshShader;
	public  UInt32  primitiveFragmentShadingRateMeshShader;
	public  UInt32  meshShaderQueries;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkPhysicalDeviceMeshShaderPropertiesEXT {
	public  VkStructureType  sType;
	public  void*  pNext;
	public  UInt32  maxTaskWorkGroupTotalCount;
	public fixed UInt32  maxTaskWorkGroupCount[3];
	public  UInt32  maxTaskWorkGroupInvocations;
	public fixed UInt32  maxTaskWorkGroupSize[3];
	public  UInt32  maxTaskPayloadSize;
	public  UInt32  maxTaskSharedMemorySize;
	public  UInt32  maxTaskPayloadAndSharedMemorySize;
	public  UInt32  maxMeshWorkGroupTotalCount;
	public fixed UInt32  maxMeshWorkGroupCount[3];
	public  UInt32  maxMeshWorkGroupInvocations;
	public fixed UInt32  maxMeshWorkGroupSize[3];
	public  UInt32  maxMeshSharedMemorySize;
	public  UInt32  maxMeshPayloadAndSharedMemorySize;
	public  UInt32  maxMeshOutputMemorySize;
	public  UInt32  maxMeshPayloadAndOutputMemorySize;
	public  UInt32  maxMeshOutputComponents;
	public  UInt32  maxMeshOutputVertices;
	public  UInt32  maxMeshOutputPrimitives;
	public  UInt32  maxMeshOutputLayers;
	public  UInt32  maxMeshMultiviewViewCount;
	public  UInt32  meshOutputPerVertexGranularity;
	public  UInt32  meshOutputPerPrimitiveGranularity;
	public  UInt32  maxPreferredTaskWorkGroupInvocations;
	public  UInt32  maxPreferredMeshWorkGroupInvocations;
	public  UInt32  prefersLocalInvocationVertexOutput;
	public  UInt32  prefersLocalInvocationPrimitiveOutput;
	public  UInt32  prefersCompactVertexOutput;
	public  UInt32  prefersCompactPrimitiveOutput;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe  struct VkDrawMeshTasksIndirectCommandEXT {
	public  UInt32  groupCountX;
	public  UInt32  groupCountY;
	public  UInt32  groupCountZ;
}

#endregion
#endregion
