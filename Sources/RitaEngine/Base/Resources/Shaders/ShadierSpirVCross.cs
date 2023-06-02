/*
https://github.com/amerkoleci/Vortice.Vulkan/tree/main/src/Vortice.SpirvCross
Realize : 06 05 2023
*/

namespace RitaEngine.Base.Resources.Shader.SpirvCross;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static RitaEngine.Base.Resources.Shader.SpirvCross.SpirvCrossApi;
#region UTILS
public static unsafe class Utils
{
    // [DebuggerHidden]  [DebuggerStepThrough]
    public static void CheckResult(this Result result, string message = "SPIRV-Cross error occured")
    {
        if (result != Result.Success)
        {
            throw new SpirvCrossException(result, message);
        }
    }
}

/// <summary>
/// The exception class for errors that occur in SPIRV-Cross.
/// </summary>
public sealed class SpirvCrossException : Exception
{
    /// <summary>
    /// Gets the result returned by SPIRV-Cross.
    /// </summary>
    public Result Result { get; }

    /// <summary>
    /// Gets if the result is considered an error.
    /// </summary>
    public bool IsError => Result < 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpirvCrossException" /> class.
    /// </summary>
    public SpirvCrossException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpirvCrossException" /> class.
    /// </summary>
    /// <param name="result">The result code that caused this exception.</param>
    /// <param name="message"></param>
    public SpirvCrossException(Result result, string message = "SPIRV-Cross error occured")
        : base($"[{(int)result}] {result} - {message}")
    {
        Result = result;
    }

    public SpirvCrossException(string message)
        : base(message)
    {
    }

    public SpirvCrossException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

}
#endregion

#region API 

internal static unsafe class SpirvCrossApi
{
    public static event DllImportResolver? ResolveLibrary;

    private const string LibName = "spirv-cross-c-shared";

    static SpirvCrossApi()
    {
        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), OnDllImport);
    }

    private static nint OnDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (TryResolveLibrary(libraryName, assembly, searchPath, out nint nativeLibrary))
        {
            return nativeLibrary;
        }

        if (libraryName.Equals(LibName) && TryResolveSpirvCross(assembly, searchPath, out nativeLibrary))
        {
            return nativeLibrary;
        }

        return IntPtr.Zero;
    }

    private static bool TryResolveSpirvCross(Assembly assembly, DllImportSearchPath? searchPath, out IntPtr nativeLibrary)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (NativeLibrary.TryLoad("spirv-cross-c-shared.dll", assembly, searchPath, out nativeLibrary))
            {
                return true;
            }
        }
        else
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (NativeLibrary.TryLoad("libspirv-cross-c-shared.so", assembly, searchPath, out nativeLibrary))
                {
                    return true;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (NativeLibrary.TryLoad("libspirv-cross-c-shared.dylib\"", assembly, searchPath, out nativeLibrary))
                {
                    return true;
                }
            }

            if (NativeLibrary.TryLoad("libspirv-cross-c-shared", assembly, searchPath, out nativeLibrary))
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath, out nint nativeLibrary)
    {
        var resolveLibrary = ResolveLibrary;

        if (resolveLibrary != null)
        {
            var resolvers = resolveLibrary.GetInvocationList();

            foreach (DllImportResolver resolver in resolvers)
            {
                nativeLibrary = resolver(libraryName, assembly, searchPath);

                if (nativeLibrary != 0)
                {
                    return true;
                }
            }
        }

        nativeLibrary = 0;
        return false;
    }

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void spvc_get_version(out uint major, out uint minor, out uint patch);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_context_create(out IntPtr context);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void spvc_context_destroy(IntPtr context);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern sbyte* spvc_context_get_last_error_string(IntPtr context);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void spvc_context_release_allocations(IntPtr context);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void spvc_context_set_error_callback(IntPtr context, delegate* unmanaged[Cdecl]<IntPtr, sbyte*, void> callback, IntPtr userData);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_context_parse_spirv(IntPtr context, uint* spirv, nuint word_count, out SpvcParsedIr parsed_ir);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_context_create_compiler(IntPtr context, Backend backend, SpvcParsedIr parsedIr, CaptureMode mode, out IntPtr compiler);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint spvc_compiler_get_current_id_bound(IntPtr context);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_create_compiler_options(IntPtr context, out IntPtr options);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_options_set_bool(IntPtr options, CompilerOption option, byte value);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_options_set_uint(IntPtr options, CompilerOption option, uint value);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_install_compiler_options(IntPtr compiler, IntPtr options);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_compile(IntPtr context, sbyte* source);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_add_header_line(IntPtr compiler, byte* source);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_require_extension(IntPtr compiler, byte* source);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_flatten_buffer_block(IntPtr compiler, uint id);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern byte spvc_compiler_variable_is_depth_or_compare(IntPtr compiler, uint id);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_mask_stage_output_by_location(IntPtr compiler, uint location, uint component);

    [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
    public static extern Result spvc_compiler_mask_stage_output_by_builtin(IntPtr compiler, SpvBuiltIn builtin);

    public static ReadOnlySpan<byte> GetUtf8(string str)
    {
        int maxLength = Encoding.UTF8.GetByteCount(str);
        var bytes = new byte[maxLength + 1];

        var length = Encoding.UTF8.GetBytes(str, bytes);
        return bytes.AsSpan(0, length);
    }
}



