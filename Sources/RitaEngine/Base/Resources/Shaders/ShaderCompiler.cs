
namespace RitaEngine.Base.Resources.Shader.ShaderCompiler;


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RitaEngine.Platform;
// using RitaEngine.Device.Implement;
using static RitaEngine.Base.Resources.Shader.ShaderCompiler.Natives;


#pragma warning disable IDE1006, CS1591
public interface IIncluder
{
    void Activate(Options options);
    void Dispose(Options options);
}

public unsafe class Includer : IIncluder
{
    public string RootPath;

    private GCHandle _includerGCHandle = new();
    private Dictionary<string, IntPtr> _shadercIncludeResults = new();
    private Dictionary<IntPtr, string> _ptrToName = new();
    private Dictionary<string, string> _sourceToPath = new();

    public Includer(string rootPath = ".")
    {
        RootPath = rootPath;
    }

    public void Activate(Options options)
    {
        if (!_includerGCHandle.IsAllocated)
            _includerGCHandle = GCHandle.Alloc(this);
        shaderc_compile_options_set_include_callbacks(options.Handle, Shaderc_include_resolve, Shaderc_include_result_release, (void*)GCHandle.ToIntPtr(_includerGCHandle));
    }

    public  void Dispose(Options options)
    {
#pragma warning disable CS8600, CS8625
        shaderc_compile_options_set_include_callbacks(options.Handle, null, null, null);
#pragma warning restore CS8600, CS8625
        foreach (var includeResultPtr in _shadercIncludeResults.Values)
            Free(includeResultPtr);
        _sourceToPath = new();
        _ptrToName = new();
        _shadercIncludeResults = new();
        if (_includerGCHandle.IsAllocated)
            _includerGCHandle.Free();
    }

    private static nint Shaderc_include_resolve(void* user_data, [MarshalAs(UnmanagedType.LPStr)] string requested_source, int type, [MarshalAs(UnmanagedType.LPStr)] string requesting_source, UIntPtr include_depth)
    {
        GCHandle gch = GCHandle.FromIntPtr((IntPtr)user_data);
#pragma warning disable CS8600
        Includer includer = (Includer)gch.Target;
#pragma warning restore CS8600

#pragma warning disable CS8602
        if (!includer._shadercIncludeResults.TryGetValue(requested_source, out IntPtr includeResultPtr))
#pragma warning restore CS8602
        {
            shaderc_include_result includeResult = new();
            string path = requested_source;
            if (!Path.IsPathRooted(path))
            {
                string? rootPath = includer.RootPath;
                if (includer._sourceToPath.ContainsKey(requesting_source)) {
                    rootPath = Path.GetDirectoryName(includer._sourceToPath[requesting_source]);
                }
                path = Path.Combine(rootPath?? "", path);
            }
            includeResult.content = System.IO.File.ReadAllText(path);
            includeResult.content_length = (UIntPtr)includeResult.content.Length;
            includeResult.source_name = requested_source;
            includeResult.source_name_length = (UIntPtr)includeResult.source_name.Length;

            includeResultPtr = Marshal.AllocHGlobal(Marshal.SizeOf(includeResult));
            Marshal.StructureToPtr(includeResult, includeResultPtr, false);
            includer._shadercIncludeResults.Add(requested_source, includeResultPtr);
            includer._ptrToName.Add(includeResultPtr, requested_source);
            includer._sourceToPath.Add(requested_source, path);
        }
        return includeResultPtr;
    }

    private static void Shaderc_include_result_release(void* user_data, nint include_result)
    {
        GCHandle gch = GCHandle.FromIntPtr((IntPtr)user_data);
#pragma warning disable CS8600
        Includer includer = (Includer)gch.Target;
#pragma warning restore CS8600

#pragma warning disable CS8602
        if (includer._ptrToName.TryGetValue(include_result, out var path))
#pragma warning restore CS8602
        {
            includer._ptrToName.Remove(include_result);
            includer._shadercIncludeResults.Remove(path);
            Free(include_result);
        }
    }

    private static void Free(IntPtr includeResultPtr)
    {
        Marshal.DestroyStructure<shaderc_include_result>(includeResultPtr);
        // Marshal.DestroyStructure(includeResultPtr, typeof(shaderc_include_result));// Bug with AOT compil
        Marshal.FreeHGlobal(includeResultPtr);
    }
}

public class Compiler : IDisposable
{
    private readonly nint _handle;

    private readonly IntPtr _dll;

    // private const string dllName = "shaderc_shared.dll" ;
#if WIN64
    private const string dllName = "shaderc_shared.dll" ;
#else
    //     else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    //         return Libraries.Load("libshaderc_shared.so");
    //     else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    //         return Libraries.Load("libshaderc_shared.dylib");
    //         return Libraries.Load("shaderc_shared");
#endif

