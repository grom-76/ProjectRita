

namespace RitaEngine.Base;

/// <summary>
/// See : https://github.com/amerkoleci/Vortice.Vulkan/blob/main/src/Vortice.Vulkan/Interop.cs FOR MORE INFO
/// </summary>
[SuppressUnmanagedCodeSecurity,SkipLocalsInit, StructLayout(LayoutKind.Sequential )]
public unsafe static class MemoryHelper
{ 
            #region ALLOCATION FREE REALLOC....
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AllocateArray<T>(nuint count)  where T : unmanaged
    {
        T* result = (T*)NativeMemory.Alloc(count, (nuint)sizeof(T));

        // if (result == null)
        // {
        //     throw new OutOfMemoryException($"The allocation of '{count}x{SizeOf<T>()}' bytes failed");
        // }

        return result;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* Allocate<T>(nuint count) where T : unmanaged
    {
        T* result = (T*)NativeMemory.Alloc(count);

        // if (result == null)
        // {
        //     throw new OutOfMemoryException($"The allocation of '{count}x{SizeOf<T>()}' bytes failed");
        // }

        return result;
    }


    #endregion
    #region SIZE
    
    public static int SizeOf<T>() =>  Unsafe.SizeOf<T>();
    
    #endregion
    #region ADDRESSOF
    //Courtesy of : https://stackoverflow.com/questions/25410158/get-memory-address-of-net-object-c/25432952#25432952

    /// <summary>
    /// Provides the current address of the given object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static System.IntPtr AddressOf(object obj)
    {
        if (obj == null) return System.IntPtr.Zero;

        System.TypedReference reference = __makeref(obj);
        #pragma warning disable CS8500
        System.TypedReference* pRef = &reference;
        #pragma warning restore
        return (System.IntPtr)pRef; //(&pRef)
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static System.IntPtr AddressOfObj(object obj)
    {
        GCHandle objHandle = default;
        IntPtr adr3 = IntPtr.Zero;
        try{
            objHandle = GCHandle.Alloc(obj,GCHandleType.WeakTrackResurrection);
            adr3 =  GCHandle.ToIntPtr(objHandle);
        }
        catch{
            return IntPtr.Zero;
        }
        finally {
            objHandle.Free();
        }
        return adr3;
    }

    
/// <summary>
/// Provides the current address of the given element
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="t"></param>
/// <returns></returns>
[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static System.IntPtr AddressOf<T>(T t)
    //refember ReferenceTypes are references to the CLRHeader
    //where TOriginal : struct
{
    System.TypedReference reference = __makeref(t);
    #pragma warning disable CS8500
    return *(System.IntPtr*)(&reference);
    #pragma warning restore
}

[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
static System.IntPtr AddressOfRef<T>(ref T t)
//refember ReferenceTypes are references to the CLRHeader
//where TOriginal : struct
{
    System.TypedReference reference = __makeref(t);
    #pragma warning disable CS8500
    System.TypedReference* pRef = &reference;
    #pragma warning restore
    return (System.IntPtr)pRef; //(&pRef)
}

/// <summary>
/// Returns the unmanaged address of the given array.
/// </summary>
/// <param name="array"></param>
/// <returns><see cref="IntPtr.Zero"/> if null, otherwise the address of the array</returns>
[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
public static System.IntPtr AddressOfByteArray(byte[] array)
{
    if (array == null) return System.IntPtr.Zero;

    fixed (byte* ptr = array)
        return (System.IntPtr)(ptr - 2 * sizeof(void*)); //Todo staticaly determine size of void?
}

public static int AdressOfLowPart( long address) =>(int)( address & 0x0000FFFF);
public static int AdressOfHightPart( long address) => (int)( address >>16 );



    #endregion

    #region POINTER        
    /// <inheritdoc cref="Unsafe.AsPointer{T}(ref T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* AsPointer<T>(ref T source) 
        where T : unmanaged 
    => (T*)Unsafe.AsPointer(ref source);

    /// <inheritdoc cref="Unsafe.As{TFrom, TTo}(ref TFrom)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly TTo AsReadOnly<TFrom, TTo>(in TFrom source)
        where TFrom : unmanaged  where TTo : unmanaged
    => ref Unsafe.As<TFrom, TTo>(ref AsRef(in source));

    /// <summary>Reinterprets the given native integer as a reference.</summary>
    /// <typeparam name="T">The type of the reference.</typeparam>
    /// <param name="source">The native integer to reinterpret.</param>
    /// <returns>A reference to a value of type <typeparamref name="T" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T AsRef<T>(nint source) 
    => ref Unsafe.AsRef<T>((void*)source);

    /// <summary>Reinterprets the given native unsigned integer as a reference.</summary>
    /// <typeparam name="T">The type of the reference.</typeparam>
    /// <param name="source">The native unsigned integer to reinterpret.</param>
    /// <returns>A reference to a value of type <typeparamref name="T" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T AsRef<T>(nuint source) 
    => ref Unsafe.AsRef<T>((void*)source);

    /// <inheritdoc cref="Unsafe.AsRef{T}(in T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T AsRef<T>(in T source) 
    => ref Unsafe.AsRef(in source);

    /// <inheritdoc cref="MemoryMarshal.CreateReadOnlySpan{T}(ref T, int)" />
    public static ReadOnlySpan<T> CreateReadOnlySpan<T>(scoped in T reference, int length) 
    => MemoryMarshal.CreateReadOnlySpan(ref AsRef(in reference), length);

    // <summary>Returns a pointer to the element of the span at index zero.</summary>
    /// <typeparam name="T">The type of items in <paramref name="span" />.</typeparam>
    /// <param name="span">The span from which the pointer is retrieved.</param>
    /// <returns>A pointer to the item at index zero of <paramref name="span" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* GetPointer<T>(this Span<T> span)
        where T : unmanaged 
    => AsPointer(ref span.GetReference());

    /// <summary>Returns a pointer to the element of the span at index zero.</summary>
    /// <typeparam name="T">The type of items in <paramref name="span" />.</typeparam>
    /// <param name="span">The span from which the pointer is retrieved.</param>
    /// <returns>A pointer to the item at index zero of <paramref name="span" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T* GetPointer<T>(this ReadOnlySpan<T> span)
        where T : unmanaged 
    => AsPointer(ref AsRef(in span.GetReference()));

    /// <inheritdoc cref="MemoryMarshal.GetReference{T}(Span{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this Span<T> span) 
    => ref MemoryMarshal.GetReference(span);

    /// <inheritdoc cref="MemoryMarshal.GetReference{T}(ReadOnlySpan{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly T GetReference<T>(this ReadOnlySpan<T> span)
    => ref MemoryMarshal.GetReference(span);

    /// <inheritdoc cref="Unsafe.As{TFrom, TTo}(ref TFrom)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TTo As<TFrom, TTo>(ref TFrom source)
    => ref Unsafe.As<TFrom, TTo>(ref source);

    /// <inheritdoc cref="Unsafe.As{TFrom, TTo}(ref TFrom)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<TTo> As<TFrom, TTo>(this ReadOnlySpan<TFrom> span)
        where TFrom : unmanaged    where TTo : unmanaged
    => CreateReadOnlySpan(in AsReadOnly<TFrom, TTo>(in span.GetReference()), span.Length);

    /// <inheritdoc cref="Unsafe.IsNullRef{T}(ref T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullRef<T>(in T source)
    => Unsafe.IsNullRef(ref AsRef(in source));
    #endregion
}
