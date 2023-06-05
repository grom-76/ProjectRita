namespace RitaEngine.Base.Platform.Structures;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;

using ConstChar = System.Byte;
using RitaEngine.Base.Platform.Structures;
using RitaEngine.Base.Platform.API.Vulkan;

[SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = BaseHelper.FORCE_ALIGNEMENT)]
public /*readonly*/ struct GraphicInstanceFunction : IEquatable<GraphicInstanceFunction>
{
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
	/*readonly*/ nint _address = nint.Zero;
	public GraphicInstanceFunction() { _address = AddressOfPtrThis(); }
    public unsafe void Init( PFN_vkGetInstanceProcAddr load , VkInstance instance)
    {
		_address = AddressOfPtrThis();
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

	public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    #region OVERRIDE
    public override string ToString() => string.Format($"Graphic Instances" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)0).ToInt32()  ,  ((nint)0).ToInt32(),  ((nint)0).ToInt32(), ((nint)0).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is GraphicInstanceFunction context && this.Equals(context) ;
    public unsafe bool Equals(GraphicInstanceFunction other)=> other is GraphicInstanceFunction input && ( ((nint)vkCreateDevice).ToInt64()).Equals(((nint)input.vkCreateDevice).ToInt64() );
    public static bool operator ==(GraphicInstanceFunction  left, GraphicInstanceFunction right) => left.Equals(right);
    public static bool operator !=(GraphicInstanceFunction  left, GraphicInstanceFunction  right) => !left.Equals(right);
    public unsafe void Release() 
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
    #endregion

}
