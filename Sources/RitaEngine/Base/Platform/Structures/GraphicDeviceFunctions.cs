

namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.Vulkan;
using ConstChar = System.Byte;
using RitaEngine.Base.Platform.Structures;

public unsafe delegate void* PFN_vkGetInstanceProcAddr(VkInstance module , string name);

public unsafe delegate void* PFN_vkGetDeviceProcAddr(VkDevice module , string name);

[SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = BaseHelper.FORCE_ALIGNEMENT)]
public struct GraphicDeviceFunctions : IEquatable<GraphicDeviceFunctions>
{
    private  unsafe static delegate* unmanaged<VkInstance,byte*  , void*> FuncvkGetInstanceProcAddr =null;//)(VkInstance instance, byte* pName);
	private  unsafe static delegate* unmanaged<VkDevice,byte*  , void*> FuncvkGetDeviceProcAddr = null;// typedef vkVoidFunction vkGetDeviceProcAddr)(VkDevice device, byte* pName);
    public unsafe /*readonly*/ delegate* unmanaged< UInt32*,VkLayerProperties*,VkResult > vkEnumerateInstanceLayerProperties = null;
    public unsafe /*readonly*/ delegate* unmanaged< UInt32*,VkResult > vkEnumerateInstanceVersion = null;
    public unsafe /*readonly*/ delegate* unmanaged< char*,UInt32*,VkExtensionProperties*,VkResult > vkEnumerateInstanceExtensionProperties = null;
    public unsafe /*readonly*/ delegate* unmanaged< VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*,VkResult > vkCreateInstance = null;
    public unsafe /*readonly*/ delegate* unmanaged< VkInstance,VkAllocationCallbacks*,void > vkDestroyInstance = null;
    public nint vulkan = nint.Zero;

#region Intance
#region VK_VERSION_1_0
// public unsafe /*readonly*/  delegate* unmanaged< void > vkVoidFunction = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*,VkResult > vkCreateInstance = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkAllocationCallbacks*,void > vkDestroyInstance = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,UInt32*,VkPhysicalDevice*,VkResult > vkEnumeratePhysicalDevices = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceFeatures*,void > vkGetPhysicalDeviceFeatures = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkFormatProperties*,void > vkGetPhysicalDeviceFormatProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkImageType,VkImageTiling,UInt32,UInt32,VkImageFormatProperties*,VkResult > vkGetPhysicalDeviceImageFormatProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceProperties*,void > vkGetPhysicalDeviceProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkQueueFamilyProperties*,void > vkGetPhysicalDeviceQueueFamilyProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceMemoryProperties*,void > vkGetPhysicalDeviceMemoryProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDeviceCreateInfo*,VkAllocationCallbacks*,VkDevice*,VkResult > vkCreateDevice = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAllocationCallbacks*,void > vkDestroyDevice = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,char*,UInt32*,VkExtensionProperties*,VkResult > vkEnumerateDeviceExtensionProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkLayerProperties*,VkResult > vkEnumerateDeviceLayerProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkImageType,VkSampleCountFlagBits,UInt32,VkImageTiling,UInt32*,VkSparseImageFormatProperties*,void > vkGetPhysicalDeviceSparseImageFormatProperties = null;
#endregion
#region VK_VERSION_1_1
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,UInt32*,VkPhysicalDeviceGroupProperties*,VkResult > vkEnumeratePhysicalDeviceGroups = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceFeatures2*,void > vkGetPhysicalDeviceFeatures2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceProperties2*,void > vkGetPhysicalDeviceProperties2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkFormatProperties2*,void > vkGetPhysicalDeviceFormatProperties2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceImageFormatInfo2*,VkImageFormatProperties2*,VkResult > vkGetPhysicalDeviceImageFormatProperties2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkQueueFamilyProperties2*,void > vkGetPhysicalDeviceQueueFamilyProperties2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceMemoryProperties2*,void > vkGetPhysicalDeviceMemoryProperties2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceSparseImageFormatInfo2*,UInt32*,VkSparseImageFormatProperties2*,void > vkGetPhysicalDeviceSparseImageFormatProperties2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceExternalBufferInfo*,VkExternalBufferProperties*,void > vkGetPhysicalDeviceExternalBufferProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceExternalFenceInfo*,VkExternalFenceProperties*,void > vkGetPhysicalDeviceExternalFenceProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceExternalSemaphoreInfo*,VkExternalSemaphoreProperties*,void > vkGetPhysicalDeviceExternalSemaphoreProperties = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetLayoutCreateInfo*,VkDescriptorSetLayoutSupport*,void > vkGetDescriptorSetLayoutSupport = null;
#endregion
#region VK_VERSION_1_2
#endregion
#region VK_VERSION_1_3
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkPhysicalDeviceToolProperties*,VkResult > vkGetPhysicalDeviceToolProperties = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceMemoryProperties*,void > vkGetPhysicalDeviceMemoryProperties = null;
#endregion
#region VK_KHR_surface
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkSurfaceKHR,VkAllocationCallbacks*,void > vkDestroySurfaceKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32,VkSurfaceKHR,UInt32*,VkResult > vkGetPhysicalDeviceSurfaceSupportKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,VkSurfaceCapabilitiesKHR*,VkResult > vkGetPhysicalDeviceSurfaceCapabilitiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkSurfaceFormatKHR*,VkResult > vkGetPhysicalDeviceSurfaceFormatsKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkPresentModeKHR*,VkResult > vkGetPhysicalDeviceSurfacePresentModesKHR = null;
#endregion
#region VK_KHR_swapchain
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult > vkCreateSwapchainKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,VkAllocationCallbacks*,void > vkDestroySwapchainKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt32*,VkImage*,VkResult > vkGetSwapchainImagesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt64,VkSemaphore,VkFence,UInt32*,VkResult > vkAcquireNextImageKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,VkPresentInfoKHR*,VkResult > vkQueuePresentKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceGroupPresentCapabilitiesKHR*,VkResult > vkGetDeviceGroupPresentCapabilitiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSurfaceKHR,UInt32*,VkResult > vkGetDeviceGroupSurfacePresentModesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkRect2D*,VkResult > vkGetPhysicalDevicePresentRectanglesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAcquireNextImageInfoKHR*,UInt32*,VkResult > vkAcquireNextImage2KHR = null;
#endregion
#region VK_KHR_display
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkDisplayPropertiesKHR*,VkResult > vkGetPhysicalDeviceDisplayPropertiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkDisplayPlanePropertiesKHR*,VkResult > vkGetPhysicalDeviceDisplayPlanePropertiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32,UInt32*,VkDisplayKHR*,VkResult > vkGetDisplayPlaneSupportedDisplaysKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayKHR,UInt32*,VkDisplayModePropertiesKHR*,VkResult > vkGetDisplayModePropertiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayKHR,VkDisplayModeCreateInfoKHR*,VkAllocationCallbacks*,VkDisplayModeKHR*,VkResult > vkCreateDisplayModeKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayModeKHR,UInt32,VkDisplayPlaneCapabilitiesKHR*,VkResult > vkGetDisplayPlaneCapabilitiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkDisplaySurfaceCreateInfoKHR*,VkAllocationCallbacks*,VkSurfaceKHR*,VkResult > vkCreateDisplayPlaneSurfaceKHR = null;
#endregion
#region VK_KHR_display_swapchain
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult > vkCreateSharedSwapchainsKHR = null;
#endregion

#region VK_KHR_get_physical_device_properties2
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceFeatures2*,void > vkGetPhysicalDeviceFeatures2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceProperties2*,void > vkGetPhysicalDeviceProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkFormatProperties2*,void > vkGetPhysicalDeviceFormatProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceImageFormatInfo2*,VkImageFormatProperties2*,VkResult > vkGetPhysicalDeviceImageFormatProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkQueueFamilyProperties2*,void > vkGetPhysicalDeviceQueueFamilyProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceMemoryProperties2*,void > vkGetPhysicalDeviceMemoryProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceSparseImageFormatInfo2*,UInt32*,VkSparseImageFormatProperties2*,void > vkGetPhysicalDeviceSparseImageFormatProperties2KHR = null;
#endregion

#region VK_KHR_device_group_creation
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,UInt32*,VkPhysicalDeviceGroupProperties*,VkResult > vkEnumeratePhysicalDeviceGroupsKHR = null;
#endregion
#region VK_KHR_external_memory_capabilities
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceExternalBufferInfo*,VkExternalBufferProperties*,void > vkGetPhysicalDeviceExternalBufferPropertiesKHR = null;
#endregion
#region VK_KHR_external_memory_fd
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMemoryGetFdInfoKHR*,int*,VkResult > vkGetMemoryFdKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkExternalMemoryHandleTypeFlagBits,int,VkMemoryFdPropertiesKHR*,VkResult > vkGetMemoryFdPropertiesKHR = null;
#endregion
#region VK_KHR_external_semaphore_capabilities
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceExternalSemaphoreInfo*,VkExternalSemaphoreProperties*,void > vkGetPhysicalDeviceExternalSemaphorePropertiesKHR = null;
#endregion

#region VK_KHR_external_fence_capabilities
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceExternalFenceInfo*,VkExternalFenceProperties*,void > vkGetPhysicalDeviceExternalFencePropertiesKHR = null;
#endregion
#region VK_KHR_external_fence_fd
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImportFenceFdInfoKHR*,VkResult > vkImportFenceFdKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFenceGetFdInfoKHR*,int*,VkResult > vkGetFenceFdKHR = null;
#endregion
#region VK_KHR_performance_query
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32,UInt32*,VkPerformanceCounterKHR*,VkPerformanceCounterDescriptionKHR*,VkResult > vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkQueryPoolPerformanceCreateInfoKHR*,UInt32*,void > vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAcquireProfilingLockInfoKHR*,VkResult > vkAcquireProfilingLockKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,void > vkReleaseProfilingLockKHR = null;
#endregion
#region VK_KHR_get_surface_capabilities2
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceSurfaceInfo2KHR*,VkSurfaceCapabilities2KHR*,VkResult > vkGetPhysicalDeviceSurfaceCapabilities2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkPhysicalDeviceSurfaceInfo2KHR*,UInt32*,VkSurfaceFormat2KHR*,VkResult > vkGetPhysicalDeviceSurfaceFormats2KHR = null;
#endregion
#region VK_KHR_get_display_properties2
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkDisplayProperties2KHR*,VkResult > vkGetPhysicalDeviceDisplayProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkDisplayPlaneProperties2KHR*,VkResult > vkGetPhysicalDeviceDisplayPlaneProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayKHR,UInt32*,VkDisplayModeProperties2KHR*,VkResult > vkGetDisplayModeProperties2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayPlaneInfo2KHR*,VkDisplayPlaneCapabilities2KHR*,VkResult > vkGetDisplayPlaneCapabilities2KHR = null;
#endregion

#region VK_KHR_fragment_shading_rate
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkPhysicalDeviceFragmentShadingRateKHR*,VkResult > vkGetPhysicalDeviceFragmentShadingRatesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkExtent2D*,VkFragmentShadingRateCombinerOpKHR,void > vkCmdSetFragmentShadingRateKHR = null;
#endregion
#region VK_KHR_present_wait
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt64,UInt64,VkResult > vkWaitForPresentKHR = null;
#endregion

#region VK_EXT_debug_report
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkDebugReportCallbackCreateInfoEXT*,VkAllocationCallbacks*,VkDebugReportCallbackEXT*,VkResult > vkCreateDebugReportCallbackEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkDebugReportCallbackEXT,VkAllocationCallbacks*,void > vkDestroyDebugReportCallbackEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,UInt32,VkDebugReportObjectTypeEXT,UInt64,Int32,Int32,ConstChar*,ConstChar*,void > vkDebugReportMessageEXT = null;
#endregion
#region VK_EXT_debug_marker
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDebugMarkerObjectTagInfoEXT*,VkResult > vkDebugMarkerSetObjectTagEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDebugMarkerObjectNameInfoEXT*,VkResult > vkDebugMarkerSetObjectNameEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDebugMarkerMarkerInfoEXT*,void > vkCmdDebugMarkerBeginEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdDebugMarkerEndEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDebugMarkerMarkerInfoEXT*,void > vkCmdDebugMarkerInsertEXT = null;
#endregion

#region VK_NV_external_memory_capabilities
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkFormat,VkImageType,VkImageTiling,UInt32,UInt32,UInt32,VkExternalImageFormatPropertiesNV*,VkResult > vkGetPhysicalDeviceExternalImageFormatPropertiesNV = null;
#endregion

#region VK_EXT_direct_mode_display
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayKHR,VkResult > vkReleaseDisplayEXT = null;
#endregion
#region VK_EXT_display_surface_counter
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkSurfaceKHR,VkSurfaceCapabilities2EXT*,VkResult > vkGetPhysicalDeviceSurfaceCapabilities2EXT = null;
#endregion


#region VK_EXT_debug_utils
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDebugUtilsObjectNameInfoEXT*,VkResult > vkSetDebugUtilsObjectNameEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDebugUtilsObjectTagInfoEXT*,VkResult > vkSetDebugUtilsObjectTagEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,VkDebugUtilsLabelEXT*,void > vkQueueBeginDebugUtilsLabelEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,void > vkQueueEndDebugUtilsLabelEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,VkDebugUtilsLabelEXT*,void > vkQueueInsertDebugUtilsLabelEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDebugUtilsLabelEXT*,void > vkCmdBeginDebugUtilsLabelEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdEndDebugUtilsLabelEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDebugUtilsLabelEXT*,void > vkCmdInsertDebugUtilsLabelEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkDebugUtilsMessengerCreateInfoEXT*,VkAllocationCallbacks*,VkDebugUtilsMessengerEXT*,VkResult > vkCreateDebugUtilsMessengerEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkDebugUtilsMessengerEXT,VkAllocationCallbacks*,void > vkDestroyDebugUtilsMessengerEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkDebugUtilsMessageSeverityFlagBitsEXT,UInt32,VkDebugUtilsMessengerCallbackDataEXT*,void > vkSubmitDebugUtilsMessageEXT = null;
#endregion
#region VK_EXT_sample_locations
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSampleLocationsInfoEXT*,void > vkCmdSetSampleLocationsEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkSampleCountFlagBits,VkMultisamplePropertiesEXT*,void > vkGetPhysicalDeviceMultisamplePropertiesEXT = null;
#endregion


#region VK_NV_shading_rate_image
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImageView,VkImageLayout,void > vkCmdBindShadingRateImageNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkShadingRatePaletteNV*,void > vkCmdSetViewportShadingRatePaletteNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCoarseSampleOrderTypeNV,UInt32,VkCoarseSampleOrderCustomNV*,void > vkCmdSetCoarseSampleOrderNV = null;
#endregion



#region VK_EXT_calibrated_timestamps
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkTimeDomainEXT*,VkResult > vkGetPhysicalDeviceCalibrateableTimeDomainsEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkCalibratedTimestampInfoEXT*,UInt64*,UInt64*,VkResult > vkGetCalibratedTimestampsEXT = null;
#endregion
#region VK_NV_mesh_shader
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,void > vkCmdDrawMeshTasksNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawMeshTasksIndirectNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawMeshTasksIndirectCountNV = null;
#endregion
#region VK_NV_scissor_exclusive
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkRect2D*,void > vkCmdSetExclusiveScissorNV = null;
#endregion
#region VK_NV_device_diagnostic_checkpoints
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void*,void > vkCmdSetCheckpointNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,UInt32*,VkCheckpointDataNV*,void > vkGetQueueCheckpointDataNV = null;
#endregion
#region VK_INTEL_performance_query
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkInitializePerformanceApiInfoINTEL*,VkResult > vkInitializePerformanceApiINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,void > vkUninitializePerformanceApiINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPerformanceMarkerInfoINTEL*,VkResult > vkCmdSetPerformanceMarkerINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPerformanceStreamMarkerInfoINTEL*,VkResult > vkCmdSetPerformanceStreamMarkerINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPerformanceOverrideInfoINTEL*,VkResult > vkCmdSetPerformanceOverrideINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPerformanceConfigurationAcquireInfoINTEL*,VkPerformanceConfigurationINTEL*,VkResult > vkAcquirePerformanceConfigurationINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPerformanceConfigurationINTEL,VkResult > vkReleasePerformanceConfigurationINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,VkPerformanceConfigurationINTEL,VkResult > vkQueueSetPerformanceConfigurationINTEL = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPerformanceParameterTypeINTEL,VkPerformanceValueINTEL*,VkResult > vkGetPerformanceParameterINTEL = null;
#endregion

#region VK_EXT_tooling_info
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkPhysicalDeviceToolProperties*,VkResult > vkGetPhysicalDeviceToolPropertiesEXT = null;
#endregion
#region VK_NV_cooperative_matrix
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkCooperativeMatrixPropertiesNV*,VkResult > vkGetPhysicalDeviceCooperativeMatrixPropertiesNV = null;
#endregion
#region VK_NV_coverage_reduction_mode
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32*,VkFramebufferMixedSamplesCombinationNV*,VkResult > vkGetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV = null;
#endregion
#region VK_EXT_headless_surface
public unsafe /*readonly*/  delegate* unmanaged< VkInstance,VkHeadlessSurfaceCreateInfoEXT*,VkAllocationCallbacks*,VkSurfaceKHR*,VkResult > vkCreateHeadlessSurfaceEXT = null;
#endregion
#region VK_EXT_line_rasterization
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt16,void > vkCmdSetLineStippleEXT = null;
#endregion


#region VK_NV_device_generated_commands
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkGeneratedCommandsMemoryRequirementsInfoNV*,VkMemoryRequirements2*,void > vkGetGeneratedCommandsMemoryRequirementsNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkGeneratedCommandsInfoNV*,void > vkCmdPreprocessGeneratedCommandsNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkGeneratedCommandsInfoNV*,void > vkCmdExecuteGeneratedCommandsNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineBindPoint,VkPipeline,UInt32,void > vkCmdBindPipelineShaderGroupNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkIndirectCommandsLayoutCreateInfoNV*,VkAllocationCallbacks*,VkIndirectCommandsLayoutNV*,VkResult > vkCreateIndirectCommandsLayoutNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkIndirectCommandsLayoutNV,VkAllocationCallbacks*,void > vkDestroyIndirectCommandsLayoutNV = null;
#endregion
#region VK_EXT_acquire_drm_display
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,Int32,VkDisplayKHR,VkResult > vkAcquireDrmDisplayEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,Int32,UInt32,VkDisplayKHR*,VkResult > vkGetDrmDisplayEXT = null;
#endregion

#region VK_NV_fragment_shading_rate_enums
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkFragmentShadingRateNV,VkFragmentShadingRateCombinerOpKHR,void > vkCmdSetFragmentShadingRateEnumNV = null;
#endregion
#region VK_EXT_image_compression_control
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,VkImageSubresource2EXT*,VkSubresourceLayout2EXT*,void > vkGetImageSubresourceLayout2EXT = null;
#endregion
#region VK_EXT_device_fault
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceFaultCountsEXT*,VkDeviceFaultInfoEXT*,VkResult > vkGetDeviceFaultInfoEXT = null;
#endregion
#region VK_NV_acquire_winrt_display
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkDisplayKHR,VkResult > vkAcquireWinrtDisplayNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,UInt32,VkDisplayKHR*,VkResult > vkGetWinrtDisplayNV = null;
#endregion

#endregion

#region Callbackss
#region VK_VERSION_1_0
public unsafe /*readonly*/  delegate* unmanaged< void*,Int32,Int32,VkSystemAllocationScope,void* > vkAllocationFunction = null;
public unsafe /*readonly*/  delegate* unmanaged< void*,void*,void > vkFreeFunction = null;
public unsafe /*readonly*/  delegate* unmanaged< void*,Int32,VkInternalAllocationType,VkSystemAllocationScope,void > vkInternalAllocationNotification = null;
public unsafe /*readonly*/  delegate* unmanaged< void*,Int32,VkInternalAllocationType,VkSystemAllocationScope,void > vkInternalFreeNotification = null;
public unsafe /*readonly*/  delegate* unmanaged< void*,void*,Int32,Int32,VkSystemAllocationScope,void* > vkReallocationFunction = null;
#endregion
#region VK_EXT_debug_report
public unsafe /*readonly*/  delegate* unmanaged< UInt32,VkDebugReportObjectTypeEXT,UInt64,Int32,Int32,char*,char*,void*,UInt32 > vkDebugReportCallbackEXT = null;
#endregion
#region VK_EXT_debug_utils
public unsafe /*readonly*/  delegate* unmanaged< VkDebugUtilsMessageSeverityFlagBitsEXT,UInt32,VkDebugUtilsMessengerCallbackDataEXT*,void*,UInt32 > vkDebugUtilsMessengerCallbackEXT = null;
#endregion
#region VK_EXT_device_memory_report
public unsafe /*readonly*/  delegate* unmanaged< VkDeviceMemoryReportCallbackDataEXT*,void*,void > vkDeviceMemoryReportCallbackEXT = null;
#endregion
#endregion

#region WINDOWS
public unsafe /*readonly*/  delegate* unmanaged< VkInstance, VkWin32SurfaceCreateInfoKHR*, VkAllocationCallbacks*, VkSurfaceKHR*,VkResult > vkCreateWin32SurfaceKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice, UInt32 ,UInt32 > vkGetPhysicalDeviceWin32PresentationSupportKHR = null;

// typedef VkResult (VKAPI_PTR *PFN_vkGetPhysicalDeviceSurfacePresentModes2EXT)(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceSurfaceInfo2KHR* pSurfaceInfo, uint32_t* pPresentModeCount, VkPresentModeKHR* pPresentModes);
// typedef VkResult (VKAPI_PTR *PFN_vkAcquireWinrtDisplayNV)(VkPhysicalDevice physicalDevice, VkDisplayKHR display);
// typedef VkResult (VKAPI_PTR *PFN_vkGetWinrtDisplayNV)(VkPhysicalDevice physicalDevice, uint32_t deviceRelativeId, VkDisplayKHR* pDisplay);


#endregion

#region Intance
#region VK_VERSION_1_0
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,UInt32,VkQueue*,void > vkGetDeviceQueue = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,UInt32,VkSubmitInfo*,VkFence,VkResult > vkQueueSubmit = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,VkResult > vkQueueWaitIdle = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkResult > vkDeviceWaitIdle = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMemoryAllocateInfo*,VkAllocationCallbacks*,VkDeviceMemory*,VkResult > vkAllocateMemory = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemory,VkAllocationCallbacks*,void > vkFreeMemory = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemory,UInt64,UInt64,UInt32,void**,VkResult > vkMapMemory = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemory,void > vkUnmapMemory = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkMappedMemoryRange*,VkResult > vkFlushMappedMemoryRanges = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkMappedMemoryRange*,VkResult > vkInvalidateMappedMemoryRanges = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemory,UInt64*,void > vkGetDeviceMemoryCommitment = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBuffer,VkDeviceMemory,UInt64,VkResult > vkBindBufferMemory = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,VkDeviceMemory,UInt64,VkResult > vkBindImageMemory = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBuffer,VkMemoryRequirements*,void > vkGetBufferMemoryRequirements = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,VkMemoryRequirements*,void > vkGetImageMemoryRequirements = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,UInt32*,VkSparseImageMemoryRequirements*,void > vkGetImageSparseMemoryRequirements = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,UInt32,VkBindSparseInfo*,VkFence,VkResult > vkQueueBindSparse = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFenceCreateInfo*,VkAllocationCallbacks*,VkFence*,VkResult > vkCreateFence = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFence,VkAllocationCallbacks*,void > vkDestroyFence = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkFence*,VkResult > vkResetFences = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFence,VkResult > vkGetFenceStatus = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkFence*,UInt32,UInt64,VkResult > vkWaitForFences = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphoreCreateInfo*,VkAllocationCallbacks*,VkSemaphore*,VkResult > vkCreateSemaphore = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphore,VkAllocationCallbacks*,void > vkDestroySemaphore = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkEventCreateInfo*,VkAllocationCallbacks*,VkEvent*,VkResult > vkCreateEvent = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkEvent,VkAllocationCallbacks*,void > vkDestroyEvent = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkEvent,VkResult > vkGetEventStatus = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkEvent,VkResult > vkSetEvent = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkEvent,VkResult > vkResetEvent = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkQueryPoolCreateInfo*,VkAllocationCallbacks*,VkQueryPool*,VkResult > vkCreateQueryPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkQueryPool,VkAllocationCallbacks*,void > vkDestroyQueryPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkQueryPool,UInt32,UInt32,Int32,void*,UInt64,UInt32,VkResult > vkGetQueryPoolResults = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferCreateInfo*,VkAllocationCallbacks*,VkBuffer*,VkResult > vkCreateBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBuffer,VkAllocationCallbacks*,void > vkDestroyBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferViewCreateInfo*,VkAllocationCallbacks*,VkBufferView*,VkResult > vkCreateBufferView = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferView,VkAllocationCallbacks*,void > vkDestroyBufferView = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageCreateInfo*,VkAllocationCallbacks*,VkImage*,VkResult > vkCreateImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,VkAllocationCallbacks*,void > vkDestroyImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,VkImageSubresource*,VkSubresourceLayout*,void > vkGetImageSubresourceLayout = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageViewCreateInfo*,VkAllocationCallbacks*,VkImageView*,VkResult > vkCreateImageView = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageView,VkAllocationCallbacks*,void > vkDestroyImageView = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkShaderModuleCreateInfo*,VkAllocationCallbacks*,VkShaderModule*,VkResult > vkCreateShaderModule = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkShaderModule,VkAllocationCallbacks*,void > vkDestroyShaderModule = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCacheCreateInfo*,VkAllocationCallbacks*,VkPipelineCache*,VkResult > vkCreatePipelineCache = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCache,VkAllocationCallbacks*,void > vkDestroyPipelineCache = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCache,Int32*,void*,VkResult > vkGetPipelineCacheData = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCache,UInt32,VkPipelineCache*,VkResult > vkMergePipelineCaches = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCache,UInt32,VkGraphicsPipelineCreateInfo*,VkAllocationCallbacks*,VkPipeline*,VkResult > vkCreateGraphicsPipelines = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCache,UInt32,VkComputePipelineCreateInfo*,VkAllocationCallbacks*,VkPipeline*,VkResult > vkCreateComputePipelines = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,VkAllocationCallbacks*,void > vkDestroyPipeline = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineLayoutCreateInfo*,VkAllocationCallbacks*,VkPipelineLayout*,VkResult > vkCreatePipelineLayout = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineLayout,VkAllocationCallbacks*,void > vkDestroyPipelineLayout = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSamplerCreateInfo*,VkAllocationCallbacks*,VkSampler*,VkResult > vkCreateSampler = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSampler,VkAllocationCallbacks*,void > vkDestroySampler = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetLayoutCreateInfo*,VkAllocationCallbacks*,VkDescriptorSetLayout*,VkResult > vkCreateDescriptorSetLayout = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetLayout,VkAllocationCallbacks*,void > vkDestroyDescriptorSetLayout = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorPoolCreateInfo*,VkAllocationCallbacks*,VkDescriptorPool*,VkResult > vkCreateDescriptorPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorPool,VkAllocationCallbacks*,void > vkDestroyDescriptorPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorPool,UInt32,VkResult > vkResetDescriptorPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetAllocateInfo*,VkDescriptorSet*,VkResult > vkAllocateDescriptorSets = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorPool,UInt32,VkDescriptorSet*,VkResult > vkFreeDescriptorSets = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkWriteDescriptorSet*,UInt32,VkCopyDescriptorSet*,void > vkUpdateDescriptorSets = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFramebufferCreateInfo*,VkAllocationCallbacks*,VkFramebuffer*,VkResult > vkCreateFramebuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFramebuffer,VkAllocationCallbacks*,void > vkDestroyFramebuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderPassCreateInfo*,VkAllocationCallbacks*,VkRenderPass*,VkResult > vkCreateRenderPass = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderPass,VkAllocationCallbacks*,void > vkDestroyRenderPass = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderPass,VkExtent2D*,void > vkGetRenderAreaGranularity = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandPoolCreateInfo*,VkAllocationCallbacks*,VkCommandPool*,VkResult > vkCreateCommandPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandPool,VkAllocationCallbacks*,void > vkDestroyCommandPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandPool,UInt32,VkResult > vkResetCommandPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandBufferAllocateInfo*,VkCommandBuffer*,VkResult > vkAllocateCommandBuffers = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandPool,UInt32,VkCommandBuffer*,void > vkFreeCommandBuffers = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCommandBufferBeginInfo*,VkResult > vkBeginCommandBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkResult > vkEndCommandBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkResult > vkResetCommandBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineBindPoint,VkPipeline,void > vkCmdBindPipeline = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkViewport*,void > vkCmdSetViewport = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkRect2D*,void > vkCmdSetScissor = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,float,void > vkCmdSetLineWidth = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,float,float,float,void > vkCmdSetDepthBias = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,float,void > vkCmdSetBlendConstants = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,float,float,void > vkCmdSetDepthBounds = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,void > vkCmdSetStencilCompareMask = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,void > vkCmdSetStencilWriteMask = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,void > vkCmdSetStencilReference = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineBindPoint,VkPipelineLayout,UInt32,UInt32,VkDescriptorSet*,UInt32,UInt32*,void > vkCmdBindDescriptorSets = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkIndexType,void > vkCmdBindIndexBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,void > vkCmdBindVertexBuffers = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,void > vkCmdDraw = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,Int32,UInt32,void > vkCmdDrawIndexed = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndirect = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndexedIndirect = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,void > vkCmdDispatch = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,void > vkCmdDispatchIndirect = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,VkBuffer,UInt32,VkBufferCopy*,void > vkCmdCopyBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImage,VkImageLayout,VkImage,VkImageLayout,UInt32,VkImageCopy*,void > vkCmdCopyImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImage,VkImageLayout,VkImage,VkImageLayout,UInt32,VkImageBlit*,VkFilter,void > vkCmdBlitImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,VkImage,VkImageLayout,UInt32,VkBufferImageCopy*,void > vkCmdCopyBufferToImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImage,VkImageLayout,VkBuffer,UInt32,VkBufferImageCopy*,void > vkCmdCopyImageToBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,UInt64,void*,void > vkCmdUpdateBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,UInt64,UInt32,void > vkCmdFillBuffer = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImage,VkImageLayout,VkClearColorValue*,UInt32,VkImageSubresourceRange*,void > vkCmdClearColorImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImage,VkImageLayout,VkClearDepthStencilValue*,UInt32,VkImageSubresourceRange*,void > vkCmdClearDepthStencilImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkClearAttachment*,UInt32,VkClearRect*,void > vkCmdClearAttachments = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImage,VkImageLayout,VkImage,VkImageLayout,UInt32,VkImageResolve*,void > vkCmdResolveImage = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkEvent,UInt32,void > vkCmdSetEvent = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkEvent,UInt32,void > vkCmdResetEvent = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkEvent*,UInt32,UInt32,UInt32,VkMemoryBarrier*,UInt32,VkBufferMemoryBarrier*,UInt32,VkImageMemoryBarrier*,void > vkCmdWaitEvents = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,VkMemoryBarrier*,UInt32,VkBufferMemoryBarrier*,UInt32,VkImageMemoryBarrier*,void > vkCmdPipelineBarrier = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkQueryPool,UInt32,UInt32,void > vkCmdBeginQuery = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkQueryPool,UInt32,void > vkCmdEndQuery = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkQueryPool,UInt32,UInt32,void > vkCmdResetQueryPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineStageFlagBits,VkQueryPool,UInt32,void > vkCmdWriteTimestamp = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkQueryPool,UInt32,UInt32,VkBuffer,UInt64,UInt64,UInt32,void > vkCmdCopyQueryPoolResults = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineLayout,UInt32,UInt32,UInt32,void*,void > vkCmdPushConstants = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkRenderPassBeginInfo*,VkSubpassContents,void > vkCmdBeginRenderPass = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSubpassContents,void > vkCmdNextSubpass = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdEndRenderPass = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkCommandBuffer*,void > vkCmdExecuteCommands = null;


#endregion
#region VK_VERSION_1_1

public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkBindBufferMemoryInfo*,VkResult > vkBindBufferMemory2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkBindImageMemoryInfo*,VkResult > vkBindImageMemory2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,UInt32,UInt32,UInt32*,void > vkGetDeviceGroupPeerMemoryFeatures = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDeviceMask = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,UInt32,UInt32,void > vkCmdDispatchBase = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageMemoryRequirementsInfo2*,VkMemoryRequirements2*,void > vkGetImageMemoryRequirements2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferMemoryRequirementsInfo2*,VkMemoryRequirements2*,void > vkGetBufferMemoryRequirements2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageSparseMemoryRequirementsInfo2*,UInt32*,VkSparseImageMemoryRequirements2*,void > vkGetImageSparseMemoryRequirements2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandPool,UInt32,void > vkTrimCommandPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceQueueInfo2*,VkQueue*,void > vkGetDeviceQueue2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSamplerYcbcrConversionCreateInfo*,VkAllocationCallbacks*,VkSamplerYcbcrConversion*,VkResult > vkCreateSamplerYcbcrConversion = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSamplerYcbcrConversion,VkAllocationCallbacks*,void > vkDestroySamplerYcbcrConversion = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorUpdateTemplateCreateInfo*,VkAllocationCallbacks*,VkDescriptorUpdateTemplate*,VkResult > vkCreateDescriptorUpdateTemplate = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorUpdateTemplate,VkAllocationCallbacks*,void > vkDestroyDescriptorUpdateTemplate = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSet,VkDescriptorUpdateTemplate,void*,void > vkUpdateDescriptorSetWithTemplate = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetLayoutCreateInfo*,VkDescriptorSetLayoutSupport*,void > vkGetDescriptorSetLayoutSupport = null;
#endregion
#region VK_VERSION_1_2
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndirectCount = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndexedIndirectCount = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderPassCreateInfo2*,VkAllocationCallbacks*,VkRenderPass*,VkResult > vkCreateRenderPass2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkRenderPassBeginInfo*,VkSubpassBeginInfo*,void > vkCmdBeginRenderPass2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSubpassBeginInfo*,VkSubpassEndInfo*,void > vkCmdNextSubpass2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSubpassEndInfo*,void > vkCmdEndRenderPass2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkQueryPool,UInt32,UInt32,void > vkResetQueryPool = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphore,UInt64*,VkResult > vkGetSemaphoreCounterValue = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphoreWaitInfo*,UInt64,VkResult > vkWaitSemaphores = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphoreSignalInfo*,VkResult > vkSignalSemaphore = null;

// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferDeviceAddressInfo*,UInt64 > vkGetBufferDeviceAddress = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferDeviceAddressInfo*,UInt64 > vkGetBufferOpaqueCaptureAddress = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemoryOpaqueCaptureAddressInfo*,UInt64 > vkGetDeviceMemoryOpaqueCaptureAddress = null;
#endregion
#region VK_VERSION_1_3

public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPrivateDataSlotCreateInfo*,VkAllocationCallbacks*,VkPrivateDataSlot*,VkResult > vkCreatePrivateDataSlot = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPrivateDataSlot,VkAllocationCallbacks*,void > vkDestroyPrivateDataSlot = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64,VkResult > vkSetPrivateData = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64*,void > vkGetPrivateData = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkEvent,VkDependencyInfo*,void > vkCmdSetEvent2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkEvent,UInt64,void > vkCmdResetEvent2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkEvent*,VkDependencyInfo*,void > vkCmdWaitEvents2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDependencyInfo*,void > vkCmdPipelineBarrier2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,VkQueryPool,UInt32,void > vkCmdWriteTimestamp2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,UInt32,VkSubmitInfo2*,VkFence,VkResult > vkQueueSubmit2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyBufferInfo2*,void > vkCmdCopyBuffer2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyImageInfo2*,void > vkCmdCopyImage2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyBufferToImageInfo2*,void > vkCmdCopyBufferToImage2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyImageToBufferInfo2*,void > vkCmdCopyImageToBuffer2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBlitImageInfo2*,void > vkCmdBlitImage2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkResolveImageInfo2*,void > vkCmdResolveImage2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkRenderingInfo*,void > vkCmdBeginRendering = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdEndRendering = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetCullMode = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkFrontFace,void > vkCmdSetFrontFace = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPrimitiveTopology,void > vkCmdSetPrimitiveTopology = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkViewport*,void > vkCmdSetViewportWithCount = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkRect2D*,void > vkCmdSetScissorWithCount = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,UInt64*,UInt64*,void > vkCmdBindVertexBuffers2 = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthTestEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthWriteEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCompareOp,void > vkCmdSetDepthCompareOp = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthBoundsTestEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetStencilTestEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkStencilOp,VkStencilOp,VkStencilOp,VkCompareOp,void > vkCmdSetStencilOp = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetRasterizerDiscardEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthBiasEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetPrimitiveRestartEnable = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceBufferMemoryRequirements*,VkMemoryRequirements2*,void > vkGetDeviceBufferMemoryRequirements = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceImageMemoryRequirements*,VkMemoryRequirements2*,void > vkGetDeviceImageMemoryRequirements = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceImageMemoryRequirements*,UInt32*,VkSparseImageMemoryRequirements2*,void > vkGetDeviceImageSparseMemoryRequirements = null;
#endregion

#region VK_KHR_swapchain
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult > vkCreateSwapchainKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,VkAllocationCallbacks*,void > vkDestroySwapchainKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt32*,VkImage*,VkResult > vkGetSwapchainImagesKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt64,VkSemaphore,VkFence,UInt32*,VkResult > vkAcquireNextImageKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkQueue,VkPresentInfoKHR*,VkResult > vkQueuePresentKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceGroupPresentCapabilitiesKHR*,VkResult > vkGetDeviceGroupPresentCapabilitiesKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSurfaceKHR,UInt32*,VkResult > vkGetDeviceGroupSurfacePresentModesKHR = null;

// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAcquireNextImageInfoKHR*,UInt32*,VkResult > vkAcquireNextImage2KHR = null;
#endregion

#region VK_KHR_display_swapchain
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult > vkCreateSharedSwapchainsKHR = null;
#endregion
#region VK_KHR_dynamic_rendering
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkRenderingInfo*,void > vkCmdBeginRenderingKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdEndRenderingKHR = null;
#endregion

#region VK_KHR_device_group
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,UInt32,UInt32,UInt32*,void > vkGetDeviceGroupPeerMemoryFeaturesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDeviceMaskKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,UInt32,UInt32,void > vkCmdDispatchBaseKHR = null;
#endregion
#region VK_KHR_maintenance1
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCommandPool,UInt32,void > vkTrimCommandPoolKHR = null;
#endregion
#region VK_KHR_external_memory_fd
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMemoryGetFdInfoKHR*,int*,VkResult > vkGetMemoryFdKHR = null;
// public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkExternalMemoryHandleTypeFlagBits,int,VkMemoryFdPropertiesKHR*,VkResult > vkGetMemoryFdPropertiesKHR = null;
#endregion
#region VK_KHR_external_semaphore_fd
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImportSemaphoreFdInfoKHR*,VkResult > vkImportSemaphoreFdKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphoreGetFdInfoKHR*,int*,VkResult > vkGetSemaphoreFdKHR = null;
#endregion
#region VK_KHR_push_descriptor
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineBindPoint,VkPipelineLayout,UInt32,UInt32,VkWriteDescriptorSet*,void > vkCmdPushDescriptorSetKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDescriptorUpdateTemplate,VkPipelineLayout,UInt32,void*,void > vkCmdPushDescriptorSetWithTemplateKHR = null;
#endregion
#region VK_KHR_descriptor_update_template
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorUpdateTemplateCreateInfo*,VkAllocationCallbacks*,VkDescriptorUpdateTemplate*,VkResult > vkCreateDescriptorUpdateTemplateKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorUpdateTemplate,VkAllocationCallbacks*,void > vkDestroyDescriptorUpdateTemplateKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSet,VkDescriptorUpdateTemplate,void*,void > vkUpdateDescriptorSetWithTemplateKHR = null;
#endregion
#region VK_KHR_create_renderpass2
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderPassCreateInfo2*,VkAllocationCallbacks*,VkRenderPass*,VkResult > vkCreateRenderPass2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkRenderPassBeginInfo*,VkSubpassBeginInfo*,void > vkCmdBeginRenderPass2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSubpassBeginInfo*,VkSubpassEndInfo*,void > vkCmdNextSubpass2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSubpassEndInfo*,void > vkCmdEndRenderPass2KHR = null;
#endregion
#region VK_KHR_shared_presentable_image
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,VkResult > vkGetSwapchainStatusKHR = null;
#endregion


#region VK_KHR_get_memory_requirements2
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageMemoryRequirementsInfo2*,VkMemoryRequirements2*,void > vkGetImageMemoryRequirements2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferMemoryRequirementsInfo2*,VkMemoryRequirements2*,void > vkGetBufferMemoryRequirements2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageSparseMemoryRequirementsInfo2*,UInt32*,VkSparseImageMemoryRequirements2*,void > vkGetImageSparseMemoryRequirements2KHR = null;
#endregion
#region VK_KHR_sampler_ycbcr_conversion
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSamplerYcbcrConversionCreateInfo*,VkAllocationCallbacks*,VkSamplerYcbcrConversion*,VkResult > vkCreateSamplerYcbcrConversionKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSamplerYcbcrConversion,VkAllocationCallbacks*,void > vkDestroySamplerYcbcrConversionKHR = null;
#endregion
#region VK_KHR_bind_memory2
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkBindBufferMemoryInfo*,VkResult > vkBindBufferMemory2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkBindImageMemoryInfo*,VkResult > vkBindImageMemory2KHR = null;
#endregion
#region VK_KHR_maintenance3
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetLayoutCreateInfo*,VkDescriptorSetLayoutSupport*,void > vkGetDescriptorSetLayoutSupportKHR = null;
#endregion
#region VK_KHR_draw_indirect_count
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndirectCountKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndexedIndirectCountKHR = null;
#endregion
#region VK_KHR_timeline_semaphore
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphore,UInt64*,VkResult > vkGetSemaphoreCounterValueKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphoreWaitInfo*,UInt64,VkResult > vkWaitSemaphoresKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSemaphoreSignalInfo*,VkResult > vkSignalSemaphoreKHR = null;
#endregion

#region VK_KHR_buffer_device_address
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferDeviceAddressInfo*,UInt64 > vkGetBufferDeviceAddressKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferDeviceAddressInfo*,UInt64 > vkGetBufferOpaqueCaptureAddressKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemoryOpaqueCaptureAddressInfo*,UInt64 > vkGetDeviceMemoryOpaqueCaptureAddressKHR = null;
#endregion
#region VK_KHR_deferred_host_operations
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAllocationCallbacks*,VkDeferredOperationKHR*,VkResult > vkCreateDeferredOperationKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkAllocationCallbacks*,void > vkDestroyDeferredOperationKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,UInt32 > vkGetDeferredOperationMaxConcurrencyKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkResult > vkGetDeferredOperationResultKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkResult > vkDeferredOperationJoinKHR = null;
#endregion
#region VK_KHR_pipeline_executable_properties
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineInfoKHR*,UInt32*,VkPipelineExecutablePropertiesKHR*,VkResult > vkGetPipelineExecutablePropertiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineExecutableInfoKHR*,UInt32*,VkPipelineExecutableStatisticKHR*,VkResult > vkGetPipelineExecutableStatisticsKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineExecutableInfoKHR*,UInt32*,VkPipelineExecutableInternalRepresentationKHR*,VkResult > vkGetPipelineExecutableInternalRepresentationsKHR = null;
#endregion
#region VK_KHR_synchronization2
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkEvent,VkDependencyInfo*,void > vkCmdSetEvent2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkEvent,UInt64,void > vkCmdResetEvent2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkEvent*,VkDependencyInfo*,void > vkCmdWaitEvents2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkDependencyInfo*,void > vkCmdPipelineBarrier2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,VkQueryPool,UInt32,void > vkCmdWriteTimestamp2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,UInt32,VkSubmitInfo2*,VkFence,VkResult > vkQueueSubmit2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,VkBuffer,UInt64,UInt32,void > vkCmdWriteBufferMarker2AMD = null;
public unsafe /*readonly*/  delegate* unmanaged< VkQueue,UInt32*,VkCheckpointData2NV*,void > vkGetQueueCheckpointData2NV = null;
#endregion
#region VK_KHR_copy_commands2
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyBufferInfo2*,void > vkCmdCopyBuffer2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyImageInfo2*,void > vkCmdCopyImage2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyBufferToImageInfo2*,void > vkCmdCopyBufferToImage2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyImageToBufferInfo2*,void > vkCmdCopyImageToBuffer2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBlitImageInfo2*,void > vkCmdBlitImage2KHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkResolveImageInfo2*,void > vkCmdResolveImage2KHR = null;
#endregion
#region VK_KHR_ray_tracing_maintenance1
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,void > vkCmdTraceRaysIndirect2KHR = null;
#endregion
#region VK_KHR_maintenance4
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceBufferMemoryRequirements*,VkMemoryRequirements2*,void > vkGetDeviceBufferMemoryRequirementsKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceImageMemoryRequirements*,VkMemoryRequirements2*,void > vkGetDeviceImageMemoryRequirementsKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceImageMemoryRequirements*,UInt32*,VkSparseImageMemoryRequirements2*,void > vkGetDeviceImageSparseMemoryRequirementsKHR = null;
#endregion

#region VK_EXT_transform_feedback
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,UInt64*,void > vkCmdBindTransformFeedbackBuffersEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,void > vkCmdBeginTransformFeedbackEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,void > vkCmdEndTransformFeedbackEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkQueryPool,UInt32,UInt32,UInt32,void > vkCmdBeginQueryIndexedEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkQueryPool,UInt32,UInt32,void > vkCmdEndQueryIndexedEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndirectByteCountEXT = null;
#endregion
#region VK_NVX_binary_import
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCuModuleCreateInfoNVX*,VkAllocationCallbacks*,VkCuModuleNVX*,VkResult > vkCreateCuModuleNVX = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCuFunctionCreateInfoNVX*,VkAllocationCallbacks*,VkCuFunctionNVX*,VkResult > vkCreateCuFunctionNVX = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCuModuleNVX,VkAllocationCallbacks*,void > vkDestroyCuModuleNVX = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkCuFunctionNVX,VkAllocationCallbacks*,void > vkDestroyCuFunctionNVX = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCuLaunchInfoNVX*,void > vkCmdCuLaunchKernelNVX = null;
#endregion
#region VK_NVX_image_view_handle
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageViewHandleInfoNVX*,UInt32 > vkGetImageViewHandleNVX = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImageView,VkImageViewAddressPropertiesNVX*,VkResult > vkGetImageViewAddressNVX = null;
#endregion
#region VK_AMD_draw_indirect_count
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndirectCountAMD = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawIndexedIndirectCountAMD = null;
#endregion
#region VK_AMD_shader_info
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,VkShaderStageFlagBits,VkShaderInfoTypeAMD,Int32*,void*,VkResult > vkGetShaderInfoAMD = null;
#endregion

#region VK_EXT_conditional_rendering
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkConditionalRenderingBeginInfoEXT*,void > vkCmdBeginConditionalRenderingEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdEndConditionalRenderingEXT = null;
#endregion
#region VK_NV_clip_space_w_scaling
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkViewportWScalingNV*,void > vkCmdSetViewportWScalingNV = null;
#endregion
#region VK_EXT_display_control
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDisplayKHR,VkDisplayPowerInfoEXT*,VkResult > vkDisplayPowerControlEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceEventInfoEXT*,VkAllocationCallbacks*,VkFence*,VkResult > vkRegisterDeviceEventEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDisplayKHR,VkDisplayEventInfoEXT*,VkAllocationCallbacks*,VkFence*,VkResult > vkRegisterDisplayEventEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,VkSurfaceCounterFlagBitsEXT,UInt64*,VkResult > vkGetSwapchainCounterEXT = null;
#endregion
#region VK_GOOGLE_display_timing
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,VkRefreshCycleDurationGOOGLE*,VkResult > vkGetRefreshCycleDurationGOOGLE = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt32*,VkPastPresentationTimingGOOGLE*,VkResult > vkGetPastPresentationTimingGOOGLE = null;
#endregion
#region VK_EXT_discard_rectangles
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkRect2D*,void > vkCmdSetDiscardRectangleEXT = null;
#endregion
#region VK_EXT_hdr_metadata
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkSwapchainKHR*,VkHdrMetadataEXT*,void > vkSetHdrMetadataEXT = null;
#endregion

#region VK_EXT_image_drm_format_modifier
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkImage,VkImageDrmFormatModifierPropertiesEXT*,VkResult > vkGetImageDrmFormatModifierPropertiesEXT = null;
#endregion
#region VK_EXT_validation_cache
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkValidationCacheCreateInfoEXT*,VkAllocationCallbacks*,VkValidationCacheEXT*,VkResult > vkCreateValidationCacheEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkValidationCacheEXT,VkAllocationCallbacks*,void > vkDestroyValidationCacheEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkValidationCacheEXT,UInt32,VkValidationCacheEXT*,VkResult > vkMergeValidationCachesEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkValidationCacheEXT,Int32*,void*,VkResult > vkGetValidationCacheDataEXT = null;
#endregion

#region VK_NV_ray_tracing
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureCreateInfoNV*,VkAllocationCallbacks*,VkAccelerationStructureNV*,VkResult > vkCreateAccelerationStructureNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureNV,VkAllocationCallbacks*,void > vkDestroyAccelerationStructureNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureMemoryRequirementsInfoNV*,VkMemoryRequirements2*,void > vkGetAccelerationStructureMemoryRequirementsNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkBindAccelerationStructureMemoryInfoNV*,VkResult > vkBindAccelerationStructureMemoryNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkAccelerationStructureInfoNV*,VkBuffer,UInt64,UInt32,VkAccelerationStructureNV,VkAccelerationStructureNV,VkBuffer,UInt64,void > vkCmdBuildAccelerationStructureNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkAccelerationStructureNV,VkAccelerationStructureNV,VkCopyAccelerationStructureModeKHR,void > vkCmdCopyAccelerationStructureNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt64,VkBuffer,UInt64,UInt64,VkBuffer,UInt64,UInt64,UInt32,UInt32,UInt32,void > vkCmdTraceRaysNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineCache,UInt32,VkRayTracingPipelineCreateInfoNV*,VkAllocationCallbacks*,VkPipeline*,VkResult > vkCreateRayTracingPipelinesNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,UInt32,UInt32,Int32,void*,VkResult > vkGetRayTracingShaderGroupHandlesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,UInt32,UInt32,Int32,void*,VkResult > vkGetRayTracingShaderGroupHandlesNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureNV,Int32,void*,VkResult > vkGetAccelerationStructureHandleNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkAccelerationStructureNV*,VkQueryType,VkQueryPool,UInt32,void > vkCmdWriteAccelerationStructuresPropertiesNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,UInt32,VkResult > vkCompileDeferredNV = null;
#endregion
#region VK_EXT_external_memory_host
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkExternalMemoryHandleTypeFlagBits,void*,VkMemoryHostPointerPropertiesEXT*,VkResult > vkGetMemoryHostPointerPropertiesEXT = null;
#endregion
#region VK_AMD_buffer_marker
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPipelineStageFlagBits,VkBuffer,UInt64,UInt32,void > vkCmdWriteBufferMarkerAMD = null;
#endregion

#region VK_AMD_display_native_hdr
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkSwapchainKHR,UInt32,void > vkSetLocalDimmingAMD = null;
#endregion
#region VK_EXT_buffer_device_address
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkBufferDeviceAddressInfo*,UInt64 > vkGetBufferDeviceAddressEXT = null;
#endregion

#region VK_EXT_host_query_reset
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkQueryPool,UInt32,UInt32,void > vkResetQueryPoolEXT = null;
#endregion
#region VK_EXT_extended_dynamic_state
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetCullModeEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkFrontFace,void > vkCmdSetFrontFaceEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPrimitiveTopology,void > vkCmdSetPrimitiveTopologyEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkViewport*,void > vkCmdSetViewportWithCountEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkRect2D*,void > vkCmdSetScissorWithCountEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,UInt64*,UInt64*,void > vkCmdBindVertexBuffers2EXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthTestEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthWriteEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCompareOp,void > vkCmdSetDepthCompareOpEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthBoundsTestEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetStencilTestEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkStencilOp,VkStencilOp,VkStencilOp,VkCompareOp,void > vkCmdSetStencilOpEXT = null;
#endregion


#region VK_EXT_private_data
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPrivateDataSlotCreateInfo*,VkAllocationCallbacks*,VkPrivateDataSlot*,VkResult > vkCreatePrivateDataSlotEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPrivateDataSlot,VkAllocationCallbacks*,void > vkDestroyPrivateDataSlotEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64,VkResult > vkSetPrivateDataEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64*,void > vkGetPrivateDataEXT = null;
#endregion

#region VK_EXT_vertex_input_dynamic_state
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkVertexInputBindingDescription2EXT*,UInt32,VkVertexInputAttributeDescription2EXT*,void > vkCmdSetVertexInputEXT = null;
#endregion
#region VK_HUAWEI_subpass_shading
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderPass,VkExtent2D*,VkResult > vkGetDeviceSubpassShadingMaxWorkgroupSizeHUAWEI = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,void > vkCmdSubpassShadingHUAWEI = null;
#endregion
#region VK_HUAWEI_invocation_mask
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkImageView,VkImageLayout,void > vkCmdBindInvocationMaskHUAWEI = null;
#endregion
#region VK_NV_external_memory_rdma
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMemoryGetRemoteAddressInfoNV*,void**,VkResult > vkGetMemoryRemoteAddressNV = null;
#endregion
#region VK_EXT_pipeline_properties
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipelineInfoKHR*,VkBaseOutStructure*,VkResult > vkGetPipelinePropertiesEXT = null;
#endregion
#region VK_EXT_extended_dynamic_state2
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetPatchControlPointsEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetRasterizerDiscardEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthBiasEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkLogicOp,void > vkCmdSetLogicOpEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetPrimitiveRestartEnableEXT = null;
#endregion
#region VK_EXT_color_write_enable
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32*,void > vkCmdSetColorWriteEnableEXT = null;
#endregion
#region VK_EXT_multi_draw
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkMultiDrawInfoEXT*,UInt32,UInt32,UInt32,void > vkCmdDrawMultiEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkMultiDrawIndexedInfoEXT*,UInt32,UInt32,UInt32,Int32*,void > vkCmdDrawMultiIndexedEXT = null;
#endregion
#region VK_EXT_opacity_micromap
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMicromapCreateInfoEXT*,VkAllocationCallbacks*,VkMicromapEXT*,VkResult > vkCreateMicromapEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMicromapEXT,VkAllocationCallbacks*,void > vkDestroyMicromapEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkMicromapBuildInfoEXT*,void > vkCmdBuildMicromapsEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,UInt32,VkMicromapBuildInfoEXT*,VkResult > vkBuildMicromapsEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkCopyMicromapInfoEXT*,VkResult > vkCopyMicromapEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkCopyMicromapToMemoryInfoEXT*,VkResult > vkCopyMicromapToMemoryEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkCopyMemoryToMicromapInfoEXT*,VkResult > vkCopyMemoryToMicromapEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkMicromapEXT*,VkQueryType,Int32,void*,Int32,VkResult > vkWriteMicromapsPropertiesEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyMicromapInfoEXT*,void > vkCmdCopyMicromapEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyMicromapToMemoryInfoEXT*,void > vkCmdCopyMicromapToMemoryEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyMemoryToMicromapInfoEXT*,void > vkCmdCopyMemoryToMicromapEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkMicromapEXT*,VkQueryType,VkQueryPool,UInt32,void > vkCmdWriteMicromapsPropertiesEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkMicromapVersionInfoEXT*,VkAccelerationStructureCompatibilityKHR*,void > vkGetDeviceMicromapCompatibilityEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureBuildTypeKHR,VkMicromapBuildInfoEXT*,VkMicromapBuildSizesInfoEXT*,void > vkGetMicromapBuildSizesEXT = null;
#endregion
#region VK_EXT_pageable_device_local_memory
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeviceMemory,float,void > vkSetDeviceMemoryPriorityEXT = null;
#endregion
#region VK_VALVE_descriptor_set_host_mapping
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSetBindingReferenceVALVE*,VkDescriptorSetLayoutHostMappingInfoVALVE*,void > vkGetDescriptorSetLayoutHostMappingInfoVALVE = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDescriptorSet,void**,void > vkGetDescriptorSetHostMappingVALVE = null;
#endregion
#region VK_NV_copy_memory_indirect
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,UInt32,UInt32,void > vkCmdCopyMemoryIndirectNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,UInt32,UInt32,VkImage,VkImageLayout,VkImageSubresourceLayers*,void > vkCmdCopyMemoryToImageIndirectNV = null;
#endregion
#region VK_NV_memory_decompression
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkDecompressMemoryRegionNV*,void > vkCmdDecompressMemoryNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt64,UInt64,UInt32,void > vkCmdDecompressMemoryIndirectCountNV = null;
#endregion
#region VK_EXT_extended_dynamic_state3
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkTessellationDomainOrigin,void > vkCmdSetTessellationDomainOriginEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthClampEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkPolygonMode,void > vkCmdSetPolygonModeEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSampleCountFlagBits,void > vkCmdSetRasterizationSamplesEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkSampleCountFlagBits,UInt32*,void > vkCmdSetSampleMaskEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetAlphaToCoverageEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetAlphaToOneEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetLogicOpEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32*,void > vkCmdSetColorBlendEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkColorBlendEquationEXT*,void > vkCmdSetColorBlendEquationEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32*,void > vkCmdSetColorWriteMaskEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetRasterizationStreamEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkConservativeRasterizationModeEXT,void > vkCmdSetConservativeRasterizationModeEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,float,void > vkCmdSetExtraPrimitiveOverestimationSizeEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthClipEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetSampleLocationsEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkColorBlendAdvancedEXT*,void > vkCmdSetColorBlendAdvancedEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkProvokingVertexModeEXT,void > vkCmdSetProvokingVertexModeEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkLineRasterizationModeEXT,void > vkCmdSetLineRasterizationModeEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetLineStippleEnableEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetDepthClipNegativeOneToOneEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetViewportWScalingEnableNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,VkViewportSwizzleNV*,void > vkCmdSetViewportSwizzleNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetCoverageToColorEnableNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetCoverageToColorLocationNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCoverageModulationModeNV,void > vkCmdSetCoverageModulationModeNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetCoverageModulationTableEnableNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,float*,void > vkCmdSetCoverageModulationTableNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetShadingRateImageEnableNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetRepresentativeFragmentTestEnableNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCoverageReductionModeNV,void > vkCmdSetCoverageReductionModeNV = null;
#endregion
#region VK_EXT_shader_module_identifier
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkShaderModule,VkShaderModuleIdentifierEXT*,void > vkGetShaderModuleIdentifierEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkShaderModuleCreateInfo*,VkShaderModuleIdentifierEXT*,void > vkGetShaderModuleCreateInfoIdentifierEXT = null;
#endregion
#region VK_NV_optical_flow
public unsafe /*readonly*/  delegate* unmanaged< VkPhysicalDevice,VkOpticalFlowImageFormatInfoNV*,UInt32*,VkOpticalFlowImageFormatPropertiesNV*,VkResult > vkGetPhysicalDeviceOpticalFlowImageFormatsNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkOpticalFlowSessionCreateInfoNV*,VkAllocationCallbacks*,VkOpticalFlowSessionNV*,VkResult > vkCreateOpticalFlowSessionNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkOpticalFlowSessionNV,VkAllocationCallbacks*,void > vkDestroyOpticalFlowSessionNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkOpticalFlowSessionNV,VkOpticalFlowSessionBindingPointNV,VkImageView,VkImageLayout,VkResult > vkBindOpticalFlowSessionImageNV = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkOpticalFlowSessionNV,VkOpticalFlowExecuteInfoNV*,void > vkCmdOpticalFlowExecuteNV = null;
#endregion
#region VK_QCOM_tile_properties
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkFramebuffer,UInt32*,VkTilePropertiesQCOM*,VkResult > vkGetFramebufferTilePropertiesQCOM = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkRenderingInfo*,VkTilePropertiesQCOM*,VkResult > vkGetDynamicRenderingTilePropertiesQCOM = null;
#endregion
#region VK_KHR_acceleration_structure
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureCreateInfoKHR*,VkAllocationCallbacks*,VkAccelerationStructureKHR*,VkResult > vkCreateAccelerationStructureKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureKHR,VkAllocationCallbacks*,void > vkDestroyAccelerationStructureKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkAccelerationStructureBuildGeometryInfoKHR*,VkAccelerationStructureBuildRangeInfoKHR*,void > vkCmdBuildAccelerationStructuresKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkAccelerationStructureBuildGeometryInfoKHR*,UInt64*,UInt32*,UInt32*,void > vkCmdBuildAccelerationStructuresIndirectKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,UInt32,VkAccelerationStructureBuildGeometryInfoKHR*,VkAccelerationStructureBuildRangeInfoKHR*,VkResult > vkBuildAccelerationStructuresKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkCopyAccelerationStructureInfoKHR*,VkResult > vkCopyAccelerationStructureKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkCopyAccelerationStructureToMemoryInfoKHR*,VkResult > vkCopyAccelerationStructureToMemoryKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkCopyMemoryToAccelerationStructureInfoKHR*,VkResult > vkCopyMemoryToAccelerationStructureKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,UInt32,VkAccelerationStructureKHR*,VkQueryType,Int32,void*,Int32,VkResult > vkWriteAccelerationStructuresPropertiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyAccelerationStructureInfoKHR*,void > vkCmdCopyAccelerationStructureKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyAccelerationStructureToMemoryInfoKHR*,void > vkCmdCopyAccelerationStructureToMemoryKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkCopyMemoryToAccelerationStructureInfoKHR*,void > vkCmdCopyMemoryToAccelerationStructureKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureDeviceAddressInfoKHR*,UInt64 > vkGetAccelerationStructureDeviceAddressKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,VkAccelerationStructureKHR*,VkQueryType,VkQueryPool,UInt32,void > vkCmdWriteAccelerationStructuresPropertiesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureVersionInfoKHR*,VkAccelerationStructureCompatibilityKHR*,void > vkGetDeviceAccelerationStructureCompatibilityKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkAccelerationStructureBuildTypeKHR,VkAccelerationStructureBuildGeometryInfoKHR*,UInt32*,VkAccelerationStructureBuildSizesInfoKHR*,void > vkGetAccelerationStructureBuildSizesKHR = null;
#endregion
#region VK_KHR_ray_tracing_pipeline
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,UInt32,UInt32,UInt32,void > vkCmdTraceRaysKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkDeferredOperationKHR,VkPipelineCache,UInt32,VkRayTracingPipelineCreateInfoKHR*,VkAllocationCallbacks*,VkPipeline*,VkResult > vkCreateRayTracingPipelinesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,UInt32,UInt32,Int32,void*,VkResult > vkGetRayTracingCaptureReplayShaderGroupHandlesKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,UInt64,void > vkCmdTraceRaysIndirectKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkDevice,VkPipeline,UInt32,VkShaderGroupShaderKHR,UInt64 > vkGetRayTracingShaderGroupStackSizeKHR = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,void > vkCmdSetRayTracingPipelineStackSizeKHR = null;
#endregion
#region VK_EXT_mesh_shader
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,UInt32,UInt32,UInt32,void > vkCmdDrawMeshTasksEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawMeshTasksIndirectEXT = null;
public unsafe /*readonly*/  delegate* unmanaged< VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void > vkCmdDrawMeshTasksIndirectCountEXT = null;
#endregion
#endregion


#region WINDOWS
// typedef VkResult (VKAPI_PTR *PFN_vkImportSemaphoreWin32HandleKHR)(VkDevice device, const VkImportSemaphoreWin32HandleInfoKHR* pImportSemaphoreWin32HandleInfo);
// typedef VkResult (VKAPI_PTR *PFN_vkGetSemaphoreWin32HandleKHR)(VkDevice device, const VkSemaphoreGetWin32HandleInfoKHR* pGetWin32HandleInfo, HANDLE* pHandle);
// typedef VkResult (VKAPI_PTR *PFN_vkImportFenceWin32HandleKHR)(VkDevice device, const VkImportFenceWin32HandleInfoKHR* pImportFenceWin32HandleInfo);
// typedef VkResult (VKAPI_PTR *PFN_vkGetFenceWin32HandleKHR)(VkDevice device, const VkFenceGetWin32HandleInfoKHR* pGetWin32HandleInfo, HANDLE* pHandle);
// typedef VkResult (VKAPI_PTR *PFN_vkGetMemoryWin32HandleNV)(VkDevice device, VkDeviceMemory memory, VkExternalMemoryHandleTypeFlagsNV handleType, HANDLE* pHandle);
// typedef VkResult (VKAPI_PTR *PFN_vkGetPhysicalDeviceSurfacePresentModes2EXT)(VkPhysicalDevice physicalDevice, const VkPhysicalDeviceSurfaceInfo2KHR* pSurfaceInfo, uint32_t* pPresentModeCount, VkPresentModeKHR* pPresentModes);
// typedef VkResult (VKAPI_PTR *PFN_vkAcquireFullScreenExclusiveModeEXT)(VkDevice device, VkSwapchainKHR swapchain);
// typedef VkResult (VKAPI_PTR *PFN_vkReleaseFullScreenExclusiveModeEXT)(VkDevice device, VkSwapchainKHR swapchain);
// typedef VkResult (VKAPI_PTR *PFN_vkGetDeviceGroupSurfacePresentModes2EXT)(VkDevice device, const VkPhysicalDeviceSurfaceInfo2KHR* pSurfaceInfo, VkDeviceGroupPresentModeFlagsKHR* pModes);
#endregion


