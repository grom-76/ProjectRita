namespace RitaEngine.Base.Platform.Native.Web;
/*

// Based on exemple : https://github.com/emepetres/HelloWebGPU.Net
// */

// namespace RITAENGINE.PLATFORM.DEPENDENCIES.GRAPHIC.WEBGPU.EMSCRIPTEN;


// using System;
// using System.Runtime.InteropServices;

// using WGPUDevice = System.IntPtr;
// using WGPUSwapChain = System.IntPtr;
// using WGPUSurface = System.IntPtr;

// using RITAENGINE.PLATFORM.DEPENDENCIES.GRAPHIC.WEBGPU.BROWSER;


// [AttributeUsage(AttributeTargets.Method)]
// internal sealed class MonoPInvokeCallbackAttribute : Attribute
// {
//     public MonoPInvokeCallbackAttribute(Type type)
//     {
//         Type = type;
//     }

//     public Type Type { get; private set; }
// }

// public unsafe class Emscripten
// {
//     private static char* canvas_str = (char*)Marshal.StringToHGlobalAnsi("canvas");

//     public delegate bool Loop();
//     public delegate int EMLoop(double time, void* userData);

//     [DllImport("libWebGPU")]
//     private static extern WGPUDevice emscripten_webgpu_get_device();

//     [DllImport("libWebGPU")]
//     private static extern WGPUDevice emscripten_request_animation_frame_loop(EMLoop em_loop, void* userData);

//     public static WGPUDevice CreateDevice(IntPtr _)
//     {
//         return emscripten_webgpu_get_device();
//     }

//     public static WGPUSwapChain CreateSwapChain(WGPUDevice device)
//     {
//         WGPUSurfaceDescriptorFromCanvasHTMLSelector canvDesc = new WGPUSurfaceDescriptorFromCanvasHTMLSelector
//         {
//             chain = new WGPUChainedStruct
//             {
//                 sType = WGPUSType.WGPUSType_SurfaceDescriptorFromCanvasHTMLSelector,
//             },
//             selector = canvas_str
//         };

//         WGPUSurfaceDescriptor surfDesc = new WGPUSurfaceDescriptor
//         {
//             nextInChain = (WGPUChainedStruct*)&canvDesc
//         };

//         WGPUSurface surface = WebGPUNative.wgpuInstanceCreateSurface(IntPtr.Zero, &surfDesc);

//         WGPUSwapChainDescriptor swapDesc = new WGPUSwapChainDescriptor
//         {
//             usage = WGPUTextureUsage.WGPUTextureUsage_OutputAttachment,
//             format = WGPUTextureFormat.WGPUTextureFormat_BGRA8Unorm,
//             width = 800,
//             height = 450,
//             presentMode = WGPUPresentMode.WGPUPresentMode_Fifo
//         };

//         return WebGPUNative.wgpuDeviceCreateSwapChain(device, surface, &swapDesc); ;
//     }

//     public static WGPUTextureFormat GetSwapChainFormat(WGPUDevice _)
//     {
//         return WGPUTextureFormat.WGPUTextureFormat_BGRA8Unorm;
//     }

//     [MonoPInvokeCallback(typeof(EMLoop))]
//     public static int em_loop(double _, void* userData)
//     {
//         Loop func = Marshal.GetDelegateForFunctionPointer<Loop>((IntPtr)userData);
//         return func() ? 1 : 0;
//     }

//     public static void MainLoop(Loop func)
//     {
//         void* userData = Marshal.GetFunctionPointerForDelegate(func).ToPointer();
//         emscripten_request_animation_frame_loop(em_loop, userData);
//     }
// }



//     public unsafe class Triangle
// {
//     public static IntPtr Device;
//     public static IntPtr Queue;
//     public static IntPtr SwapChain;

//     public static IntPtr pipeline;
//     public static IntPtr vertBuf; // vertex buffer with triangle position and colours
//     public static IntPtr indxBuf; // index buffer
//     public static IntPtr uRotBuf; // uniform buffer (containing the rotation angle)
//     public static IntPtr bindGroup;

//     static float rotDeg = 0.0f; // Current rotation angle (in degrees, updated per frame).
//     static char* str_entrypoint = (char*)Marshal.StringToHGlobalAnsi("main").ToPointer();

