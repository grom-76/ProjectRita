

namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.Vulkan;

[SkipLocalsInit, StructLayout(LayoutKind.Sequential ,Pack = BaseHelper.FORCE_ALIGNEMENT)]
public struct GraphicDeviceFunctions : IEquatable<GraphicDeviceFunctions>
{
    public GraphicDeviceFunction DeviceFunction = new();
    public GraphicInstanceFunction InstanceFunction = new();
    public GraphicDeviceLoaderFunction LoaderFunction = new();   
    public GraphicDeviceFunctions()  {  }

    public void InitLoaderFunctions( string dllname )=> LoaderFunction.Init( dllname);
    public void InitInstancesFunctions( PFN_vkGetInstanceProcAddr load , VkInstance instance )=> InstanceFunction.Init( load, instance);
    public void InitDevicesFunctions( PFN_vkGetDeviceProcAddr load , VkDevice device ) => DeviceFunction.Init( load , device);

    public void Release()
    {
        DeviceFunction.Release();
        InstanceFunction.Release();
        LoaderFunction.Release();
    }

    #region OVERRIDE
    // public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    public override string ToString() => string.Format($"Graphic Device Function" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)0).ToInt32()  ,  ((nint)0).ToInt32(),  ((nint)0).ToInt32(), ((nint)0).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is GraphicDeviceFunctions context && this.Equals(context) ;
    public unsafe bool Equals(GraphicDeviceFunctions other)=> other is GraphicDeviceFunctions input &&  LoaderFunction.Equals(other.LoaderFunction);
    public static bool operator ==(GraphicDeviceFunctions  left, GraphicDeviceFunctions right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceFunctions  left, GraphicDeviceFunctions  right) => !left.Equals(right);
    #endregion

}