    public Compiler(string absolutPathForDLL, Options? options = null)
    {
        var file = System.IO.Path.Combine(absolutPathForDLL,  dllName);
        _dll = Libraries.Load( file );

        // if (  _dll == IntPtr.Zero ){
        //    var  ass =Assembly.GetExecutingAssembly();
        //     _dll = NativeLibrary.Load(dllName,ass ,DllImportSearchPath.UserDirectories);
        // } 
        // throw new Exception( $"Failed to load : {file}");
        // Guard.ThrowIf( _dll == IntPtr.Zero);
        Load(_dll);

        _handle = shaderc_compiler_initialize();

        // Guard.ThrowIf( _handle==0,"Cannot initialize native handle");
        Options = options ?? new Options();
    }

    public Options Options { get; }
    public IIncluder? Includer;

    /// <summary> Finalizes an instance of the <see cref="Compiler" /> class. </summary>
    ~Compiler() =>Release();

    public void Dispose()
    {
        Release();
        GC.SuppressFinalize(this);
    }

    public void Release()
    {
        if (_disposed) return ;
        
        if (_handle == 0) return;

        Includer?.Dispose(Options);
        Options.Dispose();
        shaderc_compiler_release(_handle);
        Libraries.Unload(_dll);
    }
    private readonly bool _disposed=false;

    public Result Compile(string source, string fileName, ShaderKind shaderKind, string entryPoint = "main")
    {
        Includer?.Activate(Options);
        return new Result(shaderc_compile_into_spv(_handle, source, (nuint)source.Length, (byte)shaderKind, fileName, entryPoint, Options.Handle));
    }

    public static void GetSpvVersion(out uint version, out uint revision) => shaderc_get_spv_version(out version, out revision);
}

public class Result : IDisposable
{
    private readonly nint _handle;

    internal Result(nint handle)  {         _handle = handle;    }

    public CompilationStatus Status => shaderc_result_get_compilation_status(_handle);

    public nuint Length => shaderc_result_get_length(_handle);
    public uint WarningsCount => (uint)shaderc_result_get_num_warnings(_handle);
    public uint ErrorsCount => (uint)shaderc_result_get_num_errors(_handle);

    /// <summary>
    /// Returns a null-terminated string that contains any error messages generated
    /// during the compilation.
    /// </summary>
    public string? ErrorMessage => Marshal.PtrToStringAnsi(shaderc_result_get_error_message(_handle));

    public unsafe byte* GetBytes() => shaderc_result_get_bytes(_handle);

    public unsafe Span<byte> GetBytecode() =>  new(shaderc_result_get_bytes(_handle), (int)shaderc_result_get_length(_handle));
    
    /// <summary>
    /// Finalizes an instance of the <see cref="Result" /> class.
    /// </summary>
    ~Result() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_handle == 0) return;

        if (disposing)
            shaderc_result_release(_handle);
    }
}

public class Options : IDisposable
{
    private readonly nint _handle;

    public Options() : this(shaderc_compile_options_initialize())
    {
    }

    private Options(nint handle)
    {
        _handle = (handle !=0 )? handle : throw new Exception("Cannot initialize native handle") ;
    }

    public IntPtr Handle => _handle;

    /// <summary>
    /// Finalizes an instance of the <see cref="Options" /> class.
    /// </summary>
    ~Options() => Dispose(disposing: false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_handle == 0) return;