//     public static IntPtr createBindGroupLayout()
//     {
//         WGPUBindGroupLayoutEntry bglEntry = new WGPUBindGroupLayoutEntry
//         {
//             binding = 0,
//             visibility = WGPUShaderStage.WGPUShaderStage_Vertex,
//             type = WGPUBindingType.WGPUBindingType_UniformBuffer
//         };
//         WGPUBindGroupLayoutDescriptor bglDesc = new WGPUBindGroupLayoutDescriptor
//         {
//             entryCount = 1,
//             entries = &bglEntry
//         };
//         return WebGPUNative.wgpuDeviceCreateBindGroupLayout(Device, &bglDesc);
//     }

//     public static IntPtr CreatePipeline(IntPtr bindGroupLayout)
//     {
//         // Load shaders
//         var vertMod = CreateShader(triangleVert);
//         //var vertMod = TriangleCPP.createVertShader();
//         var fragMod = CreateShader(triangleFrag);
//         //var fragMod = TriangleCPP.createFragShader();

//         WGPUPipelineLayoutDescriptor layoutDesc = new WGPUPipelineLayoutDescriptor
//         {
//             bindGroupLayoutCount = 1,
//             bindGroupLayouts = &bindGroupLayout
//         };
//         IntPtr pipelineLayout = WebGPUNative.wgpuDeviceCreatePipelineLayout(Device, &layoutDesc);

//         // begin pipeline set-up
//         WGPURenderPipelineDescriptor desc = new WGPURenderPipelineDescriptor
//         {
//             layout = pipelineLayout,
//             vertexStage = new WGPUProgrammableStageDescriptor()
//             {
//                 module = vertMod,
//                 entryPoint = str_entrypoint
//             }
//         };

//         WGPUProgrammableStageDescriptor fragStage = new WGPUProgrammableStageDescriptor
//         {
//             module = fragMod,
//             entryPoint = str_entrypoint
//         };
//         desc.fragmentStage = &fragStage;

//         // describe buffer layouts
//         var vertAttrs = stackalloc WGPUVertexAttributeDescriptor[2];
//         vertAttrs[0] = new WGPUVertexAttributeDescriptor
//         {
//             format = WGPUVertexFormat.WGPUVertexFormat_Float2,
//             offset = 0,
//             shaderLocation = 0
//         };
//         vertAttrs[1] = new WGPUVertexAttributeDescriptor
//         {
//             format = WGPUVertexFormat.WGPUVertexFormat_Float3,
//             offset = 2 * sizeof(float),
//             shaderLocation = 1
//         };

//         WGPUVertexBufferLayoutDescriptor vertDesc = new WGPUVertexBufferLayoutDescriptor
//         {
//             arrayStride = 5 * sizeof(float),
//             attributeCount = 2,
//             attributes = vertAttrs
//         };
//         WGPUVertexStateDescriptor vertState = new WGPUVertexStateDescriptor
//         {
//             indexFormat = WGPUIndexFormat.WGPUIndexFormat_Uint16,
//             vertexBufferCount = 1,
//             vertexBuffers = &vertDesc
//         };

//         desc.vertexState = &vertState;
//         desc.primitiveTopology = WGPUPrimitiveTopology.WGPUPrimitiveTopology_TriangleList;

//         desc.sampleCount = 1;

//         // describe blend
//         WGPUBlendDescriptor blendDesc = new WGPUBlendDescriptor
//         {
//             operation = WGPUBlendOperation.WGPUBlendOperation_Add,
//             srcFactor = WGPUBlendFactor.WGPUBlendFactor_SrcAlpha,
//             dstFactor = WGPUBlendFactor.WGPUBlendFactor_OneMinusSrcAlpha
//         };
//         WGPUColorStateDescriptor colorDesc = new WGPUColorStateDescriptor
//         {
//             format = Emscripten.GetSwapChainFormat(Device),
//             alphaBlend = blendDesc,
//             colorBlend = blendDesc,
//             writeMask = WGPUColorWriteMask.WGPUColorWriteMask_All
//         };

//         desc.colorStateCount = 1;
//         desc.colorStates = &colorDesc;

//         desc.sampleMask = 0xFFFFFFFF; // <-- Note: this currently causes Emscripten to fail (sampleMask ends up as -1, which trips an assert)

//         IntPtr _pipeline = WebGPUNative.wgpuDeviceCreateRenderPipeline(Device, ref desc);

