#region ALL DOTNET USING 

global using System;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Security;// SuppressUnmanagedCodeSecurity for all  class
global using System.Buffers; //Bytes Helper  TODO find another way to array pool change it ????? 
global using System.Reflection;
global using System.Text;


#if SIMD
global using System.Runtime.Intrinsics;// 
global using Matrix4x4 = System.Numerics.Matrix4x4;

#endif

#endregion

namespace RitaEngine.Base;

[AttributeUsage(
    AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
    AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
    AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
public sealed class NotNullAttribute : Attribute
{
}

[SuppressUnmanagedCodeSecurity, StructLayout(LayoutKind.Sequential, Pack =BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public static class BaseHelper
{
    public const string ENGINE_NAME ="RitaEngine";
    public const string ENGINE_VERSION ="0.9.358";
    // public static ReadOnlySpan<byte> ENGINE_NAME =>  "RitaEngine"u8;
    // public static ReadOnlySpan<byte> ENGINE_VERSION => "0.1.9"u8;
    
#if WIN6464        
    public const int FORCE_ALIGNEMENT =  8;//x64
#else
    public const int FORCE_ALIGNEMENT =  4;//x64
#endif
}


