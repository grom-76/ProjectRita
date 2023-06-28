// namespace RitaEngine.API.Web;

// using System;
// using System.Runtime.InteropServices;


// public static unsafe partial class WebGPUDawnNative
// 	{
// 		private const string dllName = "dawn_proc.dll";

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuCreateInstance(WGPUInstanceDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern WGPUProc wgpuGetProcAddress(IntPtr device, char* procName);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBindGroupReference(IntPtr bindGroup);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBindGroupRelease(IntPtr bindGroup);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBindGroupLayoutReference(IntPtr bindGroupLayout);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBindGroupLayoutRelease(IntPtr bindGroupLayout);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBufferDestroy(IntPtr buffer);

// 		[DllImport(dllName)]
// 		public static extern void* wgpuBufferGetConstMappedRange(IntPtr buffer, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void* wgpuBufferGetMappedRange(IntPtr buffer, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBufferMapAsync(IntPtr buffer, WGPUMapMode mode, ulong offset, ulong size, WGPUBufferMapCallback callback, void* userdata);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBufferUnmap(IntPtr buffer);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBufferReference(IntPtr buffer);

// 		[DllImport(dllName)]
// 		public static extern void wgpuBufferRelease(IntPtr buffer);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandBufferReference(IntPtr commandBuffer);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandBufferRelease(IntPtr commandBuffer);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuCommandEncoderBeginComputePass(IntPtr commandEncoder, WGPUComputePassDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuCommandEncoderBeginRenderPass(IntPtr commandEncoder, WGPURenderPassDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderCopyBufferToBuffer(IntPtr commandEncoder, IntPtr source, ulong sourceOffset, IntPtr destination, ulong destinationOffset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderCopyBufferToTexture(IntPtr commandEncoder, WGPUBufferCopyView* source, WGPUTextureCopyView* destination, WGPUExtent3D* copySize);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderCopyTextureToBuffer(IntPtr commandEncoder, WGPUTextureCopyView* source, WGPUBufferCopyView* destination, WGPUExtent3D* copySize);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderCopyTextureToTexture(IntPtr commandEncoder, WGPUTextureCopyView* source, WGPUTextureCopyView* destination, WGPUExtent3D* copySize);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuCommandEncoderFinish(IntPtr commandEncoder, WGPUCommandBufferDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderInsertDebugMarker(IntPtr commandEncoder, char* markerLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderPopDebugGroup(IntPtr commandEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderPushDebugGroup(IntPtr commandEncoder, char* groupLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderResolveQuerySet(IntPtr commandEncoder, IntPtr querySet, uint firstQuery, uint queryCount, IntPtr destination, ulong destinationOffset);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderWriteTimestamp(IntPtr commandEncoder, IntPtr querySet, uint queryIndex);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderReference(IntPtr commandEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuCommandEncoderRelease(IntPtr commandEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderDispatch(IntPtr computePassEncoder, uint x, uint y, uint z);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderDispatchIndirect(IntPtr computePassEncoder, IntPtr indirectBuffer, ulong indirectOffset);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderEndPass(IntPtr computePassEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderInsertDebugMarker(IntPtr computePassEncoder, char* markerLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderPopDebugGroup(IntPtr computePassEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderPushDebugGroup(IntPtr computePassEncoder, char* groupLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderSetBindGroup(IntPtr computePassEncoder, uint groupIndex, IntPtr group, uint dynamicOffsetCount, uint* dynamicOffsets);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderSetPipeline(IntPtr computePassEncoder, IntPtr pipeline);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderWriteTimestamp(IntPtr computePassEncoder, IntPtr querySet, uint queryIndex);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderReference(IntPtr computePassEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePassEncoderRelease(IntPtr computePassEncoder);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuComputePipelineGetBindGroupLayout(IntPtr computePipeline, uint groupIndex);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePipelineReference(IntPtr computePipeline);

// 		[DllImport(dllName)]
// 		public static extern void wgpuComputePipelineRelease(IntPtr computePipeline);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateBindGroup(IntPtr device, WGPUBindGroupDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateBindGroupLayout(IntPtr device, WGPUBindGroupLayoutDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateBuffer(IntPtr device, ref WGPUBufferDescriptor descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateCommandEncoder(IntPtr device, WGPUCommandEncoderDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateComputePipeline(IntPtr device, WGPUComputePipelineDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateErrorBuffer(IntPtr device);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreatePipelineLayout(IntPtr device, WGPUPipelineLayoutDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateQuerySet(IntPtr device, WGPUQuerySetDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateRenderBundleEncoder(IntPtr device, WGPURenderBundleEncoderDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateRenderPipeline(IntPtr device, ref WGPURenderPipelineDescriptor descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateSampler(IntPtr device, WGPUSamplerDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateShaderModule(IntPtr device, WGPUShaderModuleDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateSwapChain(IntPtr device, IntPtr surface, WGPUSwapChainDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceCreateTexture(IntPtr device, WGPUTextureDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuDeviceGetDefaultQueue(IntPtr device);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceInjectError(IntPtr device, WGPUErrorType type, char* message);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceLoseForTesting(IntPtr device);

// 		[DllImport(dllName)]
// 		public static extern bool wgpuDevicePopErrorScope(IntPtr device, WGPUErrorCallback callback, void* userdata);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDevicePushErrorScope(IntPtr device, WGPUErrorFilter filter);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceSetDeviceLostCallback(IntPtr device, WGPUDeviceLostCallback callback, void* userdata);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceSetUncapturedErrorCallback(IntPtr device, WGPUErrorCallback callback, void* userdata);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceTick(IntPtr device);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceReference(IntPtr device);

// 		[DllImport(dllName)]
// 		public static extern void wgpuDeviceRelease(IntPtr device);

// 		[DllImport(dllName)]
// 		public static extern ulong wgpuFenceGetCompletedValue(IntPtr fence);

// 		[DllImport(dllName)]
// 		public static extern void wgpuFenceOnCompletion(IntPtr fence, ulong value, WGPUFenceOnCompletionCallback callback, void* userdata);

// 		[DllImport(dllName)]
// 		public static extern void wgpuFenceReference(IntPtr fence);

// 		[DllImport(dllName)]
// 		public static extern void wgpuFenceRelease(IntPtr fence);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuInstanceCreateSurface(IntPtr instance, WGPUSurfaceDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern void wgpuInstanceReference(IntPtr instance);

// 		[DllImport(dllName)]
// 		public static extern void wgpuInstanceRelease(IntPtr instance);

// 		[DllImport(dllName)]
// 		public static extern void wgpuPipelineLayoutReference(IntPtr pipelineLayout);

// 		[DllImport(dllName)]
// 		public static extern void wgpuPipelineLayoutRelease(IntPtr pipelineLayout);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQuerySetDestroy(IntPtr querySet);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQuerySetReference(IntPtr querySet);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQuerySetRelease(IntPtr querySet);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuQueueCreateFence(IntPtr queue, WGPUFenceDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQueueSignal(IntPtr queue, IntPtr fence, ulong signalValue);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQueueSubmit(IntPtr queue, uint commandCount, IntPtr* commands);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQueueWriteBuffer(IntPtr queue, IntPtr buffer, ulong bufferOffset, void* data, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQueueWriteTexture(IntPtr queue, WGPUTextureCopyView* destination, void* data, ulong dataSize, WGPUTextureDataLayout* dataLayout, WGPUExtent3D* writeSize);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQueueReference(IntPtr queue);

// 		[DllImport(dllName)]
// 		public static extern void wgpuQueueRelease(IntPtr queue);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleReference(IntPtr renderBundle);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleRelease(IntPtr renderBundle);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderDraw(IntPtr renderBundleEncoder, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderDrawIndexed(IntPtr renderBundleEncoder, uint indexCount, uint instanceCount, uint firstIndex, int baseVertex, uint firstInstance);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderDrawIndexedIndirect(IntPtr renderBundleEncoder, IntPtr indirectBuffer, ulong indirectOffset);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderDrawIndirect(IntPtr renderBundleEncoder, IntPtr indirectBuffer, ulong indirectOffset);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuRenderBundleEncoderFinish(IntPtr renderBundleEncoder, WGPURenderBundleDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderInsertDebugMarker(IntPtr renderBundleEncoder, char* markerLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderPopDebugGroup(IntPtr renderBundleEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderPushDebugGroup(IntPtr renderBundleEncoder, char* groupLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderSetBindGroup(IntPtr renderBundleEncoder, uint groupIndex, IntPtr group, uint dynamicOffsetCount, uint* dynamicOffsets);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderSetIndexBuffer(IntPtr renderBundleEncoder, IntPtr buffer, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderSetIndexBufferWithFormat(IntPtr renderBundleEncoder, IntPtr buffer, WGPUIndexFormat format, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderSetPipeline(IntPtr renderBundleEncoder, IntPtr pipeline);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderSetVertexBuffer(IntPtr renderBundleEncoder, uint slot, IntPtr buffer, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderReference(IntPtr renderBundleEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderBundleEncoderRelease(IntPtr renderBundleEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderDraw(IntPtr renderPassEncoder, uint vertexCount, uint instanceCount, uint firstVertex, uint firstInstance);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderDrawIndexed(IntPtr renderPassEncoder, uint indexCount, uint instanceCount, uint firstIndex, int baseVertex, uint firstInstance);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderDrawIndexedIndirect(IntPtr renderPassEncoder, IntPtr indirectBuffer, ulong indirectOffset);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderDrawIndirect(IntPtr renderPassEncoder, IntPtr indirectBuffer, ulong indirectOffset);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderEndPass(IntPtr renderPassEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderExecuteBundles(IntPtr renderPassEncoder, uint bundlesCount, IntPtr* bundles);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderInsertDebugMarker(IntPtr renderPassEncoder, char* markerLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderPopDebugGroup(IntPtr renderPassEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderPushDebugGroup(IntPtr renderPassEncoder, char* groupLabel);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetBindGroup(IntPtr renderPassEncoder, uint groupIndex, IntPtr group, uint dynamicOffsetCount, uint* dynamicOffsets);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetBlendColor(IntPtr renderPassEncoder, WGPUColor* color);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetIndexBuffer(IntPtr renderPassEncoder, IntPtr buffer, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetIndexBufferWithFormat(IntPtr renderPassEncoder, IntPtr buffer, WGPUIndexFormat format, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetPipeline(IntPtr renderPassEncoder, IntPtr pipeline);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetScissorRect(IntPtr renderPassEncoder, uint x, uint y, uint width, uint height);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetStencilReference(IntPtr renderPassEncoder, uint reference);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetVertexBuffer(IntPtr renderPassEncoder, uint slot, IntPtr buffer, ulong offset, ulong size);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderSetViewport(IntPtr renderPassEncoder, float x, float y, float width, float height, float minDepth, float maxDepth);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderWriteTimestamp(IntPtr renderPassEncoder, IntPtr querySet, uint queryIndex);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderReference(IntPtr renderPassEncoder);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPassEncoderRelease(IntPtr renderPassEncoder);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuRenderPipelineGetBindGroupLayout(IntPtr renderPipeline, uint groupIndex);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPipelineReference(IntPtr renderPipeline);

// 		[DllImport(dllName)]
// 		public static extern void wgpuRenderPipelineRelease(IntPtr renderPipeline);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSamplerReference(IntPtr sampler);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSamplerRelease(IntPtr sampler);

// 		[DllImport(dllName)]
// 		public static extern void wgpuShaderModuleReference(IntPtr shaderModule);

// 		[DllImport(dllName)]
// 		public static extern void wgpuShaderModuleRelease(IntPtr shaderModule);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSurfaceReference(IntPtr surface);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSurfaceRelease(IntPtr surface);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSwapChainConfigure(IntPtr swapChain, WGPUTextureFormat format, WGPUTextureUsage allowedUsage, uint width, uint height);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuSwapChainGetCurrentTextureView(IntPtr swapChain);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSwapChainPresent(IntPtr swapChain);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSwapChainReference(IntPtr swapChain);

// 		[DllImport(dllName)]
// 		public static extern void wgpuSwapChainRelease(IntPtr swapChain);

// 		[DllImport(dllName)]
// 		public static extern IntPtr wgpuTextureCreateView(IntPtr texture, WGPUTextureViewDescriptor* descriptor);

// 		[DllImport(dllName)]
// 		public static extern void wgpuTextureDestroy(IntPtr texture);

// 		[DllImport(dllName)]
// 		public static extern void wgpuTextureReference(IntPtr texture);

// 		[DllImport(dllName)]
// 		public static extern void wgpuTextureRelease(IntPtr texture);

// 		[DllImport(dllName)]
// 		public static extern void wgpuTextureViewReference(IntPtr textureView);

// 		[DllImport(dllName)]
// 		public static extern void wgpuTextureViewRelease(IntPtr textureView);
// 	}


//     public unsafe delegate void WGPUBufferMapCallback(
// 		 WGPUBufferMapAsyncStatus status,
// 		 void* userdata);

// 	public unsafe delegate void WGPUDeviceLostCallback(
// 		 char* message,
// 		 void* userdata);

// 	public unsafe delegate void WGPUErrorCallback(
// 		 WGPUErrorType type,
// 		 char* message,
// 		 void* userdata);

// 	public unsafe delegate void WGPUFenceOnCompletionCallback(
// 		 WGPUFenceCompletionStatus status,
// 		 void* userdata);

// 	public unsafe delegate void WGPUProc();

//     public enum WGPUAdapterType
// 	{
// 		WGPUAdapterType_DiscreteGPU = 0,
// 		WGPUAdapterType_IntegratedGPU = 1,
// 		WGPUAdapterType_CPU = 2,
// 		WGPUAdapterType_Unknown = 3,
// 		WGPUAdapterType_Force32 = 2147483647,
// 	}

// 	public enum WGPUAddressMode
// 	{
// 		WGPUAddressMode_Repeat = 0,
// 		WGPUAddressMode_MirrorRepeat = 1,
// 		WGPUAddressMode_ClampToEdge = 2,
// 		WGPUAddressMode_Force32 = 2147483647,
// 	}

// 	public enum WGPUBackendType
// 	{
// 		WGPUBackendType_Null = 0,
// 		WGPUBackendType_D3D11 = 1,
// 		WGPUBackendType_D3D12 = 2,
// 		WGPUBackendType_Metal = 3,
// 		WGPUBackendType_Vulkan = 4,
// 		WGPUBackendType_OpenGL = 5,
// 		WGPUBackendType_OpenGLES = 6,
// 		WGPUBackendType_Force32 = 2147483647,
// 	}

// 	public enum WGPUBindingType
// 	{
// 		WGPUBindingType_UniformBuffer = 0,
// 		WGPUBindingType_StorageBuffer = 1,
// 		WGPUBindingType_ReadonlyStorageBuffer = 2,
// 		WGPUBindingType_Sampler = 3,
// 		WGPUBindingType_ComparisonSampler = 4,
// 		WGPUBindingType_SampledTexture = 5,
// 		WGPUBindingType_MultisampledTexture = 6,
// 		WGPUBindingType_ReadonlyStorageTexture = 7,
// 		WGPUBindingType_WriteonlyStorageTexture = 8,
// 		WGPUBindingType_Force32 = 2147483647,
// 	}

// 	public enum WGPUBlendFactor
// 	{
// 		WGPUBlendFactor_Zero = 0,
// 		WGPUBlendFactor_One = 1,
// 		WGPUBlendFactor_SrcColor = 2,
// 		WGPUBlendFactor_OneMinusSrcColor = 3,
// 		WGPUBlendFactor_SrcAlpha = 4,
// 		WGPUBlendFactor_OneMinusSrcAlpha = 5,
// 		WGPUBlendFactor_DstColor = 6,
// 		WGPUBlendFactor_OneMinusDstColor = 7,
// 		WGPUBlendFactor_DstAlpha = 8,
// 		WGPUBlendFactor_OneMinusDstAlpha = 9,
// 		WGPUBlendFactor_SrcAlphaSaturated = 10,
// 		WGPUBlendFactor_BlendColor = 11,
// 		WGPUBlendFactor_OneMinusBlendColor = 12,
// 		WGPUBlendFactor_Force32 = 2147483647,
// 	}

// 	public enum WGPUBlendOperation
// 	{
// 		WGPUBlendOperation_Add = 0,
// 		WGPUBlendOperation_Subtract = 1,
// 		WGPUBlendOperation_ReverseSubtract = 2,
// 		WGPUBlendOperation_Min = 3,
// 		WGPUBlendOperation_Max = 4,
// 		WGPUBlendOperation_Force32 = 2147483647,
// 	}

// 	public enum WGPUBufferMapAsyncStatus
// 	{
// 		WGPUBufferMapAsyncStatus_Success = 0,
// 		WGPUBufferMapAsyncStatus_Error = 1,
// 		WGPUBufferMapAsyncStatus_Unknown = 2,
// 		WGPUBufferMapAsyncStatus_DeviceLost = 3,
// 		WGPUBufferMapAsyncStatus_DestroyedBeforeCallback = 4,
// 		WGPUBufferMapAsyncStatus_UnmappedBeforeCallback = 5,
// 		WGPUBufferMapAsyncStatus_Force32 = 2147483647,
// 	}

// 	public enum WGPUCompareFunction
// 	{
// 		WGPUCompareFunction_Undefined = 0,
// 		WGPUCompareFunction_Never = 1,
// 		WGPUCompareFunction_Less = 2,
// 		WGPUCompareFunction_LessEqual = 3,
// 		WGPUCompareFunction_Greater = 4,
// 		WGPUCompareFunction_GreaterEqual = 5,
// 		WGPUCompareFunction_Equal = 6,
// 		WGPUCompareFunction_NotEqual = 7,
// 		WGPUCompareFunction_Always = 8,
// 		WGPUCompareFunction_Force32 = 2147483647,
// 	}

// 	public enum WGPUCullMode
// 	{
// 		WGPUCullMode_None = 0,
// 		WGPUCullMode_Front = 1,
// 		WGPUCullMode_Back = 2,
// 		WGPUCullMode_Force32 = 2147483647,
// 	}

// 	public enum WGPUErrorFilter
// 	{
// 		WGPUErrorFilter_None = 0,
// 		WGPUErrorFilter_Validation = 1,
// 		WGPUErrorFilter_OutOfMemory = 2,
// 		WGPUErrorFilter_Force32 = 2147483647,
// 	}

// 	public enum WGPUErrorType
// 	{
// 		WGPUErrorType_NoError = 0,
// 		WGPUErrorType_Validation = 1,
// 		WGPUErrorType_OutOfMemory = 2,
// 		WGPUErrorType_Unknown = 3,
// 		WGPUErrorType_DeviceLost = 4,
// 		WGPUErrorType_Force32 = 2147483647,
// 	}

// 	public enum WGPUFenceCompletionStatus
// 	{
// 		WGPUFenceCompletionStatus_Success = 0,
// 		WGPUFenceCompletionStatus_Error = 1,
// 		WGPUFenceCompletionStatus_Unknown = 2,
// 		WGPUFenceCompletionStatus_DeviceLost = 3,
// 		WGPUFenceCompletionStatus_Force32 = 2147483647,
// 	}

// 	public enum WGPUFilterMode
// 	{
// 		WGPUFilterMode_Nearest = 0,
// 		WGPUFilterMode_Linear = 1,
// 		WGPUFilterMode_Force32 = 2147483647,
// 	}

// 	public enum WGPUFrontFace
// 	{
// 		WGPUFrontFace_CCW = 0,
// 		WGPUFrontFace_CW = 1,
// 		WGPUFrontFace_Force32 = 2147483647,
// 	}

// 	public enum WGPUIndexFormat
// 	{
// 		WGPUIndexFormat_Undefined = 0,
// 		WGPUIndexFormat_Uint16 = 1,
// 		WGPUIndexFormat_Uint32 = 2,
// 		WGPUIndexFormat_Force32 = 2147483647,
// 	}

// 	public enum WGPUInputStepMode
// 	{
// 		WGPUInputStepMode_Vertex = 0,
// 		WGPUInputStepMode_Instance = 1,
// 		WGPUInputStepMode_Force32 = 2147483647,
// 	}

// 	public enum WGPULoadOp
// 	{
// 		WGPULoadOp_Clear = 0,
// 		WGPULoadOp_Load = 1,
// 		WGPULoadOp_Force32 = 2147483647,
// 	}

// 	public enum WGPUPipelineStatisticName
// 	{
// 		WGPUPipelineStatisticName_VertexShaderInvocations = 0,
// 		WGPUPipelineStatisticName_ClipperInvocations = 1,
// 		WGPUPipelineStatisticName_ClipperPrimitivesOut = 2,
// 		WGPUPipelineStatisticName_FragmentShaderInvocations = 3,
// 		WGPUPipelineStatisticName_ComputeShaderInvocations = 4,
// 		WGPUPipelineStatisticName_Force32 = 2147483647,
// 	}

// 	public enum WGPUPresentMode
// 	{
// 		WGPUPresentMode_Immediate = 0,
// 		WGPUPresentMode_Mailbox = 1,
// 		WGPUPresentMode_Fifo = 2,
// 		WGPUPresentMode_Force32 = 2147483647,
// 	}

// 	public enum WGPUPrimitiveTopology
// 	{
// 		WGPUPrimitiveTopology_PointList = 0,
// 		WGPUPrimitiveTopology_LineList = 1,
// 		WGPUPrimitiveTopology_LineStrip = 2,
// 		WGPUPrimitiveTopology_TriangleList = 3,
// 		WGPUPrimitiveTopology_TriangleStrip = 4,
// 		WGPUPrimitiveTopology_Force32 = 2147483647,
// 	}

// 	public enum WGPUQueryType
// 	{
// 		WGPUQueryType_Occlusion = 0,
// 		WGPUQueryType_PipelineStatistics = 1,
// 		WGPUQueryType_Timestamp = 2,
// 		WGPUQueryType_Force32 = 2147483647,
// 	}

// 	public enum WGPUSType
// 	{
// 		WGPUSType_Invalid = 0,
// 		WGPUSType_SurfaceDescriptorFromMetalLayer = 1,
// 		WGPUSType_SurfaceDescriptorFromWindowsHWND = 2,
// 		WGPUSType_SurfaceDescriptorFromXlib = 3,
// 		WGPUSType_SurfaceDescriptorFromCanvasHTMLSelector = 4,
// 		WGPUSType_ShaderModuleSPIRVDescriptor = 5,
// 		WGPUSType_ShaderModuleWGSLDescriptor = 6,
// 		WGPUSType_SamplerDescriptorDummyAnisotropicFiltering = 7,
// 		WGPUSType_RenderPipelineDescriptorDummyExtension = 8,
// 		WGPUSType_Force32 = 2147483647,
// 	}

// 	public enum WGPUStencilOperation
// 	{
// 		WGPUStencilOperation_Keep = 0,
// 		WGPUStencilOperation_Zero = 1,
// 		WGPUStencilOperation_Replace = 2,
// 		WGPUStencilOperation_Invert = 3,
// 		WGPUStencilOperation_IncrementClamp = 4,
// 		WGPUStencilOperation_DecrementClamp = 5,
// 		WGPUStencilOperation_IncrementWrap = 6,
// 		WGPUStencilOperation_DecrementWrap = 7,
// 		WGPUStencilOperation_Force32 = 2147483647,
// 	}

// 	public enum WGPUStoreOp
// 	{
// 		WGPUStoreOp_Store = 0,
// 		WGPUStoreOp_Clear = 1,
// 		WGPUStoreOp_Force32 = 2147483647,
// 	}

// 	public enum WGPUTextureAspect
// 	{
// 		WGPUTextureAspect_All = 0,
// 		WGPUTextureAspect_StencilOnly = 1,
// 		WGPUTextureAspect_DepthOnly = 2,
// 		WGPUTextureAspect_Force32 = 2147483647,
// 	}

// 	public enum WGPUTextureComponentType
// 	{
// 		WGPUTextureComponentType_Float = 0,
// 		WGPUTextureComponentType_Sint = 1,
// 		WGPUTextureComponentType_Uint = 2,
// 		WGPUTextureComponentType_Force32 = 2147483647,
// 	}

// 	public enum WGPUTextureDimension
// 	{
// 		WGPUTextureDimension_1D = 0,
// 		WGPUTextureDimension_2D = 1,
// 		WGPUTextureDimension_3D = 2,
// 		WGPUTextureDimension_Force32 = 2147483647,
// 	}

// 	public enum WGPUTextureFormat
// 	{
// 		WGPUTextureFormat_Undefined = 0,
// 		WGPUTextureFormat_R8Unorm = 1,
// 		WGPUTextureFormat_R8Snorm = 2,
// 		WGPUTextureFormat_R8Uint = 3,
// 		WGPUTextureFormat_R8Sint = 4,
// 		WGPUTextureFormat_R16Uint = 5,
// 		WGPUTextureFormat_R16Sint = 6,
// 		WGPUTextureFormat_R16Float = 7,
// 		WGPUTextureFormat_RG8Unorm = 8,
// 		WGPUTextureFormat_RG8Snorm = 9,
// 		WGPUTextureFormat_RG8Uint = 10,
// 		WGPUTextureFormat_RG8Sint = 11,
// 		WGPUTextureFormat_R32Float = 12,
// 		WGPUTextureFormat_R32Uint = 13,
// 		WGPUTextureFormat_R32Sint = 14,
// 		WGPUTextureFormat_RG16Uint = 15,
// 		WGPUTextureFormat_RG16Sint = 16,
// 		WGPUTextureFormat_RG16Float = 17,
// 		WGPUTextureFormat_RGBA8Unorm = 18,
// 		WGPUTextureFormat_RGBA8UnormSrgb = 19,
// 		WGPUTextureFormat_RGBA8Snorm = 20,
// 		WGPUTextureFormat_RGBA8Uint = 21,
// 		WGPUTextureFormat_RGBA8Sint = 22,
// 		WGPUTextureFormat_BGRA8Unorm = 23,
// 		WGPUTextureFormat_BGRA8UnormSrgb = 24,
// 		WGPUTextureFormat_RGB10A2Unorm = 25,
// 		WGPUTextureFormat_RG11B10Ufloat = 26,
// 		WGPUTextureFormat_RGB9E5Ufloat = 27,
// 		WGPUTextureFormat_RG32Float = 28,
// 		WGPUTextureFormat_RG32Uint = 29,
// 		WGPUTextureFormat_RG32Sint = 30,
// 		WGPUTextureFormat_RGBA16Uint = 31,
// 		WGPUTextureFormat_RGBA16Sint = 32,
// 		WGPUTextureFormat_RGBA16Float = 33,
// 		WGPUTextureFormat_RGBA32Float = 34,
// 		WGPUTextureFormat_RGBA32Uint = 35,
// 		WGPUTextureFormat_RGBA32Sint = 36,
// 		WGPUTextureFormat_Depth32Float = 37,
// 		WGPUTextureFormat_Depth24Plus = 38,
// 		WGPUTextureFormat_Depth24PlusStencil8 = 39,
// 		WGPUTextureFormat_BC1RGBAUnorm = 40,
// 		WGPUTextureFormat_BC1RGBAUnormSrgb = 41,
// 		WGPUTextureFormat_BC2RGBAUnorm = 42,
// 		WGPUTextureFormat_BC2RGBAUnormSrgb = 43,
// 		WGPUTextureFormat_BC3RGBAUnorm = 44,
// 		WGPUTextureFormat_BC3RGBAUnormSrgb = 45,
// 		WGPUTextureFormat_BC4RUnorm = 46,
// 		WGPUTextureFormat_BC4RSnorm = 47,
// 		WGPUTextureFormat_BC5RGUnorm = 48,
// 		WGPUTextureFormat_BC5RGSnorm = 49,
// 		WGPUTextureFormat_BC6HRGBUfloat = 50,
// 		WGPUTextureFormat_BC6HRGBFloat = 51,
// 		WGPUTextureFormat_BC7RGBAUnorm = 52,
// 		WGPUTextureFormat_BC7RGBAUnormSrgb = 53,
// 		WGPUTextureFormat_Force32 = 2147483647,
// 	}

// 	public enum WGPUTextureViewDimension
// 	{
// 		WGPUTextureViewDimension_Undefined = 0,
// 		WGPUTextureViewDimension_1D = 1,
// 		WGPUTextureViewDimension_2D = 2,
// 		WGPUTextureViewDimension_2DArray = 3,
// 		WGPUTextureViewDimension_Cube = 4,
// 		WGPUTextureViewDimension_CubeArray = 5,
// 		WGPUTextureViewDimension_3D = 6,
// 		WGPUTextureViewDimension_Force32 = 2147483647,
// 	}

// 	public enum WGPUVertexFormat
// 	{
// 		WGPUVertexFormat_UChar2 = 0,
// 		WGPUVertexFormat_UChar4 = 1,
// 		WGPUVertexFormat_Char2 = 2,
// 		WGPUVertexFormat_Char4 = 3,
// 		WGPUVertexFormat_UChar2Norm = 4,
// 		WGPUVertexFormat_UChar4Norm = 5,
// 		WGPUVertexFormat_Char2Norm = 6,
// 		WGPUVertexFormat_Char4Norm = 7,
// 		WGPUVertexFormat_UShort2 = 8,
// 		WGPUVertexFormat_UShort4 = 9,
// 		WGPUVertexFormat_Short2 = 10,
// 		WGPUVertexFormat_Short4 = 11,
// 		WGPUVertexFormat_UShort2Norm = 12,
// 		WGPUVertexFormat_UShort4Norm = 13,
// 		WGPUVertexFormat_Short2Norm = 14,
// 		WGPUVertexFormat_Short4Norm = 15,
// 		WGPUVertexFormat_Half2 = 16,
// 		WGPUVertexFormat_Half4 = 17,
// 		WGPUVertexFormat_Float = 18,
// 		WGPUVertexFormat_Float2 = 19,
// 		WGPUVertexFormat_Float3 = 20,
// 		WGPUVertexFormat_Float4 = 21,
// 		WGPUVertexFormat_UInt = 22,
// 		WGPUVertexFormat_UInt2 = 23,
// 		WGPUVertexFormat_UInt3 = 24,
// 		WGPUVertexFormat_UInt4 = 25,
// 		WGPUVertexFormat_Int = 26,
// 		WGPUVertexFormat_Int2 = 27,
// 		WGPUVertexFormat_Int3 = 28,
// 		WGPUVertexFormat_Int4 = 29,
// 		WGPUVertexFormat_Force32 = 2147483647,
// 	}

// 	[Flags]
// 	public enum WGPUBufferUsage
// 	{
// 		WGPUBufferUsage_None = 0,
// 		WGPUBufferUsage_MapRead = 1,
// 		WGPUBufferUsage_MapWrite = 2,
// 		WGPUBufferUsage_CopySrc = 4,
// 		WGPUBufferUsage_CopyDst = 8,
// 		WGPUBufferUsage_Index = 16,
// 		WGPUBufferUsage_Vertex = 32,
// 		WGPUBufferUsage_Uniform = 64,
// 		WGPUBufferUsage_Storage = 128,
// 		WGPUBufferUsage_Indirect = 256,
// 		WGPUBufferUsage_QueryResolve = 512,
// 		WGPUBufferUsage_Force32 = 2147483647,
// 	}

// 	[Flags]
// 	public enum WGPUColorWriteMask
// 	{
// 		WGPUColorWriteMask_None = 0,
// 		WGPUColorWriteMask_Red = 1,
// 		WGPUColorWriteMask_Green = 2,
// 		WGPUColorWriteMask_Blue = 4,
// 		WGPUColorWriteMask_Alpha = 8,
// 		WGPUColorWriteMask_All = 15,
// 		WGPUColorWriteMask_Force32 = 2147483647,
// 	}

// 	[Flags]
// 	public enum WGPUMapMode
// 	{
// 		WGPUMapMode_None = 0,
// 		WGPUMapMode_Read = 1,
// 		WGPUMapMode_Write = 2,
// 		WGPUMapMode_Force32 = 2147483647,
// 	}

// 	[Flags]
// 	public enum WGPUShaderStage
// 	{
// 		WGPUShaderStage_None = 0,
// 		WGPUShaderStage_Vertex = 1,
// 		WGPUShaderStage_Fragment = 2,
// 		WGPUShaderStage_Compute = 4,
// 		WGPUShaderStage_Force32 = 2147483647,
// 	}

// 	[Flags]
// 	public enum WGPUTextureUsage
// 	{
// 		WGPUTextureUsage_None = 0,
// 		WGPUTextureUsage_CopySrc = 1,
// 		WGPUTextureUsage_CopyDst = 2,
// 		WGPUTextureUsage_Sampled = 4,
// 		WGPUTextureUsage_Storage = 8,
// 		WGPUTextureUsage_OutputAttachment = 16,
// 		WGPUTextureUsage_Present = 32,
// 		WGPUTextureUsage_Force32 = 2147483647,
// 	}

//     [StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUChainedStruct
// 	{
// 		public WGPUChainedStruct* next;
// 		public WGPUSType sType;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUAdapterProperties
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public uint deviceID;
// 		public uint vendorID;
// 		public char* name;
// 		public WGPUAdapterType adapterType;
// 		public WGPUBackendType backendType;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBindGroupEntry
// 	{
// 		public uint binding;
// 		public IntPtr buffer;
// 		public ulong offset;
// 		public ulong size;
// 		public IntPtr sampler;
// 		public IntPtr textureView;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBindGroupLayoutEntry
// 	{
// 		public uint binding;
// 		public WGPUShaderStage visibility;
// 		public WGPUBindingType type;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool hasDynamicOffset;
// 		public ulong minBufferBindingSize;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool multisampled;
// 		public WGPUTextureViewDimension viewDimension;
// 		public WGPUTextureComponentType textureComponentType;
// 		public WGPUTextureFormat storageTextureFormat;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBlendDescriptor
// 	{
// 		public WGPUBlendOperation operation;
// 		public WGPUBlendFactor srcFactor;
// 		public WGPUBlendFactor dstFactor;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBufferDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public WGPUBufferUsage usage;
// 		public ulong size;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool mappedAtCreation;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUColor
// 	{
// 		public double r;
// 		public double g;
// 		public double b;
// 		public double a;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUCommandBufferDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUCommandEncoderDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUComputePassDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUDeviceProperties
// 	{
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool textureCompressionBC;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool shaderFloat16;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool pipelineStatisticsQuery;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool timestampQuery;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUExtent3D
// 	{
// 		public uint width;
// 		public uint height;
// 		public uint depth;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUFenceDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public ulong initialValue;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUInstanceDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUOrigin3D
// 	{
// 		public uint x;
// 		public uint y;
// 		public uint z;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUPipelineLayoutDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public uint bindGroupLayoutCount;
// 		public IntPtr* bindGroupLayouts;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUProgrammableStageDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public IntPtr module;
// 		public char* entryPoint;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUQuerySetDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public WGPUQueryType type;
// 		public uint count;
// 		public WGPUPipelineStatisticName* pipelineStatistics;
// 		public uint pipelineStatisticsCount;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURasterizationStateDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public WGPUFrontFace frontFace;
// 		public WGPUCullMode cullMode;
// 		public int depthBias;
// 		public float depthBiasSlopeScale;
// 		public float depthBiasClamp;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderBundleDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderBundleEncoderDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public uint colorFormatsCount;
// 		public WGPUTextureFormat* colorFormats;
// 		public WGPUTextureFormat depthStencilFormat;
// 		public uint sampleCount;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderPassDepthStencilAttachmentDescriptor
// 	{
// 		public IntPtr attachment;
// 		public WGPULoadOp depthLoadOp;
// 		public WGPUStoreOp depthStoreOp;
// 		public float clearDepth;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool depthReadOnly;
// 		public WGPULoadOp stencilLoadOp;
// 		public WGPUStoreOp stencilStoreOp;
// 		public uint clearStencil;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool stencilReadOnly;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSamplerDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public WGPUAddressMode addressModeU;
// 		public WGPUAddressMode addressModeV;
// 		public WGPUAddressMode addressModeW;
// 		public WGPUFilterMode magFilter;
// 		public WGPUFilterMode minFilter;
// 		public WGPUFilterMode mipmapFilter;
// 		public float lodMinClamp;
// 		public float lodMaxClamp;
// 		public WGPUCompareFunction compare;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSamplerDescriptorDummyAnisotropicFiltering
// 	{
// 		public WGPUChainedStruct chain;
// 		public float maxAnisotropy;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUShaderModuleDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUShaderModuleSPIRVDescriptor
// 	{
// 		public WGPUChainedStruct chain;
// 		public uint codeSize;
// 		public uint* code;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUShaderModuleWGSLDescriptor
// 	{
// 		public WGPUChainedStruct chain;
// 		public char* source;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUStencilStateFaceDescriptor
// 	{
// 		public WGPUCompareFunction compare;
// 		public WGPUStencilOperation failOp;
// 		public WGPUStencilOperation depthFailOp;
// 		public WGPUStencilOperation passOp;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSurfaceDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSurfaceDescriptorFromCanvasHTMLSelector
// 	{
// 		public WGPUChainedStruct chain;
// 		public char* selector;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSurfaceDescriptorFromMetalLayer
// 	{
// 		public WGPUChainedStruct chain;
// 		public void* layer;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSurfaceDescriptorFromWindowsHWND
// 	{
// 		public WGPUChainedStruct chain;
// 		public void* hinstance;
// 		public void* hwnd;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSurfaceDescriptorFromXlib
// 	{
// 		public WGPUChainedStruct chain;
// 		public void* display;
// 		public uint window;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUSwapChainDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public WGPUTextureUsage usage;
// 		public WGPUTextureFormat format;
// 		public uint width;
// 		public uint height;
// 		public WGPUPresentMode presentMode;
// 		public ulong implementation;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUTextureDataLayout
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public ulong offset;
// 		public uint bytesPerRow;
// 		public uint rowsPerImage;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUTextureViewDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public WGPUTextureFormat format;
// 		public WGPUTextureViewDimension dimension;
// 		public uint baseMipLevel;
// 		public uint mipLevelCount;
// 		public uint baseArrayLayer;
// 		public uint arrayLayerCount;
// 		public WGPUTextureAspect aspect;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUVertexAttributeDescriptor
// 	{
// 		public WGPUVertexFormat format;
// 		public ulong offset;
// 		public uint shaderLocation;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBindGroupDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public IntPtr layout;
// 		public uint entryCount;
// 		public WGPUBindGroupEntry* entries;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBindGroupLayoutDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public uint entryCount;
// 		public WGPUBindGroupLayoutEntry* entries;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUBufferCopyView
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public WGPUTextureDataLayout layout;
// 		public IntPtr buffer;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUColorStateDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public WGPUTextureFormat format;
// 		public WGPUBlendDescriptor alphaBlend;
// 		public WGPUBlendDescriptor colorBlend;
// 		public WGPUColorWriteMask writeMask;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUComputePipelineDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public IntPtr layout;
// 		public WGPUProgrammableStageDescriptor computeStage;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUDepthStencilStateDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public WGPUTextureFormat format;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool depthWriteEnabled;
// 		public WGPUCompareFunction depthCompare;
// 		public WGPUStencilStateFaceDescriptor stencilFront;
// 		public WGPUStencilStateFaceDescriptor stencilBack;
// 		public uint stencilReadMask;
// 		public uint stencilWriteMask;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderPassColorAttachmentDescriptor
// 	{
// 		public IntPtr attachment;
// 		public IntPtr resolveTarget;
// 		public WGPULoadOp loadOp;
// 		public WGPUStoreOp storeOp;
// 		public WGPUColor clearColor;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderPipelineDescriptorDummyExtension
// 	{
// 		public WGPUChainedStruct chain;
// 		public WGPUProgrammableStageDescriptor dummyStage;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUTextureCopyView
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public IntPtr texture;
// 		public uint mipLevel;
// 		public WGPUOrigin3D origin;
// 		public WGPUTextureAspect aspect;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUTextureDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public WGPUTextureUsage usage;
// 		public WGPUTextureDimension dimension;
// 		public WGPUExtent3D size;
// 		public WGPUTextureFormat format;
// 		public uint mipLevelCount;
// 		public uint sampleCount;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUVertexBufferLayoutDescriptor
// 	{
// 		public ulong arrayStride;
// 		public WGPUInputStepMode stepMode;
// 		public uint attributeCount;
// 		public WGPUVertexAttributeDescriptor* attributes;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderPassDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public uint colorAttachmentCount;
// 		public WGPURenderPassColorAttachmentDescriptor* colorAttachments;
// 		public WGPURenderPassDepthStencilAttachmentDescriptor* depthStencilAttachment;
// 		public IntPtr occlusionQuerySet;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPUVertexStateDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public WGPUIndexFormat indexFormat;
// 		public uint vertexBufferCount;
// 		public WGPUVertexBufferLayoutDescriptor* vertexBuffers;
// 	}

// 	[StructLayout(LayoutKind.Sequential)]
// 	public unsafe struct WGPURenderPipelineDescriptor
// 	{
// 		public WGPUChainedStruct* nextInChain;
// 		public char* label;
// 		public IntPtr layout;
// 		public WGPUProgrammableStageDescriptor vertexStage;
// 		public WGPUProgrammableStageDescriptor* fragmentStage;
// 		public WGPUVertexStateDescriptor* vertexState;
// 		public WGPUPrimitiveTopology primitiveTopology;
// 		public WGPURasterizationStateDescriptor* rasterizationState;
// 		public uint sampleCount;
// 		public WGPUDepthStencilStateDescriptor* depthStencilState;
// 		public uint colorStateCount;
// 		public WGPUColorStateDescriptor* colorStates;
// 		public uint sampleMask;
// 		[MarshalAs(UnmanagedType.I1)]
// 		public bool alphaToCoverageEnabled;
// 	}