#endregion


public readonly partial struct SpvcParsedIr : IEquatable<SpvcParsedIr>
{
    public SpvcParsedIr(nint handle) { Handle = handle; }
    public nint Handle { get; }
    public bool IsNull => Handle == 0;
    public static SpvcParsedIr Null => new(0);
    public static implicit operator SpvcParsedIr(nint handle) => new(handle);
    public static bool operator ==(SpvcParsedIr left, SpvcParsedIr right) => left.Handle == right.Handle;
    public static bool operator !=(SpvcParsedIr left, SpvcParsedIr right) => left.Handle != right.Handle;
    public static bool operator ==(SpvcParsedIr left, nint right) => left.Handle == right;
    public static bool operator !=(SpvcParsedIr left, nint right) => left.Handle != right;
    public bool Equals(SpvcParsedIr other) => Handle == other.Handle;
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SpvcParsedIr handle && Equals(handle);
    /// <inheritdoc/>
    public override int GetHashCode() => Handle.GetHashCode();
    private string DebuggerDisplay => string.Format("SpvcParsedIr [0x{0}]", Handle.ToString("X"));
}


public sealed unsafe class Options
{
    internal readonly nint Handle;

    public Options(IntPtr handle)
    {
        Handle = handle;
    }

    public void SetBool(CompilerOption option, bool value)
    {
        spvc_compiler_options_set_bool(Handle, option, value ? (byte)1 : (byte)0).CheckResult();
    }

    public void SetUInt(CompilerOption option, uint value)
    {
        spvc_compiler_options_set_uint(Handle, option, value).CheckResult();
    }
}


public sealed unsafe class Context : IDisposable
{
    private readonly nint _handle;

    public Context()
    {
        spvc_context_create(out _handle).CheckResult("Cannot create SPIRV-Cross context");
        spvc_context_set_error_callback(_handle, &OnErrorCallback, IntPtr.Zero);
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="Context" /> class.
    /// </summary>
    ~Context() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_handle == 0)
            return;

        if (disposing)
        {
            ReleaseAllocations();
            spvc_context_destroy(_handle);
        }
    }

    public void ReleaseAllocations() => spvc_context_release_allocations(_handle);

    public static void GetVersion(out uint major, out uint minor, out uint patch) => spvc_get_version(out major, out minor, out patch);

    public string GetLastErrorString()
    {
        return new string(spvc_context_get_last_error_string(_handle));
    }

    public Result ParseSpirv(byte[] bytecode, out SpvcParsedIr parsed_ir)
    {
        fixed (byte* bytecodePtr = bytecode)
        {
            return spvc_context_parse_spirv(_handle,
                (uint*)bytecodePtr,
                (nuint)bytecode.Length / sizeof(uint),
                out parsed_ir);
        }
    }

    public Result ParseSpirv(ReadOnlySpan<byte> bytecode, out SpvcParsedIr parsed_ir)
    {
        return spvc_context_parse_spirv(_handle,
            (uint*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(bytecode)),
            (nuint)bytecode.Length / sizeof(uint),
            out parsed_ir);
    }

    public Result ParseSpirv(uint[] spirv, out SpvcParsedIr parsed_ir)
    {
        fixed (uint* spirvPtr = spirv)
        {
            return spvc_context_parse_spirv(_handle, spirvPtr, (nuint)spirv.Length, out parsed_ir);
        }
    }

    public Result ParseSpirv(uint* spirv, nuint wordCount, out SpvcParsedIr parsed_ir)
    {
        return spvc_context_parse_spirv(_handle, spirv, wordCount, out parsed_ir);
    }

    public Compiler CreateCompiler(Backend backend, in SpvcParsedIr parsedIr, CaptureMode captureMode = CaptureMode.TakeOwnership)
    {
        spvc_context_create_compiler(_handle, backend, parsedIr, captureMode, out IntPtr compiler).CheckResult();
        return new Compiler(compiler);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
    private static unsafe void OnErrorCallback(IntPtr userData, sbyte* errorPtr)
    {
    }
}