        if (disposing)
            shaderc_compile_options_release(_handle);
    }

    public Options Clone() => new(shaderc_compile_options_clone(_handle));
    
    public void AddMacroDefinition(string name, string? value = null)
    {
        shaderc_compile_options_add_macro_definition(_handle,
            name,
            (nuint)name.Length,
            value, string.IsNullOrEmpty(value) ? 0 : (nuint)value!.Length);
    }

    public void SetSourceLanguage(SourceLanguage language)
        =>   shaderc_compile_options_set_source_language(_handle, language);
    
    public void SetGenerateDebugInfo()
        => shaderc_compile_options_set_generate_debug_info(_handle);
    
    public void SetOptimizationLevel(OptimizationLevel level)
        =>  shaderc_compile_options_set_optimization_level(_handle, level);
    
    public void SetForcedVersionPofile(int version, Profile profile)
        => shaderc_compile_options_set_forced_version_profile(_handle, version, profile);
    
    public void SetSuppressWarnings()
        => shaderc_compile_options_set_suppress_warnings(_handle);
    
    public void SetTargetEnv(TargetEnvironment target, uint version)
        => shaderc_compile_options_set_target_env(_handle, target, version);
    
    public void SetargetSpirv(SpirVVersion version)
        => shaderc_compile_options_set_target_spirv(_handle, version);
    
    public void SetWarningsAsErrors() => shaderc_compile_options_set_warnings_as_errors(_handle);

    public void SetLimit(Limit limit, int value) => shaderc_compile_options_set_limit(_handle, limit, value);

    public void SetAutoBindUniforms(bool value) => shaderc_compile_options_set_auto_bind_uniforms(_handle, value);

    public void SetHLSLIoMapping(bool value) => shaderc_compile_options_set_hlsl_io_mapping(_handle, value);

    public void SetHLSLOffsets(bool value) => shaderc_compile_options_set_hlsl_offsets(_handle, value);

    public void SetBindingBase(UniformKind kind, uint @base) => shaderc_compile_options_set_binding_base(_handle, kind, @base);

    public void SetBindingBaseForStage(ShaderKind shaderKind, UniformKind kind, uint @base)
        => shaderc_compile_options_set_binding_base_for_stage(_handle, shaderKind, kind, @base);
    
    public void SetAutoMapLocations(bool value) => shaderc_compile_options_set_auto_map_locations(_handle, value);

    public void SetHLSLRegisterSetAndBindingForStage(ShaderKind shaderKind, string reg, string set, string binding)
        => shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(_handle, shaderKind, reg, set, binding);

    public void SetHLSLRegisterSetAndBinding(string reg, string set, string binding)
        => shaderc_compile_options_set_hlsl_register_set_and_binding(_handle, reg, set, binding);

    public void SetHLSLFunctionality1(bool enable) => shaderc_compile_options_set_hlsl_functionality1(_handle, enable);

    /// <summary>
    /// Sets whether the compiler should invert position.Y output in vertex shader.
    /// </summary>
    /// <param name="enable"></param>
    public void SetInvertY(bool enable) => shaderc_compile_options_set_invert_y(_handle, enable);

    public void SetNaNClamp(bool enable) => shaderc_compile_options_set_nan_clamp(_handle, enable);
    
}

internal static unsafe class Natives
{

