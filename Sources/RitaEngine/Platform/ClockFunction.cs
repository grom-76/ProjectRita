namespace RitaEngine.Platform;

using RitaEngine.API;
using RitaEngine.Base;

[ StructLayout(LayoutKind.Sequential, Pack = BaseHelper.FORCE_ALIGNEMENT),SkipLocalsInit]
public readonly struct ClockFunctions :  IEquatable<ClockFunctions>
{
    public unsafe readonly delegate* unmanaged<out UInt64, int> QueryPerformanceCounter = null;
    public unsafe readonly delegate* unmanaged<out UInt64, int> QueryPerformanceFrequency = null; 
    public readonly nint kernel32 = nint.Zero; 
      
    public unsafe ClockFunctions ( string modulename ) 
    { 
        kernel32 = Libraries.Load( modulename );
        QueryPerformanceCounter = (delegate* unmanaged<out UInt64, int>) Libraries.GetUnsafeSymbol( kernel32, "QueryPerformanceCounter");
        QueryPerformanceFrequency = (delegate* unmanaged<out UInt64, int>)Libraries.GetUnsafeSymbol( kernel32, "QueryPerformanceFrequency");
    }

    #region OVERRIDE
    public override string ToString() => string.Format($"Clock Commands" );
    public unsafe override int GetHashCode() => HashCode.Combine(((nint)QueryPerformanceCounter)  ,  (nint)QueryPerformanceFrequency  )  ;
    public override bool Equals(object? obj) => obj is ClockFunctions context && this.Equals(context) ;
    public unsafe bool Equals(ClockFunctions other)=> other is ClockFunctions input && ((nint)QueryPerformanceCounter).ToInt64().Equals( ((nint)input.QueryPerformanceCounter)) ;
    public static bool operator ==(ClockFunctions  left, ClockFunctions right) => left.Equals(right);
    public static bool operator !=(ClockFunctions  left, ClockFunctions  right) => !left.Equals(right);
  
    #endregion
}