public sealed unsafe class Compiler
{
    private readonly nint _handle;

    public Compiler(IntPtr handle)
    {
        _handle = handle;

        spvc_compiler_create_compiler_options(_handle, out IntPtr optionsPtr).CheckResult();
        Options = new(optionsPtr);
    }

    public Options Options { get; }

    public uint CurrentIdBound => spvc_compiler_get_current_id_bound(_handle);

    public void Apply()
    {
        spvc_compiler_install_compiler_options(_handle, Options.Handle).CheckResult();
    }

    public string Compile()
    {
        sbyte* utf8Str = default;
        spvc_compiler_compile(_handle, (sbyte*)&utf8Str).CheckResult();
        return new string(utf8Str);
    }

    public Result AddHeaderLine(string text)
    {
        fixed (byte* dataPtr = GetUtf8(text))
        {
            return spvc_compiler_add_header_line(_handle, dataPtr);
        }
    }

    public Result RequireExtension(string text)
    {
        fixed (byte* dataPtr = GetUtf8(text))
        {
            return spvc_compiler_require_extension(_handle, dataPtr);
        }
    }

    public Result FlattenBufferBlock(uint variableId)
    {
        return spvc_compiler_flatten_buffer_block(_handle, variableId);
    }

    public bool VariableIsDepthOrCompare(uint variableId)
    {
        return spvc_compiler_variable_is_depth_or_compare(_handle, variableId) == 1;
    }

    public Result MaskStageOutputByLocation(uint location, uint component)
    {
        return spvc_compiler_mask_stage_output_by_location(_handle, location, component);
    }

    public Result MaskStageOutputByBuiltIn(SpvBuiltIn builtin)
    {
        return spvc_compiler_mask_stage_output_by_builtin(_handle, builtin);
    }
}




#region ENums


public enum CaptureMode
{
    Copy = 0,
    TakeOwnership = 1,
}

public enum Backend
{
    None = 0,
    GLSL = 1,
    HLSL = 2,
    MSL = 3,
    CPP = 4,
    JSON = 5,
}

public enum Result 
{
    Success = 0,
	ErrorInvalidSPIRV = -1,
    ErrorUnsupportedSPIRV = -2,
	ErrorOutOfMemory = -3,
	ErrorInvalidArgumetn = -4
}