    public static void Load( IntPtr dll)
    {
        // Libraries.GetFunction<
        shaderc_compiler_initialize_ = Libraries.GetFunction<PFN_InitializeFunc>( dll, nameof(shaderc_compiler_initialize));
        shaderc_compiler_release_ = Libraries.GetFunction<PFN_ReleaseFunc>( dll, nameof(shaderc_compiler_release));
        shaderc_compile_options_initialize_ = Libraries.GetFunction<PFN_InitializeFunc>( dll, nameof(shaderc_compile_options_initialize));
        shaderc_compile_options_clone_ = Libraries.GetFunction<PFN_CloneFunc>( dll, nameof(shaderc_compile_options_clone));

        shaderc_compile_options_release_ = Libraries.GetFunction<PFN_ReleaseFunc>( dll, nameof(shaderc_compile_options_release));
        shaderc_compile_options_clone_ = Libraries.GetFunction<PFN_CloneFunc>( dll, nameof(shaderc_compile_options_clone));
        shaderc_compile_options_add_macro_definition_ = Libraries.GetFunction<PFN_shaderc_compile_options_add_macro_definition>( dll, nameof(shaderc_compile_options_add_macro_definition));
        shaderc_compile_options_set_source_language_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_source_language>( dll, "shaderc_compile_options_set_source_language");
        shaderc_compile_options_set_generate_debug_info_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_generate_debug_info>( dll, "shaderc_compile_options_set_generate_debug_info");
        shaderc_compile_options_set_optimization_level_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_optimization_level>( dll, "shaderc_compile_options_set_optimization_level");
        shaderc_compile_into_spv_ = Libraries.GetFunction<PFN_shaderc_compile_into_spv>( dll, nameof(shaderc_compile_into_spv));
        shaderc_compile_options_set_forced_version_profile_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_forced_version_profile>( dll, "shaderc_compile_options_set_forced_version_profile");
        shaderc_compile_options_set_include_callbacks_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_include_callbacks>( dll, "shaderc_compile_options_set_include_callbacks");
        shaderc_compile_options_set_suppress_warnings_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_suppress_warnings>( dll, "shaderc_compile_options_set_suppress_warnings");
        shaderc_compile_options_set_target_env_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_target_env>( dll, "shaderc_compile_options_set_target_env");
        shaderc_compile_options_set_target_spirv_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_target_spirv>( dll, "shaderc_compile_options_set_target_spirv");
        shaderc_compile_options_set_warnings_as_errors_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_warnings_as_errors>( dll, "shaderc_compile_options_set_warnings_as_errors");
        shaderc_compile_options_set_limit_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_limit>( dll, "shaderc_compile_options_set_limit");
        shaderc_compile_options_set_auto_bind_uniforms_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_auto_bind_uniforms>( dll, "shaderc_compile_options_set_auto_bind_uniforms");
        shaderc_compile_options_set_hlsl_io_mapping_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_hlsl_io_mapping>( dll, "shaderc_compile_options_set_hlsl_io_mapping");
        shaderc_compile_options_set_hlsl_offsets_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_hlsl_offsets>( dll, "shaderc_compile_options_set_hlsl_offsets");
        shaderc_compile_options_set_binding_base_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_binding_base>( dll, "shaderc_compile_options_set_binding_base");
        shaderc_compile_options_set_binding_base_for_stage_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_binding_base_for_stage>( dll, "shaderc_compile_options_set_binding_base_for_stage");
        shaderc_compile_options_set_auto_map_locations_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_auto_map_locations>( dll, "shaderc_compile_options_set_auto_map_locations");
        shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage>( dll, "shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage");
        shaderc_compile_options_set_hlsl_register_set_and_binding_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_hlsl_register_set_and_binding>( dll, "shaderc_compile_options_set_hlsl_register_set_and_binding");
        shaderc_compile_options_set_hlsl_functionality1_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_hlsl_functionality1>( dll, "shaderc_compile_options_set_hlsl_functionality1");
        shaderc_compile_options_set_invert_y_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_invert_y>( dll, "shaderc_compile_options_set_invert_y");
        shaderc_compile_options_set_nan_clamp_ = Libraries.GetFunction<PFN_shaderc_compile_options_set_nan_clamp>( dll, "shaderc_compile_options_set_nan_clamp");
        
        shaderc_result_release_ = Libraries.GetFunction<PFN_ReleaseFunc>( dll, nameof(shaderc_result_release));
        shaderc_result_get_length_ = Libraries.GetFunction<PFN_shaderc_result_get_length>( dll, nameof(shaderc_result_get_length));
        shaderc_result_get_num_warnings_ = Libraries.GetFunction<PFN_shaderc_result_get_num_warnings>( dll, nameof(shaderc_result_get_num_warnings));
        shaderc_result_get_num_errors_ = Libraries.GetFunction<PFN_shaderc_result_get_num_errors>( dll, nameof(shaderc_result_get_num_errors));
        shaderc_result_get_compilation_status_ = Libraries.GetFunction<PFN_shaderc_result_get_compilation_status>( dll, nameof(shaderc_result_get_compilation_status));
        shaderc_result_get_bytes_ = Libraries.GetFunction<PFN_shaderc_result_get_bytes>( dll, nameof(shaderc_result_get_bytes));
        shaderc_result_get_error_message_ = Libraries.GetFunction<PFN_shaderc_result_get_error_message>( dll, nameof(shaderc_result_get_error_message));
        
        shaderc_get_spv_version_ = Libraries.GetFunction<PFN_shaderc_get_spv_version>( dll, nameof(shaderc_get_spv_version));
        shaderc_parse_version_profile_ = Libraries.GetFunction<PFN_shaderc_parse_version_profile>( dll, "shaderc_parse_version_profile");
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]   private delegate nint PFN_InitializeFunc();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]    private delegate void PFN_ReleaseFunc(nint handle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]    private delegate nint PFN_CloneFunc(nint handle);

    private static PFN_InitializeFunc shaderc_compiler_initialize_  =null!;
    public static nint shaderc_compiler_initialize() => shaderc_compiler_initialize_();

    private static PFN_ReleaseFunc shaderc_compiler_release_ =null!;
    public static void shaderc_compiler_release(nint handle) => shaderc_compiler_release_(handle);

    // Options
    private static  PFN_InitializeFunc shaderc_compile_options_initialize_ = null!;
    public static nint shaderc_compile_options_initialize() => shaderc_compile_options_initialize_();

    private static  PFN_CloneFunc shaderc_compile_options_clone_ = null!;
    public static nint shaderc_compile_options_clone(nint handle) => shaderc_compile_options_clone_(handle);

    private static  PFN_ReleaseFunc shaderc_compile_options_release_ = null!;
    public static void shaderc_compile_options_release(nint handle) => shaderc_compile_options_release_(handle);


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_add_macro_definition(nint options, string name, nuint name_length, string? value, nuint value_length);
    private static  PFN_shaderc_compile_options_add_macro_definition shaderc_compile_options_add_macro_definition_ = null!;
    public static void shaderc_compile_options_add_macro_definition(nint options, string name, nuint name_length, string? value, nuint value_length)
        => shaderc_compile_options_add_macro_definition_(options, name, name_length, value, value_length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint PFN_shaderc_compile_options_set_source_language(nint options, SourceLanguage lang);
    private static  PFN_shaderc_compile_options_set_source_language shaderc_compile_options_set_source_language_ = null!;
    public static void shaderc_compile_options_set_source_language(nint options, SourceLanguage lang) => shaderc_compile_options_set_source_language_(options, lang);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint PFN_shaderc_compile_options_set_generate_debug_info(nint options);
    private static  PFN_shaderc_compile_options_set_generate_debug_info shaderc_compile_options_set_generate_debug_info_ = null!;
    public static void shaderc_compile_options_set_generate_debug_info(nint options)
        => shaderc_compile_options_set_generate_debug_info_(options);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint PFN_shaderc_compile_options_set_optimization_level(nint options, OptimizationLevel level);
    private static  PFN_shaderc_compile_options_set_optimization_level shaderc_compile_options_set_optimization_level_ =null!;
    public static void shaderc_compile_options_set_optimization_level(nint options, OptimizationLevel level)
        => shaderc_compile_options_set_optimization_level_(options, level);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private delegate nint PFN_shaderc_compile_into_spv(nint compiler, string source, nuint source_size, int shader_kind, string input_file, string entry_point, nint additional_options);

    private static  PFN_shaderc_compile_into_spv shaderc_compile_into_spv_ = null!;
    public static nint shaderc_compile_into_spv(nint compiler, string source, nuint source_size, int shader_kind, string input_file, string entry_point, nint additional_options)
        => shaderc_compile_into_spv_(compiler, source, source_size, shader_kind, input_file, entry_point, additional_options);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_forced_version_profile(nint options, int version, Profile profile);
    private static  PFN_shaderc_compile_options_set_forced_version_profile shaderc_compile_options_set_forced_version_profile_ = null!;
    public static void shaderc_compile_options_set_forced_version_profile(nint options, int version, Profile profile)
        => shaderc_compile_options_set_forced_version_profile_(options, version, profile);

    /// <summary>An include result. https://github.com/google/shaderc/blob/c42db5815fad0424f0f1311de1eec92cdd77203d/libshaderc/include/shaderc/shaderc.h#L325</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct shaderc_include_result
    {
        /// <summary>
        /// The name of the source file.  The name should be fully resolved
        /// in the sense that it should be a unique name in the context of the
        /// includer.  For example, if the includer maps source names to files in
        /// a filesystem, then this name should be the absolute path of the file.
        /// For a failed inclusion, this string is empty.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)] public string source_name;
        public UIntPtr source_name_length;
        /// <summary>
        /// The text contents of the source file in the normal case.
        /// For a failed inclusion, this contains the error message.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)] public string content;
        public UIntPtr content_length;
        /// <summary>
        /// User data to be passed along with this request.
        /// </summary>
        public void* user_data;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint PFN_shaderc_include_resolve_fn(void* user_data, [MarshalAs(UnmanagedType.LPStr)] string requested_source, int type, [MarshalAs(UnmanagedType.LPStr)] string requesting_source, UIntPtr include_depth);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PFN_shaderc_include_result_release_fn(void* user_data, nint include_result);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_include_callbacks(nint options, PFN_shaderc_include_resolve_fn resolver, PFN_shaderc_include_result_release_fn result_releaser, void* user_data);
    private static  PFN_shaderc_compile_options_set_include_callbacks shaderc_compile_options_set_include_callbacks_ = null!;
    public static void shaderc_compile_options_set_include_callbacks(nint options, PFN_shaderc_include_resolve_fn resolver, PFN_shaderc_include_result_release_fn result_releaser, void* user_data)
    =>shaderc_compile_options_set_include_callbacks_(options, resolver, result_releaser, user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint PFN_shaderc_compile_options_set_suppress_warnings(nint options);
    private static  PFN_shaderc_compile_options_set_suppress_warnings shaderc_compile_options_set_suppress_warnings_ = null!;
    public static void shaderc_compile_options_set_suppress_warnings(nint options) => shaderc_compile_options_set_suppress_warnings_(options);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_target_env(nint options, TargetEnvironment target, uint version);
    private static  PFN_shaderc_compile_options_set_target_env shaderc_compile_options_set_target_env_ = null!;
    public static void shaderc_compile_options_set_target_env(nint options, TargetEnvironment target, uint version)
    => shaderc_compile_options_set_target_env_(options, target, version);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_target_spirv(nint options, SpirVVersion version);
    private static  PFN_shaderc_compile_options_set_target_spirv shaderc_compile_options_set_target_spirv_ = null!;
    public static void shaderc_compile_options_set_target_spirv(nint options, SpirVVersion version)
    => shaderc_compile_options_set_target_spirv_(options, version);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_warnings_as_errors(nint option);
    private static  PFN_shaderc_compile_options_set_warnings_as_errors shaderc_compile_options_set_warnings_as_errors_ = null!;
    public static void shaderc_compile_options_set_warnings_as_errors(nint options)
    => shaderc_compile_options_set_warnings_as_errors_(options);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint PFN_shaderc_compile_options_set_limit(nint options, Limit limit, int value);
    private static  PFN_shaderc_compile_options_set_limit shaderc_compile_options_set_limit_ = null!;
    public static void shaderc_compile_options_set_limit(nint options, Limit limit, int value)
        => shaderc_compile_options_set_limit_(options, limit, value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_auto_bind_uniforms(nint options, bool auto_bind);
    private static  PFN_shaderc_compile_options_set_auto_bind_uniforms shaderc_compile_options_set_auto_bind_uniforms_ = null!;
    public static void shaderc_compile_options_set_auto_bind_uniforms(nint options, bool auto_bind)
        => shaderc_compile_options_set_auto_bind_uniforms_(options, auto_bind);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_hlsl_io_mapping(nint options, bool hlsl_iomap);
    private static  PFN_shaderc_compile_options_set_hlsl_io_mapping shaderc_compile_options_set_hlsl_io_mapping_ = null!;
    public static void shaderc_compile_options_set_hlsl_io_mapping(nint options, bool hlsl_iomap)
        => shaderc_compile_options_set_hlsl_io_mapping_(options, hlsl_iomap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_hlsl_offsets(nint options, bool hlsl_offsets);
    private static  PFN_shaderc_compile_options_set_hlsl_offsets shaderc_compile_options_set_hlsl_offsets_ = null!;
    public static void shaderc_compile_options_set_hlsl_offsets(nint options, bool hlsl_offsets)
        => shaderc_compile_options_set_hlsl_offsets_(options, hlsl_offsets);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_binding_base(nint options, UniformKind kind, uint _base);
    private static  PFN_shaderc_compile_options_set_binding_base shaderc_compile_options_set_binding_base_ = null!;
    public static void shaderc_compile_options_set_binding_base(nint options, UniformKind kind, uint _base)
        => shaderc_compile_options_set_binding_base_(options, kind, _base);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_binding_base_for_stage(nint options, ShaderKind shader_kind, UniformKind kind, uint _base);
    private static  PFN_shaderc_compile_options_set_binding_base_for_stage shaderc_compile_options_set_binding_base_for_stage_ = null!;
    public static void shaderc_compile_options_set_binding_base_for_stage(nint options, ShaderKind shader_kind, UniformKind kind, uint _base)
        => shaderc_compile_options_set_binding_base_for_stage_(options, shader_kind, kind, _base);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_auto_map_locations(nint options, bool auto_map);
    private static  PFN_shaderc_compile_options_set_auto_map_locations shaderc_compile_options_set_auto_map_locations_ = null!;
    public static void shaderc_compile_options_set_auto_map_locations(nint options, bool auto_map)
        => shaderc_compile_options_set_auto_map_locations_(options, auto_map);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(nint options, ShaderKind shader_kind, string reg, string set, string binding);
    private static  PFN_shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage_ = null!;
    public static void shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(nint options, ShaderKind shader_kind, string reg, string set, string binding)
        => shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage_(options, shader_kind, reg, set, binding);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_hlsl_register_set_and_binding(nint options, string reg, string set, string binding);
    private static  PFN_shaderc_compile_options_set_hlsl_register_set_and_binding shaderc_compile_options_set_hlsl_register_set_and_binding_ = null!;
    public static void shaderc_compile_options_set_hlsl_register_set_and_binding(nint options, string reg, string set, string binding)
        => shaderc_compile_options_set_hlsl_register_set_and_binding_(options, reg, set, binding);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_hlsl_functionality1(nint options, bool enable);
    private static  PFN_shaderc_compile_options_set_hlsl_functionality1 shaderc_compile_options_set_hlsl_functionality1_ = null!;
    public static void shaderc_compile_options_set_hlsl_functionality1(nint options, bool enable)
        => shaderc_compile_options_set_hlsl_functionality1_(options, enable);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_invert_y(nint options, bool enable);
    private static  PFN_shaderc_compile_options_set_invert_y shaderc_compile_options_set_invert_y_ =null!;
    public static void shaderc_compile_options_set_invert_y(nint options, bool enable)
        => shaderc_compile_options_set_invert_y_(options, enable);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_compile_options_set_nan_clamp(nint options, bool enable);
    private static  PFN_shaderc_compile_options_set_nan_clamp shaderc_compile_options_set_nan_clamp_ = null!;
    public static void shaderc_compile_options_set_nan_clamp(nint options, bool enable)
        => shaderc_compile_options_set_nan_clamp_(options, enable);

    // Result
    private static  PFN_ReleaseFunc shaderc_result_release_ = null!;
    public static void shaderc_result_release(nint handle) => shaderc_result_release_(handle);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nuint PFN_shaderc_result_get_length(nint result);
    private static  PFN_shaderc_result_get_length shaderc_result_get_length_ = null!;
    public static nuint shaderc_result_get_length(nint result) => shaderc_result_get_length_(result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nuint PFN_shaderc_result_get_num_warnings(nint result);
    private static  PFN_shaderc_result_get_num_warnings shaderc_result_get_num_warnings_ = null!;
    public static nuint shaderc_result_get_num_warnings(nint result) => shaderc_result_get_num_warnings_(result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nuint PFN_shaderc_result_get_num_errors(nint result);
    private static  PFN_shaderc_result_get_num_errors shaderc_result_get_num_errors_ = null!;
    public static nuint shaderc_result_get_num_errors(nint result) => shaderc_result_get_num_errors_(result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate CompilationStatus PFN_shaderc_result_get_compilation_status(nint result);
    private static  PFN_shaderc_result_get_compilation_status shaderc_result_get_compilation_status_ = null!;
    public static CompilationStatus shaderc_result_get_compilation_status(nint result) => shaderc_result_get_compilation_status_(result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate byte* PFN_shaderc_result_get_bytes(nint result);
    private static  PFN_shaderc_result_get_bytes shaderc_result_get_bytes_ = null!;
    public static byte* shaderc_result_get_bytes(nint result) => shaderc_result_get_bytes_(result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate nint PFN_shaderc_result_get_error_message(nint result);
    private static  PFN_shaderc_result_get_error_message shaderc_result_get_error_message_ = null!;
    public static nint shaderc_result_get_error_message(nint result) => shaderc_result_get_error_message_(result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void PFN_shaderc_get_spv_version(out uint version, out uint revision);
    private static  PFN_shaderc_get_spv_version shaderc_get_spv_version_ = null!;
    public static void shaderc_get_spv_version(out uint version, out uint revision) => shaderc_get_spv_version_(out version, out revision);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint PFN_shaderc_parse_version_profile(string str, int* version, Profile* profile);
    private static  PFN_shaderc_parse_version_profile shaderc_parse_version_profile_ = null!;
    public static bool shaderc_parse_version_profile(string str, int* version, Profile* profile) => shaderc_parse_version_profile_(str, version, profile) == 1;

}

public enum CompilationStatus
{
    Success = 0,
    InvalidStage = 1,  // error stage deduction
    compilationError = 2,
    InternalError = 3,  // unexpected failure
    NullResultObject = 4,
    InvalidAssembly = 5,
    ValidationError = 6,
    TransformationError = 7,
    ConfigurationError = 8,
}

public enum Limit
{
    MaxLights,
    MaxClipPlanes,
    MaxTextureUnits,
    MaxTextureCoords,
    MaxVertexAttribs,
    MaxVertexUniformComponents,
    MaxVaryingFloats,
    MaxVertexTextureImageUnits,
    MaxCombinedTextureImageUnits,
    MaxTextureImageUnits,
    MaxFragmentUniformComponents,
    MaxDrawBuffers,
    MaxVertexUniformVectors,
    MaxVaryingVectors,
    MaxFragmentUniformVectors,
    MaxVertexOutputVectors,
    MaxFragmentInputVectors,
    MinProgramTeelOffset,
    MaxProgramTeelOffset,
    MaxClipDistances,
    MaxComputeWorkGroupCountX,
    MaxComputeWorkGroupCountY,
    MaxComputeWorkGroupCountZ,
    MaxComputeWorkGroupSizeX,
    MaxComputeWorkGroupSizeY,
    MaxComputeWorkGroupSizeZ,
    MaxComputeUniformComponents,
    MaxComputeTextureImageUnits,
    MaxComputeImageUniforms,
    MaxComputeAtomicCounters,
    MaxComputeAtomicCounterBuffers,
    MaxVaryingComponents,
    MaxVertexOutputComponents,
    MaxGeometryInputComponents,
    MaxGeometryOutputComponents,
    MaxFragmentInputComponents,
    MaxImageUnits,
    MaxCombinedImageUnitsAndFragmentoutputs,
    MaxCombinedShaderOutputResources,
    MaxImageSamples,
    MaxVertexImageUniforms,
    MaxTessControlImageUniforms,
    MaxTessEvaluationImageUniforms,
    MaxGeometryImageUniforms,
    MaxFragmentImageUniforms,
    MaxCombinedImageUniforms,
    MaxGeometryTextureImageUnits,
    MaxGeometryOutputVertices,
    MaxGeometryTotalOutputComponents,
    MaxGeometryUniformComponents,
    MaxGeometryVaryingComponents,
    MaxTessControlInputComponents,
    MaxTessControlOutputComponents,
    MaxTessControlTextureImageUnits,
    MaxTessControlUniformComponents,
    MaxTessControlTotalOutputComponents,
    MaxTessEvaluationInputComponents,
    MaxTessEvaluationOutputComponents,
    MaxTessEvaluationTextureImageUnits,
    MaxTessEvaluationUniformComponents,
    MaxTessPatchComponents,
    MaxPatchVertices,
    MaxTessGenLevel,
    MaxViewports,
    MaxVertexAtomicCounters,
    MaxTessControlAtomicCounters,
    MaxTessEvaluationAtomicCounters,
    MaxGeometryAtomicCounters,
    MaxFragmentAtomicCounters,
    MaxCombinedAtomicCounters,
    MaxAtomicCounterBindings,
    MaxVertexAtomicCounterBuffers,
    MaxTessControlAtomicCounterBuffers,
    MaxTessEvaluationAtomicCounterBuffers,
    MaxGeometryAtomicCounterBuffers,
    MaxFragmentAtomicCounterBuffers,
    MaxCombinedAtomicCounterBuffers,
    MaxAtomicCounterBufferSize,
    MaxTransformFeedbackBuffers,
    MaxTransformFeedbackInterleavedComponents,
    MaxCullDistances,
    MaxCombinedClipAndCullDistances,
    MaxSamples,
}

public enum OptimizationLevel
{
    /// <summary> No optimization </summary>
    Zero,
    /// <summary> Optimize towards reducing code size </summary>
    Size,
    /// <summary> Optimize towards performance. </summary>
    Performance,
}

public enum Profile
{
    None,
    Core,
    Compatibility,
    Es,
}

public enum ShaderKind
{
    VertexShader,
    FragmentShader,
    ComputeShader,
    GeometryShader,
    TessControlShader,
    TessEvaluationShader,

    /// <summary>
    /// Deduce the shader kind from #pragma annotation in the source code. Compiler will emit error if #pragma annotation is not found.
    /// </summary>
    GLSLInferFromSource,
    // Default shader kinds. Compiler will fall back to compile the source code as
    // the specified kind of shader when #pragma annotation is not found in the
    // source code.
    GLSLDefaultVertexShader,
    GLSLDefaultFragmentShader,
    GLSLDefaultComputeShader,
    GLSLDefaultGeometryShader,
    GLSLDefaultTessControlShader,
    GLSLDefaultTessEvaluationShader,
    SPIRVAssembly,
    RaygenShader,
    AnyhitShader,
    ClosesthitShader,
    MissShader,
    IntersectionShader,
    CallableShader,

    GLSLDefaultRaygenShader,
    GLSLDefaultAnyhitShader,
    GLSLDefaultClosesthitShader,
    GLSLDefaultMissShader,
    GLSLDefaultIntersectionShader,
    GLSLDefaultCallableShader,

    TaskShader,
    MeshShader,
    GLSLDefaultTaskShader,
    GLSLDefaultMeshShader,
}

public enum SourceLanguage
{
    GLSL,
    HLSL
}

public enum SpirVVersion : uint
{
    Version_1_0 = 0x010000u,
    Version_1_1 = 0x010100u,
    Version_1_2 = 0x010200u,
    Version_1_3 = 0x010300u,
    Version_1_4 = 0x010400u,
    Version_1_5 = 0x010500u
}

public enum TargetEnvironment
{
    /// <summary>
    /// SPIR-V under Vulkan semantics.
    /// </summary>
    Vulkan,
    /// <summary>
    /// SPIR-V under OpenGL semantics.
    /// NOTE: SPIR-V code generation is not supported for shaders under OpenGL compatibility profile.
    /// </summary>
    OpenGL,
    /// <summary>
    /// SPIR-V under OpenGL semantics, including compatibility profile functions
    /// </summary>
    OpenGLCompate,
    /// <summary>
    /// Deprecated, SPIR-V under WebGPU semantics.
    /// </summary>
    WebGPU,
    Default = Vulkan
}

public enum UniformKind
{
    /// <summary>
    /// Image and image buffer.
    /// </summary>
    Image,
    /// <summary>
    /// Pure sampler.
    /// </summary>
    Sampler,
    /// <summary>
    /// Sampled texture in GLSL, and Shader Resource View in HLSL.
    /// </summary>
    Texture,
    /// <summary>
    /// Uniform Buffer Object (UBO) in GLSL.  Cbuffer in HLSL.
    /// </summary>
    Buffer,
    /// <summary>
    /// Shader Storage Buffer Object (SSBO) in GLSL.
    /// </summary>
    StorageBuffer,
    /// <summary>
    /// Unordered Access View, in HLSL.  (Writable storage image or storage buffer.)
    /// </summary>
    UnorderedAccessView,
}

#pragma warning restore IDE1006, CS1591