    public GraphicDeviceFunctions()  {  }

    public void InitLoaderFunctions( string dllname )=> InitLoader( dllname);
    public void InitInstancesFunctions( PFN_vkGetInstanceProcAddr load , VkInstance instance )=> InitInstance( load, instance);
    public void InitDevicesFunctions( PFN_vkGetDeviceProcAddr load , VkDevice device ) => InitDevice( load , device);

    private unsafe void InitLoader( string VulkanDLLName )
    {
        vulkan = Libraries.Load(VulkanDLLName);
        if ( vulkan == nint.Zero) throw new Exception("Vulkan Dll not found");

        vkEnumerateInstanceLayerProperties =(delegate* unmanaged< UInt32*,VkLayerProperties*,VkResult>) Libraries.GetUnsafeSymbol( vulkan,nameof(vkEnumerateInstanceLayerProperties)) ;
        vkEnumerateInstanceVersion =  (delegate* unmanaged<UInt32*  , VkResult>)Libraries.GetUnsafeSymbol(vulkan,nameof(vkEnumerateInstanceVersion)) ;
        vkEnumerateInstanceExtensionProperties = (delegate* unmanaged<char*,UInt32*,VkExtensionProperties*,VkResult>)Libraries.GetUnsafeSymbol(vulkan,nameof(vkEnumerateInstanceExtensionProperties)) ;
        vkCreateInstance = (delegate* unmanaged<VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*  , VkResult> )Libraries.GetUnsafeSymbol(vulkan,nameof(vkCreateInstance));
        vkDestroyInstance = (delegate* unmanaged<VkInstance  ,VkAllocationCallbacks*, void> ) Libraries.GetUnsafeSymbol(vulkan,nameof(vkDestroyInstance));
        FuncvkGetInstanceProcAddr = (delegate* unmanaged<VkInstance,byte*  , void*>) Libraries.GetUnsafeSymbol(vulkan,nameof(vkGetInstanceProcAddr));
        FuncvkGetDeviceProcAddr = ( delegate* unmanaged<VkDevice,byte*  , void*>) Libraries.GetUnsafeSymbol(vulkan,nameof(vkGetDeviceProcAddr));
    }

