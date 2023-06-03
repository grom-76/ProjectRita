namespace RitaEngine.Base.Platform.API.Vulkan;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using System.Security;

[SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = 4)]
public unsafe static partial class VK
{
#region EXTENSIONs BOOL
	public static bool VK_VERSION_1_0 = false;
	public static bool VK_VERSION_1_1 = false;
	public static bool VK_VERSION_1_2 = false;
	public static bool VK_VERSION_1_3 = false;
	public static bool VK_KHR_surface = false;
	public static bool VK_KHR_swapchain = false;
	public static bool VK_KHR_display = false;
	public static bool VK_KHR_display_swapchain = false;
	public static bool VK_KHR_sampler_mirror_clamp_to_edge = false;
	public static bool VK_KHR_dynamic_rendering = false;
	public static bool VK_KHR_multiview = false;
	public static bool VK_KHR_get_physical_device_properties2 = false;
	public static bool VK_KHR_device_group = false;
	public static bool VK_KHR_shader_draw_parameters = false;
	public static bool VK_KHR_maintenance1 = false;
	public static bool VK_KHR_device_group_creation = false;
	public static bool VK_KHR_external_memory_capabilities = false;
	public static bool VK_KHR_external_memory = false;
	public static bool VK_KHR_external_memory_fd = false;
	public static bool VK_KHR_external_semaphore_capabilities = false;
	public static bool VK_KHR_external_semaphore = false;
	public static bool VK_KHR_external_semaphore_fd = false;
	public static bool VK_KHR_push_descriptor = false;
	public static bool VK_KHR_shader_float16_int8 = false;
	public static bool VK_KHR_16bit_storage = false;
	public static bool VK_KHR_incremental_present = false;
	public static bool VK_KHR_descriptor_update_template = false;
	public static bool VK_KHR_imageless_framebuffer = false;
	public static bool VK_KHR_create_renderpass2 = false;
	public static bool VK_KHR_shared_presentable_image = false;
	public static bool VK_KHR_external_fence_capabilities = false;
	public static bool VK_KHR_external_fence = false;
	public static bool VK_KHR_external_fence_fd = false;
	public static bool VK_KHR_performance_query = false;
	public static bool VK_KHR_maintenance2 = false;
	public static bool VK_KHR_get_surface_capabilities2 = false;
	public static bool VK_KHR_variable_pointers = false;
	public static bool VK_KHR_get_display_properties2 = false;
	public static bool VK_KHR_dedicated_allocation = false;
	public static bool VK_KHR_storage_buffer_storage_class = false;
	public static bool VK_KHR_relaxed_block_layout = false;
	public static bool VK_KHR_get_memory_requirements2 = false;
	public static bool VK_KHR_image_format_list = false;
	public static bool VK_KHR_sampler_ycbcr_conversion = false;
	public static bool VK_KHR_bind_memory2 = false;
	public static bool VK_KHR_maintenance3 = false;
	public static bool VK_KHR_draw_indirect_count = false;
	public static bool VK_KHR_shader_subgroup_extended_types = false;
	public static bool VK_KHR_8bit_storage = false;
	public static bool VK_KHR_shader_atomic_int64 = false;
	public static bool VK_KHR_shader_clock = false;
	public static bool VK_KHR_global_priority = false;
	public static bool VK_KHR_driver_properties = false;
	public static bool VK_KHR_shader_float_controls = false;
	public static bool VK_KHR_depth_stencil_resolve = false;
	public static bool VK_KHR_swapchain_mutable_format = false;
	public static bool VK_KHR_timeline_semaphore = false;
	public static bool VK_KHR_vulkan_memory_model = false;
	public static bool VK_KHR_shader_terminate_invocation = false;
	public static bool VK_KHR_fragment_shading_rate = false;
	public static bool VK_KHR_spirv_1_4 = false;
	public static bool VK_KHR_surface_protected_capabilities = false;
	public static bool VK_KHR_separate_depth_stencil_layouts = false;
	public static bool VK_KHR_present_wait = false;
	public static bool VK_KHR_uniform_buffer_standard_layout = false;
	public static bool VK_KHR_buffer_device_address = false;
	public static bool VK_KHR_deferred_host_operations = false;
	public static bool VK_KHR_pipeline_executable_properties = false;
	public static bool VK_KHR_shader_integer_dot_product = false;
	public static bool VK_KHR_pipeline_library = false;
	public static bool VK_KHR_shader_non_semantic_info = false;
	public static bool VK_KHR_present_id = false;
	public static bool VK_KHR_synchronization2 = false;
	public static bool VK_KHR_fragment_shader_barycentric = false;
	public static bool VK_KHR_shader_subgroup_uniform_control_flow = false;
	public static bool VK_KHR_zero_initialize_workgroup_memory = false;
	public static bool VK_KHR_workgroup_memory_explicit_layout = false;
	public static bool VK_KHR_copy_commands2 = false;
	public static bool VK_KHR_format_feature_flags2 = false;
	public static bool VK_KHR_ray_tracing_maintenance1 = false;
	public static bool VK_KHR_portability_enumeration = false;
	public static bool VK_KHR_maintenance4 = false;
	public static bool VK_EXT_debug_report = false;
	public static bool VK_NV_glsl_shader = false;
	public static bool VK_EXT_depth_range_unrestricted = false;
	public static bool VK_IMG_filter_cubic = false;
	public static bool VK_AMD_rasterization_order = false;
	public static bool VK_AMD_shader_trinary_minmax = false;
	public static bool VK_AMD_shader_explicit_vertex_parameter = false;
	public static bool VK_EXT_debug_marker = false;
	public static bool VK_AMD_gcn_shader = false;
	public static bool VK_NV_dedicated_allocation = false;
	public static bool VK_EXT_transform_feedback = false;
	public static bool VK_NVX_binary_import = false;
	public static bool VK_NVX_image_view_handle = false;
	public static bool VK_AMD_draw_indirect_count = false;
	public static bool VK_AMD_negative_viewport_height = false;
	public static bool VK_AMD_gpu_shader_half_float = false;
	public static bool VK_AMD_shader_ballot = false;
	public static bool VK_AMD_texture_gather_bias_lod = false;
	public static bool VK_AMD_shader_info = false;
	public static bool VK_AMD_shader_image_load_store_lod = false;
	public static bool VK_NV_corner_sampled_image = false;
	public static bool VK_IMG_format_pvrtc = false;
	public static bool VK_NV_external_memory_capabilities = false;
	public static bool VK_NV_external_memory = false;
	public static bool VK_EXT_validation_flags = false;
	public static bool VK_EXT_shader_subgroup_ballot = false;
	public static bool VK_EXT_shader_subgroup_vote = false;
	public static bool VK_EXT_texture_compression_astc_hdr = false;
	public static bool VK_EXT_astc_decode_mode = false;
	public static bool VK_EXT_pipeline_robustness = false;
	public static bool VK_EXT_conditional_rendering = false;
	public static bool VK_NV_clip_space_w_scaling = false;
	public static bool VK_EXT_direct_mode_display = false;
	public static bool VK_EXT_display_surface_counter = false;
	public static bool VK_EXT_display_control = false;
	public static bool VK_GOOGLE_display_timing = false;
	public static bool VK_NV_sample_mask_override_coverage = false;
	public static bool VK_NV_geometry_shader_passthrough = false;
	public static bool VK_NV_viewport_array2 = false;
	public static bool VK_NVX_multiview_per_view_attributes = false;
	public static bool VK_NV_viewport_swizzle = false;
	public static bool VK_EXT_discard_rectangles = false;
	public static bool VK_EXT_conservative_rasterization = false;
	public static bool VK_EXT_depth_clip_enable = false;
	public static bool VK_EXT_swapchain_colorspace = false;
	public static bool VK_EXT_hdr_metadata = false;
	public static bool VK_EXT_external_memory_dma_buf = false;
	public static bool VK_EXT_queue_family_foreign = false;
	public static bool VK_EXT_debug_utils = false;
	public static bool VK_EXT_sampler_filter_minmax = false;
	public static bool VK_AMD_gpu_shader_int16 = false;
	public static bool VK_AMD_mixed_attachment_samples = false;
	public static bool VK_AMD_shader_fragment_mask = false;
	public static bool VK_EXT_inline_uniform_block = false;
	public static bool VK_EXT_shader_stencil_export = false;
	public static bool VK_EXT_sample_locations = false;
	public static bool VK_EXT_blend_operation_advanced = false;
	public static bool VK_NV_fragment_coverage_to_color = false;
	public static bool VK_NV_framebuffer_mixed_samples = false;
	public static bool VK_NV_fill_rectangle = false;
	public static bool VK_NV_shader_sm_builtins = false;
	public static bool VK_EXT_post_depth_coverage = false;
	public static bool VK_EXT_image_drm_format_modifier = false;
	public static bool VK_EXT_validation_cache = false;
	public static bool VK_EXT_descriptor_indexing = false;
	public static bool VK_EXT_shader_viewport_index_layer = false;
	public static bool VK_NV_shading_rate_image = false;
	public static bool VK_NV_ray_tracing = false;
	public static bool VK_NV_representative_fragment_test = false;
	public static bool VK_EXT_filter_cubic = false;
	public static bool VK_QCOM_render_pass_shader_resolve = false;
	public static bool VK_EXT_global_priority = false;
	public static bool VK_EXT_external_memory_host = false;
	public static bool VK_AMD_buffer_marker = false;
	public static bool VK_AMD_pipeline_compiler_control = false;
	public static bool VK_EXT_calibrated_timestamps = false;
	public static bool VK_AMD_shader_core_properties = false;
	public static bool VK_AMD_memory_overallocation_behavior = false;
	public static bool VK_EXT_vertex_attribute_divisor = false;
	public static bool VK_EXT_pipeline_creation_feedback = false;
	public static bool VK_NV_shader_subgroup_partitioned = false;
	public static bool VK_NV_compute_shader_derivatives = false;
	public static bool VK_NV_mesh_shader = false;
	public static bool VK_NV_fragment_shader_barycentric = false;
	public static bool VK_NV_shader_image_footprint = false;
	public static bool VK_NV_scissor_exclusive = false;
	public static bool VK_NV_device_diagnostic_checkpoints = false;
	public static bool VK_INTEL_shader_integer_functions2 = false;
	public static bool VK_INTEL_performance_query = false;
	public static bool VK_EXT_pci_bus_info = false;
	public static bool VK_AMD_display_native_hdr = false;
	public static bool VK_EXT_fragment_density_map = false;
	public static bool VK_EXT_scalar_block_layout = false;
	public static bool VK_GOOGLE_hlsl_functionality1 = false;
	public static bool VK_GOOGLE_decorate_string = false;
	public static bool VK_EXT_subgroup_size_control = false;
	public static bool VK_AMD_shader_core_properties2 = false;
	public static bool VK_AMD_device_coherent_memory = false;
	public static bool VK_EXT_shader_image_atomic_int64 = false;
	public static bool VK_EXT_memory_budget = false;
	public static bool VK_EXT_memory_priority = false;
	public static bool VK_NV_dedicated_allocation_image_aliasing = false;
	public static bool VK_EXT_buffer_device_address = false;
	public static bool VK_EXT_tooling_info = false;
	public static bool VK_EXT_separate_stencil_usage = false;
	public static bool VK_EXT_validation_features = false;
	public static bool VK_NV_cooperative_matrix = false;
	public static bool VK_NV_coverage_reduction_mode = false;
	public static bool VK_EXT_fragment_shader_interlock = false;
	public static bool VK_EXT_ycbcr_image_arrays = false;
	public static bool VK_EXT_provoking_vertex = false;
	public static bool VK_EXT_headless_surface = false;
	public static bool VK_EXT_line_rasterization = false;
	public static bool VK_EXT_shader_atomic_float = false;
	public static bool VK_EXT_host_query_reset = false;
	public static bool VK_EXT_index_type_uint8 = false;
	public static bool VK_EXT_extended_dynamic_state = false;
	public static bool VK_EXT_shader_atomic_float2 = false;
	public static bool VK_EXT_shader_demote_to_helper_invocation = false;
	public static bool VK_NV_device_generated_commands = false;
	public static bool VK_NV_inherited_viewport_scissor = false;
	public static bool VK_EXT_texel_buffer_alignment = false;
	public static bool VK_QCOM_render_pass_transform = false;
	public static bool VK_EXT_device_memory_report = false;
	public static bool VK_EXT_acquire_drm_display = false;
	public static bool VK_EXT_robustness2 = false;
	public static bool VK_EXT_custom_border_color = false;
	public static bool VK_GOOGLE_user_type = false;
	public static bool VK_NV_present_barrier = false;
	public static bool VK_EXT_private_data = false;
	public static bool VK_EXT_pipeline_creation_cache_control = false;
	public static bool VK_NV_device_diagnostics_config = false;
	public static bool VK_QCOM_render_pass_store_ops = false;
	public static bool VK_EXT_graphics_pipeline_library = false;
	public static bool VK_AMD_shader_early_and_late_fragment_tests = false;
	public static bool VK_NV_fragment_shading_rate_enums = false;
	public static bool VK_NV_ray_tracing_motion_blur = false;
	public static bool VK_EXT_ycbcr_2plane_444_formats = false;
	public static bool VK_EXT_fragment_density_map2 = false;
	public static bool VK_QCOM_rotated_copy_commands = false;
	public static bool VK_EXT_image_robustness = false;
	public static bool VK_EXT_image_compression_control = false;
	public static bool VK_EXT_attachment_feedback_loop_layout = false;
	public static bool VK_EXT_4444_formats = false;
	public static bool VK_EXT_device_fault = false;
	public static bool VK_ARM_rasterization_order_attachment_access = false;
	public static bool VK_EXT_rgba10x6_formats = false;
	public static bool VK_NV_acquire_winrt_display = false;
	public static bool VK_VALVE_mutable_descriptor_type = false;
	public static bool VK_EXT_vertex_input_dynamic_state = false;
	public static bool VK_EXT_physical_device_drm = false;
	public static bool VK_EXT_device_address_binding_report = false;
	public static bool VK_EXT_depth_clip_control = false;
	public static bool VK_EXT_primitive_topology_list_restart = false;
	public static bool VK_HUAWEI_subpass_shading = false;
	public static bool VK_HUAWEI_invocation_mask = false;
	public static bool VK_NV_external_memory_rdma = false;
	public static bool VK_EXT_pipeline_properties = false;
	public static bool VK_EXT_multisampled_render_to_single_sampled = false;
	public static bool VK_EXT_extended_dynamic_state2 = false;
	public static bool VK_EXT_color_write_enable = false;
	public static bool VK_EXT_primitives_generated_query = false;
	public static bool VK_EXT_global_priority_query = false;
	public static bool VK_EXT_image_view_min_lod = false;
	public static bool VK_EXT_multi_draw = false;
	public static bool VK_EXT_image_2d_view_of_3d = false;
	public static bool VK_EXT_opacity_micromap = false;
	public static bool VK_EXT_load_store_op_none = false;
	public static bool VK_EXT_border_color_swizzle = false;
	public static bool VK_EXT_pageable_device_local_memory = false;
	public static bool VK_VALVE_descriptor_set_host_mapping = false;
	public static bool VK_EXT_depth_clamp_zero_one = false;
	public static bool VK_EXT_non_seamless_cube_map = false;
	public static bool VK_QCOM_fragment_density_map_offset = false;
	public static bool VK_NV_copy_memory_indirect = false;
	public static bool VK_NV_memory_decompression = false;
	public static bool VK_NV_linear_color_attachment = false;
	public static bool VK_GOOGLE_surfaceless_query = false;
	public static bool VK_EXT_image_compression_control_swapchain = false;
	public static bool VK_QCOM_image_processing = false;
	public static bool VK_EXT_extended_dynamic_state3 = false;
	public static bool VK_EXT_subpass_merge_feedback = false;
	public static bool VK_EXT_shader_module_identifier = false;
	public static bool VK_EXT_rasterization_order_attachment_access = false;
	public static bool VK_NV_optical_flow = false;
	public static bool VK_EXT_legacy_dithering = false;
	public static bool VK_EXT_pipeline_protected_access = false;
	public static bool VK_QCOM_tile_properties = false;
	public static bool VK_SEC_amigo_profiling = false;
	public static bool VK_NV_ray_tracing_invocation_reorder = false;
	public static bool VK_EXT_mutable_descriptor_type = false;
	public static bool VK_ARM_shader_core_builtins = false;
	public static bool VK_KHR_acceleration_structure = false;
	public static bool VK_KHR_ray_tracing_pipeline = false;
	public static bool VK_KHR_ray_query = false;
	public static bool VK_EXT_mesh_shader = false;
#endregion

#region Methods
public static uint VK_MAKE_VERSION(uint major,uint minor,uint patch) =>    ((((UInt32)(major)) << 22) | (((UInt32)(minor)) << 12) | ((UInt32)(patch)));
public static uint VK_MAKE_API_VERSION(uint variant,uint major,uint minor,uint patch) =>    ((((UInt32)(variant)) << 29) | (((UInt32)(major)) << 22) | (((UInt32)(minor)) << 12) | ((UInt32)(patch)));
public static uint VK_API_VERSION_1_0 = VK_MAKE_API_VERSION(0, 1, 0, 0);// Patch version should always be set to 0;
public static uint VK_API_VERSION_1_3 = VK_MAKE_API_VERSION(0, 1, 3, 0);// Patch version should always be set to 0;
public static uint VK_HEADER_VERSION_COMPLETE = VK_MAKE_API_VERSION(0, 1, 3, VK_HEADER_VERSION);
public static uint VK_VERSION_MAJOR(uint version)=> ((UInt32)(version) >> 22);
public static uint VK_VERSION_MINOR(uint version)=> (((UInt32)(version) >> 12) & 0x3FFU);
public static uint VK_VERSION_PATCH(uint version)=> ((UInt32)(version) & 0xFFFU);
public static uint VK_API_VERSION_VARIANT(uint version)=> ((UInt32)(version) >> 29);
public static uint VK_API_VERSION_MAJOR(uint version)=> (((UInt32)(version) >> 22) & 0x7FU);
public static uint VK_API_VERSION_MINOR(uint version)=> (((UInt32)(version) >> 12) & 0x3FFU);
public static uint VK_API_VERSION_PATCH(uint version)=> ((UInt32)(version) & 0xFFFU);
#endregion

#region STATIC CONSTANTS 
#region VK_VERSION_1_3
public static  UInt64 VK_PIPELINE_STAGE_2_NONE = 0UL;
public static  UInt64 VK_PIPELINE_STAGE_2_NONE_KHR = 0UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TOP_OF_PIPE_BIT = 0x00000001UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TOP_OF_PIPE_BIT_KHR = 0x00000001UL;
public static  UInt64 VK_PIPELINE_STAGE_2_DRAW_INDIRECT_BIT = 0x00000002UL;
public static  UInt64 VK_PIPELINE_STAGE_2_DRAW_INDIRECT_BIT_KHR = 0x00000002UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VERTEX_INPUT_BIT = 0x00000004UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VERTEX_INPUT_BIT_KHR = 0x00000004UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VERTEX_SHADER_BIT = 0x00000008UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VERTEX_SHADER_BIT_KHR = 0x00000008UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TESSELLATION_CONTROL_SHADER_BIT = 0x00000010UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TESSELLATION_CONTROL_SHADER_BIT_KHR = 0x00000010UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TESSELLATION_EVALUATION_SHADER_BIT = 0x00000020UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TESSELLATION_EVALUATION_SHADER_BIT_KHR = 0x00000020UL;
public static  UInt64 VK_PIPELINE_STAGE_2_GEOMETRY_SHADER_BIT = 0x00000040UL;
public static  UInt64 VK_PIPELINE_STAGE_2_GEOMETRY_SHADER_BIT_KHR = 0x00000040UL;
public static  UInt64 VK_PIPELINE_STAGE_2_FRAGMENT_SHADER_BIT = 0x00000080UL;
public static  UInt64 VK_PIPELINE_STAGE_2_FRAGMENT_SHADER_BIT_KHR = 0x00000080UL;
public static  UInt64 VK_PIPELINE_STAGE_2_EARLY_FRAGMENT_TESTS_BIT = 0x00000100UL;
public static  UInt64 VK_PIPELINE_STAGE_2_EARLY_FRAGMENT_TESTS_BIT_KHR = 0x00000100UL;
public static  UInt64 VK_PIPELINE_STAGE_2_LATE_FRAGMENT_TESTS_BIT = 0x00000200UL;
public static  UInt64 VK_PIPELINE_STAGE_2_LATE_FRAGMENT_TESTS_BIT_KHR = 0x00000200UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COLOR_ATTACHMENT_OUTPUT_BIT = 0x00000400UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COLOR_ATTACHMENT_OUTPUT_BIT_KHR = 0x00000400UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COMPUTE_SHADER_BIT = 0x00000800UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COMPUTE_SHADER_BIT_KHR = 0x00000800UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ALL_TRANSFER_BIT = 0x00001000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ALL_TRANSFER_BIT_KHR = 0x00001000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TRANSFER_BIT = 0x00001000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TRANSFER_BIT_KHR = 0x00001000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_BOTTOM_OF_PIPE_BIT = 0x00002000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_BOTTOM_OF_PIPE_BIT_KHR = 0x00002000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_HOST_BIT = 0x00004000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_HOST_BIT_KHR = 0x00004000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ALL_GRAPHICS_BIT = 0x00008000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ALL_GRAPHICS_BIT_KHR = 0x00008000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ALL_COMMANDS_BIT = 0x00010000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ALL_COMMANDS_BIT_KHR = 0x00010000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COPY_BIT = 0x100000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COPY_BIT_KHR = 0x100000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_RESOLVE_BIT = 0x200000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_RESOLVE_BIT_KHR = 0x200000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_BLIT_BIT = 0x400000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_BLIT_BIT_KHR = 0x400000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_CLEAR_BIT = 0x800000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_CLEAR_BIT_KHR = 0x800000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_INDEX_INPUT_BIT = 0x1000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_INDEX_INPUT_BIT_KHR = 0x1000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VERTEX_ATTRIBUTE_INPUT_BIT = 0x2000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VERTEX_ATTRIBUTE_INPUT_BIT_KHR = 0x2000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_PRE_RASTERIZATION_SHADERS_BIT = 0x4000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_PRE_RASTERIZATION_SHADERS_BIT_KHR = 0x4000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VIDEO_DECODE_BIT_KHR = 0x04000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_VIDEO_ENCODE_BIT_KHR = 0x08000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TRANSFORM_FEEDBACK_BIT_EXT = 0x01000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_CONDITIONAL_RENDERING_BIT_EXT = 0x00040000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_COMMAND_PREPROCESS_BIT_NV = 0x00020000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_FRAGMENT_SHADING_RATE_ATTACHMENT_BIT_KHR = 0x00400000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_SHADING_RATE_IMAGE_BIT_NV = 0x00400000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ACCELERATION_STRUCTURE_BUILD_BIT_KHR = 0x02000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_RAY_TRACING_SHADER_BIT_KHR = 0x00200000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_RAY_TRACING_SHADER_BIT_NV = 0x00200000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ACCELERATION_STRUCTURE_BUILD_BIT_NV = 0x02000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_FRAGMENT_DENSITY_PROCESS_BIT_EXT = 0x00800000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TASK_SHADER_BIT_NV = 0x00080000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_MESH_SHADER_BIT_NV = 0x00100000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_TASK_SHADER_BIT_EXT = 0x00080000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_MESH_SHADER_BIT_EXT = 0x00100000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_SUBPASS_SHADING_BIT_HUAWEI = 0x8000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_INVOCATION_MASK_BIT_HUAWEI = 0x10000000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_ACCELERATION_STRUCTURE_COPY_BIT_KHR = 0x10000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_MICROMAP_BUILD_BIT_EXT = 0x40000000UL;
public static  UInt64 VK_PIPELINE_STAGE_2_OPTICAL_FLOW_BIT_NV = 0x20000000UL;
public static  UInt64 VK_ACCESS_2_NONE = 0UL;
public static  UInt64 VK_ACCESS_2_NONE_KHR = 0UL;
public static  UInt64 VK_ACCESS_2_INDIRECT_COMMAND_READ_BIT = 0x00000001UL;
public static  UInt64 VK_ACCESS_2_INDIRECT_COMMAND_READ_BIT_KHR = 0x00000001UL;
public static  UInt64 VK_ACCESS_2_INDEX_READ_BIT = 0x00000002UL;
public static  UInt64 VK_ACCESS_2_INDEX_READ_BIT_KHR = 0x00000002UL;
public static  UInt64 VK_ACCESS_2_VERTEX_ATTRIBUTE_READ_BIT = 0x00000004UL;
public static  UInt64 VK_ACCESS_2_VERTEX_ATTRIBUTE_READ_BIT_KHR = 0x00000004UL;
public static  UInt64 VK_ACCESS_2_UNIFORM_READ_BIT = 0x00000008UL;
public static  UInt64 VK_ACCESS_2_UNIFORM_READ_BIT_KHR = 0x00000008UL;
public static  UInt64 VK_ACCESS_2_INPUT_ATTACHMENT_READ_BIT = 0x00000010UL;
public static  UInt64 VK_ACCESS_2_INPUT_ATTACHMENT_READ_BIT_KHR = 0x00000010UL;
public static  UInt64 VK_ACCESS_2_SHADER_READ_BIT = 0x00000020UL;
public static  UInt64 VK_ACCESS_2_SHADER_READ_BIT_KHR = 0x00000020UL;
public static  UInt64 VK_ACCESS_2_SHADER_WRITE_BIT = 0x00000040UL;
public static  UInt64 VK_ACCESS_2_SHADER_WRITE_BIT_KHR = 0x00000040UL;
public static  UInt64 VK_ACCESS_2_COLOR_ATTACHMENT_READ_BIT = 0x00000080UL;
public static  UInt64 VK_ACCESS_2_COLOR_ATTACHMENT_READ_BIT_KHR = 0x00000080UL;
public static  UInt64 VK_ACCESS_2_COLOR_ATTACHMENT_WRITE_BIT = 0x00000100UL;
public static  UInt64 VK_ACCESS_2_COLOR_ATTACHMENT_WRITE_BIT_KHR = 0x00000100UL;
public static  UInt64 VK_ACCESS_2_DEPTH_STENCIL_ATTACHMENT_READ_BIT = 0x00000200UL;
public static  UInt64 VK_ACCESS_2_DEPTH_STENCIL_ATTACHMENT_READ_BIT_KHR = 0x00000200UL;
public static  UInt64 VK_ACCESS_2_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 0x00000400UL;
public static  UInt64 VK_ACCESS_2_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT_KHR = 0x00000400UL;
public static  UInt64 VK_ACCESS_2_TRANSFER_READ_BIT = 0x00000800UL;
public static  UInt64 VK_ACCESS_2_TRANSFER_READ_BIT_KHR = 0x00000800UL;
public static  UInt64 VK_ACCESS_2_TRANSFER_WRITE_BIT = 0x00001000UL;
public static  UInt64 VK_ACCESS_2_TRANSFER_WRITE_BIT_KHR = 0x00001000UL;
public static  UInt64 VK_ACCESS_2_HOST_READ_BIT = 0x00002000UL;
public static  UInt64 VK_ACCESS_2_HOST_READ_BIT_KHR = 0x00002000UL;
public static  UInt64 VK_ACCESS_2_HOST_WRITE_BIT = 0x00004000UL;
public static  UInt64 VK_ACCESS_2_HOST_WRITE_BIT_KHR = 0x00004000UL;
public static  UInt64 VK_ACCESS_2_MEMORY_READ_BIT = 0x00008000UL;
public static  UInt64 VK_ACCESS_2_MEMORY_READ_BIT_KHR = 0x00008000UL;
public static  UInt64 VK_ACCESS_2_MEMORY_WRITE_BIT = 0x00010000UL;
public static  UInt64 VK_ACCESS_2_MEMORY_WRITE_BIT_KHR = 0x00010000UL;
public static  UInt64 VK_ACCESS_2_SHADER_SAMPLED_READ_BIT = 0x100000000UL;
public static  UInt64 VK_ACCESS_2_SHADER_SAMPLED_READ_BIT_KHR = 0x100000000UL;
public static  UInt64 VK_ACCESS_2_SHADER_STORAGE_READ_BIT = 0x200000000UL;
public static  UInt64 VK_ACCESS_2_SHADER_STORAGE_READ_BIT_KHR = 0x200000000UL;
public static  UInt64 VK_ACCESS_2_SHADER_STORAGE_WRITE_BIT = 0x400000000UL;
public static  UInt64 VK_ACCESS_2_SHADER_STORAGE_WRITE_BIT_KHR = 0x400000000UL;
public static  UInt64 VK_ACCESS_2_VIDEO_DECODE_READ_BIT_KHR = 0x800000000UL;
public static  UInt64 VK_ACCESS_2_VIDEO_DECODE_WRITE_BIT_KHR = 0x1000000000UL;
public static  UInt64 VK_ACCESS_2_VIDEO_ENCODE_READ_BIT_KHR = 0x2000000000UL;
public static  UInt64 VK_ACCESS_2_VIDEO_ENCODE_WRITE_BIT_KHR = 0x4000000000UL;
public static  UInt64 VK_ACCESS_2_TRANSFORM_FEEDBACK_WRITE_BIT_EXT = 0x02000000UL;
public static  UInt64 VK_ACCESS_2_TRANSFORM_FEEDBACK_COUNTER_READ_BIT_EXT = 0x04000000UL;
public static  UInt64 VK_ACCESS_2_TRANSFORM_FEEDBACK_COUNTER_WRITE_BIT_EXT = 0x08000000UL;
public static  UInt64 VK_ACCESS_2_CONDITIONAL_RENDERING_READ_BIT_EXT = 0x00100000UL;
public static  UInt64 VK_ACCESS_2_COMMAND_PREPROCESS_READ_BIT_NV = 0x00020000UL;
public static  UInt64 VK_ACCESS_2_COMMAND_PREPROCESS_WRITE_BIT_NV = 0x00040000UL;
public static  UInt64 VK_ACCESS_2_FRAGMENT_SHADING_RATE_ATTACHMENT_READ_BIT_KHR = 0x00800000UL;
public static  UInt64 VK_ACCESS_2_SHADING_RATE_IMAGE_READ_BIT_NV = 0x00800000UL;
public static  UInt64 VK_ACCESS_2_ACCELERATION_STRUCTURE_READ_BIT_KHR = 0x00200000UL;
public static  UInt64 VK_ACCESS_2_ACCELERATION_STRUCTURE_WRITE_BIT_KHR = 0x00400000UL;
public static  UInt64 VK_ACCESS_2_ACCELERATION_STRUCTURE_READ_BIT_NV = 0x00200000UL;
public static  UInt64 VK_ACCESS_2_ACCELERATION_STRUCTURE_WRITE_BIT_NV = 0x00400000UL;
public static  UInt64 VK_ACCESS_2_FRAGMENT_DENSITY_MAP_READ_BIT_EXT = 0x01000000UL;
public static  UInt64 VK_ACCESS_2_COLOR_ATTACHMENT_READ_NONCOHERENT_BIT_EXT = 0x00080000UL;
public static  UInt64 VK_ACCESS_2_INVOCATION_MASK_READ_BIT_HUAWEI = 0x8000000000UL;
public static  UInt64 VK_ACCESS_2_SHADER_BINDING_TABLE_READ_BIT_KHR = 0x10000000000UL;
public static  UInt64 VK_ACCESS_2_MICROMAP_READ_BIT_EXT = 0x100000000000UL;
public static  UInt64 VK_ACCESS_2_MICROMAP_WRITE_BIT_EXT = 0x200000000000UL;
public static  UInt64 VK_ACCESS_2_OPTICAL_FLOW_READ_BIT_NV = 0x40000000000UL;
public static  UInt64 VK_ACCESS_2_OPTICAL_FLOW_WRITE_BIT_NV = 0x80000000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_BIT = 0x00000001UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_BIT_KHR = 0x00000001UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_IMAGE_BIT = 0x00000002UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_IMAGE_BIT_KHR = 0x00000002UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_IMAGE_ATOMIC_BIT = 0x00000004UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_IMAGE_ATOMIC_BIT_KHR = 0x00000004UL;
public static  UInt64 VK_FORMAT_FEATURE_2_UNIFORM_TEXEL_BUFFER_BIT = 0x00000008UL;
public static  UInt64 VK_FORMAT_FEATURE_2_UNIFORM_TEXEL_BUFFER_BIT_KHR = 0x00000008UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_TEXEL_BUFFER_BIT = 0x00000010UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_TEXEL_BUFFER_BIT_KHR = 0x00000010UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_TEXEL_BUFFER_ATOMIC_BIT = 0x00000020UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_TEXEL_BUFFER_ATOMIC_BIT_KHR = 0x00000020UL;
public static  UInt64 VK_FORMAT_FEATURE_2_VERTEX_BUFFER_BIT = 0x00000040UL;
public static  UInt64 VK_FORMAT_FEATURE_2_VERTEX_BUFFER_BIT_KHR = 0x00000040UL;
public static  UInt64 VK_FORMAT_FEATURE_2_COLOR_ATTACHMENT_BIT = 0x00000080UL;
public static  UInt64 VK_FORMAT_FEATURE_2_COLOR_ATTACHMENT_BIT_KHR = 0x00000080UL;
public static  UInt64 VK_FORMAT_FEATURE_2_COLOR_ATTACHMENT_BLEND_BIT = 0x00000100UL;
public static  UInt64 VK_FORMAT_FEATURE_2_COLOR_ATTACHMENT_BLEND_BIT_KHR = 0x00000100UL;
public static  UInt64 VK_FORMAT_FEATURE_2_DEPTH_STENCIL_ATTACHMENT_BIT = 0x00000200UL;
public static  UInt64 VK_FORMAT_FEATURE_2_DEPTH_STENCIL_ATTACHMENT_BIT_KHR = 0x00000200UL;
public static  UInt64 VK_FORMAT_FEATURE_2_BLIT_SRC_BIT = 0x00000400UL;
public static  UInt64 VK_FORMAT_FEATURE_2_BLIT_SRC_BIT_KHR = 0x00000400UL;
public static  UInt64 VK_FORMAT_FEATURE_2_BLIT_DST_BIT = 0x00000800UL;
public static  UInt64 VK_FORMAT_FEATURE_2_BLIT_DST_BIT_KHR = 0x00000800UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_FILTER_LINEAR_BIT = 0x00001000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_FILTER_LINEAR_BIT_KHR = 0x00001000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_FILTER_CUBIC_BIT = 0x00002000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_FILTER_CUBIC_BIT_EXT = 0x00002000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_TRANSFER_SRC_BIT = 0x00004000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_TRANSFER_SRC_BIT_KHR = 0x00004000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_TRANSFER_DST_BIT = 0x00008000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_TRANSFER_DST_BIT_KHR = 0x00008000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_FILTER_MINMAX_BIT = 0x00010000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_FILTER_MINMAX_BIT_KHR = 0x00010000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_MIDPOINT_CHROMA_SAMPLES_BIT = 0x00020000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_MIDPOINT_CHROMA_SAMPLES_BIT_KHR = 0x00020000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_LINEAR_FILTER_BIT = 0x00040000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_LINEAR_FILTER_BIT_KHR = 0x00040000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_SEPARATE_RECONSTRUCTION_FILTER_BIT = 0x00080000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_SEPARATE_RECONSTRUCTION_FILTER_BIT_KHR = 0x00080000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_BIT = 0x00100000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_BIT_KHR = 0x00100000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_FORCEABLE_BIT = 0x00200000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_YCBCR_CONVERSION_CHROMA_RECONSTRUCTION_EXPLICIT_FORCEABLE_BIT_KHR = 0x00200000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_DISJOINT_BIT = 0x00400000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_DISJOINT_BIT_KHR = 0x00400000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_COSITED_CHROMA_SAMPLES_BIT = 0x00800000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_COSITED_CHROMA_SAMPLES_BIT_KHR = 0x00800000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_READ_WITHOUT_FORMAT_BIT = 0x80000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_READ_WITHOUT_FORMAT_BIT_KHR = 0x80000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_WRITE_WITHOUT_FORMAT_BIT = 0x100000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_STORAGE_WRITE_WITHOUT_FORMAT_BIT_KHR = 0x100000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_DEPTH_COMPARISON_BIT = 0x200000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_SAMPLED_IMAGE_DEPTH_COMPARISON_BIT_KHR = 0x200000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_VIDEO_DECODE_OUTPUT_BIT_KHR = 0x02000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_VIDEO_DECODE_DPB_BIT_KHR = 0x04000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_ACCELERATION_STRUCTURE_VERTEX_BUFFER_BIT_KHR = 0x20000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_FRAGMENT_DENSITY_MAP_BIT_EXT = 0x01000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_FRAGMENT_SHADING_RATE_ATTACHMENT_BIT_KHR = 0x40000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_VIDEO_ENCODE_INPUT_BIT_KHR = 0x08000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_VIDEO_ENCODE_DPB_BIT_KHR = 0x10000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_LINEAR_COLOR_ATTACHMENT_BIT_NV = 0x4000000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_WEIGHT_IMAGE_BIT_QCOM = 0x400000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_WEIGHT_SAMPLED_IMAGE_BIT_QCOM = 0x800000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_BLOCK_MATCHING_BIT_QCOM = 0x1000000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_BOX_FILTER_SAMPLED_BIT_QCOM = 0x2000000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_OPTICAL_FLOW_IMAGE_BIT_NV = 0x10000000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_OPTICAL_FLOW_VECTOR_BIT_NV = 0x20000000000UL;
public static  UInt64 VK_FORMAT_FEATURE_2_OPTICAL_FLOW_COST_BIT_NV = 0x40000000000UL;
#endregion
#region VK_NV_memory_decompression
public static  UInt64 VK_MEMORY_DECOMPRESSION_METHOD_GDEFLATE_1_0_BIT_NV = 0x00000001UL;
#endregion
#endregion

#region Constantes 
#region VK_VERSION_1_0
//#define VK_API_VERSION_1_0 VK_MAKE_API_VERSION(0, 1, 0, 0)// Patch version should always be set to 0


public const int VK_HEADER_VERSION = 250;
//#define VK_HEADER_VERSION_COMPLETE VK_MAKE_API_VERSION(0, 1, 3, VK_HEADER_VERSION)
public const uint VK_ATTACHMENT_UNUSED = (~0U);
public const uint VK_FALSE = 0U;
public const float VK_LOD_CLAMP_NONE = 1000.0F;
public const uint VK_QUEUE_FAMILY_IGNORED = (~0U);
public const uint VK_REMAINING_ARRAY_LAYERS = (~0U);
public const uint VK_REMAINING_MIP_LEVELS = (~0U);
public const uint VK_SUBPASS_EXTERNAL = (~0U);
public const uint VK_TRUE = 1U;
public const ulong VK_WHOLE_SIZE = (~0UL);
public const uint VK_MAX_MEMORY_TYPES = 32U;
public const uint VK_MAX_PHYSICAL_DEVICE_NAME_SIZE = 256U;
public const uint VK_UUID_SIZE = 16U;
public const uint VK_MAX_EXTENSION_NAME_SIZE = 256U;
public const uint VK_MAX_DESCRIPTION_SIZE = 256U;
public const uint VK_MAX_MEMORY_HEAPS = 16U;
#endregion
#region VK_VERSION_1_1
//#define VK_API_VERSION_1_1 VK_MAKE_API_VERSION(0, 1, 1, 0)// Patch version should always be set to 0
public const uint VK_MAX_DEVICE_GROUP_SIZE = 32U;
public const uint VK_LUID_SIZE = 8U;
public const uint VK_QUEUE_FAMILY_EXTERNAL = (~1U);
#endregion
#region VK_VERSION_1_2
//#define VK_API_VERSION_1_2 VK_MAKE_API_VERSION(0, 1, 2, 0)// Patch version should always be set to 0
public const uint VK_MAX_DRIVER_NAME_SIZE = 256U;
public const uint VK_MAX_DRIVER_INFO_SIZE = 256U;
#endregion
#region VK_VERSION_1_3
//#define VK_API_VERSION_1_3 VK_MAKE_API_VERSION(0, 1, 3, 0)// Patch version should always be set to 0
#endregion
#region VK_KHR_surface
public const int VK_KHR_SURFACE_SPEC_VERSION = 25;
public const string VK_KHR_SURFACE_EXTENSION_NAME = "VK_KHR_surface";
#endregion
#region VK_KHR_swapchain
public const int VK_KHR_SWAPCHAIN_SPEC_VERSION = 70;
public const string VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_swapchain";
#endregion
#region VK_KHR_display
public const int VK_KHR_DISPLAY_SPEC_VERSION = 23;
public const string VK_KHR_DISPLAY_EXTENSION_NAME = "VK_KHR_display";
#endregion
#region VK_KHR_display_swapchain
public const int VK_KHR_DISPLAY_SWAPCHAIN_SPEC_VERSION = 10;
public const string VK_KHR_DISPLAY_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_display_swapchain";
#endregion
#region VK_KHR_sampler_mirror_clamp_to_edge
public const int VK_KHR_SAMPLER_MIRROR_CLAMP_TO_EDGE_SPEC_VERSION = 3;
public const string VK_KHR_SAMPLER_MIRROR_CLAMP_TO_EDGE_EXTENSION_NAME = "VK_KHR_sampler_mirror_clamp_to_edge";
#endregion
#region VK_KHR_dynamic_rendering
public const int VK_KHR_DYNAMIC_RENDERING_SPEC_VERSION = 1;
public const string VK_KHR_DYNAMIC_RENDERING_EXTENSION_NAME = "VK_KHR_dynamic_rendering";
#endregion
#region VK_KHR_multiview
public const int VK_KHR_MULTIVIEW_SPEC_VERSION = 1;
public const string VK_KHR_MULTIVIEW_EXTENSION_NAME = "VK_KHR_multiview";
#endregion
#region VK_KHR_get_physical_device_properties2
public const int VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_SPEC_VERSION = 2;
public const string VK_KHR_GET_PHYSICAL_DEVICE_PROPERTIES_2_EXTENSION_NAME = "VK_KHR_get_physical_device_properties2";
#endregion
#region VK_KHR_device_group
public const int VK_KHR_DEVICE_GROUP_SPEC_VERSION = 4;
public const string VK_KHR_DEVICE_GROUP_EXTENSION_NAME = "VK_KHR_device_group";
#endregion
#region VK_KHR_shader_draw_parameters
public const int VK_KHR_SHADER_DRAW_PARAMETERS_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_DRAW_PARAMETERS_EXTENSION_NAME = "VK_KHR_shader_draw_parameters";
#endregion
#region VK_KHR_maintenance1
public const int VK_KHR_MAINTENANCE_1_SPEC_VERSION = 2;
public const string VK_KHR_MAINTENANCE_1_EXTENSION_NAME = "VK_KHR_maintenance1";
public const int VK_KHR_MAINTENANCE1_SPEC_VERSION = VK_KHR_MAINTENANCE_1_SPEC_VERSION;
public const string VK_KHR_MAINTENANCE1_EXTENSION_NAME = VK_KHR_MAINTENANCE_1_EXTENSION_NAME;
#endregion
#region VK_KHR_device_group_creation
public const int VK_KHR_DEVICE_GROUP_CREATION_SPEC_VERSION = 1;
public const string VK_KHR_DEVICE_GROUP_CREATION_EXTENSION_NAME = "VK_KHR_device_group_creation";
public const uint VK_MAX_DEVICE_GROUP_SIZE_KHR = VK_MAX_DEVICE_GROUP_SIZE;
#endregion
#region VK_KHR_external_memory_capabilities
public const int VK_KHR_EXTERNAL_MEMORY_CAPABILITIES_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_MEMORY_CAPABILITIES_EXTENSION_NAME = "VK_KHR_external_memory_capabilities";
public const uint VK_LUID_SIZE_KHR = VK_LUID_SIZE;
#endregion
#region VK_KHR_external_memory
public const int VK_KHR_EXTERNAL_MEMORY_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_MEMORY_EXTENSION_NAME = "VK_KHR_external_memory";
public const uint VK_QUEUE_FAMILY_EXTERNAL_KHR = VK_QUEUE_FAMILY_EXTERNAL;
#endregion
#region VK_KHR_external_memory_fd
public const int VK_KHR_EXTERNAL_MEMORY_FD_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_MEMORY_FD_EXTENSION_NAME = "VK_KHR_external_memory_fd";
#endregion
#region VK_KHR_external_semaphore_capabilities
public const int VK_KHR_EXTERNAL_SEMAPHORE_CAPABILITIES_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_SEMAPHORE_CAPABILITIES_EXTENSION_NAME = "VK_KHR_external_semaphore_capabilities";
#endregion
#region VK_KHR_external_semaphore
public const int VK_KHR_EXTERNAL_SEMAPHORE_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_SEMAPHORE_EXTENSION_NAME = "VK_KHR_external_semaphore";
#endregion
#region VK_KHR_external_semaphore_fd
public const int VK_KHR_EXTERNAL_SEMAPHORE_FD_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_SEMAPHORE_FD_EXTENSION_NAME = "VK_KHR_external_semaphore_fd";
#endregion
#region VK_KHR_push_descriptor
public const int VK_KHR_PUSH_DESCRIPTOR_SPEC_VERSION = 2;
public const string VK_KHR_PUSH_DESCRIPTOR_EXTENSION_NAME = "VK_KHR_push_descriptor";
#endregion
#region VK_KHR_shader_float16_int8
public const int VK_KHR_SHADER_FLOAT16_INT8_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_FLOAT16_INT8_EXTENSION_NAME = "VK_KHR_shader_float16_int8";
#endregion
#region VK_KHR_16bit_storage
public const int VK_KHR_16BIT_STORAGE_SPEC_VERSION = 1;
public const string VK_KHR_16BIT_STORAGE_EXTENSION_NAME = "VK_KHR_16bit_storage";
#endregion
#region VK_KHR_incremental_present
public const int VK_KHR_INCREMENTAL_PRESENT_SPEC_VERSION = 2;
public const string VK_KHR_INCREMENTAL_PRESENT_EXTENSION_NAME = "VK_KHR_incremental_present";
#endregion
#region VK_KHR_descriptor_update_template
public const int VK_KHR_DESCRIPTOR_UPDATE_TEMPLATE_SPEC_VERSION = 1;
public const string VK_KHR_DESCRIPTOR_UPDATE_TEMPLATE_EXTENSION_NAME = "VK_KHR_descriptor_update_template";
#endregion
#region VK_KHR_imageless_framebuffer
public const int VK_KHR_IMAGELESS_FRAMEBUFFER_SPEC_VERSION = 1;
public const string VK_KHR_IMAGELESS_FRAMEBUFFER_EXTENSION_NAME = "VK_KHR_imageless_framebuffer";
#endregion
#region VK_KHR_create_renderpass2
public const int VK_KHR_CREATE_RENDERPASS_2_SPEC_VERSION = 1;
public const string VK_KHR_CREATE_RENDERPASS_2_EXTENSION_NAME = "VK_KHR_create_renderpass2";
#endregion
#region VK_KHR_shared_presentable_image
public const int VK_KHR_SHARED_PRESENTABLE_IMAGE_SPEC_VERSION = 1;
public const string VK_KHR_SHARED_PRESENTABLE_IMAGE_EXTENSION_NAME = "VK_KHR_shared_presentable_image";
#endregion
#region VK_KHR_external_fence_capabilities
public const int VK_KHR_EXTERNAL_FENCE_CAPABILITIES_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_FENCE_CAPABILITIES_EXTENSION_NAME = "VK_KHR_external_fence_capabilities";
#endregion
#region VK_KHR_external_fence
public const int VK_KHR_EXTERNAL_FENCE_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_FENCE_EXTENSION_NAME = "VK_KHR_external_fence";
#endregion
#region VK_KHR_external_fence_fd
public const int VK_KHR_EXTERNAL_FENCE_FD_SPEC_VERSION = 1;
public const string VK_KHR_EXTERNAL_FENCE_FD_EXTENSION_NAME = "VK_KHR_external_fence_fd";
#endregion
#region VK_KHR_performance_query
public const int VK_KHR_PERFORMANCE_QUERY_SPEC_VERSION = 1;
public const string VK_KHR_PERFORMANCE_QUERY_EXTENSION_NAME = "VK_KHR_performance_query";
#endregion
#region VK_KHR_maintenance2
public const int VK_KHR_MAINTENANCE_2_SPEC_VERSION = 1;
public const string VK_KHR_MAINTENANCE_2_EXTENSION_NAME = "VK_KHR_maintenance2";
public const int VK_KHR_MAINTENANCE2_SPEC_VERSION = VK_KHR_MAINTENANCE_2_SPEC_VERSION;
public const string VK_KHR_MAINTENANCE2_EXTENSION_NAME = VK_KHR_MAINTENANCE_2_EXTENSION_NAME;
#endregion
#region VK_KHR_get_surface_capabilities2
public const int VK_KHR_GET_SURFACE_CAPABILITIES_2_SPEC_VERSION = 1;
public const string VK_KHR_GET_SURFACE_CAPABILITIES_2_EXTENSION_NAME = "VK_KHR_get_surface_capabilities2";
#endregion
#region VK_KHR_variable_pointers
public const int VK_KHR_VARIABLE_POINTERS_SPEC_VERSION = 1;
public const string VK_KHR_VARIABLE_POINTERS_EXTENSION_NAME = "VK_KHR_variable_pointers";
#endregion
#region VK_KHR_get_display_properties2
public const int VK_KHR_GET_DISPLAY_PROPERTIES_2_SPEC_VERSION = 1;
public const string VK_KHR_GET_DISPLAY_PROPERTIES_2_EXTENSION_NAME = "VK_KHR_get_display_properties2";
#endregion
#region VK_KHR_dedicated_allocation
public const int VK_KHR_DEDICATED_ALLOCATION_SPEC_VERSION = 3;
public const string VK_KHR_DEDICATED_ALLOCATION_EXTENSION_NAME = "VK_KHR_dedicated_allocation";
#endregion
#region VK_KHR_storage_buffer_storage_class
public const int VK_KHR_STORAGE_BUFFER_STORAGE_CLASS_SPEC_VERSION = 1;
public const string VK_KHR_STORAGE_BUFFER_STORAGE_CLASS_EXTENSION_NAME = "VK_KHR_storage_buffer_storage_class";
#endregion
#region VK_KHR_relaxed_block_layout
public const int VK_KHR_RELAXED_BLOCK_LAYOUT_SPEC_VERSION = 1;
public const string VK_KHR_RELAXED_BLOCK_LAYOUT_EXTENSION_NAME = "VK_KHR_relaxed_block_layout";
#endregion
#region VK_KHR_get_memory_requirements2
public const int VK_KHR_GET_MEMORY_REQUIREMENTS_2_SPEC_VERSION = 1;
public const string VK_KHR_GET_MEMORY_REQUIREMENTS_2_EXTENSION_NAME = "VK_KHR_get_memory_requirements2";
#endregion
#region VK_KHR_image_format_list
public const int VK_KHR_IMAGE_FORMAT_LIST_SPEC_VERSION = 1;
public const string VK_KHR_IMAGE_FORMAT_LIST_EXTENSION_NAME = "VK_KHR_image_format_list";
#endregion
#region VK_KHR_sampler_ycbcr_conversion
public const int VK_KHR_SAMPLER_YCBCR_CONVERSION_SPEC_VERSION = 14;
public const string VK_KHR_SAMPLER_YCBCR_CONVERSION_EXTENSION_NAME = "VK_KHR_sampler_ycbcr_conversion";
#endregion
#region VK_KHR_bind_memory2
public const int VK_KHR_BIND_MEMORY_2_SPEC_VERSION = 1;
public const string VK_KHR_BIND_MEMORY_2_EXTENSION_NAME = "VK_KHR_bind_memory2";
#endregion
#region VK_KHR_maintenance3
public const int VK_KHR_MAINTENANCE_3_SPEC_VERSION = 1;
public const string VK_KHR_MAINTENANCE_3_EXTENSION_NAME = "VK_KHR_maintenance3";
public const int VK_KHR_MAINTENANCE3_SPEC_VERSION = VK_KHR_MAINTENANCE_3_SPEC_VERSION;
public const string VK_KHR_MAINTENANCE3_EXTENSION_NAME = VK_KHR_MAINTENANCE_3_EXTENSION_NAME;
#endregion
#region VK_KHR_draw_indirect_count
public const int VK_KHR_DRAW_INDIRECT_COUNT_SPEC_VERSION = 1;
public const string VK_KHR_DRAW_INDIRECT_COUNT_EXTENSION_NAME = "VK_KHR_draw_indirect_count";
#endregion
#region VK_KHR_shader_subgroup_extended_types
public const int VK_KHR_SHADER_SUBGROUP_EXTENDED_TYPES_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_SUBGROUP_EXTENDED_TYPES_EXTENSION_NAME = "VK_KHR_shader_subgroup_extended_types";
#endregion
#region VK_KHR_8bit_storage
public const int VK_KHR_8BIT_STORAGE_SPEC_VERSION = 1;
public const string VK_KHR_8BIT_STORAGE_EXTENSION_NAME = "VK_KHR_8bit_storage";
#endregion
#region VK_KHR_shader_atomic_int64
public const int VK_KHR_SHADER_ATOMIC_INT64_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_ATOMIC_INT64_EXTENSION_NAME = "VK_KHR_shader_atomic_int64";
#endregion
#region VK_KHR_shader_clock
public const int VK_KHR_SHADER_CLOCK_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_CLOCK_EXTENSION_NAME = "VK_KHR_shader_clock";
#endregion
#region VK_KHR_global_priority
public const uint VK_MAX_GLOBAL_PRIORITY_SIZE_KHR = 16U;
public const int VK_KHR_GLOBAL_PRIORITY_SPEC_VERSION = 1;
public const string VK_KHR_GLOBAL_PRIORITY_EXTENSION_NAME = "VK_KHR_global_priority";
#endregion
#region VK_KHR_driver_properties
public const int VK_KHR_DRIVER_PROPERTIES_SPEC_VERSION = 1;
public const string VK_KHR_DRIVER_PROPERTIES_EXTENSION_NAME = "VK_KHR_driver_properties";
public const uint VK_MAX_DRIVER_NAME_SIZE_KHR = VK_MAX_DRIVER_NAME_SIZE;
public const uint VK_MAX_DRIVER_INFO_SIZE_KHR = VK_MAX_DRIVER_INFO_SIZE;
#endregion
#region VK_KHR_shader_float_controls
public const int VK_KHR_SHADER_FLOAT_CONTROLS_SPEC_VERSION = 4;
public const string VK_KHR_SHADER_FLOAT_CONTROLS_EXTENSION_NAME = "VK_KHR_shader_float_controls";
#endregion
#region VK_KHR_depth_stencil_resolve
public const int VK_KHR_DEPTH_STENCIL_RESOLVE_SPEC_VERSION = 1;
public const string VK_KHR_DEPTH_STENCIL_RESOLVE_EXTENSION_NAME = "VK_KHR_depth_stencil_resolve";
#endregion
#region VK_KHR_swapchain_mutable_format
public const int VK_KHR_SWAPCHAIN_MUTABLE_FORMAT_SPEC_VERSION = 1;
public const string VK_KHR_SWAPCHAIN_MUTABLE_FORMAT_EXTENSION_NAME = "VK_KHR_swapchain_mutable_format";
#endregion
#region VK_KHR_timeline_semaphore
public const int VK_KHR_TIMELINE_SEMAPHORE_SPEC_VERSION = 2;
public const string VK_KHR_TIMELINE_SEMAPHORE_EXTENSION_NAME = "VK_KHR_timeline_semaphore";
#endregion
#region VK_KHR_vulkan_memory_model
public const int VK_KHR_VULKAN_MEMORY_MODEL_SPEC_VERSION = 3;
public const string VK_KHR_VULKAN_MEMORY_MODEL_EXTENSION_NAME = "VK_KHR_vulkan_memory_model";
#endregion
#region VK_KHR_shader_terminate_invocation
public const int VK_KHR_SHADER_TERMINATE_INVOCATION_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_TERMINATE_INVOCATION_EXTENSION_NAME = "VK_KHR_shader_terminate_invocation";
#endregion
#region VK_KHR_fragment_shading_rate
public const int VK_KHR_FRAGMENT_SHADING_RATE_SPEC_VERSION = 2;
public const string VK_KHR_FRAGMENT_SHADING_RATE_EXTENSION_NAME = "VK_KHR_fragment_shading_rate";
#endregion
#region VK_KHR_spirv_1_4
public const int VK_KHR_SPIRV_1_4_SPEC_VERSION = 1;
public const string VK_KHR_SPIRV_1_4_EXTENSION_NAME = "VK_KHR_spirv_1_4";
#endregion
#region VK_KHR_surface_protected_capabilities
public const int VK_KHR_SURFACE_PROTECTED_CAPABILITIES_SPEC_VERSION = 1;
public const string VK_KHR_SURFACE_PROTECTED_CAPABILITIES_EXTENSION_NAME = "VK_KHR_surface_protected_capabilities";
#endregion
#region VK_KHR_separate_depth_stencil_layouts
public const int VK_KHR_SEPARATE_DEPTH_STENCIL_LAYOUTS_SPEC_VERSION = 1;
public const string VK_KHR_SEPARATE_DEPTH_STENCIL_LAYOUTS_EXTENSION_NAME = "VK_KHR_separate_depth_stencil_layouts";
#endregion
#region VK_KHR_present_wait
public const int VK_KHR_PRESENT_WAIT_SPEC_VERSION = 1;
public const string VK_KHR_PRESENT_WAIT_EXTENSION_NAME = "VK_KHR_present_wait";
#endregion
#region VK_KHR_uniform_buffer_standard_layout
public const int VK_KHR_UNIFORM_BUFFER_STANDARD_LAYOUT_SPEC_VERSION = 1;
public const string VK_KHR_UNIFORM_BUFFER_STANDARD_LAYOUT_EXTENSION_NAME = "VK_KHR_uniform_buffer_standard_layout";
#endregion
#region VK_KHR_buffer_device_address
public const int VK_KHR_BUFFER_DEVICE_ADDRESS_SPEC_VERSION = 1;
public const string VK_KHR_BUFFER_DEVICE_ADDRESS_EXTENSION_NAME = "VK_KHR_buffer_device_address";
#endregion
#region VK_KHR_deferred_host_operations
public const int VK_KHR_DEFERRED_HOST_OPERATIONS_SPEC_VERSION = 4;
public const string VK_KHR_DEFERRED_HOST_OPERATIONS_EXTENSION_NAME = "VK_KHR_deferred_host_operations";
#endregion
#region VK_KHR_pipeline_executable_properties
public const int VK_KHR_PIPELINE_EXECUTABLE_PROPERTIES_SPEC_VERSION = 1;
public const string VK_KHR_PIPELINE_EXECUTABLE_PROPERTIES_EXTENSION_NAME = "VK_KHR_pipeline_executable_properties";
#endregion
#region VK_KHR_shader_integer_dot_product
public const int VK_KHR_SHADER_INTEGER_DOT_PRODUCT_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_INTEGER_DOT_PRODUCT_EXTENSION_NAME = "VK_KHR_shader_integer_dot_product";
#endregion
#region VK_KHR_pipeline_library
public const int VK_KHR_PIPELINE_LIBRARY_SPEC_VERSION = 1;
public const string VK_KHR_PIPELINE_LIBRARY_EXTENSION_NAME = "VK_KHR_pipeline_library";
#endregion
#region VK_KHR_shader_non_semantic_info
public const int VK_KHR_SHADER_NON_SEMANTIC_INFO_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_NON_SEMANTIC_INFO_EXTENSION_NAME = "VK_KHR_shader_non_semantic_info";
#endregion
#region VK_KHR_present_id
public const int VK_KHR_PRESENT_ID_SPEC_VERSION = 1;
public const string VK_KHR_PRESENT_ID_EXTENSION_NAME = "VK_KHR_present_id";
#endregion
#region VK_KHR_synchronization2
public const int VK_KHR_SYNCHRONIZATION_2_SPEC_VERSION = 1;
public const string VK_KHR_SYNCHRONIZATION_2_EXTENSION_NAME = "VK_KHR_synchronization2";
#endregion
#region VK_KHR_fragment_shader_barycentric
public const int VK_KHR_FRAGMENT_SHADER_BARYCENTRIC_SPEC_VERSION = 1;
public const string VK_KHR_FRAGMENT_SHADER_BARYCENTRIC_EXTENSION_NAME = "VK_KHR_fragment_shader_barycentric";
#endregion
#region VK_KHR_shader_subgroup_uniform_control_flow
public const int VK_KHR_SHADER_SUBGROUP_UNIFORM_CONTROL_FLOW_SPEC_VERSION = 1;
public const string VK_KHR_SHADER_SUBGROUP_UNIFORM_CONTROL_FLOW_EXTENSION_NAME = "VK_KHR_shader_subgroup_uniform_control_flow";
#endregion
#region VK_KHR_zero_initialize_workgroup_memory
public const int VK_KHR_ZERO_INITIALIZE_WORKGROUP_MEMORY_SPEC_VERSION = 1;
public const string VK_KHR_ZERO_INITIALIZE_WORKGROUP_MEMORY_EXTENSION_NAME = "VK_KHR_zero_initialize_workgroup_memory";
#endregion
#region VK_KHR_workgroup_memory_explicit_layout
public const int VK_KHR_WORKGROUP_MEMORY_EXPLICIT_LAYOUT_SPEC_VERSION = 1;
public const string VK_KHR_WORKGROUP_MEMORY_EXPLICIT_LAYOUT_EXTENSION_NAME = "VK_KHR_workgroup_memory_explicit_layout";
#endregion
#region VK_KHR_copy_commands2
public const int VK_KHR_COPY_COMMANDS_2_SPEC_VERSION = 1;
public const string VK_KHR_COPY_COMMANDS_2_EXTENSION_NAME = "VK_KHR_copy_commands2";
#endregion
#region VK_KHR_format_feature_flags2
public const int VK_KHR_FORMAT_FEATURE_FLAGS_2_SPEC_VERSION = 2;
public const string VK_KHR_FORMAT_FEATURE_FLAGS_2_EXTENSION_NAME = "VK_KHR_format_feature_flags2";
#endregion
#region VK_KHR_ray_tracing_maintenance1
public const int VK_KHR_RAY_TRACING_MAINTENANCE_1_SPEC_VERSION = 1;
public const string VK_KHR_RAY_TRACING_MAINTENANCE_1_EXTENSION_NAME = "VK_KHR_ray_tracing_maintenance1";
#endregion
#region VK_KHR_portability_enumeration
public const int VK_KHR_PORTABILITY_ENUMERATION_SPEC_VERSION = 1;
public const string VK_KHR_PORTABILITY_ENUMERATION_EXTENSION_NAME = "VK_KHR_portability_enumeration";
#endregion
#region VK_KHR_maintenance4
public const int VK_KHR_MAINTENANCE_4_SPEC_VERSION = 2;
public const string VK_KHR_MAINTENANCE_4_EXTENSION_NAME = "VK_KHR_maintenance4";
#endregion
#region VK_EXT_debug_report
public const int VK_EXT_DEBUG_REPORT_SPEC_VERSION = 10;
public const string VK_EXT_DEBUG_REPORT_EXTENSION_NAME = "VK_EXT_debug_report";
#endregion
#region VK_NV_glsl_shader
public const int VK_NV_GLSL_SHADER_SPEC_VERSION = 1;
public const string VK_NV_GLSL_SHADER_EXTENSION_NAME = "VK_NV_glsl_shader";
#endregion
#region VK_EXT_depth_range_unrestricted
public const int VK_EXT_DEPTH_RANGE_UNRESTRICTED_SPEC_VERSION = 1;
public const string VK_EXT_DEPTH_RANGE_UNRESTRICTED_EXTENSION_NAME = "VK_EXT_depth_range_unrestricted";
#endregion
#region VK_IMG_filter_cubic
public const int VK_IMG_FILTER_CUBIC_SPEC_VERSION = 1;
public const string VK_IMG_FILTER_CUBIC_EXTENSION_NAME = "VK_IMG_filter_cubic";
#endregion
#region VK_AMD_rasterization_order
public const int VK_AMD_RASTERIZATION_ORDER_SPEC_VERSION = 1;
public const string VK_AMD_RASTERIZATION_ORDER_EXTENSION_NAME = "VK_AMD_rasterization_order";
#endregion
#region VK_AMD_shader_trinary_minmax
public const int VK_AMD_SHADER_TRINARY_MINMAX_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_TRINARY_MINMAX_EXTENSION_NAME = "VK_AMD_shader_trinary_minmax";
#endregion
#region VK_AMD_shader_explicit_vertex_parameter
public const int VK_AMD_SHADER_EXPLICIT_VERTEX_PARAMETER_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_EXPLICIT_VERTEX_PARAMETER_EXTENSION_NAME = "VK_AMD_shader_explicit_vertex_parameter";
#endregion
#region VK_EXT_debug_marker
public const int VK_EXT_DEBUG_MARKER_SPEC_VERSION = 4;
public const string VK_EXT_DEBUG_MARKER_EXTENSION_NAME = "VK_EXT_debug_marker";
#endregion
#region VK_AMD_gcn_shader
public const int VK_AMD_GCN_SHADER_SPEC_VERSION = 1;
public const string VK_AMD_GCN_SHADER_EXTENSION_NAME = "VK_AMD_gcn_shader";
#endregion
#region VK_NV_dedicated_allocation
public const int VK_NV_DEDICATED_ALLOCATION_SPEC_VERSION = 1;
public const string VK_NV_DEDICATED_ALLOCATION_EXTENSION_NAME = "VK_NV_dedicated_allocation";
#endregion
#region VK_EXT_transform_feedback
public const int VK_EXT_TRANSFORM_FEEDBACK_SPEC_VERSION = 1;
public const string VK_EXT_TRANSFORM_FEEDBACK_EXTENSION_NAME = "VK_EXT_transform_feedback";
#endregion
#region VK_NVX_binary_import
public const int VK_NVX_BINARY_IMPORT_SPEC_VERSION = 1;
public const string VK_NVX_BINARY_IMPORT_EXTENSION_NAME = "VK_NVX_binary_import";
#endregion
#region VK_NVX_image_view_handle
public const int VK_NVX_IMAGE_VIEW_HANDLE_SPEC_VERSION = 2;
public const string VK_NVX_IMAGE_VIEW_HANDLE_EXTENSION_NAME = "VK_NVX_image_view_handle";
#endregion
#region VK_AMD_draw_indirect_count
public const int VK_AMD_DRAW_INDIRECT_COUNT_SPEC_VERSION = 2;
public const string VK_AMD_DRAW_INDIRECT_COUNT_EXTENSION_NAME = "VK_AMD_draw_indirect_count";
#endregion
#region VK_AMD_negative_viewport_height
public const int VK_AMD_NEGATIVE_VIEWPORT_HEIGHT_SPEC_VERSION = 1;
public const string VK_AMD_NEGATIVE_VIEWPORT_HEIGHT_EXTENSION_NAME = "VK_AMD_negative_viewport_height";
#endregion
#region VK_AMD_gpu_shader_half_float
public const int VK_AMD_GPU_SHADER_HALF_FLOAT_SPEC_VERSION = 2;
public const string VK_AMD_GPU_SHADER_HALF_FLOAT_EXTENSION_NAME = "VK_AMD_gpu_shader_half_float";
#endregion
#region VK_AMD_shader_ballot
public const int VK_AMD_SHADER_BALLOT_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_BALLOT_EXTENSION_NAME = "VK_AMD_shader_ballot";
#endregion
#region VK_AMD_texture_gather_bias_lod
public const int VK_AMD_TEXTURE_GATHER_BIAS_LOD_SPEC_VERSION = 1;
public const string VK_AMD_TEXTURE_GATHER_BIAS_LOD_EXTENSION_NAME = "VK_AMD_texture_gather_bias_lod";
#endregion
#region VK_AMD_shader_info
public const int VK_AMD_SHADER_INFO_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_INFO_EXTENSION_NAME = "VK_AMD_shader_info";
#endregion
#region VK_AMD_shader_image_load_store_lod
public const int VK_AMD_SHADER_IMAGE_LOAD_STORE_LOD_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_IMAGE_LOAD_STORE_LOD_EXTENSION_NAME = "VK_AMD_shader_image_load_store_lod";
#endregion
#region VK_NV_corner_sampled_image
public const int VK_NV_CORNER_SAMPLED_IMAGE_SPEC_VERSION = 2;
public const string VK_NV_CORNER_SAMPLED_IMAGE_EXTENSION_NAME = "VK_NV_corner_sampled_image";
#endregion
#region VK_IMG_format_pvrtc
public const int VK_IMG_FORMAT_PVRTC_SPEC_VERSION = 1;
public const string VK_IMG_FORMAT_PVRTC_EXTENSION_NAME = "VK_IMG_format_pvrtc";
#endregion
#region VK_NV_external_memory_capabilities
public const int VK_NV_EXTERNAL_MEMORY_CAPABILITIES_SPEC_VERSION = 1;
public const string VK_NV_EXTERNAL_MEMORY_CAPABILITIES_EXTENSION_NAME = "VK_NV_external_memory_capabilities";
#endregion
#region VK_NV_external_memory
public const int VK_NV_EXTERNAL_MEMORY_SPEC_VERSION = 1;
public const string VK_NV_EXTERNAL_MEMORY_EXTENSION_NAME = "VK_NV_external_memory";
#endregion
#region VK_EXT_validation_flags
public const int VK_EXT_VALIDATION_FLAGS_SPEC_VERSION = 2;
public const string VK_EXT_VALIDATION_FLAGS_EXTENSION_NAME = "VK_EXT_validation_flags";
#endregion
#region VK_EXT_shader_subgroup_ballot
public const int VK_EXT_SHADER_SUBGROUP_BALLOT_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_SUBGROUP_BALLOT_EXTENSION_NAME = "VK_EXT_shader_subgroup_ballot";
#endregion
#region VK_EXT_shader_subgroup_vote
public const int VK_EXT_SHADER_SUBGROUP_VOTE_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_SUBGROUP_VOTE_EXTENSION_NAME = "VK_EXT_shader_subgroup_vote";
#endregion
#region VK_EXT_texture_compression_astc_hdr
public const int VK_EXT_TEXTURE_COMPRESSION_ASTC_HDR_SPEC_VERSION = 1;
public const string VK_EXT_TEXTURE_COMPRESSION_ASTC_HDR_EXTENSION_NAME = "VK_EXT_texture_compression_astc_hdr";
#endregion
#region VK_EXT_astc_decode_mode
public const int VK_EXT_ASTC_DECODE_MODE_SPEC_VERSION = 1;
public const string VK_EXT_ASTC_DECODE_MODE_EXTENSION_NAME = "VK_EXT_astc_decode_mode";
#endregion
#region VK_EXT_pipeline_robustness
public const int VK_EXT_PIPELINE_ROBUSTNESS_SPEC_VERSION = 1;
public const string VK_EXT_PIPELINE_ROBUSTNESS_EXTENSION_NAME = "VK_EXT_pipeline_robustness";
#endregion
#region VK_EXT_conditional_rendering
public const int VK_EXT_CONDITIONAL_RENDERING_SPEC_VERSION = 2;
public const string VK_EXT_CONDITIONAL_RENDERING_EXTENSION_NAME = "VK_EXT_conditional_rendering";
#endregion
#region VK_NV_clip_space_w_scaling
public const int VK_NV_CLIP_SPACE_W_SCALING_SPEC_VERSION = 1;
public const string VK_NV_CLIP_SPACE_W_SCALING_EXTENSION_NAME = "VK_NV_clip_space_w_scaling";
#endregion
#region VK_EXT_direct_mode_display
public const int VK_EXT_DIRECT_MODE_DISPLAY_SPEC_VERSION = 1;
public const string VK_EXT_DIRECT_MODE_DISPLAY_EXTENSION_NAME = "VK_EXT_direct_mode_display";
#endregion
#region VK_EXT_display_surface_counter
public const int VK_EXT_DISPLAY_SURFACE_COUNTER_SPEC_VERSION = 1;
public const string VK_EXT_DISPLAY_SURFACE_COUNTER_EXTENSION_NAME = "VK_EXT_display_surface_counter";
#endregion
#region VK_EXT_display_control
public const int VK_EXT_DISPLAY_CONTROL_SPEC_VERSION = 1;
public const string VK_EXT_DISPLAY_CONTROL_EXTENSION_NAME = "VK_EXT_display_control";
#endregion
#region VK_GOOGLE_display_timing
public const int VK_GOOGLE_DISPLAY_TIMING_SPEC_VERSION = 1;
public const string VK_GOOGLE_DISPLAY_TIMING_EXTENSION_NAME = "VK_GOOGLE_display_timing";
#endregion
#region VK_NV_sample_mask_override_coverage
public const int VK_NV_SAMPLE_MASK_OVERRIDE_COVERAGE_SPEC_VERSION = 1;
public const string VK_NV_SAMPLE_MASK_OVERRIDE_COVERAGE_EXTENSION_NAME = "VK_NV_sample_mask_override_coverage";
#endregion
#region VK_NV_geometry_shader_passthrough
public const int VK_NV_GEOMETRY_SHADER_PASSTHROUGH_SPEC_VERSION = 1;
public const string VK_NV_GEOMETRY_SHADER_PASSTHROUGH_EXTENSION_NAME = "VK_NV_geometry_shader_passthrough";
#endregion
#region VK_NV_viewport_array2
public const int VK_NV_VIEWPORT_ARRAY_2_SPEC_VERSION = 1;
public const string VK_NV_VIEWPORT_ARRAY_2_EXTENSION_NAME = "VK_NV_viewport_array2";
public const int VK_NV_VIEWPORT_ARRAY2_SPEC_VERSION = VK_NV_VIEWPORT_ARRAY_2_SPEC_VERSION;
public const string VK_NV_VIEWPORT_ARRAY2_EXTENSION_NAME = VK_NV_VIEWPORT_ARRAY_2_EXTENSION_NAME;
#endregion
#region VK_NVX_multiview_per_view_attributes
public const int VK_NVX_MULTIVIEW_PER_VIEW_ATTRIBUTES_SPEC_VERSION = 1;
public const string VK_NVX_MULTIVIEW_PER_VIEW_ATTRIBUTES_EXTENSION_NAME = "VK_NVX_multiview_per_view_attributes";
#endregion
#region VK_NV_viewport_swizzle
public const int VK_NV_VIEWPORT_SWIZZLE_SPEC_VERSION = 1;
public const string VK_NV_VIEWPORT_SWIZZLE_EXTENSION_NAME = "VK_NV_viewport_swizzle";
#endregion
#region VK_EXT_discard_rectangles
public const int VK_EXT_DISCARD_RECTANGLES_SPEC_VERSION = 1;
public const string VK_EXT_DISCARD_RECTANGLES_EXTENSION_NAME = "VK_EXT_discard_rectangles";
#endregion
#region VK_EXT_conservative_rasterization
public const int VK_EXT_CONSERVATIVE_RASTERIZATION_SPEC_VERSION = 1;
public const string VK_EXT_CONSERVATIVE_RASTERIZATION_EXTENSION_NAME = "VK_EXT_conservative_rasterization";
#endregion
#region VK_EXT_depth_clip_enable
public const int VK_EXT_DEPTH_CLIP_ENABLE_SPEC_VERSION = 1;
public const string VK_EXT_DEPTH_CLIP_ENABLE_EXTENSION_NAME = "VK_EXT_depth_clip_enable";
#endregion
#region VK_EXT_swapchain_colorspace
public const int VK_EXT_SWAPCHAIN_COLOR_SPACE_SPEC_VERSION = 4;
public const string VK_EXT_SWAPCHAIN_COLOR_SPACE_EXTENSION_NAME = "VK_EXT_swapchain_colorspace";
#endregion
#region VK_EXT_hdr_metadata
public const int VK_EXT_HDR_METADATA_SPEC_VERSION = 2;
public const string VK_EXT_HDR_METADATA_EXTENSION_NAME = "VK_EXT_hdr_metadata";
#endregion
#region VK_EXT_external_memory_dma_buf
public const int VK_EXT_EXTERNAL_MEMORY_DMA_BUF_SPEC_VERSION = 1;
public const string VK_EXT_EXTERNAL_MEMORY_DMA_BUF_EXTENSION_NAME = "VK_EXT_external_memory_dma_buf";
#endregion
#region VK_EXT_queue_family_foreign
public const int VK_EXT_QUEUE_FAMILY_FOREIGN_SPEC_VERSION = 1;
public const string VK_EXT_QUEUE_FAMILY_FOREIGN_EXTENSION_NAME = "VK_EXT_queue_family_foreign";
public const uint VK_QUEUE_FAMILY_FOREIGN_EXT = (~2U);
#endregion
#region VK_EXT_debug_utils
public const int VK_EXT_DEBUG_UTILS_SPEC_VERSION = 2;
public const string VK_EXT_DEBUG_UTILS_EXTENSION_NAME = "VK_EXT_debug_utils";
#endregion
#region VK_EXT_sampler_filter_minmax
public const int VK_EXT_SAMPLER_FILTER_MINMAX_SPEC_VERSION = 2;
public const string VK_EXT_SAMPLER_FILTER_MINMAX_EXTENSION_NAME = "VK_EXT_sampler_filter_minmax";
#endregion
#region VK_AMD_gpu_shader_int16
public const int VK_AMD_GPU_SHADER_INT16_SPEC_VERSION = 2;
public const string VK_AMD_GPU_SHADER_INT16_EXTENSION_NAME = "VK_AMD_gpu_shader_int16";
#endregion
#region VK_AMD_mixed_attachment_samples
public const int VK_AMD_MIXED_ATTACHMENT_SAMPLES_SPEC_VERSION = 1;
public const string VK_AMD_MIXED_ATTACHMENT_SAMPLES_EXTENSION_NAME = "VK_AMD_mixed_attachment_samples";
#endregion
#region VK_AMD_shader_fragment_mask
public const int VK_AMD_SHADER_FRAGMENT_MASK_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_FRAGMENT_MASK_EXTENSION_NAME = "VK_AMD_shader_fragment_mask";
#endregion
#region VK_EXT_inline_uniform_block
public const int VK_EXT_INLINE_UNIFORM_BLOCK_SPEC_VERSION = 1;
public const string VK_EXT_INLINE_UNIFORM_BLOCK_EXTENSION_NAME = "VK_EXT_inline_uniform_block";
#endregion
#region VK_EXT_shader_stencil_export
public const int VK_EXT_SHADER_STENCIL_EXPORT_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_STENCIL_EXPORT_EXTENSION_NAME = "VK_EXT_shader_stencil_export";
#endregion
#region VK_EXT_sample_locations
public const int VK_EXT_SAMPLE_LOCATIONS_SPEC_VERSION = 1;
public const string VK_EXT_SAMPLE_LOCATIONS_EXTENSION_NAME = "VK_EXT_sample_locations";
#endregion
#region VK_EXT_blend_operation_advanced
public const int VK_EXT_BLEND_OPERATION_ADVANCED_SPEC_VERSION = 2;
public const string VK_EXT_BLEND_OPERATION_ADVANCED_EXTENSION_NAME = "VK_EXT_blend_operation_advanced";
#endregion
#region VK_NV_fragment_coverage_to_color
public const int VK_NV_FRAGMENT_COVERAGE_TO_COLOR_SPEC_VERSION = 1;
public const string VK_NV_FRAGMENT_COVERAGE_TO_COLOR_EXTENSION_NAME = "VK_NV_fragment_coverage_to_color";
#endregion
#region VK_NV_framebuffer_mixed_samples
public const int VK_NV_FRAMEBUFFER_MIXED_SAMPLES_SPEC_VERSION = 1;
public const string VK_NV_FRAMEBUFFER_MIXED_SAMPLES_EXTENSION_NAME = "VK_NV_framebuffer_mixed_samples";
#endregion
#region VK_NV_fill_rectangle
public const int VK_NV_FILL_RECTANGLE_SPEC_VERSION = 1;
public const string VK_NV_FILL_RECTANGLE_EXTENSION_NAME = "VK_NV_fill_rectangle";
#endregion
#region VK_NV_shader_sm_builtins
public const int VK_NV_SHADER_SM_BUILTINS_SPEC_VERSION = 1;
public const string VK_NV_SHADER_SM_BUILTINS_EXTENSION_NAME = "VK_NV_shader_sm_builtins";
#endregion
#region VK_EXT_post_depth_coverage
public const int VK_EXT_POST_DEPTH_COVERAGE_SPEC_VERSION = 1;
public const string VK_EXT_POST_DEPTH_COVERAGE_EXTENSION_NAME = "VK_EXT_post_depth_coverage";
#endregion
#region VK_EXT_image_drm_format_modifier
public const int VK_EXT_IMAGE_DRM_FORMAT_MODIFIER_SPEC_VERSION = 2;
public const string VK_EXT_IMAGE_DRM_FORMAT_MODIFIER_EXTENSION_NAME = "VK_EXT_image_drm_format_modifier";
#endregion
#region VK_EXT_validation_cache
public const int VK_EXT_VALIDATION_CACHE_SPEC_VERSION = 1;
public const string VK_EXT_VALIDATION_CACHE_EXTENSION_NAME = "VK_EXT_validation_cache";
#endregion
#region VK_EXT_descriptor_indexing
public const int VK_EXT_DESCRIPTOR_INDEXING_SPEC_VERSION = 2;
public const string VK_EXT_DESCRIPTOR_INDEXING_EXTENSION_NAME = "VK_EXT_descriptor_indexing";
#endregion
#region VK_EXT_shader_viewport_index_layer
public const int VK_EXT_SHADER_VIEWPORT_INDEX_LAYER_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_VIEWPORT_INDEX_LAYER_EXTENSION_NAME = "VK_EXT_shader_viewport_index_layer";
#endregion
#region VK_NV_shading_rate_image
public const int VK_NV_SHADING_RATE_IMAGE_SPEC_VERSION = 3;
public const string VK_NV_SHADING_RATE_IMAGE_EXTENSION_NAME = "VK_NV_shading_rate_image";
#endregion
#region VK_NV_ray_tracing
public const int VK_NV_RAY_TRACING_SPEC_VERSION = 3;
public const string VK_NV_RAY_TRACING_EXTENSION_NAME = "VK_NV_ray_tracing";
public const uint VK_SHADER_UNUSED_KHR = (~0U);
public const uint VK_SHADER_UNUSED_NV = VK_SHADER_UNUSED_KHR;
#endregion
#region VK_NV_representative_fragment_test
public const int VK_NV_REPRESENTATIVE_FRAGMENT_TEST_SPEC_VERSION = 2;
public const string VK_NV_REPRESENTATIVE_FRAGMENT_TEST_EXTENSION_NAME = "VK_NV_representative_fragment_test";
#endregion
#region VK_EXT_filter_cubic
public const int VK_EXT_FILTER_CUBIC_SPEC_VERSION = 3;
public const string VK_EXT_FILTER_CUBIC_EXTENSION_NAME = "VK_EXT_filter_cubic";
#endregion
#region VK_QCOM_render_pass_shader_resolve
public const int VK_QCOM_RENDER_PASS_SHADER_RESOLVE_SPEC_VERSION = 4;
public const string VK_QCOM_RENDER_PASS_SHADER_RESOLVE_EXTENSION_NAME = "VK_QCOM_render_pass_shader_resolve";
#endregion
#region VK_EXT_global_priority
public const int VK_EXT_GLOBAL_PRIORITY_SPEC_VERSION = 2;
public const string VK_EXT_GLOBAL_PRIORITY_EXTENSION_NAME = "VK_EXT_global_priority";
#endregion
#region VK_EXT_external_memory_host
public const int VK_EXT_EXTERNAL_MEMORY_HOST_SPEC_VERSION = 1;
public const string VK_EXT_EXTERNAL_MEMORY_HOST_EXTENSION_NAME = "VK_EXT_external_memory_host";
#endregion
#region VK_AMD_buffer_marker
public const int VK_AMD_BUFFER_MARKER_SPEC_VERSION = 1;
public const string VK_AMD_BUFFER_MARKER_EXTENSION_NAME = "VK_AMD_buffer_marker";
#endregion
#region VK_AMD_pipeline_compiler_control
public const int VK_AMD_PIPELINE_COMPILER_CONTROL_SPEC_VERSION = 1;
public const string VK_AMD_PIPELINE_COMPILER_CONTROL_EXTENSION_NAME = "VK_AMD_pipeline_compiler_control";
#endregion
#region VK_EXT_calibrated_timestamps
public const int VK_EXT_CALIBRATED_TIMESTAMPS_SPEC_VERSION = 2;
public const string VK_EXT_CALIBRATED_TIMESTAMPS_EXTENSION_NAME = "VK_EXT_calibrated_timestamps";
#endregion
#region VK_AMD_shader_core_properties
public const int VK_AMD_SHADER_CORE_PROPERTIES_SPEC_VERSION = 2;
public const string VK_AMD_SHADER_CORE_PROPERTIES_EXTENSION_NAME = "VK_AMD_shader_core_properties";
#endregion
#region VK_AMD_memory_overallocation_behavior
public const int VK_AMD_MEMORY_OVERALLOCATION_BEHAVIOR_SPEC_VERSION = 1;
public const string VK_AMD_MEMORY_OVERALLOCATION_BEHAVIOR_EXTENSION_NAME = "VK_AMD_memory_overallocation_behavior";
#endregion
#region VK_EXT_vertex_attribute_divisor
public const int VK_EXT_VERTEX_ATTRIBUTE_DIVISOR_SPEC_VERSION = 3;
public const string VK_EXT_VERTEX_ATTRIBUTE_DIVISOR_EXTENSION_NAME = "VK_EXT_vertex_attribute_divisor";
#endregion
#region VK_EXT_pipeline_creation_feedback
public const int VK_EXT_PIPELINE_CREATION_FEEDBACK_SPEC_VERSION = 1;
public const string VK_EXT_PIPELINE_CREATION_FEEDBACK_EXTENSION_NAME = "VK_EXT_pipeline_creation_feedback";
#endregion
#region VK_NV_shader_subgroup_partitioned
public const int VK_NV_SHADER_SUBGROUP_PARTITIONED_SPEC_VERSION = 1;
public const string VK_NV_SHADER_SUBGROUP_PARTITIONED_EXTENSION_NAME = "VK_NV_shader_subgroup_partitioned";
#endregion
#region VK_NV_compute_shader_derivatives
public const int VK_NV_COMPUTE_SHADER_DERIVATIVES_SPEC_VERSION = 1;
public const string VK_NV_COMPUTE_SHADER_DERIVATIVES_EXTENSION_NAME = "VK_NV_compute_shader_derivatives";
#endregion
#region VK_NV_mesh_shader
public const int VK_NV_MESH_SHADER_SPEC_VERSION = 1;
public const string VK_NV_MESH_SHADER_EXTENSION_NAME = "VK_NV_mesh_shader";
#endregion
#region VK_NV_fragment_shader_barycentric
public const int VK_NV_FRAGMENT_SHADER_BARYCENTRIC_SPEC_VERSION = 1;
public const string VK_NV_FRAGMENT_SHADER_BARYCENTRIC_EXTENSION_NAME = "VK_NV_fragment_shader_barycentric";
#endregion
#region VK_NV_shader_image_footprint
public const int VK_NV_SHADER_IMAGE_FOOTPRINT_SPEC_VERSION = 2;
public const string VK_NV_SHADER_IMAGE_FOOTPRINT_EXTENSION_NAME = "VK_NV_shader_image_footprint";
#endregion
#region VK_NV_scissor_exclusive
public const int VK_NV_SCISSOR_EXCLUSIVE_SPEC_VERSION = 1;
public const string VK_NV_SCISSOR_EXCLUSIVE_EXTENSION_NAME = "VK_NV_scissor_exclusive";
#endregion
#region VK_NV_device_diagnostic_checkpoints
public const int VK_NV_DEVICE_DIAGNOSTIC_CHECKPOINTS_SPEC_VERSION = 2;
public const string VK_NV_DEVICE_DIAGNOSTIC_CHECKPOINTS_EXTENSION_NAME = "VK_NV_device_diagnostic_checkpoints";
#endregion
#region VK_INTEL_shader_integer_functions2
public const int VK_INTEL_SHADER_INTEGER_FUNCTIONS_2_SPEC_VERSION = 1;
public const string VK_INTEL_SHADER_INTEGER_FUNCTIONS_2_EXTENSION_NAME = "VK_INTEL_shader_integer_functions2";
#endregion
#region VK_INTEL_performance_query
public const int VK_INTEL_PERFORMANCE_QUERY_SPEC_VERSION = 2;
public const string VK_INTEL_PERFORMANCE_QUERY_EXTENSION_NAME = "VK_INTEL_performance_query";
#endregion
#region VK_EXT_pci_bus_info
public const int VK_EXT_PCI_BUS_INFO_SPEC_VERSION = 2;
public const string VK_EXT_PCI_BUS_INFO_EXTENSION_NAME = "VK_EXT_pci_bus_info";
#endregion
#region VK_AMD_display_native_hdr
public const int VK_AMD_DISPLAY_NATIVE_HDR_SPEC_VERSION = 1;
public const string VK_AMD_DISPLAY_NATIVE_HDR_EXTENSION_NAME = "VK_AMD_display_native_hdr";
#endregion
#region VK_EXT_fragment_density_map
public const int VK_EXT_FRAGMENT_DENSITY_MAP_SPEC_VERSION = 2;
public const string VK_EXT_FRAGMENT_DENSITY_MAP_EXTENSION_NAME = "VK_EXT_fragment_density_map";
#endregion
#region VK_EXT_scalar_block_layout
public const int VK_EXT_SCALAR_BLOCK_LAYOUT_SPEC_VERSION = 1;
public const string VK_EXT_SCALAR_BLOCK_LAYOUT_EXTENSION_NAME = "VK_EXT_scalar_block_layout";
#endregion
#region VK_GOOGLE_hlsl_functionality1
public const int VK_GOOGLE_HLSL_FUNCTIONALITY_1_SPEC_VERSION = 1;
public const string VK_GOOGLE_HLSL_FUNCTIONALITY_1_EXTENSION_NAME = "VK_GOOGLE_hlsl_functionality1";
public const uint VK_GOOGLE_HLSL_FUNCTIONALITY1_SPEC_VERSION = VK_GOOGLE_HLSL_FUNCTIONALITY_1_SPEC_VERSION;
public const string VK_GOOGLE_HLSL_FUNCTIONALITY1_EXTENSION_NAME = VK_GOOGLE_HLSL_FUNCTIONALITY_1_EXTENSION_NAME;
#endregion
#region VK_GOOGLE_decorate_string
public const int VK_GOOGLE_DECORATE_STRING_SPEC_VERSION = 1;
public const string VK_GOOGLE_DECORATE_STRING_EXTENSION_NAME = "VK_GOOGLE_decorate_string";
#endregion
#region VK_EXT_subgroup_size_control
public const int VK_EXT_SUBGROUP_SIZE_CONTROL_SPEC_VERSION = 2;
public const string VK_EXT_SUBGROUP_SIZE_CONTROL_EXTENSION_NAME = "VK_EXT_subgroup_size_control";
#endregion
#region VK_AMD_shader_core_properties2
public const int VK_AMD_SHADER_CORE_PROPERTIES_2_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_CORE_PROPERTIES_2_EXTENSION_NAME = "VK_AMD_shader_core_properties2";
#endregion
#region VK_AMD_device_coherent_memory
public const int VK_AMD_DEVICE_COHERENT_MEMORY_SPEC_VERSION = 1;
public const string VK_AMD_DEVICE_COHERENT_MEMORY_EXTENSION_NAME = "VK_AMD_device_coherent_memory";
#endregion
#region VK_EXT_shader_image_atomic_int64
public const int VK_EXT_SHADER_IMAGE_ATOMIC_INT64_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_IMAGE_ATOMIC_INT64_EXTENSION_NAME = "VK_EXT_shader_image_atomic_int64";
#endregion
#region VK_EXT_memory_budget
public const int VK_EXT_MEMORY_BUDGET_SPEC_VERSION = 1;
public const string VK_EXT_MEMORY_BUDGET_EXTENSION_NAME = "VK_EXT_memory_budget";
#endregion
#region VK_EXT_memory_priority
public const int VK_EXT_MEMORY_PRIORITY_SPEC_VERSION = 1;
public const string VK_EXT_MEMORY_PRIORITY_EXTENSION_NAME = "VK_EXT_memory_priority";
#endregion
#region VK_NV_dedicated_allocation_image_aliasing
public const int VK_NV_DEDICATED_ALLOCATION_IMAGE_ALIASING_SPEC_VERSION = 1;
public const string VK_NV_DEDICATED_ALLOCATION_IMAGE_ALIASING_EXTENSION_NAME = "VK_NV_dedicated_allocation_image_aliasing";
#endregion
#region VK_EXT_buffer_device_address
public const int VK_EXT_BUFFER_DEVICE_ADDRESS_SPEC_VERSION = 2;
public const string VK_EXT_BUFFER_DEVICE_ADDRESS_EXTENSION_NAME = "VK_EXT_buffer_device_address";
#endregion
#region VK_EXT_tooling_info
public const int VK_EXT_TOOLING_INFO_SPEC_VERSION = 1;
public const string VK_EXT_TOOLING_INFO_EXTENSION_NAME = "VK_EXT_tooling_info";
#endregion
#region VK_EXT_separate_stencil_usage
public const int VK_EXT_SEPARATE_STENCIL_USAGE_SPEC_VERSION = 1;
public const string VK_EXT_SEPARATE_STENCIL_USAGE_EXTENSION_NAME = "VK_EXT_separate_stencil_usage";
#endregion
#region VK_EXT_validation_features
public const int VK_EXT_VALIDATION_FEATURES_SPEC_VERSION = 5;
public const string VK_EXT_VALIDATION_FEATURES_EXTENSION_NAME = "VK_EXT_validation_features";
#endregion
#region VK_NV_cooperative_matrix
public const int VK_NV_COOPERATIVE_MATRIX_SPEC_VERSION = 1;
public const string VK_NV_COOPERATIVE_MATRIX_EXTENSION_NAME = "VK_NV_cooperative_matrix";
#endregion
#region VK_NV_coverage_reduction_mode
public const int VK_NV_COVERAGE_REDUCTION_MODE_SPEC_VERSION = 1;
public const string VK_NV_COVERAGE_REDUCTION_MODE_EXTENSION_NAME = "VK_NV_coverage_reduction_mode";
#endregion
#region VK_EXT_fragment_shader_interlock
public const int VK_EXT_FRAGMENT_SHADER_INTERLOCK_SPEC_VERSION = 1;
public const string VK_EXT_FRAGMENT_SHADER_INTERLOCK_EXTENSION_NAME = "VK_EXT_fragment_shader_interlock";
#endregion
#region VK_EXT_ycbcr_image_arrays
public const int VK_EXT_YCBCR_IMAGE_ARRAYS_SPEC_VERSION = 1;
public const string VK_EXT_YCBCR_IMAGE_ARRAYS_EXTENSION_NAME = "VK_EXT_ycbcr_image_arrays";
#endregion
#region VK_EXT_provoking_vertex
public const int VK_EXT_PROVOKING_VERTEX_SPEC_VERSION = 1;
public const string VK_EXT_PROVOKING_VERTEX_EXTENSION_NAME = "VK_EXT_provoking_vertex";
#endregion
#region VK_EXT_headless_surface
public const int VK_EXT_HEADLESS_SURFACE_SPEC_VERSION = 1;
public const string VK_EXT_HEADLESS_SURFACE_EXTENSION_NAME = "VK_EXT_headless_surface";
#endregion
#region VK_EXT_line_rasterization
public const int VK_EXT_LINE_RASTERIZATION_SPEC_VERSION = 1;
public const string VK_EXT_LINE_RASTERIZATION_EXTENSION_NAME = "VK_EXT_line_rasterization";
#endregion
#region VK_EXT_shader_atomic_float
public const int VK_EXT_SHADER_ATOMIC_FLOAT_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_ATOMIC_FLOAT_EXTENSION_NAME = "VK_EXT_shader_atomic_float";
#endregion
#region VK_EXT_host_query_reset
public const int VK_EXT_HOST_QUERY_RESET_SPEC_VERSION = 1;
public const string VK_EXT_HOST_QUERY_RESET_EXTENSION_NAME = "VK_EXT_host_query_reset";
#endregion
#region VK_EXT_index_type_uint8
public const int VK_EXT_INDEX_TYPE_UINT8_SPEC_VERSION = 1;
public const string VK_EXT_INDEX_TYPE_UINT8_EXTENSION_NAME = "VK_EXT_index_type_uint8";
#endregion
#region VK_EXT_extended_dynamic_state
public const int VK_EXT_EXTENDED_DYNAMIC_STATE_SPEC_VERSION = 1;
public const string VK_EXT_EXTENDED_DYNAMIC_STATE_EXTENSION_NAME = "VK_EXT_extended_dynamic_state";
#endregion
#region VK_EXT_shader_atomic_float2
public const int VK_EXT_SHADER_ATOMIC_FLOAT_2_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_ATOMIC_FLOAT_2_EXTENSION_NAME = "VK_EXT_shader_atomic_float2";
#endregion
#region VK_EXT_shader_demote_to_helper_invocation
public const int VK_EXT_SHADER_DEMOTE_TO_HELPER_INVOCATION_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_DEMOTE_TO_HELPER_INVOCATION_EXTENSION_NAME = "VK_EXT_shader_demote_to_helper_invocation";
#endregion
#region VK_NV_device_generated_commands
public const int VK_NV_DEVICE_GENERATED_COMMANDS_SPEC_VERSION = 3;
public const string VK_NV_DEVICE_GENERATED_COMMANDS_EXTENSION_NAME = "VK_NV_device_generated_commands";
#endregion
#region VK_NV_inherited_viewport_scissor
public const int VK_NV_INHERITED_VIEWPORT_SCISSOR_SPEC_VERSION = 1;
public const string VK_NV_INHERITED_VIEWPORT_SCISSOR_EXTENSION_NAME = "VK_NV_inherited_viewport_scissor";
#endregion
#region VK_EXT_texel_buffer_alignment
public const int VK_EXT_TEXEL_BUFFER_ALIGNMENT_SPEC_VERSION = 1;
public const string VK_EXT_TEXEL_BUFFER_ALIGNMENT_EXTENSION_NAME = "VK_EXT_texel_buffer_alignment";
#endregion
#region VK_QCOM_render_pass_transform
public const int VK_QCOM_RENDER_PASS_TRANSFORM_SPEC_VERSION = 3;
public const string VK_QCOM_RENDER_PASS_TRANSFORM_EXTENSION_NAME = "VK_QCOM_render_pass_transform";
#endregion
#region VK_EXT_device_memory_report
public const int VK_EXT_DEVICE_MEMORY_REPORT_SPEC_VERSION = 2;
public const string VK_EXT_DEVICE_MEMORY_REPORT_EXTENSION_NAME = "VK_EXT_device_memory_report";
#endregion
#region VK_EXT_acquire_drm_display
public const int VK_EXT_ACQUIRE_DRM_DISPLAY_SPEC_VERSION = 1;
public const string VK_EXT_ACQUIRE_DRM_DISPLAY_EXTENSION_NAME = "VK_EXT_acquire_drm_display";
#endregion
#region VK_EXT_robustness2
public const int VK_EXT_ROBUSTNESS_2_SPEC_VERSION = 1;
public const string VK_EXT_ROBUSTNESS_2_EXTENSION_NAME = "VK_EXT_robustness2";
#endregion
#region VK_EXT_custom_border_color
public const int VK_EXT_CUSTOM_BORDER_COLOR_SPEC_VERSION = 12;
public const string VK_EXT_CUSTOM_BORDER_COLOR_EXTENSION_NAME = "VK_EXT_custom_border_color";
#endregion
#region VK_GOOGLE_user_type
public const int VK_GOOGLE_USER_TYPE_SPEC_VERSION = 1;
public const string VK_GOOGLE_USER_TYPE_EXTENSION_NAME = "VK_GOOGLE_user_type";
#endregion
#region VK_NV_present_barrier
public const int VK_NV_PRESENT_BARRIER_SPEC_VERSION = 1;
public const string VK_NV_PRESENT_BARRIER_EXTENSION_NAME = "VK_NV_present_barrier";
#endregion
#region VK_EXT_private_data
public const int VK_EXT_PRIVATE_DATA_SPEC_VERSION = 1;
public const string VK_EXT_PRIVATE_DATA_EXTENSION_NAME = "VK_EXT_private_data";
#endregion
#region VK_EXT_pipeline_creation_cache_control
public const int VK_EXT_PIPELINE_CREATION_CACHE_CONTROL_SPEC_VERSION = 3;
public const string VK_EXT_PIPELINE_CREATION_CACHE_CONTROL_EXTENSION_NAME = "VK_EXT_pipeline_creation_cache_control";
#endregion
#region VK_NV_device_diagnostics_config
public const int VK_NV_DEVICE_DIAGNOSTICS_CONFIG_SPEC_VERSION = 2;
public const string VK_NV_DEVICE_DIAGNOSTICS_CONFIG_EXTENSION_NAME = "VK_NV_device_diagnostics_config";
#endregion
#region VK_QCOM_render_pass_store_ops
public const int VK_QCOM_RENDER_PASS_STORE_OPS_SPEC_VERSION = 2;
public const string VK_QCOM_RENDER_PASS_STORE_OPS_EXTENSION_NAME = "VK_QCOM_render_pass_store_ops";
#endregion
#region VK_EXT_graphics_pipeline_library
public const int VK_EXT_GRAPHICS_PIPELINE_LIBRARY_SPEC_VERSION = 1;
public const string VK_EXT_GRAPHICS_PIPELINE_LIBRARY_EXTENSION_NAME = "VK_EXT_graphics_pipeline_library";
#endregion
#region VK_AMD_shader_early_and_late_fragment_tests
public const int VK_AMD_SHADER_EARLY_AND_LATE_FRAGMENT_TESTS_SPEC_VERSION = 1;
public const string VK_AMD_SHADER_EARLY_AND_LATE_FRAGMENT_TESTS_EXTENSION_NAME = "VK_AMD_shader_early_and_late_fragment_tests";
#endregion
#region VK_NV_fragment_shading_rate_enums
public const int VK_NV_FRAGMENT_SHADING_RATE_ENUMS_SPEC_VERSION = 1;
public const string VK_NV_FRAGMENT_SHADING_RATE_ENUMS_EXTENSION_NAME = "VK_NV_fragment_shading_rate_enums";
#endregion
#region VK_NV_ray_tracing_motion_blur
public const int VK_NV_RAY_TRACING_MOTION_BLUR_SPEC_VERSION = 1;
public const string VK_NV_RAY_TRACING_MOTION_BLUR_EXTENSION_NAME = "VK_NV_ray_tracing_motion_blur";
#endregion
#region VK_EXT_ycbcr_2plane_444_formats
public const int VK_EXT_YCBCR_2PLANE_444_FORMATS_SPEC_VERSION = 1;
public const string VK_EXT_YCBCR_2PLANE_444_FORMATS_EXTENSION_NAME = "VK_EXT_ycbcr_2plane_444_formats";
#endregion
#region VK_EXT_fragment_density_map2
public const int VK_EXT_FRAGMENT_DENSITY_MAP_2_SPEC_VERSION = 1;
public const string VK_EXT_FRAGMENT_DENSITY_MAP_2_EXTENSION_NAME = "VK_EXT_fragment_density_map2";
#endregion
#region VK_QCOM_rotated_copy_commands
public const int VK_QCOM_ROTATED_COPY_COMMANDS_SPEC_VERSION = 1;
public const string VK_QCOM_ROTATED_COPY_COMMANDS_EXTENSION_NAME = "VK_QCOM_rotated_copy_commands";
#endregion
#region VK_EXT_image_robustness
public const int VK_EXT_IMAGE_ROBUSTNESS_SPEC_VERSION = 1;
public const string VK_EXT_IMAGE_ROBUSTNESS_EXTENSION_NAME = "VK_EXT_image_robustness";
#endregion
#region VK_EXT_image_compression_control
public const int VK_EXT_IMAGE_COMPRESSION_CONTROL_SPEC_VERSION = 1;
public const string VK_EXT_IMAGE_COMPRESSION_CONTROL_EXTENSION_NAME = "VK_EXT_image_compression_control";
#endregion
#region VK_EXT_attachment_feedback_loop_layout
public const int VK_EXT_ATTACHMENT_FEEDBACK_LOOP_LAYOUT_SPEC_VERSION = 2;
public const string VK_EXT_ATTACHMENT_FEEDBACK_LOOP_LAYOUT_EXTENSION_NAME = "VK_EXT_attachment_feedback_loop_layout";
#endregion
#region VK_EXT_4444_formats
public const int VK_EXT_4444_FORMATS_SPEC_VERSION = 1;
public const string VK_EXT_4444_FORMATS_EXTENSION_NAME = "VK_EXT_4444_formats";
#endregion
#region VK_EXT_device_fault
public const int VK_EXT_DEVICE_FAULT_SPEC_VERSION = 1;
public const string VK_EXT_DEVICE_FAULT_EXTENSION_NAME = "VK_EXT_device_fault";
#endregion
#region VK_ARM_rasterization_order_attachment_access
public const int VK_ARM_RASTERIZATION_ORDER_ATTACHMENT_ACCESS_SPEC_VERSION = 1;
public const string VK_ARM_RASTERIZATION_ORDER_ATTACHMENT_ACCESS_EXTENSION_NAME = "VK_ARM_rasterization_order_attachment_access";
#endregion
#region VK_EXT_rgba10x6_formats
public const int VK_EXT_RGBA10X6_FORMATS_SPEC_VERSION = 1;
public const string VK_EXT_RGBA10X6_FORMATS_EXTENSION_NAME = "VK_EXT_rgba10x6_formats";
#endregion
#region VK_NV_acquire_winrt_display
public const int VK_NV_ACQUIRE_WINRT_DISPLAY_SPEC_VERSION = 1;
public const string VK_NV_ACQUIRE_WINRT_DISPLAY_EXTENSION_NAME = "VK_NV_acquire_winrt_display";
#endregion
#region VK_VALVE_mutable_descriptor_type
public const int VK_VALVE_MUTABLE_DESCRIPTOR_TYPE_SPEC_VERSION = 1;
public const string VK_VALVE_MUTABLE_DESCRIPTOR_TYPE_EXTENSION_NAME = "VK_VALVE_mutable_descriptor_type";
#endregion
#region VK_EXT_vertex_input_dynamic_state
public const int VK_EXT_VERTEX_INPUT_DYNAMIC_STATE_SPEC_VERSION = 2;
public const string VK_EXT_VERTEX_INPUT_DYNAMIC_STATE_EXTENSION_NAME = "VK_EXT_vertex_input_dynamic_state";
#endregion
#region VK_EXT_physical_device_drm
public const int VK_EXT_PHYSICAL_DEVICE_DRM_SPEC_VERSION = 1;
public const string VK_EXT_PHYSICAL_DEVICE_DRM_EXTENSION_NAME = "VK_EXT_physical_device_drm";
#endregion
#region VK_EXT_device_address_binding_report
public const int VK_EXT_DEVICE_ADDRESS_BINDING_REPORT_SPEC_VERSION = 1;
public const string VK_EXT_DEVICE_ADDRESS_BINDING_REPORT_EXTENSION_NAME = "VK_EXT_device_address_binding_report";
#endregion
#region VK_EXT_depth_clip_control
public const int VK_EXT_DEPTH_CLIP_CONTROL_SPEC_VERSION = 1;
public const string VK_EXT_DEPTH_CLIP_CONTROL_EXTENSION_NAME = "VK_EXT_depth_clip_control";
#endregion
#region VK_EXT_primitive_topology_list_restart
public const int VK_EXT_PRIMITIVE_TOPOLOGY_LIST_RESTART_SPEC_VERSION = 1;
public const string VK_EXT_PRIMITIVE_TOPOLOGY_LIST_RESTART_EXTENSION_NAME = "VK_EXT_primitive_topology_list_restart";
#endregion
#region VK_HUAWEI_subpass_shading
public const int VK_HUAWEI_SUBPASS_SHADING_SPEC_VERSION = 2;
public const string VK_HUAWEI_SUBPASS_SHADING_EXTENSION_NAME = "VK_HUAWEI_subpass_shading";
#endregion
#region VK_HUAWEI_invocation_mask
public const int VK_HUAWEI_INVOCATION_MASK_SPEC_VERSION = 1;
public const string VK_HUAWEI_INVOCATION_MASK_EXTENSION_NAME = "VK_HUAWEI_invocation_mask";
#endregion
#region VK_NV_external_memory_rdma
public const int VK_NV_EXTERNAL_MEMORY_RDMA_SPEC_VERSION = 1;
public const string VK_NV_EXTERNAL_MEMORY_RDMA_EXTENSION_NAME = "VK_NV_external_memory_rdma";
#endregion
#region VK_EXT_pipeline_properties
public const int VK_EXT_PIPELINE_PROPERTIES_SPEC_VERSION = 1;
public const string VK_EXT_PIPELINE_PROPERTIES_EXTENSION_NAME = "VK_EXT_pipeline_properties";
#endregion
#region VK_EXT_multisampled_render_to_single_sampled
public const int VK_EXT_MULTISAMPLED_RENDER_TO_SINGLE_SAMPLED_SPEC_VERSION = 1;
public const string VK_EXT_MULTISAMPLED_RENDER_TO_SINGLE_SAMPLED_EXTENSION_NAME = "VK_EXT_multisampled_render_to_single_sampled";
#endregion
#region VK_EXT_extended_dynamic_state2
public const int VK_EXT_EXTENDED_DYNAMIC_STATE_2_SPEC_VERSION = 1;
public const string VK_EXT_EXTENDED_DYNAMIC_STATE_2_EXTENSION_NAME = "VK_EXT_extended_dynamic_state2";
#endregion
#region VK_EXT_color_write_enable
public const int VK_EXT_COLOR_WRITE_ENABLE_SPEC_VERSION = 1;
public const string VK_EXT_COLOR_WRITE_ENABLE_EXTENSION_NAME = "VK_EXT_color_write_enable";
#endregion
#region VK_EXT_primitives_generated_query
public const int VK_EXT_PRIMITIVES_GENERATED_QUERY_SPEC_VERSION = 1;
public const string VK_EXT_PRIMITIVES_GENERATED_QUERY_EXTENSION_NAME = "VK_EXT_primitives_generated_query";
#endregion
#region VK_EXT_global_priority_query
public const int VK_EXT_GLOBAL_PRIORITY_QUERY_SPEC_VERSION = 1;
public const string VK_EXT_GLOBAL_PRIORITY_QUERY_EXTENSION_NAME = "VK_EXT_global_priority_query";
public const uint VK_MAX_GLOBAL_PRIORITY_SIZE_EXT = VK_MAX_GLOBAL_PRIORITY_SIZE_KHR;
#endregion
#region VK_EXT_image_view_min_lod
public const int VK_EXT_IMAGE_VIEW_MIN_LOD_SPEC_VERSION = 1;
public const string VK_EXT_IMAGE_VIEW_MIN_LOD_EXTENSION_NAME = "VK_EXT_image_view_min_lod";
#endregion
#region VK_EXT_multi_draw
public const int VK_EXT_MULTI_DRAW_SPEC_VERSION = 1;
public const string VK_EXT_MULTI_DRAW_EXTENSION_NAME = "VK_EXT_multi_draw";
#endregion
#region VK_EXT_image_2d_view_of_3d
public const int VK_EXT_IMAGE_2D_VIEW_OF_3D_SPEC_VERSION = 1;
public const string VK_EXT_IMAGE_2D_VIEW_OF_3D_EXTENSION_NAME = "VK_EXT_image_2d_view_of_3d";
#endregion
#region VK_EXT_opacity_micromap
public const int VK_EXT_OPACITY_MICROMAP_SPEC_VERSION = 2;
public const string VK_EXT_OPACITY_MICROMAP_EXTENSION_NAME = "VK_EXT_opacity_micromap";
#endregion
#region VK_EXT_load_store_op_none
public const int VK_EXT_LOAD_STORE_OP_NONE_SPEC_VERSION = 1;
public const string VK_EXT_LOAD_STORE_OP_NONE_EXTENSION_NAME = "VK_EXT_load_store_op_none";
#endregion
#region VK_EXT_border_color_swizzle
public const int VK_EXT_BORDER_COLOR_SWIZZLE_SPEC_VERSION = 1;
public const string VK_EXT_BORDER_COLOR_SWIZZLE_EXTENSION_NAME = "VK_EXT_border_color_swizzle";
#endregion
#region VK_EXT_pageable_device_local_memory
public const int VK_EXT_PAGEABLE_DEVICE_LOCAL_MEMORY_SPEC_VERSION = 1;
public const string VK_EXT_PAGEABLE_DEVICE_LOCAL_MEMORY_EXTENSION_NAME = "VK_EXT_pageable_device_local_memory";
#endregion
#region VK_VALVE_descriptor_set_host_mapping
public const int VK_VALVE_DESCRIPTOR_SET_HOST_MAPPING_SPEC_VERSION = 1;
public const string VK_VALVE_DESCRIPTOR_SET_HOST_MAPPING_EXTENSION_NAME = "VK_VALVE_descriptor_set_host_mapping";
#endregion
#region VK_EXT_depth_clamp_zero_one
public const int VK_EXT_DEPTH_CLAMP_ZERO_ONE_SPEC_VERSION = 1;
public const string VK_EXT_DEPTH_CLAMP_ZERO_ONE_EXTENSION_NAME = "VK_EXT_depth_clamp_zero_one";
#endregion
#region VK_EXT_non_seamless_cube_map
public const int VK_EXT_NON_SEAMLESS_CUBE_MAP_SPEC_VERSION = 1;
public const string VK_EXT_NON_SEAMLESS_CUBE_MAP_EXTENSION_NAME = "VK_EXT_non_seamless_cube_map";
#endregion
#region VK_QCOM_fragment_density_map_offset
public const int VK_QCOM_FRAGMENT_DENSITY_MAP_OFFSET_SPEC_VERSION = 1;
public const string VK_QCOM_FRAGMENT_DENSITY_MAP_OFFSET_EXTENSION_NAME = "VK_QCOM_fragment_density_map_offset";
#endregion
#region VK_NV_copy_memory_indirect
public const int VK_NV_COPY_MEMORY_INDIRECT_SPEC_VERSION = 1;
public const string VK_NV_COPY_MEMORY_INDIRECT_EXTENSION_NAME = "VK_NV_copy_memory_indirect";
#endregion
#region VK_NV_memory_decompression
public const int VK_NV_MEMORY_DECOMPRESSION_SPEC_VERSION = 1;
public const string VK_NV_MEMORY_DECOMPRESSION_EXTENSION_NAME = "VK_NV_memory_decompression";
#endregion
#region VK_NV_linear_color_attachment
public const int VK_NV_LINEAR_COLOR_ATTACHMENT_SPEC_VERSION = 1;
public const string VK_NV_LINEAR_COLOR_ATTACHMENT_EXTENSION_NAME = "VK_NV_linear_color_attachment";
#endregion
#region VK_GOOGLE_surfaceless_query
public const int VK_GOOGLE_SURFACELESS_QUERY_SPEC_VERSION = 2;
public const string VK_GOOGLE_SURFACELESS_QUERY_EXTENSION_NAME = "VK_GOOGLE_surfaceless_query";
#endregion
#region VK_EXT_image_compression_control_swapchain
public const int VK_EXT_IMAGE_COMPRESSION_CONTROL_SWAPCHAIN_SPEC_VERSION = 1;
public const string VK_EXT_IMAGE_COMPRESSION_CONTROL_SWAPCHAIN_EXTENSION_NAME = "VK_EXT_image_compression_control_swapchain";
#endregion
#region VK_QCOM_image_processing
public const int VK_QCOM_IMAGE_PROCESSING_SPEC_VERSION = 1;
public const string VK_QCOM_IMAGE_PROCESSING_EXTENSION_NAME = "VK_QCOM_image_processing";
#endregion
#region VK_EXT_extended_dynamic_state3
public const int VK_EXT_EXTENDED_DYNAMIC_STATE_3_SPEC_VERSION = 2;
public const string VK_EXT_EXTENDED_DYNAMIC_STATE_3_EXTENSION_NAME = "VK_EXT_extended_dynamic_state3";
#endregion
#region VK_EXT_subpass_merge_feedback
public const int VK_EXT_SUBPASS_MERGE_FEEDBACK_SPEC_VERSION = 2;
public const string VK_EXT_SUBPASS_MERGE_FEEDBACK_EXTENSION_NAME = "VK_EXT_subpass_merge_feedback";
#endregion
#region VK_EXT_shader_module_identifier
public const uint VK_MAX_SHADER_MODULE_IDENTIFIER_SIZE_EXT = 32U;
public const int VK_EXT_SHADER_MODULE_IDENTIFIER_SPEC_VERSION = 1;
public const string VK_EXT_SHADER_MODULE_IDENTIFIER_EXTENSION_NAME = "VK_EXT_shader_module_identifier";
#endregion
#region VK_EXT_rasterization_order_attachment_access
public const int VK_EXT_RASTERIZATION_ORDER_ATTACHMENT_ACCESS_SPEC_VERSION = 1;
public const string VK_EXT_RASTERIZATION_ORDER_ATTACHMENT_ACCESS_EXTENSION_NAME = "VK_EXT_rasterization_order_attachment_access";
#endregion
#region VK_NV_optical_flow
public const int VK_NV_OPTICAL_FLOW_SPEC_VERSION = 1;
public const string VK_NV_OPTICAL_FLOW_EXTENSION_NAME = "VK_NV_optical_flow";
#endregion
#region VK_EXT_legacy_dithering
public const int VK_EXT_LEGACY_DITHERING_SPEC_VERSION = 1;
public const string VK_EXT_LEGACY_DITHERING_EXTENSION_NAME = "VK_EXT_legacy_dithering";
#endregion
#region VK_EXT_pipeline_protected_access
public const int VK_EXT_PIPELINE_PROTECTED_ACCESS_SPEC_VERSION = 1;
public const string VK_EXT_PIPELINE_PROTECTED_ACCESS_EXTENSION_NAME = "VK_EXT_pipeline_protected_access";
#endregion
#region VK_QCOM_tile_properties
public const int VK_QCOM_TILE_PROPERTIES_SPEC_VERSION = 1;
public const string VK_QCOM_TILE_PROPERTIES_EXTENSION_NAME = "VK_QCOM_tile_properties";
#endregion
#region VK_SEC_amigo_profiling
public const int VK_SEC_AMIGO_PROFILING_SPEC_VERSION = 1;
public const string VK_SEC_AMIGO_PROFILING_EXTENSION_NAME = "VK_SEC_amigo_profiling";
#endregion
#region VK_NV_ray_tracing_invocation_reorder
public const int VK_NV_RAY_TRACING_INVOCATION_REORDER_SPEC_VERSION = 1;
public const string VK_NV_RAY_TRACING_INVOCATION_REORDER_EXTENSION_NAME = "VK_NV_ray_tracing_invocation_reorder";
#endregion
#region VK_EXT_mutable_descriptor_type
public const int VK_EXT_MUTABLE_DESCRIPTOR_TYPE_SPEC_VERSION = 1;
public const string VK_EXT_MUTABLE_DESCRIPTOR_TYPE_EXTENSION_NAME = "VK_EXT_mutable_descriptor_type";
#endregion
#region VK_ARM_shader_core_builtins
public const int VK_ARM_SHADER_CORE_BUILTINS_SPEC_VERSION = 2;
public const string VK_ARM_SHADER_CORE_BUILTINS_EXTENSION_NAME = "VK_ARM_shader_core_builtins";
#endregion
#region VK_KHR_acceleration_structure
public const int VK_KHR_ACCELERATION_STRUCTURE_SPEC_VERSION = 13;
public const string VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME = "VK_KHR_acceleration_structure";
#endregion
#region VK_KHR_ray_tracing_pipeline
public const int VK_KHR_RAY_TRACING_PIPELINE_SPEC_VERSION = 1;
public const string VK_KHR_RAY_TRACING_PIPELINE_EXTENSION_NAME = "VK_KHR_ray_tracing_pipeline";
#endregion
#region VK_KHR_ray_query
public const int VK_KHR_RAY_QUERY_SPEC_VERSION = 1;
public const string VK_KHR_RAY_QUERY_EXTENSION_NAME = "VK_KHR_ray_query";
#endregion
#region VK_EXT_mesh_shader
public const int VK_EXT_MESH_SHADER_SPEC_VERSION = 1;
public const string VK_EXT_MESH_SHADER_EXTENSION_NAME = "VK_EXT_mesh_shader";
#endregion
#endregion

#if WIN
#region Window
    public static bool VK_KHR_win32_surface =false;
	public const int VK_KHR_WIN32_SURFACE_SPEC_VERSION =6;
	public const string VK_KHR_WIN32_SURFACE_EXTENSION_NAME ="VK_KHR_win32_surface"; 
#endregion
#endif
}