//         // partial clean-up (just move to the end, no?)
//         WebGPUNative.wgpuPipelineLayoutRelease(pipelineLayout);

//         WebGPUNative.wgpuShaderModuleRelease(fragMod);
//         WebGPUNative.wgpuShaderModuleRelease(vertMod);

//         return _pipeline;
//     }

//     public static IntPtr CreateVertBuffer()
//     {
//         // create the buffers (x, y, r, g, b)
//         float[] vertData = {
//             -0.8f, -0.8f, 0.0f, 0.0f, 1.0f, // BL
//                 0.8f, -0.8f, 0.0f, 1.0f, 0.0f, // BR
//             -0.0f,  0.8f, 1.0f, 0.0f, 0.0f, // top
//         };
//         var p_vertData = stackalloc float[vertData.Length];
//         for (int i = 0; i < vertData.Length; i++)
//         {
//             p_vertData[i] = vertData[i];
//         }
//         return CreateBuffer(p_vertData, (ulong)(vertData.Length * sizeof(float)), WGPUBufferUsage.WGPUBufferUsage_Vertex);
//     }

//     public static IntPtr CreateIndxBuffer()
//     {
//         UInt16[] indxData = {
//             0, 1, 2,
//             0 // padding (better way of doing this?)
//         };
//         var p_indxData = stackalloc UInt16[indxData.Length];
//         for (int i = 0; i < indxData.Length; i++)
//         {
//             p_indxData[i] = indxData[i];
//         }
//         return CreateBuffer(p_indxData, (ulong)(indxData.Length * sizeof(UInt16)), WGPUBufferUsage.WGPUBufferUsage_Index);
//     }

//     public static IntPtr CreateDataBuffer()
//     {
//         IntPtr data_buff;
//         // create the uniform bind group (note 'rotDeg' is copied here, not bound in any way)
//         fixed (void* data = &rotDeg)
//         {
//             data_buff = CreateBuffer(data, sizeof(float), WGPUBufferUsage.WGPUBufferUsage_Uniform);
//         }

//         return data_buff;
//     }

//     public static IntPtr CreateBindGroup(IntPtr bindGroupLayout, IntPtr _uRotBuf)
//     {
//         WGPUBindGroupEntry bgEntry = new WGPUBindGroupEntry
//         {
//             binding = 0,
//             buffer = _uRotBuf,
//             offset = 0,
//             size = sizeof(float) // sizeof(rotDeg)
//         };

//         WGPUBindGroupDescriptor bgDesc = new WGPUBindGroupDescriptor
//         {
//             layout = bindGroupLayout,
//             entryCount = 1,
//             entries = &bgEntry
//         };

//         return WebGPUNative.wgpuDeviceCreateBindGroup(Device, &bgDesc);
//     }

//     public static void CreatePipelineAndBuffers()
//     {
//         IntPtr bindGroupLayout = createBindGroupLayout();
//         //IntPtr bindGroupLayout = TriangleCPP.createBindGroupLayout();

//         pipeline = CreatePipeline(bindGroupLayout);
//         //pipeline = TriangleCPP.createPipeline(bindGroupLayout);

//         vertBuf = CreateVertBuffer();
//         //vertBuf = TriangleCPP.createVertBuffer();
//         indxBuf = CreateIndxBuffer();
//         //indxBuf = TriangleCPP.createIndxBuffer();
//         uRotBuf = CreateDataBuffer();
//         //uRotBuf = TriangleCPP.createDataBuffer();

//         bindGroup = CreateBindGroup(bindGroupLayout, uRotBuf);
//         //bindGroup = TriangleCPP.createBindGroup(bindGroupLayout, uRotBuf);

//         // last bit of clean-up
//         WebGPUNative.wgpuBindGroupLayoutRelease(bindGroupLayout);
//     }

//     [MonoPInvokeCallback(typeof(Emscripten.Loop))]
//     public static bool redraw()
//     {
//         IntPtr backBufView = WebGPUNative.wgpuSwapChainGetCurrentTextureView(SwapChain); // create textureView

//         WGPURenderPassColorAttachmentDescriptor colorDesc = new WGPURenderPassColorAttachmentDescriptor
//         {
//             attachment = backBufView,
//             loadOp = WGPULoadOp.WGPULoadOp_Clear,
//             storeOp = WGPUStoreOp.WGPUStoreOp_Store,
//             clearColor = new WGPUColor
//             {
//                 r = 0.3f,
//                 g = 0.3f,
//                 b = 0.3f,
//                 a = 1.0f
//             }
//         };