    public unsafe static void* vkGetInstanceProcAddr(VkInstance instance, string name)
    {
		void*    result = null;
		byte[] bytes = Encoding.ASCII.GetBytes(name);
		fixed( byte* ptr = bytes){
			result =  FuncvkGetInstanceProcAddr(  instance,ptr );
		}
        return result;
    }

    public unsafe static void* vkGetDeviceProcAddr(VkDevice device, string name)
    {
		void*    result = null;
		byte[] bytes = Encoding.ASCII.GetBytes(name);
		fixed( byte* ptr = bytes){
			result =  FuncvkGetDeviceProcAddr(  device,ptr );
		}
        return result;
    }
    public void Release()
    {
        ReleaseDevice();
        ReleaseInstance();
        ReleaseLoader();
    }
    
    public unsafe void InitInstance( PFN_vkGetInstanceProcAddr load , VkInstance instance)
    {
		
        if (VK.VK_KHR_win32_surface )
        {
            vkDestroySurfaceKHR=  (delegate* unmanaged<VkInstance, VkSurfaceKHR,VkAllocationCallbacks*  , void>)load(instance,nameof(vkDestroySurfaceKHR));
            vkCreateWin32SurfaceKHR = (delegate* unmanaged<VkInstance,VkWin32SurfaceCreateInfoKHR*, VkAllocationCallbacks*,VkSurfaceKHR*  , VkResult> )load(instance,nameof(vkCreateWin32SurfaceKHR));
        }
        if (VK.VK_VERSION_1_0)
		{
			vkCreateDevice = (delegate* unmanaged<VkPhysicalDevice,VkDeviceCreateInfo*,VkAllocationCallbacks*,VkDevice*,VkResult>) load(instance,nameof(vkCreateDevice)); 
			vkDestroyDevice = (delegate* unmanaged<VkDevice,VkAllocationCallbacks*,void>) load(instance,nameof(vkDestroyDevice)); 
			vkEnumeratePhysicalDevices = (delegate* unmanaged<VkInstance,UInt32*,VkPhysicalDevice*,VkResult>) load(instance,nameof(vkEnumeratePhysicalDevices)); 
			vkGetPhysicalDeviceFeatures = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceFeatures*,void>) load(instance,nameof(vkGetPhysicalDeviceFeatures)); 
			vkGetPhysicalDeviceFormatProperties = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkFormatProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceFormatProperties)); 
			vkGetPhysicalDeviceImageFormatProperties = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkImageType,VkImageTiling,UInt32,UInt32,VkImageFormatProperties*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceImageFormatProperties)); 
			vkGetPhysicalDeviceProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceProperties)); 
			vkGetPhysicalDeviceQueueFamilyProperties = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkQueueFamilyProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceQueueFamilyProperties)); 
			vkGetPhysicalDeviceMemoryProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceMemoryProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceMemoryProperties)); 
			vkEnumerateDeviceExtensionProperties = (delegate* unmanaged<VkPhysicalDevice,char*,UInt32*,VkExtensionProperties*,VkResult>) load(instance,nameof(vkEnumerateDeviceExtensionProperties)); 
			vkEnumerateDeviceLayerProperties = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkLayerProperties*,VkResult>) load(instance,nameof(vkEnumerateDeviceLayerProperties)); 
		
			vkGetPhysicalDeviceSparseImageFormatProperties = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkImageType,VkSampleCountFlagBits,UInt32,VkImageTiling,UInt32*,VkSparseImageFormatProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceSparseImageFormatProperties)); 

			vkEnumeratePhysicalDeviceGroups = (delegate* unmanaged<VkInstance,UInt32*,VkPhysicalDeviceGroupProperties*,VkResult>) load(instance,nameof(vkEnumeratePhysicalDeviceGroups)); 
        }
        if (VK.VK_VERSION_1_1)
		{
            vkGetPhysicalDeviceFeatures2 = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceFeatures2*,void>) load(instance,nameof(vkGetPhysicalDeviceFeatures2)); 
			vkGetPhysicalDeviceProperties2 = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceProperties2)); 
			vkGetPhysicalDeviceFormatProperties2 = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkFormatProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceFormatProperties2)); 
			vkGetPhysicalDeviceImageFormatProperties2 = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceImageFormatInfo2*,VkImageFormatProperties2*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceImageFormatProperties2)); 
			vkGetPhysicalDeviceQueueFamilyProperties2 = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkQueueFamilyProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceQueueFamilyProperties2)); 
			vkGetPhysicalDeviceMemoryProperties2 = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceMemoryProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceMemoryProperties2)); 
			vkGetPhysicalDeviceSparseImageFormatProperties2 = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceSparseImageFormatInfo2*,UInt32*,VkSparseImageFormatProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceSparseImageFormatProperties2)); 
			vkGetPhysicalDeviceExternalBufferProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceExternalBufferInfo*,VkExternalBufferProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceExternalBufferProperties)); 
			vkGetPhysicalDeviceExternalFenceProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceExternalFenceInfo*,VkExternalFenceProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceExternalFenceProperties)); 
			vkGetPhysicalDeviceExternalSemaphoreProperties = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceExternalSemaphoreInfo*,VkExternalSemaphoreProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceExternalSemaphoreProperties)); 
			
			//version 1.3
			vkGetPhysicalDeviceToolProperties = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkPhysicalDeviceToolProperties*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceToolProperties)); 
			
		}
		if (VK.VK_KHR_surface){
			vkDestroySurfaceKHR = (delegate* unmanaged<VkInstance,VkSurfaceKHR,VkAllocationCallbacks*,void>) load(instance,nameof(vkDestroySurfaceKHR)); 
			vkGetPhysicalDeviceSurfaceSupportKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32,VkSurfaceKHR,uint*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfaceSupportKHR)); 
			vkGetPhysicalDeviceSurfaceCapabilitiesKHR = (delegate* unmanaged<VkPhysicalDevice,VkSurfaceKHR,VkSurfaceCapabilitiesKHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfaceCapabilitiesKHR)); 
			vkGetPhysicalDeviceSurfaceFormatsKHR = (delegate* unmanaged<VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkSurfaceFormatKHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfaceFormatsKHR)); 
			vkGetPhysicalDeviceSurfacePresentModesKHR = (delegate* unmanaged<VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkPresentModeKHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfacePresentModesKHR)); 
		}
		if (VK.VK_KHR_swapchain){
			vkQueuePresentKHR = (delegate* unmanaged<VkQueue,VkPresentInfoKHR*,VkResult>) load(instance,nameof(vkQueuePresentKHR)); 
			vkGetPhysicalDevicePresentRectanglesKHR = (delegate* unmanaged<VkPhysicalDevice,VkSurfaceKHR,UInt32*,VkRect2D*,VkResult>) load(instance,nameof(vkGetPhysicalDevicePresentRectanglesKHR)); 
		}
		if (VK.VK_KHR_display){
			vkGetPhysicalDeviceDisplayPropertiesKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkDisplayPropertiesKHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceDisplayPropertiesKHR)); 
			vkGetPhysicalDeviceDisplayPlanePropertiesKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkDisplayPlanePropertiesKHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceDisplayPlanePropertiesKHR)); 
			vkGetDisplayPlaneSupportedDisplaysKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32,UInt32*,VkDisplayKHR*,VkResult>) load(instance,nameof(vkGetDisplayPlaneSupportedDisplaysKHR)); 
			vkGetDisplayModePropertiesKHR = (delegate* unmanaged<VkPhysicalDevice,VkDisplayKHR,UInt32*,VkDisplayModePropertiesKHR*,VkResult>) load(instance,nameof(vkGetDisplayModePropertiesKHR)); 
			vkCreateDisplayModeKHR = (delegate* unmanaged<VkPhysicalDevice,VkDisplayKHR,VkDisplayModeCreateInfoKHR*,VkAllocationCallbacks*,VkDisplayModeKHR*,VkResult>) load(instance,nameof(vkCreateDisplayModeKHR)); 
			vkGetDisplayPlaneCapabilitiesKHR = (delegate* unmanaged<VkPhysicalDevice,VkDisplayModeKHR,UInt32,VkDisplayPlaneCapabilitiesKHR*,VkResult>) load(instance,nameof(vkGetDisplayPlaneCapabilitiesKHR)); 
			vkCreateDisplayPlaneSurfaceKHR = (delegate* unmanaged<VkInstance,VkDisplaySurfaceCreateInfoKHR*,VkAllocationCallbacks*,VkSurfaceKHR*,VkResult>) load(instance,nameof(vkCreateDisplayPlaneSurfaceKHR)); 
		}

		if (VK.VK_KHR_get_physical_device_properties2){
			vkGetPhysicalDeviceFeatures2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceFeatures2*,void>) load(instance,nameof(vkGetPhysicalDeviceFeatures2KHR)); 
			vkGetPhysicalDeviceProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceProperties2KHR)); 
			vkGetPhysicalDeviceFormatProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkFormatProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceFormatProperties2KHR)); 
			vkGetPhysicalDeviceImageFormatProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceImageFormatInfo2*,VkImageFormatProperties2*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceImageFormatProperties2KHR)); 
			vkGetPhysicalDeviceQueueFamilyProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkQueueFamilyProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceQueueFamilyProperties2KHR)); 
			vkGetPhysicalDeviceMemoryProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceMemoryProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceMemoryProperties2KHR)); 
			vkGetPhysicalDeviceSparseImageFormatProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceSparseImageFormatInfo2*,UInt32*,VkSparseImageFormatProperties2*,void>) load(instance,nameof(vkGetPhysicalDeviceSparseImageFormatProperties2KHR)); 
		}
		
		if (VK.VK_KHR_device_group_creation){
			// vkEnumeratePhysicalDeviceGroupsKHR = (delegate* unmanaged<VkInstance,UInt32*,VkPhysicalDeviceGroupProperties*,VkResult>) load(instance,nameof(vkEnumeratePhysicalDeviceGroupsKHR)); 
		}
		if (VK.VK_KHR_external_memory_capabilities){
			vkGetPhysicalDeviceExternalBufferPropertiesKHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceExternalBufferInfo*,VkExternalBufferProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceExternalBufferPropertiesKHR)); 
		}
		if (VK.VK_KHR_external_semaphore_capabilities){
			vkGetPhysicalDeviceExternalSemaphorePropertiesKHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceExternalSemaphoreInfo*,VkExternalSemaphoreProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceExternalSemaphorePropertiesKHR)); 
		}
       
		if (VK.VK_KHR_external_fence_capabilities){
			vkGetPhysicalDeviceExternalFencePropertiesKHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceExternalFenceInfo*,VkExternalFenceProperties*,void>) load(instance,nameof(vkGetPhysicalDeviceExternalFencePropertiesKHR)); 
		}
		if (VK.VK_KHR_performance_query){
			vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32,UInt32*,VkPerformanceCounterKHR*,VkPerformanceCounterDescriptionKHR*,VkResult>) load(instance,nameof(vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR)); 
			vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR = (delegate* unmanaged<VkPhysicalDevice,VkQueryPoolPerformanceCreateInfoKHR*,UInt32*,void>) load(instance,nameof(vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR)); 
		}
		if (VK.VK_KHR_get_surface_capabilities2){
			vkGetPhysicalDeviceSurfaceCapabilities2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceSurfaceInfo2KHR*,VkSurfaceCapabilities2KHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfaceCapabilities2KHR)); 
			vkGetPhysicalDeviceSurfaceFormats2KHR = (delegate* unmanaged<VkPhysicalDevice,VkPhysicalDeviceSurfaceInfo2KHR*,UInt32*,VkSurfaceFormat2KHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfaceFormats2KHR)); 
		}
		if (VK.VK_KHR_get_display_properties2){
			vkGetPhysicalDeviceDisplayProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkDisplayProperties2KHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceDisplayProperties2KHR)); 
			vkGetPhysicalDeviceDisplayPlaneProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkDisplayPlaneProperties2KHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceDisplayPlaneProperties2KHR)); 
			vkGetDisplayModeProperties2KHR = (delegate* unmanaged<VkPhysicalDevice,VkDisplayKHR,UInt32*,VkDisplayModeProperties2KHR*,VkResult>) load(instance,nameof(vkGetDisplayModeProperties2KHR)); 
			vkGetDisplayPlaneCapabilities2KHR = (delegate* unmanaged<VkPhysicalDevice,VkDisplayPlaneInfo2KHR*,VkDisplayPlaneCapabilities2KHR*,VkResult>) load(instance,nameof(vkGetDisplayPlaneCapabilities2KHR)); 
		}
		if (VK.VK_KHR_fragment_shading_rate){
			vkGetPhysicalDeviceFragmentShadingRatesKHR = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkPhysicalDeviceFragmentShadingRateKHR*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceFragmentShadingRatesKHR)); 
			vkCmdSetFragmentShadingRateKHR = (delegate* unmanaged<VkCommandBuffer,VkExtent2D*,VkFragmentShadingRateCombinerOpKHR,void>) load(instance,nameof(vkCmdSetFragmentShadingRateKHR)); 
		}
       
		if (VK.VK_EXT_debug_report){
			vkCreateDebugReportCallbackEXT = (delegate* unmanaged<VkInstance,VkDebugReportCallbackCreateInfoEXT*,VkAllocationCallbacks*,VkDebugReportCallbackEXT*,VkResult>) load(instance,nameof(vkCreateDebugReportCallbackEXT)); 
			vkDestroyDebugReportCallbackEXT = (delegate* unmanaged<VkInstance,VkDebugReportCallbackEXT,VkAllocationCallbacks*,void>) load(instance,nameof(vkDestroyDebugReportCallbackEXT)); 
			vkDebugReportMessageEXT = (delegate* unmanaged<VkInstance,UInt32,VkDebugReportObjectTypeEXT,UInt64,Int32,Int32,ConstChar*,ConstChar*,void>) load(instance,nameof(vkDebugReportMessageEXT)); 
		}	
        
		if (VK.VK_NV_external_memory_capabilities){
			vkGetPhysicalDeviceExternalImageFormatPropertiesNV = (delegate* unmanaged<VkPhysicalDevice,VkFormat,VkImageType,VkImageTiling,UInt32,UInt32,UInt32,VkExternalImageFormatPropertiesNV*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceExternalImageFormatPropertiesNV)); 
		}

        if (VK.VK_EXT_debug_utils){
			vkQueueBeginDebugUtilsLabelEXT = (delegate* unmanaged<VkQueue,VkDebugUtilsLabelEXT*,void>) load(instance,nameof(vkQueueBeginDebugUtilsLabelEXT)); 
			vkQueueEndDebugUtilsLabelEXT = (delegate* unmanaged<VkQueue,void>) load(instance,nameof(vkQueueEndDebugUtilsLabelEXT)); 
			vkQueueInsertDebugUtilsLabelEXT = (delegate* unmanaged<VkQueue,VkDebugUtilsLabelEXT*,void>) load(instance,nameof(vkQueueInsertDebugUtilsLabelEXT)); 
			vkCmdBeginDebugUtilsLabelEXT = (delegate* unmanaged<VkCommandBuffer,VkDebugUtilsLabelEXT*,void>) load(instance,nameof(vkCmdBeginDebugUtilsLabelEXT)); 
			vkCmdEndDebugUtilsLabelEXT = (delegate* unmanaged<VkCommandBuffer,void>) load(instance,nameof(vkCmdEndDebugUtilsLabelEXT)); 
			vkCmdInsertDebugUtilsLabelEXT = (delegate* unmanaged<VkCommandBuffer,VkDebugUtilsLabelEXT*,void>) load(instance,nameof(vkCmdInsertDebugUtilsLabelEXT)); 
			vkCreateDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance,VkDebugUtilsMessengerCreateInfoEXT*,VkAllocationCallbacks*,VkDebugUtilsMessengerEXT*,VkResult>) load(instance,nameof(vkCreateDebugUtilsMessengerEXT)); 
			vkDestroyDebugUtilsMessengerEXT = (delegate* unmanaged<VkInstance,VkDebugUtilsMessengerEXT,VkAllocationCallbacks*,void>) load(instance,nameof(vkDestroyDebugUtilsMessengerEXT)); 
			vkSubmitDebugUtilsMessageEXT = (delegate* unmanaged<VkInstance,VkDebugUtilsMessageSeverityFlagBitsEXT,UInt32,VkDebugUtilsMessengerCallbackDataEXT*,void>) load(instance,nameof(vkSubmitDebugUtilsMessageEXT)); 
		}

		if (VK.VK_EXT_direct_mode_display){
			vkReleaseDisplayEXT = (delegate* unmanaged<VkPhysicalDevice,VkDisplayKHR,VkResult>) load(instance,nameof(vkReleaseDisplayEXT)); 
		}
		if (VK.VK_EXT_display_surface_counter){
			vkGetPhysicalDeviceSurfaceCapabilities2EXT = (delegate* unmanaged<VkPhysicalDevice,VkSurfaceKHR,VkSurfaceCapabilities2EXT*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSurfaceCapabilities2EXT)); 
		}
	
		
		if (VK.VK_EXT_sample_locations){
			vkCmdSetSampleLocationsEXT = (delegate* unmanaged<VkCommandBuffer,VkSampleLocationsInfoEXT*,void>) load(instance,nameof(vkCmdSetSampleLocationsEXT)); 
			vkGetPhysicalDeviceMultisamplePropertiesEXT = (delegate* unmanaged<VkPhysicalDevice,VkSampleCountFlagBits,VkMultisamplePropertiesEXT*,void>) load(instance,nameof(vkGetPhysicalDeviceMultisamplePropertiesEXT)); 
		}
		if (VK.VK_NV_shading_rate_image){
			vkCmdBindShadingRateImageNV = (delegate* unmanaged<VkCommandBuffer,VkImageView,VkImageLayout,void>) load(instance,nameof(vkCmdBindShadingRateImageNV)); 
			vkCmdSetViewportShadingRatePaletteNV = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkShadingRatePaletteNV*,void>) load(instance,nameof(vkCmdSetViewportShadingRatePaletteNV)); 
			vkCmdSetCoarseSampleOrderNV = (delegate* unmanaged<VkCommandBuffer,VkCoarseSampleOrderTypeNV,UInt32,VkCoarseSampleOrderCustomNV*,void>) load(instance,nameof(vkCmdSetCoarseSampleOrderNV)); 
		}
		// if (VK.VK_NV_ray_tracing){
		// 	vkCmdCopyAccelerationStructureNV = (delegate* unmanaged<VkCommandBuffer,VkAccelerationStructureNV,VkAccelerationStructureNV,VkCopyAccelerationStructureModeKHR,void>) load(instance,nameof(vkCmdCopyAccelerationStructureNV)); 
		// 	vkCmdWriteAccelerationStructuresPropertiesNV = (delegate* unmanaged<VkCommandBuffer,UInt32,VkAccelerationStructureNV*,VkQueryType,VkQueryPool,UInt32,void>) load(instance,nameof(vkCmdWriteAccelerationStructuresPropertiesNV)); 
		// }
		if (VK.VK_EXT_calibrated_timestamps){
			vkGetPhysicalDeviceCalibrateableTimeDomainsEXT = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkTimeDomainEXT*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceCalibrateableTimeDomainsEXT)); 
		}
		if (VK.VK_NV_mesh_shader){
			vkCmdDrawMeshTasksNV = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,void>) load(instance,nameof(vkCmdDrawMeshTasksNV)); 
		}
		if (VK.VK_NV_scissor_exclusive){
			vkCmdSetExclusiveScissorNV = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkRect2D*,void>) load(instance,nameof(vkCmdSetExclusiveScissorNV)); 
		}
		if (VK.VK_NV_device_diagnostic_checkpoints){
			vkCmdSetCheckpointNV = (delegate* unmanaged<VkCommandBuffer,void*,void>) load(instance,nameof(vkCmdSetCheckpointNV)); 
			vkGetQueueCheckpointDataNV = (delegate* unmanaged<VkQueue,UInt32*,VkCheckpointDataNV*,void>) load(instance,nameof(vkGetQueueCheckpointDataNV)); 
		}
		if (VK.VK_INTEL_performance_query){
			vkCmdSetPerformanceMarkerINTEL = (delegate* unmanaged<VkCommandBuffer,VkPerformanceMarkerInfoINTEL*,VkResult>) load(instance,nameof(vkCmdSetPerformanceMarkerINTEL)); 
			vkCmdSetPerformanceStreamMarkerINTEL = (delegate* unmanaged<VkCommandBuffer,VkPerformanceStreamMarkerInfoINTEL*,VkResult>) load(instance,nameof(vkCmdSetPerformanceStreamMarkerINTEL)); 
			vkCmdSetPerformanceOverrideINTEL = (delegate* unmanaged<VkCommandBuffer,VkPerformanceOverrideInfoINTEL*,VkResult>) load(instance,nameof(vkCmdSetPerformanceOverrideINTEL)); 
			vkQueueSetPerformanceConfigurationINTEL = (delegate* unmanaged<VkQueue,VkPerformanceConfigurationINTEL,VkResult>) load(instance,nameof(vkQueueSetPerformanceConfigurationINTEL)); 
		}
		if (VK.VK_EXT_tooling_info){
			vkGetPhysicalDeviceToolPropertiesEXT = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkPhysicalDeviceToolProperties*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceToolPropertiesEXT)); 
		}
		if (VK.VK_NV_cooperative_matrix){
			vkGetPhysicalDeviceCooperativeMatrixPropertiesNV = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkCooperativeMatrixPropertiesNV*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceCooperativeMatrixPropertiesNV)); 
		}
		if (VK.VK_NV_coverage_reduction_mode){
			vkGetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV = (delegate* unmanaged<VkPhysicalDevice,UInt32*,VkFramebufferMixedSamplesCombinationNV*,VkResult>) load(instance,nameof(vkGetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV)); 
		}
		if (VK.VK_EXT_headless_surface){
			vkCreateHeadlessSurfaceEXT = (delegate* unmanaged<VkInstance,VkHeadlessSurfaceCreateInfoEXT*,VkAllocationCallbacks*,VkSurfaceKHR*,VkResult>) load(instance,nameof(vkCreateHeadlessSurfaceEXT)); 
		}
		if (VK.VK_EXT_line_rasterization){
			vkCmdSetLineStippleEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt16,void>) load(instance,nameof(vkCmdSetLineStippleEXT)); 
		}
		
		if (VK.VK_NV_device_generated_commands){
			vkCmdPreprocessGeneratedCommandsNV = (delegate* unmanaged<VkCommandBuffer,VkGeneratedCommandsInfoNV*,void>) load(instance,nameof(vkCmdPreprocessGeneratedCommandsNV)); 
			vkCmdExecuteGeneratedCommandsNV = (delegate* unmanaged<VkCommandBuffer,uint,VkGeneratedCommandsInfoNV*,void>) load(instance,nameof(vkCmdExecuteGeneratedCommandsNV)); 
			vkCmdBindPipelineShaderGroupNV = (delegate* unmanaged<VkCommandBuffer,VkPipelineBindPoint,VkPipeline,UInt32,void>) load(instance,nameof(vkCmdBindPipelineShaderGroupNV)); 
		}
		if (VK.VK_EXT_acquire_drm_display){
			vkAcquireDrmDisplayEXT = (delegate* unmanaged<VkPhysicalDevice,Int32,VkDisplayKHR,VkResult>) load(instance,nameof(vkAcquireDrmDisplayEXT)); 
			vkGetDrmDisplayEXT = (delegate* unmanaged<VkPhysicalDevice,Int32,UInt32,VkDisplayKHR*,VkResult>) load(instance,nameof(vkGetDrmDisplayEXT)); 
		}
	
		if (VK.VK_NV_fragment_shading_rate_enums){
			vkCmdSetFragmentShadingRateEnumNV = (delegate* unmanaged<VkCommandBuffer,VkFragmentShadingRateNV,VkFragmentShadingRateCombinerOpKHR,void>) load(instance,nameof(vkCmdSetFragmentShadingRateEnumNV)); 
		}
		if (VK.VK_NV_acquire_winrt_display){
			vkAcquireWinrtDisplayNV = (delegate* unmanaged<VkPhysicalDevice,VkDisplayKHR,VkResult>) load(instance,nameof(vkAcquireWinrtDisplayNV)); 
			vkGetWinrtDisplayNV = (delegate* unmanaged<VkPhysicalDevice,UInt32,VkDisplayKHR*,VkResult>) load(instance,nameof(vkGetWinrtDisplayNV)); 
		}

    }

    public unsafe void InitDevice( PFN_vkGetDeviceProcAddr load , VkDevice device)
    {
		if (VK.VK_KHR_win32_surface )
        {
            
        }
        if (VK.VK_VERSION_1_0)
		{	
			
			vkGetDeviceQueue = (delegate* unmanaged<VkDevice,UInt32,UInt32,VkQueue*,void>) load(device,nameof(vkGetDeviceQueue)); 
			vkDeviceWaitIdle = (delegate* unmanaged<VkDevice,VkResult>) load(device,nameof(vkDeviceWaitIdle)); 
			vkAllocateMemory = (delegate* unmanaged<VkDevice,VkMemoryAllocateInfo*,VkAllocationCallbacks*,VkDeviceMemory*,VkResult>) load(device,nameof(vkAllocateMemory)); 
			vkFreeMemory = (delegate* unmanaged<VkDevice,VkDeviceMemory,VkAllocationCallbacks*,void>) load(device,nameof(vkFreeMemory)); 
			vkMapMemory = (delegate* unmanaged<VkDevice,VkDeviceMemory,UInt64,UInt64,UInt32,void**,VkResult>) load(device,nameof(vkMapMemory)); 
			vkUnmapMemory = (delegate* unmanaged<VkDevice,VkDeviceMemory,void>) load(device,nameof(vkUnmapMemory)); 
			vkFlushMappedMemoryRanges = (delegate* unmanaged<VkDevice,UInt32,VkMappedMemoryRange*,VkResult>) load(device,nameof(vkFlushMappedMemoryRanges)); 
			vkInvalidateMappedMemoryRanges = (delegate* unmanaged<VkDevice,UInt32,VkMappedMemoryRange*,VkResult>) load(device,nameof(vkInvalidateMappedMemoryRanges)); 
			vkGetDeviceMemoryCommitment = (delegate* unmanaged<VkDevice,VkDeviceMemory,UInt64*,void>) load(device,nameof(vkGetDeviceMemoryCommitment)); 
			vkBindBufferMemory = (delegate* unmanaged<VkDevice,VkBuffer,VkDeviceMemory,UInt64,VkResult>) load(device,nameof(vkBindBufferMemory)); 
			vkBindImageMemory = (delegate* unmanaged<VkDevice,VkImage,VkDeviceMemory,UInt64,VkResult>) load(device,nameof(vkBindImageMemory)); 
			vkGetBufferMemoryRequirements = (delegate* unmanaged<VkDevice,VkBuffer,VkMemoryRequirements*,void>) load(device,nameof(vkGetBufferMemoryRequirements)); 
			vkGetImageMemoryRequirements = (delegate* unmanaged<VkDevice,VkImage,VkMemoryRequirements*,void>) load(device,nameof(vkGetImageMemoryRequirements)); 
			vkGetImageSparseMemoryRequirements = (delegate* unmanaged<VkDevice,VkImage,UInt32*,VkSparseImageMemoryRequirements*,void>) load(device,nameof(vkGetImageSparseMemoryRequirements)); 
			vkCreateFence = (delegate* unmanaged<VkDevice,VkFenceCreateInfo*,VkAllocationCallbacks*,VkFence*,VkResult>) load(device,nameof(vkCreateFence)); 
			vkDestroyFence = (delegate* unmanaged<VkDevice,VkFence,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyFence)); 
			vkResetFences = (delegate* unmanaged<VkDevice,UInt32,VkFence*,VkResult>) load(device,nameof(vkResetFences)); 
			vkGetFenceStatus = (delegate* unmanaged<VkDevice,VkFence,VkResult>) load(device,nameof(vkGetFenceStatus)); 
			vkWaitForFences = (delegate* unmanaged<VkDevice,UInt32,VkFence*,uint,UInt64,VkResult>) load(device,nameof(vkWaitForFences)); 
			vkCreateSemaphore = (delegate* unmanaged<VkDevice,VkSemaphoreCreateInfo*,VkAllocationCallbacks*,VkSemaphore*,VkResult>) load(device,nameof(vkCreateSemaphore)); 
            vkQueueSubmit = (delegate* unmanaged<VkQueue,UInt32,VkSubmitInfo*,VkFence,VkResult>) load(device,nameof(vkQueueSubmit)); 
			vkQueueWaitIdle = (delegate* unmanaged<VkQueue,VkResult>) load(device,nameof(vkQueueWaitIdle)); 
			vkDestroySemaphore = (delegate* unmanaged<VkDevice,VkSemaphore,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroySemaphore)); 
			vkCreateEvent = (delegate* unmanaged<VkDevice,VkEventCreateInfo*,VkAllocationCallbacks*,VkEvent*,VkResult>) load(device,nameof(vkCreateEvent)); 
			vkDestroyEvent = (delegate* unmanaged<VkDevice,VkEvent,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyEvent)); 
			vkGetEventStatus = (delegate* unmanaged<VkDevice,VkEvent,VkResult>) load(device,nameof(vkGetEventStatus)); 
			vkSetEvent = (delegate* unmanaged<VkDevice,VkEvent,VkResult>) load(device,nameof(vkSetEvent)); 
			vkResetEvent = (delegate* unmanaged<VkDevice,VkEvent,VkResult>) load(device,nameof(vkResetEvent)); 
			vkCreateQueryPool = (delegate* unmanaged<VkDevice,VkQueryPoolCreateInfo*,VkAllocationCallbacks*,VkQueryPool*,VkResult>) load(device,nameof(vkCreateQueryPool)); 
			vkDestroyQueryPool = (delegate* unmanaged<VkDevice,VkQueryPool,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyQueryPool)); 
			vkGetQueryPoolResults = (delegate* unmanaged<VkDevice,VkQueryPool,UInt32,UInt32,Int32,void*,UInt64,UInt32,VkResult>) load(device,nameof(vkGetQueryPoolResults)); 
			vkCreateBuffer = (delegate* unmanaged<VkDevice,VkBufferCreateInfo*,VkAllocationCallbacks*,VkBuffer*,VkResult>) load(device,nameof(vkCreateBuffer)); 
			vkDestroyBuffer = (delegate* unmanaged<VkDevice,VkBuffer,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyBuffer)); 
			vkCreateBufferView = (delegate* unmanaged<VkDevice,VkBufferViewCreateInfo*,VkAllocationCallbacks*,VkBufferView*,VkResult>) load(device,nameof(vkCreateBufferView)); 
			vkDestroyBufferView = (delegate* unmanaged<VkDevice,VkBufferView,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyBufferView)); 
			vkCreateImage = (delegate* unmanaged<VkDevice,VkImageCreateInfo*,VkAllocationCallbacks*,VkImage*,VkResult>) load(device,nameof(vkCreateImage)); 
			vkDestroyImage = (delegate* unmanaged<VkDevice,VkImage,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyImage)); 
			vkGetImageSubresourceLayout = (delegate* unmanaged<VkDevice,VkImage,VkImageSubresource*,VkSubresourceLayout*,void>) load(device,nameof(vkGetImageSubresourceLayout)); 
			vkCreateImageView = (delegate* unmanaged<VkDevice,VkImageViewCreateInfo*,VkAllocationCallbacks*,VkImageView*,VkResult>) load(device,nameof(vkCreateImageView)); 
			vkDestroyImageView = (delegate* unmanaged<VkDevice,VkImageView,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyImageView)); 
			vkCreateShaderModule = (delegate* unmanaged<VkDevice,VkShaderModuleCreateInfo*,VkAllocationCallbacks*,VkShaderModule*,VkResult>) load(device,nameof(vkCreateShaderModule)); 
			vkDestroyShaderModule = (delegate* unmanaged<VkDevice,VkShaderModule,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyShaderModule)); 
			vkCreatePipelineCache = (delegate* unmanaged<VkDevice,VkPipelineCacheCreateInfo*,VkAllocationCallbacks*,VkPipelineCache*,VkResult>) load(device,nameof(vkCreatePipelineCache)); 
			vkDestroyPipelineCache = (delegate* unmanaged<VkDevice,VkPipelineCache,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyPipelineCache)); 
			vkGetPipelineCacheData = (delegate* unmanaged<VkDevice,VkPipelineCache,Int32*,void*,VkResult>) load(device,nameof(vkGetPipelineCacheData)); 
			vkMergePipelineCaches = (delegate* unmanaged<VkDevice,VkPipelineCache,UInt32,VkPipelineCache*,VkResult>) load(device,nameof(vkMergePipelineCaches)); 
			vkCreateGraphicsPipelines = (delegate* unmanaged<VkDevice,VkPipelineCache,UInt32,VkGraphicsPipelineCreateInfo*,VkAllocationCallbacks*,VkPipeline*,VkResult>) load(device,nameof(vkCreateGraphicsPipelines)); 
			vkCreateComputePipelines = (delegate* unmanaged<VkDevice,VkPipelineCache,UInt32,VkComputePipelineCreateInfo*,VkAllocationCallbacks*,VkPipeline*,VkResult>) load(device,nameof(vkCreateComputePipelines)); 
			vkDestroyPipeline = (delegate* unmanaged<VkDevice,VkPipeline,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyPipeline)); 
			vkCreatePipelineLayout = (delegate* unmanaged<VkDevice,VkPipelineLayoutCreateInfo*,VkAllocationCallbacks*,VkPipelineLayout*,VkResult>) load(device,nameof(vkCreatePipelineLayout)); 
			vkDestroyPipelineLayout = (delegate* unmanaged<VkDevice,VkPipelineLayout,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyPipelineLayout)); 
			vkCreateSampler = (delegate* unmanaged<VkDevice,VkSamplerCreateInfo*,VkAllocationCallbacks*,VkSampler*,VkResult>) load(device,nameof(vkCreateSampler)); 
			vkDestroySampler = (delegate* unmanaged<VkDevice,VkSampler,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroySampler)); 
			vkCreateDescriptorSetLayout = (delegate* unmanaged<VkDevice,VkDescriptorSetLayoutCreateInfo*,VkAllocationCallbacks*,VkDescriptorSetLayout*,VkResult>) load(device,nameof(vkCreateDescriptorSetLayout)); 
			vkDestroyDescriptorSetLayout = (delegate* unmanaged<VkDevice,VkDescriptorSetLayout,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyDescriptorSetLayout)); 
			vkCreateDescriptorPool = (delegate* unmanaged<VkDevice,VkDescriptorPoolCreateInfo*,VkAllocationCallbacks*,VkDescriptorPool*,VkResult>) load(device,nameof(vkCreateDescriptorPool)); 
			vkDestroyDescriptorPool = (delegate* unmanaged<VkDevice,VkDescriptorPool,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyDescriptorPool)); 
			vkResetDescriptorPool = (delegate* unmanaged<VkDevice,VkDescriptorPool,UInt32,VkResult>) load(device,nameof(vkResetDescriptorPool)); 
			vkAllocateDescriptorSets = (delegate* unmanaged<VkDevice,VkDescriptorSetAllocateInfo*,VkDescriptorSet*,VkResult>) load(device,nameof(vkAllocateDescriptorSets)); 
			vkFreeDescriptorSets = (delegate* unmanaged<VkDevice,VkDescriptorPool,UInt32,VkDescriptorSet*,VkResult>) load(device,nameof(vkFreeDescriptorSets)); 
			vkUpdateDescriptorSets = (delegate* unmanaged<VkDevice,UInt32,VkWriteDescriptorSet*,UInt32,VkCopyDescriptorSet*,void>) load(device,nameof(vkUpdateDescriptorSets)); 
			vkCreateFramebuffer = (delegate* unmanaged<VkDevice,VkFramebufferCreateInfo*,VkAllocationCallbacks*,VkFramebuffer*,VkResult>) load(device,nameof(vkCreateFramebuffer)); 
			vkDestroyFramebuffer = (delegate* unmanaged<VkDevice,VkFramebuffer,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyFramebuffer)); 
			vkCreateRenderPass = (delegate* unmanaged<VkDevice,VkRenderPassCreateInfo*,VkAllocationCallbacks*,VkRenderPass*,VkResult>) load(device,nameof(vkCreateRenderPass)); 
			vkDestroyRenderPass = (delegate* unmanaged<VkDevice,VkRenderPass,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyRenderPass)); 
			vkGetRenderAreaGranularity = (delegate* unmanaged<VkDevice,VkRenderPass,VkExtent2D*,void>) load(device,nameof(vkGetRenderAreaGranularity)); 
			vkCreateCommandPool = (delegate* unmanaged<VkDevice,VkCommandPoolCreateInfo*,VkAllocationCallbacks*,VkCommandPool*,VkResult>) load(device,nameof(vkCreateCommandPool)); 
			vkDestroyCommandPool = (delegate* unmanaged<VkDevice,VkCommandPool,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyCommandPool)); 
			vkResetCommandPool = (delegate* unmanaged<VkDevice,VkCommandPool,UInt32,VkResult>) load(device,nameof(vkResetCommandPool)); 
			
             vkQueueBindSparse = (delegate* unmanaged<VkQueue,UInt32,VkBindSparseInfo*,VkFence,VkResult>) load(device,nameof(vkQueueBindSparse)); 
			vkBeginCommandBuffer = (delegate* unmanaged<VkCommandBuffer,VkCommandBufferBeginInfo*,VkResult>) load(device,nameof(vkBeginCommandBuffer)); 
			vkEndCommandBuffer = (delegate* unmanaged<VkCommandBuffer,VkResult>) load(device,nameof(vkEndCommandBuffer)); 
			vkResetCommandBuffer = (delegate* unmanaged<VkCommandBuffer,UInt32,VkResult>) load(device,nameof(vkResetCommandBuffer)); 
			vkCmdBindPipeline = (delegate* unmanaged<VkCommandBuffer,VkPipelineBindPoint,VkPipeline,void>) load(device,nameof(vkCmdBindPipeline)); 
			vkCmdSetViewport = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkViewport*,void>) load(device,nameof(vkCmdSetViewport)); 
			vkCmdSetScissor = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkRect2D*,void>) load(device,nameof(vkCmdSetScissor)); 
			vkCmdSetLineWidth = (delegate* unmanaged<VkCommandBuffer,float,void>) load(device,nameof(vkCmdSetLineWidth)); 
			vkCmdSetDepthBias = (delegate* unmanaged<VkCommandBuffer,float,float,float,void>) load(device,nameof(vkCmdSetDepthBias)); 
			vkCmdSetBlendConstants = (delegate* unmanaged<VkCommandBuffer,float,void>) load(device,nameof(vkCmdSetBlendConstants)); 
			vkCmdSetDepthBounds = (delegate* unmanaged<VkCommandBuffer,float,float,void>) load(device,nameof(vkCmdSetDepthBounds)); 
			vkCmdSetStencilCompareMask = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,void>) load(device,nameof(vkCmdSetStencilCompareMask)); 
			vkCmdSetStencilWriteMask = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,void>) load(device,nameof(vkCmdSetStencilWriteMask)); 
			vkCmdSetStencilReference = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,void>) load(device,nameof(vkCmdSetStencilReference)); 
			vkCmdBindDescriptorSets = (delegate* unmanaged<VkCommandBuffer,VkPipelineBindPoint,VkPipelineLayout,UInt32,UInt32,VkDescriptorSet*,UInt32,UInt32*,void>) load(device,nameof(vkCmdBindDescriptorSets)); 
			vkCmdDraw = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdDraw)); 
			vkCmdDrawIndexed = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,UInt32,Int32,UInt32,void>) load(device,nameof(vkCmdDrawIndexed)); 
			vkCmdDispatch = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdDispatch)); 
			vkCmdCopyBuffer = (delegate* unmanaged<VkCommandBuffer,VkBuffer,VkBuffer,UInt32,VkBufferCopy*,void>) load(device,nameof(vkCmdCopyBuffer)); 
			vkCmdCopyImage = (delegate* unmanaged<VkCommandBuffer,VkImage,VkImageLayout,VkImage,VkImageLayout,UInt32,VkImageCopy*,void>) load(device,nameof(vkCmdCopyImage)); 
			vkCmdBlitImage = (delegate* unmanaged<VkCommandBuffer,VkImage,VkImageLayout,VkImage,VkImageLayout,UInt32,VkImageBlit*,VkFilter,void>) load(device,nameof(vkCmdBlitImage)); 
			vkCmdCopyBufferToImage = (delegate* unmanaged<VkCommandBuffer,VkBuffer,VkImage,VkImageLayout,UInt32,VkBufferImageCopy*,void>) load(device,nameof(vkCmdCopyBufferToImage)); 
			vkCmdCopyImageToBuffer = (delegate* unmanaged<VkCommandBuffer,VkImage,VkImageLayout,VkBuffer,UInt32,VkBufferImageCopy*,void>) load(device,nameof(vkCmdCopyImageToBuffer)); 
			vkCmdClearColorImage = (delegate* unmanaged<VkCommandBuffer,VkImage,VkImageLayout,VkClearColorValue*,UInt32,VkImageSubresourceRange*,void>) load(device,nameof(vkCmdClearColorImage)); 
			vkCmdClearDepthStencilImage = (delegate* unmanaged<VkCommandBuffer,VkImage,VkImageLayout,VkClearDepthStencilValue*,UInt32,VkImageSubresourceRange*,void>) load(device,nameof(vkCmdClearDepthStencilImage)); 
			vkCmdClearAttachments = (delegate* unmanaged<VkCommandBuffer,UInt32,VkClearAttachment*,UInt32,VkClearRect*,void>) load(device,nameof(vkCmdClearAttachments)); 
			vkCmdResolveImage = (delegate* unmanaged<VkCommandBuffer,VkImage,VkImageLayout,VkImage,VkImageLayout,UInt32,VkImageResolve*,void>) load(device,nameof(vkCmdResolveImage)); 
			vkCmdSetEvent = (delegate* unmanaged<VkCommandBuffer,VkEvent,UInt32,void>) load(device,nameof(vkCmdSetEvent)); 
			vkCmdResetEvent = (delegate* unmanaged<VkCommandBuffer,VkEvent,UInt32,void>) load(device,nameof(vkCmdResetEvent)); 
			vkCmdWaitEvents = (delegate* unmanaged<VkCommandBuffer,UInt32,VkEvent*,UInt32,UInt32,UInt32,VkMemoryBarrier*,UInt32,VkBufferMemoryBarrier*,UInt32,VkImageMemoryBarrier*,void>) load(device,nameof(vkCmdWaitEvents)); 
			vkCmdPipelineBarrier = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,VkMemoryBarrier*,UInt32,VkBufferMemoryBarrier*,UInt32,VkImageMemoryBarrier*,void>) load(device,nameof(vkCmdPipelineBarrier)); 
			vkCmdBeginQuery = (delegate* unmanaged<VkCommandBuffer,VkQueryPool,UInt32,UInt32,void>) load(device,nameof(vkCmdBeginQuery)); 
			vkCmdEndQuery = (delegate* unmanaged<VkCommandBuffer,VkQueryPool,UInt32,void>) load(device,nameof(vkCmdEndQuery)); 
			vkCmdResetQueryPool = (delegate* unmanaged<VkCommandBuffer,VkQueryPool,UInt32,UInt32,void>) load(device,nameof(vkCmdResetQueryPool)); 
			vkCmdWriteTimestamp = (delegate* unmanaged<VkCommandBuffer,VkPipelineStageFlagBits,VkQueryPool,UInt32,void>) load(device,nameof(vkCmdWriteTimestamp)); 
			vkCmdPushConstants = (delegate* unmanaged<VkCommandBuffer,VkPipelineLayout,UInt32,UInt32,UInt32,void*,void>) load(device,nameof(vkCmdPushConstants)); 
			vkCmdBeginRenderPass = (delegate* unmanaged<VkCommandBuffer,VkRenderPassBeginInfo*,VkSubpassContents,void>) load(device,nameof(vkCmdBeginRenderPass)); 
			vkCmdNextSubpass = (delegate* unmanaged<VkCommandBuffer,VkSubpassContents,void>) load(device,nameof(vkCmdNextSubpass)); 
			vkCmdEndRenderPass = (delegate* unmanaged<VkCommandBuffer,void>) load(device,nameof(vkCmdEndRenderPass)); 
			vkCmdExecuteCommands = (delegate* unmanaged<VkCommandBuffer,UInt32,VkCommandBuffer*,void>) load(device,nameof(vkCmdExecuteCommands)); 
			// vkEnumeratedeviceVersion = (delegate* unmanaged<UInt32*,VkResult>) load(device,nameof(vkEnumeratedeviceVersion)); 
			vkCmdSetDeviceMask = (delegate* unmanaged<VkCommandBuffer,UInt32,void>) load(device,nameof(vkCmdSetDeviceMask)); 
			vkCmdDispatchBase = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdDispatchBase)); 
            
            
            vkAllocateCommandBuffers = (delegate* unmanaged<VkDevice,VkCommandBufferAllocateInfo*,VkCommandBuffer*,VkResult>) load(device,nameof(vkAllocateCommandBuffers)); 
			vkFreeCommandBuffers = (delegate* unmanaged<VkDevice,VkCommandPool,UInt32,VkCommandBuffer*,void>) load(device,nameof(vkFreeCommandBuffers)); 
			vkCmdBindIndexBuffer = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkIndexType,void>) load(device,nameof(vkCmdBindIndexBuffer)); 
			vkCmdBindVertexBuffers = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,void>) load(device,nameof(vkCmdBindVertexBuffers)); 
			vkCmdDrawIndirect = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndirect)); 
			vkCmdDrawIndexedIndirect = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndexedIndirect)); 
			vkCmdDispatchIndirect = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,void>) load(device,nameof(vkCmdDispatchIndirect)); 
			vkCmdUpdateBuffer = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,UInt64,void*,void>) load(device,nameof(vkCmdUpdateBuffer)); 
			vkCmdFillBuffer = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,UInt64,UInt32,void>) load(device,nameof(vkCmdFillBuffer)); 
			vkCmdCopyQueryPoolResults = (delegate* unmanaged<VkCommandBuffer,VkQueryPool,UInt32,UInt32,VkBuffer,UInt64,UInt64,UInt32,void>) load(device,nameof(vkCmdCopyQueryPoolResults)); 
			vkBindBufferMemory2 = (delegate* unmanaged<VkDevice,UInt32,VkBindBufferMemoryInfo*,VkResult>) load(device,nameof(vkBindBufferMemory2)); 
			vkBindImageMemory2 = (delegate* unmanaged<VkDevice,UInt32,VkBindImageMemoryInfo*,VkResult>) load(device,nameof(vkBindImageMemory2)); 
			vkGetDeviceGroupPeerMemoryFeatures = (delegate* unmanaged<VkDevice,UInt32,UInt32,UInt32,UInt32*,void>) load(device,nameof(vkGetDeviceGroupPeerMemoryFeatures)); 
			vkGetImageMemoryRequirements2 = (delegate* unmanaged<VkDevice,VkImageMemoryRequirementsInfo2*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetImageMemoryRequirements2)); 
			vkGetBufferMemoryRequirements2 = (delegate* unmanaged<VkDevice,VkBufferMemoryRequirementsInfo2*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetBufferMemoryRequirements2)); 
			vkGetImageSparseMemoryRequirements2 = (delegate* unmanaged<VkDevice,VkImageSparseMemoryRequirementsInfo2*,UInt32*,VkSparseImageMemoryRequirements2*,void>) load(device,nameof(vkGetImageSparseMemoryRequirements2)); 
			vkTrimCommandPool = (delegate* unmanaged<VkDevice,VkCommandPool,UInt32,void>) load(device,nameof(vkTrimCommandPool)); 
			vkGetDeviceQueue2 = (delegate* unmanaged<VkDevice,VkDeviceQueueInfo2*,VkQueue*,void>) load(device,nameof(vkGetDeviceQueue2)); 
			vkCreateSamplerYcbcrConversion = (delegate* unmanaged<VkDevice,VkSamplerYcbcrConversionCreateInfo*,VkAllocationCallbacks*,VkSamplerYcbcrConversion*,VkResult>) load(device,nameof(vkCreateSamplerYcbcrConversion)); 
			vkDestroySamplerYcbcrConversion = (delegate* unmanaged<VkDevice,VkSamplerYcbcrConversion,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroySamplerYcbcrConversion)); 
			vkCreateDescriptorUpdateTemplate = (delegate* unmanaged<VkDevice,VkDescriptorUpdateTemplateCreateInfo*,VkAllocationCallbacks*,VkDescriptorUpdateTemplate*,VkResult>) load(device,nameof(vkCreateDescriptorUpdateTemplate)); 
			vkDestroyDescriptorUpdateTemplate = (delegate* unmanaged<VkDevice,VkDescriptorUpdateTemplate,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyDescriptorUpdateTemplate)); 
			vkUpdateDescriptorSetWithTemplate = (delegate* unmanaged<VkDevice,VkDescriptorSet,VkDescriptorUpdateTemplate,void*,void>) load(device,nameof(vkUpdateDescriptorSetWithTemplate)); 
			vkGetDescriptorSetLayoutSupport = (delegate* unmanaged<VkDevice,VkDescriptorSetLayoutCreateInfo*,VkDescriptorSetLayoutSupport*,void>) load(device,nameof(vkGetDescriptorSetLayoutSupport)); 
		}
		if (VK.VK_VERSION_1_2)
		{	
			vkCmdDrawIndirectCount = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndirectCount)); 
			vkCmdDrawIndexedIndirectCount = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndexedIndirectCount)); 
			vkCreateRenderPass2 = (delegate* unmanaged<VkDevice,VkRenderPassCreateInfo2*,VkAllocationCallbacks*,VkRenderPass*,VkResult>) load(device,nameof(vkCreateRenderPass2)); 
			vkResetQueryPool = (delegate* unmanaged<VkDevice,VkQueryPool,UInt32,UInt32,void>) load(device,nameof(vkResetQueryPool)); 
			vkGetSemaphoreCounterValue = (delegate* unmanaged<VkDevice,VkSemaphore,UInt64*,VkResult>) load(device,nameof(vkGetSemaphoreCounterValue)); 
			vkWaitSemaphores = (delegate* unmanaged<VkDevice,VkSemaphoreWaitInfo*,UInt64,VkResult>) load(device,nameof(vkWaitSemaphores)); 
			vkSignalSemaphore = (delegate* unmanaged<VkDevice,VkSemaphoreSignalInfo*,VkResult>) load(device,nameof(vkSignalSemaphore)); 
		//version 1.3	
			vkCreatePrivateDataSlot = (delegate* unmanaged<VkDevice,VkPrivateDataSlotCreateInfo*,VkAllocationCallbacks*,VkPrivateDataSlot*,VkResult>) load(device,nameof(vkCreatePrivateDataSlot)); 
			vkDestroyPrivateDataSlot = (delegate* unmanaged<VkDevice,VkPrivateDataSlot,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyPrivateDataSlot)); 
			vkSetPrivateData = (delegate* unmanaged<VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64,VkResult>) load(device,nameof(vkSetPrivateData)); 
			vkGetPrivateData = (delegate* unmanaged<VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64*,void>) load(device,nameof(vkGetPrivateData)); 
			vkCmdBindVertexBuffers2 = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,UInt64*,UInt64*,void>) load(device,nameof(vkCmdBindVertexBuffers2)); 
			vkGetDeviceBufferMemoryRequirements = (delegate* unmanaged<VkDevice,VkDeviceBufferMemoryRequirements*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetDeviceBufferMemoryRequirements)); 
			vkGetDeviceImageMemoryRequirements = (delegate* unmanaged<VkDevice,VkDeviceImageMemoryRequirements*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetDeviceImageMemoryRequirements)); 
			vkGetDeviceImageSparseMemoryRequirements = (delegate* unmanaged<VkDevice,VkDeviceImageMemoryRequirements*,UInt32*,VkSparseImageMemoryRequirements2*,void>) load(device,nameof(vkGetDeviceImageSparseMemoryRequirements)); 

            vkCmdSetEvent2 = (delegate* unmanaged<VkCommandBuffer,VkEvent,VkDependencyInfo*,void>) load(device,nameof(vkCmdSetEvent2)); 
			vkCmdResetEvent2 = (delegate* unmanaged<VkCommandBuffer,VkEvent,UInt64,void>) load(device,nameof(vkCmdResetEvent2)); 
			vkCmdWaitEvents2 = (delegate* unmanaged<VkCommandBuffer,UInt32,VkEvent*,VkDependencyInfo*,void>) load(device,nameof(vkCmdWaitEvents2)); 
			vkCmdPipelineBarrier2 = (delegate* unmanaged<VkCommandBuffer,VkDependencyInfo*,void>) load(device,nameof(vkCmdPipelineBarrier2)); 
			vkCmdWriteTimestamp2 = (delegate* unmanaged<VkCommandBuffer,UInt64,VkQueryPool,UInt32,void>) load(device,nameof(vkCmdWriteTimestamp2)); 
			vkQueueSubmit2 = (delegate* unmanaged<VkQueue,UInt32,VkSubmitInfo2*,VkFence,VkResult>) load(device,nameof(vkQueueSubmit2)); 
			vkCmdCopyBuffer2 = (delegate* unmanaged<VkCommandBuffer,VkCopyBufferInfo2*,void>) load(device,nameof(vkCmdCopyBuffer2)); 
			vkCmdCopyImage2 = (delegate* unmanaged<VkCommandBuffer,VkCopyImageInfo2*,void>) load(device,nameof(vkCmdCopyImage2)); 
			vkCmdCopyBufferToImage2 = (delegate* unmanaged<VkCommandBuffer,VkCopyBufferToImageInfo2*,void>) load(device,nameof(vkCmdCopyBufferToImage2)); 
			vkCmdCopyImageToBuffer2 = (delegate* unmanaged<VkCommandBuffer,VkCopyImageToBufferInfo2*,void>) load(device,nameof(vkCmdCopyImageToBuffer2)); 
			vkCmdBlitImage2 = (delegate* unmanaged<VkCommandBuffer,VkBlitImageInfo2*,void>) load(device,nameof(vkCmdBlitImage2)); 
			vkCmdResolveImage2 = (delegate* unmanaged<VkCommandBuffer,VkResolveImageInfo2*,void>) load(device,nameof(vkCmdResolveImage2)); 
			vkCmdBeginRendering = (delegate* unmanaged<VkCommandBuffer,VkRenderingInfo*,void>) load(device,nameof(vkCmdBeginRendering)); 
			vkCmdEndRendering = (delegate* unmanaged<VkCommandBuffer,void>) load(device,nameof(vkCmdEndRendering)); 
			vkCmdSetCullMode = (delegate* unmanaged<VkCommandBuffer,UInt32,void>) load(device,nameof(vkCmdSetCullMode)); 
			vkCmdSetFrontFace = (delegate* unmanaged<VkCommandBuffer,VkFrontFace,void>) load(device,nameof(vkCmdSetFrontFace)); 
			vkCmdSetPrimitiveTopology = (delegate* unmanaged<VkCommandBuffer,VkPrimitiveTopology,void>) load(device,nameof(vkCmdSetPrimitiveTopology)); 
			vkCmdSetViewportWithCount = (delegate* unmanaged<VkCommandBuffer,UInt32,VkViewport*,void>) load(device,nameof(vkCmdSetViewportWithCount)); 
			vkCmdSetScissorWithCount = (delegate* unmanaged<VkCommandBuffer,UInt32,VkRect2D*,void>) load(device,nameof(vkCmdSetScissorWithCount)); 
			vkCmdSetDepthTestEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthTestEnable)); 
			vkCmdSetDepthWriteEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthWriteEnable)); 
			vkCmdSetDepthCompareOp = (delegate* unmanaged<VkCommandBuffer,VkCompareOp,void>) load(device,nameof(vkCmdSetDepthCompareOp)); 
			vkCmdSetDepthBoundsTestEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthBoundsTestEnable)); 
			vkCmdSetStencilTestEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetStencilTestEnable)); 
			vkCmdSetStencilOp = (delegate* unmanaged<VkCommandBuffer,UInt32,VkStencilOp,VkStencilOp,VkStencilOp,VkCompareOp,void>) load(device,nameof(vkCmdSetStencilOp)); 
			vkCmdSetRasterizerDiscardEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetRasterizerDiscardEnable)); 
			vkCmdSetDepthBiasEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthBiasEnable)); 
			vkCmdSetPrimitiveRestartEnable = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetPrimitiveRestartEnable)); 

            vkCmdBeginRenderPass2 = (delegate* unmanaged<VkCommandBuffer,VkRenderPassBeginInfo*,VkSubpassBeginInfo*,void>) load(device,nameof(vkCmdBeginRenderPass2)); 
			vkCmdNextSubpass2 = (delegate* unmanaged<VkCommandBuffer,VkSubpassBeginInfo*,VkSubpassEndInfo*,void>) load(device,nameof(vkCmdNextSubpass2)); 
			vkCmdEndRenderPass2 = (delegate* unmanaged<VkCommandBuffer,VkSubpassEndInfo*,void>) load(device,nameof(vkCmdEndRenderPass2)); 
		}
		if (VK.VK_KHR_surface){
		}
        if (VK.VK_KHR_dynamic_rendering){
			vkCmdBeginRenderingKHR = (delegate* unmanaged<VkCommandBuffer,VkRenderingInfo*,void>) load(device,nameof(vkCmdBeginRenderingKHR)); 
			vkCmdEndRenderingKHR = (delegate* unmanaged<VkCommandBuffer,void>) load(device,nameof(vkCmdEndRenderingKHR)); 
		}

		if (VK.VK_KHR_swapchain){
			vkQueuePresentKHR = (delegate* unmanaged<VkQueue,VkPresentInfoKHR*,VkResult>) load(device,nameof(vkQueuePresentKHR)); 
			vkCreateSwapchainKHR = (delegate* unmanaged<VkDevice,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult>) load(device,nameof(vkCreateSwapchainKHR)); 
			vkDestroySwapchainKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroySwapchainKHR)); 
			vkGetSwapchainImagesKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,UInt32*,VkImage*,VkResult>) load(device,nameof(vkGetSwapchainImagesKHR)); 
			vkAcquireNextImageKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,UInt64,VkSemaphore,VkFence,UInt32*,VkResult>) load(device,nameof(vkAcquireNextImageKHR)); 
			vkGetDeviceGroupPresentCapabilitiesKHR = (delegate* unmanaged<VkDevice,VkDeviceGroupPresentCapabilitiesKHR*,VkResult>) load(device,nameof(vkGetDeviceGroupPresentCapabilitiesKHR)); 
			vkGetDeviceGroupSurfacePresentModesKHR = (delegate* unmanaged<VkDevice,VkSurfaceKHR,UInt32*,VkResult>) load(device,nameof(vkGetDeviceGroupSurfacePresentModesKHR)); 
			vkAcquireNextImage2KHR = (delegate* unmanaged<VkDevice,VkAcquireNextImageInfoKHR*,UInt32*,VkResult>) load(device,nameof(vkAcquireNextImage2KHR)); 
		}
		if (VK.VK_KHR_display_swapchain){
			vkCreateSharedSwapchainsKHR = (delegate* unmanaged<VkDevice,UInt32,VkSwapchainCreateInfoKHR*,VkAllocationCallbacks*,VkSwapchainKHR*,VkResult>) load(device,nameof(vkCreateSharedSwapchainsKHR)); 
		}
		if (VK.VK_KHR_device_group){
			vkGetDeviceGroupPeerMemoryFeaturesKHR = (delegate* unmanaged<VkDevice,UInt32,UInt32,UInt32,UInt32*,void>) load(device,nameof(vkGetDeviceGroupPeerMemoryFeaturesKHR)); 
		}
		if (VK.VK_KHR_maintenance1){
			vkTrimCommandPoolKHR = (delegate* unmanaged<VkDevice,VkCommandPool,UInt32,void>) load(device,nameof(vkTrimCommandPoolKHR)); 
		}
		if (VK.VK_KHR_external_memory_fd){
			vkGetMemoryFdKHR = (delegate* unmanaged<VkDevice,VkMemoryGetFdInfoKHR*,int*,VkResult>) load(device,nameof(vkGetMemoryFdKHR)); 
			vkGetMemoryFdPropertiesKHR = (delegate* unmanaged<VkDevice,VkExternalMemoryHandleTypeFlagBits,int,VkMemoryFdPropertiesKHR*,VkResult>) load(device,nameof(vkGetMemoryFdPropertiesKHR)); 
		}
		if (VK.VK_KHR_external_semaphore_fd){
			vkImportSemaphoreFdKHR = (delegate* unmanaged<VkDevice,VkImportSemaphoreFdInfoKHR*,VkResult>) load(device,nameof(vkImportSemaphoreFdKHR)); 
			vkGetSemaphoreFdKHR = (delegate* unmanaged<VkDevice,VkSemaphoreGetFdInfoKHR*,int*,VkResult>) load(device,nameof(vkGetSemaphoreFdKHR)); 
		}
		if (VK.VK_KHR_descriptor_update_template){
			vkCreateDescriptorUpdateTemplateKHR = (delegate* unmanaged<VkDevice,VkDescriptorUpdateTemplateCreateInfo*,VkAllocationCallbacks*,VkDescriptorUpdateTemplate*,VkResult>) load(device,nameof(vkCreateDescriptorUpdateTemplateKHR)); 
			vkDestroyDescriptorUpdateTemplateKHR = (delegate* unmanaged<VkDevice,VkDescriptorUpdateTemplate,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyDescriptorUpdateTemplateKHR)); 
			vkUpdateDescriptorSetWithTemplateKHR = (delegate* unmanaged<VkDevice,VkDescriptorSet,VkDescriptorUpdateTemplate,void*,void>) load(device,nameof(vkUpdateDescriptorSetWithTemplateKHR)); 
		}
		if (VK.VK_KHR_create_renderpass2){
			vkCreateRenderPass2KHR = (delegate* unmanaged<VkDevice,VkRenderPassCreateInfo2*,VkAllocationCallbacks*,VkRenderPass*,VkResult>) load(device,nameof(vkCreateRenderPass2KHR)); 
		}
		if (VK.VK_KHR_shared_presentable_image){
			vkGetSwapchainStatusKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,VkResult>) load(device,nameof(vkGetSwapchainStatusKHR)); 
		}
		if (VK.VK_KHR_external_fence_fd){
			vkImportFenceFdKHR = (delegate* unmanaged<VkDevice,VkImportFenceFdInfoKHR*,VkResult>) load(device,nameof(vkImportFenceFdKHR)); 
			vkGetFenceFdKHR = (delegate* unmanaged<VkDevice,VkFenceGetFdInfoKHR*,int*,VkResult>) load(device,nameof(vkGetFenceFdKHR)); 
		}
		if (VK.VK_KHR_performance_query){
			vkAcquireProfilingLockKHR = (delegate* unmanaged<VkDevice,VkAcquireProfilingLockInfoKHR*,VkResult>) load(device,nameof(vkAcquireProfilingLockKHR)); 
			vkReleaseProfilingLockKHR = (delegate* unmanaged<VkDevice,void>) load(device,nameof(vkReleaseProfilingLockKHR)); 
		}
		if (VK.VK_KHR_get_memory_requirements2){
			vkGetImageMemoryRequirements2KHR = (delegate* unmanaged<VkDevice,VkImageMemoryRequirementsInfo2*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetImageMemoryRequirements2KHR)); 
			vkGetBufferMemoryRequirements2KHR = (delegate* unmanaged<VkDevice,VkBufferMemoryRequirementsInfo2*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetBufferMemoryRequirements2KHR)); 
			vkGetImageSparseMemoryRequirements2KHR = (delegate* unmanaged<VkDevice,VkImageSparseMemoryRequirementsInfo2*,UInt32*,VkSparseImageMemoryRequirements2*,void>) load(device,nameof(vkGetImageSparseMemoryRequirements2KHR)); 
		}
		if (VK.VK_KHR_image_format_list){
		}
		if (VK.VK_KHR_sampler_ycbcr_conversion){
			vkCreateSamplerYcbcrConversionKHR = (delegate* unmanaged<VkDevice,VkSamplerYcbcrConversionCreateInfo*,VkAllocationCallbacks*,VkSamplerYcbcrConversion*,VkResult>) load(device,nameof(vkCreateSamplerYcbcrConversionKHR)); 
			vkDestroySamplerYcbcrConversionKHR = (delegate* unmanaged<VkDevice,VkSamplerYcbcrConversion,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroySamplerYcbcrConversionKHR)); 
		}
		if (VK.VK_KHR_bind_memory2){
			vkBindBufferMemory2KHR = (delegate* unmanaged<VkDevice,UInt32,VkBindBufferMemoryInfo*,VkResult>) load(device,nameof(vkBindBufferMemory2KHR)); 
			vkBindImageMemory2KHR = (delegate* unmanaged<VkDevice,UInt32,VkBindImageMemoryInfo*,VkResult>) load(device,nameof(vkBindImageMemory2KHR)); 
		}
		if (VK.VK_KHR_maintenance3){
			vkGetDescriptorSetLayoutSupportKHR = (delegate* unmanaged<VkDevice,VkDescriptorSetLayoutCreateInfo*,VkDescriptorSetLayoutSupport*,void>) load(device,nameof(vkGetDescriptorSetLayoutSupportKHR)); 
		}
		if (VK.VK_KHR_draw_indirect_count){
			vkCmdDrawIndirectCountKHR = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndirectCountKHR)); 
			vkCmdDrawIndexedIndirectCountKHR = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndexedIndirectCountKHR)); 
		}
		if (VK.VK_KHR_timeline_semaphore){
			vkGetSemaphoreCounterValueKHR = (delegate* unmanaged<VkDevice,VkSemaphore,UInt64*,VkResult>) load(device,nameof(vkGetSemaphoreCounterValueKHR)); 
			vkWaitSemaphoresKHR = (delegate* unmanaged<VkDevice,VkSemaphoreWaitInfo*,UInt64,VkResult>) load(device,nameof(vkWaitSemaphoresKHR)); 
			vkSignalSemaphoreKHR = (delegate* unmanaged<VkDevice,VkSemaphoreSignalInfo*,VkResult>) load(device,nameof(vkSignalSemaphoreKHR)); 
		}
		if (VK.VK_KHR_present_wait){
			vkWaitForPresentKHR = (delegate* unmanaged<VkDevice,VkSwapchainKHR,UInt64,UInt64,VkResult>) load(device,nameof(vkWaitForPresentKHR)); 
		}
		if (VK.VK_KHR_deferred_host_operations){
			vkCreateDeferredOperationKHR = (delegate* unmanaged<VkDevice,VkAllocationCallbacks*,VkDeferredOperationKHR*,VkResult>) load(device,nameof(vkCreateDeferredOperationKHR)); 
			vkDestroyDeferredOperationKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyDeferredOperationKHR)); 
			vkGetDeferredOperationResultKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkResult>) load(device,nameof(vkGetDeferredOperationResultKHR)); 
			vkDeferredOperationJoinKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkResult>) load(device,nameof(vkDeferredOperationJoinKHR)); 
		}
		if (VK.VK_KHR_pipeline_executable_properties){
			vkGetPipelineExecutablePropertiesKHR = (delegate* unmanaged<VkDevice,VkPipelineInfoKHR*,UInt32*,VkPipelineExecutablePropertiesKHR*,VkResult>) load(device,nameof(vkGetPipelineExecutablePropertiesKHR)); 
			// vkGetPipelineExecutableStatisticsKHR = (delegate* unmanaged<VkDevice,VkPipelineExecutableInfoKHR*,UInt32*,VkPipelineExecutableStatisticKHR*,VkResult>) load(device,nameof(vkGetPipelineExecutableStatisticsKHR)); 
			vkGetPipelineExecutableInternalRepresentationsKHR = (delegate* unmanaged<VkDevice,VkPipelineExecutableInfoKHR*,UInt32*,VkPipelineExecutableInternalRepresentationKHR*,VkResult>) load(device,nameof(vkGetPipelineExecutableInternalRepresentationsKHR)); 
		}
		if (VK.VK_KHR_synchronization2){
			vkCmdWriteBufferMarker2AMD = (delegate* unmanaged<VkCommandBuffer,UInt64,VkBuffer,UInt64,UInt32,void>) load(device,nameof(vkCmdWriteBufferMarker2AMD)); 
		}
		if (VK.VK_KHR_ray_tracing_maintenance1){
			vkCmdTraceRaysIndirect2KHR = (delegate* unmanaged<VkCommandBuffer,UInt64,void>) load(device,nameof(vkCmdTraceRaysIndirect2KHR)); 
		}
		if (VK.VK_KHR_maintenance4){
			vkGetDeviceBufferMemoryRequirementsKHR = (delegate* unmanaged<VkDevice,VkDeviceBufferMemoryRequirements*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetDeviceBufferMemoryRequirementsKHR)); 
			vkGetDeviceImageMemoryRequirementsKHR = (delegate* unmanaged<VkDevice,VkDeviceImageMemoryRequirements*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetDeviceImageMemoryRequirementsKHR)); 
			vkGetDeviceImageSparseMemoryRequirementsKHR = (delegate* unmanaged<VkDevice,VkDeviceImageMemoryRequirements*,UInt32*,VkSparseImageMemoryRequirements2*,void>) load(device,nameof(vkGetDeviceImageSparseMemoryRequirementsKHR)); 
		}
		if (VK.VK_EXT_debug_marker){
			vkDebugMarkerSetObjectTagEXT = (delegate* unmanaged<VkDevice,VkDebugMarkerObjectTagInfoEXT*,VkResult>) load(device,nameof(vkDebugMarkerSetObjectTagEXT)); 
			vkDebugMarkerSetObjectNameEXT = (delegate* unmanaged<VkDevice,VkDebugMarkerObjectNameInfoEXT*,VkResult>) load(device,nameof(vkDebugMarkerSetObjectNameEXT)); 
		}
		if (VK.VK_EXT_transform_feedback){
			vkCmdBindTransformFeedbackBuffersEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,UInt64*,void>) load(device,nameof(vkCmdBindTransformFeedbackBuffersEXT)); 
			vkCmdBeginTransformFeedbackEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,void>) load(device,nameof(vkCmdBeginTransformFeedbackEXT)); 
			vkCmdEndTransformFeedbackEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,void>) load(device,nameof(vkCmdEndTransformFeedbackEXT)); 
			vkCmdDrawIndirectByteCountEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndirectByteCountEXT)); 
		}
		if (VK.VK_NVX_binary_import){
			vkCreateCuModuleNVX = (delegate* unmanaged<VkDevice,VkCuModuleCreateInfoNVX*,VkAllocationCallbacks*,VkCuModuleNVX*,VkResult>) load(device,nameof(vkCreateCuModuleNVX)); 
			vkCreateCuFunctionNVX = (delegate* unmanaged<VkDevice,VkCuFunctionCreateInfoNVX*,VkAllocationCallbacks*,VkCuFunctionNVX*,VkResult>) load(device,nameof(vkCreateCuFunctionNVX)); 
			vkDestroyCuModuleNVX = (delegate* unmanaged<VkDevice,VkCuModuleNVX,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyCuModuleNVX)); 
			vkDestroyCuFunctionNVX = (delegate* unmanaged<VkDevice,VkCuFunctionNVX,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyCuFunctionNVX)); 
		}
		if (VK.VK_NVX_image_view_handle){
			vkGetImageViewAddressNVX = (delegate* unmanaged<VkDevice,VkImageView,VkImageViewAddressPropertiesNVX*,VkResult>) load(device,nameof(vkGetImageViewAddressNVX)); 
		}
		if (VK.VK_AMD_draw_indirect_count){
			vkCmdDrawIndirectCountAMD = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndirectCountAMD)); 
			vkCmdDrawIndexedIndirectCountAMD = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawIndexedIndirectCountAMD)); 
		}
		if (VK.VK_AMD_shader_info){
			vkGetShaderInfoAMD = (delegate* unmanaged<VkDevice,VkPipeline,VkShaderStageFlagBits,VkShaderInfoTypeAMD,Int32*,void*,VkResult>) load(device,nameof(vkGetShaderInfoAMD)); 
		}
		if (VK.VK_EXT_display_control){
			vkDisplayPowerControlEXT = (delegate* unmanaged<VkDevice,VkDisplayKHR,VkDisplayPowerInfoEXT*,VkResult>) load(device,nameof(vkDisplayPowerControlEXT)); 
			vkRegisterDeviceEventEXT = (delegate* unmanaged<VkDevice,VkDeviceEventInfoEXT*,VkAllocationCallbacks*,VkFence*,VkResult>) load(device,nameof(vkRegisterDeviceEventEXT)); 
			vkRegisterDisplayEventEXT = (delegate* unmanaged<VkDevice,VkDisplayKHR,VkDisplayEventInfoEXT*,VkAllocationCallbacks*,VkFence*,VkResult>) load(device,nameof(vkRegisterDisplayEventEXT)); 
			vkGetSwapchainCounterEXT = (delegate* unmanaged<VkDevice,VkSwapchainKHR,VkSurfaceCounterFlagBitsEXT,UInt64*,VkResult>) load(device,nameof(vkGetSwapchainCounterEXT)); 
		}
		if (VK.VK_GOOGLE_display_timing){
			vkGetRefreshCycleDurationGOOGLE = (delegate* unmanaged<VkDevice,VkSwapchainKHR,VkRefreshCycleDurationGOOGLE*,VkResult>) load(device,nameof(vkGetRefreshCycleDurationGOOGLE)); 
			vkGetPastPresentationTimingGOOGLE = (delegate* unmanaged<VkDevice,VkSwapchainKHR,UInt32*,VkPastPresentationTimingGOOGLE*,VkResult>) load(device,nameof(vkGetPastPresentationTimingGOOGLE)); 
		}
		if (VK.VK_EXT_hdr_metadata){
			vkSetHdrMetadataEXT = (delegate* unmanaged<VkDevice,UInt32,VkSwapchainKHR*,VkHdrMetadataEXT*,void>) load(device,nameof(vkSetHdrMetadataEXT)); 
		}
		if (VK.VK_EXT_debug_utils){
			vkSetDebugUtilsObjectNameEXT = (delegate* unmanaged<VkDevice,VkDebugUtilsObjectNameInfoEXT*,VkResult>) load(device,nameof(vkSetDebugUtilsObjectNameEXT)); 
			vkSetDebugUtilsObjectTagEXT = (delegate* unmanaged<VkDevice,VkDebugUtilsObjectTagInfoEXT*,VkResult>) load(device,nameof(vkSetDebugUtilsObjectTagEXT)); 
		}
		if (VK.VK_EXT_image_drm_format_modifier){
			vkGetImageDrmFormatModifierPropertiesEXT = (delegate* unmanaged<VkDevice,VkImage,VkImageDrmFormatModifierPropertiesEXT*,VkResult>) load(device,nameof(vkGetImageDrmFormatModifierPropertiesEXT)); 
		}
		if (VK.VK_EXT_validation_cache){
			vkCreateValidationCacheEXT = (delegate* unmanaged<VkDevice,VkValidationCacheCreateInfoEXT*,VkAllocationCallbacks*,VkValidationCacheEXT*,VkResult>) load(device,nameof(vkCreateValidationCacheEXT)); 
			vkDestroyValidationCacheEXT = (delegate* unmanaged<VkDevice,VkValidationCacheEXT,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyValidationCacheEXT)); 
			vkMergeValidationCachesEXT = (delegate* unmanaged<VkDevice,VkValidationCacheEXT,UInt32,VkValidationCacheEXT*,VkResult>) load(device,nameof(vkMergeValidationCachesEXT)); 
			vkGetValidationCacheDataEXT = (delegate* unmanaged<VkDevice,VkValidationCacheEXT,Int32*,void*,VkResult>) load(device,nameof(vkGetValidationCacheDataEXT)); 
		}
		if (VK.VK_NV_ray_tracing){
			vkCreateAccelerationStructureNV = (delegate* unmanaged<VkDevice,VkAccelerationStructureCreateInfoNV*,VkAllocationCallbacks*,VkAccelerationStructureNV*,VkResult>) load(device,nameof(vkCreateAccelerationStructureNV)); 
			vkDestroyAccelerationStructureNV = (delegate* unmanaged<VkDevice,VkAccelerationStructureNV,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyAccelerationStructureNV)); 
			vkGetAccelerationStructureMemoryRequirementsNV = (delegate* unmanaged<VkDevice,VkAccelerationStructureMemoryRequirementsInfoNV*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetAccelerationStructureMemoryRequirementsNV)); 
			vkBindAccelerationStructureMemoryNV = (delegate* unmanaged<VkDevice,UInt32,VkBindAccelerationStructureMemoryInfoNV*,VkResult>) load(device,nameof(vkBindAccelerationStructureMemoryNV)); 
			vkCmdBuildAccelerationStructureNV = (delegate* unmanaged<VkCommandBuffer,VkAccelerationStructureInfoNV*,VkBuffer,UInt64,uint,VkAccelerationStructureNV,VkAccelerationStructureNV,VkBuffer,UInt64,void>) load(device,nameof(vkCmdBuildAccelerationStructureNV)); 
			vkCmdTraceRaysNV = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt64,VkBuffer,UInt64,UInt64,VkBuffer,UInt64,UInt64,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdTraceRaysNV)); 
			vkCreateRayTracingPipelinesNV = (delegate* unmanaged<VkDevice,VkPipelineCache,UInt32,VkRayTracingPipelineCreateInfoNV*,VkAllocationCallbacks*,VkPipeline*,VkResult>) load(device,nameof(vkCreateRayTracingPipelinesNV)); 
			vkGetRayTracingShaderGroupHandlesKHR = (delegate* unmanaged<VkDevice,VkPipeline,UInt32,UInt32,Int32,void*,VkResult>) load(device,nameof(vkGetRayTracingShaderGroupHandlesKHR)); 
			vkGetRayTracingShaderGroupHandlesNV = (delegate* unmanaged<VkDevice,VkPipeline,UInt32,UInt32,Int32,void*,VkResult>) load(device,nameof(vkGetRayTracingShaderGroupHandlesNV)); 
			vkGetAccelerationStructureHandleNV = (delegate* unmanaged<VkDevice,VkAccelerationStructureNV,Int32,void*,VkResult>) load(device,nameof(vkGetAccelerationStructureHandleNV)); 
			vkCompileDeferredNV = (delegate* unmanaged<VkDevice,VkPipeline,UInt32,VkResult>) load(device,nameof(vkCompileDeferredNV)); 
		}
		if (VK.VK_EXT_external_memory_host){
			vkGetMemoryHostPointerPropertiesEXT = (delegate* unmanaged<VkDevice,VkExternalMemoryHandleTypeFlagBits,void*,VkMemoryHostPointerPropertiesEXT*,VkResult>) load(device,nameof(vkGetMemoryHostPointerPropertiesEXT)); 
		}
		if (VK.VK_AMD_buffer_marker){
			vkCmdWriteBufferMarkerAMD = (delegate* unmanaged<VkCommandBuffer,VkPipelineStageFlagBits,VkBuffer,UInt64,UInt32,void>) load(device,nameof(vkCmdWriteBufferMarkerAMD)); 
		}
		if (VK.VK_EXT_calibrated_timestamps){
			vkGetCalibratedTimestampsEXT = (delegate* unmanaged<VkDevice,UInt32,VkCalibratedTimestampInfoEXT*,UInt64*,UInt64*,VkResult>) load(device,nameof(vkGetCalibratedTimestampsEXT)); 
		}
		if (VK.VK_NV_mesh_shader){
			vkCmdDrawMeshTasksIndirectNV = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawMeshTasksIndirectNV)); 
			vkCmdDrawMeshTasksIndirectCountNV = (delegate* unmanaged<VkCommandBuffer,VkBuffer,UInt64,VkBuffer,UInt64,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawMeshTasksIndirectCountNV)); 
		}
		if (VK.VK_INTEL_performance_query){
			vkInitializePerformanceApiINTEL = (delegate* unmanaged<VkDevice,VkInitializePerformanceApiInfoINTEL*,VkResult>) load(device,nameof(vkInitializePerformanceApiINTEL)); 
			vkUninitializePerformanceApiINTEL = (delegate* unmanaged<VkDevice,void>) load(device,nameof(vkUninitializePerformanceApiINTEL)); 
			vkAcquirePerformanceConfigurationINTEL = (delegate* unmanaged<VkDevice,VkPerformanceConfigurationAcquireInfoINTEL*,VkPerformanceConfigurationINTEL*,VkResult>) load(device,nameof(vkAcquirePerformanceConfigurationINTEL)); 
			vkReleasePerformanceConfigurationINTEL = (delegate* unmanaged<VkDevice,VkPerformanceConfigurationINTEL,VkResult>) load(device,nameof(vkReleasePerformanceConfigurationINTEL)); 
			vkGetPerformanceParameterINTEL = (delegate* unmanaged<VkDevice,VkPerformanceParameterTypeINTEL,VkPerformanceValueINTEL*,VkResult>) load(device,nameof(vkGetPerformanceParameterINTEL)); 
		}
		if (VK.VK_AMD_display_native_hdr){
			vkSetLocalDimmingAMD = (delegate* unmanaged<VkDevice,VkSwapchainKHR,uint,void>) load(device,nameof(vkSetLocalDimmingAMD)); 
		}
		if (VK.VK_EXT_host_query_reset){
			vkResetQueryPoolEXT = (delegate* unmanaged<VkDevice,VkQueryPool,UInt32,UInt32,void>) load(device,nameof(vkResetQueryPoolEXT)); 
		}
		if (VK.VK_EXT_extended_dynamic_state){
			vkCmdBindVertexBuffers2EXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkBuffer*,UInt64*,UInt64*,UInt64*,void>) load(device,nameof(vkCmdBindVertexBuffers2EXT)); 
		}
		if (VK.VK_NV_device_generated_commands){
			vkGetGeneratedCommandsMemoryRequirementsNV = (delegate* unmanaged<VkDevice,VkGeneratedCommandsMemoryRequirementsInfoNV*,VkMemoryRequirements2*,void>) load(device,nameof(vkGetGeneratedCommandsMemoryRequirementsNV)); 
			vkCreateIndirectCommandsLayoutNV = (delegate* unmanaged<VkDevice,VkIndirectCommandsLayoutCreateInfoNV*,VkAllocationCallbacks*,VkIndirectCommandsLayoutNV*,VkResult>) load(device,nameof(vkCreateIndirectCommandsLayoutNV)); 
			vkDestroyIndirectCommandsLayoutNV = (delegate* unmanaged<VkDevice,VkIndirectCommandsLayoutNV,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyIndirectCommandsLayoutNV)); 
		}
		if (VK.VK_EXT_private_data){
			vkCreatePrivateDataSlotEXT = (delegate* unmanaged<VkDevice,VkPrivateDataSlotCreateInfo*,VkAllocationCallbacks*,VkPrivateDataSlot*,VkResult>) load(device,nameof(vkCreatePrivateDataSlotEXT)); 
			vkDestroyPrivateDataSlotEXT = (delegate* unmanaged<VkDevice,VkPrivateDataSlot,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyPrivateDataSlotEXT)); 
			vkSetPrivateDataEXT = (delegate* unmanaged<VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64,VkResult>) load(device,nameof(vkSetPrivateDataEXT)); 
			vkGetPrivateDataEXT = (delegate* unmanaged<VkDevice,VkObjectType,UInt64,VkPrivateDataSlot,UInt64*,void>) load(device,nameof(vkGetPrivateDataEXT)); 
		}
		if (VK.VK_EXT_image_compression_control){
			vkGetImageSubresourceLayout2EXT = (delegate* unmanaged<VkDevice,VkImage,VkImageSubresource2EXT*,VkSubresourceLayout2EXT*,void>) load(device,nameof(vkGetImageSubresourceLayout2EXT)); 
		}
		if (VK.VK_HUAWEI_subpass_shading){
			vkGetDeviceSubpassShadingMaxWorkgroupSizeHUAWEI = (delegate* unmanaged<VkDevice,VkRenderPass,VkExtent2D*,VkResult>) load(device,nameof(vkGetDeviceSubpassShadingMaxWorkgroupSizeHUAWEI)); 
		}
		if (VK.VK_NV_external_memory_rdma){
			vkGetMemoryRemoteAddressNV = (delegate* unmanaged<VkDevice,VkMemoryGetRemoteAddressInfoNV*,void*,VkResult>) load(device,nameof(vkGetMemoryRemoteAddressNV)); 
		}
		if (VK.VK_EXT_pipeline_properties){
			vkGetPipelinePropertiesEXT = (delegate* unmanaged<VkDevice,VkPipelineInfoKHR*,VkBaseOutStructure*,VkResult>) load(device,nameof(vkGetPipelinePropertiesEXT)); 
		}
		if (VK.VK_EXT_pageable_device_local_memory){
			vkSetDeviceMemoryPriorityEXT = (delegate* unmanaged<VkDevice,VkDeviceMemory,float,void>) load(device,nameof(vkSetDeviceMemoryPriorityEXT)); 
		}
		if (VK.VK_VALVE_descriptor_set_host_mapping){
			vkGetDescriptorSetLayoutHostMappingInfoVALVE = (delegate* unmanaged<VkDevice,VkDescriptorSetBindingReferenceVALVE*,VkDescriptorSetLayoutHostMappingInfoVALVE*,void>) load(device,nameof(vkGetDescriptorSetLayoutHostMappingInfoVALVE)); 
			vkGetDescriptorSetHostMappingVALVE = (delegate* unmanaged<VkDevice,VkDescriptorSet,void**,void>) load(device,nameof(vkGetDescriptorSetHostMappingVALVE)); 
		}
		if (VK.VK_KHR_acceleration_structure){
			vkCreateAccelerationStructureKHR = (delegate* unmanaged<VkDevice,VkAccelerationStructureCreateInfoKHR*,VkAllocationCallbacks*,VkAccelerationStructureKHR*,VkResult>) load(device,nameof(vkCreateAccelerationStructureKHR)); 
			vkDestroyAccelerationStructureKHR = (delegate* unmanaged<VkDevice,VkAccelerationStructureKHR,VkAllocationCallbacks*,void>) load(device,nameof(vkDestroyAccelerationStructureKHR)); 
			vkCmdBuildAccelerationStructuresIndirectKHR = (delegate* unmanaged<VkCommandBuffer,UInt32,VkAccelerationStructureBuildGeometryInfoKHR*,UInt64*,UInt32*,UInt32*,void>) load(device,nameof(vkCmdBuildAccelerationStructuresIndirectKHR)); 
			vkBuildAccelerationStructuresKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,UInt32,VkAccelerationStructureBuildGeometryInfoKHR*,VkAccelerationStructureBuildRangeInfoKHR*,VkResult>) load(device,nameof(vkBuildAccelerationStructuresKHR)); 
			vkCopyAccelerationStructureKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkCopyAccelerationStructureInfoKHR*,VkResult>) load(device,nameof(vkCopyAccelerationStructureKHR)); 
			vkCopyAccelerationStructureToMemoryKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkCopyAccelerationStructureToMemoryInfoKHR*,VkResult>) load(device,nameof(vkCopyAccelerationStructureToMemoryKHR)); 
			vkCopyMemoryToAccelerationStructureKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkCopyMemoryToAccelerationStructureInfoKHR*,VkResult>) load(device,nameof(vkCopyMemoryToAccelerationStructureKHR)); 
			vkWriteAccelerationStructuresPropertiesKHR = (delegate* unmanaged<VkDevice,UInt32,VkAccelerationStructureKHR*,VkQueryType,Int32,void*,Int32,VkResult>) load(device,nameof(vkWriteAccelerationStructuresPropertiesKHR)); 
			vkGetDeviceAccelerationStructureCompatibilityKHR = (delegate* unmanaged<VkDevice,VkAccelerationStructureVersionInfoKHR*,VkAccelerationStructureCompatibilityKHR*,void>) load(device,nameof(vkGetDeviceAccelerationStructureCompatibilityKHR)); 
			vkGetAccelerationStructureBuildSizesKHR = (delegate* unmanaged<VkDevice,VkAccelerationStructureBuildTypeKHR,VkAccelerationStructureBuildGeometryInfoKHR*,UInt32*,VkAccelerationStructureBuildSizesInfoKHR*,void>) load(device,nameof(vkGetAccelerationStructureBuildSizesKHR)); 
		}
		if (VK.VK_KHR_ray_tracing_pipeline){
			vkCreateRayTracingPipelinesKHR = (delegate* unmanaged<VkDevice,VkDeferredOperationKHR,VkPipelineCache,UInt32,VkRayTracingPipelineCreateInfoKHR*,VkAllocationCallbacks*,VkPipeline*,VkResult>) load(device,nameof(vkCreateRayTracingPipelinesKHR)); 
			vkGetRayTracingCaptureReplayShaderGroupHandlesKHR = (delegate* unmanaged<VkDevice,VkPipeline,UInt32,UInt32,Int32,void*,VkResult>) load(device,nameof(vkGetRayTracingCaptureReplayShaderGroupHandlesKHR)); 
			vkCmdTraceRaysIndirectKHR = (delegate* unmanaged<VkCommandBuffer,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,UInt64,void>) load(device,nameof(vkCmdTraceRaysIndirectKHR)); 
		}
           if (VK.VK_KHR_device_group){
			vkCmdSetDeviceMaskKHR = (delegate* unmanaged<VkCommandBuffer,UInt32,void>) load(device,nameof(vkCmdSetDeviceMaskKHR)); 
			vkCmdDispatchBaseKHR = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,UInt32,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdDispatchBaseKHR)); 
		}
		if (VK.VK_KHR_push_descriptor){
			vkCmdPushDescriptorSetKHR = (delegate* unmanaged<VkCommandBuffer,VkPipelineBindPoint,VkPipelineLayout,UInt32,UInt32,VkWriteDescriptorSet*,void>) load(device,nameof(vkCmdPushDescriptorSetKHR)); 
			vkCmdPushDescriptorSetWithTemplateKHR = (delegate* unmanaged<VkCommandBuffer,VkDescriptorUpdateTemplate,VkPipelineLayout,UInt32,void*,void>) load(device,nameof(vkCmdPushDescriptorSetWithTemplateKHR)); 
		}
		if (VK.VK_KHR_create_renderpass2){
			vkCmdBeginRenderPass2KHR = (delegate* unmanaged<VkCommandBuffer,VkRenderPassBeginInfo*,VkSubpassBeginInfo*,void>) load(device,nameof(vkCmdBeginRenderPass2KHR)); 
			vkCmdNextSubpass2KHR = (delegate* unmanaged<VkCommandBuffer,VkSubpassBeginInfo*,VkSubpassEndInfo*,void>) load(device,nameof(vkCmdNextSubpass2KHR)); 
			vkCmdEndRenderPass2KHR = (delegate* unmanaged<VkCommandBuffer,VkSubpassEndInfo*,void>) load(device,nameof(vkCmdEndRenderPass2KHR)); 
		}
		if (VK.VK_KHR_synchronization2){
			vkCmdSetEvent2KHR = (delegate* unmanaged<VkCommandBuffer,VkEvent,VkDependencyInfo*,void>) load(device,nameof(vkCmdSetEvent2KHR)); 
			vkCmdResetEvent2KHR = (delegate* unmanaged<VkCommandBuffer,VkEvent,UInt64,void>) load(device,nameof(vkCmdResetEvent2KHR)); 
			vkCmdWaitEvents2KHR = (delegate* unmanaged<VkCommandBuffer,UInt32,VkEvent*,VkDependencyInfo*,void>) load(device,nameof(vkCmdWaitEvents2KHR)); 
			vkCmdPipelineBarrier2KHR = (delegate* unmanaged<VkCommandBuffer,VkDependencyInfo*,void>) load(device,nameof(vkCmdPipelineBarrier2KHR)); 
			vkCmdWriteTimestamp2KHR = (delegate* unmanaged<VkCommandBuffer,UInt64,VkQueryPool,UInt32,void>) load(device,nameof(vkCmdWriteTimestamp2KHR)); 
			vkQueueSubmit2KHR = (delegate* unmanaged<VkQueue,UInt32,VkSubmitInfo2*,VkFence,VkResult>) load(device,nameof(vkQueueSubmit2KHR)); 
			vkGetQueueCheckpointData2NV = (delegate* unmanaged<VkQueue,UInt32*,VkCheckpointData2NV*,void>) load(device,nameof(vkGetQueueCheckpointData2NV)); 
		}
		if (VK.VK_KHR_copy_commands2){
			vkCmdCopyBuffer2KHR = (delegate* unmanaged<VkCommandBuffer,VkCopyBufferInfo2*,void>) load(device,nameof(vkCmdCopyBuffer2KHR)); 
			vkCmdCopyImage2KHR = (delegate* unmanaged<VkCommandBuffer,VkCopyImageInfo2*,void>) load(device,nameof(vkCmdCopyImage2KHR)); 
			vkCmdCopyBufferToImage2KHR = (delegate* unmanaged<VkCommandBuffer,VkCopyBufferToImageInfo2*,void>) load(device,nameof(vkCmdCopyBufferToImage2KHR)); 
			vkCmdCopyImageToBuffer2KHR = (delegate* unmanaged<VkCommandBuffer,VkCopyImageToBufferInfo2*,void>) load(device,nameof(vkCmdCopyImageToBuffer2KHR)); 
			vkCmdBlitImage2KHR = (delegate* unmanaged<VkCommandBuffer,VkBlitImageInfo2*,void>) load(device,nameof(vkCmdBlitImage2KHR)); 
			vkCmdResolveImage2KHR = (delegate* unmanaged<VkCommandBuffer,VkResolveImageInfo2*,void>) load(device,nameof(vkCmdResolveImage2KHR)); 
		}
		if (VK.VK_EXT_debug_marker){
			vkCmdDebugMarkerBeginEXT = (delegate* unmanaged<VkCommandBuffer,VkDebugMarkerMarkerInfoEXT*,void>) load(device,nameof(vkCmdDebugMarkerBeginEXT)); 
			vkCmdDebugMarkerEndEXT = (delegate* unmanaged<VkCommandBuffer,void>) load(device,nameof(vkCmdDebugMarkerEndEXT)); 
			vkCmdDebugMarkerInsertEXT = (delegate* unmanaged<VkCommandBuffer,VkDebugMarkerMarkerInfoEXT*,void>) load(device,nameof(vkCmdDebugMarkerInsertEXT)); 
		}	
		if (VK.VK_EXT_transform_feedback){
			vkCmdBeginQueryIndexedEXT = (delegate* unmanaged<VkCommandBuffer,VkQueryPool,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdBeginQueryIndexedEXT)); 
			vkCmdEndQueryIndexedEXT = (delegate* unmanaged<VkCommandBuffer,VkQueryPool,UInt32,UInt32,void>) load(device,nameof(vkCmdEndQueryIndexedEXT)); 
		}
		if (VK.VK_NVX_binary_import){
			vkCmdCuLaunchKernelNVX = (delegate* unmanaged<VkCommandBuffer,VkCuLaunchInfoNVX*,void>) load(device,nameof(vkCmdCuLaunchKernelNVX)); 
		}
		if (VK.VK_EXT_conditional_rendering){
			vkCmdBeginConditionalRenderingEXT = (delegate* unmanaged<VkCommandBuffer,VkConditionalRenderingBeginInfoEXT*,void>) load(device,nameof(vkCmdBeginConditionalRenderingEXT)); 
			vkCmdEndConditionalRenderingEXT = (delegate* unmanaged<VkCommandBuffer,void>) load(device,nameof(vkCmdEndConditionalRenderingEXT)); 
		}
		if (VK.VK_NV_clip_space_w_scaling){
			vkCmdSetViewportWScalingNV = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkViewportWScalingNV*,void>) load(device,nameof(vkCmdSetViewportWScalingNV)); 
		}
        if (VK.VK_EXT_discard_rectangles){
			vkCmdSetDiscardRectangleEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,UInt32,VkRect2D*,void>) load(device,nameof(vkCmdSetDiscardRectangleEXT)); 
		}
        if (VK.VK_EXT_extended_dynamic_state){
			vkCmdSetCullModeEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,void>) load(device,nameof(vkCmdSetCullModeEXT)); 
			vkCmdSetFrontFaceEXT = (delegate* unmanaged<VkCommandBuffer,VkFrontFace,void>) load(device,nameof(vkCmdSetFrontFaceEXT)); 
			vkCmdSetPrimitiveTopologyEXT = (delegate* unmanaged<VkCommandBuffer,VkPrimitiveTopology,void>) load(device,nameof(vkCmdSetPrimitiveTopologyEXT)); 
			vkCmdSetViewportWithCountEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,VkViewport*,void>) load(device,nameof(vkCmdSetViewportWithCountEXT)); 
			vkCmdSetScissorWithCountEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,VkRect2D*,void>) load(device,nameof(vkCmdSetScissorWithCountEXT)); 
			vkCmdSetDepthTestEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthTestEnableEXT)); 
			vkCmdSetDepthWriteEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthWriteEnableEXT)); 
			vkCmdSetDepthCompareOpEXT = (delegate* unmanaged<VkCommandBuffer,VkCompareOp,void>) load(device,nameof(vkCmdSetDepthCompareOpEXT)); 
			vkCmdSetDepthBoundsTestEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthBoundsTestEnableEXT)); 
			vkCmdSetStencilTestEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetStencilTestEnableEXT)); 
			vkCmdSetStencilOpEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,VkStencilOp,VkStencilOp,VkStencilOp,VkCompareOp,void>) load(device,nameof(vkCmdSetStencilOpEXT)); 
		}
        		if (VK.VK_EXT_vertex_input_dynamic_state){
			vkCmdSetVertexInputEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,VkVertexInputBindingDescription2EXT*,UInt32,VkVertexInputAttributeDescription2EXT*,void>) load(device,nameof(vkCmdSetVertexInputEXT)); 
		}
		if (VK.VK_HUAWEI_subpass_shading){
			vkCmdSubpassShadingHUAWEI = (delegate* unmanaged<VkCommandBuffer,void>) load(device,nameof(vkCmdSubpassShadingHUAWEI)); 
		}
		if (VK.VK_HUAWEI_invocation_mask){
			vkCmdBindInvocationMaskHUAWEI = (delegate* unmanaged<VkCommandBuffer,VkImageView,VkImageLayout,void>) load(device,nameof(vkCmdBindInvocationMaskHUAWEI)); 
		}
		if (VK.VK_EXT_extended_dynamic_state2){
			vkCmdSetPatchControlPointsEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,void>) load(device,nameof(vkCmdSetPatchControlPointsEXT)); 
			vkCmdSetRasterizerDiscardEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetRasterizerDiscardEnableEXT)); 
			vkCmdSetDepthBiasEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetDepthBiasEnableEXT)); 
			vkCmdSetLogicOpEXT = (delegate* unmanaged<VkCommandBuffer,VkLogicOp,void>) load(device,nameof(vkCmdSetLogicOpEXT)); 
			vkCmdSetPrimitiveRestartEnableEXT = (delegate* unmanaged<VkCommandBuffer,uint,void>) load(device,nameof(vkCmdSetPrimitiveRestartEnableEXT)); 
		}
		if (VK.VK_EXT_color_write_enable){
			vkCmdSetColorWriteEnableEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,uint*,void>) load(device,nameof(vkCmdSetColorWriteEnableEXT)); 
		}
		if (VK.VK_EXT_multi_draw){
			vkCmdDrawMultiEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,VkMultiDrawInfoEXT*,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdDrawMultiEXT)); 
			vkCmdDrawMultiIndexedEXT = (delegate* unmanaged<VkCommandBuffer,UInt32,VkMultiDrawIndexedInfoEXT*,UInt32,UInt32,UInt32,Int32*,void>) load(device,nameof(vkCmdDrawMultiIndexedEXT)); 
		}
		if (VK.VK_KHR_acceleration_structure){
			vkCmdBuildAccelerationStructuresKHR = (delegate* unmanaged<VkCommandBuffer,UInt32,VkAccelerationStructureBuildGeometryInfoKHR*,VkAccelerationStructureBuildRangeInfoKHR*,void>) load(device,nameof(vkCmdBuildAccelerationStructuresKHR)); 
			vkCmdCopyAccelerationStructureKHR = (delegate* unmanaged<VkCommandBuffer,VkCopyAccelerationStructureInfoKHR*,void>) load(device,nameof(vkCmdCopyAccelerationStructureKHR)); 
			vkCmdCopyAccelerationStructureToMemoryKHR = (delegate* unmanaged<VkCommandBuffer,VkCopyAccelerationStructureToMemoryInfoKHR*,void>) load(device,nameof(vkCmdCopyAccelerationStructureToMemoryKHR)); 
			vkCmdCopyMemoryToAccelerationStructureKHR = (delegate* unmanaged<VkCommandBuffer,VkCopyMemoryToAccelerationStructureInfoKHR*,void>) load(device,nameof(vkCmdCopyMemoryToAccelerationStructureKHR)); 
			vkCmdWriteAccelerationStructuresPropertiesKHR = (delegate* unmanaged<VkCommandBuffer,UInt32,VkAccelerationStructureKHR*,VkQueryType,VkQueryPool,UInt32,void>) load(device,nameof(vkCmdWriteAccelerationStructuresPropertiesKHR)); 
		}
		if (VK.VK_KHR_ray_tracing_pipeline){
			vkCmdTraceRaysKHR = (delegate* unmanaged<VkCommandBuffer,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,VkStridedDeviceAddressRegionKHR*,UInt32,UInt32,UInt32,void>) load(device,nameof(vkCmdTraceRaysKHR)); 
			vkCmdSetRayTracingPipelineStackSizeKHR = (delegate* unmanaged<VkCommandBuffer,UInt32,void>) load(device,nameof(vkCmdSetRayTracingPipelineStackSizeKHR)); 
		}
        
    }

    private unsafe void ReleaseLoader() 
    { 
        vkEnumerateInstanceLayerProperties =null;
        vkEnumerateInstanceVersion =null;
        vkEnumerateInstanceExtensionProperties =null;
        vkCreateInstance =null;
        vkDestroyInstance =null;
        FuncvkGetInstanceProcAddr =null;
        FuncvkGetDeviceProcAddr =null;
        Libraries.Unload(vulkan);
    }

    public unsafe void ReleaseInstance() 
	{  	
		vkDestroySurfaceKHR =  null;
		vkCreateWin32SurfaceKHR = null;
		vkCreateDevice =null;
		vkDestroyDevice =null;
		vkEnumeratePhysicalDevices =null;
		vkGetPhysicalDeviceFeatures =null;
		vkGetPhysicalDeviceFormatProperties =null;
		vkGetPhysicalDeviceImageFormatProperties =null;
		vkGetPhysicalDeviceProperties =null;
		vkGetPhysicalDeviceQueueFamilyProperties =null;
		vkGetPhysicalDeviceMemoryProperties =null;
		vkEnumerateDeviceExtensionProperties =null;
		vkEnumerateDeviceLayerProperties =null;
		vkGetPhysicalDeviceSparseImageFormatProperties =null;
		vkEnumeratePhysicalDeviceGroups =null;
		vkGetPhysicalDeviceFeatures2 =null;
		vkGetPhysicalDeviceProperties2 =null;
		vkGetPhysicalDeviceFormatProperties2 =null;
		vkGetPhysicalDeviceImageFormatProperties2 =null;
		vkGetPhysicalDeviceQueueFamilyProperties2 =null;
		vkGetPhysicalDeviceMemoryProperties2 =null;
		vkGetPhysicalDeviceSparseImageFormatProperties2 =null;
		vkGetPhysicalDeviceExternalBufferProperties =null;
		vkGetPhysicalDeviceExternalFenceProperties =null;
		vkGetPhysicalDeviceExternalSemaphoreProperties =null;
		vkGetPhysicalDeviceToolProperties =null;
		vkDestroySurfaceKHR =null;
		vkGetPhysicalDeviceSurfaceSupportKHR =null;
		vkGetPhysicalDeviceSurfaceCapabilitiesKHR =null;
		vkGetPhysicalDeviceSurfaceFormatsKHR =null;
		vkGetPhysicalDeviceSurfacePresentModesKHR =null;
		vkQueuePresentKHR =null;
		vkGetPhysicalDevicePresentRectanglesKHR =null;
		vkGetPhysicalDeviceDisplayPropertiesKHR =null;
		vkGetPhysicalDeviceDisplayPlanePropertiesKHR =null;
		vkGetDisplayPlaneSupportedDisplaysKHR =null;
		vkGetDisplayModePropertiesKHR =null;
		vkCreateDisplayModeKHR =null;
		vkGetDisplayPlaneCapabilitiesKHR =null;
		vkCreateDisplayPlaneSurfaceKHR =null;
		vkGetPhysicalDeviceFeatures2KHR =null;
		vkGetPhysicalDeviceProperties2KHR =null;
		vkGetPhysicalDeviceFormatProperties2KHR =null;
		vkGetPhysicalDeviceImageFormatProperties2KHR =null;
		vkGetPhysicalDeviceQueueFamilyProperties2KHR =null;
		vkGetPhysicalDeviceMemoryProperties2KHR =null;
		vkGetPhysicalDeviceSparseImageFormatProperties2KHR =null;
		vkGetPhysicalDeviceExternalBufferPropertiesKHR =null;
		vkGetPhysicalDeviceExternalSemaphorePropertiesKHR =null;
		vkGetPhysicalDeviceExternalFencePropertiesKHR =null;
		vkEnumeratePhysicalDeviceQueueFamilyPerformanceQueryCountersKHR =null;
		vkGetPhysicalDeviceQueueFamilyPerformanceQueryPassesKHR =null;
		vkGetPhysicalDeviceSurfaceCapabilities2KHR =null;
		vkGetPhysicalDeviceSurfaceFormats2KHR =null;
		vkGetPhysicalDeviceDisplayProperties2KHR =null;
		vkGetPhysicalDeviceDisplayPlaneProperties2KHR =null;
		vkGetDisplayModeProperties2KHR =null;
		vkGetDisplayPlaneCapabilities2KHR =null;
		vkGetPhysicalDeviceFragmentShadingRatesKHR =null;
		vkCmdSetFragmentShadingRateKHR =null;
		vkCreateDebugReportCallbackEXT =null;
		vkDestroyDebugReportCallbackEXT =null;
		vkDebugReportMessageEXT =null;
		vkGetPhysicalDeviceExternalImageFormatPropertiesNV =null;
		vkQueueBeginDebugUtilsLabelEXT =null;
		vkQueueEndDebugUtilsLabelEXT =null;
		vkQueueInsertDebugUtilsLabelEXT =null;
		vkCmdBeginDebugUtilsLabelEXT =null;
		vkCmdEndDebugUtilsLabelEXT =null;
		vkCmdInsertDebugUtilsLabelEXT =null;
		vkCreateDebugUtilsMessengerEXT =null;
		vkDestroyDebugUtilsMessengerEXT =null;
		vkSubmitDebugUtilsMessageEXT =null;
		vkReleaseDisplayEXT =null;
		vkGetPhysicalDeviceSurfaceCapabilities2EXT =null;
		vkCmdSetSampleLocationsEXT =null;
		vkGetPhysicalDeviceMultisamplePropertiesEXT =null;
		vkCmdBindShadingRateImageNV =null;
		vkCmdSetViewportShadingRatePaletteNV =null;
		vkCmdSetCoarseSampleOrderNV =null;
		vkGetPhysicalDeviceCalibrateableTimeDomainsEXT =null;
		vkCmdDrawMeshTasksNV =null;
		vkCmdSetExclusiveScissorNV =null;
		vkCmdSetCheckpointNV =null;
		vkGetQueueCheckpointDataNV =null;
		vkCmdSetPerformanceMarkerINTEL =null;
		vkCmdSetPerformanceStreamMarkerINTEL =null;
		vkCmdSetPerformanceOverrideINTEL =null;
		vkQueueSetPerformanceConfigurationINTEL =null;
		vkGetPhysicalDeviceToolPropertiesEXT =null;
		vkGetPhysicalDeviceCooperativeMatrixPropertiesNV =null;
		vkGetPhysicalDeviceSupportedFramebufferMixedSamplesCombinationsNV =null;
		vkCreateHeadlessSurfaceEXT =null;
		vkCmdSetLineStippleEXT =null;
		vkCmdPreprocessGeneratedCommandsNV =null;
		vkCmdExecuteGeneratedCommandsNV =null;
		vkCmdBindPipelineShaderGroupNV =null;
		vkAcquireDrmDisplayEXT =null;
		vkGetDrmDisplayEXT =null;
		vkCmdSetFragmentShadingRateEnumNV =null;
		vkAcquireWinrtDisplayNV =null;
		vkGetWinrtDisplayNV =null;

	}

    public unsafe void ReleaseDevice() 
	{  
		vkGetDeviceQueue = null;
		vkQueueSubmit = null;
		vkQueueWaitIdle = null;
		vkDeviceWaitIdle = null;
		vkAllocateMemory = null;
		vkFreeMemory = null;
		vkMapMemory = null;
		vkUnmapMemory = null;
		vkFlushMappedMemoryRanges = null;
		vkInvalidateMappedMemoryRanges = null;
		vkGetDeviceMemoryCommitment = null;
		vkBindBufferMemory = null;
		vkBindImageMemory = null;
		vkGetBufferMemoryRequirements = null;
		vkGetImageMemoryRequirements = null;
		vkGetImageSparseMemoryRequirements = null;
		vkQueueBindSparse = null;
		vkCreateFence = null;
		vkDestroyFence = null;
		vkResetFences = null;
		vkGetFenceStatus = null;
		vkWaitForFences = null;
		vkCreateSemaphore = null;
		vkDestroySemaphore = null;
		vkCreateEvent = null;
		vkDestroyEvent = null;
		vkGetEventStatus = null;
		vkSetEvent = null;
		vkResetEvent = null;
		vkCreateQueryPool = null;
		vkDestroyQueryPool = null;
		vkGetQueryPoolResults = null;
		vkCreateBuffer = null;
		vkDestroyBuffer = null;
		vkCreateBufferView = null;
		vkDestroyBufferView = null;
		vkCreateImage = null;
		vkDestroyImage = null;
		vkGetImageSubresourceLayout = null;
		vkCreateImageView = null;
		vkDestroyImageView = null;
		vkCreateShaderModule = null;
		vkDestroyShaderModule = null;
		vkCreatePipelineCache = null;
		vkDestroyPipelineCache = null;
		vkGetPipelineCacheData = null;
		vkMergePipelineCaches = null;
		vkCreateGraphicsPipelines = null;
		vkCreateComputePipelines = null;
		vkDestroyPipeline = null;
		vkCreatePipelineLayout = null;
		vkDestroyPipelineLayout = null;
		vkCreateSampler = null;
		vkDestroySampler = null;
		vkCreateDescriptorSetLayout = null;
		vkDestroyDescriptorSetLayout = null;
		vkCreateDescriptorPool = null;
		vkDestroyDescriptorPool = null;
		vkResetDescriptorPool = null;
		vkAllocateDescriptorSets = null;
		vkFreeDescriptorSets = null;
		vkUpdateDescriptorSets = null;
		vkCreateFramebuffer = null;
		vkDestroyFramebuffer = null;
		vkCreateRenderPass = null;
		vkDestroyRenderPass = null;
		vkGetRenderAreaGranularity = null;
		vkCreateCommandPool = null;
		vkDestroyCommandPool = null;
		vkResetCommandPool = null;
		vkAllocateCommandBuffers = null;
		vkFreeCommandBuffers = null;
		vkBeginCommandBuffer = null;
		vkEndCommandBuffer = null;
		vkResetCommandBuffer = null;
		vkCmdBindPipeline = null;
		vkCmdSetViewport = null;
		vkCmdSetScissor = null;
		vkCmdSetLineWidth = null;
		vkCmdSetDepthBias = null;
		vkCmdSetBlendConstants = null;
		vkCmdSetDepthBounds = null;
		vkCmdSetStencilCompareMask = null;
		vkCmdSetStencilWriteMask = null;
		vkCmdSetStencilReference = null;
		vkCmdBindDescriptorSets = null;
		vkCmdBindIndexBuffer = null;
		vkCmdBindVertexBuffers = null;
		vkCmdDraw = null;
		vkCmdDrawIndexed = null;
		vkCmdDrawIndirect = null;
		vkCmdDrawIndexedIndirect = null;
		vkCmdDispatch = null;
		vkCmdDispatchIndirect = null;
		vkCmdCopyBuffer = null;
		vkCmdCopyImage = null;
		vkCmdBlitImage = null;
		vkCmdCopyBufferToImage = null;
		vkCmdCopyImageToBuffer = null;
		vkCmdUpdateBuffer = null;
		vkCmdFillBuffer = null;
		vkCmdClearColorImage = null;
		vkCmdClearDepthStencilImage = null;
		vkCmdClearAttachments = null;
		vkCmdResolveImage = null;
		vkCmdSetEvent = null;
		vkCmdResetEvent = null;
		vkCmdWaitEvents = null;
		vkCmdPipelineBarrier = null;
		vkCmdBeginQuery = null;
		vkCmdEndQuery = null;
		vkCmdResetQueryPool = null;
		vkCmdWriteTimestamp = null;
		vkCmdCopyQueryPoolResults = null;
		vkCmdPushConstants = null;
		vkCmdBeginRenderPass = null;
		vkCmdNextSubpass = null;
		vkCmdEndRenderPass = null;
		vkCmdExecuteCommands = null;
		vkBindBufferMemory2 = null;
		vkBindImageMemory2 = null;
		vkGetDeviceGroupPeerMemoryFeatures = null;
		vkCmdSetDeviceMask = null;
		vkCmdDispatchBase = null;
		vkGetImageMemoryRequirements2 = null;
		vkGetBufferMemoryRequirements2 = null;
		vkGetImageSparseMemoryRequirements2 = null;
		vkTrimCommandPool = null;
		vkGetDeviceQueue2 = null;
		vkCreateSamplerYcbcrConversion = null;
		vkDestroySamplerYcbcrConversion = null;
		vkCreateDescriptorUpdateTemplate = null;
		vkDestroyDescriptorUpdateTemplate = null;
		vkUpdateDescriptorSetWithTemplate = null;
		vkGetDescriptorSetLayoutSupport = null;
		vkCmdDrawIndirectCount = null;
		vkCmdDrawIndexedIndirectCount = null;
		vkCreateRenderPass2 = null;
		vkCmdBeginRenderPass2 = null;
		vkCmdNextSubpass2 = null;
		vkCmdEndRenderPass2 = null;
		vkResetQueryPool = null;
		vkGetSemaphoreCounterValue = null;
		vkWaitSemaphores = null;
		vkSignalSemaphore = null;
		vkCreatePrivateDataSlot = null;
		vkDestroyPrivateDataSlot = null;
		vkSetPrivateData = null;
		vkGetPrivateData = null;
		vkCmdSetEvent2 = null;
		vkCmdResetEvent2 = null;
		vkCmdWaitEvents2 = null;
		vkCmdPipelineBarrier2 = null;
		vkCmdWriteTimestamp2 = null;
		vkQueueSubmit2 = null;
		vkCmdCopyBuffer2 = null;
		vkCmdCopyImage2 = null;
		vkCmdCopyBufferToImage2 = null;
		vkCmdCopyImageToBuffer2 = null;
		vkCmdBlitImage2 = null;
		vkCmdResolveImage2 = null;
		vkCmdBeginRendering = null;
		vkCmdEndRendering = null;
		vkCmdSetCullMode = null;
		vkCmdSetFrontFace = null;
		vkCmdSetPrimitiveTopology = null;
		vkCmdSetViewportWithCount = null;
		vkCmdSetScissorWithCount = null;
		vkCmdBindVertexBuffers2 = null;
		vkCmdSetDepthTestEnable = null;
		vkCmdSetDepthWriteEnable = null;
		vkCmdSetDepthCompareOp = null;
		vkCmdSetDepthBoundsTestEnable = null;
		vkCmdSetStencilTestEnable = null;
		vkCmdSetStencilOp = null;
		vkCmdSetRasterizerDiscardEnable = null;
		vkCmdSetDepthBiasEnable = null;
		vkCmdSetPrimitiveRestartEnable = null;
		vkGetDeviceBufferMemoryRequirements = null;
		vkGetDeviceImageMemoryRequirements = null;
		vkGetDeviceImageSparseMemoryRequirements = null;
		vkCreateSwapchainKHR = null;
		vkDestroySwapchainKHR = null;
		vkGetSwapchainImagesKHR = null;
		vkAcquireNextImageKHR = null;
		vkQueuePresentKHR = null;
		vkGetDeviceGroupPresentCapabilitiesKHR = null;
		vkGetDeviceGroupSurfacePresentModesKHR = null;
		vkAcquireNextImage2KHR = null;
		vkCreateSharedSwapchainsKHR = null;
		vkCmdBeginRenderingKHR = null;
		vkCmdEndRenderingKHR = null;
		vkGetDeviceGroupPeerMemoryFeaturesKHR = null;
		vkCmdSetDeviceMaskKHR = null;
		vkCmdDispatchBaseKHR = null;
		vkTrimCommandPoolKHR = null;
		vkGetMemoryFdKHR = null;
		vkGetMemoryFdPropertiesKHR = null;
		vkImportSemaphoreFdKHR = null;
		vkGetSemaphoreFdKHR = null;
		vkCmdPushDescriptorSetKHR = null;
		vkCmdPushDescriptorSetWithTemplateKHR = null;
		vkCreateDescriptorUpdateTemplateKHR = null;
		vkDestroyDescriptorUpdateTemplateKHR = null;
		vkUpdateDescriptorSetWithTemplateKHR = null;
		vkCreateRenderPass2KHR = null;
		vkCmdBeginRenderPass2KHR = null;
		vkCmdNextSubpass2KHR = null;
		vkCmdEndRenderPass2KHR = null;
		vkGetSwapchainStatusKHR = null;
		vkGetFenceFdKHR = null;
		vkAcquireProfilingLockKHR = null;
		vkReleaseProfilingLockKHR = null;
		vkGetImageMemoryRequirements2KHR = null;
		vkGetBufferMemoryRequirements2KHR = null;
		vkGetImageSparseMemoryRequirements2KHR = null;
		vkCreateSamplerYcbcrConversionKHR = null;
		vkDestroySamplerYcbcrConversionKHR = null;
		vkBindBufferMemory2KHR = null;
		vkBindImageMemory2KHR = null;
		vkGetDescriptorSetLayoutSupportKHR = null;
		vkCmdDrawIndirectCountKHR = null;
		vkCmdDrawIndexedIndirectCountKHR = null;
		vkGetSemaphoreCounterValueKHR = null;
		vkWaitSemaphoresKHR = null;
		vkSignalSemaphoreKHR = null;
		vkWaitForPresentKHR = null;
		vkCreateDeferredOperationKHR = null;
		vkDestroyDeferredOperationKHR = null;
		vkGetDeferredOperationResultKHR = null;
		vkDeferredOperationJoinKHR = null;
		vkGetPipelineExecutablePropertiesKHR = null;
		vkGetPipelineExecutableStatisticsKHR = null;
		vkGetPipelineExecutableInternalRepresentationsKHR = null;
		vkCmdSetEvent2KHR = null;
		vkCmdResetEvent2KHR = null;
		vkCmdWaitEvents2KHR = null;
		vkCmdPipelineBarrier2KHR = null;
		vkCmdWriteTimestamp2KHR = null;
		vkQueueSubmit2KHR = null;
		vkCmdWriteBufferMarker2AMD = null;
		vkGetQueueCheckpointData2NV = null;
		vkCmdCopyBuffer2KHR = null;
		vkCmdCopyImage2KHR = null;
		vkCmdCopyBufferToImage2KHR = null;
		vkCmdCopyImageToBuffer2KHR = null;
		vkCmdBlitImage2KHR = null;
		vkCmdResolveImage2KHR = null;
		vkCmdTraceRaysIndirect2KHR = null;
		vkGetDeviceBufferMemoryRequirementsKHR = null;
		vkGetDeviceImageMemoryRequirementsKHR = null;
		vkGetDeviceImageSparseMemoryRequirementsKHR = null;
		vkDebugMarkerSetObjectTagEXT = null;
		vkDebugMarkerSetObjectNameEXT = null;
		vkCmdDebugMarkerBeginEXT = null;
		vkCmdDebugMarkerEndEXT = null;
		vkCmdDebugMarkerInsertEXT = null;
		vkCmdBindTransformFeedbackBuffersEXT = null;
		vkCmdBeginTransformFeedbackEXT = null;
		vkCmdEndTransformFeedbackEXT = null;
		vkCmdBeginQueryIndexedEXT = null;
		vkCmdEndQueryIndexedEXT = null;
		vkCmdDrawIndirectByteCountEXT = null;
		vkCreateCuModuleNVX = null;
		vkCreateCuFunctionNVX = null;
		vkDestroyCuModuleNVX = null;
		vkDestroyCuFunctionNVX = null;
		vkCmdCuLaunchKernelNVX = null;
		vkGetImageViewAddressNVX = null;
		vkCmdDrawIndirectCountAMD = null;
		vkCmdDrawIndexedIndirectCountAMD = null;
		vkGetShaderInfoAMD = null;
		vkCmdBeginConditionalRenderingEXT = null;
		vkCmdEndConditionalRenderingEXT = null;
		vkCmdSetViewportWScalingNV = null;
		vkDisplayPowerControlEXT = null;
		vkRegisterDeviceEventEXT = null;
		vkRegisterDisplayEventEXT = null;
		vkGetSwapchainCounterEXT = null;
		vkGetRefreshCycleDurationGOOGLE = null;
		vkGetPastPresentationTimingGOOGLE = null;
		vkCmdSetDiscardRectangleEXT = null;
		vkSetHdrMetadataEXT = null;
		vkSetDebugUtilsObjectNameEXT = null;
		vkSetDebugUtilsObjectTagEXT = null;
		vkQueueBeginDebugUtilsLabelEXT = null;
		vkQueueEndDebugUtilsLabelEXT = null;
		vkQueueInsertDebugUtilsLabelEXT = null;
		vkCmdBeginDebugUtilsLabelEXT = null;
		vkCmdEndDebugUtilsLabelEXT = null;
		vkCmdInsertDebugUtilsLabelEXT = null;
		vkCreateDebugUtilsMessengerEXT = null;
		vkDestroyDebugUtilsMessengerEXT = null;
		vkSubmitDebugUtilsMessageEXT = null;
		vkCmdSetSampleLocationsEXT = null;
		vkGetPhysicalDeviceMultisamplePropertiesEXT = null;
		vkGetImageDrmFormatModifierPropertiesEXT = null;
		vkCreateValidationCacheEXT = null;
		vkDestroyValidationCacheEXT = null;
		vkMergeValidationCachesEXT = null;
		vkGetValidationCacheDataEXT = null;
		vkCmdBindShadingRateImageNV = null;
		vkCmdSetViewportShadingRatePaletteNV = null;
		vkCmdSetCoarseSampleOrderNV = null;
		vkCreateAccelerationStructureNV = null;
		vkDestroyAccelerationStructureNV = null;
		vkGetAccelerationStructureMemoryRequirementsNV = null;
		vkBindAccelerationStructureMemoryNV = null;
		vkCmdBuildAccelerationStructureNV = null;
		vkCmdCopyAccelerationStructureNV = null;
		vkCmdTraceRaysNV = null;
		vkCreateRayTracingPipelinesNV = null;
		vkGetRayTracingShaderGroupHandlesKHR = null;
		vkGetRayTracingShaderGroupHandlesNV = null;
		vkGetAccelerationStructureHandleNV = null;
		vkCmdWriteAccelerationStructuresPropertiesNV = null;
		vkCompileDeferredNV = null;
		vkGetMemoryHostPointerPropertiesEXT = null;
		vkCmdWriteBufferMarkerAMD = null;
		vkGetPhysicalDeviceCalibrateableTimeDomainsEXT = null;
		vkGetCalibratedTimestampsEXT = null;
		vkCmdDrawMeshTasksNV = null;
		vkCmdDrawMeshTasksIndirectNV = null;
		vkCmdDrawMeshTasksIndirectCountNV = null;
		vkCmdSetExclusiveScissorNV = null;
		vkCmdSetCheckpointNV = null;
		vkGetQueueCheckpointDataNV = null;
		vkInitializePerformanceApiINTEL = null;
		vkUninitializePerformanceApiINTEL = null;
		vkCmdSetPerformanceMarkerINTEL = null;
		vkCmdSetPerformanceStreamMarkerINTEL = null;
		vkCmdSetPerformanceOverrideINTEL = null;
		vkAcquirePerformanceConfigurationINTEL = null;
		vkReleasePerformanceConfigurationINTEL = null;
		vkQueueSetPerformanceConfigurationINTEL = null;
		vkGetPerformanceParameterINTEL = null;
		vkSetLocalDimmingAMD = null;
		vkCmdSetLineStippleEXT = null;
		vkResetQueryPoolEXT = null;
		vkCmdSetCullModeEXT = null;
		vkCmdSetFrontFaceEXT = null;
		vkCmdSetPrimitiveTopologyEXT = null;
		vkCmdSetViewportWithCountEXT = null;
		vkCmdSetScissorWithCountEXT = null;
		vkCmdBindVertexBuffers2EXT = null;
		vkCmdSetDepthTestEnableEXT = null;
		vkCmdSetDepthWriteEnableEXT = null;
		vkCmdSetDepthCompareOpEXT = null;
		vkCmdSetDepthBoundsTestEnableEXT = null;
		vkCmdSetStencilTestEnableEXT = null;
		vkCmdSetStencilOpEXT = null;
		vkGetGeneratedCommandsMemoryRequirementsNV = null;
		vkCmdPreprocessGeneratedCommandsNV = null;
		vkCmdExecuteGeneratedCommandsNV = null;
		vkCmdBindPipelineShaderGroupNV = null;
		vkCreateIndirectCommandsLayoutNV = null;
		vkDestroyIndirectCommandsLayoutNV = null;
		vkCreatePrivateDataSlotEXT = null;
		vkDestroyPrivateDataSlotEXT = null;
		vkSetPrivateDataEXT = null;
		vkGetPrivateDataEXT = null;
		vkGetImageSubresourceLayout2EXT = null;
		vkCmdSetVertexInputEXT = null;
		vkGetDeviceSubpassShadingMaxWorkgroupSizeHUAWEI = null;
		vkCmdSubpassShadingHUAWEI = null;
		vkCmdBindInvocationMaskHUAWEI = null;
		vkGetMemoryRemoteAddressNV = null;
		vkGetPipelinePropertiesEXT = null;
		vkCmdSetPatchControlPointsEXT = null;
		vkCmdSetRasterizerDiscardEnableEXT = null;
		vkCmdSetDepthBiasEnableEXT = null;
		vkCmdSetLogicOpEXT = null;
		vkCmdSetPrimitiveRestartEnableEXT = null;
		vkCmdSetColorWriteEnableEXT = null;
		vkCmdDrawMultiEXT = null;
		vkCmdDrawMultiIndexedEXT = null;
		vkSetDeviceMemoryPriorityEXT = null;
		vkGetDescriptorSetLayoutHostMappingInfoVALVE = null;
		vkGetDescriptorSetHostMappingVALVE = null;
		vkCreateAccelerationStructureKHR = null;
		vkDestroyAccelerationStructureKHR = null;
		vkCmdBuildAccelerationStructuresKHR = null;
		vkCmdBuildAccelerationStructuresIndirectKHR = null;
		vkBuildAccelerationStructuresKHR = null;
		vkCopyAccelerationStructureKHR = null;
		vkCopyAccelerationStructureToMemoryKHR = null;
		vkCopyMemoryToAccelerationStructureKHR = null;
		vkWriteAccelerationStructuresPropertiesKHR = null;
		vkCmdCopyAccelerationStructureKHR = null;
		vkCmdCopyAccelerationStructureToMemoryKHR = null;
		vkCmdCopyMemoryToAccelerationStructureKHR = null;
		vkCmdWriteAccelerationStructuresPropertiesKHR = null;
		vkGetDeviceAccelerationStructureCompatibilityKHR = null;
		vkGetAccelerationStructureBuildSizesKHR = null;
		vkCmdTraceRaysKHR = null;
		vkCreateRayTracingPipelinesKHR = null;
		vkGetRayTracingCaptureReplayShaderGroupHandlesKHR = null;
		vkCmdTraceRaysIndirectKHR = null;
		vkCmdSetRayTracingPipelineStackSizeKHR = null;

	}

    #region OVERRIDE
    // public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    public override string ToString() => string.Format($"Graphic Device Function" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)0).ToInt32()  ,  ((nint)0).ToInt32(),  ((nint)0).ToInt32(), ((nint)0).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is GraphicDeviceFunctions context && this.Equals(context) ;
    public unsafe bool Equals(GraphicDeviceFunctions other)=> other is GraphicDeviceFunctions input &&  vulkan.Equals(other.vulkan);
    public static bool operator ==(GraphicDeviceFunctions  left, GraphicDeviceFunctions right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceFunctions  left, GraphicDeviceFunctions  right) => !left.Equals(right);
    #endregion

}
