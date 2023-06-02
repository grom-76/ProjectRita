namespace RitaEngine.Base.Platform.Structures;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public readonly struct ClockFunctions : IDisposable , IEquatable<ClockFunctions>
{
    public unsafe readonly delegate* unmanaged<out UInt64, int> QueryPerformanceCounter = null;
    public unsafe readonly delegate* unmanaged<out UInt64, int> QueryPerformanceFrequency = null;  
      
    public unsafe ClockFunctions ( PFN_GetSymbolPointer load, nint kernel ) 
    { 
        QueryPerformanceCounter = (delegate* unmanaged<out UInt64, int>) load( kernel, "QueryPerformanceCounter");
        QueryPerformanceFrequency = (delegate* unmanaged<out UInt64, int>)load( kernel, "QueryPerformanceFrequency");
    }
    public unsafe nint AddressOfPtrThis( ){fixed (void* pointer = &this)  { return((nint) pointer ) ; }  }
    #region OVERRIDE
    public override string ToString() => string.Format($"Vector" );
    public unsafe override int GetHashCode() => HashCode.Combine(((nint)QueryPerformanceCounter)  ,  (nint)QueryPerformanceFrequency  )  ;
    public override bool Equals(object? obj) => obj is ClockFunctions context && this.Equals(context) ;
    public unsafe bool Equals(ClockFunctions other)=> other is ClockFunctions input && ((nint)QueryPerformanceCounter).ToInt64().Equals( ((nint)input.QueryPerformanceCounter)) ;
    public static bool operator ==(ClockFunctions  left, ClockFunctions right) => left.Equals(right);
    public static bool operator !=(ClockFunctions  left, ClockFunctions  right) => !left.Equals(right);
    public void Dispose() {  }
    #endregion
}