//         WGPURenderPassDescriptor renderPass = new WGPURenderPassDescriptor
//         {
//             colorAttachmentCount = 1,
//             colorAttachments = &colorDesc
//         };

//         IntPtr encoder = WebGPUNative.wgpuDeviceCreateCommandEncoder(Device, null); // create encoder
//         IntPtr pass = WebGPUNative.wgpuCommandEncoderBeginRenderPass(encoder, &renderPass);

//         // update the rotation
//         rotDeg += 0.1f;
//         fixed (void* data = &rotDeg)
//         {
//             WebGPUNative.wgpuQueueWriteBuffer(Queue, uRotBuf, 0, data, sizeof(float));
//         }

//         // draw the triangle (comment these five lines to simply clear the screen)
//         WebGPUNative.wgpuRenderPassEncoderSetPipeline(pass, pipeline);
//         WebGPUNative.wgpuRenderPassEncoderSetBindGroup(pass, 0, bindGroup, 0, null);
//         WebGPUNative.wgpuRenderPassEncoderSetVertexBuffer(pass, 0, vertBuf, 0, 0);
//         WebGPUNative.wgpuRenderPassEncoderSetIndexBuffer(pass, indxBuf, 0, 0);
//         WebGPUNative.wgpuRenderPassEncoderDrawIndexed(pass, 3, 1, 0, 0, 0);

//         WebGPUNative.wgpuRenderPassEncoderEndPass(pass);
//         WebGPUNative.wgpuRenderPassEncoderRelease(pass);                         // release pass
//         IntPtr commands = WebGPUNative.wgpuCommandEncoderFinish(encoder, null);  // create commands
//         WebGPUNative.wgpuCommandEncoderRelease(encoder);                         // release encoder

//         WebGPUNative.wgpuQueueSubmit(Queue, 1, &commands);
//         WebGPUNative.wgpuCommandBufferRelease(commands);                         // release commands

//         // TODO EMSCRIPTEN: wgpuSwapChainPresent is unsupported in Emscripten, so what do we do?
//         //WebGPUNative.wgpuSwapChainPresent(SwapChain);

//         WebGPUNative.wgpuTextureViewRelease(backBufView);                        // release textureView

//         return true;
//     }

//     private static IntPtr CreateShader(UInt32[] byte_code, char* label = null)
//     {
//         IntPtr shader;
//         fixed (uint* code = byte_code)
//         {
//             WGPUShaderModuleSPIRVDescriptor spirv = new WGPUShaderModuleSPIRVDescriptor()
//             {
//                 chain = new WGPUChainedStruct()
//                 {
//                     sType = WGPUSType.WGPUSType_ShaderModuleSPIRVDescriptor
//                 },
//                 codeSize = (uint)byte_code.Length,
//                 code = code
//             };

//             WGPUShaderModuleDescriptor desc = new WGPUShaderModuleDescriptor()
//             {
//                 nextInChain = (WGPUChainedStruct*)&spirv,
//                 label = label
//             };

//             shader = WebGPUNative.wgpuDeviceCreateShaderModule(Device, &desc);
//         }

//         return shader;
//     }

//     /**
//         * Helper to create a buffer.
//         *
//         * \param[in] data pointer to the start of the raw data
//         * \param[in] size number of bytes in \a data
//         * \param[in] usage type of buffer
//         */
//     private static IntPtr CreateBuffer(void* data, ulong size, WGPUBufferUsage usage)
//     {
//         WGPUBufferDescriptor desc = new WGPUBufferDescriptor
//         {
//             usage = WGPUBufferUsage.WGPUBufferUsage_CopyDst | usage,
//             size = size
//         };
//         IntPtr buffer = WebGPUNative.wgpuDeviceCreateBuffer(Device, ref desc);
//         WebGPUNative.wgpuQueueWriteBuffer(Queue, buffer, 0, data, size);
//         return buffer;
//     }

