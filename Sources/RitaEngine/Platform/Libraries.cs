namespace RitaEngine.Platform;

using RitaEngine.Base;

/// <summary>
///  PORTABLE , STAND ALONE
/// Complete USe of MAnage a c++ Dll library  
/// used for WWindows, mac , linux , UWP ,....
/// Add NAtiveLibrary too use NativeLoad , NAtiveFree , NativeGet
///  see : https://github.com/Zekrom64/TesseractEngine
/// </summary>
[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public unsafe static class Libraries
{
    /// <summary>
    /// Work only wih window be careful
    /// </summary>
    /// <param name="libraryName"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static nint LoadEx(string libraryName ,  LoadLibraryFlags flags)
    {
    #if WIN64    
        var result = Win64.LoadLibraryExW(libraryName,nint.Zero,(uint) flags);

        return result;
    #else
    // SetLastError (bool success = false int optionalCode = 123 , string class ="Modules" , string reason="Not yet implemented" );
        return 0;
    #endif       
    }
    
    public static nint LoadNative( string libraryName)=> NativeLibrary.Load(libraryName);

    public static void UnloadNative( nint module) => NativeLibrary.Free(module);

    public static nint GetSymbolNative( nint module, string symbolName)   => NativeLibrary.GetExport(module,symbolName);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static nint Load(string libraryName)
    #if WIN64    
        => Win64.LoadLibrary(libraryName);
    #elif UNIX
        =>Linux.dlopen(libraryName);
    #elif MAC
        =>Mac.dlopen(libraryName);
    #elif UWP
        => UWP.LoadPackagedLibrary(libraryName);
    #else
        => IntPtr.Zero;
    #endif
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static void Unload(IntPtr module)
    #if WIN64    
        => Win64.FreeLibrary(module);
    #elif UNIX
        =>Linux.dlclose(module);
    #elif MAC
        =>Mac.dlclose(module);
    #elif UWP
        => UWP.FreeLibrary(module);
    #else   
        {}
    #endif

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static IntPtr GetSymbol(IntPtr library, string symbolName)
    #if WIN64    
        => Win64.GetProcAddress(library,symbolName);
    #elif UNIX
        =>Linux.dlsym( library,symbolname);
    #elif MAC
        =>Mac.dlsym( library,symbolname);
    #elif UWP
        => UWP.GetProcAddress(library,symbolname);
    #else  
        => IntPtr.Zero;
    #endif    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetFunction<T>(IntPtr library, string symbolName)
        =>Marshal.GetDelegateForFunctionPointer<T>(GetSymbol(library,symbolName));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* GetUnsafeSymbol(IntPtr library, string symbolName)
        =>GetSymbol(library,symbolName).ToPointer();

    [Flags] public enum LoadLibraryFlags : uint
    {
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
        LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
        LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
        LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
        LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008,
        LOAD_LIBRARY_REQUIRE_SIGNED_TARGET = 0x00000080,
        LOAD_LIBRARY_SAFE_CURRENT_DIRS = 0x00002000,
    }

    [SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = 4)]
    public unsafe static class Win64
    {
        private const string kernel32dllName = "kernel32.dll";

        [SuppressUnmanagedCodeSecurity][DllImport(kernel32dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern nint LoadLibraryExW(string lpFileName, nint hfile,uint flags );
    

        [SuppressUnmanagedCodeSecurity][DllImport(kernel32dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern nint LoadLibrary(string lpFileName);

        [SuppressUnmanagedCodeSecurity][DllImport(kernel32dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern nint GetProcAddress(nint hModule, string lpProcName);

        [SuppressUnmanagedCodeSecurity][DllImport(kernel32dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern void FreeLibrary(nint hModule);
    }

    [SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = 4)]
    public unsafe static class Linux
    {
        public const string SystemLibrary = "libdl.so";

        private const int RTLD_LAZY = 1;
        private const int RTLD_NOW = 2;

        public static IntPtr dlopen(string path, bool lazy = true) =>
            dlopen(path, lazy ? RTLD_LAZY : RTLD_NOW);

        [DllImport(SystemLibrary, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlopen(string path, int mode);

        [DllImport(SystemLibrary, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport(SystemLibrary)]
        public static extern void dlclose(IntPtr handle);
    }

    [SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = 4)]
    public unsafe static class Mac
    {
        public const string SystemLibrary = "/usr/lib/libSystem.dylib";

        private const int RTLD_LAZY = 1;
        private const int RTLD_NOW = 2;

        public static IntPtr dlopen(string path, bool lazy = true) =>
            dlopen(path, lazy ? RTLD_LAZY : RTLD_NOW);

        [DllImport(SystemLibrary, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlopen(string path, int mode);

        [DllImport(SystemLibrary, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlsym(IntPtr handle, string symbol);

        [DllImport(SystemLibrary)]
        public static extern void dlclose(IntPtr handle);
    }

    [SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = 4)]
    public unsafe static class UWP
    {
        public const string dllName = "api-ms-win-core-libraryloader-l1-2-0.dll";

        [SuppressUnmanagedCodeSecurity][DllImport(dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadPackagedLibrary([MarshalAs(UnmanagedType.LPWStr)] string libraryName, int reserved = 0);
        
        [SuppressUnmanagedCodeSecurity][DllImport(dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [SuppressUnmanagedCodeSecurity][DllImport(dllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern void FreeLibrary(IntPtr hModule);
    }

}


