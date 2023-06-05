namespace RitaEngine.Base.Platform.Structures;

using RitaEngine.Base.Platform.API.Vulkan;

public unsafe delegate void* PFN_vkGetInstanceProcAddr(VkInstance module , string name);

public unsafe delegate void* PFN_vkGetDeviceProcAddr(VkDevice module , string name);


[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public /*readonly*/ struct GraphicDeviceLoaderFunction : IEquatable<GraphicDeviceLoaderFunction>
{
    private  unsafe static delegate* unmanaged<VkInstance,byte*  , void*> FuncvkGetInstanceProcAddr =null;//)(VkInstance instance, byte* pName);
	private  unsafe static delegate* unmanaged<VkDevice,byte*  , void*> FuncvkGetDeviceProcAddr = null;// typedef vkVoidFunction vkGetDeviceProcAddr)(VkDevice device, byte* pName);

    public unsafe /*readonly*/ delegate* unmanaged< UInt32*,VkLayerProperties*,VkResult > vkEnumerateInstanceLayerProperties = null;
    public unsafe /*readonly*/ delegate* unmanaged< UInt32*,VkResult > vkEnumerateInstanceVersion = null;
    public unsafe /*readonly*/ delegate* unmanaged< char*,UInt32*,VkExtensionProperties*,VkResult > vkEnumerateInstanceExtensionProperties = null;
    public unsafe /*readonly*/ delegate* unmanaged< VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*,VkResult > vkCreateInstance = null;
    public unsafe /*readonly*/ delegate* unmanaged< VkInstance,VkAllocationCallbacks*,void > vkDestroyInstance = null;

    public nint vulkan = nint.Zero;
    private nint _address = nint.Zero;

    public unsafe GraphicDeviceLoaderFunction( ) {  _address = AddressOfPtrThis(); }

    public unsafe void Init( string VulkanDLLName )
    {
        vulkan = Libraries.Load(VulkanDLLName);
        if ( vulkan == nint.Zero) throw new Exception("Vulkan Dll not found");

        vkEnumerateInstanceLayerProperties =(delegate* unmanaged< UInt32*,VkLayerProperties*,VkResult>) Libraries.GetUnsafeSymbol( vulkan,nameof(vkEnumerateInstanceLayerProperties)) ;
        vkEnumerateInstanceVersion =  (delegate* unmanaged<UInt32*  , VkResult>)Libraries.GetUnsafeSymbol(vulkan,nameof(vkEnumerateInstanceVersion)) ;
        vkEnumerateInstanceExtensionProperties = (delegate* unmanaged<char*,UInt32*,VkExtensionProperties*,VkResult>)Libraries.GetUnsafeSymbol(vulkan,nameof(vkEnumerateInstanceExtensionProperties)) ;
        vkCreateInstance = (delegate* unmanaged<VkInstanceCreateInfo*,VkAllocationCallbacks*,VkInstance*  , VkResult> )Libraries.GetUnsafeSymbol(vulkan,nameof(vkCreateInstance));
        vkDestroyInstance = (delegate* unmanaged<VkInstance  ,VkAllocationCallbacks*, void> ) Libraries.GetUnsafeSymbol(vulkan,nameof(vkDestroyInstance));
        FuncvkGetInstanceProcAddr = (delegate* unmanaged<VkInstance,byte*  , void*>) Libraries.GetUnsafeSymbol(vulkan,nameof(vkGetInstanceProcAddr));
        FuncvkGetDeviceProcAddr = ( delegate* unmanaged<VkDevice,byte*  , void*>) Libraries.GetUnsafeSymbol(vulkan,nameof(vkGetDeviceProcAddr));
    }

    public unsafe static void* vkGetInstanceProcAddr(VkInstance instance, string name)
    {
		void*    result = null;
		byte[] bytes = Encoding.ASCII.GetBytes(name);
		fixed( byte* ptr = bytes){
			result =  FuncvkGetInstanceProcAddr(  instance,ptr );
		}
        return result;
    }

    public unsafe static void* vkGetDeviceProcAddr(VkDevice device, string name)
    {
		void*    result = null;
		byte[] bytes = Encoding.ASCII.GetBytes(name);
		fixed( byte* ptr = bytes){
			result =  FuncvkGetDeviceProcAddr(  device,ptr );
		}
        return result;
    }

    public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine( ((nint)0).ToInt32()  ,  ((nint)0).ToInt32(),  ((nint)0).ToInt32(), ((nint)0).ToInt32() ) ;
    public override bool Equals(object? obj) => obj is GraphicDeviceLoaderFunction context && this.Equals(context) ;
    public unsafe bool Equals(GraphicDeviceLoaderFunction other)=> other is GraphicDeviceLoaderFunction input && ( ((nint)vkEnumerateInstanceLayerProperties).ToInt64()).Equals(((nint)input.vkEnumerateInstanceLayerProperties).ToInt64() );
    public static bool operator ==(GraphicDeviceLoaderFunction  left, GraphicDeviceLoaderFunction right) => left.Equals(right);
    public static bool operator !=(GraphicDeviceLoaderFunction  left, GraphicDeviceLoaderFunction  right) => !left.Equals(right);
    public unsafe void Release() 
    {  
        Libraries.Unload(vulkan);
        vkEnumerateInstanceLayerProperties =null;
        vkEnumerateInstanceVersion =null;
        vkEnumerateInstanceExtensionProperties =null;
        vkCreateInstance =null;
        vkDestroyInstance =null;
        FuncvkGetInstanceProcAddr =null;
        FuncvkGetDeviceProcAddr =null;
    }
    #endregion
}