//     /**
//         * Vertex shader SPIR-V.
//         * \code
//         *	// glslc -Os -mfmt=num -o - -c in.vert
//         *	#version 450
//         *	layout(set = 0, binding = 0) uniform Rotation {
//         *		float uRot;
//         *	};
//         *	layout(location = 0) in  vec2 aPos;
//         *	layout(location = 1) in  vec3 aCol;
//         *	layout(location = 0) out vec3 vCol;
//         *	void main() {
//         *		float cosA = cos(radians(uRot));
//         *		float sinA = sin(radians(uRot));
//         *		mat3 rot = mat3(cosA, sinA, 0.0,
//         *					   -sinA, cosA, 0.0,
//         *						0.0,  0.0,  1.0);
//         *		gl_Position = vec4(rot * vec3(aPos, 1.0), 1.0);
//         *		vCol = aCol;
//         *	}
//         * \endcode
//         */
//     private static UInt32[] triangleVert = {
//         0x07230203, 0x00010000, 0x000d0008, 0x00000043, 0x00000000, 0x00020011, 0x00000001, 0x0006000b,
//         0x00000001, 0x4c534c47, 0x6474732e, 0x3035342e, 0x00000000, 0x0003000e, 0x00000000, 0x00000001,
//         0x0009000f, 0x00000000, 0x00000004, 0x6e69616d, 0x00000000, 0x0000002d, 0x00000031, 0x0000003e,
//         0x00000040, 0x00050048, 0x00000009, 0x00000000, 0x00000023, 0x00000000, 0x00030047, 0x00000009,
//         0x00000002, 0x00040047, 0x0000000b, 0x00000022, 0x00000000, 0x00040047, 0x0000000b, 0x00000021,
//         0x00000000, 0x00050048, 0x0000002b, 0x00000000, 0x0000000b, 0x00000000, 0x00050048, 0x0000002b,
//         0x00000001, 0x0000000b, 0x00000001, 0x00050048, 0x0000002b, 0x00000002, 0x0000000b, 0x00000003,
//         0x00050048, 0x0000002b, 0x00000003, 0x0000000b, 0x00000004, 0x00030047, 0x0000002b, 0x00000002,
//         0x00040047, 0x00000031, 0x0000001e, 0x00000000, 0x00040047, 0x0000003e, 0x0000001e, 0x00000000,
//         0x00040047, 0x00000040, 0x0000001e, 0x00000001, 0x00020013, 0x00000002, 0x00030021, 0x00000003,
//         0x00000002, 0x00030016, 0x00000006, 0x00000020, 0x0003001e, 0x00000009, 0x00000006, 0x00040020,
//         0x0000000a, 0x00000002, 0x00000009, 0x0004003b, 0x0000000a, 0x0000000b, 0x00000002, 0x00040015,
//         0x0000000c, 0x00000020, 0x00000001, 0x0004002b, 0x0000000c, 0x0000000d, 0x00000000, 0x00040020,
//         0x0000000e, 0x00000002, 0x00000006, 0x00040017, 0x00000018, 0x00000006, 0x00000003, 0x00040018,
//         0x00000019, 0x00000018, 0x00000003, 0x0004002b, 0x00000006, 0x0000001e, 0x00000000, 0x0004002b,
//         0x00000006, 0x00000022, 0x3f800000, 0x00040017, 0x00000027, 0x00000006, 0x00000004, 0x00040015,
//         0x00000028, 0x00000020, 0x00000000, 0x0004002b, 0x00000028, 0x00000029, 0x00000001, 0x0004001c,
//         0x0000002a, 0x00000006, 0x00000029, 0x0006001e, 0x0000002b, 0x00000027, 0x00000006, 0x0000002a,
//         0x0000002a, 0x00040020, 0x0000002c, 0x00000003, 0x0000002b, 0x0004003b, 0x0000002c, 0x0000002d,
//         0x00000003, 0x00040017, 0x0000002f, 0x00000006, 0x00000002, 0x00040020, 0x00000030, 0x00000001,
//         0x0000002f, 0x0004003b, 0x00000030, 0x00000031, 0x00000001, 0x00040020, 0x0000003b, 0x00000003,
//         0x00000027, 0x00040020, 0x0000003d, 0x00000003, 0x00000018, 0x0004003b, 0x0000003d, 0x0000003e,
//         0x00000003, 0x00040020, 0x0000003f, 0x00000001, 0x00000018, 0x0004003b, 0x0000003f, 0x00000040,
//         0x00000001, 0x0006002c, 0x00000018, 0x00000042, 0x0000001e, 0x0000001e, 0x00000022, 0x00050036,
//         0x00000002, 0x00000004, 0x00000000, 0x00000003, 0x000200f8, 0x00000005, 0x00050041, 0x0000000e,
//         0x0000000f, 0x0000000b, 0x0000000d, 0x0004003d, 0x00000006, 0x00000010, 0x0000000f, 0x0006000c,
//         0x00000006, 0x00000011, 0x00000001, 0x0000000b, 0x00000010, 0x0006000c, 0x00000006, 0x00000012,
//         0x00000001, 0x0000000e, 0x00000011, 0x0006000c, 0x00000006, 0x00000017, 0x00000001, 0x0000000d,
//         0x00000011, 0x0004007f, 0x00000006, 0x00000020, 0x00000017, 0x00060050, 0x00000018, 0x00000023,
//         0x00000012, 0x00000017, 0x0000001e, 0x00060050, 0x00000018, 0x00000024, 0x00000020, 0x00000012,
//         0x0000001e, 0x00060050, 0x00000019, 0x00000026, 0x00000023, 0x00000024, 0x00000042, 0x0004003d,
//         0x0000002f, 0x00000032, 0x00000031, 0x00050051, 0x00000006, 0x00000033, 0x00000032, 0x00000000,
//         0x00050051, 0x00000006, 0x00000034, 0x00000032, 0x00000001, 0x00060050, 0x00000018, 0x00000035,
//         0x00000033, 0x00000034, 0x00000022, 0x00050091, 0x00000018, 0x00000036, 0x00000026, 0x00000035,
//         0x00050051, 0x00000006, 0x00000037, 0x00000036, 0x00000000, 0x00050051, 0x00000006, 0x00000038,
//         0x00000036, 0x00000001, 0x00050051, 0x00000006, 0x00000039, 0x00000036, 0x00000002, 0x00070050,
//         0x00000027, 0x0000003a, 0x00000037, 0x00000038, 0x00000039, 0x00000022, 0x00050041, 0x0000003b,
//         0x0000003c, 0x0000002d, 0x0000000d, 0x0003003e, 0x0000003c, 0x0000003a, 0x0004003d, 0x00000018,
//         0x00000041, 0x00000040, 0x0003003e, 0x0000003e, 0x00000041, 0x000100fd, 0x00010038
//     };