public enum CompilerOption
{
    Unknown = 0,
    ForceTemporary = 1 | 0x1000000,
    FLATTEN_MULTIDIMENSIONAL_ARRAYS = 16777218,
    FIXUP_DEPTH_CONVENTION = 16777219,
    FLIP_VERTEX_Y = 16777220,
    GLSL_SUPPORT_NONZERO_BASE_INSTANCE = 33554437,
    GLSL_SEPARATE_SHADER_OBJECTS = 33554438,
    GLSL_ENABLE_420PACK_EXTENSION = 33554439,
    GLSL_Version = 33554440,
    GLSL_ES = 33554441,
    GLSL_VulkanSemantics = 33554442,
    GLSL_ES_DEFAULT_FLOAT_PRECISION_HIGHP = 33554443,
    GLSL_ES_DEFAULT_INT_PRECISION_HIGHP = 33554444,
    HLSLShaderModel = 67108877,
    HLSL_POINT_SIZE_COMPAT = 67108878,
    HLSL_POINT_COORD_COMPAT = 67108879,
    HLSL_SUPPORT_NONZERO_BASE_VERTEX_BASE_INSTANCE = 67108880,
    MSL_VERSION = 134217745,
    MSL_TEXEL_BUFFER_TEXTURE_WIDTH = 134217746,
    MSL_AUX_BUFFER_INDEX = 134217747,
    MSL_SWIZZLE_BUFFER_INDEX = 134217747,
    MSL_INDIRECT_PARAMS_BUFFER_INDEX = 134217748,
    MSL_SHADER_OUTPUT_BUFFER_INDEX = 134217749,
    MSL_SHADER_PATCH_OUTPUT_BUFFER_INDEX = 134217750,
    MSL_SHADER_TESS_FACTOR_OUTPUT_BUFFER_INDEX = 134217751,
    MSL_SHADER_INPUT_WORKGROUP_INDEX = 134217752,
    MSL_ENABLE_POINT_SIZE_BUILTIN = 134217753,
    MSL_DISABLE_RASTERIZATION = 134217754,
    MSL_CAPTURE_OUTPUT_TO_BUFFER = 134217755,
    MSL_SWIZZLE_TEXTURE_SAMPLES = 134217756,
    MSL_PAD_FRAGMENT_OUTPUT_COMPONENTS = 134217757,
    MSL_TESS_DOMAIN_ORIGIN_LOWER_LEFT = 134217758,
    MSL_PLATFORM = 134217759,
    MSL_ARGUMENT_BUFFERS = 134217760,
    GLSL_EMIT_PUSH_CONSTANT_AS_UNIFORM_BUFFER = 33554465,
    MSL_TEXTURE_BUFFER_NATIVE = 134217762,
    GLSL_EMIT_UNIFORM_BUFFER_AS_PLAIN_UNIFORMS = 33554467,
    MSL_BUFFER_SIZE_BUFFER_INDEX = 134217764,
    EMIT_LINE_DIRECTIVES = 16777253,
    MSL_MULTIVIEW = 134217766,
    MSL_VIEW_MASK_BUFFER_INDEX = 134217767,
    MSL_DEVICE_INDEX = 134217768,
    MSL_VIEW_INDEX_FROM_DEVICE_INDEX = 134217769,
    MSL_DISPATCH_BASE = 134217770,
    MSL_DYNAMIC_OFFSETS_BUFFER_INDEX = 134217771,
    MSL_TEXTURE_1D_AS_2D = 134217772,
    MSL_ENABLE_BASE_INDEX_ZERO = 134217773,
    MSL_IOS_FRAMEBUFFER_FETCH_SUBPASS = 134217774,
    MSL_FRAMEBUFFER_FETCH_SUBPASS = 134217774,
    MSL_INVARIANT_FP_MATH = 134217775,
    MSL_EMULATE_CUBEMAP_ARRAY = 134217776,
    MSL_ENABLE_DECORATION_BINDING = 134217777,
    MSL_FORCE_ACTIVE_ARGUMENT_BUFFER_RESOURCES = 134217778,
    MSL_FORCE_NATIVE_ARRAYS = 134217779,
    ENABLE_STORAGE_IMAGE_QUALIFIER_DEDUCTION = 16777268,
    HLSL_FORCE_STORAGE_BUFFER_AS_UAV = 67108917,
    FORCE_ZERO_INITIALIZED_VARIABLES = 16777270,
    HLSL_NONWRITABLE_UAV_TEXTURE_AS_SRV = 67108919,
    MSL_ENABLE_FRAG_OUTPUT_MASK = 134217784,
    MSL_ENABLE_FRAG_DEPTH_BUILTIN = 134217785,
    MSL_ENABLE_FRAG_STENCIL_REF_BUILTIN = 134217786,
    MSL_ENABLE_CLIP_DISTANCE_USER_VARYING = 134217787,
    HLSL_ENABLE_16BIT_TYPES = 67108924,
    MSL_MULTI_PATCH_WORKGROUP = 134217789,
    MSL_SHADER_INPUT_BUFFER_INDEX = 134217790,
    MSL_SHADER_INDEX_BUFFER_INDEX = 134217791,
    MSL_VERTEX_FOR_TESSELLATION = 134217792,
    MSL_VERTEX_INDEX_TYPE = 134217793,
    GLSL_FORCE_FLATTENED_IO_BLOCKS = 33554498,
    MSL_MULTIVIEW_LAYERED_RENDERING = 134217795,
    MSL_ARRAYED_SUBPASS_INPUT = 134217796,
    MSL_R32UI_LINEAR_TEXTURE_ALIGNMENT = 134217797,
    MSL_R32UI_ALIGNMENT_CONSTANT_ID = 134217798,
    HLSL_FLATTEN_MATRIX_VERTEX_INPUT_SEMANTICS = 67108935,
    MSL_IOS_USE_SIMDGROUP_FUNCTIONS = 134217800,
    MSL_EMULATE_SUBGROUPS = 134217801,
    MSL_FIXED_SUBGROUP_SIZE = 134217802,
    MSL_FORCE_SAMPLE_RATE_SHADING = 134217803,
    MSL_IOS_SUPPORT_BASE_VERTEX_INSTANCE = 134217804,
    GLSL_OVR_MULTIVIEW_VIEW_COUNT = 33554509,
    RELAX_NAN_CHECKS = 16777294,
}

public enum SpvBuiltIn
{
    Position = 0,
    PointSize = 1,
    ClipDistance = 3,
    CullDistance = 4,
    VertexId = 5,
    InstanceId = 6,
    PrimitiveId = 7,
    InvocationId = 8,
    Layer = 9,
    ViewportIndex = 10,
    TessLevelOuter = 11,
    TessLevelInner = 12,
    TessCoord = 13,
    PatchVertices = 14,
    FragCoord = 15,
    PointCoord = 16,
    FrontFacing = 17,
    SampleId = 18,
    SamplePosition = 19,
    SampleMask = 20,
    FragDepth = 22,
    HelperInvocation = 23,
    NumWorkgroups = 24,
    WorkgroupSize = 25,
    WorkgroupId = 26,
    LocalInvocationId = 27,
    GlobalInvocationId = 28,
    LocalInvocationIndex = 29,
    WorkDim = 30,
    GlobalSize = 31,
    EnqueuedWorkgroupSize = 32,
    GlobalOffset = 33,
    GlobalLinearId = 34,
    SubgroupSize = 36,
    SubgroupMaxSize = 37,
    NumSubgroups = 38,
    NumEnqueuedSubgroups = 39,
    SubgroupId = 40,
    SubgroupLocalInvocationId = 41,
    VertexIndex = 42,
    InstanceIndex = 43,
    SubgroupEqMask = 4416,
    SubgroupEqMaskKHR = 4416,
    SubgroupGeMask = 4417,
    SubgroupGeMaskKHR = 4417,
    SubgroupGtMask = 4418,
    SubgroupGtMaskKHR = 4418,
    SubgroupLeMask = 4419,
    SubgroupLeMaskKHR = 4419,
    SubgroupLtMask = 4420,
    SubgroupLtMaskKHR = 4420,
    BaseVertex = 4424,
    BaseInstance = 4425,
    DrawIndex = 4426,
    PrimitiveShadingRateKHR = 4432,
    DeviceIndex = 4438,
    ViewIndex = 4440,
    ShadingRateKHR = 4444,
    BaryCoordNoPerspAMD = 4992,
    BaryCoordNoPerspCentroidAMD = 4993,
    BaryCoordNoPerspSampleAMD = 4994,
    BaryCoordSmoothAMD = 4995,
    BaryCoordSmoothCentroidAMD = 4996,
    BaryCoordSmoothSampleAMD = 4997,
    BaryCoordPullModelAMD = 4998,
    FragStencilRefEXT = 5014,
    ViewportMaskNV = 5253,
    SecondaryPositionNV = 5257,
    SecondaryViewportMaskNV = 5258,
    PositionPerViewNV = 5261,
    ViewportMaskPerViewNV = 5262,
    FullyCoveredEXT = 5264,
    TaskCountNV = 5274,
    PrimitiveCountNV = 5275,
    PrimitiveIndicesNV = 5276,
    ClipDistancePerViewNV = 5277,
    CullDistancePerViewNV = 5278,
    LayerPerViewNV = 5279,
    MeshViewCountNV = 5280,
    MeshViewIndicesNV = 5281,
    BaryCoordKHR = 5286,
    BaryCoordNV = 5286,
    BaryCoordNoPerspKHR = 5287,
    BaryCoordNoPerspNV = 5287,
    FragSizeEXT = 5292,
    FragmentSizeNV = 5292,
    FragInvocationCountEXT = 5293,
    InvocationsPerPixelNV = 5293,
    LaunchIdKHR = 5319,
    LaunchIdNV = 5319,
    LaunchSizeKHR = 5320,
    LaunchSizeNV = 5320,
    WorldRayOriginKHR = 5321,
    WorldRayOriginNV = 5321,
    WorldRayDirectionKHR = 5322,
    WorldRayDirectionNV = 5322,
    ObjectRayOriginKHR = 5323,
    ObjectRayOriginNV = 5323,
    ObjectRayDirectionKHR = 5324,
    ObjectRayDirectionNV = 5324,
    RayTminKHR = 5325,
    RayTminNV = 5325,
    RayTmaxKHR = 5326,
    RayTmaxNV = 5326,
    InstanceCustomIndexKHR = 5327,
    InstanceCustomIndexNV = 5327,
    ObjectToWorldKHR = 5330,
    ObjectToWorldNV = 5330,
    WorldToObjectKHR = 5331,
    WorldToObjectNV = 5331,
    HitTNV = 5332,
    HitKindKHR = 5333,
    HitKindNV = 5333,
    CurrentRayTimeNV = 5334,
    IncomingRayFlagsKHR = 5351,
    IncomingRayFlagsNV = 5351,
    RayGeometryIndexKHR = 5352,
    WarpsPerSMNV = 5374,
    SMCountNV = 5375,
    WarpIDNV = 5376,
    SMIDNV = 5377,
    CullMaskKHR = 6021,
}



#endregion