//     /**
//         * Fragment shader SPIR-V.
//         * \code
//         *	// glslc -Os -mfmt=num -o - -c in.frag
//         *	#version 450
//         *	layout(location = 0) in  vec3 vCol;
//         *	layout(location = 0) out vec4 fragColor;
//         *	void main() {
//         *		fragColor = vec4(vCol, 1.0);
//         *	}
//         * \endcode
//         */
//     private static UInt32[] triangleFrag = {
//         0x07230203, 0x00010000, 0x000d0007, 0x00000013, 0x00000000, 0x00020011, 0x00000001, 0x0006000b,
//         0x00000001, 0x4c534c47, 0x6474732e, 0x3035342e, 0x00000000, 0x0003000e, 0x00000000, 0x00000001,
//         0x0007000f, 0x00000004, 0x00000004, 0x6e69616d, 0x00000000, 0x00000009, 0x0000000c, 0x00030010,
//         0x00000004, 0x00000007, 0x00040047, 0x00000009, 0x0000001e, 0x00000000, 0x00040047, 0x0000000c,
//         0x0000001e, 0x00000000, 0x00020013, 0x00000002, 0x00030021, 0x00000003, 0x00000002, 0x00030016,
//         0x00000006, 0x00000020, 0x00040017, 0x00000007, 0x00000006, 0x00000004, 0x00040020, 0x00000008,
//         0x00000003, 0x00000007, 0x0004003b, 0x00000008, 0x00000009, 0x00000003, 0x00040017, 0x0000000a,
//         0x00000006, 0x00000003, 0x00040020, 0x0000000b, 0x00000001, 0x0000000a, 0x0004003b, 0x0000000b,
//         0x0000000c, 0x00000001, 0x0004002b, 0x00000006, 0x0000000e, 0x3f800000, 0x00050036, 0x00000002,
//         0x00000004, 0x00000000, 0x00000003, 0x000200f8, 0x00000005, 0x0004003d, 0x0000000a, 0x0000000d,
//         0x0000000c, 0x00050051, 0x00000006, 0x0000000f, 0x0000000d, 0x00000000, 0x00050051, 0x00000006,
//         0x00000010, 0x0000000d, 0x00000001, 0x00050051, 0x00000006, 0x00000011, 0x0000000d, 0x00000002,
//         0x00070050, 0x00000007, 0x00000012, 0x0000000f, 0x00000010, 0x00000011, 0x0000000e, 0x0003003e,
//         0x00000009, 0x00000012, 0x000100fd, 0x00010038
//     };

//         /// <summary>
//     ///  The main entry point for the application.
//     /// </summary>
//     [STAThread]
//     static void Main()
//     {
//         //Emscripten.wgpu_set_dotnet_entry_point(EntryPoint);
//         //Emscripten.wgpu_run();

//         var device = Emscripten.CreateDevice(IntPtr.Zero);
//         Console.WriteLine("----> Device: " + device);
//         var queue = WebGPUNative.wgpuDeviceGetDefaultQueue(device);
//         Console.WriteLine("----> Queue: " + device);
//         var swapChain = Emscripten.CreateSwapChain(device);
//         Console.WriteLine("----> SwapChain: " + device);

//         Triangle.Device = device;
//         Triangle.Queue = queue;
//         Triangle.SwapChain = swapChain;

//         Triangle.CreatePipelineAndBuffers();
//         Console.WriteLine("----> PipelinesAndBuffers!");

//         Emscripten.MainLoop(Triangle.redraw);
//     }
// }

// //public class WGPUPipelineLayout
// //{
// //	private IntPtr value;
// //	public static implicit operator WGPUPipelineLayout(IntPtr val)
// //	{
// //		return new WGPUPipelineLayout() { value = val };
// //	}
// //	public static implicit operator IntPtr(WGPUPipelineLayout obj)
// //	{
// //		return ((obj == null) ? IntPtr.Zero : obj.value);
// //	}
// //}

// /*

// <!DOCTYPE html>
// <html>

// <head>
// <meta charset="UTF-8">
// <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=yes">
// <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
// <script src="mono-config.js"></script>
// <script src="runtime.js"></script>
// <script defer src="dotnet.js"></script>
// </head>

// <body>
// <div id="feedback-body" class="container-fluid">
//     <p id="loading">
//         <i class="fa fa-refresh fa-spin" style="font-size:14px;margin-right:0.5em;"></i> Loading...
//     </p>
// </div>

// <canvas id="canvas" oncontextmenu="event.preventDefault()" tabindex=-1 style="background-color:black;"></canvas>

// <script type="text/javascript">
//     var App = {
//         init: function () {
//             var feedback = document.getElementById("feedback-body");

//             if (feedback) {
//                 feedback.parentElement.removeChild(feedback);
//             }

            
//             //* None of the WebGPU properties appear to survive Closure, including
//             //* Emscripten's own `preinitializedWebGPUDevice` (which from looking at
//             //*`library_html5` is probably designed to be inited in script before
//             //* loading the Wasm).
            
//             if (navigator["gpu"]) {
//                 console.log("Initializing webgpu...");
//                 navigator["gpu"]["requestAdapter"]()
//                     .then(
//                         function (adapter) {
//                             adapter["requestDevice"]().then(function (device) {
//                                 console.log("Device: " + device);
//                                 Module["preinitializedWebGPUDevice"] = device;
//                                 BINDING.call_static_method("[HelloWebGPUNet.Web] HelloWebGPUNet.Program:Main", []);
//                             });
//                         },
//                         function () {
//                             console.error("No WebGPU adapter; not starting");
//                         });
//             }
//             else {
//                 console.error("No support for WebGPU; not starting");
//             }
//         }
//     };
//     Module["canvas"] = (function () {
//         var canvas = document.getElementById('canvas');

//         // As a default initial behavior, pop up an alert when webgl context is lost. To make your
//         // application robust, you may want to override this behavior before shipping!
//         // See http://www.khronos.org/registry/webgl/specs/latest/1.0/#5.15.2
//         canvas.addEventListener("webglcontextlost", function (e) { alert('WebGL context lost. You will need to reload the page.'); e.preventDefault(); }, false);

//         return canvas;
//     })();
// </script>
// </body>

// </html>







// */


